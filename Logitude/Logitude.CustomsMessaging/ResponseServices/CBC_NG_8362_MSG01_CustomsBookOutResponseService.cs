using System;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Storage;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using UnifreightIIG.Common.MessageLib.Docs;
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using System.Collections;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.CustomsBookServiceReference;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Logitude.CustomsMessaging.CustomsBook;
using Newtonsoft.Json;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class CBC_NG_8362_MSG01_CustomsBookOutResponseService : ResponseServiceBase<
         CustomsBookInResponseData, CBC_NG_8362_MSG01_CustomsBookOut, CustomsBookInRequestParams>
    {
        public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        private Dictionary<string, IList> _MyLocalCache = new Dictionary<string, IList>();
        private AmitalContext _AmitalContext;
        private int _Tenant;
        List<CustomsItemPM> _DBListCustomsItemRows = new List<CustomsItemPM>();

        public override void Update(CBC_NG_8362_MSG01_CustomsBookOut customResponse, CustomsBookInRequestParams requestParams)
        {
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            _Tenant = requestParams.Tenant;
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();
            CustomsBookImport customsBookImport = new CustomsBook.CustomsBookImport();

            var logContext = CustomContext.GetContext(requestParams.Tenant);
            try
            {
                var customsItemQueryService = new CustomsItemQueryService(logContext);
                _DBListCustomsItemRows = customsItemQueryService.GetAll();
            }
            catch
            {

            }


            this.MyResponseData = new CustomsBookInResponseData();

            //if (!string.IsNullOrWhiteSpace(requestParams.toDate) || !string.IsNullOrWhiteSpace(requestParams.ExtertnalID))
            {
                if (this.MyRequestSheetParam == null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                }
            }

            //Checking foe Exceptions
            if (customResponse.ResponseContentHeader.Exception != null || _ResponseHeaderExeption != null)
            {
                if (customResponse.ResponseContentHeader.Exception != null)
                {
                    this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                }
                else
                {
                    this.MyResponseData.UserMessage = _ResponseHeaderExeption.ErrorDescription;
                }
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                return;
            }

            if (customResponse.ResponseContentHeader != null)
            {
                {
                    if (customResponse.ResponseContentHeader.Exception != null)
                    {
                        if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription))
                        {
                            var errMess = "No details in the Response for " + requestParams.fromDate.Value.ToShortDateString() + " to " + requestParams.toDate.Value.ToShortDateString();
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        else
                        {
                            var errMess = "No details in the Response for " + customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription + ". For " + requestParams.fromDate.Value.ToShortDateString() + " to " + requestParams.toDate.Value.ToShortDateString();
                            LogMessagingUtil.Instance.AppendLine(errMess);
                            this.MyResponseData.UserMessage = errMess;
                        }
                        this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                        this.MyResponseData.Succeeded = true;
                        this.MyResponseData.HasException = true;
                        return;
                    }
                }
            }


            if (customResponse.CustomsBookGeneralTables == null)
            {
                var errMess = "No details in the Response for " + requestParams.fromDate.Value.ToShortDateString() + " to " + requestParams.toDate.Value.ToShortDateString();
                LogMessagingUtil.Instance.AppendLine(errMess);
                this.MyResponseData.UserMessage = errMess;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                customsBookImport.Upsert(requestParams, _Tenant);
                return;
            }

            if (customResponse.CustomsBookGeneralTables.CustomsItem == null)
            {
                var errMess = "No CustomsItem details in the Response for " + requestParams.fromDate.Value.ToShortDateString() + " to " + requestParams.toDate.Value.ToShortDateString();
                LogMessagingUtil.Instance.AppendLine(errMess);
                this.MyResponseData.UserMessage = errMess;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.ResponseStatusXML = GetDummyXml(this.MyResponseData.UserMessage);
                customsBookImport.Upsert(requestParams, _Tenant);
                return;
            }

            //Get the data
            MyResponseData.customsBookGeneralTables = new CustomsBookGeneralTables();
            var mehesCustomsItemRows = GetMehesCustomsItemRows(customResponse.CustomsBookGeneralTables.CustomsItem);
            List<string> CustomsItemIDList = mehesCustomsItemRows.Select(rec => rec.ID.ToString()).ToList();
            var mehesCustomsItemDetailsHistoryRows = GetMehesCustomsItemDetailsHistoryRows(customResponse.CustomsBookGeneralTables.CustomsItemDetailsHistory, CustomsItemIDList);
            var mehesPropertiesDetailsHistoryRows = GetMehesPropertiesDetailsHistoryRows(customResponse.CustomsBookGeneralTables.PropertiesDetailsHistory, CustomsItemIDList);

            //Upsert data
            CustomsItemImport customsItemImport = new CustomsBook.CustomsItemImport();
            customsItemImport.MultiUpsert(mehesCustomsItemRows, _Tenant);
            CustomsItemDetailsHistoryImport customsItemDetailsHistoryImport = new CustomsBook.CustomsItemDetailsHistoryImport();
            customsItemDetailsHistoryImport.MultiUpsert(mehesCustomsItemDetailsHistoryRows, _Tenant);
            PropertiesDetailsHistoryImport propertiesDetailsHistoryImport = new CustomsBook.PropertiesDetailsHistoryImport();
            propertiesDetailsHistoryImport.MultiUpsert(mehesPropertiesDetailsHistoryRows, _Tenant);
            customsBookImport.Upsert(requestParams, _Tenant);

            SetDBCustomsItem(mehesCustomsItemRows);
            SetDBCustomsItemDetailsHistory(mehesCustomsItemDetailsHistoryRows);
            SetDBPropertiesDetailsHistory(mehesPropertiesDetailsHistoryRows);

            LogMessagingUtil.Instance.AppendLine("Analyze Credit Query response for " + requestParams.fromDate.Value.ToShortDateString() + " to " + requestParams.toDate.Value.ToShortDateString());

            //customResponse.FaultGeneralDetail = customResponse.FaultGeneralDetail ?? new DF_NG_Web8333_FaultProceduralDetailsResponseFaultGeneralDetail[] { new DF_NG_Web8333_FaultProceduralDetailsResponseFaultGeneralDetail() };
            //string xml = XmlGenericUtil<DF_NG_Web8333_FaultProceduralDetailsResponseFaultGeneralDetail[]>.SerializeObject(customResponse.FaultGeneralDetail);
            //MyResponseData.ResponseStatusXML = xml;

            //customResponse.CustomsBookGeneralTables = customResponse.CustomsBookGeneralTables ?? new CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTables();
            //string xml = XmlGenericUtil<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTables>.SerializeObject(customResponse.CustomsBookGeneralTables);
            //MyResponseData.ResponseStatusXML = xml;

            // customResponse.CustomsBookGeneralTables.CustomsItem = customResponse.CustomsBookGeneralTables.CustomsItem ?? new CustomsBookInResponseData.CustomsItem[] { new CustomsBookInResponseData.CustomsItem() };

            MyResponseData.customsBookGeneralTables = MyResponseData.customsBookGeneralTables ?? new CustomsBookGeneralTables();
            if (requestParams.IsAngularClient)
            {
                MyResponseData.ResponseStatusXML = JsonConvert.SerializeObject(MyResponseData.customsBookGeneralTables);
                MyResponseData.customsBookGeneralTables = null;
            }

            else
            {
                string xml = XmlGenericUtil<CustomsBookGeneralTables>.SerializeObject(MyResponseData.customsBookGeneralTables);
                MyResponseData.ResponseStatusXML = xml;
            }
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;
            this.MyResponseData.UserMessage = "שאילתא לעדכון ספר סיווג בוצעה בהצלחה";
        }

        private void SetDBPropertiesDetailsHistory(List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> mehesPropertiesDetailsHistoryRows)
        {
            var logContext = CustomContext.GetContext(_Tenant);
            if (mehesPropertiesDetailsHistoryRows != null)
            {
                if (mehesPropertiesDetailsHistoryRows.Count() > 0)
                {
                    //Get ONLY for CustomsItemID that is in CustomsItem
                    MyResponseData.customsBookGeneralTables.PropertiesDetailsHistoryList = new List<CustomsBookPropertiesDetailsHistory>();

                    foreach (var mehesPropertiesDetailsHistoryRow in mehesPropertiesDetailsHistoryRows)
                    {
                        CustomsBookPropertiesDetailsHistory newPropertiesDetailsHistory = new CustomsBookPropertiesDetailsHistory()
                        {
                            ID = mehesPropertiesDetailsHistoryRow.ID,
                            CustomsItemID = mehesPropertiesDetailsHistoryRow.CustomsItemID,
                            StartDate = mehesPropertiesDetailsHistoryRow.StartDate,
                            StartDateSpecified = mehesPropertiesDetailsHistoryRow.StartDateSpecified,
                            EndDate = mehesPropertiesDetailsHistoryRow.EndDate,
                            EndDateSpecified = mehesPropertiesDetailsHistoryRow.EndDateSpecified,
                            EntityStatusID = mehesPropertiesDetailsHistoryRow.EntityStatusID,
                            MeasurementUnitID = mehesPropertiesDetailsHistoryRow.MeasurementUnitID,
                            MeasurementUnitIDSpecified = mehesPropertiesDetailsHistoryRow.MeasurementUnitIDSpecified,
                        };
                        MyResponseData.customsBookGeneralTables.PropertiesDetailsHistoryList.Add(newPropertiesDetailsHistory);
                    }
                }
            }
        }

        private void SetDBCustomsItemDetailsHistory(List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> mehesCustomsItemDetailsHistoryRows)
        {
            var logContext = CustomContext.GetContext(_Tenant);
            if (mehesCustomsItemDetailsHistoryRows != null)
            {
                if (mehesCustomsItemDetailsHistoryRows.Count() > 0)
                {
                    //Get ONLY for CustomsItemID that is in CustomsItem
                    MyResponseData.customsBookGeneralTables.CustomsItemDetailsHistoryList = new List<CustomsBookCustomsItemDetailsHistory>();

                    foreach (var mehesCustomsItemDetailsHistoryRow in mehesCustomsItemDetailsHistoryRows)
                    {
                        CustomsBookCustomsItemDetailsHistory newCustomsItemDetailsHistory = new CustomsBookCustomsItemDetailsHistory()
                        {
                            ID = mehesCustomsItemDetailsHistoryRow.ID,
                            Title = mehesCustomsItemDetailsHistoryRow.Title,
                            StartDate = mehesCustomsItemDetailsHistoryRow.StartDate,
                            StartDateSpecified = mehesCustomsItemDetailsHistoryRow.StartDateSpecified,
                            EndDate = mehesCustomsItemDetailsHistoryRow.EndDate,
                            EndDateSpecified = mehesCustomsItemDetailsHistoryRow.EndDateSpecified,
                            EntityStatusID = mehesCustomsItemDetailsHistoryRow.EntityStatusID,
                            CustomsItemID = mehesCustomsItemDetailsHistoryRow.CustomsItemID,
                        };
                        MyResponseData.customsBookGeneralTables.CustomsItemDetailsHistoryList.Add(newCustomsItemDetailsHistory);
                    }
                }
            }
        }

        private void SetDBCustomsItem(List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem> mehesCustomsItemRows)
        {
            var logContext = CustomContext.GetContext(_Tenant);

            if (mehesCustomsItemRows != null)
            {
                if (mehesCustomsItemRows.Count() > 0)
                {
                    MyResponseData.customsBookGeneralTables.CustomsItemList = new List<CustomsBookCustomsItem>();

                    foreach (var mehesCustomsItemRow in mehesCustomsItemRows)
                    {
                        CustomsBookCustomsItem newCustomsItem = new CustomsBookCustomsItem()
                        {
                            ID = mehesCustomsItemRow.ID,
                            CustomsBookTypeID = mehesCustomsItemRow.CustomsBookTypeID,
                            FullClassification = mehesCustomsItemRow.FullClassification,
                            CustomsItemCategoryID = mehesCustomsItemRow.CustomsItemCategoryID,
                            CustomsItemHierarchicLocationID = mehesCustomsItemRow.CustomsItemHierarchicLocationID,
                            CustomsItemHierarchicLocationIDSpecified = mehesCustomsItemRow.CustomsItemHierarchicLocationIDSpecified,
                            ComputedCheckDigit = mehesCustomsItemRow.ComputedCheckDigit,
                        };
                        MyResponseData.customsBookGeneralTables.CustomsItemList.Add(newCustomsItem);
                    }
                }
            }
        }


        private List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> GetMehesPropertiesDetailsHistoryRows(CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory[] cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory, List<string> customsItemIDList)
        {
            /*List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> mehesPropertiesDetailsHistoryRows = new List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory>();
            if (cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory != null)
            {
                mehesPropertiesDetailsHistoryRows = cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory.ToList();
                mehesPropertiesDetailsHistoryRows = mehesPropertiesDetailsHistoryRows.Where(rec => customsItemIDList.Contains(rec.CustomsItemID.ToString())).ToList();
            }*/

            List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> mehesPropertiesDetailsHistoryRows = new List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory>();
            List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> mehesPropertiesDetailsHistoryRows_All = new List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory>();
            List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory> mehesPropertiesDetailsHistoryRows_CustomsItemNOTExists = new List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory>();

            if (cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory != null)
            {
                mehesPropertiesDetailsHistoryRows_All = cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory.ToList();
                mehesPropertiesDetailsHistoryRows = mehesPropertiesDetailsHistoryRows_All.Where(rec => customsItemIDList.Contains(rec.CustomsItemID.ToString())).ToList();
                mehesPropertiesDetailsHistoryRows_CustomsItemNOTExists = mehesPropertiesDetailsHistoryRows_All.Where(rec => !customsItemIDList.Contains(rec.CustomsItemID.ToString())).ToList();

                if (mehesPropertiesDetailsHistoryRows_CustomsItemNOTExists != null && mehesPropertiesDetailsHistoryRows_CustomsItemNOTExists.Count() > 0)
                {
                    foreach (var mehesPropertiesDetailsHistoryRow_CustomsItemNOTExists in mehesPropertiesDetailsHistoryRows_CustomsItemNOTExists)
                    {
                        CustomsItemPM currentDBListCustomsItemRow = new CustomsItemPM();
                        currentDBListCustomsItemRow = _DBListCustomsItemRows.FirstOrDefault(rec => rec.ID == mehesPropertiesDetailsHistoryRow_CustomsItemNOTExists.CustomsItemID.ToString() && (rec.CustomsBookTypeID >= 1 && rec.CustomsBookTypeID <= 3));
                        if (currentDBListCustomsItemRow != null && !string.IsNullOrWhiteSpace(currentDBListCustomsItemRow.ID))
                        {
                            mehesPropertiesDetailsHistoryRows.Add(mehesPropertiesDetailsHistoryRow_CustomsItemNOTExists);
                        }
                    }
                }
            }


            return mehesPropertiesDetailsHistoryRows;
        }

        private List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> GetMehesCustomsItemDetailsHistoryRows(CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory[] cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory, List<string> customsItemIDList)
        {
            /*List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> mehesCustomsItemDetailsHistoryRows = new List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory>();
           if (cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory != null)
           {
               mehesCustomsItemDetailsHistoryRows = cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory.ToList();
               mehesCustomsItemDetailsHistoryRows = mehesCustomsItemDetailsHistoryRows.Where(rec => customsItemIDList.Contains(rec.CustomsItemID.ToString())).ToList();
           }*/

            List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> mehesCustomsItemDetailsHistoryRows = new List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory>();
            List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> mehesCustomsItemDetailsHistoryRows_All = new List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory>();
            List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory> mehesCustomsItemDetailsHistoryRows_CustomsItemNOTExists = new List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory>();

            if (cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory != null)
            {
                mehesCustomsItemDetailsHistoryRows_All = cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory.ToList();
                mehesCustomsItemDetailsHistoryRows = mehesCustomsItemDetailsHistoryRows_All.Where(rec => customsItemIDList.Contains(rec.CustomsItemID.ToString())).ToList();
                mehesCustomsItemDetailsHistoryRows_CustomsItemNOTExists = mehesCustomsItemDetailsHistoryRows_All.Where(rec => !customsItemIDList.Contains(rec.CustomsItemID.ToString())).ToList();

                if (mehesCustomsItemDetailsHistoryRows_CustomsItemNOTExists != null && mehesCustomsItemDetailsHistoryRows_CustomsItemNOTExists.Count() > 0)
                {
                    foreach (var mehesCustomsItemDetailsHistoryRow_CustomsItemNOTExists in mehesCustomsItemDetailsHistoryRows_CustomsItemNOTExists)
                    {
                        CustomsItemPM currentDBListCustomsItemRow = new CustomsItemPM();
                        currentDBListCustomsItemRow = _DBListCustomsItemRows.FirstOrDefault(rec => rec.ID == mehesCustomsItemDetailsHistoryRow_CustomsItemNOTExists.CustomsItemID.ToString() && (rec.CustomsBookTypeID >= 1 && rec.CustomsBookTypeID <= 3));
                        if (currentDBListCustomsItemRow != null && !string.IsNullOrWhiteSpace(currentDBListCustomsItemRow.ID))
                        {
                            mehesCustomsItemDetailsHistoryRows.Add(mehesCustomsItemDetailsHistoryRow_CustomsItemNOTExists);
                        }
                    }
                }
            }

            return mehesCustomsItemDetailsHistoryRows;
        }

        private List<CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem> GetMehesCustomsItemRows(CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem[] cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem)
        {
            if (cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem == null)
            {
                return null;
            }
            return cBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem.Where(rec => (rec.CustomsBookTypeID >= 1 && rec.CustomsBookTypeID <= 3)).ToList();
        }

        private static bool CompareMehesToDBCustomsItem(CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItem mehesCustomsItemRow, CustomsItemPM currentDBListCustomsItemRow)
        {
            if (mehesCustomsItemRow.FullClassification != currentDBListCustomsItemRow.FullClassification ||
                mehesCustomsItemRow.CustomsItemCategoryID != currentDBListCustomsItemRow.CustomsItemCategoryID ||
                mehesCustomsItemRow.CustomsItemHierarchicLocationID != currentDBListCustomsItemRow.CustomsItemHierarchicLocationID ||
                mehesCustomsItemRow.ComputedCheckDigit != currentDBListCustomsItemRow.ComputedCheckDigit)
            {
                return true;
            }

            return false;
        }

        private static bool CompareMehesToDBCustomsItemDetailsHistory(CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesCustomsItemDetailsHistory mehesCustomsItemDetailsHistoryRow, CustomsItemDetailsHistoryPM currentDBListCustomsItemDetailsHistoryRow)
        {
            if (mehesCustomsItemDetailsHistoryRow.Title != currentDBListCustomsItemDetailsHistoryRow.Title ||
                mehesCustomsItemDetailsHistoryRow.StartDate != currentDBListCustomsItemDetailsHistoryRow.StartDate ||
                mehesCustomsItemDetailsHistoryRow.EndDate != currentDBListCustomsItemDetailsHistoryRow.EndDate ||
                mehesCustomsItemDetailsHistoryRow.EntityStatusID != currentDBListCustomsItemDetailsHistoryRow.EntityStatusID)
            {
                return true;
            }

            return false;
        }
        private static bool CompareMehesToDBPropertiesDetailsHistory(CBC_NG_8362_MSG01_CustomsBookOutCustomsBookGeneralTablesPropertiesDetailsHistory mehesPropertiesDetailsHistoryRow, PropertiesDetailsHistoryPM currentDBListPropertiesDetailsHistoryRow)
        {
            if (mehesPropertiesDetailsHistoryRow.StartDate != currentDBListPropertiesDetailsHistoryRow.StartDate ||
                mehesPropertiesDetailsHistoryRow.EndDate != currentDBListPropertiesDetailsHistoryRow.EndDate ||
                mehesPropertiesDetailsHistoryRow.EntityStatusID != currentDBListPropertiesDetailsHistoryRow.EntityStatusID ||
                mehesPropertiesDetailsHistoryRow.MeasurementUnitID != currentDBListPropertiesDetailsHistoryRow.MeasurementUnitID)
            {
                return true;
            }

            return false;
        }


        public override CustomsBookInResponseData GetResponse(CBC_NG_8362_MSG01_CustomsBookOut customResponse, CustomsBookInRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private string GetErrosXmlFromResponseHeaderExeption()
        {
            return _ResponseHeaderExeption.ErrorDescription;
        }

        private string GetDummyXml(string message)
        {
            var myDummyXml = new GeneralMessage() { Message = message };
            var xml = XmlGenericUtil<GeneralMessage>.SerializeObject(myDummyXml);
            return xml;
        }

        public class GeneralMessage
        {
            public string Message { get; set; }
            //public CBC_NG_8362_MSG01_CustomsBookOut Response { get; set; }
        }
    }
}