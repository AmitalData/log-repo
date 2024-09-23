using Logitude.Customs.Data.Typesence.Models;
using Simplog.Data.Typesense;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.Typesence.Services
{
    public class RemarksCustomsBookServices : TClientBase<RemarksCustomsBook>
    {
        public RemarksCustomsBookServices(string apiKey, string url) :
            base(
                apiKey,
                url,
                "RemarksCustomsBook",
                "CustomsItemsID",
                 new Field[]
            {
                new Field("Id", FieldType.String),
                new Field("Tenant", FieldType.Int32),
                new Field("Drop_CB_ID", FieldType.String),
                new Field("CustomsItemsID", FieldType.Int32),
                new Field("RemarkDescription", FieldType.String)
            }
        )
        { }

        override public  RemarksCustomsBook DataRowToObject(DataRow row)
        {
            RemarksCustomsBook remarkCustomsBook = new RemarksCustomsBook
            {
                Id = row["Id"].ToString(),
                Tenant = GetIntDataRow(row, "Tenant"),
                Drop_CB_ID = row["Drop_CB_ID"].ToString(),
                CustomsItemsID = GetIntDataRow(row, "CustomsItemsID"),
                RemarkDescription = row["RemarkDescription"].ToString(),
            };

            return remarkCustomsBook;
        }

        public async Task<List<RemarkWithCustomsBook>> GetRemarksAndcustomsItems(string searchValue, string customsBookType)
        {
            string[] columns = new string[] { "RemarkDescription" };            
            List<RemarksCustomsBook> remarks = await SearchAsync(searchValue, columns);
            string[] customsItemsIds = remarks.Select(x => x.CustomsItemsID.ToString()).ToArray();
            List<CustomsItem> customsItems = await new CustomsBookServices(client.apiKey, client.baseUrl).SearchByCustomsItemsAsync(customsItemsIds, customsBookType);

            List<RemarkWithCustomsBook> remarksAndCustomsBook = remarks.Join(
                customsItems,
                remark => remark.CustomsItemsID.ToString(),
                customsItem => customsItem.CustomsItemID,
                (remark, customsItem) => new RemarkWithCustomsBook { Remark = remark, CustomsItem = customsItem }
            ).ToList();

            return remarksAndCustomsBook;
        }
    }
}