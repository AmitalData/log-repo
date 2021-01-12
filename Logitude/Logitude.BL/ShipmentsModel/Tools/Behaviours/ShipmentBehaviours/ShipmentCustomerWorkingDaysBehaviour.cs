using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EmailAlerts;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours
{
    public class ShipmentCustomerWorkingDaysBehaviour : IServiceBehaviour
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
                UpdateOnCreating();
            }

            else if (!entityPM.IsHybrid)
            {
                UpdateOnUpdating();
            }
        }

        private void UpdateOnCreating()
        {
            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                CustomerRepository customerRepository = new CustomerRepository(initializer.CommonContext);
                Customer customer = customerRepository.GetSingleCustomerWithCardOnly(entityPM.CustomerId, initializer.Tenant, false); // islam: no includes even for the card!

                if (customer != null)
                {
                    if (customer.StartWorkingDate == null && !entityPM.IsHybrid && !initializer.LoggedTenant.IsHybrid)
                    {
                        customer.StartWorkingDate = initializer.TodayDate;
                    }

                    if (customer.FirstShipmentDate == null)
                    {
                        customer.FirstShipmentDate = initializer.LoggedTenant.IsHybrid ? entityPM.CreateDateTime : initializer.TodayDate;

                        CustomerQuery customerQuery = new CustomerQuery(customerRepository);
                        CustomerPM customerpm = new CustomerPM()
                        {
                            Id = customer.Id,
                            Code = customer.Card.Code,
                            EnglishName = customer.Card.EnglishName,
                            VatNumber = customer.Card.VatNumber,
                            SalesmanUserId = customer.Card.SalesmanUserId,
                            CreatedByUserId = customer.Card.CreatedByUserId,
                            RankId = customer.RankId,
                            AccountManagerUserId = customer.AccountManagerUserId,
                            RegionId = customer.RegionId,

                            CustomerSizeId = customer.CustomerSizeId,
                            LastCallDate = customer.LastCallDate,
                            LastMeetingDate = customer.LastMeetingDate,
                            LastOpportunityDate = customer.LastOpportunityDate,
                            FirstInvoiceDate = customer.FirstInvoiceDate,
                            FirstShipmentDate = customer.FirstShipmentDate,
                            LastShipmentDate = customer.LastShipmentDate,
                            StartWorkingDate = customer.StartWorkingDate,
                            StartWorkingManuallySet = customer.StartWorkingManuallySet,
                            LastQuoteDate = customer.LastQuoteDate,
                            LastInteractionDate = customer.LastInteractionDate,
                        };

                        //CustomerMapping.GetMappedPMFromPoco(customer);//customerQuery.GetSinglePM(customer.Id, entityPM.Tenant);
                        if ((DateTime.Today.Date - customer.FirstShipmentDate.Value.Date).Days <= 10)
                        {
                            CustomerEmailAlert customerEmailAlert = new CustomerEmailAlert();
                            customerEmailAlert.SendEmailAlert(customerpm, entityPM.Tenant, "GCFS", false);
                        }
                    }

                    if (initializer.LoggedTenant.IsHybrid)
                    {
                        if (entityPM.CreateDateTime > customer.LastShipmentDate)
                        {
                            customer.LastShipmentDate = entityPM.CreateDateTime;
                        }
                    }

                    else
                    {
                        customer.LastShipmentDate = initializer.TodayDate;
                    }

                    customerRepository.Update(customer);
                    customerRepository.SubmitChanges();
                }
            }
        }

        private void UpdateOnUpdating()
        {
            if (this.entityPM.CustomerId != initializer.EntityPOCO.CustomerId)
            {
                CustomerRepository customerRepository = new CustomerRepository(initializer.CommonContext);

                if (!string.IsNullOrEmpty(this.entityPM.CustomerId))
                {
                    Customer customer = customerRepository.GetSingleCustomerWithCardOnly(entityPM.CustomerId, initializer.Tenant, false);
                    if (customer != null)
                    {
                        customer.LastShipmentDate = this.entityPM.CreateDateTime;
                        customerRepository.Update(customer);
                        customerRepository.SubmitChanges();
                    }
                }

                if (!string.IsNullOrEmpty(initializer.EntityPOCO.CustomerId))
                {
                    Customer customer = customerRepository.GetSingleCustomerWithCardOnly(initializer.EntityPOCO.CustomerId, initializer.Tenant, false);
                    if (customer != null)
                    {
                        Shipment shipment = initializer.ShipmentContext.Shipments.Where(d => d.CustomerId == customer.Id && d.Id != initializer.EntityPOCO.Id).OrderByDescending(s => s.CreateDateTime).FirstOrDefault();
                        if (shipment != null)
                        {
                            customer.LastShipmentDate = shipment.CreateDateTime;
                            customerRepository.Update(customer);
                            customerRepository.SubmitChanges();
                        }
                    }
                }
            }

        }
    }
}
