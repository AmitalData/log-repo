using Microsoft.Practices.Unity;

namespace AmitalCloud.Infrastructure.Domain.Interfaces
{

    public interface IContainerAccessor
    {
        IUnityContainer Container { get; }
    }
}
