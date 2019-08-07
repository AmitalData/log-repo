"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../../Helpers/FieldsHelper");
var DirectAWB_1 = require("./DirectAWB");
var HouseAWB_1 = require("./HouseAWB");
var MasterAWB_1 = require("./MasterAWB");
var DirectShipment_1 = require("./DirectShipment");
var HouseShipment_1 = require("./HouseShipment");
var MasterShipment_1 = require("./MasterShipment");
var GeneralFunctions_1 = require("../../../Helpers/GeneralFunctions");
// import { ShipmentSearch } from '../../ShipmentSearch';
var EditShipmentTabs_po_1 = require("../EditEntity/EditShipmentTabs.po");
var ShipmentHelper_1 = require("../ShipmentHelper");
var ShipmentWorkSpace = /** @class */ (function () {
    function ShipmentWorkSpace() {
        this.Helper = new FieldsHelper_1.FieldsHelper();
        this.DirectAWB = new DirectAWB_1.DirectAWB();
        this.HouseAWB = new HouseAWB_1.HouseAWB();
        this.MasterAWB = new MasterAWB_1.MasterAWB();
        this.DirectShipment = new DirectShipment_1.DirectShipment();
        this.HouseShipment = new HouseShipment_1.HouseShipment();
        this.MasterShipment = new MasterShipment_1.MasterShipment();
        this.GeneralFunction = new GeneralFunctions_1.GeneralFunctions();
        // this.QuickSearch = new ShipmentSearch();
        this.EditShipmentTabs = new EditShipmentTabs_po_1.EditTabsComponent();
        this.shipHelper = new ShipmentHelper_1.ShipmentHelper();
    }
    ShipmentWorkSpace.prototype.CreateShipment = function (ShipmentLevelCode, Direction, TransportMode, ShipmentType) {
        // this.DirectShipment.CreateAndCloseNewShipment('NEWDIRECT','ShipmentCancelbtn');
        if (ShipmentLevelCode == 'D') {
            this.shipHelper.CreateAndCloseNewShipment('NEWDIRECT', 'ShipmentCancelbtn', Direction, TransportMode, ShipmentType);
            if (TransportMode == 'A') {
                var shipperRef1 = this.GeneralFunction.RandomNum();
                this.DirectShipment.CreateDirectShipment(shipperRef1, Direction, TransportMode, ShipmentType);
                this.Helper.WaitBusyIndicator();
                this.GeneralFunction.UseSearchBox('Shipment_Search', shipperRef1);
                this.EditShipmentTabs.EditTabs(shipperRef1, ShipmentLevelCode, ShipmentType, Direction);
            }
            else if ((TransportMode == 'O' || TransportMode == 'I') && ShipmentType != '') {
                var shipperRef1 = this.GeneralFunction.RandomNum();
                this.DirectShipment.CreateDirectShipment(shipperRef1, Direction, TransportMode, ShipmentType);
                this.Helper.WaitBusyIndicator();
                this.GeneralFunction.UseSearchBox('Shipment_Search', shipperRef1);
                this.EditShipmentTabs.EditTabs(shipperRef1, ShipmentLevelCode, ShipmentType, Direction);
            }
            // this.shipHelper.OperationalCloseShipment();
            // this.shipHelper.AccountingCloseShipment();
            // this.shipHelper.AccountedReopenShipment();
            // this.shipHelper.OperationalReopenShipment();
            // this.shipHelper.CopyShipment();
        }
        else if (ShipmentLevelCode == 'H') {
            this.shipHelper.CreateAndCloseNewShipment('NEWDIRECT', 'ShipmentCancelbtn', Direction, TransportMode, ShipmentType);
            var shipperRef1 = this.GeneralFunction.RandomNum();
            this.HouseShipment.CreateHouseShipment(shipperRef1, ShipmentLevelCode, Direction, TransportMode, ShipmentType); // Create shipment 
            this.Helper.WaitBusyIndicator();
            this.GeneralFunction.UseSearchBox('Shipment_Search', shipperRef1);
            this.EditShipmentTabs.EditTabs(shipperRef1, ShipmentLevelCode, ShipmentType, Direction);
        }
        else if (ShipmentLevelCode == 'M') {
            this.shipHelper.CreateAndCloseNewShipment('NEWMASTER', 'MasterCancelbtn', Direction, TransportMode, ShipmentType);
            this.Helper.WaitBusyIndicator();
            var shipperRef1 = this.GeneralFunction.RandomNum();
            this.MasterShipment.CreateMasterShipment(shipperRef1, ShipmentLevelCode, Direction, TransportMode, ShipmentType); // Create shipment 
            //  this.Helper.WaitBusyIndicatorToShow();
            // this.Helper.WaitWindowClosed();
            this.Helper.WaitBusyIndicator();
            this.GeneralFunction.UseSearchBox('Shipment_Search', shipperRef1);
            this.EditShipmentTabs.EditTabs(shipperRef1, ShipmentLevelCode, ShipmentType, Direction);
        }
    };
    ShipmentWorkSpace.prototype.CreateWizard = function (LogitudeWizardType) {
        var shipperRef1 = this.GeneralFunction.RandomNum();
        if (LogitudeWizardType == 'D') {
            this.DirectAWB.CreateDirectAWB(shipperRef1, LogitudeWizardType);
        }
        else if (LogitudeWizardType == 'H') {
            this.HouseAWB.CreateHouseWizard(shipperRef1, LogitudeWizardType);
        }
        else if (LogitudeWizardType == 'M') {
            this.MasterAWB.CreateMasterWizard(shipperRef1, LogitudeWizardType);
        }
    };
    return ShipmentWorkSpace;
}());
exports.ShipmentWorkSpace = ShipmentWorkSpace;
//# sourceMappingURL=ShipmentWorkSpace.js.map