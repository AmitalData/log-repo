using System;
namespace AmitalCustomsWindowsService.BL
{
    interface IWorkerBaseWorkOnce
    {
        void ExecuteTask();
        bool ServiceStarted { get; set; }

        void InvokeStatistics();
        string MyType { get; }
        bool WhileServiceStarted_IsOut { get; }
        int ManagedThreadId { get; set; }        
    }
}
