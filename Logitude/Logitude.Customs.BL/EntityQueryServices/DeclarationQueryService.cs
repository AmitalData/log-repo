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

            //DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);

            entityPM.InvoiceHasFreight = supplierInvoiceService.DoesAnyInvoiceHasFreight(entityPM.Id, entityPM.Tenant);

            //******getting all compositionTables for response service purposes only *****///
            entityPM.Consignments = consignmentService.GetMulti(declarationKeys, true);
            // if (LoadSupplierInvoices)


            {
                if (loadSupplierInvoicesItemsParentsOnly == true)
                {
                    supplierInvoiceService.OnlyParentItem = true;
                }
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

        public string GetIdByDeclarationNumber(string declarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationNumber)) return "";
            return repository.GetIdByDeclarationNumber(declarationNumber, tenant);
        }

        public string GetIdByCustomFileNo(string customFileNo, int tenant)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            return repository.GetIdByCustomFileNo(customFileNo, tenant);
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

        public List<DeclarationErrorView> GetDeclarationErrors(string declarationId, int tenant, string listVersionId, string courierFilter = "Declaration")
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
                            errorview.FieldNameTextCode = "Customs." + errorview.EntityName + ".F." + errorview.Field;
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
                        errorview.FieldNameTextCode = "Customs." + errorview.EntityName + ".F." + errorview.Field;
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
                   rec.UpdateDateTime.Value > lst30 &&
                    rec.UserNotes == "LoadTest");
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

        public DeclarationCorrectionView GetDeclarationCorrection(string declarationId, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            Declaration declaration = Repository.GetSingle(new DeclarationKeys() { Id = declarationId });
            string CorrectionXML = declaration.CorrectionsXml;


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

                    foreach (Additional additional in item.AdditionalInformation)
                    {
                        AdditionalInformationView information = new AdditionalInformationView();
                        information.Content = additional.Content;
                        DeclarationStatementTypeList statement = statementTypes.Where(d => d.Code == additional.StatementTypeCode).FirstOrDefault();
                        if (statement != null)
                        {
                            information.StatmentName = statement.LocalName;
                        }

                        generalData.AdditionalInformation.Add(information);


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
        public DeclarationPM GetSingleDeclarationByNumber(string number, int tenant)
        {
            if (String.IsNullOrWhiteSpace(number)) return null;
            var q = repository.GetSingleDeclarationPMByNumber(number, tenant);

            var pocos = q.ToList();
            return pocos.Select(poco => this.GetEntityPM(poco, false, null)).FirstOrDefault();
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
            var listCustomsDocumentsDefinition = myCustomsDocumentsDefinitionQueryService.GetCustomsDocumentsDefinitionsForDeclaration(CargoTypeCode, myDeclaration.ProcedureCurrentCode, myDeclaration.TransportModeId, tenant);

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

        public List<DeclarationList> GetDeclarationAmendmentsByCustomFileNo(int tenant, string customFileNo)
        {
            //DeclarationRepository declarationRep = new DeclarationRepository(context);
            //var declarations=   declarationRep.GetDeclarationAmendmentsByCustomFileNo(tenant, customFileNo);
            //DeclarationDataMapping mappings = new DeclarationDataMapping();

            //List<DeclarationPM> declarationPMs = new List<DeclarationPM>();


            //DeclarationListQueryService declarationListQueryService = new DeclarationListQueryService(context);

            //IQueryable<DeclarationList> DeclarationListQuery = declarationListQueryService.GetIqueryableList(declarations);

            ////foreach (Declaration declaration in declarations)
            ////{
            ////    DeclarationPM declarationPM = new DeclarationPM();
            ////    mappings.CustomPOCOToPM(declarationPM, declaration);
            ////    mappings.POCOToPM(declarationPM, declaration);
            ////    GetComposition(new DeclarationKeys() { Id = declaration.Id, }, declarationPM);
            ////    declarationPMs.Add(declarationPM);
            ////}
            //return declarationPMs;


            List<Declaration> declarations = repository.GetDeclarationAmendmentsByCustomFileNo(tenant , customFileNo);
            List<AmendmentStatusPM> amendmentStatusPMs = new List<AmendmentStatusPM>();
            AmendmentStatusRepository amendmentStatusRepository = new AmendmentStatusRepository(context);
            List<DeclarationList> declarationLists = new List<DeclarationList>();
            UserRepository userRepository = new UserRepository();
             var amendmentStatuses=  amendmentStatusRepository.GetAll();
            var users = userRepository.GetAll();

            foreach (Declaration item in declarations)
            {

                DeclarationList declarationList = new DeclarationList()
                {

                    Id = item.Id,
                    Tenant = item.Tenant,
                    AmendmentRequestNumber = item.AmendmentRequestNumber,
                    DeclarationVersionId = item.VersionId,
                    AmendmentCorrectedByUserName = users.FirstOrDefault(x=>x.Id==item.AmendmentCorrectedByUserId).Code,
                    AmendmentStatusName = amendmentStatuses.FirstOrDefault(x => x.Code == item.AmendmentStatus).Name,


               };

 
                declarationLists.Add(declarationList);
            }

            return declarationLists;
        }

    }
}
