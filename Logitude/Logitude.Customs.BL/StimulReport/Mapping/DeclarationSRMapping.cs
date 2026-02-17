using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;

namespace Logitude.Customs.BL.StimulReport.Mapping
{
    public class DeclarationSRMapping
    {
        ICustomContext context;
        bool _OpenAccessFields = true;
        List<CurrencyAmount> amounts;

        public DeclarationSReport Get(DeclarationPM declarationPM)
        {
            //var context = CustomContext.GetContext(declarationPM.Tenant);
            if (context == null) context = CustomContext.GetContext(declarationPM.Tenant); // moran 10.9.15 - Task 16086
            CardRepository rep = new CardRepository(declarationPM.Tenant);
            Card customerCard = rep.GetSingleCardByCode(declarationPM.CustomerCode, declarationPM.Tenant, false);
            List<DeclarationErrorView> declarationErrors;
            
            
            var declarationSReport = new DeclarationSReport();
            declarationSReport.mehes_file = new List<mehes_file>(); declarationSReport.mehes_file.Add(new mehes_file());
            declarationSReport.mehes_file[0].custom_agent = declarationPM.AgentId;
            //if (_OpenAccessFields && !string.IsNullOrWhiteSpace(declarationPM.CustomFileNo))
            //{
            //    using (var cntxt = AmitalContextUtil.GetContext())
            //    {
            //        long fileNoLong;
            //        if (long.TryParse(declarationPM.CustomFileNo, out fileNoLong))
            //        {
            //            var myCCUFILEMQueryService = new CCUFILEMQueryService(cntxt);
            //            int? file_number = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO(fileNoLong);
            //            if (file_number != null)
            //            {
            //                declarationSReport.mehes_file[0].file_number = file_number.ToString();
            //            }
            //        }
            //    }
            //}
            declarationSReport.mehes_file[0].customs_file_num = declarationPM.CustomFileNo;
            declarationSReport.mehes_file[0].mehes_draft_status = declarationPM.DeclarationStatusTypeName; //0
            declarationSReport.mehes_file[0].customer = new List<customer>(); declarationSReport.mehes_file[0].customer.Add(new customer());
            declarationSReport.mehes_file[0].customer[0].customerid = declarationPM.CustomerCode; //1
            declarationSReport.mehes_file[0].customer[0].customername = declarationPM.CustomerName; //2
            declarationSReport.mehes_file[0].importer = new List<importer>(); declarationSReport.mehes_file[0].importer.Add(new importer());
            declarationSReport.mehes_file[0].importer[0].importerid = declarationPM.ImporterCode; //4
            if (customerCard != null) // moran 20.8.15 - task 13902 put VAT number in unused field
            {
                declarationSReport.mehes_file[0].transfer_importer_id = customerCard.VatNumber;
            }
            declarationSReport.mehes_file[0].importer[0].importername = declarationPM.CalculatedImporterName; //5 // moran 19.12.17 - AMI-62612 - change ImporterName to CalculatedImporterName
            declarationSReport.mehes_file[0].declaration_num = declarationPM.DeclarationNumber; //9
            declarationSReport.mehes_file[0].version_id = declarationPM.VersionId; //9
            declarationSReport.mehes_file[0].customs_branch = new List<customs_branch>(); declarationSReport.mehes_file[0].customs_branch.Add(new customs_branch());
            declarationSReport.mehes_file[0].customs_branch[0].customs_branchid = declarationPM.DeclarationOfficeCode;   //8
            declarationSReport.mehes_file[0].customs_branch[0].customs_branchname = declarationPM.DeclarationOfficeName;  //8
            declarationSReport.mehes_file[0].autonomy_name = declarationPM.AutonomyRegionTypeName; //12
            declarationSReport.mehes_file[0].reshimon_type = new List<reshimon_type>(); declarationSReport.mehes_file[0].reshimon_type.Add(new reshimon_type());
            // moran 26.8.15 - task 16093 -->
            //declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typeid = declarationPM.DeclarationDocumentTypeCode;  //10
            //declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typename = declarationPM.DeclarationDocumentTypeName;  //10
            //declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typeid = declarationPM.ProcedureCurrentCode;  //10 // moran 6.9.15 - Task 16086 - commented
            GovernmentProcedureTypeQueryService procedureQuery = new GovernmentProcedureTypeQueryService(context);
            GovernmentProcedureTypePM procedure = procedureQuery.GetSingle(declarationPM.ProcedureCurrentCode, true, false);
            if (procedure != null)
            {
                if (!String.IsNullOrWhiteSpace(procedure.LocalName))
                {
                    declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typename = procedure.LocalName;
                }
                else if (!String.IsNullOrWhiteSpace(procedure.EnglishName))
                {
                    declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typename = procedure.EnglishName;
                }
            }
            // moran 26.8.15 - task 16093 <--
            // moran 6.9.15 - Task 16086 -->
            declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typename = declarationPM.ProcedureCurrentCode + " - " + declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typename;
            declarationSReport.mehes_file[0].reshimon_num = declarationPM.DeclarationDocumentId; // ?? declarationPM.DeclarationNumber;
            LeadDocumentTypeQueryService LeadDocumentQuery = new LeadDocumentTypeQueryService(context);
            LeadDocumentTypePM leadDocument = LeadDocumentQuery.GetSingle(declarationPM.DeclarationDocumentTypeCode, true, false);
            if (leadDocument != null)
            {
                if (!String.IsNullOrWhiteSpace(leadDocument.LocalName))
                {
                    declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typeid = leadDocument.LocalName;
                }
                else if (!String.IsNullOrWhiteSpace(leadDocument.EnglishName))
                {
                    declarationSReport.mehes_file[0].reshimon_type[0].reshimon_typeid = leadDocument.EnglishName;
                }
            }
            // moran 6.9.15 - Task 16086 <--
            if (declarationPM.TaxationDateTime.HasValue)
            {
                declarationSReport.mehes_file[0].draft_date = declarationPM.TaxationDateTime.Value.Date.ToString("dd.MM.yy"); //11
            }
            declarationSReport.mehes_file[0].autonomy = declarationPM.AutonomyRegionTypeCode;
            declarationSReport.mehes_file[0].trans_import_id = declarationPM.TransferImporterCode; //13
            if (!string.IsNullOrWhiteSpace(declarationPM.CalculatedTransferImporterName))
            {
                declarationSReport.mehes_file[0].trans_import_name = declarationPM.CalculatedTransferImporterName;
            }
            else
            {
                declarationSReport.mehes_file[0].trans_import_name = declarationPM.TransferImporterName; //13
            }
            //declarationSReport.mehes_file[0].owner_right = new List<owner_right>(); declarationSReport.mehes_file[0].owner_right.Add(new owner_right());
            //declarationSReport.mehes_file[0].owner_right[0].owner_rightid = declarationPM.EntitleImporterCode; //14
            //declarationSReport.mehes_file[0].owner_right[0].owner_rightname = declarationPM.EntitleImporterName; //14
            if (declarationPM.ImporterEntitlementTypeCode != null)
            {
                if (string.IsNullOrWhiteSpace(declarationPM.ImporterEntitlementTypeName))
                {
                    EntitlementTypeQueryService entitlementTypeQueryService = new EntitlementTypeQueryService(declarationPM.Tenant);
                    EntitlementTypePM entitlementType = entitlementTypeQueryService.GetSingle(declarationPM.ImporterEntitlementTypeCode, false, false);
                    declarationPM.ImporterEntitlementTypeName = entitlementType.LocalName;
                }
                declarationSReport.mehes_file[0].owner_right = new List<owner_right>(); declarationSReport.mehes_file[0].owner_right.Add(new owner_right());
                declarationSReport.mehes_file[0].owner_right[0].owner_rightid = declarationPM.ImporterEntitlementTypeCode; //14
                declarationSReport.mehes_file[0].owner_right[0].owner_rightname = declarationPM.ImporterEntitlementTypeName; //14
            }
            declarationSReport.mehes_file[0].Entitle_importer_Code = declarationPM.EntitleImporterCode;
            if (!string.IsNullOrWhiteSpace(declarationPM.CalculatedEntitleImporterName))
            {
                declarationSReport.mehes_file[0].Entitle_importer_name = declarationPM.CalculatedEntitleImporterName;
            }
            else
            {
                declarationSReport.mehes_file[0].Entitle_importer_name = declarationPM.EntitleImporterName;
            }
            // סוג + מס' הצהרה קשורה // ?? //15 
            declarationSReport.mehes_file[0].mishgor = GetMishgor(declarationPM);

            if (declarationPM.DealValue.HasValue)
            {
                declarationSReport.mehes_file[0].accepted_price = declarationPM.DealValue.ToString(); //1
            }
            if (declarationPM.CIFValue.HasValue)
            {
                declarationSReport.mehes_file[0].cif_value = declarationPM.CIFValue.ToString(); //2
            }
            if (declarationPM.PlatformFee.HasValue)
            {
                declarationSReport.mehes_file[0].fee_platform = declarationPM.PlatformFee.Value.ToString();  //4 אגרת רציף , 
            }
            if (declarationPM.LoadingFactor.HasValue)
            {
                declarationSReport.mehes_file[0].fee_carrier = declarationPM.LoadingFactor.Value.ToString();  //5  , מקדם העמסה
            }
            if (declarationPM.DealValueWithoutFactor.HasValue)
            {
                declarationSReport.mehes_file[0].goods_value = declarationPM.DealValueWithoutFactor.ToString(); //3
            }
            if (declarationPM.TotalTax.HasValue)
            {
                declarationSReport.mehes_file[0].total_tax = declarationPM.TotalTax.ToString(); //6
            }

            if (customerCard != null)
            {
                AddressRepository AddRep = new AddressRepository(declarationPM.Tenant);
                Address cardAdress = AddRep.GetSingleAddressByCardIdAndTypeId(customerCard.Id, "L", declarationPM.Tenant);
                if (cardAdress == null)
                {
                    cardAdress = AddRep.GetSingleAddressByCardIdAndTypeId(customerCard.Id, "M", declarationPM.Tenant);
                }

                declarationSReport.mehes_file[0].customer[0].customermamps_no = customerCard.ReceivablesAccountingCard; //?? //3
                if (cardAdress != null)
                {
                    declarationSReport.mehes_file[0].customer[0].customertelephone = cardAdress.PhoneNumber;  //6
                    declarationSReport.mehes_file[0].customer[0].customerfax = cardAdress.FaxNumber;  //7
                }

            }
            declarationSReport.mehes_file[0].acc_supplier = GetAccSupplier(declarationPM);
            declarationSReport.mehes_file[0].tax_details = GetDeclarationTaxDetails(declarationPM);

            declarationSReport.mehes_file[0].tax = GetDeclarationTaxes(declarationPM);

            declarationSReport.mehes_file[0].message_details = new List<message_details>(); declarationSReport.mehes_file[0].message_details.Add(new message_details());
            // moran 31.8.15 - task 16093 -->
            //declarationSReport.mehes_file[0].message_details[0].message_detailserror_message = declarationPM.ErrosXml;
            declarationErrors = declarationPM.DeclarationErrorViews.ToList();
            if (declarationErrors != null)
            {
                var message_detailsList = new List<message_details>();
                foreach (var declarationerror in declarationErrors)
                {
                    var messagedetails = new message_details();
                    messagedetails.message_detailscode = declarationerror.ErrorType;
                    messagedetails.message_detailserror_message = declarationerror.Description;
                    //messagedetails.message_detailsnumber = declarationerror.ListVersionId;
                    //messagedetails.message_detailssegment = declarationerror.ErrorType;
                    // string packtypename = null;
                    LeadDocumentExceptionTypeQueryService LeadDocumentExceptionTypeQuery = new LeadDocumentExceptionTypeQueryService(context);
                    LeadDocumentExceptionTypePM LeadDocumentExceptionType = LeadDocumentExceptionTypeQuery.GetSingle(declarationerror.ListVersionId, true, false);
                    if (LeadDocumentExceptionType != null)
                    {
                        if (!String.IsNullOrWhiteSpace(LeadDocumentExceptionType.LocalName))
                        {
                            messagedetails.message_detailsnumber = LeadDocumentExceptionType.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(LeadDocumentExceptionType.EnglishName))
                        {
                            messagedetails.message_detailsnumber = LeadDocumentExceptionType.EnglishName;
                        }
                    }
                    if (declarationerror.ConstraintId != null)
                    {
                        DeclarationConstraintQueryService DeclarationConstraintQuery = new DeclarationConstraintQueryService(context);
                        DeclarationConstraintPM DeclarationConstraint = DeclarationConstraintQuery.GetSingle(declarationPM.Id, declarationerror.ConstraintId, true, false);
                        if (DeclarationConstraint != null)
                        {
                            if (DeclarationConstraint.ConstraintStatusCode != "")
                            {
                                ConstraintStatusQueryService ConstraintStatusQuery = new ConstraintStatusQueryService(context);
                                ConstraintStatusPM ConstraintStatus = ConstraintStatusQuery.GetSingle(DeclarationConstraint.ConstraintStatusCode, true, false);
                                if (ConstraintStatus != null)
                                {
                                    if (!String.IsNullOrWhiteSpace(ConstraintStatus.LocalName))
                                    {
                                        messagedetails.message_detailssegment = ConstraintStatus.LocalName;
                                    }
                                    else if (!String.IsNullOrWhiteSpace(ConstraintStatus.EnglishName))
                                    {
                                        messagedetails.message_detailssegment = ConstraintStatus.EnglishName;
                                    }
                                }
                            }
                        }
                    }
                    message_detailsList.Add(messagedetails);
                }
                declarationSReport.mehes_file[0].message_details = message_detailsList;
            }
            // moran 31.8.15 - task 16093 <--
            //declarationSReport.mehes_file[0].message_details = XmlGenericUtil<message_details[]>.DeSerializeObject(declarationPM.ErrosXml);
            if (amounts != null)
            {
                string currencyAmountsList = null;
                foreach (var currencyAmount in amounts)
                {
                    currencyAmountsList = currencyAmountsList + currencyAmount.currency + " " + String.Format("{0:n}", currencyAmount.amount) + Environment.NewLine;
                }
                declarationSReport.mehes_file[0].Currency_amounts = currencyAmountsList;
                declarationSReport.mehes_file[0].Currency_amount_list = amounts;
            }
            return declarationSReport;
        }

