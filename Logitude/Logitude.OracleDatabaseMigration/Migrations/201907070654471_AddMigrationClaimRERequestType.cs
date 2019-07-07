namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMigrationClaimRERequestType : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", c => c.String(maxLength: 2, unicode: false));
            AddColumn("Customs.ClaimsRelatedEntities", "Explanation", c => c.String(maxLength: 256));
            AddColumn("Customs.ClaimsRelatedEntities", "Note", c => c.String(maxLength: 256));
            CreateIndex("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode");
            AddForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "Customs.ContinuousRequestTypes", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "Customs.ContinuousRequestTypes");
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "ContinuousRequestTypeCode" });
            DropColumn("Customs.ClaimsRelatedEntities", "Note");
            DropColumn("Customs.ClaimsRelatedEntities", "Explanation");
            DropColumn("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode");
        }
    }
}
