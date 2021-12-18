using Logitude.BL.QuoteModel.BusinessUnitFilters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.DataWarehouse
{


    public class FactQuoteBusinessUnitFilter
    {
        private QuoteBusinessUnitFilter quoteBusinessUnitFilter = null;
        private List<RoleFeature> featureRoles = null;

        public FactQuoteBusinessUnitFilter(int tenant)
        {
            quoteBusinessUnitFilter = new QuoteBusinessUnitFilter(tenant);
            featureRoles = quoteBusinessUnitFilter.GetFeaturesRoles();
        }



        public string  Run()
        {
            if (featureRoles.Count() == 0 || featureRoles.Where(d => d.FeatureAccessLevelCode == "OR").Any()) return "";
         
            if (featureRoles.Where(d => d.FeatureAccessLevelCode == "US").Any())
            {
                return " Fact_Quotes.[Salesman User Id] = " + quoteBusinessUnitFilter.loggedUser.Id;
            }

            if (featureRoles.Where(d => d.FeatureAccessLevelCode == "BU").Any())
            {
                return " Fact_Quotes.[Business Unit Id] = " + quoteBusinessUnitFilter.loggedUser.BusinessUnitId;
            }

            if (featureRoles.Where(d => d.FeatureAccessLevelCode == "PR").Any())
            {
                return " Fact_Quotes.[Business Unit Id]  LIKE '" + quoteBusinessUnitFilter.loggedUser.BusinessUnitId + "%'";
            }

            return "";
        }
    }
}