using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentCustomerBehaviour : IServiceBehaviour
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
            if (entityPM.ShipmentLevelCode == "C" || string.IsNullOrEmpty(entityPM.CustomerId))
            {
                entityPM.CustomerName = null;
                entityPM.CustomerAddressId = null;
                entityPM.CustomerContactId = null;
                entityPM.CustomerReference1 = null;
                entityPM.CustomerReference2 = null;
            }

            if (entityPM.ShipmentLevelCode == "C")
            {
                entityPM.CustomerId = null;
                entityPM.ShipmentCustomerTypeCode = null;
            }

            else 
            {
                if (!entityPM.IsImporterShipment) this.SetCustomerType();
                this.MapCustomerFields();
                this.GetCustomerEntity();
            }
        }

        private void SetCustomerType()
        {
            if (entityPM.ShipmentCustomerTypeCode == null)
            {
                if (entityPM.DirectionId == "I")
                {
                    entityPM.ShipmentCustomerTypeCode = "CON";
                }

                else
                {
                    entityPM.ShipmentCustomerTypeCode = "SHI";
                }
            }

            else
            {
                //this.CheckCustomerValue();
            }
        }

        private void MapCustomerFields()
        {
            switch (entityPM.ShipmentCustomerTypeCode)
            {
                case "SHI":
                    {
                        this.MapShipper();
                        break;
                    }

                case "CON":
                    {
                        this.MapConsignee();
                        break;
                    }

                default:
                    {
                        if (entityPM.CustomerId != null)
                        {
                            if (entityPM.CustomerId == entityPM.AgentId)
                            {
                                MapAgent();
                            }

                            else if (entityPM.CustomerId == entityPM.IssuingCarrierAgentId)
                            {
                                MapIssuingCarrierAgent();
                            }

                            else if (entityPM.CustomerId == entityPM.CustomAgentExportId)
                            {
                                MapCustomAgentExport();
                            }

                            else if (entityPM.CustomerId == entityPM.CustomAgentImportId)
                            {
                                MapCustomAgentImport();
                            }

                            else if (entityPM.CustomerId == entityPM.Notify1Id)
                            {
                                MapNotify1();
                            }

                            else if (entityPM.CustomerId == entityPM.Notify2Id)
                            {
                                MapNotify2();
                            }

                            else if (entityPM.CustomerId == entityPM.ShipperNotExporterId)
                            {
                                MapShipperNotExporter();
                            }

                            else if (entityPM.CustomerId == entityPM.ConsigneeNotImporterId)
                            {
                                MapConsigneeNotImporter();
                            }

                            else if (entityPM.CustomerId == entityPM.FreightForwarderId)
                            {
                                MapFreightForwarder();
                            }

                            else if (entityPM.CustomerId == entityPM.ColoaderId)
                            {
                                MapColoader();
                            }

                            else if (entityPM.CustomerId == entityPM.CustomClearancePointId)
                            {
                                MapCustomClearance();
                            }
                        }

                        break;
                    }
            }
        }

        private void CheckCustomerValue()
        {
            if (!entityPM.IsExternalAPI)
            {
                if (entityPM.CustomerId == entityPM.ShipperId)
                {
                    entityPM.ShipmentCustomerTypeCode = "SHI";
                }

                else if (entityPM.CustomerId == entityPM.ConsigneeId)
                {
                    entityPM.ShipmentCustomerTypeCode = "CON";
                }

                else if (entityPM.CustomerId == entityPM.AgentId)
                {
                    entityPM.ShipmentCustomerTypeCode = "AGT";
                }

                else if (entityPM.CustomerId == entityPM.IssuingCarrierAgentId)
                {
                    entityPM.ShipmentCustomerTypeCode = "IGT";
                }

                else if (entityPM.CustomerId == entityPM.CustomAgentExportId)
                {
                    entityPM.ShipmentCustomerTypeCode = "CAE";
                }

                else if (entityPM.CustomerId == entityPM.CustomAgentImportId)
                {
                    entityPM.ShipmentCustomerTypeCode = "CAI";
                }

                else if (entityPM.CustomerId == entityPM.Notify1Id)
                {
                    entityPM.ShipmentCustomerTypeCode = "NT1";
                }

                else if (entityPM.CustomerId == entityPM.Notify2Id)
                {
                    entityPM.ShipmentCustomerTypeCode = "NT2";
                }

                else if (entityPM.CustomerId == entityPM.ShipperNotExporterId)
                {
                    entityPM.ShipmentCustomerTypeCode = "SNE";
                }

                else if (entityPM.CustomerId == entityPM.ConsigneeNotImporterId)
                {
                    entityPM.ShipmentCustomerTypeCode = "CNI";
                }

                else if (entityPM.CustomerId == entityPM.FreightForwarderId)
                {
                    entityPM.ShipmentCustomerTypeCode = "FOR";
                }

                else if (entityPM.CustomerId == entityPM.ColoaderId)
                {
                    entityPM.ShipmentCustomerTypeCode = "COL";
                }

                else if (entityPM.CustomerId == entityPM.CustomClearancePointId)
                {
                    entityPM.ShipmentCustomerTypeCode = "CCP";
                }

                else if (entityPM.CustomerId == entityPM.ConsolidatorId)
                {
                    entityPM.ShipmentCustomerTypeCode = "CSD";
                }

                else if (entityPM.CustomerId == entityPM.ReleasingAgentId)
                {
                    entityPM.ShipmentCustomerTypeCode = "REA";
                }

                else
                {
                    entityPM.ShipmentCustomerTypeCode = "OTH";
                }
            }
        }

        private void MapShipper()
        {
            if (!entityPM.IsImporterShipment)
            {
                if (string.IsNullOrEmpty(entityPM.CustomerId))
                {
                    entityPM.CustomerId = entityPM.ShipperId;
                }

                entityPM.CustomerName = entityPM.ShipperName;
                entityPM.CustomerNote = entityPM.ShipperNote;
                entityPM.CustomerContactId = entityPM.ShipperContactId;
                entityPM.CustomerAddressId = entityPM.ShipperAddressId;
                entityPM.CustomerReference1 = entityPM.ShipperReference1;
                entityPM.CustomerReference2 = entityPM.ShipperReference2;
            }
        }

        private void MapConsignee()
        {
            if (string.IsNullOrEmpty(entityPM.CustomerId))
            {
                entityPM.CustomerId = entityPM.ConsigneeId;
            }

            entityPM.CustomerName = entityPM.ConsigneeName;
            entityPM.CustomerNote = entityPM.ConsigneeNote;
            entityPM.CustomerContactId = entityPM.ConsigneeContactId;
            entityPM.CustomerAddressId = entityPM.ConsigneeAddressId;
            entityPM.CustomerReference1 = entityPM.ConsigneeReference1;
            entityPM.CustomerReference2 = entityPM.ConsigneeReference2;
        }

        private void MapAgent()
        {
            entityPM.CustomerName = entityPM.AgentName;
            entityPM.CustomerNote = entityPM.AgentNote;
            entityPM.CustomerContactId = entityPM.AgentContactId;
            entityPM.CustomerAddressId = entityPM.AgentAddressId;
            entityPM.CustomerReference1 = entityPM.AgentReference1;
            entityPM.CustomerReference2 = entityPM.AgentReference2;
        }

        private void MapIssuingCarrierAgent()
        {
            entityPM.CustomerName = entityPM.IssuingCarrierAgentName;
            entityPM.CustomerNote = entityPM.IssuingCarrierAgentNote;
            entityPM.CustomerContactId = null;
            entityPM.CustomerAddressId = entityPM.IssuingCarrierAddressId;
            entityPM.CustomerReference1 = null;
            entityPM.CustomerReference2 = null;
        }

        private void MapCustomAgentExport()
        {
            entityPM.CustomerName = entityPM.CustomAgentExportName;
            entityPM.CustomerNote = entityPM.CustomAgentExportNote;
            entityPM.CustomerContactId = entityPM.CustomAgentExportContactId;
            entityPM.CustomerAddressId = entityPM.CustomAgentExportAddressId;
            entityPM.CustomerReference1 = entityPM.CustomAgentExportReference;
            entityPM.CustomerReference2 = null;
        }

        private void MapCustomAgentImport()
        {
            entityPM.CustomerName = entityPM.CustomAgentImportName;
            entityPM.CustomerNote = entityPM.CustomAgentImportNote;
            entityPM.CustomerContactId = entityPM.CustomAgentImportContactId;
            entityPM.CustomerAddressId = entityPM.CustomAgentImportAddressId;
            entityPM.CustomerReference1 = entityPM.CustomAgentImportReference;
            entityPM.CustomerReference2 = null;
        }

        private void MapNotify1()
        {
            entityPM.CustomerName = entityPM.Notify1Name;
            entityPM.CustomerNote = entityPM.Notify1Note;
            entityPM.CustomerContactId = entityPM.Notify1ContactId;
            entityPM.CustomerAddressId = entityPM.Notify1AddressId;
            entityPM.CustomerReference1 = entityPM.Notify1Reference;
            entityPM.CustomerReference2 = entityPM.Notify1Reference2;
        }

        private void MapNotify2()
        {
            entityPM.CustomerName = entityPM.Notify2Name;
            entityPM.CustomerNote = entityPM.Notify2Note;
            entityPM.CustomerContactId = entityPM.Notify2ContactId;
            entityPM.CustomerAddressId = entityPM.Notify2AddressId;
            entityPM.CustomerReference1 = entityPM.Notify2Reference;
            entityPM.CustomerReference2 = null;
        }

        private void MapShipperNotExporter()
        {
            entityPM.CustomerName = entityPM.ShipperNotExporterName;
            entityPM.CustomerNote = entityPM.ShipperNotExporterNote;
            entityPM.CustomerContactId = entityPM.ShipperNotExporterContactId;
            entityPM.CustomerAddressId = entityPM.ShipperNotExporterAddressId;
            entityPM.CustomerReference1 = entityPM.ShipperNotExporterReference;
            entityPM.CustomerReference1 = entityPM.ShipperNotExporterReference1;
            entityPM.CustomerReference2 = entityPM.ShipperNotExporterReference2;
        }

        private void MapConsigneeNotImporter()
        {
            entityPM.CustomerName = entityPM.ConsigneeNotImporterName;
            entityPM.CustomerNote = entityPM.ConsigneeNotImporterNote;
            entityPM.CustomerContactId = entityPM.ConsigneeNotImporterContactId;
            entityPM.CustomerAddressId = entityPM.ConsigneeNotImporterAddressId;
            entityPM.CustomerReference1 = entityPM.ConsigneeNotImporterReference;
            entityPM.CustomerReference2 = null;
        }

        private void MapFreightForwarder()
        {
            entityPM.CustomerName = entityPM.FreightForwarderName;
            entityPM.CustomerNote = entityPM.FreightForwarderNote;
            entityPM.CustomerContactId = entityPM.FreightForwarderContactId;
            entityPM.CustomerAddressId = entityPM.FreightForwarderAddressId;
            entityPM.CustomerReference1 = entityPM.FreightForwarderReference;
            entityPM.CustomerReference2 = null;
        }

        private void MapColoader()
        {
            entityPM.CustomerName = entityPM.ColoaderName;
            entityPM.CustomerNote = entityPM.ColoaderNote;
            entityPM.CustomerContactId = entityPM.ColoaderContactId;
            entityPM.CustomerAddressId = entityPM.ColoaderAddressId;
            entityPM.CustomerReference1 = entityPM.ColoaderReference1;
            entityPM.CustomerReference2 = null;
        }

        private void MapCustomClearance()
        {
            entityPM.CustomerName = entityPM.CustomClearancePointName;
            entityPM.CustomerNote = entityPM.CustomClearancePointNote;
            entityPM.CustomerContactId = entityPM.CustomClearancePointContactId;
            entityPM.CustomerAddressId = entityPM.CustomClearancePointAddressId;
            entityPM.CustomerReference1 = entityPM.CustomClearancePointReference1;
            entityPM.CustomerReference2 = null;
        }

        private void GetCustomerEntity()
        {
            if (entityPM.CustomerId != null)
            {
                CustomerRepository customerRepository = new CustomerRepository(initializer.CommonContext);
                Customer customer = customerRepository.GetSingleCustomer(entityPM.CustomerId, entityPM.Tenant, true);
                if (customer != null)
                {
                    initializer.SetCustomer(customer);
                }
            }
        }
    }
}
