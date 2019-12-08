using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class CourierMasterWSheetExport
    {
        public byte[] GetRepo(string courierMasterId, int tenant)
        {
            ICustomContext MyContext = CustomContext.GetContext(tenant);
            DeclarationCourierStatusListQueryService declarationCourierStatusQuery = new DeclarationCourierStatusListQueryService(MyContext);

            var q = declarationCourierStatusQuery.GetByCourierMasterId(courierMasterId, tenant)
                .Select(r => new { r.CourierMasterId, r.CourierHawb });
                ;

            DataTable dt = null;
            if (false)
            {
                dt = q.AsEnumerable().ToDataTable();
            }
            else
            {

                dt = new DataTable("Courier Master");
                dt.Columns.Add(new DataColumn()
                {
                    Caption = "Air Line",
                    ColumnName = "CourierMasterId",
                    DataType = System.Type.GetType("System.String")
                });
                dt.Columns.Add(new DataColumn()
                {
                    Caption = TextCodesTranslator.TranslateText("Customs.DeclarationCourierStatus.F.CourierHawb", tenant),
                    ColumnName = "CourierHawb",
                    DataType = System.Type.GetType("System.String")
                });
                dt.Columns.Add(new DataColumn()
                {
                    Caption = "Decimal www",
                    ColumnName = "Decimal",
                    DataType = System.Type.GetType("System.Decimal")
                });




                var l =q.ToList();
                l.ForEach(r =>
                {
                    var newrow = dt.NewRow();
                    newrow[0] = r.CourierMasterId;
                    newrow[1] = r.CourierHawb;
                    newrow[2] = "100";
                    dt.Rows.Add(newrow);

                });
            }

            var xls = new ExportToExcelHelper();
           var res= xls.ExportDataTableToExcel(dt, tenant, new BITabularViewSettings()
            {
                Columns = new List<Column>()
                 {
                     new Column()
                     {
                         Index=1,  
                         Code = "CourierMasterId",
                         Name= "CourierMasterId",
                         DataTypeCode ="String",
                          Width = 400,
                     },
                     new Column()
                     {
                         Index=2,
                         Code = "CourierHawb",
                         Name = "CourierMasterId",
                         DataTypeCode ="String",
                         Width = 100,
                     },
                                          
                    new Column()
                    {
                         Index=2,
                         Code = "Decimal",
                         Name = "Decimal",
                         DataTypeCode ="Decimal",
                         Width = 150,
                     },

                     

                 }
            });



            return res;


        }
    }
}