using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrection;
using System.Xml;
using Logitude.Customs.Data.EntityListQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Logitude.CustomsMessaging.Common.Gen;
using System.Diagnostics;
using Logitude.Customs.BL.EntityDataMappings;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.Customs.BL.Validators;
using System.Data.Entity.Infrastructure;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Customs.Def.Messaging.LogitudeClient.DeclarationErrorPointer;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Data.CustomFilters;
//using Logitude.Server.Tools.Helpers;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class DeclarationQueryService : EntityQueryService<Declaration, DeclarationKeys, DeclarationPM, object, DeclarationKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, DeclarationPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;
            ConsignmentQueryService consignmentService = new ConsignmentQueryService(context);
            SupplierInvoiceQueryService supplierInvoiceService = new SupplierInvoiceQueryService(context);
            DeclarationTaxQueryService declarationTaxService = new DeclarationTaxQueryService(context);
            DeclarationConstraintQueryService declarationConstraintQueryService = new EntityQueryServices.DeclarationConstraintQueryService(context);
            DeclarationConsAcceptanceQueryService declarationConsAcceptanceQueryService = new DeclarationConsAcceptanceQueryService(context);
            DecDangersContactQueryService decDangersContactQueryService = new DecDangersContactQueryService(context);
            DeclarationPaymentQueryService declarationPaymentQueryService = new DeclarationPaymentQueryService(context);

            //DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);

            entityPM.InvoiceHasFreight = supplierInvoiceService.DoesAnyInvoiceHasFreight(entityPM.Id, entityPM.Tenant);
            //******getting all compositionTables for response service purposes only *****///
            entityPM.Consignments = consignmentService.GetMulti(declarationKeys, true);
            entityPM.DeclarationPayments = declarationPaymentQueryService.GetMulti(declarationKeys, true,true);

            // if (LoadSupplierInvoices)
            var DeclarationExportRecipientQueryService = new DeclarationExportRecipientQueryService(context);
            entityPM.DeclarationExportRecipients = DeclarationExportRecipientQueryService
                .GetMulti(declarationKeys, false);

            {
                if (loadSupplierInvoicesItemsParentsOnly == true)
                {
                    supplierInvoiceService.OnlyParentItem = true;
                }
                //FeatureQuery featureQuery = new FeatureQuery();
                //var features = featureQuery.GetAllowedFeaturesForLoggedUser(Logitude.Server.Tools.Helpers.AuthenticationUtil.ResolveUserId(entityPM.Tenant), entityPM.Tenant);
                //var featureOcr = features.Features.FirstOrDefault(x => x.Code == "OCR");
                //if (featureOcr != null)
                //{
                //    LoadSupplierInvoicesWithItems = true;
                //}
                //var flag = SecurityUtility.CheckContactFeature();
                entityPM.SupplierInvoices = supplierInvoiceService.GetSupplierInvoicesForDeclaration(declarationKeys.Id, entityPM.Tenant,

                    LoadSupplierInvoicesWithItems || entityPM.IsCourierDeclaration // courier small entity - for Classification !!//Task 40622: מסך סיווג מתוך מסך עבודה - חלק מרכזי

                    );
            }
            entityPM.DeclarationTaxes = declarationTaxService.GetMulti(declarationKeys, true);
            entityPM.DeclarationConstraints = declarationConstraintQueryService.GetMulti(declarationKeys, true);
            entityPM.DeclarationConsAcceptances = declarationConsAcceptanceQueryService.GetMulti(declarationKeys, true);
            entityPM.DecDangersContacts = decDangersContactQueryService.GetMulti(declarationKeys, true);
            //entityPM.DclarationCourierStatus = declarationCourierStatusQueryService.GetMulti(declarationKeys, true);


            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            if (LoadSupplierInvoicesWithItems == true)// Huge errors problem this boolean if false means that the call is from client
            {
                entityPM.DeclarationErrorViews = this.GetDeclarationErrors(declarationKeys.Id, entityPM.Tenant, null);
            }
            else
            {
                // entityPM.ErrosXml = "";
                entityPM.DeclarationErrorViews = new List<DeclarationErrorView>();
            }
            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //bool temp = true;
            //if (temp)
            //{
            //    if (entityPM.DeclarationErrorViews != null && entityPM.DeclarationErrorViews.Count > 1000)
            //    {
            //        entityPM.ErrosXml = "";
            //        entityPM.DeclarationErrorViews = new List<DeclarationErrorView>();
            //    }
            //}
            if (entityPM.DeclarationConstraints.Count > 0)
            {
                foreach (DeclarationConstraintPM item in entityPM.DeclarationConstraints)
                {
                    entityPM.DeclarationConstraintsActiveIds = entityPM.DeclarationConstraintsActiveIds + "," + item.ConstraintNumber;
                }
                entityPM.DeclarationConstraintsActiveIds = entityPM.DeclarationConstraintsActiveIds.TrimStart(',');
            }
            else
            {
                entityPM.DeclarationConstraintsActiveIds = "";
            }

            if (entityPM.DeclarationErrorViews.Count > 0)
            {
                foreach (DeclarationErrorView item in entityPM.DeclarationErrorViews)
                {
                    entityPM.DeclarationErrorViewsActiveIds = entityPM.DeclarationErrorViewsActiveIds + "," + item.ErrorType;
                }
                entityPM.DeclarationErrorViewsActiveIds = entityPM.DeclarationErrorViewsActiveIds.TrimStart(',');
            }
            else
            {
                entityPM.DeclarationErrorViewsActiveIds = "";
            }

            if (entityPM.DeclarationTaxes.Count > 0)
            {
                foreach (DeclarationTaxPM item in entityPM.DeclarationTaxes)
                {
                    entityPM.DeclarationTaxesActiveIds = entityPM.DeclarationTaxesActiveIds + "," + item.TaxTypeCode;
                }
                entityPM.DeclarationTaxesActiveIds = entityPM.DeclarationTaxesActiveIds.TrimStart(',');
            }
            else
            {
                entityPM.DeclarationTaxesActiveIds = "";
            }
            //****************************************************************************//
            if (entityPM.DeclarationConsignments == null)
            {
                entityPM.DeclarationConsignments = new List<DeclarationConsignmentPM>();
            }

            int index = 0;
            foreach (ConsignmentPM item in entityPM.Consignments.OrderBy(o => o.ConsignmentNumber))
            {
                index += 1;

                entityPM.DeclarationConsignments.Add(new DeclarationConsignmentPM()
                {
                    DeclarationId = item.DeclarationId,
                    ConsignmentNumber = item.ConsignmentNumber,
                    SequenceNumeric = index,
                    ManifestNumber = item.ManifestNumber,
                });

                //<--- Yuval Chalup 29.03.2015
                if (item.ConsignmentPackages.Count > 0)
                {
                    foreach (ConsignmentPackagePM consignmentPackage in item.ConsignmentPackages)
                    {
                        string tmp = consignmentPackage.ConsignmentNumber.ToString() + consignmentPackage.SequenceNumeric.ToString() + consignmentPackage.LineNumber.ToString();
                        item.ConsignmentPackagesActiveIds = item.ConsignmentPackagesActiveIds + "," + tmp;
                    }
                    item.ConsignmentPackagesActiveIds = item.ConsignmentPackagesActiveIds.TrimStart(',');
                }
                else
                {
                    item.ConsignmentPackagesActiveIds = "";
                }
                //Yuval Chalup 29.03.2015 --->
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            decimal? PlatfomFee = 0;
            SupplierInvoiceModificationQueryService supplierInvoiceModQueryService = new SupplierInvoiceModificationQueryService(context);
            List<SupplierInvoiceModificationPM> modifications = supplierInvoiceModQueryService.GetSupplierInvoiceModificationsForDeclaration(entityPM.Id);
            entityPM.PlatformFee = modifications.Where(a => a.TypeCode == "I02" || a.TypeCode == "I01").Sum(d => d.Amount);// bug 35337
            //foreach (SupplierInvoiceModificationPM mod in modifications)
            //{
            //    var modificationI02 = modifications.Where(a => a.TypeCode == "I02").FirstOrDefault();
            //    if (modificationI02 != null)
            //    {
            //        PlatfomFee += modifications.Where(a => a.TypeCode == "I02").FirstOrDefault().Amount;
            //    }
            //    else
            //    {
            //        var modificationI01 = modifications.Where(a => a.TypeCode == "I01").FirstOrDefault();
            //        if (modificationI01 != null)
            //        {
            //            PlatfomFee += modifications.Where(a => a.TypeCode == "I01").FirstOrDefault().Amount;
            //        }

            //    }

            //    entityPM.PlatformFee = PlatfomFee;
            //}

            foreach (SupplierInvoicePM item in entityPM.SupplierInvoices)
            {
                if (item.InvoiceCounterKey.ToString() == entityPM.PrimaryInvoiceCounterKey)
                {
                    item.IsPrimarySupplierInvoice = true;
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //// removed by Mohammad because of the performance enhancement replaced by the code above
            //foreach (SupplierInvoicePM item in entityPM.SupplierInvoices)
            //{

            //    List<SupplierInvoiceModificationPM> modifications = (from a in item.SupplierInvoiceModifications
            //                                                         where a.DeclarationId == item.DeclarationId

            //                                                         select a).ToList();


            //    var modificationI02 = modifications.Where(a => a.TypeCode == "I02").FirstOrDefault();
            //    if (modificationI02 != null)
            //    {
            //        PlatfomFee += modifications.Where(a => a.TypeCode == "I02").FirstOrDefault().Amount;
            //    }
            //    else
            //    {
            //        var modificationI01 = modifications.Where(a => a.TypeCode == "I01").FirstOrDefault();
            //        if (modificationI01 != null)
            //        {
            //            PlatfomFee += modifications.Where(a => a.TypeCode == "I01").FirstOrDefault().Amount;
            //        }

            //    }

            //    entityPM.PlatformFee = PlatfomFee;



            //    if (item.InvoiceCounterKey.ToString() == entityPM.PrimaryInvoiceCounterKey)
            //    {
            //        item.IsPrimarySupplierInvoice = true;
            //    }

            //}





            //if ("itzik ask ihab request 20130613 ".Length>0)
            //{
            //    SupplierInvoiceQueryService supplierInvoiceQueryService = new SupplierInvoiceQueryService(context);
            //    entityPM.SupplierInvoices = supplierInvoiceQueryService.GetMulti(declarationKeys, true);

            //}

            //    base.GetComposition(entityKeys);
        }

        public Declaration GetSingleByCustomFileNoFromCache(string customFileNo, int tenant)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return null;
            string entityKeyString = $"GetSingleByCustomFileNoFromCache{customFileNo}";
            var res = CacheManager.GetOrInsertNewObject(entityKeyString, () =>
            {
                return repository.GetDeclarationByCustomFileNo(customFileNo, tenant);
            });
            return res;
        }


        public DeclarationPM GetSingleByCustomFileNo(string customFileNo, int tenant)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return null;
            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            var declaration = repository.GetDeclarationByCustomFileNo(customFileNo, tenant);

            if (declaration == null) return null;


            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);


            return declarationPM;



        }
        public DeclarationPM GetSingleByCustomFileNoOrExportFile(string ExternalEntityReference, int tenant,string ExternalEntityName)
        {
            if (String.IsNullOrWhiteSpace(ExternalEntityReference)) return null;
            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            var declaration = repository.GetDeclarationByCustomFileNoOrExportFile(ExternalEntityReference, tenant, ExternalEntityName);

            if (declaration == null) return null;


            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);


            return declarationPM;



        }


        public DeclarationPM GetSingleByDecNoAndVersion(string decNo, string version, int tenant)
        {
            if (String.IsNullOrWhiteSpace(decNo)) return null;
            if (String.IsNullOrWhiteSpace(version)) return null;

            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            var declaration = repository.GetDeclarationByDecNoAndVersion(decNo, version, tenant);

            if (declaration == null) return null;


            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);


            return declarationPM;



        }


        public List<DeclarationPendingPM> GetDeclarationPendingListPMByDeclarationId(string declarationId, int tenant)
        {
            if (string.IsNullOrWhiteSpace(declarationId))
            {
                return null;
            }

            var myDeclarationPendingQueryService = new DeclarationPendingQueryService(context);
            List<DeclarationPendingPM> MyDeclarationPendingPMList = myDeclarationPendingQueryService.GetDeclarationPendingsByDeclarationId(declarationId, tenant);
            if (MyDeclarationPendingPMList == null)
            {
                return null;
            }
            return MyDeclarationPendingPMList;
        }



        public int GetDeclarationMaxCancelRequestNumber(int tenant, string id)
        {
            DeclarationRepository declarationRepository = new DeclarationRepository(context);
            return declarationRepository.GetDeclarationMaxCancelRequestNumber(tenant);
        }
        public int GetDeclarationMaxAmendmentAndCancelRequestNumber(int tenant, string id)
        {
            DeclarationRepository declarationRepository = new DeclarationRepository(context);
            return declarationRepository.GetDeclarationMaxAmendmentAndCancelRequestNumber(tenant);
        }


        public string GetIdByDeclarationNumber(string declarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationNumber)) return "";
            return repository.GetIdByDeclarationNumber(declarationNumber, tenant);
        }

        public string GetCustomFileNoByDeclarationNumber(string declarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationNumber)) return "";
            return repository.GetCustomFileNoByDeclarationNumber(declarationNumber, tenant);
        }
        public (string id, string direction, string declarationTypeCode) GetMinDeclarationByDeclarationNumber(string declarationNumber, int tenant)
        {

            return repository.GetMinDeclarationByDeclarationNumber(declarationNumber, tenant);
        }

        public DeclarationPM GetDeclarationByfunctionalReferenceID( string functionalReferenceID, string agentFileReferenceID, int tenant, bool isExportClose = false)
        {
            if (String.IsNullOrWhiteSpace(functionalReferenceID)) return null;

            var declaration = repository.GetDeclarationByFunctionalReferenceIDagentFileReferenceID(functionalReferenceID, agentFileReferenceID, tenant, isExportClose);
            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            if (declaration == null)
            {
                var declarations = repository.GetDeclarationByFunctionalReferenceID(functionalReferenceID, tenant, isExportClose);
                if (declarations!=null && declarations.Count() == 1)
                    declaration = declarations[0];
                else
                    return null;
            }
            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);



            return declarationPM;
        }


        public DeclarationPM GetDeclarationNotAmendmentDontDisplayInList(string id, string amendmentOriginalDeclartation, int tenant)
        {

            var declaration = repository.GetDeclarationNotAmendmentDontDisplayInList(id, amendmentOriginalDeclartation, tenant);
            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            if (declaration == null) return null;

            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);



            return declarationPM;
        }


        public DeclarationPM GetAcceptDeclarationAmendment(string id, int tenant)
        {

            var declaration = repository.GetAcceptDeclarationAmendment(id, tenant);
            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            if (declaration == null) return null;

            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);



            return declarationPM;

        }
        public DeclarationPM GetAcceptDeclarationAmendmentWithComp(string id, int tenant)
        {

            var Decid = repository.GetAcceptDeclarationIdAmendment(id, tenant);
            DeclarationPM declarationPM = this.GetSingle(Decid, true, false);
            return declarationPM;

        }



        public DeclarationPM GetWaitingDeclarationAmendmentByCustomsFile(string customFile, int tenant)
        {

            var declaration = repository.GetWaitingDeclarationAmendmentByCustomsFile(customFile, tenant);
            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            if (declaration == null) return null;

            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);



            return declarationPM;

        }

        public DeclarationPM GetAcceptDeclarationAmendmentByCustomsFile(string customFile, int tenant)
        {

            var declaration = repository.GetAcceptDeclarationAmendmentByCustomsFile(customFile, tenant);
            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            if (declaration == null) return null;

            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);



            return declarationPM;

        }


        public string GetIdByCustomFileNo(string customFileNo, int tenant)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            return repository.GetIdByCustomFileNo(customFileNo, tenant);
        }
        public string GetIdByCustomFileNoOrExportFile(string ExternalEntityReference, int tenant)
        {
            if (String.IsNullOrWhiteSpace(ExternalEntityReference)) return "";
            return repository.GetIdByCustomFileNoOrExportFile(ExternalEntityReference, tenant);
        }

        public string GetIdByCustomFileNoAndAmendmentDontDisplayInList(string customFileNo, int tenant , bool AmendmentDontDisplayInList)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            return repository.GetIdByCustomFileNoAndAmendmentDontDisplayInList(customFileNo, tenant, AmendmentDontDisplayInList);
        }






        public string GetIdByExternalDeclarationNumber(string externalDeclarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(externalDeclarationNumber)) return "";
            return repository.GetIdByExternalDeclarationNumber(externalDeclarationNumber, tenant);
        }
        public List<string> GetListByCourierHAWB(string CourierHAWB, int tenant)
        {

            return repository.GetListByCourierHAWB(CourierHAWB, tenant);

        }

        public List<DeclarationErrorView> GetDeclarationErrors(string declarationId, int tenant, string listVersionId, string courierFilter = "Declaration", bool IsAmendmentErrors = false, bool IsExportCloseErrors = false)
        {
            ICustomContext context = MainContext as CustomContext;
            Declaration declaration = Repository.GetSingle(new DeclarationKeys() { Id = declarationId });
            string xmlErrors = declaration.ErrosXml;
            List<DeclarationErrorView> declarationErrors = new List<DeclarationErrorView>();
            DeclarationConstraintQueryService constraintsService = new DeclarationConstraintQueryService(context);
            List<DeclarationConstraintPM> constraints = constraintsService.GetDeclarationConstraintsByDeclrationId(declarationId, tenant);
            UIMessageQueryService uIMessageQueryService = new UIMessageQueryService(context);
            if (courierFilter == "Manifest")
            {
                if (!string.IsNullOrEmpty(declaration.ManifestErrorXml))
                {
                    byte[] errorsByte = Encoding.UTF8.GetBytes(declaration.ManifestErrorXml);
                    MemoryStream memorystream = new MemoryStream(errorsByte);
                    XmlSerializer serializer = new XmlSerializer(typeof(DeclarationError));
                    DeclarationError declarationError = (DeclarationError)serializer.Deserialize(memorystream);



                    foreach (Entity entity in declarationError.Entitites)
                    {

                        List<error> errors = new List<error>();
                        if (listVersionId != null)
                        {
                            errors = entity.EntityErrors.Where(a => a.ListVersionID == listVersionId).ToList();
                        }

                        else
                        {
                            errors = entity.EntityErrors;
                        }

                        foreach (error error in errors)
                        {
                            DeclarationErrorView errorview = new DeclarationErrorView() { Id = Guid.NewGuid().ToString() };
                            errorview.ConstraintId = error.ConstraintID;
                            if (!string.IsNullOrEmpty(errorview.ConstraintId))
                            {
                                errorview.ConstraintIndication = constraints.Where(d => d.ConstraintNumber == errorview.ConstraintId).Any();
                            }
                            errorview.Description = error.MessageError.Replace(',', ';');
                            errorview.ErrorType = error.Code;
                            if (!string.IsNullOrEmpty(error.Code))
                            {
                                UIMessagePM uIMessagePM = uIMessageQueryService.GetUIMessageWithAdditional(error.Code, tenant);
                                if (uIMessagePM != null)
                                {
                                    if (uIMessagePM.Sort == null)
                                    {
                                        uIMessagePM.Sort = 99999999;
                                    }
                                    errorview.Sort = uIMessagePM.Sort;
                                }
                            }
                            errorview.DeclarationId = declaration.Id;
                            errorview.ListVersionId = error.ListVersionID;
                            if (!string.IsNullOrEmpty(entity.Child3Type))
                            {

                                errorview.EntityName = entity.Child3Type;
                                errorview.Line = int.Parse(entity.Child3Sequence);
                                errorview.ParentEntityName = entity.Child2Type;
                                errorview.ParentLine = int.Parse(entity.Child2Sequence);
                                errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence + "," + entity.Child3Sequence;
                            }
                            else if (!string.IsNullOrEmpty(entity.Child2Type))
                            {
                                errorview.EntityName = entity.Child2Type;
                                errorview.Line = int.Parse(entity.Child2Sequence);
                                errorview.ParentEntityName = entity.Child1Type;
                                errorview.ParentLine = int.Parse(entity.Child1Sequence);
                                errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child1Type))
                            {
                                errorview.EntityName = entity.Child1Type;
                                errorview.Line = int.Parse(entity.Child1Sequence);
                                errorview.ParentEntityName = "Declaration";
                                errorview.LineNumber = entity.Child1Sequence;

                            }
                            else
                            {
                                errorview.EntityName = "Declaration";
                            }
                            errorview.TableNameTextCode = "Customs." + errorview.EntityName;
                            declarationErrors.Add(errorview);
                        }


                        List<field> fields = new List<field>();
                        if (listVersionId != null)
                        {
                            fields = entity.FieldErrors.Where(a => a.ListVersionID == listVersionId).ToList();
                        }

                        else
                        {
                            fields = entity.FieldErrors;
                        }

                        foreach (field error in fields)
                        {
                            DeclarationErrorView errorview = new DeclarationErrorView() { Id = Guid.NewGuid().ToString() }; ;
                            errorview.ConstraintId = error.ConstraintID;
                            if (!string.IsNullOrEmpty(errorview.ConstraintId))
                            {
                                errorview.ConstraintIndication = constraints.Where(d => d.ConstraintNumber == errorview.ConstraintId).Any();
                            }
                            errorview.Description = error.MessageError.Replace(',', ';');
                            errorview.ErrorType = error.Code;
                            if (!string.IsNullOrEmpty(error.Code))
                            {
                                UIMessagePM uIMessagePM = uIMessageQueryService.GetUIMessageWithAdditional(error.Code, tenant);
                                if (uIMessagePM != null)
                                {
                                    if (uIMessagePM.Sort == null)
                                    {
                                        uIMessagePM.Sort = 99999999;
                                    }
                                    errorview.Sort = uIMessagePM.Sort;
                                }
                            }
                            errorview.Field = error.Fieldcode;
                            errorview.DeclarationId = declaration.Id;
                            errorview.ListVersionId = error.ListVersionID;
                            if (!string.IsNullOrEmpty(entity.Child3Type))
                            {
                                errorview.EntityName = entity.Child3Type;
                                errorview.Line = int.Parse(entity.Child3Sequence);
                                errorview.ParentEntityName = entity.Child2Type;
                                errorview.ParentLine = int.Parse(entity.Child2Sequence);
                                errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence + "," + entity.Child3Sequence;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child2Type))
                            {
                                errorview.EntityName = entity.Child2Type;
                                errorview.Line = int.Parse(entity.Child2Sequence);
                                errorview.ParentEntityName = entity.Child1Type;
                                errorview.ParentLine = int.Parse(entity.Child1Sequence);
                                errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child1Type))
                            {
                                errorview.EntityName = entity.Child1Type;
                                errorview.Line = int.Parse(entity.Child1Sequence);
                                errorview.ParentEntityName = "Declaration";
                                errorview.LineNumber = entity.Child1Sequence;

                            }
                            else
                            {
                                errorview.EntityName = "Declaration";
                            }
                            if (errorview.EntityName == "SupplierInvioceItemsCertificate")
                            {
                                errorview.EntityName = "SupplierInvioceItemCertificat";
                            }
                            errorview.FieldNameTextCode = errorview.Field != null ? "Customs." + errorview.EntityName + ".F." + errorview.Field : "Customs." + errorview.EntityName;
                            errorview.TableNameTextCode = "Customs." + errorview.EntityName;
                            declarationErrors.Add(errorview);
                        }
                    }
                }

            }
            else if (IsAmendmentErrors || IsExportCloseErrors)
            {
                if (!string.IsNullOrEmpty(declaration.AmendmentErrorXml) || !string.IsNullOrEmpty(declaration.ExportClosedErrorXML))
                {
                    byte[] errorsByte;
                    if (IsExportCloseErrors)
                    {
                        errorsByte = Encoding.UTF8.GetBytes(declaration.ExportClosedErrorXML);
                    }
                    else
                    {
                        errorsByte = Encoding.UTF8.GetBytes(declaration.AmendmentErrorXml);
                    }
                    MemoryStream memorystream = new MemoryStream(errorsByte);
                    XmlSerializer serializer = new XmlSerializer(typeof(DeclarationError));
                    DeclarationError declarationError = (DeclarationError)serializer.Deserialize(memorystream);



                    foreach (Entity entity in declarationError.Entitites)
                    {

                        List<error> errors = new List<error>();
                        if (listVersionId != null)
                        {
                            errors = entity.EntityErrors.Where(a => a.ListVersionID == listVersionId).ToList();
                        }

                        else
                        {
                            errors = entity.EntityErrors;
                        }

                        foreach (error error in errors)
                        {
                            DeclarationErrorView errorview = new DeclarationErrorView() { Id = Guid.NewGuid().ToString() };
                            errorview.ConstraintId = error.ConstraintID;
                            if (!string.IsNullOrEmpty(errorview.ConstraintId))
                            {
                                errorview.ConstraintIndication = constraints.Where(d => d.ConstraintNumber == errorview.ConstraintId).Any();
                            }
                            errorview.Description = error.MessageError.Replace(',', ';');
                            errorview.ErrorType = error.Code;
                            if (!string.IsNullOrEmpty(error.Code))
                            {
                                UIMessagePM uIMessagePM = uIMessageQueryService.GetUIMessageWithAdditional(error.Code, tenant);
                                if (uIMessagePM != null)
                                {
                                    if (uIMessagePM.Sort == null)
                                    {
                                        uIMessagePM.Sort = 99999999;
                                    }
                                    errorview.Sort = uIMessagePM.Sort;
                                }
                            }
                            errorview.DeclarationId = declaration.Id;
                            errorview.ListVersionId = error.ListVersionID;
                            if (!string.IsNullOrEmpty(entity.Child3Type))
                            {

                                errorview.EntityName = entity.Child3Type;
                                errorview.Line = int.Parse(entity.Child3Sequence);
                                errorview.ParentEntityName = entity.Child2Type;
                                errorview.ParentLine = int.Parse(entity.Child2Sequence);
                                errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence + "," + entity.Child3Sequence;
                            }
                            else if (!string.IsNullOrEmpty(entity.Child2Type))
                            {
                                errorview.EntityName = entity.Child2Type;
                                errorview.Line = int.Parse(entity.Child2Sequence);
                                errorview.ParentEntityName = entity.Child1Type;
                                errorview.ParentLine = int.Parse(entity.Child1Sequence);
                                errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child1Type))
                            {
                                errorview.EntityName = entity.Child1Type;
                                errorview.Line = int.Parse(entity.Child1Sequence);
                                errorview.ParentEntityName = "Declaration";
                                errorview.LineNumber = entity.Child1Sequence;

                            }
                            else
                            {
                                errorview.EntityName = "Declaration";
                            }
                            errorview.TableNameTextCode = "Customs." + errorview.EntityName;
                            declarationErrors.Add(errorview);
                        }


                        List<field> fields = new List<field>();
                        if (listVersionId != null)
                        {
                            fields = entity.FieldErrors.Where(a => a.ListVersionID == listVersionId).ToList();
                        }

                        else
                        {
                            fields = entity.FieldErrors;
                        }

                        foreach (field error in fields)
                        {
                            DeclarationErrorView errorview = new DeclarationErrorView() { Id = Guid.NewGuid().ToString() }; ;
                            errorview.ConstraintId = error.ConstraintID;
                            if (!string.IsNullOrEmpty(errorview.ConstraintId))
                            {
                                errorview.ConstraintIndication = constraints.Where(d => d.ConstraintNumber == errorview.ConstraintId).Any();
                            }
                            errorview.Description = error.MessageError.Replace(',', ';');
                            errorview.ErrorType = error.Code;
                            if (!string.IsNullOrEmpty(error.Code))
                            {
                                UIMessagePM uIMessagePM = uIMessageQueryService.GetUIMessageWithAdditional(error.Code, tenant);
                                if (uIMessagePM != null)
                                {
                                    if (uIMessagePM.Sort == null)
                                    {
                                        uIMessagePM.Sort = 99999999;
                                    }
                                    errorview.Sort = uIMessagePM.Sort;
                                }
                            }
                            errorview.Field = error.Fieldcode;
                            errorview.DeclarationId = declaration.Id;
                            errorview.ListVersionId = error.ListVersionID;
                            if (!string.IsNullOrEmpty(entity.Child3Type))
                            {
                                errorview.EntityName = entity.Child3Type;
                                errorview.Line = int.Parse(entity.Child3Sequence);
                                errorview.ParentEntityName = entity.Child2Type;
                                errorview.ParentLine = int.Parse(entity.Child2Sequence);
                                errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence + "," + entity.Child3Sequence;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child2Type))
                            {
                                errorview.EntityName = entity.Child2Type;
                                errorview.Line = int.Parse(entity.Child2Sequence);
                                errorview.ParentEntityName = entity.Child1Type;
                                errorview.ParentLine = int.Parse(entity.Child1Sequence);
                                errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child1Type))
                            {
                                errorview.EntityName = entity.Child1Type;
                                errorview.Line = int.Parse(entity.Child1Sequence);
                                errorview.ParentEntityName = "Declaration";
                                errorview.LineNumber = entity.Child1Sequence;

                            }
                            else
                            {
                                errorview.EntityName = "Declaration";
                            }
                            if (errorview.EntityName == "SupplierInvioceItemsCertificate")
                            {
                                errorview.EntityName = "SupplierInvioceItemCertificat";
                            }
                            errorview.FieldNameTextCode = errorview.Field != null ? "Customs." + errorview.EntityName + ".F." + errorview.Field : "Customs." + errorview.EntityName;
                            errorview.TableNameTextCode = "Customs." + errorview.EntityName;
                            declarationErrors.Add(errorview);
                        }
                    }
                }

            }
            else if (!string.IsNullOrEmpty(xmlErrors))
            {

#if false
                XDocument document = XDocument.Parse(xmlErrors);
                var entities = document.Elements().Elements().Elements();
                
                foreach (var entity in entities)
                {

                    var child1Type = (from a in entity.Elements()
                                      where a.Name == "Child1Type"
                                      select a).FirstOrDefault();
                    string child1Value = child1Type.Value;

                    var child2Type = (from a in entity.Elements()
                                      where a.Name == "Child2Type"
                                      select a).FirstOrDefault();
                    string child2Value = child2Type.Value;

                    var child3Type = (from a in entity.Elements()
                                      where a.Name == "Child3Type"
                                      select a).FirstOrDefault();
                    string child3Value = child3Type.Value;

                    var child1Sequence = (from a in entity.Elements()
                                          where a.Name == "Child1Sequence"
                                          select a).FirstOrDefault();
                    string child1SequenceValue = child1Sequence.Value;

                    var child2Sequence = (from a in entity.Elements()
                                          where a.Name == "Child2Sequence"
                                          select a).FirstOrDefault();
                    string child2SequenceValue = child2Sequence.Value;

                    var child3Sequence = (from a in entity.Elements()
                                          where a.Name == "Child3Sequence"
                                          select a).FirstOrDefault();
                    string child3SequenceValue = child3Sequence.Value;

                    var entityErrorsElement = from a in entity.Elements()
                                              where a.Name == "EntityErrors"
                                              select a;
                    var entityErrors = from a in entityErrorsElement.Elements()
                                       where a.Name == "Error"
                                       select a;

                    var fieldErrorsElement = from a in entity.Elements()
                                             where a.Name == "FieldErrors"
                                             select a;
                    var fieldErrors = from a in fieldErrorsElement.Elements()
                                      where a.Name == "Field"
                                      select a;

                    foreach (var entityError in entityErrors)
                    {
                        DeclarationErrorView error = new DeclarationErrorView();
                        if (!string.IsNullOrEmpty(child3Value))
                        {
                            error.EntityName = child3Value;
                            error.Line = int.Parse(child3SequenceValue);
                        }
                        else if (!string.IsNullOrEmpty(child2Value))
                        {
                            error.EntityName = child2Value;
                            error.Line = int.Parse(child2SequenceValue);
                        }
                        else if (!string.IsNullOrEmpty(child1Value))
                        {
                            error.EntityName = child1Value;
                            error.Line = int.Parse(child1SequenceValue);
                        }
                        else
                        {
                            error.EntityName = "Declaration";
                        }
                        error.Description = (from a in entityError.Elements()
                                             where a.Name == "Message"
                                             select a).FirstOrDefault().Value;
                        error.ErrorType = (from a in entityError.Elements()
                                           where a.Name == "Code"
                                           select a).FirstOrDefault().Value;
                        declarationErrors.Add(error);
                    }

                    foreach (var entityError in fieldErrors)
                    {
                        DeclarationErrorView error = new DeclarationErrorView();
                        if (!string.IsNullOrEmpty(child3Value))
                        {
                            error.EntityName = child3Value;
                            error.Line = int.Parse(child3SequenceValue);
                        }
                        else if (!string.IsNullOrEmpty(child2Value))
                        {
                            error.EntityName = child2Value;
                            error.Line = int.Parse(child2SequenceValue);
                        }
                        else if (!string.IsNullOrEmpty(child1Value))
                        {
                            error.EntityName = child1Value;
                            error.Line = int.Parse(child1SequenceValue);
                        }
                        else
                        {
                            error.EntityName = "Declaration";
                        }
                        error.Description = (from a in entityError.Elements()
                                             where a.Name == "Message"
                                             select a).FirstOrDefault().Value;
                        error.ErrorType = (from a in entityError.Elements()
                                           where a.Name == "Code"
                                           select a).FirstOrDefault().Value;
                        error.Field = (from a in entityError.Elements()
                                       where a.Name == "FieldCode"
                                       select a).FirstOrDefault().Value;

                        declarationErrors.Add(error);
                    }

                }

            }
                
