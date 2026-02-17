using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.CW_API.ABM
{
    public class ABMDataContext
    {
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public ShipmentPM Shipment { get; set; }

        public string UserID { get; set; }
        public string Password { get; set; }
        public string CompanyID { get; set; }
        public string ApplicationID { get; set; }

        public string ShipmentNumber { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }

        public string MainCarriageFromPortCountryCode { get; set; }
        public string FinalDestinationPortCountryCode { get; set; }
        public string FromPortCode { get; set; }
        public string FinalDestinationPortCode { get; set; }

        public string ShipperName { get; set; }
        public string ShipperAddress1 { get; set; }
        public string ShipperAddress2 { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperZipCode { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }

        public string ConsigneeName { get; set; }
        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneeZipCode { get; set; }
        public string ConsigneeCountryCode { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }

        public string DescriptionOfGoods { get; set; }

        public string NumberOfPackages { get; set; }
        public string GrossWeight { get; set; }
        public string NetWeight { get; set; } // net weight = gross - tare
        //public string FreightProrate { get; set; }
        //public string WeightProrate { get; set; }

        public string IncotermCode { get; set; }

        public List<ShipmentPackagePM> Containers = new List<ShipmentPackagePM>();

        public ABMDataContext(string shipmentId, int tenant)
        {
            this.Tenant = tenant;
            this.ShipmentId = shipmentId;

            this.GetCredentialsData();
            this.GetShipmentObject();

            if (this.Shipment != null)
            {
                this.BuildGeneralData();
                this.BuildPortsData();
                this.BuildPartnersData();
                this.BuildContainersData();
                this.BuildDimenssionsData();               
            }            
        }

        private void GetCredentialsData()
        {
            this.ApplicationID = LogitudeSettings.ABMProductId;

            CustomsInterfaceSettingRepository rep = new CustomsInterfaceSettingRepository(this.Tenant);
            CustomsInterfaceSetting entity = rep.GetSingleCustomsInterfaceSetting(this.Tenant, this.Tenant);

            if (entity != null)
            {
                this.UserID = entity.LocalUserId;
                this.Password = entity.LocalPassword;
                this.CompanyID = entity.LocalCompanyId;
            }
        }

        private void GetShipmentObject()
        {
            ShipmentQuery query = new ShipmentQuery(this.Tenant);
            Shipment = query.GetSinglePMWithoutComposition(ShipmentId, Tenant);
        }

        private void BuildGeneralData()
        {
            this.ShipmentNumber = this.Shipment.ShipmentNumber;
            this.HouseNumber = this.Shipment.House;
            this.MasterNumber = this.Shipment.Master;

            this.DescriptionOfGoods = this.Shipment.DescriptionOfGoods;
            this.IncotermCode = this.Shipment.IncotermCode;
        }

        private void BuildPortsData()
        {
            this.MainCarriageFromPortCountryCode = Shipment.MainCarriageFromPortCountryCode;
            this.FinalDestinationPortCountryCode = Shipment.MainCarriageFinalDestinationPortCountryCode;
            this.FromPortCode = Shipment.MainCarriageFromPortCode;
            this.FinalDestinationPortCode = Shipment.MainCarriageFinalDestinationPortCode;
        }

        private void BuildPartnersData()
        {
            CountryRepository rep = new CountryRepository(Tenant);

            this.ShipperName = Shipment.ShipperName;
            this.ShipperAddress1 = Shipment.ShipperAddress1;
            this.ShipperAddress2 = Shipment.ShipperAddress2;
            this.ShipperCity = Shipment.ShipperCity;
            this.ShipperZipCode = Shipment.ShipperZipCode;
            this.ShipperReference1 = Shipment.ShipperReference1;
            this.ShipperReference2 = Shipment.ShipperReference2;

            if (!string.IsNullOrEmpty(Shipment.ShipperCountryId))
            {
                Country country = rep.GetSingleCountry(Shipment.ShipperCountryId, Tenant);
                if(country != null)
                {
                    this.ShipperCountryCode = country.Code;
                }
            }           

            this.ConsigneeName = Shipment.ConsigneeName;
            this.ConsigneeAddress1 = Shipment.ConsigneeAddress1;
            this.ConsigneeAddress2 = Shipment.ConsigneeAddress2;
            this.ConsigneeCity = Shipment.ConsigneeCity;
            this.ConsigneeZipCode = Shipment.ConsigneeZipCode;
            this.ConsigneeReference1 = Shipment.ConsigneeReference1;
            this.ConsigneeReference2 = Shipment.ConsigneeReference2;

            if (!string.IsNullOrEmpty(Shipment.ConsigneeCountryId))
            {
                Country country = rep.GetSingleCountry(Shipment.ConsigneeCountryId, Tenant);
                if (country != null)
                {
                    this.ConsigneeCountryCode = country.Code;
                }
            }
        }

        private void BuildContainersData()
        {
            ShipmentPackageQuery rep = new ShipmentPackageQuery(Tenant);

            this.Containers = rep.GetShipmentPackages(ShipmentId, this.ShipmentNumber, Tenant);
        }

        private void BuildDimenssionsData()
        {
            if (Shipment.NumberOfPackages != null)
            {
                this.NumberOfPackages = Shipment.NumberOfPackages.ToString();
            }
            else if (Shipment.NumberOfContainers != null)
            {
                this.NumberOfPackages = Shipment.NumberOfContainers.ToString();
            }

            if (Shipment.GrossWeight != null)
            {
                this.GrossWeight = Shipment.GrossWeight.ToString();

                if(this.Containers.Count() > 0)
                {
                    double? totalTare = 0;

                    totalTare = this.Containers.Where(d => d.Tare != null).Sum(s => s.Tare);

                    this.NetWeight = (Shipment.GrossWeight - totalTare).ToString();
                }
            }            
        }       
    }
}
