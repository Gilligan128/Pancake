using System;

namespace Pancake.Core
{
    internal interface IStartProviderLifecycle
    {
        IDisposable Execute(ResourceProvider provider);
    }
}