using DocumentFormat.OpenXml.Office.CustomXsn;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.CustomsBook;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CustomItemDetailsServiceReference;
using UnifreightIIG.Common.CustomsBookServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public  class Get_CB_MSG_8314_8888_CustomItemDetailsHeaderResponseService: ResponseServiceBase<CB_NG_8314_8888_CustomsItemDetailsResponseData, CB_NG_8888_CustomsItemOut, CD_NG_8314_Web01_CustomsItemDetailsRequestParams>
    {
        public override CB_NG_8314_8888_CustomsItemDetailsResponseData GetResponse(
            CB_NG_8888_CustomsItemOut customResponse, CD_NG_8314_Web01_CustomsItemDetailsRequestParams requestParams)
        {
            bool succeeded = false;
            bool hasException = false;
            string exceptionMessage = null;

            if (customResponse.CustomsItem != null)
            {
                succeeded = true;
            }
           

            CB_NG_8314_8888_CustomsItemDetailsResponseData responseData = new CB_NG_8314_8888_CustomsItemDetailsResponseData() { Succeeded = succeeded, HasException = hasException, UserMessage = exceptionMessage };
            responseData.CustomsItemList = new List<CustomsItem>();
             foreach (var item in customResponse.CustomsItem)
             {

                responseData.CustomsItemList.Add(new CustomsItem()
                {
                    fullClassification = item.fullClassification,
                    statisticMeasurementUnitExternalID = item.statisticMeasurementUnitExternalID,
                    isDiscountCode = item.isDiscountCode,
                    goodsDescription = item.GoodsDescription,
                    customsBookTypeName = item.CustomsBookTypeName

                });


           }
            return responseData;


           
        }

        public override void Update(CB_NG_8888_CustomsItemOut customResponse, CD_NG_8314_Web01_CustomsItemDetailsRequestParams requestParams)
        {

            var logContext = CustomContext.GetContext(requestParams.Tenant);
            this.MyResponseData = new CB_NG_8314_8888_CustomsItemDetailsResponseData();
          

            var mehesCustomsItemRows = customResponse.CustomsItem;
            //List<string> CustomsItemIDList = mehesCustomsItemRows.Select(rec => rec.ID.ToString()).ToList();


            if (mehesCustomsItemRows != null)
            {

                var customsItemQueryService = new CustomsItemQueryService(logContext);
                var customsItemUpdateService = new CustomsItemUpdateService(logContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                List<CustomsItemPM> dbListCustomsItemRows = customsItemQueryService.GetAll();
                var customsItemDetailsHistoryQueryService = new CustomsItemDetailsHistoryQueryService(logContext);
                var customsItemDetailsHistoryUpdateService = new CustomsItemDetailsHistoryUpdateService(logContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                List<CustomsItemDetailsHistoryPM> dbListCustomsItemDetailsHistoryRows = customsItemDetailsHistoryQueryService.GetAll();
                var propertiesDetailsHistoryQueryService = new PropertiesDetailsHistoryQueryService(logContext);
                var propertiesDetailsHistoryUpdateService = new PropertiesDetailsHistoryUpdateService(logContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                List<PropertiesDetailsHistoryPM> dbListPropertiesDetailsHistoryRows = propertiesDetailsHistoryQueryService.GetAll();

                foreach (var mehesCustomsItemRow in mehesCustomsItemRows)
                {
                    bool doUpdateCustomsItem = true;
                    bool doUpdateCustomsItemDetailsHistory = true;
                    bool doUpdatePropertiesDetailsHistory = true;
                    bool doNotAdd= mehesCustomsItemRow.isDiscountCode==true?false:true;
                    var currentDBListCustomsItemRow = new CustomsItemPM();
                    var currentDBListCustomsItemDetailsHistoryRow = new CustomsItemDetailsHistoryPM();
                    var currentDBListPropertiesDetailsHistoryRow = new PropertiesDetailsHistoryPM();


                    if ((dbListCustomsItemRows == null || dbListCustomsItemRows.Count() == 0 ))
                    {
                        currentDBListCustomsItemRow.ID = mehesCustomsItemRow.ID.ToString();
                        currentDBListCustomsItemRow.ChangeSetOp = ChangeSetOperation.Insert;
                        currentDBListCustomsItemDetailsHistoryRow.CustomsItemID= mehesCustomsItemRow.ID.ToString();
                        currentDBListCustomsItemDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Insert;
                        currentDBListPropertiesDetailsHistoryRow.CustomsItemID = mehesCustomsItemRow.ID.ToString();
                        currentDBListPropertiesDetailsHistoryRow.ChangeSetOp=ChangeSetOperation.Insert;
                    }
                    else
                    {
                        currentDBListCustomsItemRow = dbListCustomsItemRows.FirstOrDefault(rec => rec.ID == mehesCustomsItemRow.ID.ToString());
                        currentDBListCustomsItemDetailsHistoryRow = dbListCustomsItemDetailsHistoryRows.Where(rec => rec.CustomsItemID == mehesCustomsItemRow.ID.ToString() && rec.EntityStatusID!=4).OrderByDescending (rec => rec.EndDate).FirstOrDefault();
                        currentDBListPropertiesDetailsHistoryRow= dbListPropertiesDetailsHistoryRows.Where(rec=>rec.CustomsItemID==mehesCustomsItemRow.ID.ToString() && rec.EntityStatusID != 4).OrderByDescending(rec=>rec.EndDate).FirstOrDefault();
                        if (currentDBListCustomsItemRow == null)
                        {
                            currentDBListCustomsItemRow = new CustomsItemPM();
                            currentDBListCustomsItemRow.ID = mehesCustomsItemRow.ID.ToString();
                            currentDBListCustomsItemRow.ChangeSetOp = ChangeSetOperation.Insert;
                         
                           

                        }
                        else
                        {
                            doUpdateCustomsItem = CompareMehesToDBCustomsItem(mehesCustomsItemRow, currentDBListCustomsItemRow);
                            currentDBListCustomsItemRow.ChangeSetOp = ChangeSetOperation.Update;
                        }
                        if (currentDBListCustomsItemDetailsHistoryRow == null)
                        {
                            currentDBListCustomsItemDetailsHistoryRow = new CustomsItemDetailsHistoryPM();
                            currentDBListCustomsItemDetailsHistoryRow.CustomsItemID = mehesCustomsItemRow.ID.ToString();
                            currentDBListCustomsItemDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        else
                        {
                            doUpdateCustomsItemDetailsHistory = CompareMehesToDBCustomsItemDetailsHistory(mehesCustomsItemRow, currentDBListCustomsItemDetailsHistoryRow);
                            currentDBListCustomsItemDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Update;
                        }
                        if (currentDBListPropertiesDetailsHistoryRow == null)
                        {
                            currentDBListPropertiesDetailsHistoryRow = new PropertiesDetailsHistoryPM();
                            currentDBListPropertiesDetailsHistoryRow.CustomsItemID = mehesCustomsItemRow.ID.ToString();
                            currentDBListPropertiesDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        else
                        {
                            doUpdatePropertiesDetailsHistory = CompareMehesToDBPropertiesDetailsHistory(mehesCustomsItemRow, currentDBListPropertiesDetailsHistoryRow);
                            currentDBListPropertiesDetailsHistoryRow.ChangeSetOp = ChangeSetOperation.Update;

                         
                             
                        }
                    }

                    if (doUpdateCustomsItem && (mehesCustomsItemRow.isDiscountCode == true || mehesCustomsItemRow.fullClassification.Contains("-")))
                    {
                        currentDBListCustomsItemRow.FullClassification = mehesCustomsItemRow.fullClassification.Split('/')[0]?? mehesCustomsItemRow.fullClassification;
                        currentDBListCustomsItemRow.CustomsItemHierarchicLocationID = mehesCustomsItemRow.HierarchicLocationID;
                        currentDBListCustomsItemRow.ComputedCheckDigit =mehesCustomsItemRow.fullClassification.Length>10 ?mehesCustomsItemRow.fullClassification.Substring(mehesCustomsItemRow.fullClassification.Length - 1):null ;
                        currentDBListCustomsItemRow.CustomsBookTypeID = requestParams.CustomsBookType;
                        currentDBListCustomsItemRow.CustomsItemCategoryID = mehesCustomsItemRow.isDiscountCode == false ? 1 : currentDBListCustomsItemRow.CustomsItemCategoryID;
                       // currentDBListCustomsItemRow.CustomsItemCategoryID = mehesCustomsItemRow.statisticMeasurementUnitCode?? 0;

                        customsItemUpdateService.Update(currentDBListCustomsItemRow, false);
                       
                            logContext.SaveChanges();
                       
                    }
                    if(doUpdateCustomsItemDetailsHistory && (mehesCustomsItemRow.isDiscountCode == true || mehesCustomsItemRow.fullClassification.Contains("-")))
                     {
                        currentDBListCustomsItemDetailsHistoryRow.CustomsItemID = mehesCustomsItemRow.ID.ToString();
                        currentDBListCustomsItemDetailsHistoryRow.Title = mehesCustomsItemRow.GoodsDescription;
                        currentDBListCustomsItemDetailsHistoryRow.StartDate = DateTime.Today.AddDays(-30);
                        currentDBListCustomsItemDetailsHistoryRow.EndDate= DateTime.Today.AddDays(30);

                        customsItemDetailsHistoryUpdateService.Update(currentDBListCustomsItemDetailsHistoryRow, false);
                        logContext.SaveChanges();


                    }

                    if (doUpdatePropertiesDetailsHistory && (mehesCustomsItemRow.isDiscountCode == true ||mehesCustomsItemRow.fullClassification.Contains("-")))
                    {
                        currentDBListPropertiesDetailsHistoryRow.CustomsItemID = mehesCustomsItemRow.ID.ToString();
                        currentDBListPropertiesDetailsHistoryRow.StartDate= DateTime.Today.AddDays(-30);
                        currentDBListPropertiesDetailsHistoryRow.EndDate= DateTime.Today.AddDays(30);
                        currentDBListPropertiesDetailsHistoryRow.MeasurementUnitID = mehesCustomsItemRow.statisticMeasurementUnitCode;
                        propertiesDetailsHistoryUpdateService.Update(currentDBListPropertiesDetailsHistoryRow, false);
                        logContext.SaveChanges();
                    }
                }


                }




            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שאילתא לעדכון ספר סיווג בוצעה בהצלחה";

        }



        private static bool CompareMehesToDBCustomsItem(CB_NG_8888_CustomsItemOutCustomsItem mehesCustomsItemRow, CustomsItemPM currentDBListCustomsItemRow)
        {
            string fullClassification = currentDBListCustomsItemRow.FullClassification;
              if (mehesCustomsItemRow.fullClassification.Contains("/"))
            {
                fullClassification = currentDBListCustomsItemRow.FullClassification + "/" + currentDBListCustomsItemRow.ComputedCheckDigit;
            }
            if (mehesCustomsItemRow.fullClassification != fullClassification ||

                mehesCustomsItemRow.HierarchicLocationID != currentDBListCustomsItemRow.CustomsItemHierarchicLocationID )
                
            {
                return true;
            }

            return false;
        }


        private static bool CompareMehesToDBCustomsItemDetailsHistory(CB_NG_8888_CustomsItemOutCustomsItem mehesCustomsItemRow, CustomsItemDetailsHistoryPM currentDBListCustomsItemDetailsHistoryRow)
        {
            if (mehesCustomsItemRow.GoodsDescription != currentDBListCustomsItemDetailsHistoryRow.Title ||
                             currentDBListCustomsItemDetailsHistoryRow.EndDate < DateTime.UtcNow.Date)
            
            {
                return true;
            }

            return false;
        }
        private static bool CompareMehesToDBPropertiesDetailsHistory(CB_NG_8888_CustomsItemOutCustomsItem mehesCustomsItemRow, PropertiesDetailsHistoryPM currentDBListPropertiesDetailsHistoryRow)
        {
            if (currentDBListPropertiesDetailsHistoryRow.EndDate< DateTime.UtcNow.Date ||
            
                mehesCustomsItemRow.statisticMeasurementUnitCode != currentDBListPropertiesDetailsHistoryRow.MeasurementUnitID)
            {
                return true;
            }

            return false;
        }




    }
}
