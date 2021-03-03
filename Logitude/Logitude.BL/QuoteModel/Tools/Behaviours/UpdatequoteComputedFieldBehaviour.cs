using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.Tools.Initializers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Interfaces;
using System.Linq;

namespace Logitude.BL.QuoteModel.Tools.Behaviours
{
    public class UpdatequoteComputedFieldBehaviour : IServiceBehaviour
    {
        private QuoteServiceInitializer initializer;
        private QuoteComputedField quoteComputedField;
        private QuotePM quoteEntityPM;
        public void Handle(IServiceInitializer initializer)
        {
            this.initializer = (QuoteServiceInitializer)initializer;
            if (this.initializer.quoteComputedFieldPOCO != null)
            {
                this.quoteComputedField = this.initializer.quoteComputedFieldPOCO;
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
            MapDeliveryFromField();
        }

        private void MapConnectedToShipmentField()
        {
             quoteComputedField.ConnectedToShipment = (from shipment in initializer.ShipmentContext.Shipments where shipment.QuoteId == quoteEntityPM.Id && shipment.Tenant == quoteEntityPM.Tenant select shipment).Any();
        }

        private void MapConnectedToTicketField()
        {
             quoteComputedField.ConnectedToTicket = (from tickect in initializer.CRMcontext.Tickets where tickect.QuoteId == quoteEntityPM.Id && tickect.Tenant == quoteEntityPM.Tenant select tickect).Any();
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
                Address toAddress = initializer.addressRepository.GetSingleAddress(addressId, initializer.Tenant);
                if (toAddress != null)
                {
                    string toLocation = (!string.IsNullOrEmpty(toAddress.City)) ? toAddress.City : "";

                    if (toAddress.Country != null)
                    {
                        toLocation = string.IsNullOrEmpty(toLocation) ? toAddress.Country.Code : toLocation + " " + toAddress.Country.Code;
                    }
                  return toLocation;
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
                    quoteComputedField.PickupFrom = CalculatePickupAndDeliveryFromByAddressId(quoteEntityPM.PickUpAddressId);
                }
                else
                {
                    quoteComputedField.PickupFrom = CalculatePickupFromUsingFromAddressCountryAndCity();
                }
            }
        }

        private void MapDeliveryFromField()
        {
            if (quoteEntityPM.IncludeDelivery)
            {
                if (!string.IsNullOrEmpty(quoteEntityPM.DeliveryAddressId))
                {
                    quoteComputedField.DeliveryFrom = CalculatePickupAndDeliveryFromByAddressId(quoteEntityPM.DeliveryAddressId);
                }
                else
                {
                    quoteComputedField.DeliveryFrom = CalculateDeliveryFromUsingToAddressCountryAndCity();
                }
            }
        }

        private bool IsInlandDomestic()
        {
                return (quoteEntityPM.DirectionId == "D" && quoteEntityPM.TransportModeId == "I");
        }

        private string CalculatePickupAndDeliveryFromByAddressId(string id)
        {
            Address address = initializer.addressRepository.GetSingleAddress(id, initializer.Tenant);
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
                Country country = initializer.countryRepository.GetSingleCountry(quoteEntityPM.FromAddressCountryId, initializer.Tenant);
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

        private string CalculateDeliveryFromUsingToAddressCountryAndCity()
        {
            string location = (!string.IsNullOrEmpty(quoteEntityPM.ToAddressCity)) ? quoteEntityPM.ToAddressCity: "";

            if (!string.IsNullOrEmpty(quoteEntityPM.ToAddressCountryId))
            {
                Country country = initializer.countryRepository.GetSingleCountry(quoteEntityPM.ToAddressCountryId, initializer.Tenant);
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
    }
}
