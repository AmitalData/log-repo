using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentFieldsBehaviour : IServiceBehaviour
    {
        private ShipmentPM entityPM;
        private ShipmentServiceInitializer initializer;

        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (ShipmentServiceInitializer)initializer;
            this.entityPM = this.initializer.EntityPM;

            this.HandleBehaviour();
        }

        private void HandleBehaviour()
        {           
            if (initializer.IsNewEntity)
            {
                InitializeOnCreating();
            }

            else
            {
                InitializeOnUpdating();
            }

            InitializeFields();
        }



        private void InitializeOnCreating()
        {
            entityPM.FWBStatusCode = "NSEN";
            entityPM.FHLStatusCode = "NSEN";
            entityPM.CargonautFHLStatusCode = "NSEN";
           entityPM.CargonautFWBStatusCode = "NSEN";
            entityPM.ShipmentReceivableStatusCode = "NORE";
            entityPM.ShipmentPayableStatusCode = "NOPA";
            entityPM.INTTRASIStatusCode = "NSEN";
            entityPM.INTTRABookingStatusCode = "NS";
            entityPM.INTTRABookingTransStatusCode = "NST";

            if (string.IsNullOrEmpty(entityPM.CreatedByUserId))
            {
                entityPM.CreatedByUserId = initializer.LoggedContact.Id;
            }

            if (!entityPM.IsHybrid)
            {
                entityPM.CreateDateTime = initializer.TodayDateTime;
            }

            if (entityPM.ShipmentLevelCode == "C")
            {
                entityPM.ProrateReceivables = initializer.LoggedTenant.ProrateMasterReceivables;
            }

            
        }



        private void InitializeOnUpdating()
        {
            entityPM.OldStatusValue = initializer.EntityPOCO.StatusId;


        }

        private void InitializeFields()
        {
            if (string.IsNullOrEmpty(entityPM.ShipmentTypeId) && entityPM.TransportModeId == "A")
            {
                entityPM.ShipmentTypeId = "Air";
            }

            entityPM.House = MethodHelper.Trim(entityPM.House);

            InitializeUpdateFields();
            InitializeProfitExchangeRate();
        }

        private void InitializeUpdateFields()
        {
            entityPM.UpdatedByUserId = initializer.LoggedContactId;
            entityPM.UpdatedByUserName = initializer.LoggedContactName;
            entityPM.LastUpdateDate = initializer.TodayDateTime;

            if (entityPM.IsUpdatedByChampAnalyzer)
            {
                entityPM.UpdatedByPartner = "Airline Transmission";
            }

            else if (entityPM.IsUpdatedByGLSHKAnalyzer)
            {
                entityPM.UpdatedByPartner = "Airline Transmission";
            }

            else if (entityPM.IsUpdatedByINTTRAAnalyzer)
            {
                entityPM.UpdatedByPartner = "INTTRA";
            }

            else
            {
                entityPM.UpdatedByPartner = entityPM.UpdatedByUserName;
            }
        }

        private void InitializeProfitExchangeRate()
        {
            if (initializer.EntityPM.ProfitExchangeRate == null)
            {
                if (!string.IsNullOrEmpty(initializer.EntityPM.ProfitCurrencyId))
                {
                    if (initializer.EntityPM.ProfitCurrencyId == initializer.LoggedTenant.CurrencyId)
                    {
                        initializer.EntityPM.ProfitExchangeRate = 1;
                    }

                    else
                    {
                        RatesTableQuery lastRateQuery = new RatesTableQuery(initializer.Tenant);
                        LastRate lastRate = lastRateQuery.GetLastRecordByValueDate(initializer.Tenant, initializer.EntityPM.ProfitCurrencyId, initializer.LoggedTenant.CurrencyId, initializer.EntityPM.CreateDateTime);
                        if (lastRate != null)
                        {
                            initializer.EntityPM.ProfitExchangeRate = lastRate.Rate;
                        }
                    }
                }
            }
        }
    }
}
