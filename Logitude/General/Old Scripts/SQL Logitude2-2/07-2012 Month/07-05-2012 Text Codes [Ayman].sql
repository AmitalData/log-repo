
USE [Logitude2-2_Main]
GO

update TextCodes
set IsSpellChecked = 0
where Code = 'ShipmentPickUpDelivery.F.FromAddressId'

update TextCodes
set IsSpellChecked = 0
where Code = 'ShipmentPickUpDelivery.F.ToAddressId'

delete from TextCodes where Code = 'ShipmentPickUpDelivery.O.Address'
delete from TextCodes where Code = 'ShipmentPickUpDelivery.O.Warehouse'
delete from TextCodes where Code = 'ShipmentPickUpDelivery.B.ChooseAddress'

delete from ObjectFields where FieldName = 'FromWarehouseId'
delete from ObjectFields where FieldName = 'ToWarehouseId'

delete from TextCodes where Code = 'ShipmentPickUpDelivery.F.FromWarehouseId'
delete from TextCodes where Code = 'ShipmentPickUpDelivery.FromWarehouseIdHelpText'
delete from TextCodes where Code = 'ShipmentPickUpDelivery.M.FromWarehouseIsRequired'

delete from TextCodes where Code = 'ShipmentPickUpDelivery.F.ToWarehouseId'
delete from TextCodes where Code = 'ShipmentPickUpDelivery.ToWarehouseIdHelpText'
delete from TextCodes where Code = 'ShipmentPickUpDelivery.M.ToWarehouseIsRequired'


delete from TextCodes where Code = 'Shipment.B.Next'
delete from TextCodes where Code = 'Shipment.B.Previous'

delete from TextCodes where Code = 'Master.B.Next'
delete from TextCodes where Code = 'Master.B.Previous'

delete from TextCodes where Code = 'Shipment.O.NewShipment.Create'
delete from TextCodes where Code = 'Shipment.S.NewShipment.Create'

delete from TextCodes where Code = 'Master.O.NewMaster.Routings'
delete from TextCodes where Code = 'Master.O.NewMaster.ExpectedOrderDetails'
delete from TextCodes where Code = 'Master.O.NewMaster.Address'
delete from TextCodes where Code = 'Master.O.NewMaster.Contact'
delete from TextCodes where Code = 'Master.O.NewMaster.Reference1'
delete from TextCodes where Code = 'Master.O.NewMaster.Reference2'
delete from TextCodes where Code = 'Master.O.NewMaster.MyCustomer'
delete from TextCodes where Code = 'Master.O.NewMaster.IncludePickUp'
delete from TextCodes where Code = 'Master.O.NewMaster.PickUpAddress'
delete from TextCodes where Code = 'Master.O.NewMaster.IncludeDelivery'
delete from TextCodes where Code = 'Master.O.NewMaster.DeliveryAddress'
delete from TextCodes where Code = 'Master.O.NewMaster.Days'
delete from TextCodes where Code = 'Master.O.NewMaster.HAWB'
delete from TextCodes where Code = 'Master.O.NewMaster.FBL'
delete from TextCodes where Code = 'Master.O.NewMaster.HAWBDate'
delete from TextCodes where Code = 'Master.O.NewMaster.FBLDate'

update TextCodes
set Code = 'Master.S.NewMaster.General', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.General'

update TextCodes
set Code = 'Master.S.NewMaster.BookingDetails', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.BookingDetails'

update TextCodes
set Code = 'Master.S.NewMaster.AdditionalFields', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.AdditionalFields'

update TextCodes
set Code = 'Master.S.NewMaster.Name', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.Name'

update TextCodes
set Code = 'Master.S.NewMaster.Gateway', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.Gateway'

update TextCodes
set Code = 'Master.S.NewMaster.LoadingPort', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.LoadingPort'

update TextCodes
set Code = 'Master.S.NewMaster.From', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.From'

