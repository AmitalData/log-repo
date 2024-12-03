using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
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
//using System.Transactions;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.Helpers.ClosedTable
{
    public class ClosedTableGenericService<TEntityPM> : IUpdateSingleClosedTable
        where TEntityPM : class , IIIGClosedTable, new()
    {
        private List<TEntityPM> _AllDbPM;
        protected ICustomContext _CustomContext;
        //private IClosedTableDBService _IClosedTableDBService;
        private string _TableId;
        List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> _MehesTableRowsQ;
        private Func<ICustomContext, ICanGetAllClosedTable<TEntityPM>> _GetAllDbPMFunc;
        private Func<ICustomContext, ICanUpdateClosedTable<TEntityPM>> _CreateNewUpdateServiceFunc;
        private List<string> _ListPMKey2MakeInactive;
        private int _MakeInactiveCount = 0;
        private int _TotalUpsert;
        

        private int _modify;
        private int _add;
        private int _unChanges;
        protected int _Tenant;
        private bool _ForceUpdateUnChanged;




        public ClosedTableGenericService(
            ICustomContext CustomContext,
            ///string tableId,
            //SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse,
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
            Func<ICustomContext, ICanUpdateClosedTable<TEntityPM>> CreateNewUpdateServiceFunc,
            Func<ICustomContext, ICanGetAllClosedTable<TEntityPM>> GetAllDbPMFunc,
            int tenant,
            bool forceUpdateUnChanged)
        {
            _CustomContext = CustomContext;
            //_TableId = tableId;
            _MehesTableRowsQ = mehesTableRows;
            _CreateNewUpdateServiceFunc = CreateNewUpdateServiceFunc;
            _GetAllDbPMFunc = GetAllDbPMFunc;
            _Tenant = tenant;
            _ForceUpdateUnChanged = forceUpdateUnChanged;
        }
        public void UpdateSingleClosedTable()
        {
            //_CustomContext = CustomContext.GetContext(tenant);
            if (false)
            {
                CustomsClosedTableRepository closedTableRep = new CustomsClosedTableRepository(_CustomContext);
                CustomsClosedTable table = closedTableRep.GetSingle(new CustomsClosedTableKeys() { Id = _TableId });

                var objectTableRepository = new ObjectTableRepository(0);

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
        //List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows,
        //Dictionary<string, ObjectTable> objectTables)
        {
            //var mehesTableRows = _CustomResponse.TableData.ToList();
            _MehesTableRowsQ = MakeUniqeList(_MehesTableRowsQ);

            var qs = _GetAllDbPMFunc(this._CustomContext);
            bool canGetAllWithAdditional = qs.GetType().GetInterfaces().Any(x =>
  x.IsGenericType &&
  x.GetGenericTypeDefinition() == typeof(ICanGetAllWithAdditionalClosedTable<>));
            var qsAdd = qs as ICanGetAllWithAdditionalClosedTable<TEntityPM>;
            if (qsAdd != null)
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

            _ListPMKey2MakeInactive = _AllDbPM.Select(rec =>
                //rec.Code
                GetPMKey(rec)
                ).ToList();
            while (_MehesTableRowsQ.Count > 0)
            {
                UpdateAddChunkOf(100);
            }//while (_MehesTableRows.Count > 0)
            if (SuppressMakeInactiveDueUpdatePatch())
            {
                LogMessagingUtil.Instance.AppendLine("SuppressMakeInactiveDueUpdatePatch !!!");
                return;

            }
            LogMessagingUtil.Instance.AppendLine("Make Inactive =" + _ListPMKey2MakeInactive.Count.ToString());
            if (_ListPMKey2MakeInactive.Count < 1)
            {
                return;
            }

            while (_ListPMKey2MakeInactive.Count > 0)
            {
                MakeInactive(100);
            }//while (_MehesTableRows.Count > 0)
            //InactiveList( list2MakeInactive;;

        }

        protected virtual bool SuppressMakeInactiveDueUpdatePatch()
        {
            return false;
        }






        private void MakeInactive(int chunk)
        {
            
            ICanUpdateClosedTable<TEntityPM> updateService = _CreateNewUpdateServiceFunc(this._CustomContext);
            var makeInactiveChunk = _ListPMKey2MakeInactive.Take(chunk).ToList();
            foreach (var key in makeInactiveChunk)
            {
                var curDbPM = _AllDbPM.FirstOrDefault(rec =>
                    //rec.Code == key
                    this.GetPMKey(rec) == key
                    );
                if (curDbPM.Inactive != true)
                {
                    _MakeInactiveCount++;
                    curDbPM.Inactive = true;
                    curDbPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    updateService.Update(curDbPM, false);
                }
                _ListPMKey2MakeInactive.Remove(key);
            }
            LogMessagingUtil.Instance.AppendLine("makeInactiveChunk " +
                //makeInactiveChunk.Count.ToString()
                _MakeInactiveCount
                );
            SaveMyChanges();
        }

        private void UpdateAddChunkOf(int chunk)
        {
            _modify = 0;
            _add = 0;
            _unChanges = 0;
            //using (var tras =new TransactionScope(myTransactionScopeOption,TimeSpan.FromMinutes(1)))

            ICanUpdateClosedTable<TEntityPM> updateService = _CreateNewUpdateServiceFunc(this._CustomContext);  //_IClosedTableDBService.GetNewUpdateService();
            var mehesTableRowsChunk = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>(_MehesTableRowsQ.Take(
                chunk
                //_IClosedTableDBService.GetTransRowMax()
                ).ToList());
            foreach (var mehesTableRow in mehesTableRowsChunk)
            {
                if (string.IsNullOrWhiteSpace(
                    //mehesTableRow.id
                    GetPMKeyFromCustomRow(mehesTableRow)
                    ))
                {
                    continue;//???
                }

                DoOne(updateService, mehesTableRow);
            }
            SaveMyChanges();
            _TotalUpsert += _add + _modify;
            LogMessagingUtil.Instance.AppendLine(
                string.Format(
@"SaveChanges 
add = {0}  
modify = {1} 
unChanges ={2}", _add, _modify, _unChanges));
        }

        

        protected virtual int SaveMyChanges()
        {
            int saved = 0;
            try
            {
                LogMessagingUtil.Instance.AppendLine("_CustomContext.SaveChanges ...");
                saved = _CustomContext.SaveChanges();
                //_TotalSaved += saved;
                
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
            return saved; 
        }

        private void DoOne(ICanUpdateClosedTable<TEntityPM> updateService, SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow)
        {
            var curDbPM = _AllDbPM.FirstOrDefault(rec =>
                //rec.Code == mehesTableRow.id
                GetPMKey(rec).ToLower() == GetPMKeyFromCustomRow(mehesTableRow).ToLower()
                );

            if (curDbPM == null)
            {
                _add++;
                curDbPM = new TEntityPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    //Code = mehesTableRow.id
                };
                SetPMKey(curDbPM, mehesTableRow);

            }
            else
            {
                _ListPMKey2MakeInactive.Remove(
                    //curDbPM.Code
                    GetPMKey(curDbPM)
                    );
                if (IsEqual(mehesTableRow, curDbPM))
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
            }


            if (curDbPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.None)
            {

                var myIIIGClosedTableDummyTenant = curDbPM as IIIGClosedTableDummyTenant;
                if (myIIIGClosedTableDummyTenant != null)
                {
                    myIIIGClosedTableDummyTenant.Tenant = _Tenant;
                }


                SetOtherFields(mehesTableRow, curDbPM);



                bool debug1 = false;
                if (debug1)
                {
                    UpdateDebug1(updateService, curDbPM);
                }
                else
                {
                    updateService.Update(curDbPM //as TEntityPM
                    , false);
                }



            }
            _MehesTableRowsQ.Remove(mehesTableRow);
        }

        private void UpdateDebug1(ICanUpdateClosedTable<TEntityPM> updateService, TEntityPM curDbPM)
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
        protected virtual string GetPMKey(TEntityPM rec)
        {
            return rec.Code;
        }
        protected virtual string GetPMKeyFromCustomRow(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow)
        {
            return mehesTableRow.id;
        }

        protected virtual void SetOtherFields(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, TEntityPM curDbPM)
        {
            curDbPM.Inactive = false;
            curDbPM.LocalName = mehesTableRow.name;
            curDbPM.SearchFields = mehesTableRow.id + "," + mehesTableRow.name;
            curDbPM.Inactive = TranslateMehesInactive(mehesTableRow);
            if (string.IsNullOrWhiteSpace(curDbPM.EnglishName))
            {
                curDbPM.EnglishName = mehesTableRow.name;
            }
        }

        protected virtual bool IsEqual(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow, TEntityPM curDbPM)
        {
            return curDbPM.LocalName == mehesTableRow.name &&
                            curDbPM.LocalName == mehesTableRow.name &&
                            curDbPM.Inactive == TranslateMehesInactive(mehesTableRow) &&
                            curDbPM.SearchFields == mehesTableRow.id + "," + mehesTableRow.name;
            ;
        }

        protected bool TranslateMehesInactive(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow)
        {
            return mehesTableRow.state <= 0;
        }

        protected virtual void SetPMKey(TEntityPM curDbPM, SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt mehesTableRow)
        {
            curDbPM.Code = mehesTableRow.id;
        }
        protected virtual List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> MakeUniqeList(List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt> mehesTableRows)
        {
            return (new ClosedTableUniqeListGenericService()).MakeUniqeList(mehesTableRows);
        }


        public bool ClosedTableHasChanged()
        {
            if (_TotalUpsert + _MakeInactiveCount > 0)
            {
                return true;
            }
            return false;
        }

        
    }


}
