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
using Logitude.Customs.BL.EntityDataMappings;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CourierMasterUpdateService
    {
        private Boolean toSendTask = false;
        private Boolean toSetDeclarationChanged = false;
        private AmitalContext _AmitalContext;

        public bool CloseCourierMaster { get; set; }

        protected override void OnCreating(CourierMasterPM entityPM, EntityPM entityParentPM)
        {
            ValidateEntity(entityPM);

            entityPM.Id = IdCounter.GetNumber("Customs.CourierMaster", entityPM.Tenant);
            entityPM.CreateDateTime = DateTime.Now;
            entityPM.IsOpen = true;
            int.TryParse(EntityPM.NoOfCourierHawb, out int noOfCourierHawb);
            //EntityPM.OpenDeclarations = noOfCourierHawb;
            entityPM.IsCancelled = false;
        }

        protected override void OnUpdating(CourierMasterPM entityPM, CourierMaster entityPOCO)
        {
            ValidateEntity(entityPM);
            if (entityPOCO != null && entityPM.IsReadyForInvoice && !entityPOCO.IsReadyForInvoice)
            {
                BuildGGGQ_FLIGHT_CREDIT_LETTER(entityPM);
            }


            Contact loggedContact = GetLoggedContact(entityPM.Tenant);
            ICustomContext context = MainContext as CustomContext;
            entityPM.UpdateDateTime = DateTime.Now;
            entityPM.UpdatedByUserId = loggedContact.Id;
            CourierDeclarationQueryService courierDeclarationQuery = new CourierDeclarationQueryService(entityPM.Tenant);
            CourierDeclarationUpdateService courierDeclarationUpdateService = new CourierDeclarationUpdateService(context, new Dictionary<string, IContext>(), entityPOCO.Tenant);
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), entityPOCO.Tenant);
            DeclarationRepository declarationRepository1 = new DeclarationRepository(context);

            if (entityPM.ConnectedDeclarations != null && entityPM.ConnectedDeclarations.Length > 0)
            {
                CourierDeclarationQueryService service = new CourierDeclarationQueryService(entityPM.Tenant);
                int? maxSequenceNunmeric = 0;
                maxSequenceNunmeric = service.GetCourierMasterMaxSequenceNumeric(entityPM.Id, entityPM.Tenant);
                if (maxSequenceNunmeric == null) maxSequenceNunmeric = 0;
               DeclarationCourierStatusRepository rep = new DeclarationCourierStatusRepository(context);
                entityPM.OpenDeclarations = rep.CountOpenDeclarations(entityPM.Id, entityPM.Tenant);
                if (entityPM.ConnectedDeclarations == "ALL")
                {

                    var decsC = declarationRepository1.GetNotConnectedDeclarations(entityPM.Tenant);
                    foreach (var dec in decsC)
                    {
                        ++maxSequenceNunmeric;
                        CourierDeclarationPM courierDeclaration = new CourierDeclarationPM() { DeclarationId = dec.Id, CourierMasterId = entityPOCO.Id, Tenant = entityPOCO.Tenant, ChangeSetOp = ChangeSetOperation.Insert, SequenceNumeric = maxSequenceNunmeric };
                        courierDeclarationUpdateService.Update(courierDeclaration, false);
                        DeclarationCourierStatus decCourier = rep.GetDeclarationsById(dec.Id, dec.Tenant);
                        if(decCourier!= null)
                        {
                            if (!decCourier.IsClosedForFollowUp)
                            {
                                entityPM.OpenDeclarations += 1;
                            }
                        }
                    }
                }
                else
                {
                    //entityPM.ConnectedDeclarations = entityPM.ConnectedDeclarations.Substring(1, entityPM.ConnectedDeclarations.Length - 1);
                    entityPM.ConnectedDeclarations = entityPM.ConnectedDeclarations.Substring(0, entityPM.ConnectedDeclarations.Length - 1);
                    string[] items = entityPM.ConnectedDeclarations.Split(',');
                    //DeclarationCourierStatusRepository rep = new DeclarationCourierStatusRepository(context);
                   // entityPM.OpenDeclarations = rep.CountOpenDeclarations(entityPM.Id, entityPM.Tenant);
                    foreach (string item in items)
                    {
                        ++maxSequenceNunmeric;
                        CourierDeclarationPM courierDeclaration = new CourierDeclarationPM() { DeclarationId = item, CourierMasterId = entityPOCO.Id, Tenant = entityPOCO.Tenant, ChangeSetOp = ChangeSetOperation.Insert, SequenceNumeric = maxSequenceNunmeric };
                        courierDeclarationUpdateService.Update(courierDeclaration, false);
                        DeclarationCourierStatus decCourier = rep.GetDeclarationsById(item, entityPOCO.Tenant);
                        if (decCourier != null)
                        {
                            if (!decCourier.IsClosedForFollowUp)
                            {
                                entityPM.OpenDeclarations += 1;
                            }
                        }
                    }
                }
            }

            if (entityPM.NotConnectedDeclarations != null && entityPM.NotConnectedDeclarations.Length > 0)
            {
                //entityPM.NotConnectedDeclarations = entityPM.NotConnectedDeclarations.Substring(1, entityPM.NotConnectedDeclarations.Length - 1);

                DeclarationCourierStatusRepository rep = new DeclarationCourierStatusRepository(context);
                entityPM.OpenDeclarations = rep.CountOpenDeclarations(entityPM.Id, entityPM.Tenant);
                if (entityPM.NotConnectedDeclarations == "ALL")
                {
                    var decsCN = declarationRepository1.GetCourierConnectedDeclaratins(entityPOCO.Id, entityPM.Tenant);
                    foreach (var item in decsCN)
                    {
                        CourierDeclarationPM courierDeclaration = new CourierDeclarationPM();
                        CourierDeclarationQueryService courierDeclarationDelQuery = new CourierDeclarationQueryService(entityPM.Tenant);
                        courierDeclaration = courierDeclarationDelQuery.GetSingle(item.Id, entityPOCO.Id, false, true);
                        courierDeclaration.ChangeSetOp = ChangeSetOperation.Delete;
                        courierDeclarationUpdateService.Update(courierDeclaration, true);
                        DeclarationCourierStatus decCourier = rep.GetDeclarationsById(item.Id, item.Tenant);
                        if (decCourier != null)
                        {
                            if (!decCourier.IsClosedForFollowUp)
                            {
                                entityPM.OpenDeclarations -= 1;
                            }
                        }
                    }
                }
                else
                {
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
                            DeclarationCourierStatus decCourier = rep.GetDeclarationsById(item, entityPOCO.Tenant);
                            if (decCourier != null)
                            {
                                if (!decCourier.IsClosedForFollowUp)
                                {
                                    entityPM.OpenDeclarations -= 1;
                                }
                            }
                        }
                    }
                }
            }

            //Task 44476 remove if in order to always create task - in case another field was changed but cfi don't has updated value
            //            if (entityPM.HAWB != entityPOCO.HAWB || entityPM.MAWB != entityPOCO.MAWB || entityPM.AirlineId != entityPOCO.AirlineId)
            //            {
            if (entityPM.HAWB != entityPOCO.HAWB || entityPM.MAWB != entityPOCO.MAWB || entityPM.AirlineId != entityPOCO.AirlineId || entityPM.TruckerId != entityPOCO.TruckerId || 
                entityPM.IsCancelled != entityPOCO.IsCancelled || entityPM.IsReadyForInvoice != entityPOCO.IsReadyForInvoice || 
                ((entityPM.EstimatedArrivalDateOnly.HasValue && (!entityPOCO.EstimatedArrivalDate.HasValue || entityPOCO.EstimatedArrivalDate.Value.Date != entityPM.EstimatedArrivalDateOnly)) || 
                (!entityPM.EstimatedArrivalDateOnly.HasValue && entityPOCO.EstimatedArrivalDate.HasValue)) || 
                entityPM.GatewayPortCode != entityPOCO.GatewayPortCode)
            {
                if (entityPM.ConnectedDeclarations != null && entityPM.ConnectedDeclarations.Length > 0)
                {
                    this.toSendTask = true;
                }
                else
                {

                    int? maxSequenceNunmeric = 0;
                    maxSequenceNunmeric = courierDeclarationQuery.GetCourierMasterMaxSequenceNumeric(entityPM.Id, entityPM.Tenant);
                    if (maxSequenceNunmeric != null && maxSequenceNunmeric > 0)
                    {
                        this.toSendTask = true;
                    }
                }
            }
            //            }

            string declarations = "";
            var declarationRepository = new DeclarationRepository(context);

            var decs = declarationRepository.GetCourierConnectedDeclaratins(entityPM.Id, entityPM.Tenant);

            if (decs.Count() > 0)
            {
                declarations = string.Join("','", decs.Select(x => x.Id));
                declarations = "'" + declarations + "'";
            }


            if (!this.CloseCourierMaster)
            {


                if (entityPM.IsCancelled == true && entityPOCO.IsCancelled != true)
                {

                    entityPM.IsOpen = false;
                    this.toSendTask = true;
                    if (!string.IsNullOrWhiteSpace(declarations))
                    {
                        CancelledDeclarations(declarations, entityPM.Tenant);
                    }

                }

                else if (entityPM.IsCancelled != true && entityPOCO.IsCancelled == true)
                {

                    entityPM.IsOpen = true;
                    this.toSendTask = true;
                    if (!string.IsNullOrWhiteSpace(declarations))
                    {
                        OpenDeclarations(declarations, entityPM.Tenant);
                    }

                }
            }
            entityPM.ConnectedDeclarations = null;
            entityPM.NotConnectedDeclarations = null;


            if (entityPM.EstimatedArrivalDateOnly != null && entityPM.EstimatedArrivalDateOnly.HasValue)
            {
                DateTime date = (DateTime)entityPM.EstimatedArrivalDateOnly;
                if (entityPM.EstimatedArrivalTimeOnly != null && entityPM.EstimatedArrivalTimeOnly.HasValue)
                {
                    date = DateTime.Parse(string.Format("{0} {1}:{2}", entityPM.EstimatedArrivalDateOnly.Value.ToString("dd-MM-yyyy"), entityPM.EstimatedArrivalTimeOnly.Value.Hour, entityPM.EstimatedArrivalTimeOnly.Value.Minute));
                }
                entityPM.EstimatedArrivalDate = date;
            }

            if (entityPM.LandingDateDateOnly != null && entityPM.LandingDateDateOnly.HasValue)
            {
                DateTime date = (DateTime)entityPM.LandingDateDateOnly;
                if (entityPM.LandingDateTimeOnly != null && entityPM.LandingDateTimeOnly.HasValue)
                {
                    date = DateTime.Parse(string.Format("{0} {1}:{2}", entityPM.LandingDateDateOnly.Value.ToString("dd-MM-yyyy"), entityPM.LandingDateTimeOnly.Value.Hour, entityPM.LandingDateTimeOnly.Value.Minute));
                }
                entityPM.LandingDate = date;
            }

            if (!string.IsNullOrWhiteSpace(entityPM.HAWB))
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
                    entityPM.ShortHAWB = entityPM.ShortHAWB.Substring(0, indexEnd);
                }

                if (entityPM.ShortHAWB.Length > 8)
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

        private void BuildGGGQ_FLIGHT_CREDIT_LETTER(CourierMasterPM entityPM)
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
                using (_AmitalContext = AmitalContext.GetContext(entityPM.Tenant))
                {
                    var myGGGQUpdateService = new GGGQUpdateService(_AmitalContext);
                    myGGGQUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.

                    var requestData = "";

                    string unifreightUser = null;

                    if (String.IsNullOrWhiteSpace(unifreightUser))
                    {
                        unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(entityPM.Tenant);
                    }



                    var myGGGQPM = new GGGQPM()
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        ORIGINQUE = "LGT", //LugitudeRequest
                        STATUS = "1",
                        EXPTASKTIME = 5,
                        EXECDATE = (new DualQueryService(_AmitalContext as AmitalContext)).GetServerDateTime() ?? DateTime.Now.AddMinutes(-20), //-20 because of time differences between the server where the code runs in and the DB server
                        TRY = 5,
                        PRIORITY = 8,

                        ENTNAME = "CFIFILEM",
                        PRIMARYNUM = "-1",
                        FORMID = "A1468",
                        GSTRING1 = "A1468",
                        GSTRING2 = "NONE",
                        GSTRING3 = entityPM.Id,

                        DEBUG = "F",
                        DONEOPERATION = "D",
                        QUEUEMANAGEMENT = true,


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
                throw new Exception(TranslateTextsClass.Translate("Customs.General.O.CourierAlreadyExist", entityPM.Tenant, true));
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



        private void OpenDeclarations(string declarations, int Tenant)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(Tenant);
            string whereIn = "";
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
                //     whereIn += ")";


            }

            else
            {
                whereIn = declarations;
            }

            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update DECLARATIONS set " +
                        "ISCLOSE= 0  , ISCANCELLED =0 ";
                    cmd = cmd + " where ID IN " + "(" + whereIn + ")";

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
                    string cmd = "Update DECLARATIONS set " +
                        "ISCLOSE= 0  , ISCANCELLED =0 ";
                    cmd = cmd + " where ID IN " + "(" + declarations + ")";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }



        private void CancelledDeclarations(string declarations, int Tenant)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(Tenant);
            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update DECLARATIONS set " +
                        "ISCLOSE= 1  , ISCANCELLED =1 ";
                    cmd = cmd + " where ID IN " + "(" + declarations + ")";

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
                    string cmd = "Update DECLARATIONS set " +
                        "ISCLOSE= 1  , ISCANCELLED =1 ";
                    cmd = cmd + " where ID IN " + "(" + declarations + ")";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }


        private void SetDeclarationChanged(string declarations, string courierManifestStatusCode, int Tenant)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(Tenant);
            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update DECLARATIONCOURIERSTATUSES set " +
                        "COURIERMANIFESTSTATUSCODE= '" + courierManifestStatusCode + "' ";
                    cmd = cmd + " where DECLARATIONID IN " + "(" + declarations + ")";

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

            if (toSetDeclarationChanged == true)
            {
                string setCourierManifestStatusCode = "R";
                string setDeclarationsList = "";

                CustomsRequiredFieldErrors errorsForCourierDeclaration = CustomsRequiredFieldsValidator.GetCourierMasterRequiredFieldErrorsForCourierDeclaration(entityPM.Id, entityPM.Tenant);
                if (errorsForCourierDeclaration == null || (errorsForCourierDeclaration != null && errorsForCourierDeclaration.RequiredFields == null) ||
                    (errorsForCourierDeclaration != null && errorsForCourierDeclaration.RequiredFields != null && errorsForCourierDeclaration.RequiredFields.Count == 0))
                {
                    string CourierMasterId = EntityPM.Id;
                    //string toCourierManifestStatusCode = "R";
                    string updateCourierManifestStatusCodeToR =
                        $"Update DECLARATIONCOURIERSTATUSES set CourierManifestStatusCode ='{setCourierManifestStatusCode}' where DECLARATIONID  in (select DECLARATIONID   from CourierDeclarations  where CourierMasterId ='{CourierMasterId}' and tenant ={EntityPM.Tenant} ) and CourierManifestStatusCode !='M' and CourierManifestStatusCode !='R' ";
                    int commandTimeout = 30;
                    CustomContext.CommandExecuteNonQuery(EntityPM.Tenant, updateCourierManifestStatusCodeToR, commandTimeout);

                    //setDeclarationsList = OldNotInUse(entityPM, setCourierManifestStatusCode, setDeclarationsList);
                }
            }
        }

        private string OldNotInUse(CourierMasterPM entityPM, string setCourierManifestStatusCode, string setDeclarationsList)
        {
            CourierDeclarationRepository courierDeclarationRepository = new CourierDeclarationRepository(entityPM.Tenant);
            List<string> declarations = courierDeclarationRepository.GetCourierConnectedDeclaratinsList(entityPM.Id, entityPM.Tenant);
            if (declarations != null)
            {
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(entityPM.Tenant);
                foreach (var declarationId in declarations)
                {
                    DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationId, true, false);
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

            return setDeclarationsList;
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
                        DONEOPERATION = "D",
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
