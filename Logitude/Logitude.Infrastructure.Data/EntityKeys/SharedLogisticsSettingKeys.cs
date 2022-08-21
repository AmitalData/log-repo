using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.EntityKeys
{
    public partial class SharedLogisticsSettingKeys : EntityKeyFields
   {
   	  public string Id  { get; set; }
	 
	  public override string GetFullKey()
      {
         return Id;
      }

      public override string GetEntityPMName()
      {
          return "SharedLogisticsSettingPM";
      }
   }
}
	 