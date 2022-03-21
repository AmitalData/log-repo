using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.LocationsPreparation;
using Logitude.Base.Models.PartnersPreparation;
using Logitude.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;

namespace Logitude.QuoteTests.Models.Builders
{
    public class QuoteBuilder
    {
        private QuotePM _quotePM;

        public QuoteBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _quotePM = new QuotePM();
        }

        public QuoteBuilder Id(string Id)
        {
            _quotePM.Id = Id;
            return this;
        }
        public QuoteBuilder Tenant(int Tenant)
        {
            _quotePM.Tenant = Tenant;
            return this;
        }
        public QuoteBuilder CustomerId(string CustomerId)
        {
            _quotePM.CustomerId = CustomerId;
            return this;
        }
        public QuoteBuilder BranchId(string BranchId)
        {
            _quotePM.BranchId = BranchId;
            return this;
        }
        public QuoteBuilder CreatedByUserId(string CreatedByUserId)
        {
            _quotePM.CreatedByUserId = CreatedByUserId;
            return this;
        }
        public QuoteBuilder UpdatedByUserId(string UpdatedByUserId)
        {
            _quotePM.UpdatedByUserId = UpdatedByUserId;
            return this;
        }
        public QuoteBuilder DirectionId(string DirectionId)
        {
            _quotePM.DirectionId = DirectionId;
            return this;
        }
        public QuoteBuilder QuoteTypeCode(string QuoteTypeCode)
        {
            _quotePM.QuoteTypeCode = QuoteTypeCode;
            return this;
        }
        public QuoteBuilder TransportModeId(string TransportModeId)
        {
            _quotePM.TransportModeId = TransportModeId;
            return this;
        }
        public QuoteBuilder FromPortId(string FromPortId)
        {
            _quotePM.FromPortId = PortCodeMapping(FromPortId);
            return this;
        }
        public QuoteBuilder ToPortId(string ToPortId)
        {
            _quotePM.ToPortId = PortCodeMapping(ToPortId);
            return this;
        }
        public QuoteBuilder QuoteCustomerTypeCode(string QuoteCustomerTypeCode)
        {
            _quotePM.QuoteCustomerTypeCode = QuoteCustomerTypeCode;
            return this;
        }
        public QuoteBuilder SaleCurrencyId(string SaleCurrencyId)
        {
            _quotePM.SaleCurrencyId = SaleCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }
        public QuoteBuilder ExchangeRate(double ExchangeRate)
        {
            _quotePM.ExchangeRate = ExchangeRate;
            return this;
        }
        public QuoteBuilder NumberOfPackages(int? NumberOfPackages)
        {
            _quotePM.NumberOfPackages = NumberOfPackages;
            return this;
        }

        public QuoteBuilder QuoteCharges(QuoteChargePM QuoteCharges)
        {
            if (_quotePM.QuoteCharges == null)
            {
                _quotePM.QuoteCharges = new List<QuoteChargePM>();
            }
            _quotePM.QuoteCharges.Add(QuoteCharges);
            return this;
        }

        public QuoteBuilder QuotePackages(QuotePackagePM QuotePackages)
        {
            if (_quotePM.QuotePackages == null)
            {
                _quotePM.QuotePackages = new List<QuotePackagePM>();
            }
            _quotePM.QuotePackages.Add(QuotePackages);
            return this;
        }

        public QuotePM Build()
        {
            QuotePM result = _quotePM;
            this.Reset();
            return result;
        }

        public QuoteBuilder WithModel(QuotePM quotePM)
        {
            _quotePM = quotePM;
            return this;
        }

        public QuoteBuilder WithDefualtValues()
        {
            _quotePM = new QuotePM
            {
                Tenant = UserTenant.Tenant,
                ConcurrencyGUID = Guid.NewGuid().ToString(),
                BranchId = UserTenant.BranchId,
                DepartmentId = UserTenant.DepartmentId,
                CustomerId = PartnersData.CustomerId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                BusinessUnitId = UserTenant.Tenant.ToString()
            };
            return this;
        }
        private string PortCodeMapping(string portCode)
        {
            switch (portCode)
            {
                case "LHR":
                    return LocationsData.PortLHRId;
                case "MIA":
                    return LocationsData.PortMIADomesticId;
                case "JFK":
                    return LocationsData.PortAirJFKId;
                case "SOU":
                    return LocationsData.PortOceanSOUId;
                case "NYC":
                    return LocationsData.PortInlandNYCId;
                case "LON":
                    return LocationsData.PortLONId;
                case "MAN":
                    return LocationsData.PortMANId;
                default:
                    return null;
            }
        }
    }
}