update TextCodes
set Code = 'Master.S.NewMaster.Destination', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.Destination'

update TextCodes
set Code = 'Master.S.NewMaster.DischargePort', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.DischargePort'

update TextCodes
set Code = 'Master.S.NewMaster.To', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.To'

update TextCodes
set Code = 'Master.S.NewMaster.Airline', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.Airline'

update TextCodes
set Code = 'Master.S.NewMaster.Shippingline', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.Shippingline'

update TextCodes
set Code = 'Master.S.NewMaster.Trucker', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.Trucker'

update TextCodes
set Code = 'Master.S.NewMaster.Carrier', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.Carrier'

update TextCodes
set Code = 'Master.S.NewMaster.Quantity', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.Quantity'

update TextCodes
set Code = 'Master.S.NewMaster.PackageType', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.PackageType'

update TextCodes
set Code = 'Master.S.NewMaster.MAWB', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.MAWB'

update TextCodes
set Code = 'Master.S.NewMaster.OBL', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.OBL'

update TextCodes
set Code = 'Master.S.NewMaster.MAWBDate', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.MAWBDate'

update TextCodes
set Code = 'Master.S.NewMaster.OBLDate', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.OBLDate'

update TextCodes
set Code = 'Master.S.NewMaster.No', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.No'

update TextCodes
set Code = 'Master.S.NewMaster.FlightNo', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.FlightNo'

update TextCodes
set Code = 'Master.S.NewMaster.VoyageNo', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.VoyageNo'

update TextCodes
set Code = 'Master.S.NewMaster.TruckerNo', TextCodeTypeCode = 'S'
where Code = 'Master.O.NewMaster.TruckerNo'

delete from TextCodes where Code = 'Master.O.NewMaster.Trucker'
delete from TextCodes where Code = 'Master.O.NewMaster.Carrier'
delete from TextCodes where Code = 'Master.O.NewMaster.Quantity'
delete from TextCodes where Code = 'Master.O.NewMaster.PackageType'
delete from TextCodes where Code = 'Master.O.NewMaster.MAWB'
delete from TextCodes where Code = 'Master.O.NewMaster.OBL'
delete from TextCodes where Code = 'Master.O.NewMaster.MAWBDate'
delete from TextCodes where Code = 'Master.O.NewMaster.OBLDate'
delete from TextCodes where Code = 'Master.O.NewMaster.No'
delete from TextCodes where Code = 'Master.O.NewMaster.FlightNo'
delete from TextCodes where Code = 'Master.O.NewMaster.VoyageNo'
delete from TextCodes where Code = 'Master.O.NewMaster.TruckerNo'
delete from TextCodes where Code = 'Master.O.NewMaster.General'
delete from TextCodes where Code = 'Master.O.NewMaster.BookingDetails'
delete from TextCodes where Code = 'Master.O.NewMaster.AdditionalFields'
delete from TextCodes where Code = 'Master.O.NewMaster.Name'
delete from TextCodes where Code = 'Master.O.NewMaster.Gateway'
delete from TextCodes where Code = 'Master.O.NewMaster.LoadingPort'
delete from TextCodes where Code = 'Master.O.NewMaster.From'
delete from TextCodes where Code = 'Master.O.NewMaster.Destination'
delete from TextCodes where Code = 'Master.O.NewMaster.DischargePort'
delete from TextCodes where Code = 'Master.O.NewMaster.To'
delete from TextCodes where Code = 'Master.O.NewMaster.Airline'
delete from TextCodes where Code = 'Master.O.NewMaster.Shippingline'

delete from TextCodes where Code = 'Quote.O.NewQuote.NewQuote'
delete from TextCodes where Code = 'Quote.O.NewQuote.Direction'
delete from TextCodes where Code = 'Quote.O.NewQuote.Transport'
delete from TextCodes where Code = 'Quote.O.NewQuote.ShipmentType'
delete from TextCodes where Code = 'Quote.O.NewQuote.Days'
