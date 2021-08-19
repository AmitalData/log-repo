using Amital.QuoteOPM.BL.Tools.Initializers;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;

using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Amital.QuoteOPM.BL.Tools.Behaviours
{
    public class UpdateQuoteOPComputedFieldBehaviour : IServiceBehaviour
    {
        private QuoteOPServiceInitializer initializer;
        private QuoteOPComputedField quoteComputedField;
        private QuoteOPPM quoteEntityPM;
        private List<QuoteOPChargePM> quoteCharges;
        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (QuoteOPServiceInitializer)initializer;
            if (this.initializer.QuoteComputedFieldPOCO != null)
            {
                this.quoteComputedField = this.initializer.QuoteComputedFieldPOCO;
            }
            if (this.initializer.EntityPM != null)
            {
                this.quoteEntityPM = this.initializer.EntityPM;
            }
            this.HandleBehaviour();
        }
        private void HandleBehaviour()
        {
            MapConnectedToShipmentField();
            MapConnectedToTicketField();
            MapFromLocationField();
            MapToLocationField();
            MapPickupFromField();
            MapDeliveryToField();
            FilterDeletedQuoteCharges();
            MapEstimatedPayablesInLocalCurrencyField();
            MapEstimatedPayablesInSalesCurrencyField();
            MapEstimatedReceivablesInLocalCurrencyField();
            MapEstimatedReceivablesInSalesCurrencyField();
        }

        private void MapConnectedToShipmentField()
        {
            quoteComputedField.ConnectedToShipment = false; //(from shipment in initializer.ShipmentContext.Shipments where shipment.QuoteId == quoteEntityPM.Id && shipment.Tenant == quoteEntityPM.Tenant select shipment).Any();
        }

        private void MapConnectedToTicketField()
        {
             quoteComputedField.ConnectedToTicket = false; //(from tickect in initializer.CRMcontext.Tickets where tickect.QuoteId == quoteEntityPM.Id && tickect.Tenant == quoteEntityPM.Tenant select tickect).Any();
        }

        private void MapFromLocationField()
        {
            if (IsInlandDomestic())
            {        
                   quoteComputedField.FromLocation = GetLocationAddress(quoteEntityPM.FromPartnerAddressId);                    
            }
        }

        private void MapToLocationField()
        {
            if (IsInlandDomestic())
            {
                  quoteComputedField.ToLocation = GetLocationAddress(quoteEntityPM.ToPartnerAddressId); 
            }
        }

        private string GetLocationAddress(string addressId)
        {
            if (!string.IsNullOrEmpty(addressId))
            {
                Address address = initializer.AddressRepository.GetSingleAddress(addressId, initializer.Tenant);
                if (address != null)
                {
                    string locationAddress = (!string.IsNullOrEmpty(address.City)) ? address.City : "";

                    if (address.Country != null)
                    {
                        locationAddress = string.IsNullOrEmpty(locationAddress) ? address.Country.Code : locationAddress + " " + address.Country.Code;
                    }
                  return locationAddress;
                }
            }
            return "";
        }

        private void MapPickupFromField()
        {
            if (quoteEntityPM.IncludePickUp)
            {
                if (!string.IsNullOrEmpty(quoteEntityPM.PickUpAddressId))
                {
                    quoteComputedField.PickupFrom = CalculatePickupAndDeliveryToByAddressId(quoteEntityPM.PickUpAddressId);
                }
                else
                {
                    quoteComputedField.PickupFrom = CalculatePickupFromUsingFromAddressCountryAndCity();
                }
            }
            else
            {
                quoteComputedField.PickupFrom = null;
            }
        }

        private void MapDeliveryToField()
        {
            if (quoteEntityPM.IncludeDelivery)
            {
                if (!string.IsNullOrEmpty(quoteEntityPM.DeliveryAddressId))
                {
                    quoteComputedField.DeliveryTo = CalculatePickupAndDeliveryToByAddressId(quoteEntityPM.DeliveryAddressId);
                }
                else
                {
                    quoteComputedField.DeliveryTo = CalculateDeliveryToUsingToAddressCountryAndCity();
                }
            }
            else
            {
                quoteComputedField.DeliveryTo = null;
            }
        }

        private bool IsInlandDomestic()
        {
                return (quoteEntityPM.DirectionId == "D" && quoteEntityPM.TransportModeId == "I");
        }

        private string CalculatePickupAndDeliveryToByAddressId(string id)
        {
            Address address = initializer.AddressRepository.GetSingleAddress(id, initializer.Tenant);
            if (address != null)
            {
                string location = (!string.IsNullOrEmpty(address.City)) ? address.City : "";

                if (address.Country != null)
                {
                    location = (string.IsNullOrEmpty(location)) ? address.Country.Code : location + " " + address.Country.Code;
                }
                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    location = (string.IsNullOrEmpty(location)) ? address.ZipCode : location + " - " + address.ZipCode;
                }
               return location;
            }
            return "";
        }

        private string CalculatePickupFromUsingFromAddressCountryAndCity()
        {

            string location = (!string.IsNullOrEmpty(quoteEntityPM.FromAddressCity))? quoteEntityPM.FromAddressCity:"";

            if (!string.IsNullOrEmpty(quoteEntityPM.FromAddressCountryId))
            {
                Country country = initializer.CountryRepository.GetSingleCountry(quoteEntityPM.FromAddressCountryId, initializer.Tenant);
                if (country != null)
                {
                     location = (string.IsNullOrEmpty(location))? country.Code : location + " " + country.Code;
                }
            }
            if (!string.IsNullOrEmpty(quoteEntityPM.FromAddressZipCode))
            {
                    location = (string.IsNullOrEmpty(location))? quoteEntityPM.FromAddressZipCode: location +" - "+ quoteEntityPM.FromAddressZipCode;
            }
            return location;
        }

        private string CalculateDeliveryToUsingToAddressCountryAndCity()
        {
            string location = (!string.IsNullOrEmpty(quoteEntityPM.ToAddressCity)) ? quoteEntityPM.ToAddressCity: "";

            if (!string.IsNullOrEmpty(quoteEntityPM.ToAddressCountryId))
            {
                Country country = initializer.CountryRepository.GetSingleCountry(quoteEntityPM.ToAddressCountryId, initializer.Tenant);
                if (country != null)
                {
                     location = (string.IsNullOrEmpty(location))? country.Code : location + " " + country.Code; ;
                }
            }
            if (!string.IsNullOrEmpty(quoteEntityPM.ToAddressZipCode))
            {
                    location = (string.IsNullOrEmpty(location))?quoteEntityPM.ToAddressZipCode : location += " - " + quoteEntityPM.ToAddressZipCode;
            }
            return location;
        }

        private void FilterDeletedQuoteCharges()
        {
            this.quoteCharges =  new List<QuoteOPChargePM>();
            this.quoteCharges = quoteEntityPM.QuoteCharges.Where(d => d.ChangeSetOp != ChangeSetOperation.Delete).ToList();
        }

        private void MapEstimatedPayablesInLocalCurrencyField()
        {
            if (quoteEntityPM.QuoteCharges != null)
            {
                quoteComputedField.EstimatedPayablesInLocal = quoteCharges.Sum(d => d.CostTotalAmountLocal);
            }
        }

        private void MapEstimatedPayablesInSalesCurrencyField()
        {
            if (quoteEntityPM.QuoteCharges != null)
            {
                quoteComputedField.EstimatedPayablesInSales = quoteCharges.Sum(d => d.CostAmountInSaleCurrency);
            }
        }

        private void MapEstimatedReceivablesInLocalCurrencyField()
        {
            if (quoteEntityPM.QuoteCharges != null)
            {
                quoteComputedField.EstimatedReceivablesInLocal = quoteCharges.Where(d => d.IsAllIN == false).Sum(d => d.SaleTotalAmountLocal);
            }
        }

        private void MapEstimatedReceivablesInSalesCurrencyField()
        {
            if (quoteEntityPM.QuoteCharges != null)
            {
                quoteComputedField.EstimatedReceivablesInSales = quoteCharges.Where(d => d.IsAllIN == false).Sum(d => d.SaleAmountInSaleCurrency);
            } 
        }

    }
}
