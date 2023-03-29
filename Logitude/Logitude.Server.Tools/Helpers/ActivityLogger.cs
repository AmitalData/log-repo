using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.SystemLogs.POCOs;

using Logitude.SystemLogs.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;


using Simplog.Data.CommonDataModel.EntityPOCOs;

using Logitude.Server.Tools.Counters;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.SqlClient;
using System.Data;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Data.Common;

namespace Logitude.Server.Tools.Helpers
{
    public class ActivityLogger
    {
        private static string GetConnection(int tenant)
        {
            GlobalDBRepository globalDbRep;
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                //GlobalDBRep = new GlobalDBRepository();
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);

            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }
        public static void AddAcitivityLog(string entityId, string objectTableId, int tenant, string activityTypeCode, string userId)
        {
            try
            {
                string strConnString = GetConnection(tenant);
                string query = "INSERT INTO EntityLastActivities (Id, ActivityDate, ActivityTypeCode, EntityId, ObjectTableId, Tenant,UserId) " +
                 "VALUES (@Id, @ActivityDate, @ActivityTypeCode, @EntityId, @ObjectTableId, @Tenant, @UserId) ";

                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50).Value = IdCounter.GetNumber("EntityLastActivity", tenant);
                    cmd.Parameters.Add("@ActivityDate", SqlDbType.DateTime).Value = TenantServerConfigration.GetCurrentDateTime(tenant);
                    cmd.Parameters.Add("@ActivityTypeCode", SqlDbType.VarChar, 50).Value = activityTypeCode;
                    cmd.Parameters.Add("@EntityId", SqlDbType.VarChar, 50).Value = entityId.ToString();
                    cmd.Parameters.Add("@ObjectTableId", SqlDbType.VarChar, 50).Value = objectTableId;
                    cmd.Parameters.Add("@Tenant", SqlDbType.Int).Value = tenant;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 50).Value = userId;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 5;
                    cn.Open();
                    var output = cmd.ExecuteNonQuery();
                    cn.Close();
                }
                //EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
                //EntityLastActivity activity = new EntityLastActivity()
                //{
                //    ActivityDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                //    Id = IdCounter.GetNumber("EntityLastActivity", tenant),
                //    ActivityTypeCode = activityTypeCode,
                //    EntityId = entityId,
                //    ObjectTableId = objectTableId,
                //    Tenant = tenant,
                //    UserId = userId,
                //};
                //entityLastActivityRepository.Add(activity);
                //entityLastActivityRepository.SubmitChanges();
            }
            catch { }
        }

        public static void SendTotangoContactActivity(string email, string module, string activity, int tenant, bool isSharedLogisticsContact, string cardId)
        {

            try
            {
                ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
                UserRepository userRepository = new UserRepository(commonDataContext);
                ContactRepository contactrep = new ContactRepository(commonDataContext);
                Contact loggedContact = contactrep.GetSingleContactByEmail(email, tenant);
                User loggedUser = userRepository.GetSingleUserByEmail(email, loggedContact.Tenant, true);

                //TenantRepository rep = new TenantRepository(commonDataContext);
                Tenant currentTenant = TenantRepository.GetSingleTenant(tenant, true);

                string CountryName = currentTenant.Address != null ? (currentTenant.Address.Country != null ? currentTenant.Address.Country.EnglishName : null) : null;

                string orgDisplayName = currentTenant.Company + (CountryName != null ? ("-" + CountryName.Trim()) : "");
                string organizationId = tenant.ToString();

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    SettingRepository mySettingRepository = new SettingRepository();
                    var isDemoTenant = mySettingRepository.IsDemoTenant(tenant.ToString());

                    if (tenant == 65 || tenant == 153)
                    {
                        orgDisplayName = loggedUser.Notes;
                        organizationId = loggedUser.Id;
                    }

                    scope.Complete();
                }

                if (isSharedLogisticsContact)
                {
                    Card card = commonDataContext.Cards.Where(d => d.Id == cardId & d.Tenant == tenant).FirstOrDefault();
                    TotangoActivityLogger.SendUserActivity(organizationId, orgDisplayName, "External Contact", module, activity, email, tenant, true, cardId, card.PartnerTypeId);
                }
                else
                {
                    TotangoActivityLogger.SendUserActivity(organizationId, orgDisplayName, loggedContact.EnglishName, module, activity, email, tenant, false, null, null);
                }
            }
            catch { }
        }
    }

    public interface IActivityLogger
    {
        void AddAcitivityLog(string entityId, string objectTableId, int tenant, string activityTypeCode, string userId);
    }
    public class ActivityLoggerWrapper :IActivityLogger
    {

        public void AddAcitivityLog(string entityId, string objectTableId, int tenant, string activityTypeCode, string userId)
        {
            
            ActivityLogger.AddAcitivityLog(entityId, objectTableId, tenant, activityTypeCode, userId);
        }
    }
}
