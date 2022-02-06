using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for AWBLabelsWebSerivce
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AWBLabelsWebSerivce : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] GetAWBLabelsData(string shipmentId, int tenant, string documentTypeId)
        {
            List<AWBLabelsDataProvider> awbLabelsList = GetAWBLabelsDataProvider(shipmentId, tenant, documentTypeId);
            XmlSerializer serializer = new XmlSerializer(typeof(List<AWBLabelsDataProvider>));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, awbLabelsList);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public List<AWBLabelsDataProvider> GetAWBLabelsDataProvider(string shipmentId, int tenant,string documentTypeId)
        {
            AWBLabelsDataProvider myDataProvider = new AWBLabelsDataProvider();
            List<AWBLabelsDataProvider> myResult = new List<AWBLabelsDataProvider>();

            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            AddressRepository addressRepository = new AddressRepository(tenant);
            ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(shipmentId, tenant);

            if (shipmentPM != null)
            {
                myDataProvider.ShipmentNumber = shipmentPM.ShipmentNumber;
                myDataProvider.MAWBFull = shipmentPM.Master != null ? shipmentPM.Master : "";
                myDataProvider.HAWBFull = shipmentPM.House != null ? shipmentPM.House : "";
                myDataProvider.BookingNumber = shipmentPM.BookingConfirmationNumber != null ? shipmentPM.BookingConfirmationNumber : "";
                myDataProvider.FlightNumber = shipmentPM.MainCarriageCarrierNumber != null ? shipmentPM.MainCarriageCarrierNumber : "";

                if (!string.IsNullOrEmpty(shipmentPM.LongMaster))
                {
                    string str = shipmentPM.LongMaster.Replace("-", "");
                    myDataProvider.ModifiedFullMAWB = str;
                }

                myDataProvider.HouseNumber = shipmentPM.House != null ? shipmentPM.House : "";
                myDataProvider.AirlineLogo = DataProviders.General.GetCarrierLogo(shipmentPM.MainCarriageCarrierId, tenant);

                #region Amounts
                if (shipmentPM.ChargeableWeight != null)
                {
                    myDataProvider.ChargeableWeight = String.Format("{0:#,0.00}", shipmentPM.ChargeableWeight.Value);
                }

                if (shipmentPM.ChargeableWeightUnitCode != null)
                {
                    myDataProvider.ChargeableWeight = myDataProvider.ChargeableWeight + " " + shipmentPM.ChargeableWeightUnitCode;
                }

                if (shipmentPM.NumberOfPackages != null)
                {
                    myDataProvider.TotalQuantity = shipmentPM.NumberOfPackages.ToString();
                }
                #endregion

                #region Shipper & Consignee
                myDataProvider.ShipperName = shipmentPM.ShipperName != null ? shipmentPM.ShipperName : "";
                myDataProvider.ConsigneeName = shipmentPM.ConsigneeName != null ? shipmentPM.ConsigneeName : "";

                if (!string.IsNullOrEmpty(shipmentPM.ShipperId))
                {
                    Card myCard = CardRepository.GetSingleCard(shipmentPM.ShipperId, tenant, true);
                    if (myCard != null)
                    {
                        Address myAddress = addressRepository.GetMainAddressByCardId(shipmentPM.ShipperId, tenant);
                        if (myAddress != null)
                        {
                            myDataProvider.ShipperAddress = myCard.EnglishName + Environment.NewLine + DataProviders.General.GetAddress(myAddress);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(shipmentPM.ConsigneeId))
                {
                    Card myCard = CardRepository.GetSingleCard(shipmentPM.ConsigneeId, tenant, true);
                    if (myCard != null)
                    {
                        Address myAddress = addressRepository.GetMainAddressByCardId(shipmentPM.ConsigneeId, tenant);
                        if (myAddress != null)
                        {
                            myDataProvider.ConsigneeAddress = myCard.EnglishName + Environment.NewLine + DataProviders.General.GetAddress(myAddress);
                            myDataProvider.ConsigneePhoneNumber = myAddress.PhoneNumber;
                        }
                    }
                }
                #endregion

                #region DocumentType
                DocumentType currentdocumentType = commonContext.DocumentTypes.Where(doc => doc.Id == documentTypeId).FirstOrDefault();
                if (currentdocumentType != null)
                {
                    List<FormCustomField> customfieldsList = commonContext.FormCustomFields.Where(fc => fc.DocumentTypeId == currentdocumentType.Id).ToList();

                    List<DocumentTypeCustomField> documentCustomfieldsList = commonContext.DocumentTypeCustomFields.Where(fc => fc.DocumentTypeId == currentdocumentType.Id).ToList();

                    //AdditionalInformation
                    FormCustomField additionalInformationCustomField = (from a in customfieldsList
                                                                        where a.FieldCode == "AdditionalInformation" && a.EntityId == shipmentPM.Id
                                                                        select a).FirstOrDefault();

                    DocumentTypeCustomField additionalInformationDocumentCustom = (from a in documentCustomfieldsList
                                                                                   where a.FieldCode == "AdditionalInformation"
                                                                                   select a).FirstOrDefault();

                    // NumberOfLabels
                    FormCustomField numberOfLabelsCustomField = (from a in customfieldsList
                                                                 where a.FieldCode == "NumberOfLabels" && a.EntityId == shipmentPM.Id
                                                                 select a).FirstOrDefault();

                    DocumentTypeCustomField numberOfLabelsnDocumentCustom = (from a in documentCustomfieldsList
                                                                             where a.FieldCode == "NumberOfLabels"
                                                                             select a).FirstOrDefault();

                    //Contents
                    FormCustomField contentsCustomField = (from a in customfieldsList
                                                                        where a.FieldCode == "Contents" && a.EntityId == shipmentPM.Id
                                                                        select a).FirstOrDefault();

                    DocumentTypeCustomField contentsDocumentCustom = (from a in documentCustomfieldsList
                                                                                   where a.FieldCode == "Contents"
                                                                                   select a).FirstOrDefault();

                    myDataProvider.AdditionalInformation = additionalInformationCustomField != null ? additionalInformationCustomField.Value : (additionalInformationDocumentCustom != null ? additionalInformationDocumentCustom.DefaultValue : "");
                    myDataProvider.NumberOfLabels = numberOfLabelsCustomField != null ? numberOfLabelsCustomField.Value : (numberOfLabelsnDocumentCustom != null ? numberOfLabelsnDocumentCustom.DefaultValue : "");
                    myDataProvider.Contents = contentsCustomField != null ? contentsCustomField.Value : (contentsDocumentCustom != null ? contentsDocumentCustom.DefaultValue : "");
                }
                #endregion

                #region User Name 
                this.GetLoggedContactData(myDataProvider, tenant);
                #endregion 

                this.GetPortsData(myDataProvider, shipmentPM, commonContext);
                this.GetCarriersData(myDataProvider, shipmentPM, commonContext);
            }

            try
            {
                Type awbLabelDpType = myDataProvider.GetType();
                PropertyInfo[] properties = awbLabelDpType.GetProperties();
                foreach (PropertyInfo pi in properties)
                {

                    if (pi.GetValue(myDataProvider, null) == null || pi.GetValue(myDataProvider, null).ToString() == "0" || pi.GetValue(myDataProvider, null).ToString() == "00.00")
                    {
                        pi.SetValue(myDataProvider, "", null);
                    }
                }
            }

            catch
            {

            }

            if (shipmentPM != null)
            {
                int quantity = 0;
                if (!string.IsNullOrEmpty(myDataProvider.NumberOfLabels))
                {
                    quantity = Convert.ToInt32(myDataProvider.NumberOfLabels);
                }

                else
                {
                    quantity = string.IsNullOrEmpty(myDataProvider.TotalQuantity) ? 0 : Convert.ToInt32(myDataProvider.TotalQuantity);
                }

                for (int counter = 1; counter <= quantity; counter++)
                {
                    AWBLabelsDataProvider newlabel = new AWBLabelsDataProvider();
                    newlabel.AdditionalInformation = myDataProvider.AdditionalInformation;
                    newlabel.NumberOfLabels = myDataProvider.NumberOfLabels;
                    newlabel.Contents = myDataProvider.Contents;

                    newlabel.ChargeableWeight = myDataProvider.ChargeableWeight;
                    newlabel.MainCarriageCarrierCode = myDataProvider.MainCarriageCarrierCode;
                    newlabel.MainCarriageCarrierName = myDataProvider.MainCarriageCarrierName;
                    newlabel.MainCarriageFromPortCode = myDataProvider.MainCarriageFromPortCode;
                    newlabel.MainCarriageToPortCode = myDataProvider.MainCarriageToPortCode;
                    newlabel.MAWBFull = myDataProvider.MAWBFull;
                    newlabel.ShipmentNumber = myDataProvider.ShipmentNumber;
                    newlabel.HAWBFull = myDataProvider.HAWBFull;
                    newlabel.PieceNumber = myDataProvider.PieceNumber;
                    newlabel.PieceNumberString = myDataProvider.PieceNumberString;
                    newlabel.TotalQuantity = myDataProvider.TotalQuantity;
                    newlabel.Transshipment1ToPortCode = myDataProvider.Transshipment1ToPortCode;
                    newlabel.Transshipment2ToPortCode = myDataProvider.Transshipment2ToPortCode;
                    newlabel.BookingNumber = myDataProvider.BookingNumber;
                    newlabel.ShipperName = myDataProvider.ShipperName;
                    newlabel.ShipperAddress = myDataProvider.ShipperAddress;
                    newlabel.ConsigneeName = myDataProvider.ConsigneeName;
                    newlabel.ConsigneeAddress = myDataProvider.ConsigneeAddress;
                    newlabel.BarCode = myDataProvider.BarCode + String.Format("{0:00000}", counter);
                    newlabel.HouseBarCode = myDataProvider.HouseBarCode + String.Format("{0:00000}", counter);
                    newlabel.ModifiedFullMAWB = myDataProvider.ModifiedFullMAWB;
                    newlabel.HouseNumber = myDataProvider.HouseNumber;
                    newlabel.UserName = myDataProvider.UserName;
                    newlabel.ConsigneePhoneNumber = myDataProvider.ConsigneePhoneNumber;
                    newlabel.AirlineLogo = myDataProvider.AirlineLogo;
                    newlabel.FlightNumber = myDataProvider.FlightNumber;

                    CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipmentPM, newlabel);

                    newlabel.PieceNumber = counter.ToString();
                    newlabel.PieceNumberString = counter.ToString() + " / " + (newlabel.TotalQuantity == null ? "" : newlabel.TotalQuantity);
                    newlabel.OneDigitPieceNumber = counter;
                    myResult.Add(newlabel);
                }
            }

            return myResult;
        }

        private void GetLoggedContactData(AWBLabelsDataProvider myDataProvider, int tenant)
        {
            string contactEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);
            if (!string.IsNullOrEmpty(contactEmail))
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contactPM = contactQuery.GetContactByEmailOnly(contactEmail, tenant);

                if (contactPM != null)
                {
                    myDataProvider.UserName = contactPM.EnglishName;
                }
            }
        }

        private void GetPortsData(AWBLabelsDataProvider myDataProvider, ShipmentPM shipmentPM, ICommonDataContext commonContext)
        {

            Port mainCarriageFromPort = (from a in commonContext.Ports where a.Id == shipmentPM.MainCarriageFromPortId select a).FirstOrDefault();
            Port mainCarriageToPort = (from a in commonContext.Ports where a.Id == shipmentPM.MainCarriageToPortId select a).FirstOrDefault();
            Port transshipment1ToPort = (from a in commonContext.Ports where a.Id == shipmentPM.Transshipment1ToPortId select a).FirstOrDefault();
            Port transshipment1FromPort = (from a in commonContext.Ports where a.Id == shipmentPM.Transshipment1FromPortId select a).FirstOrDefault();
            Port transshipment2ToPort = (from a in commonContext.Ports where a.Id == shipmentPM.Transshipment2ToPortId select a).FirstOrDefault();
            Port transshipment2FromPort = (from a in commonContext.Ports where a.Id == shipmentPM.Transshipment2FromPortId select a).FirstOrDefault();
            Port transshipment3ToPort = (from a in commonContext.Ports where a.Id == shipmentPM.Transshipment3ToPortId select a).FirstOrDefault();

            if (mainCarriageFromPort != null)
            {
                myDataProvider.MainCarriageFromPortCode = mainCarriageFromPort.Code;
            }

            if (transshipment3ToPort != null)
            {
                myDataProvider.MainCarriageToPortCode = transshipment3ToPort.Code;
            }

            else if (transshipment2ToPort != null)
            {
                myDataProvider.MainCarriageToPortCode = transshipment2ToPort.Code;
            }

            else if (transshipment1ToPort != null)
            {
                myDataProvider.MainCarriageToPortCode = transshipment1ToPort.Code;
            }

            else if (mainCarriageToPort != null)
            {
                myDataProvider.MainCarriageToPortCode = mainCarriageToPort.Code;
            }

            if (transshipment1FromPort != null)
            {
                myDataProvider.Transshipment1ToPortCode = transshipment1FromPort.Code;
            }

            if (transshipment2FromPort != null)
            {
                myDataProvider.Transshipment2ToPortCode = transshipment2FromPort.Code;
            }            
        }

        private void GetCarriersData(AWBLabelsDataProvider myDataProvider, ShipmentPM shipmentPM, ICommonDataContext commonContext)
        {          
            int tenant = shipmentPM.Tenant;
            string mainCarrierPrefix = string.IsNullOrEmpty(shipmentPM.MainCarriageCarrierPrefix) ? "" : shipmentPM.MainCarriageCarrierPrefix.Trim();

            if (!string.IsNullOrEmpty(mainCarrierPrefix))
            {
                myDataProvider.MainCarriageCarrierCode = mainCarrierPrefix.ToUpper();
            }

            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageCarrierId))
            {
                Card mainCarrier = CardRepository.GetSingleCard(shipmentPM.MainCarriageCarrierId, tenant, false);
                if (mainCarrier != null)
                {
                    if (string.IsNullOrEmpty(myDataProvider.MainCarriageCarrierCode))
                    {
                        myDataProvider.MainCarriageCarrierCode = mainCarrier.Code;
                    }

                    myDataProvider.MainCarriageCarrierName = mainCarrier.EnglishName;

                    myDataProvider.MAWBFull = shipmentPM.LongMaster;
                    myDataProvider.AirlinePrefix = shipmentPM.AirlinePrefix;
                }
            }

            string myBarCode = "";
            string myHouseBarCode = "";

            if (!string.IsNullOrEmpty(myDataProvider.AirlinePrefix))
            {
                myBarCode += myDataProvider.AirlinePrefix;
                myHouseBarCode += myDataProvider.AirlinePrefix;
            }

            if (!string.IsNullOrEmpty(shipmentPM.Master))
            {
                myBarCode += shipmentPM.Master;
            }

            if (!string.IsNullOrEmpty(shipmentPM.House))
            {
                myHouseBarCode = shipmentPM.House;
            }

            myDataProvider.BarCode = myBarCode;
            myDataProvider.HouseBarCode = myHouseBarCode;
        }
    }
}
