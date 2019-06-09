using System;
using Microsoft.Extensions.Logging;

namespace TinyLittleMvvm.Demo
{
    public class ScopedService : IDisposable {
        private ILogger _logger;

        public ScopedService(ILogger<ScopedService> logger) {
            _logger = logger;
            _logger.LogDebug("Instantiating ScopedService");
        }

        public void Dispose() {
            _logger.LogDebug("Disposing ScopedService");
        }
    }
}
