using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentNumberBehaviour : IServiceBehaviour
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
                if (!initializer.EntityPM.IsHybrid)
                {
                    this.GetCounterShipmentNumber();
                }
            }

            else
            {
                if (!initializer.EntityPOCO.IsCancelled || !initializer.EntityPM.IsCancelled)
                {
                    if (initializer.EntityPM.ShipmentDirectionConverted)
                    {
                        if (initializer.EntityPM.ShipmentConvertedNewNumber)
                        {
                            this.GetCounterShipmentNumber();
                        }
                    }
                }
            }
        }

        private void GetCounterShipmentNumber()
        {
            bool isTakenCounter = false;

            if (initializer.IsNewEntity && initializer.EntityPM.ShipmentNumber == null)
            {
                isTakenCounter = true;
            }

            else if (initializer.EntityPM.ShipmentDirectionConverted && initializer.EntityPM.ShipmentConvertedNewNumber)
            {
                this.entityPM.OldShipmentNumber = initializer.EntityPM.ShipmentNumber;
                isTakenCounter = true;

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = initializer.Tenant,
                    EventTypeCode = "SNOC",
                    UserId = initializer.LoggedContactId,
                    EntityId = initializer.EntityPM.Id,
                    ObjectTableName = "Shipment",
                    Notes = "Old Number: " + initializer.EntityPM.ShipmentNumber,
                    Entity = initializer.EntityPM,
                });
            }

            if (isTakenCounter)
            {
                string branchId =!string.IsNullOrEmpty(initializer.EntityPM.BranchId) ? initializer.EntityPM.BranchId : GetCreatedByUserBranchId(initializer.EntityPM.CreatedByUserId , initializer.EntityPM.Tenant);
                Dictionary<string, string> counterAdditionalParameters = new Dictionary<string, string>() { { "[B]", "" },{ "[BranchName]",""} };
                if (!string.IsNullOrEmpty(branchId))
                {
                    BranchRepository branchRepository = new BranchRepository(initializer.CommonContext);
                    Branch myBranch = branchRepository.GetSingleBranch(branchId, initializer.EntityPM.Tenant);
                    if(myBranch != null)
                    {
                        counterAdditionalParameters["[BranchName]"] = myBranch.EnglishName;
                    }
                    if (myBranch != null && !string.IsNullOrEmpty(myBranch.CounterCode))
                    {
                        counterAdditionalParameters["[B]"] = myBranch.CounterCode;
                    }
                }

                if (initializer.EntityPM.ShipmentLevelCode == "C")
                {
                    initializer.EntityPM.ShipmentNumber = TableCounter.GetNumber(initializer.Tenant, "MAST", initializer.EntityPM.DirectionId, initializer.EntityPM.TransportModeId, counterAdditionalParameters);
                }

                else
                {
                    if (initializer.EntityPM.DirectionId.ToUpper() == "C")
                    {
                        initializer.EntityPM.ShipmentNumber = TableCounter.GetNumber(initializer.Tenant, "SHIP", "I", initializer.EntityPM.TransportModeId, counterAdditionalParameters);
                    }

                    else
                    {
                        initializer.EntityPM.ShipmentNumber = TableCounter.GetNumber(initializer.Tenant, "SHIP", initializer.EntityPM.DirectionId, initializer.EntityPM.TransportModeId, counterAdditionalParameters);
                    }
                }

                if(initializer.EntityPM.ShipmentLevelCode != "H")
                {
                    initializer.EntityPM.MasterShipmentNumber = initializer.EntityPM.ShipmentNumber;
                }
            }

            if (IsUpdatingHousesConnectedMasters())
            {
                
            }
        }

        private string GetCreatedByUserBranchId(string createdByUserId , int tenant)
        {
            if(string.IsNullOrEmpty(createdByUserId)) return null;
            var commonContext = CommonDataContext.GetContext(tenant);
            User user = (from d in commonContext.Users where d.Id == entityPM.CreatedByUserId && d.Tenant == tenant select d).FirstOrDefault();
            if (user == null) return null;
            return user.BranchId;
        }

        private bool IsUpdatingHousesConnectedMasters()
        {
            if(!initializer.EntityPM.ShipmentDirectionConverted)            
                return false;            

            else if(!initializer.EntityPM.ShipmentConvertedNewNumber)            
                return false;

            else if (initializer.EntityPM.ShipmentLevelCode != "C")
                return false;

            else if (initializer.EntityPM.ShipmentConsoleShipments.Count == 0)
                return false;

            return true;
        }
    }
}
