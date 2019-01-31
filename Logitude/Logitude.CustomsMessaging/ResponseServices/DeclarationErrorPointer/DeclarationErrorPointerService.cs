using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.TraceEvents;
//using UnifreightIIG.Common.MANIFESTRequestServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer
{
    public class DeclarationErrorPointerService
    {
        public DeclarationError _declarationErrorPointer = null;
        private DeclarationPM _MyDeclarationPM;

        public string Analyze(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse)
        {
            var myWCOTranslateSet = new WCOTranslateSet();
            var myWCOTranslate = myWCOTranslateSet.GetByWCOCode("");
            var declarationErrorPointer = new DeclarationError();

            var xml = XmlGenericUtil<DeclarationError>.SerializeObject(declarationErrorPointer);
            return xml;

        }

        public string AnalyzeErrorPionter(ResponseError[] responseError, DeclarationPM declarationPM, WCOTypeEnum myWCOTypeEnum = WCOTypeEnum.WCO, bool isRaiseUnifreightEvent = false) // to add ref to ResponseError in Logitude.CustomsMessaging
        {
            if (responseError == null)
            {
                return null;
            }

            _MyDeclarationPM = declarationPM;

            _declarationErrorPointer = new DeclarationError();
            _declarationErrorPointer.Entitites = new List<Entity>();

            foreach (var errorItem in responseError)
            {
                //if (Environment.UserDomainName.Equals("ntdomain", StringComparison.OrdinalIgnoreCase))
                //{
                //Analyze pointers
                GetLogitudeEntity(errorItem, myWCOTypeEnum, isRaiseUnifreightEvent);
                //}
            }

            var myDeclaretionErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(_declarationErrorPointer);

            return myDeclaretionErrorXml;
        }

        public string AnalyzeDeclarationException(UnifreightIIG.Common.ImportDeclarationServiceReference.Exception[] exception)
        {
            if (exception == null)
            {
                return null;
            }

            _declarationErrorPointer = new DeclarationError();
            _declarationErrorPointer.Entitites = new List<Entity>();

            Entity myEntityErrorDetail = new Entity();
            myEntityErrorDetail.Child1Type = "";
            myEntityErrorDetail.Child1Sequence = "";
            myEntityErrorDetail.Child2Type = "";
            myEntityErrorDetail.Child2Sequence = "";
            myEntityErrorDetail.Child3Type = "";
            myEntityErrorDetail.Child3Sequence = "";

            var myFieldError = new field();
            myFieldError.Code = "Buisness";
            myFieldError.ListVersionID = "1";
            myFieldError.MessageError = exception[0].ExeptionDescription;
            myEntityErrorDetail.FieldErrors = new List<field>();
            myEntityErrorDetail.FieldErrors.Add(myFieldError);

            _declarationErrorPointer.Entitites.Add(myEntityErrorDetail);

            var myDeclaretionErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(_declarationErrorPointer);

            return myDeclaretionErrorXml;
        }

        private void GetLogitudeEntity(ResponseError errorItem, WCOTypeEnum myWCOTypeEnum = WCOTypeEnum.WCO, bool isRaiseUnifreightEvent = false)
        {
            var myDB = WCO.Instance.CreateDB(myWCOTypeEnum);
            int pointerLevelCounter = 0;
            WCOErrorPointerModel myCargoDescription = null;
            WCOErrorPointerModel.LogitudeEntityEnum lastLogitudeEntity = WCOErrorPointerModel.LogitudeEntityEnum.None;

            string myChild1Type = "";
            string myChild1Sequence = "";
            string myChild2Type = "";
            string myChild2Sequence = "";
            string myChild3Type = "";
            string myChild3Sequence = "";
            bool isNewEntity = false;

            foreach (var pointerItem in errorItem.Pointer)
            {
                //string  NaturalKey = "";
                string SequenceNumeric = "0";
                myCargoDescription = WCO.Instance.CreateDB().GetTagID(pointerLevelCounter, pointerItem.DocumentSectionCode.Value);
                int pointerLevelCounterCurrent = pointerLevelCounter;
                myDB = myDB.GetNode(pointerLevelCounter++, pointerItem.DocumentSectionCode.Value);
                //lastLogitudeEntity = myCargoDescription.LogitudeEntity;

                if (pointerItem.TagID != null && !string.IsNullOrEmpty(pointerItem.TagID.Value))
                {
                    myCargoDescription = myDB.GetTagID(pointerLevelCounter, pointerItem.TagID.Value);
                }

                SequenceNumeric = pointerItem.SequenceNumeric.ToString();

                if (pointerItem.DMExtensions != null && pointerItem.DMExtensions.NaturalKey != null && pointerItem.DMExtensions.NaturalKey.Value != null)
                {
                    //NaturalKey = pointerItem.DMExtensions.NaturalKey.Value;
                    SequenceNumeric = pointerItem.DMExtensions.NaturalKey.Value;
                }

                if (myCargoDescription == null) //Temporary treatment until mapping all WCO records
                {
                    myCargoDescription = new WCOErrorPointerModel();
                    myCargoDescription.LogitudeEntity = WCOErrorPointerModel.LogitudeEntityEnum.None;
                    myCargoDescription.LogitudeFieldID = "";
                }

                switch (myCargoDescription.LogitudeEntity)
                {
                    case WCOErrorPointerModel.LogitudeEntityEnum.None:
                        HandleSpecialError(errorItem, ref myChild1Type, ref myChild1Sequence, ref myChild2Type, ref myChild2Sequence, ref myChild3Type, ref myChild3Sequence, pointerLevelCounterCurrent, SequenceNumeric, pointerItem.DocumentSectionCode.Value); // Handling special cases
                        break;
                    case WCOErrorPointerModel.LogitudeEntityEnum.Declaration:
                        break;
                    case WCOErrorPointerModel.LogitudeEntityEnum.DeclarationTaxes:
                        myChild1Type = "DeclarationTaxes";
                        myChild1Sequence = SequenceNumeric;
                        break;
                    case WCOErrorPointerModel.LogitudeEntityEnum.Consignment:
                        myChild1Type = "Consignment";
                        myChild1Sequence = SequenceNumeric;
                        break;
                    case WCOErrorPointerModel.LogitudeEntityEnum.ConsignmentPackages:
                        myChild2Type = "ConsignmentPackages";
                        myChild2Sequence = SequenceNumeric;
                        break;
                    case WCOErrorPointerModel.LogitudeEntityEnum.ConsignmentInternalTransitions:
                        myChild2Type = "ConsignmentInternalTransitions";
                        myChild2Sequence = SequenceNumeric;
                        break;
                    case WCOErrorPointerModel.LogitudeEntityEnum.GoodsShipment:
                        myChild1Type = "SupplierInvoice";
                        myChild1Sequence = SequenceNumeric;
                        break;
                    case WCOErrorPointerModel.LogitudeEntityEnum.CustomsValuation:
                        myChild2Type = "SupplierInvoiceModifications";
                        myChild2Sequence = SequenceNumeric;
                        break;
                    case WCOErrorPointerModel.LogitudeEntityEnum.GoodsItem:
                        myChild2Type = "SupplierInvoiceItem";
                        myChild2Sequence = SequenceNumeric;
                        break;
                    default:
                        break;
                }
            }

            HandleSpecialError(errorItem, ref myChild1Type, ref myChild1Sequence, ref myChild2Type, ref myChild2Sequence, ref myChild3Type, ref myChild3Sequence);// Handling special cases
            if (isRaiseUnifreightEvent)
            {
                HandleValidationCodeError(errorItem);
            }

            Entity myEntityErrorDetail = FindEntityinList(myChild1Type, myChild1Sequence, myChild2Type, myChild2Sequence, myChild3Type, myChild3Sequence);
            if (myEntityErrorDetail == null)
            {
                myEntityErrorDetail = new Entity();
                myEntityErrorDetail.Child1Type = myChild1Type;
                myEntityErrorDetail.Child1Sequence = myChild1Sequence;
                myEntityErrorDetail.Child2Type = myChild2Type;
                myEntityErrorDetail.Child2Sequence = myChild2Sequence;
                myEntityErrorDetail.Child3Type = myChild3Type;
                myEntityErrorDetail.Child3Sequence = myChild3Sequence;
                isNewEntity = true;
            }

            if (myCargoDescription.LogitudeFieldID == "")
            {
                //Add New Entity Error
                var myEntityError = new error();
                myEntityError.Code = errorItem.ValidationCode.Value;
                myEntityError.ListVersionID = errorItem.ValidationCode.listVersionID;
                myEntityError.MessageError = errorItem.ValidationCode.name;
                if (errorItem.DMExtensions != null && errorItem.DMExtensions.ConstraintID != 0) //Get Constraint
                {
                    myEntityError.ConstraintID = errorItem.DMExtensions.ConstraintID.ToString();
                }
                if (myEntityErrorDetail.EntityErrors == null)
                {
                    myEntityErrorDetail.EntityErrors = new List<error>();
                }
                myEntityErrorDetail.EntityErrors.Add(myEntityError);
            }
            else
            {
                //Add New Fields Error
                var myFieldError = new field();
                myFieldError.Code = errorItem.ValidationCode.Value;
                myFieldError.ListVersionID = errorItem.ValidationCode.listVersionID;
                myFieldError.MessageError = errorItem.ValidationCode.name;
                myFieldError.Fieldcode = myCargoDescription.LogitudeFieldID;
                if (errorItem.DMExtensions != null && errorItem.DMExtensions.ConstraintID != 0) //Get Constraint
                {
                    myFieldError.ConstraintID = errorItem.DMExtensions.ConstraintID.ToString();
                }
                if (myEntityErrorDetail.FieldErrors == null)
                {
                    myEntityErrorDetail.FieldErrors = new List<field>();
                }
                myEntityErrorDetail.FieldErrors.Add(myFieldError);
            }

            if (isNewEntity == true)
            {
                _declarationErrorPointer.Entitites.Add(myEntityErrorDetail);
            }
        }

        private void HandleSpecialError(ResponseError errorItem, ref string myChild1Type, ref string myChild1Sequence, ref string myChild2Type, ref string myChild2Sequence, ref string myChild3Type, ref string myChild3Sequence, int pointerLevelCounterCurrent = 0, string SequenceNumeric = "", string DocumentSectionCode = "")
        {
            string code = errorItem.ValidationCode.Value;
            switch (code)
            {
                case "2592":
                    {
                        myChild3Type = "SupplierInvioceItemsCertificate";
                        myChild3Sequence = "1";
                        break;
                    }

            }

            //Special: If this is level 3 of SupplierInvoiceItem as level 2 AND it is AdditionalDocument
            if (!string.IsNullOrWhiteSpace(SequenceNumeric) && myChild2Type == "SupplierInvoiceItem" && pointerLevelCounterCurrent == 3 && DocumentSectionCode == "02A")
            {
                myChild3Type = "SupplierInvioceItemsCertificate";
                myChild3Sequence = SequenceNumeric;
            }
        }

        private void HandleValidationCodeError(ResponseError errorItem)
        {
            switch (errorItem.ValidationCode.Value)
            {
                case "1501":
                    {
                        RaiseUnifreightEvent("MPOA", "MPOA", ""); //errorItem.ValidationCode.name
                        break;
                    }
                case "4589":
                    {
                        RaiseUnifreightEvent("MID", "MID", "");
                        break;
                    }
                case "2244":
                    {
                        RaiseUnifreightEvent("IDE", "IDE", "");
                        break;
                    }
            }

        }


        private void RaiseUnifreightEvent(string eventCode, string unifrieghtEvent, string eventRemarks)
        {
            try
            {
                string loggingUserId = AuthenticationUtil.ResolveUserId(_MyDeclarationPM.Tenant);

                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = _MyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = eventRemarks,
                    CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                    EntityId = _MyDeclarationPM.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "Event from logitude",
                    MyUnifreightEventParam = new UnifreightEventParam()
                    {
                        Code = unifrieghtEvent,
                        Mode = UnifreightEventMode.@new,
                        EventDateTime = DateTime.Now,
                        Entname = "CFIFILEM",
                        PrimaryNum = _MyDeclarationPM.CustomFileNo,
                        EventRemarks = eventRemarks,
                        EventUser = loggingUserId,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: eventCode = " + eventCode + " CustomFileNo= " + _MyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, true);

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private Entity FindEntityinList(string myChild1Type, string myChild1Sequence, string myChild2Type, string myChild2Sequence, string myChild3Type, string myChild3Sequence)
        {

            if (_declarationErrorPointer.Entitites.Count == 0)
            {
                return null;
            }

            List<Entity> entityError = (from a in _declarationErrorPointer.Entitites
                                       where (a.Child1Type == myChild1Type && a.Child1Sequence == myChild1Sequence
                                       && a.Child2Type == myChild2Type && a.Child2Sequence == myChild2Sequence
                                       && a.Child3Type == myChild3Type && a.Child3Sequence == myChild3Sequence)
                                       select a).ToList();

            if (entityError.Count > 0)
            {
                return entityError[0];
            }
            else
            {
                return null;
            }
        }

        internal string CreateBuisnessErrorPionter(IResponseHeaderOrFault responseExeption)
        {
            if (responseExeption == null)
            {
                return null;
            }

            _declarationErrorPointer = new DeclarationError();
            _declarationErrorPointer.Entitites = new List<Entity>();

            Entity myEntityErrorDetail = new Entity();
            myEntityErrorDetail.Child1Type = "";
            myEntityErrorDetail.Child1Sequence = "";
            myEntityErrorDetail.Child2Type = "";
            myEntityErrorDetail.Child2Sequence = "";
            myEntityErrorDetail.Child3Type = "";
            myEntityErrorDetail.Child3Sequence = "";

            var myEntityError = new error();
            myEntityError.Code = responseExeption.ErrorCode;
            myEntityError.ListVersionID = "1";
            myEntityError.MessageError = responseExeption.ErrorDescription;

            myEntityErrorDetail.EntityErrors.Add(myEntityError);

            var myDeclaretionErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(_declarationErrorPointer);
            return myDeclaretionErrorXml;
        }

        internal string AddDeclarationException(string errorXml, string errorType, UnifreightIIG.Common.ImportDeclarationServiceReference.Exception exception,bool errorUpsert=false)
        {
            if (exception == null)
            {
                return errorXml;
            }

            if (string.IsNullOrWhiteSpace(errorXml))
            {
                _declarationErrorPointer = new DeclarationError();
                _declarationErrorPointer.Entitites = new List<Entity>();
            }
            else
            {
                byte[] errorsByte = Encoding.UTF8.GetBytes(errorXml);
                MemoryStream memorystream = new MemoryStream(errorsByte);
                XmlSerializer serializer = new XmlSerializer(typeof(DeclarationError));
                _declarationErrorPointer = (DeclarationError)serializer.Deserialize(memorystream);
            }

            Entity myEntityErrorDetail = new Entity();
            Entity currentEntityErrorDetail = FindEntityinList("", "", "", "", "", "");
            if (currentEntityErrorDetail == null)
            {
                myEntityErrorDetail.Child1Type = "";
                myEntityErrorDetail.Child1Sequence = "";
                myEntityErrorDetail.Child2Type = "";
                myEntityErrorDetail.Child2Sequence = "";
                myEntityErrorDetail.Child3Type = "";
                myEntityErrorDetail.Child3Sequence = "";
            }
            else
            {
                myEntityErrorDetail = currentEntityErrorDetail;
                if (!errorUpsert)
                {
                    _declarationErrorPointer.Entitites.Remove(currentEntityErrorDetail);
                }
            }

            var myFieldError = new field();
            if (errorType == "Buisness")
            {
                myFieldError.Code = "Buisness";
                myFieldError.ListVersionID = "1";
            }
            else if (errorType == "Warning")
            {
                myFieldError.Code = "Warning";
                myFieldError.ListVersionID = "4";
            }

            myFieldError.MessageError = exception.ExeptionDescription;
            myEntityErrorDetail.FieldErrors = new List<field>();
            myEntityErrorDetail.FieldErrors.Add(myFieldError);

            _declarationErrorPointer.Entitites.Add(myEntityErrorDetail);

            var myDeclaretionErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(_declarationErrorPointer);
            return myDeclaretionErrorXml;
        }

        internal string AddManifestException(string errorXml, string errorType, UnifreightIIG.Common.MANIFESTRequestServiceReference.Exception exception, bool errorUpsert = false)
        {
            if (exception == null)
            {
                return errorXml;
            }

            if (string.IsNullOrWhiteSpace(errorXml))
            {
                _declarationErrorPointer = new DeclarationError();
                _declarationErrorPointer.Entitites = new List<Entity>();
            }
            else
            {
                byte[] errorsByte = Encoding.UTF8.GetBytes(errorXml);
                MemoryStream memorystream = new MemoryStream(errorsByte);
                XmlSerializer serializer = new XmlSerializer(typeof(DeclarationError));
                _declarationErrorPointer = (DeclarationError)serializer.Deserialize(memorystream);
            }

            Entity myEntityErrorDetail = new Entity();
            Entity currentEntityErrorDetail = FindEntityinList("", "", "", "", "", "");
            if (currentEntityErrorDetail == null)
            {
                myEntityErrorDetail.Child1Type = "";
                myEntityErrorDetail.Child1Sequence = "";
                myEntityErrorDetail.Child2Type = "";
                myEntityErrorDetail.Child2Sequence = "";
                myEntityErrorDetail.Child3Type = "";
                myEntityErrorDetail.Child3Sequence = "";
            }
            else
            {
                myEntityErrorDetail = currentEntityErrorDetail;
                if (!errorUpsert)
                {
                    _declarationErrorPointer.Entitites.Remove(currentEntityErrorDetail);
                }
            }

            var myFieldError = new field();
            if (errorType == "Buisness")
            {
                myFieldError.Code = "Buisness";
                myFieldError.ListVersionID = "1";
            }
            else if (errorType == "Warning")
            {
                myFieldError.Code = "Warning";
                myFieldError.ListVersionID = "4";
            }

            myFieldError.MessageError = exception.ExeptionDescription;
            myEntityErrorDetail.FieldErrors = new List<field>();
            myEntityErrorDetail.FieldErrors.Add(myFieldError);

            _declarationErrorPointer.Entitites.Add(myEntityErrorDetail);

            var myManifestErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(_declarationErrorPointer);
            return myManifestErrorXml;
        }

        internal string AddDeclarationExceptionList(string errorXml, string exceptionXml)
        {
            if (exceptionXml == null)
            {
                return errorXml;
            }

            if (errorXml == null)
            {
                return exceptionXml;
            }

            byte[] errorsByte = Encoding.UTF8.GetBytes(errorXml);
            MemoryStream memorystream = new MemoryStream(errorsByte);
            XmlSerializer serializer = new XmlSerializer(typeof(DeclarationError));
            _declarationErrorPointer = (DeclarationError)serializer.Deserialize(memorystream);

            byte[] errorsByteException = Encoding.UTF8.GetBytes(exceptionXml);
            MemoryStream memorystreamException = new MemoryStream(errorsByteException);
            XmlSerializer serializerException = new XmlSerializer(typeof(DeclarationError));
            DeclarationError declarationErrorPointer = (DeclarationError)serializerException.Deserialize(memorystreamException);

            var declarationErrorPointerArray = declarationErrorPointer.Entitites.ToArray();
            foreach (Entity entityError in declarationErrorPointerArray)
            {
                Entity myEntityErrorDetail = FindEntityinList(entityError.Child1Type, entityError.Child1Sequence, entityError.Child2Type, entityError.Child2Sequence, entityError.Child3Type, entityError.Child3Sequence);
                if (myEntityErrorDetail == null)
                {
                    _declarationErrorPointer.Entitites.Add(entityError);
                }
                else
                {
                    if (entityError.EntityErrors != null)
                    {
                        foreach (var error in entityError.EntityErrors)
                        {
                            myEntityErrorDetail.EntityErrors.Add(error);
                        }
                    }
                    if (entityError.FieldErrors != null)
                    {
                        foreach (var error in entityError.FieldErrors)
                        {
                            myEntityErrorDetail.FieldErrors.Add(error);
                        }
                    }
                }
            }

            var myDeclaretionErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(_declarationErrorPointer);
            return myDeclaretionErrorXml;
        }

        public string AddErrorPionter(DeclarationError declarationErrorPointer, string child1Type, string child1Sequence, string child2Type, string child2Sequence, string child3Type, string child3Sequence,
           string code, string listVersionID, string messageError, string fieldcode, string other1="", string other2="", string other3="")
        {
            bool isNewEntity = false;
            Entity myEntityErrorDetail = new Entity();

            _declarationErrorPointer = declarationErrorPointer;
            if (_declarationErrorPointer == null)
            {
                _declarationErrorPointer = new DeclarationError();
            }
            if (_declarationErrorPointer.Entitites == null)
            {
                _declarationErrorPointer.Entitites = new List<Entity>();
            }
            
            Entity currentEntityErrorDetail = FindEntityinList(child1Type, child1Sequence, child2Type, child2Sequence, child3Type, child3Sequence);
            if (currentEntityErrorDetail == null)
            {
                myEntityErrorDetail.Child1Type = child1Type;
                myEntityErrorDetail.Child1Sequence = child1Sequence;
                myEntityErrorDetail.Child2Type = child2Type;
                myEntityErrorDetail.Child2Sequence = child2Sequence;
                myEntityErrorDetail.Child3Type = child3Type;
                myEntityErrorDetail.Child3Sequence = child3Sequence;
                isNewEntity = true;
            }
            else
            {
                myEntityErrorDetail = currentEntityErrorDetail;
            }            

            var myFieldError = new field();
            myFieldError.Code = code;
            myFieldError.ListVersionID = listVersionID;
            myFieldError.MessageError = messageError;
            if (myEntityErrorDetail.FieldErrors == null)
            {
                myEntityErrorDetail.FieldErrors = new List<field>();
            }
            myEntityErrorDetail.FieldErrors.Add(myFieldError);
            if (isNewEntity == true)
            {
                _declarationErrorPointer.Entitites.Add(myEntityErrorDetail);
            }

            var myDeclaretionErrorXml = XmlGenericUtil<DeclarationError>.SerializeObject(_declarationErrorPointer);
            return myDeclaretionErrorXml;
        }
    }
}
