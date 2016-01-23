using System;

namespace Pancake.Core
{
    public interface IStartProviderLifecycle
    {
        IDisposable Execute(ResourceProvider provider);
    }
}