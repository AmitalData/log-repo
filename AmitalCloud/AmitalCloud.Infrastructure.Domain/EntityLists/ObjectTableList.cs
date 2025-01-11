using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityLists
{
    public partial class ObjectTableList
    {
        public string HeaderScreenCode { get; set; }
        public bool IsTabsHidden { get; set; }
        public string LookUp1 { get; set; }
        public string LookUp2 { get; set; }
        public string FullNameTextCodeCode { get; set; }
    }
}