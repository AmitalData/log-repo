using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Unifreight.Data.AmitalModel;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityPMs;
using Logitude.Customs.BL.BL;
using Devart.Data.Oracle;
using Logitude.Customs.BL.Validators;
using System.Data.SqlClient;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;

namespace Logitude.Customs.BL.EntityUpdateServices
{
   public partial class CourierMasterUpdateService
    {
        private Boolean toSendTask = false;
        private Boolean toSetDeclarationChanged = false;
        private AmitalContext _AmitalContext;

        protected override void OnCreating(CourierMasterPM entityPM, EntityPM entityParentPM)
        {
            ValidateEntity(entityPM);

            entityPM.Id = IdCounter.GetNumber("Customs.CourierMaster", entityPM.Tenant);
            entityPM.CreateDateTime = DateTime.Now;
            entityPM.IsOpen = true;
            entityPM.IsCancelled = false;
        }

        protected override void OnUpdating(CourierMasterPM entityPM, CourierMaster entityPOCO)
        {
            ValidateEntity(entityPM);
            Contact loggedContact = GetLoggedContact(entityPM.Tenant);
            ICustomContext context = MainContext as CustomContext;
            entityPM.UpdateDateTime = DateTime.Now;
            entityPM.UpdatedByUserId = loggedContact.Id;
            CourierDeclarationQueryService courierDeclarationQuery = new CourierDeclarationQueryService(entityPM.Tenant);
            CourierDeclarationUpdateService courierDeclarationUpdateService = new CourierDeclarationUpdateService(context, new Dictionary<string, IContext>(), entityPOCO.Tenant);
            if (entityPM.ConnectedDeclarations != null && entityPM.ConnectedDeclarations.Length > 0 )
            {

                //entityPM.ConnectedDeclarations = entityPM.ConnectedDeclarations.Substring(1, entityPM.ConnectedDeclarations.Length - 1);
                entityPM.ConnectedDeclarations = entityPM.ConnectedDeclarations.Substring(0, entityPM.ConnectedDeclarations.Length - 1);
                string[] items = entityPM.ConnectedDeclarations.Split(',');
                CourierDeclarationQueryService service = new CourierDeclarationQueryService(entityPM.Tenant);
                int? maxSequenceNunmeric = 0;
                 maxSequenceNunmeric = service.GetCourierMasterMaxSequenceNumeric(entityPM.Id, entityPM.Tenant);
                if (maxSequenceNunmeric == null) maxSequenceNunmeric = 0;
                foreach (string item in items)
                {
                    ++maxSequenceNunmeric;
                    CourierDeclarationPM courierDeclaration = new CourierDeclarationPM() { DeclarationId = item, CourierMasterId = entityPOCO.Id, Tenant = entityPOCO.Tenant, ChangeSetOp = ChangeSetOperation.Insert,SequenceNumeric=maxSequenceNunmeric};
                    courierDeclarationUpdateService.Update(courierDeclaration, false);

                }
            }

            if (entityPM.NotConnectedDeclarations != null && entityPM.NotConnectedDeclarations.Length > 0)
            {
                //entityPM.NotConnectedDeclarations = entityPM.NotConnectedDeclarations.Substring(1, entityPM.NotConnectedDeclarations.Length - 1);
                entityPM.NotConnectedDeclarations = entityPM.NotConnectedDeclarations.Substring(0, entityPM.NotConnectedDeclarations.Length - 1);
                string[] NotConnecteditems = entityPM.NotConnectedDeclarations.Split(',');

                if (NotConnecteditems != null && NotConnecteditems.Length > 0)
                {
                    foreach (string item in NotConnecteditems)
                    {
                        CourierDeclarationPM courierDeclaration = new CourierDeclarationPM();
                        CourierDeclarationQueryService courierDeclarationDelQuery = new CourierDeclarationQueryService(entityPM.Tenant);
                        courierDeclaration = courierDeclarationDelQuery.GetSingle(item, entityPOCO.Id, false, true);
                        courierDeclaration.ChangeSetOp = ChangeSetOperation.Delete;
                        courierDeclarationUpdateService.Update(courierDeclaration, true);
                    }
                }
            }

//Task 44476 remove if in order to always create task - in case another field was changed but cfi don't has updated value
//            if (entityPM.HAWB != entityPOCO.HAWB || entityPM.MAWB != entityPOCO.MAWB || entityPM.AirlineId != entityPOCO.AirlineId)
//            {
                if (entityPM.ConnectedDeclarations != null && entityPM.ConnectedDeclarations.Length > 0)
                {
                    this.toSendTask = true;
                }
                else
                {

                    int? maxSequenceNunmeric = 0;
                    maxSequenceNunmeric = courierDeclarationQuery.GetCourierMasterMaxSequenceNumeric(entityPM.Id, entityPM.Tenant);
                    if(maxSequenceNunmeric != null && maxSequenceNunmeric > 0)
                    {
                        this.toSendTask = true;
                    }
                }
//            }

            entityPM.ConnectedDeclarations = null;
            entityPM.NotConnectedDeclarations = null;

            if(entityPM.EstimatedArrivalDateOnly != null && entityPM.EstimatedArrivalDateOnly.HasValue)
            {
                DateTime date = (DateTime)entityPM.EstimatedArrivalDateOnly;
                if(entityPM.EstimatedArrivalTimeOnly != null && entityPM.EstimatedArrivalTimeOnly.HasValue)
                {
                    date = DateTime.Parse(string.Format("{0} {1}:{2}", entityPM.EstimatedArrivalDateOnly.Value.ToString("dd-MM-yyyy"), entityPM.EstimatedArrivalTimeOnly.Value.Hour, entityPM.EstimatedArrivalTimeOnly.Value.Minute));
                }
                entityPM.EstimatedArrivalDate = date;
            }

            if(!string.IsNullOrWhiteSpace(entityPM.HAWB))
            {
                entityPM.ShortHAWB = entityPM.HAWB;
                int indexStart = entityPM.ShortHAWB.IndexOf("-");
                if (indexStart > 0)
                {
                    entityPM.ShortHAWB = entityPM.ShortHAWB.Substring(indexStart + 1);
                }

                int indexEnd = entityPM.ShortHAWB.IndexOf("/");
                if (indexEnd > 0)
                {
                    entityPM.ShortHAWB = entityPM.ShortHAWB.Substring(0,indexEnd);
                }

                if(entityPM.ShortHAWB.Length > 8)
                {
                    var start = entityPM.ShortHAWB.Length - 8;
                    entityPM.ShortHAWB = entityPM.ShortHAWB.Substring(start);
                }
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                CourierMasterPM dbOccCourierMasterPM = GetDBEntity(entityPM.Id, entityPM.Tenant);

                if ((entityPM.GatewayPortCode != null && entityPM.GatewayPortCode != dbOccCourierMasterPM.GatewayPortCode) ||
                    (entityPM.OriginPortCode != null && entityPM.OriginPortCode != dbOccCourierMasterPM.OriginPortCode) ||
                    (entityPM.MAWB != null && entityPM.MAWB != dbOccCourierMasterPM.MAWB) ||
                    (entityPM.MAWBTypeCode != null && entityPM.MAWBTypeCode != dbOccCourierMasterPM.MAWBTypeCode) ||
                    (entityPM.AirlineId != null && entityPM.AirlineId != dbOccCourierMasterPM.AirlineId) ||
                    (entityPM.WeightValueCode != null && entityPM.WeightValueCode != dbOccCourierMasterPM.WeightValueCode))
                {
                    toSetDeclarationChanged = true;
                }
            }

            base.OnUpdating(entityPM, entityPOCO);
        }


