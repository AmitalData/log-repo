using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.LT2UT
{
    public partial class ItemsTableService
    {
        private List<GITITEMPM> _ListGITITEMPM;
        private int _Tenant;

        public ItemsTableService(List<GITITEMPM> listGITITEMPM, int tenant)
        {
            _Tenant = tenant;
            _ListGITITEMPM = listGITITEMPM;
        }

        public void OpenUnifreighTask(string xmlReq)
        {
            CustomsSettingQueryService settingService = new CustomsSettingQueryService(_Tenant);
            bool isConnectedToUniFreight  = settingService.GetSettingByTenantN(_Tenant).IsConnectedToUniFreight;

          
          
                var sw = Stopwatch.StartNew();

                TransactionScope scope = null;
                if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
                {
                    scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
                }
            try
            {
               
                var requestData = "";

                //var unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(_Tenant);
                string unifreightUser = null;
                if (RequestSheetContext.Current != null)
                {
                    var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                    if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                    {
                        UserRepository userRep = new UserRepository(_Tenant);
                        User user = userRep.GetSingleUser(loggingUserIdFromRS, _Tenant, true);
                        if (user != null)
                        {
                            if (!String.IsNullOrWhiteSpace(user.Code))
                            {
                                unifreightUser = user.Code;
                            }
                        }
                    }
                }
                if (String.IsNullOrWhiteSpace(unifreightUser))
                {
                    unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(_Tenant);
                }

                var myUpdateList = new List<UpdateItemsTable>();

                _ListGITITEMPM.ForEach(pm =>
                {
                    var myUpdateItemsTable = new UpdateItemsTable();
                    var myItemData = new ItemData();
                    myItemData.itemNo = pm.ITEMNO;
                    myItemData.customerId = pm.PARTNERID;
                    myItemData.supplierId = pm.SAPAKID;
                    myItemData.pratCode = pm.PRATID;
                    myItemData.itemName = pm.NAMEENG;
                    myItemData.originCountryCode = pm.ORIGINCOUNTRY;
                    myItemData.unitId = pm.UNITID;
                    myUpdateItemsTable.ItemsDataList = new List<ItemData>() { myItemData };
                });

                //myUpdateList.ItemsDataList =  new List<ItemData> { myItemData };
                var xml = XmlGenericUtil<List<UpdateItemsTable>>.SerializeObject(myUpdateList, true);
                requestData = xml;
                var doc = new XDocument(
   new XElement("UpdateItemsTable",
   from c in _ListGITITEMPM
   select new XElement("ItemsDataList",
   new XElement("ItemData",
            new XElement("customerId", c.PARTNERID),
            new XElement("supplierId", c.SAPAKID),
        new XElement("itemNo", c.ITEMNO),
        new XElement("itemName", c.NAMEENG),
        new XElement("pratCode", c.PRATID),
        new XElement("originCountryCode", c.ORIGINCOUNTRY),
        new XElement("unitId", c.UNITID)
   )
   )
));
                requestData = doc.ToString(SaveOptions.None);

                var myYCULTASKPM = new YCULTASKPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    STATUS = "W",
                    REQUESTDATA = requestData,
                    ENTNAME = "GITITEM",
                    PRIMARYNUM = _ListGITITEMPM.First().COUNTER.ToString(),//EITAN SEE ITS ZERO
                    PRIORITY = YCULTASKPM.calcPriority("LT2UT"),
                    TYPE = "LT2UT",
                    USRCODE = unifreightUser,
                    ARCHIVE = "F",
                    //LOGTIME = (new DualQueryService(MainContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now,
                };
                //myYCULTASKPM.TASKID = CommCounterUtil.GetUnique30(myYCULTASKPM.LOGTIME);
                if (!isConnectedToUniFreight)
                {
                    myYCULTASKPM.Tenant = _Tenant;
                }
                AmitalContext MyContext = AmitalContext.GetContext(_Tenant);

                var myGGGQUpdateService = new GGGQUpdateService(MyContext);
                myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                var myYCULTASKUpdateService = new YCULTASKUpdateService(MyContext);
                myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                var myGGGQPM = new GGGQPM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    ORIGINQUE = "LGT", //LugitudeRequest
                    STATUS = "1",
                    EXPTASKTIME = 5,
                    EXECDATE = DateTime.Now,
                    TRY = 9,
                    PRIORITY = 8,
                    ENTNAME = "GITITEM",
                    PRIMARYNUM = _ListGITITEMPM.First().COUNTER.ToString(),//EITAN SEE ITS ZERO
                    FORMID = "LGT_UPDATE_FCI",
                    DEBUG = "F",
                    DONEOPERATION = "D",
                    GSTRING1 = "NO_LOCK",
                    //GSTRING1 = myYCULTASKPM.TASKID,
                };
                if(!isConnectedToUniFreight)
                {
                    myGGGQPM.Tenant = _Tenant;
                }
                myGGGQUpdateService.Update(myGGGQPM, true);
                
                if (scope != null)
                {
                    scope.Complete();
                }
            }
            finally
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
            }
                LogMessagingUtil.Instance.AppendLine("OpenUnifreighTask:Took:" + sw.ElapsedMilliseconds);
            }
      


    }
}
