using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityKeys;
using System.Configuration;
using System.Globalization;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.BL.EntityQueryServices;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.BL.BL;
using System.Diagnostics;
using Logitude.Server.Tools.Contracts;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.Messaging.ILSWS;
using Logitude.Customs.BL.Messaging.Maman;
using System.Data.Entity.Validation;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging.ILOVS;


namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationCourierStatusUpdateService //: EntityUpdateService<DeclarationCourierStatus, DeclarationCourierStatusPM, DeclarationPM>
    {
        //public bool IsAfterUpdatingUpdate { get; set; }
        public string TruckerId;
        public string DistributionArea;
        public string LastMileServiceType;
        public string MAWB;
        public string ImporterCode;

        protected override void OnCreating(DeclarationCourierStatusPM entityPM, EntityPM entityParentPM)
        {
            //if (entityParentPM != null)
            //{
            //    entityPM.DeclarationId = entityParentPM.Id;
            //    entityPM.Tenant = entityParentPM.Tenant;
            //}
        }

        protected override void OnUpdating(DeclarationCourierStatusPM entityPM, DeclarationCourierStatus entityPOCO)
        {
            var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            if (setting != null & setting.IsConnectedToUniFreight)
            {
                var declarationCourierStatusRepository = new DeclarationCourierStatusRepository(entityPM.Tenant);
                declarationCourierStatusRepository.Lock_forUpdateNOWAIT(entityPM.DeclarationId);
            }
            ICustomContext context = MainContext as CustomContext;
            if (entityPM != null)
            {
                var prevCourierPendingReasonList = entityPM.CourierPendingReasonList;
                entityPM.CourierPendingReasonList = null;
                entityPM.NotApprovedPendingList = null;
                foreach (var declarationPending in entityPM.DeclarationPendings)
                {
                    CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(entityPM.Tenant);
                    var courierPendingReason = courierPendingReasonRepositoryRepository.GetByCode(declarationPending.CourierPendingReasonCode, entityPM.Tenant);
                    if (courierPendingReason != null && !courierPendingReason.Inactive)
                    {
                        if (declarationPending.ChangeSetOp != ChangeSetOperation.Delete)
                        {
                            if (declarationPending.Status == "A")
                            {
                                if (entityPM.CourierPendingReasonList == null)
                                {
                                    entityPM.CourierPendingReasonList = declarationPending.CourierPendingReasonCode;
                                }
                                else
                                {
                                    entityPM.CourierPendingReasonList = string.Concat(entityPM.CourierPendingReasonList, ",", declarationPending.CourierPendingReasonCode);
                                }
                            }
                            if (declarationPending.Status == "A" && declarationPending.Approval != true && courierPendingReason.RequiresApproval == true)
                            {
                                if (entityPM.NotApprovedPendingList == null)
                                {
                                    entityPM.NotApprovedPendingList = declarationPending.CourierPendingReasonCode;
                                }
                                else
                                {
                                    entityPM.NotApprovedPendingList = string.Concat(entityPM.NotApprovedPendingList, ",", declarationPending.CourierPendingReasonCode);
                                }
                            }
                        }
                    }


                }

                if (entityPM.CourierPendingReasonList != entityPOCO.CourierPendingReasonList && entityPM.CourierCustomStatusCode == null)
                {
                    this.SendToMassof(entityPM, entityPOCO);
                }
            }
            if (entityPM.IsClosedForFollowUp != entityPOCO.IsClosedForFollowUp)
            {



                CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(entityPM.Tenant);
                var courierMasterPM = courierMasterQueryService.GetByDeclarationId(entityPM.DeclarationId, entityPM.Tenant);
                var repository = new CardRepository(entityPM.Tenant);
                if (setting != null & !setting.StandAlone)
                {
                    if (courierMasterPM != null)
                    {
                        var myCard = repository.GetSingleCard(courierMasterPM.IntegratorCode, entityPM.Tenant);
                        if (myCard != null && !String.IsNullOrWhiteSpace(myCard.Code))
                        {
                            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(entityPM.Tenant);

                            string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_GDPR_PRIVAC", "NON", myCard.Code, entityPM.Tenant);
                            if (defValue == "Y")
                            {
                                if (entityPM.IsClosedForFollowUp)
                                {
                                    DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                                    var dec = declarationQueryService.GetSingleDeclarationById(entityPM.DeclarationId, entityPM.Tenant);
                                    var casual = new DeclarationCasualDetailsPM
                                    {
                                        ImporterCode = dec.ImporterCode,
                                        TransferImporterCode = dec.TransferImporterCode,
                                        EntitleImporterCode = dec.EntitleImporterCode,
                                        ImporterName = dec.ImporterName,
                                        ImporterAddress = dec.ImporterAddress,
                                        TransferImporterName = dec.TransferImporterName,
                                        TransferImporterAddress = dec.TransferImporterAddress,
                                        EntitleImporterName = dec.EntitleImporterName,
                                        EntitleImporterAddress = dec.EntitleImporterAddress,
                                        ImporterPassportNumber = dec.ImporterPassportNumber,
                                        EntitlePassportNumber = dec.EntitlePassportNumber,
                                        TransferPassportNumber = dec.TransferPassportNumber,
                                        CasualImporterAddress1 = dec.CasualImporterAddress1,
                                        CasualImporterAddress2 = dec.CasualImporterAddress2,
                                        CasualImporterCity = dec.CasualImporterCity,
                                        CasualImporterZipCode = dec.CasualImporterZipCode,
                                        CasualImporterFax = dec.CasualImporterFax,
                                        CasualImporterEmail = dec.CasualImporterEmail,
                                        CasualImporterTel = dec.CasualImporterTel,
                                        CasualImporterContact = dec.CasualImporterContact,
                                        DeclarationId = dec.Id,
                                        Tenant = dec.Tenant,
                                        //CasualSupplierName = dec.CasualSupplierName,
                                        //CasualSupplierAddress = dec.CasualSupplierAddress,
                                    };
                                    casual.ChangeSetOp = ChangeSetOperation.Insert;
                                    DeclarationCasualDetailsUpdateService declarationCasualDetailsUpdateService = new DeclarationCasualDetailsUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                                    declarationCasualDetailsUpdateService.Update(casual, true);

                                    //dec.CasualSupplierName = null;
                                    // dec.CasualSupplierAddress = null;
                                    dec.ImporterCode = null;
                                    dec.TransferImporterCode = null;
                                    dec.EntitleImporterCode = null;
                                    dec.ImporterName = null;
                                    dec.ImporterAddress = null;
                                    dec.TransferImporterName = null;
                                    dec.TransferImporterAddress = null;
                                    dec.EntitleImporterName = null;
                                    dec.EntitleImporterAddress = null;
                                    dec.ImporterPassportNumber = null;
                                    dec.EntitlePassportNumber = null;
                                    dec.TransferPassportNumber = null;
                                    dec.CasualImporterAddress1 = null;
                                    dec.CasualImporterAddress2 = null;
                                    dec.CasualImporterCity = null;
                                    dec.CasualImporterZipCode = null;
                                    dec.CasualImporterFax = null;
                                    dec.CasualImporterEmail = null;
                                    dec.CasualImporterTel = null;
                                    dec.CasualImporterContact = null;
                                    dec.ChangeSetOp = ChangeSetOperation.Update;
                                    //DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                                    //declarationUpdateService.Update(dec, true);
                                    DeclarationUpdateService.DeclarationRepositoryUpdatePOCO(dec, true);
                                }
                                else
                                {
                                    DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                                    var dec = declarationQueryService.GetSingleDeclarationById(entityPM.DeclarationId, entityPM.Tenant);
                                    DeclarationCasualDetailsQueryService declarationCasualDetailsQueryService = new DeclarationCasualDetailsQueryService(entityPM.Tenant);
                                    var casual = declarationCasualDetailsQueryService.GetSingle(entityPM.DeclarationId, false, false);
                                    if (casual != null)
                                    {
                                        //dec.CasualSupplierName = casual.CasualSupplierName;
                                        //dec.CasualSupplierAddress = casual.CasualSupplierAddress;
                                        dec.ImporterCode = casual.ImporterCode;
                                        dec.TransferImporterCode = casual.TransferImporterCode;
                                        dec.EntitleImporterCode = casual.EntitleImporterCode;
                                        dec.ImporterName = casual.ImporterName;
                                        dec.ImporterAddress = casual.ImporterAddress;
                                        dec.TransferImporterName = casual.TransferImporterName;
                                        dec.TransferImporterAddress = casual.TransferImporterAddress;
                                        dec.EntitleImporterName = casual.EntitleImporterName;
                                        dec.EntitleImporterAddress = casual.EntitleImporterAddress;
                                        dec.ImporterPassportNumber = casual.ImporterPassportNumber;
                                        dec.EntitlePassportNumber = casual.EntitlePassportNumber;
                                        dec.TransferPassportNumber = casual.TransferPassportNumber;
                                        dec.CasualImporterAddress1 = casual.CasualImporterAddress1;
                                        dec.CasualImporterAddress2 = casual.CasualImporterAddress2;
                                        dec.CasualImporterCity = casual.CasualImporterCity;
                                        dec.CasualImporterZipCode = casual.CasualImporterZipCode;
                                        dec.CasualImporterFax = casual.CasualImporterFax;
                                        dec.CasualImporterEmail = casual.CasualImporterEmail;
                                        dec.CasualImporterTel = casual.CasualImporterTel;
                                        dec.CasualImporterContact = casual.CasualImporterContact;
                                        dec.ChangeSetOp = ChangeSetOperation.Update;
                                        //DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                                        //declarationUpdateService.Update(dec, true);

                                        casual.ChangeSetOp = ChangeSetOperation.Delete;
                                        DeclarationCasualDetailsUpdateService declarationCasualDetailsUpdateService = new DeclarationCasualDetailsUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                                        declarationCasualDetailsUpdateService.Update(casual, true);
                                        DeclarationUpdateService.DeclarationRepositoryUpdatePOCO(dec, true);

                                    }
                                }
                            }
                        }
                    }
                }
                DeclarationCourierStatusRepository rep = new DeclarationCourierStatusRepository(context);
                CourierMasterUpdateService CourierMasterUpdateService = new CourierMasterUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                bool useCRS = true;
                if (useCRS)
                {
                    if (courierMasterPM != null)
                    {
                        IUpdateOpenDeclarationInCourierMasterService myIUpdateOpenDeclarationInCourierMasterService = ContainerAccessor.Container.Resolve(typeof(IUpdateOpenDeclarationInCourierMasterService), "UpdateOpenDeclarationInCourierMasterService", new ParameterOverride("", entityPM.Tenant)) as IUpdateOpenDeclarationInCourierMasterService;
                        myIUpdateOpenDeclarationInCourierMasterService.UpdateOpenDeclarationInCourierMaster(entityPM.Tenant, courierMasterPM.Id, null);
                    }
                }
                else
                {


                    courierMasterPM.OpenDeclarations = rep.CountOpenDeclarations(courierMasterPM.Id, courierMasterPM.Tenant);
                    if (entityPM.IsClosedForFollowUp && !entityPOCO.IsClosedForFollowUp)
                    {
                        courierMasterPM.OpenDeclarations -= 1;
                    }
                    if (!entityPM.IsClosedForFollowUp && entityPOCO.IsClosedForFollowUp)
                    {
                        courierMasterPM.OpenDeclarations += 1;
                    }
                    courierMasterPM.ChangeSetOp = ChangeSetOperation.Update;
                    //if (!entityPM.IsClosedForFollowUp && !courierMasterPM.IsOpen)courierMasterPM.IsOpen = true;
                    CourierMasterUpdateService.Update(courierMasterPM, true);
                }
            }

            DateTime stopLogAt = new DateTime(2020, 03, 01);
            Debug.WriteLine("DeclarationCourierStatusUpdateServiceOnUpdating");
            string logData = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(entityPOCO.CourierPaymentStatusCode) && String.IsNullOrWhiteSpace(entityPM.CourierPaymentStatusCode))
                {
                    logData = $"CourierPaymentStatusCode was {entityPOCO.CourierPaymentStatusCode}, and changed to null";
                    LogitudeSettings.HandleLogMe("CourierPaymentStatusCode " + logData, false, "DeclarationCourierStatus.CourierPaymentStatusCode", stopLogAt);
                    Debug.WriteLine("CourierPaymentStatusCode==null");
                }
                if (!String.IsNullOrWhiteSpace(entityPOCO.DocumentStatusCode) && String.IsNullOrWhiteSpace(entityPM.DocumentStatusCode))
                {
                    logData += $"DocumentStatusCode was {entityPOCO.DocumentStatusCode}, and changed to null";
                    LogitudeSettings.HandleLogMe("DocumentStatusCode " + logData, false, "DeclarationCourierStatus.DocumentStatusCode", stopLogAt);
                    Debug.WriteLine("DocumentStatusCode==null");
                }
                if (!String.IsNullOrWhiteSpace(entityPOCO.CourierDeclarationStatusCode) && String.IsNullOrWhiteSpace(entityPM.CourierDeclarationStatusCode))
                {
                    logData += $"CourierDeclarationStatusCode was {entityPOCO.CourierDeclarationStatusCode}, and changed to null";
                    LogitudeSettings.HandleLogMe("CourierDeclarationStatusCode " + logData, false, "DeclarationCourierStatus.CourierDeclarationStatusCode", stopLogAt);
                    Debug.WriteLine("CourierDeclarationStatusCode==null");
                }
            }
            catch (Exception E)
            {

                LogitudeSettings.HandleLogMe(E.ToString() + logData, true, "DeclarationCourierStatusUpdateServiceOnUpdating", stopLogAt);
                throw;
            }
            finally
            {

            }

            UpdateUnifreight(entityPM);

            base.OnUpdating(entityPM, entityPOCO);
        }
      

        protected override void UpdateComposition(DeclarationCourierStatusPM entityPM)
        {
            DeclarationPendingUpdateService declarationPendingUpdateService = new DeclarationPendingUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            declarationPendingUpdateService.IsUpdateComposition = true;
            declarationPendingUpdateService.UpdateMulti(entityPM.DeclarationPendings.Where(p => p.ChangeSetOp == ChangeSetOperation.Delete).ToList(), entityPM.DeletedDeclarationPendings, entityPM, true);
            declarationPendingUpdateService.UpdateMulti(entityPM.DeclarationPendings.Where(p => p.ChangeSetOp != ChangeSetOperation.Delete).ToList(), entityPM.DeletedDeclarationPendings, entityPM, true);
            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(DeclarationCourierStatusPM entityPM, EntityPM entityParentPM)
        {
            LogMessagingUtil.Instance.AppendLine("DeclarationCourierStatusPM.DocumentStatusCode: " + entityPM.DocumentStatusCode);
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            //(Repository as Logitude.Customs.Data.Repsitories.DeclarationCourierStatusRepository).FastDeleteMulti(entityKeyFields);
        }

        public DeclarationCourierStatusPM UpdateTrucker(DeclarationCourierStatusPM currentDeclarationCourierStatusPM, DeclarationPM declarationPM)
        {
            var context = CustomContext.GetContext(declarationPM.Tenant);
            if (currentDeclarationCourierStatusPM == null)
            {
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
            }

            if (currentDeclarationCourierStatusPM != null)
            {
                string truckerId = TruckerId;

                if (truckerId != currentDeclarationCourierStatusPM.TruckerId || DistributionArea != currentDeclarationCourierStatusPM.DistributionArea)
                {
                    //currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    currentDeclarationCourierStatusPM.TruckerId = truckerId;
                    currentDeclarationCourierStatusPM.MAWB = MAWB;
                    currentDeclarationCourierStatusPM.DistributionArea = DistributionArea;
                    LogMessagingUtil.Instance.AppendLine("try to update trucker " + truckerId + " to declarationCourierStatus for DeclarationPM.Id: " + declarationPM.Id);
                }
            }
            return currentDeclarationCourierStatusPM;
        }
        public DeclarationCourierStatusPM UpdateLastMileServiceType(DeclarationCourierStatusPM currentDeclarationCourierStatusPM, DeclarationPM declarationPM)
        {
            if (currentDeclarationCourierStatusPM == null)
            {
                var context = CustomContext.GetContext(declarationPM.Tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
            }
            if (currentDeclarationCourierStatusPM != null && currentDeclarationCourierStatusPM.LastMileServiceType != LastMileServiceType)
            {
                currentDeclarationCourierStatusPM.LastMileServiceType = LastMileServiceType;
                //if (currentDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                LogMessagingUtil.Instance.AppendLine("try to update Last Mile Service Type " + currentDeclarationCourierStatusPM.LastMileServiceType + " to declarationCourierStatus for DeclarationPM.Id: " + declarationPM.Id);
            }
            if (currentDeclarationCourierStatusPM != null && currentDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                currentDeclarationCourierStatusPM.MAWB = MAWB;
            }
            return currentDeclarationCourierStatusPM;
        }
        public DeclarationCourierStatusPM CalculateDeclarationCourierStatus(DeclarationPM declarationPM, bool isRequiredFieldHasChanged = false, DeclarationCourierStatusPM _MyDeclarationCourierStatusPM=null)
        {
            LogMessagingUtil.Instance.AppendLine("CalculateDeclarationCourierStatus()");
            CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(declarationPM);
            calculateDeclarationCourierStatus.ImporterCode = ImporterCode; 
            return calculateDeclarationCourierStatus.CalcAll(_MyDeclarationCourierStatusPM);

            if (declarationPM.IsCourierDeclaration)
            {
                //task 39471 added GetSingle here
                var context = CustomContext.GetContext(declarationPM.Tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, false, false);
                if (myDeclarationCourierStatusPM == null)
                {
                    myDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                    {
                        DeclarationId = declarationPM.Id,
                        Tenant = declarationPM.Tenant,
                        IsClosedForFollowUp = false,
                        IsCourierMissingClassification = false,
                        CourierDeclarationStatusCode = "",
                        ChangeSetOp = ChangeSetOperation.Insert,
                    };
                }
                else
                {
                    myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                }

                //Set CourierManifestStatusCode according to Manifest Message Required fields
                CustomsRequiredFieldErrors errorsForCourierDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForCourierDeclaration(declarationPM.Id, declarationPM.Tenant, declarationPM);
                if (errorsForCourierDeclaration != null && errorsForCourierDeclaration.RequiredFields != null && errorsForCourierDeclaration.RequiredFields.Count() > 0)
                {
                    myDeclarationCourierStatusPM.CourierManifestStatusCode = "M";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(declarationPM.ManifestCargoStatusCode))
                    {
                        myDeclarationCourierStatusPM.CourierManifestStatusCode = "R";
                    }
                    else
                    {
                        switch (declarationPM.ManifestCargoStatusCode)
                        {
                            case "1":
                                myDeclarationCourierStatusPM.CourierManifestStatusCode = "V";
                                break;
                            case "2":
                            case "3":
                                myDeclarationCourierStatusPM.CourierManifestStatusCode = "X";
                                break;
                            default:
                                myDeclarationCourierStatusPM.CourierManifestStatusCode = "";
                                break;
                        }
                    }
                }

                //Set TotalInvoiceAmountInUSD - sum field InvoiceAmountInUSD from all SupplierInvoices
                myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD = 0;
                if (declarationPM.SupplierInvoices != null && declarationPM.SupplierInvoices.Count() > 0)
                {
                    myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD = declarationPM.SupplierInvoices.Sum(r => r.InvoiceAmountInUSD);
                }

                //Set CourierDeclarationStatusCode according to Declaration Message Required fields
                CustomsRequiredFieldErrors errorsForDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(declarationPM.Id, declarationPM.Tenant, declarationPM);
                if (errorsForDeclaration != null && errorsForDeclaration.RequiredFields != null && errorsForDeclaration.RequiredFields.Count() > 0)
                {
                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
                }
                else if (myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > 100 && string.IsNullOrEmpty(declarationPM.ImporterId) && string.IsNullOrEmpty(declarationPM.ImporterCode))
                {
                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(declarationPM.DeclarationStatusTypeCode) || declarationPM.IsChanged == true && myDeclarationCourierStatusPM.CourierDeclarationStatusCode == "V")//task 39471
                    {
                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "R";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(declarationPM.PaymentDate.ToString()))//task 40950
                        {
                            myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "V";
                        }
                        else
                        {
                         
                                switch (declarationPM.DeclarationStatusTypeCode)
                                {
                                    case "12":
                                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "X";
                                        break;
                                    case "11":
                                    case "13":
                                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "V";
                                        break;
                                    default:
                                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "";
                                        break;
                                }
                           
                        }
                    }
                }

                //Set CourierPaymentStatusCode
                if (declarationPM.PaymentDate == null)
                {
                    if (declarationPM.DeclarationStatusTypeCode == "13" && myDeclarationCourierStatusPM.CourierDeclarationStatusCode == "V")//Task 39471
                    {
                        myDeclarationCourierStatusPM.CourierPaymentStatusCode = "R";
                    }
                    else
                    {
                        myDeclarationCourierStatusPM.CourierPaymentStatusCode = "";//Task 39471
                    }
                }
                else
                {
                    switch (declarationPM.PaymentStatusCode)
                    {
                        case "":
                        case null://task 40951
                            myDeclarationCourierStatusPM.CourierPaymentStatusCode = "O";
                            break;
                        case "3":
                            myDeclarationCourierStatusPM.CourierPaymentStatusCode = "P";
                            break;
                        default:
                            myDeclarationCourierStatusPM.CourierPaymentStatusCode = "";
                            break;
                    }
                }

                //Set IsCourierMissingClassification
                if (declarationPM.SupplierInvoices != null && declarationPM.SupplierInvoices.Count() > 0)
                {

                    List<SupplierInvoicePM> emptyClassificationCodeList = declarationPM.SupplierInvoices.
                        Where(SI => SI.SupplierInvoiceItems != null && SI.SupplierInvoiceItems.Any(u => string.IsNullOrWhiteSpace(u.ClassificationCode))).ToList();

                    if (emptyClassificationCodeList != null && emptyClassificationCodeList.Count() > 0)
                    {
                        myDeclarationCourierStatusPM.IsCourierMissingClassification = true;
                    }
                    else
                    {
                        List<SupplierInvoicePM> emptyItemsList = declarationPM.SupplierInvoices.
                        Where(SI => SI.SupplierInvoiceItems == null || SI.SupplierInvoiceItems.Count() == 0).ToList();
                        if (emptyItemsList != null && emptyItemsList.Count() > 0)
                        {
                            myDeclarationCourierStatusPM.IsCourierMissingClassification = true;
                        }
                        else
                        {
                            myDeclarationCourierStatusPM.IsCourierMissingClassification = false;
                        }
                    }
                }
                else//task 40894
                {
                    myDeclarationCourierStatusPM.IsCourierMissingClassification = true;
                }

                var setting = CustomsSettingQueryService.GetSettingByTenant(myDeclarationCourierStatusPM.Tenant);

                if (setting != null & setting.IsConnectedToUniFreight)
                {


                    //Set HighLowValue

                    DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(myDeclarationCourierStatusPM.Tenant);

                    string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_HIGH_VALUE", "NON", "NON", myDeclarationCourierStatusPM.Tenant);
                    decimal defaultAmount = 0;
                    var boolvar = (decimal.TryParse(defValue, out defaultAmount));

                    if (myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > defaultAmount)
                    {
                        myDeclarationCourierStatusPM.HighLowValue = "H";
                    }
                    else
                    {
                        myDeclarationCourierStatusPM.HighLowValue = "L";
                    }
                }

                if (string.IsNullOrWhiteSpace(myDeclarationCourierStatusPM.DocumentStatusCode)) myDeclarationCourierStatusPM.DocumentStatusCode = "M";

                return myDeclarationCourierStatusPM;

            }
            return null;
        }

       
        private void SendToMassof(DeclarationCourierStatusPM entityPM, DeclarationCourierStatus entityPOCO)
        {
            CourierPendingReasonQueryService courierPendingReasonQueryService = new CourierPendingReasonQueryService(entityPM.Tenant);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            List<string> listReasonCode;
            if ((entityPM.CourierPendingReasonList == null && entityPOCO.CourierPendingReasonList != null) || (entityPM.CourierPendingReasonList != null && entityPOCO.CourierPendingReasonList == null))
            {
                listReasonCode = entityPM.DeclarationPendings.Select(c => c.CourierPendingReasonCode).ToList();
            }
            else
            {
                listReasonCode = entityPM.DeclarationPendings.Where(c => ((entityPM.CourierPendingReasonList+",").Contains(c.CourierPendingReasonCode+",") && !((entityPOCO.CourierPendingReasonList+",").Contains(c.CourierPendingReasonCode+",")))
                || ((entityPOCO.CourierPendingReasonList+",").Contains(c.CourierPendingReasonCode+",") && !((entityPM.CourierPendingReasonList + ",").Contains(c.CourierPendingReasonCode + ",")))).Select(d => d.CourierPendingReasonCode).ToList();
            }
            foreach (var item in listReasonCode)
            {
                CourierPendingReasonPM courierPendingReason = courierPendingReasonQueryService.GetSingleCourierPendingReasonByCode(item, entityPM.Tenant);

                if (courierPendingReason?.SwissportSuspendedCode != null)
                {
                    var courierECSWSTHRMessageRequestService = new CourierECSWSTHRMessageRequestService();
                    string drityMessage = courierECSWSTHRMessageRequestService.GetMessageUpdateHawbStatus(entityPM.DeclarationId, entityPM.Tenant, null, null, entityPM, true);
                    if (drityMessage != null)
                    {
                        var XMLdrityMessage = courierECSWSTHRMessageRequestService.DeserializeXmlNode(drityMessage);
                        var res = courierECSWSTHRMessageRequestService.BuildUpdateHawbStatus(entityPM.DeclarationId, entityPM.Tenant, XMLdrityMessage);
                    }
                    break;
                }
                //string.IsNullOrEmpty(entityPM.MAWB) = not from u2l
                if (string.IsNullOrEmpty(entityPM.MAWB) && courierPendingReason?.MamanSuspendedCode != null)
                {
                    var courierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanRequestService();
                    string drityMessage = courierGWMessageECTHRDataMamanService.GetMessage2Maman(entityPM.DeclarationId, entityPM.Tenant, null, null, entityPM,true);
                    if (drityMessage != null)
                    {
                         var res = courierGWMessageECTHRDataMamanService.BuildComm2Maman(entityPM.DeclarationId, entityPM.Tenant, drityMessage);
                    }
                    break;
                }
                if (courierPendingReason?.OverseasSuspendedCode != null)
                {
                    var courierMessageDataOverseasRequestService = new CourierOVSECTHMessageRequestService();
                    string drityMessage = courierMessageDataOverseasRequestService.GetMessageUpdateHawbStatus(entityPM.DeclarationId, entityPM.Tenant, null, null, entityPM);
                    if (drityMessage != null)
                    {
                        var res = courierMessageDataOverseasRequestService.BuildUpdateHawbStatus(entityPM.DeclarationId, entityPM.Tenant, drityMessage);
                    }
                    break;
                }
            }
        }

        
    }
}
