using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators
{
    public class ShipmentPartnersValidator : IServiceValidator
    {
        private ShipmentServiceInitializer serviceInitializer;
        public void Validate(IServiceInitializer initializer)
        {
             this.serviceInitializer = (ShipmentServiceInitializer)initializer;

            this.RunValidator();
        }

        private void RunValidator()
        {
            if (this.serviceInitializer != null)
            {
                if (!string.IsNullOrEmpty(this.serviceInitializer.EntityPM.ShipperId))
                {
                    ValidateShipmentCustomerPartnersType("Shipper", this.serviceInitializer.EntityPM.ShipperId);
                }

                if (!string.IsNullOrEmpty(this.serviceInitializer.EntityPM.ConsigneeId))
                {
                    ValidateShipmentCustomerPartnersType("Consignee", this.serviceInitializer.EntityPM.ConsigneeId);
                }
            }
        }

        private void ValidateShipmentCustomerPartnersType(string partnerTypeName, string partnerId)
        {
            Card partner = this.serviceInitializer.CardRepository.GetSingleCard(partnerId, this.serviceInitializer.Tenant);
            if (partner != null)
            {
                if (this.serviceInitializer.EntityPM.ShipmentLevelCode == "C")
                {
                    if (this.serviceInitializer.LoggedTenant.AllowCustomersInAgentsLOV)
                    {
                        if (IsPartnerNotCustomerAndAgent(partner))
                        {
                            throw new ApplicationException(partnerTypeName + " partner type should be agent or customer");
                        }
                    }

                    else
                    {
                        if (partner.PartnerTypeId != "AG")
                        {
                            throw new ApplicationException(partnerTypeName + " partner type should be agent");
                        }
                    }
                }
                else
                {
                    if (this.serviceInitializer.LoggedTenant.AllowAgentInCustomersLOV)
                    {
                        if (IsPartnerNotCustomerAndAgent(partner))
                        {
                            ValidateDirectOrHouseShipmentCustomerPartnersType(partner, partnerTypeName);
                        }
                    }

                    else
                    {
                        if (partner.PartnerTypeId != "CS")
                        {
                            ValidateDirectOrHouseShipmentCustomerPartnersType(partner, partnerTypeName);
                        }
                    }
                }
            }
        }

        private void ValidateDirectOrHouseShipmentCustomerPartnersType(Card card, string partnerTypeName)
        {
            if (IsInlandDomesticShipment())
            {
                if (card.PartnerTypeId != "WH")
                {
                    throw new ApplicationException(partnerTypeName + " partner type should be customer or Warehouse");
                }
            }
            else
            {
                throw new ApplicationException(partnerTypeName + " partner type should be customer");
            }
        }

        private bool IsInlandDomesticShipment()
        {
            return this.serviceInitializer.EntityPM.DirectionId == "D" && this.serviceInitializer.EntityPM.TransportModeId == "I";
        }

        private bool IsPartnerNotCustomerAndAgent(Card card)
        {
            return (card.PartnerTypeId != "CS" && card.PartnerTypeId != "AG");
        }


    }
}
