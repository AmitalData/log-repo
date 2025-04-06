namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class SettingQuery
    {
        private enum SettingIds
        {
            DefaultSetting = 1
        }
        public static string GetDefaultSettingId()
        {
            return SettingIds.DefaultSetting.ToString();
        }
    }
}
