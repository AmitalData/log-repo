using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.Data.EntityPOCOs;
//using Simplog.Infrastructure.SimplogUtilities;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.BL.DataContracts;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.Customs.BL.Messaging.U2L.ImportDeclaration
{

    //Logitude.Customs.BL.Messaging.U2L.ImportDeclaration.DeclarationUpsertService 
    public class DeclarationUpsertService : UnifreightGenericService
    {
        private LOGICUSTFILE _LOGICUSTFILE;
        public LogitudeCustomsFile _AmitalCustomsFile;
        public Def.EntityPMs.DeclarationPM _MyDeclarationPM;
        private ICustomContext _context;
        private CourierMasterPM _CourierMasterPM;
        private CourierDeclarationPM _CourierDeclarationPM;
        private DeclarationCourierStatusPM currentDeclarationCourierStatusPM;
        private string mode;
        public Boolean suppressNewTrans;

        private AmitalContext amitalContext;
        private DeclarationReferantDataPM _DeclarationReferantDataPM;
        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.ImportDeclaration.DeclarationUpsertService.Upsert()";

        public DeclarationUpsertService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "DeclarationUpsertService";
            true
            )
        {

        }

        public override string GetAssemblyQualifiedName()
        {
            return this.GetType().Name;
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGICUSTFILE();
            var myAmitalCustom = new LogitudeCustomsFile();
            myAmitalCustom.CustomFileNo = "41350144";
            myAmitalCustom.DealId = "11202A23";
            myAmitalCustom.DeclarationOfficeCode = "2";
            myAmitalCustom.DepartmentId = "ADMIN";
            myAmitalCustom.FileState = "R";
            myAmitalCustom.AgentId = "10011837";
            myAmitalCustom.CargoDescription = "Cargo Description up to 256 chars";
            //myAmitalCustom.CreatedByUserId = "YARONC";
            myAmitalCustom.CreatedByUserId = "1-1";
            myAmitalCustom.CustomerId = "10011837";
            myAmitalCustom.GrossMassMeasure = "230";
            myAmitalCustom.HAWB = "1255";
            myAmitalCustom.LoadingPortCode = "AUFRB";
            myAmitalCustom.ManifestNumber = "132256";
            myAmitalCustom.MAWB = "22334";
            myAmitalCustom.OriginCountryCode = "0059";
            myAmitalCustom.PackageMeasureQualifierCode = "KG";
            myAmitalCustom.PackageQuantity = "1";
            myAmitalCustom.PackageTypeCode = "05";
            //myAmitalCustom.ReferentUserId = "EITAN";
            myAmitalCustom.ReferentUserId = "1-1";
            myAmitalCustom.TransportModeId = "O";
            myAmitalCustom.VendorId = "ZM";
            myAmitalCustom.Tenant = "1";
            ///myAmitalCustom.URL = @"http://logitudevm8.cloudapp.net/logitude/default.aspx?userdata=jalal@mail.com:1-2:1&ischamplogin=false";


            amitalObjExample.LogitudeCustomsFile = new LogitudeCustomsFile[] { myAmitalCustom };

            xml = XmlGenericUtil<LOGICUSTFILE>.SerializeObject(amitalObjExample);

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

        private void Upsert(bool suppressNewTrans = false, string moreParams = null)
        {
            //CheckExist();
            using (TransactionScope scope =
                suppressNewTrans ? TransactionFactory.GetTransaction() : TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            {
                AppendLogLine("Upsert..");

                MyGenericResponseObj.Stage = "Check integrity ";
                ///must 
                if (String.IsNullOrWhiteSpace(_AmitalCustomsFile.CustomFileNo))
                {

                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "CustomFileNo is missing";
                    return;

                }

                AppendLogLine("CustomFileNo = " + _AmitalCustomsFile.CustomFileNo);
                MyGenericResponseObj.Stage = "GetContext";
                _context = CustomContext.GetContext(ResolvedTenant());
                (_context as CustomContext).Database.Connection.StateChange += (sender, e) =>
                {
                    Debug.WriteLine(e.CurrentState.ToString());
                };
                var myQueryService = new DeclarationQueryService(_context);
                var myDeclarationUpdateService = new DeclarationUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());
                //var mySupplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(context, new Dictionary<string, IContext>(), ResolvedTenant());
                //var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(context, new Dictionary<string, IContext>(), ResolvedTenant());
                //amitalContext = AmitalContext.GetContext(ResolvedTenant());
                if ((new CustomsSettingQueryService(ResolvedTenant())).GetSettingByTenantN(ResolvedTenant()).IsConnectedToUniFreight)
                {
                    amitalContext = AmitalContext.GetContext(ResolvedTenant());
                }


                if (String.IsNullOrWhiteSpace(_AmitalCustomsFile.Id))
                {
                    string existId = myQueryService.GetIdByCustomFileNo(_AmitalCustomsFile.CustomFileNo, ResolvedTenant());
                    _AmitalCustomsFile.Id = existId;
                }


                /// Exist
                if (String.IsNullOrWhiteSpace(_AmitalCustomsFile.Id))
                {
                    this._MyDeclarationPM = new Def.EntityPMs.DeclarationPM();
                    this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    MyCommunicationsParams.LoggingEntityReference = _AmitalCustomsFile.Id;

                    AppendLogLine("_AmitalCustomsFile.Id = " + _AmitalCustomsFile.Id);
                    MyGenericResponseObj.Stage = "GetSingle";
                    this._MyDeclarationPM = myQueryService.GetSingle(_AmitalCustomsFile.Id, true, false);
                    if (this._MyDeclarationPM == null)
                    {
                        MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.TecinicalFailure;
                        MyGenericResponseObj.Message = "_AmitalCustomsFile.Id " + _AmitalCustomsFile.Id + "Not found";
                        return;
                    }
                    MyCommunicationsParams.LoggingEntityId =
                        MyGenericResponseObj.ApplicationId =
                        _MyDeclarationPM.Id;

                    //<-- Yuval Chalup 04.03.2015 TASK-11617 - CHANGED FROM:
                    //if (this._MyDeclarationPM.PaymentDate != null)
                    //{
                    //    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    //    MyGenericResponseObj.Message = "Declaration has already been paid (Payment date " + this._MyDeclarationPM.PaymentDate + ")";
                    //    return;
                    //}
                    //TO:
                    //Check if the declaration can be updated
                    //Check if there are requests in progress
                    CustomsRequestsSheetQueryService customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(_context);
                    List<CustomsRequestsSheetPM> customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(_MyDeclarationPM.Tenant, "2750", "", "", null, null, _MyDeclarationPM.CustomFileNo, true);
                    if (customsRequestsSheetPMList != null)
                    {
                        if (customsRequestsSheetPMList.Count > 0)
                        {
                            var RequestInProgressInterfaceTypeName = customsRequestsSheetPMList.First().InterfaceTypeName;
                            var text = TranslateTextsClass.Translate("Customs.General.RequestInProgress", _MyDeclarationPM.Tenant, true);
                            MyGenericResponseObj.Message = String.Format(text, RequestInProgressInterfaceTypeName);
                            return;
                        }
                    }

                    //Check if Declaration was already paid, constraint in progress or Future payment was done
                    var declarationValidator = new Logitude.Customs.BL.Validators.DeclarationValidator(_MyDeclarationPM);
                        if (_MyDeclarationPM.IsCourierDeclaration) declarationValidator.ToUpdateWithPaymentDate = true;
                    declarationValidator.DeclarationViewDisplayOnlyChecks();
                    if (declarationValidator.ErrorCode.Count > 0)
                    {
                        MyGenericResponseObj.Message = TranslateTextsClass.Translate(declarationValidator.ErrorCode[0], _MyDeclarationPM.Tenant, true);
                        MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                        return;
                    }
                    if (_MyDeclarationPM.IsCourierDeclaration && _MyDeclarationPM.PaymentDate.HasValue)
                    {
                        UpdateTrucker();
                        MyGenericResponseObj.Message = "Declaration has already been paid (Payment date " + this._MyDeclarationPM.PaymentDate + "), only Trucker details will be updated";
                        MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
                        MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
                        MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;
                        scope.Complete();
                        return;
                    }
                    //Yuval Chalup 04.03.2015 TASK-11617 --->

                    //Dont allow to cancel if there is a connected vehicle (SupplierInvoiceItemVehicle)
                    if (_AmitalCustomsFile.Mode == "CANCEL" || _AmitalCustomsFile.Mode == "DELETE")
                    {
                        var supplierInvoiceItemVehicleQueryService = new SupplierInvoiceItemVehicleQueryService(_context);
                        List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehicles = supplierInvoiceItemVehicleQueryService.GetAllSupplierInvoiceItemVehiclesForDeclaration(_MyDeclarationPM.Id, _MyDeclarationPM.Tenant);
                        if (supplierInvoiceItemVehicles != null && supplierInvoiceItemVehicles.Count > 0)
                        {
                            MyGenericResponseObj.Message = "לא ניתן למחוק/לבטל תיק שמקושר לריכבית";
                            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                            return;
                        }
                    }

                    this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                CustomsSettingQueryService settingService = new CustomsSettingQueryService(ResolvedTenant());
                CustomsSettingPM setting = settingService.GetSettingByTenantN(ResolvedTenant());
                this._MyDeclarationPM.MarkAsChanged = true; // moran 2.6.15 - Task 13803

                MyGenericResponseObj.Stage = "Mapping";
                if (_AmitalCustomsFile.Direction == "E" && _AmitalCustomsFile.Mode != "NEW")
                {
                    ExportDeclarationUpdate();
                    MyGenericResponseObj.Message = "עודכנה הצהרת יצוא";
                    MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
                    MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;
                    scope.Complete();
                    return;
                }

                if (this._MyDeclarationPM.Consignments == null)
                {
                    this._MyDeclarationPM.Consignments = new List<Def.EntityPMs.ConsignmentPM>();
                }
                if (this._MyDeclarationPM.Consignments.Count == 0)
                {
                    this._MyDeclarationPM.Consignments.Add(new Def.EntityPMs.ConsignmentPM() { ChangeSetOp = ChangeSetOperation.Insert, IsLastReleaseFromWarehous = "N", Tenant = ResolvedTenant() }); // moran 7.6.15 - Task 13887 - add handle to initiate IsLastReleaseFromWarehous // moran 20.8.15 Task 15049 - add handle to initiate Tenant
                }


                if (this._MyDeclarationPM.Consignments[0].ConsignmentPackages == null)
                {
                    this._MyDeclarationPM.Consignments[0].ConsignmentPackages = new List<Def.EntityPMs.ConsignmentPackagePM>();
                }
                if (this._MyDeclarationPM.Consignments[0].ConsignmentPackages.Count == 0)
                {
                    this._MyDeclarationPM.Consignments[0].ConsignmentPackages.Add(new Def.EntityPMs.ConsignmentPackagePM() { ChangeSetOp = ChangeSetOperation.Insert, Tenant = ResolvedTenant() }); // moran 20.8.15 Task 15049 - add handle to initiate Tenant
                }



                if (_MyDeclarationPM.ChangeSetOp == ChangeSetOperation.Insert)
                {
                    this._MyDeclarationPM.TaxationDateTime = DateTime.Now;
                    this._MyDeclarationPM.ExternalDeclarationNumber = (_AmitalCustomsFile.CustomFileNo + DateTime.Today.Year.ToString());
                    this._MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Insert;
                    if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.CargoTypeCode))
                    {
                        this._MyDeclarationPM.Consignments[0].CargoTypeCode = _AmitalCustomsFile.CargoTypeCode;
                    }
                    else
                    {
                        if (_AmitalCustomsFile.TransportModeId == "A")
                        {
                            if (_AmitalCustomsFile.DeclarationOfficeCode == "49")
                            {
                                this._MyDeclarationPM.Consignments[0].CargoTypeCode = "7";
                            }
                            else
                            {
                                this._MyDeclarationPM.Consignments[0].CargoTypeCode = "1";
                            }
                        }
                        else if (_AmitalCustomsFile.TransportModeId == "O")
                        {
                            this._MyDeclarationPM.Consignments[0].CargoTypeCode = "11";
                        }
                        else if (_AmitalCustomsFile.TransportModeId == "L")
                        {
                            this._MyDeclarationPM.Consignments[0].CargoTypeCode = "20";
                        }
                    }
                }
                AppendLogLine("_AmitalCustomsFile.Id = " + _AmitalCustomsFile.Id + " " + _MyDeclarationPM.ChangeSetOp.ToString());
                //this._MyDeclarationPM.m = _AmitalCustomsFile.MAWB 
                this._MyDeclarationPM.CustomFileNo = _AmitalCustomsFile.CustomFileNo;
                this._MyDeclarationPM.DeclarationOfficeCode = _AmitalCustomsFile.DeclarationOfficeCode;
                this._MyDeclarationPM.FileState = _AmitalCustomsFile.FileState;
                this._MyDeclarationPM.AgentId = _AmitalCustomsFile.AgentId;//translate?
                string DBcustomer = this._MyDeclarationPM.CustomerId; // moran 12.7.15 - Task 14510
                this._MyDeclarationPM.CustomerId = TranslateCustomer(_AmitalCustomsFile.CustomerId);//check translate
                this._MyDeclarationPM.ForwarderFiles = _AmitalCustomsFile.ForwarderFiles;
                if (String.IsNullOrWhiteSpace(this._MyDeclarationPM.CustomerId) && _AmitalCustomsFile.Direction != "E")
                {
                        MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                        MyGenericResponseObj.Message += "CustomerId " + _AmitalCustomsFile.CustomerId + " could not translate (is must )";
                        return;
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.Direction))
                {
                    this._MyDeclarationPM.Direction = _AmitalCustomsFile.Direction;
                    this._MyDeclarationPM.Consignments[0].ConsignmentType = "E";
                    if (this._MyDeclarationPM.Direction == "E" && string.IsNullOrWhiteSpace(this._MyDeclarationPM.AgentRoleCode)) this._MyDeclarationPM.AgentRoleCode = "A";
                }
                if (mode == "UpdateNotEmpty" || this._MyDeclarationPM.CustomerId != DBcustomer) // moran 12.7.15 - Task 14510 - insert into 'if'
                {
                    if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.ImporterId))
                    {
                        if (_AmitalCustomsFile.ImporterId.Length > 1 && _AmitalCustomsFile.ImporterId.Substring(0, 2) == "P-")
                        {
                            this._MyDeclarationPM.ImporterPassportNumber = _AmitalCustomsFile.ImporterId.Substring(2);
                            this._MyDeclarationPM.ImporterTypeCode = "2";
                            this._MyDeclarationPM.ImporterCode = _AmitalCustomsFile.ImporterId;
                            if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.CasualImporterCountry))
                            {
                                string countryCode = "";
                                if (_AmitalCustomsFile.CasualImporterCountry.Length > 2 && setting != null && setting.IsConnectedToUniFreight)
                                {
                                    countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", _AmitalCustomsFile.CasualImporterCountry);
                                }
                                else
                                {
                                    countryCode = _AmitalCustomsFile.CasualImporterCountry;
                                }
                                if (!string.IsNullOrWhiteSpace(countryCode)) this._MyDeclarationPM.ImporterPassCountryCode = countryCode;
                            }
                        }
                        else
                        {
                            string importerId = _AmitalCustomsFile.ImporterId;
                            if (_AmitalCustomsFile.ImporterId.Length > 9)
                            {
                                importerId = _AmitalCustomsFile.ImporterId.Substring(0, 9);
                            }
                            string clientId = TranslateClient(importerId);

                            this._MyDeclarationPM.ImporterId = clientId;
                            this._MyDeclarationPM.ImporterCode = importerId;

                        }
                    }
                }
                this._MyDeclarationPM.TransportModeId = _AmitalCustomsFile.TransportModeId;
                //this._MyDeclarationPM.CreatedByUserId = _AmitalCustomsFile.CreatedByUserId;
                this._MyDeclarationPM.CreatedByUserId = TranslateUser(_AmitalCustomsFile.CreatedByUserId);
                this._MyDeclarationPM.ReferentUserId = TranslateUser(_AmitalCustomsFile.ReferentUserId);
                this._MyDeclarationPM.DepartmentId = TranslateDepartment(_AmitalCustomsFile.DepartmentId);
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.COUWTVAL))
                {
                    this._MyDeclarationPM.WeightValue = _AmitalCustomsFile.COUWTVAL;
                }
                

                if (String.IsNullOrWhiteSpace(this._MyDeclarationPM.CustomerId) && _AmitalCustomsFile.Direction != "E")
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "CustomerId is missing " + _AmitalCustomsFile.CustomerId + "Not found";
                    return;
                }

                /*-->
                 * Cancelld due to Task 11322:שינוי במסר U2L - לאפשר לפתוח הצהרה גם אם היבואן לא קיים בלוגיטיוד
                 * eitan h 17/2/15
                            if (String.IsNullOrWhiteSpace(this._MyDeclarationPM.ImporterId))
                            {
                                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                                MyGenericResponseObj.Message = "ImporterId is missing " + _AmitalCustomsFile.ImporterId + "Not found";
                                return;
                            }
                 * <--
                 */
                if (_AmitalCustomsFile.Mode == "CANCEL") // moran 25.12.13 - task 2431
                {
                    this._MyDeclarationPM.IsCancelled = true;
                    this._MyDeclarationPM.DeclarationNumber = null;
                }
                else if (_AmitalCustomsFile.Mode == "DELETE")
                {
                    this._MyDeclarationPM.IsCancelled = true;
                    this._MyDeclarationPM.CustomFileNo = null;
                    this._MyDeclarationPM.DeclarationNumber = null;
                }
                else if (_AmitalCustomsFile.Mode == "UNCANCEL")
                {
                    this._MyDeclarationPM.IsCancelled = false;
                }

                //           if (String.IsNullOrWhiteSpace(this._MyDeclarationPM.ReferentUserId))
                //           {
                //              _GenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                //               _GenericResponseObj.Message = "ReferentUserId is missing " + _AmitalCustomsFile.ReferentUserId + "Not found";
                //               return;
                //          }

                //           if (String.IsNullOrWhiteSpace(this._MyDeclarationPM.DepartmentId))
                //           {
                //               _GenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                //               _GenericResponseObj.Message = "DepartmentId is missing " + _AmitalCustomsFile.DepartmentId + "Not found";
                //               return;
                //           }

                if (this._MyDeclarationPM.Consignments.Count == 1)
                {
                    if (this._MyDeclarationPM.Consignments[0].ChangeSetOp != ChangeSetOperation.Insert)
                    {
                        this._MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update;
                    }
                    if (this._MyDeclarationPM.Consignments[0].CargoTypeCode == "17")
                    {

                        if (mode != "UpdateNotEmpty")
                        {
                            this._MyDeclarationPM.Consignments[0].ManifestNumber = _AmitalCustomsFile.HAWB;
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.HAWB)) this._MyDeclarationPM.Consignments[0].ManifestNumber = _AmitalCustomsFile.HAWB;
                        }
                        this._MyDeclarationPM.Consignments[0].ThirdCargoID = _AmitalCustomsFile.HAWBDATE;

                        if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.DealId) || !string.IsNullOrWhiteSpace(_AmitalCustomsFile.ManifestNumber))
                        {
                            this._MyDeclarationPM.Consignments[0].SecondCargoID = _AmitalCustomsFile.DealId;
                            if (this._MyDeclarationPM.Consignments[0].SecondCargoID.Length == 15)
                            {
                                this._MyDeclarationPM.Consignments[0].SecondCargoID = "I" + this._MyDeclarationPM.Consignments[0].SecondCargoID;
                            }
                            if (this._MyDeclarationPM.Consignments[0].SecondCargoID.Length < 15)
                            {
                                this._MyDeclarationPM.Consignments[0].SecondCargoID = "I" + _AmitalCustomsFile.ManifestNumber + this._MyDeclarationPM.Consignments[0].SecondCargoID;
                            }
                        }
                    }
                    else if (this._MyDeclarationPM.Consignments[0].CargoTypeCode == "2" || this._MyDeclarationPM.Consignments[0].CargoTypeCode == "02")
                    {
                        this._MyDeclarationPM.Consignments[0].ManifestNumber = null;
                        this._MyDeclarationPM.Consignments[0].SecondCargoID = null;
                        this._MyDeclarationPM.Consignments[0].ThirdCargoID = null;
                    }
                    //else if (this._MyDeclarationPM.Consignments[0].CargoTypeCode == "20")
                    //{
                    //    if (_AmitalCustomsFile.TransportModeId == "L")
                    //    {
                    //        this._MyDeclarationPM.Consignments[0].ManifestNumber = _AmitalCustomsFile.DealId;
                    //        if (!this._MyDeclarationPM.Consignments[0].ManifestNumber.StartsWith("I"))
                    //        {
                    //            this._MyDeclarationPM.Consignments[0].ManifestNumber = "I" + this._MyDeclarationPM.Consignments[0].ManifestNumber;
                    //        }
                    //    }
                    //}
                    else if (this._MyDeclarationPM.Consignments[0].CargoTypeCode == "20")
                    {
                        //this._MyDeclarationPM.Consignments[0].ManifestNumber = null;
                        //this._MyDeclarationPM.Consignments[0].SecondCargoID = null;
                        //this._MyDeclarationPM.Consignments[0].ThirdCargoID = null;
                    }
                    else if (this._MyDeclarationPM.Consignments[0].CargoTypeCode == "8" || this._MyDeclarationPM.Consignments[0].CargoTypeCode == "08")
                    {
                        //this._MyDeclarationPM.Consignments[0].ManifestNumber = null;
                        //this._MyDeclarationPM.Consignments[0].SecondCargoID = null;
                        //this._MyDeclarationPM.Consignments[0].ThirdCargoID = null;
                    }
                    else if (this._MyDeclarationPM.Consignments[0].CargoTypeCode == "11")
                    {
                        this._MyDeclarationPM.Consignments[0].ManifestNumber = _AmitalCustomsFile.ManifestNumber;
                        if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.DealId))
                        {
                            this._MyDeclarationPM.Consignments[0].SecondCargoID = "I" + _AmitalCustomsFile.DealId.GetLast(9);
                        }

                        //this._MyDeclarationPM.Consignments[0].ThirdCargoID = null;
                    }
                    else if (this._MyDeclarationPM.Consignments[0].CargoTypeCode == "1")
                    {
                        if (this._MyDeclarationPM.TransportModeId != "A")
                        {
                            this._MyDeclarationPM.Consignments[0].ManifestNumber = _AmitalCustomsFile.ManifestNumber;
                        }
                        if (mode != "UpdateNotEmpty")
                        {
                            this._MyDeclarationPM.Consignments[0].SecondCargoID = _AmitalCustomsFile.MAWB;
                            this._MyDeclarationPM.Consignments[0].ThirdCargoID = _AmitalCustomsFile.HAWB;
                        }
                        else
                        {
                            if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.MAWB)) this._MyDeclarationPM.Consignments[0].SecondCargoID = _AmitalCustomsFile.MAWB;
                            if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.HAWB)) this._MyDeclarationPM.Consignments[0].ThirdCargoID = _AmitalCustomsFile.HAWB;
                        }

                    }
                    else
                    {
                        if (this._MyDeclarationPM.TransportModeId == "A")
                        {
                            if (mode != "UpdateNotEmpty")
                            {
                                this._MyDeclarationPM.Consignments[0].SecondCargoID = _AmitalCustomsFile.MAWB;
                                this._MyDeclarationPM.Consignments[0].ThirdCargoID = _AmitalCustomsFile.HAWB;
                            }
                            else
                            {
                                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.MAWB)) this._MyDeclarationPM.Consignments[0].SecondCargoID = _AmitalCustomsFile.MAWB;
                                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.HAWB)) this._MyDeclarationPM.Consignments[0].ThirdCargoID = _AmitalCustomsFile.HAWB;
                            }
                        }
                        else
                        {
                            this._MyDeclarationPM.Consignments[0].SecondCargoID = _AmitalCustomsFile.DealId;
                        }

                        this._MyDeclarationPM.Consignments[0].ManifestNumber = _AmitalCustomsFile.ManifestNumber;
                    }
                    //this._MyDeclarationPM.Consignments[0].LoadingPortCode = _AmitalCustomsFile.LoadingPortCode;
                    string loadingPortCode = null;
                    if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.LoadingPortCode))
                    {
                        loadingPortCode = TranslateloadPort(_AmitalCustomsFile.LoadingPortCode);
                    }
                    if (mode != "UpdateNotEmpty")
                    {
                        this._MyDeclarationPM.Consignments[0].LoadingPortCode = loadingPortCode;
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(loadingPortCode)) this._MyDeclarationPM.Consignments[0].LoadingPortCode = loadingPortCode;
                    }

                    //this._MyDeclarationPM.Consignments[0].OriginCountryCode = TranslateCountry(_AmitalCustomsFile.OriginCountryCode);
                    // moran 2.4.14 - add handle in case of empty value -->
                    //this._MyDeclarationPM.Consignments[0].OriginCountryCode = _AmitalCustomsFile.OriginCountryCode;
                    if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.OriginCountryCode))
                    {
                        //this._MyDeclarationPM.Consignments[0].OriginCountryCode = _AmitalCustomsFile.OriginCountryCode;
                        string countryCode = "";
                        if (_AmitalCustomsFile.OriginCountryCode.Length > 2 && setting != null  && setting.IsConnectedToUniFreight)
                        {
                            countryCode = GetTranslationL2P("IIGC", "CTBCOUNTRY", _AmitalCustomsFile.OriginCountryCode);
                        }
                        else
                        {
                            countryCode = _AmitalCustomsFile.OriginCountryCode;
                        }
                        if (!string.IsNullOrWhiteSpace(countryCode)) this._MyDeclarationPM.Consignments[0].OriginCountryCode = countryCode;
                    }
                    else
                    {
                        if (mode != "UpdateNotEmpty") this._MyDeclarationPM.Consignments[0].OriginCountryCode = null;
                    }
                    // moran 2.4.14 - add handle in case of empty value <--
                    this._MyDeclarationPM.Consignments[0].CargoDescription = _AmitalCustomsFile.CargoDescription;
                    AppendLogLine("_AmitalCustomsFile.OriginCountryCode " + _AmitalCustomsFile.OriginCountryCode + ",  _MyDeclarationPM.Consignments[0].OriginCountryCode " + this._MyDeclarationPM.Consignments[0].OriginCountryCode +
                        ", _AmitalCustomsFile.CargoDescription " + _AmitalCustomsFile.CargoDescription + ",  _MyDeclarationPM.Consignments[0].CargoDescription " + this._MyDeclarationPM.Consignments[0].CargoDescription);

                    //moran wi 1829 + 1855 14.11.13 -->
                    // moran 20.5.15 - Task 13527 - change handle for formatting dates -->
                    //DateTime dat;
                    //var success = DateTime.TryParse(_AmitalCustomsFile.ManifestDate, out dat);
                    //if (success)
                    //{
                    //    this._MyDeclarationPM.Consignments[0].ManifestDate = dat;
                    //}
                    //success = DateTime.TryParse(_AmitalCustomsFile.ArrivalDateTime, out dat);
                    //if (success)
                    //{
                    //    this._MyDeclarationPM.Consignments[0].UnloadDate = dat;
                    //}

                    //this._MyDeclarationPM.Consignments[0].ManifestDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.ManifestDate, "AmitalCustomsFile.ManifestDate");
                    this._MyDeclarationPM.Consignments[0].UnloadDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.ArrivalDateTime, "AmitalCustomsFile.ArrivalDateTime");
                        // moran 20.5.15 - Task 13527 - change handle for formatting dates <--
                        //moran wi 1829 + 1855 14.11.13 <--
                        // moran 1.2.17 - AMI-59543 -->
                        string warehouseId = null;
                        if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.WarehouseId))
                        {
                            warehouseId = TranslateWarehouse(_AmitalCustomsFile.WarehouseId);
                        }
                        this._MyDeclarationPM.Consignments[0].StorageSiteCode = warehouseId;
                    
                    if (string.IsNullOrWhiteSpace(_AmitalCustomsFile.IsCourierDeclaration) || (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.IsCourierDeclaration) && _AmitalCustomsFile.IsCourierDeclaration.ToLower() != "true"))
                    {

                        if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.HAWBDATE))
                        {
                            this._MyDeclarationPM.Consignments[0].ManifestDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.HAWBDATE, "AmitalCustomsFile.HAWBDATE");
                        }
                        else if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.ManifestDate))
                        {
                            this._MyDeclarationPM.Consignments[0].ManifestDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.ManifestDate, "AmitalCustomsFile.ManifestDate");
                        }
                    }

                    // moran 1.2.17 - AMI-59543 <--
                    if (this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].ChangeSetOp != ChangeSetOperation.Insert)
                    {
                        this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].ChangeSetOp = ChangeSetOperation.Update;
                    }
                    //this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageTypeCode = TranslatePackingType(_AmitalCustomsFile.PackageTypeCode); 
                    // moran 2.4.14 - add handle in case of empty value -->
                    //this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageTypeCode = _AmitalCustomsFile.PackageTypeCode;
                    if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.PackageTypeCode))
                    {
                        //this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageTypeCode = _AmitalCustomsFile.PackageTypeCode;

                        var packingType = new PackingTypeRepository(ResolvedTenant());
                        var myPackingType = packingType.GetSingle(_AmitalCustomsFile.PackageTypeCode);
                        if (myPackingType == null && setting != null && setting.IsConnectedToUniFreight)
                        {
                            string PackageTypeCode = "";
                            PackageTypeCode = GetTranslationL2P("IIGC", "CTBPACKTYPE", _AmitalCustomsFile.PackageTypeCode);

                            if (!string.IsNullOrWhiteSpace(PackageTypeCode)) this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageTypeCode = PackageTypeCode;
                        }
                        else
                        {
                            this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageTypeCode = myPackingType.Code.ToString();
                        }
                    }
                    else
                    {
                        this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageTypeCode = null;
                    }
                    // moran 2.4.14 - add handle in case of empty value <--
                    this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageMeasureQualifierCode = "2";
                    int packageQuantity = 0;
                    if (int.TryParse(_AmitalCustomsFile.PackageQuantity, out packageQuantity) || string.IsNullOrWhiteSpace(_AmitalCustomsFile.PackageQuantity)) //Yuval Chalup 23.02.2016 TASK-20330 (Add  || string.IsNullOrWhiteSpace(_AmitalCustomsFile.PackageQuantity))
                    {
                        this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].PackageQuantity = packageQuantity;
                    }
                    decimal GrossMassMeasure = 0;
                    if (String.IsNullOrWhiteSpace(this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasureTypeCode))
                    {
                        this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasureTypeCode = "KGM";
                    }
                    if (decimal.TryParse(_AmitalCustomsFile.GrossMassMeasure, out GrossMassMeasure) || string.IsNullOrWhiteSpace(_AmitalCustomsFile.GrossMassMeasure)) //Yuval Chalup 23.02.2016 TASK-20330 (Add  || string.IsNullOrWhiteSpace(_AmitalCustomsFile.GrossMassMeasure))
                    {
                        
                        string isOverrideWeight = CustomsSettingQueryService.GetSettingByTenant(ResolvedTenant()).IsConnectedToUniFreight ? 
                            GetAmitalDefault("ISRAEL", "CIM_NO_OVR_WGT", "NON", _AmitalCustomsFile.CustomerId, ResolvedTenant()) : 
                            "N";
                        if (isOverrideWeight == "Y" && this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasure > 0)
                        {
                            GrossMassMeasure = (decimal)this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasure;
                        }
                        decimal weight = Math.Truncate(GrossMassMeasure);
                        if (weight > 99999999)
                        {
                            GrossMassMeasure = GrossMassMeasure / 1000;
                            this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasureTypeCode = "TNE";
                        }
                        else
                        {
                            if (this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasureTypeCode == "TNE")
                            {
                                this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasureTypeCode = "KGM";
                            }
                        }
                        this._MyDeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasure = GrossMassMeasure;
                    }
                }
                else // moran 19.12.13 - task 2423 - multi Consignments adjusments
                {
                    MyGenericResponseObj.Message = "Declaration has multiple Consignments(" + this._MyDeclarationPM.Consignments.Count.ToString() + ") and Consignment details didn't update";
                    AppendLogLine("Declaration has multiple Consignments(" + this._MyDeclarationPM.Consignments.Count.ToString() + ") and Consignment details didn't update");
                }

                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.VendorId))
                {
                    if (false) //yaron !!!
                    {
                        if (this._MyDeclarationPM.SupplierInvoices == null)
                        {
                            this._MyDeclarationPM.SupplierInvoices = new List<Def.EntityPMs.SupplierInvoicePM>();

                        }
                        if (this._MyDeclarationPM.SupplierInvoices.Count == 0)
                        {
                            this._MyDeclarationPM.SupplierInvoices.Add(new Def.EntityPMs.SupplierInvoicePM() { ChangeSetOp = ChangeSetOperation.Insert, Tenant = ResolvedTenant() }); // moran 20.8.15 Task 15049 - add handle to initiate Tenant
                        }
                        this._MyDeclarationPM.SupplierInvoices[0].VendorId = _AmitalCustomsFile.VendorId;
                    }
                }
                _MyDeclarationPM.Tenant = ResolvedTenant();

				if (string.IsNullOrWhiteSpace(this._MyDeclarationPM.Direction) || this._MyDeclarationPM.Direction == "I") _MyDeclarationPM.IsConnectedToUnifreight = true; //Yuval Chalup 19.10.2016 TASK-22516

				_MyDeclarationPM.IsConnectedToUnifreight = CustomsSettingQueryService.GetSettingByTenant(ResolvedTenant()).IsConnectedToUniFreight;

				//_MyDeclarationPM.ProcedureCurrentCode = ResolveProcedureCurrentCode();//remarked by eitan h 24/9/15 16527
				if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.ProcedureCurrentCode)) _MyDeclarationPM.ProcedureCurrentCode = _AmitalCustomsFile.ProcedureCurrentCode;
                

                MyGenericResponseObj.Stage = "Updating ";
                
                _MyDeclarationPM.CurrentContextTag = UpsertActionConst;

                if (string.IsNullOrWhiteSpace(_AmitalCustomsFile.IsDiamondsDeclaration) || (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.IsDiamondsDeclaration) && _AmitalCustomsFile.IsDiamondsDeclaration.ToLower() != "true"))
                {
                    _MyDeclarationPM.IsDiamondDeclaration = false;

                }
                else
                {
                    _MyDeclarationPM.IsDiamondDeclaration = true;
                }


                //  UpdateTrucker();
                AppendLogLine("ExportDeclarationInsert");
              ExportDeclarationInsert();
                if (string.IsNullOrWhiteSpace(_AmitalCustomsFile.IsCourierDeclaration) || (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.IsCourierDeclaration) && _AmitalCustomsFile.IsCourierDeclaration.ToLower() != "true"))
                {
                    if (_MyDeclarationPM.Direction == "E")
                        ClearWrongValues(_MyDeclarationPM);

                    _MyDeclarationPM.IsCourierDeclaration = false;
                }
                else
                {
                    if (_AmitalCustomsFile.ImporterId != null && string.IsNullOrEmpty(this._MyDeclarationPM.ImporterCode))
                    {
                        myDeclarationUpdateService.ImporterCode = _AmitalCustomsFile.ImporterId;
                    }
                    //Update Declaration
                    _MyDeclarationPM.IsCourierDeclaration = true;
                    _MyDeclarationPM.ProcedureCurrentCode = _AmitalCustomsFile.ProcedureCurrentCode;


                    _MyDeclarationPM.ImporterAddress = _AmitalCustomsFile.ImporterAddress;
                    _MyDeclarationPM.ImporterName = _AmitalCustomsFile.EnglishName;
                    _MyDeclarationPM.ImporterCode = _AmitalCustomsFile.ImporterId;
                    if (string.IsNullOrWhiteSpace(_MyDeclarationPM.ImporterName))
                    {
                        _MyDeclarationPM.ImporterName = _AmitalCustomsFile.HebrewName;
                    }

                    _MyDeclarationPM.CasualSupplierName = _AmitalCustomsFile.CasualSupplierName;
                    _MyDeclarationPM.CasualSupplierAddress = _AmitalCustomsFile.CasualSupplierAddress;
                    _MyDeclarationPM.CourierHAWB = _AmitalCustomsFile.CourierHawb;
                    _MyDeclarationPM.CasualImporterAddress1 = _AmitalCustomsFile.CasualImporterAddress1;
                    _MyDeclarationPM.CasualImporterAddress2 = _AmitalCustomsFile.CasualImporterAddress2;
                    _MyDeclarationPM.CasualImporterCity = _AmitalCustomsFile.CasualImporterCity;
                    _MyDeclarationPM.CasualImporterZipCode = _AmitalCustomsFile.CasualImporterZipCode;
                    _MyDeclarationPM.CasualImporterFax = _AmitalCustomsFile.CasualImporterFax;
                    _MyDeclarationPM.CasualImporterEmail = _AmitalCustomsFile.CasualImporterEmail;
                    _MyDeclarationPM.CasualImporterCountry = _AmitalCustomsFile.CasualImporterCountry;
                   
                    if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.CasualImportelTel)) _MyDeclarationPM.CasualImporterTel = string.Concat(_AmitalCustomsFile.CasualImportelTel.Where(c => !char.IsWhiteSpace(c)));
                    _MyDeclarationPM.CasualImporterContact = _AmitalCustomsFile.CasualImporterContact;
                    if (_MyDeclarationPM.Consignments.Count == 1)
                    {
                        AppendLogLine("one Consignment1");
                        _MyDeclarationPM.Consignments[0].CargoTypeCode = _AmitalCustomsFile.CargoTypeCode;
                        _MyDeclarationPM.Consignments[0].ManifestNumber = _AmitalCustomsFile.ManifestNumber;
                        _MyDeclarationPM.Consignments[0].SecondCargoID = _AmitalCustomsFile.SecondCargoID;
                        if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.ThirdCargoID))
                        {
                            DateTime? datetime = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.ThirdCargoID, "AmitalCustomsFile.ThirdCargoID");
                            _MyDeclarationPM.Consignments[0].ThirdCargoID = datetime.HasValue ? datetime.Value.ToString("ddMMyy") : "";
                        }
                        _MyDeclarationPM.Consignments[0].UnloadDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.UnloadDate, "AmitalCustomsFile.UnloadDate");
                        if (String.IsNullOrWhiteSpace(this._MyDeclarationPM.Consignments[0].UnloadPortCode))
                        {
                            AppendLogLine("one Consignment2");
                            if (this._CourierMasterPM == null)
                            {
                                AppendLogLine("one Consignment3");
                                var myCourierMasterQueryService = new CourierMasterQueryService(_context);
                                _CourierMasterPM = myCourierMasterQueryService.GetByDeclarationId(this._MyDeclarationPM.Id, ResolvedTenant());
                            }
                            if (_CourierMasterPM != null)
                            {
                                AppendLogLine("one Consignment4");
                                CustomsAirlineQueryService customsAirlineQueryService = new CustomsAirlineQueryService(_CourierMasterPM.Tenant);
                                CustomsAirlinePM customsAirline = customsAirlineQueryService.GetSingle(_CourierMasterPM.AirlineId, false, true);
                                if (customsAirline != null)
                                {
                                    AppendLogLine("one Consignment5");
                                    if (!String.IsNullOrWhiteSpace(customsAirline.UnloadPortCode)) this._MyDeclarationPM.Consignments[0].UnloadPortCode = customsAirline.UnloadPortCode;
                                }
                            }
                        }
                        _MyDeclarationPM.Consignments[0].StorageSiteCode = TranslateWarehouse(_AmitalCustomsFile.WarehouseId);
                        AppendLogLine("one Consignment6 Consignment.UnloadPortCode=" + this._MyDeclarationPM.Consignments[0].UnloadPortCode);
                        if (this._MyDeclarationPM.Consignments[0].ChangeSetOp != ChangeSetOperation.Insert)
                        {
                            this._MyDeclarationPM.Consignments[0].ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                    if ("ECommDecInsertService" == moreParams)
                    {
                        scope.Complete();
                        return;
                    }

                    if (_MyDeclarationPM.Direction == "E")
                        ClearWrongValues(_MyDeclarationPM);


#if NOT_OpenCourierMasterourierDeclarationFromUNF
                    //Upsert CourierMaster
                    var myCourierMasterQueryService = new CourierMasterQueryService(_context);
                    var myCourierMasterUpdateService = new CourierMasterUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());
                    var airline = TranslateAirline(_AmitalCustomsFile.AirlineId);
                    _CourierMasterPM = myCourierMasterQueryService.GetSingleByAirlineAWBs(airline, _AmitalCustomsFile.HAWB, _AmitalCustomsFile.MAWB, ResolvedTenant());
                    if (_CourierMasterPM == null)
                    {
                        _CourierMasterPM = new CourierMasterPM();
                        _CourierMasterPM.ChangeSetOp = ChangeSetOperation.Insert;
                        _CourierMasterPM.AirlineId = airline;
                        _CourierMasterPM.MAWB = _AmitalCustomsFile.MAWB;
                        _CourierMasterPM.MAWBTypeCode = "741";
                        _CourierMasterPM.HAWB = _AmitalCustomsFile.HAWB;
                    }
                    else
                    {
                        _CourierMasterPM.ChangeSetOp = ChangeSetOperation.Update;
                    }

                    _CourierMasterPM.MAWBTypeCode = "741";
                    _CourierMasterPM.EstimatedArrivalDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.EstimatedArrivelDate, "AmitalCustomsFile.EstimatedArrivelDate");
                    _CourierMasterPM.GatewayPortCode = TranslateInternationalSite(_AmitalCustomsFile.GatewayPortCode); 
                    _CourierMasterPM.OriginPortCode = TranslateInternationalSite(_AmitalCustomsFile.OriginPortCode);
                    _CourierMasterPM.Tenant = ResolvedTenant();
                    myCourierMasterUpdateService.Update(_CourierMasterPM, true);

                    //Upsert CourierDeclaration
                    var myCourierDeclarationQueryService = new CourierDeclarationQueryService(_context);
                    var myCourierDeclarationUpdateService = new CourierDeclarationUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());
                    _CourierDeclarationPM = myCourierDeclarationQueryService.GetSingle(_MyDeclarationPM.Id, _CourierMasterPM.Id, false, true);
                    if (_CourierDeclarationPM == null)
                    {
                        _CourierDeclarationPM = new CourierDeclarationPM();
                        _CourierDeclarationPM.ChangeSetOp = ChangeSetOperation.Insert;
                        _CourierDeclarationPM.DeclarationId = _MyDeclarationPM.Id;
                        _CourierDeclarationPM.CourierMasterId = _CourierMasterPM.Id;
                    }
                    else
                    {
                        _CourierDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    if (_CourierDeclarationPM.SequenceNumeric == null)
                    {
                        int? sequenceNumericMax = myCourierDeclarationQueryService.GetCourierMasterMaxSequenceNumeric(_CourierDeclarationPM.CourierMasterId, ResolvedTenant());
                        if (sequenceNumericMax == null)
                        {
                            sequenceNumericMax = 0;
                        }
                        _CourierDeclarationPM.SequenceNumeric = sequenceNumericMax + 1;
                    }
                    _CourierDeclarationPM.Tenant = ResolvedTenant();
                    myCourierDeclarationUpdateService.Update(_CourierDeclarationPM, true);


#endif

                }

                //UpdateTrucker();

                try
                {
                    string truckerId = GetTruckerId(this._MyDeclarationPM.Tenant, _AmitalCustomsFile.TruckerId);
                    myDeclarationUpdateService.IsFromU2L = true;
                    myDeclarationUpdateService.TruckerId = truckerId;
                    myDeclarationUpdateService.DistributionArea = _AmitalCustomsFile.DistributionArea;
                    myDeclarationUpdateService.LastMileServiceType = _AmitalCustomsFile.LastMileServiceType;
                    myDeclarationUpdateService.MAWB = _AmitalCustomsFile.MAWB;
                    myDeclarationUpdateService.Update(_MyDeclarationPM, true);
                }
                catch (DbEntityValidationException ex)
                {
                    var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                    AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                    return;
                }
                catch (Exception e)
                {
                    AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                    return;
                }

                AppendLogLine(LogMessagingUtil.Instance.ToString());

                

                if (String.IsNullOrWhiteSpace(_MyDeclarationPM.Id))
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "Update return null";
                    return;
                }

                DeclarationReferantDataUpdate();
                MyGenericResponseObj.ApplicationId = _MyDeclarationPM.Id;
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
                MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;

                scope.Complete();

            }
        }

        private void ClearWrongValues(DeclarationPM declarationPm)
        {
            int tenant = ResolvedTenant();
            ForiegnKeyCheck.CheckClosedTable(declarationPm, tenant);
            ForiegnKeyCheck.Check<Declaration>(declarationPm, tenant);

            declarationPm.Consignments.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<Consignment>(x, tenant);
            });

            declarationPm.SupplierInvoices.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<SupplierInvoice>(x, tenant);
            });

            declarationPm.DeclarationTaxes.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DeclarationTax>(x, tenant);
            });

            declarationPm.DeclarationConstraints.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DeclarationConstraint>(x, tenant);
            });

            declarationPm.DeclarationConsAcceptances.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DeclarationConsAcceptancePM>(x, tenant);
            });

            declarationPm.DecDangersContacts.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DecDangersContact>(x, tenant);
            });

            declarationPm.DeclarationExportRecipients.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DeclarationExportRecipient>(x, tenant);
            });
        }
        private void ExportDeclarationUpdate()
        {
            if (_AmitalCustomsFile.Direction == "E" && _AmitalCustomsFile.Mode != "NEW")
            {
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.FlightDate))
                {
                    this._MyDeclarationPM.ExportFlightDate = DateTime.Parse(_AmitalCustomsFile.FlightDate);
                    _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                else
                {
                    if(this._MyDeclarationPM.ExportFlightDate != null)
                    {
                        this._MyDeclarationPM.ExportFlightDate = null;
                        _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(_context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                AppendLogLine("try to update FlightDate " + _AmitalCustomsFile.FlightDate + " to DeclarationPM.Id: " + _MyDeclarationPM.Id);
                try
                {
                    declarationUpdateService.Update(_MyDeclarationPM, true);
                }
                catch (DbEntityValidationException ex)
                {
                    var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                    AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                    return;
                }
                catch (Exception e)
                {
                    AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                    return;
                }
            }
        }

        private void ExportDeclarationInsert()
        {
            AppendLogLine("in ExportDeclarationInsert");
            if (_AmitalCustomsFile.Direction == "E" && _AmitalCustomsFile.Mode == "NEW")
            {

                ///DeclarationPM
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.DestinationCountryCode))
                {
                    this._MyDeclarationPM.DestinationCountryCode = _AmitalCustomsFile.DestinationCountryCode;
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.ImporterFile))
                {
                    this._MyDeclarationPM.ExportFile = _AmitalCustomsFile.ImporterFile;
                }

                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.UNFCourier) || (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.UNFCourier) && _AmitalCustomsFile.UNFCourier.ToLower() != "true"))
                {
                    _MyDeclarationPM.UNFCourier = false;
                }
                else
                {
                    _MyDeclarationPM.UNFCourier = true;

                }

                //DeclarationExportRecipients
                if (this._MyDeclarationPM.DeclarationExportRecipients.Count == 0)
                {
                    this._MyDeclarationPM.DeclarationExportRecipients.Add(new DeclarationExportRecipientPM() { ChangeSetOp = ChangeSetOperation.Insert, Tenant = ResolvedTenant() });
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.BuyerName))
                {
                    this._MyDeclarationPM.DeclarationExportRecipients[0].RecipientName = _AmitalCustomsFile.BuyerName;
                }

                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.BuyerAddress))
                {
                    this._MyDeclarationPM.DeclarationExportRecipients[0].RecipientAddress = _AmitalCustomsFile.BuyerAddress;
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.BuyerCountryCode))
                {
                    this._MyDeclarationPM.DeclarationExportRecipients[0].RecipientIssueCountryCode = _AmitalCustomsFile.BuyerCountryCode;
                }
                //SupplierInvoices
                this.CreateSupplierInvoices();
               
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.BuyerRoleCode))
                {
                    this._MyDeclarationPM.SupplierInvoices[0].BuyerRoleCode = _AmitalCustomsFile.BuyerRoleCode;
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.CargoTypeCode))
                {
                    this._MyDeclarationPM.Consignments[0].CargoTypeCode = _AmitalCustomsFile.CargoTypeCode;
                }

                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.SecondCargoID))
                {
                    this._MyDeclarationPM.Consignments[0].SecondCargoID = _AmitalCustomsFile.SecondCargoID;
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.ThirdCargoID))
                {
                    this._MyDeclarationPM.Consignments[0].ThirdCargoID = _AmitalCustomsFile.ThirdCargoID;
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.LoadingPortCode))
                {
                    this._MyDeclarationPM.Consignments[0].ExportLoadingPortCode = _AmitalCustomsFile.LoadingPortCode;
                }

                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.UnloadportId))
                {
                    this._MyDeclarationPM.Consignments[0].FinalDestinationPortCode = _AmitalCustomsFile.UnloadportId;
                }
                if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.ExportUnloadingPortCode))
                {
                    this._MyDeclarationPM.Consignments[0].ExportUnloadingPortCode = _AmitalCustomsFile.ExportUnloadingPortCode;
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.StorageSiteCode))
                {
                    this._MyDeclarationPM.Consignments[0].StorageSiteCode = _AmitalCustomsFile.StorageSiteCode;
                }
                if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.ShipCode))
                {
                    this._MyDeclarationPM.Consignments[0].ShipCode = _AmitalCustomsFile.ShipCode;
                }
            }
        }

        private void CreateSupplierInvoices()
        {
            AppendLogLine("CreateSupplierInvoices");
          var firstExits = this._MyDeclarationPM.SupplierInvoices.Count() > 0;
            AppendLogLine("CreateSupplierInvoices" + firstExits);
            var invoicesArrayToAdd = _AmitalCustomsFile.Invoices?.Invoice?.Length > 0 ? _AmitalCustomsFile.Invoices?.Invoice : new ExportInvoice[] { new ExportInvoice() };
            int sequenceCounter = 0;
            Array.ForEach(invoicesArrayToAdd, (invoice) =>
            {
                AppendLogLine("CreateSupplierInvoices" + invoice.ToString());
                if (firstExits)
                {
                    AppendLogLine("CreateSupplierInvoices firstExits");
                    InitSupplierInvoice(invoice, this._MyDeclarationPM.SupplierInvoices[0]);

                }
                else
                {
                    AppendLogLine("CreateSupplierInvoices firstExits else");
                    sequenceCounter++;
                     var suppplierInvoice = new SupplierInvoicePM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        Tenant = ResolvedTenant(),
                        SequenceNumeric = sequenceCounter++,
                    };

                    InitSupplierInvoice(invoice, suppplierInvoice);
                    AppendLogLine(" this._MyDeclarationPM.SupplierInvoices.Add(suppplierInvoice);");

                    this._MyDeclarationPM.SupplierInvoices.Add(suppplierInvoice);
                }
             });

        }
        public List<SupplierInvoiceItemPM> initSupplierInvoiceItems(ExportInvoice invoice,SupplierInvoicePM supplierInvoice)
        {
            var SupplierInvoiceItemPMList = new List<SupplierInvoiceItemPM>();
            int int1 = 0;
            foreach (var invoiceItem in invoice?.InvoiceItems?.InvoiceItem)
            {
                int1++;
                decimal decimal1 = 0;
                var SupplierInvoiceItemPM = new SupplierInvoiceItemPM();
                SupplierInvoiceItemPM.CounterKey = supplierInvoice.InvoiceCounterKey;
                SupplierInvoiceItemPM.DeclarationId = supplierInvoice.DeclarationId;
                SupplierInvoiceItemPM.SequenceNumeric = int1;
                SupplierInvoiceItemPM.LineNumber = int1;
                if (decimal.TryParse(invoiceItem.ItemQuantity, out  decimal1))
                {
                    SupplierInvoiceItemPM.InvoiceQuantity = decimal1;
                }
                SupplierInvoiceItemPM.ItemCode = invoiceItem.ItemNo;
                SupplierInvoiceItemPM.ItemDescription = invoiceItem.ItemDescription;
                SupplierInvoiceItemPM.ClassificationCode = invoiceItem.ItemHScode;
                if (decimal.TryParse(invoiceItem.ItemQuantity, out  decimal1))
                {
                    SupplierInvoiceItemPM.InvoiceQuantity = decimal1;
                }
                SupplierInvoiceItemPM.InvoiceQuantityType = TranslateMeasurmentUnit(invoiceItem.ItemQuantityType);
                if (decimal.TryParse(invoiceItem.ItemAmount, out decimal ItemAmount))
                {
                    SupplierInvoiceItemPM.ItemPrice = ItemAmount;
                }
                SupplierInvoiceItemPM.OriginCountryCode = invoiceItem.ItemOriginCountry;
                SupplierInvoiceItemPM.Tenant = ResolvedTenant();
                SupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                SupplierInvoiceItemPMList.Add(SupplierInvoiceItemPM);

            }
            return SupplierInvoiceItemPMList;

        }
        private string TranslateMeasurmentUnit(string amitalMeasurmentUnitCode)
        {
            var measurmentUnit = new MeasurmentUnitRepository(ResolvedTenant());
            var myMeasurmentUnit = measurmentUnit.GetSingle(amitalMeasurmentUnitCode);
            if (myMeasurmentUnit == null)
            {
                AppendLogLine("amitalMeasurmentUnitCode = " + amitalMeasurmentUnitCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalMeasurmentUnitCode = " + amitalMeasurmentUnitCode + " Translated to " + myMeasurmentUnit.Code);
            return myMeasurmentUnit.Code;
        }
        private void InitSupplierInvoice(ExportInvoice invoice, SupplierInvoicePM supplierInvoice)
        {
            AppendLogLine("InitSupplierInvoice" + supplierInvoice.DeclarationId);
            AppendLogLine("InitSupplierInvoice" + invoice?.Number);
            AppendLogLine("InitSupplierInvoice" + invoice?.Date);
            AppendLogLine("InitSupplierInvoice hh" + invoice?.IsEmpty);

            supplierInvoice.VendorId = _AmitalCustomsFile.VendorId;
            if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.InvoiceNumber))
            {
                AppendLogLine("!String.IsNullOrWhiteSpace(_AmitalCustomsFile.InvoiceNumber");

                supplierInvoice.InvoiceNumber = _AmitalCustomsFile.InvoiceNumber;
            }
            AppendLogLine("else !String.IsNullOrWhiteSpace(_AmitalCustomsFile.InvoiceNumber");
            // if (!invocie.IsEmpty)
            
            if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.IncotermCode))
            {
                supplierInvoice.IncotermCode = _AmitalCustomsFile.IncotermCode;
            }
            if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.BuyerName))
            {
                supplierInvoice.BuyerName = _AmitalCustomsFile.BuyerName;
            }
            if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.BuyerAddress))
            {
                supplierInvoice.BuyerAddress = _AmitalCustomsFile.BuyerAddress;
            }
            if (!String.IsNullOrWhiteSpace(_AmitalCustomsFile.BuyerCountryCode))
            {
                supplierInvoice.BuyerCountryCode = _AmitalCustomsFile.BuyerCountryCode;
            }
            if (invoice != null)
            {
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceNum))
                {
                    supplierInvoice.InvoiceNumber = invoice.InvoiceNum;
                }
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceDate))
                {
                    supplierInvoice.IssueDate = !String.IsNullOrWhiteSpace(invoice?.InvoiceDate) ? DateTime.Parse(invoice?.InvoiceDate) : supplierInvoice.IssueDate;
                }
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceAmount))
                {
                    if (decimal.TryParse(invoice.InvoiceAmount, out decimal amount))
                    {
                        supplierInvoice.InvoiceAmount = amount;
                    }
                }
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceCurrency))
                {
                    supplierInvoice.InvoiceCurrencyTypeCode = invoice.InvoiceCurrency;
                }
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceType))
                {
                    supplierInvoice.AccountTypeCode = invoice.InvoiceType;
                }
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceIncoterms))
                {
                    supplierInvoice.IncotermCode = invoice.InvoiceIncoterms;
                }
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceBuyerName))
                {
                    supplierInvoice.BuyerName = invoice.InvoiceBuyerName;
                }
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceBuyerAddress))
                {
                    supplierInvoice.BuyerAddress = invoice.InvoiceBuyerAddress;
                }
                if (!string.IsNullOrWhiteSpace(invoice.InvoiceBuyerCountryCode))
                {
                    supplierInvoice.BuyerCountryCode = invoice.InvoiceBuyerCountryCode;
                }
                if (invoice.InvoiceItems != null && invoice.InvoiceItems.InvoiceItem.Length > 0)
                {
                    supplierInvoice.SupplierInvoiceItems = initSupplierInvoiceItems(invoice, supplierInvoice);
                }
            }
        }
        private void DeclarationReferantDataUpdate()
        {
            MyGenericResponseObj.Stage = "DeclarationReferantDataUpsert";

            var myDeclarationReferantDataQueryService = new DeclarationReferantDataQueryService(_context);
            var myDeclarationReferantDataUpdateService = new DeclarationReferantDataUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());

            MyGenericResponseObj.Stage = "GetSingle DeclarationReferantData";
            this._DeclarationReferantDataPM = myDeclarationReferantDataQueryService.GetSingle(this._MyDeclarationPM.Id, true, false);
            /// Exist
            bool isNew = false;
            if (_DeclarationReferantDataPM == null)
            {
                this._DeclarationReferantDataPM = new Def.EntityPMs.DeclarationReferantDataPM();
                this._DeclarationReferantDataPM.ChangeSetOp = ChangeSetOperation.Insert;
                this._DeclarationReferantDataPM.DeclarationId = this._MyDeclarationPM.Id;
                this._DeclarationReferantDataPM.IsClosedForFollowUp = "0";

                isNew = true;
            }
            else
            {
                this._DeclarationReferantDataPM.ChangeSetOp = ChangeSetOperation.Update;
            }
            decimal myGrossMassMeasure = 0;
            if (decimal.TryParse(_AmitalCustomsFile.GrossMassMeasure, out myGrossMassMeasure) || string.IsNullOrWhiteSpace(_AmitalCustomsFile.GrossMassMeasure))
            {
                this._DeclarationReferantDataPM.Weight = myGrossMassMeasure;
            }
            this._DeclarationReferantDataPM.ArrivalDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.ArrivalDateTime, "AmitalCustomsFile.ArrivalDateTime");
            this._DeclarationReferantDataPM.VendorId = TranslateVendor(_AmitalCustomsFile.VendorId);

            this._DeclarationReferantDataPM.EstimatedArrivalDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.EstimatedTimeOfArrival, "AmitalCustomsFile.EstimatedTimeOfSrrival");
            this._DeclarationReferantDataPM.OrderNumber = _AmitalCustomsFile.OrderNumber;
            if (string.IsNullOrWhiteSpace(_AmitalCustomsFile.WithPaper) || (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.WithPaper) && _AmitalCustomsFile.WithPaper.ToLower() != "true"))
            {
                this._DeclarationReferantDataPM.WithPaper = false;

            }
            else
            {
                this._DeclarationReferantDataPM.WithPaper = true;
            }
            if (string.IsNullOrWhiteSpace(_AmitalCustomsFile.NewFile) || (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.NewFile) && _AmitalCustomsFile.NewFile.ToLower() != "true"))
            {
                this._DeclarationReferantDataPM.NewFile = false;

            }
            else
            {
                this._DeclarationReferantDataPM.NewFile = true;
            }
            this._DeclarationReferantDataPM.ImporterFile = _AmitalCustomsFile.ImporterFile;
            this._DeclarationReferantDataPM.Team = TranslateTeam(_AmitalCustomsFile.Team);

            this._DeclarationReferantDataPM.FileOpenDate = AmitalConvertUtil.GetUnifreightFormatedDate(_AmitalCustomsFile.FileOpenDate, "AmitalCustomsFile.FileOpenDate");
            if (!string.IsNullOrWhiteSpace(_AmitalCustomsFile.FclLcl))
            {
                this._DeclarationReferantDataPM.FclLcl = _AmitalCustomsFile.FclLcl;
            }
            this._DeclarationReferantDataPM.ForwarderId = TranslateForwarder(_AmitalCustomsFile.ForwarderId);
            
            int packageQuantity = 0;
            if (int.TryParse(_AmitalCustomsFile.PackageQuantity, out packageQuantity) || string.IsNullOrWhiteSpace(_AmitalCustomsFile.PackageQuantity))
            {
                this._DeclarationReferantDataPM.PackageQuantity = packageQuantity;
            }
            this._DeclarationReferantDataPM.Commodity = _AmitalCustomsFile.Commodity;
            this._DeclarationReferantDataPM.Hawb = _AmitalCustomsFile.ReferentHAWB;
            this._DeclarationReferantDataPM.Mawb = _AmitalCustomsFile.ReferentMAWB;
            this._DeclarationReferantDataPM.Tenant = ResolvedTenant();
            myDeclarationReferantDataUpdateService.Update(this._DeclarationReferantDataPM, true);

        }

        private string TranslateForwarder(string forwarderId)
        {
            if (String.IsNullOrWhiteSpace(forwarderId))
            {
                AppendLogLine("forwarderId is null");
                return null;
            }
            CardRepository cardRep = new CardRepository(ResolvedTenant());
            Card card = cardRep.GetSingleCard(forwarderId, ResolvedTenant());
            if (card != null)
            {
                return card.Id;
            }
            else
            {
                card = cardRep.GetSingleCardByCode(forwarderId, ResolvedTenant(), true);
                if (card != null)
                {
                    return card.Id;
                }
            }
            AppendLogLine("No forwarder found for forwarderId " + forwarderId);
            return null;
        }

        private string TranslateVendor(string amitalvendorId)
        {
            if (String.IsNullOrWhiteSpace(amitalvendorId))
            {
                AppendLogLine("amitalvendorId is null");
                return null;

            }
            CustomsVendor myCustomsVendor = null;

            var repository = new CustomsVendorRepository(ResolvedTenant());
            myCustomsVendor = repository.GetVendorByNumber(amitalvendorId, ResolvedTenant());

            if (myCustomsVendor != null)
            {
                return myCustomsVendor.Id;
            }
            AppendLogLine("No vendor found for vendorId " + amitalvendorId);
            return null;
        }


        private string TranslateTeam(string amitalTeamId)
        {
            if (String.IsNullOrWhiteSpace(amitalTeamId))
            {
                AppendLogLine("amitalTeamId is null");
                return null;
            }
            string referantTeamCode = null;
            ReferantTeamListQueryService ReferantTeamListQuery = new ReferantTeamListQueryService(_context);
            ReferantTeamList ReferantTeam = ReferantTeamListQuery.GetSingle(amitalTeamId);
            if (ReferantTeam != null && !ReferantTeam.Inactive)
            {
                referantTeamCode = ReferantTeam.Code;
            }
            else
            {
                AppendLogLine("amitalTeamId = " + amitalTeamId + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalTeamId = " + amitalTeamId + " Translated to " + referantTeamCode);
            return referantTeamCode;


        }

        private string TranslateAirline(string airlineId)
        {
            if (String.IsNullOrWhiteSpace(airlineId))
            {
                AppendLogLine("airlineId is null");
                return null;
            }

            //GET Airline.Id BY PREFIX
            AirlineRepository airlineRepository = new AirlineRepository(_MyDeclarationPM.Tenant);
            Airline airline = airlineRepository.GetSingleAirlineByPrefix(airlineId, _MyDeclarationPM.Tenant);
            if (airline != null)
            {
                return airline.Id;
            }
            AppendLogLine("No Airline found for airlineId " + airlineId);
            return null;
        }

        private string TranslateWarehouse(string warehouseId)
        {
            if (String.IsNullOrWhiteSpace(warehouseId))
            {
                AppendLogLine("warehouseId is null");
                return null;
            }
            string storageSiteCode = null;
            DeliverySiteTypeQueryService DeliverySiteQuery = new DeliverySiteTypeQueryService(ResolvedTenant());
            DeliverySiteTypePM DeliverySite = DeliverySiteQuery.GetSingle(warehouseId, true, false);
            if (DeliverySite != null && !DeliverySite.Inactive)
            {
                storageSiteCode = DeliverySite.Code;
            }
            else
            {
                AppendLogLine("warehouseId = " + warehouseId + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("warehouseId = " + warehouseId + " Translated to " + storageSiteCode);
            return storageSiteCode;
        }

        private string TranslateInternationalSite(string internationalSite)
        {
            if (String.IsNullOrWhiteSpace(internationalSite))
            {
                AppendLogLine("unloadportId is null");
                return null;
            }
            string internationalSiteId = null;
            InternationalSiteQueryService internationalSiteQueryService = new InternationalSiteQueryService(ResolvedTenant());
            InternationalSitePM internationalSitePM = internationalSiteQueryService.GetSingle(internationalSite, true, false);
            if (internationalSitePM != null && !internationalSitePM.Inactive)
            {
                internationalSiteId = internationalSitePM.Code;
            }
            else
            {
                AppendLogLine("unloadportId = " + internationalSite + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("unloadportId = " + internationalSite + " Translated to " + internationalSiteId);
            return internationalSiteId;
        }


        private string TranslateUnloadPort(string unloadportId)
        {
            if (String.IsNullOrWhiteSpace(unloadportId))
            {
                AppendLogLine("unloadportId is null");
                return null;
            }
            string portId = null;
            UnloadingSiteTypeQueryService UnloadingPortQuery = new UnloadingSiteTypeQueryService(ResolvedTenant());
            UnloadingSiteTypePM UnloadingPort = UnloadingPortQuery.GetSingle(unloadportId, true, false);
            if (UnloadingPort != null && !UnloadingPort.Inactive)
            {
                portId = UnloadingPort.Code;
            }
            else
            {
                AppendLogLine("unloadportId = " + unloadportId + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("unloadportId = " + unloadportId + " Translated to " + portId);
            return portId;
        }



        private string TranslateloadPort(string loadportId)
        {
            if (String.IsNullOrWhiteSpace(loadportId))
            {
                AppendLogLine("loadportId is null");
                return null;
            }
            string portId = null;
            InternationalSiteQueryService InternationalSiteQuery = new InternationalSiteQueryService(ResolvedTenant());
            InternationalSitePM InternationalSite = InternationalSiteQuery.GetSingle(loadportId, true, false);
            if (InternationalSite != null && !InternationalSite.Inactive)
            {
                portId = InternationalSite.Code;
            }
            else
            {
                AppendLogLine("loadportId = " + loadportId + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("loadportId = " + loadportId + " Translated to " + portId);
            return portId;
        }

        private string TranslateClient(string amitalImporterId)
        {
            if (String.IsNullOrWhiteSpace(amitalImporterId))
            {
                AppendLogLine("amitalImporterId is null");
                return null;
            }
            ///using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ClientQueryService clientQueryService = new ClientQueryService(ResolvedTenant());

                var clientId = clientQueryService.GetIdByCode(amitalImporterId, ResolvedTenant());

                if (clientId == null)
                {
                    AppendLogLine("amitalImporterId = " + amitalImporterId + " could not translate to Logitude Id");
                    return null;
                }
                AppendLogLine("amitalImporterId = " + amitalImporterId + " Translated to " + clientId);
                return clientId;
            }
        }


        private string TranslateUser(string userId)
        {

            if (String.IsNullOrWhiteSpace(userId))
            {
                AppendLogLine("amitalReferentUserId is null");
                return null;
            }
            var repository = new UserRepository(ResolvedTenant());
            var myUserCard = repository.GetSingleUserByCode(userId, ResolvedTenant(), false);  //TODO: this function include all 
            if (myUserCard == null)
            {
                AppendLogLine("amitalReferentUserId = " + userId + " could not translate to Logitude Id");
                return null;
            }
            var cardId = myUserCard.Id;
            AppendLogLine("amitalReferentUserId = " + userId + " Translated to " + cardId);
            return cardId;
        }



        private string TranslateDepartment(string amitalDepartmentCode)
        {
            if (String.IsNullOrWhiteSpace(amitalDepartmentCode))
            {
                AppendLogLine("amitalDepartmentCode is null");
                return null;
            }
            var repository = new DepartmentRepository(ResolvedTenant());
            var myCard = repository.GetSingleDepartmentByCode(amitalDepartmentCode, ResolvedTenant(),true);  //TODO: this function include all 
            if (myCard == null)
            {
                AppendLogLine("amitalDepartmentCode = " + amitalDepartmentCode + " could not translate to Logitude Id");
                return null;
            }
            var cardId = myCard.Id;
            AppendLogLine("amitalDepartmentCode = " + amitalDepartmentCode + " Translated to " + cardId);
            return cardId;
        }

        private string TranslateCustomer(string amitalCustomerCode)
        {
            if (String.IsNullOrWhiteSpace(amitalCustomerCode))
            {
                AppendLogLine("AmitalCustomerCode is null");
                return null;
            }
            Card myCard = null;
            ///using (var cardScope = TransactionFactory.GetNewTransaction())
            //using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                var repository = new CardRepository(ResolvedTenant());
                myCard = repository.GetSingleCardByCode(amitalCustomerCode, ResolvedTenant(), false);// why not from cache - maybe just now updated !!

                if (myCard == null)
                {
                    AppendLogLine("Customer Card does not exist for Amital Customer Code " + amitalCustomerCode);
                    MyGenericResponseObj.Message = "Customer Card does not exist for Amital Customer Code " + amitalCustomerCode;

                    if(CustomsSettingQueryService.GetSettingByTenant(ResolvedTenant()).IsConnectedToUniFreight)
                        return null;

                    AppendLogLine("Open A new Card in the same Transaction Scope ");
                    var cardRep = new CardRepository(ResolvedTenant());

                    myCard = new Card()
                    {
                        Id = IdCounter.GetNumber("Card", ResolvedTenant()).ToString(),
                        Code = _AmitalCustomsFile.CustomerId,
                        EnglishName = _AmitalCustomsFile.EnglishName,
                        LocalName = _AmitalCustomsFile.HebrewName,
                        InActive = true,
                        VatNumber = _AmitalCustomsFile.ImporterId,
                        PartnerTypeId = "CS",
                        Tenant = ResolvedTenant(),
                        CreateDate = DateTime.Now,
                        CountryCode = _AmitalCustomsFile.OriginCountryCode,
                        SearchFields =
                           _AmitalCustomsFile.CustomerId + ", " +
                           _AmitalCustomsFile.EnglishName + ", " +
                           _AmitalCustomsFile.HebrewName + ", " +
                           _AmitalCustomsFile.ImporterId + ", " +
                           "CS, " +
                           _AmitalCustomsFile.OriginCountryCode
                    };

                    cardRep.Add(myCard);
                    cardRep.SubmitChanges();

                    AppendLogLine("Create new Card  = " + amitalCustomerCode + " because could not translate to Logitude Id");
                    AppendLogLine("  teannt is   = "+ ResolvedTenant());

                    //Create a new customer
                    Customer myCustomer = new Customer()
                    {
                        Id = myCard.Id,
                        Tenant = myCard.Tenant,
                    };
                    RankRepository rankRep = new RankRepository(CommonDataContext.GetContext(myCard.Tenant));
                    Rank rank = rankRep.GetSingleRankByCode("1", myCard.Tenant);
                    if (rank != null)
                    {
                        myCustomer.RankId = rank.Id;
                    }
                    var customerRepository = new CustomerRepository(myCard.Tenant);
                    customerRepository.Add(myCustomer);
                    customerRepository.SubmitChanges();
                    AppendLogLine("Create new Customer (Customer tables)  = " + amitalCustomerCode);

                    //RunStoredProcedureClass.UpdateCardSearcsRecords(myCard.Id, myCard.Tenant);

                }
            }
            var cardId = myCard.Id;

            AppendLogLine("AmitalCustomerCode = " + amitalCustomerCode + " Translated to " + cardId);
            var checkIfExist = false;
            if (checkIfExist)
            {
                var repository1 = new CardRepository(ResolvedTenant());
                var myCard1 = repository1.GetSingleCardByCode(amitalCustomerCode, ResolvedTenant(), false);
                if (myCard1 == null)
                {

                    AppendLogLine("after cardScope commit  not found ");
                }
            }

            return cardId;
        }

        private string TranslateCountry(string amitalCountryCode)
        {
            if (String.IsNullOrWhiteSpace(amitalCountryCode))
            {
                AppendLogLine("amitalCountryCode is null");
                return null;
            }

            CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(ResolvedTenant());
            var myCountry = customsCountryQueryService.GetSingle(amitalCountryCode, false, false);

            if (myCountry.Code == null)
            {
                AppendLogLine("amitalCountryCode = " + amitalCountryCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalCountryCode = " + amitalCountryCode + " Translated to " + myCountry.Code);
            return myCountry.Code;

            //var repository = new CountryRepository(ResolvedTenant());
            //var myCountry = repository.GetSingleCountryByCode(amitalCountryCode, ResolvedTenant(), false);
            //if (myCountry == null)
            //{
            //    AppendLogLine("amitalCountryCode = " + amitalCountryCode + " could not translate to Logitude Id");
            //    return null;
            //}
            //var CountryId = myCountry.Id;
            //AppendLogLine("amitalCountryCode = " + amitalCountryCode + " Translated to " + CountryId);
            //return CountryId;
        }

        private string TranslatePackingType(string amitalPackingTypeCode)
        {
            if (String.IsNullOrWhiteSpace(amitalPackingTypeCode))
            {
                AppendLogLine("amitalPackingTypeCode is null");
                return null;
            }

            PackingTypeQueryService packingTypeQueryService = new PackingTypeQueryService(ResolvedTenant());
            var myPackingType = packingTypeQueryService.GetSingle(amitalPackingTypeCode, false, false);

            if (myPackingType.Code == null)
            {
                AppendLogLine("amitalPackingTypeCode = " + amitalPackingTypeCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalPackingTypeCode = " + amitalPackingTypeCode + " Translated to " + myPackingType.Code);
            return myPackingType.Code;

            // var repository = new PackageTypeRepository(ResolvedTenant());
            ////PackingTypeQueryService packingTypeQueryService = new PackingTypeQueryService(ResolvedTenant());

            // var myPackageType = repository.GetSinglePackageType(amitalPackingTypeCode, ResolvedTenant());   
            // if (myPackageType == null)
            // {
            //     AppendLogLine("amitalPackingTypeCode = " + amitalPackingTypeCode + " could not translate to Logitude Id");
            //     return null;
            // }
            // var PackageTypeId = myPackageType.Id;
            // AppendLogLine("amitalPackingTypeCode = " + amitalPackingTypeCode + " Translated to " + PackageTypeId);
            // return PackageTypeId;
        }


        private string ResolveProcedureCurrentCode()
        {
            return null;
            //return "";//eitan h 16/9/15 task 16527 changed to null again...
            //return "4000001";   // moran 30.6.14 - Task 6624 - changed to null - moran 7.7.14 - undo - mail by yaron
        }






        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }




        protected override int ResolvedTenant() // moran 7.9.14 - Task 7860
        {
            return int.Parse(_LOGICUSTFILE.LogitudeCustomsFile[0].Tenant);
        }




        public override void ProccessGenericRequest(
                string xmlLOGICUSTFILE,
                ref string MoreParams,
                out string MessageOut)
        {
            //DataOut1 = DataOut2 = 
            MessageOut = "";
            //SUCCESS = false.ToString();


            try
            {

                MyCommunicationsParams.Subject = "DeclarationUpsertService ";


                MyGenericResponseObj.Stage = "Initalize ProccessRequest";
                AppendLogLine("DeclarationUpsertService.ProccessRequest");

                AppendLogLine("Deserialize(DataIn1) ..");


                //int tenant;
                //if (!int.TryParse(stenant, out tenant))
                //{
                //    throw new Exception("Tenant is not int  !!!");
                //}

                if (string.IsNullOrWhiteSpace(xmlLOGICUSTFILE))
                {

                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "DataIn1 is missing !!!";
                    return;
                    //_GenericResponseObj.ExceptionType=""
                    //throw new Exception("DataIn1 is missing !!!");
                }
                if (xmlLOGICUSTFILE.Length > 1000)
                {
                    AppendLogLine("XmlIn=" + xmlLOGICUSTFILE.Substring(0, 1000));
                    AppendLogLine(".Substring(0, 1000)");
                }
                else
                {
                    AppendLogLine("XmlIn=" + xmlLOGICUSTFILE);
                }
                AppendLogLine("Tring DeserilazeObject");
                MyGenericResponseObj.Stage = "Tring DeserilazeObject";
                this._LOGICUSTFILE = XmlGenericUtil<LOGICUSTFILE>.DeSerializeObject(xmlLOGICUSTFILE);

                if (_LOGICUSTFILE.LogitudeCustomsFile == null || _LOGICUSTFILE.LogitudeCustomsFile.Length != 1)
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "customFile.LogitudeCustomsFile.Length !=1 !!!";
                    return;
                    //throw new Exception("customFile.LogitudeCustomsFile.Length !=1 !!!");
                }

                this._AmitalCustomsFile = _LOGICUSTFILE.LogitudeCustomsFile[0];
                MyCommunicationsParams.Tenant = ResolvedTenant();

                if (MoreParams == "CommDecService")
                {
                    mode = "UpdateNotEmpty";
                }

                MyGenericResponseObj.Stage = "Upsert";
                Upsert(suppressNewTrans, MoreParams);
                MyGenericResponseObj.Stage = "Done";



            }
            catch (DbEntityValidationException ex)
            {
                var formatedException = ExceptionFormatUtil.GetFormated(ex);

                InsertLogLine(0, "ProccessRequest():Exception " + formatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                Debug.WriteLine("ProccessRequest():Exception " + formatedException.ToString(), true);
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                MyGenericResponseObj.Message = "Error while DeclarationUpdateService.Update " + formatedException.Message;
                MyGenericResponseObj.ErrorDescription = formatedException.ToString();
                if (formatedException.InnerException != null)
                {
                    MyGenericResponseObj.InnerException = formatedException.InnerException.ToString();
                }

                MessageOut = MyGenericResponseObj.ErrorDescription;
            }
            catch (Exception e)
            {

                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.TecinicalFailure;
                MyGenericResponseObj.Message = "Exception: " + e.Message;
                MyGenericResponseObj.ErrorDescription = e.ToString();
                if (e.InnerException != null)
                {
                    MyGenericResponseObj.InnerException = e.InnerException.ToString();
                }
                MessageOut = MyGenericResponseObj.ErrorDescription;

            }
            finally
            {
                //_GenericResponseObj 
                try
                {
                    if (!String.IsNullOrWhiteSpace(MyCommunicationsParams.LoggingEntityId))
                    {
                        MyCommunicationsParams.LoggingObjectTableId = GetLoggingObjectTableId("Customs.Declaration");

                    }
                    ///DataOut1 = XmlGenericUtil<GenericResponseObj>.SerializeObject(MyGenericResponseObj);

                    //MyGenericResponseObj = null;
                }
                catch (Exception)
                {

                    ///                    throw;
                }

            }
        }

        public string GetTranslationL2P(string partnerID, string tableID, string localCode)
        {
            if (amitalContext == null) return null;
            var rec = (from a in amitalContext.GTRTRANs
                       where a.PARTNERID == partnerID && a.TABLEID == tableID && a.LOCALCODE == localCode
                       select a).FirstOrDefault();
            if (rec == null)
            {
                return null;
            }
            return rec.PARTNERCODE;
        }


        private string GetAmitalDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant)
        {
            if (amitalContext == null) return null;
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
        }


        public static void TestDeclarationAdd(int CustomFileNo, string CopyFromDeclarationId)
        {
            var amitalObjExample = new LOGICUSTFILE();
            var myAmitalCustom = new LogitudeCustomsFile();
            myAmitalCustom.CustomFileNo = CustomFileNo.ToString();//"41350144";

            myAmitalCustom.DeclarationOfficeCode = "4";
            myAmitalCustom.AgentId = "550221105";

            myAmitalCustom.CreatedByUserId = "ITZIK";
            myAmitalCustom.CustomerId = "10010650";
            myAmitalCustom.TransportModeId = "O";
            myAmitalCustom.Tenant = "1";
            myAmitalCustom.ImporterId = "00000000";

            amitalObjExample.LogitudeCustomsFile = new LogitudeCustomsFile[] { myAmitalCustom };

            var xml = XmlGenericUtil<LOGICUSTFILE>.SerializeObject(amitalObjExample);
            xml = @"<?xml version=""1.0""?><LOGICUSTFILE xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://tempuri.org/LOGICUSTFILE"">
<LogitudeCustomsFile> <CustomFileNo>10</CustomFileNo> <Id/> <DeclarationOfficeCode>4</DeclarationOfficeCode> <FileState>P</FileState> <AgentId>513046615</AgentId> <CustomerId>10017414</CustomerId> <TransportModeId>A</TransportModeId> <CreatedByUserId>AMS</CreatedByUserId> <ReferentUserId/> <DepartmentId>ADMIN</DepartmentId> <MAWB/> <DealId/> <HAWB/> <ManifestNumber>2023</ManifestNumber> <LoadingPortCode/> <OriginCountryCode>IL</OriginCountryCode> <CargoDescription>Theo Embellished Leather and Canvas Trainer</CargoDescription> <PackageTypeCode>PP</PackageTypeCode> <PackageMeasureQualifierCode>2</PackageMeasureQualifierCode> <PackageQuantity>1</PackageQuantity> <GrossMassMeasure>0.20</GrossMassMeasure> <VendorId/> <ImporterId>511812463</ImporterId> <Tenant>6</Tenant> <GrantDate/> <ManifestDate/> <ArrivalDateTime/> <Mode>NEW</Mode> <EnglishName>NOVA MEASURING INSTRUMENT LTD.</EnglishName> <HebrewName>נובה מכשירי מדידה בעמ</HebrewName> <WarehouseId/> <UnloadportId>NLAMS</UnloadportId> <ProcedureCurrentCode>1000001</ProcedureCurrentCode> <ImporterAddress/> <CargoTypeCode>16</CargoTypeCode> <SecondCargoID/> <ThirdCargoID>FEA</ThirdCargoID> <UnloadDate/> <IsCourierDeclaration/> <CasualSupplierName/> <CasualSupplierAddress/> <CourierHawb/> <HAWBDATE/> <COUWTVAL/> <CasualImporterAddress1/> <CasualImporterAddress2/> <CasualImporterCity/> <CasualImporterZipCode/> <CasualImporterFax/> <CasualImporterEmail/> <CasualImportelTel/> <CasualImporterContact/> <CasualImporterCountry/> <IsDiamondsDeclaration/> <EstimatedTimeOfArrival/> <OrderNumber/> <WithPaper/> <FileStatus/> <NewFile>true</NewFile> <ImporterFile>38417</ImporterFile> <Team/> <FileOpenDate>16.08.23</FileOpenDate> <TruckerId/> <DistributionArea/> <FclLcl/> <ForwarderId/> <shopId/> <LastMileServiceType/> <Commodity/> <DestinationCountryCode>NL</DestinationCountryCode> <BuyerName>Hadil Qashua</BuyerName> <BuyerAddress>Tarik abd alhai - Tarik abd alhai T</BuyerAddress> <BuyerCountryCode>IL</BuyerCountryCode> <BuyerRoleCode/> <InvoiceNumber>400106</InvoiceNumber> <IncotermCode>3</IncotermCode> <ExportUnloadingPortCode>NLAMS</ExportUnloadingPortCode> <StorageSiteCode/> <MarksNumbers>Name &amp; Add</MarksNumbers> <ReferentMAWB/> <ReferentHAWB/> <Invoices> <Invoice> <InvoiceNum>400106</InvoiceNum> <InvoiceDate>03.01.2023</InvoiceDate> <InvoiceAmount>951.64</InvoiceAmount> <InvoiceCurrency>ILS</InvoiceCurrency> <InvoiceType/> <InvoiceIncoterms>3</InvoiceIncoterms> <InvoiceBuyerName>Hadil Qashua</InvoiceBuyerName> <InvoiceBuyerAddress>Tarik abd alhai - Tarik abd alhai T</InvoiceBuyerAddress> <InvoiceBuyerCountryCode>IL</InvoiceBuyerCountryCode> <InvoiceItems> <InvoiceItem> <ItemNo>897134428</ItemNo> <ItemDescription>Theo Embellished Leather and Canvas Trainer</ItemDescription> <ItemHScode>64021900</ItemHScode> <ItemQuantity>1</ItemQuantity> <ItemQuantityType/> <ItemAmount>459.0600</ItemAmount> <ItemOriginCountry>KH</ItemOriginCountry> </InvoiceItem> <InvoiceItem> <ItemNo>897134429</ItemNo> <ItemDescription>Parker Leather Loafer</ItemDescription> <ItemHScode>64029900</ItemHScode> <ItemQuantity>1</ItemQuantity> <ItemQuantityType/> <ItemAmount>492.5800</ItemAmount> <ItemOriginCountry>VN</ItemOriginCountry> </InvoiceItem> </InvoiceItems> </Invoice> </Invoices> <ForwarderFiles/> <FlightDate/> <Direction>E</Direction> </LogitudeCustomsFile>
</LOGICUSTFILE>";

            var dus = new DeclarationUpsertService();
            string MoreParams = ""; string MessageOut = "";
            //dus.CopyFromDeclarationId = CopyFromDeclarationId; //1-3033 616200697;
            dus.ProccessGenericRequest(xml, ref MoreParams,
                out MessageOut);


            var _context = CustomContext.GetContext(dus._MyDeclarationPM.Tenant);
            var myCopyDeclarationUpdateService = new DeclarationUpdateService(_context, new Dictionary<string, IContext>(), dus._MyDeclarationPM.Tenant);
            myCopyDeclarationUpdateService.CopyDeclaration(CopyFromDeclarationId, dus._MyDeclarationPM.Id, dus._MyDeclarationPM.Tenant);


            //dus._MyDeclarationPM.Id;
        }

        public void UpdateTrucker()
        {
            if (currentDeclarationCourierStatusPM == null)
            {
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(_context);
                currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(_MyDeclarationPM.Id, true, false);
            }

            if (currentDeclarationCourierStatusPM != null)
            {
                string truckerId = GetTruckerId(this._MyDeclarationPM.Tenant, _AmitalCustomsFile.TruckerId);

                if (truckerId != currentDeclarationCourierStatusPM.TruckerId || _AmitalCustomsFile.DistributionArea != currentDeclarationCourierStatusPM.DistributionArea)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(_context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    currentDeclarationCourierStatusPM.TruckerId = truckerId;
                    currentDeclarationCourierStatusPM.MAWB = _AmitalCustomsFile.MAWB;
                    currentDeclarationCourierStatusPM.DistributionArea = _AmitalCustomsFile.DistributionArea;
                    AppendLogLine("try to update trucker " + truckerId + " to declarationCourierStatus for DeclarationPM.Id: " + _MyDeclarationPM.Id);
                    try
                    {
                        declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                        AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                        return;
                    }
                    catch (Exception e)
                    {
                        AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                        return;
                    }
                }

            }

        }

        public static string GetTruckerId(int tenant, string amitalCustomsFileTruckerId)
        {
            string truckerId = null;
            if (!String.IsNullOrWhiteSpace(amitalCustomsFileTruckerId))
            {
                CardRepository cardRep = new CardRepository(tenant);
                Card card = cardRep.GetSingleCard(amitalCustomsFileTruckerId, tenant);
                if (card != null)
                {
                    truckerId = amitalCustomsFileTruckerId;
                }
                else
                {
                    card = cardRep.GetSingleCardByCode(amitalCustomsFileTruckerId, tenant, true);
                    if (card != null)
                    {
                        truckerId = card.Id;
                    }
                }
            }

            return truckerId;
        }

    }

}
