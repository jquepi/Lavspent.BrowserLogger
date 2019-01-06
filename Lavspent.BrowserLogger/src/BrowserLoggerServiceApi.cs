﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Lavspent.BrowserLogger
{
    internal class AsyncQueue<TType>
    {
        private ConcurrentQueue<TType> _queue = new ConcurrentQueue<TType>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public AsyncQueue()
        {
        }

        public void Enqueue(TType value)
        {
            if (value== null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _queue.Enqueue(value);
            _signal.Release();
        }

        public async Task<TType> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _queue.TryDequeue(out var workItem);
            return workItem;
        }
    }

    internal class BrowserLoggerQueue : AsyncQueue<string>
    {
    }
}