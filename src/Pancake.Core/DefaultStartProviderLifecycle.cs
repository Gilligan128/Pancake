using System;

namespace Pancake.Core
{
    public class DefaultStartProviderLifecycle : IStartProviderLifecycle
    {
        public IDisposable Execute(ResourceProvider provider)
        {
            return new ProviderLifcycle(provider);
        }

        internal class ProviderLifcycle : IDisposable
        {
            private readonly ResourceProvider _provider;

            public ProviderLifcycle(ResourceProvider provider)
            {
                _provider = provider;
                _provider.Prefetch();
            }

            public void Dispose()
            {
                _provider.Flush();
            }
        }
    }
}