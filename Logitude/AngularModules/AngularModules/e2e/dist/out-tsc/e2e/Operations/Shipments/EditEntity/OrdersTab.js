"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var OrderTabComponent = /** @class */ (function () {
    function OrderTabComponent() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
    }
    OrderTabComponent.prototype.OrderTab = function (LogitudeShipType, ShipmentType) {
        this.Helper.WaitByIdAndClick('Shipment.TH.Orders');
        if (LogitudeShipType == 'D' || LogitudeShipType == 'M') {
            if (ShipmentType == '') {
                this.AddOrderPackages('1', '10', '10', '10', '100');
                this.AddOrderPackages('2', '20', '20', '20', '40');
            }
            else if (ShipmentType == 'FCL' || ShipmentType == 'FTL') {
                this.AddOrderContainers('1', 'ft', '10', '10', '10', '100');
                this.AddOrderContainers('2', 'st', '20', '20', '20', '40');
            }
            else if (ShipmentType == 'LCL' || ShipmentType == 'LTL') {
                this.AddLCL_LTLPackage('1', 'h6', '10', '10', '10', '100');
                this.AddLCL_LTLPackage('2', 'e5', '20', '20', '20', '40');
            }
            var BookingConfirmationNumber = this.Helper.WaitByIdAndFill('Shipment_BookingConfirmationNumber', 'ConfirmedBy ... ');
            var BookingConfirmedBy = this.Helper.WaitByIdAndFill('Shipment_BookingConfirmedBy', 'Razan ');
            if (ShipmentType == 'FCL' || ShipmentType == 'LCL' || ShipmentType == '') {
                var CutoffDate = this.Helper.WaitByIdAndFill('date_Shipment_CutoffDate', '3');
            }
            // else if (ShipmentType=='FTL'||ShipmentType=='LTL'){
            //   var WarehouseLegCutOffDate = this.Helper.WaitByIdAndFill('date_Shipment_WarehouseLegCutOffDate', '4');
            // }
        }
        else if (LogitudeShipType == 'H') {
            if (ShipmentType == '') {
                this.AddOrderPackages('1', '10', '10', '10', '100');
                this.AddOrderPackages('2', '20', '20', '20', '40');
            }
            else if (ShipmentType == 'FCL' || ShipmentType == 'FTL') {
                this.AddOrderContainers('1', 'ft', '10', '10', '10', '100');
                this.AddOrderContainers('2', 'st', '20', '20', '20', '40');
            }
            else if (ShipmentType == 'LCL' || ShipmentType == 'LTL') {
                this.AddLCL_LTLPackage('1', 'h6', '10', '10', '10', '100');
                this.AddLCL_LTLPackage('2', 'e5', '20', '20', '20', '40');
            }
        }
    };
    OrderTabComponent.prototype.AddOrderPackages = function (quantity, length, width, height, grossWeight) {
        this.Helper.WaitByIdAndClick('Orders-AddPackage');
        var quantityValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Quantity', quantity);
        var lengthValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Length', length);
        var widthValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Width', width);
        var heightValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Height', height);
        var grossWeightValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_GrossWeight', grossWeight);
        this.Helper.WaitByIdAndClick('OrderOKbtn');
    };
    OrderTabComponent.prototype.AddOrderContainers = function (quantity, containerType, length, width, height, grossWeight) {
        this.Helper.WaitByIdAndClick('Orders-AddPackage');
        var quantityValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Quantity', quantity);
        var packageTypeValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_PackageTypeId', containerType);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        var grossWeightValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_GrossWeight', grossWeight);
        this.Helper.WaitByIdAndClick('OrderOKbtn');
    };
    OrderTabComponent.prototype.AddLCL_LTLPackage = function (quantity, packageType, length, width, height, grossWeight) {
        this.Helper.WaitByIdAndClick('Orders-AddPackage');
        var quantityValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Quantity', quantity);
        var packageTypeValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_PackageTypeId', packageType);
        this.Helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        var lengthValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Length', length);
        var widthValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Width', width);
        var heightValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_Height', height);
        var grossWeightValue = this.Helper.WaitByIdAndFill('ShipmentOrderPackage_GrossWeight', grossWeight);
        this.Helper.WaitByIdAndClick('OrderOKbtn');
    };
    return OrderTabComponent;
}());
exports.OrderTabComponent = OrderTabComponent;
//# sourceMappingURL=OrdersTab.js.map