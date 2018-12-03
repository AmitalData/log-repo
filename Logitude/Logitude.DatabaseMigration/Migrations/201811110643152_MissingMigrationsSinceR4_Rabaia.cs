namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MissingMigrationsSinceR4_Rabaia : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Journals", "TaxReportStatusCode", "dbo.TaxReportStatuses");
            //DropIndex("dbo.Activities", new[] { "OwnerId" });
            //DropIndex("dbo.Activities", new[] { "BusinessUnitId" });
            //DropIndex("dbo.ActivityOwnerHistories", new[] { "OwnerId" });
            //DropIndex("dbo.Journals", new[] { "TaxReportStatusCode" });
            //DropIndex("Customs.Vehicles", new[] { "ImporterIdentityId" });
            //CreateTable(
            //    "Customs.AcceptanceStatuses",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 3, unicode: false),
            //            Name = c.String(maxLength: 100, unicode: false),
            //            SearchFields = c.String(maxLength: 1000),
            //            LocalName = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "dbo.BusinessProcessQueues",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            SearchFields = c.String(),
            //            Name = c.String(nullable: false, maxLength: 40, unicode: false),
            //            LocalName = c.String(maxLength: 40),
            //            InActive = c.Boolean(nullable: false),
            //            BusinessRoleId = c.String(maxLength: 15, unicode: false),
            //            Notes = c.String(maxLength: 250),
            //            ObjectTableId = c.String(maxLength: 15, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.BusinessRoles", t => t.BusinessRoleId)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.ObjectTables", t => t.ObjectTableId)
            //    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdatedByUserId)
            //    .Index(t => t.BusinessRoleId)
            //    .Index(t => t.ObjectTableId);
            
            //CreateTable(
            //    "dbo.BusinessRoles",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            SearchFields = c.String(),
            //            Name = c.String(nullable: false, maxLength: 40, unicode: false),
            //            LocalName = c.String(maxLength: 40),
            //            InActive = c.Boolean(nullable: false),
            //            Description = c.String(maxLength: 250),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdatedByUserId);
            
            //CreateTable(
            //    "dbo.Teams",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            SearchFields = c.String(),
            //            Name = c.String(nullable: false, maxLength: 40, unicode: false),
            //            LocalName = c.String(maxLength: 40),
            //            InActive = c.Boolean(nullable: false),
            //            ManagerUserId = c.String(maxLength: 15, unicode: false),
            //            Notify = c.String(maxLength: 4000, unicode: false),
            //            Notes = c.String(maxLength: 250),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.Users", t => t.ManagerUserId)
            //    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdatedByUserId)
            //    .Index(t => t.ManagerUserId);
            
            //CreateTable(
            //    "dbo.BatchTaskExecutions",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            SearchFields = c.String(),
            //            ClassName = c.String(maxLength: 120, unicode: false),
            //            PrametersXml = c.String(maxLength: 4000),
            //            StatusCode = c.String(maxLength: 1, unicode: false),
            //            ErrorLog = c.String(maxLength: 4000),
            //            StartDateTime = c.DateTime(),
            //            DoneDateTime = c.DateTime(),
            //            ProgressMessage = c.String(maxLength: 120),
            //            ProgressPercentage = c.Int(nullable: false),
            //            Subject = c.String(maxLength: 200),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.BatchTaskExecutionStatus", t => t.StatusCode)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.StatusCode);
            
            //CreateTable(
            //    "dbo.BatchTaskExecutionStatus",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 1, unicode: false),
            //            Name = c.String(maxLength: 60, unicode: false),
            //            SearchFields = c.String(),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "Customs.MamanStatuses",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 3, unicode: false),
            //            Name = c.String(maxLength: 100, unicode: false),
            //            SearchFields = c.String(maxLength: 1000),
            //            LocalName = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "dbo.DWObjectFields",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            Name = c.String(nullable: false, maxLength: 100, unicode: false),
            //            Code = c.String(nullable: false, maxLength: 100, unicode: false),
            //            DWObjectTableCode = c.String(nullable: false, maxLength: 50, unicode: false),
            //            DataTypeCode = c.String(nullable: false, maxLength: 10, unicode: false),
            //            DimensionTableCode = c.String(maxLength: 50, unicode: false),
            //            MaxLength = c.Int(nullable: false),
            //            MinLength = c.Int(nullable: false),
            //            IsRequired = c.Boolean(nullable: false),
            //            IsPrimaryKey = c.Boolean(nullable: false),
            //            IsMeasurement = c.Boolean(nullable: false),
            //            AggregationTypeCode = c.String(maxLength: 5, unicode: false),
            //            DisplayInQueryBuilder = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.DWObjectTables", t => t.DimensionTableCode)
            //    .ForeignKey("dbo.DWObjectTables", t => t.DWObjectTableCode)
            //    .Index(t => t.DWObjectTableCode)
            //    .Index(t => t.DimensionTableCode);
            
            //CreateTable(
            //    "dbo.DWObjectTables",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 50, unicode: false),
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            Name = c.String(nullable: false, maxLength: 50, unicode: false),
            //            TypeCode = c.String(nullable: false, maxLength: 15, unicode: false),
            //            IsClosed = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "dbo.DWQueries",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            DWObjectTableCode = c.String(nullable: false, maxLength: 50, unicode: false),
            //            SQLString = c.String(),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            UpdateByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            CreatedDate = c.DateTime(nullable: false),
            //            UpdatedDate = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.DWObjectTables", t => t.DWObjectTableCode)
            //    .ForeignKey("dbo.Users", t => t.UpdateByUserId)
            //    .Index(t => t.DWObjectTableCode)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdateByUserId);
            
            //CreateTable(
            //    "dbo.DWQueryColumns",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            DWQueryId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            DWObjectFieldId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            IndexOrder = c.Int(nullable: false),
            //            ColumnWidth = c.Double(nullable: false),
            //            UserId = c.String(maxLength: 15, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.DWObjectFields", t => t.DWObjectFieldId)
            //    .ForeignKey("dbo.DWQueries", t => t.DWQueryId)
            //    .ForeignKey("dbo.Users", t => t.UserId)
            //    .Index(t => t.DWQueryId)
            //    .Index(t => t.DWObjectFieldId)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.DWQueryFilters",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            DWQueryId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            DWObjectFieldId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            IsPredefined = c.Boolean(nullable: false),
            //            PredefinedValue = c.String(maxLength: 100, unicode: false),
            //            PredefinedValue2 = c.String(maxLength: 100, unicode: false),
            //            Operator = c.String(maxLength: 40, unicode: false),
            //            IndexOrder = c.Int(nullable: false),
            //            UserId = c.String(maxLength: 15, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.DWObjectFields", t => t.DWObjectFieldId)
            //    .ForeignKey("dbo.DWQueries", t => t.DWQueryId)
            //    .ForeignKey("dbo.Users", t => t.UserId)
            //    .Index(t => t.DWQueryId)
            //    .Index(t => t.DWObjectFieldId)
            //    .Index(t => t.UserId);
            
            //CreateTable(
            //    "dbo.JournalMoreDatas",
            //    c => new
            //        {
            //            JournalId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Line = c.Int(nullable: false),
            //            Tenant = c.Int(nullable: false),
            //            GeneralData = c.String(nullable: false, maxLength: 4000),
            //            TaxReportId = c.String(maxLength: 15, unicode: false),
            //            TaxReportStatusCode = c.String(maxLength: 1, unicode: false),
            //        })
            //    .PrimaryKey(t => new { t.JournalId, t.Line })
            //    .ForeignKey("dbo.Journals", t => t.JournalId)
            //    .ForeignKey("dbo.TaxReports", t => t.TaxReportId)
            //    .ForeignKey("dbo.TaxReportStatuses", t => t.TaxReportStatusCode)
            //    .Index(t => t.JournalId)
            //    .Index(t => t.TaxReportId)
            //    .Index(t => t.TaxReportStatusCode);
            
            //CreateTable(
            //    "dbo.LBPTeamMembers",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            MemberUserId = c.String(maxLength: 15, unicode: false),
            //            TeamId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            AddDate = c.DateTime(nullable: false),
            //            AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            MemberTeamId = c.String(maxLength: 15, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.AddedByUserId)
            //    .ForeignKey("dbo.Teams", t => t.MemberTeamId)
            //    .ForeignKey("dbo.Users", t => t.MemberUserId)
            //    .ForeignKey("dbo.Teams", t => t.TeamId)
            //    .Index(t => t.MemberUserId)
            //    .Index(t => t.TeamId)
            //    .Index(t => t.AddedByUserId)
            //    .Index(t => t.MemberTeamId);
            
            //CreateTable(
            //    "dbo.SharedLogisticsSettings",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            IsAgentShared = c.Boolean(nullable: false),
            //            IsShipperNotExporterShared = c.Boolean(nullable: false),
            //            IsNotify1Shared = c.Boolean(nullable: false),
            //            IsNotify2Shared = c.Boolean(nullable: false),
            //            IsFreightForwarderShared = c.Boolean(nullable: false),
            //            IsColoaderShared = c.Boolean(nullable: false),
            //            IsConsigneeNotImporterShared = c.Boolean(nullable: false),
            //            IsMainCarrierShared = c.Boolean(nullable: false),
            //            IsPickDelivCarriesShared = c.Boolean(nullable: false),
            //            IsInvoicesMenuEnabled = c.Boolean(nullable: false),
            //            IsMoneyTabEnabled = c.Boolean(nullable: false),
            //            IsIssuingCarrierAgentShared = c.Boolean(nullable: false),
            //            IsCustomsAgentExportShared = c.Boolean(nullable: false),
            //            IsCustomsAgentImportShared = c.Boolean(nullable: false),
            //            IsCustomClearancePoinShared = c.Boolean(nullable: false),
            //            IsConsolidatorShared = c.Boolean(nullable: false),
            //            IsReleasingAgentShared = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.ShipmentPackageHarmonize",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            PackageId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Harmonize = c.String(nullable: false, maxLength: 60, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.ShipmentPackages", t => t.PackageId)
            //    .Index(t => t.PackageId);
            
            //CreateTable(
            //    "dbo.Sprints",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            SearchFields = c.String(),
            //            FromDate = c.DateTime(nullable: false),
            //            ToDate = c.DateTime(nullable: false),
            //            Name = c.String(nullable: false, maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdatedByUserId);
            
            //CreateTable(
            //    "dbo.TaxDeductionReports",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            CreateDate = c.DateTime(nullable: false),
            //            CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            UpdateDate = c.DateTime(nullable: false),
            //            UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            SearchFields = c.String(),
            //            ReportNumber = c.String(maxLength: 20, unicode: false),
            //            StatusTypeCode = c.String(maxLength: 1, unicode: false),
            //            IsAdditionalReportExist = c.Boolean(nullable: false),
            //            TaxYear = c.Int(),
            //            Email = c.String(maxLength: 70, unicode: false),
            //            ErrorMessage = c.String(maxLength: 4000),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.CreatedByUserId)
            //    .ForeignKey("dbo.TaxDeductionReportStatuses", t => t.StatusTypeCode)
            //    .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
            //    .Index(t => t.CreatedByUserId)
            //    .Index(t => t.UpdatedByUserId)
            //    .Index(t => t.StatusTypeCode);
            
            //CreateTable(
            //    "dbo.TaxDeductionReportStatuses",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 1, unicode: false),
            //            EnglishName = c.String(maxLength: 100, unicode: false),
            //            SearchFields = c.String(),
            //            LocalName = c.String(maxLength: 30),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "dbo.TeamMemberBusinessRoles",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            TeamMemberId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            AddDate = c.DateTime(nullable: false),
            //            BusinessRoleId = c.String(nullable: false, maxLength: 15, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Users", t => t.AddedByUserId)
            //    .ForeignKey("dbo.BusinessRoles", t => t.BusinessRoleId)
            //    .ForeignKey("dbo.LBPTeamMembers", t => t.TeamMemberId)
            //    .Index(t => t.TeamMemberId)
            //    .Index(t => t.AddedByUserId)
            //    .Index(t => t.BusinessRoleId);
            
            //CreateTable(
            //    "dbo.TMBudgets",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            SearchFields = c.String(),
            //            Name = c.String(nullable: false, maxLength: 70),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.TMProjectCategories",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            SearchFields = c.String(),
            //            Name = c.String(nullable: false, maxLength: 60),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.TMReleases",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            ReleaseName = c.String(nullable: false, maxLength: 40, unicode: false),
            //            FromDate = c.DateTime(nullable: false),
            //            ToDate = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "Customs.CustomsAirlines",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            AirlineCode = c.String(nullable: false, maxLength: 2, unicode: false),
            //            LocalName = c.String(maxLength: 100),
            //            EnglishName = c.String(maxLength: 70, unicode: false),
            //            InActive = c.Boolean(nullable: false),
            //            SearchFields = c.String(maxLength: 500),
            //            AirlinePrefix = c.String(nullable: false, maxLength: 3, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.Tenants", "StockTypeCode", c => c.String());
            //AddColumn("dbo.Cards", "IsInternationalPartner", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Cards", "IsAutonomy", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Customers", "ActivationDate", c => c.DateTime());
            //AddColumn("dbo.Customers", "InactiveDate", c => c.DateTime());
            //AddColumn("dbo.Customers", "ActivationRequestDate", c => c.DateTime());
            //AddColumn("dbo.Customers", "ActivatedByUserId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Customers", "SetAsInactiveByUserId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Customers", "ActivationRequestedByUserId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Activities", "DueDateOffset", c => c.Int());
            //AddColumn("dbo.Activities", "DueDateDateField", c => c.String(maxLength: 100, unicode: false));
            //AddColumn("dbo.Activities", "BusinessProcessQueueId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Activities", "TeamId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.Activities", "ShipmentId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.ObjectTables", "AllowedInQueues", c => c.Boolean(nullable: false));
            //AddColumn("dbo.ARPayments", "TipoCadenaPago", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("dbo.ARPayments", "CertPago", c => c.String(unicode: false));
            //AddColumn("dbo.ARPayments", "CadPago", c => c.String(maxLength: 200, unicode: false));
            //AddColumn("dbo.ARPayments", "SelloPago", c => c.String(unicode: false));
            //AddColumn("dbo.Shipments", "Field21", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field22", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field23", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field24", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field25", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field26", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field27", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field28", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field29", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field30", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field31", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field32", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field33", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field34", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field35", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field36", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field37", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field38", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field39", c => c.String(maxLength: 250));
            //AddColumn("dbo.Shipments", "Field40", c => c.String(maxLength: 250));
            //AddColumn("dbo.GLAccounts", "ExcludeFromDeductionReport", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Declarations", "AcceptanceStatusCode", c => c.String(maxLength: 3, unicode: false));
            //AddColumn("Customs.Declarations", "CasualImporterAddress1", c => c.String(maxLength: 35));
            //AddColumn("Customs.Declarations", "CasualImporterAddress2", c => c.String(maxLength: 35));
            //AddColumn("Customs.Declarations", "CasualImporterCity", c => c.String(maxLength: 17));
            //AddColumn("Customs.Declarations", "CasualImporterZipCode", c => c.String(maxLength: 10, unicode: false));
            //AddColumn("Customs.Declarations", "CasualImporterFax", c => c.String(maxLength: 30, unicode: false));
            //AddColumn("Customs.Declarations", "CasualImporterEmail", c => c.String(maxLength: 50, unicode: false));
            //AddColumn("Customs.Declarations", "CasualImporterTel", c => c.String(maxLength: 30, unicode: false));
            //AddColumn("Customs.Declarations", "CasualImporterContact", c => c.String(maxLength: 50));
            //AddColumn("Customs.Declarations", "MamanStatusCode", c => c.String(maxLength: 3, unicode: false));
            //AddColumn("Customs.Declarations", "MamanErrorXml", c => c.String(unicode: false));
            //AddColumn("Customs.Declarations", "ItemsProcessTypesList", c => c.String(unicode: false));
            //AddColumn("Customs.Declarations", "IsClose", c => c.Boolean(nullable: false));
            //AddColumn("Customs.CourierMasters", "PackageQuantity", c => c.Int());
            //AddColumn("Customs.CourierMasters", "GrossMassMeasure", c => c.Decimal(precision: 18, scale: 2));
            //AddColumn("Customs.CourierMasters", "ShortHAWB", c => c.String(maxLength: 8));
            //AddColumn("Customs.CourierMasters", "FlightNumber", c => c.String(maxLength: 4));
            //AddColumn("Customs.CourierMasters", "DepartureDate", c => c.DateTime());
            //AddColumn("dbo.CustomerTenantAccesses", "StockTypeCode", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.EntityChanges", "QueuedTaskAutomationFailedXml", c => c.String());
            //AddColumn("dbo.EntityChanges", "QueuedTaskAutomationSsucceedXml", c => c.String());
            //AddColumn("dbo.JournalLines", "ExternalReconcileNumber", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.FullAccountingSettings", "DefaultDifferencesGLAccountId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.FullAccountingSettings", "DefaultExternalDiffGLAccountId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.ShipmentPackages", "IsMultiHarmonize", c => c.Boolean(nullable: false));
            //AddColumn("dbo.QuoteSettings", "IsSaleAsCostCurrency", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "TaxiMedalOwner", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("Customs.Vehicles", "ImporterPassportNumber", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("Customs.Vehicles", "ImporterPassCountryCode", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("Customs.Vehicles", "ImporterPassportTypeCode", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("Customs.Vehicles", "IsCBS", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "IsSlipperClutch", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "IsSteeringDamper", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "IsTCS", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "IsTPS", c => c.Boolean(nullable: false));
            //AddColumn("Customs.Vehicles", "VehicleCategory", c => c.String(maxLength: 3, unicode: false));
            //AddColumn("Customs.Vehicles", "VehicleMaxPowerKW", c => c.Decimal(precision: 7, scale: 2));
            //AddColumn("dbo.TaxReports", "NeedsRebulid", c => c.Boolean(nullable: false));
            //AddColumn("dbo.TenantLoginPolicies", "SessionTimeout", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AddColumn("dbo.TMEmployeeTimes", "SprintId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("dbo.TMEmployeeTimes", "ProratedDuration", c => c.Double(nullable: false));
            //AddColumn("dbo.TMEmployeeTimes", "FullDuration", c => c.Double(nullable: false));
            //AddColumn("dbo.TMEmployeeTimes", "NeedsProrating", c => c.Boolean(nullable: false));
            //AddColumn("dbo.TMProjects", "BudgetId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AddColumn("dbo.TMProjects", "CategoryId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //AddColumn("dbo.TMProjects", "IsProrated", c => c.Boolean(nullable: false));
            //AddColumn("dbo.TMProjects", "ExternalProjectNumber", c => c.String(maxLength: 10, unicode: false));
            //AddColumn("Customs.VehicleOwners", "PassportNumber", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("Customs.VehicleOwners", "PassCountryCode", c => c.String(maxLength: 2, unicode: false));
            //AddColumn("Customs.VehicleOwners", "ImporterPassportTypeCode", c => c.String(maxLength: 2, unicode: false));
            //AlterColumn("dbo.Activities", "OwnerId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.Activities", "BusinessUnitId", c => c.String(maxLength: 50, unicode: false));
            //AlterColumn("dbo.ActivityOwnerHistories", "OwnerId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.GLAccounts", "LocalName", c => c.String(nullable: false, maxLength: 105));
            //AlterColumn("dbo.GLAccounts", "EnglishName", c => c.String(maxLength: 75, unicode: false));
            //AlterColumn("dbo.GLAccounts", "PreviousEnglishName", c => c.String(maxLength: 75, unicode: false));
            //AlterColumn("dbo.GLAccounts", "PreviousLocalName", c => c.String(maxLength: 105));
            //AlterColumn("Customs.Claims", "SearchFields", c => c.String(maxLength: 2000));
            //AlterColumn("dbo.ExternalReconciliations", "SearchFields", c => c.String());
            //AlterColumn("dbo.LedgerTransactions", "Reference1", c => c.String(maxLength: 30));
            //AlterColumn("dbo.LedgerTransactions", "Reference2", c => c.String(maxLength: 30));
            //AlterColumn("dbo.LedgerTransactions", "Reference3", c => c.String(maxLength: 30));
            //AlterColumn("dbo.JournalLines", "Reference1", c => c.String(maxLength: 30));
            //AlterColumn("dbo.JournalLines", "Reference2", c => c.String(maxLength: 30));
            //AlterColumn("dbo.JournalLines", "Reference3", c => c.String(maxLength: 30));
            //AlterColumn("Customs.Vehicles", "VehiclePowerKW", c => c.Decimal(precision: 12, scale: 2));
            //AlterColumn("Customs.Vehicles", "ImporterIdentityId", c => c.String(maxLength: 15, unicode: false));
            //AlterColumn("dbo.TaxReportLines", "IsEquipment", c => c.Boolean(nullable: false));
            //AlterColumn("dbo.TaxReports", "TaxReportMonth", c => c.DateTime(nullable: false));
            //CreateIndex("dbo.Customers", "ActivatedByUserId");
            //CreateIndex("dbo.Customers", "SetAsInactiveByUserId");
            //CreateIndex("dbo.Customers", "ActivationRequestedByUserId");
            //CreateIndex("dbo.Activities", "OwnerId");
            //CreateIndex("dbo.Activities", "BusinessUnitId");
            //CreateIndex("dbo.Activities", "BusinessProcessQueueId");
            //CreateIndex("dbo.Activities", "TeamId");
            //CreateIndex("dbo.ActivityOwnerHistories", "OwnerId");
            //CreateIndex("Customs.Declarations", "AcceptanceStatusCode");
            //CreateIndex("Customs.Declarations", "MamanStatusCode");
            //CreateIndex("dbo.FullAccountingSettings", "DefaultDifferencesGLAccountId");
            //CreateIndex("dbo.FullAccountingSettings", "DefaultExternalDiffGLAccountId");
            //CreateIndex("Customs.Vehicles", "ImporterIdentityId");
            //CreateIndex("Customs.Vehicles", "ImporterPassCountryCode");
            //CreateIndex("Customs.Vehicles", "ImporterPassportTypeCode");
            //CreateIndex("dbo.TaxReportLines", "JournalId");
            //CreateIndex("dbo.TMProjects", "BudgetId");
            //CreateIndex("dbo.TMProjects", "CategoryId");
            //CreateIndex("Customs.VehicleOwners", "PassCountryCode");
            //CreateIndex("Customs.VehicleOwners", "ImporterPassportTypeCode");
            //AddForeignKey("dbo.Customers", "ActivatedByUserId", "dbo.Users", "Id");
            //AddForeignKey("dbo.Customers", "ActivationRequestedByUserId", "dbo.Users", "Id");
            //AddForeignKey("dbo.Customers", "SetAsInactiveByUserId", "dbo.Users", "Id");
            //AddForeignKey("dbo.Activities", "BusinessProcessQueueId", "dbo.BusinessProcessQueues", "Id");
            //AddForeignKey("dbo.Activities", "TeamId", "dbo.Teams", "Id");
            //AddForeignKey("Customs.Declarations", "AcceptanceStatusCode", "Customs.AcceptanceStatuses", "Code");
            //AddForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses", "Code");
            //AddForeignKey("dbo.FullAccountingSettings", "DefaultDifferencesGLAccountId", "dbo.GLAccounts", "Id");
            //AddForeignKey("dbo.FullAccountingSettings", "DefaultExternalDiffGLAccountId", "dbo.GLAccounts", "Id");
            //AddForeignKey("Customs.Vehicles", "ImporterPassCountryCode", "Customs.CustomsCountries", "Code");
            //AddForeignKey("Customs.Vehicles", "ImporterPassportTypeCode", "Customs.PassportTypes", "Code");
            //AddForeignKey("dbo.TaxReportLines", "JournalId", "dbo.Journals", "Id");
            //AddForeignKey("dbo.TMProjects", "BudgetId", "dbo.TMBudgets", "Id");
            //AddForeignKey("dbo.TMProjects", "CategoryId", "dbo.TMProjectCategories", "Id");
            //AddForeignKey("Customs.VehicleOwners", "PassCountryCode", "Customs.CustomsCountries", "Code");
            //AddForeignKey("Customs.VehicleOwners", "ImporterPassportTypeCode", "Customs.PassportTypes", "Code");
            //DropColumn("dbo.Journals", "TaxReportId");
            //DropColumn("dbo.Journals", "TaxReportStatusCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Journals", "TaxReportStatusCode", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.Journals", "TaxReportId", c => c.String(maxLength: 15, unicode: false));
            DropForeignKey("Customs.VehicleOwners", "ImporterPassportTypeCode", "Customs.PassportTypes");
            DropForeignKey("Customs.VehicleOwners", "PassCountryCode", "Customs.CustomsCountries");
            DropForeignKey("dbo.TMProjects", "CategoryId", "dbo.TMProjectCategories");
            DropForeignKey("dbo.TMProjects", "BudgetId", "dbo.TMBudgets");
            DropForeignKey("dbo.TeamMemberBusinessRoles", "TeamMemberId", "dbo.LBPTeamMembers");
            DropForeignKey("dbo.TeamMemberBusinessRoles", "BusinessRoleId", "dbo.BusinessRoles");
            DropForeignKey("dbo.TeamMemberBusinessRoles", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.TaxReportLines", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.TaxDeductionReports", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.TaxDeductionReports", "StatusTypeCode", "dbo.TaxDeductionReportStatuses");
            DropForeignKey("dbo.TaxDeductionReports", "CreatedByUserId", "dbo.Users");
            DropForeignKey("Customs.Vehicles", "ImporterPassportTypeCode", "Customs.PassportTypes");
            DropForeignKey("Customs.Vehicles", "ImporterPassCountryCode", "Customs.CustomsCountries");
            DropForeignKey("dbo.Sprints", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Sprints", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentPackageHarmonize", "PackageId", "dbo.ShipmentPackages");
            DropForeignKey("dbo.LBPTeamMembers", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.LBPTeamMembers", "MemberUserId", "dbo.Users");
            DropForeignKey("dbo.LBPTeamMembers", "MemberTeamId", "dbo.Teams");
            DropForeignKey("dbo.LBPTeamMembers", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.JournalMoreDatas", "TaxReportStatusCode", "dbo.TaxReportStatuses");
            DropForeignKey("dbo.JournalMoreDatas", "TaxReportId", "dbo.TaxReports");
            DropForeignKey("dbo.JournalMoreDatas", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.FullAccountingSettings", "DefaultExternalDiffGLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.FullAccountingSettings", "DefaultDifferencesGLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.DWQueryFilters", "UserId", "dbo.Users");
            DropForeignKey("dbo.DWQueryFilters", "DWQueryId", "dbo.DWQueries");
            DropForeignKey("dbo.DWQueryFilters", "DWObjectFieldId", "dbo.DWObjectFields");
            DropForeignKey("dbo.DWQueryColumns", "UserId", "dbo.Users");
            DropForeignKey("dbo.DWQueryColumns", "DWQueryId", "dbo.DWQueries");
            DropForeignKey("dbo.DWQueryColumns", "DWObjectFieldId", "dbo.DWObjectFields");
            DropForeignKey("dbo.DWQueries", "UpdateByUserId", "dbo.Users");
            DropForeignKey("dbo.DWQueries", "DWObjectTableCode", "dbo.DWObjectTables");
            DropForeignKey("dbo.DWQueries", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.DWObjectFields", "DWObjectTableCode", "dbo.DWObjectTables");
            DropForeignKey("dbo.DWObjectFields", "DimensionTableCode", "dbo.DWObjectTables");
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses");
            DropForeignKey("Customs.Declarations", "AcceptanceStatusCode", "Customs.AcceptanceStatuses");
            DropForeignKey("dbo.BatchTaskExecutions", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BatchTaskExecutions", "StatusCode", "dbo.BatchTaskExecutionStatus");
            DropForeignKey("dbo.Activities", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Teams", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Teams", "ManagerUserId", "dbo.Users");
            DropForeignKey("dbo.Teams", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Activities", "BusinessProcessQueueId", "dbo.BusinessProcessQueues");
            DropForeignKey("dbo.BusinessProcessQueues", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BusinessProcessQueues", "ObjectTableId", "dbo.ObjectTables");
            DropForeignKey("dbo.BusinessProcessQueues", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BusinessProcessQueues", "BusinessRoleId", "dbo.BusinessRoles");
            DropForeignKey("dbo.BusinessRoles", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.BusinessRoles", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "SetAsInactiveByUserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "ActivationRequestedByUserId", "dbo.Users");
            DropForeignKey("dbo.Customers", "ActivatedByUserId", "dbo.Users");
            DropIndex("Customs.VehicleOwners", new[] { "ImporterPassportTypeCode" });
            DropIndex("Customs.VehicleOwners", new[] { "PassCountryCode" });
            DropIndex("dbo.TMProjects", new[] { "CategoryId" });
            DropIndex("dbo.TMProjects", new[] { "BudgetId" });
            DropIndex("dbo.TeamMemberBusinessRoles", new[] { "BusinessRoleId" });
            DropIndex("dbo.TeamMemberBusinessRoles", new[] { "AddedByUserId" });
            DropIndex("dbo.TeamMemberBusinessRoles", new[] { "TeamMemberId" });
            DropIndex("dbo.TaxReportLines", new[] { "JournalId" });
            DropIndex("dbo.TaxDeductionReports", new[] { "StatusTypeCode" });
            DropIndex("dbo.TaxDeductionReports", new[] { "UpdatedByUserId" });
            DropIndex("dbo.TaxDeductionReports", new[] { "CreatedByUserId" });
            DropIndex("Customs.Vehicles", new[] { "ImporterPassportTypeCode" });
            DropIndex("Customs.Vehicles", new[] { "ImporterPassCountryCode" });
            DropIndex("Customs.Vehicles", new[] { "ImporterIdentityId" });
            DropIndex("dbo.Sprints", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Sprints", new[] { "CreatedByUserId" });
            DropIndex("dbo.ShipmentPackageHarmonize", new[] { "PackageId" });
            DropIndex("dbo.LBPTeamMembers", new[] { "MemberTeamId" });
            DropIndex("dbo.LBPTeamMembers", new[] { "AddedByUserId" });
            DropIndex("dbo.LBPTeamMembers", new[] { "TeamId" });
            DropIndex("dbo.LBPTeamMembers", new[] { "MemberUserId" });
            DropIndex("dbo.JournalMoreDatas", new[] { "TaxReportStatusCode" });
            DropIndex("dbo.JournalMoreDatas", new[] { "TaxReportId" });
            DropIndex("dbo.JournalMoreDatas", new[] { "JournalId" });
            DropIndex("dbo.FullAccountingSettings", new[] { "DefaultExternalDiffGLAccountId" });
            DropIndex("dbo.FullAccountingSettings", new[] { "DefaultDifferencesGLAccountId" });
            DropIndex("dbo.DWQueryFilters", new[] { "UserId" });
            DropIndex("dbo.DWQueryFilters", new[] { "DWObjectFieldId" });
            DropIndex("dbo.DWQueryFilters", new[] { "DWQueryId" });
            DropIndex("dbo.DWQueryColumns", new[] { "UserId" });
            DropIndex("dbo.DWQueryColumns", new[] { "DWObjectFieldId" });
            DropIndex("dbo.DWQueryColumns", new[] { "DWQueryId" });
            DropIndex("dbo.DWQueries", new[] { "UpdateByUserId" });
            DropIndex("dbo.DWQueries", new[] { "CreatedByUserId" });
            DropIndex("dbo.DWQueries", new[] { "DWObjectTableCode" });
            DropIndex("dbo.DWObjectFields", new[] { "DimensionTableCode" });
            DropIndex("dbo.DWObjectFields", new[] { "DWObjectTableCode" });
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            DropIndex("Customs.Declarations", new[] { "AcceptanceStatusCode" });
            DropIndex("dbo.BatchTaskExecutions", new[] { "StatusCode" });
            DropIndex("dbo.BatchTaskExecutions", new[] { "CreatedByUserId" });
            DropIndex("dbo.ActivityOwnerHistories", new[] { "OwnerId" });
            DropIndex("dbo.Teams", new[] { "ManagerUserId" });
            DropIndex("dbo.Teams", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Teams", new[] { "CreatedByUserId" });
            DropIndex("dbo.BusinessRoles", new[] { "UpdatedByUserId" });
            DropIndex("dbo.BusinessRoles", new[] { "CreatedByUserId" });
            DropIndex("dbo.BusinessProcessQueues", new[] { "ObjectTableId" });
            DropIndex("dbo.BusinessProcessQueues", new[] { "BusinessRoleId" });
            DropIndex("dbo.BusinessProcessQueues", new[] { "UpdatedByUserId" });
            DropIndex("dbo.BusinessProcessQueues", new[] { "CreatedByUserId" });
            DropIndex("dbo.Activities", new[] { "TeamId" });
            DropIndex("dbo.Activities", new[] { "BusinessProcessQueueId" });
            DropIndex("dbo.Activities", new[] { "BusinessUnitId" });
            DropIndex("dbo.Activities", new[] { "OwnerId" });
            DropIndex("dbo.Customers", new[] { "ActivationRequestedByUserId" });
            DropIndex("dbo.Customers", new[] { "SetAsInactiveByUserId" });
            DropIndex("dbo.Customers", new[] { "ActivatedByUserId" });
            AlterColumn("dbo.TaxReports", "TaxReportMonth", c => c.DateTime());
            AlterColumn("dbo.TaxReportLines", "IsEquipment", c => c.Boolean());
            AlterColumn("Customs.Vehicles", "ImporterIdentityId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("Customs.Vehicles", "VehiclePowerKW", c => c.Int());
            AlterColumn("dbo.JournalLines", "Reference3", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.JournalLines", "Reference2", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.JournalLines", "Reference1", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.LedgerTransactions", "Reference3", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.LedgerTransactions", "Reference2", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.LedgerTransactions", "Reference1", c => c.String(maxLength: 30, unicode: false));
            AlterColumn("dbo.ExternalReconciliations", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("Customs.Claims", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.GLAccounts", "PreviousLocalName", c => c.String(maxLength: 200));
            AlterColumn("dbo.GLAccounts", "PreviousEnglishName", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.GLAccounts", "EnglishName", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.GLAccounts", "LocalName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.ActivityOwnerHistories", "OwnerId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.Activities", "BusinessUnitId", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Activities", "OwnerId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            DropColumn("Customs.VehicleOwners", "ImporterPassportTypeCode");
            DropColumn("Customs.VehicleOwners", "PassCountryCode");
            DropColumn("Customs.VehicleOwners", "PassportNumber");
            DropColumn("dbo.TMProjects", "ExternalProjectNumber");
            DropColumn("dbo.TMProjects", "IsProrated");
            DropColumn("dbo.TMProjects", "CategoryId");
            DropColumn("dbo.TMProjects", "BudgetId");
            DropColumn("dbo.TMEmployeeTimes", "NeedsProrating");
            DropColumn("dbo.TMEmployeeTimes", "FullDuration");
            DropColumn("dbo.TMEmployeeTimes", "ProratedDuration");
            DropColumn("dbo.TMEmployeeTimes", "SprintId");
            DropColumn("dbo.TenantLoginPolicies", "SessionTimeout");
            DropColumn("dbo.TaxReports", "NeedsRebulid");
            DropColumn("Customs.Vehicles", "VehicleMaxPowerKW");
            DropColumn("Customs.Vehicles", "VehicleCategory");
            DropColumn("Customs.Vehicles", "IsTPS");
            DropColumn("Customs.Vehicles", "IsTCS");
            DropColumn("Customs.Vehicles", "IsSteeringDamper");
            DropColumn("Customs.Vehicles", "IsSlipperClutch");
            DropColumn("Customs.Vehicles", "IsCBS");
            DropColumn("Customs.Vehicles", "ImporterPassportTypeCode");
            DropColumn("Customs.Vehicles", "ImporterPassCountryCode");
            DropColumn("Customs.Vehicles", "ImporterPassportNumber");
            DropColumn("Customs.Vehicles", "TaxiMedalOwner");
            DropColumn("dbo.QuoteSettings", "IsSaleAsCostCurrency");
            DropColumn("dbo.ShipmentPackages", "IsMultiHarmonize");
            DropColumn("dbo.FullAccountingSettings", "DefaultExternalDiffGLAccountId");
            DropColumn("dbo.FullAccountingSettings", "DefaultDifferencesGLAccountId");
            DropColumn("dbo.JournalLines", "ExternalReconcileNumber");
            DropColumn("dbo.EntityChanges", "QueuedTaskAutomationSsucceedXml");
            DropColumn("dbo.EntityChanges", "QueuedTaskAutomationFailedXml");
            DropColumn("dbo.CustomerTenantAccesses", "StockTypeCode");
            DropColumn("Customs.CourierMasters", "DepartureDate");
            DropColumn("Customs.CourierMasters", "FlightNumber");
            DropColumn("Customs.CourierMasters", "ShortHAWB");
            DropColumn("Customs.CourierMasters", "GrossMassMeasure");
            DropColumn("Customs.CourierMasters", "PackageQuantity");
            DropColumn("Customs.Declarations", "IsClose");
            DropColumn("Customs.Declarations", "ItemsProcessTypesList");
            DropColumn("Customs.Declarations", "MamanErrorXml");
            DropColumn("Customs.Declarations", "MamanStatusCode");
            DropColumn("Customs.Declarations", "CasualImporterContact");
            DropColumn("Customs.Declarations", "CasualImporterTel");
            DropColumn("Customs.Declarations", "CasualImporterEmail");
            DropColumn("Customs.Declarations", "CasualImporterFax");
            DropColumn("Customs.Declarations", "CasualImporterZipCode");
            DropColumn("Customs.Declarations", "CasualImporterCity");
            DropColumn("Customs.Declarations", "CasualImporterAddress2");
            DropColumn("Customs.Declarations", "CasualImporterAddress1");
            DropColumn("Customs.Declarations", "AcceptanceStatusCode");
            DropColumn("dbo.GLAccounts", "ExcludeFromDeductionReport");
            DropColumn("dbo.Shipments", "Field40");
            DropColumn("dbo.Shipments", "Field39");
            DropColumn("dbo.Shipments", "Field38");
            DropColumn("dbo.Shipments", "Field37");
            DropColumn("dbo.Shipments", "Field36");
            DropColumn("dbo.Shipments", "Field35");
            DropColumn("dbo.Shipments", "Field34");
            DropColumn("dbo.Shipments", "Field33");
            DropColumn("dbo.Shipments", "Field32");
            DropColumn("dbo.Shipments", "Field31");
            DropColumn("dbo.Shipments", "Field30");
            DropColumn("dbo.Shipments", "Field29");
            DropColumn("dbo.Shipments", "Field28");
            DropColumn("dbo.Shipments", "Field27");
            DropColumn("dbo.Shipments", "Field26");
            DropColumn("dbo.Shipments", "Field25");
            DropColumn("dbo.Shipments", "Field24");
            DropColumn("dbo.Shipments", "Field23");
            DropColumn("dbo.Shipments", "Field22");
            DropColumn("dbo.Shipments", "Field21");
            DropColumn("dbo.ARPayments", "SelloPago");
            DropColumn("dbo.ARPayments", "CadPago");
            DropColumn("dbo.ARPayments", "CertPago");
            DropColumn("dbo.ARPayments", "TipoCadenaPago");
            DropColumn("dbo.ObjectTables", "AllowedInQueues");
            DropColumn("dbo.Activities", "ShipmentId");
            DropColumn("dbo.Activities", "TeamId");
            DropColumn("dbo.Activities", "BusinessProcessQueueId");
            DropColumn("dbo.Activities", "DueDateDateField");
            DropColumn("dbo.Activities", "DueDateOffset");
            DropColumn("dbo.Customers", "ActivationRequestedByUserId");
            DropColumn("dbo.Customers", "SetAsInactiveByUserId");
            DropColumn("dbo.Customers", "ActivatedByUserId");
            DropColumn("dbo.Customers", "ActivationRequestDate");
            DropColumn("dbo.Customers", "InactiveDate");
            DropColumn("dbo.Customers", "ActivationDate");
            DropColumn("dbo.Cards", "IsAutonomy");
            DropColumn("dbo.Cards", "IsInternationalPartner");
            DropColumn("dbo.Tenants", "StockTypeCode");
            DropTable("Customs.CustomsAirlines");
            DropTable("dbo.TMReleases");
            DropTable("dbo.TMProjectCategories");
            DropTable("dbo.TMBudgets");
            DropTable("dbo.TeamMemberBusinessRoles");
            DropTable("dbo.TaxDeductionReportStatuses");
            DropTable("dbo.TaxDeductionReports");
            DropTable("dbo.Sprints");
            DropTable("dbo.ShipmentPackageHarmonize");
            DropTable("dbo.SharedLogisticsSettings");
            DropTable("dbo.LBPTeamMembers");
            DropTable("dbo.JournalMoreDatas");
            DropTable("dbo.DWQueryFilters");
            DropTable("dbo.DWQueryColumns");
            DropTable("dbo.DWQueries");
            DropTable("dbo.DWObjectTables");
            DropTable("dbo.DWObjectFields");
            DropTable("Customs.MamanStatuses");
            DropTable("dbo.BatchTaskExecutionStatus");
            DropTable("dbo.BatchTaskExecutions");
            DropTable("dbo.Teams");
            DropTable("dbo.BusinessRoles");
            DropTable("dbo.BusinessProcessQueues");
            DropTable("Customs.AcceptanceStatuses");
            CreateIndex("Customs.Vehicles", "ImporterIdentityId");
            CreateIndex("dbo.Journals", "TaxReportStatusCode");
            CreateIndex("dbo.ActivityOwnerHistories", "OwnerId");
            CreateIndex("dbo.Activities", "BusinessUnitId");
            CreateIndex("dbo.Activities", "OwnerId");
            AddForeignKey("dbo.Journals", "TaxReportStatusCode", "dbo.TaxReportStatuses", "Code");
        }
    }
}
