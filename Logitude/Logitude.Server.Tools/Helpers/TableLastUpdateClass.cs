using System;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using Logitude.Server.Tools;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using System.Diagnostics;
//using Logitude.Server.Tools.SignalRHubs;

namespace Logitude.BL.Helpers
{
    public class TableLastUpdateM
    {
        public string ObjectTableId { get; set; }
        public int AlternativeUserTenant { get; set; }
        public string AlternativeUserId { get; set; }

    }
    public static class TableLastUpdateClass
    {

        public static void UpdateTableHistory(int tenant, string tableName
            , TableLastUpdateM tableLastUpdateM = null)
        {
            
            User loggedUser = null;
            ObjectTable entityObjectTable = null;
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            ObjectTableLastUpdateRepository tableLastUpdateRepository = new ObjectTableLastUpdateRepository(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(context);
            //ContactRepository contactRepository = new ContactRepository(tenant);
            UserRepository userRepository = new UserRepository(tenant);




            entityObjectTable = objectTabelRepository.GetObjectTableByName(tableName, tenant, true);
            //Customs.CurrencyTypes
            tableName = tableName ?? "";
            if (entityObjectTable == null && tableName.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                //Customs.CurrencyType
                var tableWithoutS = tableName.Substring(0, Math.Max(0, tableName.Length - 1));
                entityObjectTable = objectTabelRepository.GetObjectTableByName(tableName, tenant, true);
            }


            loggedUser = loggedUser ?? GetUser(tenant, tableLastUpdateM);
            entityObjectTable = entityObjectTable ?? GetObjectTable(tenant, tableName, tableLastUpdateM, context);


            if (loggedUser == null)
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                    {
                        loggedUser = userRepository.GetSingleUserByEmail(HttpContext.Current.User.Identity.Name, tenant, true);

                    }
                }
            }
            if (entityObjectTable != null && !entityObjectTable.CacheOnClient)
            {
                return;
            }

            if (entityObjectTable != null && loggedUser != null)
            {
                ObjectTableLastUpdateUpsert(tenant, loggedUser, entityObjectTable, tableLastUpdateRepository);
            }
            //Mohammad: not every table has an objectfield like features or queries or screens it crashes here when update features in tenant 0 .
            //else
            //{
            //    var mess=String.Format("UpdateTableHistory(tenant={0},tableName={1}) : Bad Params", tenant, tableName);
            //    LogMessagingUtil.Instance.AppendLine(mess);
                
            //    //If it were up to me I would crash it like this 
            //    //throw new Exception(mess);
            //    //But I do not know what it means to Ramallah
            //    if (Debugger.IsAttached) Debugger.Break();
            //    throw new Exception("ObjectTableLastUpdateUpsert is must :" + mess);//ihab confirm : ObjectTableLastUpdateUpsert is must
            //}

        }

        private static void ObjectTableLastUpdateUpsert(int tenant, User loggedContact, ObjectTable entityObjectTable, ObjectTableLastUpdateRepository tableLastUpdateRepository)
        {
            ObjectTableLastUpdate tableLastUpdate = tableLastUpdateRepository.GetSingleObjectTableLastUpdate(entityObjectTable.Id, tenant);
            if (tableLastUpdate != null)
            {
                tableLastUpdate.LastUpdateDate = DateTime.UtcNow;
                tableLastUpdate.UpdatedByUserId = loggedContact.Id;

                tableLastUpdateRepository.Update(tableLastUpdate);
            }
            else
            {

                tableLastUpdate = new ObjectTableLastUpdate()
                {
                    Id = IdCounter.GetNumber("ObjectTableLastUpdate", tenant),
                    Tenant = tenant,
                    LastUpdateDate = DateTime.UtcNow,
                    ObjectTableId = entityObjectTable.Id,
                    UpdatedByUserId = loggedContact.Id,

                };

                tableLastUpdateRepository.Add(tableLastUpdate);
            }

            tableLastUpdateRepository.SubmitChanges();
            //SignalRHubMessageSender.SendTenantChannelMessage("CachedTableUpdate", entityObjectTable.Name, tenant);
            //HubEventPublisher.PublishChannelEvent(new HubChannelEvent() { ChannelName = "Tenant" + tenant, EventName = "CachedTableUpdate", Data = entityObjectTable.Name });
        }
    

