using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class ShipmentFinalArrivalDateBehaviour
    {
        private int Tenant;
        private ShipmentPM entityPM;
        private ShipmentMasterData EntityMasterData;
        private DateTime? FinalArrivalDate;
        private DateTime? ActualFinalArrivalDate;
        private DateTime? EstimatedFinalArrivalDate;
        private List<ShipmentDeliveryPM> Deliveries;
        private IShipmentsContext Context;
        public bool IsUpdatingHouses { get; private set; }
        public ShipmentFinalArrivalDateBehaviour(ShipmentPM entityPM, IShipmentsContext context, bool isNewEntity)
        {
            this.Tenant = entityPM.Tenant;
            this.entityPM = entityPM;
            this.Context = context;
            this.FinalArrivalDate = null;
            this.ActualFinalArrivalDate = null;
            this.EstimatedFinalArrivalDate = null;
            this.Deliveries = entityPM.ShipmentDeliveries.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();

            EntityMasterData = new ShipmentMasterData();
            List<ShipmentPackagePM> myPackagesList = entityPM.ShipmentPackages.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();

            ShipmentMapping.MapConcurrencyFields(entityPM, new Shipment(), EntityMasterData, myPackagesList.Count, isNewEntity, false);
        }

        public void Handle()
        {
            bool HasDeliveries = false;
            bool HasOnCarriage = false;

            if (Deliveries.Count > 0)
            {
                HasDeliveries = true;
                this.HandleDeliveries();
            }

            else if (entityPM.ShipmentLevelCode == "H" && (entityPM.OnForwardingFromPortId != null && entityPM.OnForwardingToPortId != null))
            {
                //HasOnCarriage = true;
                this.HandleOnForwarding();
            }

            else if (entityPM.OnCarriageFromPortId != null && entityPM.OnCarriageToPortId != null)
            {
                HasOnCarriage = true;
                this.HandleOnCarriage();
            }

            if (entityPM.ShipmentLevelCode == "H" && entityPM.MasterShipmentDataId != null && !HasDeliveries)
            {
                bool IsTakingMasterDates = false;

                if (!HasOnCarriage)
                {
                    IsTakingMasterDates = true;
                }

                else if (Context.ShipmentPickUpDeliveries.Where(d => d.Id == entityPM.MasterShipmentDataId && d.PickUpDeliveryTypeCode == "DELV").Any())
                {
                    IsTakingMasterDates = true;
                }

                if (IsTakingMasterDates)
                {
                    this.HandleMasterDates();
                }
            }

            else if (entityPM.ShipmentLevelCode != "H" && !HasDeliveries && !HasOnCarriage)
            {
                if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
                {
                    this.HandleMainCarriage();
                }

                else
                {
                    if (entityPM.Transshipment3FromPortId != null && EntityMasterData.Transshipment3ToPortId != null)
                    {
                        this.HandleTransshipment3();
                    }

                    else if (entityPM.Transshipment2FromPortId != null && EntityMasterData.Transshipment2ToPortId != null)
                    {
                        this.HandleTransshipment2();
                    }

                    else if (entityPM.Transshipment1FromPortId != null && EntityMasterData.Transshipment1ToPortId != null)
                    {
                        this.HandleTransshipment1();
                    }

                    else if (entityPM.MainCarriageFromPortId != null && EntityMasterData.MainCarriageToPortId != null)
                    {
                        this.HandleMainCarriage();
                    }
                }
            }

            if (entityPM.ShipmentLevelCode == "C")
            {
                // 1 On Connect & Disconnect houses
                // 2 if master dates changed

                IsUpdatingHouses = false;

                if (entityPM.FinalArrivalDate != FinalArrivalDate)
                {
                    IsUpdatingHouses = true;
                }

                else if (entityPM.ActualFinalArrivalDate != ActualFinalArrivalDate)
                {
                    IsUpdatingHouses = true;
                }

                else if (entityPM.EstimatedFinalArrivalDate != EstimatedFinalArrivalDate)
                {
                    IsUpdatingHouses = true;
                }
            }

            entityPM.FinalArrivalDate = this.FinalArrivalDate;
            entityPM.ActualFinalArrivalDate = this.ActualFinalArrivalDate;
            entityPM.EstimatedFinalArrivalDate = this.EstimatedFinalArrivalDate;
        }

        private void HandleDeliveries()
        {
            DateTime? DeliveriesDate = null;
            DateTime? ActualDeliveriesDate = null;
            DateTime? EstimatedDeliveriesDate = null;

            foreach (ShipmentDeliveryPM item in Deliveries)
            {
                DateTime? DeliveryDate = item.ATA != null ? item.ATA : item.ETA;

                DeliveriesDate = this.GetBiggestDate(DeliveryDate, DeliveriesDate);
                ActualDeliveriesDate = this.GetBiggestDate(item.ATA, ActualDeliveriesDate);
                EstimatedDeliveriesDate = this.GetBiggestDate(item.ETA, EstimatedDeliveriesDate);
            }

            this.FinalArrivalDate = DeliveriesDate;
            this.ActualFinalArrivalDate = ActualDeliveriesDate;
            this.EstimatedFinalArrivalDate = EstimatedDeliveriesDate;
        }

        private void HandleOnCarriage()
        {
            DateTime? ATA = entityPM.OnCarriageATA;
            DateTime? ETA = entityPM.OnCarriageETA;

            if (ATA != null)
            {
                this.FinalArrivalDate = ATA;
                this.ActualFinalArrivalDate = ATA;
            }

            if (ETA != null)
            {
                if(this.FinalArrivalDate == null)
                {
                    this.FinalArrivalDate= ETA;
                }

                this.EstimatedFinalArrivalDate = ETA;
            }
        }

        private void HandleOnForwarding()
        {
            DateTime? ATA = entityPM.OnForwardingATA;
            DateTime? ETA = entityPM.OnForwardingETA;

            if (ATA != null)
            {
                this.FinalArrivalDate = ATA;
                this.ActualFinalArrivalDate = ATA;
            }

            if (ETA != null)
            {
                if (this.FinalArrivalDate == null)
                {
                    this.FinalArrivalDate = ETA;
                }

                this.EstimatedFinalArrivalDate = ETA;
            }
        }

        private void HandleMasterDates()
        {
            var masterShipmentData = (from d in Context.Shipments
                                      where d.Id == entityPM.MasterShipmentDataId
                                      && d.Tenant == this.Tenant
                                      select new
                                      {
                                          FinalArrivalDate = d.FinalArrivalDate,
                                          ActualFinalArrivalDate = d.ActualFinalArrivalDate,
                                          EstimatedFinalArrivalDate = d.EstimatedFinalArrivalDate
                                      }).FirstOrDefault();

            this.FinalArrivalDate = masterShipmentData.FinalArrivalDate;
            this.ActualFinalArrivalDate = masterShipmentData.ActualFinalArrivalDate;
            this.EstimatedFinalArrivalDate = masterShipmentData.EstimatedFinalArrivalDate;
        }

        private void HandleTransshipment3()
        {
            if (EntityMasterData.Transshipment3ATA != null)
            {
                this.FinalArrivalDate = EntityMasterData.Transshipment3ATA;
                this.ActualFinalArrivalDate = EntityMasterData.Transshipment3ATA;
            }

            if (EntityMasterData.Transshipment3ETA != null)
            {
                if (this.FinalArrivalDate == null)
                {
                    this.FinalArrivalDate = EntityMasterData.Transshipment3ETA;
                }

                this.EstimatedFinalArrivalDate = EntityMasterData.Transshipment3ETA;
            }
        }

        private void HandleTransshipment2()
        {
            if (EntityMasterData.Transshipment2ATA != null)
            {
                this.FinalArrivalDate = EntityMasterData.Transshipment2ATA;
                this.ActualFinalArrivalDate = EntityMasterData.Transshipment2ATA;
            }

            if (EntityMasterData.Transshipment2ETA != null)
            {
                if (this.FinalArrivalDate == null)
                {
                    this.FinalArrivalDate = EntityMasterData.Transshipment2ETA;
                }

                this.EstimatedFinalArrivalDate = EntityMasterData.Transshipment2ETA;
            }
        }

        private void HandleTransshipment1()
        {
            if (EntityMasterData.Transshipment1ATA != null)
            {
                this.FinalArrivalDate = EntityMasterData.Transshipment1ATA;
                this.ActualFinalArrivalDate = EntityMasterData.Transshipment1ATA;
            }

            if (EntityMasterData.Transshipment1ETA != null)
            {
                if (this.FinalArrivalDate == null)
                {
                    this.FinalArrivalDate = EntityMasterData.Transshipment1ETA;
                }

                this.EstimatedFinalArrivalDate = EntityMasterData.Transshipment1ETA;
            }
        }

        private void HandleMainCarriage()
        {
            if (EntityMasterData.MainCarriageATA != null)
            {
                this.FinalArrivalDate = EntityMasterData.MainCarriageATA;
                this.ActualFinalArrivalDate = EntityMasterData.MainCarriageATA;
            }

            if (EntityMasterData.MainCarriageETA != null)
            {
                if (this.FinalArrivalDate == null)
                {
                    this.FinalArrivalDate = EntityMasterData.MainCarriageETA;
                }

                this.EstimatedFinalArrivalDate = EntityMasterData.MainCarriageETA;
            }
        }

        private DateTime? GetBiggestDate(DateTime? newDate, DateTime? oldDate)
        {
            if (newDate != null)
            {
                if (oldDate == null)
                {
                    oldDate = newDate;
                }

                else if (newDate > oldDate)
                {
                    oldDate = newDate;
                }
            }

            return oldDate;
        }
    }
}
