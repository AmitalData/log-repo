using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CustomsMessaging.Testers.LoadTest
{
    public class CreateCustomFileService
    {
        void testc()
        {
            //var a =  UnifreightIIG.Common.WCOResource.DB;
            //var a1 = UnifreightIIG.Common.WCOResource.DBManifest_18;


        }
        public string SendHybridInterface(ShipmentAM shipmentAM)
        {
            //var shipmentAM  = GetShipmentAM();
            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
              Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.AnalyzeStandard, "", "")
            {
                Tenant = 1,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = shipmentAM.Id,
                EntityId = shipmentAM.Id,
                UserId = "",
                CommunicationSubject = "Hybrid Create Declaration /Custom File",
                special_instruction = new AmitalMessaging.Infrastructure.Transmission.special_instructions()
                {
                    //@internal="Automatic Envelope use convert",
                    convert = new AmitalMessaging.Infrastructure.Transmission.convert() { Value = "CFIFDIGITAL" },
                    flat_file = "yes"

                    /*
                     <internal>Automatic Envelope use convert </internal>
                       <uworkj></uworkj>
                       <convert>CFIFDIGITAL</convert>
                       <flat_file>yes</flat_file>
                       <prefix></prefix>
                       <more_params></more_params>
                       <separate_analyze></separate_analyze>
                       <split></split>
                     */
                }
                //LogitudeFile = myFile,
            };
            amitalCustomFileCommunicationModel.special_instruction.@internal = new AmitalMessaging.Infrastructure.Transmission.@internal() { Value = "Automatic Envelope use convert" };
            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService<
                Customs.BL.Messaging.Amital.AmitalCommunicationModelBase,
                ShipmentAM>
                (amitalCustomFileCommunicationModel, shipmentAM);
            var info = myUServerCommunicationService.Send(true);
            if (!string.IsNullOrWhiteSpace(info.ImmediatelyResponse))
            {
                var responseHSH = UnifreightListsUtil.Deserialize(info.ImmediatelyResponse);
                var entityValue = UnifreightListsUtil.GetValue(ref responseHSH, "ENTITY");
                /*
 <Entry>
  <Key>ENTITY</Key>
  <Value>CFIFILEM\;171900034</Value>
 </Entry>

                 */
                var fileNo = entityValue.Split(';')[1];
                return fileNo;

            }
            return null;

        }
        public ShipmentAM GetShipmentAM(string ConsigneeId = "1000", string CustomerId = "10009065")
        {
            var seedTenant = -1;
            var currId = Guid.NewGuid().ToString();
            var hash = currId.GetHashCode().ToString();
            var shipmentAM = new ShipmentAM()
            {
                Id = hash,
                Tenant = seedTenant,
                ImporterTenant = seedTenant,
                TransportModeId = "O",
                DirectionId = "C",
                ShipmentLevelCode = "A",
                CustomerShipmentNumber = hash,//2712
                Consignee = new Simplog.Server.Infrastructure.DataContracts.CodeProperties() { Code = ConsigneeId },
                StatusCode = "",//INPR
                StatusDate = DateTime.Now,
                FromPort = new Simplog.Server.Infrastructure.DataContracts.CodeProperties() { Code = "DUR", CountryCode = "ZA" },
                ToPort = new Simplog.Server.Infrastructure.DataContracts.CodeProperties() { Code = "HFA", CountryCode = "IL" },
                Customer = new Simplog.Server.Infrastructure.DataContracts.CodeProperties() { Code = CustomerId },
                ShipperName = "",//Granor Passi
                CustomerReference1 = "",//< CustomerReference1 > 396 - 1 - 10428 </ CustomerReference1 >
                ShipmentCustomerTypeCode = "",//< ShipmentCustomerTypeCode > SHI </ ShipmentCustomerTypeCode >
                IsCancelled = false,//< IsCancelled > false </ IsCancelled >
                FreightPrepaidCollectId = "C",//< FreightPrepaidCollectId > C </ FreightPrepaidCollectId >
                OtherPrepaidCollectId = "C",// < OtherPrepaidCollectId > C </ OtherPrepaidCollectId >
                IsOperationalClosed = false,//< IsOperationalClosed > false </ IsOperationalClosed >
                                            //< OriginalStatusDate xsi:nil = "true" />
                                            //< HasException > false </ HasException >
                                            //< ExceptionDate xsi: nil = "true" />
                                            ////  < IsImporterApprovalRequired > false </ IsImporterApprovalRequired >
                                            //  < ApproveDateTime xsi: nil = "true" />
                                            //Notes ="Loadtest ",//< Notes > B / L is SWB</ Notes >
                                            //< ShipmentAddtionalDataXML >< PLForwarding > false </ PLForwarding ></ ShipmentAddtionalDataXML >
                                            //< ForwarderCode > 15 </ ForwarderCode >


                //< Quantity > 2 </ Quantity >
                //< Weight > 53526 </ Weight >

                ShipmentTypeId = "FCL"
            };


            return shipmentAM;
        }

        public void ConnectTicket(int tenant, string declarationId)
        {
            using (var scope = TransactionFactory.GetNewTransaction())
            {


                ICustomContext dbContext = CustomContext.GetContext(tenant);
                var myCustomsDocumentQueryService = new CustomsDocumentQueryService(dbContext);
                var documentFilingRep = new DocumentsFilingRepository(tenant);
                var documentsFilingList = documentFilingRep.GetDocumentsFilingsByEntityId_noInclude(declarationId, tenant);
                if (documentsFilingList.Count < 1)
                {
                    throw new Exception("No documentsFilingList  for dec id  =" + declarationId);
                }
                var myFiling = documentsFilingList.FirstOrDefault();

                //var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(dbContext);
                var myCustomsDocumentPointerUpdateService = new CustomsDocumentPointerUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
                //var myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(dbContext);
                var myCustomsDocumentsTicketUpdateService = new CustomsDocumentsTicketUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);
                var myCustomsDocumentUpdateService = new CustomsDocumentUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);

                var customsDocumentPM = myCustomsDocumentQueryService.GetSingle(myFiling.Id, true, false);
                if (customsDocumentPM == null)
                {

                    customsDocumentPM = new CustomsDocumentPM();
                    customsDocumentPM.ChangeSetOp = ChangeSetOperation.Insert;
                    customsDocumentPM.DocumentsFilingId = myFiling.Id;
                    customsDocumentPM.DocumentTypeCode = "380";// myFiling.DocumentTypeId;
                    customsDocumentPM.Tenant = tenant;
                    //customsDocumentPM.CurrentCustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                    foreach (CustomsDocumentMetaDataValuePM value in customsDocumentPM.CustomsDocumentMetaDataValues)
                    {
                        value.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    myCustomsDocumentUpdateService.Update(customsDocumentPM, true);
                }


                CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();
                customsDocumentsTicketPM.ChangeSetOp = ChangeSetOperation.Insert;
                customsDocumentsTicketPM.Tenant = tenant;
                customsDocumentsTicketPM.DocumentsFilingId = myFiling.Id;//  customsDocument.COM_ID;
                customsDocumentsTicketPM.DocumentTypeCode = "380";// myFiling.DocumentTypeId; ; //customsDocument.DocumentTypeCode;
                myCustomsDocumentsTicketUpdateService.Update(customsDocumentsTicketPM, true);

                var customsDocumentPointerPM = new CustomsDocumentPointerPM();
                customsDocumentPointerPM.ChangeSetOp = ChangeSetOperation.Insert;

                // Create Document Pointer (every document pointer has a ticket)
                customsDocumentPointerPM.Tenant = tenant;
                customsDocumentPointerPM.CustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                customsDocumentPointerPM.ParentEntityCode = "Declaration";
                customsDocumentPointerPM.ParentEntityId = declarationId;

                customsDocumentPointerPM.Child1EntityCode = "SupplierInvoice";
                customsDocumentPointerPM.Child1EntityId = "1"; // customsDocument.Key_2;
                customsDocumentPointerPM.DocumentTypeCode = customsDocumentsTicketPM.DocumentTypeCode;
                myCustomsDocumentPointerUpdateService.Update(customsDocumentPointerPM, true);

                customsDocumentPM.ChangeSetOp = ChangeSetOperation.Update;
                customsDocumentPM.CurrentCustomsDocumentsTicketId = customsDocumentsTicketPM.Id;
                myCustomsDocumentUpdateService.Update(customsDocumentPM, true);

                scope.Complete();
            }
        }

        public string DeclarationUpsert(string fileNo, string filingCopy)
        {
            var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
               Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,
               "CWSFLTEST", "DeclarationUpsert")
            {
                Tenant = 1,
                objectTableName = "Customs.Declaration",
                CommunicationLoggingEntityReference = fileNo,
                EntityId = fileNo,
                UserId = "",
                CommunicationSubject = "DeclarationUpsert",

                //LogitudeFile = myFile,
            };


            var myDictionary = new Dictionary<string, string>();
            myDictionary.Add("FILE_NO", fileNo);
            myDictionary.Add("FilingCopy", "pkajungqyegfkg6hhqjizw00000000");//COM_ID=pkajungqyegfkg6hhqjizw00000000


            var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, Dictionary<string, string>>(
                amitalCustomFileCommunicationModel, myDictionary);
            var info = myUServerCommunicationService.Send(true);

            return info.GenericResponseObj.ApplicationId;
        }

        public void PutCopyDeclaration(string fromDeclarationId, string toDeclarationId)
        {
            //DeclarartionController.PutCopyDeclaration
            var tenant = 1;
            var customContext = CustomContext.GetContext(tenant);
            var service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
            service.CopyDeclaration(fromDeclarationId, toDeclarationId, tenant);

        }

        public string GetDeclarationId(string fileNo)
        {
            throw new NotImplementedException();
        }

        public void DeletSI(object declarationId)
        {
            throw new NotImplementedException();
        }

        public void PostPrintRequestRequest(string declarationId)
        {
            //  CustomMessageProgressComponent
            //.ShowProgressBar(currRequestParams.PBId,
            //"שליחת שאילתא להדפסת הצהרה", true)
            //.then((res) => {
            //    this.ResponseData = res;
            //    this.OnMassageDisplayMethod();
            //}
            //).catch ((err) => {
            //    this.ValidationErrorsList.push(err);
            //});


            //  this._DeclarationMessagesService.PostPrintRequestRequest(currRequestParams)
            //      .subscribe((myServiceResponse: ServiceResponse) => {
            //  });

        }

        public void SendDeclaration(string declarationId)
        {
            //  CustomMessageProgressComponent
            //    .ShowProgressBar(searchParams.PBId,
            //    "שליחת הצהרת יבוא", false)
            //    .then((res) => {
            //        this.ResponseData = res;
            //        SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            //    }
            //).catch ((err) => {
            //    SessionLocator.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            //    this.ValidationErrors.push(err);
            //    this.FillValidationErrors("Errors");
            //});

            //  this.DeclarationService.PostSendDeclaration(searchParams).subscribe((response: ServiceResponse) => {
            //      //SessionLocator.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            //  });
            //  }
        }

    }
}
