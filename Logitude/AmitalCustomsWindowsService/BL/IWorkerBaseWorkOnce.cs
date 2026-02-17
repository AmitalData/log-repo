using System;
using System.Threading.Tasks;
namespace AmitalCustomsWindowsService.BL
{
    interface IWorkerBaseWorkOnce
	{
		Task ExecuteTaskAsync();

		void ExecuteTask();
        bool ServiceStarted { get; set; }

        void InvokeStatistics();
        string MyType { get; }
        bool WhileServiceStarted_IsOut { get; }
        int ManagedThreadId { get; set; }
    }
}
