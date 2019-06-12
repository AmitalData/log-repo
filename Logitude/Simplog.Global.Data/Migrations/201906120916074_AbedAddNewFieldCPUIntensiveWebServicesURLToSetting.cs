namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddNewFieldCPUIntensiveWebServicesURLToSetting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "CPUIntensiveWebServicesURL", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "CPUIntensiveWebServicesURL");
        }
    }
}
