using System.Collections.Generic;

namespace Simplog.Server.Infrastructure.DataContracts.Models
{
    public class DigitalContactLastSettingInfo
    {
        public string CardId { get; set; }
        public string ContactId { get; set; }
        public string Entity { get; set; }
        public List<FilterCodes> FilterCodes { get; set; }
    }

    public class FilterCodes
    {
        public string FilterName { get; set; }
        public string FilterCode { get; set; }
        public bool IsChecked { get; set; }
    }
}
