using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.Infrastructure;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Tariff.Models.Builders
{
    public class TariffBuilder
    {
        private TariffPM _tariff;
        private static readonly Dictionary<string, string> typeMapper = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"A", "AFC"},
            {"Air", "AFC"},
            {"Ocean LCL", "OLC"},
            {"Ocean FCL", "OFC"},
            {"Air Surcharge", "ASC"},
            {"Ocean LCL Surcharge", "OSC"},
            {"Ocean FCL Surcharge", "OFS"},
        };
        public TariffBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _tariff = new TariffPM();
        }

        public TariffBuilder TypeCode(string typeName)
        {
            _tariff.TypeCode = typeMapper[typeName];
            return this;
        }

        public TariffBuilder ContractNumber(string contractNumber)
        {
            _tariff.ContractNumber = contractNumber;
            return this;
        }

        public TariffBuilder CurrencyId(string currency)
        {
            _tariff.CurrencyId = currency == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }

        public TariffBuilder ExpirationDate(DateTime? expirationDate)
        {
            _tariff.ExpirationDate = expirationDate;
            return this;
        }

        public TariffBuilder StartDate(DateTime startDate)
        {
            _tariff.StartDate = startDate;
            return this;
        }

        public TariffBuilder FreightChargeId(string freightChargeId)
        {
            _tariff.FreightChargeId = freightChargeId;
            return this;
        }

        public TariffBuilder Name(string name)
        {
            _tariff.Name = name;
            return this;
        }

        public TariffBuilder Notes(string notes)
        {
            _tariff.Notes = notes;
            return this;
        }

        public TariffBuilder SellerId(string sellerId)
        {
            _tariff.SellerId = sellerId;
            return this;
        }

        public TariffBuilder TariffProductId(string tariffProductId)
        {
            _tariff.TariffProductId = tariffProductId;
            return this;
        }

        public TariffBuilder Surcharge1Id(string surcharge1Id)
        {
            _tariff.Surcharge1Id = surcharge1Id;
            return this;
        }

        public TariffBuilder Surcharge1UOM(string surcharge1UOM)
        {
            _tariff.Surcharge1UOM = surcharge1UOM;
            return this;
        }

        public TariffBuilder ContainerType1Id(string containerType1Id)
        {
            _tariff.ContainerType1Id = containerType1Id;
            return this;
        }

        public TariffPM Build()
        {
            TariffPM result = _tariff;
            this.Reset();
            return result;
        }

        public TariffBuilder WithModel(TariffPM tariff)
        {
            _tariff = tariff;
            return this;
        }

        public TariffBuilder WithDefualtValues()
        {
            string randomGuid = RandomGeneratorService.RandomGuid();
            _tariff = new TariffPM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                ConcurrencyGUID = randomGuid,
                NewConcurrencyGUID = randomGuid,
                CreateDate = DateTime.Now,
                UpdateDate =DateTime.Now,
                TariffVersions = GetDefaultTariffVersions()
            };
            return this;
        }

        private List<TariffVersionPM> GetDefaultTariffVersions()
        {
            return new List<TariffVersionPM>
            {
                new TariffVersionPM
                {
                    Tenant = UserTenant.Tenant,
                    StartDate = null,
                    ExpirationDate =null,
                    CreateDate =DateTime.Now,
                    Version =1,
                    IsDraft =true,
                    ChangeSetOp ="Insert"
                }
            };
        }
    }
}
