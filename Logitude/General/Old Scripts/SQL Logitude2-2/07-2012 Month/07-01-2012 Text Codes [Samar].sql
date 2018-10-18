
USE [Logitude2-2_Main]
GO

--AWB--
delete from textcodes where code = 'Shipment.O.AWB.AWBSettings'
delete from textcodes where code = 'Shipment.O.AWB.Freight'
delete from textcodes where code = 'Shipment.O.AWB.AdditionalFields'
delete from textcodes where code = 'Shipment.O.AWB.TotalOtherChargesPrepaidAgent'
delete from textcodes where code = 'Shipment.O.AWB.TotalOtherChargesCollectAgent'
delete from textcodes where code = 'Shipment.O.AWB.TotalOtherChargesPrepaidCarrier'
delete from textcodes where code = 'Shipment.O.AWB.TotalOtherChargesCollectCarrier'
delete from textcodes where code = 'Shipment.O.AWB.ValuationTotalPrepaid'
delete from textcodes where code = 'Shipment.O.AWB.ValuationTotalCollect'
delete from textcodes where code = 'Shipment.O.AWB.TaxTotalPrepaid'
delete from textcodes where code = 'Shipment.O.AWB.TaxTotalCollect'
delete from textcodes where code = 'Shipment.O.AWB.Code'
delete from textcodes where code = 'Shipment.O.AWB.PC'
delete from textcodes where code = 'Shipment.O.AWB.Due'
delete from textcodes where code = 'Shipment.O.AWB.Quantity'
delete from textcodes where code = 'Shipment.O.AWB.UnitPrice'
delete from textcodes where code = 'Shipment.O.AWB.Amount'
delete from textcodes where code = 'Shipment.O.AWB.ChargeType'
delete from textcodes where code = 'Shipment.O.AWB.Currency'

update textcodes
set code = 'Shipment.B.AWB.FillExRate' , textcodetypecode = 'B'
where code = 'Shipment.O.AWB.FillExRate'

update textcodes
set code = 'Shipment.S.AWB.Disbursement' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.Disbursement'

update textcodes
set code = 'Shipment.S.AWB.Totals' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.Totals'

update textcodes
set code = 'Shipment.S.AWB.TotalPrepaid' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.TotalPrepaid'

update textcodes
set code = 'Shipment.S.AWB.TotalCollect' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.TotalCollect'

update textcodes
set code = 'Shipment.S.AWB.WeightCharge' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.WeightCharge'

update textcodes
set code = 'Shipment.S.AWB.ValuationCharge' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.ValuationCharge'

update textcodes
set code = 'Shipment.S.AWB.ValuationCharge' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.ValuationCharge'

update textcodes
set code = 'Shipment.S.AWB.Tax' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.Tax'

update textcodes
set code = 'Shipment.S.AWB.TotalOtherChargesDueAgent' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.TotalOtherChargesDueAgent'

update textcodes
set code = 'Shipment.S.AWB.TotalOtherChargesDueCarrier' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.TotalOtherChargesDueCarrier'

update textcodes
set code = 'Shipment.S.AWB.AaiwayBill' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.AaiwayBill'

update textcodes
set code = 'Shipment.S.AWB.FrtDisburs' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.FrtDisburs'

update textcodes
set code = 'Shipment.S.AWB.Additional' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.Additional'

update textcodes
set code = 'Shipment.S.AWB.PrepaidCharge' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.PrepaidCharge'

update textcodes
set code = 'Shipment.S.AWB.CollectCharge' , textcodetypecode = 'S'
where code = 'Shipment.O.AWB.CollectCharge'

--********************************************

--Order
delete from textcodes where code = 'Shipment.O.Orders.PackageType'
delete from textcodes where code = 'Shipment.O.Orders.Quantity'
delete from textcodes where code = 'Shipment.O.Orders.EmptyContainer'
delete from textcodes where code = 'Shipment.O.Orders.From'
delete from textcodes where code = 'Shipment.O.Orders.To'

update textcodes
set code = 'Shipment.S.Orders.OrderBookingDetails' , textcodetypecode = 'S'
where code = 'Shipment.O.Orders.OrderBookingDetails'

update textcodes
set code = 'Shipment.S.Orders.Details' , textcodetypecode = 'S'
where code = 'Shipment.O.Orders.Details'

update textcodes
set code = 'Shipment.S.Orders.BookingConfirmation' , textcodetypecode = 'S'
where code = 'Shipment.O.Orders.BookingConfirmation'