        private List<mishgor> GetMishgor(DeclarationPM declarationPM)
        {
            //var context = CustomContext.GetContext(declarationPM.Tenant); // moran 11.8.15 - task 13902
            if (context == null) context = CustomContext.GetContext(declarationPM.Tenant); // moran 10.9.15 - Task 16086
            var mishgorList = new List<mishgor>();

            foreach (var consignment in declarationPM.Consignments)
            {
                var Mishgor = new mishgor();
                //"CargoType").Include("OriginCountry").Include("ReceiverWarehouse").Include("StorageSite").Include("UnloadPort
                Mishgor.mishgortransp_type = new List<mishgortransp_type>(); Mishgor.mishgortransp_type.Add(new mishgortransp_type());
                Mishgor.mishgortransp_type[0].mishgortransp_typeid = consignment.CargoTypeCode;//1 
                // moran 19.8.15 - task 13902 -->
                //Mishgor.mishgortransp_type[0].mishgortransp_typename = consignment.CargoTypeName; //1
                CargoIdentifireTypeQueryService cargoQuery = new CargoIdentifireTypeQueryService(context);
                CargoIdentifireTypePM cargo = cargoQuery.GetSingle(consignment.CargoTypeCode, true, false);
                if (cargo != null)
                {
                    if (!String.IsNullOrWhiteSpace(cargo.LocalName))
                    {
                        Mishgor.mishgortransp_type[0].mishgortransp_typename = cargo.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(cargo.EnglishName))
                    {
                        Mishgor.mishgortransp_type[0].mishgortransp_typename = cargo.EnglishName;
                    }
                }
                // moran 19.8.15 - task 13902 <--
                Mishgor.mishgorident_type = new List<mishgorident_type>(); Mishgor.mishgorident_type.Add(new mishgorident_type());
                Mishgor.mishgorident_type[0].mishgorident_typeid = consignment.CargoTypeCode; //1 
                Mishgor.mishgorident_type[0].mishgorident_typename = consignment.CargoTypeName; //1
                Mishgor.mishgormanifest = consignment.ManifestNumber; //2
                Mishgor.mishgorident_num = consignment.SecondCargoID; //3
                Mishgor.mishgorhawb = consignment.ThirdCargoID; //4
                if (consignment.ManifestDate.HasValue)
                {
                    Mishgor.mishgorhawb_date = consignment.ManifestDate.Value.Date.ToString("dd.MM.yy"); //5
                }
                if (consignment.UnloadDate.HasValue)
                {
                    Mishgor.mishgorunload_date = consignment.UnloadDate.Value.Date.ToString("dd.MM.yy"); //6
                }
                Mishgor.mishgorexport_land = new List<mishgorexport_land>(); Mishgor.mishgorexport_land.Add(new mishgorexport_land());
                Mishgor.mishgorexport_land[0].mishgorexport_landid = consignment.OriginCountryCode; //7
                // moran 19.8.15 - task 13902 -->
                //Mishgor.mishgorexport_land[0].mishgorexport_landname = consignment.OriginCountryName; //7
                CustomsCountryQueryService countryQuery = new CustomsCountryQueryService(context);
                CustomsCountryPM country = countryQuery.GetSingle(consignment.OriginCountryCode, true, false);
                if (country != null)
                {
                    if (!String.IsNullOrWhiteSpace(country.LocalName))
                    {
                        Mishgor.mishgorexport_land[0].mishgorexport_landname = country.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(country.EnglishName))
                    {
                        Mishgor.mishgorexport_land[0].mishgorexport_landname = country.EnglishName;
                    }
                }
                // moran 19.8.15 - task 13902 <--
                Mishgor.mishgorload_port = new List<mishgorload_port>(); Mishgor.mishgorload_port.Add(new mishgorload_port());
                Mishgor.mishgorload_port[0].mishgorload_portid = consignment.LoadingPortCode; //8
                //Mishgor.mishgorload_port[0].mishgorload_portname = consignment.LoadingPortName; //8 //??
                // moran 19.8.15 - task 13902 -->
                InternationalSiteQueryService loadingPortQuery = new InternationalSiteQueryService(context);
                InternationalSitePM loadingPort = loadingPortQuery.GetSingle(consignment.LoadingPortCode, true, false);
                if (loadingPort != null)
                {
                    if (!String.IsNullOrWhiteSpace(loadingPort.LocalName))
                    {
                        Mishgor.mishgorload_port[0].mishgorload_portname = loadingPort.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(loadingPort.EnglishName))
                    {
                        Mishgor.mishgorload_port[0].mishgorload_portname = loadingPort.EnglishName;
                    }
                }
                // moran 19.8.15 - task 13902 <--
                Mishgor.mishgorunload_port = new List<mishgorunload_port>(); Mishgor.mishgorunload_port.Add(new mishgorunload_port());
                Mishgor.mishgorunload_port[0].mishgorunload_portid = consignment.UnloadPortCode; //9
                // moran 19.8.15 - task 13902 -->
                //Mishgor.mishgorunload_port[0].mishgorunload_portname = consignment.UnloadPortName; //9
                UnloadingSiteTypeQueryService UnloadingPortQuery = new UnloadingSiteTypeQueryService(context);
                UnloadingSiteTypePM UnloadingPort = UnloadingPortQuery.GetSingle(consignment.UnloadPortCode, true, false);
                if (UnloadingPort != null)
                {
                    if (!String.IsNullOrWhiteSpace(UnloadingPort.LocalName))
                    {
                        Mishgor.mishgorunload_port[0].mishgorunload_portname = UnloadingPort.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(UnloadingPort.EnglishName))
                    {
                        Mishgor.mishgorunload_port[0].mishgorunload_portname = UnloadingPort.EnglishName;
                    }
                }
                // moran 19.8.15 - task 13902 <--
                Mishgor.mishgorstorage_type = new List<mishgorstorage_type>(); Mishgor.mishgorstorage_type.Add(new mishgorstorage_type());
                Mishgor.mishgorstorage_type[0].mishgorstorage_typeid = consignment.StorageSiteCode; //10
                // moran 19.8.15 - task 13902 -->
                //Mishgor.mishgorstorage_type[0].mishgorstorage_typename = consignment.StorageSiteName; //10
                DeliverySiteTypeQueryService storageSiteQuery = new DeliverySiteTypeQueryService(context);
                DeliverySiteTypePM storageSite = storageSiteQuery.GetSingle(consignment.StorageSiteCode, true, false);
                if (storageSite != null)
                {
                    if (!String.IsNullOrWhiteSpace(storageSite.LocalName))
                    {
                        Mishgor.mishgorstorage_type[0].mishgorstorage_typename = storageSite.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(storageSite.EnglishName))
                    {
                        Mishgor.mishgorstorage_type[0].mishgorstorage_typename = storageSite.EnglishName;
                    }
                }
                // moran 19.8.15 - task 13902 <--
                Mishgor.mishgorbonded_rec = new List<mishgorbonded_rec>(); Mishgor.mishgorbonded_rec.Add(new mishgorbonded_rec());
                Mishgor.mishgorbonded_rec[0].mishgorbonded_recid = consignment.ReceiverWarehouseCode; //11
                // moran 19.8.15 - task 13902 -->
                //Mishgor.mishgorbonded_rec[0].mishgorbonded_recname = consignment.ReceiverWarehouseName; //11
                RegisteredWarehouseSiteTypeQueryService receiverWarehouseQuery = new RegisteredWarehouseSiteTypeQueryService(context);
                RegisteredWarehouseSiteTypePM receiverWarehouse = receiverWarehouseQuery.GetSingle(consignment.ReceiverWarehouseCode, true, false);
                if (receiverWarehouse != null)
                {
                    if (!String.IsNullOrWhiteSpace(receiverWarehouse.LocalName))
                    {
                        Mishgor.mishgorbonded_rec[0].mishgorbonded_recname = receiverWarehouse.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(receiverWarehouse.EnglishName))
                    {
                        Mishgor.mishgorbonded_rec[0].mishgorbonded_recname = receiverWarehouse.EnglishName;
                    }
                }
                // moran 19.8.15 - task 13902 <--
                Mishgor.mishgorpartiality = new List<mishgorpartiality>(); Mishgor.mishgorpartiality.Add(new mishgorpartiality());
                // moran 19.8.15 - task 13902 -->
                //Mishgor.mishgorpartiality[0].mishgorpartialityid = consignment.IsLastReleaseFromWarehous;  //12
                LastReleaseFromWarehouseQueryService lastReleaseFromWarehouseQuery = new LastReleaseFromWarehouseQueryService(context);
                LastReleaseFromWarehousePM lastReleaseFromWarehouse = lastReleaseFromWarehouseQuery.GetSingle(consignment.IsLastReleaseFromWarehous, true, false);
                if (lastReleaseFromWarehouse != null)
                {
                    if (!String.IsNullOrWhiteSpace(lastReleaseFromWarehouse.LocalName))
                    {
                        Mishgor.mishgorpartiality[0].mishgorpartialityid = lastReleaseFromWarehouse.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(lastReleaseFromWarehouse.EnglishName))
                    {
                        Mishgor.mishgorpartiality[0].mishgorpartialityid = lastReleaseFromWarehouse.EnglishName;
                    }
                }
                // moran 19.8.15 - task 13902 <--
                Mishgor.mishgordesc_of_goods1 = consignment.CargoDescription; //15
                //Mishgor.mishgorweight = consignment.ConsignmentPackages[0].
                if (consignment.ConsignmentInternalTransitions != null && consignment.ConsignmentInternalTransitions.Count > 0)
                {
                    InternalBorderSiteTypeQueryService internalBorderSiteTypeQueryService = new InternalBorderSiteTypeQueryService(context);
                    InternalBorderSiteTypePM internalBorderSiteTypePM = new Def.EntityPMs.InternalBorderSiteTypePM();

                    if (!string.IsNullOrWhiteSpace(consignment.ConsignmentInternalTransitions[0].SiteCode))
                    {
                        Mishgor.internaltransitions1 = consignment.ConsignmentInternalTransitions[0].SiteCode; //13
                        internalBorderSiteTypePM = internalBorderSiteTypeQueryService.GetSingle(consignment.ConsignmentInternalTransitions[0].SiteCode, false, true);
                        if (internalBorderSiteTypePM != null)
                        {
                            Mishgor.internaltransitionsName1 = internalBorderSiteTypePM.LocalName;
                        }
                    }
                    if (consignment.ConsignmentInternalTransitions.Count > 1)
                    {
                        if (!string.IsNullOrWhiteSpace(consignment.ConsignmentInternalTransitions[1].SiteCode))
                        {
                            Mishgor.internaltransitions2 = consignment.ConsignmentInternalTransitions[1].SiteCode; //14
                            internalBorderSiteTypePM = internalBorderSiteTypeQueryService.GetSingle(consignment.ConsignmentInternalTransitions[1].SiteCode, false, true);
                            if (internalBorderSiteTypePM != null)
                            {
                                Mishgor.internaltransitionsName2 = internalBorderSiteTypePM.LocalName;
                            }
                        }
                    }
                }


                Mishgor.mishgornumber = consignment.SequenceNumeric.ToString();
                var mishgorPackageList = new List<mishgorPackage>();
                for (int consignmentPackageSeq = 0; consignmentPackageSeq < consignment.ConsignmentPackages.Count(); consignmentPackageSeq++)
                {
                    var consignmentPackagePM = consignment.ConsignmentPackages[consignmentPackageSeq];
                    var mishgorpackage = new mishgorPackage();
                    mishgorpackage.PackageQuantityMeasure = consignmentPackagePM.PackageMeasureQualifierName; // PackageMeasureQualifierCode; //1 moran 2.8.15 - task 13902 - Name instead of Code
                    if (consignmentPackagePM.PackageQuantity.HasValue)
                    {
                        mishgorpackage.PackageQuantity = consignmentPackagePM.PackageQuantity.Value;  //3
                    }
                    if (consignmentPackagePM.GrossMassMeasure.HasValue)
                    {
                        mishgorpackage.PackageWeight = Convert.ToDouble(consignmentPackagePM.GrossMassMeasure.Value); //4
                    }

                    mishgorpackage.PackageTypeCode = consignmentPackagePM.PackageTypeCode; //2
                    // moran 19.8.15 - task 13902 -->
                    string packtypename = null;
                    PackingTypeQueryService packingTypeQuery = new PackingTypeQueryService(context);
                    PackingTypePM packingType = packingTypeQuery.GetSingle(consignmentPackagePM.PackageTypeCode, true, false);
                    if (packingType != null)
                    {
                        if (!String.IsNullOrWhiteSpace(packingType.LocalName))
                        {
                            packtypename = packingType.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(packingType.EnglishName))
                        {
                            packtypename = packingType.EnglishName;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(packtypename))
                    {
                        mishgorpackage.PackageTypeCode = mishgorpackage.PackageTypeCode + " - " + packtypename;
                    }
                    // moran 19.8.15 - task 13902 <--
                    mishgorpackage.MarksNumbers = consignmentPackagePM.MarksNumbers; //5
                    mishgorPackageList.Add(mishgorpackage);
                }
                Mishgor.mishgorPackage = mishgorPackageList;

                mishgorList.Add(Mishgor);
            }

            return mishgorList;

        }

        private List<acc_supplier> GetAccSupplier(DeclarationPM declarationPM)
        {
            //var context = CustomContext.GetContext(declarationPM.Tenant); // moran 2.8.15 - task 13902
            if (context == null) context = CustomContext.GetContext(declarationPM.Tenant); // moran 10.9.15 - Task 16086
            var acc_supplierList = new List<acc_supplier>();

            foreach (var supplierInvoice in declarationPM.SupplierInvoices)
            {
                var Acc_supplier = new acc_supplier();
                Acc_supplier.acc_supplierline = supplierInvoice.SequenceNumeric.ToString();
                Acc_supplier.acc_suppliersup_account = supplierInvoice.InvoiceNumber;
                Acc_supplier.acc_suppliersupplier = new List<acc_suppliersupplier>(); Acc_supplier.acc_suppliersupplier.Add(new acc_suppliersupplier());
                // moran 2.8.15 - task 13902 -->
                //Acc_supplier.acc_suppliersupplier[0].acc_suppliersupplierid = supplierInvoice.VendorId;
                Acc_supplier.acc_suppliersupplier[0].acc_suppliersuppliername = supplierInvoice.VendorName;
                CustomsVendorQueryService vendorQuery = new CustomsVendorQueryService(context);
                CustomsVendorPM vendor = vendorQuery.GetSingle(supplierInvoice.VendorId, true, false);
                if (vendor != null) // moran 16.8.15 - task 13902 - enter into 'if'
                {
                    Acc_supplier.acc_suppliersupplier[0].acc_suppliersupplierid = vendor.VendorNumber;
                    if (String.IsNullOrWhiteSpace(supplierInvoice.VendorName))
                    {
                        Acc_supplier.acc_suppliersupplier[0].acc_suppliersuppliername = vendor.VendorName;
                    }
                }
                // moran 2.8.15 - task 13902 <--

                Acc_supplier.acc_supplieracc_type = new List<acc_supplieracc_type>(); Acc_supplier.acc_supplieracc_type.Add(new acc_supplieracc_type());
                Acc_supplier.acc_supplieracc_type[0].acc_supplieracc_typeid = supplierInvoice.AccountTypeCode;
                // moran 19.8.15 - task 13902 -->
                InvoiceTypeQueryService invoiceTypeQuery = new InvoiceTypeQueryService(context);
                InvoiceTypePM invoiceType = invoiceTypeQuery.GetSingle(supplierInvoice.AccountTypeCode, true, false);
                if (invoiceType != null)
                {
                    if (!String.IsNullOrWhiteSpace(invoiceType.LocalName))
                    {
                        Acc_supplier.acc_supplieracc_type[0].acc_supplieracc_typename = invoiceType.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(invoiceType.EnglishName))
                    {
                        Acc_supplier.acc_supplieracc_type[0].acc_supplieracc_typename = invoiceType.EnglishName;
                    }
                }
                // moran 19.8.15 - task 13902 <--
                Acc_supplier.acc_suppliercountry = new List<acc_suppliercountry>(); Acc_supplier.acc_suppliercountry.Add(new acc_suppliercountry());
                Acc_supplier.acc_suppliercountry[0].acc_suppliercountryid = supplierInvoice.IssueCountryCode;
                Acc_supplier.acc_suppliercountry[0].acc_suppliercountryname = supplierInvoice.IssueCountryName;

                Acc_supplier.acc_supplierincoterm = new List<acc_supplierincoterm>(); Acc_supplier.acc_supplierincoterm.Add(new acc_supplierincoterm());
                Acc_supplier.acc_supplierincoterm[0].acc_supplierincotermid = supplierInvoice.IncotermCode;
                if (supplierInvoice.InvoiceAmount.HasValue)
                {
                    Acc_supplier.acc_suppliervalue = supplierInvoice.InvoiceAmount.Value.ToString();
                    Acc_supplier.acc_suppliercurrency = new List<acc_suppliercurrency>(); Acc_supplier.acc_suppliercurrency.Add(new acc_suppliercurrency());
                    Acc_supplier.acc_suppliercurrency[0].acc_suppliercurrencyid = supplierInvoice.InvoiceCurrencyTypeCode;
                    CurrencyRepository CurrRep = new CurrencyRepository(declarationPM.Tenant);
                    Currency InvCurr = CurrRep.GetSingleCurrencyByCode(supplierInvoice.InvoiceCurrencyTypeCode, declarationPM.Tenant);
                    if (InvCurr != null) // moran 16.8.15 - task 13902 - enter into 'if'
                    {
                        Acc_supplier.acc_suppliercurrency[0].acc_suppliercurrencyname = InvCurr.LocalName;
                    }
                    if (amounts == null || amounts.Where(d => d.currency == supplierInvoice.InvoiceCurrencyTypeCode).FirstOrDefault() == null)
                    {
                        if (amounts == null) amounts = new List<CurrencyAmount>();
                        var currencyAmount = new CurrencyAmount();
                        currencyAmount.currency = supplierInvoice.InvoiceCurrencyTypeCode;
                        currencyAmount.amount = supplierInvoice.InvoiceAmount.Value.ToString();
                        amounts.Add(currencyAmount);
                    }
                    else
                    {
                        decimal tempAmount;
                        if(decimal.TryParse(amounts.Where(d => d.currency == supplierInvoice.InvoiceCurrencyTypeCode).FirstOrDefault().amount, out tempAmount))
                        {
                            tempAmount += supplierInvoice.InvoiceAmount.Value;
                            amounts.Where(d => d.currency == supplierInvoice.InvoiceCurrencyTypeCode).FirstOrDefault().amount = tempAmount.ToString();
                        }
                    }
                }
                if (supplierInvoice.InsruancePercentage.HasValue)
                {
                    Acc_supplier.Acc_supplierinsurancepercent = Convert.ToDouble(supplierInvoice.InsruancePercentage);
                }
                if (supplierInvoice.InsuranceAmount.HasValue)
                {
                    Acc_supplier.Acc_supplierinsurance = Convert.ToDouble(supplierInvoice.InsuranceAmount);
                }
                Acc_supplier.Acc_supplierinsurancecurr = supplierInvoice.InsruanceCurrencyTypeCode;
                Acc_supplier.Acc_suppliertariff = supplierInvoice.PreferenceDocumentTypeCode;
                if (_OpenAccessFields && !string.IsNullOrWhiteSpace(supplierInvoice.PreferenceDocumentTypeCode))
                {
                    TradeAgreementQueryService tradeAgreementQueryService = new TradeAgreementQueryService(context);
                    TradeAgreementPM tradeAgreementPM = tradeAgreementQueryService.GetSingle(supplierInvoice.PreferenceDocumentTypeCode, true, false);
                    if (tradeAgreementPM != null)
                    {
                        if (!String.IsNullOrWhiteSpace(tradeAgreementPM.LocalName))
                        {
                            Acc_supplier.Acc_suppliertariff = Acc_supplier.Acc_suppliertariff + " - " + tradeAgreementPM.LocalName;
                        }
                    }
                }
                Acc_supplier.Acc_supplierpreference = supplierInvoice.IsPreference;
                Acc_supplier.Acc_supplierfreight = Convert.ToDouble(supplierInvoice.TotalFreightInFreightCurrency); // moran 26.8.15 - task 16093
                Acc_supplier.Acc_supplierfreightcurr = supplierInvoice.FreightCurrencyTypeCode; // moran 26.8.15 - task 16093
                Acc_supplier.Acc_suppliercurrencyrate = supplierInvoice.ExchangeRate.GetValueOrDefault().ToString(); // moran 26.8.15 - task 16093
                Acc_supplier.Acc_supplierVendorComissionPercentage = supplierInvoice.VendorComissionPercentage; // moran 24.12.17 - AMI-62725
                Acc_supplier.acc_supplierActualPayedAmount = Convert.ToDouble(supplierInvoice.ActualPayedAmount);
                Acc_supplier.acc_supplierActualPayedCurrencyTypeCode = supplierInvoice.ActualPayedCurrencyTypeCode;
                Acc_supplier.acc_supplierActualPayedCurrencyTypeName = supplierInvoice.ActualPayedCurrencyTypeName;
                var supplierInvoiceModificationList = new List<SupplierInvoiceModificationM>();
                foreach (var supplierInvoiceModificationPM in supplierInvoice.SupplierInvoiceModifications)
                {
                    if (supplierInvoiceModificationPM.TypeCode != "I02")
                    {
                        var supplierInvoiceModification = new SupplierInvoiceModificationM();

                        supplierInvoiceModification.ModificationAndDiscountTypeLocalName = supplierInvoiceModificationPM.TypeName;
                        supplierInvoiceModification.ModificationAndDiscountTypeCode = supplierInvoiceModificationPM.TypeCode;
                        supplierInvoiceModification.CurrencyTypeCode = supplierInvoiceModificationPM.CurrencyTypeCode;
                        supplierInvoiceModification.Amount = Convert.ToDouble(supplierInvoiceModificationPM.Amount);
                        supplierInvoiceModificationList.Add(supplierInvoiceModification);
                    }
                }
                if (supplierInvoiceModificationList != null)
                {
                    Acc_supplier.Acc_suppliermodification = supplierInvoiceModificationList;
                }
                Acc_supplier.acc_suppliersup_items = GetSupplierInvoiceItems(supplierInvoice);

                Acc_supplier.isAccumalated = supplierInvoice.IsAccumalated;

                acc_supplierList.Add(Acc_supplier);
            }

            return acc_supplierList;

        }

        private List<tax_details> GetDeclarationTaxDetails(DeclarationPM declarationPM)
        {
            if (context == null) context = CustomContext.GetContext(declarationPM.Tenant); // moran 10.9.15 - Task 16086

            var declarationTaxList = new List<tax_details>();

            foreach (var supplierInvoice in declarationPM.SupplierInvoices)
            {
                foreach (var suppliersup_item in supplierInvoice.SupplierInvoiceItems)
                {
                    foreach (var tax in suppliersup_item.SupplierInvoiceItemTaxes)
                    {
                        var declarationTax = new tax_details();

                        declarationTax.tax_detailsline = tax.LineNumber.ToString();
                        declarationTax.tax_detailstaxtype = new List<tax_detailstaxtype>(); declarationTax.tax_detailstaxtype.Add(new tax_detailstaxtype());
                        declarationTax.tax_detailstaxtype[0].tax_detailstaxtypeid = tax.TaxTypeCode;
                        // moran 10.9.15 - Task 16086 -->
                        //declarationTax.tax_detailstaxtype[0].tax_detailstaxtypename = tax.TaxTypeName;
                        ParagraphTypeQueryService ParagraphTypeQuery = new ParagraphTypeQueryService(context);
                        ParagraphTypePM ParagraphType = ParagraphTypeQuery.GetSingle(tax.TaxTypeCode, true, false);
                        if (ParagraphType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(ParagraphType.LocalName))
                            {
                                declarationTax.tax_detailstaxtype[0].tax_detailstaxtypename = ParagraphType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(ParagraphType.EnglishName))
                            {
                                declarationTax.tax_detailstaxtype[0].tax_detailstaxtypename = ParagraphType.EnglishName;
                            }
                        }
                        // moran 10.9.15 - Task 16086 <--

                        if (tax.TaxBaseAmount.HasValue)
                        {
                            declarationTax.tax_detailstax_basis = tax.TaxBaseAmount.ToString();
                        }
                        if (tax.TaxAmount.HasValue)
                        {
                            declarationTax.tax_detailstax_amount = tax.TaxAmount.ToString();
                            declarationTax.tax_detailstax_to_pay = tax.TaxAmount.ToString();
                        }
                        if (tax.DeferedTaxAmount.HasValue)
                        {
                            declarationTax.tax_detailspostponed_tax = tax.DeferedTaxAmount.ToString();
                        }
                        if (tax.TaxRate.HasValue)
                        {
                            declarationTax.tax_detailstax_rate = tax.TaxRate.ToString();
                        }
                        if (tax.DefinedPerUnitMeasure.HasValue)
                        {
                            declarationTax.tax_detailstax_DefinedPerUnitMeasure = tax.DefinedPerUnitMeasure.ToString();
                        }
                        if (tax.AlternateRate.HasValue)
                        {
                            declarationTax.tax_detailstax_AlternateRate = tax.AlternateRate.ToString();
                        }
                        if (tax.AlternateDefinedPerUnitMeasure.HasValue)
                        {
                            declarationTax.tax_detailstax_AlternateDefinedPerUnitMeasure = tax.AlternateDefinedPerUnitMeasure.ToString();
                        }
                        declarationTax.tax_detailsprat_mehes = suppliersup_item.ClassificationCode; // moran 26.8.15 - task 16093
                        declarationTax.tax_detailssuppliersup_item_counterKey = suppliersup_item.CounterKey.ToString();
                        declarationTaxList.Add(declarationTax);
                    }

                }
            }

            return declarationTaxList;
        }

