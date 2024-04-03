using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Services;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.CRM
{
    public class MonthlyConversionManager
    {
        private int tenant;

        private DateTime fromDate;
        private DateTime toDate;
        private string opportunityTypeCode = null;
        private string countryId = null;
        private string ownerId = null;
        private string businessUnitId = null;
        private string leadSources = null;
        private string resellerId = null;
        private string stageCount = null;
        private List<string> myLeadSourcesList = new List<string>();
        private bool IncludeCancelled = false;

        public MonthlyConversionManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_FromDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_DataType = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DataType").FirstOrDefault();
            QueryFilterItem filterItem_CountryId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CountryId").FirstOrDefault();
            QueryFilterItem filterItem_OwnerId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "OwnerId").FirstOrDefault();
            QueryFilterItem filterItem_BusinessUnitId = queryOperations.QueryFilterItems.Where(d => d.FieldName == "BusinessUnitId").FirstOrDefault();
            QueryFilterItem filterItem_LeadSources = queryOperations.QueryFilterItems.Where(d => d.FieldName == "LeadSources").FirstOrDefault();
            QueryFilterItem filterItem_Reseller = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ResellerId").FirstOrDefault();
            QueryFilterItem filterItem_StageCount = queryOperations.QueryFilterItems.Where(d => d.FieldName == "StageCount").FirstOrDefault();
            QueryFilterItem filterItem_IncludeCancelled = queryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeCancelled").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            fromDate = todayDate;
            toDate = todayDate;

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);

                if (filterItem_ToDate != null)
                {
                    DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
                }
            }

            if (filterItem_DataType != null)
            {
                if (filterItem_DataType.FieldValue != null)
                {
                    opportunityTypeCode = filterItem_DataType.FieldValue.ToString();
                }
            }

            if (filterItem_CountryId != null)
            {
                if (filterItem_CountryId.FieldValue != null)
                {
                    countryId = filterItem_CountryId.FieldValue.ToString();
                }
            }

            if (filterItem_OwnerId != null)
            {
                if (filterItem_OwnerId.FieldValue != null)
                {
                    ownerId = filterItem_OwnerId.FieldValue.ToString();
                }
            }

            if (filterItem_BusinessUnitId != null)
            {
                if (filterItem_BusinessUnitId.FieldValue != null)
                {
                    businessUnitId = filterItem_BusinessUnitId.FieldValue.ToString();
                }
            }

            if (filterItem_LeadSources != null)
            {
                if (filterItem_LeadSources.FieldValue != null)
                {
                    leadSources = filterItem_LeadSources.FieldValue.ToString();
                }
            }

            if (!string.IsNullOrEmpty(leadSources))
            {
                leadSources = leadSources.Replace(" ", "");

                if (leadSources.ToLower() == "all")
                {

                }

                else
                {
                    leadSources = leadSources.Trim(',');
                    string[] myLeadSources = leadSources.Split(',');
                    myLeadSourcesList = myLeadSources.ToList();
                }
            }

            if (filterItem_Reseller != null)
            {
                if (filterItem_Reseller.FieldValue != null)
                {
                    resellerId = filterItem_Reseller.FieldValue.ToString();
                }
            }

            if (filterItem_StageCount != null)
            {
                if (filterItem_StageCount.FieldValue != null)
                {
                    stageCount = filterItem_StageCount.FieldValue.ToString();
                }
            }

            if (filterItem_IncludeCancelled?.FieldValue != null)
                IncludeCancelled = Convert.ToBoolean(filterItem_IncludeCancelled.FieldValue);
        }

        public byte[] GetData()
        {
            OpportunityMonthlyConversionDataProvider myDataProvider = new OpportunityMonthlyConversionDataProvider();
            myDataProvider = this.LoadDataProvider();
            return new ReportMemoryStreamService().Convert(myDataProvider, typeof(OpportunityMonthlyConversionDataProvider), tenant);
        }

        private OpportunityMonthlyConversionDataProvider LoadDataProvider()
        {
            OpportunityMonthlyConversionDataProvider myDataProvider = new OpportunityMonthlyConversionDataProvider();
            myDataProvider.MonthlyDataList = new List<MonthItemClass>();

            #region Get Base Data
            ICRMContext myCRMCotnext = CRMContext.GetContext(tenant);
            StageRepository stageRepository = new StageRepository(myCRMCotnext);
            OpportunityRepository opportunityRepository = new OpportunityRepository(myCRMCotnext);
            OpportunityStageRepository opportunityStageRepository = new OpportunityStageRepository(myCRMCotnext);

            IQueryable<OpportunityStage> iQueryable_OpportunityStages = opportunityStageRepository.GetAll(tenant);

            IQueryable<Opportunity> iQueryable_Opportunities =
                (from a in opportunityRepository.GetAllForReport(tenant)
                 where System.Data.Entity.DbFunctions.TruncateTime(a.CreateDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate)
                 && System.Data.Entity.DbFunctions.TruncateTime(a.CreateDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate)
                 select a);

            if (!IncludeCancelled)
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.IsCancelled == false);
            }

            if (!string.IsNullOrEmpty(opportunityTypeCode))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.OpportunityTypeId == opportunityTypeCode);
            }

            if (!string.IsNullOrEmpty(countryId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.Customer.CountryId == countryId);
            }

            if (!string.IsNullOrEmpty(ownerId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.OwnerId == ownerId);
            }

            if (!string.IsNullOrEmpty(resellerId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.Customer.Customer != null && d.Customer.Customer.Field2 == resellerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => d.BusinessUnitId == businessUnitId);
            }

            if (myLeadSourcesList.Count() > 0)
            {
                iQueryable_Opportunities = iQueryable_Opportunities.Where(d => myLeadSourcesList.Contains(d.LeadSourceId));
            }

            IQueryable<Stage> allStages = stageRepository.GetAll(tenant).Where(d => !d.InActive);
            IQueryable<Stage> allStages_WithProbability = allStages.Where(d => d.Probability != 0);
            IQueryable<Stage> allStages_ZeroProbability = allStages.Where(d => d.Probability == 0);

            //Qualification Stage
            string myQuaStageId = null;
            string myQuaStageName = null;
            Stage myQuaStage = allStages_WithProbability.Where(d => d.Code == "QUA").FirstOrDefault();
            if (myQuaStage != null)
            {
                myQuaStageId = myQuaStage.Id;
                myQuaStageName = myQuaStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myQuaStageId);
            }

            // Closed Won Stage
            string myCloseWonStageId = null;
            string myCloseWonStageName = null;
            Stage myCloseWonStage = allStages_WithProbability.Where(d => d.Code == "CWN").FirstOrDefault();
            if (myCloseWonStage != null)
            {
                myCloseWonStageId = myCloseWonStage.Id;
                myCloseWonStageName = myCloseWonStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myCloseWonStageId);
            }

            // Closed Lost Stage
            string myCloseLostStageId = null;
            string myCloseLostStageName = null;
            Stage myCloseLostStage = allStages_ZeroProbability.Where(d => d.Code == "CLS").FirstOrDefault();
            if (myCloseLostStage != null)
            {
                myCloseLostStageId = myCloseLostStage.Id;
                myCloseLostStageName = myCloseLostStage.Name;
                allStages_ZeroProbability = allStages_ZeroProbability.Where(d => d.Id != myCloseLostStageId);
            }

            // First Stage
            string myFirstStageId = null;
            string myFirstStageName = null;
            allStages_WithProbability = allStages_WithProbability.OrderBy(o => o.Probability);
            Stage myFirstStage = allStages_WithProbability.FirstOrDefault();
            if (myFirstStage != null)
            {
                myFirstStageId = myFirstStage.Id;
                myFirstStageName = myFirstStage.Name;
                allStages_WithProbability = allStages_WithProbability.Where(d => d.Id != myFirstStageId);
            }
            #endregion

            int index = 0;

            DateTime myDate1 = fromDate;
            int myMonthItemIndex = 0;
            int numberOfMonths = 0;

            while (myDate1 <= toDate)
            {
                numberOfMonths++;

                List<MonthItemClass> closeWonAndUpList = new List<MonthItemClass>();
                
                #region
                index = 0;

                IQueryable<Opportunity> iQueryable_Monthly =
                    (from f in iQueryable_Opportunities
                     where f.CreateDate.Value.Year == myDate1.Year
                     && f.CreateDate.Value.Month == myDate1.Month
                     select f);

                #region First Stage
                int myLeadCount = iQueryable_Monthly.Count();

                closeWonAndUpList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myFirstStageName,
                    StageId = myFirstStageId,
                    OpportunitiesCount = myLeadCount,
                    RowIndex = index++,
                });
                #endregion

                #region QUA Stage
                int myQuasCount = (from a in iQueryable_Monthly
                                   join b in iQueryable_OpportunityStages
                                   on a.Id equals b.OpportunityId
                                   where b.FromStageId == myQuaStageId
                                   select a).Distinct().Count();

                closeWonAndUpList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myQuaStageName,
                    StageId = myQuaStageId,
                    OpportunitiesCount = myQuasCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myQuasCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myQuasCount * 100 / myLeadCount) + "%"
                });
                #endregion

                #region Loop stages Probability != 0
                foreach (Stage myStage in allStages_WithProbability.OrderBy(o => o.Probability))
                {
                    IQueryable<Opportunity> iQueryable_ByStage =
                        (from a in iQueryable_Monthly
                         join b in iQueryable_OpportunityStages
                         on a.Id equals b.OpportunityId
                         where b.ToStageId == myStage.Id
                         select a).Distinct();

                    int myCount = iQueryable_ByStage.Count();

                    closeWonAndUpList.Add(new MonthItemClass()
                    {
                        Id = myMonthItemIndex++,
                        Date = myDate1,
                        DateString = myDate1.Month + "." + myDate1.Year,
                        StageName = myStage.Name,
                        StageId = myStage.Id,
                        OpportunitiesCount = myCount,
                        RowIndex = index++,
                        Percentage = myLeadCount == 0 ? 0 : (myCount * 100 / myLeadCount),
                        PercentageString = myLeadCount == 0 ? "" : (myCount * 100 / myLeadCount) + "%"
                    });
                }
                #endregion

                #region Closed Won Stage
                int myWonsCount = iQueryable_Monthly.Where(d => d.Stage.Id == myCloseWonStageId).Count();

                closeWonAndUpList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myCloseWonStageName,
                    StageId = myCloseWonStageId,
                    OpportunitiesCount = myWonsCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myWonsCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myWonsCount * 100 / myLeadCount) + "%"
                });
                #endregion

                #region Sort
                if (stageCount == "Adjusted")
                {
                    this.CorrectListValues(closeWonAndUpList);
                }
                #endregion

                foreach (MonthItemClass item in closeWonAndUpList)
                {
                    myDataProvider.MonthlyDataList.Add(item);
                }

                #region Closed Lost Stage
                int myLostCount = iQueryable_Monthly.Where(d => d.Stage.Id == myCloseLostStageId).Count();

                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = myDate1.Month + "." + myDate1.Year,
                    StageName = myCloseLostStageName,
                    StageId = myCloseLostStageId,
                    OpportunitiesCount = myLostCount,
                    RowIndex = index++,
                    Percentage = myLeadCount == 0 ? 0 : (myLostCount * 100 / myLeadCount),
                    PercentageString = myLeadCount == 0 ? "" : (myLostCount * 100 / myLeadCount) + "%"
                });
                #endregion

                #region Loop stages Probability == 0
                foreach (Stage myStage in allStages_ZeroProbability.OrderBy(o => o.Name))
                {
                    IQueryable<Opportunity> iQueryable_ByStage =
                        (from a in iQueryable_Monthly
                         join b in iQueryable_OpportunityStages
                         on a.Id equals b.OpportunityId
                         where b.ToStageId == myStage.Id
                         select a).Distinct();

                    int myCount = iQueryable_ByStage.Count();

                    myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                    {
                        Id = myMonthItemIndex++,
                        Date = myDate1,
                        DateString = myDate1.Month + "." + myDate1.Year,
                        StageName = myStage.Name,
                        StageId = myStage.Id,
                        OpportunitiesCount = myCount,
                        RowIndex = index++,
                        Percentage = myLeadCount == 0 ? 0 : (myCount * 100 / myLeadCount),
                        PercentageString = myLeadCount == 0 ? "" : (myCount * 100 / myLeadCount) + "%"
                    });
                }
                #endregion

                myDate1 = myDate1.AddMonths(1);
                #endregion
            }

            #region Average

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myFirstStageName,
                StageId = myFirstStageId,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageId == myFirstStageId).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageId == myFirstStageId).FirstOrDefault().RowIndex,
            });

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myQuaStageName,
                StageId = myQuaStageId,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageId == myQuaStageId).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageId == myQuaStageId).FirstOrDefault().RowIndex,
            });

            foreach (Stage myStage in allStages_WithProbability.OrderBy(o => o.Probability))
            {
                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = "Average",
                    StageName = myStage.Name,
                    StageId = myStage.Id,
                    OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageId == myStage.Id).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                    RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageId == myStage.Id).FirstOrDefault().RowIndex,
                });
            }

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myCloseWonStageName,
                StageId = myCloseWonStageId,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageId == myCloseWonStageId).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageId == myCloseWonStageId).FirstOrDefault().RowIndex,
            });

            myDataProvider.MonthlyDataList.Add(new MonthItemClass()
            {
                Id = myMonthItemIndex++,
                Date = myDate1,
                DateString = "Average",
                StageName = myCloseLostStageName,
                StageId = myCloseLostStageId,
                OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageId == myCloseLostStageId).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageId == myCloseLostStageId).FirstOrDefault().RowIndex,
            });

            foreach (Stage myStage in allStages_ZeroProbability.OrderBy(o => o.Name))
            {
                myDataProvider.MonthlyDataList.Add(new MonthItemClass()
                {
                    Id = myMonthItemIndex++,
                    Date = myDate1,
                    DateString = "Average",
                    StageName = myStage.Name,
                    StageId = myStage.Id,
                    OpportunitiesCount = MethodHelper.Round(myDataProvider.MonthlyDataList.Where(d => d.StageId == myStage.Id).Sum(s => s.OpportunitiesCount) / numberOfMonths, 2),
                    RowIndex = myDataProvider.MonthlyDataList.Where(d => d.StageId == myStage.Id).FirstOrDefault().RowIndex,
                });
            }

            #endregion

            return myDataProvider;
        }

        private void CorrectListValues(List<MonthItemClass> monthlyDataList)
        {
            List<decimal?> myOrder = monthlyDataList.Select(s => s.OpportunitiesCount).ToList();
            List<decimal?> expectedOrder = monthlyDataList.OrderByDescending(d => d.OpportunitiesCount).Select(s => s.OpportunitiesCount).ToList();
            bool isOrdered = myOrder.SequenceEqual(expectedOrder);

            while (!isOrdered)
            {
                for (int i = 0; i < monthlyDataList.Count; i++)
                {
                    if (i > 0)
                    {
                        if (monthlyDataList[i - 1].OpportunitiesCount != null)
                        {
                            if (monthlyDataList[i - 1].OpportunitiesCount < monthlyDataList[i].OpportunitiesCount)
                            {
                                monthlyDataList[i - 1].OpportunitiesCount = monthlyDataList[i].OpportunitiesCount;
                            }
                        }

                        else
                        {
                            monthlyDataList[i - 1].OpportunitiesCount = monthlyDataList[i].OpportunitiesCount;
                        }
                    }
                }

                myOrder = monthlyDataList.Select(s => s.OpportunitiesCount).ToList();
                expectedOrder = monthlyDataList.OrderByDescending(d => d.OpportunitiesCount).Select(s => s.OpportunitiesCount).ToList();
                isOrdered = myOrder.SequenceEqual(expectedOrder);
            }
        }
    }
}