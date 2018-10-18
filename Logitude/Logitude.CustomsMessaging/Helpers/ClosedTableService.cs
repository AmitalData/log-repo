using Logitude.Customs.BL.Contracts;
using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Helpers.ClosedTable;
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

namespace Logitude.CustomsMessaging.Helpers
{
    public class ClosedTableServiceOldNotInUse<TEntityPM> : IUpdateSingleClosedTable
        where TEntityPM : class , IIIGClosedTable, new()
    {
        private List<TEntityPM> _AllDbPM;
        private ICustomContext _CustomContext;
        //private IClosedTableDBService _IClosedTableDBService;
        private string _TableId;
        List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> _MehesTableRows;
        private Func<ICustomContext, ICanGetAllClosedTable<TEntityPM>> _GetAllDbPMFunc;
        private Func<ICustomContext, ICanUpdateClosedTable<TEntityPM>> _CreateNewUpdateServiceFunc;
        private List<string> _List2MakeInactive;
        private int _modify;
        private int _add;
        private int _unChanges;
        private int _Tenant;
        private bool _ForceUpdateUnChanged;


        

        public ClosedTableServiceOldNotInUse(
            ICustomContext CustomContext,
            ///string tableId,
            //SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<TEntityPM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<TEntityPM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
        {
            _CustomContext = CustomContext;
            //_TableId = tableId;
            _MehesTableRows = mehesTableRows;
            _CreateNewUpdateServiceFunc = CreateNewUpdateServiceFunc;
            _GetAllDbPMFunc = GetAllDbPMFunc;
            _Tenant = tenant;
            _ForceUpdateUnChanged = forceUpdateUnChanged;
        }
        public void UpdateSingleClosedTable()
        {
            //_CustomContext = CustomContext.GetContext(0);
            if (false)
            {
                CustomsClosedTableRepository closedTableRep = new CustomsClosedTableRepository(_CustomContext);
                CustomsClosedTable table = closedTableRep.GetSingle(new CustomsClosedTableKeys() { Id = _TableId });

                var objectTableRepository = new ObjectTabelRepository(0);

                table.StatusCode = "2";
                closedTableRep.Update(table);
                closedTableRep.SubmitChanges();
                ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(table.ObjectTableId, 0, false);
                var myMehesSystemTables = new SystemTables();
                string tableName = objectTable.Name.Substring(8);
            }


            Upsert();
        }

        void Upsert()
        //List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> mehesTableRows,
        //Dictionary<string, ObjectTable> objectTables)
        {
            //var mehesTableRows = _CustomResponse.TableData.ToList();
            _MehesTableRows = (new ClosedTableUniqeListGenericService()).MakeUniqeList(_MehesTableRows);

            var qs = _GetAllDbPMFunc(this._CustomContext);
            bool canGetAllWithAdditional = qs.GetType().GetInterfaces().Any(x =>
  x.IsGenericType &&
  x.GetGenericTypeDefinition() == typeof(ICanGetAllWithAdditionalClosedTable<>));
            var qsAdd = qs as ICanGetAllWithAdditionalClosedTable<TEntityPM>;
            if (qsAdd !=null)
            {
                _AllDbPM = qsAdd.GetAllWithAdditional(_Tenant); 
            }
            else
            {
                _AllDbPM = //_IClosedTableDBService.GetAllDbPM(); ;
                qs.GetAll();
            }
            
            //TransactionScopeOption myTransactionScopeOption = TransactionScopeOption.Required;
            //if (_MehesTableRows.Count > 5000)
            //{
            //    myTransactionScopeOption = TransactionScopeOption.RequiresNew;
            //}

            _List2MakeInactive = _AllDbPM.Select(rec => rec.Code).ToList();
            while (_MehesTableRows.Count > 0)
            {
                UpdateAddChankOf(100);
            }//while (_MehesTableRows.Count > 0)
            LogMessagingUtil.Instance.AppendLine("Make Inactive =" + _List2MakeInactive.Count.ToString());
            if (_List2MakeInactive.Count < 1)
            {
                return;
            }

            while (_List2MakeInactive.Count > 0)
            {
                MakeInactive(100);
            }//while (_MehesTableRows.Count > 0)
            //InactiveList( list2MakeInactive;;

        }



        private void MakeInactive(int chunk)
        {
            ICanUpdateClosedTable<TEntityPM> updateService = _CreateNewUpdateServiceFunc(this._CustomContext);
            var makeInactiveChunk = _List2MakeInactive.Take(chunk).ToList();
            foreach (var key in makeInactiveChunk)
            {
                var curDbPM = _AllDbPM.FirstOrDefault(rec => rec.Code == key);
                curDbPM.Inactive = true;
                curDbPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                updateService.Update(curDbPM, false);
                _List2MakeInactive.Remove(key);
            }
            LogMessagingUtil.Instance.AppendLine("makeInactiveChunk " + makeInactiveChunk.Count.ToString());
            SaveMyChanges();
        }

        private void UpdateAddChankOf(int chunk)
        {
            _modify = 0;
            _add = 0;
            _unChanges = 0;
            //using (var tras =new TransactionScope(myTransactionScopeOption,TimeSpan.FromMinutes(1)))

            ICanUpdateClosedTable<TEntityPM> updateService = _CreateNewUpdateServiceFunc(this._CustomContext);  //_IClosedTableDBService.GetNewUpdateService();
            var mehesTableRowsChunk = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>(_MehesTableRows.Take(
                chunk
                //_IClosedTableDBService.GetTransRowMax()
                ).ToList());
            foreach (var mehesTableRow in mehesTableRowsChunk)
            {
                if (string.IsNullOrWhiteSpace(mehesTableRow.id))
                {
                    continue;//???
                }

                DoOne(updateService, mehesTableRow);
            }
            SaveMyChanges();

            LogMessagingUtil.Instance.AppendLine(
                string.Format(
@"SaveChanges 
add = {0}  
modify = {1} 
unChanges ={2}", _add, _modify, _unChanges));
        }

        private void SaveMyChanges()
        {
            try
            {
                LogMessagingUtil.Instance.AppendLine("_CustomContext.SaveChanges ...");
                _CustomContext.SaveChanges();
                //tras.Complete();
            }
            catch (DbEntityValidationException ex)
            {
                LogMessagingUtil.Instance.AppendLine("Maybe u forgot to mapp that this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code); ");
                var FormatedException = ExceptionFormatUtil.GetFormated(ex);

                LogMessagingUtil.Instance.AppendLine("SaveChanges():Exception " + FormatedException.ToString());
                throw FormatedException;
            }
            catch (System.Exception)
            {
                //due No Interface 
                throw;
            }
        }

        private void DoOne(ICanUpdateClosedTable<TEntityPM> updateService, SYSTBL_NG_9001_MSG_SystemTablesResponseTableData mehesTableRow)
        {
            var curDbPM = _AllDbPM.FirstOrDefault(rec => rec.Code == mehesTableRow.id);

            if (curDbPM == null)
            {
                _add++;
                curDbPM = new TEntityPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    Code = mehesTableRow.id
                };


            }
            else if (
                curDbPM.LocalName == mehesTableRow.name &&
                curDbPM.LocalName == mehesTableRow.name &&
                !curDbPM.Inactive
                )
            {
                _unChanges++;
                curDbPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.None;
                if (_ForceUpdateUnChanged)
                {
                    _modify++;
                    curDbPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                }
            }
            else
            {
                _modify++;
                curDbPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            }
            var myIIIGClosedTableDummyTenant = curDbPM as IIIGClosedTableDummyTenant;
            if (myIIIGClosedTableDummyTenant != null)
            {
                myIIIGClosedTableDummyTenant.Tenant = _Tenant;
            }
            _List2MakeInactive.Remove(curDbPM.Code);
            curDbPM.Inactive = false;
            curDbPM.LocalName = mehesTableRow.name;
            curDbPM.SearchFields = mehesTableRow.id + "," + mehesTableRow.name;
            if (curDbPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
            {
                //_IClosedTableDBService.Update(curDbPM);
                bool debug1 = false;
                if (debug1)
                {
                    try
                    {
                        updateService.Update(curDbPM //as TEntityPM
                    , true);
                        _CustomContext.SaveChanges();
                        //tras.Complete();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var FormatedException = ExceptionFormatUtil.GetFormated(ex);

                        LogMessagingUtil.Instance.AppendLine("SaveChanges():Exception " + FormatedException.ToString());
                        throw FormatedException;
                    }
                }
                else
                {
                    updateService.Update(curDbPM //as TEntityPM
                    , false);
                }



            }
            _MehesTableRows.Remove(mehesTableRow);
        }

    }
   

}


#if false

 public class ClosedTableServiceFactory
    {


        public static List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> MakeUniqeList(List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> mehesTableRows)
        {

            mehesTableRows.ForEach(
                rec =>
                {
                    rec.id = rec.id.ToUpper();
                });
            mehesTableRows = mehesTableRows.OrderBy(rec => rec.id).ToList();


            var distinctMehesTableRows = mehesTableRows
  .GroupBy(p => p.id)
  .Select(g => g.Last())
  .ToList();
            int keyIsNullOrWhiteSpace = distinctMehesTableRows.RemoveAll(rec => string.IsNullOrWhiteSpace(rec.id));
            if (keyIsNullOrWhiteSpace > 0)
            {
                LogMessagingUtil.Instance.AppendLine(@" Remove All rows that is   IsNullOrWhiteSpace Count = " + keyIsNullOrWhiteSpace);
            }
            if (mehesTableRows.Count != distinctMehesTableRows.Count)
            {

                var repaet5 = mehesTableRows
                .GroupBy(p => p.id)
                .Where(grp => grp.Count() > 1)
                .Select(grp => grp.Key).Take(5);
                var l = string.Join(",", repaet5);
                LogMessagingUtil.Instance.AppendLine(
@" Distinct  !!!! המכס שלח רשומות בעלי קוד (מפתח ) זהה
_MehesTableRows.Count = " + mehesTableRows.Count.ToString() + @"  
While distinctMehesTableRows.Count =" + distinctMehesTableRows.Count.ToString() + @"
Example:" + l);

                //throw new System.Exception("המכס שלח רשומות בעלי קוד (מפתח ) זהה");
                mehesTableRows = distinctMehesTableRows;
            }
            return mehesTableRows;
        }


        public static IUpdateSingleClosedTable CreateNew(
            ICustomContext customContext,
            string tableName,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> entitySystemTables,
            int tenant)
        {
            IUpdateSingleClosedTable closedTableService = null;
            //var customContext = CustomContext.GetContext(0);
            switch (tableName)
            {
                case "2011":
                case "CustomsHouseType":
                    {
                        closedTableService = new ClosedTableService<CustomsHouseTypePM>(customContext, entitySystemTables,
                            (mycustomContext) => { return new CustomsHouseTypeUpdateService(mycustomContext, new Dictionary<string, IContext>(), tenant); },
                            (mycustomContext) =>
                            {
                                var qs = new CustomsHouseTypeQueryService(mycustomContext);
                                return qs as ICanGetAllClosedTable<CustomsHouseTypePM>;
                            }
                            , tenant
                            ,true
                            );
                        ///xxxclosedTableService.SetAllDB();
                    }
                    break;
                case "1136":
                case "CustomsCountry":
                    {
                        closedTableService = new ClosedTableService<CustomsCountryPM>(customContext, entitySystemTables,
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
                        closedTableService = new ClosedTableService<CityPM>(customContext, entitySystemTables,
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
                        closedTableService = new ClosedTableService<ValidCustomsItemPM>(customContext, entitySystemTables,
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
                        closedTableService = new ClosedTableService<TradeAgreementPM>(customContext, entitySystemTables,
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
                default:

                    break;
            }
            return closedTableService;

        }
    }

    public class DynamicDBService
        : Logitude.CustomsMessaging.Helpers.IClosedTableDBService
    {
        private ICustomContext _CustomContext;
        private dynamic _UpdateService;
        private Func<ICustomContext, dynamic> _CreateNewUpdateServiceFunc;
        private Func<ICustomContext, List<IClosedTable>> _GetAllDbPMFunc;


        public DynamicDBService(ICustomContext CustomContext,
            Func<ICustomContext, dynamic> CreateNewUpdateServiceFunc,
            Func<ICustomContext, List<IClosedTable>> GetAllDbPMFunc
            )
        {
            _CustomContext = CustomContext;
            _CreateNewUpdateServiceFunc = CreateNewUpdateServiceFunc;
            _GetAllDbPMFunc = GetAllDbPMFunc;

        }
        public void GetNewUpdateService()
        {
            _UpdateService = _CreateNewUpdateServiceFunc(this._CustomContext);
        }

        public void Update(IClosedTable curDbPM)
        {
            //var realPM = curDbPM as TradeAgreementPM;
            //if (realPM == null)
            //{
            //    throw new System.Exception("dddddddddd");
            //}

            //dynamic updateS = new TradeAgreementUpdateService(_CustomContext);
            //updateS.Update(realPM, true);
            _UpdateService.Update(curDbPM, true);
        }
        public int GetTransRowMax()
        {
            return 1000;
        }

        public List<IClosedTable> GetAllDbPM()
        {
            var list = _GetAllDbPMFunc(this._CustomContext);
            return list;
            var repo = new TradeAgreementRepository(_CustomContext);
            var qs = new TradeAgreementQueryService(_CustomContext);
            var pocos = repo.GetAll().ToList();
            var pms = pocos.Select(poco => qs.GetEntityPM(poco));
            var cts = pms.Select(rec => rec as IClosedTable).ToList();
            return cts;
        }
    }
    public class FactoryService
    {
        private ICustomContext _CustomContext;
        public FactoryService(ICustomContext CustomContext)
        {
            _CustomContext = CustomContext;
        }
        public dynamic CreateUpdateService()
        {
            var updateS = new TradeAgreementUpdateService(_CustomContext) as dynamic;
            return updateS;
        }
        public void Create(out dynamic repo, out dynamic QueryService)
        {
            repo = new TradeAgreementRepository(_CustomContext) as dynamic;
            QueryService = new TradeAgreementQueryService(_CustomContext) as dynamic;
        }
    }

    public class TradeAgreementDBService<TEntityPM>
        : Logitude.CustomsMessaging.Helpers.IClosedTableDBService
        where TEntityPM : IClosedTable, new()
    {
        private ICustomContext _CustomContext;
        private TradeAgreementUpdateService _UpdateService;
        public void GetNewUpdateService()
        {
            _UpdateService = new TradeAgreementUpdateService(_CustomContext);
        }

        public void Update(IClosedTable curDbPM)
        {
            var realPM = curDbPM as TradeAgreementPM;
            if (realPM == null)
            {
                throw new System.Exception("dddddddddd");
            }

            dynamic updateS = new TradeAgreementUpdateService(_CustomContext);
            updateS.Update(realPM, true);
            _UpdateService.Update(realPM, true);
        }
        public int GetTransRowMax()
        {
            return 1000;
        }

        public List<IClosedTable> GetAllDbPM()
        {
            var repo = new TradeAgreementRepository(_CustomContext);
            var qs = new TradeAgreementQueryService(_CustomContext);
            var pocos = repo.GetAll().ToList();
            var pms = pocos.Select(poco => qs.GetEntityPM(poco));
            var cts = pms.Select(rec => rec as IClosedTable).ToList();
            return cts;
        }
    }

    public class TradeAgreementClosedTableService
    {


        void dd()
        {
            //    var allPM = new List<TEntityPM>();
            //    allPM = Repository.GetAll().ToList().Select(poco => GetEntityPM(poco)).ToList();
            //    return allPM;
        }
        public void UpdateSingleClosedTable(string tableId, SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse = null)
        {
            ICustomContext customContext = CustomContext.GetContext(0);
            CustomsClosedTableRepository closedTableRep = new CustomsClosedTableRepository(customContext);
            CustomsClosedTable table = closedTableRep.GetSingle(new CustomsClosedTableKeys() { Id = tableId });

            var objectTableRepository = new ObjectTabelRepository(0);

            table.StatusCode = "2";
            closedTableRep.Update(table);
            closedTableRep.SubmitChanges();
        }
        public void Upsert(
            ICustomContext customContext,
            CustomsClosedTable closedTable,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> entitySystemTables,
            Dictionary<string, ObjectTable> objectTables)
        {
            entitySystemTables.ForEach(
                rec =>
                {
                    rec.id = rec.id.ToUpper();
                });
            entitySystemTables = entitySystemTables.OrderBy(rec => rec.id).ToList();
            if (entitySystemTables.Count !=
                entitySystemTables.Select(rec => rec.id).Distinct().ToList().Count)
            {
                LogMessagingUtil.Instance.AppendLine("distinct  ...");
                throw new System.Exception("distinct  ...");
            }
            var repo = new TradeAgreementRepository(customContext);
            var qs = new TradeAgreementQueryService(customContext);
            var allDbPM = repo.GetAll().ToList().Select(poco => qs.GetEntityPM(poco)).ToList();

            while (entitySystemTables.Count > 0)
            {

                var us = new TradeAgreementUpdateService(customContext);
                var chunk = entitySystemTables.Take(1000);
                foreach (var systemrecord in chunk)
                {
                    var curDbPM = allDbPM.FirstOrDefault(rec => rec.Code == systemrecord.id);

                    if (curDbPM == null)
                    {
                        curDbPM = new TradeAgreementPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Code = systemrecord.id,
                        };

                    }
                    else if (curDbPM.LocalName == systemrecord.name &&
                       curDbPM.LocalName == systemrecord.name)
                    {
                        curDbPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.None;
                    }
                    else
                    {
                        curDbPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                    }
                    curDbPM.LocalName = systemrecord.name;
                    curDbPM.SearchFields = systemrecord.id + "," + systemrecord.name;
                    if (curDbPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
                    {
                        us.Update(curDbPM, true);
                    }
                    entitySystemTables.Remove(systemrecord);
                }


            }
        }


    }
#endif

