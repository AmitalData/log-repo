using System;

namespace Logitude.Customs.Data.Typesence.Models
{
    public class CustomsItem
    {
        private long startDateInt;
        private long endDateInt;

        public string CB_ID { get; set; }
        public int ID { get; set; }
        public string CustomsItemID { get; set; }
        public string FullClassification { get; set; }
        public bool IsLeaf { get; set; }
        public int CustomsItemDetailsHistoryID { get; set; }
        public int PropertiesDetailsHistoryID { get; set; }
        public int PH_MeasurementUnitID { get; set; }
        public bool IsHistoryExists { get; set; }
        public bool IsRulesExists { get; set; }
        public DateTime? StartDate { get; set; }
        public long StartDateInt
        {
            get => startDateInt;
            set
            {
                StartDate = new DateTime(value);
                startDateInt = value;
            }
        }
        public DateTime? EndDate { get; set; }
        public long EndDateInt 
        { 
            get => endDateInt; 
            set
            {
                EndDate = new DateTime(value);
                endDateInt = value; 
            }
        }
        public int CI_Parent_CustomsItemIDNum { get; set; }
        public string CI_BaseFullClassification { get; set; }
        public string CI_ComputedCheckDigit { get; set; }
        public string CI_CustomsBookTypeIDNum { get; set; }
        public string CI_CustomsItemCategoryIDNum { get; set; }
        public string ItemHierarchicLocationID { get; set; }
        public string CIH_Title { get; set; }
        public string CIH_GoodsDescription { get; set; }
        public int CustomsItemEntityStatusIDNum { get; set; }
        public bool PH_IsCarItem { get; set; }
        public string FullGoodsDescription { get; set; }
    }
}