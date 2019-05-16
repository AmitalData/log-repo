namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFields_Tariff_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "Surcharge1Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge2Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge3Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge4Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge5Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge6Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge7Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge8Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge9Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge10Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge1UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge2UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge3UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge4UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge5UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge6UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge7UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge8UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge9UOM", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Surcharge10UOM", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Tariffs", "Surcharge1Id");
            CreateIndex("dbo.Tariffs", "Surcharge2Id");
            CreateIndex("dbo.Tariffs", "Surcharge3Id");
            CreateIndex("dbo.Tariffs", "Surcharge4Id");
            CreateIndex("dbo.Tariffs", "Surcharge5Id");
            CreateIndex("dbo.Tariffs", "Surcharge6Id");
            CreateIndex("dbo.Tariffs", "Surcharge7Id");
            CreateIndex("dbo.Tariffs", "Surcharge8Id");
            CreateIndex("dbo.Tariffs", "Surcharge9Id");
            CreateIndex("dbo.Tariffs", "Surcharge10Id");
            CreateIndex("dbo.Tariffs", "Surcharge1UOM");
            CreateIndex("dbo.Tariffs", "Surcharge2UOM");
            CreateIndex("dbo.Tariffs", "Surcharge3UOM");
            CreateIndex("dbo.Tariffs", "Surcharge4UOM");
            CreateIndex("dbo.Tariffs", "Surcharge5UOM");
            CreateIndex("dbo.Tariffs", "Surcharge6UOM");
            CreateIndex("dbo.Tariffs", "Surcharge7UOM");
            CreateIndex("dbo.Tariffs", "Surcharge8UOM");
            CreateIndex("dbo.Tariffs", "Surcharge9UOM");
            CreateIndex("dbo.Tariffs", "Surcharge10UOM");
            AddForeignKey("dbo.Tariffs", "Surcharge10Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge10UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge1Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge1UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge2Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge2UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge3Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge3UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge4Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge4UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge5Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge5UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge6Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge6UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge7Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge7UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge8Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge8UOM", "dbo.Measurements", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge9Id", "dbo.ChargesTypes", "Id");
            AddForeignKey("dbo.Tariffs", "Surcharge9UOM", "dbo.Measurements", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tariffs", "Surcharge9UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge9Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge8UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge8Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge7UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge7Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge6UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge6Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge5UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge5Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge4UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge4Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge3UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge3Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge2UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge2Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge1UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge1Id", "dbo.ChargesTypes");
            DropForeignKey("dbo.Tariffs", "Surcharge10UOM", "dbo.Measurements");
            DropForeignKey("dbo.Tariffs", "Surcharge10Id", "dbo.ChargesTypes");
            DropIndex("dbo.Tariffs", new[] { "Surcharge10UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge9UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge8UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge7UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge6UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge5UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge4UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge3UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge2UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge1UOM" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge10Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge9Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge8Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge7Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge6Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge5Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge4Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge3Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge2Id" });
            DropIndex("dbo.Tariffs", new[] { "Surcharge1Id" });
            DropColumn("dbo.TariffVersions", "ParentVersionId");
            DropColumn("dbo.Tariffs", "Surcharge10UOM");
            DropColumn("dbo.Tariffs", "Surcharge9UOM");
            DropColumn("dbo.Tariffs", "Surcharge8UOM");
            DropColumn("dbo.Tariffs", "Surcharge7UOM");
            DropColumn("dbo.Tariffs", "Surcharge6UOM");
            DropColumn("dbo.Tariffs", "Surcharge5UOM");
            DropColumn("dbo.Tariffs", "Surcharge4UOM");
            DropColumn("dbo.Tariffs", "Surcharge3UOM");
            DropColumn("dbo.Tariffs", "Surcharge2UOM");
            DropColumn("dbo.Tariffs", "Surcharge1UOM");
            DropColumn("dbo.Tariffs", "Surcharge10Id");
            DropColumn("dbo.Tariffs", "Surcharge9Id");
            DropColumn("dbo.Tariffs", "Surcharge8Id");
            DropColumn("dbo.Tariffs", "Surcharge7Id");
            DropColumn("dbo.Tariffs", "Surcharge6Id");
            DropColumn("dbo.Tariffs", "Surcharge5Id");
            DropColumn("dbo.Tariffs", "Surcharge4Id");
            DropColumn("dbo.Tariffs", "Surcharge3Id");
            DropColumn("dbo.Tariffs", "Surcharge2Id");
            DropColumn("dbo.Tariffs", "Surcharge1Id");
        }
    }
}
