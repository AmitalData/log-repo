 
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.StimulReport.Mapping;
using Logitude.Customs.Data;
using Logitude.Server.Tools.Contracts;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.AmitalMessaging.Customs.CustomFile.ExportStorageDTD;

namespace Logitude.Customs.BL.Messaging.U2L.ExportStorage
{
    public class ExportStorageUpsertService : UnifreightGenericService
    {
        private LogitudeStorage _UnifreigntExportStorage;
        
        private ICustomContext _context;
        private GTRTRANQueryService _GTRTRANQueryService;
        private AmitalContext _AmitalContext;
        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.ExportStorage.ExportStorageService.Upsert()";
        private Stopwatch _Stopwatch;
        private ExportStoragePM _DBExportStoragePM;
        public Boolean suppressNewTrans;

        public ExportStorageUpsertService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGIEXPORTSTORAGE,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            try
            {

                _Stopwatch = Stopwatch.StartNew();
                MyCommunicationsParams.Subject = "ExportStorageService ";

                DeserilazeObject(xmlLOGIEXPORTSTORAGE);
                AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                CheckIntegrity();
                AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.Stage = "GetContext";
                _context = CustomContext.GetContext(ResolvedTenant());
                var myQueryService = new ExportStorageQueryService(_context);
                if (!String.IsNullOrWhiteSpace(this._UnifreigntExportStorage.Id))
                {
                    this._DBExportStoragePM = myQueryService.GetSingle(this._UnifreigntExportStorage.Id, true, false);       
                }
                if (this._DBExportStoragePM ==null )
                {
                    this._DBExportStoragePM = myQueryService.GetByStorageNo(this._UnifreigntExportStorage.StorageNo, tenant: ResolvedTenant());
                }
                

                

                ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
                MyCommunicationsParams.Tenant = ResolvedTenant();
                MyGenericResponseObj.Stage = "Upsert";
                Upsert(suppressNewTrans);
                MyGenericResponseObj.Stage = "Done";
                AppendLogLine("Upsert Storage Data:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.ApplicationId = _DBExportStoragePM.Id;
                //MyGenericResponseObj.ResponseXml = xml;
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
                MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;
            }
            catch (DbEntityValidationException ex)
            {
                var formatedException = ExceptionFormatUtil.GetFormated(ex);

                InsertLogLine(0, "ProccessRequest():Exception " + formatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                Debug.WriteLine("ProccessRequest():Exception " + formatedException.ToString(), true);
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                MyGenericResponseObj.Message = "Error while ExportStorageUpdateService.Update " + formatedException.Message;
                MyGenericResponseObj.ErrorDescription = formatedException.ToString();
                if (formatedException.InnerException != null)
                {
                    MyGenericResponseObj.InnerException = formatedException.InnerException.ToString();
                }

                MessageOut = MyGenericResponseObj.ErrorDescription;
            }
            catch (Exception e)
            {
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.TecinicalFailure;
                MyGenericResponseObj.Message = "Exception: " + e.Message;
                MyGenericResponseObj.ErrorDescription = e.ToString();
                if (e.InnerException != null)
                {
                    MyGenericResponseObj.InnerException = e.InnerException.ToString();
                }
                MessageOut = MyGenericResponseObj.ErrorDescription;
            }
            finally
            {
                try
                {
                    if (!String.IsNullOrWhiteSpace(MyCommunicationsParams.LoggingEntityId))
                    {
                        MyCommunicationsParams.LoggingObjectTableId = GetLoggingObjectTableId("Customs.ExportStorage");
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void Upsert(bool suppressNewTrans)
        {

            AppendLogLine("Update:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            using (TransactionScope scope =
               suppressNewTrans ? TransactionFactory.GetTransaction() : TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            {
                AppendLogLine("Upsert..");
                var exportStorageUpdateService = new ExportStorageUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenant());

                if (this._DBExportStoragePM == null)
                {
                    AppendLogLine("Insert!!");
                    _DBExportStoragePM = new ExportStoragePM
                    {
                        ChangeSetOp = ChangeSetOperation.Insert,
                        Tenant = ResolvedTenant(),
                        StorageNo = _UnifreigntExportStorage.StorageNo,
                        OpenDate = AmitalConvertUtil.GetUnifreightFormatedDate(_UnifreigntExportStorage.OpenDate, "UnifreigntExportStorage.OpenDate") ?? DateTime.Now,
                        ExportFileNo = _UnifreigntExportStorage.ExportFileNo,
                    };
                }
                else
                {
                    if (_DBExportStoragePM.StorageNo != _UnifreigntExportStorage.StorageNo)
                    {
                        throw new BusinessErrorException($"_DBExportStoragePM.StorageNo != _UnifreigntExportStorage.StorageNo  ({_DBExportStoragePM.StorageNo} != {_UnifreigntExportStorage.StorageNo})");
                    }
                    //if (_DBExportStoragePM.ExportFileNo != _UnifreigntExportStorage.ExportFileNo)
                    //{
                    //    throw new BusinessErrorException($"_DBExportStoragePM.StorageNo != _UnifreigntExportStorage.StorageNo  ({_DBExportStoragePM.StorageNo} != {_UnifreigntExportStorage.StorageNo})");
                    //}

                    AppendLogLine("Update!!");
                    _DBExportStoragePM.ChangeSetOp = ChangeSetOperation.Update;

                }


                _DBExportStoragePM.DeclarationId = _UnifreigntExportStorage.DeclarationId;

                _DBExportStoragePM.ExportDealIdentification = _UnifreigntExportStorage.General.ExportDealIdentification;
                _DBExportStoragePM.ExporterID = _UnifreigntExportStorage.General.ExporterNumber;
                //UnifreigntExportStorage.General.ExporterFileNumber = "Exp_Ref_INV1";
                _DBExportStoragePM.ShipCode = _UnifreigntExportStorage.General.ShipCode;



                _DBExportStoragePM.CargoTypeCode = _UnifreigntExportStorage.CargoIdentifier.CargoIdentifierType;
                _DBExportStoragePM.FirstCargoID = _UnifreigntExportStorage.CargoIdentifier.CargoIdentifierKey1;
                _DBExportStoragePM.SecondCargoID = _UnifreigntExportStorage.CargoIdentifier.CargoIdentifierKey2;
                _DBExportStoragePM.ThirdCargoID = _UnifreigntExportStorage.CargoIdentifier.CargoIdentifierKey3;


                _DBExportStoragePM.CargoType = _UnifreigntExportStorage.CargoDetails.CargoType;



                
                exportStorageUpdateService.Update(_DBExportStoragePM, true);

                scope.Complete();
            }
        }

        

        protected override int ResolvedTenant() 
        {
            return int.Parse(_UnifreigntExportStorage.Tenant);
        }

       

        private string GetTranslationP2L(string partnerID, string tableID, string partnerCode)
        {

            if (partnerID == null || tableID == null || partnerCode == null)
            {
                return ("");
            }

            using (_AmitalContext = AmitalContext.GetContext(_DBExportStoragePM.Tenant))
            {
                _GTRTRANQueryService = new GTRTRANQueryService(_AmitalContext);
                var myGTRTRANPM = _GTRTRANQueryService.GetSingle(partnerID, tableID, partnerCode, null, true);

                if (myGTRTRANPM == null)
                {
                    return ("");
                }
                return (myGTRTRANPM.LOCALCODE);
            }

        }

        private void DeserilazeObject(string xmlLOGIEXPORTSTORAGE)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("ExportStorageService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGIEXPORTSTORAGE))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGIEXPORTSTORAGE.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGIEXPORTSTORAGE.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGIEXPORTSTORAGE);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._UnifreigntExportStorage = XmlGenericUtil<LogitudeStorage>.DeSerializeObject(xmlLOGIEXPORTSTORAGE);


            _UnifreigntExportStorage.General = _UnifreigntExportStorage.General ?? new AmitalMessaging.Customs.CustomFile.ExportStorageDTD.General();
            _UnifreigntExportStorage.CargoIdentifier = _UnifreigntExportStorage.CargoIdentifier ?? new CargoIdentifier();
            _UnifreigntExportStorage.CargoDetails = _UnifreigntExportStorage.CargoDetails ?? new CargoDetails();


        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._UnifreigntExportStorage.StorageNo))
            {
                throw new BusinessErrorException("StorageNo is missing");
            }
            AppendLogLine("StorageNo = " + this._UnifreigntExportStorage.StorageNo);
            

        }

        public override string GetAssemblyQualifiedName()
        {
            return this.GetType().Name;
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";

            var UnifreigntExportStorage = new LogitudeStorage();
            UnifreigntExportStorage.StorageNo = "92282";
            UnifreigntExportStorage.Tenant = "1";
            UnifreigntExportStorage.ExportFileNo = "3079";
            
            UnifreigntExportStorage.DeclarationId = "1-2";
            UnifreigntExportStorage.OpenDate = "26.10.21";

            UnifreigntExportStorage.General = new AmitalMessaging.Customs.CustomFile.ExportStorageDTD.General();
            UnifreigntExportStorage.General.ExportDealIdentification = "E413540111005994";
            UnifreigntExportStorage.General.ExporterNumber = "024310187";
            ///UnifreigntExportStorage.General.ExporterFileNumber = "Exp_Ref_INV1";
            UnifreigntExportStorage.General.ShipCode = "9231808";


            UnifreigntExportStorage.CargoIdentifier = new CargoIdentifier();
            UnifreigntExportStorage.CargoIdentifier.CargoIdentifierType = "13";
            UnifreigntExportStorage.CargoIdentifier. CargoIdentifierKey1= "0111092263";
            UnifreigntExportStorage.CargoIdentifier.CargoIdentifierKey2 = "2021";
            UnifreigntExportStorage.CargoIdentifier.CargoIdentifierKey3 = "55";


            UnifreigntExportStorage.CargoDetails = new CargoDetails();
            UnifreigntExportStorage.CargoDetails.CargoType = "1";
            xml = XmlGenericUtil<LogitudeStorage>.SerializeObject(UnifreigntExportStorage);

            return xml;
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

    }
}

