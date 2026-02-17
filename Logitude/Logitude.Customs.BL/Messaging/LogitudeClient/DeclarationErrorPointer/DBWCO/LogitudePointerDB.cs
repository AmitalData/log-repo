using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO
{
    public class LogitudePointerDB :List<LogitudePointerModel> 
    {
        static LogitudePointerDB _Instance;

        public static LogitudePointerDB Rows
        {
            get
            {

                return LogitudePointerDB._Instance = LogitudePointerDB._Instance ?? new LogitudePointerDB();
            }
        }

        LogitudePointerDB()
        {
            //Declaration
            this.Add(new LogitudePointerModel(1, "42A", WCOErrorPointerModel.LogitudeEntityEnum.Declaration , ""));
            this.Add(new LogitudePointerModel(2, "023", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AcceptanceDateTime")); //????
            this.Add(new LogitudePointerModel(5, "065", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DeclarationOfficeCode"));
            this.Add(new LogitudePointerModel(8, "D014", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DeclarationNumber"));
            this.Add(new LogitudePointerModel(10, "D011", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "IssueDateTime")); //????
            this.Add(new LogitudePointerModel(17, "D013", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "TypeCode")); //????
            this.Add(new LogitudePointerModel(1851, "01L", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DMExtensions"));
            this.Add(new LogitudePointerModel(3, "382", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "TaxationDateTime"));
            this.Add(new LogitudePointerModel(1732, "I02", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "CustomFileNo"));
            this.Add(new LogitudePointerModel(1733, "I03", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ExternalDeclarationNumber"));
            this.Add(new LogitudePointerModel(1918, "383", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "VersionID"));
            this.Add(new LogitudePointerModel(1919, "I76", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ExpenseLoadingFactor"));//????
            this.Add(new LogitudePointerModel(2013, "I90", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AutonomyRegionTypeCode"));
            this.Add(new LogitudePointerModel(2017, "I92", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "TehilaDeclarationID"));//????
            this.Add(new LogitudePointerModel(1763, "02A", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AdditionalDocument"));
            this.Add(new LogitudePointerModel(1852, "01L", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DMExtensions"));
            this.Add(new LogitudePointerModel(1915, "I74", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AttachmentID")); //????
            this.Add(new LogitudePointerModel(1765, "I06", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ExternalAttachmentID")); //????
            this.Add(new LogitudePointerModel(1870, "99A", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, ""));
            this.Add(new LogitudePointerModel(1871, "D018", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DeclarationDocumentId"));
            this.Add(new LogitudePointerModel(2014, "D019 ", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "DeclarationDocumentTypeCode"));
            this.Add(new LogitudePointerModel(26, "05A", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, ""));
            this.Add(new LogitudePointerModel(28, "R004", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "AgentId"));
            this.Add(new LogitudePointerModel(31, "R005", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "RoleCode")); // Agent.RoleCode ?????
            this.Add(new LogitudePointerModel(26, "74A", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, ""));
            this.Add(new LogitudePointerModel(28, "R038", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "ImporterId"));
            this.Add(new LogitudePointerModel(896, "R059", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, ""));
            this.Add(new LogitudePointerModel(895, "17B", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, ""));

            this.Add(new LogitudePointerModel(28, "I07", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "EntitlementTypeCode"));
            this.Add(new LogitudePointerModel(28, "D027", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "IssueLocation"));
            this.Add(new LogitudePointerModel(28, "I94", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "Name"));
            this.Add(new LogitudePointerModel(28, "I95", WCOErrorPointerModel.LogitudeEntityEnum.Declaration, "Address"));
            this.Add(new LogitudePointerModel(1544, "166", WCOErrorPointerModel.LogitudeEntityEnum.None, "ProcedureCurrentCode"));

            //DeclarationTaxes
            this.Add(new LogitudePointerModel(128, "50A", WCOErrorPointerModel.LogitudeEntityEnum.DeclarationTaxes, ""));
            this.Add(new LogitudePointerModel(136, "113", WCOErrorPointerModel.LogitudeEntityEnum.None, "TaxTypeCode"));
            this.Add(new LogitudePointerModel(1921, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1926, "04L", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1927, "I48", WCOErrorPointerModel.LogitudeEntityEnum.None, "TotalAmount"));
            this.Add(new LogitudePointerModel(1928, "I49", WCOErrorPointerModel.LogitudeEntityEnum.None, "DeferredTaxAmount"));

            //GoodsShipment
            this.Add(new LogitudePointerModel(148, "67A", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, ""));
            this.Add(new LogitudePointerModel(275, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(155, "02A", WCOErrorPointerModel.LogitudeEntityEnum.None, "AdditionalDocument"));
            this.Add(new LogitudePointerModel(1854, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1350, "78A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1351, "D016", WCOErrorPointerModel.LogitudeEntityEnum.None, "InvoiceNumber"));
            this.Add(new LogitudePointerModel(1352, "D015", WCOErrorPointerModel.LogitudeEntityEnum.None, "IssueDate"));
            this.Add(new LogitudePointerModel(1354, "D025", WCOErrorPointerModel.LogitudeEntityEnum.None, "AccountTypeCode"));
            this.Add(new LogitudePointerModel(1868, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1788, "I28", WCOErrorPointerModel.LogitudeEntityEnum.None, "IsPreference"));
            this.Add(new LogitudePointerModel(1789, "I29", WCOErrorPointerModel.LogitudeEntityEnum.None, "PreferenceDocumentTypeCode"));
            this.Add(new LogitudePointerModel(1790, "I30", WCOErrorPointerModel.LogitudeEntityEnum.None, "PaymentTypeCode"));
            this.Add(new LogitudePointerModel(1791, "I31", WCOErrorPointerModel.LogitudeEntityEnum.None, "InvoiceAmount"));
            this.Add(new LogitudePointerModel(1793, "I33", WCOErrorPointerModel.LogitudeEntityEnum.None, "ActualPayedAmount"));
            this.Add(new LogitudePointerModel(1794, "I34", WCOErrorPointerModel.LogitudeEntityEnum.None, "InvoiceLineQuantity")); //??? Invoice.DMExtensions.InvoiceLineQuantity
            this.Add(new LogitudePointerModel(1489, "18B", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1490, "R053", WCOErrorPointerModel.LogitudeEntityEnum.None, "VendorId"));
            this.Add(new LogitudePointerModel(1511, "22B", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1512, "090", WCOErrorPointerModel.LogitudeEntityEnum.None, "IncotermCode"));
            this.Add(new LogitudePointerModel(1515, "L002", WCOErrorPointerModel.LogitudeEntityEnum.None, "IssueCountryCode"));
            this.Add(new LogitudePointerModel(1517, "35B", WCOErrorPointerModel.LogitudeEntityEnum.None, "")); // UCR ????
            this.Add(new LogitudePointerModel(1518, "016", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1519, "009", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            //CustomsValuation
            this.Add(new LogitudePointerModel(557, "41A", WCOErrorPointerModel.LogitudeEntityEnum.CustomsValuation, "")); // ??? CustomsValuation - to do - dif tables????
            this.Add(new LogitudePointerModel(558, "371", WCOErrorPointerModel.LogitudeEntityEnum.None, "TypeCode"));
            this.Add(new LogitudePointerModel(563, "181", WCOErrorPointerModel.LogitudeEntityEnum.None, "Amount"));

            //GoodsItem
            this.Add(new LogitudePointerModel(627, "68A", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, ""));
            this.Add(new LogitudePointerModel(632, "006", WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(1859, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(712, "23A", WCOErrorPointerModel.LogitudeEntityEnum.None, "Commodity"));
            this.Add(new LogitudePointerModel(1913, "05L", WCOErrorPointerModel.LogitudeEntityEnum.None, "GoodsItemAmount"));
            this.Add(new LogitudePointerModel(634, "02A", WCOErrorPointerModel.LogitudeEntityEnum.None, "AdditionalDocument"));
            this.Add(new LogitudePointerModel(842, "21A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1781, "I13", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1223, "65A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1226, "130", WCOErrorPointerModel.LogitudeEntityEnum.None, "InvoiceQuantity")); 
            this.Add(new LogitudePointerModel(1865, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "")); 
            this.Add(new LogitudePointerModel(1753, "I17", WCOErrorPointerModel.LogitudeEntityEnum.None, "MeasureQualifier"));
            this.Add(new LogitudePointerModel(628, "108", WCOErrorPointerModel.LogitudeEntityEnum.None, "CustomsValueAmount"));
            this.Add(new LogitudePointerModel(1914, "I15", WCOErrorPointerModel.LogitudeEntityEnum.None, "AmountType"));
            
            this.Add(new LogitudePointerModel(1232, "86A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1233, "R043", WCOErrorPointerModel.LogitudeEntityEnum.None, "ManufactureIdentifier"));
            this.Add(new LogitudePointerModel(1250, "92A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(1251, "063", WCOErrorPointerModel.LogitudeEntityEnum.None, "OriginCountryCode"));
            this.Add(new LogitudePointerModel(843, "145", WCOErrorPointerModel.LogitudeEntityEnum.None, "ClassificationCode"));

            //Consignment
            this.Add(new LogitudePointerModel(269, "28A", WCOErrorPointerModel.LogitudeEntityEnum.Consignment, ""));
            this.Add(new LogitudePointerModel(275, "006", WCOErrorPointerModel.LogitudeEntityEnum.None, "SequenceNumeric"));
            this.Add(new LogitudePointerModel(1856, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1731, "138", WCOErrorPointerModel.LogitudeEntityEnum.None, "CargoDescription"));
            this.Add(new LogitudePointerModel(1849, "I05", WCOErrorPointerModel.LogitudeEntityEnum.None, "IsLastReleaseFromWarehous"));
            this.Add(new LogitudePointerModel(151, "062", WCOErrorPointerModel.LogitudeEntityEnum.None, "OriginCountryCode"));
            this.Add(new LogitudePointerModel(370, "83A", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(371, "L010", WCOErrorPointerModel.LogitudeEntityEnum.None, "LoadingPortCode"));
            this.Add(new LogitudePointerModel(430, "30B", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(431, "D023", WCOErrorPointerModel.LogitudeEntityEnum.None, "ManifestNumber"));
            this.Add(new LogitudePointerModel(433, "D024", WCOErrorPointerModel.LogitudeEntityEnum.None, "CargoTypeCode"));
            this.Add(new LogitudePointerModel(432, "D020", WCOErrorPointerModel.LogitudeEntityEnum.None, "ManifestDate"));
            this.Add(new LogitudePointerModel(1858, "01L", WCOErrorPointerModel.LogitudeEntityEnum.None, "DMExtensions"));
            this.Add(new LogitudePointerModel(1846, "I11", WCOErrorPointerModel.LogitudeEntityEnum.None, "SecondCargoID"));
            this.Add(new LogitudePointerModel(1847, "I12", WCOErrorPointerModel.LogitudeEntityEnum.None, "ThirdCargoID"));
            this.Add(new LogitudePointerModel(512, "38B", WCOErrorPointerModel.LogitudeEntityEnum.None, ""));
            this.Add(new LogitudePointerModel(513, "172", WCOErrorPointerModel.LogitudeEntityEnum.None, "UnloadDate"));
            this.Add(new LogitudePointerModel(514, "L013", WCOErrorPointerModel.LogitudeEntityEnum.None, "UnloadPortCode"));

            this.Add(new LogitudePointerModel(312, "R014", WCOErrorPointerModel.LogitudeEntityEnum.None, "ImporterId"));
            this.Add(new LogitudePointerModel(318, "239", WCOErrorPointerModel.LogitudeEntityEnum.None, "ImporterAddress"));
            this.Add(new LogitudePointerModel(500, "R020", WCOErrorPointerModel.LogitudeEntityEnum.None, "CasualSupplierName"));
            this.Add(new LogitudePointerModel(509, "239", WCOErrorPointerModel.LogitudeEntityEnum.None, "CasualSupplierAddress"));
            this.Add(new LogitudePointerModel(209, "L011", WCOErrorPointerModel.LogitudeEntityEnum.None, "LoadingPortCode"));
            this.Add(new LogitudePointerModel(560, "098", WCOErrorPointerModel.LogitudeEntityEnum.None, "IncotermCode"));
            this.Add(new LogitudePointerModel(569, "66A", WCOErrorPointerModel.LogitudeEntityEnum.None, "StorageSiteCode"));
            this.Add(new LogitudePointerModel(2045, "105", WCOErrorPointerModel.LogitudeEntityEnum.None, "ThirdCargoID"));
            this.Add(new LogitudePointerModel(809, "173", WCOErrorPointerModel.LogitudeEntityEnum.None, "UnloadDate"));

           //ConsignmentPackages
            this.Add(new LogitudePointerModel(1783, "15L", WCOErrorPointerModel.LogitudeEntityEnum.ConsignmentPackages, ""));
            this.Add(new LogitudePointerModel(1785, "I10", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageMeasureQualifierCode"));
            this.Add(new LogitudePointerModel(1786, "146", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageQuantity"));
            this.Add(new LogitudePointerModel(1787, "126", WCOErrorPointerModel.LogitudeEntityEnum.None, "GrossMassMeasure"));
            this.Add(new LogitudePointerModel(1744, "141", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageTypeCode"));
            this.Add(new LogitudePointerModel(1988, "142", WCOErrorPointerModel.LogitudeEntityEnum.None, "MarksNumbers"));
            this.Add(new LogitudePointerModel(1784, "006", WCOErrorPointerModel.LogitudeEntityEnum.None, "LineNumber"));
            this.Add(new LogitudePointerModel(438, "144", WCOErrorPointerModel.LogitudeEntityEnum.None, "PackageQuantityTy"));

            //ConsignmentInternalTransitions
            this.Add(new LogitudePointerModel(1777, "05B", WCOErrorPointerModel.LogitudeEntityEnum.ConsignmentInternalTransitions, ""));
            this.Add(new LogitudePointerModel(1779, "L086", WCOErrorPointerModel.LogitudeEntityEnum.None, "StorageSiteCode"));
            this.Add(new LogitudePointerModel(1780, "L047", WCOErrorPointerModel.LogitudeEntityEnum.None, "StorageSiteCode"));

        }
    }
}
