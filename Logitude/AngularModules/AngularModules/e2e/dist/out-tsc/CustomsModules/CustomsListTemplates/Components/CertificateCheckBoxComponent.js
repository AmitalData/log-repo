"use strict";
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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var CertificateCheckBoxComponent = /** @class */ (function () {
    function CertificateCheckBoxComponent(cd) {
        var _this = this;
        this.cd = cd;
        this.publish = true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrentSession.SubscriptionAdd(this.CurrentSession.ConnectedItemSelectedEvent.subscribe(function (res) {
            _this.publish = false;
            if (res.Count == "All") {
                _this.IsSelected = true;
                _this.isAllSelected = true;
            }
            else {
                _this.IsSelected = false;
                _this.isAllSelected = false;
            }
            _this.publish = true;
        }));
    }
    CertificateCheckBoxComponent.prototype.setVariables = function (rowData, fieldName) {
        this.rowData = rowData;
        this.fieldName = fieldName;
    };
    Object.defineProperty(CertificateCheckBoxComponent.prototype, "IsSelected", {
        get: function () { return this.rowData.IsSelected; },
        set: function (value) {
            this.isSelected = value;
            if (this.isSelected) {
                //if (!this.isAllSelected) {
                this.CurrentSession.SelectItemEvent.emit({ data: this.rowData, selected: true });
                //}
                this.rowData.IsSelected = true;
            }
            else {
                this.rowData.IsSelected = false;
                //if (!this.isAllSelected) {
                this.CurrentSession.SelectItemEvent.emit({ data: this.rowData, selected: false });
                //}
            }
            var isDestroyed = this.cd['destroyed'];
            if (!isDestroyed) {
                this.cd.detectChanges();
            }
        },
        enumerable: true,
        configurable: true
    });
    ;
    CertificateCheckBoxComponent.prototype.FirePreventSelect = function () {
        this.CurrentSession.PseventRowSelectEvent.emit("certificate");
    };
    CertificateCheckBoxComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CertificateCheckBoxComponent',
            templateUrl: './CertificateCheckBoxComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], CertificateCheckBoxComponent);
    return CertificateCheckBoxComponent;
}());
exports.CertificateCheckBoxComponent = CertificateCheckBoxComponent;
//# sourceMappingURL=CertificateCheckBoxComponent.js.map