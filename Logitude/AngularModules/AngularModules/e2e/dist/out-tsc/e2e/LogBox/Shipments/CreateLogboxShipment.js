"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FieldsHelper_1 = require("./../Helpers/FieldsHelper");
var GeneralFunctions_1 = require("./../Helpers/GeneralFunctions");
var LogboxShipment = /** @class */ (function () {
    function LogboxShipment() {
        this.helper = new FieldsHelper_1.FieldsHelper();
        this.GeneralFun = new GeneralFunctions_1.GeneralFunctions();
    }
    LogboxShipment.prototype.CreateShip = function (orderNumber) {
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('LogBoxNEWSHIP');
        this.helper.WaitByIdAndFill('TransportModeId', 'Air');
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitByIdAndFill('CustomerReference1', orderNumber);
        this.helper.WaitByIdAndFill('ForwarderPartnerId', 'Ahmad');
        this.helper.WaitByCssAndClick_FromTagInsideList('.DropDownListItem', 0);
        this.helper.WaitByIdAndClick('OKButton');
        this.helper.WaitBusyIndicator();
        this.helper.WaitWindowClosed();
    };
    LogboxShipment.prototype.SearchForCreatedShipment = function (searchFeildId, orderNumber) {
        this.helper.WaitByIdAndFill(searchFeildId, orderNumber);
        this.helper.WaitBusyIndicator();
    };
    LogboxShipment.prototype.AddDocument = function () {
        this.helper.WaitByIdAndClick('LogGrid_0_0row0');
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('AddLogboxDocument');
        this.helper.WaitByIdAndClick('DocumentTypeCode');
        this.helper.WaitByIdAndFill('Description', 'Test Document');
        // this.helper.WaitByIdAndFill('Notes', 'Logbox test scenario');
        this.helper.WaitByIdAndClick('OK');
        this.helper.WaitBusyIndicator();
        this.helper.WaitByIdAndClick('EmptyTicket');
        this.helper.WaitByIdAndFill('Notes', 'Empty Tiketed added');
        this.helper.WaitByIdAndClick('OK');
        this.helper.WaitBusyIndicator();
    };
    return LogboxShipment;
}());
exports.LogboxShipment = LogboxShipment;
//# sourceMappingURL=CreateLogboxShipment.js.map