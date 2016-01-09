﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Nimbus.Configuration.Settings;

namespace Nimbus.Infrastructure.MessageSendersAndReceivers
{
    public class GlobalHandlerThrottle : IGlobalHandlerThrottle, IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private bool _isDisposed;

        public GlobalHandlerThrottle(GlobalConcurrentHandlerLimitSetting globalConcurrentHandlerLimit)
        {
            _semaphore = new SemaphoreSlim(globalConcurrentHandlerLimit, globalConcurrentHandlerLimit);
        }

        public Task Wait(CancellationToken cancellationToken)
        {
            return _semaphore.WaitAsync(cancellationToken);
        }

        public void Release()
        {
            _semaphore.Release();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_isDisposed) throw new ObjectDisposedException("Object already disposed");
            _isDisposed = true;

            _semaphore.Dispose();
        }
    }
}