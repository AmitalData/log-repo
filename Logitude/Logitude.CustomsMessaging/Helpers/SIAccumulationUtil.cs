using Logitude.Customs.Def.EntityPMs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.EntityDataMappings;
using System.IO;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using System.Runtime.Serialization.Formatters.Binary;
using Logitude.Server.Tools.Helpers;
using System.Diagnostics;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.CustomsMessaging.Helpers
{
    public class SIAccumulationUtil
    {
        private DeclarationPM _DeclarationPM;
        private List<SupplierInvoiceItemPM> _AllSIItems;
        private List<SupplierInvoiceItemPM> _ParentSIItems;
        private List<SupplierInvoiceItemPM> _OriginalSIItems;
        private List<SupplierInvoiceItemPM> _OriginalSIItemsForAccumulation;
        private List<SupplierInvoiceItemPM> _OriginalSIItemWithoutHash;
        private GenericRequestParams _RequestParams;
        private int sequenceNumeric = 0;
        private Stopwatch _Stopwatch;
        int SItoAccumulate = 0;

        public bool HaveException { get; private set; }

        public SIAccumulationUtil()
        {

        }

        public void Run()
        {
            try
            {
                _Stopwatch = Stopwatch.StartNew();
                
                DeclarationPM declarationPM = GetDeclarationComposite();
                _DeclarationPM = declarationPM;

                LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Get Declaration Composite Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                
                var siqs = new SupplierInvoiceQueryService(_RequestParams.Tenant);
                SItoAccumulate = siqs.GetSupplierInvoiceToAccumulateCount(_RequestParams.Tenant, _RequestParams.AppicationId);

                _AllSIItems = _DeclarationPM.SupplierInvoices.SelectMany(si => si.SupplierInvoiceItems).ToList();
                _ParentSIItems = _AllSIItems.ToList()
                .Where(rec => rec.IsParent).ToList();
                    if (_ParentSIItems.Count() > 0)
                    {
                        throw new System.Exception("Fast Delete Needed");
                    }
                ;
                _OriginalSIItems = _AllSIItems
                .Where(rec => !rec.IsParent).ToList();
                //LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Initiate items vars Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                _OriginalSIItemsForAccumulation = _DeclarationPM.SupplierInvoices.Where(rec => rec.AccumalationStateCode != "3").SelectMany(si => si.SupplierInvoiceItems.Where(rec => !rec.IsParent)).ToList();
                //if (_OriginalSIItems.Count() < 999 && SItoAccumulate < 1)
                if (_OriginalSIItemsForAccumulation.Count() < 999)
                {
                    if (SItoAccumulate < 1)
                    {
                        LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "less than 999 items - clear accumulation data");
                        /// when notice there is a change we delete parant !!
                        ClearSIItem(); //ItemHash , ParentLineNumber , ///  ?????? NotForAccumaltion (yes/no), IsParent (yes/no)
                        ClearSI();//IsAccumalted
                        UpdateDeclartion();// +Commit
                        LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "clear accumulation data Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                        return;
                    }
                    else
                    {
                        ClearSIItem(); //ItemHash , ParentLineNumber , IsAccumalted
                        LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "clear accumulation data Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                        _OriginalSIItemsForAccumulation = _DeclarationPM.SupplierInvoices.Where(rec => rec.AccumalationStateCode == "2").SelectMany(si => si.SupplierInvoiceItems.Where(rec => !rec.IsParent)).ToList();
                    }
                }


                //_OriginalSIItemWithoutHash = _OriginalSIItems
                _OriginalSIItemWithoutHash = _OriginalSIItemsForAccumulation
                .Where( rec=> String.IsNullOrWhiteSpace(rec.ItemHash))
                .ToList();

                if (_OriginalSIItemWithoutHash.Count == 0)
                {
                    LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "all items has Hash value - abort accumulation");
                    ///All Original Items Have ItemHash nothing to do ???
                    ///--- maybe to do a check Method ?!?!?  Parent == Original Total ??
                    return;
                }


                CreateHashToEmptyItems();
                LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Create Hash To Empty Items (" + _OriginalSIItemWithoutHash.Count() + ") Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                GroupByHash();
                LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Group By Hash (and adding parents items) Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                UpdateDeclartion();
                LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Update Declartion Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                CheckValid();
            }
            catch (Exception)
            {
                HaveException = true;
                throw;
            }
            finally
            {
                if (!HaveException)
                {
                    CheckParentTotalEqualOriginalItemsTotal();
                }
            }
        }

        private DeclarationPM GetDeclarationComposite()
        {
            if (_DeclarationPM == null)
            {
                ICustomContext dbContext = CustomContext.GetContext(_RequestParams.Tenant);
                var declarationQueryService = new DeclarationQueryService(dbContext);
                declarationQueryService.LoadSupplierInvoicesWithItems = true;
                _DeclarationPM = declarationQueryService.GetSingle(_RequestParams.AppicationId, true, false);
            }
            return (_DeclarationPM);
        }

        private void CheckValid()
        {
            //throw new NotImplementedException();
        }

        private void UpdateDeclartion()
        {
            ICustomContext dbContext = CustomContext.GetContext(_RequestParams.Tenant);
            DeclarationUpdateService DeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), _RequestParams.Tenant);
            this._DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            DeclarationUpdateService.Update(_DeclarationPM,true);
        }

        private void CheckParentTotalEqualOriginalItemsTotal()
        {
            //throw new NotImplementedException();
        }


        private void ClearSI()
        {
            //throw new NotImplementedException();
        }

        private void ClearSIItem()
        {
            ICustomContext dbContext = CustomContext.GetContext(_RequestParams.Tenant);
            foreach (var si in _DeclarationPM.SupplierInvoices)
            {
                if (SItoAccumulate < 1 || si.AccumalationStateCode != "2")
                {
                    si.IsAccumalated = false;
                    si.ChangeSetOp = ChangeSetOperation.Update;
                    si.SupplierInvoiceItems.ForEach(i => i.ItemHash = null);
                    si.SupplierInvoiceItems.ForEach(i => i.ParentLineNumber = null);
                    si.SupplierInvoiceItems.ForEach(i => i.ChangeSetOp = ChangeSetOperation.Update);
                }
                //var mySupplierInvoiceUpdateService = new SupplierInvoiceUpdateService(dbContext, new Dictionary<string, IContext>(), _RequestParams.Tenant);
                //mySupplierInvoiceUpdateService.DeclarationSupplierInvoiceItemsParentsFastDelete(si, dbContext);
            }
        }



        public void CreateHashToEmptyItems()
        {


            var itemsWithDownEntityExistNoNeedToAccumulate = 
                _OriginalSIItemWithoutHash
                .Where(rec => rec.SupplierInvoiceItemsConDeclars.Count > 0 ||
            rec.SupplierInvoiceItemsDescripts.Count > 0 ||
            rec.SupplierInvoiceItemLevies.Count > 0 ||
            rec.SupplierInvoiceItemsMods.Count > 0 ||
            rec.SupplierInvoiceItemsProdIdents.Count > 0 ||
            rec.SupplierInvoiceItemsSerialNums.Count > 0 ||
            rec.SupplierInvoiceItemVehicles.Count > 0 ||
            (rec.SupplierInvoiceItemVehicles.Count > 0 &&
            rec.SupplierInvoiceItemVehicles.FirstOrDefault(rec1 => rec1.SupplierInvoiceItemVehicleAdds.Count > 0) != null)
            )
            .ToList()
            ;

            SetGUIDAsHash(itemsWithDownEntityExistNoNeedToAccumulate);
            var ItemsMarkNotForAccumaltion = _OriginalSIItemWithoutHash.Where(rec => rec.NotForAccumaltion).ToList(); //GetItemsMarkNotForAccumaltion(_OriginalSIItemWithoutHash);
            SetGUIDAsHash(ItemsMarkNotForAccumaltion);
            var ItemsWithPointer = GetItemsLinesWithPointer(_OriginalSIItemWithoutHash);
            SetGUIDAsHash(ItemsWithPointer);
            var allNotNeeded = itemsWithDownEntityExistNoNeedToAccumulate.Union(ItemsMarkNotForAccumaltion);
            allNotNeeded = allNotNeeded.Union(ItemsWithPointer);
            var toDoRealHash = _OriginalSIItemWithoutHash.Except(allNotNeeded).ToList();
            toDoRealHash.ForEach(
                item =>
                {

                    CalcHash(item);
                }
                );
        }

        private void CalcHash(SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            SupplierInvoiceItemDataMapping dataMapping = new SupplierInvoiceItemDataMapping();
            supplierInvoiceItemPM.ItemHash = dataMapping.CalcHash(supplierInvoiceItemPM);
            supplierInvoiceItemPM.CurrentContextTag = "ACCUMULATION";
            supplierInvoiceItemPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        }

        private List<SupplierInvoiceItemPM> GetItemsLinesWithPointer(List<SupplierInvoiceItemPM> _OriginalSIItemWithoutHash)
        {
            var allItemsWithPointer = new List<SupplierInvoiceItemPM>();
            string declarationId = _DeclarationPM.Id;
            
            ICustomContext dbContext = CustomContext.GetContext(_RequestParams.Tenant);
            var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(dbContext);
            List<CustomsDocumentPointerPM> customsDocumentPointerList = myCustomsDocumentPointerQueryService.GetParentDocumentPointer(declarationId, "Declaration", _RequestParams.Tenant);

            if (customsDocumentPointerList == null || customsDocumentPointerList.Count() < 1) return (allItemsWithPointer);

            foreach (var supplierInvoice in _DeclarationPM.SupplierInvoices)
            {
                List<int> itemLinesWithPointer = new List<int>();
                string invoiceCounter = supplierInvoice.InvoiceCounterKey.ToString();
                //List<int> Lines = _OriginalSIItemWithoutHash.Where(rec => rec.CounterKey == supplierInvoice.InvoiceCounterKey).Select(r => r.LineNumber).ToList();
                //string listLines = string.Join(",", Lines);

                itemLinesWithPointer = customsDocumentPointerList.Where(rec => rec.Child1EntityId == invoiceCounter && rec.Child2EntityId != null).Select(r => Convert.ToInt32(r.Child2EntityId)).ToList();
                /*
                foreach (var customsDocumentPointerPM in customsDocumentPointerList)
                {
                    if (customsDocumentPointerPM != null)
                    {
                        if (customsDocumentPointerPM.Child2EntityId != null) //customsDocumentPointerPM.Child2EntityCode == "SupplierInvoiceItem" && 
                        {
                            int child2EntityId = 0;
                            int.TryParse(customsDocumentPointerPM.Child2EntityId, out child2EntityId);
                            if (child2EntityId > 0) itemLinesWithPointer.Add(child2EntityId);
                        }
                    }
                }
                */
                if (itemLinesWithPointer != null && itemLinesWithPointer.Count() > 0)
                {
                    var suppItemsWithPointer = _OriginalSIItemWithoutHash.Where(r => r.CounterKey == supplierInvoice.InvoiceCounterKey && itemLinesWithPointer.Contains(r.LineNumber)).ToList();
                    allItemsWithPointer = allItemsWithPointer.Concat(suppItemsWithPointer).ToList();
                }
            }
            return (allItemsWithPointer);
        }

        private void GroupByHash()
        {
            var q =
                 (from siItm in this._OriginalSIItemsForAccumulation
                  group siItm by 
                  siItm.ItemHash
                  into g
                  select new
                  {
                      itemHash = g.Key,
                      //Sum this fields from all child lines


                      //InvoiceQuantity
                      SumItemPrice = g.Sum(a => a.ItemPrice),//ItemPrice
                      SumStatisticQuantity = g.Sum(a => a.StatisticQuantity),//StatisticQuantity
                      SumInvoiceQuantity = g.Sum(a => a.InvoiceQuantity),//InvoiceQuantity
                      //FirstOriginalItem2Copy = g.First()
                      OriginalItems = g
                  }
                  )
                  ;
            int counterKey = 0;
            ICustomContext dbContext = CustomContext.GetContext(_RequestParams.Tenant);
            var myGList = q.ToList();
            myGList.ForEach(g =>
            {
                var originalItems = g.OriginalItems.ToList();
                //g.itemHash
                var siItem = originalItems.First();
                //SupplierInvoiceItemKeys return DeclarationId + '_' + CounterKey + '_' + LineNumber;

                var isSIItemWithoutHash = _OriginalSIItemWithoutHash.Exists(rec =>
                rec.DeclarationId == siItem.DeclarationId &&
                rec.CounterKey == siItem.CounterKey &&
                rec.LineNumber == siItem.LineNumber);


                var currSupplierInvoice = _DeclarationPM.SupplierInvoices.First(si =>
                   si.DeclarationId == siItem.DeclarationId &&
                   si.InvoiceCounterKey == siItem.CounterKey
                );
                if (currSupplierInvoice.IsAccumalated != true) currSupplierInvoice.IsAccumalated = true;
                if (currSupplierInvoice.ChangeSetOp != ChangeSetOperation.Update) currSupplierInvoice.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                if (_DeclarationPM.ChangeSetOp != ChangeSetOperation.Update) _DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                

                //this.LogIt("CopyComposite 1st match SIItem As Parent (And All Tree)");
                if(counterKey != siItem.CounterKey)
                {
                    counterKey = siItem.CounterKey;
                    sequenceNumeric = 0;
                }
                SupplierInvoiceItemPM copySIItemTreeAsParent = CopyCompositeSIItemTreeAsParent(siItem);
                if (g.SumInvoiceQuantity.HasValue && g.SumInvoiceQuantity > 0) copySIItemTreeAsParent.InvoiceQuantity = g.SumInvoiceQuantity;
                if (g.SumStatisticQuantity.HasValue && g.SumStatisticQuantity > 0) copySIItemTreeAsParent.StatisticQuantity = g.SumStatisticQuantity;
                if(g.SumItemPrice.HasValue && g.SumItemPrice > 0) copySIItemTreeAsParent.ItemPrice = g.SumItemPrice;
                //copySIItemTreeAsParent.SequenceNumeric = GetSequenceNumeric();
                
                var mySupplierInvoiceItemUpdateService = new SupplierInvoiceItemUpdateService(dbContext);
                mySupplierInvoiceItemUpdateService.DoOnCreating(copySIItemTreeAsParent, currSupplierInvoice);
                currSupplierInvoice.SupplierInvoiceItems.Add(copySIItemTreeAsParent);
                
                //this.LogIt("/// from here parant line - is must !!!");
                
                ConnectOriginalItem2Parent(copySIItemTreeAsParent, originalItems);

            });

            //throw new NotImplementedException();
        }

        internal DeclarationPM GetDeclarationPM()
        {
            if (_DeclarationPM == null)
            {
                ICustomContext dbContext = CustomContext.GetContext(_RequestParams.Tenant);
                var declarationQueryService = new DeclarationQueryService(dbContext);
                //declarationQueryService.LoadSupplierInvoicesWithItems = true;
                _DeclarationPM = declarationQueryService.GetSingle(_RequestParams.AppicationId, true, false);
            }
            return (_DeclarationPM);
            //throw new NotImplementedException();
        }

        private void ConnectOriginalItem2Parent(SupplierInvoiceItemPM copySIItemTreeAsParent, List<SupplierInvoiceItemPM> originalItems)
        {
            //this.LogIt("ConnectOriginal Update ParentLineNumber of all Related Hash");


            originalItems.ForEach(siItem =>
            {
                siItem.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                siItem.CurrentContextTag = "ACCUMULATION";
                siItem.ParentLineNumber = copySIItemTreeAsParent.LineNumber;
            });
        }

        private void LogIt(string v)
        {
            //throw new NotImplementedException();
            LogMessagingUtil.Instance.AppendLine(v);
        }

        private int? GetSequenceNumeric()
        {
            return sequenceNumeric += 1;
        }

        private SupplierInvoiceItemPM CopyCompositeSIItemTreeAsParent(SupplierInvoiceItemPM siItem)
        {
            //Altrenetive 1 >Serilaze ->DeSerilaze
            //Altrenetive 2 >ShallowCopy
            //Stopwatch localStopwatch;
            //after change all keys + ChangeOp== Insert !!!!
            SupplierInvoiceItemPM siItemCopy;

            //localStopwatch = Stopwatch.StartNew();
            
            var xmlSupplierInvoiceItem = XmlGenericUtil<SupplierInvoiceItemPM>.SerializeObject(siItem);
            siItemCopy = XmlGenericUtil<SupplierInvoiceItemPM>.DeSerializeObject(xmlSupplierInvoiceItem);

            //LogMessagingUtil.Instance.AppendLine(Environment.NewLine + "Copy item by Serialization to create parent Took:" + localStopwatch.Elapsed.ToString()); localStopwatch.Restart();


            siItemCopy.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            if (siItemCopy.SupplierInvioceItemCertificats != null && siItemCopy.SupplierInvioceItemCertificats.Count > 0) siItemCopy.SupplierInvioceItemCertificats.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemsConDeclars != null && siItemCopy.SupplierInvoiceItemsConDeclars.Count > 0) siItemCopy.SupplierInvoiceItemsConDeclars.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemsDescripts != null && siItemCopy.SupplierInvoiceItemsDescripts.Count > 0) siItemCopy.SupplierInvoiceItemsDescripts.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemLevies != null && siItemCopy.SupplierInvoiceItemLevies.Count > 0) siItemCopy.SupplierInvoiceItemLevies.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemsMods != null && siItemCopy.SupplierInvoiceItemsMods.Count > 0) siItemCopy.SupplierInvoiceItemsMods.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemsProdIdents != null && siItemCopy.SupplierInvoiceItemsProdIdents.Count > 0) siItemCopy.SupplierInvoiceItemsProdIdents.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemsSerialNums != null && siItemCopy.SupplierInvoiceItemsSerialNums.Count > 0) siItemCopy.SupplierInvoiceItemsSerialNums.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemTaxes != null && siItemCopy.SupplierInvoiceItemTaxes.Count > 0) siItemCopy.SupplierInvoiceItemTaxes.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemProcesTypes != null && siItemCopy.SupplierInvoiceItemProcesTypes.Count > 0) siItemCopy.SupplierInvoiceItemProcesTypes.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            if (siItemCopy.SupplierInvoiceItemModVehicles != null && siItemCopy.SupplierInvoiceItemModVehicles.Count > 0) siItemCopy.SupplierInvoiceItemModVehicles.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
            
            if (siItemCopy.SupplierInvoiceItemVehicles != null && siItemCopy.SupplierInvoiceItemVehicles.Count > 0)
            {
                siItemCopy.SupplierInvoiceItemVehicles.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
                foreach (var itemVehicle in siItemCopy.SupplierInvoiceItemVehicles)
                {
                    if (itemVehicle.SupplierInvoiceItemVehicleAdds != null && itemVehicle.SupplierInvoiceItemVehicleAdds.Count > 0)
                    {
                        itemVehicle.SupplierInvoiceItemVehicleAdds.ForEach(siitem => siitem.ChangeSetOp = ChangeSetOperation.Insert);
                    }
                }
            }

            siItemCopy.IsParent = true;
            siItemCopy.CurrentContextTag = "ACCUMULATION";
            siItemCopy.LineNumber = 0;
            siItemCopy.SequenceNumeric = null;
            siItemCopy.ParentLineNumber = null;
            return siItemCopy;

            //throw new NotImplementedException();
        }

        public static SupplierInvoiceItemPM DeepCopy<SupplierInvoiceItemPM>(SupplierInvoiceItemPM other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (SupplierInvoiceItemPM)formatter.Deserialize(ms);
            }
        }

        internal void FastDeleteAllParent()
        {
            if (this._DeclarationPM == null)
            {
                DeclarationPM declarationPM = GetDeclarationPM();
                this._DeclarationPM = declarationPM;
            }
            ICustomContext dbContext = CustomContext.GetContext(_RequestParams.Tenant);
            DeclarationUpdateService DeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), _RequestParams.Tenant);
            this._DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            DeclarationUpdateService.DeclarationSupplierInvoiceItemsParentsFastDelete(_DeclarationPM);
            //var dbContext = CustomContext.GetContext(_RequestParams.Tenant);
            DeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), _RequestParams.Tenant);
            DeclarationUpdateService.Update(_DeclarationPM,true);
            this._DeclarationPM = null;
        }

        internal void SetParam(GenericRequestParams requestParams)
        {
            _RequestParams = requestParams;
        }

        internal bool Fast_IsAllItemsHaveHash_IsAccurate(bool onlyAlwaysAccumulate)
        {
            //var qs = new SupplierInvoiceItemQueryService(_RequestParams.Tenant);
            var siqs = new SupplierInvoiceQueryService(_RequestParams.Tenant);
            int existSupplierInvoiceItemsWithoutHash = 0;
            
            //existSupplierInvoiceItemsWithoutHash = qs.ExistSupplierInvoiceItemsWithoutHash(_RequestParams.Tenant, _RequestParams.AppicationId);
            existSupplierInvoiceItemsWithoutHash = siqs.ExistSupplierInvoiceItemsWithoutHashForAccumulation(_RequestParams.Tenant, _RequestParams.AppicationId, onlyAlwaysAccumulate);
            if (existSupplierInvoiceItemsWithoutHash > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            //throw new NotImplementedException();
        }

        private void SetHashCode(object p)
        {
            //throw new NotImplementedException();
        }

        private void SetGUIDAsHash(List<SupplierInvoiceItemPM> itemsWithDownEntityExistNoNeedToAccumulate)
        {
            itemsWithDownEntityExistNoNeedToAccumulate.ForEach(
                itm =>
                {
                    itm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    itm.ItemHash = Guid.NewGuid().ToString();
                    itm.CurrentContextTag = "ACCUMULATION";
                }
                );
            //throw new NotImplementedException();
        }

        private List<SupplierInvoiceItemPM> GetItemsMarkNotForAccumaltion(List<SupplierInvoiceItemPM> allItemWithoutHash)
        {
            throw new NotImplementedException();
        }

  
        public static ulong CreateHashCode(object obj)
        {
            ulong hash = 0;
            Type objType = obj.GetType();

            if (objType.IsValueType || obj is string)
            {
                unchecked
                {
                    hash = (uint)obj.GetHashCode() * 397;
                }

                return hash;
            }
            unchecked
            {
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    object value = property.GetValue(obj, null);
                    hash ^= CreateHashCode(value);
                }
            }

            return hash;
        }

        

        private List<SupplierInvoiceItemPM> GetAllItemWithoutHash()
        {
            return this._OriginalSIItemWithoutHash;
            //return declarationPM.SupplierInvoices.se
            //return null;
        }
    }
    class SIAccumulationExample
    {

        void Keys()
        {
            //this.MemberwiseClone()
            //SupplierInvoiceItemKeys return DeclarationId+'_'+CounterKey+'_'+LineNumber;
            //SupplierInvoiceItemsConDeclarKeys return DeclarationId+'_'+InvoiceCounterKey+'_'+InvoiceItemLineNumber+'_'+LineNumber;


        }
        public DeclarationPM Get()
        {
            var declarationId = "1-1";
            var SupplierInvoices = new List<SupplierInvoicePM>();

            var a_SupplierInvoicePM = new SupplierInvoicePM()
            {
                //IsAccumalted=true,
                DeclarationId = declarationId,
            };
            SupplierInvoices.Add(a_SupplierInvoicePM);
            var b_SupplierInvoicePM = new SupplierInvoicePM()
            {
                //IsAccumalted=true,
                DeclarationId = declarationId,
            };
            SupplierInvoices.Add(b_SupplierInvoicePM);
            for (int i = 0; i < 10; i++)
            {
                var SupplierInvoiceItemPM = new SupplierInvoiceItemPM()
                {
                    DeclarationId = declarationId,
                    AdditionalQuantity = i,
                    AdditionalQuantityType = "AdditionalQuantityType",
                    CatalogNumber = "CatalogNumber",
                    ClassificationCode = "ClassificationCode",
                    CounterKey = 1,
                    CustomsBookTypeCode = "CustomsBookTypeCode",
                    WholeSaleItemPriceCurrencyCode = "WholeSaleItemPriceCurrencyCode",


                };
            }
            var declarationPM = new DeclarationPM()
            {
                Id = declarationId,
                SupplierInvoices = new List<SupplierInvoicePM>()
                {


                }
                ,
            };
            return declarationPM;
        }
    }
}
/*
ItemHash		VC36			The field will be calculated by Accumulation service - WI 30926
ParentLineNumber		Int			Relation to parent item
NotForAccumaltion		Bit			Indication not to accumulate this row - maybe this field should contain the LineNumber value to make a unique Hash (check with Ihab)
IsParent		Bit			Indication whether this item is a parent item (Accumulated item)





    
Supplier Invoice Items - Accumulation

DB changes
SupplierInvoiceItems  - ItemHash , Accumulate (yes/no), ParentLineNumber, IsAccumulated (yes/no)
SupplierInvoice - IsAccumulated (yes/no)
Service for summing (above 998 lines) - The summing will be done per invoice
Create XML Object from this tables (fields under table should be part of the object) 
*SupplierInvoiceItems
SequenceNumeric
LineNumber
ItemCode
ItemDescription
ItemPrice
StatisticQuantity
InvoiceQuantity
ItemHash , Accumulate, ParentLineNumber, IsAccumulated
*SupplierInvioceItemCertificats
LineNumber
ItemCertificateCounterKey
SequenceNumeric
SupplierInvoiceItemsConDeclars
SupplierInvoiceItemsDescripts
SupplierInvoiceItemsLevies
SupplierInvoiceItemsMods
SupplierInvoiceItemsProdIdents
SupplierInvoiceItemsSerialNums
SupplierInvoiceItemVehicles
SupplierInvoiceItemVehicleAdds
Calculate Hash from each line object and save the hash in SupplierInvoiceItems.ItemHash
Group lines by ItemHash
For each group of the same ItemHash
Create a new SupplierInvoiceItem line + all of the related table (same data as the group) - this line will be the Accumulated line 
Sum this fields from all child lines
ItemPrice
StatisticQuantity
InvoiceQuantity
Calculate Sequence Numeric for parents (make sure there is no constraint)
For all child lines
Update field AccumulatedItemLine with the value of the LineNumber (relation to accumulated line)
For all new accumulated lines re-calculated field SequenceNumeric
Update field IsAccumalted.SupplierInvoices = True
In case 

    ****Delete ItemHash each time there is any update of SupplierInvoiceItems or childs (itzik)
 
Screens
Documents/Payment Protest  Pointers Screen 
Certificate multi entry
Interface (Send declaration)
CCUFILEM build
Supplier Invoice Item Data entry (switch between modes)


*/
