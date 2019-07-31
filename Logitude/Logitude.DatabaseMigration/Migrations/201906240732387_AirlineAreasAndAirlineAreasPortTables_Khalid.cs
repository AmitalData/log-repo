namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AirlineAreasAndAirlineAreasPortTables_Khalid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AirlineAreas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AirlineId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreateDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Airlines", t => t.AirlineId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.AirlineId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.AirlineAreasPorts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AirlineAreaId = c.String(nullable: false, maxLength: 128),
                        PortId = c.String(maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        AddedDate = c.DateTime(),
                        AddedByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedByUserId)
                .ForeignKey("dbo.AirlineAreas", t => t.AirlineAreaId)
                .ForeignKey("dbo.Ports", t => t.PortId)
                .Index(t => t.AirlineAreaId)
                .Index(t => t.PortId)
                .Index(t => t.AddedByUserId);
            
                  }
        
        public override void Down()
        {
            DropTable("dbo.AirlineAreasPorts");
            DropTable("dbo.AirlineAreas");
        }
    }
}