        private CourierMasterPM GetDBEntity(string dirtyCourierMasterId, int tenant)
        {

            var courierMasterQueryService = new CourierMasterQueryService(tenant);
            var myDBEntity = courierMasterQueryService.GetSingle(dirtyCourierMasterId, false, false);
            return myDBEntity ?? new CourierMasterPM();

        }

        protected override void Trace(CourierMasterPM entityPM, CourierMaster entityPOCO, string changesXml)
        {
            Contact loggedContact = GetLoggedContact(entityPM.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "Customs.CourierMaster",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {

                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "Customs.CourierMaster",
                    IsAddedManually = false,
                    EventTypeCode = "UPEV",
                  

                };
                EventTracer.CreateTraceEvent(eventTracerArgs);


              
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }

        internal void ValidateEntity(CourierMasterPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CourierMasterRepository rep = new CourierMasterRepository(context);
            bool exist = rep.ChcekIfCourierExists(entityPM.Id, entityPM.AirlineId, entityPM.HAWB, entityPM.MAWB, entityPM.Tenant);
            if (exist)
            {
                throw new Exception(TranslateTextsClass.Translate("Customs.General.O.CourierAlreadyExist", entityPM.Tenant,true));
            }

        }

     
        private Contact GetLoggedContact(int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            email = (AuthenticationUtil.IsAuthenticatedUserExists() ? AuthenticationUtil.GetAuthenticatedUser() : ("system@tenant" + tenant + ".com"));

            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);
            return contact;
        }

        private void SetDeclarationChanged(string declarations , string courierManifestStatusCode, int Tenant)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(Tenant);
            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update DECLARATIONCOURIERSTATUSES set " +
                        "COURIERMANIFESTSTATUSCODE= '" + courierManifestStatusCode + "' ";
                    cmd = cmd + " where DECLARATIONID IN " + "(" + declarations + ")" ;