        private List<acc_suppliersup_items> GetSupplierInvoiceItems(SupplierInvoicePM supplierInvoice)
        {
            var acc_suppliersup_itemList = new List<acc_suppliersup_items>();

            List<SupplierInvoiceItemPM> SupplierInvoiceItemParentListPM = new List<SupplierInvoiceItemPM>();
            if (supplierInvoice.IsAccumalated)
            {
                SupplierInvoiceItemParentListPM = supplierInvoice.SupplierInvoiceItems.Where(d => d.IsParent == true).ToList();
                foreach (var SupplierInvoiceItemParentPM in SupplierInvoiceItemParentListPM)
                {
                    acc_suppliersup_itemList.Add(BuildSupplierInvoiceItem(supplierInvoice, SupplierInvoiceItemParentPM));
                    List<SupplierInvoiceItemPM> SupplierInvoiceItemChildListPM = new List<SupplierInvoiceItemPM>();
                    SupplierInvoiceItemChildListPM = supplierInvoice.SupplierInvoiceItems.Where(d => d.IsParent == false && d.ParentLineNumber == SupplierInvoiceItemParentPM.LineNumber).ToList();
                    foreach (var supplierInvoiceItemChildPM in SupplierInvoiceItemChildListPM)
                    {
                        acc_suppliersup_itemList.Add(BuildSupplierInvoiceItem(supplierInvoice, supplierInvoiceItemChildPM));
                    }
                }
            }
            else
            {
                foreach (var supplierInvoiceItem in supplierInvoice.SupplierInvoiceItems)
                {
                    acc_suppliersup_itemList.Add(BuildSupplierInvoiceItem(supplierInvoice, supplierInvoiceItem));
                }
            }

            return acc_suppliersup_itemList;

        }

