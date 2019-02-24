using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.StimulReport.Mapping;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments
{
    public class DeclarationDocumentsService : UnifreightGenericService
    {
        private LOGIDOCS _LOGIDOCS;
        private LogitudeDocs _LogitudeDocs;
        private Def.EntityPMs.SupplierInvoicePM _MySupplierInvoicePM;
        private ICustomContext _context;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments.DeclarationDocumentsService.Upsert()";
        private DeclarationPM _MyDeclarationPM;
        private Stopwatch _Stopwatch;

        public DeclarationDocumentsService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGIDOCS,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();  
            MyCommunicationsParams.Subject = "DeclarationDocumentsService ";

            DeserilazeObject(xmlLOGIDOCS);
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
            
            CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString());_Stopwatch.Restart(); 
            MyGenericResponseObj.Stage = "GetContext";
            _context = CustomContext.GetContext(ResolvedTenant());
            var myQueryService = new DeclarationQueryService(_context);

            MyGenericResponseObj.Stage = "GetSingle";
            this._MyDeclarationPM = myQueryService.GetSingle(this._LogitudeDocs.Id, true, false);
            if (this._MyDeclarationPM == null)
            {
                throw new BusinessErrorException("Id is " + this._LogitudeDocs.Id + " but not found");
            }
            AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart(); 
            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            MyGenericResponseObj.Stage = "Add Ticket for file " + this._MyDeclarationPM.CustomFileNo;

            if (!String.IsNullOrWhiteSpace(this._LogitudeDocs.COM_ID))
            {
                var myCustomsDocumentQueryService = new CustomsDocumentQueryService(dbContext);
                var myCustomsDocumentPointerUpdateService = new CustomsDocumentPointerUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(_MyDeclarationPM.Tenant);
                DocumentsFilingPM documentIn = documentsFilingQuery.GetSinglePM(this._LogitudeDocs.COM_ID, _MyDeclarationPM.Tenant);
                if (documentIn != null)
                {
                    if (this._LogitudeDocs.DOC_ID == null)
                    {
                        throw new BusinessErrorException("DOC_ID is missing");
                    }
                    CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_MyDeclarationPM.Tenant);
                    List<CustomsDocumentsTicketPM> myCustomsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketsByDocumentsFilingId(this._LogitudeDocs.COM_ID, _MyDeclarationPM.Tenant);
                    if(myCustomsDocumentsTicketPMList != null && myCustomsDocumentsTicketPMList.Count() > 0)
                    {
                        List<string> ticketdIds = myCustomsDocumentsTicketPMList.Select(r => r.Id).ToList();
                        if(ticketdIds != null && ticketdIds.Count() > 0)
                        {
                            CustomsDocumentPointerQueryService myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(_MyDeclarationPM.Tenant);
                            List<CustomsDocumentPointerPM> myCustomsDocumentPointerPMList = myCustomsDocumentPointerQueryService.GetPointersForMultipleTickets(ticketdIds, _MyDeclarationPM.Tenant);
                            if (myCustomsDocumentPointerPMList != null && myCustomsDocumentPointerPMList.Count() > 0)
                            {
                                var myCustomsDocumentPointerPMListforDec = myCustomsDocumentPointerPMList.Where(o => o.ParentEntityCode == "Declaration" && o.ParentEntityId == _MyDeclarationPM.Id);
                                if(myCustomsDocumentPointerPMListforDec != null && myCustomsDocumentPointerPMListforDec.Count() > 0)
                                {
                                    throw new BusinessErrorException("Ticket already Exist for this Document");
                                }
                            }
                        }
                    }
                    //List<CustomsDocumentsTicketPM> myCustomsDocumentsTicketPMList2 = myCustomsDocumentsTicketQueryService.
                    

                    string pointerLevel = null;
                    var myDocumentTypeCustomsDataQueryService = new DocumentTypeCustomsDataQueryService(dbContext);
                    var myDocumentTypeCustomsData = myDocumentTypeCustomsDataQueryService.GetSingle(this._LogitudeDocs.DOC_ID, true, true);
                    if (myDocumentTypeCustomsData == null || String.IsNullOrWhiteSpace(myDocumentTypeCustomsData.CustomsDoucumentTypeCode))
                    {
                        throw new BusinessErrorException("DOC_ID " + this._LogitudeDocs.DOC_ID + " but not found");
                    }
                    CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();
                    customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Insert;
                    customsDocumentsTicketPM.Tenant = _MyDeclarationPM.Tenant;
                    customsDocumentsTicketPM.DocumentsFilingId = this._LogitudeDocs.COM_ID;
                    if (myDocumentTypeCustomsData != null && !String.IsNullOrWhiteSpace(myDocumentTypeCustomsData.CustomsDoucumentTypeCode))
                    {
                        customsDocumentsTicketPM.DocumentTypeCode = myDocumentTypeCustomsData.CustomsDoucumentTypeCode;
                        var myCustomDocumentTypeQueryService = new CustomDocumentTypeQueryService(dbContext);
                        var myCustomDocumentType = myCustomDocumentTypeQueryService.GetSingle(myDocumentTypeCustomsData.CustomsDoucumentTypeCode, true, true);
                        if (myCustomDocumentType != null && !String.IsNullOrWhiteSpace(myCustomDocumentType.PointerLevel))
                        {
                            pointerLevel = myCustomDocumentType.PointerLevel;
                        }
                    }
                    myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);



                    CustomsDocumentPointerPM customsDocumentPointerPM = new CustomsDocumentPointerPM();
                    customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Insert;

                    // Create Document Pointer (every document pointer has a ticket)
                    customsDocumentPointerPM.Tenant = _MyDeclarationPM.Tenant;
                    customsDocumentPointerPM.CustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                    customsDocumentPointerPM.ParentEntityCode = "Declaration";
                    customsDocumentPointerPM.ParentEntityId = _MyDeclarationPM.Id;
                    if (pointerLevel == "I")
                    {
                        customsDocumentPointerPM.Child1EntityCode = "SupplierInvoice";
                        customsDocumentPointerPM.Child1EntityId = "1";
                    }
                    if (pointerLevel == "P")
                    {

                        customsDocumentPointerPM.Child2EntityCode = "SupplierInvoiceItem";
                        customsDocumentPointerPM.Child2EntityId = "1";
                    }

                    customsDocumentPointerPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                    myCustomsDocumentPointerUpdateService.Update(customsDocumentPointerPM, true);

                    var myDocumentId = myCustomsDocumentQueryService.GetSingle(this._LogitudeDocs.COM_ID, true, false);
                    CustomsDocumentPM customsDocumentPM;
                    var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(dbContext, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                    myCustomsDocumentUpdateService.OnCreating_InsertPerfectCustomsDocumentMetaDataValues = true;
                    CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQuery = new CustomsDocumentMetaDataValueQueryService(_context);
                    List<CustomsDocumentMetaDataValuePM> CustomsDocumentMetaDataValues = customsDocumentMetaDataValueQuery.GetCustomsDocumentMetaDataValuesByConnectedEntity(_MyDeclarationPM.Id, _MyDeclarationPM.Tenant);
                    //if(CustomsDocumentMetaDataValues.Where(r => r.MetaDataValue == "1" && r.MetaDataTypeCode == "380").FirstOrDefault() == null)
                    //{
                    //    CustomsDocumentMetaDataValues.Add(new CustomsDocumentMetaDataValuePM { CustomsDocumentId = this._LogitudeDocs.COM_ID, MetaDataTypeCode = "380", MetaDataValue = "1", Tenant = _MyDeclarationPM.Tenant, ChangeSetOp = ChangeSetOperation.Insert });

                    //}
                    if (myDocumentId == null)
                    {
                        customsDocumentPM = new CustomsDocumentPM();
                        customsDocumentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        customsDocumentPM.IsSendToQueue = true;
                        customsDocumentPM.DocumentsFilingId = this._LogitudeDocs.COM_ID;
                        customsDocumentPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                        customsDocumentPM.CurrentCustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                        customsDocumentPM.Tenant = _MyDeclarationPM.Tenant;
                        customsDocumentPM.CustomsDocumentMetaDataValues = CustomsDocumentMetaDataValues;
                        foreach (CustomsDocumentMetaDataValuePM value in customsDocumentPM.CustomsDocumentMetaDataValues)
                        {
                            value.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                    }
                    else
                    {
                        customsDocumentPM = myDocumentId;
                        customsDocumentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        customsDocumentPM.IsSendToQueue = true;
                        if (customsDocumentPM.DocumentsFilingId != this._LogitudeDocs.COM_ID) customsDocumentPM.DocumentsFilingId = this._LogitudeDocs.COM_ID;
                        if (customsDocumentPM.DocumentTypeCode != customsDocumentsTicketPM.DocumentTypeCode) customsDocumentPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                        if (customsDocumentPM.CurrentCustomsDocumentsTicketId != customsDocumentsTicketPM.Id) customsDocumentPM.CurrentCustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                    }
                    if (CustomsDocumentMetaDataValues != null && CustomsDocumentMetaDataValues.Count() > 0 && (customsDocumentPM.CustomsDocumentMetaDataValues == null || customsDocumentPM.CustomsDocumentMetaDataValues.Count() < CustomsDocumentMetaDataValues.Count())) customsDocumentPM.CustomsDocumentMetaDataValues = CustomsDocumentMetaDataValues;
                    myCustomsDocumentUpdateService.Update(customsDocumentPM, true);
                }
                else
                {
                    throw new BusinessErrorException("Document with Id " + this._LogitudeDocs.COM_ID + " not found");
                }
            }

            MyGenericResponseObj.Stage = "Add Ticket Done ";
            AppendLogLine("Add Ticket:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
            //MyGenericResponseObj.ResponseXml ;
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;
        }

        private void DeserilazeObject(string xmlLOGIDOCS)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("DeclarationDocumentsService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGIDOCS))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGIDOCS.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGIDOCS.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGIDOCS);
            }

            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGIDOCS = XmlGenericUtil<LOGIDOCS>.DeSerializeObject(xmlLOGIDOCS);

            if (_LOGIDOCS.LogitudeDocs == null || _LOGIDOCS.LogitudeDocs.Length != 1)
            {
                throw new BusinessErrorException("_LOGIDOCS.DeclarationDocuments.Length != 1");
            }
            this._LogitudeDocs = _LOGIDOCS.LogitudeDocs[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudeDocs.Id))
            {
                throw new BusinessErrorException("Id is missing");
            }
            AppendLogLine("Id = " + this._LogitudeDocs.Id);
        }

        

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGIDOCS();
            var myAmitalDocs = new LogitudeDocs();
           

            myAmitalDocs.Id = "1-1";
            myAmitalDocs.Tenant = "1";
            
            amitalObjExample.LogitudeDocs = new LogitudeDocs[] { myAmitalDocs };

            xml = XmlGenericUtil<LOGIDOCS>.SerializeObject(amitalObjExample);

            return xml;
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }
    
    }
}

