using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data;
using Logitude.Customs.BL.NotificationBL;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.BL.Models;
using System.Data;
using Logitude.Customs.Data.EntityKeys;
using System.Data.Entity.Core;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.CustomsMessaging.Common.Gen;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Microsoft.Practices.Unity;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common;
using System.IO;
using System.Xml.Serialization;
using Logitude.Customs.BL.Messaging.Customs;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.Helpers;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.BL.Utils;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.Messaging.Maman;
using Logitude.Customs.BL.Messaging.ILOVS;
using System.Diagnostics;
using Logitude.Customs.BL.CloseTables;
using System.Text.RegularExpressions;
using Logitude.Customs.BL.BL;
using System.Configuration;
using System.Globalization;
using Logitude.Customs.BL.Messaging.ILSWS;
using System.Xml;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationUpdateService
    {
        private DataProvider currentDataProvider;
        private EventTracerArgs _LastTraceEventParams;
        private CourierMasterPM _CourierMasterPM;
        public string TruckerId;
        public bool IsFromU2L;
        public string DistributionArea;
        public string ImporterCode;
        public string LastMileServiceType;
        public string MAWB;

        public bool IsFromApproveAmend;

        public bool IsProcedureCurrentCodeChanged { get; set; }

        public bool IsFromCustomsFeedback { get; set; }
        public bool ToUpdateWithPaymentDate { get; set; }
        protected override void OnCreating(DeclarationPM entityPM, EntityPM entityParentPM)
        {

            CustomsSettingQueryService settingsQuery = new CustomsSettingQueryService(entityPM.Tenant);
            if (!CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant).IsConnectedToUniFreight)
            {
                if (entityPM.IsDiamondDeclaration || entityPM.IsCourierDeclaration)
                {
                    DeclarationCounterQueryService declarationCounterQueryService = new DeclarationCounterQueryService(entityPM.Tenant);
                    var declarationCounter = declarationCounterQueryService.GetSingleByCustomFileNo(entityPM.CustomFileNo, entityPM.Tenant);
                    if (declarationCounter != null)
                        entityPM.Id = declarationCounter.DeclarationId;
                }
            }
            if (string.IsNullOrEmpty(entityPM.Id))
                entityPM.Id = IdCounter.GetNumber("Customs.Declaration", entityPM.Tenant);

            if (entityPM.CreateDateTime == null)
            {
                entityPM.CreateDateTime = DateTime.Now;
            }

            if(entityPM.TransportModeId == "L")
            {
                entityPM.TransportModeId = "I";
            }


            ICustomContext context = MainContext as CustomContext;
            if (string.IsNullOrWhiteSpace(entityPM.ImporterTypeCode)) entityPM.ImporterTypeCode = "1";
            if (string.IsNullOrWhiteSpace(entityPM.TransferImporterTypeCode)) entityPM.TransferImporterTypeCode = "1";
            if (string.IsNullOrWhiteSpace(entityPM.EntitleImporterTypeCode)) entityPM.EntitleImporterTypeCode = "1";

            //CustomsSettingQueryService customsSettingQueryservice = new CustomsSettingQueryService(context);

            ConsignmentPM consignment = null;
            ConsignmentPackagePM package = null;
            if (entityPM.Consignments.Count == 0)
            {
                consignment = new ConsignmentPM()
                {
                    Tenant = entityPM.Tenant,
                    IsLastReleaseFromWarehous = "N",
                    DeclarationId = entityPM.Id,
                    ///oncreate while Init ConsignmentNumber = CodeCounter.GetNumber("Customs.Consignment", entityPM.Tenant),
                    ChangeSetOp = ChangeSetOperation.Insert,//CargoDescription = "z",

                    //OriginCountryCode = "AD",

                };

                package = new ConsignmentPackagePM()
                {
                    DeclarationId = entityPM.Id,
                    ConsignmentNumber = consignment.ConsignmentNumber,
                    PackageMeasureQualifierCode = "2",
                    Tenant = entityPM.Tenant,
                    LineNumber = 1,
                    // PackageQuantity = 33,
                    ChangeSetOp = ChangeSetOperation.Insert,
                };

                consignment.ConsignmentPackages.Add(package);
                entityPM.Consignments.Add(consignment);
            }

            CustomsHouseTypeQueryService houseTypeQuery = new CustomsHouseTypeQueryService(context);

            var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            if (setting != null)
            {
                entityPM.AgentId = setting.CustomsAgentId.Length <= 9 ? setting.CustomsAgentId : null;
                //if (!setting.IsConnectedToUniFreight)
                if (!entityPM.IsConnectedToUnifreight && entityPM.IsAmendment != true)
                {

                    if (string.IsNullOrEmpty(entityPM.CustomFileNo))
                    {
                        entityPM.CustomFileNo = TableCounter.GetNumber(entityPM.Tenant, "DECL", "DC", null);
                    }
                    else
                    {
                        entityRepository = new DeclarationRepository(entityPM.Tenant);
                        string exists = entityRepository.GetIdByCustomFileNo(entityPM.CustomFileNo, entityPM.Tenant);
                        if (!string.IsNullOrEmpty(exists))
                        {
                            throw new Exception("Custom file no. already exists");
                        }
                    }
                }
            }
            if (!entityPM.IsCourierDeclaration)
            {
                CustomsHouseTypePM houseType = houseTypeQuery.GetHouseTypewithAdditional(entityPM.DeclarationOfficeCode, entityPM.Tenant);
                if (houseType != null && entityPM.IsAmendment != true)
                {
                    entityPM.Consignments[0].UnloadPortCode = houseType.UnloadPortCode;
                }
                if (entityPM.TransportModeId == "O")
                {
                    //<--- Yuval Chalup 14.09.2014 TASK 3075 (Override  houseType.UnloadPortCode)
                    var customsHouseTypeAdditionalRepository = new CustomsHouseTypeAdditionalRepository(entityPM.Tenant);
                    if (!String.IsNullOrWhiteSpace(entityPM.DeclarationOfficeCode))
                    {
                        var poko = customsHouseTypeAdditionalRepository.GetSingle(entityPM.Id, entityPM.Tenant);
                        if (poko != null)
                        {
                            entityPM.Consignments[0].UnloadPortCode = poko.UnloadPortCode;
                        }
                    }
                    //Yuval Chalup 14.09.2014 TASK 3075 (Override  houseType.UnloadPortCode) --->
                }
            }

            CardRepository cardRep = new CardRepository(entityPM.Tenant);
            Card card = cardRep.GetSingleCard(entityPM.CustomerId, entityPM.Tenant);
            if (card != null && entityPM.IsAmendment != true)
            {
                // moran 31.5.15 - Task 13325 -->
                //entityPM.ImporterCode = card.VatNumber;
                if (entityPM.ImporterCode == null && card.VatNumber != null)
                {
                    entityPM.ImporterCode = card.VatNumber;
                }
                // moran 31.5.15 - Task 13325 <--
            }
            if (entityPM.IsAmendment != true && entityPM.IsConvertedDeclaration == false)

                entityPM.TaxationDateTime = DateTime.Now.Date;

            if (entityPM.IsAmendment == true || entityPM.Direction == "E")
                entityPM.ExternalDeclarationNumber = entityPM.CustomFileNo + DateTime.Now.Year;

            if (entityPM.SystemConnection != "N") 
            { 
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                entityPM.CreatedByUserId = contact.Id;
			}
			if ((!entityPM.IsConnectedToUnifreight && entityPM.Direction != "E" && entityPM.IsAmendment != true && entityPM.SystemConnection != "N") ||
                (entityPM.Direction == "E" && entityPM.IsAmendment != true && string.IsNullOrEmpty(entityPM.ReferentUserId)))

            {
                entityPM.ReferentUserId = entityPM.CreatedByUserId;
            }
            if (string.IsNullOrWhiteSpace(entityPM.DeclarationTypeCode))
            {
                if (entityPM.Direction == "E") { entityPM.DeclarationTypeCode = "2"; } else { entityPM.DeclarationTypeCode = "1"; }

            }




            OnCreatingExportDeclaration(entityPM);
			if (entityPM.SystemConnection == "N")
			    entityPM.IsChanged = true;

		}
        private void UpdateShipment(DeclarationPM entityPM)
		{
            DeclarationReferantDataQueryService declarationReferantDataQueryService = new DeclarationReferantDataQueryService(entityPM.Tenant);
			DeclarationReferantDataPM declarationReferantDataPM = declarationReferantDataQueryService.GetSingle(entityPM.Id, false,false);
			ShipmentPM shipmentPM = new ShipmentPM();
			if (entityPM.TransportModeId == "A")
            {
				shipmentPM.House = entityPM.Consignments[0].ThirdCargoID;

			}
			if (entityPM.TransportModeId == "O")
			{
                shipmentPM.IskaNumber = "I_" + entityPM.Consignments[0].ManifestNumber + "_" + entityPM.Consignments[0].SecondCargoID;

			}
			if (entityPM.TransportModeId == "L")
			{
				shipmentPM.IskaNumber = entityPM.Consignments[0].ManifestNumber;

			}

			declarationReferantDataPM.ArrivalDate = entityPM.Consignments[0].UnloadDate;
            if (string.IsNullOrEmpty(entityPM.Consignments[0].ThirdCargoID))
            {
                shipmentPM.HAWBDate = entityPM.Consignments[0].ManifestDate;
			}
            else
            {
				declarationReferantDataPM.MawbDate = entityPM.Consignments[0].ManifestDate;

			}
			declarationReferantDataPM.PackageTypeCode = entityPM.Consignments[0].ConsignmentPackages.Where(x => x.PackageMeasureQualifierCode == "2").FirstOrDefault()?.PackageTypeCode;
			shipmentPM.NumberOfPackages = entityPM.Consignments[0].ConsignmentPackages.Where(x => x.PackageMeasureQualifierCode == "2").Sum(y=>y.PackageQuantity);
			shipmentPM.GrossWeight = (double)entityPM.Consignments[0].ConsignmentPackages.Where(x => x.PackageMeasureQualifierCode == "2").Sum(y => y.GrossMassMeasure);
            shipmentPM.DescriptionOfGoods = entityPM.Consignments[0].CargoDescription;
			shipmentPM.ShipmentNumber = entityPM.CustomFileNo;
			shipmentPM.CreatedByUserId = entityPM.CreatedByUserId;
			shipmentPM.DirectionId = "C";
			shipmentPM.Tenant = entityPM.Tenant;
            shipmentPM.IsCustomShipment = true;
 
			
			ICustomContext context = MainContext as CustomContext;			
			DeclarationReferantDataUpdateService declarationReferantDataUpdateService = new DeclarationReferantDataUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
			declarationReferantDataPM.ChangeSetOp = ChangeSetOperation.Update;
			declarationReferantDataUpdateService.Update(declarationReferantDataPM, true);
			var respnse = APIConnectionHelper.Instance.PostViaWebAPI<Response, object[]>("api/ShipmentHybrid/Upsert", new object[] { shipmentPM ,false});

		}
		private void OnCreatingExportDeclaration(DeclarationPM declarationPM)
        {
            if (String.IsNullOrWhiteSpace(declarationPM.Direction))
            {
                declarationPM.Direction = "I";
            }

            switch (declarationPM.Direction)
            {
                case "I":
                    {
                        declarationPM.ExcludeManifest = true;
                        return;
                    }

                case "E":
                    bool initOnNewExportDeclarationScreen = true;
                    if (initOnNewExportDeclarationScreen)
                    {
                        if (string.IsNullOrEmpty(declarationPM.TransportModeId))
                        {
                            throw new Exception("סוג הובלה - שדה חובה ");

                        }
                        if (string.IsNullOrEmpty(declarationPM.DeclarationTypeCode))
                        {
                            throw new Exception("סוג הצהרה - שדה חובה ");

                        }
                        if (string.IsNullOrEmpty(declarationPM.AgentRoleCode))
                        {
                            throw new Exception("תפקיד סוכן - שדה חובה ");

                        }

                        var allAgentRoleCodeDetails = (new DeclaraionDetails()).GetAllAgentRoleCodeDetails();
                        var allAgentRoleCodes = allAgentRoleCodeDetails.Select(r => r.Code).ToList();
                        if (!allAgentRoleCodes.Contains(declarationPM.AgentRoleCode))
                        {
                            throw new Exception("תפקיד סוכן - ערכים שגויים  ");
                        }
                    }


                    break;

                default:
                    {
                        throw new Exception("declarationPM.Direction should be E/I current {declarationPM.Direction}");
                    }

            }


        }

        protected override void UpdateCalculatedFields(DeclarationPM entityPM, EntityPM entityParentPM, Declaration entityPOCO)
        {
            DeclarationDataMapping.UpdateCourierDeclarationFields(entityPM, entityPOCO);
        }
        protected override void UpdateComposition(DeclarationPM entityPM)
        {
            ConsignmentUpdateService consignmentUpdateService = new ConsignmentUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            consignmentUpdateService.UpdateMulti(entityPM.Consignments, entityPM.DeletedConsignments, entityPM, false);

            SupplierInvoiceUpdateService supplierInvoiceUpdateService = new SupplierInvoiceUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            supplierInvoiceUpdateService.Multi_LastSIWillUpdateCCU = true;
            supplierInvoiceUpdateService.UpdateFromDeclaration = IsFromCustomsFeedback;
            supplierInvoiceUpdateService.IsProcedureCurrentCodeChanged = IsProcedureCurrentCodeChanged;
            supplierInvoiceUpdateService.UpdateMulti(entityPM.SupplierInvoices, entityPM.DeletedSupplierInvoices, entityPM, false);

            DeclarationTaxUpdateService declarationTaxUpdateService = new DeclarationTaxUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            declarationTaxUpdateService.UpdateMulti(entityPM.DeclarationTaxes, entityPM.DeletedDeclarationTaxes, entityPM, false);


            DeclarationConstraintUpdateService declarationConstraintUpdateService = new DeclarationConstraintUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            declarationConstraintUpdateService.UpdateMulti(entityPM.DeclarationConstraints, entityPM.DeletedDeclarationConstraints, entityPM, false);

            DecDangersContactUpdateService decDangersContactUpdateService = new DecDangersContactUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            decDangersContactUpdateService.UpdateMulti(entityPM.DecDangersContacts, entityPM.DeletedDecDangersContacts, entityPM, false);

            var DeclarationExportRecipientUpdateService = new DeclarationExportRecipientUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            DeclarationExportRecipientUpdateService.UpdateMulti(entityPM.DeclarationExportRecipients, entityPM.DeletedDeclarationExportRecipients, entityPM, false);


            //DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            //declarationCourierStatusUpdateService.UpdateMulti(entityPM.DeclarationCourierStatus, entityPM.DeletedDeclarationCourierStatus, entityPM, false);

            base.UpdateComposition(entityPM);
        }

        private void LogPayment(string msg, DeclarationPM declarationPM)
        {
            DateTime stopLogAt = DateTime.MinValue;
            string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20230601T000000.LogUntilDateyyyyMMdd"];
            if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
            {
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                LogitudeSettings.HandleLogMe(msg + " DeclarationUpdateService.OnUpdating = declarationPM.Id: " + declarationPM.Id + ", PaymentOrderPM.PaymentNumber: " + declarationPM.PaymentOrderNumber + declarationPM.PaymentStatusCode + ", courierPaymentStatusCode: " + declarationPM.CourierPaymentStatusCode, false, "CreateUD2LTService", stopLogAt);
            }
        }

        protected override void OnUpdating(DeclarationPM entityPM)
        {
            try
            {
                LogPayment("1", entityPM);

                //<--- Yuval Chalup 30.12.2015 TASK-18507
                if (HttpContextUtil.IsCustomDomainService())
                {
                    if (!CheckIfUpdatingAllowed(entityPM)) return;
                }
                //Yuval Chalup 30.12.2015 TASK-18507 --->
                if (!entityPM.IsCopiedFromOtherDeclaration)
                {


                    if (entityPM.SupplierInvoices.Count > 0)
                    {
                        if (entityPM.SupplierInvoices.Count == 1)
                        {
                            SupplierInvoicePM invocie = entityPM.SupplierInvoices.FirstOrDefault();
                            entityPM.PrimaryInvoiceCounterKey = invocie.InvoiceCounterKey.ToString();
                        }
                        else
                        {
                            SupplierInvoicePM primaryInvoice = entityPM.SupplierInvoices.Where(d => d.IsPrimarySupplierInvoice).FirstOrDefault();
                            if (primaryInvoice != null)
                            {
                                if (primaryInvoice.InvoiceCounterKey == 0) // moran 10.3.16 - Task 16452
                                {
                                    entityPM.PrimaryInvoiceCounterKey = "1";
                                }
                                else
                                {
                                    entityPM.PrimaryInvoiceCounterKey = primaryInvoice.InvoiceCounterKey.ToString();
                                }
                            }
                            else
                            {
                                entityPM.PrimaryInvoiceCounterKey = null;
                            }
                        }
                    }
                }

                if (entityPM.CurrentContextTag ==
                    Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateIIGExcptionConst)
                {
                    return;
                }

                //CustomsSettingQueryService settingsQuery = new CustomsSettingQueryService(entityPM.Tenant);
                //var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
                //if (setting.IsConnectedToUniFreight)
                var eventContextTagModel = entityPM.CurrentContextTag as EventContextTagModel;

                var declarationQueryService = new DeclarationQueryService(entityPM.Tenant);

                if (entityPM.IsAmendment == true && eventContextTagModel != null && eventContextTagModel.CallProccessID == EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate)
                {

                    var entityPMOrg = declarationQueryService.GetSingle(entityPM.AmendmentOriginalDeclartation, true, false);
                    entityPMOrg.CurrentContextTag = eventContextTagModel;
                    entityPMOrg.HatraDate = entityPM.HatraDate;
                    //  entityPMOrg.DeclarationNumber = entityPM.DeclarationNumber;

                    UpdateUnifreight(entityPMOrg);


                }


                // var entityAmend = declarationQueryService.GetAcceptDeclarationAmendment(entityPM.Id, entityPM.Tenant);

                if (entityPM.Direction!="E" &&!(entityPM.PaymentDate.HasValue && string.IsNullOrEmpty(entityPM.DeclarationNumber)))
                {
                    UpdateUnifreight(entityPM);
                }


                if (entityPM.IsDiamondDeclaration)
                {
                    //  DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);

                    entityPM.IsValidTicketsDiamond = declarationQueryService.IsValidTickets(entityPM);

                    entityPM.IsMissMandatoryDiamond = declarationQueryService.IsMissingMandatoryFields(entityPM);

                }

                if (entityPM.Direction == "E")
                {
                    if (!string.IsNullOrEmpty(entityPM.ProcedureCurrentCode))
                    {
                        GovernmentProcedureTypeQueryService governmentProcedureTypeQueryService = new GovernmentProcedureTypeQueryService(entityPM.Tenant);
                        GovernmentProcedureTypePM governmentProcedureType = governmentProcedureTypeQueryService.GetSingle(entityPM.ProcedureCurrentCode, false, true);
                        if (governmentProcedureType != null)
                        {
                            entityPM.ShortProcedure = governmentProcedureType.ShortProcedure;
                        }
                    }


                    foreach (var con in entityPM.Consignments)
                    {
                        if (con.ConsignmentType == "E")
                        {
                            entityPM.ShipCode = con.ShipCode;
                            break;
                        }
                    }
                }
                LogPayment("2", entityPM);

                ConsignmentPM consignment = (from a in entityPM.Consignments select a).FirstOrDefault();
                if (consignment != null) //itzik - due below crash 
                {
                    entityPM.ExportLoadingPortCode = consignment.ExportLoadingPortCode;
                    entityPM.StorageSiteCode = consignment.StorageSiteCode;
                    if (entityPM.IsCourierDeclaration)
                    {
                        entityPM.CourierHAWB = consignment.ManifestNumber;
                    }


                    entityPM.CargoDescription = consignment.CargoDescription;


                    DeliverySiteTypeQueryService deliverySiteTypeQueryService = new DeliverySiteTypeQueryService(entityPM.Tenant);
                    DeliverySiteTypePM deliverySiteType = deliverySiteTypeQueryService.GetSingle(entityPM.StorageSiteCode, false, true);
                    if (deliverySiteType != null)
                    {
                        entityPM.StorageSiteName = deliverySiteType.LocalName;
                    }

                }
                entityPM.UpdateDateTime = DateTime.Now;

                if (entityPM.MarkAsChanged && entityPM.ChangeSetOp == ChangeSetOperation.Update && !string.IsNullOrEmpty(this.EntityChangeFieldXml) && entityPM.IsCourierDeclaration)
                {
                    DateTime stopLogAt = new DateTime(2025, 06, 01);
                    LogitudeSettings.HandleLogMe(" DeclarationUpdateService.OnUpdating = EntityChangeFieldXml " + this.EntityChangeFieldXml, false, "CreateUD2LTService", stopLogAt);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(this.EntityChangeFieldXml);
                    foreach (XmlNode xmlnode in doc?.DocumentElement)
                    {
                        var changes = xmlnode?.InnerXml?.Split(new string[] { "<c" }, StringSplitOptions.None)?.Skip(1)?.ToArray();
                        foreach (var change in changes)
                        {
                            string OldValue = "", NewValue = "";
                            var oFrom = change.IndexOf("o=\"");
                            if (change.Length > oFrom + 3)
                            {
                                var oSubStrined = change.Substring(oFrom + 3);
                                var oFromDoubleQuote = oSubStrined.IndexOf("\"");
                                OldValue = oSubStrined.Substring(0, oFromDoubleQuote);
                            }
                            var nFrom = change.IndexOf("n=\"");
                            if (change.Length > nFrom + 3)
                            {
                                var nSubStrined = change.Substring(nFrom + 3);
                                var nFromDoubleQuote = nSubStrined.IndexOf("\"");
                                NewValue = nSubStrined.Substring(0, nFromDoubleQuote);
                            }
                            if (OldValue != NewValue && !change.Contains("CreatedByUserId") && !change.Contains("IsChanged") &&  (!AreEqualIgnoringSpaces(OldValue, NewValue)))
                            {
                                LogitudeSettings.HandleLogMe(" DeclarationUpdateService.OnUpdating: " + " CustomFileno : " + this.EntityPM.CustomFileNo + " = EntityChangeFieldXml " + this.EntityChangeFieldXml, false, "CreateUD2LTService", stopLogAt);
                                LogitudeSettings.HandleLogMe(" DeclarationUpdateService.OnUpdating: " + " Old : " + OldValue + " = New " + NewValue, false, "CreateUD2LTService", stopLogAt);
                                entityPM.IsChanged = true;
                                break;
                            }
                        }
                    }
                }
                else if (entityPM.MarkAsChanged && !entityPM.IsCourierDeclaration)
                {
                    if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
                    {
                        entityPM.IsChanged = true;
                    }
                }
                ClientQueryService clientQueryService = new ClientQueryService(entityPM.Tenant);
                if (!string.IsNullOrEmpty(entityPM.ImporterId))
                {
                    ClientPM client = clientQueryService.GetSingle(entityPM.ImporterId, false, true);
                    if (client != null)
                    {
                        entityPM.ImporterId = client.Id;

                    }
                    else
                    {
                        entityPM.ImporterId = null;
                    }
                }

                else if (!string.IsNullOrEmpty(entityPM.ImporterCode))
                {
                    ClientPM client = clientQueryService.GetClientByCode(entityPM.ImporterCode, entityPM.Tenant);
                    if (client != null)
                    {
                        entityPM.ImporterId = client.Id;

                    }
                    else
                    {
                        entityPM.ImporterId = null;
                    }
                }
                LogPayment("3", entityPM);
                if (!string.IsNullOrEmpty(entityPM.TransferImporterCode))
                {
                    ClientPM client = clientQueryService.GetClientByCode(entityPM.TransferImporterCode, entityPM.Tenant);
                    if (client != null)
                    {
                        entityPM.TransferImporterId = client.Id;
                        //entityPM.TransferImporterTypeCode = "1";
                        //if (!string.IsNullOrWhiteSpace(client.PassportNumber) && !string.IsNullOrWhiteSpace(client.PassportCountryCode))
                        //{
                        //    if (client.PassportTypeCode == "1")
                        //    {
                        //        entityPM.TransferImporterTypeCode = "2";
                        //    }
                        //    else
                        //    {
                        //        entityPM.TransferImporterTypeCode = "3";
                        //    }
                        //    entityPM.TransferImporterCountryCode = client.PassportCountryCode;
                        //}
                    }
                    else
                    {
                        entityPM.TransferImporterId = null;
                    }
                }

                if (!string.IsNullOrEmpty(entityPM.EntitleImporterCode))
                {
                    ClientPM client = clientQueryService.GetClientByCode(entityPM.EntitleImporterCode, entityPM.Tenant);
                    if (client != null)
                    {
                        entityPM.EntitleImporterId = client.Id;
                        //entityPM.EntitleImporterTypeCode = "1";
                        //if (!string.IsNullOrWhiteSpace(client.PassportNumber) && !string.IsNullOrWhiteSpace(client.PassportCountryCode))
                        //{
                        //    if (client.PassportTypeCode == "1")
                        //    {
                        //        entityPM.EntitleImporterTypeCode = "2";
                        //    }
                        //    else
                        //    {
                        //        entityPM.EntitleImporterTypeCode = "3";
                        //    }
                        //    entityPM.EntitleImporterCountryCode = client.PassportCountryCode;
                        //}
                    }

                    else
                    {
                        entityPM.EntitleImporterId = null;
                    }
                }

                //      DeclarationConstraintQueryService constraintQuery = new DeclarationConstraintQueryService(entityPM.Tenant);

                bool constraintExist = entityPM.DeclarationConstraints.Any();
                if (entityPM.DeclarationStatusTypeCode == "13" || entityPM.DeclarationStatusTypeCode == "5" || entityPM.DeclarationStatusTypeCode == "6" || entityPM.DeclarationStatusTypeCode == "10" || entityPM.DeclarationStatusTypeCode == "15")
                {
                    if (constraintExist)
                    {
                        entityPM.HasConstraint = true;
                    }
                    else
                    {
                        entityPM.HasConstraint = false;
                    }
                }

                else if (entityPM.DeclarationStatusTypeCode == "2" || entityPM.DeclarationStatusTypeCode == "12")
                {
                    if (constraintExist)
                    {
                        if (!String.IsNullOrWhiteSpace(entityPM.ErrosXml))// itzik + yaromn (ihab in background ) - if HUGE SIItem - entityPM.ErrosXml ==null
                        {
                            if (entityPM.ErrosXml.Contains("<ListVersionID>1</ListVersionID>"))
                            {
                                entityPM.HasConstraint = false;
                            }

                            else
                            {
                                entityPM.HasConstraint = true;
                            }
                        }
                    }
                    else
                    {
                        entityPM.HasConstraint = false;
                    }
                }

                else if (string.IsNullOrEmpty(entityPM.DeclarationStatusTypeCode))
                {
                    entityPM.HasConstraint = false;
                }
                LogPayment("4", entityPM);
                CustomsHouseTypeQueryService customHouseQuery = new CustomsHouseTypeQueryService(entityPM.Tenant);
                CustomsHouseTypePM houseType = customHouseQuery.GetHouseTypewithAdditional(entityPM.DeclarationOfficeCode, entityPM.Tenant);
                //entityPM.TransportModeId = houseType.TransportModeId;

                UpdateNotification(entityPM); // moran 2.9.14 - Task 7086
                if (entityPM.ResetDeclarationNumber)//לא לאפשר איפוס הצהרה  במקרה וישנה בקשה לש הגשת תשלום בגליון בקשות (Call# 310088) CALL#310416
                {
                    var crsQS = new CustomsRequestsSheetQueryService(entityPM.Tenant);
                    var canResetDeclaration = entityPM.Direction == "E" ? true : entityPM.PaymentDate == null;
                    var interfaceTypeCode = entityPM.Direction == "E" ? "2755E" : "2755";
                    const string analyzed = "30";
                    if (canResetDeclaration)
                    {
                        var listCRS = crsQS.GetCustomsRequestsSheetByCustomFileNumber(entityPM.CustomFileNo, entityPM.Tenant);
                        var CRS2755 = listCRS.Where(r => r.InterfaceTypeCode == interfaceTypeCode).ToList();// - מסר הגשה

                        if (CRS2755.Count > 0)
                        {
                            if (entityPM.Direction == "E")
                            {
                                throw new Exception(TranslateTextsClass.Translate("Customs.Declaration.O.CantResetDeclarationNumberSubmissionExsist", entityPM.Tenant, true));
                            }
                            else
                            {
                                foreach (var request in CRS2755)
                                {
                                    if (request.RequestStatusCode != analyzed)
                                    {
                                        throw new Exception(TranslateTextsClass.Translate("Customs.Declaration.O.CantResetDeclarationNumberDifferentFromAnalyzed", entityPM.Tenant, true));
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        throw new Exception(TranslateTextsClass.Translate("Customs.Declaration.O.CantResetDeclarationNumberPaymentDate", entityPM.Tenant, true));
                    }


                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);

                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

                    EventTracer.CreateTraceEvent(new EventTracerArgs() { EntityId = entityPM.Id, ObjectTableName = "Customs.Declaration", Tenant = entityPM.Tenant, UserId = contact.Id, EventTypeCode = "DNR", Notes = "Declaration Number Reset", });
                    ResetMetadataVER(entityPM);

                    if (entityPM.IsCourierDeclaration)
                    {
                        var context = CustomContext.GetContext(entityPM.Tenant);
                        DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                        DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);

                        DeclarationCourierStatusPM declarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPM.Id, true, false);
                        declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        declarationCourierStatusPM.CourierDeclarationStatusCode = null;
                        declarationCourierStatusPM.CourierPaymentStatusCode = null;
                        declarationCourierStatusUpdateService.Update(declarationCourierStatusPM, true);
                    }

                }
                LogPayment("4", entityPM);
                if (!string.IsNullOrEmpty(entityPM.ImporterCode) && entityPM.IsCourierDeclaration)
                {

                    string ImporterCode = entityPM.ImporterCode;


                    if (entityPM.ImporterCode.Length > 9)
                    {
                        ImporterCode = entityPM.ImporterCode.Substring(0, 9);
                    }
                    string clientId = TranslateClient(ImporterCode);
                    FeatureQuery featureQuery = new FeatureQuery();
                    var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(entityPM.Tenant), entityPM.Tenant);
                    var feature = features.Features.FirstOrDefault(x => x.Code == "AddNewClientFromManifest");


                    if (clientId == null && feature != null)
                    {
                        SendClientSearch(entityPM);

                    }
                }


                if (entityPM.VatChanged)
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);

                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);

                    DeclarationRepository rep = new DeclarationRepository(entityPM.Tenant);
                    Declaration declaration = rep.GetSingle(entityPM.Id, entityPM.Tenant);

                    if (declaration.ImporterCode != entityPM.ImporterCode)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs() { EntityId = entityPM.Id, ObjectTableName = "Customs.Declaration", Tenant = entityPM.Tenant, UserId = contact.Id, EventTypeCode = "VATC", Notes = "Old VAT: " + declaration.ImporterCode + " New VAT: " + entityPM.ImporterCode });
                    }
                }
                if (!string.IsNullOrEmpty(entityPM.CalculatedImporterName) && entityPM.CalculatedImporterName.Length > 35) entityPM.CalculatedImporterName = entityPM.CalculatedImporterName.Substring(0, 35);
                if (!string.IsNullOrEmpty(entityPM.ImporterName) && entityPM.ImporterName.Length > 35) entityPM.ImporterName = entityPM.ImporterName.Substring(0, 35);
                LogPayment("5", entityPM);

                if (!IsFromApproveAmend)
                {
                    if (entityPM.Direction == "E" && entityPM.TransportModeId == "O")
                    {

                        Dictionary<string, List<string>> dicExp = new Dictionary<string, List<string>>();
                        // check if tabs delete
                        var deleteConsignment = entityPM?.Consignments.Where(x => x.ChangeSetOp == ChangeSetOperation.Delete && x.ExportStoragesId != null).ToList();
                        if (deleteConsignment != null)
                        {
                            if (deleteConsignment.Any())

                            {

                                foreach (var item in deleteConsignment)
                                {
                                    if (!(dicExp.ContainsKey(item.ExportStoragesId)))
                                        dicExp.Add(item.ExportStoragesId, new List<string>());
                                    dicExp[item.ExportStoragesId].Add(item.ConsignmentNumber.ToString());
                                }
                            }
                        }
                        // check if comsignment update
                        DeclarationPM oldDeclaration = new DeclarationQueryService(entityPM.Tenant).GetSingle(entityPM.Id, true, false);

                        var updateConsignment = oldDeclaration?.Consignments.Where(oldCon => oldCon.ExportStoragesId != null && !entityPM.Consignments.Any(con => oldCon.DeclarationId == con.DeclarationId && oldCon.ManifestNumber == con.ManifestNumber && oldCon.SecondCargoID == con.SecondCargoID && oldCon.ThirdCargoID == con.ThirdCargoID)).ToList();
                        if (updateConsignment != null)
                        {
                            if (updateConsignment.Any())
                            {

                                foreach (var item in updateConsignment)
                                {
                                    if (!(dicExp.ContainsKey(item.ExportStoragesId)))
                                        dicExp.Add(item.ExportStoragesId, new List<string>());
                                    dicExp[item.ExportStoragesId].Add(item.ConsignmentNumber.ToString());
                                }
                            }
                        }
                        foreach (var exportStorageKey in dicExp)

                        {
                            ExportStoragePM exportStoragePM = new ExportStorageQueryService(entityPM.Tenant).GetSingle(exportStorageKey.Key, true, false);
                            if (entityPM.Id != exportStoragePM.DeclarationId) continue;

                            ConsignmentRepository consignmentRepository = new ConsignmentRepository(entityPM.Tenant);
                            List<Consignment> consignmentsByExportStorage = consignmentRepository.GetAllByExportStorageID(entityPM.Tenant, exportStorageKey.Key);


                            if (consignmentsByExportStorage.Any(cons => cons.DeclarationId != entityPM.Id ||
                             !exportStorageKey.Value.Contains(cons.ConsignmentNumber.ToString()))) continue;

                            DeleteExportStorage(entityPM.Tenant, exportStorageKey.Key);


                        }
                    }
                }
                LogPayment("6", entityPM);


                string siteCode = entityPM.Consignments?.FirstOrDefault()?.ConsignmentInternalTransitions?.FirstOrDefault()?.SiteCode;

                if (entityPM.IsCourierDeclaration && String.IsNullOrEmpty(entityPM.AutonomyRegionTypeCode) && !String.IsNullOrEmpty(siteCode))
                {
                    InternalBorderSiteTypeQueryService internalBorderSiteTypeQueryService = new InternalBorderSiteTypeQueryService(entityPM.Tenant);
                    entityPM.AutonomyRegionTypeCode = internalBorderSiteTypeQueryService.GetSingle(siteCode, false, true)?.AutonomyRegionTypeCode;
                }
				if (entityPM.SystemConnection == "N" && entityPM.ChangeSetOp == ChangeSetOperation.Update && !entityPM.IsFromUpdateShipment)
					UpdateShipment(entityPM);
			}
            finally
            {
                //if (String.IsNullOrWhiteSpace(entityPM.PrimaryInvoiceCounterKey))
                //{
                //    if (entityPM.SupplierInvoices != null)
                //    {
                //        var firstInv = entityPM.SupplierInvoices.FirstOrDefault();
                //        if (firstInv != null)
                //        {
                //            entityPM.PrimaryInvoiceCounterKey = firstInv.InvoiceCounterKey.ToString();
                //        }

                //    }
                //}
                var mySend2MasofIfNeededService = new Send2MasofIfNeededService();
                mySend2MasofIfNeededService.Send2Masof(entityPM, CourierStorageSiteChanged, GetDBEntity(entityPM.Id, entityPM.Tenant));
                LogPayment("7", entityPM);
            }
        }

        private void DeleteExportStorage(int tenant, string id)
        {
            ExportStoragePM exportStoragePM = new ExportStorageQueryService(tenant).GetSingle(id, true, false);
            exportStoragePM.DeclarationId = null;
            exportStoragePM.ChangeSetOp = ChangeSetOperation.Update;
            new ExportStorageUpdateService(MainContext as CustomContext, new Dictionary<string, IContext>(), tenant).Update(exportStoragePM, true);
        }

        private bool DeclarationIsSigned(DeclarationPM entityPM)
        {
            return true;
            // entityPM.IsSignedVersion
        }

        private bool SendDeclarationMandatoryFields(DeclarationPM declarationPM)
        {
            Boolean sendDeclarationMandatory = true;
            CustomsRequiredFieldErrors errorsForDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(declarationPM.Id, declarationPM.Tenant, declarationPM);
            if (errorsForDeclaration != null && errorsForDeclaration.RequiredFields != null && errorsForDeclaration.RequiredFields.Count() > 0)
            {
                sendDeclarationMandatory = false;
            }
            return sendDeclarationMandatory;
        }

        private string DeclarationTicketsStatus(DeclarationPM declarationPM)
        {
            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(declarationPM.Tenant);
            string ticketValidStatus = "C";

            List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationPM.Id, "", "", "", declarationPM.Tenant, "Declaration").ToList();
            if (customsDocumentsTicketPMList != null && customsDocumentsTicketPMList.Count() > 0)
            {

                var DocumentsFilingIdList = new List<string>();
                foreach (var customsDocumentsTicketPM in customsDocumentsTicketPMList)
                {
                    if (!string.IsNullOrWhiteSpace(customsDocumentsTicketPM.DocumentsFilingId))
                    {
                        DocumentsFilingIdList.Add(customsDocumentsTicketPM.DocumentsFilingId);
                    }
                }
                var customsDocumentPMList = new List<CustomsDocumentPM>();
                if (DocumentsFilingIdList != null)
                {
                    var myCustomsDocumentQueryService = new CustomsDocumentQueryService(declarationPM.Tenant);
                    customsDocumentPMList = myCustomsDocumentQueryService.GetCustomsDocumentList(DocumentsFilingIdList, declarationPM.Tenant);
                }
                if (customsDocumentPMList != null && customsDocumentPMList.Count() > 0)

                {
                    foreach (var customsDocumentPM in customsDocumentPMList)
                    {
                        if (customsDocumentPM.DocumentStatusCode != "1")
                        {
                            ticketValidStatus = "F";
                            break;
                        }
                    }
                }
            }

            if (IsDocumentMissing(declarationPM))
            {
                ticketValidStatus = "M";

            }


            return ticketValidStatus;
        }


        private bool IsDocumentMissing(DeclarationPM myDeclarationPM)
        {
            var customContext = CustomContext.GetContext(myDeclarationPM.Tenant);
            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(customContext);
            CustomDocumentTypeQueryService docTypeQuery = new CustomDocumentTypeQueryService(customContext);

            List<CustomDocumentTypePM> documentTypePMs = docTypeQuery.GetMandatoryCustomDocumentTypes(myDeclarationPM.Tenant);

            foreach (var doc in documentTypePMs)
            {
                List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(myDeclarationPM.Id, "", "", "", myDeclarationPM.Tenant, "Declaration").Where(r => r.DocumentTypeCode == doc.Code).ToList();
                if (customsDocumentsTicketPMList == null || customsDocumentsTicketPMList.Count() < 1)
                    return true;
            }

            return false;

        }

        public bool CourierStorageSiteChanged { get; set; }


        private void ResetMetadataVER(DeclarationPM entityPM)
        {

            var documentsFilingQuery = new DocumentsFilingQuery(entityPM.Tenant);
            //DocumentsFilingPM documentIn = documentsFilingQuery.GetSinglePM(_MyCustomsDocumentPM.DocumentsFilingId, requestParams.Tenant);

            var documentTypeQuery = new DocumentTypeQuery(entityPM.Tenant);
            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("DEC", entityPM.Tenant);
            if (documentType == null)
            {
                throw new Exception("DEC documentType  NOT EXIST ?!?!?!?");
            }

            var documentsFilingPMList = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityPM.Id, null, objectTableId, "I", entityPM.Tenant);
            var extDocPM = documentsFilingPMList.FirstOrDefault(r => r.DocumentTypeId == documentType.Id);
            if (extDocPM != null)
            {
                var DeclarationVersionId = "0";
                DocumentsFilingMetaDataValueQuery.UpSert_Del(extDocPM, "VER", DeclarationVersionId);
            }
        }

        public bool SuppressNewConcurrencyGUID
        {
            get
            {
                var myDeclarationDataMapping = this.Mapping as DeclarationDataMapping;
                return myDeclarationDataMapping.SuppressNewConcurrencyGUID;
            }

            set
            {
                var myDeclarationDataMapping = this.Mapping as DeclarationDataMapping;
                myDeclarationDataMapping.SuppressNewConcurrencyGUID = value;
            }
        }
        protected override void OnUpdating(DeclarationPM entityPM, Declaration entityPOCO)
        {
            //mohammad insurance if taxation changed

            ReCalculateDueTaxationDateChange(entityPM, entityPOCO);
            UpdateHataraStatusByContarization(entityPM, entityPOCO);
            if (this.SuppressNewConcurrencyGUID)
            {
                LogMessagingUtil.Instance.AppendLine("SuppressNewConcurrencyGUID");
                entityPM.NewConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            }
            if (entityPM.VersionId != entityPOCO.VersionId) //VersionId	"0.202"	string
            {
                if (entityPOCO.UserNotes == "LoadTestOnProgress")
                {
                    LogMessagingUtil.Instance.AppendLine("LoadTest !!!");
                    entityPM.UserNotes = "LoadTest";

                }

            }
            if (entityPM.ImporterName != entityPOCO.ImporterName)
            {

                {
                    UpdatePendingByKeyWordsByImporterName(entityPM, false);
                }
            }

            if (entityPM.CasualImporterTel != null)
            {
                entityPM.CasualImporterTel = Regex.Replace(entityPM.CasualImporterTel, "[^0-9]", "");///- יש להוריד את כל התווים הלא נומריים 
                entityPM.CasualImporterTel = Regex.Replace(entityPM.CasualImporterTel, @"\s+", "");///שיהייה
                if (!string.IsNullOrWhiteSpace(entityPM.CasualImporterTel) && entityPM.CasualImporterTel.StartsWith("5"))//If the number start with 5 add 0 
                {
                    entityPM.CasualImporterTel = "0" + entityPM.CasualImporterTel;//Task 139114: בדיקת חוקיות של הזנת מספר טלפון והעלאת PENDING 903- טלפון לא חוקי + טיפול נוסף
                }

            }
            if (entityPM.CasualImporterTel != entityPOCO.CasualImporterTel)
            {
                LogMessagingUtil.Instance.AppendLine("UpdateDeclarationPending903InvalidPhoneNumber");
                UpdateDeclarationPending903InvalidPhoneNumber(entityPM);
            }



            if (entityPM.IsCourierDeclaration && entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                if (CheckIfRequiredFieldForCourierHasChanged(entityPM, entityPOCO))
                {
                    entityPM.ManifestCargoStatusCode = null;
                }
            }


            if (entityPM.CourierCustomStatusCode != entityPOCO.CourierCustomStatusCode)
                LogMessagingUtil.Instance.AppendLine("CourierCustomStatusCode update to=" + entityPM.CourierCustomStatusCode + DateTime.Now.ToString("hh: mm:ss.fff tt"));


            base.OnUpdating(entityPM, entityPOCO);
        }

        private void UpdateDeclarationPending903InvalidPhoneNumber(DeclarationPM declarationPM)
        {

            ICustomContext context = MainContext as CustomContext;
            DeclarationCourierStatusQueryService myDeclarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(MainContext, new Dictionary<string, IContext>(), declarationPM.Tenant);
            DeclarationCourierStatusPM myDeclarationCourierStatusPM = myDeclarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
            var updateDeclarationPending903InvalidPhoneNumberService = new UpdateDeclarationPending903InvalidPhoneNumberService(declarationPM);
            if (myDeclarationCourierStatusPM == null)
            {
                return;// not courier !!
            }
            LogMessagingUtil.Instance.AppendLine("UpdateDeclarationPending903InvalidPhoneNumber() 903");

            updateDeclarationPending903InvalidPhoneNumberService.Calc(myDeclarationCourierStatusPM);
            if (myDeclarationCourierStatusPM != null && myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
            {

                declarationCourierStatusUpdateService.Update(myDeclarationCourierStatusPM, true);
            }
        }

        private void UpdateReferantData(DeclarationPM entityPM)
        {
            if (entityPM.Direction == "E") return;                   
                DateTime stopLogAt = new DateTime(2021, 06, 01);
                string logData = "";
                var loggedUser = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                if (entityPM.SystemConnection != "N")
			{
				logData = $"entityPM.CustomFileNo={entityPM.CustomFileNo}, User name={loggedUser}, before update1";
                LogitudeSettings.HandleLogMe("Referant update " + logData, false, "referant.NewFile", stopLogAt);
            }

			DeclarationReferantDataQueryService declarationReferantDataQueryService = new DeclarationReferantDataQueryService(entityPM.Tenant);
            DeclarationReferantDataPM referant = declarationReferantDataQueryService.GetSingle(entityPM.Id, false, false);
            if (referant != null)
            {
                if (entityPM.ProcedureCurrentCode != null)
                {
                    if (entityPM.ProcedureCurrentCode.Length > 3)
                    {
                        string ProcedureCurrentCode = entityPM.ProcedureCurrentCode.Substring(0, 3);
                        if (ProcedureCurrentCode == "407")
                        {
                            if (referant.ClassificationStatus == null)
                            {
                                referant.ClassificationStatus = "N";
                                referant.ChangeSetOp = ChangeSetOperation.Update;
                            }
                        }
                    }
                }

                var package = entityPM.Consignments.SelectMany(c => c.ConsignmentPackages.Select(p => new { c, p }))
                              .Where(g => g.p.PackageMeasureQualifierCode == "2").FirstOrDefault(x => x.p.PackageTypeCode != null)?.p;
                if (package != null)
                {
                    if (referant.PackageTypeCode != package.PackageTypeCode)
                    {
                        referant.PackageTypeCode = package.PackageTypeCode;
                        referant.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
                else
                {
                    if (referant.PackageTypeCode != null)
                    {
                        referant.PackageTypeCode = null;
                        referant.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }


                logData = $"entityPM.CustomFileNo={entityPM.CustomFileNo}, referant.NewFile={referant.NewFile}, before update2";
                LogitudeSettings.HandleLogMe("Referant update " + logData, false, "referant.NewFile", stopLogAt);
                if (referant.NewFile != false)
                {
                    if (HttpContextUtil.IsCustomDomainService())
                    {
                        logData = $"entityPM.CustomFileNo={entityPM.CustomFileNo}, before update3";
                        LogitudeSettings.HandleLogMe("Referant update " + logData, false, "referant.NewFile", stopLogAt);
                        referant.NewFile = false;
                        referant.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
                if (referant.ChangeSetOp == ChangeSetOperation.Update)
                {
                    ICustomContext context = MainContext as CustomContext;
                    DeclarationReferantDataUpdateService service = new DeclarationReferantDataUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                    service.Update(referant, true);
                    DeclarationReferantDataRepository declarationReferantDataRepository = new DeclarationReferantDataRepository(context);
                    logData = $"entityPM.CustomFileNo={entityPM.CustomFileNo}, referant.NewFile={referant.NewFile}, after update";
                    LogitudeSettings.HandleLogMe("Referant update " + logData, false, "referant.NewFile", stopLogAt);
                }
            }
        }

        public void UpdatePendingByKeyWordsByImporterName(DeclarationPM entityPM, Boolean IsAfterDeclarationCourierStatusInsert)
        {

            if (String.IsNullOrWhiteSpace(entityPM.ImporterName))
            {
                return;
            }


            ICustomContext context = MainContext as CustomContext;
            DeclarationCourierStatusQueryService myDeclarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            DeclarationCourierStatusPM declarationCourierStatusPM = myDeclarationCourierStatusQueryService.GetSingle(entityPM.Id, true, false);
            if (declarationCourierStatusPM != null)
            {
                this.updateDeclarationCourierStatusWithPending(entityPM, declarationCourierStatusPM);
            }
        }

        public void updateDeclarationCourierStatusWithPending(DeclarationPM entityPM, DeclarationCourierStatusPM declarationCourierStatusPM)
        {
            List<string> pendingReasonCodeList = new List<string>();
            DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            var pendingByKeywordQueryService = new PendingByKeywordQueryService(entityPM.Tenant);
            var courierReasonCodeList = pendingByKeywordQueryService.GetCourierPendingReasonCodeBykeyWords(
                entityPM.ImporterName,
                 /*SearchByFieldCode:*/ "2"  /*שם יבואן*/,
                entityPM.Tenant);
            foreach (var courierReasonCode in courierReasonCodeList)
            {

                if (!String.IsNullOrWhiteSpace(courierReasonCode) && !pendingReasonCodeList.Contains(courierReasonCode))
                {
                    pendingReasonCodeList.Add(courierReasonCode);
                    DeclarationPendingPM declarationPendingPM = new DeclarationPendingPM();
                    declarationPendingPM = declarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == entityPM.Id && r.CourierPendingReasonCode == courierReasonCode).FirstOrDefault();
                    if (declarationPendingPM != null)
                    {
                        if (declarationPendingPM.Status != "A")
                        {
                            declarationPendingPM.ChangeSetOp = ChangeSetOperation.Update;
                            declarationPendingPM.Status = "A";
                            if (declarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                    else
                    {
                        declarationPendingPM = new DeclarationPendingPM();
                        declarationPendingPM.ChangeSetOp = ChangeSetOperation.Insert;
                        declarationPendingPM.Status = "A";
                        declarationPendingPM.DeclarationID = entityPM.Id;
                        declarationPendingPM.Tenant = entityPM.Tenant;
                        declarationPendingPM.CourierPendingReasonCode = courierReasonCode;
                        declarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM);
                        if (declarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
            }
            if (declarationCourierStatusPM != null && declarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                declarationCourierStatusUpdateService.Update(declarationCourierStatusPM, true);
            }
        }

        public bool CheckIfUpdatingAllowed(DeclarationPM myDeclarationPM)
        {
            string text = "";
            var context = CustomContext.GetContext(myDeclarationPM.Tenant);
            CustomsRequestsSheetQueryService customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(context);
            List<CustomsRequestsSheetPM> customsRequestsSheetPMList = customsRequestsSheetQueryService.GetRequestInProgress(myDeclarationPM.Tenant, "2750", "", "", null, null, myDeclarationPM.CustomFileNo, true);
            if (customsRequestsSheetPMList != null)
            {
                if (customsRequestsSheetPMList.Count > 0)
                {
                    var RequestInProgressInterfaceTypeName = customsRequestsSheetPMList.First().InterfaceTypeName;
                    text = TranslateTextsClass.Translate("Customs.General.RequestInProgress", myDeclarationPM.Tenant, true);
                    text = String.Format(text, RequestInProgressInterfaceTypeName);

                }
            }

            //Check if Declaration was already paid, constraint in progress or Future payment was done
            var declarationValidator = new Logitude.Customs.BL.Validators.DeclarationValidator(myDeclarationPM);
            if (ToUpdateWithPaymentDate) declarationValidator.ToUpdateWithPaymentDate = true;
            declarationValidator.DeclarationViewDisplayOnlyChecks();
            if (declarationValidator.ErrorCode.Count > 0)
            {
                text = TranslateTextsClass.Translate(declarationValidator.ErrorCode[0], myDeclarationPM.Tenant, true);
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                LogMessagingUtil.Instance.AppendLine(text);
                return false;
            }
            return true;
        }
        //Yuval Chalup 30.12.2015 TASK-18507 --->

        protected override void TraceLoadTest(string LoadTestLog)
        {

            //this commented code is just for sample you can create a trace event now you only need to add the needed event types.
            bool LoadTest = false;
            if (!LoadTest)
            {
                return;
            }
            if (!LogitudeSettings.LogitudeURL.Equals(@"http://192.116.221.103/Oracle", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            if (!HttpContextUtil.IsCustomDomainService())
            {
                return;
            }
            _LastTraceEventParams.Notes = LoadTestLog;
            if (!String.IsNullOrWhiteSpace(_LastTraceEventParams.Notes))
            {
                EventTracer.CreateTraceEvent(_LastTraceEventParams);
            }



        }

        protected override void Trace(DeclarationPM entityPM, Declaration entityPOCO, string changesXml)
        {
            //this commented code is just for sample you can create a trace event now you only need to add the needed event types.

            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);

            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant); //itzik
                                                                                                       //Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);//itzik

            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
            _LastTraceEventParams = new EventTracerArgs() { EntityId = entityPM.Id, ObjectTableName = "Customs.Declaration", Tenant = entityPM.Tenant, UserId = contact.Id, EventTypeCode = "UPDT", Notes = "Update Declaration", };
            EventTracer.CreateTraceEvent(_LastTraceEventParams);

        }

        protected override void AfterUpdating(DeclarationPM entityPM, EntityPM entityParentPM)
        {
            LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating start");
            bool fromcache = true; // why i need the Name ?? 
            _AfterCommitUpdate = true;
            if (entityPM.CustomerId != null)
            {
                CardRepository rep = new CardRepository(entityPM.Tenant);
                Card customerCard = rep.GetSingleCardByIdAndTenant(entityPM.CustomerId, entityPM.Tenant, fromcache);//i leave not from cache-due 4 update 
                if (customerCard != null)
                {
                    entityPM.CustomerName = customerCard.LocalName != null ? customerCard.LocalName : customerCard.EnglishName;
                    entityPM.CustomerCode = customerCard.Code;
                }
            }
            LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step1");
            if (!string.IsNullOrEmpty(entityPM.DepartmentId))
            {
                DepartmentRepository departmentRep = new DepartmentRepository(entityPM.Tenant);
                Department department = departmentRep.GetSingleDepartmentCache(entityPM.DepartmentId, entityPM.Tenant);
                if (department != null)
                {
                    entityPM.DepartmentName = department.LocalName != null ? department.LocalName : department.EnglishName;
                }
            }
            LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step2");

            if (entityPM.DeclarationOfficeCode != null)
            {
                CustomsHouseTypeQueryService customsHouseTypeQueryService = new CustomsHouseTypeQueryService(entityPM.Tenant);
                CustomsHouseTypePM declarationOffice = customsHouseTypeQueryService.GetSingle(entityPM.DeclarationOfficeCode, false, fromcache);
                if (declarationOffice != null)
                {
                    entityPM.DeclarationOfficeName = declarationOffice.LocalName;
                }
            }
            LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step3");

            if (entityPM.AutonomyRegionTypeCode != null)
            {

                AutonomyTypeQueryService autonomyTypeQueryService = new AutonomyTypeQueryService(entityPM.Tenant);
                AutonomyTypePM autonomyType = autonomyTypeQueryService.GetSingle(entityPM.AutonomyRegionTypeCode, false, fromcache);
                if (autonomyType != null)
                {
                    entityPM.AutonomyRegionTypeName = autonomyType.LocalName;
                }

            }

            LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step4");

            if (entityPM.ImporterEntitlementTypeCode != null)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step5");
                EntitlementTypeQueryService entitlementTypeQueryService = new EntitlementTypeQueryService(entityPM.Tenant);
                EntitlementTypePM entitlementType = entitlementTypeQueryService.GetSingle(entityPM.ImporterEntitlementTypeCode, false, fromcache);
                if (entitlementType != null)
                {
                    entityPM.ImporterEntitlementTypeName = entitlementType.LocalName;
                }
            }

            if (entityPM.ImporterPassCountryCode != null)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step6");
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(entityPM.Tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(entityPM.ImporterPassCountryCode, false, fromcache);
                if (country != null)
                {
                    entityPM.ImporterPassCountryName = country.LocalName;
                }
            }

            if (entityPM.TransferImporterCountryCode != null)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step7");
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(entityPM.Tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(entityPM.TransferImporterCountryCode, false, fromcache);
                if (country != null)
                {
                    entityPM.ImporterPassCountryName = country.LocalName;
                }
            }

            if (entityPM.ProcedureCurrentCode != null)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step8");
                GovernmentProcedureTypeQueryService governmentProcedureTypeQueryService = new GovernmentProcedureTypeQueryService(entityPM.Tenant);
                GovernmentProcedureTypePM governmentProcedureType = governmentProcedureTypeQueryService.GetSingle(entityPM.ProcedureCurrentCode, false, fromcache);
                if (governmentProcedureType != null)
                {
                    entityPM.ProcedureCurrentName = governmentProcedureType.LocalName;
                }
            }

            if (entityPM.ImporterId != null)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step9");
                ClientQueryService clientQueryService = new ClientQueryService(entityPM.Tenant);
                ClientPM client = clientQueryService.GetSingle(entityPM.ImporterId, false, fromcache);
                if (client != null)
                {
                    entityPM.CalculatedImporterName = client.LocalFirstName;
                }
            }

            if (entityPM.DeclarationStatusTypeCode != null)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step10");
                DeclarationStatusTypeQueryService declarationStatusTypeQueryService = new DeclarationStatusTypeQueryService(entityPM.Tenant);
                DeclarationStatusTypePM declarationStatusType = declarationStatusTypeQueryService.GetSingle(entityPM.DeclarationStatusTypeCode, false, fromcache);
                if (declarationStatusType != null)
                {
                    entityPM.DeclarationStatusTypeName = declarationStatusType.LocalName;

                }
            }
            if (entityPM.ImporterId != null)
            {
                ClientQueryService clientQueryService = new ClientQueryService(entityPM.Tenant);
                ClientPM client = clientQueryService.GetSingle(entityPM.ImporterId, false, true);
                if (client != null)
                {

                    entityPM.CalculatedImporterName = client.FullName;
                    FacilitationTypeQueryService FacilitationTypeQueryService = new FacilitationTypeQueryService(entityPM.Tenant);
                    FacilitationTypePM FacilitationType = FacilitationTypeQueryService.GetSingle(client.FacilitationTypeCode, false, true);
                    entityPM.FacilityTypeName = FacilitationType != null ? FacilitationType.LocalName : null;
                }

                else
                {
                    entityPM.FacilityTypeName = null;
                }
            }

            if (entityPM.ImporterId != null)
            {
                ClientQueryService clientQueryService = new ClientQueryService(entityPM.Tenant);
                ClientPM client = clientQueryService.GetSingle(entityPM.ImporterId, false, true);
                if (client != null)
                {

                    entityPM.CalculatedImporterName = client.FullName;
                    FacilitationTypeQueryService FacilitationTypeQueryService = new FacilitationTypeQueryService(entityPM.Tenant);
                    FacilitationTypePM FacilitationType = FacilitationTypeQueryService.GetSingle(client.FacilitationTypeCode, false, true);
                    entityPM.FacilityTypeName = FacilitationType != null ? FacilitationType.LocalName : null;
                }

                else
                {
                    entityPM.FacilityTypeName = null;
                }
            }
            else if (entityPM.ImporterCode != null)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step11");
                ClientQueryService clientQueryService = new ClientQueryService(entityPM.Tenant);
                ClientPM client = clientQueryService.GetClientByCode(entityPM.ImporterCode, entityPM.Tenant);
                if (client != null)
                {

                    entityPM.CalculatedImporterName = client.FullName;
                    FacilitationTypeQueryService FacilitationTypeQueryService = new FacilitationTypeQueryService(entityPM.Tenant);
                    FacilitationTypePM FacilitationType = FacilitationTypeQueryService.GetSingle(client.FacilitationTypeCode, false, true);
                    entityPM.FacilityTypeName = FacilitationType != null ? FacilitationType.LocalName : null;
                }

                else
                {
                    entityPM.FacilityTypeName = null;
                }
            }
            if (!string.IsNullOrEmpty(entityPM.CalculatedImporterName) && entityPM.CalculatedImporterName.Length > 35) entityPM.CalculatedImporterName = entityPM.CalculatedImporterName.Substring(0, 35);
            if (!string.IsNullOrEmpty(entityPM.ImporterName) && entityPM.ImporterName.Length > 35) entityPM.ImporterName = entityPM.ImporterName.Substring(0, 35);
            LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating _step12");
            bool isSubmitChanges = false;
            //----- consignment 
            bool isConsignmentInsert = (from a in entityPM.Consignments
                                        where a.ChangeSetOp == ChangeSetOperation.Insert
                                        select a).Any();

            bool isConsignmentDelete = (from a in entityPM.DeletedConsignments
                                        select a).Any();

            if (isConsignmentInsert || isConsignmentDelete)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step12");
                if (!isSubmitChanges)
                {
                    SubmitChanges();
                    isSubmitChanges = true;
                }


                ICustomContext context = MainContext as CustomContext;
                ConsignmentRepository consignmentRepository = new ConsignmentRepository(context);
                List<Consignment> consignments = consignmentRepository.GetMulti(new DeclarationKeys() { Id = entityPM.Id });
                if (consignments.Count == 0)
                {
                    throw new Exception("כל פרטי המשלוח במסך הכללי נעלמו  (CALL#291407)");
                }
                bool dirty = false;

                if (entityPM.Direction != "E")
                {
                    int index = 0;
                    foreach (Consignment item in consignments)


                    {

                        index += 1;

                        if (item.SequenceNumeric == index) continue;

                        dirty = true;

                        item.SequenceNumeric = index;

                        consignmentRepository.Update(item);

                        ConsignmentPM itemPM = (from a in entityPM.Consignments

                                                where a.DeclarationId == item.DeclarationId && a.ConsignmentNumber == item.ConsignmentNumber

                                                select a).FirstOrDefault();

                        itemPM.SequenceNumeric = item.SequenceNumeric;

                    }

                }
                else
                {
                    if (entityPM.IsAmendment != true)
                    {
                        int i_index = 0, e_index = 0;
                        foreach (Consignment item in consignments)
                        {
                            if (item.ConsignmentType == "I")
                            {
                                i_index += 1;
                                if (item.SequenceNumeric == i_index) continue;
                                dirty = true;
                                item.SequenceNumeric = i_index;
                            }
                            else
                            {
                                e_index += 1;
                                if (item.SequenceNumeric == e_index) continue;
                                dirty = true;
                                item.SequenceNumeric = e_index;
                            }
                            consignmentRepository.Update(item);
                            ConsignmentPM itemPM = (from a in entityPM.Consignments
                                                    where a.DeclarationId == item.DeclarationId && a.ConsignmentNumber == item.ConsignmentNumber
                                                    select a).FirstOrDefault();
                            itemPM.SequenceNumeric = item.SequenceNumeric;

                        }


                    }
                }

                if (dirty)
                {
                    consignmentRepository.SubmitChanges();
                }
            }
            //-------------consignment packages
            foreach (ConsignmentPM consignmentPM in entityPM.Consignments.Where(d => d.ChangeSetOp == ChangeSetOperation.Update || d.ChangeSetOp == ChangeSetOperation.Insert))
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step13");
                bool isConsignmentPackageInsert = (from a in consignmentPM.ConsignmentPackages
                                                   where a.ChangeSetOp == ChangeSetOperation.Insert
                                                   select a).Any();

                bool isConsignmentPackageDelete = (from a in consignmentPM.ConsignmentPackages
                                                   select a).Any();

                if (isConsignmentPackageInsert || isConsignmentPackageDelete)
                {
                    if (!isSubmitChanges)
                    {
                        SubmitChanges();
                        isSubmitChanges = true;
                    }
                    ICustomContext context = MainContext as CustomContext;
                    ConsignmentPackageRepository consignmentPackageRepository = new ConsignmentPackageRepository(context);
                    List<ConsignmentPackage> consignmentPackages = consignmentPackageRepository.GetMulti(new ConsignmentKeys() { DeclarationId = consignmentPM.DeclarationId, ConsignmentNumber = consignmentPM.ConsignmentNumber });


                    bool dirty = false;
                    int index = 0;
                    foreach (ConsignmentPackage item in consignmentPackages)
                    {
                        index += 1;
                        if (item.SequenceNumeric == index) continue;
                        dirty = true;
                        item.SequenceNumeric = index;
                        consignmentPackageRepository.Update(item);
                        ConsignmentPackagePM itemPM = (from a in consignmentPM.ConsignmentPackages
                                                       where a.DeclarationId == item.DeclarationId && a.ConsignmentNumber == item.ConsignmentNumber && a.LineNumber == item.LineNumber
                                                       select a).FirstOrDefault();
                        itemPM.SequenceNumeric = item.SequenceNumeric;

                    }
                    if (dirty)
                    {
                        consignmentPackageRepository.SubmitChanges();
                    }
                }
            }
            if (entityPM.IsCourierDeclaration)
            {
                LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating step14");

                //if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
                {
                    LogMessagingUtil.Instance.AppendLine("declarationAfterUpdating, entityPM.IsCourierDeclaration = true");
                    var context = CustomContext.GetContext(entityPM.Tenant);
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);

                    if (IsFromU2L)
                    {
                        declarationCourierStatusUpdateService.ImporterCode = ImporterCode;
                    }

                    DeclarationCourierStatusPM newDeclarationCourierStatusPM = declarationCourierStatusUpdateService.CalculateDeclarationCourierStatus(entityPM);
                    if (IsFromU2L)
                    {
                        declarationCourierStatusUpdateService.TruckerId = TruckerId;
                        declarationCourierStatusUpdateService.DistributionArea = DistributionArea;
                        declarationCourierStatusUpdateService.LastMileServiceType = LastMileServiceType;
                        declarationCourierStatusUpdateService.MAWB = MAWB;

                        newDeclarationCourierStatusPM = declarationCourierStatusUpdateService.UpdateTrucker(newDeclarationCourierStatusPM, entityPM);
                        newDeclarationCourierStatusPM = declarationCourierStatusUpdateService.UpdateLastMileServiceType(newDeclarationCourierStatusPM, entityPM);
                    }

                    /*
                     * getSingle moved to CalculateDeclarationCourierStatus
                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                    DeclarationCourierStatusPM newDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPM.Id, false, false);
                    if (newDeclarationCourierStatusPM == null)
                    {
                        newDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                    }
                    else
                    {
                        newDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    */

                    declarationCourierStatusUpdateService.Update(newDeclarationCourierStatusPM, true);
                    /*
                    if(entityPM.CurrentContextTag != null && entityPM.CurrentContextTag.ToString() == "Logitude.Customs.BL.Messaging.U2L.CommDec.CommDecService.Upsert()")
                    {
                        if (!string.IsNullOrWhiteSpace(entityPM.ImporterCode))
                        {
                            if (newDeclarationCourierStatusPM != null)
                            {
                                Boolean toUpdate = false;
                                if (entityPM.ImporterCode.Substring(0, 1) == "5")
                                {
                                    if (newDeclarationCourierStatusPM.HighLowValue == "L" && entityPM.ProcedureCurrentCode != "4000007")
                                    {
                                        entityPM.ProcedureCurrentCode = "4000007";
                                        toUpdate = true;
                                    }
                                    else if (newDeclarationCourierStatusPM.HighLowValue == "H" && entityPM.ProcedureCurrentCode != "4000001")
                                    {
                                        entityPM.ProcedureCurrentCode = "4000001";
                                        toUpdate = true;
                                    }
                                }
                                else
                                {
                                    if (newDeclarationCourierStatusPM.HighLowValue == "L" && entityPM.ProcedureCurrentCode != "4000507")
                                    {
                                        entityPM.ProcedureCurrentCode = "4000507";
                                        toUpdate = true;
                                    }
                                    else if (newDeclarationCourierStatusPM.HighLowValue == "H" && entityPM.ProcedureCurrentCode != "4000501")
                                    {
                                        entityPM.ProcedureCurrentCode = "4000501";
                                        toUpdate = true;
                                    }
                                }
                                if(toUpdate)
                                {
                                    DeclarationRepository rep = new DeclarationRepository(entityPM.Tenant);
                                    Declaration declaration = rep.GetSingle(entityPM.Id, entityPM.Tenant);
                                    declaration.ProcedureCurrentCode = entityPM.ProcedureCurrentCode;
                                    rep.Update(declaration);
                                    rep.SubmitChanges();
                                }
                            }
                        }
                    }
                    */
                    if (newDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Insert)
                    {
                        if (entityPM.Consignments != null && entityPM.Consignments.Count() > 0)
                        {
                            ConsignmentPM consignmentPM = entityPM.Consignments.FirstOrDefault();
                            if (consignmentPM != null)
                            {
                                ConsignmentUpdateService consignmentUpdateService = new ConsignmentUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                                consignmentUpdateService.UpdatePendingByKeyWords(consignmentPM, true);
                            }
                        }
                        this.updateDeclarationCourierStatusWithPending(entityPM, newDeclarationCourierStatusPM);
                    }

                    /*
                    if(newDeclarationCourierStatusPM != null && !newDeclarationCourierStatusPM.IsClosedForFollowUp)
                    {
                        if (this._CourierMasterPM == null)
                        {
                            var myCourierDeclarationQueryService = new CourierDeclarationQueryService(context);
                            CourierDeclarationPM _CourierDeclarationPM = myCourierDeclarationQueryService.GetCourierDeclarationByDeclarationId(entityPM.Id, entityPM.Tenant);
                            if (_CourierDeclarationPM != null)
                            {
                                var myCourierMasterQueryService = new CourierMasterQueryService(context);
                                this._CourierMasterPM = myCourierMasterQueryService.GetSingle(_CourierDeclarationPM.CourierMasterId, true, false);
                            }
                        }
                        if (this._CourierMasterPM != null && !this._CourierMasterPM.IsOpen)
                        {
                            this._CourierMasterPM.IsOpen = true;
                            this._CourierMasterPM.ChangeSetOp = ChangeSetOperation.Update;
                            var myCourierMasterUpdateService = new CourierMasterUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                            myCourierMasterUpdateService.Update(this._CourierMasterPM, true);
                        }
                    }
                    */
                }

            }
            //  -------- Declaration Referant Data 
            UpdateReferantData(entityPM);
            /*
            DateTime stopLogAt = new DateTime(2020, 06, 01);
            string logData = "";
            var loggedUser = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            logData = $"entityPM.CustomFileNo={entityPM.CustomFileNo}, User name={loggedUser}, before update1";
            LogitudeSettings.HandleLogMe("Referant update " + logData, false, "referant.NewFile", stopLogAt);
            
            DeclarationReferantDataQueryService declarationReferantDataQueryService = new DeclarationReferantDataQueryService(entityPM.Tenant);
            DeclarationReferantDataPM referant = declarationReferantDataQueryService.GetSingle(entityPM.Id, false, true);
            if (referant != null)
            {
                if (entityPM.ProcedureCurrentCode != null)
                {
                    if (entityPM.ProcedureCurrentCode.Length > 3)
                    {
                        string ProcedureCurrentCode = entityPM.ProcedureCurrentCode.Substring(0, 3);
                        if (ProcedureCurrentCode == "407")
                        {
                            if (referant.ClassificationStatus == null)
                            {
                                referant.ClassificationStatus = "N";
                                referant.ChangeSetOp = ChangeSetOperation.Update;
                            }
                        }
                    }
                }
                logData = $"entityPM.CustomFileNo={entityPM.CustomFileNo}, referant.NewFile={referant.NewFile}, before update2";
                LogitudeSettings.HandleLogMe("Referant update " + logData, false, "referant.NewFile", stopLogAt);
                if (referant.NewFile != false)
                {
                    if (HttpContextUtil.IsCustomDomainService())
                    {
                        logData = $"entityPM.CustomFileNo={entityPM.CustomFileNo}, before update3";
                        LogitudeSettings.HandleLogMe("Referant update " + logData, false, "referant.NewFile", stopLogAt);
                        referant.NewFile = false;
                        referant.ChangeSetOp = ChangeSetOperation.Update;
                    }
                }
                if(referant.ChangeSetOp == ChangeSetOperation.Update)
                {
                    ICustomContext context = MainContext as CustomContext;
                    DeclarationReferantDataUpdateService service = new DeclarationReferantDataUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                    service.Update(referant, true);
                    DeclarationReferantDataRepository declarationReferantDataRepository = new DeclarationReferantDataRepository(context);
                    declarationReferantDataRepository.SubmitChanges();
                    logData = $"entityPM.CustomFileNo={entityPM.CustomFileNo}, referant.NewFile={referant.NewFile}, after update";
                    LogitudeSettings.HandleLogMe("Referant update " + logData, false, "referant.NewFile", stopLogAt);
                }
            }*/
            //var mySend2MasofIfNeededService = new Send2MasofIfNeededService();
            //mySend2MasofIfNeededService.Send2Masof(entityPM, CourierStorageSiteChanged, GetDBEntity(entityPM.Id, entityPM.Tenant));
        }


        private void UpdateNotification(DeclarationPM dirtyDeclarationPM) // moran 2.9.14 - Task 7086 -->
        {
            if (dirtyDeclarationPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                return;
            }

            string loggingUserId = AuthenticationUtil.ResolveUserId(dirtyDeclarationPM.Tenant);

            DeclarationPM dbOccDeclarationPM = GetDBEntity(dirtyDeclarationPM.Id, dirtyDeclarationPM.Tenant);

            var eventContextTagModel = dirtyDeclarationPM.CurrentContextTag as EventContextTagModel;
            if (eventContextTagModel != null)
            {
                switch (eventContextTagModel.CallProccessID)
                {
                    case EventContextTagModel.ProccessEnum.DF_NG_5018_MSG14004_ImportDeclarationCancellation:
                    case EventContextTagModel.ProccessEnum.DE_NG_5107_MSG10_AcceptanceOrRejectionMessageResponseService: // moran 2.11.14 - Task 8597
                    case EventContextTagModel.ProccessEnum.DF_NG_5117_ImportDeclerationAmendmentReplyResponseService:
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode))
                            {
                                DoUpdateNotification(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode); // moran 11.8.14 - Task 7086
                            }
                        }
                        break;
                    case EventContextTagModel.ProccessEnum.DF_NG_2470_DF_MSG16001_ReleaseGoodsMessageResponseServiceUpdate:
                        {
                            if (!string.IsNullOrWhiteSpace(eventContextTagModel.EventCode) && !dirtyDeclarationPM.IsCourierDeclaration)
                            {
                                DoUpdateNotification(dirtyDeclarationPM, loggingUserId, eventContextTagModel.EventCode);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        // moran 11.8.14 - Task 7086 -->
        private void DoUpdateNotification(DeclarationPM declarationPM, string loggingUserId, string eventCode)
        {
            string notificationDefinitionCode = "";
            string desc = "";
            string type = "";

            if (eventCode == "RSG")
            {
                notificationDefinitionCode = "2470N";
                desc = "התרה לתיק עמילות " + declarationPM.CustomFileNo;
                type = "I";
                LogMessagingUtil.Instance.AppendLine("New Release Notification");
            }
            else if (eventCode == "RSC")
            {
                notificationDefinitionCode = "2470C";
                desc = "בוטלה התרה לתיק עמילות " + declarationPM.CustomFileNo;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Cancel Release Notification");
            }
            else if (eventCode == "PRS")
            {
                notificationDefinitionCode = "2470P";
                desc = "הודעה מוקדמת לסוכן מכס " + declarationPM.CustomFileNo;
                type = "I";
                LogMessagingUtil.Instance.AppendLine("Pre Clearance Notification");
            }
            else if (eventCode == "PRA")
            {
                notificationDefinitionCode = "2470A";
                desc = "תיק מאושר להתרה לאחר הגשת טובין " + declarationPM.CustomFileNo;
                type = "I";
                LogMessagingUtil.Instance.AppendLine("Release When Arrived");
            }
            else if (eventCode == "DCA") // moran 2.11.14 - Task 8597
            {
                notificationDefinitionCode = "5107N";
                var eventContextTagModel = declarationPM.CurrentContextTag as EventContextTagModel;
                int posA = eventContextTagModel.EventRemarks.LastIndexOf("Leading file number:");
                var leadingFile = eventContextTagModel.EventRemarks.Substring(posA);
                int posB = leadingFile.IndexOf("\n");
                if (posB > 0)
                {
                    leadingFile = leadingFile.Substring(0, posB);
                }
                posA = eventContextTagModel.EventRemarks.LastIndexOf("Decision Code:");
                var decisionCode = eventContextTagModel.EventRemarks.Substring(posA);
                posB = decisionCode.IndexOf("\n");
                if (posB > 0)
                {
                    decisionCode = decisionCode.Substring(0, posB);
                }
                desc = "החלטה בגין גרעון עצמי לתיק עמילות - " + leadingFile + " - " + decisionCode;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("New Deficit Customs Answer Notification");
            }
            else if (eventCode == "DCN")
            {
                notificationDefinitionCode = "5018N";
                desc = "בוטלה הצהרת יבוא " + declarationPM.CustomFileNo;
                type = "I";
                LogMessagingUtil.Instance.AppendLine("Declaration Cancellation Notification");
            }
            else if (eventCode == "DCH")
            {
                notificationDefinitionCode = "5117N";
                desc = "בוצע תיקון הצהרה " + declarationPM.DeclarationNumber;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Declaration Changed By Customs Notification");
            }


            else if (eventCode == "DMA")
            {
                notificationDefinitionCode = "5117A";
                desc = "תיקון הצהרה אושר " + declarationPM.DeclarationNumber;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Declaration Changed By Customs Notification");
            }

            else if (eventCode == "DMP")
            {
                notificationDefinitionCode = "5117P";
                desc = "תיקון הצהרה אושר חלקית " + declarationPM.DeclarationNumber;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Declaration Changed By Customs Notification");
            }
            else if (eventCode == "DMD")
            {
                notificationDefinitionCode = "5117D";
                desc = "תיקון הצהרה נדחה " + declarationPM.DeclarationNumber;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Declaration Changed By Customs Notification");
            }
            else if (eventCode == "DMC")
            {
                notificationDefinitionCode = "5117C";
                desc = "תיקון הצהרה בוטל" + declarationPM.DeclarationNumber;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Declaration Changed By Customs Notification");
            }
            else if (eventCode == "DWR")
            {
                notificationDefinitionCode = "5117W";
                desc = "תיקון הצהרה ממתין להחלטת המכס" + declarationPM.DeclarationNumber;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Declaration Changed By Customs Notification");
            }
            else if (eventCode == "TAS")
            {
                notificationDefinitionCode = "2470S";
                desc = "אישור שטעון" + declarationPM.DeclarationNumber;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Transshipment Approved");
            }
            else if (eventCode == "TAC")
            {
                notificationDefinitionCode = "2470T";
                desc = "אישור שטעון בוטל" + declarationPM.DeclarationNumber;
                type = "A";
                LogMessagingUtil.Instance.AppendLine("Transshipment Approved Canceled");
            }
            var notificationUpdateService = new NotificationUpdateService(this.MainContext as ICustomContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), declarationPM.Tenant);    //Yuval Chalup 17.11.2014 TASK-9089
            var notificationQueryService = new NotificationQueryService(this.MainContext as ICustomContext);  //Yuval Chalup 17.11.2014 TASK-9089

            var newNotificationPM = new NotificationPM();
            newNotificationPM.ChangeSetOp = ChangeSetOperation.Insert;
            newNotificationPM.Tenant = declarationPM.Tenant;

            newNotificationPM.NotificationDefinitionCode = notificationDefinitionCode;

            newNotificationPM.EntityId = declarationPM.Id;
            newNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            newNotificationPM.Reference1Number = declarationPM.CustomFileNo;
            newNotificationPM.CreatedByRequestID = declarationPM.CustomsRequestsSheetId;
            if (string.IsNullOrWhiteSpace(newNotificationPM.CreatedByRequestID))
            {
                newNotificationPM.CreatedByRequestID = Logitude.Customs.Def.Messaging.Customs.RequestSheetContext.Current.GetContextOrDefault().CustomsRequestsSheetId;
            }
            newNotificationPM.CreateDate = DateTime.Now;
            newNotificationPM.Description = desc;

            newNotificationPM.DepartmentId = declarationPM.DepartmentId;
            newNotificationPM.DeclarationOfficeCode = declarationPM.DeclarationOfficeCode;
            newNotificationPM.DueDate = DateTime.Now;
            newNotificationPM.AssigneToNotificationTypeCode = type;
            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.CustomerId)) newNotificationPM.CustomerId = declarationPM.CustomerId; // moran 20.6.16 - Task 20789



            newNotificationPM.AssigneToId =
               NotificationBase.
               CalcAssigneToId(newNotificationPM.Tenant, declarationPM.CustomerId, declarationPM.ReferentUserId, notificationDefinitionCode, "");

            if (declarationPM != null && !string.IsNullOrWhiteSpace(declarationPM.DeclarationOfficeCode))
            {
                newNotificationPM.IsHandledByCustomOffice = true;
            }

            if (notificationDefinitionCode == "2470N")
            {
                //var physicalCheckQueryService = new PhysicalCheckQueryService(this.MainContext as ICustomContext);
                //var physicalCheckListPM = physicalCheckQueryService.GethysicalCheckByDeclarationIdOnly(declarationPM.Id, declarationPM.Tenant);
                //var physicalCheckPM = physicalCheckListPM.FirstOrDefault();
                //if (physicalCheckPM != null)
                //{
                //    var tmpNotificationPM = new NotificationPM();
                //    tmpNotificationPM.EntityId = physicalCheckPM.Id;
                //    tmpNotificationPM.ObjectTableId = ObjectTabelRepository.GetObjectTableByName("Customs.PhysicalCheck");
                //    tmpNotificationPM.Tenant = physicalCheckPM.Tenant;
                //    NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "190");
                //    tmpNotificationPM.Reference1Number = declarationPM.CustomFileNo;
                //}

                var tmpNotificationPM = new NotificationPM();
                tmpNotificationPM.EntityId = declarationPM.Id;
                tmpNotificationPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                tmpNotificationPM.Tenant = declarationPM.Tenant;

                CloseAllRelatedNotificationFor2470N(tmpNotificationPM);
            }
            else if (notificationDefinitionCode == "2470C")
            {
                NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, newNotificationPM, "2470N");
            }

            notificationUpdateService.Update(newNotificationPM, true);

            // moran 11.8.14 - Task 7086 <--
        }

        private void CloseAllRelatedNotificationFor2470N(NotificationPM tmpNotificationPM)
        {
            LogMessagingUtil.Instance.AppendLine("Close All Related Notification For 2470N");

            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "190");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "8215");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "8213N");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "8211");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "10");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "60A");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "70");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "5101D");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "5101T");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "5101C");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "5101B");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "5101P");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "5101S");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "5101U");
            NotificationBase.CloseAllRelatedNotification(this.MainContext as ICustomContext, tmpNotificationPM, "5101G");
        }

        protected override void CheckConcurrency(DeclarationPM entityPM, Declaration entityPOCO)
        {
            //if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            if (entityPM.ConcurrencyGUID != entityPOCO.ConcurrencyGUID && entityPM.NewConcurrencyGUID != entityPOCO.ConcurrencyGUID)
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant, true);
                throw new OptimisticConcurrencyException(msg);
            }

        }

        //<--- Yuval Chalup 19.11.2015 TASK-17450
        public DeclarationPM GetSertByConvertedDeclarationNumber(string declarationNumber, int tenant, bool getComposition = false)
        {
            if (String.IsNullOrWhiteSpace(declarationNumber)) return null;

            ICustomContext customContext = CustomContext.GetContext(tenant);

            //Check if the Declaration exists
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);
            DeclarationPM myDeclarationPM = declarationQueryService.GetSingleDeclarationByNumber(declarationNumber, tenant, getComposition);

            //If the Declaration exists - return it
            if (myDeclarationPM != null)
            {
                //If it is a converted
                if (declarationNumber.Substring(2, 2) == "98" || declarationNumber.Substring(2, 2) == "99")
                {
                    myDeclarationPM.IsConvertedDeclaration = true;
                }
                return myDeclarationPM;
            }

            //If the Declaration doesn't exist and it is NOT a converted declaration - return empty
            if (declarationNumber.Substring(2, 2) != "99")
            {
                return myDeclarationPM;
            }


            //Convert the Declaration number into Reshimon number
            var reshimonNumber = LuhnAlgorithm.ConvertDeclartionToReshimon(declarationNumber);
            if (string.IsNullOrWhiteSpace(reshimonNumber))
            {
                return null;
            }

            //Create a new Declaration
            myDeclarationPM = new DeclarationPM();
            myDeclarationPM.ChangeSetOp = ChangeSetOperation.Insert;
            myDeclarationPM.IsConvertedDeclaration = true;
            myDeclarationPM.DeclarationNumber = declarationNumber;
            myDeclarationPM.Tenant = tenant;
            myDeclarationPM.UserNotes = "444";

            //Look for CCUFILEM file according to the Converted Reshimon Number
            CCUFILEM myCCUFILEM = null;
            var setting = CustomsSettingQueryService.GetSettingByTenant(tenant);
            if (setting != null)
            {
                //if (setting.IsConnectedToUniFreight)
                if (!string.IsNullOrWhiteSpace(setting.UnfConnectionString))
                {
                    AmitalContext amitalContext;
                    using (amitalContext = AmitalContext.GetContext(tenant))
                    {
                        //AmitalContext.SetOracleMonitor();
                        var myCCUFILEMQueryService = new CCUFILEMQueryService(amitalContext);

                        myCCUFILEM = myCCUFILEMQueryService.GetCCUFILEMByRESHIMONNO(reshimonNumber);

                        if (!setting.IsConnectedToUniFreight)
                        {
                            myCCUFILEM.TENANT = EntityPM.Tenant;
                        }
                    }
                }
            }

            //If CCUFILEM was NOT found
            if (myCCUFILEM == null)
            {
                myDeclarationPM.CustomFileNo = "R" + reshimonNumber;
                //myDeclarationPM.CustomFileNo = reshimonNumber;
                myDeclarationPM.UserNotes = "הצהרה מוסבת - מספר רשומון " + myDeclarationPM.CustomFileNo;
            }
            else
            {
                myDeclarationPM.CustomFileNo = myCCUFILEM.CUSTOMFILENO.ToString();
                //myDeclarationPM.UserNotes = "הצהרה מוסבת - מספר תיק " + myDeclarationPM.CustomFileNo;
                myDeclarationPM.UserNotes = "הצהרה מוסבת - מספר הצהרה " + myDeclarationPM.DeclarationNumber;

                //<--- Yuval Chalup 22.12.2015 TASK-19429 // Mirit 03/02/16 Task 18509
                myDeclarationPM.DeclarationOfficeCode = myCCUFILEM.CUSTOMSBRANCH;
                myDeclarationPM.ImporterCode = myCCUFILEM.IMPORTERID;
                myDeclarationPM.ImporterId = "";
                if (!String.IsNullOrWhiteSpace(myCCUFILEM.IMPORTERID))
                {
                    var clientRepository = new ClientRepository(tenant);
                    string importerID = clientRepository.GetIdByCode(tenant, myCCUFILEM.IMPORTERID);
                    if (!String.IsNullOrWhiteSpace(importerID))
                    {
                        myDeclarationPM.ImporterId = importerID;
                    }
                }
                //Yuval Chalup 22.12.2015 TASK-19429 --->
                //Eitan H 23/5/17 29726-->
                if (!String.IsNullOrWhiteSpace(myCCUFILEM.CUSTOMERID))
                {
                    var cardRepository = new CardRepository(tenant);
                    Card myCard = cardRepository.GetSingleCardByCode(myCCUFILEM.CUSTOMERID, tenant, true);
                    if (myCard != null)
                    {
                        myDeclarationPM.CustomerId = myCard.Id;
                    }
                }
                //<--Eitan H 23/5/17 29726
                if (string.IsNullOrWhiteSpace(myDeclarationPM.Direction) || myDeclarationPM.Direction == "I") myDeclarationPM.IsConnectedToUnifreight = true;
            }
            this.Update(myDeclarationPM, true);

            //}
            myDeclarationPM = declarationQueryService.GetSingle(myDeclarationPM.Id, true, false); // Mirit 29/11/15 Task 18543

            //<--- Yuval Chalup 10.12.2015 TASK-17450
            if (myDeclarationPM != null)
            {
                //If this is a Converted Declaration
                if (myDeclarationPM.IsConvertedDeclaration)
                {
                    SendDeclarationStatusRequest(myDeclarationPM);
                }
            }
            //Yuval Chalup 10.12.2015 TASK-17450 --->

            return myDeclarationPM;
        }
        //Yuval Chalup 19.11.2015 TASK-17450 --->

        void SendDeclarationStatusRequest(DeclarationPM myDeclarationPM)
        {
            LogitudeSettings.HandleLogMe(
                "DeclarationId:" + myDeclarationPM.Id + Environment.NewLine + Environment.StackTrace.ToString()
                , false, "8250", new DateTime(2021, 1, 1));

            var mySBQMessage = new SBQMessageService();
            var newSearchDeclarationStatusRequestParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = myDeclarationPM.CustomFileNo,
                DeclarationNumber = myDeclarationPM.DeclarationNumber,
                Tenant = myDeclarationPM.Tenant,
                RequestName = "Declaration Status " + myDeclarationPM.DeclarationNumber,
                ResponseName = "Declaration Status " + myDeclarationPM.DeclarationNumber,
                RequestVIA = SendRequestVIA.WebServiceBatch,
                InterfaceTypeCode = "8250",
                LoggingEntityId = myDeclarationPM.Id,
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                LoggingUserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
            };

            try
            {
                SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.DeclarationStatusRequestParams>(newSearchDeclarationStatusRequestParams
                    , false
                    );
            }
            catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
            {
                if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                {
                    Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("8250 RequestInProgress stop create a new one !! ");
                }
                // throw;
            }
        }
        //Yuval Chalup 10.12.2015 TASK-17450 --->

        //<--- Yuval Chalup 17.12.2015 TASK-18939
        public void ResetDeclarationNumber(DeclarationPM myDeclarationPM)
        {
            //If the Declaration exists
            if (myDeclarationPM == null) return;

            //Update ExternalDeclarationNumber
            string externalDeclarationNumber = null;
            int pos = myDeclarationPM.ExternalDeclarationNumber.IndexOf("-");
            if (pos == -1)
            {
                externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber + "-1";
            }
            else
            {
                if (pos + 1 >= myDeclarationPM.ExternalDeclarationNumber.Length)
                {
                    externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber.Substring(0, myDeclarationPM.ExternalDeclarationNumber.Length - 1) + "-1";
                }
                else
                {
                    int after;
                    if (int.TryParse(myDeclarationPM.ExternalDeclarationNumber.Substring(pos + 1), out after))
                    {
                        after++;
                        externalDeclarationNumber = myDeclarationPM.ExternalDeclarationNumber.Substring(0, pos) + "-" + after.ToString();
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(externalDeclarationNumber))
            {
                myDeclarationPM.ExternalDeclarationNumber = externalDeclarationNumber;
            }

            var traceEventParams = new EventTracerArgs()
            {
                EntityId = myDeclarationPM.Id,
                ObjectTableName = "Customs.Declaration",
                Tenant = myDeclarationPM.Tenant,
                UserId = AuthenticationUtil.ResolveUserId(myDeclarationPM.Tenant),
                EventTypeCode = "DNR",
                Notes = "Reset Declaration Number." + Environment.NewLine + "Old DeclarationNumber: " + myDeclarationPM.DeclarationNumber + Environment.NewLine + "Old VersionId: " + myDeclarationPM.VersionId,
            };

            EventTracer.CreateTraceEvent(traceEventParams);

            LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: EventCode= " + "DNR" + "CustomFileNo= " + myDeclarationPM.CustomFileNo + "  ");

            myDeclarationPM.DeclarationNumber = null;
            myDeclarationPM.VersionId = null;
            myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

            return;
        }
        //Yuval Chalup 17.12.2015 TASK-18939 --->
        //Yuval Chalup 17.12.2015 TASK-18939 --->

        public bool CopyDeclaration_test(string fromDeclarationId, int tenant)
        {
            List<string> ids = new List<string>();
            ICustomContext context = MainContext as CustomContext;

            ids.Add(fromDeclarationId);

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(tenant);

            List<DeclarationPM> declarationPMs = declarationQueryService.GetDeclarationsByIds(ids, tenant);

            DeclarationPM fromDeclaration = declarationPMs.Where(d => d.Id == fromDeclarationId).FirstOrDefault();

            DeclarationPM newDeclaration = new DeclarationPM();
            newDeclaration = fromDeclaration;
            newDeclaration.ChangeSetOp = ChangeSetOperation.Insert;

            foreach (var consignment in newDeclaration.Consignments)
            {
                consignment.ChangeSetOp = ChangeSetOperation.Insert;
                foreach (var package in consignment.ConsignmentPackages)
                {
                    package.ChangeSetOp = ChangeSetOperation.Insert;

                    foreach (var consignmentPackDangers in package.ConsignmentPackDangers)
                    {
                        consignmentPackDangers.ChangeSetOp = ChangeSetOperation.Insert;
                    }


                }


            }


            foreach (var dangerContact in newDeclaration.DecDangersContacts)
            {
                dangerContact.ChangeSetOp = ChangeSetOperation.Insert;
            }

            newDeclaration.SupplierInvoices = null;


            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);


            declarationUpdateService.Update(newDeclaration, true);


            declarationPMs = declarationQueryService.GetDeclarationsByIds(ids, tenant);

            newDeclaration = declarationPMs.Where(d => d.Id == fromDeclarationId).FirstOrDefault();

            foreach (var invoice in newDeclaration.SupplierInvoices)
            {
                invoice.ChangeSetOp = ChangeSetOperation.Insert;
                foreach (var item in invoice.SupplierInvoiceItems)
                {
                    item.ChangeSetOp = ChangeSetOperation.Insert;
                    foreach (var supplierInvioceItemCertificats in item.SupplierInvioceItemCertificats)
                    {
                        supplierInvioceItemCertificats.ChangeSetOp = ChangeSetOperation.Insert;
                    }


                    foreach (var supplierInvoiceItemLevies in item.SupplierInvoiceItemLevies)
                    {
                        supplierInvoiceItemLevies.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    foreach (var supplierInvoiceItemModVehicles in item.SupplierInvoiceItemModVehicles)
                    {
                        supplierInvoiceItemModVehicles.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    foreach (var supplierInvoiceItemProcesTypes in item.SupplierInvoiceItemProcesTypes)
                    {
                        supplierInvoiceItemProcesTypes.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    foreach (var supplierInvoiceItemsConDeclars in item.SupplierInvoiceItemsConDeclars)
                    {
                        supplierInvoiceItemsConDeclars.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    foreach (var supplierInvoiceItemsDescripts in item.SupplierInvoiceItemsDescripts)
                    {
                        supplierInvoiceItemsDescripts.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    foreach (var supplierInvoiceItemsMods in item.SupplierInvoiceItemsMods)
                    {
                        supplierInvoiceItemsMods.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    foreach (var supplierInvoiceItemsProdIdents in item.SupplierInvoiceItemsProdIdents)
                    {
                        supplierInvoiceItemsProdIdents.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    foreach (var supplierInvoiceItemsSerialNums in item.SupplierInvoiceItemsSerialNums)
                    {
                        supplierInvoiceItemsSerialNums.ChangeSetOp = ChangeSetOperation.Insert;
                    }

                    foreach (var supplierInvoiceItemTaxes in item.SupplierInvoiceItemTaxes)
                    {
                        supplierInvoiceItemTaxes.ChangeSetOp = ChangeSetOperation.Insert;
                    }


                    foreach (var supplierInvoiceItemVehicles in item.SupplierInvoiceItemVehicles)
                    {
                        supplierInvoiceItemVehicles.ChangeSetOp = ChangeSetOperation.Insert;
                    }
                }
            }

            declarationUpdateService.Update(newDeclaration, true);


            return true;
        }

        public bool CopyDeclaration(string fromDeclarationId, string toDeclarationId, int tenant)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                List<string> ids = new List<string>();
                ids.Add(fromDeclarationId);
                ids.Add(toDeclarationId);
                ICustomContext context = MainContext as CustomContext;
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(tenant);
                declarationQueryService.LoadSupplierInvoicesWithItems = false;
                List<DeclarationPM> declarationPMs = declarationQueryService.GetDeclarationsByIds(ids, tenant);
                DeclarationPM fromDeclaration = declarationPMs.Where(d => d.Id == fromDeclarationId).FirstOrDefault();
                DeclarationPM toDeclaration = declarationPMs.Where(d => d.Id == toDeclarationId).FirstOrDefault();

                if (toDeclaration.Direction == "E")
                {
                    toDeclaration.DeclarationTypeCode = fromDeclaration.DeclarationTypeCode;
                }

                if (string.IsNullOrEmpty(toDeclaration.ProcedureCurrentCode))
                {
                    toDeclaration.ProcedureCurrentCode = fromDeclaration.ProcedureCurrentCode;
                }
                if (string.IsNullOrEmpty(toDeclaration.DeclarationOfficeCode))
                {
                    toDeclaration.DeclarationOfficeCode = fromDeclaration.DeclarationOfficeCode;
                }
                if (toDeclaration.TaxationDateTime == null)
                {
                    toDeclaration.TaxationDateTime = fromDeclaration.TaxationDateTime;
                }

                if (string.IsNullOrEmpty(toDeclaration.AutonomyRegionTypeCode))
                {
                    toDeclaration.AutonomyRegionTypeCode = fromDeclaration.AutonomyRegionTypeCode;
                }

                if (string.IsNullOrEmpty(toDeclaration.AgentId))
                {
                    toDeclaration.AgentId = fromDeclaration.AgentId;
                }

                if (string.IsNullOrEmpty(toDeclaration.ImporterId))
                {
                    toDeclaration.ImporterId = fromDeclaration.ImporterId;
                }

                if (string.IsNullOrEmpty(toDeclaration.ImporterCode))
                {
                    toDeclaration.ImporterCode = fromDeclaration.ImporterCode;
                }

                toDeclaration.DeclarationNumber = null;
                toDeclaration.VersionId = null;
                //   toDeclaration.ExternalDeclarationNumber = null;
                toDeclaration.DeclarationNumberandVersionId = null;
                toDeclaration.IsSignedVersion = false;

                if (string.IsNullOrEmpty(toDeclaration.DestinationCountryCode))
                {
                    toDeclaration.DestinationCountryCode = fromDeclaration.DestinationCountryCode;
                }

                if (toDeclaration.LoadingDateTime == null)
                {
                    toDeclaration.LoadingDateTime = fromDeclaration.LoadingDateTime;
                }

                if (string.IsNullOrEmpty(toDeclaration.ShipCode))
                {
                    toDeclaration.ShipCode = fromDeclaration.ShipCode;
                }

                if (toDeclaration.IsExporterConfirmation == false)
                {
                    toDeclaration.IsExporterConfirmation = fromDeclaration.IsExporterConfirmation;
                }

                if (string.IsNullOrEmpty(toDeclaration.ExportAutonomyRegionTypeCode))
                {
                    toDeclaration.ExportAutonomyRegionTypeCode = fromDeclaration.ExportAutonomyRegionTypeCode;
                }

                if (fromDeclaration.DeclarationExportRecipients != null && fromDeclaration.DeclarationExportRecipients.Count() > 0)
                {

                    foreach (DeclarationExportRecipientPM declarationExportRecipient in fromDeclaration.DeclarationExportRecipients)
                    {
                        if (toDeclaration.DeclarationExportRecipients == null)
                        {
                            toDeclaration.DeclarationExportRecipients = new List<DeclarationExportRecipientPM>();
                        }
                        DeclarationExportRecipientPM declarationExportRecipientPM = toDeclaration.DeclarationExportRecipients.Where(d => d.LineNumber == declarationExportRecipient.LineNumber).FirstOrDefault();

                        if (declarationExportRecipientPM != null)
                        {

                            if (string.IsNullOrEmpty(declarationExportRecipientPM.RecipientAddress))
                            {
                                declarationExportRecipientPM.RecipientAddress = declarationExportRecipient.RecipientAddress;

                            }

                            if (string.IsNullOrEmpty(declarationExportRecipientPM.RecipientIssueCountryCode))
                            {
                                declarationExportRecipientPM.RecipientIssueCountryCode = declarationExportRecipient.RecipientIssueCountryCode;

                            }

                            if (string.IsNullOrEmpty(declarationExportRecipientPM.RecipientName))
                            {
                                declarationExportRecipientPM.RecipientName = declarationExportRecipient.RecipientName;

                            }

                            declarationExportRecipientPM.ChangeSetOp = ChangeSetOperation.Update;


                        }
                        else
                        {
                            int? number = 0;
                            if (toDeclaration.DeclarationExportRecipients != null)
                                number = toDeclaration.DeclarationExportRecipients.Max(d => d.LineNumber);
                            number += 1;


                            declarationExportRecipientPM = new DeclarationExportRecipientPM()
                            {
                                LineNumber = number,
                                DeclarationId = toDeclaration.Id,
                                ChangeSetOp = ChangeSetOperation.Insert,
                                RecipientAddress = declarationExportRecipient.RecipientAddress,
                                RecipientIssueCountryCode = declarationExportRecipient.RecipientIssueCountryCode,
                                RecipientName = declarationExportRecipient.RecipientName,
                                Tenant = toDeclaration.Tenant,

                            };

                            toDeclaration.DeclarationExportRecipients.Add(declarationExportRecipientPM);

                        }
                    }
                }

                if (toDeclaration.Consignments.Count > 0)
                {
                    foreach (ConsignmentPM Consignment in fromDeclaration.Consignments)
                    {


                        ConsignmentPM consignmentPM = toDeclaration.Consignments.Where(d => d.SequenceNumeric == Consignment.SequenceNumeric).FirstOrDefault();

                        if (consignmentPM != null)
                        {
                            if (toDeclaration.Direction == "E")
                            {
                                consignmentPM.ConsignmentType = Consignment.ConsignmentType;
                            }



                            if (string.IsNullOrEmpty(consignmentPM.CargoTypeCode))
                            {
                                consignmentPM.CargoTypeCode = Consignment.CargoTypeCode;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.CargoTypeName))
                            {
                                consignmentPM.CargoTypeName = Consignment.CargoTypeName;

                            }


                            if (string.IsNullOrEmpty(consignmentPM.ConsignmentPackagesActiveIds))
                            {
                                consignmentPM.ConsignmentPackagesActiveIds = Consignment.ConsignmentPackagesActiveIds;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.LoadingPortCode))
                            {
                                consignmentPM.LoadingPortCode = Consignment.LoadingPortCode;

                            }

                            if (consignmentPM.ManifestDate == null)
                            {
                                consignmentPM.ManifestDate = Consignment.ManifestDate;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.ManifestNumber))
                            {
                                consignmentPM.ManifestNumber = Consignment.ManifestNumber;

                            }


                            if (string.IsNullOrEmpty(consignmentPM.OriginCountryCode))
                            {
                                consignmentPM.OriginCountryCode = Consignment.OriginCountryCode;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.OriginCountryName))
                            {
                                consignmentPM.OriginCountryName = Consignment.OriginCountryName;

                            }
                            if (string.IsNullOrEmpty(consignmentPM.ReceiverWarehouseCode))
                            {
                                consignmentPM.ReceiverWarehouseCode = Consignment.ReceiverWarehouseCode;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.ReceiverWarehouseName))
                            {
                                consignmentPM.ReceiverWarehouseName = Consignment.ReceiverWarehouseName;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.SecondCargoID))
                            {
                                consignmentPM.SecondCargoID = Consignment.SecondCargoID;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.StorageSiteCode))
                            {
                                consignmentPM.StorageSiteCode = Consignment.StorageSiteCode;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.StorageSiteName))
                            {
                                consignmentPM.StorageSiteName = Consignment.StorageSiteName;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.IsLastReleaseFromWarehous))
                            {
                                consignmentPM.IsLastReleaseFromWarehous = Consignment.IsLastReleaseFromWarehous;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.ThirdCargoID))
                            {
                                consignmentPM.ThirdCargoID = Consignment.ThirdCargoID;

                            }


                            if (consignmentPM.UnloadDate == null)
                            {
                                consignmentPM.UnloadDate = Consignment.UnloadDate;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.UnloadPortCode))
                            {
                                consignmentPM.UnloadPortCode = Consignment.UnloadPortCode;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.UnloadPortName))
                            {
                                consignmentPM.UnloadPortName = Consignment.UnloadPortName;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.CargoDescription))
                            {
                                consignmentPM.CargoDescription = Consignment.CargoDescription;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.ExportLoadingPortCode))
                            {
                                consignmentPM.ExportLoadingPortCode = Consignment.ExportLoadingPortCode;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.ExportRecieverWareHouseCode))
                            {
                                consignmentPM.ExportRecieverWareHouseCode = Consignment.ExportRecieverWareHouseCode;

                            }

                            if (string.IsNullOrEmpty(consignmentPM.ExportUnloadingPortCode))
                            {
                                consignmentPM.ExportUnloadingPortCode = Consignment.ExportUnloadingPortCode;

                            }

                            consignmentPM.IsDangerousGoods = Consignment.IsDangerousGoods;



                            if (string.IsNullOrEmpty(consignmentPM.FinalDestinationPortCode))
                            {
                                consignmentPM.FinalDestinationPortCode = Consignment.FinalDestinationPortCode;

                            }


                            consignmentPM.ChangeSetOp = ChangeSetOperation.Update;
                        }

                        else
                        {

                            int? number = toDeclaration.Consignments.Max(d => d.ConsignmentNumber);
                            number += 1;
                            consignmentPM = new ConsignmentPM()
                            {
                                ConsignmentType = Consignment.ConsignmentType,
                                CargoDescription = Consignment.CargoDescription,
                                CargoTypeCode = Consignment.CargoTypeCode,
                                CargoTypeName = Consignment.CargoTypeName,
                                ConsignmentInternalTransitionLastLineNumber = Consignment.ConsignmentInternalTransitionLastLineNumber,
                                ConsignmentNumber = number,
                                ConsignmentPackagesActiveIds = Consignment.ConsignmentPackagesActiveIds,
                                DeclarationId = toDeclaration.Id,
                                ConsignmentPackagLastLineNumber = Consignment.ConsignmentPackagLastLineNumber,
                                IsLastReleaseFromWarehous = Consignment.IsLastReleaseFromWarehous,
                                LoadingPortCode = Consignment.LoadingPortCode,
                                ManifestDate = Consignment.ManifestDate,
                                ManifestNumber = Consignment.ManifestNumber,
                                OriginCountryCode = Consignment.OriginCountryCode,
                                OriginCountryName = Consignment.OriginCountryName,
                                ReceiverWarehouseCode = Consignment.ReceiverWarehouseCode,
                                ReceiverWarehouseName = Consignment.ReceiverWarehouseName,
                                SecondCargoID = Consignment.SecondCargoID,
                                SequenceNumeric = Consignment.SequenceNumeric,
                                StorageSiteCode = Consignment.StorageSiteCode,
                                StorageSiteName = Consignment.StorageSiteName,
                                Tenant = Consignment.Tenant,
                                ThirdCargoID = Consignment.ThirdCargoID,
                                UnloadDate = Consignment.UnloadDate,
                                UnloadPortCode = Consignment.UnloadPortCode,
                                UnloadPortName = Consignment.UnloadPortName,
                                ExportLoadingPortCode = Consignment.ExportLoadingPortCode,
                                ExportRecieverWareHouseCode = Consignment.ExportRecieverWareHouseCode,
                                ExportUnloadingPortCode = Consignment.ExportUnloadingPortCode,
                                IsDangerousGoods = Consignment.IsDangerousGoods,
                                FinalDestinationPortCode = Consignment.FinalDestinationPortCode,

                                ChangeSetOp = ChangeSetOperation.Insert,
                            };

                            toDeclaration.Consignments.Add(consignmentPM);
                            DeclarationConsignmentPM declarationConsignment = new DeclarationConsignmentPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                ManifestNumber = Consignment.ManifestNumber,
                                SequenceNumeric = Consignment.SequenceNumeric,
                                ConsignmentNumber = number,
                                //ChangeSetOp = ChangeSetOperation.Insert,

                            };

                            toDeclaration.DeclarationConsignments.Add(declarationConsignment);

                        }


                        if (consignmentPM.ConsignmentPackages.Count == 0)
                        {
                            foreach (ConsignmentPackagePM package in Consignment.ConsignmentPackages)
                            {
                                ConsignmentPackagePM packagePM = new ConsignmentPackagePM()
                                {
                                    ConsignmentNumber = package.ConsignmentNumber,
                                    DeclarationId = toDeclaration.Id,
                                    GrossMassMeasure = package.GrossMassMeasure,
                                    LineNumber = package.LineNumber,
                                    MarksNumbers = package.MarksNumbers,
                                    PackageMeasureQualifierCode = package.PackageMeasureQualifierCode,
                                    PackageMeasureQualifierName = package.PackageMeasureQualifierName,
                                    PackageQuantity = package.PackageQuantity,
                                    PackageTypeCode = package.PackageTypeCode,
                                    PackageTypeName = package.PackageTypeName,
                                    Tenant = package.Tenant,
                                    SequenceNumeric = package.SequenceNumeric,
                                    ChangeSetOp = ChangeSetOperation.Insert,


                                };
                                consignmentPM.ConsignmentPackages.Add(packagePM);
                            }



                        }


                        else if (consignmentPM.ConsignmentPackages.Count > 0)
                        {
                            foreach (ConsignmentPackagePM package in Consignment.ConsignmentPackages)
                            {

                                ConsignmentPackagePM packagePM = consignmentPM.ConsignmentPackages.Where(d => d.PackageQuantity == null && d.GrossMassMeasure == null && d.SequenceNumeric == package.SequenceNumeric).FirstOrDefault();
                                if (packagePM != null)
                                {

                                    packagePM.GrossMassMeasure = package.GrossMassMeasure;
                                    packagePM.MarksNumbers = package.MarksNumbers;
                                    packagePM.PackageMeasureQualifierCode = package.PackageMeasureQualifierCode;
                                    packagePM.PackageMeasureQualifierName = package.PackageMeasureQualifierName;
                                    packagePM.PackageQuantity = package.PackageQuantity;
                                    packagePM.GrossMassMeasure = package.GrossMassMeasure;
                                    packagePM.PackageTypeCode = package.PackageTypeCode;
                                    packagePM.PackageTypeName = package.PackageTypeName;
                                    packagePM.ChangeSetOp = ChangeSetOperation.Update;

                                }

                            }


                        }




                        if (consignmentPM.ConsignmentInternalTransitions.Count == 0)
                        {
                            foreach (ConsignmentInternalTransitionPM transition in Consignment.ConsignmentInternalTransitions)
                            {
                                ConsignmentInternalTransitionPM transitionPM = new ConsignmentInternalTransitionPM()
                                {
                                    ConsignmentNumber = transition.ConsignmentNumber,
                                    DeclarationId = toDeclaration.Id,
                                    LineNumber = transition.LineNumber,
                                    Tenant = transition.Tenant,
                                    SiteCode = transition.SiteCode,

                                    ChangeSetOp = ChangeSetOperation.Insert,

                                };
                                consignmentPM.ConsignmentInternalTransitions.Add(transitionPM);
                            }



                        }


                    }
                }





                toDeclaration.PrimaryInvoiceCounterKey = fromDeclaration.PrimaryInvoiceCounterKey;
                toDeclaration.IsCopiedFromOtherDeclaration = true;
                toDeclaration.ChangeSetOp = ChangeSetOperation.Update;
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), tenant);
                declarationUpdateService.Update(toDeclaration, true);

                SupplierInvoiceItemQueryService supplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
                SupplierInvioceItemCertificatQueryService supplierInvioceItemCertificatQueryService = new SupplierInvioceItemCertificatQueryService(context);
                SupplierInvoiceFreightAmountQueryService supplierInvoiceFreightAmountQueryService = new SupplierInvoiceFreightAmountQueryService(context);
                SupplierInvoiceUCRQueryService supplierInvoiceUCRQueryService = new SupplierInvoiceUCRQueryService(context);
                SupplierInvoicePaymentQueryService supplierInvoicePaymentQueryService = new SupplierInvoicePaymentQueryService(context);
                SupplierInvoiceItemProcesTypeQueryService supplierInvoiceItemProcesTypesQueryService = new SupplierInvoiceItemProcesTypeQueryService(context);
                SupplierInvoiceItemsModQueryService supplierInvoiceItemsModsQueryService = new SupplierInvoiceItemsModQueryService(context);
                SupplierInvoiceItemsPriceQueryService supplierInvoiceItemsPricesQueryService = new SupplierInvoiceItemsPriceQueryService(context);
                SuppInvoiceItemsAbachStatementQueryService suppInvoiceItemsAbachStatementQueryService = new SuppInvoiceItemsAbachStatementQueryService(context);
                SupplierInvoiceItemsLevyQueryService supplierInvoiceItemsLevyQueryService = new SupplierInvoiceItemsLevyQueryService(context);
                SupplierInvoiceItemsConDeclarQueryService supplierInvoiceItemsConDeclarQueryService = new SupplierInvoiceItemsConDeclarQueryService(context);
                SupplierInvoiceItemsDescriptQueryService supplierInvoiceItemsDescriptQueryService = new SupplierInvoiceItemsDescriptQueryService(context);
                SupplierInvoiceItemsProdIdentQueryService supplierInvoiceItemsProdIdentQueryService = new SupplierInvoiceItemsProdIdentQueryService(context);
                SupplierInvoiceItemsSerialNumQueryService supplierInvoiceItemsSerialNumQueryService = new SupplierInvoiceItemsSerialNumQueryService(context);

                //SupplierInvoiceModificationQueryService supplierInvoiceModificationQueryService = new SupplierInvoiceModificationQueryService(context); --- mohammad bug 36761

                foreach (SupplierInvoicePM invoice in fromDeclaration.SupplierInvoices)
                {

                    SupplierInvoicePM invoicePM = new SupplierInvoicePM()
                    {
                        AccountTypeCode = invoice.AccountTypeCode,
                        ActualPayedAmount = invoice.ActualPayedAmount,
                        ActualPayedCurrencyTypeCode = invoice.ActualPayedCurrencyTypeCode,
                        DeclarationId = toDeclaration.Id,
                        ExchangeRate = invoice.ExchangeRate,
                        FreightCurrencyTypeCode = invoice.FreightCurrencyTypeCode,
                        IncotermCode = invoice.IncotermCode,
                        InsruanceCurrencyTypeCode = invoice.InsruanceCurrencyTypeCode,
                        InsruancePercentage = invoice.InsruancePercentage,
                        InsuranceAmount = invoice.InsuranceAmount,
                        InvoiceAmount = invoice.InvoiceAmount,
                        InvoiceCounterKey = invoice.InvoiceCounterKey,
                        InvoiceCurrencyTypeCode = invoice.InvoiceCurrencyTypeCode,
                        InvoiceNumber = invoice.InvoiceNumber,
                        IsPreference = invoice.IsPreference,
                        IsPrimarySupplierInvoice = invoice.IsPrimarySupplierInvoice,
                        IssueCountryCode = invoice.IssueCountryCode,
                        IssueCountryName = invoice.IssueCountryName,
                        IssueDate = invoice.IssueDate,
                        PaymentTermsCode = invoice.PaymentTermsCode,
                        PaymentTypeCode = invoice.PaymentTypeCode,
                        PreferenceDocumentTypeCode = invoice.PreferenceDocumentTypeCode,
                        PreferenceDocumentTypeName = invoice.PreferenceDocumentTypeName,
                        SequenceNumeric = invoice.SequenceNumeric,
                        InvoiceItemLastLineNumber = invoice.InvoiceItemLastLineNumber,
                        Tenant = invoice.Tenant,
                        TotalFreightInNIS = invoice.TotalFreightInNIS,
                        TotalFreightInFreightCurrency = invoice.TotalFreightInFreightCurrency,
                        VendorId = invoice.VendorId,
                        VendorName = invoice.VendorName,
                        IsAccumalated = false,
                        FullChildrenCount = invoice.FullChildrenCount,
                        FullItemsCount = invoice.FullItemsCount,
                        FullParentsCount = invoice.FullParentsCount,
                        AccumalationStateCode = invoice.AccumalationStateCode,
                        BuyerAddress = invoice.BuyerAddress,
                        BuyerCountryCode = invoice.BuyerCountryCode,
                        BuyerName = invoice.BuyerName,
                        BuyerRoleCode = invoice.BuyerRoleCode,
                        PartyRelationshipCode = invoice.PartyRelationshipCode,

                        ChangeSetOp = ChangeSetOperation.Insert,


                    };

                    //--- mohammad bug 36761 ----------------------
                    //invoice.SupplierInvoiceModifications = supplierInvoiceModificationQueryService.GetMulti(new SupplierInvoiceKeys() { DeclarationId = fromDeclarationId, InvoiceCounterKey = invoicePM.InvoiceCounterKey }, true, true); 


                    //foreach (SupplierInvoiceModificationPM modification in invoice.SupplierInvoiceModifications)
                    //{
                    //    SupplierInvoiceModificationPM modificationPM = new SupplierInvoiceModificationPM()
                    //    {
                    //        Amount = modification.Amount,
                    //        DeclarationId = toDeclaration.Id,
                    //        CurrencyTypeCode = modification.CurrencyTypeCode,
                    //        CurrencyTypeName = modification.CurrencyTypeName,
                    //        Tenant = toDeclaration.Tenant,
                    //        TypeCode = modification.TypeCode,
                    //        TypeName = modification.TypeName,
                    //        InvoiceCounterKey = modification.InvoiceCounterKey,
                    //        ModificationCounterKey = modification.ModificationCounterKey,
                    //        ChangeSetOp = ChangeSetOperation.Insert,


                    //    };
                    //    invoicePM.SupplierInvoiceModifications.Add(modificationPM);
                    //}
                    //----------------------

                    invoice.SupplierInvoiceUCRs = supplierInvoiceUCRQueryService.GetMulti(new SupplierInvoiceKeys() { DeclarationId = fromDeclarationId, InvoiceCounterKey = invoicePM.InvoiceCounterKey }, true, true);

                    foreach (SupplierInvoiceUCRPM UCR in invoice.SupplierInvoiceUCRs)
                    {
                        SupplierInvoiceUCRPM UCRPM = new SupplierInvoiceUCRPM()
                        {
                            DeclarationId = toDeclaration.Id,
                            Tenant = toDeclaration.Tenant,
                            InvoiceCounterKey = UCR.InvoiceCounterKey,
                            AgentChargeID = UCR.AgentChargeID,
                            SequenceNumeric = UCR.SequenceNumeric,
                            SupplierChargeID = UCR.SupplierChargeID,
                            ChangeSetOp = ChangeSetOperation.Insert,

                        };
                        invoicePM.SupplierInvoiceUCRs.Add(UCRPM);
                    }


                    invoice.SupplierInvoicePayments = supplierInvoicePaymentQueryService.GetMulti(new SupplierInvoiceKeys() { DeclarationId = fromDeclarationId, InvoiceCounterKey = invoicePM.InvoiceCounterKey }, true, true);

                    foreach (SupplierInvoicePaymentPM Payment in invoice.SupplierInvoicePayments)
                    {
                        SupplierInvoicePaymentPM paymentPM = new SupplierInvoicePaymentPM()
                        {
                            DeclarationId = toDeclaration.Id,
                            Tenant = toDeclaration.Tenant,
                            InvoiceCounterKey = Payment.InvoiceCounterKey,
                            SequenceNumeric = Payment.SequenceNumeric,
                            PaymentAmount = Payment.PaymentAmount,
                            PaymentTypeCode = Payment.PaymentTypeCode,
                            ChangeSetOp = ChangeSetOperation.Insert,

                        };
                        invoicePM.SupplierInvoicePayments.Add(paymentPM);
                    }



                    invoice.SupplierInvoiceFreightAmounts = supplierInvoiceFreightAmountQueryService.GetMulti(new SupplierInvoiceKeys() { DeclarationId = fromDeclarationId, InvoiceCounterKey = invoicePM.InvoiceCounterKey }, true, true);

                    foreach (SupplierInvoiceFreightAmountPM amount in invoice.SupplierInvoiceFreightAmounts)
                    {
                        SupplierInvoiceFreightAmountPM amountPM = new SupplierInvoiceFreightAmountPM()
                        {
                            Amount = amount.Amount,
                            DeclarationId = toDeclaration.Id,
                            CurrencyTypeCode = amount.CurrencyTypeCode,
                            Tenant = toDeclaration.Tenant,
                            InvoiceCounterKey = amount.InvoiceCounterKey,
                            ChangeSetOp = ChangeSetOperation.Insert,

                        };
                        invoicePM.SupplierInvoiceFreightAmounts.Add(amountPM);
                    }

                    invoice.SupplierInvoiceItems = supplierInvoiceItemQueryService.GetMulti(new SupplierInvoiceKeys() { DeclarationId = fromDeclarationId, InvoiceCounterKey = invoicePM.InvoiceCounterKey }, true, true);

                    foreach (SupplierInvoiceItemPM item in invoice.SupplierInvoiceItems.Where(d => !d.IsParent).OrderBy(d => d.SequenceNumeric))
                    {
                        SupplierInvoiceItemPM invoiceItem = new SupplierInvoiceItemPM()
                        {
                            AdditionalQuantity = item.AdditionalQuantity,
                            AdditionalQuantityType = item.AdditionalQuantityType,
                            AdditionalQuantityTypeName = item.AdditionalQuantityTypeName,
                            CatalogNumber = item.CatalogNumber,
                            //  CertificatesStatusCode = item.CertificatesStatusCode,
                            ClassificationCode = item.ClassificationCode,
                            CustomsBookTypeCode = item.CustomsBookTypeCode,
                            DangerousClassificationCode = item.DangerousClassificationCode,
                            DangerousPackingGroupTypeCode = item.DangerousPackingGroupTypeCode,
                            DeclarationId = toDeclaration.Id,
                            InvoiceQuantity = item.InvoiceQuantity,
                            InvoiceQuantityType = item.InvoiceQuantityType,
                            InvoiceQuantityTypeName = item.InvoiceQuantityTypeName,
                            IsItemChanged = item.IsItemChanged,
                            ItemCode = item.ItemCode,
                            ItemDescription = item.ItemDescription,
                            ItemPrice = item.ItemPrice,
                            ItemPriceCurrencyCode = item.ItemPriceCurrencyCode,
                            ManufactureIdentifier = item.ManufactureIdentifier,
                            NonCustomsItemPrice = item.NonCustomsItemPrice,
                            NonCustomsItemPriceCurCode = item.NonCustomsItemPriceCurCode,
                            OptionalTamaPercentage = item.OptionalTamaPercentage,
                            OriginCountryCode = item.OriginCountryCode,
                            OriginCountryName = item.OriginCountryName,
                            PreferenceDocumentNumber = toDeclaration.Direction == "E" ? "" : item.PreferenceDocumentNumber,
                            SalesTaxExemptionTypeCode = item.SalesTaxExemptionTypeCode,
                            StatisticQuantity = item.StatisticQuantity,
                            StatisticQuantityType = item.StatisticQuantityType,
                            StatisticQuantityTypeName = item.StatisticQuantityTypeName,
                            TaxExemptCode = item.TaxExemptCode,
                            TaxExemptName = item.TaxExemptName,
                            Tenant = toDeclaration.Tenant,
                            TradeAgreementCode = item.TradeAgreementCode,
                            TradeAgreementName = item.TradeAgreementName,
                            WholeSaleItemPrice = item.WholeSaleItemPrice,
                            WholeSaleItemPriceCurrencyCode = item.WholeSaleItemPriceCurrencyCode,
                            CounterKey = item.CounterKey,
                            LineNumber = item.LineNumber,
                            SequenceNumeric = item.SequenceNumeric,
                            IsParent = item.IsParent,
                            ParentLineNumber = null,
                            NotForAccumaltion = false,
                            ItemHash = null,
                            DeferredCustomsTax = item.DeferredCustomsTax,
                            DeferredPurchaseTax = item.DeferredPurchaseTax,
                            ClassificationTypeCode = item.ClassificationTypeCode,
                            ClaimReasonCode = item.ClaimReasonCode,
                            TransactionNatureCode = item.TransactionNatureCode,
                            ItemAdditionalStatus = item.ItemAdditionalStatus,

                            ChangeSetOp = ChangeSetOperation.Insert,


                        };



                        item.SupplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypesQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SupplierInvoiceItemProcesTypePM ItemProcesTypes in item.SupplierInvoiceItemProcesTypes)
                        {
                            SupplierInvoiceItemProcesTypePM ItemProcesTypePM = new SupplierInvoiceItemProcesTypePM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemProcesTypes.InvoiceCounterKey,
                                InvoiceItemLineNumber = ItemProcesTypes.InvoiceItemLineNumber,
                                LineNumber = ItemProcesTypes.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                ProcessTypeCode = ItemProcesTypes.ProcessTypeCode,
                                ProcessTypeName = ItemProcesTypes.ProcessTypeName,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemProcesTypes.Add(ItemProcesTypePM);
                        }
                        //SupplierInvoiceItemsMods
                        item.SupplierInvoiceItemsMods = supplierInvoiceItemsModsQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SupplierInvoiceItemsModPM ItemsMods in item.SupplierInvoiceItemsMods)
                        {
                            SupplierInvoiceItemsModPM ItemsModsPM = new SupplierInvoiceItemsModPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemsMods.InvoiceCounterKey,
                                TypeCode = ItemsMods.TypeCode,
                                LineNumber = ItemsMods.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                CurrencyTypeCode = ItemsMods.CurrencyTypeCode,
                                Amount = ItemsMods.Amount,
                                TypeName = ItemsMods.TypeName,
                                CurrencyTypeName = ItemsMods.CurrencyTypeName,
                                ModificationCounterKey = ItemsMods.ModificationCounterKey,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemsMods.Add(ItemsModsPM);
                        }
                        //SupplierInvoiceItemsMods
                        item.SupplierInvoiceItemsMods = supplierInvoiceItemsModsQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SupplierInvoiceItemsModPM ItemsMods in item.SupplierInvoiceItemsMods)
                        {
                            SupplierInvoiceItemsModPM ItemsModsPM = new SupplierInvoiceItemsModPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemsMods.InvoiceCounterKey,
                                TypeCode = ItemsMods.TypeCode,
                                LineNumber = ItemsMods.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                CurrencyTypeCode = ItemsMods.CurrencyTypeCode,
                                Amount = ItemsMods.Amount,
                                TypeName = ItemsMods.TypeName,
                                CurrencyTypeName = ItemsMods.CurrencyTypeName,
                                ModificationCounterKey = ItemsMods.ModificationCounterKey,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemsMods.Add(ItemsModsPM);
                        }

                        try
                        {
                            item.SupplierInvoiceItemsPrices = supplierInvoiceItemsPricesQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);
                        }
                        catch (Exception ex)
                        {
                            Exception message = ex;

                        }
                        //SupplierInvoiceItemsPrices

                        foreach (SupplierInvoiceItemsPricePM ItemsPrices in item.SupplierInvoiceItemsPrices)
                        {
                            SupplierInvoiceItemsPricePM ItemsPricesPM = new SupplierInvoiceItemsPricePM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemsPrices.InvoiceCounterKey,
                                InvoiceItemLineNumber = ItemsPrices.InvoiceItemLineNumber,
                                LineNumber = ItemsPrices.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                AdditionalPriceTypeCode = ItemsPrices.AdditionalPriceTypeCode,
                                AdditionalPriceTypeName = ItemsPrices.AdditionalPriceTypeName,
                                AdditionalPrice = ItemsPrices.AdditionalPrice,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemsPrices.Add(ItemsPricesPM);
                        }

                        //SuppInvoiceItemsAbachStatements
                        item.SuppInvoiceItemsAbachStatements = suppInvoiceItemsAbachStatementQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SuppInvoiceItemsAbachStatementPM ItemsAbachStatements in item.SuppInvoiceItemsAbachStatements)
                        {
                            SuppInvoiceItemsAbachStatementPM ItemsAbachStatementsPM = new SuppInvoiceItemsAbachStatementPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemsAbachStatements.InvoiceCounterKey,
                                InvoiceItemLineNumber = ItemsAbachStatements.InvoiceItemLineNumber,
                                SequenceNumeric = ItemsAbachStatements.SequenceNumeric,
                                Tenant = toDeclaration.Tenant,
                                StatementTypeCode = ItemsAbachStatements.StatementTypeCode,
                                IsStatementInd = ItemsAbachStatements.IsStatementInd,
                                StatementTypeName = ItemsAbachStatements.StatementTypeName,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SuppInvoiceItemsAbachStatements.Add(ItemsAbachStatementsPM);
                        }

                        //SupplierInvoiceItemLevies
                        item.SupplierInvoiceItemLevies = supplierInvoiceItemsLevyQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SupplierInvoiceItemsLevyPM ItemLevy in item.SupplierInvoiceItemLevies)
                        {
                            SupplierInvoiceItemsLevyPM ItemLevyPM = new SupplierInvoiceItemsLevyPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemLevy.InvoiceCounterKey,
                                InvoiceItemLineNumber = ItemLevy.InvoiceItemLineNumber,
                                LineNumber = ItemLevy.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                TradeLevyExamptCode = ItemLevy.TradeLevyExamptCode,
                                TradeLevyNumber = ItemLevy.TradeLevyNumber,
                                TradeLevyExamptName = ItemLevy.TradeLevyExamptName,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemLevies.Add(ItemLevyPM);
                        }
                        //SupplierInvoiceItemsConDeclars

                        item.SupplierInvoiceItemsConDeclars = supplierInvoiceItemsConDeclarQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SupplierInvoiceItemsConDeclarPM ItemsConDeclar in item.SupplierInvoiceItemsConDeclars)
                        {
                            SupplierInvoiceItemsConDeclarPM ItemsConDeclarPM = new SupplierInvoiceItemsConDeclarPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemsConDeclar.InvoiceCounterKey,
                                InvoiceItemLineNumber = ItemsConDeclar.InvoiceItemLineNumber,
                                LineNumber = ItemsConDeclar.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                DeclarationNumber = ItemsConDeclar.DeclarationNumber,
                                ItemSequence = ItemsConDeclar.ItemSequence,
                                DeclarationTypeCode = ItemsConDeclar.DeclarationTypeCode,
                                InvoiceNumber = ItemsConDeclar.InvoiceNumber,
                                Quantity = ItemsConDeclar.Quantity,
                                DeclarationTypeName = ItemsConDeclar.DeclarationTypeName,
                                QuantityTypeCode = ItemsConDeclar.QuantityTypeCode,
                                QuantityTypeName = ItemsConDeclar.QuantityTypeName,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemsConDeclars.Add(ItemsConDeclarPM);
                        }
                        //SupplierInvoiceItemsDescripts

                        item.SupplierInvoiceItemsDescripts = supplierInvoiceItemsDescriptQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SupplierInvoiceItemsDescriptPM ItemsDescript in item.SupplierInvoiceItemsDescripts)
                        {
                            SupplierInvoiceItemsDescriptPM ItemsDescriptPM = new SupplierInvoiceItemsDescriptPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemsDescript.InvoiceCounterKey,
                                InvoiceItemLineNumber = ItemsDescript.InvoiceItemLineNumber,
                                LineNumber = ItemsDescript.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                TypeCode = ItemsDescript.TypeCode,
                                Description = ItemsDescript.Description,
                                TypeName = ItemsDescript.TypeName,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemsDescripts.Add(ItemsDescriptPM);
                        }
                        //SupplierInvoiceItemsProdIdents
                        item.SupplierInvoiceItemsProdIdents = supplierInvoiceItemsProdIdentQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SupplierInvoiceItemsProdIdentPM ItemsProdIdent in item.SupplierInvoiceItemsProdIdents)
                        {
                            SupplierInvoiceItemsProdIdentPM ItemsProdIdentPM = new SupplierInvoiceItemsProdIdentPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemsProdIdent.InvoiceCounterKey,
                                InvoiceItemLineNumber = ItemsProdIdent.InvoiceItemLineNumber,
                                LineNumber = ItemsProdIdent.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                TypeCode = ItemsProdIdent.TypeCode,
                                Identification = ItemsProdIdent.Identification,
                                TypeName = ItemsProdIdent.TypeName,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemsProdIdents.Add(ItemsProdIdentPM);
                        }
                        //SupplierInvoiceItemsSerialNums
                        item.SupplierInvoiceItemsSerialNums = supplierInvoiceItemsSerialNumQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoiceItem.CounterKey, LineNumber = invoiceItem.LineNumber }, true, true);

                        foreach (SupplierInvoiceItemsSerialNumPM ItemsSerialNum in item.SupplierInvoiceItemsSerialNums)
                        {
                            SupplierInvoiceItemsSerialNumPM ItemsSerialNumPM = new SupplierInvoiceItemsSerialNumPM()
                            {
                                DeclarationId = toDeclaration.Id,
                                InvoiceCounterKey = ItemsSerialNum.InvoiceCounterKey,
                                InvoiceItemLineNumber = ItemsSerialNum.InvoiceItemLineNumber,
                                LineNumber = ItemsSerialNum.LineNumber,
                                Tenant = toDeclaration.Tenant,
                                TypeCode = ItemsSerialNum.TypeCode,
                                SerialNumber = ItemsSerialNum.SerialNumber,
                                TypeName = ItemsSerialNum.TypeName,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };
                            invoiceItem.SupplierInvoiceItemsSerialNums.Add(ItemsSerialNumPM);
                        }

                        //


                        //item.SupplierInvioceItemCertificats = supplierInvioceItemCertificatQueryService.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = fromDeclarationId, CounterKey = invoicePM.InvoiceCounterKey, LineNumber = item.LineNumber }, true, true); 

                        //foreach (SupplierInvioceItemCertificatPM certificate in item.SupplierInvioceItemCertificats)
                        //{
                        //    SupplierInvioceItemCertificatPM certificatePM = new SupplierInvioceItemCertificatPM()
                        //    {
                        //        DeclarationId = toDeclaration.Id,
                        //        InvoiceCounterKey = certificate.InvoiceCounterKey,
                        //        LineNumber = certificate.LineNumber,
                        //        Tenant = certificate.Tenant,
                        //        ItemCertificateCounterKey = certificate.ItemCertificateCounterKey,
                        //        CertificateNumber = certificate.CertificateNumber,
                        //        AttachmentTypeCode = certificate.AttachmentTypeCode,
                        //        AttachmentTypeName = certificate.AttachmentTypeName,
                        //        CustomsAttachmentID = certificate.CustomsAttachmentID,
                        //        CertificateExemptionTypeCode = certificate.CertificateExemptionTypeCode,
                        //        CertificateExemptionTypeName = certificate.CertificateExemptionTypeName,
                        //        ReqConfirmationTypeCode = certificate.ReqConfirmationTypeCode,
                        //        ReqConfirmationTypeName = certificate.ReqConfirmationTypeName,
                        //        ResConfirmationTypeCode = certificate.ResConfirmationTypeCode,
                        //        ResConfirmationTypeName = certificate.ResConfirmationTypeName,
                        //        ChangeSetOp = ChangeSetOperation.Insert,


                        //    };

                        //    invoiceItem.SupplierInvioceItemCertificats.Add(certificatePM);
                        //}


                        //foreach (SupplierInvoiceItemsConDeclarPM connectedDeclaration in item.SupplierInvoiceItemsConDeclars)
                        //{
                        //    SupplierInvoiceItemsConDeclarPM connectedDeclarationPM = new SupplierInvoiceItemsConDeclarPM()
                        //    {
                        //        DeclarationId = toDeclaration.Id,
                        //        DeclarationNumber = toDeclaration.DeclarationNumber,
                        //        DeclarationTypeCode = connectedDeclaration.DeclarationTypeCode,
                        //        DeclarationTypeName = connectedDeclaration.DeclarationTypeName,
                        //        InvoiceNumber = connectedDeclaration.InvoiceNumber,
                        //        Quantity = connectedDeclaration.Quantity,
                        //        Tenant = toDeclaration.Tenant,
                        //        InvoiceCounterKey = connectedDeclaration.InvoiceCounterKey,
                        //        InvoiceItemLineNumber = connectedDeclaration.InvoiceItemLineNumber,
                        //        ItemSequence = connectedDeclaration.ItemSequence,
                        //        LineNumber = connectedDeclaration.LineNumber
                        //    };

                        //    invoiceItem.SupplierInvoiceItemsConDeclars.Add(connectedDeclarationPM);

                        //}

                        //foreach (SupplierInvoiceItemsDescriptPM desc in item.SupplierInvoiceItemsDescripts)
                        //{
                        //    SupplierInvoiceItemsDescriptPM descreption = new SupplierInvoiceItemsDescriptPM()
                        //        {
                        //            DeclarationId = toDeclaration.Id,
                        //            TypeCode = desc.TypeCode,
                        //            TypeName = desc.TypeName,
                        //            Description = desc.Description,
                        //            InvoiceCounterKey = desc.InvoiceCounterKey,
                        //            InvoiceItemLineNumber = desc.InvoiceItemLineNumber,
                        //            LineNumber = desc.LineNumber,
                        //            Tenant = desc.Tenant
                        //        };

                        //    invoiceItem.SupplierInvoiceItemsDescripts.Add(descreption);
                        //}

                        //foreach (SupplierInvoiceItemsLevyPM levy in item.SupplierInvoiceItemLevies)
                        //{
                        //    SupplierInvoiceItemsLevyPM levyPM = new SupplierInvoiceItemsLevyPM()
                        //    {
                        //        DeclarationId = toDeclaration.Id,
                        //        Tenant = toDeclaration.Tenant,
                        //        TradeLevyExamptCode = levy.TradeLevyExamptCode,
                        //        TradeLevyExamptName = levy.TradeLevyExamptName,
                        //        TradeLevyNumber = levy.TradeLevyNumber,
                        //        InvoiceCounterKey = levy.InvoiceCounterKey,
                        //        InvoiceItemLineNumber = levy.InvoiceItemLineNumber,
                        //        LineNumber = levy.LineNumber

                        //    };

                        //    invoiceItem.SupplierInvoiceItemLevies.Add(levyPM);
                        //}

                        //foreach (SupplierInvoiceItemsModPM mod in item.SupplierInvoiceItemsMods)
                        //{
                        //    SupplierInvoiceItemsModPM modPM = new SupplierInvoiceItemsModPM()
                        //    {
                        //        DeclarationId = toDeclaration.Id,
                        //        Tenant = toDeclaration.Tenant,
                        //        Amount = mod.Amount,
                        //        CurrencyTypeCode = mod.CurrencyTypeCode,
                        //        CurrencyTypeName = mod.CurrencyTypeName,
                        //        TypeCode = mod.TypeCode,
                        //        TypeName = mod.TypeName,
                        //        InvoiceCounterKey = mod.InvoiceCounterKey,
                        //        LineNumber = mod.LineNumber,
                        //        ModificationCounterKey = mod.ModificationCounterKey



                        //    };

                        //    invoiceItem.SupplierInvoiceItemsMods.Add(modPM);
                        //}

                        //foreach (SupplierInvoiceItemsProdIdentPM productIdent in item.SupplierInvoiceItemsProdIdents)
                        //{
                        //    SupplierInvoiceItemsProdIdentPM productIdentPM = new SupplierInvoiceItemsProdIdentPM()
                        //    {
                        //        DeclarationId = toDeclaration.Id,
                        //        Tenant = toDeclaration.Tenant,
                        //        Identification = productIdent.Identification,
                        //        TypeCode = productIdent.TypeCode,
                        //        TypeName = productIdent.TypeName,
                        //        InvoiceCounterKey = productIdent.InvoiceCounterKey,
                        //        LineNumber = productIdent.LineNumber,
                        //        InvoiceItemLineNumber = productIdent.InvoiceItemLineNumber

                        //    };

                        //    invoiceItem.SupplierInvoiceItemsProdIdents.Add(productIdentPM);
                        //}

                        //foreach (SupplierInvoiceItemsSerialNumPM serialNum in item.SupplierInvoiceItemsSerialNums)
                        //{
                        //    SupplierInvoiceItemsSerialNumPM serialNumPM = new SupplierInvoiceItemsSerialNumPM()
                        //    {
                        //        DeclarationId = toDeclaration.Id,
                        //        Tenant = toDeclaration.Tenant,
                        //        SerialNumber = serialNum.SerialNumber,

                        //        TypeCode = serialNum.TypeCode,
                        //        TypeName = serialNum.TypeName,
                        //        InvoiceCounterKey = serialNum.InvoiceCounterKey,
                        //        LineNumber = serialNum.LineNumber,
                        //        InvoiceItemLineNumber = serialNum.InvoiceItemLineNumber

                        //    };

                        //    invoiceItem.SupplierInvoiceItemsSerialNums.Add(serialNumPM);
                        //}

                        //foreach (SupplierInvoiceItemVehiclePM vehicle in item.SupplierInvoiceItemVehicles)
                        //{
                        //    SupplierInvoiceItemVehiclePM vehiclePM = new SupplierInvoiceItemVehiclePM()
                        //    {
                        //        DeclarationId = toDeclaration.Id,
                        //        Tenant = toDeclaration.Tenant,
                        //        RichbitFileNumber = vehicle.RichbitFileNumber,
                        //        RichbitFileStatus = vehicle.RichbitFileStatus,
                        //        VehicleChassisNumber = vehicle.VehicleChassisNumber,
                        //        VehicleTypeCode = vehicle.VehicleTypeCode,
                        //        VehicleId = vehicle.VehicleId,
                        //        InvoiceCounterKey = vehicle.InvoiceCounterKey,
                        //        LineNumber = vehicle.LineNumber,
                        //        InvoiceItemLineNumber = vehicle.InvoiceItemLineNumber,

                        //        SequenceNumeric = vehicle.SequenceNumeric,
                        //    };

                        //    invoiceItem.SupplierInvoiceItemVehicles.Add(vehiclePM);
                        //}


                        invoicePM.SupplierInvoiceItems.Add(invoiceItem);

                    }


                    GetSupplierInvoiceModifications(toDeclarationId, fromDeclarationId, tenant, fromDeclaration.Direction, invoice, invoicePM);

                    SupplierInvoiceUpdateService invoiceUpdateService = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), tenant);
                    invoiceUpdateService.Update(invoicePM, true);
                    //     CustomsStoredProcedures.CopySupplierInvoiceItems(fromDeclarationId, toDeclarationId,invoicePM.InvoiceCounterKey,  tenant);
                }

                //CustomsStoredProcedures.CopySupplierInvoiceItems(fromDeclarationId, toDeclarationId, tenant);
                //CustomsStoredProcedures.CopySupplierInvoiceItemsCer(fromDeclarationId, toDeclarationId, tenant);

                //toDeclaration.SupplierInvoices.Add(invoicePM);

                scope.Complete();

            }

            //SaveCurrentEntityChangesEvent saveEntityChanges = currentAssemlyLocator.CurrentEditControlViewModel.eventAggregator.GetEvent<SaveCurrentEntityChangesEvent>();
            //saveEntityChanges.Publish(new SaveCurrentEntityChangesEventArgs());

            //RefreshScreenAfterReloadWithNewContextEvent RefreshScreenAfterReloadWithNewContext = currentAssemlyLocator.CurrentEditControlViewModel.eventAggregator.GetEvent<RefreshScreenAfterReloadWithNewContextEvent>();
            //RefreshScreenAfterReloadWithNewContext.Publish(new RefreshScreenAfterReloadWithNewContextEventArgs() { ObjectTableName = "Customs.Declaration", CurrentEntity = entityPM, Context = declarationViewModel.context });

            // declarationViewModel.context.SubmitChanges();

            //declarationSelectionWindow.Close();
            //declarationSelectionWindow = null;


            return true;

        }

        private void GetSupplierInvoiceModifications(string toDeclarationId, string fromDeclarationId, int tenant, string declarationDirection, SupplierInvoicePM supplierInvoicePMOrg, SupplierInvoicePM supplierInvoicePMNew)
        {
            if (declarationDirection != "E") return;

            supplierInvoicePMOrg.SupplierInvoiceModifications = new SupplierInvoiceModificationQueryService(tenant).GetMulti(new SupplierInvoiceKeys() { DeclarationId = fromDeclarationId, InvoiceCounterKey = supplierInvoicePMNew.InvoiceCounterKey }, true, true);

            List<SupplierInvoiceModificationPM> supplierInvoiceItemPMs = new List<SupplierInvoiceModificationPM>();
            var sims = supplierInvoicePMOrg.SupplierInvoiceModifications;
            sims.ForEach(sim =>
            {
                supplierInvoiceItemPMs.Add(new SupplierInvoiceModificationPM
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    DeclarationId = toDeclarationId,
                    InvoiceCounterKey = sim.InvoiceCounterKey,
                    ModificationCounterKey = sim.ModificationCounterKey,
                    TypeCode = sim.TypeCode,
                    Tenant = sim.Tenant,
                    CurrencyTypeCode = sim.CurrencyTypeCode,
                    Amount = sim.Amount,
                    TypeDesc = sim.TypeDesc
                });
            });

            supplierInvoicePMNew.SupplierInvoiceModifications = supplierInvoiceItemPMs;
        }


        public SendDeclarationChecksResult DoSendDeclarationChekcs(string declarationId, int tenant)
        {
            //-----------------------Defenitions-------------------------------//
            ICustomContext myContext = CustomContext.GetContext(tenant);
            DeclarationQueryService declarationQueryService = new EntityQueryServices.DeclarationQueryService(myContext);
            ClientQueryService clientQueryService = new ClientQueryService(myContext);
            CustomsExchangeRateQueryService exchangeRateQueryService = new CustomsExchangeRateQueryService(myContext);

            //--------------------Getting single declaration---------------------------------------//
            declarationQueryService.LoadSupplierInvoicesWithItems = false;
            DeclarationPM declaration = declarationQueryService.GetSingle(declarationId, true, false);

            //-----------------Clients Check----------------//
            if (!string.IsNullOrEmpty(declaration.ImporterCode))
            {
                ClientPM client = clientQueryService.GetClientByCode(declaration.ImporterCode, tenant);
                if (client != null)
                {
                    declaration.ImporterId = client.Id;
                }
                //else //this check doesn't happen in silverlight...
                //{
                //    SendDeclarationChecksResult result = new Contracts.SendDeclarationChecksResult();
                //    result.HasError = true;
                //    result.ErrorMessages = new List<string>();
                //    result.ErrorMessages.Add("יש לשלוף יבואן מהמכס לפני שליחה");
                //    result.ErrorsType = "Customs.General.O.PreSendValidations";
                //    return result;
                //}
            }
            //--------------------------------------------------------//

            //----------------------exchange rates checks-----------------------//
            if (declaration.SupplierInvoices != null)
            {
                if (declaration.SupplierInvoices.Count > 0)
                {
                    List<string> codesList = new List<string>();
                    //Get all Currency types
                    foreach (var supplierInvoice in declaration.SupplierInvoices)
                    {
                        if (!string.IsNullOrWhiteSpace(supplierInvoice.InvoiceCurrencyTypeCode))
                        {
                            codesList.Add(supplierInvoice.InvoiceCurrencyTypeCode);
                        }
                        foreach (var supplierInvoiceFreightAmounts in supplierInvoice.SupplierInvoiceFreightAmounts)
                        {
                            if (!string.IsNullOrWhiteSpace(supplierInvoiceFreightAmounts.CurrencyTypeCode))
                            {
                                codesList.Add(supplierInvoiceFreightAmounts.CurrencyTypeCode);
                            }
                        }
                    }

                    codesList = codesList.Distinct<string>().ToList();
                    string codes = string.Join(",", codesList.ToArray());
                    List<CustomsExchangeRatePM> myCustomsExchangeRatePMList = exchangeRateQueryService.GetExchangeRateByCurrencyAndDate(codes, declaration.TaxationDateTime, tenant);
                    foreach (var supplierInvoice in declaration.SupplierInvoices.Where(rec => !string.IsNullOrWhiteSpace(rec.InvoiceCurrencyTypeCode)))
                    {
                        Decimal? invoiceExchangeRate = 1;
                        Decimal? amountExchangeRate = 1;
                        Decimal? totalFreightInInvoiceCurrency = 0;
                        Decimal? totalFreightInNIS = 0;
                        //Get the Exchange Rate for the Invoice Currency type
                        CustomsExchangeRatePM rate = myCustomsExchangeRatePMList.FirstOrDefault(obj => obj.CurrencyTypeCode == supplierInvoice.InvoiceCurrencyTypeCode);
                        if (rate != null)
                        {
                            if (!string.IsNullOrWhiteSpace(rate.ExchangeRate.ToString()))
                            {
                                invoiceExchangeRate = rate.ExchangeRate;
                            }
                        }

                        //Go over all Freight Amounts for the Invoice
                        foreach (var supplierInvoiceFreightAmounts in supplierInvoice.SupplierInvoiceFreightAmounts)
                        {
                            //Get the Exchange Rate for the Freight Amount Currency type
                            rate = myCustomsExchangeRatePMList.FirstOrDefault(obj => obj.CurrencyTypeCode == supplierInvoiceFreightAmounts.CurrencyTypeCode);
                            if (rate != null)
                            {
                                if (!string.IsNullOrWhiteSpace(rate.ExchangeRate.ToString()))
                                {
                                    amountExchangeRate = rate.ExchangeRate;
                                }
                            }
                            //SUM
                            totalFreightInInvoiceCurrency = totalFreightInInvoiceCurrency + ((supplierInvoiceFreightAmounts.Amount * amountExchangeRate) / invoiceExchangeRate);
                            //SUM in ILS
                            totalFreightInNIS = totalFreightInNIS + (supplierInvoiceFreightAmounts.Amount * amountExchangeRate);
                        }
                        //supplierInvoice.TotalFreightInInvoiceCurrency = LogitudeUtilities.Round(totalFreightInInvoiceCurrency, 2);
                        //SUM in ILS
                        supplierInvoice.TotalFreightInNIS = this.Round(totalFreightInNIS, 2);
                    }
                }
            }
            //---------------save updated data for declaration and invoices--------------//
            this.Update(declaration, true);


            var res = new Def.Contracts.SendDeclarationChecksResult() { HasError = false };

            if (declaration.Direction != "E")
            {
                var siWithInsurance = declaration.SupplierInvoices.Where(r => r.InsuranceAmount.HasValue).ToList();
                if (siWithInsurance.Count > 1)
                {

                    var siWithValues = siWithInsurance.Where(r => r.InsuranceAmount > 0).ToList();
                    if (siWithValues.Count > 1)
                    {

                        res.HasError = true;
                        res.ErrorMessages = new List<string>() { "קיים יותר מחשבון ספק אחד עם ערך בשדה ביטוח " };

                    }



                }

            }


            return res;//new Contracts.SendDeclarationChecksResult() { HasError = false };
        }

        public decimal? Round(object value, int digits)
        {
            double? myValue = null;
            if (value != null)
            {
                myValue = Convert.ToDouble(value);
            }

            double? myResult = myValue;

            if (myValue != null && digits >= 1 && digits <= 15)
            {
                string mySTR = String.Format("{0:N" + digits + "}", myValue);

                myResult = Convert.ToDouble(mySTR);
            }

            if (myResult == null)
            {
                return null;
            }

            else
            {
                return (decimal)myResult;
            }
        }





        public override void InitializeEntityPM(DeclarationPM entityPM)
        {
            entityPM.MarkAsChanged = true;
        }

        public void UpdateHataraStatusByContarization(DeclarationPM entityPM, Declaration entityPOCO)
        {

            if (entityPM.Direction == "E" && entityPM.HatraDate != entityPOCO.HatraDate)

            {

                ConsignmentQueryService consignmentQueryService = new ConsignmentQueryService(entityPM.Tenant);
                var list = consignmentQueryService.GetConsgnmentByDeclarationId(entityPM.Id, entityPM.Tenant);
                list?.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.ExportContainerizationID))
                    {
                        DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                        var listDec = declarationQueryService.GetByConsigmentExportContainerizationID(x.ExportContainerizationID, entityPM.Tenant);
                        if (listDec.All(y => y.HatraDate.HasValue || y.Id == entityPM.Id))

                        {
                            ContainerizationQueryService containerizationQueryService = new ContainerizationQueryService(entityPM.Tenant);
                            var containerization = containerizationQueryService.GetSingle(x.ExportContainerizationID, false, true);
                            containerization.HataraStatus = "1";
                            containerization.ChangeSetOp = ChangeSetOperation.Update;
                            ContainerizationUpdateService containerizationUpdateService = new ContainerizationUpdateService(MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                            containerizationUpdateService.Update(containerization, true);
                        }
                    }

                });

            }
        }
        public void ReCalculateDueTaxationDateChange(DeclarationPM entityPM, Declaration entityPOCO)
        {
            if (entityPOCO.TaxationDateTime != entityPM.TaxationDateTime)
            {
                CustomContext context = this.MainContext as CustomContext;
                InsuranceFreightUtil util = new InsuranceFreightUtil();
                //EntityQueryServices.SupplierInvoiceQueryService invoicequeryservice = new EntityQueryServices.SupplierInvoiceQueryService(context);
                SupplierInvoiceRepository invoiceRepository = new Data.Repsitories.SupplierInvoiceRepository(context);

                //SupplierInvoiceUpdateService updateservice = new EntityUpdateServices.SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                List<SupplierInvoice> invoices = invoiceRepository.GetSupplierInvoicesForDeclaration(entityPM.Id, entityPM.Tenant);
                foreach (SupplierInvoice invoice in invoices)
                {


                    invoice.InvoiceAmountInUSD = InsuranceFreightUtil.CalcInvoiceAmountInUSD(entityPM.TaxationDateTime, invoice.InvoiceCurrencyTypeCode, invoice.InvoiceAmount.GetValueOrDefault(), invoice.Tenant);



                    util.CalculateFreightForInvoice(invoice, entityPM.TaxationDateTime);

                    invoiceRepository.SubmitChanges();
                    //invoice.ChangeSetOp = ChangeSetOperation.Update;
                    //updateservice.Update(invoice, true);
                }

                util.CalculateInsurance(entityPM);

            }
        }

        public bool CheckIfRequiredFieldForCourierHasChanged(DeclarationPM entityPM, Declaration entityPOCO)
        {
            //Check Declaration fields
            if (!AreEqualIgnoringSpaces(entityPOCO.AgentId, entityPM.AgentId)  || !AreEqualIgnoringSpaces(entityPOCO.ImporterName, entityPM.ImporterName) || !AreEqualIgnoringSpaces(entityPOCO.ImporterAddress, entityPM.ImporterAddress))
            {
                return true;
            }

            DeclarationPM dbOccDeclarationPM = GetDBEntity(entityPM.Id, entityPM.Tenant);
            //Check SupplierInvoice fields
            if (entityPM.SupplierInvoices != null)
            {
                foreach (SupplierInvoicePM supplierInvoiceItem in entityPM.SupplierInvoices)
                {
                    foreach (SupplierInvoicePM dbOccsupplierInvoiceItem in dbOccDeclarationPM.SupplierInvoices)
                    {
                        if (supplierInvoiceItem.SequenceNumeric == dbOccsupplierInvoiceItem.SequenceNumeric)
                        {
                            if (!AreEqualIgnoringSpaces(supplierInvoiceItem.VendorId, dbOccsupplierInvoiceItem.VendorId)
                                || !AreEqualIgnoringSpaces(supplierInvoiceItem.IncotermCode, dbOccsupplierInvoiceItem.IncotermCode))
                            {
                                return true;
                            }
                            else
                            {
                                if (!AreEqualIgnoringSpaces(entityPOCO.CasualSupplierName, entityPM.CasualSupplierName)   ||
                                    !AreEqualIgnoringSpaces(entityPOCO.CasualSupplierAddress,entityPM.CasualSupplierAddress))
                                {
                                    return true;
                                }

                            }
                            break;
                        }
                    }
                }
            }

            //Check Consignments fields
            if (entityPM.Consignments != null)
            {
                foreach (ConsignmentPM consignmentItem in entityPM.Consignments)
                {
                    foreach (ConsignmentPM dbOccconsignmentItem in dbOccDeclarationPM.Consignments)
                    {
                        if (consignmentItem.SequenceNumeric == dbOccconsignmentItem.SequenceNumeric)
                        {
                            if (!AreEqualIgnoringSpaces(consignmentItem.StorageSiteCode, dbOccconsignmentItem.StorageSiteCode) 
                                || !AreEqualIgnoringSpaces(consignmentItem.ManifestNumber, dbOccconsignmentItem.ManifestNumber)  
                                || !AreEqualIgnoringSpaces(consignmentItem.ThirdCargoID, dbOccconsignmentItem.ThirdCargoID)
                                || !AreEqualIgnoringSpaces(consignmentItem.CargoDescription, dbOccconsignmentItem.CargoDescription))  
                            {
                                return true;
                            }

                            foreach (ConsignmentPackagePM consignmentPackageItem in consignmentItem.ConsignmentPackages)
                            {
                                foreach (ConsignmentPackagePM dbOccconsignmentPackageItem in dbOccconsignmentItem.ConsignmentPackages)
                                {
                                    if (consignmentPackageItem.LineNumber == dbOccconsignmentPackageItem.LineNumber)
                                    {
                                        if ( consignmentPackageItem.PackageQuantity != dbOccconsignmentPackageItem.PackageQuantity
                                            || consignmentPackageItem.GrossMassMeasure != dbOccconsignmentPackageItem.GrossMassMeasure
                                            || !AreEqualIgnoringSpaces(consignmentPackageItem.PackageTypeCode, dbOccconsignmentPackageItem.PackageTypeCode) )
                                        {
                                            return true;
                                        }
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }

            return false;
        }

        public void SendClientSearch(DeclarationPM entityPM)
        {

            if (entityPM.IsCourierDeclaration && this.EntityPOCO.ImporterCode != entityPM.ImporterCode)
            {

                var loggedUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
                string importerId = entityPM.ImporterCode;
                if (entityPM.ImporterCode.Length > 9)
                {
                    importerId = entityPM.ImporterCode.Substring(0, 9);
                }
                var newClientSearchRequestParams = new ClientSearchRequestParams()
                {
                    LoggingEnabled = true,
                    IsFakeResponse = true,
                    InterfaceTypeCode = "3610",
                    Tenant = Tenant,
                    RequestName = "Client Search",
                    ResponseName = "Client Search",
                    LoggingUserId = loggedUserId,
                    RequestVIA = SendRequestVIA.WebServiceBatch,
                    SuppressSplitWR = true,
                    ExternalId = importerId,
                };

                try
                {
                    SBQMessageService.CreateSheetSBQMessage<Logitude.CustomsMessaging.Common.RequestParams.ClientSearchRequestParams>(newClientSearchRequestParams
                        , false, DateTime.Now
                        );
                }
                catch (CustomsRequestsSheetDomainModelServiceException myCustomsRequestsSheetServiceException)
                {
                    if (myCustomsRequestsSheetServiceException.Where == CustomsRequestsSheetDomainModelServiceException.WhereEnum.SameRequestInProgress)
                    {
                        Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.AppendLine("3610 RequestInProgress stop create a new one !! ");
                    }
                    throw;
                }
            }
        }

        private string TranslateClient(string Importercode)
        {

            ClientQueryService clientQueryService = new ClientQueryService(ResolvedTenant());

            var clientId = clientQueryService.GetIdByCode(Importercode, ResolvedTenant());

            if (clientId == null)
            {
                return null;
            }
            return clientId;
        }
        private bool AreEqualIgnoringSpaces(string str1, string str2)
        {
            if (str1 == null && str2 == null)
                return true;
            if (str1 == null || str2 == null)
                return false;
            string cleanStr1 =  str1.Replace(" ", "");
            string cleanStr2 = str2.Replace(" ", "");

            return cleanStr1.Equals(cleanStr2);
        }
        public static string UpdateSupplierInvoiceItemsWhoHasError12195(string declarationId, int tenant)
        {
            try
            {
                var customContext = CustomContext.GetContext(tenant);
                var declarationQuery = new DeclarationQueryService(customContext);
                DeclarationPM myDeclarationPM = declarationQuery.GetSingle(declarationId, true, false);

                //If the Declaration exists
                if (myDeclarationPM == null) return null;

                DeclarationQueryService query = new DeclarationQueryService(customContext);
                var errors12195 = query.GetDeclarationErrors(myDeclarationPM.Id, tenant, null).Where(x => x.ErrorType == "12195").ToList();
                foreach (var error in errors12195)
                {
                    var line = error.Line - 1;
                    var ParentLine = error.ParentLine - 1;
                    if (line >= 0 && ParentLine >= 0 && myDeclarationPM.SupplierInvoices.Count > ParentLine)
                    {
                        if (myDeclarationPM.SupplierInvoices[ParentLine.Value].SupplierInvoiceItems.Count >= line)
                        {
                            myDeclarationPM.SupplierInvoices[ParentLine.Value].SupplierInvoiceItems[line.Value].CustomsBookTypeCode = "3";
                            myDeclarationPM.SupplierInvoices[ParentLine.Value].SupplierInvoiceItems[line.Value].ChangeSetOp = ChangeSetOperation.Update;
                            myDeclarationPM.SupplierInvoices[ParentLine.Value].ChangeSetOp = ChangeSetOperation.Update;
                            myDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;

                        }
                    }
                }

                if (myDeclarationPM.ChangeSetOp == ChangeSetOperation.Update)
                {
                    DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), myDeclarationPM.Tenant);
                    service.Update(myDeclarationPM, true);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
    }
}
