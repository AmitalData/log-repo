namespace AmitalCloud.Infrastructure.APITools.Interface
{
    public interface IPOAExpireReminder
    {
        void StartRun(string taskId, int seedDefaultTenant);
    }
}
