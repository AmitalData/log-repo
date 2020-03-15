namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryAndStatePropertiesToPort_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ports", "CountryCode", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Ports", "CountryName", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Ports", "StateCode", c => c.String(maxLength: 10, unicode: false));

            Sql(@"
                update Ports set 
                StateCode = (select Code from States where Id = StateId),
                CountryCode = (select Code from Countries where Id = CountryId),
                CountryName = (select EnglishName from Countries where Id = CountryId)");
        }
        
        public override void Down()
        {            
            DropColumn("dbo.Ports", "StateCode");
            DropColumn("dbo.Ports", "CountryName");
            DropColumn("dbo.Ports", "CountryCode");
        }
    }
}