update textcodes
set code = 'Shipment.S.Orders.CutOffDate' , textcodetypecode = 'S'
where code = 'Shipment.O.Orders.CutOffDate'

update textcodes
set code = 'Shipment.S.Orders.CutOffTime' , textcodetypecode = 'S'
where code = 'Shipment.O.Orders.CutOffTime'

update textcodes
set code = 'Shipment.S.Orders.PickupDetails' , textcodetypecode = 'S'
where code = 'Shipment.O.Orders.PickupDetails'

update textcodes
set code = 'Shipment.M.Orders.NoPickupEntered', textcodetypecode = 'M'
where code = 'Shipment.O.Orders.NoPickupEntered'

--************************************************
-- Packages
delete from textcodes where code = 'Shipment.O.Packages.PackageType'
delete from textcodes where code = 'Shipment.O.Packages.Quantity'
delete from textcodes where code = 'Shipment.O.Packages.ContainerNo'
delete from textcodes where code = 'Shipment.O.Packages.Seal'
delete from textcodes where code = 'InsideShipmentPackage.O.PackageType'
delete from textcodes where code = 'InsideShipmentPackage.O.Quantity'
delete from textcodes where code = 'Shipment.O.Packages.AddPackage'
delete from textcodes where code = 'Shipment.O.Packages.EditPackage'
delete from textcodes where code = 'Shipment.O.Packages.AddContainer'
delete from textcodes where code = 'Shipment.O.Packages.DeleteContainer'
delete from textcodes where code = 'Shipment.O.Packages.ContainNoPackages'

update textcodes
set code = 'Shipment.S.Packages.Packages' , textcodetypecode = 'S'
where code = 'Shipment.O.Packages.Packages'

update textcodes
set code = 'Shipment.S.Packages.DangerouseGoods' , textcodetypecode = 'S'
where code = 'Shipment.O.Packages.DangerouseGoods'

update textcodes
set code = 'Shipment.S.Packages.Details' , textcodetypecode = 'S'
where code = 'Shipment.O.Packages.Details'

update textcodes
set code = 'Shipment.S.Packages.Summary', textcodetypecode = 'S'
where code = 'Shipment.O.Packages.Summary'

--********************************************
--Partners
delete from textcodes where code = 'Shipment.O.Partners.Customer'

update textcodes
set code = 'Shipment.S.Partners.Partner', textcodetypecode = 'S'
where code = 'Shipment.O.Partners.Partner'

--*************************************************
--Payables
delete from textcodes where code = 'Shipment.O.Payables.UnitPrice'
delete from textcodes where code = 'Shipment.O.Payables.Quantity'
delete from textcodes where code = 'Shipment.O.Payables.UOM'
delete from textcodes where code = 'Shipment.O.Payables.PC'
delete from textcodes where code = 'Shipment.O.Payables.Vendor'
delete from textcodes where code = 'Shipment.O.Payables.Currency'
delete from textcodes where code = 'Shipment.O.Payables.Amount'
delete from textcodes where code = 'Shipment.O.Payables.Due'
delete from textcodes where code = 'Shipment.O.Payables.ChargeTypeSummary'
delete from textcodes where code = 'Shipment.O.Payables.EstimateProfit'
delete from textcodes where code = 'Shipment.O.Payables.ReceiveAPInvoice'

update textcodes
set code = 'Shipment.S.Payables.Receivables', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.Receivables'

update textcodes
set code = 'Shipment.S.Payables.Receivable', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.Receivable'

update textcodes
set code = 'Shipment.S.Payables.Payables', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.Payables'

update textcodes
set code = 'Shipment.S.Payables.Payable', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.Payable'

update textcodes
set code = 'Shipment.S.Payables.Summary', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.Summary'

update textcodes
set code = 'Shipment.S.Payables.Difference', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.Difference'

update textcodes
set code = 'Shipment.S.Payables.Profit', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.Profit'

update textcodes
set code = 'Shipment.S.Payables.Accruals', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.Accruals'

update textcodes
set code = 'Shipment.S.Payables.AccountedPayables', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.AccountedPayables'

update textcodes
set code = 'Shipment.S.Payables.OpenPayables', textcodetypecode = 'S'
where code = 'Shipment.O.Payables.OpenPayables'

update textcodes
set code = 'Shipment.O.Payables.PartiallyPaid'
where code = 'Shipment.O.Payables.Partially Paid'

--***************************************************





























