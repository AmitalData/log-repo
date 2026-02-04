using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityKeys;
using System.Configuration;
using System.Globalization;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.EntityQueryServices;
using System.Data.Entity.Validation;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;
using Devart.Data.Oracle;
using System.Data.SqlClient;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using System.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ConsignmentUpdateService : EntityUpdateService<Consignment, ConsignmentPM, DeclarationPM>
    {

        int? maxCounter;
        protected override void OnCreating(ConsignmentPM entityPM, DeclarationPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.Id;

            if (!this.maxCounter.HasValue)
            {
                ICustomContext _Context = MainContext as CustomContext;
                var consignmentQueryService = new ConsignmentQueryService(_Context);

                this.maxCounter = consignmentQueryService.GetMaxCounterKey(entityPM.DeclarationId, entityPM.Tenant) ?? 0;
            }
            entityPM.Tenant = entityParentPM.Tenant;
            maxCounter = entityPM.ConsignmentNumber = maxCounter.Value + 1;

        }


        protected override void OnUpdating(ConsignmentPM entityPM, Consignment entityPOCO)
        {

            if (EntityParentPM?.Direction == "I")
            {

                entityPM.ManifestNumber = entityPM.ManifestNumber?.Trim();
                entityPM.SecondCargoID = entityPM.SecondCargoID?.Trim();
                entityPM.ThirdCargoID = entityPM.ThirdCargoID?.Trim();
            }

            if (String.IsNullOrWhiteSpace(entityPM.UnloadPortCode))
            {
                if (!string.IsNullOrWhiteSpace(EntityPOCO.UnloadPortCode))
                {
                    LogHowClearUnloadPort(entityPM, entityPOCO);
                }

            }

            if ((string.IsNullOrWhiteSpace(entityPM.OriginCountryCode) && entityPM.OriginCountryCode != entityPOCO.OriginCountryCode) || (string.IsNullOrWhiteSpace(entityPM.CargoDescription) && entityPM.CargoDescription != entityPOCO.CargoDescription))
            {
                DateTime stopLogAt = DateTime.MinValue;
                string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20220424HD390614.LogUntilDateyyyyMMdd"];
                if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                {
                    stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None);
                }

                string logData = "";
                logData = $"entityPOCO.OriginCountryCode={entityPOCO.OriginCountryCode},entityPOCO.CargoDescription={entityPOCO.CargoDescription}";
                LogitudeSettings.HandleLogMe("Origin Country || Cargo Description deleted " + logData, false, "DeletedData", stopLogAt);
            }

            UpdatePendingByKeyWords(entityPM, entityPOCO: entityPOCO);
            //BUG String Empty 175687
            entityPM.CargoTypeCode = !string.IsNullOrEmpty(entityPM.CargoTypeCode) ? entityPM.CargoTypeCode : null;
            entityPM.ManifestNumber = !string.IsNullOrEmpty(entityPM.ManifestNumber) ? entityPM.ManifestNumber : null;
            entityPM.SecondCargoID = !string.IsNullOrEmpty(entityPM.SecondCargoID) ? entityPM.SecondCargoID : null;
            entityPM.ThirdCargoID = !string.IsNullOrEmpty(entityPM.ThirdCargoID) ? entityPM.ThirdCargoID : null;

            base.OnUpdating(entityPM, entityPOCO);
        }


        public void UpdatePendingByKeyWords(ConsignmentPM entityPM, Boolean IsAfterDeclarationCourierStatusInsert = false, Consignment entityPOCO = null)
        {
            LogMessagingUtil.Instance.AppendLine("UpdatePendingByKeyWords");
            try
            {
                if (!String.IsNullOrWhiteSpace(entityPM.CargoDescription))
                {
                    string dbCargoDescription = entityPOCO != null
                            ? entityPOCO.CargoDescription
                            : GetDBEntity(entityPM)?.CargoDescription;
                    if (IsAfterDeclarationCourierStatusInsert) dbCargoDescription = null;
                    if (entityPM.CargoDescription != dbCargoDescription)
                    {
                        List<string> pendingReasonCodeList = new List<string>();
                        ICustomContext context = MainContext as CustomContext;
                        DeclarationCourierStatusQueryService myDeclarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                        DeclarationCourierStatusPM declarationCourierStatusPM = myDeclarationCourierStatusQueryService.GetSingle(entityPM.DeclarationId, true, false);
                        if (declarationCourierStatusPM != null)
                        {
                            var pendingByKeywordQueryService = new PendingByKeywordQueryService(entityPM.Tenant);
                            var courierReasonCodeList = pendingByKeywordQueryService.GetCourierPendingReasonCodeBykeyWords(
                                entityPM.CargoDescription,
                                 "1"  /*תאור טובין*/,
                                entityPM.Tenant);
                            CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(entityPM.Tenant);
                            foreach (var courierReasonCode in courierReasonCodeList)
                            {
                                Boolean isActive = courierPendingReasonRepositoryRepository.IsActive(courierReasonCode, entityPM.Tenant);
                                if (!isActive) continue;
                                if (!String.IsNullOrWhiteSpace(courierReasonCode) && !pendingReasonCodeList.Contains(courierReasonCode))
                                {
                                    pendingReasonCodeList.Add(courierReasonCode);
                                    DeclarationPendingPM declarationPendingPM = new DeclarationPendingPM();
                                    declarationPendingPM = declarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == entityPM.DeclarationId && r.CourierPendingReasonCode == courierReasonCode).FirstOrDefault();
                                    if (declarationPendingPM != null)
                                    {
                                        if (declarationPendingPM.Status != "A")
                                        {
                                            declarationPendingPM.ChangeSetOp = ChangeSetOperation.Update;
                                            declarationPendingPM.Status = "A";
                                            if (declarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                        }
                                    }
                                    else
                                    {
                                        declarationPendingPM = new DeclarationPendingPM();
                                        declarationPendingPM.ChangeSetOp = ChangeSetOperation.Insert;
                                        declarationPendingPM.Status = "A";
                                        declarationPendingPM.DeclarationID = entityPM.DeclarationId;
                                        declarationPendingPM.Tenant = entityPM.Tenant;
                                        declarationPendingPM.CourierPendingReasonCode = courierReasonCode;
                                        declarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM);
                                        if (declarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                    }
                                }
                            }
                            if (declarationCourierStatusPM != null && declarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
                            {
                                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                                declarationCourierStatusUpdateService.Update(declarationCourierStatusPM, true);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogMessagingUtil.Instance.AppendLine("UpdatePendingByKeyWords:" + ex.Message);
                throw;
            }
        }


        private Consignment GetDBEntity(ConsignmentPM entityPM)
        {
            var consignmentQueryService = new ConsignmentQueryService(entityPM.Tenant);
            var myDBEntity = consignmentQueryService.GetSinglePoco(entityPM.DeclarationId, entityPM.ConsignmentNumber, true, false);
            return myDBEntity;
        }

        private void LogHowClearUnloadPort(ConsignmentPM entityPM, Consignment entityPOCO)
        {
            try
            {
                string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20180208.LogHowClearUnloadPortUntilDateyyyyMMdd"];
                if (string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                {
                    return;
                }

                DateTime stopLogAt = DateTime.MinValue; //new DateTime(2018, 02, 20);
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None);

                if (DateTime.Now > stopLogAt)
                {
                    return;
                }



                string jsonPM = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPM);
                string jsonPOCO = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(EntityPOCO);

                var usr = AuthenticationUtil.ResolveUserIdentityName(entityPOCO.Tenant);
                var sb = new StringBuilder();
                sb
                    .AppendLine("ResolveUserIdentityName:" + usr)
                    .AppendLine("**Stack:")
                    .AppendLine(Environment.StackTrace)
                    .AppendLine("**PM:New:")
                    .AppendLine(jsonPM)
                    .AppendLine("**POCO:old:")
                    .AppendLine(jsonPOCO);

                LogitudeSettings.HandleLogMe
                    //(mess, err, suffix, stopLogAt)
                    (sb.ToString(), false, "HowClearUnloadPort", stopLogAt);

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void UpdateComposition(ConsignmentPM entityPM)
        {
            ConsignmentPackageUpdateService consignmentPackageUpdateService = new ConsignmentPackageUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            consignmentPackageUpdateService.UpdateMulti(entityPM.ConsignmentPackages, entityPM.DeletedConsignmentPackages, entityPM, false);






            ConsignmentInternalTransitionUpdateService consignmentInternalTransitionUpdateService = new ConsignmentInternalTransitionUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            consignmentInternalTransitionUpdateService.UpdateMulti(entityPM.ConsignmentInternalTransitions, entityPM.DeletedConsignmentInternalTransitions, entityPM, false);
            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(ConsignmentPM entityPM, DeclarationPM entityParentPM)
        {

            //if (entityPM.ChangeSetOp == ChangeSetOperation.Insert || entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            //{
            //    SubmitChanges();
            //    ICustomContext context = MainContext as CustomContext;
            //    ConsignmentRepository consignmentRepository = new ConsignmentRepository(context);
            //    List<Consignment> consignments = consignmentRepository.GetMulti(new DeclarationKeys() { Id = entityPM.DeclarationId });

            //    int index = 0;
            //    foreach (Consignment item in consignments)
            //    {
            //        index += 1;
            //        item.SequenceNumeric = index;
            //        consignmentRepository.Update(item);
            //        if (item.DeclarationId == entityPM.DeclarationId && item.ConsignmentNumber == entityPM.ConsignmentNumber)
            //        {
            //            entityPM.SequenceNumeric = item.SequenceNumeric;
            //        }
            //    }
            //    consignmentRepository.SubmitChanges();
            //}



        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.ConsignmentRepository).FastDeleteMulti(entityKeyFields);
        }

        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);
            return context.Database.Connection.ConnectionString;
        }

        public void FastUpdateUnloadPortCode(List<string> declarationIds, int tenant, string unloadPortCode)
        {
            ICustomContext context = MainContext as CustomContext;
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(Tenant);
            string whereIn = "";
            string declarations = "";
            if (declarationIds.Count() > 0)
            {
                declarations = string.Join("','", declarationIds);
                declarations = "'" + declarations + "'";
            }
            if (!string.IsNullOrEmpty(unloadPortCode)) unloadPortCode = "'" + unloadPortCode + "'";
            int i = 0;
            if (!string.IsNullOrEmpty(declarations) && declarations.Split(',').Count() > 990)
            {
                foreach (var item in declarations.Split(','))
                {
                    if (i < 990)
                    {
                        whereIn += item + ',';
                        i++;
                    }
                    else
                    {
                        whereIn = whereIn.TrimEnd(',');
                        whereIn += ") OR  ID IN (" + item + ',';
                        i = 0;
                    }
                }
                whereIn = whereIn.TrimEnd(',');
            }
            else
            {
                whereIn = declarations;
            }

            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    if (Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("SQL_P", tenant))
                    {
                        string cmd = "Update Consignments set UnloadPortCode =:pu ";
                        cmd = cmd + " where UnloadPortCode is null and declarationid IN (";
                        //cmd = cmd + " where UnloadPortCode is null and declarationid IN (:p2)";

                        OracleCommand oracleCommand = new OracleCommand(cmd, con);
                        oracleCommand.Parameters.Add(new OracleParameter("pu", unloadPortCode));
                        //oracleCommand.Parameters.Add(new OracleParameter("p2", whereIn));

                        string formattedParams = whereIn.Replace(" ", string.Empty); // Or a custom format
                        string[] splitParams = formattedParams.Split(',');

                        ////List<OracleParamter> parameters = new List<OracleParameter>();

                        ////string sql = @"SELECT * FROM FooTable WHERE FooValue IN (";
                        for (int n = 0; n < splitParams.Length; n++)
                        {
                            cmd += ":p" + n + ",";
                            //oracleCommand.Parameters.Add(new OracleParameter(":p" + n, OracleDbType.VarChar, splitParams[n], ParameterDirection.Input));
                            oracleCommand.Parameters.Add(new OracleParameter(":p" + n, splitParams[n]));
                        }
                        cmd = cmd.Substring(0, (cmd.Length - 1));
                        cmd += ')';

                        con.Open();
                        oracleCommand.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        string cmd = "Update Consignments set UnloadPortCode = " + unloadPortCode;
                        cmd = cmd + " where UnloadPortCode is null and declarationid IN (" + whereIn + ")";

                        OracleCommand oracleCommand = new OracleCommand(cmd, con);

                        con.Open();
                        oracleCommand.ExecuteNonQuery();
                        con.Close();
                    }

                }
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {

                    if (Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("SQL_P", tenant))
                    {
                        string cmd = "Update customs.Consignments set UnloadPortCode =:pu ";
                        cmd = cmd + " where UnloadPortCode is null and declarationid IN (";

                        SqlCommand sqlCommand = new SqlCommand(cmd, cn);
                        sqlCommand.Parameters.Add(new SqlParameter("pu", unloadPortCode));

                        string formattedParams = whereIn.Replace(" ", string.Empty); // Or a custom format
                        string[] splitParams = formattedParams.Split(',');

                        for (int n = 0; n < splitParams.Length; n++)
                        {
                            cmd += ":p" + n + ",";
                            sqlCommand.Parameters.Add(new SqlParameter(":p" + n, splitParams[n]));
                        }
                        cmd = cmd.Substring(0, (cmd.Length - 1));
                        cmd += ')';

                        cn.Open();
                        sqlCommand.ExecuteNonQuery();
                        cn.Close();
                    }
                    else
                    {
                        string cmd = "Update customs.Consignments set UnloadPortCode = " + unloadPortCode;
                        cmd = cmd + " where UnloadPortCode is null and declarationid IN (" + whereIn + ")";

                        SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                        cn.Open();
                        sqlCommand.ExecuteNonQuery();
                        cn.Close();
                    }
                }
            }
        }
    }
}
