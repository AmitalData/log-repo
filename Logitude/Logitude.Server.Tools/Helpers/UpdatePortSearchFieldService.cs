using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
 public   class UpdatePortSearchFieldService
    {

       private static  Country portCountry = null;
        public static void Update(Port port)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, port.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, port.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, port.LocalName);
            AddCountrySearchFields(port, mySearchFields);
            UpdateCombinedCode(port);
            MethodHelper.AddToSearchFields(ref mySearchFields, port.CombinedCode);
            if (mySearchFields.Length > 1000) mySearchFields = mySearchFields.Substring(0, 1000);
            port.SearchFields = mySearchFields;
        }

        private static void UpdateCombinedCode(Port port)
        {
            if (portCountry == null) return;
            port.CombinedCode = portCountry.Code + port.Code;
        }

        private static string AddCountrySearchFields(Port port, string mySearchFields)
        {
            if (string.IsNullOrEmpty(port.CountryId)) return mySearchFields;
            portCountry  = CountryRepository.GetSingleCountry(port.CountryId, port.Tenant, true);
            if (portCountry == null) return mySearchFields;

            MethodHelper.AddToSearchFields(ref mySearchFields, portCountry.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, portCountry.EnglishName);
           
            return mySearchFields;
        }
    }
}
