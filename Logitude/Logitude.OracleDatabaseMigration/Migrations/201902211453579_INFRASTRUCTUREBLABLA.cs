namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class INFRASTRUCTUREBLABLA : DbMigration
    {
        public override void Up()
        {
            return;
            //DropForeignKey("dbo.Activities", "BusinessProcessQueueId", "dbo.BusinessProcessQueues");
            //DropForeignKey("dbo.BusinessProcessQueues", "BusinessRoleId", "dbo.BusinessRoles");
            //DropForeignKey("dbo.Activities", "TeamId", "dbo.Teams");
            //DropIndex("dbo.Activities", new[] { "BusinessProcessQueueId" });
            //DropIndex("dbo.Activities", new[] { "TeamId" });
            //DropIndex("dbo.BusinessProcessQueues", new[] { "CreatedByUserId" });
            //DropIndex("dbo.BusinessProcessQueues", new[] { "UpdatedByUserId" });
            //DropIndex("dbo.BusinessProcessQueues", new[] { "BusinessRoleId" });
            //DropIndex("dbo.BusinessRoles", new[] { "CreatedByUserId" });
            //DropIndex("dbo.BusinessRoles", new[] { "UpdatedByUserId" });
            //DropIndex("dbo.Teams", new[] { "CreatedByUserId" });
            //DropIndex("dbo.Teams", new[] { "UpdatedByUserId" });
            //DropPrimaryKey("dbo.BusinessProcessQueues");
            //DropPrimaryKey("dbo.BusinessRoles");
            //DropPrimaryKey("dbo.Teams");
            //DropPrimaryKey("dbo.SharedLogisticsSettings");
            //return;
            CreateTable(
                "dbo.BatchTaskExecutions",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    CreateDate = c.DateTime(nullable: false, precision: 7),
                    CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    SearchFields = c.String(),
                    ClassName = c.String(maxLength: 120, unicode: false),
                    PrametersXml = c.String(maxLength: 2000),
                    StatusCode = c.String(maxLength: 1, unicode: false),
                    ErrorLog = c.String(maxLength: 2000),
                    StartDateTime = c.DateTime(precision: 7),
                    DoneDateTime = c.DateTime(precision: 7),
                    ProgressMessage = c.String(maxLength: 120),
                    ProgressPercentage = c.Int(nullable: false),
                    Subject = c.String(maxLength: 200),
                    CallStack = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BatchTaskExecutionStatus", t => t.StatusCode)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.StatusCode);

            CreateTable(
                "dbo.BatchTaskExecutionStatus",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 1, unicode: false),
                    Name = c.String(maxLength: 60, unicode: false),
                    SearchFields = c.String(),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.BIReports",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    CreateDate = c.DateTime(nullable: false, precision: 7),
                    CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    UpdateDate = c.DateTime(nullable: false, precision: 7),
                    UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    SearchFields = c.String(),
                    Name = c.String(nullable: false, maxLength: 80),
                    Description = c.String(maxLength: 1000),
                    DWQueryId = c.String(nullable: false, maxLength: 128),
                    Inactive = c.Boolean(nullable: false),
                    TypeCode = c.String(maxLength: 3, unicode: false),
                    AGGridOptionsXML = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BIReportsTypes", t => t.TypeCode)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.DWQueries", t => t.DWQueryId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.DWQueryId)
                .Index(t => t.TypeCode);

            CreateTable(
                "dbo.BIReportsTypes",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 3, unicode: false),
                    Name = c.String(maxLength: 60, unicode: false),
                    SearchFields = c.String(),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.DWQueries",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Tenant = c.Int(nullable: false),
                    SQLString = c.String(),
                    CreatedByUserId = c.String(maxLength: 15, unicode: false),
                    UpdateByUserId = c.String(maxLength: 15, unicode: false),
                    CreatedDate = c.DateTime(nullable: false, precision: 7),
                    UpdatedDate = c.DateTime(nullable: false, precision: 7),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdateByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdateByUserId);

            CreateTable(
                "dbo.FeatureToggles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    CreateDate = c.DateTime(nullable: false, precision: 7),
                    CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    UpdateDate = c.DateTime(nullable: false, precision: 7),
                    UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                    TenantNumber = c.Int(nullable: false),
                    Inactive = c.Boolean(nullable: false),
                    ToggleCode = c.String(nullable: false, maxLength: 3, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Toggles", t => t.ToggleCode)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.ToggleCode);

            CreateTable(
                "dbo.Toggles",
                c => new
                {
                    Code = c.String(nullable: false, maxLength: 3, unicode: false),
                    Name = c.String(nullable: false, maxLength: 100, unicode: false),
                    SearchFields = c.String(maxLength: 1000),
                })
                .PrimaryKey(t => t.Code);

            CreateTable(
                "dbo.LBPTeamMembers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    MemberUserId = c.String(maxLength: 15, unicode: false),
                    TeamId = c.String(nullable: false, maxLength: 15, unicode: false),
                    AddDate = c.DateTime(nullable: false, precision: 7),
                    AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    MemberTeamId = c.String(maxLength: 15, unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedByUserId)
                .ForeignKey("dbo.Teams", t => t.MemberTeamId)
                .ForeignKey("dbo.Users", t => t.MemberUserId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .Index(t => t.MemberUserId)
                .Index(t => t.TeamId)
                .Index(t => t.AddedByUserId)
                .Index(t => t.MemberTeamId);
            //starthere:
            CreateTable(
                "dbo.TeamMemberBusinessRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 15, unicode: false),
                    Tenant = c.Int(nullable: false),
                    TeamMemberId = c.String(nullable: false, maxLength: 15, unicode: false),
                    AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    AddDate = c.DateTime(nullable: false, precision: 7),
                    BusinessRoleId = c.String(nullable: false, maxLength: 15, unicode: false),
                })
                .PrimaryKey(t => t.Id);
                //.ForeignKey("dbo.Users", t => t.AddedByUserId)
                //.ForeignKey("dbo.BusinessRoles", t => t.BusinessRoleId)
                //.ForeignKey("dbo.LBPTeamMembers", t => t.TeamMemberId)
                //.Index(t => t.TeamMemberId)
                //.Index(t => t.AddedByUserId)
                //.Index(t => t.BusinessRoleId);

            //AlterColumn("dbo.Activities", "BusinessProcessQueueId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.Activities", "TeamId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.BusinessProcessQueues", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.BusinessProcessQueues", "CreatedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.BusinessProcessQueues", "UpdatedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.BusinessProcessQueues", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //AlterColumn("dbo.BusinessProcessQueues", "LocalName", c => c.String(maxLength: 40));
            //AlterColumn("dbo.BusinessProcessQueues", "BusinessRoleId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.BusinessProcessQueues", "Notes", c => c.String(maxLength: 250));
            //AlterColumn("dbo.BusinessRoles", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.BusinessRoles", "CreatedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.BusinessRoles", "UpdatedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.BusinessRoles", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //AlterColumn("dbo.BusinessRoles", "LocalName", c => c.String(maxLength: 40));
            //AlterColumn("dbo.BusinessRoles", "Description", c => c.String(maxLength: 250));
            //AlterColumn("dbo.Teams", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.Teams", "CreatedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.Teams", "UpdatedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AlterColumn("dbo.Teams", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //AlterColumn("dbo.Teams", "LocalName", c => c.String(maxLength: 40));
            //AlterColumn("dbo.Teams", "Notify", c => c.String(maxLength: 4000, unicode: false));
            //AlterColumn("dbo.Teams", "Notes", c => c.String(maxLength: 250));
            //AlterColumn("dbo.SharedLogisticsSettings", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AddPrimaryKey("dbo.BusinessProcessQueues", "Id");
            //AddPrimaryKey("dbo.BusinessRoles", "Id");
            //AddPrimaryKey("dbo.Teams", "Id");
            //AddPrimaryKey("dbo.SharedLogisticsSettings", "Id");
            //CreateIndex("dbo.Activities", "BusinessProcessQueueId");
            //CreateIndex("dbo.Activities", "TeamId");
            //CreateIndex("dbo.BusinessProcessQueues", "CreatedByUserId");
            //CreateIndex("dbo.BusinessProcessQueues", "UpdatedByUserId");
            //CreateIndex("dbo.BusinessProcessQueues", "BusinessRoleId");
            //CreateIndex("dbo.BusinessRoles", "CreatedByUserId");
            //CreateIndex("dbo.BusinessRoles", "UpdatedByUserId");
            //CreateIndex("dbo.Teams", "CreatedByUserId");
            //CreateIndex("dbo.Teams", "UpdatedByUserId");
            //AddForeignKey("dbo.Activities", "BusinessProcessQueueId", "dbo.BusinessProcessQueues", "Id");
            //AddForeignKey("dbo.BusinessProcessQueues", "BusinessRoleId", "dbo.BusinessRoles", "Id");
            //AddForeignKey("dbo.Activities", "TeamId", "dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.BusinessProcessQueues", "BusinessRoleId", "dbo.BusinessRoles");
            DropForeignKey("dbo.Activities", "BusinessProcessQueueId", "dbo.BusinessProcessQueues");
            DropForeignKey("dbo.TeamMemberBusinessRoles", "TeamMemberId", "dbo.LBPTeamMembers");
            DropForeignKey("dbo.TeamMemberBusinessRoles", "BusinessRoleId", "dbo.BusinessRoles");
            DropForeignKey("dbo.TeamMemberBusinessRoles", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.LBPTeamMembers", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.LBPTeamMembers", "MemberUserId", "dbo.Users");
            DropForeignKey("dbo.LBPTeamMembers", "MemberTeamId", "dbo.Teams");
            DropForeignKey("dbo.LBPTeamMembers", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.FeatureToggles", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.FeatureToggles", "ToggleCode", "dbo.Toggles");
            DropForeignKey("dbo.FeatureToggles", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReports", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReports", "DWQueryId", "dbo.DWQueries");
            DropForeignKey("dbo.DWQueries", "UpdateByUserId", "dbo.Users");
            DropForeignKey("dbo.DWQueries", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReports", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BIReports", "TypeCode", "dbo.BIReportsTypes");
            DropForeignKey("dbo.BatchTaskExecutions", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BatchTaskExecutions", "StatusCode", "dbo.BatchTaskExecutionStatus");
            DropIndex("dbo.TeamMemberBusinessRoles", new[] { "BusinessRoleId" });
            DropIndex("dbo.TeamMemberBusinessRoles", new[] { "AddedByUserId" });
            DropIndex("dbo.TeamMemberBusinessRoles", new[] { "TeamMemberId" });
            DropIndex("dbo.LBPTeamMembers", new[] { "MemberTeamId" });
            DropIndex("dbo.LBPTeamMembers", new[] { "AddedByUserId" });
            DropIndex("dbo.LBPTeamMembers", new[] { "TeamId" });
            DropIndex("dbo.LBPTeamMembers", new[] { "MemberUserId" });
            DropIndex("dbo.FeatureToggles", new[] { "ToggleCode" });
            DropIndex("dbo.FeatureToggles", new[] { "UpdatedByUserId" });
            DropIndex("dbo.FeatureToggles", new[] { "CreatedByUserId" });
            DropIndex("dbo.DWQueries", new[] { "UpdateByUserId" });
            DropIndex("dbo.DWQueries", new[] { "CreatedByUserId" });
            DropIndex("dbo.BIReports", new[] { "TypeCode" });
            DropIndex("dbo.BIReports", new[] { "DWQueryId" });
            DropIndex("dbo.BIReports", new[] { "UpdatedByUserId" });
            DropIndex("dbo.BIReports", new[] { "CreatedByUserId" });
            DropIndex("dbo.BatchTaskExecutions", new[] { "StatusCode" });
            DropIndex("dbo.BatchTaskExecutions", new[] { "CreatedByUserId" });
            DropIndex("dbo.Teams", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Teams", new[] { "CreatedByUserId" });
            DropIndex("dbo.BusinessRoles", new[] { "UpdatedByUserId" });
            DropIndex("dbo.BusinessRoles", new[] { "CreatedByUserId" });
            DropIndex("dbo.BusinessProcessQueues", new[] { "BusinessRoleId" });
            DropIndex("dbo.BusinessProcessQueues", new[] { "UpdatedByUserId" });
            DropIndex("dbo.BusinessProcessQueues", new[] { "CreatedByUserId" });
            DropIndex("dbo.Activities", new[] { "TeamId" });
            DropIndex("dbo.Activities", new[] { "BusinessProcessQueueId" });
            DropPrimaryKey("dbo.SharedLogisticsSettings");
            DropPrimaryKey("dbo.Teams");
            DropPrimaryKey("dbo.BusinessRoles");
            DropPrimaryKey("dbo.BusinessProcessQueues");
            AlterColumn("dbo.SharedLogisticsSettings", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Teams", "Notes", c => c.String());
            AlterColumn("dbo.Teams", "Notify", c => c.String());
            AlterColumn("dbo.Teams", "LocalName", c => c.String());
            AlterColumn("dbo.Teams", "Name", c => c.String());
            AlterColumn("dbo.Teams", "UpdatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Teams", "CreatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Teams", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.BusinessRoles", "Description", c => c.String());
            AlterColumn("dbo.BusinessRoles", "LocalName", c => c.String());
            AlterColumn("dbo.BusinessRoles", "Name", c => c.String());
            AlterColumn("dbo.BusinessRoles", "UpdatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.BusinessRoles", "CreatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.BusinessRoles", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.BusinessProcessQueues", "Notes", c => c.String());
            AlterColumn("dbo.BusinessProcessQueues", "BusinessRoleId", c => c.String(maxLength: 128));
            AlterColumn("dbo.BusinessProcessQueues", "LocalName", c => c.String());
            AlterColumn("dbo.BusinessProcessQueues", "Name", c => c.String());
            AlterColumn("dbo.BusinessProcessQueues", "UpdatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.BusinessProcessQueues", "CreatedByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.BusinessProcessQueues", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Activities", "TeamId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Activities", "BusinessProcessQueueId", c => c.String(maxLength: 128));
            DropTable("dbo.TeamMemberBusinessRoles");
            DropTable("dbo.LBPTeamMembers");
            DropTable("dbo.Toggles");
            DropTable("dbo.FeatureToggles");
            DropTable("dbo.DWQueries");
            DropTable("dbo.BIReportsTypes");
            DropTable("dbo.BIReports");
            DropTable("dbo.BatchTaskExecutionStatus");
            DropTable("dbo.BatchTaskExecutions");
            AddPrimaryKey("dbo.SharedLogisticsSettings", "Id");
            AddPrimaryKey("dbo.Teams", "Id");
            AddPrimaryKey("dbo.BusinessRoles", "Id");
            AddPrimaryKey("dbo.BusinessProcessQueues", "Id");
            CreateIndex("dbo.Teams", "UpdatedByUserId");
            CreateIndex("dbo.Teams", "CreatedByUserId");
            CreateIndex("dbo.BusinessRoles", "UpdatedByUserId");
            CreateIndex("dbo.BusinessRoles", "CreatedByUserId");
            CreateIndex("dbo.BusinessProcessQueues", "BusinessRoleId");
            CreateIndex("dbo.BusinessProcessQueues", "UpdatedByUserId");
            CreateIndex("dbo.BusinessProcessQueues", "CreatedByUserId");
            CreateIndex("dbo.Activities", "TeamId");
            CreateIndex("dbo.Activities", "BusinessProcessQueueId");
            AddForeignKey("dbo.Activities", "TeamId", "dbo.Teams", "Id");
            AddForeignKey("dbo.BusinessProcessQueues", "BusinessRoleId", "dbo.BusinessRoles", "Id");
            AddForeignKey("dbo.Activities", "BusinessProcessQueueId", "dbo.BusinessProcessQueues", "Id");
        }
    }
}
