
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class CustomsBookInResponseData : ResponseDataBase
    {
        public CustomsBookGeneralTables customsBookGeneralTables { get; set; }
        public string ResponseStatusXML { get; set; }
    }

    public class CustomsBookGeneralTables
    {
        public List<CustomsBookCustomsItem> CustomsItemList { get; set; }
        public List<CustomsBookCustomsItemDetailsHistory> CustomsItemDetailsHistoryList { get; set; }
        public List<CustomsBookPropertiesDetailsHistory> PropertiesDetailsHistoryList { get; set; }
    }

    public class CustomsBookCustomsItem
    {
        public int ID { get; set; }
        public int CustomsBookTypeID { get; set; }
        public string FullClassification { get; set; }
        public int CustomsItemCategoryID { get; set; }
        public int? CustomsItemHierarchicLocationID { get; set; }
        public bool CustomsItemHierarchicLocationIDSpecified { get; set; }
        public string ComputedCheckDigit { get; set; }
    }

    public class CustomsBookCustomsItemDetailsHistory
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public bool StartDateSpecified { get; set; }
        public DateTime? EndDate { get; set; }
        public bool EndDateSpecified { get; set; }
        public int EntityStatusID { get; set; }
        public int CustomsItemID { get; set; }
    }

    public class CustomsBookPropertiesDetailsHistory
    {
        public int ID { get; set; }
        public int CustomsItemID { get; set; }
        public DateTime? StartDate { get; set; }
        public bool StartDateSpecified { get; set; }
        public DateTime? EndDate { get; set; }
        public bool EndDateSpecified { get; set; }
        public int EntityStatusID { get; set; }
        public int? MeasurementUnitID { get; set; }
        public bool MeasurementUnitIDSpecified { get; set; }
    }
}