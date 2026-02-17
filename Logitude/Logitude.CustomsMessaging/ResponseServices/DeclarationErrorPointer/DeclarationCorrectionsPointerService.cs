using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationCorrectionPointer;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.ImportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer
{
    public class DeclarationCorrectionsPointerService
    {
        General _DeclarationCorrection = null;

        public string AnalyzeCorrectionsPointer(string currentCorrectionXml, Response response, List<error> systemMessagesList, int tenant)
        {
            DeclarationCorrection declarationCorrectionXml = new DeclarationCorrection();
            if (response == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(currentCorrectionXml))
            {

                declarationCorrectionXml.GeneralData = new List<General>();
            }
            else
            {
                byte[] errorsByte = Encoding.UTF8.GetBytes(currentCorrectionXml);
                MemoryStream memorystream = new MemoryStream(errorsByte);
                XmlSerializer serializer = new XmlSerializer(typeof(DeclarationCorrection));
                declarationCorrectionXml = (DeclarationCorrection)serializer.Deserialize(memorystream);
            }

            //Get General Data
            _DeclarationCorrection = new General();
            _DeclarationCorrection.IssueDateTime = response.IssueDateTime;
            if (response.Declaration.DMExtensions != null && response.Declaration.DMExtensions.VersionID != null)
            {
                _DeclarationCorrection.VersionId = response.Declaration.DMExtensions.VersionID.Value;
            }

            if (response.AdditionalInformation != null)
            {
                _DeclarationCorrection.AdditionalInformation = new List<Additional>();
                foreach (var additionalInformationItem in response.AdditionalInformation)
                {
                    //Analyze AdditionalInformation
                    GetCorrectionAdditionalInformation(additionalInformationItem, tenant);
                }
            }

            if (response.Amendment != null)
            {
                _DeclarationCorrection.Amendments = new List<Entity>();
                foreach (var amendmentItem in response.Amendment)
                {
                    //Analyze Amendment pointers
                    GetCorrectionAmendment(amendmentItem);
                }
            }

            if (systemMessagesList != null && systemMessagesList.Count > 0)
            {
                _DeclarationCorrection.SystemMessages = systemMessagesList;
            }

            declarationCorrectionXml.GeneralData.Add(_DeclarationCorrection);

            var myDeclaretionCorrectionXml = XmlGenericUtil<DeclarationCorrection>.SerializeObject(declarationCorrectionXml);
            return myDeclaretionCorrectionXml;
        }

        private void GetCorrectionAdditionalInformation(ResponseAdditionalInformation additionalInformationItem, int tenant)
        {
            Additional additionalInfo = new Additional();
            if (additionalInformationItem.StatementTypeCode != null)
            {
                additionalInfo.StatementTypeCode = additionalInformationItem.StatementTypeCode.Value;
                DeclarationStatementTypeQueryService statementTypeQueryService = new DeclarationStatementTypeQueryService(tenant);
                DeclarationStatementTypePM statementTypePM = statementTypeQueryService.GetSingle(additionalInformationItem.StatementTypeCode.Value, false, true);
                if (statementTypePM != null)
                {
                    additionalInfo.StatementName = statementTypePM.LocalName;
                }
            }

            additionalInfo.Content = GetInformationContent(additionalInformationItem.StatementTypeCode.Value, additionalInformationItem.Content.Value, tenant);
            _DeclarationCorrection.AdditionalInformation.Add(additionalInfo);
        }

        private string GetInformationContent(string statementTypeCode, string content, int tenant)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return "";
            }

            if (content == "True")
            {
                return"כן";
            }

            if (content == "False")
            {
                return "לא";
            }

            if (statementTypeCode == "32")
            {
                AmendmentRequestStatusQueryService amendmentRequestStatusQueryService = new AmendmentRequestStatusQueryService(tenant);
                AmendmentRequestStatusPM amendmentRequestStatuPM = amendmentRequestStatusQueryService.GetSingle(content, false, true);
                if (amendmentRequestStatuPM != null)
                {
                    return amendmentRequestStatuPM.LocalName;
                }
            }
            return content;
        }

        private void GetCorrectionAmendment(ResponseAmendment amendmentItem)
        {
            var myDB = WCO.Instance.CreateDB();
            int pointerLevelCounter = 0;
            WCOErrorPointerModel myCargoDescription = new WCOErrorPointerModel();
            WCOErrorPointerModel.LogitudeEntityEnum lastLogitudeEntity = WCOErrorPointerModel.LogitudeEntityEnum.None ;
            string[] MessageErrorArray = null;

            string myChild1Type = "";
            string myChild1Sequence = "";
            string myChild2Type = "";
            string myChild2Sequence = "";
            string myChild3Type = "";
            string myChild3Sequence = "";
            bool isNewEntity = false;

            if (amendmentItem.Pointer != null)
            {
                foreach (var pointerItem in amendmentItem.Pointer)
                {
                    //string  NaturalKey = "";
                    string SequenceNumeric = "0";

                    myCargoDescription = WCO.Instance.CreateDB().GetTagID(pointerLevelCounter, pointerItem.DocumentSectionCode.Value);
                    myDB = myDB.GetNode(pointerLevelCounter++, pointerItem.DocumentSectionCode.Value);
                    //lastLogitudeEntity = myCargoDescription.LogitudeEntity;

                    if (pointerItem.TagID != null && !string.IsNullOrEmpty(pointerItem.TagID.Value))
                    {
                        myCargoDescription = myDB.GetTagID(pointerLevelCounter, pointerItem.TagID.Value);
                        bool isContaineLetters = pointerItem.TagID.Value.Any(x => char.IsLetter(x));
                        if (myCargoDescription == null && isContaineLetters == true)
                        {
                            string[] splitWords = pointerItem.TagID.Value.Split('[');
                            string tagId = splitWords[0];
                            myCargoDescription = myDB.GetTagID(pointerLevelCounter, tagId);
                        }
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

            if (!string.IsNullOrWhiteSpace(amendmentItem.ChangeReasonCode.name))
            {
                MessageErrorArray = amendmentItem.ChangeReasonCode.name.Split('|');
            }

            if (myCargoDescription.LogitudeFieldID == null || myCargoDescription.LogitudeFieldID == "")
            {
                //Add New Entity Error
                var myEntityError = new error();
                myEntityError.Code = amendmentItem.ChangeReasonCode.Value;
                myEntityError.ListVersionID = amendmentItem.ChangeReasonCode.listVersionID;
                if (MessageErrorArray != null && MessageErrorArray.Count() > 0)
                {
                    myEntityError.MessageError = MessageErrorArray[0];
                    if (MessageErrorArray.Count() > 1)
                    {
                        myEntityError.OldValue = MessageErrorArray[1];
                        myEntityError.NewValue = MessageErrorArray[2];
                    }
                }
                else
                {
                    myEntityError.MessageError = amendmentItem.ChangeReasonCode.name;
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
                myFieldError.Code = amendmentItem.ChangeReasonCode.Value;
                myFieldError.ListVersionID = amendmentItem.ChangeReasonCode.listVersionID;
                if (MessageErrorArray != null && MessageErrorArray.Count() > 0)
                {
                    myFieldError.MessageError = MessageErrorArray[0];
                    if (MessageErrorArray.Count() > 1)
                    {
                        myFieldError.OldValue = MessageErrorArray[1];
                        myFieldError.NewValue = MessageErrorArray[2];
                    }
                }
                else
                {
                    myFieldError.MessageError = amendmentItem.ChangeReasonCode.name;
                }

                myFieldError.Fieldcode = myCargoDescription.LogitudeFieldID;

                if (myEntityErrorDetail.FieldErrors == null)
                {
                    myEntityErrorDetail.FieldErrors = new List<field>();
                }
                myEntityErrorDetail.FieldErrors.Add(myFieldError);
            }

            if (isNewEntity == true)
            {
                _DeclarationCorrection.Amendments.Add(myEntityErrorDetail);
            }
        }

        private Entity FindEntityinList(string myChild1Type, string myChild1Sequence, string myChild2Type, string myChild2Sequence, string myChild3Type, string myChild3Sequence)
        {

            if (_DeclarationCorrection.Amendments.Count == 0)
            {
                return null;
            }

            List<Entity> entityError = (from a in _DeclarationCorrection.Amendments
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

        public List<string> GetVersionIdFromCorrectionXML(string correctionXml)
        {
            if (correctionXml == null)
            {
                return null;
            }

            byte[] errorsByte = Encoding.UTF8.GetBytes(correctionXml);
            MemoryStream memorystream = new MemoryStream(errorsByte);
            XmlSerializer serializer = new XmlSerializer(typeof(DeclarationCorrection));
            DeclarationCorrection declarationCorrection = (DeclarationCorrection)serializer.Deserialize(memorystream);

            if (declarationCorrection == null || declarationCorrection.GeneralData == null)
            {
                return null;
            }

            List<string> versionList = new List<string>();
            foreach (var correctionItem in declarationCorrection.GeneralData)
            {
                versionList.Add(correctionItem.VersionId);
            }
            return versionList;
        }

    }
}
