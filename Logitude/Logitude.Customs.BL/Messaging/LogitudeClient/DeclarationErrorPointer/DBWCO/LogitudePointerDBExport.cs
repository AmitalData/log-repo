using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO
{
    public class LogitudePointerDBExport : List<LogitudePointerModel>
    {
        static LogitudePointerDBExport _Instance;

        public static LogitudePointerDBExport Rows
        {
            get
            {

                return LogitudePointerDBExport._Instance = LogitudePointerDBExport._Instance ?? new LogitudePointerDBExport();
            }
        }

        LogitudePointerDBExport()
        {

            //Declaration
            this.Add(new LogitudePointerModel(1, "42A", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, ""));
            //this.Add(new LogitudePointerModel(2, "023", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AcceptanceDateTime")); 
            this.Add(new LogitudePointerModel(4, "065", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DeclarationOfficeCode"));
            this.Add(new LogitudePointerModel(2061, "065", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ExportDeclarationOfficeCode"));
            //70A ,05A,57A ,R032?,01L?,R005?,01L,44A,I117



            this.Add(new LogitudePointerModel(7, "D014", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DeclarationNumber"));
            this.Add(new LogitudePointerModel(009, "D011", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "CreateDateTime"));
            this.Add(new LogitudePointerModel(14, "D013", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "TypeCode"));
            this.Add(new LogitudePointerModel(939, "166", WCOErrorPointerModel.LogitudeEntityEnum.None, "ProcedureCurrentCode"));
            this.Add(new LogitudePointerModel(69, "R004", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AgentId"));
            this.Add(new LogitudePointerModel(72, "R005", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AgentRoleCode"));
            this.Add(new LogitudePointerModel(179, "R032", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ImporterCodeTransferImporterCode"));
            this.Add(new LogitudePointerModel(1070, "D027", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ImporterPassCountryCode"));
            this.Add(new LogitudePointerModel(2093, "I94", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ImporterName"));
            this.Add(new LogitudePointerModel(2094, "I95", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ImporterAddress"));
            this.Add(new LogitudePointerModel(1201, "383", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "VersionID"));
            this.Add(new LogitudePointerModel(1196, "382", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "TaxationDateTime"));
            this.Add(new LogitudePointerModel(2055, "277", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "HatraDate"));
            this.Add(new LogitudePointerModel(1063, "I02", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "CustomFileNo"));
            this.Add(new LogitudePointerModel(1064, "I03", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ExternalDeclarationNumber"));
            this.Add(new LogitudePointerModel(2092, "I116", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "TransshipmentApprovalDateTime"));
            this.Add(new LogitudePointerModel(1177, "I42", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DestinationCountryCode"));
            this.Add(new LogitudePointerModel(2054, "I110", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "IsExporterConfirmation"));
            this.Add(new LogitudePointerModel(2013, "I90", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ExportAutonomyRegionTypeCode"));
            //DeclarationExportRecipients
            this.Add(new LogitudePointerModel(2035, "I101", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "RecipientName"));
            this.Add(new LogitudePointerModel(2036, "I102", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "RecipientAddress"));
            this.Add(new LogitudePointerModel(2037, "242", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "RecipientIssueCountryCode"));
            //
            //"ExportDecalarationClosingData "
            this.Add(new LogitudePointerModel(2031, "T006", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "LoadingDateTime"));
            this.Add(new LogitudePointerModel(2030, "169", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "FinalShipCode"));
            this.Add(new LogitudePointerModel(2096, "L010", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "FinalLoadingSite"));
            this.Add(new LogitudePointerModel(2029, "004", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "FinalManifestNumber"));
            this.Add(new LogitudePointerModel(2051, "D024", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "FainalCargoTypeCode"));
            this.Add(new LogitudePointerModel(2052, "I11", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "FinalSecondCargoId"));
            this.Add(new LogitudePointerModel(2053, "I12", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "FinalThirdCargoId"));
            //
            this.Add(new LogitudePointerModel(2045, "I76", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ExpenseLoadingFactor"));
            this.Add(new LogitudePointerModel(2025, "I114", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "FOBValueNIS"));
            this.Add(new LogitudePointerModel(2060, "I115", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "FOBValueDollar"));
            this.Add(new LogitudePointerModel(2027, "120", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "TotalTax"));
            this.Add(new LogitudePointerModel(15, "02A", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AdditionalDocument"));
            this.Add(new LogitudePointerModel(1183, "01L", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DMExtensions"));
            this.Add(new LogitudePointerModel(1191, "I06", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ExternalAttachmentID"));
            this.Add(new LogitudePointerModel(951, "99A", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, ""));
            this.Add(new LogitudePointerModel(952, "D018", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DeclarationDocumentId"));
            this.Add(new LogitudePointerModel(954, "D019 ", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DeclarationDocumentTypeCode"));

            //DeclarationTaxes
            this.Add(new LogitudePointerModel(657, "50A", WCOErrorPointerModel.LogitudeEntityEnum.DeclarationTaxes, ""));
            this.Add(new LogitudePointerModel(167, "116", WCOErrorPointerModel.LogitudeEntityEnum.DeclarationTaxes, "TaxBaseAmount"));
            this.Add(new LogitudePointerModel(171, "113", WCOErrorPointerModel.LogitudeEntityEnum.None, "TaxTypeCode"));
            this.Add(new LogitudePointerModel(1203, "01L", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DMExtensions"));
            this.Add(new LogitudePointerModel(1208, "04L", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1209, "I48", WCOErrorPointerModel.LogitudeEntityEnum.None, "TotalAmount"));
            //GoodsShipment
            this.Add(new LogitudePointerModel(196, "67A", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, ""));
            this.Add(new LogitudePointerModel(201, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(831, "78A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(832, "D016", WCOErrorPointerModel.LogitudeEntityEnum.None, "InvoiceNumber"));
            this.Add(new LogitudePointerModel(833, "D015", WCOErrorPointerModel.LogitudeEntityEnum.None, "IssueDate"));
            this.Add(new LogitudePointerModel(835, "D025", WCOErrorPointerModel.LogitudeEntityEnum.None, "AccountTypeCode"));
            this.Add(new LogitudePointerModel(1097, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(460, "163", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "PartyRelationshipCode"));
            this.Add(new LogitudePointerModel(1099, "I28", WCOErrorPointerModel.LogitudeEntityEnum.None, "IsPreference"));
            this.Add(new LogitudePointerModel(1100, "I29", WCOErrorPointerModel.LogitudeEntityEnum.None, "PreferenceDocumentTypeCode"));
            this.Add(new LogitudePointerModel(2102, "I119", WCOErrorPointerModel.LogitudeEntityEnum.None, "DutyRegimeProtocolCode"));
            this.Add(new LogitudePointerModel(1102, "I31", WCOErrorPointerModel.LogitudeEntityEnum.None, "InvoiceAmount"));
            this.Add(new LogitudePointerModel(2023, "118", WCOErrorPointerModel.LogitudeEntityEnum.None, "ExchangeRate"));
            this.Add(new LogitudePointerModel(1111, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(1101, "I30", WCOErrorPointerModel.LogitudeEntityEnum.None, "PaymentType"));
            this.Add(new LogitudePointerModel(2049, "I108", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "PaymentAmount"));
            this.Add(new LogitudePointerModel(2047, "44A", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, ""));
            this.Add(new LogitudePointerModel(2042, "I94", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "BuyerName"));
            this.Add(new LogitudePointerModel(2043, "I95", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "BuyerAddress"));
            this.Add(new LogitudePointerModel(2044, "242", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "BuyerCountryCode"));
            this.Add(new LogitudePointerModel(1957, "R005", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "BuyerRoleCode"));
            this.Add(new LogitudePointerModel(2100, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(927, "22B", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(928, "90", WCOErrorPointerModel.LogitudeEntityEnum.None, "IncotermCode"));
            this.Add(new LogitudePointerModel(203, "02A", WCOErrorPointerModel.LogitudeEntityEnum.None, "AdditionalDocument"));
            this.Add(new LogitudePointerModel(1184, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1195, "I06", WCOErrorPointerModel.LogitudeEntityEnum.None, "ExternalAttachmentID"));
            this.Add(new LogitudePointerModel(933, "35B", WCOErrorPointerModel.LogitudeEntityEnum.None, "")); 
            this.Add(new LogitudePointerModel(934, "16", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "SupplierChargeID")); 
            this.Add(new LogitudePointerModel(935, "009", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "AgentChargeID")); 


            //CustomsValuation
            this.Add(new LogitudePointerModel(458, "41A", WCOErrorPointerModel.LogitudeEntityEnum.CustomsValuation, "")); 
            this.Add(new LogitudePointerModel(1112, "371", WCOErrorPointerModel.LogitudeEntityEnum.None, "TypeCode"));
            this.Add(new LogitudePointerModel(1115, "181", WCOErrorPointerModel.LogitudeEntityEnum.None, "Amount"));
            //Consignment
            this.Add(new LogitudePointerModel(264, "28A", WCOErrorPointerModel.LogitudeEntityEnum.Consignment, ""));
            this.Add(new LogitudePointerModel(267, "006", WCOErrorPointerModel.LogitudeEntityEnum.None, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(409, "30B", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(410, "D023", WCOErrorPointerModel.LogitudeEntityEnum.None, "ManifestNumber"));
            this.Add(new LogitudePointerModel(411, "D024", WCOErrorPointerModel.LogitudeEntityEnum.None, "CargoTypeCode"));
            this.Add(new LogitudePointerModel(1080, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1081, "I11", WCOErrorPointerModel.LogitudeEntityEnum.None, "SecondCargoID"));
            this.Add(new LogitudePointerModel(1082, "I12", WCOErrorPointerModel.LogitudeEntityEnum.None, "ThirdCargoID"));
            this.Add(new LogitudePointerModel(380, "83A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(382, "L010", WCOErrorPointerModel.LogitudeEntityEnum.None, "ExportLoadingPortCode"));
            this.Add(new LogitudePointerModel(434, "38B", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(435, "L013", WCOErrorPointerModel.LogitudeEntityEnum.None, "ExportUnloadPortCode"));
            this.Add(new LogitudePointerModel(1084, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(2039, "I111", WCOErrorPointerModel.LogitudeEntityEnum.Consignment, "IsDangerousGoods"));
            this.Add(new LogitudePointerModel(1086, "138", WCOErrorPointerModel.LogitudeEntityEnum.None, "CargoDescription"));
            this.Add(new LogitudePointerModel(1178, "L013", WCOErrorPointerModel.LogitudeEntityEnum.None, "FinalDestinationPortCode"));
            this.Add(new LogitudePointerModel(2097, "T006", WCOErrorPointerModel.LogitudeEntityEnum.None, "ShipCode"));
            this.Add(new LogitudePointerModel(1095, "L086", WCOErrorPointerModel.LogitudeEntityEnum.Consignment, "ExportRecieverWareHouseCode"));
            this.Add(new LogitudePointerModel(2062, "28A", WCOErrorPointerModel.LogitudeEntityEnum.Consignment, ""));
            this.Add(new LogitudePointerModel(2063, "006", WCOErrorPointerModel.LogitudeEntityEnum.None, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(2065, "138", WCOErrorPointerModel.LogitudeEntityEnum.None, "CargoDescription"));
            this.Add(new LogitudePointerModel(2066, "I05", WCOErrorPointerModel.LogitudeEntityEnum.None, "IsLastReleaseFromWarehous"));
            this.Add(new LogitudePointerModel(2067, "062", WCOErrorPointerModel.LogitudeEntityEnum.None, "OriginalCountryCode"));
            this.Add(new LogitudePointerModel(2099, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(2069, "006", WCOErrorPointerModel.LogitudeEntityEnum.None, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(2071, "146", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageMeasureQualifierCode"));
            this.Add(new LogitudePointerModel(2072, "126", WCOErrorPointerModel.LogitudeEntityEnum.None, "GrossMassMeasure"));
            this.Add(new LogitudePointerModel(2073, "141", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageTypeCode"));
            this.Add(new LogitudePointerModel(2074, "142", WCOErrorPointerModel.LogitudeEntityEnum.None, "MarksNumbers"));
            this.Add(new LogitudePointerModel(2082, "D032", WCOErrorPointerModel.LogitudeEntityEnum.None, "MamifestNumber"));
            this.Add(new LogitudePointerModel(2083, "D024", WCOErrorPointerModel.LogitudeEntityEnum.None, "CargoTypeCode"));
            this.Add(new LogitudePointerModel(2086, "I11", WCOErrorPointerModel.LogitudeEntityEnum.None, "SecondCargoID"));
            this.Add(new LogitudePointerModel(2087, "I12", WCOErrorPointerModel.LogitudeEntityEnum.None, "ThirdCargoID"));


            //ConsignmentPackages
            this.Add(new LogitudePointerModel(1087, "15L", WCOErrorPointerModel.LogitudeEntityEnum.ConsignmentPackages, ""));
            this.Add(new LogitudePointerModel(1088, "006", WCOErrorPointerModel.LogitudeEntityEnum.ConsignmentPackages, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(1089, "I10", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageMeasureQualifierCode"));
            this.Add(new LogitudePointerModel(1090, "126", WCOErrorPointerModel.LogitudeEntityEnum.None, "GrossMassMeasure"));
            this.Add(new LogitudePointerModel(1091, "146", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageQuantity"));
            this.Add(new LogitudePointerModel(1092, "141", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageTypeCode"));
            this.Add(new LogitudePointerModel(1998, "142", WCOErrorPointerModel.LogitudeEntityEnum.None, "MarksNumbers"));
            this.Add(new LogitudePointerModel(1094, "006", WCOErrorPointerModel.LogitudeEntityEnum.None, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(2064, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));

            //ConsignmentInternalTransitions
            this.Add(new LogitudePointerModel(1093, "05B", WCOErrorPointerModel.LogitudeEntityEnum.ConsignmentInternalTransitions, ""));
            this.Add(new LogitudePointerModel(1096, "L047", WCOErrorPointerModel.LogitudeEntityEnum.None, "StorageSiteCode"));
            this.Add(new LogitudePointerModel(2077, "L086", WCOErrorPointerModel.LogitudeEntityEnum.None, "RecieverWareHouseCode"));
            this.Add(new LogitudePointerModel(2098, "I118", WCOErrorPointerModel.LogitudeEntityEnum.None, "SiteCode"));
            //GoodsItem
            this.Add(new LogitudePointerModel(503, "68A", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, ""));
            this.Add(new LogitudePointerModel(504, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(764, "166", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "ProcedureCurrentCode"));
            this.Add(new LogitudePointerModel(558, "23A", WCOErrorPointerModel.LogitudeEntityEnum.None, "Commodity"));
            this.Add(new LogitudePointerModel(631, "21A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(632, "145", WCOErrorPointerModel.LogitudeEntityEnum.None, "ClassificationCode"));
            this.Add(new LogitudePointerModel(633, "337", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "ClassificationTypeCode"));
            this.Add(new LogitudePointerModel(2006, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(2012, "164", WCOErrorPointerModel.LogitudeEntityEnum.None, "TradeAgreemenCode"));
            this.Add(new LogitudePointerModel(1197, "I14", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1125, "258", WCOErrorPointerModel.LogitudeEntityEnum.None, "Description"));
            this.Add(new LogitudePointerModel(1126, "335", WCOErrorPointerModel.LogitudeEntityEnum.None, "TypeCode"));
            this.Add(new LogitudePointerModel(1156, "338", WCOErrorPointerModel.LogitudeEntityEnum.None, "TypeCode"));
            this.Add(new LogitudePointerModel(1134, "147", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "SerialNumber"));
            this.Add(new LogitudePointerModel(1135, "347", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "TypeCode"));
            this.Add(new LogitudePointerModel(1137, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(1138, "D006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "StatementType"));
            this.Add(new LogitudePointerModel(1954, "I80", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "StatementInd"));
            this.Add(new LogitudePointerModel(1961, "I85", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "TradeLevyNumber"));
            this.Add(new LogitudePointerModel(1962, "I86", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "TradeLevyExamptCode"));
            this.Add(new LogitudePointerModel(658, "116", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "TaxBaseAmount"));
            this.Add(new LogitudePointerModel(661, "115", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "TaxRate"));
            this.Add(new LogitudePointerModel(662, "113", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "TaxTypeCode"));
            this.Add(new LogitudePointerModel(1223, "I48", WCOErrorPointerModel.LogitudeEntityEnum.None, "TaxAmount"));
            this.Add(new LogitudePointerModel(1224, "I49", WCOErrorPointerModel.LogitudeEntityEnum.None, "DeferredTaxAmount"));
            this.Add(new LogitudePointerModel(1226, "I51", WCOErrorPointerModel.LogitudeEntityEnum.None, "DefinedPerUnitMeasure"));
            this.Add(new LogitudePointerModel(1227, "I52", WCOErrorPointerModel.LogitudeEntityEnum.None, "AlternateRate"));
            this.Add(new LogitudePointerModel(1228, "I53", WCOErrorPointerModel.LogitudeEntityEnum.None, "AlternateDefinedPerUnitMeasure"));
            this.Add(new LogitudePointerModel(2021, "I96", WCOErrorPointerModel.LogitudeEntityEnum.None, "DefinedPerUnitQuantity"));
            this.Add(new LogitudePointerModel(1229, "I54", WCOErrorPointerModel.LogitudeEntityEnum.None, "MeasurementUnitCode"));
            this.Add(new LogitudePointerModel(2022, "I97", WCOErrorPointerModel.LogitudeEntityEnum.None, "AlternateDefinedPerUnitQuantity"));
            this.Add(new LogitudePointerModel(1230, "I55", WCOErrorPointerModel.LogitudeEntityEnum.None, "AlternateMeasurementUnitCode"));
            this.Add(new LogitudePointerModel(2008, "I85", WCOErrorPointerModel.LogitudeEntityEnum.None, "TradeLevyNumber"));
            this.Add(new LogitudePointerModel(1161, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(2058, "I112", WCOErrorPointerModel.LogitudeEntityEnum.None, "ItemFOBAmountForeign"));
            this.Add(new LogitudePointerModel(2059, "I113", WCOErrorPointerModel.LogitudeEntityEnum.None, "ItemFOBAmountNIS"));
            this.Add(new LogitudePointerModel(759, "65A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(762, "130", WCOErrorPointerModel.LogitudeEntityEnum.None, "TariffQuantity"));
            this.Add(new LogitudePointerModel(1160, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1162, "I17", WCOErrorPointerModel.LogitudeEntityEnum.None, "MeasureQualifier"));
            this.Add(new LogitudePointerModel(506, "02A", WCOErrorPointerModel.LogitudeEntityEnum.None, "AdditionalDocument"));
            this.Add(new LogitudePointerModel(508, "D005", WCOErrorPointerModel.LogitudeEntityEnum.None, "CertificateNumber"));
            this.Add(new LogitudePointerModel(512, "D006", WCOErrorPointerModel.LogitudeEntityEnum.None, "AttachmentTypeCode"));
            this.Add(new LogitudePointerModel(1057, "360", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "CertificateExemptionTypeCode"));
            this.Add(new LogitudePointerModel(1963, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1131, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(1958, "I82", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "ResponseConfirmationTypeCode"));
            this.Add(new LogitudePointerModel(2005, "I89", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "RequestConfirmationTypeCode"));
            this.Add(new LogitudePointerModel(1116, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1120, "103", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "TransactionNature"));
            this.Add(new LogitudePointerModel(1176, "I41", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "ClaimReason"));
            this.Add(new LogitudePointerModel(2033, "I27", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "ActualInvoiceLines"));
            this.Add(new LogitudePointerModel(2032, "I04", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "PreferenceDocumentNumber"));
            this.Add(new LogitudePointerModel(1198, "05L", WCOErrorPointerModel.LogitudeEntityEnum.None, "GoodsItemAmount"));
            this.Add(new LogitudePointerModel(1199, "108", WCOErrorPointerModel.LogitudeEntityEnum.None, "ItemPriceAdditionalPrice"));
            this.Add(new LogitudePointerModel(1200, "I15", WCOErrorPointerModel.LogitudeEntityEnum.None, "AdditionalPriceType"));
            this.Add(new LogitudePointerModel(1167, "188", WCOErrorPointerModel.LogitudeEntityEnum.None, "TypeCode"));
            this.Add(new LogitudePointerModel(1168, "125", WCOErrorPointerModel.LogitudeEntityEnum.None, "Amount"));
            this.Add(new LogitudePointerModel(1170, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(2001, "147", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "IdentifierIDב"));
            this.Add(new LogitudePointerModel(2002, "347", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "VehicleTypeCode"));
            this.Add(new LogitudePointerModel(792, "D018", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "DeclarationNumber"));
            this.Add(new LogitudePointerModel(1171, "171", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "ItemSequence"));
            this.Add(new LogitudePointerModel(794, "D019", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "DeclarationTypeCode"));
            this.Add(new LogitudePointerModel(1174, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(2015, "006", WCOErrorPointerModel.LogitudeEntityEnum.None, "AccountNumber"));
            this.Add(new LogitudePointerModel(1173, "I19", WCOErrorPointerModel.LogitudeEntityEnum.None, "QuantityType"));
            this.Add(new LogitudePointerModel(784, "92A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(785, "063", WCOErrorPointerModel.LogitudeEntityEnum.None, "OriginCountryCode"));







           



        }


    }
}
