using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{

    [EnableClientAccess()]
    public partial class ClaimDomainService : LogitudeDomainService
    {

        IDomainServiceUpdateClass<ClaimPM> service;
        ICustomContext MyContext;
        public ClaimDomainService()
        {
            //service = ContainerAccessor.Container.Resolve(typeof(IDomainServiceUpdateClass<ClaimPM>), "CustomDomainServiceUpdateClass", new ParameterOverride("", 1)) as IDomainServiceUpdateClass<ClaimPM>;
        }

        public ClaimPM GetSingleClaimPM(string id, int tenant)
        {
            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            }

            ClaimQueryService claimQuery = new ClaimQueryService(MyContext);
            ClaimPM claimPM = claimQuery.GetSingle(id, true, false);
            return claimPM;

        }


        public ClaimList GetSingleClaimList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Claim", "READ", tenant);

            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            }


            ClaimListQueryService listService = new ClaimListQueryService(MyContext);
            return listService.GetSingle(id);
        }

        public List<ClaimList> GetClaimLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            }
            ClaimListQueryService listService = new ClaimListQueryService(MyContext);
            return listService.GetList(tenant);
        }

        public List<ClaimList> GetClaimsFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            };
            ClaimListQueryService listService = new ClaimListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetClaimFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            };
            ClaimListQueryService queryService = new ClaimListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertClaim(ClaimPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            };
            ClaimUpdateService service = new ClaimUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            //List<ClaimImporterDeclarsPage3PM> ClaimImporterDeclarsPage3sChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimImporterDeclarsPage3s).Cast<ClaimImporterDeclarsPage3PM>().ToList();
            //foreach (ClaimImporterDeclarsPage3PM ClaimImporterDeclarsPage3 in ClaimImporterDeclarsPage3sChangeSet)
            //{
            //    entityPM.ClaimImporterDeclarsPage3s.Where(d => d.ClaimId == ClaimImporterDeclarsPage3.ClaimId && d.ImporterLoiDeclarationTypeCode == ClaimImporterDeclarsPage3.ImporterLoiDeclarationTypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //}


            //List<ClaimsRelatedEntityPM> ClaimsRelatedEntitysChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimsRelatedEntities).Cast<ClaimsRelatedEntityPM>().ToList();
            //foreach (ClaimsRelatedEntityPM ClaimImporterDeclarsPage3 in ClaimsRelatedEntitysChangeSet)
            //{
            //    entityPM.ClaimsRelatedEntities.Where(d => d.ClaimId == ClaimImporterDeclarsPage3.ClaimId && d.EntityCounterKey == ClaimImporterDeclarsPage3.EntityCounterKey).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //    List<ClaimsRelatedEntitiesAmountPM> ClaimsRelatedEntitiesAmountsChangeSet = ChangeSet.GetAssociatedChanges(ClaimsRelatedEntity, d => d.ClaimsRelatedEntitiesAmounts).Cast<ClaimsRelatedEntitiesAmountPM>().ToList();
            //    foreach (ClaimsRelatedEntitiesAmountPM ClaimsRelatedEntsExpDeclar in ClaimsRelatedEntitiesAmountsChangeSet)
            //    {
            //        ClaimsRelatedEntity.ClaimsRelatedEntitiesAmounts.Where(d => d.ClaimId == ClaimsRelatedEntsExpDeclar.ClaimId && d.CounterKey == ClaimsRelatedEntsExpDeclar.CounterKey && d.LineNo == ClaimsRelatedEntsExpDeclar.LineNo).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //    }


            //    List<ClaimsRelatedEntitiesReasonPM> ClaimsRelatedEntitiesReasonsChangeSet = ChangeSet.GetAssociatedChanges(ClaimsRelatedEntity, d => d.ClaimsRelatedEntitiesReasons).Cast<ClaimsRelatedEntitiesReasonPM>().ToList();
            //    foreach (ClaimsRelatedEntitiesReasonPM ClaimsRelatedEntsExpDeclar in ClaimsRelatedEntitiesReasonsChangeSet)
            //    {
            //        ClaimsRelatedEntity.ClaimsRelatedEntitiesReasons.Where(d => d.ClaimId == ClaimsRelatedEntsExpDeclar.ClaimId && d.CounterKey == ClaimsRelatedEntsExpDeclar.CounterKey && d.ReasonListTypeCode == ClaimsRelatedEntsExpDeclar.ReasonListTypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //        List<ClaimsRelatedEntsReasonsExpPM> ClaimsRelatedEntsReasonsExpsChangeSet = ChangeSet.GetAssociatedChanges(ClaimsRelatedEntitiesReason, d => d.ClaimsRelatedEntsReasonsExps).Cast<ClaimsRelatedEntsReasonsExpPM>().ToList();
            //        foreach (ClaimsRelatedEntsReasonsExpPM ClaimsRelatedEntsReasonsExp in ClaimsRelatedEntsReasonsExpsChangeSet)
            //        {
            //            ClaimsRelatedEntitiesReason.ClaimsRelatedEntsReasonsExps.Where(d => d.ClaimId == ClaimsRelatedEntsReasonsExp.ClaimId && d.CounterKey == ClaimsRelatedEntsReasonsExp.CounterKey && d.ReasonListTypeCode == ClaimsRelatedEntsReasonsExp.ReasonListTypeCode && d.ClaimExplanationTypeCode == ClaimsRelatedEntsReasonsExp.ClaimExplanationTypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //        }


            //    }


            //    List<ClaimsRelatedEntsExpDeclarPM> ClaimsRelatedEntsExpDeclarsChangeSet = ChangeSet.GetAssociatedChanges(ClaimsRelatedEntity, d => d.ClaimsRelatedEntsExpDeclars).Cast<ClaimsRelatedEntsExpDeclarPM>().ToList();
            //    foreach (ClaimsRelatedEntsExpDeclarPM ClaimsRelatedEntsExpDeclar in ClaimsRelatedEntsExpDeclarsChangeSet)
            //    {
            //        ClaimsRelatedEntity.ClaimsRelatedEntsExpDeclars.Where(d => d.ClaimId == ClaimsRelatedEntsExpDeclar.ClaimId && d.CounterKey == ClaimsRelatedEntsExpDeclar.CounterKey && d.ExportDeclarationNumber == ClaimsRelatedEntsExpDeclar.ExportDeclarationNumber).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //    }


            //}

            //List<ClaimImporterDeclarsPage3PM> ClaimImporterDeclarsPage3sChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimImporterDeclarsPage3s).Cast<ClaimImporterDeclarsPage3PM>().ToList();
            //foreach (ClaimImporterDeclarsPage3PM ClaimsRelatedEntity in ClaimImporterDeclarsPage3sChangeSet)
            //{
            //    entityPM.ClaimImporterDeclarsPage3s.Where(d => d.ClaimId == ClaimsRelatedEntity.ClaimId && d.ImporterLoiDeclarationTypeCode == ClaimsRelatedEntity.ImporterLoiDeclarationTypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //}


            //List<ClaimsRelatedEntityPM> ClaimsRelatedEntitiesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimsRelatedEntities).Cast<ClaimsRelatedEntityPM>().ToList();
            //foreach (ClaimsRelatedEntityPM ClaimsRelatedEntity in ClaimsRelatedEntitysChangeSet)
            //{
            //    entityPM.ClaimsRelatedEntities.Where(d => d.ClaimId == ClaimsRelatedEntity.ClaimId && d.EntityCounterKey == ClaimsRelatedEntity.EntityCounterKey).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //    List<ClaimsRelatedEntitiesAmountPM> ClaimsRelatedEntitiesAmountsChangeSet = ChangeSet.GetAssociatedChanges(ClaimsRelatedEntity, d => d.ClaimsRelatedEntitiesAmounts).Cast<ClaimsRelatedEntitiesAmountPM>().ToList();
            //    foreach (ClaimsRelatedEntitiesAmountPM ClaimsRelatedEntsExpDeclar in ClaimsRelatedEntitiesAmountsChangeSet)
            //    {
            //        ClaimsRelatedEntity.ClaimsRelatedEntitiesAmounts.Where(d => d.ClaimId == ClaimsRelatedEntsExpDeclar.ClaimId && d.CounterKey == ClaimsRelatedEntsExpDeclar.CounterKey && d.LineNo == ClaimsRelatedEntsExpDeclar.LineNo).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //    }


            //    List<ClaimsRelatedEntitiesReasonPM> ClaimsRelatedEntitiesReasonsChangeSet = ChangeSet.GetAssociatedChanges(ClaimsRelatedEntity, d => d.ClaimsRelatedEntitiesReasons).Cast<ClaimsRelatedEntitiesReasonPM>().ToList();
            //    foreach (ClaimsRelatedEntitiesReasonPM ClaimsRelatedEntsExpDeclar in ClaimsRelatedEntitiesReasonsChangeSet)
            //    {
            //        ClaimsRelatedEntity.ClaimsRelatedEntitiesReasons.Where(d => d.ClaimId == ClaimsRelatedEntsExpDeclar.ClaimId && d.CounterKey == ClaimsRelatedEntsExpDeclar.CounterKey && d.ReasonListTypeCode == ClaimsRelatedEntsExpDeclar.ReasonListTypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //        List<ClaimsRelatedEntsReasonsExpPM> ClaimsRelatedEntsReasonsExpsChangeSet = ChangeSet.GetAssociatedChanges(ClaimsRelatedEntitiesReason, d => d.ClaimsRelatedEntsReasonsExps).Cast<ClaimsRelatedEntsReasonsExpPM>().ToList();
            //        foreach (ClaimsRelatedEntsReasonsExpPM ClaimsRelatedEntsReasonsExp in ClaimsRelatedEntsReasonsExpsChangeSet)
            //        {
            //            ClaimsRelatedEntitiesReason.ClaimsRelatedEntsReasonsExps.Where(d => d.ClaimId == ClaimsRelatedEntsReasonsExp.ClaimId && d.CounterKey == ClaimsRelatedEntsReasonsExp.CounterKey && d.ReasonListTypeCode == ClaimsRelatedEntsReasonsExp.ReasonListTypeCode && d.ClaimExplanationTypeCode == ClaimsRelatedEntsReasonsExp.ClaimExplanationTypeCode).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //        }


            //    }


            //    List<ClaimsRelatedEntsExpDeclarPM> ClaimsRelatedEntsExpDeclarsChangeSet = ChangeSet.GetAssociatedChanges(ClaimsRelatedEntity, d => d.ClaimsRelatedEntsExpDeclars).Cast<ClaimsRelatedEntsExpDeclarPM>().ToList();
            //    foreach (ClaimsRelatedEntsExpDeclarPM ClaimsRelatedEntsExpDeclar in ClaimsRelatedEntsExpDeclarsChangeSet)
            //    {
            //        ClaimsRelatedEntity.ClaimsRelatedEntsExpDeclars.Where(d => d.ClaimId == ClaimsRelatedEntsExpDeclar.ClaimId && d.CounterKey == ClaimsRelatedEntsExpDeclar.CounterKey && d.ExportDeclarationNumber == ClaimsRelatedEntsExpDeclar.ExportDeclarationNumber).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
            //    }


            //}

            service.Update(entityPM, true);

            /*ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Claim", 0, true);
            string email = HttpContext.Current.User.Identity.Name;
            ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
            Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
            if (loggedContact != null)
            {
                ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
            }*/
        }

        public void UpdateClaim(ClaimPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            //SecurityUtility.CheckContactFeature("Claim", "UPDATE", entityPM.Tenant);
            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(entityPM.Tenant);
            };
            ClaimUpdateService service = new ClaimUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            SetClaimImporterDeclarsPage3ChangeSet(entityPM);
            SetClaimsRelatedEntityChangeSet(entityPM);
            ClaimImporterDeclarsPage3AChangeSet(entityPM);
            SetClaimImporterDeclarsPage3BChangeSet(entityPM);
            service.Update(entityPM, true);
        }


        private void SetClaimImporterDeclarsPage3ChangeSet(ClaimPM entityPM)
        {
            List<ClaimImporterDeclarsPage3PM> ClaimImporterDeclarsPage3sChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimImporterDeclarsPage3).Cast<ClaimImporterDeclarsPage3PM>().ToList();

            foreach (ClaimImporterDeclarsPage3PM itemPM in ClaimImporterDeclarsPage3sChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ClaimImporterDeclarsPage3PM currentItemPM = entityPM.ClaimImporterDeclarsPage3.Where(d => d.ClaimId == itemPM.ClaimId && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            SetClaimImporterDeclarsP3LoiChangeSet(currentItemPM);
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ClaimImporterDeclarsPage3PM currentItemPM = entityPM.ClaimImporterDeclarsPage3.Where(d => d.ClaimId == itemPM.ClaimId && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            SetClaimImporterDeclarsP3LoiChangeSet(currentItemPM);
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            ClaimImporterDeclarsPage3PM currentItemPM = new ClaimImporterDeclarsPage3PM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ClaimId = itemPM.ClaimId,
                                LineNo = itemPM.LineNo,
                            };


                            //Delete ClaimsRelatedEntitiesAmounts
                            List<ClaimImporterDeclarsP3LoiPM> ClaimImporterDeclarsP3LoiChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ClaimImporterDeclarsP3Loi).Cast<ClaimImporterDeclarsP3LoiPM>().ToList();
                            foreach (ClaimImporterDeclarsP3LoiPM loiItemPM in ClaimImporterDeclarsP3LoiChangeSet)
                            {
                                ClaimImporterDeclarsP3LoiPM deletedItem = new ClaimImporterDeclarsP3LoiPM()
                                {
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    ClaimId = loiItemPM.ClaimId,
                                    CounterKey = loiItemPM.CounterKey,
                                    LineNo = loiItemPM.LineNo,
                                    Tenant = loiItemPM.Tenant,
                            };

                                currentItemPM.DeletedClaimImporterDeclarsP3Loi.Add(deletedItem);
                            }

                            entityPM.DeletedClaimImporterDeclarsPage3.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            ClaimImporterDeclarsPage3PM currentItemPM = entityPM.ClaimImporterDeclarsPage3.Where(d => d.ClaimId == itemPM.ClaimId && d.ImporterLoiDeclarationTypeCode == itemPM.ImporterLoiDeclarationTypeCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetClaimImporterDeclarsP3LoiChangeSet(ClaimImporterDeclarsPage3PM entityPM)
        {
            var claimImporterDeclarsP3LoiChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimImporterDeclarsP3Loi).Cast<ClaimImporterDeclarsP3LoiPM>().ToList();

            foreach (ClaimImporterDeclarsP3LoiPM itemPM in claimImporterDeclarsP3LoiChangeSet)
            {
                ClaimImporterDeclarsP3LoiPM currentItemPM = entityPM.ClaimImporterDeclarsP3Loi.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.LineNo == itemPM.LineNo).FirstOrDefault();

                switch (ChangeSet.GetChangeOperation(itemPM))
                {

                    case ChangeOperation.Delete:
                        currentItemPM = new ClaimImporterDeclarsP3LoiPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ClaimId = itemPM.ClaimId,
                                CounterKey = itemPM.CounterKey,
                                LineNo = itemPM.LineNo
                            };

                        entityPM.DeletedClaimImporterDeclarsP3Loi.Add(currentItemPM);
                        break;
                    case ChangeOperation.Insert:
                        currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                        break;
                    case ChangeOperation.Update:
                        currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                        break;
                    case ChangeOperation.None:
                    default:
                        break;
                }
            }

        }

        private void SetClaimsRelatedEntityChangeSet(ClaimPM entityPM)
        {
            List<ClaimsRelatedEntityPM> ClaimsRelatedEntitysChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimsRelatedEntities).Cast<ClaimsRelatedEntityPM>().ToList();

            foreach (ClaimsRelatedEntityPM itemPM in ClaimsRelatedEntitysChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ClaimsRelatedEntityPM currentItemPM = entityPM.ClaimsRelatedEntities.Where(d => d.ClaimId == itemPM.ClaimId && d.EntityCounterKey == itemPM.EntityCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            SetClaimsRelatedEntitiesAmountChangeSet(currentItemPM);

                            SetClaimsRelatedEntitiesReasonChangeSet(currentItemPM);

                            SetClaimsRelatedEntsExpDeclarChangeSet(currentItemPM);

                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ClaimsRelatedEntityPM currentItemPM = entityPM.ClaimsRelatedEntities.Where(d => d.ClaimId == itemPM.ClaimId && d.EntityCounterKey == itemPM.EntityCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            SetClaimsRelatedEntitiesAmountChangeSet(currentItemPM);
                            SetClaimsRelatedEntitiesReasonChangeSet(currentItemPM);
                            SetClaimsRelatedEntsExpDeclarChangeSet(currentItemPM);

                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            ClaimsRelatedEntityPM currentItemPM = new ClaimsRelatedEntityPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ClaimId = itemPM.ClaimId,
                                EntityCounterKey = itemPM.EntityCounterKey,
                                Tenant = itemPM.Tenant,
                            };

                            //Delete ClaimsRelatedEntitiesAmounts
                            List<ClaimsRelatedEntitiesAmountPM> ClaimsRelatedEntitiesAmountChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ClaimsRelatedEntitiesAmounts).Cast<ClaimsRelatedEntitiesAmountPM>().ToList();
                            foreach (ClaimsRelatedEntitiesAmountPM amountItemPM in ClaimsRelatedEntitiesAmountChangeSet)
                            {
                                ClaimsRelatedEntitiesAmountPM deletedItem = new ClaimsRelatedEntitiesAmountPM()
                                {
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    ClaimId = amountItemPM.ClaimId,
                                    CounterKey = amountItemPM.CounterKey,
                                    LineNo = amountItemPM.LineNo,
                                    Tenant = amountItemPM.Tenant,
                                };

                                currentItemPM.DeletedClaimsRelatedEntitiesAmounts.Add(deletedItem);
                            }

                            //Delete ClaimsRelatedEntitiesReasons
                            List<ClaimsRelatedEntitiesReasonPM> ClaimsRelatedEntitiesReasonChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ClaimsRelatedEntitiesReasons).Cast<ClaimsRelatedEntitiesReasonPM>().ToList();
                            foreach (ClaimsRelatedEntitiesReasonPM reasonItemPM in ClaimsRelatedEntitiesReasonChangeSet)
                            {
                                ClaimsRelatedEntitiesReasonPM deletedItem = new ClaimsRelatedEntitiesReasonPM()
                                {
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    ClaimId = reasonItemPM.ClaimId,
                                    CounterKey = reasonItemPM.CounterKey,
                                    LineNo = reasonItemPM.LineNo,
                                    Tenant = reasonItemPM.Tenant,
                                };

                                //Delete ClaimsRelatedEntitiesReasonExplanation
                                List<ClaimsRelatedEntsReasonsExpPM> ClaimsRelatedEntsReasonsExpChangeSet = ChangeSet.GetAssociatedChanges(reasonItemPM, d => d.ClaimsRelatedEntsReasonsExps).Cast<ClaimsRelatedEntsReasonsExpPM>().ToList();
                                foreach (ClaimsRelatedEntsReasonsExpPM reasonExpItemPM in ClaimsRelatedEntsReasonsExpChangeSet)
                                {
                                    ClaimsRelatedEntsReasonsExpPM deletedExpItem = new ClaimsRelatedEntsReasonsExpPM()
                                    {
                                        ChangeSetOp = ChangeSetOperation.Delete,
                                        ClaimId = reasonExpItemPM.ClaimId,
                                        CounterKey = reasonExpItemPM.CounterKey,
                                        ReasonLineNo = reasonExpItemPM.ReasonLineNo,
                                        LineNo = reasonExpItemPM.LineNo,
                                        Tenant = reasonExpItemPM.Tenant,
                            };

                                    deletedItem.DeletedClaimsRelatedEntsReasonsExps.Add(deletedExpItem);
                                }

                                currentItemPM.DeletedClaimsRelatedEntitiesReasons.Add(deletedItem);
                            }

                            entityPM.DeletedClaimsRelatedEntities.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            ClaimsRelatedEntityPM currentItemPM = entityPM.ClaimsRelatedEntities.Where(d => d.ClaimId == itemPM.ClaimId && d.EntityCounterKey == itemPM.EntityCounterKey).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void ClaimImporterDeclarsPage3AChangeSet(ClaimPM entityPM)
        {
            var listChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimImporterDeclarsPage3A).Cast<ClaimImporterDeclarsPage3APM>().ToList();

            foreach (ClaimImporterDeclarsPage3APM itemPM in listChangeSet)
            {
                var currentItemPM = entityPM.ClaimImporterDeclarsPage3A.Where(d => d.ClaimId == itemPM.ClaimId & d.LineNo == itemPM.LineNo).FirstOrDefault();
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            currentItemPM = new ClaimImporterDeclarsPage3APM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ClaimId = itemPM.ClaimId,
                                LineNo = itemPM.LineNo,
                            };

                            entityPM.DeletedClaimImporterDeclarsPage3A.Add(currentItemPM);
                            break;
                        }
                    default:
                        {
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetClaimImporterDeclarsPage3BChangeSet(ClaimPM entityPM)
        {
            var listChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimImporterDeclarsPage3B).Cast<ClaimImporterDeclarsPage3BPM>().ToList();
            foreach (ClaimImporterDeclarsPage3BPM itemPM in listChangeSet)
            {
                var currentItemPM = entityPM.ClaimImporterDeclarsPage3B.Where(d => d.ClaimId == itemPM.ClaimId & d.LineNo == itemPM.LineNo).FirstOrDefault();
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            currentItemPM = new ClaimImporterDeclarsPage3BPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ClaimId = itemPM.ClaimId,
                                LineNo = itemPM.LineNo,
                            };
                            entityPM.DeletedClaimImporterDeclarsPage3B.Add(currentItemPM);
                            break;
                        }
                    default:
                        {
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetClaimsRelatedEntitiesAmountChangeSet(ClaimsRelatedEntityPM entityPM)
        {
            List<ClaimsRelatedEntitiesAmountPM> ClaimsRelatedEntitiesAmountsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimsRelatedEntitiesAmounts).Cast<ClaimsRelatedEntitiesAmountPM>().ToList();

            foreach (ClaimsRelatedEntitiesAmountPM itemPM in ClaimsRelatedEntitiesAmountsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ClaimsRelatedEntitiesAmountPM currentItemPM = entityPM.ClaimsRelatedEntitiesAmounts.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ClaimsRelatedEntitiesAmountPM currentItemPM = entityPM.ClaimsRelatedEntitiesAmounts.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            ClaimsRelatedEntitiesAmountPM currentEntityPM = new ClaimsRelatedEntitiesAmountPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ClaimId = itemPM.ClaimId,
                                CounterKey = itemPM.CounterKey,
                                LineNo = itemPM.LineNo,
                            };

                            entityPM.DeletedClaimsRelatedEntitiesAmounts.Add(currentEntityPM);
                            break;
                        }

                    default:
                        {
                            ClaimsRelatedEntitiesAmountPM currentItemPM = entityPM.ClaimsRelatedEntitiesAmounts.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetClaimsRelatedEntitiesReasonChangeSet(ClaimsRelatedEntityPM entityPM)
        {
            List<ClaimsRelatedEntitiesReasonPM> ClaimsRelatedEntitiesReasonsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimsRelatedEntitiesReasons).Cast<ClaimsRelatedEntitiesReasonPM>().ToList();

            foreach (ClaimsRelatedEntitiesReasonPM itemPM in ClaimsRelatedEntitiesReasonsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ClaimsRelatedEntitiesReasonPM currentItemPM = entityPM.ClaimsRelatedEntitiesReasons.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            SetClaimsRelatedEntsReasonsExpChangeSet(currentItemPM);

                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ClaimsRelatedEntitiesReasonPM currentItemPM = entityPM.ClaimsRelatedEntitiesReasons.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            SetClaimsRelatedEntsReasonsExpChangeSet(currentItemPM);

                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            ClaimsRelatedEntitiesReasonPM currentEntityPM = new ClaimsRelatedEntitiesReasonPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ClaimId = itemPM.ClaimId,
                                CounterKey = itemPM.CounterKey,
                                LineNo = itemPM.LineNo,
                            };

                            //Delete ClaimsRelatedEntitiesReasonExplanation
                            List<ClaimsRelatedEntsReasonsExpPM> ClaimsRelatedEntsReasonsExpChangeSet = ChangeSet.GetAssociatedChanges(itemPM, d => d.ClaimsRelatedEntsReasonsExps).Cast<ClaimsRelatedEntsReasonsExpPM>().ToList();
                            foreach (ClaimsRelatedEntsReasonsExpPM reasonItemPM in ClaimsRelatedEntsReasonsExpChangeSet)
                            {
                                ClaimsRelatedEntsReasonsExpPM deletedItem = new ClaimsRelatedEntsReasonsExpPM()
                                {
                                    ChangeSetOp = ChangeSetOperation.Delete,
                                    ClaimId = reasonItemPM.ClaimId,
                                    CounterKey = reasonItemPM.CounterKey,
                                    ReasonLineNo = reasonItemPM.ReasonLineNo,
                                    LineNo = reasonItemPM.LineNo,
                                    Tenant = reasonItemPM.Tenant,
                            };

                                currentEntityPM.DeletedClaimsRelatedEntsReasonsExps.Add(deletedItem);
                            }

                            entityPM.DeletedClaimsRelatedEntitiesReasons.Add(currentEntityPM);
                            break;
                        }

                    default:
                        {
                            ClaimsRelatedEntitiesReasonPM currentItemPM = entityPM.ClaimsRelatedEntitiesReasons.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetClaimsRelatedEntsReasonsExpChangeSet(ClaimsRelatedEntitiesReasonPM entityPM)
        {
            List<ClaimsRelatedEntsReasonsExpPM> ClaimsRelatedEntsReasonsExpChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimsRelatedEntsReasonsExps).Cast<ClaimsRelatedEntsReasonsExpPM>().ToList();

            foreach (ClaimsRelatedEntsReasonsExpPM itemPM in ClaimsRelatedEntsReasonsExpChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ClaimsRelatedEntsReasonsExpPM currentItemPM = entityPM.ClaimsRelatedEntsReasonsExps.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.ReasonLineNo == itemPM.ReasonLineNo && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ClaimsRelatedEntsReasonsExpPM currentItemPM = entityPM.ClaimsRelatedEntsReasonsExps.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.ReasonLineNo == itemPM.ReasonLineNo && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;

                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            //ClaimsRelatedEntsReasonsExpPM currentItemPM = new ClaimsRelatedEntsReasonsExpPM()
                            //{
                            //    ChangeSetOp = ChangeSetOperation.Delete,
                            //    ClaimId = itemPM.ClaimId,
                            //    CounterKey = itemPM.CounterKey,
                            //    ReasonLineNo = itemPM.ReasonLineNo,
                            //    LineNo = itemPM.LineNo,
                            //};

                            ClaimsRelatedEntsReasonsExpPM currentItemPM = new ClaimsRelatedEntsReasonsExpPM() { ChangeSetOp = ChangeSetOperation.Delete, ClaimId = itemPM.ClaimId, CounterKey = itemPM.CounterKey, Tenant = entityPM.Tenant, ReasonLineNo = itemPM.ReasonLineNo, LineNo = itemPM.LineNo };
                            entityPM.DeletedClaimsRelatedEntsReasonsExps.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            ClaimsRelatedEntsReasonsExpPM currentItemPM = entityPM.ClaimsRelatedEntsReasonsExps.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.ReasonLineNo == itemPM.ReasonLineNo && d.LineNo == itemPM.LineNo).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetClaimsRelatedEntsExpDeclarChangeSet(ClaimsRelatedEntityPM entityPM)
        {
            List<ClaimsRelatedEntsExpDeclarPM> ClaimsRelatedEntsExpDeclarsChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.ClaimsRelatedEntsExpDeclars).Cast<ClaimsRelatedEntsExpDeclarPM>().ToList();

            foreach (ClaimsRelatedEntsExpDeclarPM itemPM in ClaimsRelatedEntsExpDeclarsChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ClaimsRelatedEntsExpDeclarPM currentItemPM = entityPM.ClaimsRelatedEntsExpDeclars.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.ExportDeclarationNumber == itemPM.ExportDeclarationNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            ClaimsRelatedEntsExpDeclarPM currentItemPM = entityPM.ClaimsRelatedEntsExpDeclars.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.ExportDeclarationNumber == itemPM.ExportDeclarationNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            ClaimsRelatedEntsExpDeclarPM currentItemPM = new ClaimsRelatedEntsExpDeclarPM()
                            {
                                ChangeSetOp = ChangeSetOperation.Delete,
                                ClaimId = itemPM.ClaimId,
                                CounterKey = itemPM.CounterKey,
                                ExportDeclarationNumber = itemPM.ExportDeclarationNumber,

                            };

                            entityPM.DeletedClaimsRelatedEntsExpDeclars.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            ClaimsRelatedEntsExpDeclarPM currentItemPM = entityPM.ClaimsRelatedEntsExpDeclars.Where(d => d.ClaimId == itemPM.ClaimId && d.CounterKey == itemPM.CounterKey && d.ExportDeclarationNumber == itemPM.ExportDeclarationNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        public List<ClaimList> GetClaimFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);

            MyContext = CustomContext.GetContext(tenant);
            ClaimListQueryService listService = new ClaimListQueryService(MyContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        /*public ClaimList GetSingleClaimListByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            }
            MyContext = CustomContext.GetContext(tenant);
            ClaimListQueryService listService = new ClaimListQueryService(MyContext);
            return listService.GetSingleClaimListByCode(code, tenant);
        }*/

        //[Invoke]
        public bool CheckIfCorporationNameExists(ClaimPM entityPM, int tenant)
        {
            var setting = CustomsSettingQueryService.GetSettingByTenant(tenant);
            string claimSubmiterNumber = "";

            if (setting != null)
            {
                claimSubmiterNumber = setting.CustomsAgentId.Length <= 9 ? setting.CustomsAgentId : null;
            }

            bool exists = false;
            if (!string.IsNullOrEmpty(claimSubmiterNumber))
            {
                ClientQueryService clientQueryService = new ClientQueryService(tenant);
                ClientPM clientPM = clientQueryService.GetClientByCode(claimSubmiterNumber, tenant);
                if (clientPM != null && !string.IsNullOrWhiteSpace(clientPM.LocalCorporationName))
                {
                    exists = true;
                }
            }
            return exists;
        }

        public List<ClaimsRelatedEntityList> GetClaimsRelatedEntityListsForClaim(string claimId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (MyContext == null)
            {
                MyContext = CustomContext.GetContext(tenant);
            };

            ClaimsRelatedEntityListQueryService queryService = new ClaimsRelatedEntityListQueryService(MyContext);
            return queryService.GetClaimsRelatedEntityListsForClaim(claimId, tenant);
        }

    }
}