#endif
                byte[] errorsByte = Encoding.UTF8.GetBytes(xmlErrors);
                MemoryStream memorystream = new MemoryStream(errorsByte);
                XmlSerializer serializer = new XmlSerializer(typeof(DeclarationError));
                DeclarationError declarationError = (DeclarationError)serializer.Deserialize(memorystream);




                foreach (Entity entity in declarationError.Entitites)
                {

                    List<error> errors = new List<error>();
                    if (listVersionId != null)
                    {
                        errors = entity.EntityErrors.Where(a => a.ListVersionID == listVersionId).ToList();
                    }

                    else
                    {
                        errors = entity.EntityErrors;
                    }

                    foreach (error error in errors)
                    {
                        DeclarationErrorView errorview = new DeclarationErrorView() { Id = Guid.NewGuid().ToString() };
                        errorview.ConstraintId = error.ConstraintID;
                        if (!string.IsNullOrEmpty(errorview.ConstraintId))
                        {
                            errorview.ConstraintIndication = constraints.Where(d => d.ConstraintNumber == errorview.ConstraintId).Any();
                        }
                        errorview.Description = error.MessageError.Replace(',', ';');
                        errorview.ErrorType = error.Code;
                        if (!string.IsNullOrEmpty(error.Code))
                        {
                            UIMessagePM uIMessagePM = uIMessageQueryService.GetUIMessageWithAdditional(error.Code, tenant);
                            if (uIMessagePM != null)
                            {
                                if (uIMessagePM.Sort == null)
                                {
                                    uIMessagePM.Sort = 99999999;
                                }
                                errorview.Sort = uIMessagePM.Sort;
                            }
                        }
                        errorview.DeclarationId = declaration.Id;
                        errorview.ListVersionId = error.ListVersionID;
                        if (!string.IsNullOrEmpty(entity.Child3Type))
                        {

                            errorview.EntityName = entity.Child3Type;
                            errorview.Line = int.Parse(entity.Child3Sequence);
                            errorview.ParentEntityName = entity.Child2Type;
                            errorview.ParentLine = int.Parse(entity.Child2Sequence);
                            errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence + "," + entity.Child3Sequence;
                        }
                        else if (!string.IsNullOrEmpty(entity.Child2Type))
                        {
                            errorview.EntityName = entity.Child2Type;
                            errorview.Line = int.Parse(entity.Child2Sequence);
                            errorview.ParentEntityName = entity.Child1Type;
                            errorview.ParentLine = int.Parse(entity.Child1Sequence);
                            errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence;

                        }
                        else if (!string.IsNullOrEmpty(entity.Child1Type))
                        {
                            errorview.EntityName = entity.Child1Type;
                            errorview.Line = int.Parse(entity.Child1Sequence);
                            errorview.ParentEntityName = "Declaration";
                            errorview.LineNumber = entity.Child1Sequence;

                        }
                        else
                        {
                            errorview.EntityName = "Declaration";
                        }
                        if (declaration.Direction == "E" && errorview.EntityName == "Declaration")
                        {
                            errorview.TableNameTextCode = "Customs.Declaration.O.Export";
                            //  errorview.EntityName = "Customs.Declaration.O.Export";

                        }
                        else
                            errorview.TableNameTextCode = "Customs." + errorview.EntityName;
                        declarationErrors.Add(errorview);
                    }


                    List<field> fields = new List<field>();
                    if (listVersionId != null)
                    {
                        fields = entity.FieldErrors.Where(a => a.ListVersionID == listVersionId).ToList();
                    }

                    else
                    {
                        fields = entity.FieldErrors;
                    }

                    foreach (field error in fields)
                    {
                        DeclarationErrorView errorview = new DeclarationErrorView() { Id = Guid.NewGuid().ToString() }; ;
                        errorview.ConstraintId = error.ConstraintID;
                        if (!string.IsNullOrEmpty(errorview.ConstraintId))
                        {
                            errorview.ConstraintIndication = constraints.Where(d => d.ConstraintNumber == errorview.ConstraintId).Any();
                        }
                        errorview.Description = error.MessageError.Replace(',', ';');
                        errorview.ErrorType = error.Code;
                        if (!string.IsNullOrEmpty(error.Code))
                        {
                            UIMessagePM uIMessagePM = uIMessageQueryService.GetUIMessageWithAdditional(error.Code, tenant);
                            if (uIMessagePM != null)
                            {
                                if (uIMessagePM.Sort == null)
                                {
                                    uIMessagePM.Sort = 99999999;
                                }
                                errorview.Sort = uIMessagePM.Sort;
                            }
                        }
                        errorview.Field = error.Fieldcode;
                        errorview.DeclarationId = declaration.Id;
                        errorview.ListVersionId = error.ListVersionID;
                        if (!string.IsNullOrEmpty(entity.Child3Type))
                        {
                            errorview.EntityName = entity.Child3Type;
                            errorview.Line = int.Parse(entity.Child3Sequence);
                            errorview.ParentEntityName = entity.Child2Type;
                            errorview.ParentLine = int.Parse(entity.Child2Sequence);
                            errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence + "," + entity.Child3Sequence;

                        }
                        else if (!string.IsNullOrEmpty(entity.Child2Type))
                        {
                            errorview.EntityName = entity.Child2Type;
                            errorview.Line = int.Parse(entity.Child2Sequence);
                            errorview.ParentEntityName = entity.Child1Type;
                            errorview.ParentLine = int.Parse(entity.Child1Sequence);
                            errorview.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence;

                        }
                        else if (!string.IsNullOrEmpty(entity.Child1Type))
                        {
                            errorview.EntityName = entity.Child1Type;
                            errorview.Line = int.Parse(entity.Child1Sequence);
                            errorview.ParentEntityName = "Declaration";
                            errorview.LineNumber = entity.Child1Sequence;

                        }
                        else
                        {
                            errorview.EntityName = "Declaration";
                        }
                        if (errorview.EntityName == "SupplierInvioceItemsCertificate")
                        {
                            errorview.EntityName = "SupplierInvioceItemCertificat";
                            if (errorview.Field == "ClassificationCode")
                            {
                                errorview.EntityName = "SupplierInvoiceItem";

                            }
                        }

                        if (errorview.Field == "CargoTypeCode" && declaration.Direction == "E")
                        {
                            errorview.FieldNameTextCode = "Customs.Declaration.O.CargoTypeCode";

                        }
                        else
                        {
                            errorview.FieldNameTextCode = errorview.Field != null ? "Customs." + errorview.EntityName + ".F." + errorview.Field : "Customs." + errorview.EntityName;

                        }

                        if (declaration.Direction == "E" && errorview.EntityName == "Declaration")
                        {
                            errorview.TableNameTextCode = "Customs.Declaration.O.Export";
                            errorview.FieldNameTextCode = "Customs.Declaration.O.Export";

                        }
                        else
                            errorview.TableNameTextCode = "Customs." + errorview.EntityName;
                        declarationErrors.Add(errorview);
                    }
                }
            }
            return declarationErrors;

        }

        public List<DeclarationList> GetTapagDeclarations(string tapagId, int tenant)
        {



            ICustomContext context = MainContext as CustomContext;
            TapagConnectionTableRepository connectionRep = new TapagConnectionTableRepository(context);
            List<TapagConnectionTable> connections = connectionRep.GetDearationTapagConnectionTables(null, tapagId, tenant);

            List<string> declarationIds = (from a in connections select a.DeclarationId).ToList();

            List<Declaration> declarations = repository.GetDeclarationsById(declarationIds);


            List<DeclarationList> declarationLists = new List<DeclarationList>();
            foreach (Declaration item in declarations)
            {
                TapagConnectionTable connection = (from a in connections
                                                   where a.DeclarationId == item.Id
                                                   select a).FirstOrDefault();

                DeclarationList declarationList = new DeclarationList()
                {

                    Id = item.Id,
                    Tenant = item.Tenant,
                    CustomFileNo = item.CustomFileNo,
                    PaymentDate = item.PaymentDate,
                    DeclarationNumber = item.DeclarationNumber,
                    TotalTax = item.TotalTax,
                    // CustomsTapagNumeral = connection.CustomsTapagFile + "/" + connection.CustomsNumeral,
                    CustomsTapagFile = connection.CustomsTapagFile,
                    CustomsNumeral = connection.CustomsNumeral,
                    RequestFileNumber = connection.RequestFileNumber,



                };

                if (!string.IsNullOrEmpty(connection.CustomsTapagFile) & connection.CustomsNumeral != null)
                {
                    declarationList.CustomsTapagNumeral = connection.CustomsTapagFile + "-" + connection.CustomsNumeral;
                }

                else if (string.IsNullOrEmpty(connection.CustomsTapagFile) & connection.CustomsNumeral != null)
                {
                    declarationList.CustomsTapagNumeral = connection.CustomsNumeral.ToString();
                }

                else if (!string.IsNullOrEmpty(connection.CustomsTapagFile) & connection.CustomsNumeral == null)
                {
                    declarationList.CustomsTapagNumeral = connection.CustomsTapagFile;
                }

                declarationLists.Add(declarationList);
            }
            return declarationLists;

        }

        public List<SupplierInvoicePM> GetInvoicesByDeclaration(string declarationId, int tenant)
        {
            List<SupplierInvoice> invoices = repository.GetSupplierInvoicesByDeclaration(declarationId, tenant);
            List<SupplierInvoicePM> invoicePMs = new List<SupplierInvoicePM>();
            foreach (SupplierInvoice a in invoices)
            {
                SupplierInvoicePM invoicePM = new SupplierInvoicePM()
                {
                    DeclarationId = a.DeclarationId,
                    Tenant = a.Tenant,
                    InvoiceCounterKey = a.InvoiceCounterKey,
                    InvoiceCurrencyTypeCode = a.InvoiceCurrencyTypeCode,
                    ExchangeRate = a.ExchangeRate


                };
                invoicePMs.Add(invoicePM);
            }



            return invoicePMs;
        }

        //<--- Yuval Chalup 22.10.2014 TASK-6711
        public DeclarationPM GetDeclarationPMByCargoIdentifiers(string cargoTypeCode, string manifestNumber, string secondCargoID, int tenant)
        {
            if (string.IsNullOrWhiteSpace(cargoTypeCode) || string.IsNullOrWhiteSpace(manifestNumber) || string.IsNullOrWhiteSpace(secondCargoID))
            {
                return null;
            }

            var myConsignmentQueryService = new ConsignmentQueryService(context);
            var consignment = myConsignmentQueryService.GetConsignmentByIdentifiers(cargoTypeCode, manifestNumber, secondCargoID, tenant);
            if (consignment == null)
            {
                return null;
            }
            return this.GetSingle(consignment.DeclarationId, false, false);

        }


        public List<ConsignmentPM> GetConsignmentListPMByDeclarationId(string DeclarationId, int tenant)
        {
            if (string.IsNullOrWhiteSpace(DeclarationId))
            {
                return null;
            }

            var myConsignmentQueryService = new ConsignmentQueryService(context);
            DeclarationKeys declarationKeys = new DeclarationKeys();
            declarationKeys.Id = DeclarationId;
            List<ConsignmentPM> MyConsignmentPMList = myConsignmentQueryService.GetMulti(declarationKeys, false);
            if (MyConsignmentPMList == null)
            {
                return null;
            }
            return MyConsignmentPMList;
        }
        //Yuval Chalup 22.10.2014 TASK-6711 --->

        public string GetCustomFileNoByDeclarationId(string declarationId, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationId)) return "";
            return repository.GetCustomFileNoByDeclarationId(declarationId, tenant);
        }
        public Declaration GetDeclarationByConsignment(string cargoTypeCode, string manifestNumber, string secondCargoID, string thirdCargoID)
        {
            return repository.GetDeclarationByConsignment(cargoTypeCode, manifestNumber, secondCargoID, thirdCargoID);
        }

        public List<DeclarationPM> GetMultiByKeys(int tenant, List<string> keys)
        {
            var q = this.repository.GetAll(tenant)
                .Where(rec => keys.Contains(rec.Id));

            var pocos = q.ToList();
            var pmList = pocos.Select(poco => this.GetEntityPM(poco, false, null))
               .ToList();
            return pmList;
        }
        public List<DeclarationPM> GetLoadTest(int tenant, int top, List<string> keys = null)
        {
            var lst30 = DateTime.Now.Subtract(TimeSpan.FromDays(131));
            var q = this.repository.GetAll(tenant)
                .Where(rec =>
                   ///rec.Tenant == tenant &&
                   //rec.CreateDateTime.Value > lst30 &&
                   rec.UpdateDateTime.Value > lst30
                   );
            if (keys != null)
            {
                q = q.Where(rec => keys.Contains(rec.Id));
            }
            var pocos = q
                    .Take(top).ToList();
            var pmList = pocos.Select(poco => this.GetEntityPM(poco, false, null))
               .ToList();
            return pmList;
        }

        public DeclarationCorrectionView GetDeclarationCorrection(string declarationId, int tenant,bool isExportClose)
        {
            ICustomContext context = MainContext as CustomContext;
            Declaration declaration = Repository.GetSingle(new DeclarationKeys() { Id = declarationId });
            string CorrectionXML;
            if (isExportClose)
            {
                 CorrectionXML = declaration.ClosingXml;
            }
            else
            {
                 CorrectionXML = declaration.CorrectionsXml;
            }

            List<DeclarationStatementTypeList> statementTypes = new List<DeclarationStatementTypeList>();
            DeclarationStatementTypeListQueryService declarationStatementTypeQueryService = new DeclarationStatementTypeListQueryService(context);
            statementTypes = declarationStatementTypeQueryService.GetList(tenant);
            DeclarationCorrectionView correctionView = null;


            if (!string.IsNullOrEmpty(CorrectionXML))
            {
                byte[] errorsByte = Encoding.UTF8.GetBytes(CorrectionXML);
                MemoryStream memorystream = new MemoryStream(errorsByte);
                XmlSerializer serializer = new XmlSerializer(typeof(DeclarationCorrection));
                DeclarationCorrection declarationCorrection = (DeclarationCorrection)serializer.Deserialize(memorystream);
                correctionView = new DeclarationCorrectionView() { Id = Guid.NewGuid().ToString(), DeclarationId = declaration.Id };
                correctionView.GeneralDataViews = new List<GeneralDataView>();



                foreach (General item in declarationCorrection.GeneralData)
                {
                    GeneralDataView generalData = new GeneralDataView();
                    generalData.AdditionalInformation = new List<AdditionalInformationView>();
                    generalData.AmendmentViews = new List<AmendmentView>();
                    generalData.CorrectionDate = item.IssueDateTime;
                    generalData.Version = item.VersionId;
                    generalData.SystemMessageViews = new List<error>();
                    generalData.ReferenceViews = new List<ReferenceView>();

                    foreach (Additional additional in item.AdditionalInformation)
                    {
                        AdditionalInformationView information = new AdditionalInformationView();
                        information.Content = additional.Content;
                        DeclarationStatementTypeList statement = statementTypes.Where(d => d.Code == additional.StatementTypeCode).FirstOrDefault();
                        if (statement != null)
                        {
                            information.StatmentName = statement.LocalName;
                        }

                        if (additional.StatementTypeCode != "27")
                            generalData.AdditionalInformation.Add(information);


                    }


                    foreach (Reference reference in item.References)
                    {
                        ReferenceView referenceView = new ReferenceView();
                        referenceView.Remarks = reference.Remarks;
                        referenceView.RefernceID = reference.RefernceID;

                        LogisticsReferenceTypeQueryService logisticsReferenceTypeQueryService = new LogisticsReferenceTypeQueryService(tenant);
                        LogisticsReferenceTypePM logisticsReferenceType = logisticsReferenceTypeQueryService.GetSingle(reference.ReferenceType, false, false);
                        if (logisticsReferenceType != null)
                        {
                            referenceView.ReferenceTypeName = logisticsReferenceType.LocalName;
                        }

                        ReferenceStatusQueryService referenceStatusQueryService = new ReferenceStatusQueryService(tenant);
                        ReferenceStatusPM referenceStatus = referenceStatusQueryService.GetSingle(reference.RefernceStatus, false, false);
                        if (referenceStatus != null)
                        {
                            referenceView.RefernceStatusName = referenceStatus.LocalName;
                        }



                        ReferenceInputTypeQueryService referenceInputTypeQueryService = new ReferenceInputTypeQueryService(tenant);
                        ReferenceInputTypePM referenceInputType = referenceInputTypeQueryService.GetSingle(reference.RefernceInputType, false, false);
                        if (referenceInputType != null)
                        {
                            referenceView.RefernceInputTypeName = referenceInputType.LocalName;
                        }

                        generalData.ReferenceViews.Add(referenceView);

                    }

                    foreach (Entity entity in item.Amendments)
                    {
                        foreach (field field in entity.FieldErrors)
                        {
                            AmendmentView amendment = new AmendmentView();
                            if (!string.IsNullOrEmpty(entity.Child3Type))
                            {
                                amendment.EntityName = entity.Child3Type;
                                if (!string.IsNullOrEmpty(entity.Child3Sequence))
                                {
                                    amendment.Line = int.Parse(entity.Child3Sequence);
                                }
                                amendment.ParentEntityName = entity.Child2Type;
                                if (!string.IsNullOrEmpty(entity.Child2Sequence))
                                {
                                    amendment.Line = int.Parse(entity.Child2Sequence);
                                }
                                amendment.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence + "," + entity.Child3Sequence;
                                amendment.ErrorType = field.Code;
                                amendment.Field = field.Fieldcode;
                                amendment.MessageError = field.MessageError;
                                amendment.OldValue = field.OldValue;
                                amendment.NewValue = field.NewValue;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child2Type))
                            {
                                amendment.EntityName = entity.Child2Type;
                                if (!string.IsNullOrEmpty(entity.Child2Sequence))
                                {
                                    amendment.Line = int.Parse(entity.Child2Sequence);
                                }
                                amendment.ParentEntityName = entity.Child1Type;
                                if (!string.IsNullOrEmpty(entity.Child1Sequence))
                                {
                                    amendment.Line = int.Parse(entity.Child1Sequence);
                                }
                                amendment.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence;
                                amendment.ErrorType = field.Code;
                                amendment.Field = field.Fieldcode;
                                amendment.MessageError = field.MessageError;
                                amendment.OldValue = field.OldValue;
                                amendment.NewValue = field.NewValue; ;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child1Type))
                            {
                                amendment.EntityName = entity.Child1Type;
                                if (!string.IsNullOrEmpty(entity.Child1Sequence))
                                {
                                    amendment.Line = int.Parse(entity.Child1Sequence);
                                }
                                amendment.ParentEntityName = "Declaration";
                                amendment.LineNumber = entity.Child1Sequence;
                                amendment.ErrorType = field.Code;
                                amendment.Field = field.Fieldcode;
                                amendment.MessageError = field.MessageError;
                                amendment.OldValue = field.OldValue;
                                amendment.NewValue = field.NewValue;

                            }
                            else
                            {
                                amendment.EntityName = "Declaration";
                                amendment.ErrorType = field.Code;
                                amendment.Field = field.Fieldcode;
                                amendment.MessageError = field.MessageError;
                                amendment.OldValue = field.OldValue;
                                amendment.NewValue = field.NewValue;
                            }
                            amendment.FieldNameTextCode = "Customs." + amendment.EntityName + ".F." + amendment.Field;
                            amendment.TableNameTextCode = "Customs." + amendment.EntityName;
                            AmendmentFieldStatusTypeQueryService amendmentFieldStatusTypeQueryService = new AmendmentFieldStatusTypeQueryService(tenant);
                            AmendmentFieldStatusTypePM amendmentFieldStatusTypePM = amendmentFieldStatusTypeQueryService.GetSingle(field.AmendmentFieldStatus, false, true);
                            if (amendmentFieldStatusTypePM != null)
                            {
                                amendment.AmendmentFieldStatus = amendmentFieldStatusTypePM.LocalName;

                            }


                            AmendCancellRequestInitiatorQueryService amendCancellRequestInitiatorQueryService = new AmendCancellRequestInitiatorQueryService(tenant);
                            AmendCancellRequestInitiatorPM amendCancellRequestInitiatorPM = amendCancellRequestInitiatorQueryService.GetSingle(field.AmendmentRequestInitiatorType, false, true);
                            if (amendCancellRequestInitiatorPM != null)
                            {
                                amendment.AmendmentRequestInitiatorType = amendCancellRequestInitiatorPM.LocalName;

                            }

                            amendment.FieldAmendmentRejectReasonRemarks = amendment.FieldAmendmentRejectReasonRemarks;
                            generalData.AmendmentViews.Add(amendment);

                        }


                        foreach (error error in entity.EntityErrors)
                        {
                            AmendmentView amendment = new AmendmentView();
                            if (!string.IsNullOrEmpty(entity.Child3Type))
                            {
                                amendment.EntityName = entity.Child3Type;
                                if (!string.IsNullOrEmpty(entity.Child3Sequence))
                                {
                                    amendment.Line = int.Parse(entity.Child3Sequence);
                                }
                                amendment.ParentEntityName = entity.Child2Type;
                                if (!string.IsNullOrEmpty(entity.Child2Sequence))
                                {
                                    amendment.ParentLine = int.Parse(entity.Child2Sequence);
                                }

                                amendment.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence + "," + entity.Child3Sequence;
                                amendment.ErrorType = error.Code;
                                amendment.MessageError = error.MessageError;
                                amendment.OldValue = error.OldValue;
                                amendment.NewValue = error.NewValue;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child2Type))
                            {
                                amendment.EntityName = entity.Child2Type;
                                if (!string.IsNullOrEmpty(entity.Child2Sequence))
                                {
                                    amendment.Line = int.Parse(entity.Child2Sequence);
                                }

                                amendment.ParentEntityName = entity.Child1Type;
                                if (!string.IsNullOrEmpty(entity.Child1Sequence))
                                {
                                    amendment.ParentLine = int.Parse(entity.Child1Sequence);
                                }

                                amendment.LineNumber = entity.Child1Sequence + "," + entity.Child2Sequence;
                                amendment.ErrorType = error.Code;
                                amendment.MessageError = error.MessageError;
                                amendment.OldValue = error.OldValue;
                                amendment.NewValue = error.NewValue;

                            }
                            else if (!string.IsNullOrEmpty(entity.Child1Type))
                            {
                                amendment.EntityName = entity.Child1Type;
                                if (!string.IsNullOrEmpty(entity.Child1Sequence))
                                {
                                    amendment.Line = int.Parse(entity.Child1Sequence);
                                }
                                amendment.ParentEntityName = "Declaration";
                                amendment.LineNumber = entity.Child1Sequence;
                                amendment.ErrorType = error.Code;
                                amendment.MessageError = error.MessageError;
                                amendment.OldValue = error.OldValue;
                                amendment.NewValue = error.NewValue;

                            }
                            else
                            {
                                amendment.ErrorType = error.Code;

                                amendment.MessageError = error.MessageError;
                                amendment.OldValue = error.OldValue;
                                amendment.NewValue = error.NewValue;
                                amendment.EntityName = "Declaration";
                            }
                            amendment.FieldNameTextCode = "Customs." + amendment.EntityName + ".F." + amendment.Field;
                            amendment.TableNameTextCode = "Customs." + amendment.EntityName;
                            AmendmentFieldStatusTypeQueryService amendmentFieldStatusTypeQueryService = new AmendmentFieldStatusTypeQueryService(tenant);
                            AmendmentFieldStatusTypePM amendmentFieldStatusTypePM = amendmentFieldStatusTypeQueryService.GetSingle(error.AmendmentFieldStatus, false, true);
                            if (amendmentFieldStatusTypePM != null)
                            {
                                amendment.AmendmentFieldStatus = amendmentFieldStatusTypePM.LocalName;

                            }

                            AmendCancellRequestInitiatorQueryService amendCancellRequestInitiatorQueryService = new AmendCancellRequestInitiatorQueryService(tenant);
                            AmendCancellRequestInitiatorPM amendCancellRequestInitiatorPM = amendCancellRequestInitiatorQueryService.GetSingle(error.AmendmentRequestInitiatorType, false, true);
                            if (amendCancellRequestInitiatorPM != null)
                            {
                                amendment.AmendmentRequestInitiatorType = amendCancellRequestInitiatorPM.LocalName;

                            }

                            amendment.FieldAmendmentRejectReasonRemarks = error.FieldAmendmentRejectReasonRemarks;

                            generalData.AmendmentViews.Add(amendment);

                        }
                    }

                    foreach (error errorItem in item.SystemMessages)
                    {
                        error systemMessagesError = new error();
                        systemMessagesError.Code = errorItem.Code;
                        systemMessagesError.ListVersionID = errorItem.ListVersionID;
                        systemMessagesError.MessageError = errorItem.MessageError;
                        generalData.SystemMessageViews.Add(systemMessagesError);

                    }

                    correctionView.GeneralDataViews.Add(generalData);

                }



                #region oldCode
                // DeclarationCorrectionView correctionView = new DeclarationCorrectionView() { Id = Guid.NewGuid().ToString() };

                // List<GeneralDataView> generalData = new List<GeneralDataView>();
                // GeneralDataView generalDataView = new GeneralDataView();

                // List<AdditionalInformationView> additionalInformation = new List<AdditionalInformationView>();
                // AdditionalInformationView additionalInformationView = new AdditionalInformationView();

                // List<AmendmentView> Amendments = new List<AmendmentView>();
                // AmendmentView amendmentView = new AmendmentView();


                // XmlDocument doc = new XmlDocument();
                // doc.LoadXml(CorrectionXML);

                // XmlNodeList generalDataNodes = doc.DocumentElement.SelectNodes("/DeclarationCorrection/GeneralData");

                // foreach (XmlNode generalDataNode in generalDataNodes)
                // {
                //     if (generalDataNode.SelectSingleNode("IssueDateTime") != null)
                //     {
                //         generalDataView.CorrectionDate = generalDataNode.SelectSingleNode("IssueDateTime").InnerText;
                //     }

                //     if (generalDataNode.SelectSingleNode("VersionId") != null)
                //     {
                //         generalDataView.Version = generalDataNode.SelectSingleNode("VersionId").InnerText;
                //     }

                //      generalData.Add(generalDataView);
                // }

                // XmlNodeList additionalNodes = doc.DocumentElement.SelectNodes("/DeclarationCorrection/AdditionalInformation/Additional");

                // foreach (XmlNode additionalNode in additionalNodes)
                // {
                //     if (additionalNode.SelectSingleNode("StatementTypeCode") != null)
                //     {
                //         string statementTypeCode = additionalNode.SelectSingleNode("StatementTypeCode").InnerText;
                //         DeclarationStatementTypeList statement = statementTypes.Where(d => d.Code == statementTypeCode).FirstOrDefault();
                //         if (statement != null)
                //         {
                //             additionalInformationView.StatmentName = statement.LocalName;
                //         }
                //     }

                //     if (additionalNode.SelectSingleNode("Content") != null)
                //     {

                //         additionalInformationView.Content = additionalNode.SelectSingleNode("Content").InnerText;
                //     }

                //     additionalInformation.Add(additionalInformationView);
                // }

                // XmlNodeList entityNodes = doc.DocumentElement.SelectNodes("/DeclarationCorrection/Amendments/Entity");
                // List<AmendmentView> amedments = new List<AmendmentView>();
                // List<Field> fieldErrors = new List<Field>();

                // foreach (XmlNode entityNode in entityNodes)
                // {





                //     XmlNodeList FieldErrorNodes = doc.DocumentElement.SelectNodes("/DeclarationCorrection/Amendments/Entity/FieldErrors/field");




                //     foreach (XmlNode errorNode in FieldErrorNodes)
                //     {
                //         AmendmentView correctionEntity = new AmendmentView();
                //         if (entityNode.SelectSingleNode("Child3Type") != null && !(string.IsNullOrEmpty(entityNode.SelectSingleNode("Child3Type").InnerText)))
                //         {
                //             if (entityNode.SelectSingleNode("Child3Type") != null)
                //             {
                //                 correctionEntity.EntityName = entityNode.SelectSingleNode("Child3Type").InnerText;
                //             }

                //             if (entityNode.SelectSingleNode("Child3Sequence") != null)
                //             {
                //                 correctionEntity.Line = int.Parse(entityNode.SelectSingleNode("Child3Sequence").InnerText);
                //             }

                //             if (entityNode.SelectSingleNode("Child2Type") != null)
                //             {
                //                 correctionEntity.ParentEntityName = entityNode.SelectSingleNode("Child2Type").InnerText;
                //             }

                //             if (entityNode.SelectSingleNode("Child2Sequence") != null && !(string.IsNullOrEmpty(entityNode.SelectSingleNode("Child3Type").InnerText)))
                //            {
                //                correctionEntity.ParentLine = int.Parse(entityNode.SelectSingleNode("Child2Sequence").InnerText);
                //            }

                //             if (entityNode.SelectSingleNode("Child1Sequence") != null && entityNode.SelectSingleNode("Child2Sequence") != null && entityNode.SelectSingleNode("Child3Sequence") != null)
                //             {
                //                 correctionEntity.LineNumber = entityNode.SelectSingleNode("Child1Sequence").InnerText + "," + entityNode.SelectSingleNode("Child2Sequence").InnerText + "," + entityNode.SelectSingleNode("Child3Sequence").InnerText;

                //             }
                //         }
                //         else if (entityNode.SelectSingleNode("Child2Type") != null && !(string.IsNullOrEmpty(entityNode.SelectSingleNode("Child2Type").InnerText)))
                //         {
                //             if (entityNode.SelectSingleNode("Child2Type") != null)
                //             {
                //                 correctionEntity.EntityName = entityNode.SelectSingleNode("Child2Type").InnerText;
                //             }

                //             if (entityNode.SelectSingleNode("Child2Sequence") != null)
                //             {
                //                 correctionEntity.Line = int.Parse(entityNode.SelectSingleNode("Child2Sequence").InnerText);
                //             }

                //             if (entityNode.SelectSingleNode("Child1Type") != null)
                //             {
                //                 correctionEntity.ParentEntityName = entityNode.SelectSingleNode("Child1Type").InnerText;
                //             }

                //             if (entityNode.SelectSingleNode("Child1Sequence") != null)
                //             {
                //                 correctionEntity.ParentLine = int.Parse(entityNode.SelectSingleNode("Child1Sequence").InnerText);
                //             }

                //             if (entityNode.SelectSingleNode("Child1Sequence") != null && entityNode.SelectSingleNode("Child2Sequence") != null)
                //             {
                //                 correctionEntity.LineNumber = entityNode.SelectSingleNode("Child1Sequence").InnerText + "," + entityNode.SelectSingleNode("Child2Sequence").InnerText;
                //             }


                //         }
                //         else if (entityNode.SelectSingleNode("Child1Type") != null && !(string.IsNullOrEmpty(entityNode.SelectSingleNode("Child1Type").InnerText)))
                //         {
                //             if (entityNode.SelectSingleNode("Child1Type") != null)
                //             {
                //                 correctionEntity.EntityName = entityNode.SelectSingleNode("Child1Type").InnerText;
                //             }
                //             if (entityNode.SelectSingleNode("Child1Sequence") != null)
                //             {
                //                 correctionEntity.Line = int.Parse(entityNode.SelectSingleNode("Child1Sequence").InnerText);
                //             }

                //             correctionEntity.ParentEntityName = "Declaration";
                //             if (entityNode.SelectSingleNode("Child1Sequence") != null)
                //             {
                //                 correctionEntity.LineNumber = entityNode.SelectSingleNode("Child1Sequence").InnerText;
                //             }


                //         }
                //         else
                //         {
                //             correctionEntity.EntityName = "Declaration";
                //         }
                //         correctionEntity.TableNameTextCode = "Customs." + correctionEntity.EntityName;

                //       //  Field field = new Field();

                //         if (errorNode.SelectSingleNode("Code") != null)
                //         {
                //             correctionEntity.Code = errorNode.SelectSingleNode("Code").InnerText;
                //            // field.Code = errorNode.SelectSingleNode("Code").InnerText;
                //         }

                //         if (errorNode.SelectSingleNode("Fieldcode") != null)
                //         {
                //             correctionEntity.Fieldcode = errorNode.SelectSingleNode("Fieldcode").InnerText;
                //            // field.Fieldcode = errorNode.SelectSingleNode("Fieldcode").InnerText;
                //         }
                //         amedments.Add(correctionEntity);
                //       //  fieldErrors.Add(field);
                //     }


                ////     correctionEntity.FieldErrors = fieldErrors;




                // }
                // correctionView.AdditionalInformationViews = additionalInformation;
                // correctionView.GeneralDataViews = generalData;
                // correctionView.AmendmentViews = amedments;
                // correctionViews.Add(correctionView);
                #endregion

            }

            return correctionView;
        }

        //<--- Yuval Chalup 19.11.2015 TASK-17450
        public DeclarationPM GetSingleDeclarationByNumber(string number, int tenant, bool getComposition = false)
        {
            if (String.IsNullOrWhiteSpace(number)) return null;
            var q = repository.GetSingleDeclarationPMByNumber(number, tenant);

            var pocos = q.ToList();
            return pocos.Select(poco => this.GetEntityPM(poco, getComposition, new DeclarationKeys() { Id = pocos.FirstOrDefault().Id })).FirstOrDefault();
        }
        //Yuval Chalup 19.11.2015 TASK-17450 --->

        public DeclarationPM GetSingleDeclarationById(string id, int tenant)
        {
            Declaration declaration = repository.GetSingleDeclarationById(id, tenant);
            DeclarationPM declarationPM = new DeclarationPM();
            DeclarationDataMapping mapping = new DeclarationDataMapping();
            mapping.CustomPOCOToPM(declarationPM, declaration);
            mapping.POCOToPM(declarationPM, declaration);

            return declarationPM;
        }
        //private bool loadSupplierInvoices = true;
        //public bool LoadSupplierInvoices 
        //{
        //    get { return loadSupplierInvoices; }
        //    set { loadSupplierInvoices = value; }
        //}

        private bool loadSupplierInvoicesWithItems = true;
        public bool LoadSupplierInvoicesWithItems
        {
            get { return loadSupplierInvoicesWithItems; }
            set { loadSupplierInvoicesWithItems = value; }
        }

        private bool loadSupplierInvoicesItemsParentsOnly = false;
        public bool LoadSupplierInvoicesItemsParentsOnly
        {
            get { return loadSupplierInvoicesItemsParentsOnly; }
            set { loadSupplierInvoicesItemsParentsOnly = value; }
        }

        public List<DeclarationPM> GetDeclarationsByIds(List<string> ids, int tenant)
        {
            List<Declaration> declarations = repository.GetDeclarationsById(ids);
            DeclarationDataMapping mappings = new DeclarationDataMapping();
            List<DeclarationPM> declarationPMs = new List<DeclarationPM>();
            foreach (Declaration declaration in declarations)
            {
                DeclarationPM declarationPM = new DeclarationPM();
                mappings.CustomPOCOToPM(declarationPM, declaration);
                mappings.POCOToPM(declarationPM, declaration);
                GetComposition(new DeclarationKeys() { Id = declaration.Id, }, declarationPM);
                declarationPMs.Add(declarationPM);
            }
            return declarationPMs;
        }

        public List<DeclarationPM> GetByConsigmentExportContainerizationID(string containerizationId, int tenant)
        {
            var query = repository.GetByConsigmentExportContainerizationID(containerizationId, tenant);
            List<Declaration> declarations = query.ToList();
            DeclarationDataMapping mappings = new DeclarationDataMapping();
            List<DeclarationPM> declarationPMs = new List<DeclarationPM>();
            foreach (Declaration declaration in declarations)
            {
                DeclarationPM declarationPM = new DeclarationPM();
                mappings.CustomPOCOToPM(declarationPM, declaration);
                mappings.POCOToPM(declarationPM, declaration);
                GetComposition(new DeclarationKeys() { Id = declaration.Id, }, declarationPM);
                declarationPMs.Add(declarationPM);
            }
            return declarationPMs;
        }


        public int GetInvoiceItemsWithTradeAgreementCount(string declarationId, int tenant)
        {
            return repository.GetInvoiceItemsWithTradeAgreementCount(declarationId, tenant);
        }


        public string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID, int tenant) // moran 3.1.17 - AMI-58876
        {
            var setting = CustomsSettingQueryService.GetLogitudeCustomsSettingsM(tenant);
            if (setting.IsConnectedToUniFreight)
            {
                var cntxt = AmitalContext.GetContext(tenant);
                var myGDFDATAQueryService = new GDFDATAQueryService(cntxt);

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
            return ("");
        }

        public override void InitializeSettings() // mohammad 1-3-2017 to initialize properties and other settings from a generated controller.
        {
            this.LoadSupplierInvoicesWithItems = false;
            this.LoadSupplierInvoicesItemsParentsOnly = false;
        }

        public CustomsRequiredFieldErrors CheckCertificateStatus(string declarationId, int tenant, bool FeatureOn = false)
        {
            ICustomContext customContext = MainContext as CustomContext;
            CustomContext activeContext = customContext.GetActiveDbContext() as CustomContext;
            object[] parameters = new object[] { };
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");


            DbRawSqlQuery<SupplierInvoiceItem> items = null;

            if (dbms == "oracle")
            {
                if (FeatureOn)
                {
                    items = activeContext.Database.SqlQuery<SupplierInvoiceItem>("select * from supplierInvoiceItems where (CertificatesStatusCode ='2' or CertificatesStatusCode ='4')  and  IsParent=0 and declarationid ='" + declarationId + "' and tenant = " + tenant, parameters);

                }
                else
                {
                    items = activeContext.Database.SqlQuery<SupplierInvoiceItem>("select * from supplierInvoiceItems where CertificatesStatusCode ='2'  and  IsParent=0 and declarationid ='" + declarationId + "' and tenant = " + tenant, parameters);
                }
            }
            else
            {
                if (FeatureOn)
                {
                    items = activeContext.Database.SqlQuery<SupplierInvoiceItem>("select * from customs.SupplierInvoiceItems  where (CertificatesStatusCode = '2' or CertificatesStatusCode ='4') and  IsParent=0 and DeclarationId='" + declarationId + "' and tenant = " + tenant, parameters);
                }
                else
                {
                    items = activeContext.Database.SqlQuery<SupplierInvoiceItem>("select * from customs.SupplierInvoiceItems  where CertificatesStatusCode = '2'  and  IsParent=0 and DeclarationId='" + declarationId + "' and tenant = " + tenant, parameters);

                }
            }

            CustomsRequiredFieldErrors errors = new CustomsRequiredFieldErrors() { RequiredFields = new List<CustomsRequiredFieldsErrorItem>() };

            foreach (SupplierInvoiceItem item in items)
            {
                errors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Certificate fields are not valid", EntityReference = item.CounterKey.ToString(), EntityReference2 = item.SequenceNumeric.ToString(), FieldName = "CertificateStatusCode", TableName = "SupplierInvoiceItem" });
            }

            return errors;
        }

        public bool GetIsValueForCustomsOnlyFromDeclaration(string declarationId, int tenant)
        {
            return repository.GetIsValueForCustomsOnlyFromDeclaration(declarationId, tenant);
        }


        public bool IsValidTickets(DeclarationPM declarationPM)
        {
            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(declarationPM.Tenant);
            bool ticketValidStatus = true;

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
                            ticketValidStatus = false;
                            break;
                        }
                    }
                }
            }

            if (IsDocumentMissing(declarationPM))
            {
                ticketValidStatus = false;

            }


            return ticketValidStatus;
        }

        public bool CheckDiamondsDeclarationReadyForSending(DeclarationPM declarationPM)
        {
            bool declarationReadyForSending;

            // get all documents CONNECTED to the declaration
            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(declarationPM.Tenant);
            List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationPM.Id, "", "", "", declarationPM.Tenant, "Declaration").ToList();
            var DocumentsFilingIdList = new List<string>();
            foreach (var customsDocumentsTicketPM in customsDocumentsTicketPMList)
            {
                if (!string.IsNullOrWhiteSpace(customsDocumentsTicketPM.DocumentsFilingId))
                {
                    DocumentsFilingIdList.Add(customsDocumentsTicketPM.DocumentsFilingId);
                }
            }

            // if the declaration has no connected document, it can not been sent to the mehes
            if (DocumentsFilingIdList.Count() == 0)
            {
                declarationReadyForSending = false;
            }
            else
            {
                // get all declaration Customs Document
                var myCustomsDocumentQueryService = new CustomsDocumentQueryService(declarationPM.Tenant);
                var customsDocumentPMList = myCustomsDocumentQueryService.GetCustomsDocumentList(DocumentsFilingIdList, declarationPM.Tenant);

                // determine how many supplier invoices documents had been successfully sent to the mekhes
                int sentSupplierInvoices = customsDocumentPMList.Where(document => document.DocumentTypeCode == "380" && document.DocumentStatusCode == "1").Count();

                // get all supplier invoices count of the declaration
                SupplierInvoiceListQueryService supplierInvoiceQuery = new SupplierInvoiceListQueryService(context);
                QueryOperations queryOperations = new QueryOperations();
                queryOperations.SetFilter("DeclarationId", declarationPM.Id, false, "Equals", null, false, false, "string");
                int declarationSupplierInvoiceCount = supplierInvoiceQuery.GetListCount(queryOperations, declarationPM.Tenant);

                // the declaration may be sent if documents about all its supplier invoices have been sent to the mehes and received simuhin
                declarationReadyForSending = sentSupplierInvoices >= declarationSupplierInvoiceCount;

                /* if need to check for every invoice, the relation between document and invoice is
                 * (invoice.SequenceNumeric == customsDocumentsTicketPM.ConnectedInvoicesSequences) */
            }

            return declarationReadyForSending;
        }

        public DiamondsDeclarationSummary GetDiamondsDeclarationsCounts(int tenant, List<string> requestedCounts)
        {
            var mycontext = CustomContext.GetContext(tenant);
            IQueryable<Declaration> declaration =
                 from dc in mycontext.Declarations
                 where dc.Tenant == tenant && dc.Direction == "E" && dc.IsCancelled == false &&
                     dc.AmendmentDontDisplayInList == false && dc.IsDiamondDeclaration == true
                 select dc;

            DeclarationCustomFilters declarationCustomFilters = new DeclarationCustomFilters();
            DiamondsDeclarationSummary diamondsDeclarationSummary = new DiamondsDeclarationSummary()
            {
                Counts = new Dictionary<string, int>()
            };

            int total = 0;
            foreach (var requestedCount in requestedCounts) {
                var query = declarationCustomFilters.AddDiamondsDeclarationFilter(declaration, requestedCount);
                if (query != null)
                {
                    int count = query.Count();
                    diamondsDeclarationSummary.Counts[requestedCount] = count;
                    total += count;
                }
            }

            diamondsDeclarationSummary.TotalCount = total;
            return diamondsDeclarationSummary;
        }

        public bool IsDocumentMissing(DeclarationPM myDeclarationPM)
        {
            var customContext = CustomContext.GetContext(myDeclarationPM.Tenant);
            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(customContext);
            CustomDocumentTypeQueryService docTypeQuery = new CustomDocumentTypeQueryService(customContext);

            List<CustomDocumentTypePM> documentTypePMs = docTypeQuery.GetMandatoryCustomDocumentTypes(myDeclarationPM.Tenant);

            foreach (var doc in documentTypePMs)
            {
                List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(myDeclarationPM.Id, "", "", "", myDeclarationPM.Tenant, "Declaration").Where(r => r.DocumentTypeCode == doc.Code && r.DocumentsFilingId != null).ToList();
                if (customsDocumentsTicketPMList == null || customsDocumentsTicketPMList.Count() < 1)
                    return true;
            }

            return false;

        }


        public bool IsMissingMandatoryFields(DeclarationPM declarationPM)
        {
            bool isMissingMandatoryFields = false;
            CustomsRequiredFieldErrors errorsForDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(declarationPM.Id, declarationPM.Tenant, declarationPM);
            if (errorsForDeclaration != null && errorsForDeclaration.RequiredFields != null && errorsForDeclaration.RequiredFields.Count() > 0)
            {
                isMissingMandatoryFields = true;
            }
            return isMissingMandatoryFields;
        }


        public List<CustomsDocumentsTicketPM> GetDeclarationMandatoryTicket(string declarationId, int tenant)
        {


            var myCustomsDocumentPointerRepository = new CustomsDocumentPointerRepository(this.context);
            var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(this.context);
            var customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(this.context);
            var customsDocumentRepository = new CustomsDocumentRepository(this.context);


            var listDeclarationTicket = (
                from cdp in myCustomsDocumentPointerRepository.GetQParentDocumentPointer(declarationId, "Declaration", tenant)
                join cdt in customsDocumentsTicketRepository.GetAll(tenant)
                on cdp.CustomsDocumentsTicketId equals cdt.Id
                select cdt).ToList();

            var listDeclarationTicket_DocumentsFilingId = listDeclarationTicket.Select(r => r.DocumentsFilingId);
            var listDeclarationCustomDocument
                =
////**** full scan 


//(
//from cdt in listDeclarationTicket
//   join cd in customsDocumentRepository.GetAll(tenant)
//   on cdt.DocumentsFilingId equals cd.DocumentsFilingId
//   select cd
//)


(

                     from cd in customsDocumentRepository.GetAll(tenant)
                         //on cdt.DocumentsFilingId equals cd.DocumentsFilingId
                     where listDeclarationTicket_DocumentsFilingId.Contains(cd.DocumentsFilingId)
                     select cd
                ).ToList();


            var mandatoryTicketWithDocumentNotSend =
                (from t in listDeclarationTicket.Where(t => t.IsSendMandatory)
                 join d in listDeclarationCustomDocument.Where(d => string.IsNullOrWhiteSpace(d.CustomsDocId))
                 on t.DocumentsFilingId equals d.DocumentsFilingId
                 select t
                 ).ToList();
            if (mandatoryTicketWithDocumentNotSend.Count > 0)
            {
                //there is SendMandatory CustomDocument !!!
                return mandatoryTicketWithDocumentNotSend.Select(
                    r => customsDocumentsTicketQueryService.GetEntityPM(r)
                    ).ToList();

            }


            var mandatoryTicketWithoutDocument =
                listDeclarationTicket
                .Where(t => t.IsSendMandatory)
                .Where(t => string.IsNullOrWhiteSpace(t.DocumentsFilingId))
                .ToList();
            if (mandatoryTicketWithoutDocument.Count > 0)
            {
                //there is mandatory Ticket  that not connected to Document !!!
                return mandatoryTicketWithoutDocument.Select(
                    r => customsDocumentsTicketQueryService.GetEntityPM(r)
                    ).ToList(); ;
            }




            var myDeclaration = this.GetSingle(declarationId, true, false);
            if (myDeclaration == null
                //&& 
                )
            {
                throw new Exception("No declaration ???");
            }
            string CargoTypeCode = null;
            if (myDeclaration.Consignments != null && myDeclaration.Consignments.Count() > 0)
            {
                CargoTypeCode = myDeclaration.Consignments[0].CargoTypeCode;
            }
            var myCustomsDocumentsDefinitionQueryService = new CustomsDocumentsDefinitionQueryService(tenant);
            var listCustomsDocumentsDefinition = myCustomsDocumentsDefinitionQueryService.GetCustomsDocumentsDefinitionsForDeclaration(CargoTypeCode, myDeclaration.ProcedureCurrentCode, myDeclaration.TransportModeId, myDeclaration.DeclarationTypeCode, tenant);

            var ticketDocumentTypeCodeInDB = listDeclarationTicket.Select(r => r.DocumentTypeCode).Distinct().ToList();
            var notInDbTicketActiveMandatory =
                listCustomsDocumentsDefinition
                //.Where(def => def.Tenant == tenant)
                .Where(def => !def.Inactive)
                .Where(def => def.Mandatory)
                .Where(def => !ticketDocumentTypeCodeInDB.Contains(def.DocumentTypeCode))
                .Select(virtualTicket => new CustomsDocumentsTicketPM()
                {
                    Tenant = tenant,
                    DocumentTypeCode = virtualTicket.DocumentTypeCode,
                    DocumentsFilingId = "Virtual/Not In DB"
                });
            return notInDbTicketActiveMandatory.ToList();
        }

        public bool CheckFreightAmountsByIncoterm(string declarationId, int tenant)
        {
            bool isFreight = false;
            var supplierInvoiceQueryService = new SupplierInvoiceQueryService(this.context);

            List<SupplierInvoicePM> supplierInvoiceList = supplierInvoiceQueryService.GetSupplierInvoicesForDeclaration(declarationId, tenant, true);
            if (supplierInvoiceList != null)
            {
                foreach (SupplierInvoicePM item in supplierInvoiceList)
                {
                    if (item.SupplierInvoiceFreightAmounts != null && item.SupplierInvoiceFreightAmounts.Count > 0)
                    {
                        if (item.IncotermCode != null && (item.IncotermCode.StartsWith("C") || item.IncotermCode.StartsWith("D")))
                        {
                            return true;
                        }
                    }
                }
            }
            return isFreight;
        }

        //public List<DeclarationList> GetDeclarationAmendmentsByIdCache(int Tenant, string id, bool orderById = false)
        //{

        //    string entityKeyString = $"GetDeclarationAmendmentsByIdCache({id},{Tenant},{orderById})";
        //    var res = CacheManager.GetOrInsertNewObject<List<DeclarationList>>(entityKeyString, () =>
        //    {
        //        return this.GetDeclarationAmendmentsById(Tenant, id, orderById);
        //    });
        //    return res;
        //}
        public DeclarationPM GetDeclarationAmendmentByIdAndAmendmentNo(int tenant, string id, string requestNumber)
        {
            DeclarationDataMapping mapping = new DeclarationDataMapping();

            DeclarationPM decPm = new DeclarationPM();
            var decs = repository.GetDeclarationAmendmentsById(tenant, id);

            var dec = decs.FirstOrDefault(x => x.AmendmentRequestNumber == requestNumber);

            mapping.CustomPOCOToPM(decPm, dec);
            mapping.POCOToPM(decPm, dec);


            return decPm;
        }

        public List<Declaration> GetDeclarationById(int tenant, string id)
        {
            List<Declaration> declarations = repository.GetDeclarationById(tenant, id);
            return declarations;
        }

        public List<DeclarationList> GetDeclarationAmendmentsById_Cache(int tenant, string id, bool orderById = false)
        {
            string key = $"GetDeclarationAmendmentsById({tenant}, {id}, {orderById})";
            var res = CacheManager.GetOrInsertNewObject<List<DeclarationList>>(key,
                () =>
                {
                    return this.GetDeclarationAmendmentsById(tenant, id, orderById);
                });
            return res;
        }

      

        
        public List<DeclarationList> GetAllDeclarationPOCOs(int tenant, string customFileNo, string direction)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
            if (direction != "E")
            {
                return
                  (
                  from rec in this.context.Declarations

                  join right in this.context.AmendmentStatuses on rec.AmendmentStatus equals right.Code into joined
                  from j in joined.DefaultIfEmpty()

                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select
                      new DeclarationList()
                      {
                          Id = rec.Id,
                          Tenant = rec.Tenant,
                          AmendmentRequestNumber = rec.AmendmentRequestNumber,
                          DeclarationVersionId = rec.VersionId,
                          AmendmentStatus = rec.AmendmentStatus,
                          AmendmentOriginalDeclartation = rec.AmendmentOriginalDeclartation,
                          AmendmentissueDate = rec.AmendmentissueDate,
                          IsAmendment = rec.IsAmendment,
                          AmedmentType = rec.AmedmentType,
                          AmendmentStatusName = j == null ? "" : j.Name
                      }
                  ).ToList();
            }
            else
            {
                return
                (
                from rec in this.context.Declarations

                join right in this.context.AmendmentRequestStatuses on rec.AmendmentStatus equals right.Code into joined
                from j in joined.DefaultIfEmpty()

                where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                select
                    new DeclarationList()
                    {
                        Id = rec.Id,
                        Tenant = rec.Tenant,
                        AmendmentRequestNumber = rec.AmendmentRequestNumber,
                        DeclarationVersionId = rec.VersionId,
                        AmendmentStatus = rec.AmendmentStatus,
                        AmendmentOriginalDeclartation = rec.AmendmentOriginalDeclartation,
                        AmendmentissueDate = rec.AmendmentissueDate,
                        IsAmendment = rec.IsAmendment,
                        AmedmentType = rec.AmedmentType,
                        AmendmentStatusName = j == null ? "" : j.LocalName
                    }
                ).ToList();
            }

        }

        public List<DeclarationList> GetDeclarationAmendmentsById(int tenant, string id, bool orderById = false)
        {

            List<Declaration> declarations = repository.GetDeclarationAmendmentsById(tenant, id);
            List<AmendmentStatusPM> amendmentStatusPMs = new List<AmendmentStatusPM>();
            AmedmentTypeRepository amendmentTypesRepository = new AmedmentTypeRepository(context);
            List<DeclarationList> declarationLists = new List<DeclarationList>();
            UserRepository userRepository = new UserRepository(tenant);
            var amendmentTypes = amendmentTypesRepository.GetAll();
            var users = userRepository.GetAll();
            var i = 1;
            foreach (Declaration item in declarations)
            {

                DeclarationList declarationList = new DeclarationList()
                {

                    Id = item.Id,
                    Tenant = item.Tenant,
                    AmendmentRequestNumber = item.AmendmentRequestNumber,
                    DeclarationVersionId = item.VersionId,
                    AmendmentStatus = item.AmendmentStatus,
                    AmendmentOriginalDeclartation = item.AmendmentOriginalDeclartation,
                    AmendmentissueDate = item.AmendmentissueDate,
                    IsAmendment=item.IsAmendment,
                    AmedmentType = item.AmedmentType,
                    ExportCloseAmendRequestNumber = item.ExportCloseAmendRequestNumber,
                };
                if (item.AmendmentCorrectedByUserId != null) declarationList.AmendmentCorrectedByUserName = users.FirstOrDefault(x => x.Id == item.AmendmentCorrectedByUserId).Code;
                if (item.Direction == "E")
                {
                    AmendmentRequestStatusRepository amendmentRequestStatusRepository = new AmendmentRequestStatusRepository(context);
                    var amendmentRequestStatus = amendmentRequestStatusRepository.GetAll();
                    if (item.AmendmentStatus != null) declarationList.AmendmentStatusName = amendmentRequestStatus.FirstOrDefault(x => x.Code == item.AmendmentStatus)?.LocalName;
                }
                else
                {
                    AmendmentStatusRepository amendmentStatusRepository = new AmendmentStatusRepository(context);
                    var amendmentStatuses = amendmentStatusRepository.GetAll();
                    if (item.AmendmentStatus != null) declarationList.AmendmentStatusName = amendmentStatuses.FirstOrDefault(x => x.Code == item.AmendmentStatus).Name;
                }
                if (item.AmedmentType != null) declarationList.AmendmentTypeName = amendmentTypes.FirstOrDefault(x => x.Code == item.AmedmentType).Name;

                declarationLists.Add(declarationList);
            }
            if (orderById)
            {
                declarationLists = declarationLists.OrderBy(x => x.Id).ToList();
                declarationLists.ForEach(x => { x.AmendmentNumber = i; i++; });

                return declarationLists.ToList();


            }
            declarationLists = declarationLists.OrderByDescending(x => x.AmendmentissueDate).ToList();

            declarationLists.ForEach(x => { x.AmendmentNumber = i; i++; });

            return declarationLists.ToList();

        }


        public ExportStorageConnectToDeclaration GetExportStorageConnectToDeclaration(string declarationId, int tenant) =>
            new DeclarationRepository(Tenant).GetExportStorageConnectToDeclaration(declarationId, tenant);



        public string GetHatraDateForDecId(string decId, int tenant)
        {
            return repository.GetHatraDateForDecId(decId, tenant);
        }

        public List<ContainerizationUniqueConsignment> GetContainerizationUniqueConsignment(List<string> declarationList)
        {
            return this.repository.GetContainerizationUniqueConsignment(declarationList);
        }
        public Declaration GetDeclarationByDeclarationNum(string decNumber, int tenant)
        {
            return repository.GetDeclarationByDeclarationNum(decNumber, tenant);
        }

        public List<ExportReport1> GetReportDeclarationForExportReport1(DateTime? ExportFrom, DateTime? ExportTo)
        {

            var ExportReportData = this.repository.GetReportDeclarationForExportReport1(ExportFrom, ExportTo);
            return ExportReportData;
        }
        public List<ExportReport2> GetReportDeclarationForExportReport2(DateTime? ExportFrom, DateTime? ExportTo)
        {

            var ExportReportData = this.repository.GetReportDeclarationForExportReport2(ExportFrom, ExportTo);
            return ExportReportData;
        }
        public string GetDeclarationByConsignment()
        {
            return "";
        }

        public List<string> CheckDeclarationsInDisplayOnly(string[] declarationIdsList, string[] allWithoutdeclarationIdsList, bool checkboxAll, QueryOperations filter, int tenant)
        {
            if (checkboxAll)
                declarationIdsList = new DeclarationCourierStatusListQueryService(context).GetDeclarationCourierStatusListPendingBulk(filter, tenant).Select(x => x.DeclarationId).ToArray();

            if (checkboxAll && allWithoutdeclarationIdsList != null)
                declarationIdsList = declarationIdsList.Where(x => !allWithoutdeclarationIdsList.Contains(x)).ToArray();

            string[] sheetStatusInProcessId = Enum.GetNames(typeof(SheetStatusInProcessEnum));

            List<string> res = repository.GetDisplayOnly(declarationIdsList, tenant, sheetStatusInProcessId);

            return res;
        }
   
        public string GetDeclaratNumberByCustomFileNo(int tenant, string customFileNo, string direction)
        {
            return this.repository.GetDeclaratNumberByCustomFileNo(tenant, customFileNo, direction);
        }
        public Boolean CheckIfDeclarationHasError12195(string declarationID, int tenant)
        {
            var errors= this.GetDeclarationErrors(declarationID, tenant, null);
            if(errors != null && errors.Find(x=>x.ErrorType == "12195") != null)
            {
                return true;
            }
            return false;
        }
		public string GetSignedByUserIdByCustomFileNo(int tenant, string customFileNo)
		{
			return this.repository.GetSignedByUserIdByCustomFileNo(tenant, customFileNo);
		}
        public DeclarationPM GetDeclarationByExportFile(int tenant, string customFileNo)
        {
            Declaration declaration =this.repository.GetDeclarationsByExportFile(tenant, customFileNo);
            DeclarationDataMapping mappings = new DeclarationDataMapping();
            DeclarationPM declarationPM = new DeclarationPM();
            if (declaration != null)
            {
                mappings.CustomPOCOToPM(declarationPM, declaration);
                mappings.POCOToPM(declarationPM, declaration);
            }
            return declarationPM;
        }
        public List<DeclarationPM> GetDeclarationsByExportFile(int tenant, string exportFile)
		{
			List<Declaration> declarations = repository.GetDeclarationsByExportFileNotClose(tenant, exportFile);
			DeclarationDataMapping mappings = new DeclarationDataMapping();
			List<DeclarationPM> declarationPMs = new List<DeclarationPM>();
			foreach (Declaration declaration in declarations)
			{
				DeclarationPM declarationPM = new DeclarationPM();
				mappings.CustomPOCOToPM(declarationPM, declaration);
				mappings.POCOToPM(declarationPM, declaration);
				declarationPMs.Add(declarationPM);
			}
			return declarationPMs;
		}
	}

    public class DiamondsDeclarationSummary
    {
        public Dictionary<string, int> Counts { get; set; }
        public int TotalCount { get; set; }
    }
}
