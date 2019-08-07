"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var EditShipmentTabs_po_1 = require("./EditShipmentTabs.po");
describe('Operations Module', function () {
    var page;
    beforeEach(function () {
        page = new EditShipmentTabs_po_1.EditTabsComponent();
    });
    it('ShipmentTabs', function () {
        page.GoToShipment();
        //   page.EditTabs('314971','D','');
        // this.QuickSearch.UseQuickSearch('4445364363');
        // this.EditShipmentTabs.EditTabs('4445364363',LogitudeShipType, ShipmentType);
    });
});
//# sourceMappingURL=EditShipmentTabs.e2e-spec.js.map