        private acc_suppliersup_items BuildSupplierInvoiceItem(SupplierInvoicePM supplierInvoice, SupplierInvoiceItemPM supplierInvoiceItem)
        {

            var acc_suppliersup_item = new acc_suppliersup_items();

            acc_suppliersup_item.isParent = supplierInvoiceItem.IsParent;
            acc_suppliersup_item.parentLineNumber = supplierInvoiceItem.ParentLineNumber;

            acc_suppliersup_item.ACC_LINE_NOCCUSUPITEMSAMITAL = supplierInvoice.SequenceNumeric.ToString(); // suppliersup_item.LineNumber.ToString();
            acc_suppliersup_item.acc_suppliersup_itemsprat_mehs = supplierInvoiceItem.ClassificationCode;
            acc_suppliersup_item.acc_suppliersup_itemstariff = supplierInvoiceItem.TradeAgreementCode;
            acc_suppliersup_item.acc_suppliersup_itemstariff_name = supplierInvoiceItem.TradeAgreementName;
            if (supplierInvoiceItem.StatisticQuantity.HasValue)
            {
                acc_suppliersup_item.acc_suppliersup_itemsquantity_val = Convert.ToDouble(supplierInvoiceItem.StatisticQuantity);
                acc_suppliersup_item.acc_suppliersup_itemsquantity = supplierInvoiceItem.StatisticQuantityType;
            }
            if (supplierInvoiceItem.InvoiceQuantity.HasValue)
            {
                acc_suppliersup_item.acc_suppliersup_itemsinvquantity_val = Convert.ToDouble(supplierInvoiceItem.InvoiceQuantity);
                acc_suppliersup_item.acc_suppliersup_itemsinvquantity_type = supplierInvoiceItem.InvoiceQuantityType;
            }

            if (_OpenAccessFields)
            {
                acc_suppliersup_item.acc_suppliersup_itemsnidh_mas = string.Format("{0:0,0.00}", supplierInvoiceItem.DeferredPurchaseTax);
                acc_suppliersup_item.acc_suppliersup_itemsnidh_mhs = string.Format("{0:0,0.00}", supplierInvoiceItem.DeferredCustomsTax);
            }


            if (supplierInvoiceItem.ItemPrice.HasValue)
            {
                acc_suppliersup_item.acc_suppliersup_itemsfc_val = supplierInvoiceItem.ItemPrice.ToString();
            }

            acc_suppliersup_item.acc_suppliersup_itemsorig_cnty = supplierInvoiceItem.OriginCountryCode;
            acc_suppliersup_item.acc_suppliersup_itemsorig_cnty_name = supplierInvoiceItem.OriginCountryName;
            acc_suppliersup_item.acc_suppliersup_itemsPreferenceDocument = supplierInvoiceItem.PreferenceDocumentNumber;

            // moran 4.5.16 - task 20319 -->
            string supplierInvoiceItemProcesTypes = "";
            if (supplierInvoiceItem.SupplierInvoiceItemProcesTypes != null && supplierInvoiceItem.SupplierInvoiceItemProcesTypes.Count() > 0)
            {
                foreach (var supplierInvoiceItemProcesTypePM in supplierInvoiceItem.SupplierInvoiceItemProcesTypes.Where(d => d.DeclarationId == supplierInvoiceItem.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItem.CounterKey && d.InvoiceItemLineNumber == supplierInvoiceItem.LineNumber).ToList())
                {
                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemProcesTypePM.ProcessTypeCode))
                    {
                        SupplierInvoiceItemProcesTypeQueryService SupplierInvoiceItemProcesTypeQuery = new SupplierInvoiceItemProcesTypeQueryService(context);
                        SupplierInvoiceItemProcesTypePM SupplierInvoiceItemProcesType = SupplierInvoiceItemProcesTypeQuery.GetSingle(supplierInvoiceItemProcesTypePM.DeclarationId, supplierInvoiceItemProcesTypePM.InvoiceCounterKey, supplierInvoiceItemProcesTypePM.InvoiceItemLineNumber, supplierInvoiceItemProcesTypePM.LineNumber, true, false);
                        if (SupplierInvoiceItemProcesType != null)
                        {
                            if (String.IsNullOrWhiteSpace(supplierInvoiceItemProcesTypes))
                            {
                                supplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypePM.ProcessTypeCode + " - " + SupplierInvoiceItemProcesType.ProcessTypeName;
                            }
                            else
                            {
                                supplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypes + ", " + supplierInvoiceItemProcesTypePM.ProcessTypeCode + " - " + SupplierInvoiceItemProcesType.ProcessTypeName;
                            }
                        }
                        else
                        {
                            if (String.IsNullOrWhiteSpace(supplierInvoiceItemProcesTypes))
                            {
                                supplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypePM.ProcessTypeCode;
                            }
                            else
                            {
                                supplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypes + ", " + supplierInvoiceItemProcesTypePM.ProcessTypeCode;
                            }
                        }
                    }
                }
            }

            acc_suppliersup_item.SupplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypes;
            if (supplierInvoiceItem.StatisticQuantity.HasValue)
            {
                acc_suppliersup_item.StatisticQuantity = supplierInvoiceItem.StatisticQuantity.Value.ToString();
                if (!String.IsNullOrWhiteSpace(supplierInvoiceItem.StatisticQuantityType))
                {
                    acc_suppliersup_item.StatisticQuantityType = supplierInvoiceItem.StatisticQuantityType;
                    MeasurmentUnitQueryService MeasurmentUnitQuery = new MeasurmentUnitQueryService(context);
                    MeasurmentUnitPM MeasurmentUnit = MeasurmentUnitQuery.GetSingle(supplierInvoiceItem.StatisticQuantityType, true, false);
                    if (MeasurmentUnit != null)
                    {
                        if (!String.IsNullOrWhiteSpace(MeasurmentUnit.LocalName))
                        {
                            acc_suppliersup_item.StatisticQuantityTypeName = MeasurmentUnit.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(MeasurmentUnit.EnglishName))
                        {
                            acc_suppliersup_item.StatisticQuantityTypeName = MeasurmentUnit.EnglishName;
                        }
                    }
                }
            }
            if (supplierInvoiceItem.AdditionalQuantity.HasValue)
            {
                acc_suppliersup_item.AddQuantity = supplierInvoiceItem.AdditionalQuantity.Value.ToString();
                if (!String.IsNullOrWhiteSpace(supplierInvoiceItem.AdditionalQuantityType))
                {
                    acc_suppliersup_item.AddQuantityType = supplierInvoiceItem.AdditionalQuantityType;
                    MeasurmentUnitQueryService MeasurmentUnitQuery = new MeasurmentUnitQueryService(context);
                    MeasurmentUnitPM MeasurmentUnit = MeasurmentUnitQuery.GetSingle(supplierInvoiceItem.AdditionalQuantityType, true, false);
                    if (MeasurmentUnit != null)
                    {
                        if (!String.IsNullOrWhiteSpace(MeasurmentUnit.LocalName))
                        {
                            acc_suppliersup_item.AddQuantityTypeName = MeasurmentUnit.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(MeasurmentUnit.EnglishName))
                        {
                            acc_suppliersup_item.AddQuantityTypeName = MeasurmentUnit.EnglishName;
                        }
                    }
                }
            }
            acc_suppliersup_item.PreferenceDocumentNumber = supplierInvoiceItem.PreferenceDocumentNumber;
            acc_suppliersup_item.TaxExemptCode = supplierInvoiceItem.TaxExemptCode;
            if (!String.IsNullOrWhiteSpace(supplierInvoiceItem.CustomsBookTypeCode))
            {
                acc_suppliersup_item.CustomsBookTypeCode = supplierInvoiceItem.CustomsBookTypeCode;
                CustomsBookTypeQueryService CustomsBookTypeQuery = new CustomsBookTypeQueryService(context);
                CustomsBookTypePM CustomsBookType = CustomsBookTypeQuery.GetSingle(supplierInvoiceItem.CustomsBookTypeCode, true, false);
                if (CustomsBookType != null)
                {
                    if (!String.IsNullOrWhiteSpace(CustomsBookType.LocalName))
                    {
                        acc_suppliersup_item.CustomsBookTypeCodeName = CustomsBookType.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(CustomsBookType.EnglishName))
                    {
                        acc_suppliersup_item.CustomsBookTypeCodeName = CustomsBookType.EnglishName;
                    }
                }
            }
            acc_suppliersup_item.DangerousClassificationCode = supplierInvoiceItem.DangerousClassificationCode;
            if (supplierInvoiceItem.NonCustomsItemPrice.HasValue)
            {
                acc_suppliersup_item.NonCustomsItemPrice = supplierInvoiceItem.NonCustomsItemPrice.Value.ToString();
                if (!String.IsNullOrWhiteSpace(supplierInvoiceItem.NonCustomsItemPriceCurCode))
                {
                    acc_suppliersup_item.NonCustomsItemPriceCurCode = supplierInvoiceItem.NonCustomsItemPriceCurCode;
                    CurrencyTypeQueryService CurrencyTypeQuery = new CurrencyTypeQueryService(context);
                    CurrencyTypePM CurrencyType = CurrencyTypeQuery.GetSingle(supplierInvoiceItem.NonCustomsItemPriceCurCode, true, false);
                    if (CurrencyType != null)
                    {
                        if (!String.IsNullOrWhiteSpace(CurrencyType.LocalName))
                        {
                            acc_suppliersup_item.NonCustomsItemPriceCurCodeName = CurrencyType.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(CurrencyType.EnglishName))
                        {
                            acc_suppliersup_item.NonCustomsItemPriceCurCodeName = CurrencyType.EnglishName;
                        }
                    }
                }
            }
            if (supplierInvoiceItem.WholeSaleItemPrice.HasValue)
            {
                acc_suppliersup_item.WholeSaleItemPrice = supplierInvoiceItem.WholeSaleItemPrice.Value.ToString();
                if (!String.IsNullOrWhiteSpace(supplierInvoiceItem.WholeSaleItemPriceCurrencyCode))
                {
                    acc_suppliersup_item.WholeSaleItemPriceCurrencyCode = supplierInvoiceItem.WholeSaleItemPriceCurrencyCode;
                    CurrencyTypeQueryService CurrencyTypeQuery = new CurrencyTypeQueryService(context);
                    CurrencyTypePM CurrencyType = CurrencyTypeQuery.GetSingle(supplierInvoiceItem.WholeSaleItemPriceCurrencyCode, true, false);
                    if (CurrencyType != null)
                    {
                        if (!String.IsNullOrWhiteSpace(CurrencyType.LocalName))
                        {
                            acc_suppliersup_item.WholeSaleItemPriceCurrencyCode = CurrencyType.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(CurrencyType.EnglishName))
                        {
                            acc_suppliersup_item.WholeSaleItemPriceCurrencyCode = CurrencyType.EnglishName;
                        }
                    }
                }
            }
            acc_suppliersup_item.ManufactureIdentifier = supplierInvoiceItem.ManufactureIdentifier;
            if (!String.IsNullOrWhiteSpace(supplierInvoiceItem.SalesTaxExemptionTypeCode))
            {
                acc_suppliersup_item.SalesTaxExemptionTypeCode = supplierInvoiceItem.SalesTaxExemptionTypeCode;
                SalesTaxExemptionTypeQueryService SalesTaxExemptionTypeQuery = new SalesTaxExemptionTypeQueryService(context);
                SalesTaxExemptionTypePM SalesTaxExemptionType = SalesTaxExemptionTypeQuery.GetSingle(supplierInvoiceItem.SalesTaxExemptionTypeCode, true, false);
                if (SalesTaxExemptionType != null)
                {
                    if (!String.IsNullOrWhiteSpace(SalesTaxExemptionType.LocalName))
                    {
                        acc_suppliersup_item.SalesTaxExemptionTypeName = SalesTaxExemptionType.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(SalesTaxExemptionType.EnglishName))
                    {
                        acc_suppliersup_item.SalesTaxExemptionTypeName = SalesTaxExemptionType.EnglishName;
                    }
                }
            }
            if (supplierInvoiceItem.OptionalTamaPercentage.HasValue) acc_suppliersup_item.OptionalTamaPercentage = supplierInvoiceItem.OptionalTamaPercentage.ToString();

            acc_suppliersup_item.Acc_supplieritemAdds = GetSupplierInvoiceItemAdds(acc_suppliersup_item);

            acc_suppliersup_item.Acc_supplieritemscertificate = GetSupplierInvoiceItemCertificates(supplierInvoiceItem);
            acc_suppliersup_item.Acc_supplieritemsConDeclars = GetSupplierInvoiceItemConDeclars(supplierInvoiceItem);
            acc_suppliersup_item.Acc_supplieritemsMods = GetSupplierInvoiceItemMods(supplierInvoiceItem);
            acc_suppliersup_item.Acc_supplieritemsLevies = GetSupplierInvoiceItemLevies(supplierInvoiceItem);
            acc_suppliersup_item.Acc_supplieritemsProdIdents = GetSupplierInvoiceItemProdIdents(supplierInvoiceItem); 
            acc_suppliersup_item.Acc_supplieritemsSerialNums = GetSupplierInvoiceItemSerialNums(supplierInvoiceItem);
            acc_suppliersup_item.Acc_supplieritemsDescripts = GetSupplierInvoiceItemDescripts(supplierInvoiceItem);   
            acc_suppliersup_item.Acc_supplieritemsVehicles = GetSupplierInvoiceItemVehicles(supplierInvoiceItem);
            acc_suppliersup_item.Acc_supplieritemsVehicleAndMods = GetSupplierInvoiceItemVehicleMods(supplierInvoiceItem); 

