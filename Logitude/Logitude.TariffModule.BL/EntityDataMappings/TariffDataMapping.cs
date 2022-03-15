
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs; 
using Logitude.TariffModule.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityKeys;

namespace Logitude.TariffModule.BL.EntityDataMappings
{
   public partial class TariffDataMapping: IMapping<TariffPM, Tariff>
   {
        public void CustomPMToPOCO(TariffPM entityPM, Tariff entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.ConcurrencyGUID);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;

                if (entityPM.NewConcurrencyGUID == null)
                {
                    entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
                }
            }

            if (!string.IsNullOrEmpty(entityPM.SellerId))
            {
                Card seller = CardRepository.GetSingleCard(entityPM.SellerId, entityPM.Tenant, true);
                if (seller != null)
                {
                    entityPM.SellerPartnerTypeId = seller.PartnerTypeId;
                    entityPOCO.SellerPartnerTypeId = seller.PartnerTypeId;
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomsBrokerId))
            {
                Card broker = CardRepository.GetSingleCard(entityPM.CustomsBrokerId, entityPM.Tenant, true);
                if (broker != null)
                {
                    entityPM.CustomsBrokerPartnerTypeId = broker.PartnerTypeId;
                    entityPOCO.CustomsBrokerPartnerTypeId = broker.PartnerTypeId;
                }
            }

            entityPOCO.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID;
            entityPM.SetAsInActive = false;
            entityPM.SetAsReActive = false;
            entityPM.TariffLinesAdded = false;
            entityPM.IsSurchargeUpdate = false;
            entityPM.FileUploadedName = null;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(TariffPM entityPM, Tariff entityPOCO)
        {
            entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            this.CustomMappedPMProperties.Add(PMPropertyNames.SellerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.SellerPartnerTypeId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomsBrokerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomsBrokerPartnerTypeId);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerGroupName);

            if (!string.IsNullOrEmpty(entityPOCO.SellerId))
            {
                Card seller = CardRepository.GetSingleCard(entityPOCO.SellerId, entityPOCO.Tenant, true);
                if (seller != null)
                {
                    entityPM.SellerName = seller.EnglishName;
                    entityPM.SellerPartnerTypeId = seller.PartnerTypeId;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.CustomerGroupId))
            {
                CustomerGroupRepository customerGroupRepository = new CustomerGroupRepository(entityPOCO.Tenant);
                CustomerGroup customerGroup = customerGroupRepository.GetSingleCustomerGroup(entityPOCO.CustomerGroupId, entityPOCO.Tenant);
                if (customerGroup != null)
                {
                    entityPM.CustomerGroupName = customerGroup.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.CustomsBrokerId))
            {
                Card broker = CardRepository.GetSingleCard(entityPOCO.CustomsBrokerId, entityPOCO.Tenant, true);
                if (broker != null)
                {
                    entityPM.CustomsBrokerName = broker.EnglishName;
                    entityPM.CustomsBrokerPartnerTypeId = broker.PartnerTypeId;
                }
            }

            TariffTypeRepository tariffTypeRepository = new TariffTypeRepository(entityPOCO.Tenant);
            TariffTypeKeys tariffTypetKeys = new TariffTypeKeys() { Code = entityPOCO.TypeCode };
            TariffType tariffType = tariffTypeRepository.GetSingle(tariffTypetKeys);
            if (tariffType != null)
            {
                entityPM.TypeName = tariffType.Name;
            }
        }

        private void BuildSearchFields(TariffPM entityPM, Tariff entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Notes);

            if (entityPM.ContractNumber != null)
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ContractNumber.ToString());
            }

            if (!string.IsNullOrEmpty(entityPM.SellerId))
            {
                Card iCard = CardRepository.GetSingleCard(entityPM.SellerId, entityPM.Tenant, true);
                if (iCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, iCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomsBrokerId))
            {
                Card iCard = CardRepository.GetSingleCard(entityPM.CustomsBrokerId, entityPM.Tenant, true);
                if (iCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, iCard.EnglishName);
                }
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   