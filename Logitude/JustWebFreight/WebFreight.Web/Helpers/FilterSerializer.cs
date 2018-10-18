using Simplog.Server.Infrastructure.DataContracts;
using System.IO;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.Helpers
{
    public class FilterSerializer
    {
        public byte[] SerializeFilterItems(QueryOperations filterItems)
        {
            MemoryStream memstream = new MemoryStream();

            XmlSerializer ser = new XmlSerializer(typeof(QueryOperations));


            ser.Serialize(memstream, filterItems);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;

        }
    }
}