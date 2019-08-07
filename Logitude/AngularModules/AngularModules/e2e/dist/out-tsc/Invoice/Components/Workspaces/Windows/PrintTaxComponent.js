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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var InvoiceDomainService_1 = require("../../../Services/InvoiceDomainService");
var PrintTaxComponent = /** @class */ (function (_super) {
    __extends(PrintTaxComponent, _super);
    function PrintTaxComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.TransferTypeCode = null;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    PrintTaxComponent.prototype.SetWindowArgs = function (transferTypeCode) {
    };
    Object.defineProperty(PrintTaxComponent.prototype, "Date1", {
        get: function () { return this.date1; },
        set: function (value) {
            if (this.date1 != value) {
                this.date1 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrintTaxComponent.prototype, "Date2", {
        get: function () { return this.date2; },
        set: function (value) {
            if (this.date2 != value) {
                this.date2 = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrintTaxComponent.prototype, "Email", {
        get: function () { return this.email; },
        set: function (value) {
            if (this.email != value) {
                this.email = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    PrintTaxComponent.prototype.SendButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (this.Date1 == null) {
            errors.push("Start Date must be determined");
        }
        if (this.Date2 == null) {
            errors.push("End Date must be determined");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.Email)) {
            errors.push("E-mail must be determined");
        }
        else {
            if (!Tools_1.FormatTool.IsEmail(this.Email)) {
                errors.push("The E-mail you entered is not valid");
            }
        }
        if (this.Date1 != null && this.Date2 != null) {
            if (this.Date1 > this.Date2) {
                errors.push("Start Date must be less than end date");
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Building Files...");
            var service = new InvoiceDomainService_1.InvoiceDomainService();
            service.PrintTaxData(this.Date1, this.Date2, this.Email).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                _this.CurrentSession.CloseCurrentWindowEmit("Ok");
            });
        }
    };
    PrintTaxComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    PrintTaxComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PrintTaxComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PrintTaxComponent);
    return PrintTaxComponent;
}(BaseComponent_1.BaseComponent));
exports.PrintTaxComponent = PrintTaxComponent;
//# sourceMappingURL=PrintTaxComponent.js.map