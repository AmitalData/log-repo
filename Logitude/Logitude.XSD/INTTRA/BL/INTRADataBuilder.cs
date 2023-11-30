using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.INTTRA.BL
{
    public class INTTRADataBuilder
    {
        public INTTRADataContext Context { get; set; }
        public INTTRADataBuilder(INTTRADataContext myContext)
        {
            this.Context = myContext;
        }
        public INTTRA_Out.Header GetHeader()
        {
            // shipment#-tenant-datetime-Communication log Id
            // OE1233-42-201804161230-1-3493

            string myDocumentIdentifier = this.Context.ShipmentNumber + "-" + this.Context.Tenant + "-" + this.Context.XMLCreateDate + "-" + this.Context.CommunicationLogIdCounter;

            INTTRA_Out.Header myResult = new INTTRA_Out.Header()
            {
                MessageType = new INTTRA_Out.MessageType()
                {
                    MessageVersion = 1,
                    //Value = "ShippingInstruction", (_OLD32)
                    Value = INTTRA_Out.MessageTypeValues.ShippingInstruction,
                },
                DocumentIdentifier = this.Context.iNTTRAGeneralMethods.FormatString(myDocumentIdentifier, 35),
                DateTime = new INTTRA_Out.DateTime()
                {
                    DateType = INTTRA_Out.DateTimeDateType.Document,
                    Value = this.Context.XMLCreateDate,
                },

                Parties = this.Context.MessageHeaderParties.ToArray<INTTRA_Out.PartnerInformation>(),
            };

            return myResult;
        }
        public INTTRA_Out.MessageProperties GetMessageProperties()
        {
            INTTRA_Out.MessageProperties myResult = new INTTRA_Out.MessageProperties()
            {
                ShipmentID = new INTTRA_Out.ShipmentID()
                {
                    ShipmentIdentifier = new INTTRA_Out.ShipmentIdentifier()
                    {
                        MessageStatus = INTTRA_Out.ShipmentIdentifierMessageStatus.Original,
                        Value = this.Context.ShipmentNumber,
                        VGMProcessingIndicator = INTTRA_Out.ShipmentIdentifierVGMProcessingIndicator.ShippingInstruction,
                    },

                    DocumentVersion = 000001,
                },

                DateTime = new INTTRA_Out.DateTime()
                {
                    DateType = INTTRA_Out.DateTimeDateType.Message,
                    Value = this.Context.XMLCreateDate_Long,
                },

                ChargeCategory = this.Context.ChargeCategories.ToArray<INTTRA_Out.ChargeCategory>(),

                BlLocations = this.Context.BlLocations.ToArray<INTTRA_Out.Location>(),

                ReferenceInformation = this.Context.ReferenceInformations.ToArray<INTTRA_Out.ReferenceInformation>(),

                HaulageDetails = this.Context.HaulageDetails,

                TransportationDetails = this.Context.TransportationDetails,

                Parties = this.Context.MessagePropertiesParties.ToArray<INTTRA_Out.PartnerInformation>(),
            };

            if(this.Context.ShipmentIndicator != null)
            {
                myResult.ShipmentIndicator = this.Context.ShipmentIndicator;
            }

            if(this.Context.HeaderCustomsInformation != null)
            {
                myResult.HeaderCustomsInformation = this.Context.HeaderCustomsInformation.ToArray<INTTRA_Out.HeaderCustomsFilerInstruction>();
            }

            if (this.Context.Shipment.ValueOfGoodsCurrencyId != null)
            {
                Currency myCurrency = (from d in this.Context.CommonContext.Currencies where d.Id == this.Context.Shipment.ValueOfGoodsCurrencyId select d).FirstOrDefault();
                if (myCurrency != null)
                {
                    double? valueOfGoods = 0;

                    if(this.Context.Shipment.ValueOfGoods != null)
                    {
                        valueOfGoods = MethodHelper.Round(this.Context.Shipment.ValueOfGoods, 2);
                    }

                    myResult.ShipmentDeclaredValue = new INTTRA_Out.ShipmentDeclaredValue()
                    {
                        Currency = myCurrency.Code,
                        Value = (float)valueOfGoods,
                    };
                }
            }

            if (this.Context.Instructions.Count > 0) {
                myResult.Instructions = this.Context.Instructions.ToArray<INTTRA_Out.ShipmentComments>();
            }

            return myResult;
        }
        public INTTRA_Out.MessageDetails GetMessageDetails()
        {
            INTTRA_Out.MessageDetails myResult = new INTTRA_Out.MessageDetails()
            {
                GoodsDetails = this.Context.GoodsDetails.ToArray<INTTRA_Out.GoodsDetails>(),
                EquipmentDetails = this.Context.EquipmentDetails.ToArray<INTTRA_Out.EquipmentDetails>(),                 
            };

            return myResult;
        }
    }
}
