using System.Collections.Generic;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Linq;
using Logitude.Server.Tools.Helpers;
using System;
using Logitude.BL.Helpers;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class VatTypeService: DomainService
    {
        bool isNewEntity;
        private int tenant;
        public VatType Poco { get; set; }      
        private VatTypePM entityPM;
        private ICommonDataContext objectContext;
        private VatTypeRepository entityRepository;
        private VATTypesGroupRepository vatTypesGroupRepository;
        private VatTypePercentageRepository vatTypePercentageRepository;
        public VatTypeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new VatTypeRepository(objectContext);
            this.vatTypesGroupRepository = new VATTypesGroupRepository(objectContext);
            this.vatTypePercentageRepository = new VatTypePercentageRepository(objectContext);
        }

        private List<VATTypesGroupPM> vatTypeGroupsChangesSet;
        private List<VatTypePercentagePM> vatTypePercentageChangesSet;
        public void SetChangeSet(List<VatTypePercentagePM> vatTypePercentageChangesSet)
        {
            this.vatTypePercentageChangesSet = vatTypePercentageChangesSet;
        }

        public void Create(VatTypePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;

            this.ValidateCodeExists();

            VatTypeValidating.Validate(entityPM, this.objectContext, this.isNewEntity);

            entityPM.Id = IdCounter.GetNumber("VatType", this.tenant).ToString();

            this.Poco = new VatType()
            {
                Id = entityPM.Id,
                Tenant = this.tenant,
            };

            if (entityPM.VatTypeGroups != null)
            {
                if (entityPM.IsMultiPercentage)
                {
                    foreach (VATTypesGroupPM itemPM in entityPM.VatTypeGroups)
                    {
                        this.CreateVatTypeGroup(itemPM);
                    }
                }

                else
                {
                    entityPM.VatTypeGroups.Clear();
                }
            }

            if (!entityPM.IsMultiPercentage)
            {
                if (entityPM.VatTypePercentages != null)
                {
                    if (entityPM.NewEntityPercentage != null && entityPM.NewEntityPercentageDate != null)
                    {
                        entityPM.VatTypePercentages.Add(new VatTypePercentagePM()
                        {
                            Percentage = entityPM.NewEntityPercentage,
                            FromDate = entityPM.NewEntityPercentageDate,
                        });
                    }

                    foreach (VatTypePercentagePM itemPM in entityPM.VatTypePercentages)
                    {
                        itemPM.Id = IdCounter.GetNumber("VatTypePercentage", this.tenant).ToString();
                        itemPM.VatTypeId = entityPM.Id;

                        VatTypePercentage newPercentage = new VatTypePercentage()
                        {
                            Tenant = this.tenant,
                            Id = itemPM.Id,
                            VatTypeId = itemPM.VatTypeId,
                            Percentage = itemPM.Percentage,
                            FromDate = itemPM.FromDate,
                        };

                        vatTypePercentageRepository.Add(newPercentage);
                    }
                }
            }

            VatTypeTracing.Trace(entityPM, Poco, isNewEntity);
            VatTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }
        public void Update(VatTypePM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleVatType(entityPM.Id, entityPM.Tenant);

            if (mapComposition)
            {
                this.vatTypeGroupsChangesSet = entityPM.VatTypeGroups;
                this.vatTypePercentageChangesSet = entityPM.VatTypePercentages;
            }

            VatTypeValidating.Validate(entityPM, this.objectContext, isNewEntity);

            string entityName = "VatType" + entityPM.Id + entityPM.Tenant;
            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    CacheManager.CacheWrapper.Remove(entityName);
                }
            }

            this.UpdateVatTypeGroups();
            this.UpdateVatTypePercentages();
            
            VatTypeTracing.Trace(entityPM, Poco, isNewEntity);
            VatTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void UpdateVatTypeGroups()
        {
            if (vatTypeGroupsChangesSet != null)
            {
                if (entityPM.IsMultiPercentage)
                {
                    foreach (VATTypesGroupPM itemPM in vatTypeGroupsChangesSet)
                    {
                        switch (itemPM.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    this.CreateVatTypeGroup(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    this.UpdateVatTypeGroup(itemPM);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    this.DeleteVatTypeGroup(itemPM);
                                    break;
                                }
                        }
                    }
                }

                else
                {
                    foreach (VATTypesGroupPM itemPM in vatTypeGroupsChangesSet)
                    {
                        this.DeleteVatTypeGroup(itemPM);
                    }

                    entityPM.VatTypeGroups.Clear();
                    vatTypeGroupsChangesSet.Clear();
                }
            }
        }
        private void UpdateVatTypePercentages()
        {
            if (vatTypePercentageChangesSet != null)
            {
                if (vatTypePercentageChangesSet.Count > 0)
                {
                    VatTypePercentageService service = new VatTypePercentageService(objectContext, this.tenant);

                    foreach (VatTypePercentagePM item in vatTypePercentageChangesSet)
                    {
                        switch (item.ChangeSetOp)
                        {
                            case ChangeSetOperation.Insert:
                                {
                                    service.Create(item, entityPM);
                                    break;
                                }

                            case ChangeSetOperation.Update:
                                {
                                    service.Update(item);
                                    break;
                                }

                            case ChangeSetOperation.Delete:
                                {
                                    VatTypePercentage vatTypePercentage = vatTypePercentageRepository.GetSingleVatTypePercentage(item.Id);
                                    vatTypePercentageRepository.Remove(vatTypePercentage);
                                    break;
                                }

                            default:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
        }


        private void CreateVatTypeGroup(VATTypesGroupPM itemPM)
        {
            itemPM.Tenant = this.tenant;
            itemPM.GroupVATTypeId = this.entityPM.Id;

            VATTypesGroup newVatGroup = new VATTypesGroup()
            {
                Tenant = itemPM.Tenant,
                GroupVATTypeId = itemPM.GroupVATTypeId,
                SingleVATTypeId = itemPM.SingleVATTypeId,
            };

            this.vatTypesGroupRepository.Add(newVatGroup);
        }

        private void UpdateVatTypeGroup(VATTypesGroupPM itemPM)
        {
        }

        private void DeleteVatTypeGroup(VATTypesGroupPM itemPM)
        {
            VATTypesGroup itemPOCO = vatTypesGroupRepository.GetSingleVATTypesGroup(itemPM.GroupVATTypeId, itemPM.SingleVATTypeId, itemPM.Tenant);
            if(itemPOCO != null)
            {
                vatTypesGroupRepository.Remove(itemPOCO);
            }
        }

        private void ValidateCodeExists()
        {
            if (this.isNewEntity)
            {
                bool exist = (from a in entityRepository.GetVatTypes(tenant)
                              where a.Code == entityPM.Code && a.Tenant == entityPM.Tenant
                              select a).Any();

                if (exist)
                {
                    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", this.tenant);
                    msg = msg.Replace("%Entity", "VatType");
                    throw new Exception(msg);
                }
            }
        }
    }
}
