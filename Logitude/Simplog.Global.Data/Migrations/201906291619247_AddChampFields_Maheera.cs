namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChampFields_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "ChampTestAPIURL", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            AddColumn("dbo.Settings", "ChampTestAPIPassword", c => c.String(nullable: false, maxLength: 40, unicode: false));
            AddColumn("dbo.Settings", "ChampProdAPIURL", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            AddColumn("dbo.Settings", "ChampProdAPIPassword", c => c.String(nullable: false, maxLength: 40, unicode: false));

            Sql("update Settings set ChampTestAPIURL = 'https://community.champ.aero:8444/logitude/test/NO_WAIT',ChampTestAPIPassword='logitudett',ChampProdAPIURL='https://community.champ.aero:8443/logitude/prod/NO_WAIT',ChampProdAPIPassword='logitudepp'");
        }

        public override void Down()
        {
            DropColumn("dbo.Settings", "ChampProdAPIPassword");
            DropColumn("dbo.Settings", "ChampProdAPIURL");
            DropColumn("dbo.Settings", "ChampTestAPIPassword");
            DropColumn("dbo.Settings", "ChampTestAPIURL");
        }
    }
}
