namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingmigrationssqlserver : DbMigration
    {
        public override void Up()
        {
            
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses");
            DropForeignKey("dbo.CarrierAreasPorts", "CarrierAreaId", "dbo.CarrierAreas");
            DropForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes");
            DropIndex("dbo.CarrierAreasPorts", new[] { "CarrierAreaId" });
            DropIndex("Customs.ClaimsRelatedEntities", new[] { "DecisionCode" });
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            DropPrimaryKey("dbo.CarrierAreas");
            DropPrimaryKey("dbo.DecisionTypes");
            MoveTable(name: "dbo.DecisionTypes", newSchema: "Customs");
            //DropPrimaryKey("Customs.VendorCommissions");
            CreateTable(
                "Customs.AmendmentStatuses",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 3, unicode: false),
                        Name = c.String(maxLength: 50, unicode: false),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
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
                "Customs.ContinuousRequestTypes",
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

            DropTable("Customs.DeclarationCourierStatuses");
            DropTable("Customs.CourierPendingReasons");
           
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
            
            
            
            //CreateTable(
            //    "Customs.CustomsDocumentsDefinitions",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 15, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            DocumentTypeCode = c.String(maxLength: 7, unicode: false),
            //            TransportationTypeCode = c.String(maxLength: 1, unicode: false),
            //            ProcessTypeCode = c.String(maxLength: 7, unicode: false),
            //            CargoTypeCode = c.String(maxLength: 4, unicode: false),
            //            Mandatory = c.Boolean(nullable: false),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("Customs.CargoIdentifireTypes", t => t.CargoTypeCode)
            //    .ForeignKey("Customs.CustomDocumentTypes", t => t.DocumentTypeCode)
            //    .ForeignKey("Customs.CustomsTransportModes", t => t.TransportationTypeCode)
            //    .ForeignKey("Customs.GovernmentProcedureTypes", t => t.ProcessTypeCode)
            //    .Index(t => t.DocumentTypeCode)
            //    .Index(t => t.TransportationTypeCode)
            //    .Index(t => t.ProcessTypeCode)
            //    .Index(t => t.CargoTypeCode);
            
            //CreateTable(
            //    "dbo.CustomsPartnerFtps",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Tenant = c.Int(nullable: false),
            //            TypeCode = c.String(),
            //            PartnerCode = c.String(),
            //            InterfaceName = c.String(),
            //            FtpDetailsId = c.String(maxLength: 15, unicode: false),
            //            FileName = c.String(),
            //            FileExt = c.String(),
            //            CommunicationDetails = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.FTPDetails", t => t.FtpDetailsId)
            //    .Index(t => t.FtpDetailsId);
            
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
                        LastMileStatusCode = c.String(maxLength: 30, unicode: false),
                        LastMileStatusDate = c.DateTime(),
                        LastMileStatusRemarks = c.String(maxLength: 2000),
                        StorageSiteStatusCode = c.String(maxLength: 3, unicode: false),
                        StorageSiteErrorText = c.String(maxLength: 1200),
                        CourierPendingReasonList = c.String(maxLength: 1000),
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
            
            //CreateTable(
            //    "Customs.MamanSpecialActions",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 2, unicode: false),
            //            LocalName = c.String(maxLength: 40),
            //            SearchFields = c.String(maxLength: 1000),
            //            EnglishName = c.String(maxLength: 40, unicode: false),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "Customs.MamanSpecialActionStatuses",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 3, unicode: false),
            //            LocalName = c.String(maxLength: 40),
            //            SearchFields = c.String(maxLength: 1000),
            //            EnglishName = c.String(maxLength: 40, unicode: false),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
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
            
            //CreateTable(
            //    "Customs.DeclarationMamanSpecialActions",
            //    c => new
            //        {
            //            DeclarationId = c.String(nullable: false, maxLength: 15, unicode: false),
            //            MamanSpecialActionCode = c.String(nullable: false, maxLength: 2, unicode: false),
            //            Tenant = c.Int(nullable: false),
            //            MamanLabelText1 = c.String(maxLength: 40),
            //            MamanLabelText2 = c.String(maxLength: 40),
            //            MamanLabelText3 = c.String(maxLength: 40),
            //            MamanLabelText4 = c.String(maxLength: 40),
            //            MamanLabelText5 = c.String(maxLength: 40),
            //            MamanSpecialActionStatusCode = c.String(maxLength: 3, unicode: false),
            //            MamanSpecialActionsErrorXml = c.String(maxLength: 1200),
            //        })
            //    .PrimaryKey(t => new { t.DeclarationId, t.MamanSpecialActionCode })
            //    .ForeignKey("Customs.Declarations", t => t.DeclarationId)
            //    .ForeignKey("Customs.MamanSpecialActions", t => t.MamanSpecialActionCode)
            //    .ForeignKey("Customs.MamanSpecialActionStatuses", t => t.MamanSpecialActionStatusCode)
            //    .Index(t => t.DeclarationId)
            //    .Index(t => t.MamanSpecialActionCode)
            //    .Index(t => t.MamanSpecialActionStatusCode);
            
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
            
            //CreateTable(
            //    "dbo.TPGFileTypes",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 128),
            //            LocalName = c.String(),
            //            SearchFields = c.String(),
            //            EnglishName = c.String(),
            //            Inactive = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            
            
            
            
            AddColumn("Customs.Declarations", "IsPaymentProtested", c => c.Boolean(nullable: false));
            AddColumn("Customs.Declarations", "AmendmentRequestNumber", c => c.String(maxLength: 9, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentStatus", c => c.String(maxLength: 3, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentissueDate", c => c.DateTime());
            AddColumn("Customs.Declarations", "AmendmentRemarks", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "AmendmentDeficitInitiated", c => c.Boolean());
            AddColumn("Customs.Declarations", "AmendDeficitInitiatedReasTo", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "AmendmentCorrectedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.Declarations", "AmendmentRejectionReason", c => c.String(maxLength: 512));
            AddColumn("Customs.Declarations", "IsAmendment", c => c.Boolean());
            AddColumn("Customs.Declarations", "AmendmentOriginalDeclartation", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("Customs.CourierMasters", "StorageSiteCode", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("Customs.CourierMasters", "TruckerId", c => c.String(maxLength: 15, unicode: false));
            //AddColumn("Customs.CourierMasters", "IntegratorCode", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.CourierMasters", "IsReadyForInvoice", c => c.Boolean(nullable: false));
            AddColumn("Customs.CustomDocumentTypes", "IsCourierManadatory", c => c.Boolean(nullable: false));
            //AddColumn("Customs.InterfaceManagements", "InterfaceType", c => c.String(maxLength: 1, unicode: false));
            //AddColumn("Customs.CustomsSettings", "CompanyType", c => c.String(nullable: false, maxLength: 1, unicode: false));
            //AddColumn("Customs.PhysicalChecks", "VehicleChassisNumber", c => c.String(maxLength: 20, unicode: false));
            //AddColumn("Customs.ProceduralFaults", "SignedByUserId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers", c => c.String(maxLength: 30, unicode: false));
            AddColumn("Customs.SupplierInvoiceItems", "PackageQuantity", c => c.Int());
            AddColumn("Customs.SupplierInvoiceItems", "Weight", c => c.Decimal(precision: 15, scale: 3));
            //AddColumn("Customs.SupplierInvoices", "ChangeInSupplierInvoice", c => c.String(maxLength: 1, unicode: false));
            //AddColumn("Customs.VendorCommissions", "ModificationsTypeCode", c => c.String(nullable: false, maxLength: 3, unicode: false));
            AlterColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Int());
            AlterColumn("dbo.CarrierAreas", "Id", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.CarrierAreasPorts", "CarrierAreaId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("Customs.DecisionTypes", "Code", c => c.String(nullable: false, maxLength: 2, unicode: false));
            AlterColumn("Customs.DecisionTypes", "LocalName", c => c.String(maxLength: 50));
            AlterColumn("Customs.DecisionTypes", "EnglishName", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("Customs.DecisionTypes", "SearchFields", c => c.String(maxLength: 1000));
            AddPrimaryKey("dbo.CarrierAreas", "Id");
            AddPrimaryKey("Customs.DecisionTypes", "Code");
            AddPrimaryKey("Customs.VendorCommissions", new[] { "VendorId", "CustomerId", "ModificationsTypeCode" });
            //CreateIndex("dbo.CarrierAreas", "TransportModeCode");
            //CreateIndex("dbo.CarrierAreasPorts", "CarrierAreaId");
            CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            //CreateIndex("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode");
            CreateIndex("Customs.Declarations", "AmendmentStatus");
            CreateIndex("Customs.Declarations", "AmendmentCorrectedByUserId");
            //CreateIndex("Customs.CourierMasters", "StorageSiteCode");
            //CreateIndex("Customs.CourierMasters", "IntegratorCode");
            //CreateIndex("Customs.ProceduralFaults", "SignedByUserId");
            //CreateIndex("Customs.VendorCommissions", "ModificationsTypeCode");
            //AddForeignKey("dbo.CarrierAreas", "TransportModeCode", "dbo.TransportModes", "Id");
            //AddForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "Customs.ContinuousRequestTypes", "Code");
            AddForeignKey("Customs.Declarations", "AmendmentCorrectedByUserId", "dbo.Users", "Id");
            AddForeignKey("Customs.Declarations", "AmendmentStatus", "Customs.AmendmentStatuses", "Code");
            //AddForeignKey("Customs.CourierMasters", "IntegratorCode", "dbo.Cards", "Id");
            //AddForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes", "Code");
            //AddForeignKey("Customs.ProceduralFaults", "SignedByUserId", "dbo.Users", "Id");
            //AddForeignKey("Customs.VendorCommissions", "ModificationsTypeCode", "Customs.ModificationAndDiscountTypes", "Code");
            AddForeignKey("dbo.CarrierAreasPorts", "CarrierAreaId", "dbo.CarrierAreas", "Id");
            AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "Customs.DecisionTypes", "Code");
            //DropColumn("Customs.Declarations", "MamanStatusCode");
            //DropColumn("Customs.Declarations", "MamanErrorXml");
        }
        
        public override void Down()
        {
            AddColumn("Customs.Declarations", "MamanErrorXml", c => c.String(unicode: false));
            AddColumn("Customs.Declarations", "MamanStatusCode", c => c.String(maxLength: 3, unicode: false));
            DropForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "Customs.DecisionTypes");
            DropForeignKey("dbo.CarrierAreasPorts", "CarrierAreaId", "dbo.CarrierAreas");
            DropForeignKey("Customs.VendorCommissions", "ModificationsTypeCode", "Customs.ModificationAndDiscountTypes");
            DropForeignKey("Customs.ProceduralFaults", "SignedByUserId", "dbo.Users");
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionStatusCode", "Customs.MamanSpecialActionStatuses");
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "MamanSpecialActionCode", "Customs.MamanSpecialActions");
            DropForeignKey("Customs.DeclarationMamanSpecialActions", "DeclarationId", "Customs.Declarations");
            DropForeignKey("Customs.PendingByKeywords", "CourierPendingReasonCode", "Customs.CourierPendingReasons");
            DropForeignKey("Customs.GatepassRequests", "UpdateCode", "Customs.UpdateCodes");
            DropForeignKey("Customs.GatepassRequests", "TransportationTypeCode", "Customs.TransferCargoMethodTypes");
            DropForeignKey("Customs.GatepassRequests", "OriginSiteCode", "Customs.SiteLookups");
            DropForeignKey("Customs.GatepassRequests", "DesignateSiteCode", "Customs.SiteLookups");
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
            DropForeignKey("dbo.CustomsPartnerFtps", "FtpDetailsId", "dbo.FTPDetails");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "ProcessTypeCode", "Customs.GovernmentProcedureTypes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "TransportationTypeCode", "Customs.CustomsTransportModes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "DocumentTypeCode", "Customs.CustomDocumentTypes");
            DropForeignKey("Customs.CustomsDocumentsDefinitions", "CargoTypeCode", "Customs.CargoIdentifireTypes");
            DropForeignKey("Customs.CourierPendingReasons", "ErrorPlace", "Customs.PendingErrorPlaces");
            DropForeignKey("Customs.CourierMasters", "StorageSiteCode", "Customs.DeliverySiteTypes");
            DropForeignKey("Customs.CourierMasters", "IntegratorCode", "dbo.Cards");
            DropForeignKey("Customs.ConsignmentPackDangers", "UNCode", "Customs.HazardousSubstances");
            DropForeignKey("Customs.ConsignmentPackDangers", "DangerousGoodsPackingReqCode", "Customs.DangerousGoodsPackingReqs");
            DropForeignKey("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" }, "Customs.ConsignmentPackages");
            DropForeignKey("Customs.Declarations", "AmendmentStatus", "Customs.AmendmentStatuses");
            DropForeignKey("Customs.Declarations", "AmendmentCorrectedByUserId", "dbo.Users");
            DropForeignKey("Customs.ClientDrivingLicenseTypes", new[] { "ClientId", "ClientDrivingLicenseLine" }, "Customs.ClientDrivingLicenses");
            DropForeignKey("Customs.ClientDrivingLicenses", "DrivingLicenseCountryID", "Customs.CustomsCountries");
            DropForeignKey("Customs.ClientDrivingLicenses", "ClientId", "Customs.Clients");
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", "SeizureMethodCode", "Customs.SeizureMethodTypes");
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", "SeizureFactorCode", "Customs.SeizureFactorTypes");
            DropForeignKey("Customs.ClaimsRelatedEntitiesSeizures", new[] { "ClaimId", "CounterKey" }, "Customs.ClaimsRelatedEntities");
            DropForeignKey("Customs.ClaimsRelatedEntitiesRefunds", new[] { "ClaimId", "CounterKey" }, "Customs.ClaimsRelatedEntities");
            DropForeignKey("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode", "Customs.ContinuousRequestTypes");
            DropForeignKey("dbo.CarrierAreas", "TransportModeCode", "dbo.TransportModes");
            DropIndex("Customs.VendorCommissions", new[] { "ModificationsTypeCode" });
            DropIndex("Customs.ProceduralFaults", new[] { "SignedByUserId" });
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "MamanSpecialActionStatusCode" });
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "MamanSpecialActionCode" });
            DropIndex("Customs.DeclarationMamanSpecialActions", new[] { "DeclarationId" });
            DropIndex("Customs.PendingByKeywords", new[] { "CourierPendingReasonCode" });
            DropIndex("Customs.GatepassRequests", new[] { "TransportationTypeCode" });
            DropIndex("Customs.GatepassRequests", new[] { "DesignateSiteCode" });
            DropIndex("Customs.GatepassRequests", new[] { "UpdateCode" });
            DropIndex("Customs.GatepassRequests", new[] { "OriginSiteCode" });
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
            DropIndex("dbo.CustomsPartnerFtps", new[] { "FtpDetailsId" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "CargoTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "ProcessTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "TransportationTypeCode" });
            DropIndex("Customs.CustomsDocumentsDefinitions", new[] { "DocumentTypeCode" });
            DropIndex("Customs.CourierPendingReasons", new[] { "ErrorPlace" });
            DropIndex("Customs.CourierMasters", new[] { "IntegratorCode" });
            DropIndex("Customs.CourierMasters", new[] { "StorageSiteCode" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "DangerousGoodsPackingReqCode" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "UNCode" });
            DropIndex("Customs.ConsignmentPackDangers", new[] { "DeclarationId", "ConsignmentNumber", "LineNumber" });
            DropIndex("Customs.Declarations", new[] { "AmendmentCorrectedByUserId" });
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
            DropIndex("dbo.CarrierAreasPorts", new[] { "CarrierAreaId" });
            DropIndex("dbo.CarrierAreas", new[] { "TransportModeCode" });
            DropPrimaryKey("Customs.VendorCommissions");
            DropPrimaryKey("Customs.DecisionTypes");
            DropPrimaryKey("dbo.CarrierAreas");
            AlterColumn("Customs.DecisionTypes", "SearchFields", c => c.String());
            AlterColumn("Customs.DecisionTypes", "EnglishName", c => c.String());
            AlterColumn("Customs.DecisionTypes", "LocalName", c => c.String());
            AlterColumn("Customs.DecisionTypes", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Customs.ClaimsRelatedEntities", "DecisionCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.CarrierAreasPorts", "CarrierAreaId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CarrierAreas", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.GLAccounts", "MinimumInterestInvoiceBilling", c => c.Decimal(precision: 5, scale: 0));
            DropColumn("Customs.VendorCommissions", "ModificationsTypeCode");
            DropColumn("Customs.SupplierInvoices", "ChangeInSupplierInvoice");
            DropColumn("Customs.SupplierInvoiceItems", "Weight");
            DropColumn("Customs.SupplierInvoiceItems", "PackageQuantity");
            DropColumn("Customs.SupplierInvoiceItems", "MarksAndNumbers");
            DropColumn("Customs.ProceduralFaults", "SignedByUserId");
            DropColumn("Customs.PhysicalChecks", "VehicleChassisNumber");
            DropColumn("Customs.CustomsSettings", "CompanyType");
            DropColumn("Customs.InterfaceManagements", "InterfaceType");
            DropColumn("Customs.CustomDocumentTypes", "IsCourierManadatory");
            DropColumn("Customs.CourierMasters", "IsReadyForInvoice");
            DropColumn("Customs.CourierMasters", "IntegratorCode");
            DropColumn("Customs.CourierMasters", "TruckerId");
            DropColumn("Customs.CourierMasters", "StorageSiteCode");
            DropColumn("Customs.Declarations", "AmendmentOriginalDeclartation");
            DropColumn("Customs.Declarations", "IsAmendment");
            DropColumn("Customs.Declarations", "AmendmentRejectionReason");
            DropColumn("Customs.Declarations", "AmendmentCorrectedByUserId");
            DropColumn("Customs.Declarations", "AmendDeficitInitiatedReasTo");
            DropColumn("Customs.Declarations", "AmendmentDeficitInitiated");
            DropColumn("Customs.Declarations", "AmendmentRemarks");
            DropColumn("Customs.Declarations", "AmendmentissueDate");
            DropColumn("Customs.Declarations", "AmendmentStatus");
            DropColumn("Customs.Declarations", "AmendmentRequestNumber");
            DropColumn("Customs.Declarations", "IsPaymentProtested");
            DropColumn("Customs.ClaimsRelatedEntities", "Note");
            DropColumn("Customs.ClaimsRelatedEntities", "Explanation");
            DropColumn("Customs.ClaimsRelatedEntities", "ContinuousRequestTypeCode");
            DropColumn("Customs.Clients", "NationalIdentificationNumber");
            DropColumn("dbo.CarrierAreas", "TransportModeCode");
            DropColumn("dbo.Tips", "ShortTextCodeCode");
            DropTable("dbo.TPGFileTypes");
            DropTable("Customs.RefundCustomerActivityTypes");
            DropTable("Customs.DeclarationMamanSpecialActions");
            DropTable("Customs.PendingByKeywords");
            DropTable("Customs.MamanSpecialActionStatuses");
            DropTable("Customs.MamanSpecialActions");
            DropTable("Customs.GatepassReturnCodes");
            DropTable("Customs.UpdateCodes");
            DropTable("Customs.TransferCargoMethodTypes");
            DropTable("Customs.GatepassRequests");
            DropTable("Customs.RequestTypes");
            DropTable("Customs.DeficitDecisions");
            DropTable("Customs.DeclarationPendings");
            DropTable("Customs.DeclarationCourierStatuses");
            DropTable("Customs.DecDangersContacts");
            DropTable("dbo.CustomsPartnerFtps");
            DropTable("Customs.CustomsDocumentsDefinitions");
            DropTable("Customs.PendingErrorPlaces");
            DropTable("Customs.CourierPendingReasons");
            DropTable("Customs.HazardousSubstances");
            DropTable("Customs.ConsignmentPackDangers");
            DropTable("Customs.ClientDrivingLicenseTypes");
            DropTable("Customs.ClientDrivingLicenses");
            DropTable("Customs.SeizureMethodTypes");
            DropTable("Customs.SeizureFactorTypes");
            DropTable("Customs.ClaimsRelatedEntitiesSeizures");
            DropTable("Customs.ClaimsRelatedEntitiesRefunds");
            DropTable("Customs.ContinuousRequestTypes");
            DropTable("dbo.ApprovedProfessions");
            DropTable("Customs.AmendmentStatuses");
            AddPrimaryKey("Customs.VendorCommissions", new[] { "VendorId", "CustomerId" });
            AddPrimaryKey("Customs.DecisionTypes", "Code");
            AddPrimaryKey("dbo.CarrierAreas", "Id");
            CreateIndex("Customs.Declarations", "MamanStatusCode");
            CreateIndex("Customs.ClaimsRelatedEntities", "DecisionCode");
            CreateIndex("dbo.CarrierAreasPorts", "CarrierAreaId");
            AddForeignKey("Customs.ClaimsRelatedEntities", "DecisionCode", "dbo.DecisionTypes", "Code");
            AddForeignKey("dbo.CarrierAreasPorts", "CarrierAreaId", "dbo.CarrierAreas", "Id");
            AddForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses", "Code");
            MoveTable(name: "Customs.DecisionTypes", newSchema: "dbo");
        }
    }
}
