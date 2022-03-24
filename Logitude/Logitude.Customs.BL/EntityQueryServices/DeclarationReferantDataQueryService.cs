 
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
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Data.Entity;

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

             
                    //myResult.Add(new DeclarationReferantDataChartingClass()
                    //{
                    //    Id = "WAT",
                    //    DataTypeCode = "WAT",
                    //    StringProperty = "aaa",
                    //    IntegerProperty = data_WAT.Count(),
                    //   // MainCarriageCarrierId = item.Id,
                    //});

                //myResult.Add(new DeclarationReferantDataChartingClass()
                //{
                //    Id = "WAT1",
                //    DataTypeCode = "WAT",
                //    StringProperty = "aaa",
                //    IntegerProperty = data_WAT.Count(),
                //    // MainCarriageCarrierId = item.Id,
                //});

                //myResult.Add(new DeclarationReferantDataChartingClass()
                //    {
                //        Id ="WAC",
                //        DataTypeCode = "WAC",
                //        StringProperty = "bbb",
                //        IntegerProperty = data_WAC.Count(),
                //      //  MainCarriageCarrierId = item.Id,
                //    });

                //    myResult.Add(new DeclarationReferantDataChartingClass()
                //    {
                //        Id = "CNF",
                //        DataTypeCode = "CNF",
                //        StringProperty = "ccc",
                //        IntegerProperty = data_CNF.Count(),
                //      //  MainCarriageCarrierId = item.Id,
                //    });

                //    myResult.Add(new DeclarationReferantDataChartingClass()
                //    {
                //        Id = "ERR",
                //        DataTypeCode = "ERR",
                //        StringProperty = "ddd",
                //        IntegerProperty = data_ERR.Count(),
                //      //  MainCarriageCarrierId = item.Id,
                //    });
               
            }

            return myResult;
        }
        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFilterPMsByTenantAndUserAndQuery(int tenant, string userId, string queryCode)
        {
           var advancedQueryFilterRepositoryRepo = new AdvancedQueryFilterRepository(tenant);

            IQueryable<AdvancedQueryFilterPM> advancedFilters = null;

            advancedFilters = from a in advancedQueryFilterRepositoryRepo.context.AdvancedQueryFilters.Include("ObjectField").Include("Query").Include("Query.ObjectTable")
                              where (a.Tenant == tenant && (a.Query.UserId == userId && a.UserId != null) && a.QueryCode == queryCode) || (a.Tenant == tenant && a.QueryCode == queryCode && a.UserId == userId) || (a.Tenant == 0 && a.QueryCode == queryCode && a.UserId == null)
                              select new AdvancedQueryFilterPM()
                              {
                                  DisplayInList = a.ObjectField.DisplayInList,
                                  Id = a.Id,
                                  IndexOrder = a.IndexOrder,
                                  IsCustomFilter = a.ObjectField.IsCustomFilter,
                                  IsPredefined = a.IsPredefined,
                                  ObjectFieldId = a.ObjectFieldId,
                                  ObjectFieldName = a.ObjectField.FieldName,
                                  Operator = a.Operator,
                                  PredefinedValue = a.PredefinedValue,
                                  PredefinedValue2 = a.PredefinedValue2,
                                  QueryCode = a.QueryCode,
                                  QueryId = a.QueryId,
                                  QueryObjectTableName = a.Query.ObjectTable.Name,
                                  QueryUserId = a.Query.UserId,
                                  Tenant = a.Tenant,
                                  DataTypeCode = a.ObjectField.DataTypeCode,
                                  ObjectFieldOperator = a.ObjectField.Operator,
                                  UserId = a.UserId,
                                  ObjectFieldCode = a.ObjectFieldCode,
                              };

            return advancedFilters;
        }


    }

}
	 