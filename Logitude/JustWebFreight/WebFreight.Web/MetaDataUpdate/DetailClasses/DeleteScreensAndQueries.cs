using System.Collections.Generic;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace WebFreight.Web.MetaDataUpdate.DetailClasses
{
    public class DeleteScreensAndQueries
    {
        public static void DeleteTenantZeroScreens( ScreensRepository screenRepository,ScreenFieldsRepository screenfieldRepository, List<Screen> tenantScreens,List<ScreenField> screenfields)
        {
            if (tenantScreens != null)
            {
                if (tenantScreens.Count != 0)
                {
                    foreach (ScreenField screenfield in screenfields)
                    {
                        screenfieldRepository.Remove(screenfield);
                    }
                    foreach (Screen screen in tenantScreens)
                    {
                        screenRepository.Remove(screen);
                    }
                    screenfieldRepository.SubmitChanges();
                    screenRepository.SubmitChanges();
                }
            }

        }



        public static void DeleteTenantZeroQueries(QueryRepository queryRepository, QueryColumnRepository queryColumnRepository, AdvancedQueryFilterRepository advancedQueryFilterRepository, List<Query> tenantQueries, List<QueryColumn> tenantQueryColumns,List<AdvancedQueryFilter> tenantAdvancedQueryFilters)
        {
            if (tenantQueries != null)
            {
                if (tenantQueries.Count != 0)
                {
                    foreach (QueryColumn queryColumn in tenantQueryColumns)
                    {
                        queryColumnRepository.Remove(queryColumn);
                    }
                  
                    foreach (AdvancedQueryFilter advancedQueryFilter in tenantAdvancedQueryFilters)
                    {
                       advancedQueryFilterRepository.Remove(advancedQueryFilter);
                    }

                    foreach (Query query in tenantQueries)
                    {
                        queryRepository.Remove(query);
                    }

                    queryColumnRepository.SubmitChanges();
                  
                    advancedQueryFilterRepository.SubmitChanges();
                    queryRepository.SubmitChanges();
                }
            }

        }

       


    }
}
