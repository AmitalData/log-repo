namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContainerTypesFieldsToTariffTable_Bisan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "ContainerType1Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "ContainerType2Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "ContainerType3Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "ContainerType4Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "ContainerType5Id", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Tariffs", "ContainerType1Id");
            CreateIndex("dbo.Tariffs", "ContainerType2Id");
            CreateIndex("dbo.Tariffs", "ContainerType3Id");
            CreateIndex("dbo.Tariffs", "ContainerType4Id");
            CreateIndex("dbo.Tariffs", "ContainerType5Id");
            AddForeignKey("dbo.Tariffs", "ContainerType1Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType2Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType3Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType4Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType5Id", "dbo.PackageTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tariffs", "ContainerType5Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Tariffs", "ContainerType4Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Tariffs", "ContainerType3Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Tariffs", "ContainerType2Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Tariffs", "ContainerType1Id", "dbo.PackageTypes");
            DropIndex("dbo.Tariffs", new[] { "ContainerType5Id" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType4Id" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType3Id" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType2Id" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType1Id" });
            DropColumn("dbo.Tariffs", "ContainerType5Id");
            DropColumn("dbo.Tariffs", "ContainerType4Id");
            DropColumn("dbo.Tariffs", "ContainerType3Id");
            DropColumn("dbo.Tariffs", "ContainerType2Id");
            DropColumn("dbo.Tariffs", "ContainerType1Id");
        }
    }
}
