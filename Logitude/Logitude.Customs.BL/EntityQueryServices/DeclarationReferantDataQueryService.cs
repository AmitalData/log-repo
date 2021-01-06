 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.DataContracts;

namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class DeclarationReferantDataQueryService: EntityQueryService<DeclarationReferantData,DeclarationReferantDataKeys,DeclarationReferantDataPM,object,DeclarationReferantDataKeys>
   {

        public List<DeclarationReferantDataChartingClass> GetDeclarationReferantDataDashBoard(int tenant)
        {
            List<DeclarationReferantDataChartingClass> myResult = new List<DeclarationReferantDataChartingClass>();
            ICustomContext context = MainContext as ICustomContext;

            IQueryable<DeclarationReferantData> dataSourceQuery =
                (from d in context.DeclarationReferantDatas
                 where d.Tenant == tenant
                  select d);

            if (dataSourceQuery != null)
            {
                IQueryable<DeclarationReferantData> data_WAT = dataSourceQuery;//.Where(d => d.BookingStatusCode == "CRT");
                IQueryable<DeclarationReferantData> data_WAC = dataSourceQuery;//.Where(d => d.WaitingForResponse);
                IQueryable<DeclarationReferantData> data_CNF = dataSourceQuery;//.Where(d => d.BookingStatusCode == "CNF");
                IQueryable<DeclarationReferantData> data_ERR = dataSourceQuery;//.Where(d => d.HasErrors);

                List<DeclarationReferantDataChartingClass> myData = new List<DeclarationReferantDataChartingClass>();

             
                    myResult.Add(new DeclarationReferantDataChartingClass()
                    {
                        Id = "WAT",
                        DataTypeCode = "WAT",
                        StringProperty = "aaa",
                        IntegerProperty = data_WAT.Count(),
                       // MainCarriageCarrierId = item.Id,
                    });

                myResult.Add(new DeclarationReferantDataChartingClass()
                {
                    Id = "WAT1",
                    DataTypeCode = "WAT",
                    StringProperty = "aaa",
                    IntegerProperty = data_WAT.Count(),
                    // MainCarriageCarrierId = item.Id,
                });

                myResult.Add(new DeclarationReferantDataChartingClass()
                    {
                        Id ="WAC",
                        DataTypeCode = "WAC",
                        StringProperty = "bbb",
                        IntegerProperty = data_WAC.Count(),
                      //  MainCarriageCarrierId = item.Id,
                    });

                    myResult.Add(new DeclarationReferantDataChartingClass()
                    {
                        Id = "CNF",
                        DataTypeCode = "CNF",
                        StringProperty = "ccc",
                        IntegerProperty = data_CNF.Count(),
                      //  MainCarriageCarrierId = item.Id,
                    });

                    myResult.Add(new DeclarationReferantDataChartingClass()
                    {
                        Id = "ERR",
                        DataTypeCode = "ERR",
                        StringProperty = "ddd",
                        IntegerProperty = data_ERR.Count(),
                      //  MainCarriageCarrierId = item.Id,
                    });
               
            }

            return myResult;
        }


    }

}
	 