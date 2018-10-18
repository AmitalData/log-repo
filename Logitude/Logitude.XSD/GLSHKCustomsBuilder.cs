using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD
{
    public class GLSHKCustomsBuilder
    {
        public int Tenant { get; set; }
        public GLSHKCustomsContext Context { get; set; }
        public GLSHKCustomsBuilder(GLSHKCustomsContext myContext)
        {
            this.Context = myContext;
            this.Tenant = myContext.Tenant;
        }

        public GLSHK_ISAC.Message GetMessage()
        {
            GLSHK_ISAC.Message myMessage = new GLSHK_ISAC.Message()
            {
                version = GLSHK_ISAC.MessageVersion.Item40,

                Envelope = new GLSHK_ISAC.Envelope()
                {
                    SenderID = Context.PIMA,
                    RecipientID = "RHKCCS83GLSCMMS", //Context.Recipient,
                    RefID = Context.RefFullID,
                    MsgFormat = "XML",
                    MsgType = "CUSEXP",
                    MsgDateTime = TenantServerConfigration.GetCurrentDateTime(Context.Tenant),
                    Version = (decimal)4.1,
                    Password = "0",
                    Function = "APPEND",
                    MessageRefNum = Context.ShipmentNumberTrimmed,
                    InterchangeControlRef = Context.ShipmentNumberTrimmed,
                    Item = Context.Tenant.ToString(),
                    ItemElementName = GLSHK_ISAC.ItemChoiceType.CompanyID,
                }
            };

            if (Context.ManifestStatusCode == "NSEN")
            {
                myMessage.Envelope.Function = "APPEND";
            }

            else
            {
                myMessage.Envelope.Function = "REPLACE";
            }

            if (Context.IsViaColoader && !string.IsNullOrEmpty(Context.ColoaderKey))
            {
                myMessage.Envelope.Item = Context.ColoaderKey;
                myMessage.Envelope.ItemElementName = GLSHK_ISAC.ItemChoiceType.ColoaderKey;
            }

            #region MANIFEST_INFO
            myMessage.MANIFEST_INFO = new GLSHK_ISAC.MANIFEST_INFO()
            {
                booTDEC = 0,
                booCUSEXP = 1,

                Transport_Info = new GLSHK_ISAC.TRANSPORT_INFO()
                {
                    TransportID = "1",
                    Transport_Mode = "4",
                    TransportRefNo = Context.MainCarriageCarrierNumber,
                    Arrival_Departure_Date = Context.MainCarriageCarrierETDFormatted,

                    Origin = new GLSHK_ISAC.LOCATION_INFO()
                    {
                        Place = Context.MainCarriageFromPortCode,
                        ISO_Country_Code = Context.MainCarriageFromPortCountryCode,
                    },

                    Destination = new GLSHK_ISAC.LOCATION_INFO()
                    {
                        Place = Context.FinalDestinationPortCode,
                        ISO_Country_Code = Context.FinalDestinationPortCountryCode,
                    },
                },

                Master_Info = new GLSHK_ISAC.MANIFEST_INFOMaster_Info()
                {
                    AWB_Prefix = Context.AirlinePrefix,
                    AWB_Suffix = Context.Master,
                    Master_Bill_No = Context.AirlinePrefix + Context.Master,
                    Total_Pieces = Context.NumberOfPackages.ToString(),

                    Total_Weight = new GLSHK_ISAC.WEIGHT_INFO()
                    {
                        Value = Context.GrossWeight,
                        Code = Context.GrossWeightUnitCode == "KGM" ? GLSHK_ISAC.WEIGHT_INFOCode.KGM : GLSHK_ISAC.WEIGHT_INFOCode.LBR,
                    },

                    Agent_Code = Context.AgentCode,
                },
            };
            #endregion

            #region HOUSE_INFOHouse
            if (Context.AllHousesContext.Count > 0)
            {
                List<GLSHK_ISAC.HOUSE_INFOHouse_Manifest_Details> allHousesItems = new List<GLSHK_ISAC.HOUSE_INFOHouse_Manifest_Details>();

                foreach (HouseContext item in Context.AllHousesContext)
                {
                    allHousesItems.Add(GetHouseInfo(item));
                }

                myMessage.MANIFEST_INFO.House_Manifest_Info = allHousesItems.ToArray();
            }
            #endregion

            return myMessage;
        }

        private GLSHK_ISAC.HOUSE_INFOHouse_Manifest_Details GetHouseInfo(HouseContext myContext)
        {
            GLSHK_ISAC.HOUSE_INFOHouse_Manifest_Details myXSDElement = new GLSHK_ISAC.HOUSE_INFOHouse_Manifest_Details()
            {
                Serial_No = myContext.SerialNumber,
                House_Bill_No = myContext.House,
                Pieces = myContext.NumberOfPackages.ToString(),

                Gross_Weight = new GLSHK_ISAC.WEIGHT_INFO()
                {
                    Value = myContext.GrossWeight,
                    Code = myContext.GrossWeightUnitCode == "KGM" ? GLSHK_ISAC.WEIGHT_INFOCode.KGM : GLSHK_ISAC.WEIGHT_INFOCode.LBR,
                },

                Origin = new GLSHK_ISAC.LOCATION_INFO()
                {
                    Place = Context.MainCarriageFromPortCode,
                    ISO_Country_Code = Context.MainCarriageFromPortCountryCode,
                },

                Destination = new GLSHK_ISAC.LOCATION_INFO()
                {
                    Place = Context.FinalDestinationPortCode,
                    ISO_Country_Code = Context.FinalDestinationPortCountryCode,
                },
            };

            #region Shipper
            if (!string.IsNullOrEmpty(Context.Shipment.ShipperId))
            {
                myXSDElement.Shipper = new GLSHK_ISAC.SHIPPING_PARTY_INFO()
                {
                    Name = myContext.ShipperName,
                    Place = myContext.ShipperCity,
                    ISO_Country_Code = myContext.ShipperCountryCode,
                };

                if (!string.IsNullOrEmpty(myContext.ShipperZipCode))
                {
                    myXSDElement.Shipper.Post_Code = myContext.ShipperZipCode;
                }

                if (!string.IsNullOrEmpty(myContext.ShipperStateCode))
                {
                    myXSDElement.Shipper.State_Province = myContext.ShipperStateCode;
                }

                if (!string.IsNullOrEmpty(myContext.ShipperAddress1) || !string.IsNullOrEmpty(myContext.ShipperAddress2))
                {
                    List<GLSHK_ISAC.ADDRESS_DETAIL> myAddresses = new List<GLSHK_ISAC.ADDRESS_DETAIL>();

                    if (!string.IsNullOrEmpty(myContext.ShipperAddress1))
                    {
                        myAddresses.Add(new GLSHK_ISAC.ADDRESS_DETAIL() { Address = myContext.ShipperAddress1 });
                    }

                    if (!string.IsNullOrEmpty(myContext.ShipperAddress2))
                    {
                        myAddresses.Add(new GLSHK_ISAC.ADDRESS_DETAIL() { Address = myContext.ShipperAddress2 });
                    }

                    myXSDElement.Shipper.AddressInfo = myAddresses.ToArray();
                }

                if (!string.IsNullOrEmpty(myContext.ShipperPhone) || !string.IsNullOrEmpty(myContext.ShipperFax))
                {
                    List<GLSHK_ISAC.CONTACT_DETAIL> myContacts = new List<GLSHK_ISAC.CONTACT_DETAIL>();

                    if (!string.IsNullOrEmpty(myContext.ShipperPhone))
                    {
                        myContacts.Add(new GLSHK_ISAC.CONTACT_DETAIL() { Contact_Num = myContext.ShipperPhone, Contact_ID = GLSHK_ISAC.CONTACT_DETAILContact_ID.TE });
                    }

                    if (!string.IsNullOrEmpty(myContext.ShipperFax))
                    {
                        myContacts.Add(new GLSHK_ISAC.CONTACT_DETAIL() { Contact_Num = myContext.ShipperFax, Contact_ID = GLSHK_ISAC.CONTACT_DETAILContact_ID.FX });
                    }

                    myXSDElement.Shipper.ContactInfo = myContacts.ToArray();
                }
            }
            #endregion

            #region Consignee
            if (!string.IsNullOrEmpty(Context.Shipment.ConsigneeId))
            {
                myXSDElement.Consignee = new GLSHK_ISAC.SHIPPING_PARTY_INFO()
                {
                    Name = myContext.ConsigneeName,
                    Place = myContext.ConsigneeCity,
                    ISO_Country_Code = myContext.ConsigneeCountryCode,
                };

                if (!string.IsNullOrEmpty(myContext.ConsigneeZipCode))
                {
                    myXSDElement.Consignee.Post_Code = myContext.ConsigneeZipCode;
                }

                if (!string.IsNullOrEmpty(myContext.ConsigneeStateCode))
                {
                    myXSDElement.Consignee.State_Province = myContext.ConsigneeStateCode;
                }

                if (!string.IsNullOrEmpty(myContext.ConsigneeAddress1) || !string.IsNullOrEmpty(myContext.ConsigneeAddress2))
                {
                    List<GLSHK_ISAC.ADDRESS_DETAIL> myAddresses = new List<GLSHK_ISAC.ADDRESS_DETAIL>();

                    if (!string.IsNullOrEmpty(myContext.ConsigneeAddress1))
                    {
                        myAddresses.Add(new GLSHK_ISAC.ADDRESS_DETAIL() { Address = myContext.ConsigneeAddress1 });
                    }

                    if (!string.IsNullOrEmpty(myContext.ConsigneeAddress2))
                    {
                        myAddresses.Add(new GLSHK_ISAC.ADDRESS_DETAIL() { Address = myContext.ConsigneeAddress2 });
                    }

                    myXSDElement.Consignee.AddressInfo = myAddresses.ToArray();
                }

                if (!string.IsNullOrEmpty(myContext.ConsigneePhone) || !string.IsNullOrEmpty(myContext.ConsigneeFax))
                {
                    List<GLSHK_ISAC.CONTACT_DETAIL> myContacts = new List<GLSHK_ISAC.CONTACT_DETAIL>();

                    if (!string.IsNullOrEmpty(myContext.ConsigneePhone))
                    {
                        myContacts.Add(new GLSHK_ISAC.CONTACT_DETAIL() { Contact_Num = myContext.ConsigneePhone, Contact_ID = GLSHK_ISAC.CONTACT_DETAILContact_ID.TE });
                    }

                    if (!string.IsNullOrEmpty(myContext.ConsigneeFax))
                    {
                        myContacts.Add(new GLSHK_ISAC.CONTACT_DETAIL() { Contact_Num = myContext.ConsigneeFax, Contact_ID = GLSHK_ISAC.CONTACT_DETAILContact_ID.FX });
                    }

                    myXSDElement.Consignee.ContactInfo = myContacts.ToArray();
                }
            }
            #endregion

            #region Charge_Declarations_Info
            myXSDElement.Charge_Declarations_Info = new GLSHK_ISAC.CHARGE_DECLARATIONS_INFO()
            {
                Currency_Code = myContext.AWBCurrencyCode,
            };

            if (!string.IsNullOrEmpty(myContext.AWBChargeCode))
            {
                myXSDElement.Charge_Declarations_Info.Charge_Code = myContext.AWBChargeCode;
            }

            if (!string.IsNullOrEmpty(myContext.FreightPrepaidCollectId) || !string.IsNullOrEmpty(myContext.OtherPrepaidCollectId))
            {
                GLSHK_ISAC.DECLARED_PREPAID_COLLECT myPrepaidCollect = new GLSHK_ISAC.DECLARED_PREPAID_COLLECT();

                if (!string.IsNullOrEmpty(myContext.FreightPrepaidCollectId))
                {
                    myPrepaidCollect.Item = myContext.FreightPrepaidCollectId == "P" ? GLSHK_ISAC.DECLARED_PREPAID_COLLECTWeight_Value.P : GLSHK_ISAC.DECLARED_PREPAID_COLLECTWeight_Value.C;
                }

                if (!string.IsNullOrEmpty(myContext.OtherPrepaidCollectId))
                {
                    myPrepaidCollect.Other_ChargeSpecified = true;
                    myPrepaidCollect.Other_Charge = myContext.OtherPrepaidCollectId == "P" ? GLSHK_ISAC.DECLARED_PREPAID_COLLECTOther_Charge.P : GLSHK_ISAC.DECLARED_PREPAID_COLLECTOther_Charge.C;
                }

                myXSDElement.Charge_Declarations_Info.Prepaid_Collect_Declarations = myPrepaidCollect;
            }

            if (myContext.IsChargeCarriageDeclared)
            {
                myXSDElement.Charge_Declarations_Info.Declared_Value_Carriage = new GLSHK_ISAC.DECLARED_VALUE_INFO()
                {
                    Value = myContext.ChargeCarriageValue,
                };
            }

            if (myContext.IsChargeCustomsDeclared)
            {
                myXSDElement.Charge_Declarations_Info.Declared_Value_Customs = new GLSHK_ISAC.DECLARED_VALUE_INFO()
                {
                    Value = myContext.ChargeCustomsValue
                };
            }

            if (myContext.IsChargeInsurrenceDeclared)
            {
                myXSDElement.Charge_Declarations_Info.Declared_Value_Insurance = new GLSHK_ISAC.DECLARED_VALUE_INFO()
                {
                    Value = myContext.ChargeInsurrenceValue
                };
            }
            #endregion

            #region Manifest_Description
            if (!string.IsNullOrEmpty(myContext.ManifestDescription))
            {
                myXSDElement.Manifest_Description = myContext.ManifestDescription;
            }
            #endregion

            #region Special Handling Info
            if (myContext.HandlingCodesList != null && myContext.HandlingCodesList.Count > 0)
            {
                List<GLSHK_ISAC.SPECIAL_HANDLING_DETAIL> list = new List<GLSHK_ISAC.SPECIAL_HANDLING_DETAIL>();

                foreach (string item in myContext.HandlingCodesList)
                {
                    list.Add(new GLSHK_ISAC.SPECIAL_HANDLING_DETAIL()
                    {
                        Special_Handling_Code = item,
                    });
                }

                myXSDElement.SpecialHandlingInfo = list.ToArray();
            }
            #endregion

            #region FreeText DescriptionOfGoods
            if (myContext.DescriptionOfGoodsList != null && myContext.DescriptionOfGoodsList.Count > 0)
            {
                List<GLSHK_ISAC.FREETEXT_DOG_DETAIL> list = new List<GLSHK_ISAC.FREETEXT_DOG_DETAIL>();

                foreach (string item in myContext.DescriptionOfGoodsList)
                {
                    list.Add(new GLSHK_ISAC.FREETEXT_DOG_DETAIL() { DescriptionOfGoods = item });
                }

                myXSDElement.FreeTextDescriptionOfGoods = list.ToArray();
            }
            #endregion

            return myXSDElement;
        }
    }
}
