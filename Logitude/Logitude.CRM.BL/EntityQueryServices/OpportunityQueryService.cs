using Logitude.CRM.BL.DataContracts;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.CRM.Data.BusinessUnitFilters;
using Logitude.Server.Tools.Helpers;
using Logitude.CRM.BL.EntityDws;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Global.Data.GlobalModel;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class OpportunityQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, OpportunityPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;
            OpportunityKeys opportunityKeys = entityKeys as OpportunityKeys;

            OpportunityProductQueryService queryService = new OpportunityProductQueryService(context);
            entityPM.OpportunityProducts = queryService.GetMulti(opportunityKeys, true);

            OpportunityCompetitorQueryService competitorsQueryService = new OpportunityCompetitorQueryService(context);
            entityPM.OpportunityCompetitors = competitorsQueryService.GetMulti(opportunityKeys, true);

            OpportunityAdditionalServiceQueryService additionalServiceQueryService = new OpportunityAdditionalServiceQueryService(context);
            entityPM.OpportunityAdditionalServices = additionalServiceQueryService.GetMulti(opportunityKeys, true);
        }

        public void UpdateAccount(string opportunityId, string customerId, int tenant)
        {
            OpportunityProductRepository oppRepository = new OpportunityProductRepository(tenant);
            CustomerProductRepository cusReposiory = new CustomerProductRepository(tenant);

            List<OpportunityProduct> opportunityProducts = oppRepository.GetProductsByOpportunityId(opportunityId, tenant);
            List<CustomerProduct> customerProduct = cusReposiory.GetProductsByCustomerId(customerId, tenant).ToList();

            foreach (CustomerProduct item in customerProduct)
            {
                item.PotentialChargeableWeight = 0;
                item.PotentialNumberOfShipments = 0;
                item.PotentialRevenue = 0;
                item.PotentialTEU = 0;

                cusReposiory.Update(item);
            }

            foreach (OpportunityProduct item in opportunityProducts)
            {
                CustomerProduct customerItem = customerProduct.Where(d => d.ProductTypeCode == item.OpportunityProductTypeCode).FirstOrDefault();

                if (customerItem == null)
                {
                    customerItem = new CustomerProduct()
                    {
                        CustomerId = customerId,
                        Tenant = tenant,
                        ProductTypeCode = item.OpportunityProductTypeCode,
                        PotentialChargeableWeight = item.ChargeableWeight,
                        PotentialNumberOfShipments = item.NumberOfShipments,
                        PotentialRevenue = item.Revenue,
                        PotentialTEU = item.TEU,
                    };

                    cusReposiory.Add(customerItem);
                }

                else
                {
                    customerItem.PotentialChargeableWeight = item.ChargeableWeight;
                    customerItem.PotentialNumberOfShipments = item.NumberOfShipments;
                    customerItem.PotentialRevenue = item.Revenue;
                    customerItem.PotentialTEU = item.TEU;

                    cusReposiory.Update(customerItem);
                }
            }

            OpportunityProductLocationRepository oppLocationRepository = new OpportunityProductLocationRepository(tenant);
            CustomerProductLocationRepository cusLocationReposiory = new CustomerProductLocationRepository(tenant);

            List<OpportunityProductLocation> list = oppLocationRepository.GetLocationsByOpportunityId(opportunityId, tenant);
            List<CustomerProductLocation> list2 = cusLocationReposiory.GetProductLocationsByCustomerId(customerId, tenant).ToList();

            foreach (CustomerProductLocation item in list2)
            {
                item.PotentialChargeableWeight = 0;
                item.PotentialNumberOfShipments = 0;
                item.PotentialRevenue = 0;
                item.PotentialTEU = 0;

                cusLocationReposiory.Update(item);
            }

            foreach (OpportunityProductLocation item in list)
            {
                CustomerProductLocation customerItem = list2.Where(d => d.ProductTypeCode == item.OpportunityProductTypeCode && d.CountryId == item.CountryId).FirstOrDefault();

                if (customerItem == null)
                {
                    customerItem = new CustomerProductLocation()
                    {
                        CustomerId = customerId,
                        Tenant = tenant,
                        CountryId = item.CountryId,
                        ProductTypeCode = item.OpportunityProductTypeCode,
                        PotentialChargeableWeight = item.ChargeableWeight,
                        PotentialNumberOfShipments = item.NumberOfShipments,
                        PotentialRevenue = item.Revenue,
                        PotentialTEU = item.TEU,
                    };

                    cusLocationReposiory.Add(customerItem);
                }

                else
                {
                    customerItem.PotentialChargeableWeight = item.ChargeableWeight;
                    customerItem.PotentialNumberOfShipments = item.NumberOfShipments;
                    customerItem.PotentialRevenue = item.Revenue;
                    customerItem.PotentialTEU = item.TEU;

                    cusLocationReposiory.Update(customerItem);
                }
            }

            cusReposiory.SubmitChanges();
            cusLocationReposiory.SubmitChanges();
        }

        public List<CRMChartingClass> GetStageFunnelData(string ownerId, string businessUnitId, string filterCode, int tenant, string RecordsTypeCode)
        {
            ICRMContext context = MainContext as ICRMContext;

            IQueryable<Opportunity> dataSourceQuery =
                (from d in context.Opportunities.Include("Stage")
                 where d.Tenant == tenant
                 && !d.IsClosed
                 && !d.IsCancelled
                 && d.StageId != null
                 && d.Stage.IsSelectable
                 select d);

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (RecordsTypeCode == "C")
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.CreatedByUserId == ownerId);
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
                }
            }

            if (filterCode == "SHI")
            {
                dataSourceQuery = dataSourceQuery.Where(s => s.NumberOfShipments != null);
            }

            List<CRMChartingClass> result =
                (from d in dataSourceQuery
                 group d by new { d.StageId, d.Stage.Name, d.Stage.Probability } into g
                 select new CRMChartingClass()
                 {
                     Id = g.Key.StageId,
                     LabelProperty = g.Key.Name,
                     DecimalProperty = filterCode == "CNT" ? g.Count() : g.Sum(s => s.NumberOfShipments).Value,
                     IntegerProperty = g.Key.Probability == null ? 0 : g.Key.Probability.Value,
                     GroupedId = g.Key.StageId,
                 }).ToList();

            return result;
        }

        public List<CRMChartingClass> GetOpportunitiesGroupBySalesman(string code, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = helper.Date1;
            DateTime? date2 = helper.Date2;
            int days = helper.Days;

            IQueryable<Opportunity> dataSourceQuery =
                (from d in context.Opportunities.Include("Owner").Include("Stage")
                 where d.Tenant == tenant
                 && d.OwnerId != null
                 && d.IsCancelled == false
                 select d);

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (fieldCode == "C")
            {
                if (days >= 0)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) == todayDate);
                }

                else if (days == -1)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) == date1);
                }

                else
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= date2);
                }

                if (dataSourceQuery != null)
                {
                    if (isTopTen)
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                             select new CRMChartingClass()
                             {
                                 Id = g.Key.OwnerId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.OwnerId,
                             })
                             .OrderByDescending(o => o.IntegerProperty)
                             .Take(10)
                             .ToList();
                    }

                    else
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                             select new CRMChartingClass()
                             {
                                 Id = g.Key.OwnerId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.OwnerId,
                             })
                             .ToList();
                    }
                }
            }

            else if (fieldCode == "P")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.IsClosed == false);

                if (dataSourceQuery != null)
                {
                    if (isTopTen)
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                             select new CRMChartingClass()
                             {
                                 Id = g.Key.OwnerId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.OwnerId,
                             })
                             .OrderByDescending(o => o.IntegerProperty)
                             .Take(10)
                             .ToList();
                    }

                    else
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                             select new CRMChartingClass()
                             {
                                 Id = g.Key.OwnerId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.OwnerId,
                             })
                             .ToList();
                    }
                }
            }

            else if (fieldCode == "S")
            {
                // Stages
                // CLS (Closed Lost)
                // CWN (Closed Won)

                dataSourceQuery = dataSourceQuery.Where(d => d.IsClosed == true && (d.Stage.Code == "CLS" || d.Stage.Code == "CWN"));

                if (days >= 0)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) == todayDate);
                }

                else if (days == -1)
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) == date1);
                }

                else
                {
                    dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) <= date2);
                }

                if (dataSourceQuery != null)
                {
                    IQueryable<Opportunity> data_Won = dataSourceQuery.Where(d => d.Stage.Code == "CWN");
                    IQueryable<Opportunity> data_Lost = dataSourceQuery.Where(d => d.Stage.Code == "CLS");

                    List<CRMChartingClass> myData = new List<CRMChartingClass>();

                    if (isTopTen)
                    {
                        myData =
                           (from d in dataSourceQuery
                            group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.OwnerId,
                                StringProperty = g.Key.EnglishName,
                                OwnerId = g.Key.OwnerId,
                                IntegerProperty = g.Count(),
                            })
                            .OrderByDescending(o => o.IntegerProperty)
                            .Take(10)
                            .ToList();
                    }

                    else
                    {
                        myData =
                           (from d in dataSourceQuery
                            group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.OwnerId,
                                StringProperty = g.Key.EnglishName,
                                OwnerId = g.Key.OwnerId,
                                IntegerProperty = g.Count(),
                            })
                            .ToList();
                    }

                    foreach (CRMChartingClass item in myData)
                    {
                        myResult.Add(new CRMChartingClass()
                        {
                            Id = item.Id + ":CWN",
                            DataTypeCode = "CWN",
                            StringProperty = item.StringProperty,
                            IntegerProperty = data_Won.Where(d => d.OwnerId == item.Id).Count(),
                            OwnerId = item.Id,
                        });

                        myResult.Add(new CRMChartingClass()
                        {
                            Id = item.Id + ":CLS",
                            DataTypeCode = "CLS",
                            StringProperty = item.StringProperty,
                            IntegerProperty = data_Lost.Where(d => d.OwnerId == item.Id).Count(),
                            OwnerId = item.Id,
                        });
                    }
                }
            }

            return myResult;
        }
        public List<CRMChartingClass> GetOpportunitiesGroupBySalesmanCustom(DateTime? FromDate, DateTime? ToDate, string ownerId, string businessUnitId, string fieldCode, int tenant, bool isTopTen)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;




            IQueryable<Opportunity> dataSourceQuery =
                (from d in context.Opportunities.Include("Owner").Include("Stage")
                 where d.Tenant == tenant
                 && d.OwnerId != null
                 && d.IsCancelled == false
                 select d);

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (fieldCode == "C")
            {

                dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.CreateDate) <= ToDate);


                if (dataSourceQuery != null)
                {
                    if (isTopTen)
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                             select new CRMChartingClass()
                             {
                                 Id = g.Key.OwnerId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.OwnerId,
                             })
                             .OrderByDescending(o => o.IntegerProperty)
                             .Take(10)
                             .ToList();
                    }

                    else
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                             select new CRMChartingClass()
                             {
                                 Id = g.Key.OwnerId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.OwnerId,
                             })
                             .ToList();
                    }
                }
            }

            else if (fieldCode == "P")
            {
                dataSourceQuery = dataSourceQuery.Where(d => d.IsClosed == false);

                if (dataSourceQuery != null)
                {
                    if (isTopTen)
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                             select new CRMChartingClass()
                             {
                                 Id = g.Key.OwnerId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.OwnerId,
                             })
                             .OrderByDescending(o => o.IntegerProperty)
                             .Take(10)
                             .ToList();
                    }

                    else
                    {
                        myResult =
                            (from d in dataSourceQuery
                             group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                             select new CRMChartingClass()
                             {
                                 Id = g.Key.OwnerId,
                                 StringProperty = g.Key.EnglishName,
                                 IntegerProperty = g.Count(),
                                 OwnerId = g.Key.OwnerId,
                             })
                             .ToList();
                    }
                }
            }

            else if (fieldCode == "S")
            {
                // Stages
                // CLS (Closed Lost)
                // CWN (Closed Won)

                dataSourceQuery = dataSourceQuery.Where(d => d.IsClosed == true && (d.Stage.Code == "CLS" || d.Stage.Code == "CWN"));


                dataSourceQuery = dataSourceQuery.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) <= ToDate);


                if (dataSourceQuery != null)
                {
                    IQueryable<Opportunity> data_Won = dataSourceQuery.Where(d => d.Stage.Code == "CWN");
                    IQueryable<Opportunity> data_Lost = dataSourceQuery.Where(d => d.Stage.Code == "CLS");

                    List<CRMChartingClass> myData = new List<CRMChartingClass>();

                    if (isTopTen)
                    {
                        myData =
                           (from d in dataSourceQuery
                            group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.OwnerId,
                                StringProperty = g.Key.EnglishName,
                                OwnerId = g.Key.OwnerId,
                                IntegerProperty = g.Count(),
                            })
                            .OrderByDescending(o => o.IntegerProperty)
                            .Take(10)
                            .ToList();
                    }

                    else
                    {
                        myData =
                           (from d in dataSourceQuery
                            group d by new { d.OwnerId, d.Owner.Contact.EnglishName } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.OwnerId,
                                StringProperty = g.Key.EnglishName,
                                OwnerId = g.Key.OwnerId,
                                IntegerProperty = g.Count(),
                            })
                            .ToList();
                    }

                    foreach (CRMChartingClass item in myData)
                    {
                        myResult.Add(new CRMChartingClass()
                        {
                            Id = item.Id + ":CWN",
                            DataTypeCode = "CWN",
                            StringProperty = item.StringProperty,
                            IntegerProperty = data_Won.Where(d => d.OwnerId == item.Id).Count(),
                            OwnerId = item.Id,
                        });

                        myResult.Add(new CRMChartingClass()
                        {
                            Id = item.Id + ":CLS",
                            DataTypeCode = "CLS",
                            StringProperty = item.StringProperty,
                            IntegerProperty = data_Lost.Where(d => d.OwnerId == item.Id).Count(),
                            OwnerId = item.Id,
                        });
                    }
                }
            }

            return myResult;
        }

        public List<CRMChartingClass> GetOpportunitiesChartDataCustom(DateTime? FromDate, DateTime? ToDate, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;



            IQueryable<Opportunity> dataSource =
                (from d in context.Opportunities.Include("Owner").Include("OpportunityType").Include("LeadSource").Include("Stage")
                 where d.Tenant == tenant
                 && d.IsClosed == true
                 && d.IsCancelled == false
                 select d);

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            dataSource = filter.RunFilter(dataSource);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
            }


            dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) >= FromDate && System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) <= ToDate);


            if (chartCode == "WL")
            {
                // Stages
                // CLS (Closed Lost)
                // CWN (Closed Won)

                dataSource = dataSource.Where(d => d.Stage.Code == "CLS" || d.Stage.Code == "CWN");

                myResult = (from d in dataSource
                            where d.StageId != null
                            group d by new { d.Stage } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.Stage.Code,
                                GroupedId = g.Key.Stage.Id,
                                DataTypeCode = g.Key.Stage.Code,
                                StringProperty = g.Key.Stage.Name,
                                IntegerProperty = g.Count(),
                                TypeIndex = g.Key.Stage.Code == "CWN" ? 0 : 1,
                            }).ToList();
            }

            else if (chartCode == "T")
            {
                myResult = (from d in dataSource
                            where d.OpportunityTypeId != null
                            group d by new { d.OpportunityType } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.OpportunityType.Id,
                                GroupedId = g.Key.OpportunityType.Id,
                                DataTypeCode = g.Key.OpportunityType.Code,
                                StringProperty = g.Key.OpportunityType.Name,
                                IntegerProperty = g.Count(),
                            }).ToList();
            }

            else if (chartCode == "LS")
            {
                myResult = (from d in dataSource
                            where d.LeadSourceId != null
                            group d by new { d.LeadSource } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.LeadSource.Id,
                                GroupedId = g.Key.LeadSource.Id,
                                DataTypeCode = g.Key.LeadSource.Id,
                                StringProperty = g.Key.LeadSource.Name,
                                IntegerProperty = g.Count(),
                            }).ToList();
            }

            return myResult;
        }


        public List<CRMChartingClass> GetOpportunitiesChartData(string code, string ownerId, string businessUnitId, string chartCode, int tenant)
        {
            List<CRMChartingClass> myResult = new List<CRMChartingClass>();
            ICRMContext context = MainContext as ICRMContext;

            DatesHelper helper = MethodHelper.GetDates(code, tenant);

            DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime? date1 = helper.Date1;
            DateTime? date2 = helper.Date2;
            int days = helper.Days;

            IQueryable<Opportunity> dataSource =
                (from d in context.Opportunities.Include("Owner").Include("OpportunityType").Include("LeadSource").Include("Stage")
                 where d.Tenant == tenant
                 && d.IsClosed == true
                 && d.IsCancelled == false
                 select d);

            OpportunityBusinessUnitFilter filter = new OpportunityBusinessUnitFilter(tenant);
            dataSource = filter.RunFilter(dataSource);

            if (!string.IsNullOrEmpty(ownerId))
            {
                dataSource = dataSource.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                dataSource = dataSource.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (days >= 0)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) == todayDate);
            }

            else if (days == -1)
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) == date1);
            }

            else
            {
                dataSource = dataSource.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) >= date1 && System.Data.Entity.DbFunctions.TruncateTime(d.ActualClosingDate) <= date2);
            }

            if (chartCode == "WL")
            {
                // Stages
                // CLS (Closed Lost)
                // CWN (Closed Won)

                dataSource = dataSource.Where(d => d.Stage.Code == "CLS" || d.Stage.Code == "CWN");

                myResult = (from d in dataSource
                            where d.StageId != null
                            group d by new { d.Stage } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.Stage.Code,
                                GroupedId = g.Key.Stage.Id,
                                DataTypeCode = g.Key.Stage.Code,
                                StringProperty = g.Key.Stage.Name,
                                IntegerProperty = g.Count(),
                                TypeIndex = g.Key.Stage.Code == "CWN" ? 0 : 1,
                            }).ToList();
            }

            else if (chartCode == "T")
            {
                myResult = (from d in dataSource
                            where d.OpportunityTypeId != null
                            group d by new { d.OpportunityType } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.OpportunityType.Id,
                                GroupedId = g.Key.OpportunityType.Id,
                                DataTypeCode = g.Key.OpportunityType.Code,
                                StringProperty = g.Key.OpportunityType.Name,
                                IntegerProperty = g.Count(),
                            }).ToList();
            }

            else if (chartCode == "LS")
            {
                myResult = (from d in dataSource
                            where d.LeadSourceId != null
                            group d by new { d.LeadSource } into g
                            select new CRMChartingClass()
                            {
                                Id = g.Key.LeadSource.Id,
                                GroupedId = g.Key.LeadSource.Id,
                                DataTypeCode = g.Key.LeadSource.Id,
                                StringProperty = g.Key.LeadSource.Name,
                                IntegerProperty = g.Count(),
                            }).ToList();
            }

            return myResult;
        }


        public List<OpportunityDW> GetOpportunitiesDWByDates(int tenant, DateTime fromDate, DateTime toDate, int skip, int take)
        {

            ICRMContext context = MainContext as ICRMContext;
            List<OpportunityDW> OpportunityDWList = (from a in context.Opportunities.Include("OpportunityType").Include("Rating").Include("OpportunityClosingReason").Include("LeadSource")
                                                     where a.Tenant == tenant && a.CreateDate >= fromDate && a.CreateDate <= toDate
                                                     select new OpportunityDW()
                                                     {

                                                         OpportunityId = a.Id,
                                                         CustomerId = a.CustomerId,
                                                         Subject = a.Subject,
                                                         OpportunityTypeId = a.OpportunityTypeId,
                                                         OpportunityTypeName = a.OpportunityType != null ? a.OpportunityType.Name : "",
                                                         StageId = a.StageId,
                                                         StageName = a.Stage != null ? a.Stage.Name : "",
                                                         EstimatedClosingDate = a.EstimatedClosingDate,
                                                         CreateDate = a.CreateDate,
                                                         UpdateDate = a.UpdateDate,
                                                         RatingCode = a.RatingCode,
                                                         ClosingReasonId = a.ClosingReasonId,
                                                         ClosingReasonName = a.OpportunityClosingReason != null ? a.OpportunityClosingReason.Name : "",
                                                         ClosingDescription = a.ClosingDescription,
                                                         ClientId = a.Customer.Id,
                                                         RatingName = a.Rating != null ? a.Rating.Name : "",
                                                         ClientIDDW = a.CustomerId,
                                                         NumberOfShipments = a.NumberOfShipments,
                                                         Field4 = a.Field4,
                                                         LeadsourceName = a.LeadSource != null ? a.LeadSource.Name : "",
                                                         ActualClosingDate = a.ActualClosingDate,
                                                         Reseller = a.Field1,
                                                         OwnerId = a.OwnerId,
                                                         IsCancelled = a.IsCancelled,
                                                     }).OrderBy(d => d.UpdateDate).Skip(skip).Take(take).ToList();

            return SetOtherPropInOpportunityDwLists(OpportunityDWList, tenant);


        }

        public int GetOpportunitiesCountDWByDates(int tenant, DateTime fromDate, DateTime toDate)
        {
            ///ICRMContext context = MainContext as ICRMContext;
            return context.Opportunities.Where(a => a.Tenant == tenant && a.CreateDate >= fromDate && a.CreateDate <= toDate).Count();
        }

        public List<OpportunityDW> GetOpportunitiesDWByUpdateDate(int tenant, DateTime updateDate, int skip, int take)
        {
            ICRMContext context = MainContext as ICRMContext;
            List<OpportunityDW> OpportunityDWList = (from a in context.Opportunities.Include("OpportunityType").Include("Rating").Include("OpportunityClosingReason").Include("LeadSource")
                                                     where a.Tenant == tenant && a.UpdateDate > updateDate
                                                     select new OpportunityDW()
                                                     {

                                                         OpportunityId = a.Id,
                                                         CustomerId = a.CustomerId,
                                                         Subject = a.Subject,
                                                         OpportunityTypeId = a.OpportunityTypeId,
                                                         OpportunityTypeName = a.OpportunityType != null ? a.OpportunityType.Name : "",
                                                         StageId = a.StageId,
                                                         StageName = a.Stage != null ? a.Stage.Name : "",
                                                         EstimatedClosingDate = a.EstimatedClosingDate,
                                                         CreateDate = a.CreateDate,
                                                         UpdateDate = a.UpdateDate,
                                                         RatingCode = a.RatingCode,
                                                         ClosingReasonId = a.ClosingReasonId,
                                                         ClosingReasonName = a.OpportunityClosingReason != null ? a.OpportunityClosingReason.Name : "",
                                                         ClosingDescription = a.ClosingDescription,
                                                         ClientId = a.Customer.Id,
                                                         RatingName = a.Rating != null ? a.Rating.Name : "",
                                                         ClientIDDW = a.CustomerId,
                                                         NumberOfShipments = a.NumberOfShipments,
                                                         Field4 = a.Field4,
                                                         LeadsourceName = a.LeadSource != null ? a.LeadSource.Name : "",
                                                         ActualClosingDate = a.ActualClosingDate,
                                                         Reseller = a.Field1,
                                                         OwnerId = a.OwnerId,
                                                         IsCancelled = a.IsCancelled,
                                                     }).OrderBy(d => d.UpdateDate).Skip(skip).Take(take).ToList();

            return SetOtherPropInOpportunityDwLists(OpportunityDWList,tenant);



        }

        private List<OpportunityDW> SetOtherPropInOpportunityDwLists(List<OpportunityDW> opportunityDWList, int tenant)
        {
            #region Card
            List<string> cardIds = new List<string>();
            List<string> resellerIds = new List<string>();
            List<string> contactIds = new List<string>();
            foreach (OpportunityDW item in opportunityDWList.Where(d => !string.IsNullOrEmpty(d.CustomerId) || !string.IsNullOrEmpty(d.Reseller) || !string.IsNullOrEmpty(d.OwnerId)).ToList())
            {
                if (!string.IsNullOrEmpty(item.CustomerId))
                {
                    if (!cardIds.Contains(item.CustomerId)) cardIds.Add(item.CustomerId);

                }

                if (!string.IsNullOrEmpty(item.Reseller))
                {
                    if (!resellerIds.Contains(item.Reseller)) resellerIds.Add(item.Reseller);
                }

                if (!string.IsNullOrEmpty(item.OwnerId))
                {
                    if (!contactIds.Contains(item.OwnerId)) contactIds.Add(item.OwnerId);
                }

            }

            CardRepository cardRepository = new CardRepository(tenant);
            List<Card> cardLists = new List<Card>();
            List<Card> cardResellerLists = new List<Card>();
            if (cardIds.Count > 0) cardLists = cardRepository.GetCardsByIds(cardIds, tenant).ToList();

            if (resellerIds.Count > 0) cardResellerLists = cardRepository.GetCardsByIds(resellerIds, tenant).ToList();

            #endregion

            #region Contacts

            foreach (Card card in cardLists.Where(d => !string.IsNullOrEmpty(d.SalesmanUserId)).ToList())
            {
                if (!contactIds.Contains(card.SalesmanUserId)) contactIds.Add(card.SalesmanUserId);
            }

            List<Contact> contactLists = new List<Contact>();
            if (contactIds.Count > 0)
            {
                ContactRepository contactRepository = new ContactRepository(tenant);
                contactLists = contactRepository.GetContactListsByListids(contactIds, tenant).ToList();
            }

            #endregion

            foreach (OpportunityDW item in opportunityDWList)
            {
                #region Field4
                if (!string.IsNullOrEmpty(item.Field4))
                {
                    item.IncomeCurrency = "USD";
                    if (item.Field4.Contains("E"))
                    {
                        item.Field4 = item.Field4.Replace("E", "");
                        item.IncomeCurrency = "EUR";
                    }
                }
                #endregion

                #region Card
                Card card = cardLists.Where(d => d.Id == item.CustomerId).FirstOrDefault();

                if (card != null)
                {
                    item.CustomerCode = card.Code;

                    #region Contacts
                    //SalesmanUserName
                    if (!string.IsNullOrEmpty(card.SalesmanUserId) && contactLists != null)
                    {
                        item.SalesmanId = card.SalesmanUserId;
                        Contact contact = contactLists.Where(d => d.Id == item.SalesmanId).FirstOrDefault();
                        if (contact != null) item.SalesmanName = contact.EnglishName;
                    }


                    #endregion
                }
                #endregion

                #region Reseller
                if (!string.IsNullOrEmpty(item.Reseller))
                {
                    Card resellerCard = cardResellerLists.Where(d => d.Id == item.Reseller).FirstOrDefault();
                    if (resellerCard != null)
                    {
                        item.Reseller = resellerCard.EnglishName;
                    }
                }
                #endregion

                #region Owner
                if (!string.IsNullOrEmpty(item.OwnerId) && contactLists != null)
                {
                    Contact contact = contactLists.Where(d => d.Id == item.OwnerId).FirstOrDefault();
                    if (contact != null) item.OwnerName = contact.EnglishName;
                }
                #endregion
            }

            return opportunityDWList;
        }

        public int GetOpportunitiesDWCountByUpdateDate(int tenant, DateTime updateDate)
        {
            ICRMContext context = MainContext as ICRMContext;
            return context.Opportunities.Where(a => a.Tenant == tenant && a.UpdateDate > updateDate).Count();
        }

        public List<OpportunityCRMDetails> GetLogitudeOpportunities(int tenant, LogitudeCRMReportFilter logitudeCRMReportFilter)
        {

            ICRMContext context = MainContext as ICRMContext;
            IQueryable<OpportunityCRMDetails> opportunityDetails = (from a in context.Opportunities.Include("Customer").Include("Customer.Customer").Include("OpportunityClosingReason")
                                                                    join opportunityType in context.OpportunityTypes on a.OpportunityTypeId equals opportunityType.Id
                                                                    where a.Tenant == tenant && a.IsClosed && a.OpportunityClosingReason.Code == "WN" && !a.IsCancelled
                                                                    && (a.Customer == null || a.Customer.Customer == null || a.Customer.Customer.IsCustomer == true)
                                                                    select new OpportunityCRMDetails()
                                                                    {
                                                                        CustomerStatusCode = (a.Customer != null && a.Customer.Customer != null) ? a.Customer.Customer.CustomerStatusCode : null,
                                                                        ResellerId = (a.Customer != null && a.Customer.Customer != null) ? a.Customer.Customer.Field2 : null,
                                                                        OpportunityTypeId = a.OpportunityTypeId,
                                                                        OpportunityTypeCode = opportunityType.Code,
                                                                        IsCancelled = a.IsCancelled,
                                                                        CreateDate = a.CreateDate.Value,

                                                                        ClientId = a.Customer != null ? a.Customer.Code : null,
                                                                        TenantNumber = a.Customer != null ? a.Customer.ReceivablesAccountingCard : null,
                                                                        ClientName = a.Customer != null ? a.Customer.EnglishName : null,
                                                                        Reseller = (a.Customer != null && a.Customer.Customer != null) ? a.Customer.Customer.Field2 : null,
                                                                        CountryName = a.Customer != null ? a.Customer.CountryName : null,
                                                                        NumberOfUsers = a.NumberOfShipments,
                                                                        Field4 = a.Field4,
                                                                        IsNewCustomer = opportunityType.Code == "R" ? -1 : opportunityType.Code == "N" ? 1 : (int?)null,
                                                                        InActive = (a.Customer != null && a.Customer.Customer != null) ? a.Customer.Customer.CustomerStatusCode != "ACT" : false,
                                                                        ActualClosingDate = a.ActualClosingDate ?? a.CreateDate.Value,

                                                                    });
            opportunityDetails = ApplyOpportunitiesFlter(logitudeCRMReportFilter, opportunityDetails);
            return opportunityDetails.ToList();
        }

        private static IQueryable<OpportunityCRMDetails> ApplyOpportunitiesFlter(LogitudeCRMReportFilter logitudeCRMReportFilter, IQueryable<OpportunityCRMDetails> opportunityDetails)
        {
            if (!string.IsNullOrEmpty(logitudeCRMReportFilter.CustomerStatus))
            {
                opportunityDetails = opportunityDetails.Where(d => d.CustomerStatusCode == logitudeCRMReportFilter.CustomerStatus);
            }

            if (!string.IsNullOrEmpty(logitudeCRMReportFilter.ResellerId))
            {
                opportunityDetails = opportunityDetails.Where(d => d.ResellerId == logitudeCRMReportFilter.ResellerId);
            }

            if (logitudeCRMReportFilter.OpportunityTypes != null && logitudeCRMReportFilter.OpportunityTypes.Count > 0)
            {
                opportunityDetails = opportunityDetails.Where(d => logitudeCRMReportFilter.OpportunityTypes.Contains(d.OpportunityTypeId));
            }

            return opportunityDetails;
        }
    }
}
