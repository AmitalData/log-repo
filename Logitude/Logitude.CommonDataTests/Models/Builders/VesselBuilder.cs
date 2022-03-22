using Logitude.Base.Models.Partners;
using Logitude.Base.Models.UserTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CommonDataTests.Models.Builders
{
    public class VesselBuilder
    {
        private VesselPM _vessel;
        public VesselBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _vessel = new VesselPM();
        }

        public VesselPM Build()
        {
            VesselPM result = _vessel;
            this.Reset();
            return result;
        }

        public VesselBuilder WithModel(VesselPM vessel)
        {
            _vessel = vessel;
            return this;
        }

        public VesselBuilder Tenant(int tenant)
        {
            _vessel.Tenant = tenant;
            return this;
        }

        public VesselBuilder EnglishName(string englishName)
        {
            _vessel.EnglishName = englishName;
            return this;
        }

        public VesselBuilder IMOCode(string iMOCode)
        {
            _vessel.IMOCode = iMOCode;
            return this;
        }

        public VesselBuilder LocalName(string localName)
        {
            _vessel.LocalName = localName;
            return this;
        }

        public VesselBuilder Notes(string notes)
        {
            _vessel.Notes = notes;
            return this;
        }

        public VesselBuilder WithDefualtValues()
        {
            _vessel = new VesselPM
            {
                Tenant = UserTenant.Tenant
            };
            return this;
        }

    }
}
