using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.FieldShortNameGetters
{
    public class ObjectFieldsShortNamesGetter : FieldShortNameGetter
    {
        public override void InitializeShortNames()
        {
            FieldShortNames = new Dictionary<string, string>();
            FieldShortNames.Add("DisplayInSearchWindowFiltersIndex", "SearchWindowFiltersIndex");
            FieldShortNames.Add("AllowedInCustomerFieldsSettings", "AllowedInCustFieldsSettings");
        }
    }
}
