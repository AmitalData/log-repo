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
var BookingPMService_1 = require("../../Services/StandardPMs/BookingPMService");
var EntityLastActivityService_1 = require("../../../Infrastructure/Services/EntityLastActivityService");
var Args_1 = require("../../Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var BookingWizardLoadComponent = /** @class */ (function () {
    function BookingWizardLoadComponent() {
        this.EntityId = null;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isViewInited = false;
    }
    BookingWizardLoadComponent.prototype.SetWindowArgs = function (entityId) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.EntityId = entityId;
        this.Load();
    };
    BookingWizardLoadComponent.prototype.ngAfterViewInit = function () {
        this.isViewInited = true;
        this.Load();
    };
    BookingWizardLoadComponent.prototype.Load = function () {
        var _this = this;
        if (this.EntityId != null && this.isViewInited) {
            var myService = new BookingPMService_1.BookingPMService();
            myService.get(this.EntityId).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    if (_this.EntityPM != null) {
                        _this.ImportWizard();
                        _this.SendActivityLog();
                    }
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    BookingWizardLoadComponent.prototype.ImportWizard = function () {
        var _this = this;
        var myBookingWizardArgs = new Args_1.BookingWizardArgs();
        myBookingWizardArgs.EntityPM = this.EntityPM;
        this._entityResourceService.getEntityResourceByTableName("Booking", 0).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Booking/Components/BookingWizard/BookingWizardComponent', _this.target)
                .then(function (cmpRef) {
                cmpRef.instance.SetWindowArgs(myBookingWizardArgs);
                _this.CurrentSession.StopBusyIndicator();
            });
        });
    };
    BookingWizardLoadComponent.prototype.SendActivityLog = function () {
        var ObjectTableName = "Booking";
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === ObjectTableName; })[0];
        var ObjectTableId = ObjectTable.Id;
        var myService = new EntityLastActivityService_1.EntityLastActivityService();
        myService.AddActivityLog(this.EntityId, ObjectTableId, SessionLocator_1.SessionLocator.LoggedUserId, 'V').subscribe();
    };
    __decorate([
        core_1.ViewChild('WizardView', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], BookingWizardLoadComponent.prototype, "target", void 0);
    BookingWizardLoadComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './BookingWizardLoadComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BookingWizardLoadComponent);
    return BookingWizardLoadComponent;
}());
exports.BookingWizardLoadComponent = BookingWizardLoadComponent;
//# sourceMappingURL=BookingWizardLoadComponent.js.map