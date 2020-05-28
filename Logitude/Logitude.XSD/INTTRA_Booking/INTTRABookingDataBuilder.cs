using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.INTTRA_Booking
{
    public class INTTRABookingDataBuilder
    {
        public INTRABookingContext Context { get; set; }

        public INTTRABookingDataBuilder(INTRABookingContext myContext)
        {
            this.Context = myContext;
        }

        public INTTRA_Booking.HeaderType GetHeader()
        {
            string myDocumentIdentifier = "B-" + this.Context.ShipmentNumber + "-" + this.Context.Tenant + "-" + this.Context.XMLCreateDate + "-" + this.Context.CommunicationLogIdCounter;
            var transactionStatus = HeaderTypeTransactionStatus.Original;
            if (this.Context.Shipment.INTTRABookingTransStatusCode != "NST")
            {
                transactionStatus = HeaderTypeTransactionStatus.Change;
            }

            INTTRA_Booking.HeaderType myResult = new INTTRA_Booking.HeaderType
            {
                SenderId = "LOGITUDE",
                ReceiverId = "INTTRA",
                RequestDateTimeStamp = new DateTime(this.Context.TodayDateTime.Year, this.Context.TodayDateTime.Month, this.Context.TodayDateTime.Day, this.Context.TodayDateTime.Hour, this.Context.TodayDateTime.Minute, this.Context.TodayDateTime.Second),
                RequestMessageVersion = HeaderTypeRequestMessageVersion.Item10,
                TransactionType = TransactionTypeValues.Booking,
                TransactionVersion = HeaderTypeTransactionVersion.Item20,
                DocumentIdentifier = this.Context.iNTTRAGeneralMethods.FormatString(myDocumentIdentifier, 35), 
                TransactionStatus = transactionStatus,
                TransactionSplitIndicator = false,
            };

            return myResult;
        }
        public INTTRA_Booking.MessagePropertiesType GetMessageProperties()
        {
            INTTRA_Booking.MessagePropertiesType myResult = new INTTRA_Booking.MessagePropertiesType()
            {
                ShipmentID = new INTTRA_Booking.ShipmentIDType()
                {
                    Value = this.Context.ShipmentNumber,
                },
                ContactInformation = new ContactInformationType()
                {
                    Type = ContactTypeValues.InformationContact,
                    Name = this.Context.iNTTRAGeneralMethods.GetStringList(this.Context.LoggedContact.EnglishName, 2, 35).FirstOrDefault(),
                    CommunicationDetails = new CoordinatesType()
                    {
                        Email = this.Context.iNTTRAGeneralMethods.GetStringList(this.Context.LoggedContact.Email, 9, 512).ToArray(),
                        Fax = this.Context.iNTTRAGeneralMethods.GetStringList(this.Context.LoggedContact.Fax, 9, 512).ToArray(),
                        Phone = this.Context.iNTTRAGeneralMethods.GetStringList(this.Context.LoggedContact.BusinessPhone, 9, 512).ToArray(),
                    }
                },
                DateTime = new INTTRA_Booking.DateTimeCodeType()
                {
                    Type = INTTRA_Booking.DateTimeCodeTypeType.DateTime,
                    Value = new DateTime(this.Context.TodayDateTime.Year, this.Context.TodayDateTime.Month, this.Context.TodayDateTime.Day, this.Context.TodayDateTime.Hour, this.Context.TodayDateTime.Minute, this.Context.TodayDateTime.Second),
                },
                MovementType = this.Context.MovementType,
                MovementTypeSpecified = true,
                Location = this.Context.Locations.ToArray<INTTRA_Booking.LocationDateTimeType>(),
                ReferenceInformation = this.Context.ReferenceInformations != null ? this.Context.ReferenceInformations.ToArray<INTTRA_Booking.ReferenceInformationType>(): null,
                TransportationDetails = this.Context.TransportationDetails.ToArray(),
                Party = this.Context.MessagePropertiesParties.ToArray(),

            };
            return myResult;
        }
        public INTTRA_Booking.MessageDetailsType GetMessageDetails()
        {
            INTTRA_Booking.MessageDetailsType myResult = new INTTRA_Booking.MessageDetailsType()
            {
                GoodsDetails = this.Context.GoodsDetails.ToArray<INTTRA_Booking.GoodsDetailsType>(),
                EquipmentDetails = this.Context.EquipmentDetails.ToArray<INTTRA_Booking.EquipmentDetailsType>(),
            };

            return myResult;
        }
    }
}
