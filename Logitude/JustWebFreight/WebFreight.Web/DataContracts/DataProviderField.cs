using Dropbox.Api.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

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

        public List<DataProviderField> Fields { get; set; }
    }
}