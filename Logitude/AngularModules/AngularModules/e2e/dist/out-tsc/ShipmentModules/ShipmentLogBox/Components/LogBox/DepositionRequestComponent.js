"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var ShipmentComputedFieldExtendedService_1 = require("../../../../Shipment/Services/ExtendedPMs/ShipmentComputedFieldExtendedService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var DepositionRequestComponent = /** @class */ (function (_super) {
    __extends(DepositionRequestComponent, _super);
    function DepositionRequestComponent(_entityListService) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.shipmentComputedFieldExtendedService = new ShipmentComputedFieldExtendedService_1.ShipmentComputedFieldExtendedService();
        return _this;
    }
    DepositionRequestComponent.prototype.ngOnInit = function () {
    };
    DepositionRequestComponent.prototype.SetWindowArgs = function (args) {
        this.VendorCodeId = Guid_1.Guid.newGuid();
        this.ShipmentId = !Tools_1.AppTool.IsNullOrEmpty(args.ShipmentId) ? args.ShipmentId : null;
        this.ForwardershipmentNumber = !Tools_1.AppTool.IsNullOrEmpty(args.ForwarderShipmentNumber) ? args.ForwarderShipmentNumber : null;
        this.DirectionId = !Tools_1.AppTool.IsNullOrEmpty(args.DirectionId) ? args.DirectionId : null;
        this.ForwarderPartnerId = !Tools_1.AppTool.IsNullOrEmpty(args.ForwarderPartnerId) ? args.ForwarderPartnerId : null;
        if (!Tools_1.AppTool.IsNullOrEmpty(args.ImporterDepositionRequestDetails)) {
            var result = args.ImporterDepositionRequestDetails.indexOf("^") > -1 ? args.ImporterDepositionRequestDetails.split("^") : args.ImporterDepositionRequestDetails.split(",");
            if (result && result.length > 0) {
                this.VendorCode = result[0];
                this.VendorName = result.length > 1 ? result[1] : "";
            }
        }
    };
    DepositionRequestComponent.prototype.CopyTextButtonClicked = function () {
        CopyText(this.VendorCodeId);
    };
    DepositionRequestComponent.prototype.NewDepositionFormClcik = function () {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("CustomsShipper", "Deposition Link");
        var link = "https://forms.gov.il/globaldata/getsequence/getHtmlForm.aspx?formType=SOVE01_hasava@taxes.gov.il";
        var win = window.open(link, '_blank');
        win.focus();
    };
    DepositionRequestComponent.prototype.MarkAsComplete = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentId)) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this.shipmentComputedFieldExtendedService.GetMarkCompleteDepositionRequest(this.ShipmentId, this.DirectionId, this.ForwardershipmentNumber, this.ForwarderPartnerId).subscribe(function (myResult) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var pmResponse = myResult;
                if (!pmResponse.HasError) {
                    _this.CurrentSession.CurrentWindow.Close("DepositionRequest");
                }
                else {
                    if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show(pmResponse.ErrorsArray[0]);
                    }
                }
            });
        }
        else
            this.CloseButtonClicked();
    };
    DepositionRequestComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DepositionRequestComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DepositionRequestComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], DepositionRequestComponent);
    return DepositionRequestComponent;
}(BaseComponent_1.BaseComponent));
exports.DepositionRequestComponent = DepositionRequestComponent;
//# sourceMappingURL=DepositionRequestComponent.js.map