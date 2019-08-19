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
var ExportDocumentService_1 = require("../../../../Common/Services/DocumentServices/ExportDocumentService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var SystemInfoComponent = /** @class */ (function () {
    function SystemInfoComponent(_exportDocumentService) {
        this._exportDocumentService = _exportDocumentService;
        this.BluesnapVisibility = false;
        this.PaidVisibility = false;
        this.IsTrailVisibility = false;
        this.TemporalPackageVisibility = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    SystemInfoComponent.prototype.SetDataContext = function (data) {
        this.ID = SessionLocator_1.SessionLocator.Tenant.toString();
        this.PackageName = SessionLocator_1.SessionLocator.TenantManagementJS.PackageName;
        this.NumberOfUsers = SessionLocator_1.SessionLocator.TenantManagementJS.NumberOfUsers.toString();
        this.IsTrial = SessionLocator_1.SessionLocator.TenantManagementJS.IsTrial ? "Yes" : "No";
        if (SessionLocator_1.SessionLocator.TenantManagementJS.TemporalPackageCode) {
            this.TemporalPackageVisibility = true;
        }
        this.IsRecurring = SessionLocator_1.SessionLocator.TenantManagementJS.IsRecurring ? "Yes" : "No";
        if (SessionLocator_1.SessionLocator.TenantManagementJS.PaidUntilDate) {
            this.PaidVisibility = true;
        }
        if (SessionLocator_1.SessionLocator.TenantManagementJS.BluesnapAccount) {
            this.BluesnapVisibility = true;
        }
        if (SessionLocator_1.SessionLocator.TenantManagementJS.IsTrial) {
            this.IsTrailVisibility = true;
        }
        this.GetUsedSpaceFromServer();
    };
    SystemInfoComponent.prototype.GetUsedSpaceFromServer = function () {
        var _this = this;
        this._exportDocumentService.GetUsedSpaceForTenant(SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.UsedSpace = myResult;
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Title = "Logitude Message";
                    messageWindow.Show(pmResponse.ErrorsArray[0]);
                }
            }
        });
    };
    SystemInfoComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SystemInfoComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SystemInfo',
            templateUrl: './SystemInfoComponent.html',
            providers: [ExportDocumentService_1.ExportDocumentService],
        }),
        __metadata("design:paramtypes", [ExportDocumentService_1.ExportDocumentService])
    ], SystemInfoComponent);
    return SystemInfoComponent;
}());
exports.SystemInfoComponent = SystemInfoComponent;
//# sourceMappingURL=SystemInfoComponent.js.map