                    OracleCommand sqlCommand = new OracleCommand(cmd, con);

                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = "Update DECLARATIONCOURIERSTATUSES set " +
                        "COURIERMANIFESTSTATUSCODE= '" + courierManifestStatusCode + "' ";
                    cmd = cmd + " where DECLARATIONID IN " + "(" + declarations + ")";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
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

        protected override void AfterUpdating(CourierMasterPM entityPM, EntityPM entityParentPM)
        {
            if (this.toSendTask == true)
            {
                OpenUnifreighTask(entityPM, "LMC2U", "RSH", true, "");
            }

            if (entityPM.AirlineId != null)
            {
                CustomsAirlineQueryService customsAirlineQueryService = new CustomsAirlineQueryService(entityPM.Tenant);
                CustomsAirlinePM customsAirline = customsAirlineQueryService.GetSingle(entityPM.AirlineId, false, true);
                    if (customsAirline != null)
                {
                    entityPM.AirlinePrefix = customsAirline.AirlinePrefix;
                    entityPM.AirlineName = customsAirline.LocalName;
                }
            }

            if (entityPM.GatewayPortCode != null)
            {
                InternationalSiteQueryService internationalSiteQueryService = new InternationalSiteQueryService(entityPM.Tenant);
                InternationalSitePM internationalSitePM = internationalSiteQueryService.GetSingle(entityPM.GatewayPortCode, false, true);
                if (internationalSitePM != null)
                {
                    entityPM.GatewayPortName = internationalSitePM.LocalName;
                }
            }

            if(toSetDeclarationChanged == true)
            {
                string setCourierManifestStatusCode = "R";
                string setDeclarationsList = "";

                CustomsRequiredFieldErrors errorsForCourierDeclaration = CustomsRequiredFieldsValidator.GetCourierMasterRequiredFieldErrorsForCourierDeclaration(entityPM.Id, entityPM.Tenant);
                if (errorsForCourierDeclaration == null || (errorsForCourierDeclaration != null && errorsForCourierDeclaration.RequiredFields == null))
                { 
                    CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(entityPM.Tenant);
                    List<string> declarations = courierDeclarationRepository.GetCourierConnectedDeclaratinsList(entityPM.Id, entityPM.Tenant);
                    if (declarations != null)
                    {
                        DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(entityPM.Tenant);
                        foreach (var declarationId in declarations)
                        {
                            DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationId, false, false);
                            if (myDeclarationCourierStatusPM.CourierManifestStatusCode != "M" && myDeclarationCourierStatusPM.CourierManifestStatusCode != "R")
                            {
                                if (setDeclarationsList == "")
                                {
                                    setDeclarationsList = string.Concat("'", declarationId, "'");
                                }
                                else
                                {
                                    setDeclarationsList = string.Concat(setDeclarationsList, ",", "'", declarationId, "'");
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(setDeclarationsList))
                        {
                            this.SetDeclarationChanged(setDeclarationsList, setCourierManifestStatusCode, entityPM.Tenant);
                        }
                    }
                }
            }
        }


        private void OpenUnifreighTask(CourierMasterPM dirtyCourierMasterPM, string taskType, string status, bool raiseStatus, string xmlStatus)
        {
            var sw = Stopwatch.StartNew();
            TransactionScope scope = null;
            var statusDateTime = DateTime.Now;
            
            if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
            {
                scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
            }
            try
            {
                using (_AmitalContext = AmitalContext.GetContext(dirtyCourierMasterPM.Tenant))
                {
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var myYCULTASKUpdateService = new YCULTASKUpdateService(_AmitalContext);
                    myYCULTASKUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.
                    var requestData = "";
                    
                    string unifreightUser = null;
                    
                    if (String.IsNullOrWhiteSpace(unifreightUser))
                    {
                        unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(dirtyCourierMasterPM.Tenant);
                    }

                    var myYCULTASKPM = new YCULTASKPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        STATUS = "W",
                        REQUESTDATA = requestData,
                        ENTNAME = "MASTER",
                        PRIMARYNUM = dirtyCourierMasterPM.Id,
                        PRIORITY = YCULTASKPM.calcPriority(taskType),
                        //PRIORITY = priority,
                        TYPE = taskType,
                        USRCODE = unifreightUser,
                        ARCHIVE = "F", 
                    };
                    
                    myYCULTASKUpdateService.Update(myYCULTASKPM, true);

                    var myGGGQPM = new GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", //LugitudeRequest
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
                        TRY = 9,
                        PRIORITY = 8,
                        ENTNAME = "MASTER",
                        PRIMARYNUM = dirtyCourierMasterPM.Id,
                        FORMID = "LGT_UPDATE_FCI",
                        DEBUG = "F",
                        DONEOPERATION = "A",
                        //GSTRING1 = myYCULTASKPM.TASKID,
                    };
                    myGGGQUpdateService.Update(myGGGQPM, true);

                    if (scope != null)
                    {
                        scope.Complete();
                    }
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
