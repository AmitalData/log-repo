using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class ClosedTableServiceFactory
    {





        public static IUpdateSingleClosedTable CreateNew(
            ICustomContext customContext,
            string tableName,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> entitySystemTables,
            int tenant) 
        {
            IUpdateSingleClosedTable closedTableService = null;
            //var customContext = CustomContext.GetContext(0);
            switch (tableName)
            {

                case "1118":
                case "CustomsBranch":
                    {

                        closedTableService = new Update1118CustumBranch(customContext, entitySystemTables,
                            (mycustomContext) => { return new CustomsBranchUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant); },
                            (mycustomContext) =>
                            {
                                var qs = new CustomsBranchQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<CustomsBranchPM>;
                            }
                            , tenant
                            , false
                            );
                        ///xxxclosedTableService.SetAllDB();
                    }
                    break;
                case "2011":
                case "CustomsHouseType":
                    {
                        closedTableService = new ClosedTableGenericService<CustomsHouseTypePM>(customContext, entitySystemTables,
                            (mycustomContext) => { return new CustomsHouseTypeUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant); },
                            (mycustomContext) =>
                            {
                                var qs = new CustomsHouseTypeQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<CustomsHouseTypePM>;
                            }
                            , tenant
                            , true
                            );
                        ///xxxclosedTableService.SetAllDB();
                    }
                    break;

                case "43":
                case "UIMessage":
                    {
                        closedTableService = new ClosedTableGenericService<UIMessagePM>(customContext, entitySystemTables,
                            (mycustomContext) => { return new UIMessageUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant); },
                            (mycustomContext) =>
                            {
                                var qs = new UIMessageQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<UIMessagePM>;
                            }
                            , tenant
                            , true
                            );
                        ///xxxclosedTableService.SetAllDB();
                    }
                    break;

                case "1136":
                case "CustomsCountry":
                    {




                        closedTableService = new Update1136CustomsCountry(customContext, entitySystemTables,
                            (mycustomContext) =>
                            {
                                return new
                                    //:EntityUpdateService<City,CityPM,EntityPM>
                                    CustomsCountryUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant);
                            },
                            (mycustomContext) =>
                            {
                                var qs = new CustomsCountryQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<CustomsCountryPM>;
                            }
                            , tenant
                            , false
                            );
                    }
                    break;

                case "6":
                case "City":
                    {
                        //done CityDataMapping
                        //public partial class CityUpdateService : ICanUpdateClosedTable<CityPM>
                        /*
 public partial class CityQueryService : ICanGetAllClosedTable<CityPM>
    {
        public List<CityPM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }
    }
                         */
                        //IClosedTable
                        closedTableService = new ClosedTableGenericService<CityPM>(customContext, entitySystemTables,
                            (mycustomContext) => { return new CityUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant); },
                            (mycustomContext) =>
                            {
                                var qs = new CityQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<CityPM>;
                            }
                            , tenant
                            , false
                            );
                    }
                    break;
                case "1966":
                case "ValidCustomsItems":
                    {
                        closedTableService = new ClosedTableGenericService<ValidCustomsItemPM>(customContext, entitySystemTables,
                            (mycustomContext) => { return new ValidCustomsItemUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant); },
                            (mycustomContext) =>
                            {
                                var qs = new ValidCustomsItemQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<ValidCustomsItemPM>;
                            }
                            , tenant
                            , false
                        );

                    }
                    break;
                case "CustomsCountries":
                    {
                        //        //done CityDataMapping
                        //        closedTableService = new ClosedTableService<CustomsCountryPM>(customContext, entitySystemTables,
                        //(mycustomContext) => { return new CustomsCountryUpdateService(mycustomContext, new Dictionary<string, IContext>(), Tenant); },
                        //(mycustomContext) =>
                        //{
                        //    var qs = new CustomsCountryQueryService(mycustomContext);
                        //    return qs as ICanGetAllClosedTable<CityPM>;
                        //}

                        //);
                    }
                    break;
                case "TradeAgreement":
                    {
                        closedTableService = new ClosedTableGenericService<TradeAgreementPM>(customContext, entitySystemTables,
                            (mycustomContext) => { return new TradeAgreementUpdateService(mycustomContext); },
                            (mycustomContext) =>
                            {
                                var qs = new TradeAgreementQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<TradeAgreementPM>;
                            }
                    , tenant
                    , false
                            );

                    }
                    break;

                case "2653": //טבלת נמלי טעינה - טבלה חדשה 2653 במקום 1344 =Task19842
                case "1344":
                case "InternationalSite":
                    {
                        //closedTableService 
                        var myUpdate1344InternationalSite
                            = new Update1344InternationalSite(customContext, entitySystemTables,
                            (mycustomContext) =>
                            {
                                return new
                                    //:EntityUpdateService<City,CityPM,EntityPM>
                                    InternationalSiteUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant);
                            },
                            (mycustomContext) =>
                            {
                                var qs = new InternationalSiteQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<InternationalSitePM>;
                            }
                            , tenant
                            , false
                            );
                        myUpdate1344InternationalSite.RealTableName = tableName;
                        closedTableService =myUpdate1344InternationalSite;
                        break;
                    }
                    case "1354":
                    case "GovernmentProcedureType":
                    {
                        
                           closedTableService = new Update1354GovernmentProcedureType(customContext, entitySystemTables,
                            (mycustomContext) =>
                            {
                                return new
                                    //:EntityUpdateService<City,CityPM,EntityPM>
                                    GovernmentProcedureTypeUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant);
                            },
                            (mycustomContext) =>
                            {
                                var qs = new GovernmentProcedureTypeQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<GovernmentProcedureTypePM>;
                            }
                            , tenant
                            , false
                            );
                    }
                    break;
                    
                default:

                    break;
            }
            return closedTableService;

        }
    }
}
