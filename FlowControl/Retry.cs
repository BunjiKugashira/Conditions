namespace FlowControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MathExtensions;

    public class Retry
    {
        public IList<Tuple<Type, Action<Retry, Exception>>> CatchDuringCallbacks { private get; set; } = new List<Tuple<Type, Action<Retry, Exception>>>();
        public IList<Tuple<Type, Action<Retry, Exception>>> CatchAfterCallbacks { private get; set; } = new List<Tuple<Type, Action<Retry, Exception>>>();
        public Action<AggregateException> FinallyCallback { private get; set; } = (_) => { };
        public Func<Retry, bool> TryWhile { private get; set; } = (_) => true;
        public CancellationToken CancellationToken { private get; set; } = new CancellationTokenSource().Token;
        public uint? MaxTryNr { private get; set; } = null;
        public TimeSpan? MaxTotalDuration { private get; set; } = null;
        public TimeSpan MaxDelay { private get; set; } = TimeSpan.FromSeconds(1);
        public Func<Retry, TimeSpan> NextDelay { private get; set; } = (x) => TimeSpan.FromMilliseconds(Math.Pow(x.TryNr, 2));

        public bool Success { get; private set; } = false;
        public bool CouldThrow { get; private set; } = false;
        public uint TryNr { get; private set; } = 0;
        public DateTime? StartTime { get; private set; } = null;

        public TimeSpan? TimeSinceStart => this.StartTime == null ? null : DateTime.Now - this.StartTime;

        public async Task<bool> StartAsync(Func<Task> callback, IList<Exception> exceptions)
        {
            while (
                !this.Success
                && !this.CouldThrow
                && this.TryWhile(this)
                && (this.TimeSinceStart == null || this.TimeSinceStart < this.MaxTotalDuration)
                && (this.MaxTryNr == null || this.TryNr <= this.MaxTryNr)
                && !this.CancellationToken.IsCancellationRequested
            )
            {
                try
                {
                    await callback();
                    this.Success = true;
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                    if (this.HandleException(this.CatchDuringCallbacks, e))
                    {
                        this.CancellationToken.WaitHandle.WaitOne(Math2.MinOrDefault(this.NextDelay(this), this.MaxDelay));
                        this.TryNr++;
                    }
                    else
                    {
                        this.CouldThrow = true;
                    }
                }
            }

            if (this.Success)
            {
                this.FinallyCallback(null);
            }
            else
            {
                var handledExceptions = exceptions.Where(e => this.HandleException(this.CatchAfterCallbacks, e)).ToList();
                foreach (var e in handledExceptions)
                {
                    exceptions.Remove(e);
                }
                this.FinallyCallback(new AggregateException(exceptions));
            }
            return this.Success;
        }

        private bool HandleException(IList<Tuple<Type, Action<Retry, Exception>>> handlers, Exception e)
        {
            foreach (var tuple in handlers)
            {
                if (e.GetType().IsAssignableTo(tuple.Item1))
                {
                    tuple.Item2(this, e);
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> StartAsync(Action callback, IList<Exception> exceptions)
        {
            return await this.StartAsync(async () => { await Task.Run(callback); }, exceptions);
        }

        public async Task StartAsync(Func<Task> callback)
        {
            var exceptions = new List<Exception>();
            if (!await this.StartAsync(callback, exceptions) && exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        public async Task StartAsync(Action callback)
        {
            await this.StartAsync(async () => { await Task.Run(callback); });
        }

        public Retry WithToken(CancellationToken cancellationToken)
        {
            this.CancellationToken = cancellationToken;
            return this;
        }

        public Retry While(Func<Retry, bool> condition)
        {
            this.TryWhile = condition;
            return this;
        }

        public Retry CatchDuring<TException>(Action<Retry, TException> callback)
            where TException : Exception
        {
            this.CatchDuringCallbacks.Add(new Tuple<Type, Action<Retry, Exception>>(typeof(TException), (x, e) => { callback(x, (TException)e); }));
            return this;
        }

        public Retry CatchDuring<TException>(Action<TException> callback)
            where TException : Exception
        {
            this.CatchDuringCallbacks.Add(new Tuple<Type, Action<Retry, Exception>>(typeof(TException), (x, e) => { callback((TException)e); }));
            return this;
        }

        public Retry CatchDuring<TException>(Action<Retry> callback)
            where TException : Exception
        {
            this.CatchDuringCallbacks.Add(new Tuple<Type, Action<Retry, Exception>>(typeof(TException), (x, e) => { callback(x); }));
            return this;
        }

        public Retry CatchDuring<TException>(Action callback)
            where TException : Exception
        {
            this.CatchDuringCallbacks.Add(new Tuple<Type, Action<Retry, Exception>>(typeof(TException), (x, e) => { callback(); }));
            return this;
        }

        public Retry CatchAfter<TException>(Action<Retry, TException> callback)
            where TException : Exception
        {
            this.CatchAfterCallbacks.Add(new Tuple<Type, Action<Retry, Exception>>(typeof(TException), (x, e) => { callback(x, (TException)e); }));
            return this;
        }

        public Retry CatchAfter<TException>(Action<TException> callback)
            where TException : Exception
        {
            this.CatchAfterCallbacks.Add(new Tuple<Type, Action<Retry, Exception>>(typeof(TException), (x, e) => { callback((TException)e); }));
            return this;
        }

        public Retry CatchAfter<TException>(Action<Retry> callback)
            where TException : Exception
        {
            this.CatchAfterCallbacks.Add(new Tuple<Type, Action<Retry, Exception>>(typeof(TException), (x, e) => { callback(x); }));
            return this;
        }

        public Retry CatchAfter<TException>(Action callback)
            where TException : Exception
        {
            this.CatchAfterCallbacks.Add(new Tuple<Type, Action<Retry, Exception>>(typeof(TException), (x, e) => { callback(); }));
            return this;
        }

        public Retry Finally(Action<AggregateException> callback)
        {
            this.FinallyCallback = callback;
            return this;
        }

        public Retry Finally(Action callback)
        {
            this.FinallyCallback = (_) => callback();
            return this;
        }

        public Retry StopAfterTries(uint maxTryNr)
        {
            this.MaxTryNr = maxTryNr;
            return this;
        }

        public Retry StopAfterDuration(TimeSpan maxTotalDuration)
        {
            this.MaxTotalDuration = maxTotalDuration;
            return this;
        }

        public Retry DontDelayLongerThan(TimeSpan maxDelay)
        {
            this.MaxDelay = maxDelay;
            return this;
        }

        public Retry DelayByFunction(Func<Retry, TimeSpan> nextDelay)
        {
            this.NextDelay = nextDelay;
            return this;
        }
    }
}
