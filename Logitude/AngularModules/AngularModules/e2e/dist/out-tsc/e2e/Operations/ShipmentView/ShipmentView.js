"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("../../Helpers/FieldsHelper");
var ShipmentView = /** @class */ (function () {
    function ShipmentView() {
        this.helper = new FieldsHelper_1.FieldsHelper();
    }
    ShipmentView.prototype.OpenShipmentView = function () {
        this.helper.WaitByIdAndClick('General.MH.Operations');
        this.helper.WaitByIdAndClick('SHIP');
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('Shipments-O-Q');
        this.helper.WaitBusyIndicator();
    };
    ShipmentView.prototype.CreatNewView = function () {
        this.helper.WaitByIdAndClick('QueryList_0_0');
        this.helper.WaitByIdAndClick('NewViewId_0_0');
        //fill view name
        //this.helper.WaitByIdAndFill("NewTab.View.Name ",'Raghads View')
        this.helper.WaitByIdAndFill('NewViewSearchFields_0_0', "Agent");
        this.helper.WaitByCssAndClick_FromTagInsideList('.ListBoxItem', 0);
        this.helper.WaitByIdAndClick("NewButton.View.Add");
        //this.helper.WaitByIdAndClick("NewButton.View.Create");
        //to fill filters 
        this.helper.WaitByIdAndClick("NewTab.View.Filters");
        this.helper.WaitByIdAndFill("NewViewFiltersSearchFieldsId_0_0", 'Agent');
        this.helper.WaitByIdAndClick("CheckBox_0_627_LBL");
        this.helper.WaitByIdAndFill("Shipment_TextValue", 'Raghad');
        this.helper.WaitByCssAndClick_FromTagInsideList(".DropDownList", 0);
    };
    return ShipmentView;
}());
exports.ShipmentView = ShipmentView;
//# sourceMappingURL=ShipmentView.js.map