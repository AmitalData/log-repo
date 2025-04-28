using System;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class SearchIndexEditHistoryList
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
