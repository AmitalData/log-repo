using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class ReferentPartnerService
    {

        private ShipmentPartnerPM shipmentPartnerPM = null;
        public ShipmentPartnerPM GetReferentPartner(string accountManagerId, int tenant)
        {
            shipmentPartnerPM  = new ShipmentPartnerPM() { PartnerType = "Account Manager" };
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact accountManagerContact = contactRepository.GetSingleContactByIdAndTenant(accountManagerId, tenant, true);
            if (accountManagerContact != null)
            { 
                shipmentPartnerPM.ContactName = accountManagerContact.EnglishName;
                shipmentPartnerPM.Phone =!string.IsNullOrEmpty(accountManagerContact.BusinessPhone) ? accountManagerContact.BusinessPhone : accountManagerContact.Mobile;
                shipmentPartnerPM.Fax = accountManagerContact.Fax;
                shipmentPartnerPM.Email = accountManagerContact.Email;
            }
            //else
            //{
            //    TenantQuery tenantQuery = new TenantQuery(tenant);
            //    var tenantCompany = tenantQuery.GetSinglePM(tenant);
            //    shipmentPartnerPM.PartnerName = tenantCompany.Company;
            //    SetCompanyAddress(tenantCompany);
            //}
            return shipmentPartnerPM;
        }

        //private  void SetCompanyAddress(TenantPM tenantCompany)
        //{
        //    if (!string.IsNullOrEmpty(tenantCompany.AddressId))
        //    {
        //        int tenant = tenantCompany.Id;
        //        AddressRepository addressRepository = new AddressRepository(tenant);
        //        Address address = addressRepository.GetSingleAddress(tenantCompany.AddressId, tenant);
        //        if (address != null)
        //        {
        //            shipmentPartnerPM.Name = address.Name;
        //            shipmentPartnerPM.Address1 = string.IsNullOrEmpty(address.Address1) ? "" : address.Address1;
        //            shipmentPartnerPM.Address2 = string.IsNullOrEmpty(address.Address2) ? "" : address.Address2;
        //            shipmentPartnerPM.Phone = string.IsNullOrEmpty(address.PhoneNumber) ? "" : address.PhoneNumber;
        //            shipmentPartnerPM.Fax = string.IsNullOrEmpty(address.FaxNumber) ? "" : address.FaxNumber;
        //            shipmentPartnerPM.CityZipCode = address.City + (string.IsNullOrEmpty(address.ZipCode) ? "" : ", " + address.ZipCode);
        //            SetCompanyCountry(tenant, address);
        //        }
        //    }
        //}

        //private void SetCompanyCountry(int tenant, Address address)
        //{
        //    CountryRepository countryRepository = new CountryRepository(tenant);
        //    Country country = countryRepository.GetSingleCountryByIdAndTenant(address.CountryId, tenant, true);
        //    if (country != null)
        //    {
        //        shipmentPartnerPM.CountryName = country.EnglishName;
        //        shipmentPartnerPM.FlagSRC = "../images/Flags/" + country.Code + ".png";
        //    }
        //}
    }
}