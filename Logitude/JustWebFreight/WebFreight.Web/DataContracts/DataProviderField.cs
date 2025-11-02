using System.Collections.Generic;

namespace WebFreight.Web.DataContracts
{
    public class DataProviderField
    {
        public DataProviderField()
        {

        }

        public string Name { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        public string Expression { get; set; }

        public bool IsChecked { get; set; }

        public int Sort { get; set; }

        public List<DataProviderField> Fields { get; set; }
        public string Translation { get; set; }
    }
}