            return acc_suppliersup_item;
        }

        //private List<acc_suppliersup_items> GetSupplierInvoiceItems_OLD(SupplierInvoicePM supplierInvoice)
        //{
        //    var acc_suppliersup_itemList = new List<acc_suppliersup_items>();

        //    foreach (var suppliersup_item in supplierInvoice.SupplierInvoiceItems)
        //    {
        //        var acc_suppliersup_item = new acc_suppliersup_items();
        //        acc_suppliersup_item.ACC_LINE_NOCCUSUPITEMSAMITAL = supplierInvoice.SequenceNumeric.ToString(); // suppliersup_item.LineNumber.ToString();
        //        acc_suppliersup_item.acc_suppliersup_itemsprat_mehs = suppliersup_item.ClassificationCode;
        //        acc_suppliersup_item.acc_suppliersup_itemstariff = suppliersup_item.TradeAgreementCode;
        //        acc_suppliersup_item.acc_suppliersup_itemstariff_name = suppliersup_item.TradeAgreementName;
        //        if (suppliersup_item.StatisticQuantity.HasValue)
        //        {
        //            acc_suppliersup_item.acc_suppliersup_itemsquantity_val = Convert.ToDouble(suppliersup_item.StatisticQuantity);
        //            acc_suppliersup_item.acc_suppliersup_itemsquantity = suppliersup_item.StatisticQuantityType;
        //        }
        //        if (suppliersup_item.InvoiceQuantity.HasValue)
        //        {
        //            acc_suppliersup_item.acc_suppliersup_itemsinvquantity_val = Convert.ToDouble(suppliersup_item.InvoiceQuantity);
        //            acc_suppliersup_item.acc_suppliersup_itemsinvquantity_type = suppliersup_item.InvoiceQuantityType;
        //        }

        //        if (_OpenAccessFields)
        //        {
        //            acc_suppliersup_item.acc_suppliersup_itemsnidh_mas = string.Format("{0:0,0.00}", suppliersup_item.DeferredPurchaseTax);
        //            acc_suppliersup_item.acc_suppliersup_itemsnidh_mhs = string.Format("{0:0,0.00}", suppliersup_item.DeferredCustomsTax);
        //        }


        //        if (suppliersup_item.ItemPrice.HasValue)
        //        {
        //            acc_suppliersup_item.acc_suppliersup_itemsfc_val = suppliersup_item.ItemPrice.ToString();
        //        }

        //        acc_suppliersup_item.acc_suppliersup_itemsorig_cnty = suppliersup_item.OriginCountryCode;
        //        acc_suppliersup_item.acc_suppliersup_itemsorig_cnty_name = suppliersup_item.OriginCountryName;
        //        acc_suppliersup_item.acc_suppliersup_itemsPreferenceDocument = suppliersup_item.PreferenceDocumentNumber;

        //        // moran 4.5.16 - task 20319 -->
        //        string supplierInvoiceItemProcesTypes = "";
        //        if (suppliersup_item.SupplierInvoiceItemProcesTypes != null && suppliersup_item.SupplierInvoiceItemProcesTypes.Count() > 0)
        //        {
        //            foreach (var supplierInvoiceItemProcesTypePM in suppliersup_item.SupplierInvoiceItemProcesTypes.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.InvoiceItemLineNumber == suppliersup_item.LineNumber).ToList())
        //            {
        //                if (!String.IsNullOrWhiteSpace(supplierInvoiceItemProcesTypePM.ProcessTypeCode))
        //                {
        //                    SupplierInvoiceItemProcesTypeQueryService SupplierInvoiceItemProcesTypeQuery = new SupplierInvoiceItemProcesTypeQueryService(context);
        //                    SupplierInvoiceItemProcesTypePM SupplierInvoiceItemProcesType = SupplierInvoiceItemProcesTypeQuery.GetSingle(supplierInvoiceItemProcesTypePM.DeclarationId, supplierInvoiceItemProcesTypePM.InvoiceCounterKey, supplierInvoiceItemProcesTypePM.InvoiceItemLineNumber, supplierInvoiceItemProcesTypePM.LineNumber, true, false);
        //                    if (SupplierInvoiceItemProcesType != null)
        //                    {
        //                        if (String.IsNullOrWhiteSpace(supplierInvoiceItemProcesTypes))
        //                        {
        //                            supplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypePM.ProcessTypeCode + " - " + SupplierInvoiceItemProcesType.ProcessTypeName;
        //                        }
        //                        else
        //                        {
        //                            supplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypes + ", " + supplierInvoiceItemProcesTypePM.ProcessTypeCode + " - " + SupplierInvoiceItemProcesType.ProcessTypeName;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (String.IsNullOrWhiteSpace(supplierInvoiceItemProcesTypes))
        //                        {
        //                            supplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypePM.ProcessTypeCode;
        //                        }
        //                        else
        //                        {
        //                            supplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypes + ", " + supplierInvoiceItemProcesTypePM.ProcessTypeCode;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        acc_suppliersup_item.SupplierInvoiceItemProcesTypes = supplierInvoiceItemProcesTypes;
        //        if (suppliersup_item.StatisticQuantity.HasValue)
        //        {
        //            acc_suppliersup_item.StatisticQuantity = suppliersup_item.StatisticQuantity.Value.ToString();
        //            if (!String.IsNullOrWhiteSpace(suppliersup_item.StatisticQuantityType))
        //            {
        //                acc_suppliersup_item.StatisticQuantityType = suppliersup_item.StatisticQuantityType;
        //                MeasurmentUnitQueryService MeasurmentUnitQuery = new MeasurmentUnitQueryService(context);
        //                MeasurmentUnitPM MeasurmentUnit = MeasurmentUnitQuery.GetSingle(suppliersup_item.StatisticQuantityType, true, false);
        //                if (MeasurmentUnit != null)
        //                {
        //                    if (!String.IsNullOrWhiteSpace(MeasurmentUnit.LocalName))
        //                    {
        //                        acc_suppliersup_item.StatisticQuantityTypeName = MeasurmentUnit.LocalName;
        //                    }
        //                    else if (!String.IsNullOrWhiteSpace(MeasurmentUnit.EnglishName))
        //                    {
        //                        acc_suppliersup_item.StatisticQuantityTypeName = MeasurmentUnit.EnglishName;
        //                    }
        //                }
        //            }
        //        }
        //        if (suppliersup_item.AdditionalQuantity.HasValue)
        //        {
        //            acc_suppliersup_item.AddQuantity = suppliersup_item.AdditionalQuantity.Value.ToString();
        //            if (!String.IsNullOrWhiteSpace(suppliersup_item.AdditionalQuantityType))
        //            {
        //                acc_suppliersup_item.AddQuantityType = suppliersup_item.AdditionalQuantityType;
        //                MeasurmentUnitQueryService MeasurmentUnitQuery = new MeasurmentUnitQueryService(context);
        //                MeasurmentUnitPM MeasurmentUnit = MeasurmentUnitQuery.GetSingle(suppliersup_item.AdditionalQuantityType, true, false);
        //                if (MeasurmentUnit != null)
        //                {
        //                    if (!String.IsNullOrWhiteSpace(MeasurmentUnit.LocalName))
        //                    {
        //                        acc_suppliersup_item.AddQuantityTypeName = MeasurmentUnit.LocalName;
        //                    }
        //                    else if (!String.IsNullOrWhiteSpace(MeasurmentUnit.EnglishName))
        //                    {
        //                        acc_suppliersup_item.AddQuantityTypeName = MeasurmentUnit.EnglishName;
        //                    }
        //                }
        //            }
        //        }
        //        acc_suppliersup_item.PreferenceDocumentNumber = suppliersup_item.PreferenceDocumentNumber;
        //        acc_suppliersup_item.TaxExemptCode = suppliersup_item.TaxExemptCode;
        //        if (!String.IsNullOrWhiteSpace(suppliersup_item.CustomsBookTypeCode))
        //        {
        //            acc_suppliersup_item.CustomsBookTypeCode = suppliersup_item.CustomsBookTypeCode;
        //            CustomsBookTypeQueryService CustomsBookTypeQuery = new CustomsBookTypeQueryService(context);
        //            CustomsBookTypePM CustomsBookType = CustomsBookTypeQuery.GetSingle(suppliersup_item.CustomsBookTypeCode, true, false);
        //            if (CustomsBookType != null)
        //            {
        //                if (!String.IsNullOrWhiteSpace(CustomsBookType.LocalName))
        //                {
        //                    acc_suppliersup_item.CustomsBookTypeCodeName = CustomsBookType.LocalName;
        //                }
        //                else if (!String.IsNullOrWhiteSpace(CustomsBookType.EnglishName))
        //                {
        //                    acc_suppliersup_item.CustomsBookTypeCodeName = CustomsBookType.EnglishName;
        //                }
        //            }
        //        }
        //        acc_suppliersup_item.DangerousClassificationCode = suppliersup_item.DangerousClassificationCode;
        //        if (suppliersup_item.NonCustomsItemPrice.HasValue)
        //        {
        //            acc_suppliersup_item.NonCustomsItemPrice = suppliersup_item.NonCustomsItemPrice.Value.ToString();
        //            if (!String.IsNullOrWhiteSpace(suppliersup_item.NonCustomsItemPriceCurCode))
        //            {
        //                acc_suppliersup_item.NonCustomsItemPriceCurCode = suppliersup_item.NonCustomsItemPriceCurCode;
        //                CurrencyTypeQueryService CurrencyTypeQuery = new CurrencyTypeQueryService(context);
        //                CurrencyTypePM CurrencyType = CurrencyTypeQuery.GetSingle(suppliersup_item.NonCustomsItemPriceCurCode, true, false);
        //                if (CurrencyType != null)
        //                {
        //                    if (!String.IsNullOrWhiteSpace(CurrencyType.LocalName))
        //                    {
        //                        acc_suppliersup_item.NonCustomsItemPriceCurCodeName = CurrencyType.LocalName;
        //                    }
        //                    else if (!String.IsNullOrWhiteSpace(CurrencyType.EnglishName))
        //                    {
        //                        acc_suppliersup_item.NonCustomsItemPriceCurCodeName = CurrencyType.EnglishName;
        //                    }
        //                }
        //            }
        //        }
        //        if (suppliersup_item.WholeSaleItemPrice.HasValue)
        //        {
        //            acc_suppliersup_item.WholeSaleItemPrice = suppliersup_item.WholeSaleItemPrice.Value.ToString();
        //            if (!String.IsNullOrWhiteSpace(suppliersup_item.WholeSaleItemPriceCurrencyCode))
        //            {
        //                acc_suppliersup_item.WholeSaleItemPriceCurrencyCode = suppliersup_item.WholeSaleItemPriceCurrencyCode;
        //                CurrencyTypeQueryService CurrencyTypeQuery = new CurrencyTypeQueryService(context);
        //                CurrencyTypePM CurrencyType = CurrencyTypeQuery.GetSingle(suppliersup_item.WholeSaleItemPriceCurrencyCode, true, false);
        //                if (CurrencyType != null)
        //                {
        //                    if (!String.IsNullOrWhiteSpace(CurrencyType.LocalName))
        //                    {
        //                        acc_suppliersup_item.WholeSaleItemPriceCurrencyCode = CurrencyType.LocalName;
        //                    }
        //                    else if (!String.IsNullOrWhiteSpace(CurrencyType.EnglishName))
        //                    {
        //                        acc_suppliersup_item.WholeSaleItemPriceCurrencyCode = CurrencyType.EnglishName;
        //                    }
        //                }
        //            }
        //        }
        //        acc_suppliersup_item.ManufactureIdentifier = suppliersup_item.ManufactureIdentifier;
        //        if (!String.IsNullOrWhiteSpace(suppliersup_item.SalesTaxExemptionTypeCode))
        //        {
        //            acc_suppliersup_item.SalesTaxExemptionTypeCode = suppliersup_item.SalesTaxExemptionTypeCode;
        //            SalesTaxExemptionTypeQueryService SalesTaxExemptionTypeQuery = new SalesTaxExemptionTypeQueryService(context);
        //            SalesTaxExemptionTypePM SalesTaxExemptionType = SalesTaxExemptionTypeQuery.GetSingle(suppliersup_item.SalesTaxExemptionTypeCode, true, false);
        //            if (SalesTaxExemptionType != null)
        //            {
        //                if (!String.IsNullOrWhiteSpace(SalesTaxExemptionType.LocalName))
        //                {
        //                    acc_suppliersup_item.SalesTaxExemptionTypeName = SalesTaxExemptionType.LocalName;
        //                }
        //                else if (!String.IsNullOrWhiteSpace(SalesTaxExemptionType.EnglishName))
        //                {
        //                    acc_suppliersup_item.SalesTaxExemptionTypeName = SalesTaxExemptionType.EnglishName;
        //                }
        //            }
        //        }
        //        if (suppliersup_item.OptionalTamaPercentage.HasValue) acc_suppliersup_item.OptionalTamaPercentage = suppliersup_item.OptionalTamaPercentage.ToString();
        //        // moran 4.5.16 - task 20319 <--
        //        acc_suppliersup_item.Acc_supplieritemAdds = GetSupplierInvoiceItemAdds(acc_suppliersup_item); // moran 5.5.16 - task 20319

        //        acc_suppliersup_item.Acc_supplieritemscertificate = GetSupplierInvoiceItemCertificates(suppliersup_item); // moran 26.8.15 - task 16086
        //        acc_suppliersup_item.Acc_supplieritemsConDeclars = GetSupplierInvoiceItemConDeclars(suppliersup_item); // moran 3.5.16 - task 20319 
        //        acc_suppliersup_item.Acc_supplieritemsMods = GetSupplierInvoiceItemMods(suppliersup_item); // moran 3.5.16 - task 20319
        //        acc_suppliersup_item.Acc_supplieritemsLevies = GetSupplierInvoiceItemLevies(suppliersup_item); // moran 3.5.16 - task 20319  
        //        acc_suppliersup_item.Acc_supplieritemsProdIdents = GetSupplierInvoiceItemProdIdents(suppliersup_item); // moran 3.5.16 - task 20319  
        //        acc_suppliersup_item.Acc_supplieritemsSerialNums = GetSupplierInvoiceItemSerialNums(suppliersup_item); // moran 3.5.16 - task 20319   
        //        acc_suppliersup_item.Acc_supplieritemsDescripts = GetSupplierInvoiceItemDescripts(suppliersup_item); // moran 3.5.16 - task 20319  
        //        acc_suppliersup_item.Acc_supplieritemsVehicles = GetSupplierInvoiceItemVehicles(suppliersup_item); // moran 3.5.16 - task 20319  
        //        acc_suppliersup_item.Acc_supplieritemsVehicleAndMods = GetSupplierInvoiceItemVehicleMods(suppliersup_item); // moran 8.5.16 - task 20319

        //        acc_suppliersup_itemList.Add(acc_suppliersup_item);
        //    }

        //    return acc_suppliersup_itemList;

        //}


        private List<SupplierInvioceItemVehicleAndModsM> GetSupplierInvoiceItemVehicleMods(SupplierInvoiceItemPM suppliersup_item) // moran 8.5.16 - task 20319
        {
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant);
            var acc_suppliersup_item_VehicleAndModList = new List<SupplierInvioceItemVehicleAndModsM>();
            if (suppliersup_item.SupplierInvoiceItemVehicles != null && suppliersup_item.SupplierInvoiceItemVehicles.Count() > 0)
            {
                foreach (var supplierInvoiceItemVehiclePM in suppliersup_item.SupplierInvoiceItemVehicles.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.InvoiceItemLineNumber == suppliersup_item.LineNumber).ToList())
                {

                    decimal? totDeductAmount = 0;
                    int? counter = 0;
                    var acc_suppliersup_item_VehicleAndModMain = new SupplierInvioceItemVehicleAndModsM();



                    if (supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods != null && supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods.Count() > 0)
                    {
                        foreach (var supplierInvoiceItemVehicleModPM in supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods.Where(d => d.DeclarationId == supplierInvoiceItemVehiclePM.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItemVehiclePM.InvoiceCounterKey && d.InvoiceItemLineNumber == supplierInvoiceItemVehiclePM.InvoiceItemLineNumber && d.VehicleLineNumber == supplierInvoiceItemVehiclePM.LineNumber).ToList())
                        {
                            counter++;

                            var acc_suppliersup_item_VehicleAndMod = new SupplierInvioceItemVehicleAndModsM();
                            if (supplierInvoiceItemVehicleModPM.DeductAmount.HasValue)
                            {
                                totDeductAmount += supplierInvoiceItemVehicleModPM.DeductAmount;
                                acc_suppliersup_item_VehicleAndMod.ModDeductAmount = supplierInvoiceItemVehicleModPM.DeductAmount.ToString();
                            }
                            if (!String.IsNullOrWhiteSpace(supplierInvoiceItemVehicleModPM.AdjustmentTypeCode))
                            {
                                acc_suppliersup_item_VehicleAndMod.ModAdjustmentTypeCode = supplierInvoiceItemVehicleModPM.AdjustmentTypeCode;
                                VehicleReductionTypeQueryService VehicleReductionTypeQuery = new VehicleReductionTypeQueryService(context);
                                VehicleReductionTypePM VehicleReductionType = VehicleReductionTypeQuery.GetSingle(supplierInvoiceItemVehicleModPM.AdjustmentTypeCode, true, false);
                                if (VehicleReductionType != null)
                                {
                                    if (!String.IsNullOrWhiteSpace(VehicleReductionType.LocalName))
                                    {
                                        acc_suppliersup_item_VehicleAndMod.ModAdjustmentTypeName = VehicleReductionType.LocalName;
                                    }
                                    else if (!String.IsNullOrWhiteSpace(VehicleReductionType.EnglishName))
                                    {
                                        acc_suppliersup_item_VehicleAndMod.ModAdjustmentTypeName = VehicleReductionType.EnglishName;
                                    }
                                }
                            }

                            if (counter > 1)
                            {
                                acc_suppliersup_item_VehicleAndModList.Add(acc_suppliersup_item_VehicleAndMod);
                            }
                            else
                            {
                                acc_suppliersup_item_VehicleAndModMain = acc_suppliersup_item_VehicleAndMod;
                                if (supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds != null && supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.Count() > 0)
                                {
                                    var supplierInvoiceItemVehicleAddPM = supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.Where(d => d.DeclarationId == supplierInvoiceItemVehiclePM.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItemVehiclePM.InvoiceCounterKey && d.InvoiceItemLineNumber == supplierInvoiceItemVehiclePM.InvoiceItemLineNumber && d.LineNumber == supplierInvoiceItemVehiclePM.LineNumber).FirstOrDefault();
                                    acc_suppliersup_item_VehicleAndModMain.ModClassificationCode = suppliersup_item.ClassificationCode;
                                    acc_suppliersup_item_VehicleAndModMain.ModRichbitNumber = supplierInvoiceItemVehicleAddPM.RichbitNumber;
                                    acc_suppliersup_item_VehicleAndModMain.ModChassisNumber = supplierInvoiceItemVehicleAddPM.ChassisNumber;
                                    acc_suppliersup_item_VehicleAndModMain.ModVehicleModel = supplierInvoiceItemVehicleAddPM.VehicleModel;
                                    if (supplierInvoiceItemVehicleAddPM.ChassisTax.HasValue) acc_suppliersup_item_VehicleAndModMain.ModChassisTax = supplierInvoiceItemVehicleAddPM.ChassisTax.ToString();
                                    if (supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax.HasValue) acc_suppliersup_item_VehicleAndModMain.ModChassisPurchaseTax = supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax.ToString();
                                    if (supplierInvoiceItemVehicleAddPM.ChassisVat.HasValue) acc_suppliersup_item_VehicleAndModMain.ModChassisVat = supplierInvoiceItemVehicleAddPM.ChassisVat.ToString();
                                }
                                else
                                {
                                    acc_suppliersup_item_VehicleAndModMain.ModClassificationCode = suppliersup_item.ClassificationCode;
                                    acc_suppliersup_item_VehicleAndModMain.ModRichbitNumber = supplierInvoiceItemVehiclePM.RichbitFileNumber;
                                    acc_suppliersup_item_VehicleAndModMain.ModChassisNumber = supplierInvoiceItemVehiclePM.VehicleChassisNumber;
                                }
                                acc_suppliersup_item_VehicleAndModList.Add(acc_suppliersup_item_VehicleAndModMain);
                            }
                        }

                        if (acc_suppliersup_item_VehicleAndModList != null && acc_suppliersup_item_VehicleAndModList.LastOrDefault() != null)
                        {
                            acc_suppliersup_item_VehicleAndModList.LastOrDefault().ModTotDeductAmount = totDeductAmount.ToString();
                        }
                    }
                    else
                    {
                        if (supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds != null && supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.Count() > 0)
                        {
                            var supplierInvoiceItemVehicleAddPM = supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.Where(d => d.DeclarationId == supplierInvoiceItemVehiclePM.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItemVehiclePM.InvoiceCounterKey && d.InvoiceItemLineNumber == supplierInvoiceItemVehiclePM.InvoiceItemLineNumber && d.LineNumber == supplierInvoiceItemVehiclePM.LineNumber).FirstOrDefault();
                            acc_suppliersup_item_VehicleAndModMain.ModClassificationCode = suppliersup_item.ClassificationCode;
                            acc_suppliersup_item_VehicleAndModMain.ModRichbitNumber = supplierInvoiceItemVehicleAddPM.RichbitNumber;
                            acc_suppliersup_item_VehicleAndModMain.ModChassisNumber = supplierInvoiceItemVehicleAddPM.ChassisNumber;
                            acc_suppliersup_item_VehicleAndModMain.ModVehicleModel = supplierInvoiceItemVehicleAddPM.VehicleModel;
                            if (supplierInvoiceItemVehicleAddPM.ChassisTax.HasValue) acc_suppliersup_item_VehicleAndModMain.ModChassisTax = supplierInvoiceItemVehicleAddPM.ChassisTax.ToString();
                            if (supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax.HasValue) acc_suppliersup_item_VehicleAndModMain.ModChassisPurchaseTax = supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax.ToString();
                            if (supplierInvoiceItemVehicleAddPM.ChassisVat.HasValue) acc_suppliersup_item_VehicleAndModMain.ModChassisVat = supplierInvoiceItemVehicleAddPM.ChassisVat.ToString();
                        }
                        else
                        {
                            acc_suppliersup_item_VehicleAndModMain.ModClassificationCode = suppliersup_item.ClassificationCode;
                            acc_suppliersup_item_VehicleAndModMain.ModRichbitNumber = supplierInvoiceItemVehiclePM.RichbitFileNumber;
                            acc_suppliersup_item_VehicleAndModMain.ModChassisNumber = supplierInvoiceItemVehiclePM.VehicleChassisNumber;
                        }
                        acc_suppliersup_item_VehicleAndModList.Add(acc_suppliersup_item_VehicleAndModMain);
                    }
                }
            }
            else
            {
                var acc_suppliersup_item_VehicleAndMod = new SupplierInvioceItemVehicleAndModsM();
                acc_suppliersup_item_VehicleAndModList.Add(acc_suppliersup_item_VehicleAndMod);
            }

            return acc_suppliersup_item_VehicleAndModList;
        }


        private List<SupplierInvioceItemAddsM> GetSupplierInvoiceItemAdds(acc_suppliersup_items acc_suppliersup_item) // moran 5.5.16 - task 20319
        {
            var acc_suppliersup_item_AddList = new List<SupplierInvioceItemAddsM>();
            var acc_suppliersup_item_Add = new SupplierInvioceItemAddsM();

            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.SupplierInvoiceItemProcesTypes))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "קוד תהליך", AddFieldValue = acc_suppliersup_item.SupplierInvoiceItemProcesTypes });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.StatisticQuantity))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "כמות סטטיסטית", AddFieldValue = acc_suppliersup_item.StatisticQuantity + " " + acc_suppliersup_item.StatisticQuantityType });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.AddQuantity))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "כמות נוספת", AddFieldValue = acc_suppliersup_item.AddQuantity + " " + acc_suppliersup_item.AddQuantityType });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.PreferenceDocumentNumber))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "מספר מסמך העדפה", AddFieldValue = acc_suppliersup_item.PreferenceDocumentNumber });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.TaxExemptCode))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "קוד הנחה", AddFieldValue = acc_suppliersup_item.TaxExemptCode });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.CustomsBookTypeCode))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "סוג ספר מכס", AddFieldValue = acc_suppliersup_item.CustomsBookTypeCode });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.DangerousClassificationCode))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "פרט מכס מסוכנים", AddFieldValue = acc_suppliersup_item.DangerousClassificationCode });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.NonCustomsItemPrice))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "סכום ללא ערך מכס", AddFieldValue = acc_suppliersup_item.NonCustomsItemPrice + " " + acc_suppliersup_item.NonCustomsItemPriceCurCode });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.WholeSaleItemPrice))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "מחיר סיטונאי", AddFieldValue = acc_suppliersup_item.WholeSaleItemPrice + " " + acc_suppliersup_item.WholeSaleItemPriceCurrencyCode });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.ManufactureIdentifier))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "זהוי יצרן", AddFieldValue = acc_suppliersup_item.ManufactureIdentifier });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.SalesTaxExemptionTypeCode))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "פטור ממס קניה", AddFieldValue = acc_suppliersup_item.SalesTaxExemptionTypeCode + " " + acc_suppliersup_item.SalesTaxExemptionTypeName });
            }
            if (!String.IsNullOrWhiteSpace(acc_suppliersup_item.OptionalTamaPercentage))
            {
                acc_suppliersup_item_AddList.Add(new SupplierInvioceItemAddsM() { AddField = "אחוז תמ''א", AddFieldValue = acc_suppliersup_item.OptionalTamaPercentage });
            }
            return acc_suppliersup_item_AddList;
        }


        private List<SupplierInvioceItemVehiclesM> GetSupplierInvoiceItemVehicles(SupplierInvoiceItemPM suppliersup_item) // moran 3.5.16 - task 20319
        {
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant);
            var acc_suppliersup_item_VehicleList = new List<SupplierInvioceItemVehiclesM>();
            if (suppliersup_item.SupplierInvoiceItemVehicles != null && suppliersup_item.SupplierInvoiceItemVehicles.Count() > 0)
            {
                foreach (var supplierInvoiceItemVehiclePM in suppliersup_item.SupplierInvoiceItemVehicles.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.InvoiceItemLineNumber == suppliersup_item.LineNumber).ToList())
                {
                    var acc_suppliersup_item_Vehicle = new SupplierInvioceItemVehiclesM();
                    acc_suppliersup_item_Vehicle.RichbitFileNumber = supplierInvoiceItemVehiclePM.RichbitFileNumber;
                    acc_suppliersup_item_Vehicle.RichbitFileStatus = supplierInvoiceItemVehiclePM.RichbitFileStatus;
                    acc_suppliersup_item_Vehicle.VehicleChassisNumber = supplierInvoiceItemVehiclePM.VehicleChassisNumber;
                    acc_suppliersup_item_Vehicle.VehicleId = supplierInvoiceItemVehiclePM.VehicleId;

                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemVehiclePM.VehicleTypeCode))
                    {
                        acc_suppliersup_item_Vehicle.VehicleTypeCode = supplierInvoiceItemVehiclePM.VehicleTypeCode;
                        CargoIdentityQualifierQueryService CargoIdentityQualifierQuery = new CargoIdentityQualifierQueryService(context);
                        CargoIdentityQualifierPM CargoIdentityQualifier = CargoIdentityQualifierQuery.GetSingle(supplierInvoiceItemVehiclePM.VehicleTypeCode, true, false);
                        if (CargoIdentityQualifier != null)
                        {
                            if (!String.IsNullOrWhiteSpace(CargoIdentityQualifier.LocalName))
                            {
                                acc_suppliersup_item_Vehicle.VehicleTypeName = CargoIdentityQualifier.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(CargoIdentityQualifier.EnglishName))
                            {
                                acc_suppliersup_item_Vehicle.VehicleTypeName = CargoIdentityQualifier.EnglishName;
                            }
                        }
                    }

                    acc_suppliersup_item_Vehicle.Acc_supplieritemsVehicleAdds = GetSupplierInvoiceItemVehicleAdds(supplierInvoiceItemVehiclePM);
                    acc_suppliersup_item_Vehicle.Acc_supplieritemsVehicleMods = GetSupplierInvoiceItemVehicleMods(supplierInvoiceItemVehiclePM, acc_suppliersup_item_Vehicle);

                    acc_suppliersup_item_VehicleList.Add(acc_suppliersup_item_Vehicle);
                }
            }
            else
            {
                var acc_suppliersup_item_Vehicle = new SupplierInvioceItemVehiclesM();
                acc_suppliersup_item_VehicleList.Add(acc_suppliersup_item_Vehicle);
            }

            return acc_suppliersup_item_VehicleList;
        }


        private List<SupplierInvioceItemVehicleModsM> GetSupplierInvoiceItemVehicleMods(SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM, SupplierInvioceItemVehiclesM acc_suppliersup_item_Vehicle) // moran 3.5.16 - task 20319
        {
            var acc_suppliersup_item_VehicleModList = new List<SupplierInvioceItemVehicleModsM>();
            decimal? totDeductAmount = 0;
            if (supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods != null && supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods.Count() > 0)
            {
                foreach (var supplierInvoiceItemVehicleModPM in supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods.Where(d => d.DeclarationId == supplierInvoiceItemVehiclePM.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItemVehiclePM.InvoiceCounterKey && d.InvoiceItemLineNumber == supplierInvoiceItemVehiclePM.InvoiceItemLineNumber && d.VehicleLineNumber == supplierInvoiceItemVehiclePM.LineNumber).ToList())
                {
                    var acc_suppliersup_item_VehicleMod = new SupplierInvioceItemVehicleModsM();
                    if (supplierInvoiceItemVehicleModPM.DeductAmount.HasValue)
                    {
                        totDeductAmount += supplierInvoiceItemVehicleModPM.DeductAmount;
                        acc_suppliersup_item_VehicleMod.DeductAmount = supplierInvoiceItemVehicleModPM.DeductAmount.ToString();
                    }
                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemVehicleModPM.AdjustmentTypeCode))
                    {
                        acc_suppliersup_item_VehicleMod.AdjustmentTypeCode = supplierInvoiceItemVehicleModPM.AdjustmentTypeCode;
                        VehicleReductionTypeQueryService VehicleReductionTypeQuery = new VehicleReductionTypeQueryService(context);
                        VehicleReductionTypePM VehicleReductionType = VehicleReductionTypeQuery.GetSingle(supplierInvoiceItemVehicleModPM.AdjustmentTypeCode, true, false);
                        if (VehicleReductionType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(VehicleReductionType.LocalName))
                            {
                                acc_suppliersup_item_VehicleMod.AdjustmentTypeName = VehicleReductionType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(VehicleReductionType.EnglishName))
                            {
                                acc_suppliersup_item_VehicleMod.AdjustmentTypeName = VehicleReductionType.EnglishName;
                            }
                        }
                    }

                    acc_suppliersup_item_VehicleModList.Add(acc_suppliersup_item_VehicleMod);
                }
            }
            else
            {
                //                var acc_suppliersup_item_VehicleMod = new SupplierInvioceItemVehicleModsM(); // moran 13.6.16 - AMI-57045 - commented
                //                acc_suppliersup_item_VehicleModList.Add(acc_suppliersup_item_VehicleMod); // moran 13.6.16 - AMI-57045 - commented
            }

            return acc_suppliersup_item_VehicleModList;
        }


        private List<SupplierInvioceItemVehicleAddsM> GetSupplierInvoiceItemVehicleAdds(SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM) // moran 3.5.16 - task 20319
        {
            var acc_suppliersup_item_VehicleAddList = new List<SupplierInvioceItemVehicleAddsM>();
            if (supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds != null && supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.Count() > 0)
            {
                foreach (var supplierInvoiceItemVehicleAddPM in supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.Where(d => d.DeclarationId == supplierInvoiceItemVehiclePM.DeclarationId && d.InvoiceCounterKey == supplierInvoiceItemVehiclePM.InvoiceCounterKey && d.InvoiceItemLineNumber == supplierInvoiceItemVehiclePM.InvoiceItemLineNumber && d.LineNumber == supplierInvoiceItemVehiclePM.LineNumber).ToList())
                {
                    var acc_suppliersup_item_VehicleAdd = new SupplierInvioceItemVehicleAddsM();
                    acc_suppliersup_item_VehicleAdd.RichbitNumber = supplierInvoiceItemVehicleAddPM.RichbitNumber;
                    acc_suppliersup_item_VehicleAdd.ChassisNumber = supplierInvoiceItemVehicleAddPM.ChassisNumber;
                    acc_suppliersup_item_VehicleAdd.VehicleModel = supplierInvoiceItemVehicleAddPM.VehicleModel;
                    if (supplierInvoiceItemVehicleAddPM.ChassisTax.HasValue) acc_suppliersup_item_VehicleAdd.ChassisTax = supplierInvoiceItemVehicleAddPM.ChassisTax.ToString();
                    if (supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax.HasValue) acc_suppliersup_item_VehicleAdd.ChassisPurchaseTax = supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax.ToString();
                    if (supplierInvoiceItemVehicleAddPM.ChassisVat.HasValue) acc_suppliersup_item_VehicleAdd.ChassisVat = supplierInvoiceItemVehicleAddPM.ChassisVat.ToString();
                    acc_suppliersup_item_VehicleAdd.EngineNumber = supplierInvoiceItemVehicleAddPM.EngineNumber;
                    acc_suppliersup_item_VehicleAdd.Exempt_type = supplierInvoiceItemVehicleAddPM.Exempt_type;
                    if (supplierInvoiceItemVehicleAddPM.VehicleValue.HasValue) acc_suppliersup_item_VehicleAdd.VehicleValue = supplierInvoiceItemVehicleAddPM.VehicleValue.ToString();
                    acc_suppliersup_item_VehicleAdd.WindowNumber = supplierInvoiceItemVehicleAddPM.WindowNumber;

                    acc_suppliersup_item_VehicleAddList.Add(acc_suppliersup_item_VehicleAdd);
                }
            }
            else
            {
                var acc_suppliersup_item_VehicleAdd = new SupplierInvioceItemVehicleAddsM();
                acc_suppliersup_item_VehicleAddList.Add(acc_suppliersup_item_VehicleAdd);
            }

            return acc_suppliersup_item_VehicleAddList;
        }


        private List<SupplierInvioceItemDescriptsM> GetSupplierInvoiceItemDescripts(SupplierInvoiceItemPM suppliersup_item) // moran 3.5.16 - task 20319
        {
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant);
            var acc_suppliersup_item_DescriptList = new List<SupplierInvioceItemDescriptsM>();
            if (suppliersup_item.SupplierInvoiceItemsDescripts != null && suppliersup_item.SupplierInvoiceItemsDescripts.Count() > 0)
            {
                foreach (var supplierInvoiceItemDescriptPM in suppliersup_item.SupplierInvoiceItemsDescripts.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.InvoiceItemLineNumber == suppliersup_item.LineNumber).ToList())
                {
                    var acc_suppliersup_item_Descript = new SupplierInvioceItemDescriptsM();

                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemDescriptPM.TypeCode))
                    {
                        acc_suppliersup_item_Descript.DescriptionTypeCode = supplierInvoiceItemDescriptPM.TypeCode;
                        ProductNameTypeQueryService ProductNameTypeQuery = new ProductNameTypeQueryService(context);
                        ProductNameTypePM ProductNameType = ProductNameTypeQuery.GetSingle(supplierInvoiceItemDescriptPM.TypeCode, true, false);
                        if (ProductNameType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(ProductNameType.LocalName))
                            {
                                acc_suppliersup_item_Descript.DescriptionTypeName = ProductNameType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(ProductNameType.EnglishName))
                            {
                                acc_suppliersup_item_Descript.DescriptionTypeName = ProductNameType.EnglishName;
                            }
                        }
                    }

                    acc_suppliersup_item_Descript.Description = supplierInvoiceItemDescriptPM.Description;

                    acc_suppliersup_item_DescriptList.Add(acc_suppliersup_item_Descript);
                }
            }
            else
            {
                var acc_suppliersup_item_Descript = new SupplierInvioceItemDescriptsM();
                acc_suppliersup_item_DescriptList.Add(acc_suppliersup_item_Descript);
            }

            return acc_suppliersup_item_DescriptList;
        }


        private List<SupplierInvioceItemSerialNumsM> GetSupplierInvoiceItemSerialNums(SupplierInvoiceItemPM suppliersup_item) // moran 3.5.16 - task 20319
        {
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant);
            var acc_suppliersup_item_SerialNumList = new List<SupplierInvioceItemSerialNumsM>();
            if (suppliersup_item.SupplierInvoiceItemsSerialNums != null && suppliersup_item.SupplierInvoiceItemsSerialNums.Count() > 0)
            {
                foreach (var supplierInvoiceItemSerialNumPM in suppliersup_item.SupplierInvoiceItemsSerialNums.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.InvoiceItemLineNumber == suppliersup_item.LineNumber).ToList())
                {
                    var acc_suppliersup_item_SerialNum = new SupplierInvioceItemSerialNumsM();

                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemSerialNumPM.TypeCode))
                    {
                        acc_suppliersup_item_SerialNum.SerialNumberTypeCode = supplierInvoiceItemSerialNumPM.TypeCode;
                        CargoIdentityQualifierQueryService CargoIdentityQualifierQuery = new CargoIdentityQualifierQueryService(context);
                        CargoIdentityQualifierPM CargoIdentityQualifier = CargoIdentityQualifierQuery.GetSingle(supplierInvoiceItemSerialNumPM.TypeCode, true, false);
                        if (CargoIdentityQualifier != null)
                        {
                            if (!String.IsNullOrWhiteSpace(CargoIdentityQualifier.LocalName))
                            {
                                acc_suppliersup_item_SerialNum.SerialNumberTypeName = CargoIdentityQualifier.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(CargoIdentityQualifier.EnglishName))
                            {
                                acc_suppliersup_item_SerialNum.SerialNumberTypeName = CargoIdentityQualifier.EnglishName;
                            }
                        }
                    }

                    acc_suppliersup_item_SerialNum.SerialNumber = supplierInvoiceItemSerialNumPM.SerialNumber;

                    acc_suppliersup_item_SerialNumList.Add(acc_suppliersup_item_SerialNum);
                }
            }
            else
            {
                var acc_suppliersup_item_SerialNum = new SupplierInvioceItemSerialNumsM();
                acc_suppliersup_item_SerialNumList.Add(acc_suppliersup_item_SerialNum);
            }

            return acc_suppliersup_item_SerialNumList;
        }


        private List<SupplierInvioceItemProdIdentsM> GetSupplierInvoiceItemProdIdents(SupplierInvoiceItemPM suppliersup_item) // moran 3.5.16 - task 20319
        {
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant);
            var acc_suppliersup_item_ProdIdentList = new List<SupplierInvioceItemProdIdentsM>();
            if (suppliersup_item.SupplierInvoiceItemsProdIdents != null && suppliersup_item.SupplierInvoiceItemsProdIdents.Count() > 0)
            {
                foreach (var supplierInvoiceItemProdIdentPM in suppliersup_item.SupplierInvoiceItemsProdIdents.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.InvoiceItemLineNumber == suppliersup_item.LineNumber).ToList())
                {
                    var acc_suppliersup_item_ProdIdent = new SupplierInvioceItemProdIdentsM();

                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemProdIdentPM.TypeCode))
                    {
                        acc_suppliersup_item_ProdIdent.ProdIdentificationTypeCode = supplierInvoiceItemProdIdentPM.TypeCode;
                        ProductIdentificationTypeQueryService ProductIdentificationTypeQuery = new ProductIdentificationTypeQueryService(context);
                        ProductIdentificationTypePM ProductIdentificationType = ProductIdentificationTypeQuery.GetSingle(supplierInvoiceItemProdIdentPM.TypeCode, true, false);
                        if (ProductIdentificationType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(ProductIdentificationType.LocalName))
                            {
                                acc_suppliersup_item_ProdIdent.ProdIdentificationTypeName = ProductIdentificationType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(ProductIdentificationType.EnglishName))
                            {
                                acc_suppliersup_item_ProdIdent.ProdIdentificationTypeName = ProductIdentificationType.EnglishName;
                            }
                        }
                    }

                    acc_suppliersup_item_ProdIdent.ProductIdentification = supplierInvoiceItemProdIdentPM.Identification;

                    acc_suppliersup_item_ProdIdentList.Add(acc_suppliersup_item_ProdIdent);
                }
            }
            else
            {
                var acc_suppliersup_item_ProdIdent = new SupplierInvioceItemProdIdentsM();
                acc_suppliersup_item_ProdIdentList.Add(acc_suppliersup_item_ProdIdent);
            }

            return acc_suppliersup_item_ProdIdentList;
        }


        private List<SupplierInvioceItemLeviesM> GetSupplierInvoiceItemLevies(SupplierInvoiceItemPM suppliersup_item) // moran 3.5.16 - task 20319
        {
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant);
            var acc_suppliersup_item_LevyList = new List<SupplierInvioceItemLeviesM>();
            if (suppliersup_item.SupplierInvoiceItemLevies != null && suppliersup_item.SupplierInvoiceItemLevies.Count() > 0)
            {
                foreach (var supplierInvoiceItemLevyPM in suppliersup_item.SupplierInvoiceItemLevies.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.InvoiceItemLineNumber == suppliersup_item.LineNumber).ToList())
                {
                    var acc_suppliersup_item_Levy = new SupplierInvioceItemLeviesM();

                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemLevyPM.TradeLevyExamptCode))
                    {
                        acc_suppliersup_item_Levy.LevyTypeCode = supplierInvoiceItemLevyPM.TradeLevyExamptCode;
                        TradeLevyExamptTypeQueryService TradeLevyExamptTypeQuery = new TradeLevyExamptTypeQueryService(context);
                        TradeLevyExamptTypePM TradeLevyExamptType = TradeLevyExamptTypeQuery.GetSingle(supplierInvoiceItemLevyPM.TradeLevyExamptCode, true, false);
                        if (TradeLevyExamptType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(TradeLevyExamptType.LocalName))
                            {
                                acc_suppliersup_item_Levy.LevyTypeName = TradeLevyExamptType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(TradeLevyExamptType.EnglishName))
                            {
                                acc_suppliersup_item_Levy.LevyTypeName = TradeLevyExamptType.EnglishName;
                            }
                        }
                    }

                    acc_suppliersup_item_Levy.LevyNumber = supplierInvoiceItemLevyPM.TradeLevyNumber;

                    acc_suppliersup_item_LevyList.Add(acc_suppliersup_item_Levy);
                }
            }
            else
            {
                var acc_suppliersup_item_Levy = new SupplierInvioceItemLeviesM();
                acc_suppliersup_item_LevyList.Add(acc_suppliersup_item_Levy);
            }

            return acc_suppliersup_item_LevyList;
        }


        private List<SupplierInvioceItemModsM> GetSupplierInvoiceItemMods(SupplierInvoiceItemPM suppliersup_item) // moran 3.5.16 - task 20319
        {
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant);
            var acc_suppliersup_item_ModList = new List<SupplierInvioceItemModsM>();
            if (suppliersup_item.SupplierInvoiceItemsMods != null && suppliersup_item.SupplierInvoiceItemsMods.Count() > 0)
            {
                foreach (var supplierInvoiceItemModPM in suppliersup_item.SupplierInvoiceItemsMods.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.LineNumber == suppliersup_item.LineNumber).ToList())
                {
                    var acc_suppliersup_item_Mod = new SupplierInvioceItemModsM();

                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemModPM.TypeCode))
                    {
                        acc_suppliersup_item_Mod.ModificationTypeCode = supplierInvoiceItemModPM.TypeCode;
                        ModificationAndDiscountTypeQueryService ModificationAndDiscountTypeQuery = new ModificationAndDiscountTypeQueryService(context);
                        ModificationAndDiscountTypePM ModificationAndDiscountType = ModificationAndDiscountTypeQuery.GetSingle(supplierInvoiceItemModPM.TypeCode, true, false);
                        if (ModificationAndDiscountType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(ModificationAndDiscountType.LocalName))
                            {
                                acc_suppliersup_item_Mod.ModificationTypeName = ModificationAndDiscountType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(ModificationAndDiscountType.EnglishName))
                            {
                                acc_suppliersup_item_Mod.ModificationTypeName = ModificationAndDiscountType.EnglishName;
                            }
                        }
                    }

                    if (supplierInvoiceItemModPM.Amount.HasValue) acc_suppliersup_item_Mod.Amount = supplierInvoiceItemModPM.Amount.ToString();
                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemModPM.CurrencyTypeCode))
                    {
                        acc_suppliersup_item_Mod.CurrencyTypeCode = supplierInvoiceItemModPM.CurrencyTypeCode;
                        CurrencyTypeQueryService CurrencyTypeQuery = new CurrencyTypeQueryService(context);
                        CurrencyTypePM CurrencyType = CurrencyTypeQuery.GetSingle(supplierInvoiceItemModPM.CurrencyTypeCode, true, false);
                        if (CurrencyType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(CurrencyType.LocalName))
                            {
                                acc_suppliersup_item_Mod.CurrencyTypeName = CurrencyType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(CurrencyType.EnglishName))
                            {
                                acc_suppliersup_item_Mod.CurrencyTypeName = CurrencyType.EnglishName;
                            }
                        }
                    }

                    acc_suppliersup_item_ModList.Add(acc_suppliersup_item_Mod);
                }
            }
            else
            {
                var acc_suppliersup_item_Mod = new SupplierInvioceItemModsM();
                acc_suppliersup_item_ModList.Add(acc_suppliersup_item_Mod);
            }

            return acc_suppliersup_item_ModList;
        }


        private List<SupplierInvioceItemConDeclarsM> GetSupplierInvoiceItemConDeclars(SupplierInvoiceItemPM suppliersup_item) // moran 3.5.16 - task 20319
        {
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant);
            var acc_suppliersup_item_ConDeclarList = new List<SupplierInvioceItemConDeclarsM>();
            if (suppliersup_item.SupplierInvoiceItemsConDeclars != null && suppliersup_item.SupplierInvoiceItemsConDeclars.Count() > 0)
            {
                foreach (var supplierInvoiceItemConDeclarPM in suppliersup_item.SupplierInvoiceItemsConDeclars.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.InvoiceItemLineNumber == suppliersup_item.LineNumber).ToList())
                {
                    var acc_suppliersup_item_ConDeclar = new SupplierInvioceItemConDeclarsM();

                    acc_suppliersup_item_ConDeclar.DeclarationNumber = supplierInvoiceItemConDeclarPM.DeclarationNumber;
                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemConDeclarPM.DeclarationTypeCode))
                    {
                        acc_suppliersup_item_ConDeclar.DeclarationTypeCode = supplierInvoiceItemConDeclarPM.DeclarationTypeCode;
                        LeadDocumentTypeQueryService LeadDocumentTypeQuery = new LeadDocumentTypeQueryService(context);
                        LeadDocumentTypePM LeadDocumentType = LeadDocumentTypeQuery.GetSingle(supplierInvoiceItemConDeclarPM.DeclarationTypeCode, true, false);
                        if (LeadDocumentType != null)
                        {
                            if (!String.IsNullOrWhiteSpace(LeadDocumentType.LocalName))
                            {
                                acc_suppliersup_item_ConDeclar.DeclarationTypeName = LeadDocumentType.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(LeadDocumentType.EnglishName))
                            {
                                acc_suppliersup_item_ConDeclar.DeclarationTypeName = LeadDocumentType.EnglishName;
                            }
                        }
                    }
                    if (supplierInvoiceItemConDeclarPM.InvoiceNumber.HasValue) acc_suppliersup_item_ConDeclar.InvoiceNumber = supplierInvoiceItemConDeclarPM.InvoiceNumber.ToString();
                    if (supplierInvoiceItemConDeclarPM.Quantity.HasValue) acc_suppliersup_item_ConDeclar.Quantity = supplierInvoiceItemConDeclarPM.Quantity.ToString();
                    if (!String.IsNullOrWhiteSpace(supplierInvoiceItemConDeclarPM.QuantityTypeCode))
                    {
                        acc_suppliersup_item_ConDeclar.QuantityTypeCode = supplierInvoiceItemConDeclarPM.QuantityTypeCode;
                        MeasurmentUnitQueryService MeasurmentUnitQuery = new MeasurmentUnitQueryService(context);
                        MeasurmentUnitPM MeasurmentUnit = MeasurmentUnitQuery.GetSingle(supplierInvoiceItemConDeclarPM.QuantityTypeCode, true, false);
                        if (MeasurmentUnit != null)
                        {
                            if (!String.IsNullOrWhiteSpace(MeasurmentUnit.LocalName))
                            {
                                acc_suppliersup_item_ConDeclar.QuantityTypeName = MeasurmentUnit.LocalName;
                            }
                            else if (!String.IsNullOrWhiteSpace(MeasurmentUnit.EnglishName))
                            {
                                acc_suppliersup_item_ConDeclar.QuantityTypeName = MeasurmentUnit.EnglishName;
                            }
                        }
                    }

                    acc_suppliersup_item_ConDeclarList.Add(acc_suppliersup_item_ConDeclar);
                }
            }
            else
            {
                var acc_suppliersup_item_ConDeclar = new SupplierInvioceItemConDeclarsM();
                acc_suppliersup_item_ConDeclarList.Add(acc_suppliersup_item_ConDeclar);
            }

            return acc_suppliersup_item_ConDeclarList;
        }


        private List<SupplierInvioceItemsCertificateM> GetSupplierInvoiceItemCertificates(SupplierInvoiceItemPM suppliersup_item) // moran 26.8.15 - task 16086
        {
            //var context = CustomContext.GetContext(suppliersup_item.Tenant); 
            if (context == null) context = CustomContext.GetContext(suppliersup_item.Tenant); // moran 10.9.15 - Task 16086
            var acc_suppliersup_item_certificateList = new List<SupplierInvioceItemsCertificateM>();
            if (suppliersup_item.SupplierInvioceItemCertificats != null && suppliersup_item.SupplierInvioceItemCertificats.Count() > 0) // moran 2.9.15 - Task 16086 - enter into 'if'
            {
                foreach (var supplierInvoiceItemCertificatePM in suppliersup_item.SupplierInvioceItemCertificats.Where(d => d.DeclarationId == suppliersup_item.DeclarationId && d.InvoiceCounterKey == suppliersup_item.CounterKey && d.LineNumber == suppliersup_item.LineNumber).ToList())
                {
                    var acc_suppliersup_item_certificate = new SupplierInvioceItemsCertificateM();

                    acc_suppliersup_item_certificate.ReqConfirmationTypeCode = supplierInvoiceItemCertificatePM.ReqConfirmationTypeCode;
                    ConfirmationTypeQueryService ConfirmationTypeQuery = new ConfirmationTypeQueryService(context);
                    ConfirmationTypePM ConfirmationType = ConfirmationTypeQuery.GetSingle(supplierInvoiceItemCertificatePM.ReqConfirmationTypeCode, true, false);
                    if (ConfirmationType != null)
                    {
                        if (!String.IsNullOrWhiteSpace(ConfirmationType.LocalName))
                        {
                            acc_suppliersup_item_certificate.ReqConfirmationTypeName = ConfirmationType.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(ConfirmationType.EnglishName))
                        {
                            acc_suppliersup_item_certificate.ReqConfirmationTypeName = ConfirmationType.EnglishName;
                        }
                    }

                    acc_suppliersup_item_certificate.CertificateNumber = supplierInvoiceItemCertificatePM.CertificateNumber;

                    acc_suppliersup_item_certificate.CertificateExemptionTypeCode = supplierInvoiceItemCertificatePM.CertificateExemptionTypeCode;
                    CertificateExemptionTypeQueryService CertificateExemptionTypeQuery = new CertificateExemptionTypeQueryService(context);
                    CertificateExemptionTypePM CertificateExemptionType = CertificateExemptionTypeQuery.GetSingle(supplierInvoiceItemCertificatePM.CertificateExemptionTypeCode, true, false);
                    if (CertificateExemptionType != null)
                    {
                        if (!String.IsNullOrWhiteSpace(CertificateExemptionType.LocalName))
                        {
                            acc_suppliersup_item_certificate.CertificateExemptionTypeName = CertificateExemptionType.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(CertificateExemptionType.EnglishName))
                        {
                            acc_suppliersup_item_certificate.CertificateExemptionTypeName = CertificateExemptionType.EnglishName;
                        }
                    }

                    acc_suppliersup_item_certificate.AttachmentTypeCode = supplierInvoiceItemCertificatePM.AttachmentTypeCode;
                    AttachmentTypeQueryService AttachmentTypeQuery = new AttachmentTypeQueryService(context);
                    AttachmentTypePM AttachmentType = AttachmentTypeQuery.GetSingle(supplierInvoiceItemCertificatePM.AttachmentTypeCode, true, false);
                    if (AttachmentType != null)
                    {
                        if (!String.IsNullOrWhiteSpace(AttachmentType.LocalName))
                        {
                            acc_suppliersup_item_certificate.AttachmentTypeName = AttachmentType.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(AttachmentType.EnglishName))
                        {
                            acc_suppliersup_item_certificate.AttachmentTypeName = AttachmentType.EnglishName;
                        }
                    }

                    acc_suppliersup_item_certificate.CustomsAttachmentID = supplierInvoiceItemCertificatePM.CustomsAttachmentID;

                    acc_suppliersup_item_certificate.ResConfirmationTypeCode = supplierInvoiceItemCertificatePM.ResConfirmationTypeCode;
                    ConfirmationTypePM rConfirmationType = ConfirmationTypeQuery.GetSingle(supplierInvoiceItemCertificatePM.ResConfirmationTypeCode, true, false);
                    if (rConfirmationType != null)
                    {
                        if (!String.IsNullOrWhiteSpace(rConfirmationType.LocalName))
                        {
                            acc_suppliersup_item_certificate.ResConfirmationTypeName = rConfirmationType.LocalName;
                        }
                        else if (!String.IsNullOrWhiteSpace(rConfirmationType.EnglishName))
                        {
                            acc_suppliersup_item_certificate.ResConfirmationTypeName = rConfirmationType.EnglishName;
                        }
                    }

                    acc_suppliersup_item_certificateList.Add(acc_suppliersup_item_certificate);
                }
            }
            else // moran 2.9.15 - Task 16086 
            {
                var acc_suppliersup_item_certificate = new SupplierInvioceItemsCertificateM();
                acc_suppliersup_item_certificateList.Add(acc_suppliersup_item_certificate);
            }

            return acc_suppliersup_item_certificateList;
        }


        private List<tax> GetDeclarationTaxes(DeclarationPM dirtyDeclarationPM)
        {
            if (context == null) context = CustomContext.GetContext(dirtyDeclarationPM.Tenant); // moran 10.9.15 - Task 16086

            var declarationTaxList = new List<tax>();

            foreach (var Tax in dirtyDeclarationPM.DeclarationTaxes)
            {
                var declarationTax = new tax();

                declarationTax.taxtaxtype = new List<taxtaxtype>(); declarationTax.taxtaxtype.Add(new taxtaxtype());
                declarationTax.taxtaxtype[0].taxtaxtypeid = Tax.TaxTypeCode;
                // moran 10.9.15 - Task 16086 -->
                //declarationTax.taxtaxtype[0].taxtaxtypename = Tax.TaxTypeName;
                ParagraphTypeQueryService ParagraphTypeQuery = new ParagraphTypeQueryService(context);
                ParagraphTypePM ParagraphType = ParagraphTypeQuery.GetSingle(Tax.TaxTypeCode, true, false);
                if (ParagraphType != null)
                {
                    if (!String.IsNullOrWhiteSpace(ParagraphType.LocalName))
                    {
                        declarationTax.taxtaxtype[0].taxtaxtypename = ParagraphType.LocalName;
                    }
                    else if (!String.IsNullOrWhiteSpace(ParagraphType.EnglishName))
                    {
                        declarationTax.taxtaxtype[0].taxtaxtypename = ParagraphType.EnglishName;
                    }
                }
                // moran 10.9.15 - Task 16086 <--
                if (Tax.TaxBaseAmount.HasValue)
                {
                    declarationTax.taxtax_basis = Tax.TaxBaseAmount.ToString();
                }
                if (Tax.TotalAmount.HasValue)
                {
                    declarationTax.taxtax_amount = Tax.TotalAmount.ToString();
                    declarationTax.taxtax_to_pay = Tax.TotalAmount.ToString();
                }
                if (Tax.DeferredTaxAmount.HasValue)
                {
                    declarationTax.taxpostponed_tax = Tax.DeferredTaxAmount.ToString();
                }
                /*if (Tax.TaxRate.HasValue) // moran 26.8.15 - task 16093 ?
                {
                    declarationTax.taxrate = Tax.Rate.ToString();
                }*/
                declarationTaxList.Add(declarationTax);
            }

            return declarationTaxList;
        }

        public DeclarationSReport GetSingle(int tenant, string declarationId)
        {
            DeclarationPM MyDeclarationPM;
            var context = CustomContext.GetContext(tenant);
            var myQueryService = new DeclarationQueryService(context);
            MyDeclarationPM = myQueryService.GetSingle(
                //"1-190"
                declarationId
                , true, false); // "1-293"
            // DeclarationSRMapping declarationSRMapping = new DeclarationSRMapping();
            //var myFile = declarationSRMapping.Get(MyDeclarationPM);
            var myFile = Get(MyDeclarationPM);
            return myFile;

        }

        public string GetXml(int tenant, string declarationId)
        {

            var myFile = GetSingle(tenant, declarationId);
            var xml = XmlGenericUtil<DeclarationSReport>.SerializeObject(myFile);
            return xml;
        }

        public string GetXml(DeclarationPM myDeclarationPM)
        {

            var myFile = Get(myDeclarationPM);
            var xml = XmlGenericUtil<DeclarationSReport>.SerializeObject(myFile);
            return xml;
        }

    }

    public class CurrencyAmount
    {
        private string currencyField;

        private string amountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("currency")]
        public string currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }
        public string amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }
    }
}