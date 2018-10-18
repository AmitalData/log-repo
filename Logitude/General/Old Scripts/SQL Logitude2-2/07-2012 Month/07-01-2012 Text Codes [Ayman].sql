
USE [Logitude2-2_Main]
GO

delete from TextCodes where Code = 'Shipment.O.NewShipment.NewShipment'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Shipper'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Consignee'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Adhoc'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Periodical'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Direction'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Transport'
delete from TextCodes where Code = 'Shipment.O.NewShipment.ShipmentType'
delete from TextCodes where Code = 'Shipment.O.NewShipment.ShipmentLevel'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Address'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Contact'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Reference1'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Reference2'
delete from TextCodes where Code = 'Shipment.O.NewShipment.DeliveryAddress'
delete from TextCodes where Code = 'Shipment.O.NewShipment.Days'
delete from TextCodes where Code = 'Shipment.O.NewShipment.HAWBDate'
delete from TextCodes where Code = 'Shipment.O.NewShipment.FBLDate'
delete from TextCodes where Code = 'Shipment.O.NewShipment.MAWBDate'
delete from TextCodes where Code = 'Shipment.O.NewShipment.OBLDate'
delete from TextCodes where Code = 'Shipment.S.Orders.CutOffDate'
delete from TextCodes where Code = 'Shipment.O.Orders.CutOffDate'

update TextCodes
set code = 'Customer.F.BillToId'
where code = 'Customer.F.BillTold'

update TextCodes
set code = 'Customer.BillToIdHelpText'
where code = 'Customer.BillToldHelpText'

update TextCodes
set code = 'Customer.CH.BillToIdListLable'
where code = 'Customer.CH.BillToldistLable'

update TextCodes
set DefaultText = 'Delivery Address'
where Code = 'Shipment.F.DelivaryToAddressId'

update TextCodes
set DefaultText = 'Confirmed By'
where Code = 'Master.F.BookingConfirmedBy'

update TextCodes
set DefaultText = 'Confirmed By'
where Code = 'Shipment.F.BookingConfirmedBy'

update TextCodes
set DefaultText = 'Confirmation Notes'
where Code = 'Shipment.F.BookingConfirmationNotes'

update TextCodes
set DefaultText = 'Confirmation Notes'
where Code = 'Master.F.BookingConfirmationNotes'

update TextCodes
set Code = 'Shipment.S.NewShipment.Routings', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Routings'

update TextCodes
set Code = 'Shipment.S.NewShipment.General', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.General'

update TextCodes
set Code = 'Shipment.S.NewShipment.ExpectedOrderDetails', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.ExpectedOrderDetails'

update TextCodes
set Code = 'Shipment.S.NewShipment.AdditionalFields', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.AdditionalFields'

update TextCodes
set Code = 'Shipment.S.NewShipment.Name', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Name'

update TextCodes
set Code = 'Shipment.S.NewShipment.MyCustomer', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.MyCustomer'

update TextCodes
set Code = 'Shipment.S.NewShipment.Gateway', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Gateway'

update TextCodes
set Code = 'Shipment.S.NewShipment.LoadingPort', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.LoadingPort'

update TextCodes
set Code = 'Shipment.S.NewShipment.From', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.From'

update TextCodes
set Code = 'Shipment.S.NewShipment.Destination', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Destination'

update TextCodes
set Code = 'Shipment.S.NewShipment.DischargePort', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.DischargePort'

update TextCodes
set Code = 'Shipment.S.NewShipment.To', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.To'

update TextCodes
set Code = 'Shipment.S.NewShipment.Airline', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Airline'

update TextCodes
set Code = 'Shipment.S.NewShipment.Shippingline', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Shippingline'

update TextCodes
set Code = 'Shipment.S.NewShipment.Trucker', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Trucker'

update TextCodes
set Code = 'Shipment.S.NewShipment.Carrier', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Carrier'

update TextCodes
set Code = 'Shipment.S.NewShipment.Quantity', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Quantity'

update TextCodes
set Code = 'Shipment.S.NewShipment.PackageType', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.PackageType'

update TextCodes
set Code = 'Shipment.S.NewShipment.HAWB', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.HAWB'

update TextCodes
set Code = 'Shipment.S.NewShipment.FBL', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.FBL'

update TextCodes
set Code = 'Shipment.S.NewShipment.MAWB', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.MAWB'

update TextCodes
set Code = 'Shipment.S.NewShipment.OBL', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.OBL'

update TextCodes
set Code = 'Shipment.S.NewShipment.No', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.No'

update TextCodes
set Code = 'Shipment.S.NewShipment.FlightNo', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.FlightNo'

update TextCodes
set Code = 'Shipment.S.NewShipment.VoyageNo', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.VoyageNo'

update TextCodes
set Code = 'Shipment.S.NewShipment.TruckerNo', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.TruckerNo'

update TextCodes
set Code = 'Shipment.S.NewShipment.IncludePickUp', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.IncludePickUp'

update TextCodes
set Code = 'Shipment.S.NewShipment.PickUpAddress', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.PickUpAddress'

update TextCodes
set Code = 'Shipment.S.NewShipment.IncludeDelivery', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.IncludeDelivery'

update TextCodes
set Code = 'Shipment.S.NewShipment.SetAsMyCustomer', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.SetAsMyCustomer'

update TextCodes
set Code = 'Shipment.S.NewShipment.SetAsMyCustomer', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.SetAsMyCustomer'

update TextCodes
set Code = 'Shipment.S.NewShipment.AddNewShipper', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.AddNewShipper'

update TextCodes
set Code = 'Shipment.S.NewShipment.AddNewConsignee', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.AddNewConsignee'

update TextCodes
set Code = 'Shipment.S.NewShipment.Create', TextCodeTypeCode = 'S'
where Code = 'Shipment.O.NewShipment.Create'