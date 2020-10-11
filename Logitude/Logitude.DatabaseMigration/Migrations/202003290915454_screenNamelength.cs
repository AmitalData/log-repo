namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class screenNamelength : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AmendmentStatus", newName: "AmendmentStatuses");
            RenameTable(name: "dbo.PaymentGatewayPartners", newName: "TariffSurchargesUpdateMethods");
            MoveTable(name: "dbo.ContinuousRequestTypes", newSchema: "Customs");
            MoveTable(name: "dbo.DecisionTypes", newSchema: "Customs");
            MoveTable(name: "dbo.AmendmentStatuses", newSchema: "Customs");
            DropForeignKey("dbo.Tenants", "LogBoxAdminUserId", "dbo.Contacts");
            DropForeignKey("dbo.AirlineAreas", "AirlineId", "dbo.Airlines");
            DropForeignKey("dbo.AirlineAreas", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.AirlineAreas", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.AirlineAreasPorts", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.AirlineAreasPorts", "AirlineAreaId", "dbo.AirlineAreas");
            DropForeignKey("dbo.AirlineAreasPorts", "PortId", "dbo.Ports");
            DropForeignKey("dbo.ReconcileExternalPages", "BankAccountId", "dbo.BankAccounts");
            DropForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory");
            DropForeignKey("dbo.JournalLines", "ActionCode", "dbo.JournalActionTypes");
            DropForeignKey("dbo.Quotes", "QuoteClosingReasonCode", "dbo.QuoteClosingReasons");
            DropForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "dbo.ContinuousRequestTypes");
            DropForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes");
            DropForeignKey("Customs.Declarations", "AmendmentStatus", "dbo.AmendmentStatus");
            DropForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners");
            DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            DropIndex("dbo.Tenants", new[] { "LogBoxAdminUserId" });
            DropIndex("dbo.Quotes", new[] { "QuoteClosingReasonCode" });
            DropIndex("dbo.AirlineAreas", new[] { "AirlineId" });
            DropIndex("dbo.AirlineAreas", new[] { "CreatedByUserId" });
            DropIndex("dbo.AirlineAreas", new[] { "UpdatedByUserId" });
            DropIndex("dbo.AirlineAreasPorts", new[] { "AirlineAreaId" });
            DropIndex("dbo.AirlineAreasPorts", new[] { "PortId" });
            DropIndex("dbo.AirlineAreasPorts", new[] { "AddedByUserId" });
            DropIndex("dbo.APInvoiceLines", new[] { "ForiegnCurrencyId" });
            DropIndex("dbo.APPayments", new[] { "AccountingPaymentMethodId" });
            DropIndex("dbo.APPayments", new[] { "ApprovedByUserId" });
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "DecisionCode" });
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "ContinuousRequestTypeCode" });
            DropIndex("Customs.Declarations", new[] { "AmendmentStatus" });
            DropIndex("dbo.JournalLines", new[] { "ActionCode" });
            DropIndex("dbo.ReconcileExternalPages", new[] { "BankAccountId" });
            DropIndex("dbo.QueueMessageMoreDetails", new[] { "QueueDefinitionCode" });
            DropIndex("dbo.SchedulerLogs", new[] { "HistoryId" });
            DropPrimaryKey("dbo.QuoteClosingReasons");
            DropPrimaryKey("Customs.ContinuousRequestTypes");
            DropPrimaryKey("Customs.DecisionTypes");
            DropPrimaryKey("Customs.AmendmentStatuses");
            DropPrimaryKey("dbo.CustomerCompetitorProducts");
            DropPrimaryKey("dbo.TariffSurchargesUpdateMethods");
            CreateTable(
                "dbo.AccountingPartners",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PrimaryContactName = c.String(maxLength: 60, unicode: false),
                        PrimaryContactEmail = c.String(maxLength: 70, unicode: false),
                        PrimaryContactPhone = c.String(maxLength: 25, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.SharedLogisticsContactLastLogins",
                c => new
                    {
                        ContactId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CardId = c.String(nullable: false, maxLength: 15, unicode: false),
                        PartnerTypeId = c.String(nullable: false, maxLength: 2, unicode: false),
                        Via = c.String(nullable: false, maxLength: 20, unicode: false),
                        LoginDateTime = c.DateTime(),
                        Tenant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ContactId, t.CardId, t.PartnerTypeId, t.Via })
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.LogBoxTenantSettings",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IsDocumentsArchive = c.Boolean(nullable: false),
                        CustomerTenantShareImportFile = c.Boolean(nullable: false),
                        LogBoxAdminUserId = c.String(maxLength: 15, unicode: false),
                        DocumentShareAsDefault = c.Boolean(nullable: false),
                        StockTypeCode = c.String(maxLength: 15, unicode: false),
                        AutoArchiveOnInvoice = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contacts", t => t.LogBoxAdminUserId)
                .ForeignKey("dbo.Tenants", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.LogBoxAdminUserId);
            
            CreateTable(
                "dbo.SupportMailboxes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Mailbox = c.String(nullable: false, maxLength: 100, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.ApprovedProfessions",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        LocalName = c.String(),
                        EnglishName = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.CarrierAreas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        CarrierId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreateDate = c.DateTime(),
                        UpdateDate = c.DateTime(),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        TransportModeCode = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.CarrierId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.TransportModes", t => t.TransportModeCode)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CarrierId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.TransportModeCode);
            
            CreateTable(
                "dbo.CarrierAreasPorts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CarrierAreaId = c.String(nullable: false, maxLength: 15, unicode: false),
                        PortId = c.String(maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        AddedDate = c.DateTime(),
                        AddedByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedByUserId)
                .ForeignKey("dbo.CarrierAreas", t => t.CarrierAreaId)
                .ForeignKey("dbo.Ports", t => t.PortId)
                .Index(t => t.CarrierAreaId)
                .Index(t => t.PortId)
                .Index(t => t.AddedByUserId);
            
            CreateTable(
                "Customs.ClaimsRelatedEntitiesRefunds",
                c => new
                    {
                        ClaimId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CounterKey = c.Int(nullable: false),
                        RefundQuntityLineNo = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        InvoiceNumber = c.Int(),
                        SequenceNumeric = c.Int(),
                        RefundQuntity = c.Decimal(precision: 16, scale: 6),
                    })
                .PrimaryKey(t => new { t.ClaimId, t.CounterKey, t.RefundQuntityLineNo })
                .ForeignKey("Customs.ClaimsRelatedEntities", t => new { t.ClaimId, t.CounterKey })
                .Index(t => new { t.ClaimId, t.CounterKey });
            
            CreateTable(
                "Customs.ClaimsRelatedEntitiesSeizures",
                c => new
                    {
                        ClaimId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CounterKey = c.Int(nullable: false),
                        SeizureLinoNo = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        SeizureFactorCode = c.String(maxLength: 2, unicode: false),
                        SeizureMethodCode = c.String(maxLength: 2, unicode: false),
                        SeizureAmount = c.Decimal(precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.ClaimId, t.CounterKey, t.SeizureLinoNo })
                .ForeignKey("Customs.ClaimsRelatedEntities", t => new { t.ClaimId, t.CounterKey })
                .ForeignKey("Customs.SeizureFactorTypes", t => t.SeizureFactorCode)
                .ForeignKey("Customs.SeizureMethodTypes", t => t.SeizureMethodCode)
                .Index(t => new { t.ClaimId, t.CounterKey })
                .Index(t => t.SeizureFactorCode)
                .Index(t => t.SeizureMethodCode);
            
            CreateTable(
                "Customs.SeizureFactorTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.SeizureMethodTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 50),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 50, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.ClientDrivingLicenses",
                c => new
                    {
                        ClientId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Line = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        DrivingLicenseNumber = c.String(maxLength: 20, unicode: false),
                        DriverLicenseValidityDate = c.DateTime(),
                        DrivingLicenseCountryID = c.String(maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => new { t.ClientId, t.Line })
                .ForeignKey("Customs.Clients", t => t.ClientId)
                .ForeignKey("Customs.CustomsCountries", t => t.DrivingLicenseCountryID)
                .Index(t => t.ClientId)
                .Index(t => t.DrivingLicenseCountryID);
            
            CreateTable(
                "Customs.ClientDrivingLicenseTypes",
                c => new
                    {
                        ClientId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ClientDrivingLicenseLine = c.Int(nullable: false),
                        DriversLicenseTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        Tenant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClientId, t.ClientDrivingLicenseLine, t.DriversLicenseTypeCode })
                .ForeignKey("Customs.ClientDrivingLicenses", t => new { t.ClientId, t.ClientDrivingLicenseLine })
                .Index(t => new { t.ClientId, t.ClientDrivingLicenseLine });
            
            CreateTable(
                "Customs.ConsignmentPackDangers",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ConsignmentNumber = c.Int(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        DangerousLineNo = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        UNCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        DangerousGoodsPackingReqCode = c.String(maxLength: 3, unicode: false),
                        FlashpointTemperature = c.String(maxLength: 8, unicode: false),
                        StorageTemperature = c.String(maxLength: 20, unicode: false),
                        ClassificationFourDigit = c.String(maxLength: 4),
                    })
                .PrimaryKey(t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber, t.DangerousLineNo })
                .ForeignKey("Customs.ConsignmentPackages", t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber })
                .ForeignKey("Customs.DangerousGoodsPackingReqs", t => t.DangerousGoodsPackingReqCode)
                .ForeignKey("Customs.HazardousSubstances", t => t.UNCode)
                .Index(t => new { t.DeclarationId, t.ConsignmentNumber, t.LineNumber })
                .Index(t => t.UNCode)
                .Index(t => t.DangerousGoodsPackingReqCode);
            
            CreateTable(
                "Customs.HazardousSubstances",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        EnglishName = c.String(maxLength: 300, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 300),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.CourierPendingReasons",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        ErrorPlace = c.String(maxLength: 1, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UnifreightStatusCode = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("Customs.PendingErrorPlaces", t => t.ErrorPlace)
                .Index(t => t.ErrorPlace);
            
            CreateTable(
                "Customs.CurrencyTypeTenants",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Code = c.String(maxLength: 3, unicode: false),
                        TenantInactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Customs.CurrencyTypes", t => t.Code)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.Code);
            
            CreateTable(
                "dbo.CustomerOpenFilesAmounts",
                c => new
                    {
                        CustomerId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TotalOpenFilesAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tenant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "Customs.CustomsDocumentsDefinitions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        DocumentTypeCode = c.String(maxLength: 7, unicode: false),
                        TransportationTypeCode = c.String(maxLength: 1, unicode: false),
                        ProcessTypeCode = c.String(maxLength: 7, unicode: false),
                        CargoTypeCode = c.String(maxLength: 4, unicode: false),
                        Mandatory = c.Boolean(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Customs.CargoIdentifireTypes", t => t.CargoTypeCode)
                .ForeignKey("Customs.CustomDocumentTypes", t => t.DocumentTypeCode)
                .ForeignKey("Customs.CustomsTransportModes", t => t.TransportationTypeCode)
                .ForeignKey("Customs.GovernmentProcedureTypes", t => t.ProcessTypeCode)
                .Index(t => t.DocumentTypeCode)
                .Index(t => t.TransportationTypeCode)
                .Index(t => t.ProcessTypeCode)
                .Index(t => t.CargoTypeCode);
            
            CreateTable(
                "Customs.DecDangersContacts",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CompanyName = c.String(nullable: false, maxLength: 70),
                        CompanyCommNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        CompanyCommTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        ContactName = c.String(nullable: false, maxLength: 70),
                        ContactCommNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        ContactCommTypeCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        ContactId = c.String(maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => t.DeclarationId)
                .ForeignKey("Customs.CommunicationTypes", t => t.CompanyCommTypeCode)
                .ForeignKey("Customs.CommunicationTypes", t => t.ContactCommTypeCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .Index(t => t.DeclarationId)
                .Index(t => t.CompanyCommTypeCode)
                .Index(t => t.ContactCommTypeCode);
            
            CreateTable(
                "Customs.DeclarationCourierStatuses",
                c => new
                    {
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CourierManifestStatusCode = c.String(maxLength: 1, unicode: false),
                        CourierDeclarationStatusCode = c.String(maxLength: 1, unicode: false),
                        CourierPaymentStatusCode = c.String(maxLength: 1, unicode: false),
                        IsCourierMissingClassification = c.Boolean(nullable: false),
                        IsClosedForFollowUp = c.Boolean(nullable: false),
                        HighLowValue = c.String(maxLength: 1, unicode: false),
                        DocumentStatusCode = c.String(maxLength: 1, unicode: false),
                        TotalInvoiceAmountInUSD = c.Decimal(precision: 16, scale: 2),
                        CourierPendingReasonCode = c.String(maxLength: 4, unicode: false),
                        PendingRemarks = c.String(maxLength: 1024),
                        SpecialActionStatus = c.String(maxLength: 1, unicode: false),
                        FastIndividualProcessCode = c.String(maxLength: 3, unicode: false),
                        ManualProcessCode = c.String(maxLength: 3, unicode: false),
                        TerminalSuspentionNumber = c.String(maxLength: 6, unicode: false),
                        LastMileStatusCode = c.String(maxLength: 3, unicode: false),
                        LastMileStatusDate = c.DateTime(),
                        LastMileStatusRemarks = c.String(maxLength: 2000),
                        StorageSiteStatusCode = c.String(maxLength: 3, unicode: false),
                        StorageSiteErrorText = c.String(maxLength: 1200),
                        CourierPendingReasonList = c.String(maxLength: 1000),
                        LastMileStatusName = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.DeclarationId)
                .ForeignKey("Customs.CourierPendingReasons", t => t.CourierPendingReasonCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .ForeignKey("Customs.MamanStatuses", t => t.StorageSiteStatusCode)
                .Index(t => t.DeclarationId)
                .Index(t => t.CourierPendingReasonCode)
                .Index(t => t.StorageSiteStatusCode);
            
            CreateTable(
                "Customs.DeclarationPendings",
                c => new
                    {
                        DeclarationID = c.String(nullable: false, maxLength: 15, unicode: false),
                        CourierPendingReasonCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PendingRemarks = c.String(maxLength: 1024),
                        Status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => new { t.DeclarationID, t.CourierPendingReasonCode })
                .ForeignKey("Customs.CourierPendingReasons", t => t.CourierPendingReasonCode)
                .ForeignKey("Customs.DeclarationCourierStatuses", t => t.DeclarationID)
                .Index(t => t.DeclarationID)
                .Index(t => t.CourierPendingReasonCode);
            
            CreateTable(
                "Customs.DeficitDecisions",
                c => new
                    {
                        DeficitId = c.String(nullable: false, maxLength: 15, unicode: false),
                        DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        RequestDate = c.DateTime(),
                        RequestID = c.String(maxLength: 9, unicode: false),
                        RequestTypeCode = c.String(maxLength: 2, unicode: false),
                        ApprovedProfessionCode = c.String(maxLength: 128),
                        DecisionCode = c.String(maxLength: 2, unicode: false),
                        DecisionNoteForLetter = c.String(maxLength: 512),
                        TotalComponentAmount = c.Decimal(precision: 16, scale: 2),
                        TotalEstimatedAmount = c.Decimal(precision: 16, scale: 2),
                        TotalFinancialPenaltyAmount = c.Decimal(precision: 16, scale: 2),
                        TotalInterestAmount = c.Decimal(precision: 16, scale: 2),
                        TotalLinkingAmount = c.Decimal(precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.DeficitId, t.DeclarationId })
                .ForeignKey("dbo.ApprovedProfessions", t => t.ApprovedProfessionCode)
                .ForeignKey("Customs.DecisionTypes", t => t.DecisionCode)
                .ForeignKey("Customs.Declarations", t => t.DeclarationId)
                .ForeignKey("Customs.Deficits", t => t.DeficitId)
                .ForeignKey("Customs.RequestTypes", t => t.RequestTypeCode)
                .Index(t => t.DeficitId)
                .Index(t => t.DeclarationId)
                .Index(t => t.RequestTypeCode)
                .Index(t => t.ApprovedProfessionCode)
                .Index(t => t.DecisionCode);
            
            CreateTable(
                "Customs.RequestTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 100),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.DocumentsExecutionLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        StatusCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        ExceptionMessage = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        DoneDate = c.DateTime(),
                        RequestXML = c.String(),
                        DocumentTypeId = c.String(maxLength: 15, unicode: false),
                        DocumentTypeTemplateId = c.String(maxLength: 15, unicode: false),
                        RetryNumber = c.Int(nullable: false),
                        StartDate = c.DateTime(),
                        Logs = c.String(),
                        Subject = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommunicationStatusTypes", t => t.StatusCode)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.StatusCode);
            
            CreateTable(
                "dbo.DWHBuildStatus",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 40, unicode: false),
                        LastIncrementalDWUpdateDate = c.DateTime(),
                        DWNextRunTime = c.DateTime(),
                        IsFullBuildDWRunning = c.Boolean(nullable: false),
                        IsIncrementalDWRunning = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExternalPageAdditionalDatas",
                c => new
                    {
                        ObjectTableId = c.String(nullable: false, maxLength: 15, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        LastPageNumber = c.String(maxLength: 15, unicode: false),
                        LastPageEndDate = c.DateTime(),
                        LastPageCloseBalance = c.Decimal(precision: 16, scale: 2),
                    })
                .PrimaryKey(t => new { t.ObjectTableId, t.EntityId });
            
            CreateTable(
                "Customs.GatepassRequests",
                c => new
                    {
                        MasterCourierId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        GatepassNumber = c.Int(nullable: false),
                        OriginSiteCode = c.String(maxLength: 17, unicode: false),
                        UpdateCode = c.String(maxLength: 2, unicode: false),
                        DesignateSiteCode = c.String(maxLength: 17, unicode: false),
                        TransportationTypeCode = c.String(maxLength: 4, unicode: false),
                        GatepassRequestStatus = c.String(maxLength: 2, unicode: false),
                        CustomsUpdateDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.MasterCourierId)
                .ForeignKey("Customs.SiteLookups", t => t.DesignateSiteCode)
                .ForeignKey("Customs.SiteLookups", t => t.OriginSiteCode)
                .ForeignKey("Customs.TransferCargoMethodTypes", t => t.TransportationTypeCode)
                .ForeignKey("Customs.UpdateCodes", t => t.UpdateCode)
                .Index(t => t.OriginSiteCode)
                .Index(t => t.UpdateCode)
                .Index(t => t.DesignateSiteCode)
                .Index(t => t.TransportationTypeCode);
            
            CreateTable(
                "Customs.TransferCargoMethodTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.UpdateCodes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "Customs.GatepassReturnCodes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        LocalName = c.String(maxLength: 40),
                        SearchFields = c.String(maxLength: 1000),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.GLAccountInterestPeriods",
                c => new
                    {
                        LineNumber = c.Int(nullable: false),
                        GLAccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        PeriodStartDate = c.DateTime(nullable: false),
                        StandardInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                        StandardAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                        ExceptionalInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ExceptionalAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                        CreditInterestRateBaseId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CreditAddInterestPercent = c.Decimal(nullable: false, precision: 4, scale: 2),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.LineNumber, t.GLAccountId })
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.CreditInterestRateBaseId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.ExceptionalInterestRateBaseId)
                .ForeignKey("dbo.GLAccounts", t => t.GLAccountId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.StandardInterestRateBaseId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.GLAccountId)
                .Index(t => t.StandardInterestRateBaseId)
                .Index(t => t.ExceptionalInterestRateBaseId)
                .Index(t => t.CreditInterestRateBaseId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId);
            
            CreateTable(
                "dbo.InterestBasesTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        LocalName = c.String(maxLength: 256),
                        EnglishName = c.String(maxLength: 256, unicode: false),
                        Description = c.String(maxLength: 1024),
                        InActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.InterestBasesPeriods",
                c => new
                    {
                        InterestBaseTypeId = c.String(nullable: false, maxLength: 15, unicode: false),
                        LineNumber = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        InterestBaseStartDate = c.DateTime(nullable: false),
                        InterestRate = c.Decimal(nullable: false, precision: 4, scale: 2),
                    })
                .PrimaryKey(t => new { t.InterestBaseTypeId, t.LineNumber })
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.InterestBasesTypes", t => t.InterestBaseTypeId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.InterestBaseTypeId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.InterestEntityTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 1, unicode: false),
                        EnglishName = c.String(maxLength: 100, unicode: false),
                        SearchFields = c.String(),
                        LocalName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.InterestReportLines",
                c => new
                    {
                        InterestReportId = c.String(nullable: false, maxLength: 15, unicode: false),
                        InterestTransactionId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InterestReportId, t.InterestTransactionId })
                .ForeignKey("dbo.InterestReports", t => t.InterestReportId)
                .ForeignKey("dbo.InterestTransactions", t => t.InterestTransactionId)
                .Index(t => t.InterestReportId)
                .Index(t => t.InterestTransactionId);
            
            CreateTable(
                "dbo.InterestReports",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(),
                        CreatedByUserId = c.String(maxLength: 15, unicode: false),
                        UpdateDateTime = c.DateTime(),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        GLAccountId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ReportNumber = c.String(maxLength: 15, unicode: false),
                        InterestCalculationDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(precision: 18, scale: 2),
                        OpenBalance = c.Decimal(precision: 18, scale: 2),
                        CloseBalance = c.Decimal(precision: 18, scale: 2),
                        ARinvoiceId = c.String(maxLength: 15, unicode: false),
                        InvoiceAmount = c.Decimal(precision: 18, scale: 2),
                        GLAccountInterestCreditLimit = c.Decimal(precision: 18, scale: 2),
                        InterestReportStatusCode = c.String(maxLength: 4, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                        CustomerId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ARInvoices", t => t.ARinvoiceId)
                .ForeignKey("dbo.Cards", t => t.CustomerId)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.GLAccounts", t => t.GLAccountId)
                .ForeignKey("dbo.InterestReportStatuses", t => t.InterestReportStatusCode)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.GLAccountId)
                .Index(t => t.ARinvoiceId)
                .Index(t => t.InterestReportStatusCode)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.InterestReportStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        LocalName = c.String(maxLength: 30),
                        SearchFields = c.String(),
                        EnglishName = c.String(nullable: false, maxLength: 60, unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.InterestTransactions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                        SearchFields = c.String(),
                        GLAccountId = c.String(maxLength: 15, unicode: false),
                        InterestEntityTypeCode = c.String(nullable: false, maxLength: 1, unicode: false),
                        EntityId = c.String(nullable: false, maxLength: 15, unicode: false),
                        OriginalEntityLineNumber = c.Int(nullable: false),
                        LocalAmount = c.Decimal(nullable: false, precision: 16, scale: 2),
                        ForeignAmount = c.Decimal(precision: 16, scale: 2),
                        CurrencyId = c.String(maxLength: 15, unicode: false),
                        InterestValueDate = c.DateTime(nullable: false),
                        InterestReportId = c.String(maxLength: 15, unicode: false),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.GLAccounts", t => t.GLAccountId)
                .ForeignKey("dbo.InterestEntityTypes", t => t.InterestEntityTypeCode)
                .Index(t => t.GLAccountId)
                .Index(t => t.InterestEntityTypeCode)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.InterestReportLinesByDates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        InterestReportId = c.String(nullable: false, maxLength: 15, unicode: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        TotalInterestDays = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccumulatedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StandardInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        ExceptionalInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        CreditInterestPercentage = c.Decimal(nullable: false, precision: 4, scale: 2),
                        StandardInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExceptionalInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditInterestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CalculatedStandInterestAmount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        CalculatedExcepInterestAmount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        CalculatedCreditInterestAmount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        CalculationDetails = c.String(maxLength: 256),
                        LineNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterestReports", t => t.InterestReportId)
                .Index(t => t.InterestReportId);
            
            CreateTable(
                "dbo.JournalExternalReconciles",
                c => new
                    {
                        JournalId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Line = c.Int(nullable: false),
                        Tenant = c.Int(nullable: false),
                        LedgerTransactionId = c.String(maxLength: 15, unicode: false),
                        ReconcileExternalPageLineId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => new { t.JournalId, t.Line })
                .ForeignKey("dbo.Journals", t => t.JournalId)
                .ForeignKey("dbo.LedgerTransactions", t => t.LedgerTransactionId)
                .ForeignKey("dbo.ReconcileExternalPageLines", t => t.ReconcileExternalPageLineId)
                .Index(t => t.JournalId)
                .Index(t => t.LedgerTransactionId)
                .Index(t => t.ReconcileExternalPageLineId);
            
            CreateTable(
                "dbo.OccasionInvitees",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                        AddedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Notes = c.String(maxLength: 4000),
                        OccasionId = c.String(maxLength: 15, unicode: false),
                        ContactId = c.String(maxLength: 15, unicode: false),
                        Invited = c.Boolean(nullable: false),
                        Participated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedByUserId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .ForeignKey("dbo.Occasions", t => t.OccasionId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.AddedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.OccasionId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "Customs.PendingByKeywords",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CourierPendingReasonCode = c.String(maxLength: 4, unicode: false),
                        KeywordsList = c.String(maxLength: 2000),
                        SearchFields = c.String(maxLength: 2000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Customs.CourierPendingReasons", t => t.CourierPendingReasonCode)
                .Index(t => t.CourierPendingReasonCode);
            
            CreateTable(
                "dbo.PriceSteps",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Name = c.String(nullable: false, maxLength: 80),
                        Inactive = c.Boolean(nullable: false),
                        Steps = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "Customs.RefundCustomerActivityTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 2, unicode: false),
                        EnglishName = c.String(maxLength: 40, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                        LocalName = c.String(maxLength: 40),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.RuleUpdateHistories",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        RuleCode = c.String(maxLength: 40, unicode: false),
                        EventName = c.String(nullable: false, maxLength: 100, unicode: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.TariffLinesContainersPrices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        TariffId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TariffLineId = c.String(nullable: false, maxLength: 15, unicode: false),
                        SurchargeId = c.String(nullable: false, maxLength: 15, unicode: false),
                        Price1 = c.Decimal(precision: 18, scale: 3),
                        Price2 = c.Decimal(precision: 18, scale: 3),
                        Price3 = c.Decimal(precision: 18, scale: 3),
                        Price4 = c.Decimal(precision: 18, scale: 3),
                        Price5 = c.Decimal(precision: 18, scale: 3),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChargesTypes", t => t.SurchargeId)
                .ForeignKey("dbo.Tariffs", t => t.TariffId)
                .ForeignKey("dbo.TariffLines", t => t.TariffLineId)
                .Index(t => t.TariffId)
                .Index(t => t.TariffLineId)
                .Index(t => t.SurchargeId);
            
            CreateTable(
                "dbo.TariffSurchargesUpdates",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        TariffId = c.String(maxLength: 15, unicode: false),
                        StartDate = c.DateTime(),
                        LinesUpdated = c.Int(),
                        From = c.String(),
                        To = c.String(),
                        Version = c.Int(nullable: false),
                        Surcharges = c.String(),
                        UpdateMethodCode = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.TariffSurchargesUpdateMethods", t => t.UpdateMethodCode)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdateMethodCode);
            
            CreateTable(
                "dbo.PaymentGatewayPartners",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.UserLastSettings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ControlNameSpace = c.String(nullable: false, maxLength: 100, unicode: false),
                        FilterName = c.String(nullable: false, maxLength: 50, unicode: false),
                        FilterValue = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CardContactAdditionalServices",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CardContactId = c.String(nullable: false, maxLength: 15, unicode: false),
                        AdditionalServiceId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdditionalServices", t => t.AdditionalServiceId)
                .ForeignKey("dbo.CardContacts", t => t.CardContactId)
                .Index(t => t.CardContactId)
                .Index(t => t.AdditionalServiceId);
            
            CreateTable(
                "dbo.CustomsTransferHeaders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        TransferNumber = c.String(nullable: false, maxLength: 20, unicode: false),
                        TransferDate = c.DateTime(),
                        FileName = c.String(nullable: false, maxLength: 40, unicode: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        CustomsTransferTypeCode = c.String(nullable: false, maxLength: 4, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                        Notes = c.String(maxLength: 250),
                        ShipmentNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.CustomsTransferTypes", t => t.CustomsTransferTypeCode)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.CustomsTransferTypeCode);
            
            CreateTable(
                "dbo.CustomsTransferTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        Name = c.String(nullable: false, maxLength: 40, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.CustomsTransferLines",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CustomsTransferHeaderId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ShipmentId = c.String(nullable: false, maxLength: 15, unicode: false),
                        ShipmentNumber = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomsTransferHeaders", t => t.CustomsTransferHeaderId)
                .Index(t => t.CustomsTransferHeaderId);
            
            AddColumn("dbo.AccountingIntegrityChecks", "ShouldFix", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cards", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.Cards", "StorageFreeDays", c => c.Int());
            AddColumn("dbo.Cards", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Users", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Branches", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Addresses", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Countries", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.States", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Contacts", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_ContactId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_CardId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_PartnerTypeId", c => c.String(maxLength: 2, unicode: false));
            AddColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_Via", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.Departments", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginTop", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginBottom", c => c.Int(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPerContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesContainers", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Customers", "LastOpportunitySubject", c => c.String(maxLength: 250));
            AddColumn("dbo.Customers", "LastOpportunityStatus", c => c.String(maxLength: 60, unicode: false));
            AddColumn("dbo.Ranks", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Currencies", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Tenants", "HideFCLAllIn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "AllowCustomersInAgentsLOV", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.AccountingSettings", "RefreshToken", c => c.String(maxLength: 2000));
            AddColumn("dbo.AccountingSettings", "QBOOAuth", c => c.Int(nullable: false));
            AddColumn("dbo.AccountingSettings", "AllowManualARPaymentNumber", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "AllowRegionalTaxManagement", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTerms", "Code", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.PartnerTypes", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.ShippingLines", "INTTRAUpdatesShipment", c => c.Boolean(nullable: false));
            AddColumn("dbo.VatTypes", "PayablesExternalId", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.VatTypes", "ReceivablesExternalId", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.VatTypes", "RecognizedPercentage", c => c.Double());
            AddColumn("dbo.VatTypes", "IsRegionalTax", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingPaymentMethods", "LocalName", c => c.String(maxLength: 100));
            AddColumn("dbo.ObjectTables", "HeaderScreenCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTables", "DescriptionTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTables", "NewButtonTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Tips", "ShortTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.Quotes", "QuoteHTMLDocumentId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Quotes", "Field11", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field12", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field13", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field14", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field15", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field16", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field17", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field18", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field19", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "Field20", c => c.String(maxLength: 250));
            AddColumn("dbo.Quotes", "CountryForStatisticsId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Quotes", "RequestDate", c => c.DateTime());
            AddColumn("dbo.Quotes", "EstimatedProfitInLocal", c => c.Double());
            AddColumn("dbo.Quotes", "EstimatedProfitInProfit", c => c.Double());
            AddColumn("dbo.Quotes", "ProfitCurrencyId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Quotes", "ProfitExchangeRate", c => c.Double());
            AddColumn("dbo.Directions", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Ports", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Incoterms", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.TransportModes", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.ShipmentTypes", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Tickets", "SupportMailboxId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tickets", "LastCorrespondence", c => c.String(maxLength: 4000));
            AddColumn("dbo.AdvancedQueryFilters", "QueryCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFields", "FieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFields", "FullNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "HelpTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "ListTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "ShortNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectFields", "DisplayInAutomationAsEnitity", c => c.Boolean(nullable: false));
            AddColumn("dbo.ObjectFields", "RecordType", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Queries", "UniqueCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Queries", "OriginalQueryCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Queries", "NameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Queries", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Features", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Features", "NameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentLevels", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.AirlineMessagingRules", "RuleFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.APInvoices", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.APInvoiceLines", "ContainerTypeId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.APInvoiceLines", "Quantity", c => c.Int());
            AddColumn("dbo.ChargesTypes", "ApplyRegionalTax", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChargesGroups", "ViewOrder", c => c.Int(nullable: false));
            AddColumn("dbo.APPayments", "VendorBankAddress", c => c.String(maxLength: 100));
            AddColumn("dbo.APPayments", "VendorBankName", c => c.String(maxLength: 40));
            AddColumn("dbo.APPayments", "VendorBankAccountNumber", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.APPayments", "VendorSwift", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.APPayments", "VendorIBANNumber", c => c.String(maxLength: 30));
            AddColumn("dbo.APPayments", "AccountingCancelationDate", c => c.DateTime());
            AddColumn("dbo.APPayments", "DontIncludeInDeductionReport", c => c.Boolean(nullable: false));
            AddColumn("dbo.APPayments", "CancelationNotes", c => c.String());
            AddColumn("dbo.APPayments", "Field1", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field2", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field3", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field4", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field5", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field6", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field7", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field8", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field9", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field10", c => c.String(maxLength: 250));
            AddColumn("dbo.ARInvoices", "DateForInterest", c => c.DateTime());
            AddColumn("dbo.ARInvoices", "DocumentFilingId", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ARInvoices", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.ARInvoices", "BillToGLAccountId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ARPayments", "OpenAmountInLocalCurrency", c => c.Double());
            AddColumn("dbo.ARPayments", "CreatedByPartner", c => c.String(maxLength: 25, unicode: false));
            AddColumn("dbo.ARPayments", "IsPaymentNumberManuallySet", c => c.Boolean(nullable: false));
            AddColumn("dbo.ARPayments", "Field1", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field2", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field3", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field4", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field5", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field6", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field7", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field8", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field9", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field10", c => c.String(maxLength: 250));
            AddColumn("dbo.Shipments", "GrossWeightPerStorageDays", c => c.Double());
            AddColumn("dbo.Shipments", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Shipments", "INTTRABookingError", c => c.String(maxLength: 256, unicode: false));
            AddColumn("dbo.Shipments", "INTTRALastBookingResponse", c => c.String());
            AddColumn("dbo.Shipments", "NotInvoicedReceivablesAmount", c => c.Double());
            AddColumn("dbo.Shipments", "CreatedByPartner", c => c.String());
            AddColumn("dbo.Shipments", "FirstARInvoiceApprovalDate", c => c.DateTime());
            AddColumn("dbo.Shipments", "WarehouseStorageFreeDays", c => c.Int());
            AddColumn("dbo.EntityStatus", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "IsUserIDNumberRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberUpdateDate", c => c.DateTime());
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberXMLData", c => c.String());
            AddColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber", c => c.String(maxLength: 35));
            AddColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "NumberOfDeliveries", c => c.Int());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupETA", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupETD", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupATA", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "LastPickupATD", c => c.DateTime());
            AddColumn("dbo.ShipmentComputedFields", "DeliveryToPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "DeliveryFrom", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "DeliveryTo", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "PickupFrom", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "PickupTo", c => c.String(maxLength: 40, unicode: false));
            AddColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserName", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ShipmentMasterDatas", "AutomaticLastUpdateDate", c => c.DateTime());
            AddColumn("dbo.ShipmentMasterDatas", "CutoffDate", c => c.DateTime());
            AddColumn("dbo.BankAccounts", "PrintingBranchNumber", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.BankAccounts", "PrintingAccountNumber", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.GLAccounts", "AllowEditChequePayToName", c => c.Boolean(nullable: false));
            AddColumn("dbo.GLAccounts", "ActiveForInterest", c => c.Boolean());
            AddColumn("dbo.GLAccounts", "InterestCalculationStartDate", c => c.DateTime());
            AddColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice", c => c.Boolean());
            AddColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int());
            AddColumn("dbo.GLAccounts", "InterestCreditLimit", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GLAccounts", "NameForPrintingCheques", c => c.String(maxLength: 1000));
            AddColumn("dbo.GLAccounts", "Smallcashbook", c => c.Boolean(nullable: false));
            AddColumn("dbo.BIReports", "LastRunDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BIReports", "LastRunByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.CreditLimitSettings", "CustomersShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShipperConsigneeShipmentBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomsAgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingAgentsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AirlinesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingLinesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "TruckersShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "VendorsShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "WarehousesShipmentsBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomersInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShipperConsigneeInvoiceBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "CustomsAgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingAgentsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "AirlinesInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "ShippingLinesInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "TruckersInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "VendorsInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditLimitSettings", "WarehousesInvoicesBlock", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomerFieldsUpdateSettings", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.CustomsInterfaceSettings", "AMCAirStartDate", c => c.DateTime());
            AddColumn("dbo.CustomsInterfaceSettings", "AMCOceanStartDate", c => c.DateTime());
            AddColumn("dbo.DocumentTypeTemplates", "BCC", c => c.String(maxLength: 4000));
            AddColumn("dbo.LedgerTransactions", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
            AddColumn("dbo.JournalLines", "ActionId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReconcileExternalPageLines", "InReconcileProgress", c => c.Boolean(nullable: false));
            AddColumn("dbo.ReconcileExternalPages", "ObjectTableId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ReconcileExternalPages", "EntityId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.FullAccountingSettings", "NumberOfAgingMonths", c => c.Int());
            AddColumn("dbo.MenuButtons", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.MenuButtons", "LabelTextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.MenusTables", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.ObjectFieldModifications", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectFieldValidations", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableHelperControls", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableRules", "TriggerFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ObjectTableTabs", "TabNameTextCodeCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.ObjectTableTabs", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Occasions", "ParticipatedCustomers", c => c.Int(nullable: false));
            AddColumn("dbo.Occasions", "ParticipatedContacts", c => c.Int(nullable: false));
            AddColumn("dbo.Occasions", "InvitedCustomers", c => c.Int(nullable: false));
            AddColumn("dbo.Occasions", "InvitedContacts", c => c.Int(nullable: false));
            AddColumn("dbo.PackageFeatures", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.QueryColumns", "QueryCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.QueryColumns", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.QuoteSettings", "CopyExchangeRates", c => c.Boolean(nullable: false));
            AddColumn("dbo.QuoteSettings", "AutomaticallyCloseDays", c => c.Int(nullable: false));
            AddColumn("dbo.ReportExecutionLogs", "RetryNumber", c => c.Int(nullable: false));
            AddColumn("dbo.ReportExecutionLogs", "StartDate", c => c.DateTime());
            AddColumn("dbo.ReportExecutionLogs", "ExecutedByServerName", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.Reports", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.Reports", "AvailableForScheduling", c => c.Boolean(nullable: false));
            AddColumn("dbo.Restrictions", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.RoleFeatures", "FeatureUniqeCode", c => c.String(maxLength: 120, unicode: false));
            AddColumn("dbo.RuleConditionFields", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.TaskSchedulerHistory", "LogDocumentId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TasksScheduler", "AverageRunTime", c => c.Double(nullable: false));
            AddColumn("dbo.TasksScheduler", "EntityId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.SchedulerProcedure", "IsInternallyDefined", c => c.Boolean());
            AddColumn("dbo.ScreenFields", "ScreenCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ScreenFields", "ObjectFieldCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ScreenModifications", "ScreenCode", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.SharedLogisticsSettings", "IsShowAmountLocalCurrency", c => c.Boolean(nullable: false));
            AddColumn("dbo.SharedUserQueries", "QueryCode", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.ShipmentPayables", "TariffId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.ShipmentPayables", "TariffNumber", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.ShipmentPayables", "TariffVersion", c => c.Int(nullable: false));
            AddColumn("dbo.SLAHeaders", "SearchFields", c => c.String(maxLength: 1000));
            AddColumn("dbo.TariffLines", "Surcharge1MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge2MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge3MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge4MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge5MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge6MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge7MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge8MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge9MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "Surcharge10MinPrice", c => c.Decimal(precision: 18, scale: 3));
            AddColumn("dbo.TariffLines", "CurrencyId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "Notes", c => c.String(maxLength: 250));
            AddColumn("dbo.Tariffs", "ContainerType1Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "ContainerType2Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "ContainerType3Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "ContainerType4Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tariffs", "ContainerType5Id", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TariffSettings", "AirDefaultStepsId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TariffSettings", "LCLDefaultStepsId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.TariffTypes", "TransportModeCode", c => c.String(maxLength: 1, fixedLength: true, unicode: false));
            AddColumn("dbo.Translations", "TextCodeCode", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseEntries", "LastStatusUpdateDate", c => c.DateTime());
            AddColumn("dbo.WarehouseEntries", "ConnectedTo", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseEntries", "Ratio", c => c.Double());
            AddColumn("dbo.WarehouseEntries", "ToTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.WarehouseEntries", "FromTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.WarehouseEntries", "FromCountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseEntries", "ToCountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ConnectedTo", c => c.String(maxLength: 100, unicode: false));
            AddColumn("dbo.WarehouseReleases", "FromPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToPortId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "CustomerAddressId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "TotalVolumetricWeight", c => c.Decimal(nullable: false, precision: 18, scale: 3));
            AddColumn("dbo.WarehouseReleases", "Ratio", c => c.Double());
            AddColumn("dbo.WarehouseReleases", "ToTypeCode", c => c.String(maxLength: 4, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToPartnerCardId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToAddressId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToAddressZipCode", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "ToAddressCity", c => c.String(maxLength: 25));
            AddColumn("dbo.WarehouseReleases", "ToAddressCountryId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.WarehouseReleases", "IsUsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Cards", "BankName", c => c.String(maxLength: 40));
            AlterColumn("dbo.Cards", "BankAddress", c => c.String(maxLength: 100));
            AlterColumn("dbo.Contacts", "ExternalId", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.Industries", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(maxLength: 4, unicode: false));
            AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 60, unicode: false));
            AlterColumn("dbo.ObjectTables", "SplitComponentPath", c => c.String());
            AlterColumn("dbo.Screens", "Name", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Quotes", "ExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.Quotes", "QuoteClosingReasonCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("dbo.QuoteClosingReasons", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("dbo.QuoteClosingReasons", "Name", c => c.String(nullable: false, maxLength: 60, unicode: false));
            AlterColumn("dbo.QuoteClosingReasons", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Tickets", "Subject", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Features", "Code", c => c.String(maxLength: 120, unicode: false));
            AlterColumn("dbo.APInvoices", "InvoiceDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.APInvoices", "DueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.APInvoices", "InvoiceCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "SubTotalInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "SubTotalInInvoiceCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "AmountInInvoiceCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "AmountInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.APInvoices", "ProfitCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "AmountInProfitCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoices", "UpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.APInvoiceLines", "InvoiceCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoiceLines", "LocalCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoiceLines", "ProfitCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoiceLines", "ForiegnCurrencyId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.APInvoicePayments", "LocalAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoicePayments", "ForeignAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.APPayments", "AmountInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "AccountingPaymentMethodId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.APPayments", "AmountInPaymentCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "PaymentCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "RegisterDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.APPayments", "OpenAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APPayments", "ValueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.APPayments", "UpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.APPayments", "ApprovedByUserId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.APInvoiceTotalVATs", "ProfitCurrencyVATAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.APInvoiceTotalVATs", "ProfitVatableAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "InvoiceDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ARInvoices", "DueDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ARInvoices", "SubTotalInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "SubTotalInInvoiceCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "AmountInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "AmountInInvoiceCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "InvoiceCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ARInvoices", "AmountDue", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "AmountInProfitCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "ProfitCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoices", "UpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "ForiegnCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "LocalCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "InvoiceCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "ForiegnExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceLines", "ProfitCurrencyAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoicePayments", "LocalAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoicePayments", "ForeignAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ARPayments", "AmountInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "AmountInPaymentCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "PaymentCurrencyExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "RegisterDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ARPayments", "OpenAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARPayments", "UpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "InvoiceCurrencyVATAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "LocalVATAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "InvoiceCurrencyVatableAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "LocalVatableAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "VatPercent", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "ProfitCurrencyVATAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "ProfitVatableAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.AutomationResultEmailRecipients", "RecipientValue", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.Shipments", "OpenReceivablesInLocalCurrency", c => c.Double(nullable: false));
            AlterColumn("dbo.Shipments", "SecurityKey", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ApprovedByUserName", c => c.String(maxLength: 200));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "VersionApproved", c => c.String(maxLength: 10));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "DenyReason", c => c.String(maxLength: 1024));
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ShipmentAddtionalDataXML", c => c.String(maxLength: 4000));
            AlterColumn("dbo.ShipmentComputedFields", "ImporterDepositionRequestDetails", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.ShipmentComputedFields", "Commodity", c => c.String(maxLength: 15));
            AlterColumn("dbo.ShipmentComputedFields", "FirstPickupLocation", c => c.String(maxLength: 100));
            AlterColumn("dbo.ShipmentComputedFields", "ContainersNumbers", c => c.String(maxLength: 1000));
            AlterColumn("dbo.GLAccounts", "DisplayNumber", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("Customs.ContinuousRequestTypes", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("Customs.ContinuousRequestTypes", "LocalName", c => c.String(maxLength: 40));
            AlterColumn("Customs.ContinuousRequestTypes", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("Customs.ContinuousRequestTypes", "EnglishName", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("Customs.DecisionTypes", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("Customs.DecisionTypes", "LocalName", c => c.String(maxLength: 50));
            AlterColumn("Customs.DecisionTypes", "EnglishName", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("Customs.DecisionTypes", "SearchFields", c => c.String(maxLength: 1000));
            AlterColumn("Customs.Declarations", "AmendmentStatus", c => c.String(maxLength: 3, unicode: false));
            AlterColumn("Customs.AmendmentStatuses", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("Customs.AmendmentStatuses", "Name", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Competitors", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.JournalLines", "ActionCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Followers", "CancelledDate", c => c.DateTime());
            AlterColumn("dbo.InboundEmailLines", "Subject", c => c.String(maxLength: 256));
            AlterColumn("dbo.InsideShipmentPackages", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentPackages", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentPackages", "Temperature", c => c.String(maxLength: 8, unicode: false));
            AlterColumn("dbo.PaymentCheques", "PayToName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.QueueMessageMoreDetails", "QueueDefinitionCode", c => c.String(nullable: false, maxLength: 265, unicode: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "MessageBody", c => c.String(nullable: false, maxLength: 1000, unicode: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "Field1", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "Field2", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.QueueMessageMoreDetails", "Field3", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.QuoteCharges", "CostExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.QuotePriceSteps", "Step", c => c.Double(nullable: false));
            AlterColumn("dbo.ShipmentOrderPackages", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentPackageItems", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentPayables", "UpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ShipmentPayables", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ShipmentPickUpDeliveryPackages", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ShipmentReceivables", "UpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ShipmentReceivables", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TarrifCharges", "MaxPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifCharges", "MinPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifCharges", "UnitPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifSteps", "Step", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifSteps", "MaxPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifSteps", "MinPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TarrifSteps", "UnitPrice", c => c.Decimal(precision: 14, scale: 3));
            AlterColumn("dbo.TariffSurchargesUpdateMethods", "Code", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("dbo.TariffSurchargesUpdateMethods", "Name", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.VatTypePercentages", "Percentage", c => c.Double(nullable: false));
            AddPrimaryKey("dbo.QuoteClosingReasons", "Code");
            AddPrimaryKey("Customs.ContinuousRequestTypes", "Code");
            AddPrimaryKey("Customs.DecisionTypes", "Code");
            AddPrimaryKey("Customs.AmendmentStatuses", "Code");
            AddPrimaryKey("dbo.CustomerCompetitorProducts", new[] { "CustomerId", "CompetitorId", "ProductTypeCode" });
            AddPrimaryKey("dbo.TariffSurchargesUpdateMethods", "Code");
            CreateIndex("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" });
            CreateIndex("dbo.Tenants", "ChargeableWeightUnitCode");
            CreateIndex("dbo.Tenants", "PasswordPolicyCode");
            CreateIndex("dbo.Quotes", "QuoteHTMLDocumentId");
            CreateIndex("dbo.Quotes", "CountryForStatisticsId");
            CreateIndex("dbo.Quotes", "QuoteClosingReasonCode");
            CreateIndex("dbo.Quotes", "ProfitCurrencyId");
            CreateIndex("dbo.Tickets", "SupportMailboxId");
            CreateIndex("dbo.ObjectFields", "FieldCode", unique: true);
            CreateIndex("dbo.Features", "FeatureUniqeCode", unique: true);
            CreateIndex("dbo.APInvoiceLines", "ForiegnCurrencyId");
            CreateIndex("dbo.APInvoiceLines", "ContainerTypeId");
            CreateIndex("dbo.APPayments", "AccountingPaymentMethodId");
            CreateIndex("dbo.APPayments", "ApprovedByUserId");
            CreateIndex("dbo.Shipments", "AgentComputed");
            CreateIndex("dbo.ShipmentComputedFields", "OperationallyClosedByUserId");
            CreateIndex("dbo.ShipmentComputedFields", "DeliveryToPortId");
            CreateIndex("dbo.BIReports", "LastRunByUserId");
            CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            CreateIndex("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode");
            CreateIndex("Customs.Declarations", "AmendmentStatus");
            CreateIndex("dbo.JournalLines", "ActionId");
            CreateIndex("dbo.QueueMessageMoreDetails", "QueueDefinitionCode");
            CreateIndex("dbo.Tariffs", "ContainerType1Id");
            CreateIndex("dbo.Tariffs", "ContainerType2Id");
            CreateIndex("dbo.Tariffs", "ContainerType3Id");
            CreateIndex("dbo.Tariffs", "ContainerType4Id");
            CreateIndex("dbo.Tariffs", "ContainerType5Id");
            CreateIndex("dbo.TariffTypes", "TransportModeCode");
            CreateIndex("dbo.TaskSchedulerHistory", "LogDocumentId");
            CreateIndex("dbo.WarehouseEntries", "FromCountryId");
            CreateIndex("dbo.WarehouseEntries", "ToCountryId");
            CreateIndex("dbo.WarehouseReleases", "FromPortId");
            CreateIndex("dbo.WarehouseReleases", "ToPortId");
            CreateIndex("dbo.WarehouseReleases", "ToPartnerCardId");
            CreateIndex("dbo.WarehouseReleases", "ToAddressId");
            CreateIndex("dbo.WarehouseReleases", "ToAddressCountryId");
            AddForeignKey("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" }, "dbo.SharedLogisticsContactLastLogins", new[] { "ContactId", "CardId", "PartnerTypeId", "Via" });
            AddForeignKey("dbo.Tenants", "ChargeableWeightUnitCode", "dbo.WeightUnits", "Code");
            AddForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries", "Id");
            AddForeignKey("dbo.Quotes", "ProfitCurrencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.Quotes", "QuoteHTMLDocumentId", "dbo.Documents", "Id");
            AddForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes", "Id");
            AddForeignKey("dbo.APInvoiceLines", "ContainerTypeId", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Shipments", "AgentComputed", "dbo.Cards", "Id");
            AddForeignKey("dbo.ShipmentComputedFields", "DeliveryToPortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType1Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType2Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType3Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType4Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.Tariffs", "ContainerType5Id", "dbo.PackageTypes", "Id");
            AddForeignKey("dbo.TariffTypes", "TransportModeCode", "dbo.TransportModes", "Id");
            AddForeignKey("dbo.TaskSchedulerHistory", "LogDocumentId", "dbo.Documents", "Id");
            AddForeignKey("dbo.WarehouseEntries", "FromCountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseEntries", "ToCountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Warehouses", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToAddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToAddressCountryId", "dbo.Countries", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToPartnerCardId", "dbo.Cards", "Id");
            AddForeignKey("dbo.WarehouseReleases", "ToPortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.JournalLines", "ActionId", "dbo.JournalActionTypes", "Id");
            AddForeignKey("dbo.Quotes", "QuoteClosingReasonCode", "dbo.QuoteClosingReasons", "Code");
            AddForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "Customs.ContinuousRequestTypes", "Code");
            AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "Customs.DecisionTypes", "Code");
            AddForeignKey("Customs.Declarations", "AmendmentStatus", "Customs.AmendmentStatuses", "Code");
            DropColumn("dbo.Tenants", "IsDocumentsArchive");
            DropColumn("dbo.Tenants", "CustomerTenantShareImportFile");
            DropColumn("dbo.Tenants", "LogBoxAdminUserId");
            DropColumn("dbo.Tenants", "DocumentShareAsDefault");
            DropColumn("dbo.Tenants", "StockTypeCode");
            DropColumn("dbo.Tenants", "AutoArchiveOnInvoice");
            DropColumn("dbo.ARInvoices", "DateForVATInterest");
            DropColumn("dbo.Shipments", "CutoffDate");
            DropColumn("dbo.ReconcileExternalPages", "BankAccountId");
            DropColumn("dbo.Tariffs", "Description");
            DropTable("dbo.AirlineAreas");
            DropTable("dbo.AirlineAreasPorts");
            DropTable("dbo.SchedulerLogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SchedulerLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Log = c.String(unicode: false),
                        HistoryId = c.String(nullable: false, maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AirlineAreasPorts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AirlineAreaId = c.String(nullable: false, maxLength: 128),
                        PortId = c.String(maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        AddedDate = c.DateTime(),
                        AddedByUserId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tariffs", "Description", c => c.String(maxLength: 250));
            AddColumn("dbo.ReconcileExternalPages", "BankAccountId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AddColumn("dbo.Shipments", "CutoffDate", c => c.DateTime());
            AddColumn("dbo.ARInvoices", "DateForVATInterest", c => c.DateTime());
            AddColumn("dbo.Tenants", "AutoArchiveOnInvoice", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "StockTypeCode", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tenants", "DocumentShareAsDefault", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "LogBoxAdminUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Tenants", "CustomerTenantShareImportFile", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tenants", "IsDocumentsArchive", c => c.Boolean(nullable: false));
            DropForeignKey("Customs.Declarations", "AmendmentStatus", "Customs.AmendmentStatuses");
            DropForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "Customs.DecisionTypes");
            DropForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "Customs.ContinuousRequestTypes");
            DropForeignKey("dbo.Quotes", "QuoteClosingReasonCode", "dbo.QuoteClosingReasons");
            DropForeignKey("dbo.JournalLines", "ActionId", "dbo.JournalActionTypes");
            DropForeignKey("dbo.CustomsTransferLines", "CustomsTransferHeaderId", "dbo.CustomsTransferHeaders");
            DropForeignKey("dbo.CustomsTransferHeaders", "CustomsTransferTypeCode", "dbo.CustomsTransferTypes");
            DropForeignKey("dbo.CustomsTransferHeaders", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.CardContactAdditionalServices", "CardContactId", "dbo.CardContacts");
            DropForeignKey("dbo.CardContactAdditionalServices", "AdditionalServiceId", "dbo.AdditionalServices");
            DropForeignKey("dbo.WarehouseReleases", "ToPortId", "dbo.Ports");
            DropForeignKey("dbo.WarehouseReleases", "ToPartnerCardId", "dbo.Cards");
            DropForeignKey("dbo.WarehouseReleases", "ToAddressCountryId", "dbo.Countries");
            DropForeignKey("dbo.WarehouseReleases", "ToAddressId", "dbo.Addresses");
            DropForeignKey("dbo.WarehouseReleases", "FromPortId", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseEntries", "ToCountryId", "dbo.Countries");
            DropForeignKey("dbo.WarehouseEntries", "FromCountryId", "dbo.Countries");
            DropForeignKey("dbo.UserLastSettings", "UserId", "dbo.Users");
            DropForeignKey("dbo.TaskSchedulerHistory", "LogDocumentId", "dbo.Documents");
            DropForeignKey("dbo.TariffTypes", "TransportModeCode", "dbo.TransportModes");
            DropForeignKey("dbo.TariffSurchargesUpdates", "UpdateMethodCode", "dbo.TariffSurchargesUpdateMethods");
            DropForeignKey("dbo.TariffSurchargesUpdates", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.TariffLinesContainersPrices", "TariffLineId", "dbo.TariffLines");
            DropForeignKey("dbo.TariffLinesContainersPrices", "TariffId", "dbo.Tariffs");
            DropForeignKey("dbo.Tariffs", "ContainerType5Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Tariffs", "ContainerType4Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Tariffs", "ContainerType3Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Tariffs", "ContainerType2Id", "dbo.PackageTypes");
            DropForeignKey("dbo.Tariffs", "ContainerType1Id", "dbo.PackageTypes");
            DropForeignKey("dbo.TariffLinesContainersPrices", "SurchargeId", "dbo.ChargesTypes");
            DropForeignKey("dbo.RuleUpdateHistories", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.RuleUpdateHistories", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.PriceSteps", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.PriceSteps", "CreatedByUserId", "dbo.Users");
            DropForeignKey("Customs.PendingByKeywords", "CourierPendingReasonCode", "Customs.CourierPendingReasons");
            DropForeignKey("dbo.OccasionInvitees", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.OccasionInvitees", "OccasionId", "dbo.Occasions");
            DropForeignKey("dbo.OccasionInvitees", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.OccasionInvitees", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.JournalExternalReconciles", "ReconcileExternalPageLineId", "dbo.ReconcileExternalPageLines");
            DropForeignKey("dbo.JournalExternalReconciles", "LedgerTransactionId", "dbo.LedgerTransactions");
            DropForeignKey("dbo.JournalExternalReconciles", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.InterestReportLinesByDates", "InterestReportId", "dbo.InterestReports");
            DropForeignKey("dbo.InterestReportLines", "InterestTransactionId", "dbo.InterestTransactions");
            DropForeignKey("dbo.InterestTransactions", "InterestEntityTypeCode", "dbo.InterestEntityTypes");
            DropForeignKey("dbo.InterestTransactions", "GLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.InterestTransactions", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.InterestReportLines", "InterestReportId", "dbo.InterestReports");
            DropForeignKey("dbo.InterestReports", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestReports", "InterestReportStatusCode", "dbo.InterestReportStatuses");
            DropForeignKey("dbo.InterestReports", "GLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.InterestReports", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestReports", "CustomerId", "dbo.Cards");
            DropForeignKey("dbo.InterestReports", "ARinvoiceId", "dbo.ARInvoices");
            DropForeignKey("dbo.InterestBasesPeriods", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesPeriods", "InterestBaseTypeId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.InterestBasesPeriods", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccountInterestPeriods", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccountInterestPeriods", "StandardInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.GLAccountInterestPeriods", "GLAccountId", "dbo.GLAccounts");
            DropForeignKey("dbo.GLAccountInterestPeriods", "ExceptionalInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.GLAccountInterestPeriods", "CreditInterestRateBaseId", "dbo.InterestBasesTypes");
            DropForeignKey("dbo.InterestBasesTypes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesTypes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.GLAccountInterestPeriods", "CreatedByUserId", "dbo.Users");
            DropForeignKey("Customs.GatepassRequests", "UpdateCode", "Customs.UpdateCodes");
            DropForeignKey("Customs.GatepassRequests", "TransportationTypeCode", "Customs.TransferCargoMethodTypes");
            DropForeignKey("Customs.GatepassRequests", "OriginSiteCode", "Customs.SiteLookups");
            DropForeignKey("Customs.GatepassRequests", "DesignateSiteCode", "Customs.SiteLookups");
            DropForeignKey("dbo.DocumentsExecutionLogs", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.DocumentsExecutionLogs", "StatusCode", "dbo.CommunicationStatusTypes");
            DropForeignKey("Customs.DeficitDecisions", "RequestTypeCode", "Customs.RequestTypes");
            DropForeignKey("Customs.DeficitDecisions", "DeficitId", "Customs.Deficits");
            DropForeignKey("Customs.DeficitDecisions", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.DeficitDecisions", "DecisionCode", "Customs.DecisionTypes");
            DropForeignKey("Customs.DeficitDecisions", "ApprovedProfessionCode", "dbo.ApprovedProfessions");
            DropForeignKey("Customs.DeclarationPendings", "DeclarationID", "Customs.DeclarationCourierStatuses");
            DropForeignKey("Customs.DeclarationPendings", "CourierPendingReasonCode", "Customs.CourierPendingReasons");
            DropForeignKey("Customs.DeclarationCourierStatuses", "StorageSiteStatusCode", "Customs.MamanStatuses");
            DropForeignKey("Customs.DeclarationCourierStatuses", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.DeclarationCourierStatuses", "CourierPendingReasonCode", "Customs.CourierPendingReasons");
            DropForeignKey("Customs.DecDangersContacts", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.DecDangersContacts", "ContactCommTypeCode", "Customs.CommunicationTypes");
            DropForeignKey("Customs.DecDangersContacts", "CompanyCommTypeCode", "Customs.CommunicationTypes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "ProcessTypeCode", "Customs.GovernmentProcedureTypes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "TransportationTypeCode", "Customs.CustomsTransportModes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "DocumentTypeCode", "Customs.CustomDocumentTypes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "CargoTypeCode", "Customs.CargoIdentifireTypes");
            DropForeignKey("dbo.CustomerOpenFilesAmounts", "CustomerId", "dbo.Customers");
            DropForeignKey("Customs.CurrencyTypeTenants", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("Customs.CurrencyTypeTenants", "Code", "Customs.CurrencyTypes");
            DropForeignKey("Customs.CourierPendingReasons", "ErrorPlace", "Customs.PendingErrorPlaces");
            DropForeignKey("Customs.ConsignmentPackDangers", "UNCode", "Customs.HazardousSubstances");
            DropForeignKey("Customs.ConsignmentPackDangers", "DangerousGoodsPackingReqCode", "Customs.DangerousGoodsPackingReqs");
            DropForeignKey("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" }, "Customs.ConsignmentPackages");
            DropForeignKey("Customs.ClientDrivingLicenseTypes", new[] { "ClientId", "ClientDrivingLicenseLine" }, "Customs.ClientDrivingLicenses");
            DropForeignKey("Customs.ClientDrivingLicenses", "DrivingLicenseCountryID", "Customs.CustomsCountries");
            DropForeignKey("Customs.ClientDrivingLicenses", "ClientId", "Customs.Clients");
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", "SeizureMethodCode", "Customs.SeizureMethodTypes");
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", "SeizureFactorCode", "Customs.SeizureFactorTypes");
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", new[] { "ClaimId", "CounterKey" }, "Customs.ClaimsRelatedEntities");
            DropForeignKey("Customs.ClaimsRelatedEntitiesRefunds", new[] { "ClaimId", "CounterKey" }, "Customs.ClaimsRelatedEntities");
            DropForeignKey("dbo.CarrierAreasPorts", "PortId", "dbo.Ports");
            DropForeignKey("dbo.CarrierAreasPorts", "CarrierAreaId", "dbo.CarrierAreas");
            DropForeignKey("dbo.CarrierAreasPorts", "AddedByUserId", "dbo.Users");
            DropForeignKey("dbo.CarrierAreas", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.CarrierAreas", "TransportModeCode", "dbo.TransportModes");
            DropForeignKey("dbo.CarrierAreas", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.CarrierAreas", "CarrierId", "dbo.Cards");
            DropForeignKey("dbo.BIReports", "LastRunByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentComputedFields", "OperationallyClosedByUserId", "dbo.Users");
            DropForeignKey("dbo.ShipmentComputedFields", "DeliveryToPortId", "dbo.Ports");
            DropForeignKey("dbo.Shipments", "AgentComputed", "dbo.Cards");
            DropForeignKey("dbo.APInvoiceLines", "ContainerTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.Tickets", "SupportMailboxId", "dbo.SupportMailboxes");
            DropForeignKey("dbo.SupportMailboxes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.SupportMailboxes", "CreatedByUserId", "dbo.Users");
            DropForeignKey("dbo.Quotes", "QuoteHTMLDocumentId", "dbo.Documents");
            DropForeignKey("dbo.Quotes", "ProfitCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Quotes", "CountryForStatisticsId", "dbo.Countries");
            DropForeignKey("dbo.Tenants", "ChargeableWeightUnitCode", "dbo.WeightUnits");
            DropForeignKey("dbo.LogBoxTenantSettings", "Id", "dbo.Tenants");
            DropForeignKey("dbo.LogBoxTenantSettings", "LogBoxAdminUserId", "dbo.Contacts");
            DropForeignKey("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" }, "dbo.SharedLogisticsContactLastLogins");
            DropForeignKey("dbo.SharedLogisticsContactLastLogins", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.AccountingPartners", "Id", "dbo.Cards");
            DropIndex("dbo.CustomsTransferLines", new[] { "CustomsTransferHeaderId" });
            DropIndex("dbo.CustomsTransferHeaders", new[] { "CustomsTransferTypeCode" });
            DropIndex("dbo.CustomsTransferHeaders", new[] { "CreatedByUserId" });
            DropIndex("dbo.CardContactAdditionalServices", new[] { "AdditionalServiceId" });
            DropIndex("dbo.CardContactAdditionalServices", new[] { "CardContactId" });
            DropIndex("dbo.WarehouseReleases", new[] { "ToAddressCountryId" });
            DropIndex("dbo.WarehouseReleases", new[] { "ToAddressId" });
            DropIndex("dbo.WarehouseReleases", new[] { "ToPartnerCardId" });
            DropIndex("dbo.WarehouseReleases", new[] { "ToPortId" });
            DropIndex("dbo.WarehouseReleases", new[] { "FromPortId" });
            DropIndex("dbo.WarehouseEntries", new[] { "ToCountryId" });
            DropIndex("dbo.WarehouseEntries", new[] { "FromCountryId" });
            DropIndex("dbo.UserLastSettings", new[] { "UserId" });
            DropIndex("dbo.TaskSchedulerHistory", new[] { "LogDocumentId" });
            DropIndex("dbo.TariffTypes", new[] { "TransportModeCode" });
            DropIndex("dbo.TariffSurchargesUpdates", new[] { "UpdateMethodCode" });
            DropIndex("dbo.TariffSurchargesUpdates", new[] { "CreatedByUserId" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType5Id" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType4Id" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType3Id" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType2Id" });
            DropIndex("dbo.Tariffs", new[] { "ContainerType1Id" });
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "SurchargeId" });
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "TariffLineId" });
            DropIndex("dbo.TariffLinesContainersPrices", new[] { "TariffId" });
            DropIndex("dbo.RuleUpdateHistories", new[] { "UpdatedByUserId" });
            DropIndex("dbo.RuleUpdateHistories", new[] { "CreatedByUserId" });
            DropIndex("dbo.QueueMessageMoreDetails", new[] { "QueueDefinitionCode" });
            DropIndex("dbo.PriceSteps", new[] { "UpdatedByUserId" });
            DropIndex("dbo.PriceSteps", new[] { "CreatedByUserId" });
            DropIndex("Customs.PendingByKeywords", new[] { "CourierPendingReasonCode" });
            DropIndex("dbo.OccasionInvitees", new[] { "ContactId" });
            DropIndex("dbo.OccasionInvitees", new[] { "OccasionId" });
            DropIndex("dbo.OccasionInvitees", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OccasionInvitees", new[] { "AddedByUserId" });
            DropIndex("dbo.JournalExternalReconciles", new[] { "ReconcileExternalPageLineId" });
            DropIndex("dbo.JournalExternalReconciles", new[] { "LedgerTransactionId" });
            DropIndex("dbo.JournalExternalReconciles", new[] { "JournalId" });
            DropIndex("dbo.InterestReportLinesByDates", new[] { "InterestReportId" });
            DropIndex("dbo.InterestTransactions", new[] { "CurrencyId" });
            DropIndex("dbo.InterestTransactions", new[] { "InterestEntityTypeCode" });
            DropIndex("dbo.InterestTransactions", new[] { "GLAccountId" });
            DropIndex("dbo.InterestReports", new[] { "CustomerId" });
            DropIndex("dbo.InterestReports", new[] { "InterestReportStatusCode" });
            DropIndex("dbo.InterestReports", new[] { "ARinvoiceId" });
            DropIndex("dbo.InterestReports", new[] { "GLAccountId" });
            DropIndex("dbo.InterestReports", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestReports", new[] { "CreatedByUserId" });
            DropIndex("dbo.InterestReportLines", new[] { "InterestTransactionId" });
            DropIndex("dbo.InterestReportLines", new[] { "InterestReportId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "CreatedByUserId" });
            DropIndex("dbo.InterestBasesPeriods", new[] { "InterestBaseTypeId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "CreatedByUserId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "CreatedByUserId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "UpdatedByUserId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "CreditInterestRateBaseId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "ExceptionalInterestRateBaseId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "StandardInterestRateBaseId" });
            DropIndex("dbo.GLAccountInterestPeriods", new[] { "GLAccountId" });
            DropIndex("Customs.GatepassRequests", new[] { "TransportationTypeCode" });
            DropIndex("Customs.GatepassRequests", new[] { "DesignateSiteCode" });
            DropIndex("Customs.GatepassRequests", new[] { "UpdateCode" });
            DropIndex("Customs.GatepassRequests", new[] { "OriginSiteCode" });
            DropIndex("dbo.JournalLines", new[] { "ActionId" });
            DropIndex("dbo.DocumentsExecutionLogs", new[] { "StatusCode" });
            DropIndex("dbo.DocumentsExecutionLogs", new[] { "CreatedByUserId" });
            DropIndex("Customs.DeficitDecisions", new[] { "DecisionCode" });
            DropIndex("Customs.DeficitDecisions", new[] { "ApprovedProfessionCode" });
            DropIndex("Customs.DeficitDecisions", new[] { "RequestTypeCode" });
            DropIndex("Customs.DeficitDecisions", new[] { "DeclarationId" });
            DropIndex("Customs.DeficitDecisions", new[] { "DeficitId" });
            DropIndex("Customs.DeclarationPendings", new[] { "CourierPendingReasonCode" });
            DropIndex("Customs.DeclarationPendings", new[] { "DeclarationID" });
            DropIndex("Customs.DeclarationCourierStatuses", new[] { "StorageSiteStatusCode" });
            DropIndex("Customs.DeclarationCourierStatuses", new[] { "CourierPendingReasonCode" });
            DropIndex("Customs.DeclarationCourierStatuses", new[] { "DeclarationId" });
            DropIndex("Customs.DecDangersContacts", new[] { "ContactCommTypeCode" });
            DropIndex("Customs.DecDangersContacts", new[] { "CompanyCommTypeCode" });
            DropIndex("Customs.DecDangersContacts", new[] { "DeclarationId" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "CargoTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "ProcessTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "TransportationTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "DocumentTypeCode" });
            DropIndex("dbo.CustomerOpenFilesAmounts", new[] { "CustomerId" });
            DropIndex("Customs.CurrencyTypeTenants", new[] { "Code" });
            DropIndex("Customs.CurrencyTypeTenants", new[] { "UpdatedByUserId" });
            DropIndex("Customs.CourierPendingReasons", new[] { "ErrorPlace" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "DangerousGoodsPackingReqCode" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "UNCode" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" });
            DropIndex("Customs.Declarations", new[] { "AmendmentStatus" });
            DropIndex("Customs.ClientDrivingLicenseTypes", new[] { "ClientId", "ClientDrivingLicenseLine" });
            DropIndex("Customs.ClientDrivingLicenses", new[] { "DrivingLicenseCountryID" });
            DropIndex("Customs.ClientDrivingLicenses", new[] { "ClientId" });
            DropIndex("Customs.ClaimsRelatedEntitiesSeizures", new[] { "SeizureMethodCode" });
            DropIndex("Customs.ClaimsRelatedEntitiesSeizures", new[] { "SeizureFactorCode" });
            DropIndex("Customs.ClaimsRelatedEntitiesSeizures", new[] { "ClaimId", "CounterKey" });
            DropIndex("Customs.ClaimsRelatedEntitiesRefunds", new[] { "ClaimId", "CounterKey" });
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "ContinuousRequestTypeCode" });
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "DecisionCode" });
            DropIndex("dbo.CarrierAreasPorts", new[] { "AddedByUserId" });
            DropIndex("dbo.CarrierAreasPorts", new[] { "PortId" });
            DropIndex("dbo.CarrierAreasPorts", new[] { "CarrierAreaId" });
            DropIndex("dbo.CarrierAreas", new[] { "TransportModeCode" });
            DropIndex("dbo.CarrierAreas", new[] { "UpdatedByUserId" });
            DropIndex("dbo.CarrierAreas", new[] { "CreatedByUserId" });
            DropIndex("dbo.CarrierAreas", new[] { "CarrierId" });
            DropIndex("dbo.BIReports", new[] { "LastRunByUserId" });
            DropIndex("dbo.ShipmentComputedFields", new[] { "DeliveryToPortId" });
            DropIndex("dbo.ShipmentComputedFields", new[] { "OperationallyClosedByUserId" });
            DropIndex("dbo.Shipments", new[] { "AgentComputed" });
            DropIndex("dbo.APPayments", new[] { "ApprovedByUserId" });
            DropIndex("dbo.APPayments", new[] { "AccountingPaymentMethodId" });
            DropIndex("dbo.APInvoiceLines", new[] { "ContainerTypeId" });
            DropIndex("dbo.APInvoiceLines", new[] { "ForiegnCurrencyId" });
            DropIndex("dbo.Features", new[] { "FeatureUniqeCode" });
            DropIndex("dbo.ObjectFields", new[] { "FieldCode" });
            DropIndex("dbo.SupportMailboxes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.SupportMailboxes", new[] { "CreatedByUserId" });
            DropIndex("dbo.Tickets", new[] { "SupportMailboxId" });
            DropIndex("dbo.Quotes", new[] { "ProfitCurrencyId" });
            DropIndex("dbo.Quotes", new[] { "QuoteClosingReasonCode" });
            DropIndex("dbo.Quotes", new[] { "CountryForStatisticsId" });
            DropIndex("dbo.Quotes", new[] { "QuoteHTMLDocumentId" });
            DropIndex("dbo.LogBoxTenantSettings", new[] { "LogBoxAdminUserId" });
            DropIndex("dbo.LogBoxTenantSettings", new[] { "Id" });
            DropIndex("dbo.Tenants", new[] { "PasswordPolicyCode" });
            DropIndex("dbo.Tenants", new[] { "ChargeableWeightUnitCode" });
            DropIndex("dbo.SharedLogisticsContactLastLogins", new[] { "ContactId" });
            DropIndex("dbo.Contacts", new[] { "SharedLogisticsContactLastLogin_ContactId", "SharedLogisticsContactLastLogin_CardId", "SharedLogisticsContactLastLogin_PartnerTypeId", "SharedLogisticsContactLastLogin_Via" });
            DropIndex("dbo.AccountingPartners", new[] { "Id" });
            DropPrimaryKey("dbo.TariffSurchargesUpdateMethods");
            DropPrimaryKey("dbo.CustomerCompetitorProducts");
            DropPrimaryKey("Customs.AmendmentStatuses");
            DropPrimaryKey("Customs.DecisionTypes");
            DropPrimaryKey("Customs.ContinuousRequestTypes");
            DropPrimaryKey("dbo.QuoteClosingReasons");
            AlterColumn("dbo.VatTypePercentages", "Percentage", c => c.Double());
            AlterColumn("dbo.TariffSurchargesUpdateMethods", "Name", c => c.String());
            AlterColumn("dbo.TariffSurchargesUpdateMethods", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TarrifSteps", "UnitPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifSteps", "MinPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifSteps", "MaxPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifSteps", "Step", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifCharges", "UnitPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifCharges", "MinPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TarrifCharges", "MaxPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ShipmentReceivables", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.ShipmentReceivables", "UpdateDate", c => c.DateTime());
            AlterColumn("dbo.ShipmentPickUpDeliveryPackages", "Quantity", c => c.Int());
            AlterColumn("dbo.ShipmentPayables", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.ShipmentPayables", "UpdateDate", c => c.DateTime());
            AlterColumn("dbo.ShipmentPackageItems", "Quantity", c => c.Int());
            AlterColumn("dbo.ShipmentOrderPackages", "Quantity", c => c.Int());
            AlterColumn("dbo.QuotePriceSteps", "Step", c => c.Double());
            AlterColumn("dbo.QuoteCharges", "CostExchangeRate", c => c.Double());
            AlterColumn("dbo.QueueMessageMoreDetails", "Field3", c => c.String());
            AlterColumn("dbo.QueueMessageMoreDetails", "Field2", c => c.String());
            AlterColumn("dbo.QueueMessageMoreDetails", "Field1", c => c.String());
            AlterColumn("dbo.QueueMessageMoreDetails", "MessageBody", c => c.String());
            AlterColumn("dbo.QueueMessageMoreDetails", "QueueDefinitionCode", c => c.String(maxLength: 265, unicode: false));
            AlterColumn("dbo.PaymentCheques", "PayToName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.ShipmentPackages", "Temperature", c => c.Double());
            AlterColumn("dbo.ShipmentPackages", "Quantity", c => c.Int());
            AlterColumn("dbo.InsideShipmentPackages", "Quantity", c => c.Int());
            AlterColumn("dbo.InboundEmailLines", "Subject", c => c.String(maxLength: 100));
            AlterColumn("dbo.Followers", "CancelledDate", c => c.Boolean());
            AlterColumn("dbo.JournalLines", "Notes", c => c.String(maxLength: 60));
            AlterColumn("dbo.JournalLines", "ActionCode", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Competitors", "Name", c => c.String(maxLength: 60));
            AlterColumn("Customs.AmendmentStatuses", "Name", c => c.String());
            AlterColumn("Customs.AmendmentStatuses", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.Declarations", "AmendmentStatus", c => c.String(maxLength: 128));
            AlterColumn("Customs.DecisionTypes", "SearchFields", c => c.String());
            AlterColumn("Customs.DecisionTypes", "EnglishName", c => c.String());
            AlterColumn("Customs.DecisionTypes", "LocalName", c => c.String());
            AlterColumn("Customs.DecisionTypes", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.ContinuousRequestTypes", "EnglishName", c => c.String());
            AlterColumn("Customs.ContinuousRequestTypes", "SearchFields", c => c.String());
            AlterColumn("Customs.ContinuousRequestTypes", "LocalName", c => c.String());
            AlterColumn("Customs.ContinuousRequestTypes", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", c => c.String(maxLength: 128));
            AlterColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.GLAccounts", "DisplayNumber", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ShipmentComputedFields", "ContainersNumbers", c => c.String());
            AlterColumn("dbo.ShipmentComputedFields", "FirstPickupLocation", c => c.String());
            AlterColumn("dbo.ShipmentComputedFields", "Commodity", c => c.String());
            AlterColumn("dbo.ShipmentComputedFields", "ImporterDepositionRequestDetails", c => c.String());
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ShipmentAddtionalDataXML", c => c.String());
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "DenyReason", c => c.String());
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "VersionApproved", c => c.String());
            AlterColumn("dbo.ShipmentAdditionalCloudDatas", "ApprovedByUserName", c => c.String());
            AlterColumn("dbo.Shipments", "SecurityKey", c => c.String(maxLength: 40));
            AlterColumn("dbo.Shipments", "OpenReceivablesInLocalCurrency", c => c.Double());
            AlterColumn("dbo.AutomationResultEmailRecipients", "RecipientValue", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.ARInvoiceTotalVATs", "ProfitVatableAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "ProfitCurrencyVATAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "VatPercent", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "LocalVatableAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "InvoiceCurrencyVatableAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "LocalVATAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceTotalVATs", "InvoiceCurrencyVATAmount", c => c.Double());
            AlterColumn("dbo.ARPayments", "UpdateDate", c => c.DateTime());
            AlterColumn("dbo.ARPayments", "OpenAmount", c => c.Double());
            AlterColumn("dbo.ARPayments", "RegisterDate", c => c.DateTime());
            AlterColumn("dbo.ARPayments", "PaymentCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.ARPayments", "AmountInPaymentCurrency", c => c.Double());
            AlterColumn("dbo.ARPayments", "AmountInLocalCurrency", c => c.Double());
            AlterColumn("dbo.ARPayments", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.ARInvoicePayments", "ForeignAmount", c => c.Double());
            AlterColumn("dbo.ARInvoicePayments", "LocalAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "ProfitCurrencyAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "ForiegnExchangeRate", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "InvoiceCurrencyAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "LocalCurrencyAmount", c => c.Double());
            AlterColumn("dbo.ARInvoiceLines", "ForiegnCurrencyAmount", c => c.Double());
            AlterColumn("dbo.ARInvoices", "UpdateDate", c => c.DateTime());
            AlterColumn("dbo.ARInvoices", "ProfitCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.ARInvoices", "AmountInProfitCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "AmountDue", c => c.Double());
            AlterColumn("dbo.ARInvoices", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.ARInvoices", "InvoiceCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.ARInvoices", "AmountInInvoiceCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "AmountInLocalCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "SubTotalInInvoiceCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "SubTotalInLocalCurrency", c => c.Double());
            AlterColumn("dbo.ARInvoices", "DueDate", c => c.DateTime());
            AlterColumn("dbo.ARInvoices", "InvoiceDate", c => c.DateTime());
            AlterColumn("dbo.APInvoiceTotalVATs", "ProfitVatableAmount", c => c.Double());
            AlterColumn("dbo.APInvoiceTotalVATs", "ProfitCurrencyVATAmount", c => c.Double());
            AlterColumn("dbo.APPayments", "ApprovedByUserId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.APPayments", "UpdateDate", c => c.DateTime());
            AlterColumn("dbo.APPayments", "ValueDate", c => c.DateTime());
            AlterColumn("dbo.APPayments", "OpenAmount", c => c.Double());
            AlterColumn("dbo.APPayments", "RegisterDate", c => c.DateTime());
            AlterColumn("dbo.APPayments", "PaymentCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.APPayments", "AmountInPaymentCurrency", c => c.Double());
            AlterColumn("dbo.APPayments", "AccountingPaymentMethodId", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.APPayments", "AmountInLocalCurrency", c => c.Double());
            AlterColumn("dbo.APPayments", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.APInvoicePayments", "ForeignAmount", c => c.Double());
            AlterColumn("dbo.APInvoicePayments", "LocalAmount", c => c.Double());
            AlterColumn("dbo.APInvoiceLines", "ForiegnCurrencyId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.APInvoiceLines", "ProfitCurrencyAmount", c => c.Double());
            AlterColumn("dbo.APInvoiceLines", "LocalCurrencyAmount", c => c.Double());
            AlterColumn("dbo.APInvoiceLines", "InvoiceCurrencyAmount", c => c.Double());
            AlterColumn("dbo.APInvoices", "UpdateDate", c => c.DateTime());
            AlterColumn("dbo.APInvoices", "AmountInProfitCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "ProfitCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.APInvoices", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.APInvoices", "AmountInLocalCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "AmountInInvoiceCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "SubTotalInInvoiceCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "SubTotalInLocalCurrency", c => c.Double());
            AlterColumn("dbo.APInvoices", "InvoiceCurrencyExchangeRate", c => c.Double());
            AlterColumn("dbo.APInvoices", "DueDate", c => c.DateTime());
            AlterColumn("dbo.APInvoices", "InvoiceDate", c => c.DateTime());
            AlterColumn("dbo.Features", "Code", c => c.String(nullable: false, maxLength: 80, unicode: false));
            AlterColumn("dbo.Tickets", "Subject", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.QuoteClosingReasons", "SearchFields", c => c.String());
            AlterColumn("dbo.QuoteClosingReasons", "Name", c => c.String());
            AlterColumn("dbo.QuoteClosingReasons", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Quotes", "QuoteClosingReasonCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.Quotes", "ExchangeRate", c => c.Double());
            AlterColumn("dbo.Screens", "Name", c => c.String(maxLength: 40, unicode: false));
            AlterColumn("dbo.ObjectTables", "SplitComponentPath", c => c.String(maxLength: 250));
            AlterColumn("dbo.ObjectTables", "CodeField", c => c.String(maxLength: 15, unicode: false));
            AlterColumn("dbo.Tenants", "PasswordPolicyCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("dbo.Industries", "Code", c => c.String());
            AlterColumn("dbo.Contacts", "ExternalId", c => c.String());
            AlterColumn("dbo.Cards", "BankAddress", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.Cards", "BankName", c => c.String(maxLength: 40, unicode: false));
            DropColumn("dbo.WarehouseReleases", "IsUsed");
            DropColumn("dbo.WarehouseReleases", "ToAddressCountryId");
            DropColumn("dbo.WarehouseReleases", "ToAddressCity");
            DropColumn("dbo.WarehouseReleases", "ToAddressZipCode");
            DropColumn("dbo.WarehouseReleases", "ToAddressId");
            DropColumn("dbo.WarehouseReleases", "ToPartnerCardId");
            DropColumn("dbo.WarehouseReleases", "ToTypeCode");
            DropColumn("dbo.WarehouseReleases", "Ratio");
            DropColumn("dbo.WarehouseReleases", "TotalVolumetricWeight");
            DropColumn("dbo.WarehouseReleases", "CustomerAddressId");
            DropColumn("dbo.WarehouseReleases", "ToPortId");
            DropColumn("dbo.WarehouseReleases", "FromPortId");
            DropColumn("dbo.WarehouseReleases", "ConnectedTo");
            DropColumn("dbo.WarehouseEntries", "ToCountryId");
            DropColumn("dbo.WarehouseEntries", "FromCountryId");
            DropColumn("dbo.WarehouseEntries", "FromTypeCode");
            DropColumn("dbo.WarehouseEntries", "ToTypeCode");
            DropColumn("dbo.WarehouseEntries", "Ratio");
            DropColumn("dbo.WarehouseEntries", "ConnectedTo");
            DropColumn("dbo.WarehouseEntries", "LastStatusUpdateDate");
            DropColumn("dbo.Translations", "TextCodeCode");
            DropColumn("dbo.TariffTypes", "TransportModeCode");
            DropColumn("dbo.TariffSettings", "LCLDefaultStepsId");
            DropColumn("dbo.TariffSettings", "AirDefaultStepsId");
            DropColumn("dbo.Tariffs", "ContainerType5Id");
            DropColumn("dbo.Tariffs", "ContainerType4Id");
            DropColumn("dbo.Tariffs", "ContainerType3Id");
            DropColumn("dbo.Tariffs", "ContainerType2Id");
            DropColumn("dbo.Tariffs", "ContainerType1Id");
            DropColumn("dbo.Tariffs", "Notes");
            DropColumn("dbo.TariffLines", "CurrencyId");
            DropColumn("dbo.TariffLines", "Surcharge10MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge9MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge8MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge7MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge6MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge5MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge4MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge3MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge2MinPrice");
            DropColumn("dbo.TariffLines", "Surcharge1MinPrice");
            DropColumn("dbo.SLAHeaders", "SearchFields");
            DropColumn("dbo.ShipmentPayables", "TariffVersion");
            DropColumn("dbo.ShipmentPayables", "TariffNumber");
            DropColumn("dbo.ShipmentPayables", "TariffId");
            DropColumn("dbo.SharedUserQueries", "QueryCode");
            DropColumn("dbo.SharedLogisticsSettings", "IsShowAmountLocalCurrency");
            DropColumn("dbo.ScreenModifications", "ScreenCode");
            DropColumn("dbo.ScreenFields", "ObjectFieldCode");
            DropColumn("dbo.ScreenFields", "ScreenCode");
            DropColumn("dbo.SchedulerProcedure", "IsInternallyDefined");
            DropColumn("dbo.TasksScheduler", "EntityId");
            DropColumn("dbo.TasksScheduler", "AverageRunTime");
            DropColumn("dbo.TaskSchedulerHistory", "LogDocumentId");
            DropColumn("dbo.RuleConditionFields", "ObjectFieldCode");
            DropColumn("dbo.RoleFeatures", "FeatureUniqeCode");
            DropColumn("dbo.Restrictions", "ObjectFieldCode");
            DropColumn("dbo.Reports", "AvailableForScheduling");
            DropColumn("dbo.Reports", "FeatureUniqeCode");
            DropColumn("dbo.ReportExecutionLogs", "ExecutedByServerName");
            DropColumn("dbo.ReportExecutionLogs", "StartDate");
            DropColumn("dbo.ReportExecutionLogs", "RetryNumber");
            DropColumn("dbo.QuoteSettings", "AutomaticallyCloseDays");
            DropColumn("dbo.QuoteSettings", "CopyExchangeRates");
            DropColumn("dbo.QueryColumns", "ObjectFieldCode");
            DropColumn("dbo.QueryColumns", "QueryCode");
            DropColumn("dbo.PackageFeatures", "FeatureUniqeCode");
            DropColumn("dbo.Occasions", "InvitedContacts");
            DropColumn("dbo.Occasions", "InvitedCustomers");
            DropColumn("dbo.Occasions", "ParticipatedContacts");
            DropColumn("dbo.Occasions", "ParticipatedCustomers");
            DropColumn("dbo.ObjectTableTabs", "FeatureUniqeCode");
            DropColumn("dbo.ObjectTableTabs", "TabNameTextCodeCode");
            DropColumn("dbo.ObjectTableRules", "TriggerFieldCode");
            DropColumn("dbo.ObjectTableRuleFields", "ObjectFieldCode");
            DropColumn("dbo.ObjectTableHelperControls", "FeatureUniqeCode");
            DropColumn("dbo.ObjectFieldValidations", "ObjectFieldCode");
            DropColumn("dbo.ObjectFieldModifications", "ObjectFieldCode");
            DropColumn("dbo.MenusTables", "FeatureUniqeCode");
            DropColumn("dbo.MenuButtons", "LabelTextCodeCode");
            DropColumn("dbo.MenuButtons", "FeatureUniqeCode");
            DropColumn("dbo.FullAccountingSettings", "NumberOfAgingMonths");
            DropColumn("dbo.ReconcileExternalPages", "EntityId");
            DropColumn("dbo.ReconcileExternalPages", "ObjectTableId");
            DropColumn("dbo.ReconcileExternalPageLines", "InReconcileProgress");
            DropColumn("dbo.ReconcileExternalPageLines", "InProgressExternalReconcile");
            DropColumn("dbo.JournalLines", "ActionId");
            DropColumn("dbo.LedgerTransactions", "InProgressExternalReconcile");
            DropColumn("dbo.DocumentTypeTemplates", "BCC");
            DropColumn("dbo.CustomsInterfaceSettings", "AMCOceanStartDate");
            DropColumn("dbo.CustomsInterfaceSettings", "AMCAirStartDate");
            DropColumn("dbo.CustomerFieldsUpdateSettings", "ObjectFieldCode");
            DropColumn("dbo.CreditLimitSettings", "WarehousesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "VendorsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "TruckersInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingLinesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "AirlinesInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingAgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomsAgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "ShipperConsigneeInvoiceBlock");
            DropColumn("dbo.CreditLimitSettings", "AgentsInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomersInvoicesBlock");
            DropColumn("dbo.CreditLimitSettings", "WarehousesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "VendorsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "TruckersShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingLinesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "AirlinesShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShippingAgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomsAgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "ShipperConsigneeShipmentBlock");
            DropColumn("dbo.CreditLimitSettings", "AgentsShipmentsBlock");
            DropColumn("dbo.CreditLimitSettings", "CustomersShipmentsBlock");
            DropColumn("dbo.BIReports", "LastRunByUserId");
            DropColumn("dbo.BIReports", "LastRunDate");
            DropColumn("dbo.GLAccounts", "Smallcashbook");
            DropColumn("dbo.GLAccounts", "NameForPrintingCheques");
            DropColumn("dbo.GLAccounts", "InterestCreditLimit");
            DropColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling");
            DropColumn("dbo.GLAccounts", "ActiveForInterestCreditInvoice");
            DropColumn("dbo.GLAccounts", "InterestCalculationStartDate");
            DropColumn("dbo.GLAccounts", "ActiveForInterest");
            DropColumn("dbo.GLAccounts", "AllowEditChequePayToName");
            DropColumn("dbo.BankAccounts", "PrintingAccountNumber");
            DropColumn("dbo.BankAccounts", "PrintingBranchNumber");
            DropColumn("dbo.ShipmentMasterDatas", "CutoffDate");
            DropColumn("dbo.ShipmentMasterDatas", "AutomaticLastUpdateDate");
            DropColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserName");
            DropColumn("dbo.ShipmentComputedFields", "PickupTo");
            DropColumn("dbo.ShipmentComputedFields", "PickupFrom");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryTo");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryFrom");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryToPortId");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupATD");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupATA");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupETD");
            DropColumn("dbo.ShipmentComputedFields", "LastPickupETA");
            DropColumn("dbo.ShipmentComputedFields", "NumberOfDeliveries");
            DropColumn("dbo.ShipmentComputedFields", "OperationallyClosedByUserId");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumber");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberXMLData");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "UserIdNumberUpdateDate");
            DropColumn("dbo.ShipmentAdditionalCloudDatas", "IsUserIDNumberRequired");
            DropColumn("dbo.EntityStatus", "AutomaticLastUpdateDate");
            DropColumn("dbo.Shipments", "WarehouseStorageFreeDays");
            DropColumn("dbo.Shipments", "FirstARInvoiceApprovalDate");
            DropColumn("dbo.Shipments", "CreatedByPartner");
            DropColumn("dbo.Shipments", "NotInvoicedReceivablesAmount");
            DropColumn("dbo.Shipments", "INTTRALastBookingResponse");
            DropColumn("dbo.Shipments", "INTTRABookingError");
            DropColumn("dbo.Shipments", "AutomaticLastUpdateDate");
            DropColumn("dbo.Shipments", "GrossWeightPerStorageDays");
            DropColumn("dbo.ARPayments", "Field10");
            DropColumn("dbo.ARPayments", "Field9");
            DropColumn("dbo.ARPayments", "Field8");
            DropColumn("dbo.ARPayments", "Field7");
            DropColumn("dbo.ARPayments", "Field6");
            DropColumn("dbo.ARPayments", "Field5");
            DropColumn("dbo.ARPayments", "Field4");
            DropColumn("dbo.ARPayments", "Field3");
            DropColumn("dbo.ARPayments", "Field2");
            DropColumn("dbo.ARPayments", "Field1");
            DropColumn("dbo.ARPayments", "IsPaymentNumberManuallySet");
            DropColumn("dbo.ARPayments", "CreatedByPartner");
            DropColumn("dbo.ARPayments", "OpenAmountInLocalCurrency");
            DropColumn("dbo.ARInvoices", "BillToGLAccountId");
            DropColumn("dbo.ARInvoices", "CreatedByPartner");
            DropColumn("dbo.ARInvoices", "DocumentFilingId");
            DropColumn("dbo.ARInvoices", "DateForInterest");
            DropColumn("dbo.APPayments", "Field10");
            DropColumn("dbo.APPayments", "Field9");
            DropColumn("dbo.APPayments", "Field8");
            DropColumn("dbo.APPayments", "Field7");
            DropColumn("dbo.APPayments", "Field6");
            DropColumn("dbo.APPayments", "Field5");
            DropColumn("dbo.APPayments", "Field4");
            DropColumn("dbo.APPayments", "Field3");
            DropColumn("dbo.APPayments", "Field2");
            DropColumn("dbo.APPayments", "Field1");
            DropColumn("dbo.APPayments", "CancelationNotes");
            DropColumn("dbo.APPayments", "DontIncludeInDeductionReport");
            DropColumn("dbo.APPayments", "AccountingCancelationDate");
            DropColumn("dbo.APPayments", "VendorIBANNumber");
            DropColumn("dbo.APPayments", "VendorSwift");
            DropColumn("dbo.APPayments", "VendorBankAccountNumber");
            DropColumn("dbo.APPayments", "VendorBankName");
            DropColumn("dbo.APPayments", "VendorBankAddress");
            DropColumn("dbo.ChargesGroups", "ViewOrder");
            DropColumn("dbo.ChargesTypes", "ApplyRegionalTax");
            DropColumn("dbo.APInvoiceLines", "Quantity");
            DropColumn("dbo.APInvoiceLines", "ContainerTypeId");
            DropColumn("dbo.APInvoices", "CreatedByPartner");
            DropColumn("dbo.AirlineMessagingRules", "RuleFieldCode");
            DropColumn("dbo.ShipmentLevels", "AutomaticLastUpdateDate");
            DropColumn("dbo.Features", "NameTextCodeCode");
            DropColumn("dbo.Features", "FeatureUniqeCode");
            DropColumn("dbo.Queries", "FeatureUniqeCode");
            DropColumn("dbo.Queries", "NameTextCodeCode");
            DropColumn("dbo.Queries", "OriginalQueryCode");
            DropColumn("dbo.Queries", "UniqueCode");
            DropColumn("dbo.ObjectFields", "RecordType");
            DropColumn("dbo.ObjectFields", "DisplayInAutomationAsEnitity");
            DropColumn("dbo.ObjectFields", "ShortNameTextCodeCode");
            DropColumn("dbo.ObjectFields", "ListTextCodeCode");
            DropColumn("dbo.ObjectFields", "HelpTextCodeCode");
            DropColumn("dbo.ObjectFields", "FullNameTextCodeCode");
            DropColumn("dbo.ObjectFields", "FieldCode");
            DropColumn("dbo.AdvancedQueryFilters", "ObjectFieldCode");
            DropColumn("dbo.AdvancedQueryFilters", "QueryCode");
            DropColumn("dbo.Tickets", "LastCorrespondence");
            DropColumn("dbo.Tickets", "SupportMailboxId");
            DropColumn("dbo.ShipmentTypes", "AutomaticLastUpdateDate");
            DropColumn("dbo.TransportModes", "AutomaticLastUpdateDate");
            DropColumn("dbo.Incoterms", "AutomaticLastUpdateDate");
            DropColumn("dbo.Ports", "AutomaticLastUpdateDate");
            DropColumn("dbo.Directions", "AutomaticLastUpdateDate");
            DropColumn("dbo.Quotes", "ProfitExchangeRate");
            DropColumn("dbo.Quotes", "ProfitCurrencyId");
            DropColumn("dbo.Quotes", "EstimatedProfitInProfit");
            DropColumn("dbo.Quotes", "EstimatedProfitInLocal");
            DropColumn("dbo.Quotes", "RequestDate");
            DropColumn("dbo.Quotes", "CountryForStatisticsId");
            DropColumn("dbo.Quotes", "Field20");
            DropColumn("dbo.Quotes", "Field19");
            DropColumn("dbo.Quotes", "Field18");
            DropColumn("dbo.Quotes", "Field17");
            DropColumn("dbo.Quotes", "Field16");
            DropColumn("dbo.Quotes", "Field15");
            DropColumn("dbo.Quotes", "Field14");
            DropColumn("dbo.Quotes", "Field13");
            DropColumn("dbo.Quotes", "Field12");
            DropColumn("dbo.Quotes", "Field11");
            DropColumn("dbo.Quotes", "QuoteHTMLDocumentId");
            DropColumn("dbo.Tips", "ShortTextCodeCode");
            DropColumn("dbo.ObjectTables", "NewButtonTextCodeCode");
            DropColumn("dbo.ObjectTables", "DescriptionTextCodeCode");
            DropColumn("dbo.ObjectTables", "HeaderScreenCode");
            DropColumn("dbo.AccountingPaymentMethods", "LocalName");
            DropColumn("dbo.VatTypes", "IsRegionalTax");
            DropColumn("dbo.VatTypes", "RecognizedPercentage");
            DropColumn("dbo.VatTypes", "ReceivablesExternalId");
            DropColumn("dbo.VatTypes", "PayablesExternalId");
            DropColumn("dbo.ShippingLines", "INTTRAUpdatesShipment");
            DropColumn("dbo.PartnerTypes", "AutomaticLastUpdateDate");
            DropColumn("dbo.PaymentTerms", "Code");
            DropColumn("dbo.AccountingSettings", "AllowRegionalTaxManagement");
            DropColumn("dbo.AccountingSettings", "AllowManualARPaymentNumber");
            DropColumn("dbo.AccountingSettings", "QBOOAuth");
            DropColumn("dbo.AccountingSettings", "RefreshToken");
            DropColumn("dbo.Tenants", "AutomaticLastUpdateDate");
            DropColumn("dbo.Tenants", "AllowCustomersInAgentsLOV");
            DropColumn("dbo.Tenants", "HideFCLAllIn");
            DropColumn("dbo.Currencies", "AutomaticLastUpdateDate");
            DropColumn("dbo.Ranks", "AutomaticLastUpdateDate");
            DropColumn("dbo.Customers", "LastOpportunityStatus");
            DropColumn("dbo.Customers", "LastOpportunitySubject");
            DropColumn("dbo.Customers", "AutomaticLastUpdateDate");
            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesContainers");
            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPackages");
            DropColumn("dbo.QuoteTemplateSettings", "ShowIncludedChargesPerContainers");
            DropColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginBottom");
            DropColumn("dbo.QuoteTemplateSettings", "QuoteTemplatePDFMarginTop");
            DropColumn("dbo.Departments", "AutomaticLastUpdateDate");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_Via");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_PartnerTypeId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_CardId");
            DropColumn("dbo.Contacts", "SharedLogisticsContactLastLogin_ContactId");
            DropColumn("dbo.Contacts", "AutomaticLastUpdateDate");
            DropColumn("dbo.States", "AutomaticLastUpdateDate");
            DropColumn("dbo.Countries", "AutomaticLastUpdateDate");
            DropColumn("dbo.Addresses", "AutomaticLastUpdateDate");
            DropColumn("dbo.Branches", "AutomaticLastUpdateDate");
            DropColumn("dbo.Users", "AutomaticLastUpdateDate");
            DropColumn("dbo.Cards", "AutomaticLastUpdateDate");
            DropColumn("dbo.Cards", "StorageFreeDays");
            DropColumn("dbo.Cards", "CreatedByPartner");
            DropColumn("dbo.AccountingIntegrityChecks", "ShouldFix");
            DropTable("dbo.CustomsTransferLines");
            DropTable("dbo.CustomsTransferTypes");
            DropTable("dbo.CustomsTransferHeaders");
            DropTable("dbo.CardContactAdditionalServices");
            DropTable("dbo.UserLastSettings");
            DropTable("dbo.PaymentGatewayPartners");
            DropTable("dbo.TariffSurchargesUpdates");
            DropTable("dbo.TariffLinesContainersPrices");
            DropTable("dbo.RuleUpdateHistories");
            DropTable("Customs.RefundCustomerActivityTypes");
            DropTable("dbo.PriceSteps");
            DropTable("Customs.PendingByKeywords");
            DropTable("dbo.OccasionInvitees");
            DropTable("dbo.JournalExternalReconciles");
            DropTable("dbo.InterestReportLinesByDates");
            DropTable("dbo.InterestTransactions");
            DropTable("dbo.InterestReportStatuses");
            DropTable("dbo.InterestReports");
            DropTable("dbo.InterestReportLines");
            DropTable("dbo.InterestEntityTypes");
            DropTable("dbo.InterestBasesPeriods");
            DropTable("dbo.InterestBasesTypes");
            DropTable("dbo.GLAccountInterestPeriods");
            DropTable("Customs.GatepassReturnCodes");
            DropTable("Customs.UpdateCodes");
            DropTable("Customs.TransferCargoMethodTypes");
            DropTable("Customs.GatepassRequests");
            DropTable("dbo.ExternalPageAdditionalDatas");
            DropTable("dbo.DWHBuildStatus");
            DropTable("dbo.DocumentsExecutionLogs");
            DropTable("Customs.RequestTypes");
            DropTable("Customs.DeficitDecisions");
            DropTable("Customs.DeclarationPendings");
            DropTable("Customs.DeclarationCourierStatuses");
            DropTable("Customs.DecDangersContacts");
            DropTable("Customs.CustomsDocumentsDefinitions");
            DropTable("dbo.CustomerOpenFilesAmounts");
            DropTable("Customs.CurrencyTypeTenants");
            DropTable("Customs.CourierPendingReasons");
            DropTable("Customs.HazardousSubstances");
            DropTable("Customs.ConsignmentPackDangers");
            DropTable("Customs.ClientDrivingLicenseTypes");
            DropTable("Customs.ClientDrivingLicenses");
            DropTable("Customs.SeizureMethodTypes");
            DropTable("Customs.SeizureFactorTypes");
            DropTable("Customs.ClaimsRelatedEntitiesSeizures");
            DropTable("Customs.ClaimsRelatedEntitiesRefunds");
            DropTable("dbo.CarrierAreasPorts");
            DropTable("dbo.CarrierAreas");
            DropTable("dbo.ApprovedProfessions");
            DropTable("dbo.SupportMailboxes");
            DropTable("dbo.LogBoxTenantSettings");
            DropTable("dbo.SharedLogisticsContactLastLogins");
            DropTable("dbo.AccountingPartners");
            AddPrimaryKey("dbo.TariffSurchargesUpdateMethods", "Code");
            AddPrimaryKey("dbo.CustomerCompetitorProducts", "ProductTypeCode");
            AddPrimaryKey("Customs.AmendmentStatuses", "Code");
            AddPrimaryKey("Customs.DecisionTypes", "Code");
            AddPrimaryKey("Customs.ContinuousRequestTypes", "Code");
            AddPrimaryKey("dbo.QuoteClosingReasons", "Code");
            CreateIndex("dbo.SchedulerLogs", "HistoryId");
            CreateIndex("dbo.QueueMessageMoreDetails", "QueueDefinitionCode");
            CreateIndex("dbo.ReconcileExternalPages", "BankAccountId");
            CreateIndex("dbo.JournalLines", "ActionCode");
            CreateIndex("Customs.Declarations", "AmendmentStatus");
            CreateIndex("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode");
            CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            CreateIndex("dbo.APPayments", "ApprovedByUserId");
            CreateIndex("dbo.APPayments", "AccountingPaymentMethodId");
            CreateIndex("dbo.APInvoiceLines", "ForiegnCurrencyId");
            CreateIndex("dbo.AirlineAreasPorts", "AddedByUserId");
            CreateIndex("dbo.AirlineAreasPorts", "PortId");
            CreateIndex("dbo.AirlineAreasPorts", "AirlineAreaId");
            CreateIndex("dbo.AirlineAreas", "UpdatedByUserId");
            CreateIndex("dbo.AirlineAreas", "CreatedByUserId");
            CreateIndex("dbo.AirlineAreas", "AirlineId");
            CreateIndex("dbo.Quotes", "QuoteClosingReasonCode");
            CreateIndex("dbo.Tenants", "LogBoxAdminUserId");
            CreateIndex("dbo.Tenants", "PasswordPolicyCode");
            AddForeignKey("dbo.TenantAdditionalDatas", "PaymentGatewayPartnerCode", "dbo.PaymentGatewayPartners", "Code");
            AddForeignKey("Customs.Declarations", "AmendmentStatus", "dbo.AmendmentStatus", "Code");
            AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes", "Code");
            AddForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "dbo.ContinuousRequestTypes", "Code");
            AddForeignKey("dbo.Quotes", "QuoteClosingReasonCode", "dbo.QuoteClosingReasons", "Code");
            AddForeignKey("dbo.JournalLines", "ActionCode", "dbo.JournalActionTypes", "Id");
            AddForeignKey("dbo.SchedulerLogs", "HistoryId", "dbo.TaskSchedulerHistory", "Id");
            AddForeignKey("dbo.ReconcileExternalPages", "BankAccountId", "dbo.BankAccounts", "Id");
            AddForeignKey("dbo.AirlineAreasPorts", "PortId", "dbo.Ports", "Id");
            AddForeignKey("dbo.AirlineAreasPorts", "AirlineAreaId", "dbo.AirlineAreas", "Id");
            AddForeignKey("dbo.AirlineAreasPorts", "AddedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.AirlineAreas", "UpdatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.AirlineAreas", "CreatedByUserId", "dbo.Users", "Id");
            AddForeignKey("dbo.AirlineAreas", "AirlineId", "dbo.Airlines", "Id");
            AddForeignKey("dbo.Tenants", "LogBoxAdminUserId", "dbo.Contacts", "Id");
            MoveTable(name: "Customs.AmendmentStatuses", newSchema: "dbo");
            MoveTable(name: "Customs.DecisionTypes", newSchema: "dbo");
            MoveTable(name: "Customs.ContinuousRequestTypes", newSchema: "dbo");
            RenameTable(name: "dbo.TariffSurchargesUpdateMethods", newName: "PaymentGatewayPartners");
            RenameTable(name: "dbo.AmendmentStatuses", newName: "AmendmentStatus");
        }
    }
}
