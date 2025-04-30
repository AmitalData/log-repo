using Microsoft.Extensions.DependencyInjection;

namespace AmitalCloud.Infrastructure.Domain.Interfaces
{

    public interface IContainerAccessor
    {
        IServiceProvider Container { get; }
    }
}
