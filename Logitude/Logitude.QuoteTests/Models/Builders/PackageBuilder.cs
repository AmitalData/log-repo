using Logitude.Base.Models.UserTenantPreparation;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.QuoteTests.Models.Builders
{
    public class PackageBuilder
    {
        private QuotePackagePM _packagePM;

        public PackageBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _packagePM = new QuotePackagePM();
        }

        public PackageBuilder Id(string id)
        {
            _packagePM.Id = id;
            return this;
        }

        public PackageBuilder Tenant(int tenant)
        {
            _packagePM.Tenant = tenant;
            return this;
        }
        public PackageBuilder QuoteId(string QuoteId)
        {
            _packagePM.QuoteId = QuoteId;
            return this;
        }

        public PackageBuilder Height(double? height)
        {
            _packagePM.Height = height;
            return this;
        }

        public PackageBuilder Length(double? length)
        {
            _packagePM.Length = length;
            return this;
        }

        public PackageBuilder Weight(double? weight)
        {
            _packagePM.Weight = weight;
            return this;
        }

        public PackageBuilder Width(double? width)
        {
            _packagePM.Width = width;
            return this;
        }

        public PackageBuilder Quantity(int? quantity)
        {
            _packagePM.Quantity = quantity;
            return this;
        }

        public PackageBuilder ChangeSetOp(int changeSetOp)
        {
            _packagePM.ChangeSetOp = changeSetOp;
            return this;
        }

        public QuotePackagePM Build()
        {
            QuotePackagePM result = _packagePM;

            this.Reset();

            return result;
        }

        public PackageBuilder WithModel(QuotePackagePM packagePM)
        {
            _packagePM = packagePM;
            return this;
        }

        public PackageBuilder WithDefualtValues()
        {
            _packagePM = new QuotePackagePM
            {
                Tenant = UserTenant.Tenant,
            };
            return this;
        }

        public PackageBuilder FromDataTable(Table dataTable)
        {
            _packagePM = dataTable.CreateInstance<QuotePackagePM>();
            return this;
        }
    }
}