        private static ObjectTable GetObjectTable(int tenant, string tableName, TableLastUpdateM tableLastUpdateM, IWebFreightContext context)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(context);
            ObjectTable entityObjectTable = null;

            if (tableLastUpdateM != null && !string.IsNullOrWhiteSpace(tableLastUpdateM.ObjectTableId))
            {
                entityObjectTable = objectTabelRepository.GetObjectTableById(tableLastUpdateM.ObjectTableId, tenant);

            }
            if (entityObjectTable == null)
            {
                entityObjectTable = objectTabelRepository.GetObjectTableByName(tableName, tenant, true);
            }
            return entityObjectTable;
        }

        private static User GetUser(int tenant, TableLastUpdateM tableLastUpdateM)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            UserRepository userRepository = new UserRepository(tenant);
            User loggedUser = null;

            if (tableLastUpdateM != null && !String.IsNullOrWhiteSpace(tableLastUpdateM.AlternativeUserId))
            {
                var email = contactRepository.GetEmailContactByIdAndTenant(tableLastUpdateM.AlternativeUserTenant, tableLastUpdateM.AlternativeUserId);
                loggedUser = userRepository.GetSingleUserByEmail(email, tableLastUpdateM.AlternativeUserTenant);
            }
            if (loggedUser == null)
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null &&
                    !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    loggedUser = userRepository.GetSingleUserByEmail(HttpContext.Current.User.Identity.Name, tenant);
                }
                else
                {
                    loggedUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant);
                }

            }




            return loggedUser;
        }

        private static Contact GetContact(int tenant, TableLastUpdateM tableLastUpdateM)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggedContact = null;

            if (tableLastUpdateM != null && !String.IsNullOrWhiteSpace(tableLastUpdateM.AlternativeUserId))
            {
                var email = contactRepository.GetEmailContactByIdAndTenant(tableLastUpdateM.AlternativeUserTenant, tableLastUpdateM.AlternativeUserId);
                loggedContact = contactRepository.GetSingleContactByEmail(email, tableLastUpdateM.AlternativeUserTenant);


                var maybeCostomerCare = true;
                if (maybeCostomerCare && loggedContact == null)// mayby customer care 
                {
                    email = contactRepository.GetEmailContactByIdAndTenant(0, tableLastUpdateM.AlternativeUserId);
                    loggedContact = contactRepository.GetSingleContactByEmail(email, 0);
                }
            }

            if (loggedContact == null)
            {
                loggedContact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), tenant);//In Worker Role Can Crash !!!
            }
            return loggedContact;
        }
        public static void UpdateTableHistory(int tenant, string tableName, IWebFreightContext context)
        {
            ObjectTableLastUpdateRepository tableLastUpdateRepository = new ObjectTableLastUpdateRepository(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(context);
            ContactRepository contactRepository = new ContactRepository(tenant);

            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    User loggedContact = GetUser(tenant, null);//contactRepository.GetSingleContactByEmail(HttpContext.Current.User.Identity.Name, tenant);
                    ObjectTable entityObjectTable = objectTabelRepository.GetObjectTableByName(tableName, tenant, true);
                    if (entityObjectTable != null && loggedContact != null && entityObjectTable.CacheOnClient)
                    {
                        ObjectTableLastUpdate tableLastUpdate = tableLastUpdateRepository.GetSingleObjectTableLastUpdate(entityObjectTable.Id, tenant);
                        if (tableLastUpdate != null)
                        {
                            tableLastUpdate.LastUpdateDate = DateTime.UtcNow;
                            tableLastUpdate.UpdatedByUserId = loggedContact.Id;

                            tableLastUpdateRepository.Update(tableLastUpdate);
                        }
                        else
                        {

                            tableLastUpdate = new ObjectTableLastUpdate()
                            {
                                Id = IdCounter.GetNumber("ObjectTableLastUpdate", tenant),
                                Tenant = tenant,
                                LastUpdateDate = DateTime.UtcNow,
                                ObjectTableId = entityObjectTable.Id,
                                UpdatedByUserId = loggedContact.Id,

                            };

                            tableLastUpdateRepository.Add(tableLastUpdate);
                        }

                        tableLastUpdateRepository.SubmitChanges();

                    }
                }
            }
        }
        public static void UpdateCacheTableHistory()
        {
            string sqlDDL_NoNeedCommit = "delete objecttablelastupdates where objecttableid   in ( select id From  objecttables where id in (select objecttableid from objecttablelastupdates ) and name not like 'Custom%') ";
            ((CustomContext.GetContext(0)) as DbContextBase).ExecuteReaderSingleResult<int>(sqlDDL_NoNeedCommit,
(dr) =>
{

   NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"ExecuteReaderSingleResult: {dr.GetString(0)}");
return 0;
});


            

            var objectTabelRepository = new ObjectTableRepository(0);
            var list = objectTabelRepository.GetAllCacheOnClient(0);
            if (LogitudeSettings.IsCostomsDeploy)
            {
                list = list.Where(r => (r.Name ?? "").StartsWith("Customs.")).ToList();
            }
            foreach (var item in list)
            {
                TableLastUpdateClass.UpdateTableHistory(0, item.Name, new TableLastUpdateM()
                {
                    ObjectTableId = item.Id,
                    AlternativeUserTenant = 0,
                    AlternativeUserId = "1-1" ///in oracle  //"admin@fnarsoft.com"=1-1

                });
            }
        }
        public static void UpdateSystemMetaDataHistory(bool updateFields = true, bool updateTranslations = true)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SystemMetadataLastUpdateRepository systemMetdaDataRep = new SystemMetadataLastUpdateRepository();
                SystemMetadataLastUpdate lastUpdates = systemMetdaDataRep.GetSingleSystemMetadataLastUpdate("1");
                if (lastUpdates != null)
                {
                    if (updateFields)
                        lastUpdates.ObjectFieldsUpdateDateGMT = DateTime.UtcNow;
                    if (updateTranslations)
                        lastUpdates.TranslationsUpdateDateGMT = DateTime.UtcNow;

                    systemMetdaDataRep.Update(lastUpdates);
                }
                else
                {
                    lastUpdates = new SystemMetadataLastUpdate()
                    {
                        Id = "1",
                        ObjectFieldsUpdateDateGMT = DateTime.UtcNow,
                        TranslationsUpdateDateGMT = DateTime.UtcNow,
                    };

                    systemMetdaDataRep.Add(lastUpdates);
                }

                systemMetdaDataRep.SubmitChanges();

                scope.Complete();
            }
        }


        public static void UpdateAllClosedTablesHistory()
        {
            IWebFreightContext context = WebFreightContext.GetContext(0);
            ObjectTableLastUpdateRepository tableLastUpdateRepository = new ObjectTableLastUpdateRepository(context);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(context);
            ContactRepository contactRepository = new ContactRepository(0);

            List<ObjectTable> objectTablesList = objectTabelRepository.GetObjectsByTenant(0).Where(t=>t.IsClosed).ToList();
            Contact loggedContact = contactRepository.GetSingleContactByEmail("system@tenant0.com", 0);

            foreach (ObjectTable table in objectTablesList)
            {

                ObjectTableLastUpdate tableLastUpdate = tableLastUpdateRepository.GetSingleObjectTableLastUpdate(table.Id, 0);
                if (tableLastUpdate != null)
                {
                    tableLastUpdate.LastUpdateDate = DateTime.UtcNow;
                    tableLastUpdate.UpdatedByUserId = loggedContact.Id;

                    tableLastUpdateRepository.Update(tableLastUpdate);
                }
                else
                {

                    tableLastUpdate = new ObjectTableLastUpdate()
                    {
                        Id = IdCounter.GetNumber("ObjectTableLastUpdate", 0),
                        Tenant = 0,
                        LastUpdateDate = DateTime.UtcNow,
                        ObjectTableId = table.Id,
                        UpdatedByUserId = loggedContact.Id,

                    };

                    tableLastUpdateRepository.Add(tableLastUpdate);
                }

               

            }

            tableLastUpdateRepository.SubmitChanges();


        }
    }
}
