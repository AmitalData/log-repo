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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var PasswordChangeService_1 = require("../../../../Common/Services/Others/PasswordChangeService");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var UserUnlockComponent = /** @class */ (function () {
    function UserUnlockComponent(_passwordChangeService) {
        this._passwordChangeService = _passwordChangeService;
        this.IsValidPassword = false;
        this.IsShowProgressLoading = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    UserUnlockComponent.prototype.ngOnInit = function () {
    };
    UserUnlockComponent.prototype.SetWindowArgs = function (args) {
        //this.Tenant = args.EnttiyPM.Tenant;
        //this.entityPM = args.EnttiyPM;
        //this.ValidationErrorsList = args.ErrorsList;
        //this.ValidationWarningsList = args.WarningsList;
        //this.InitializeComponent();
    };
    UserUnlockComponent.prototype.OnSignoutClicked = function () {
        SessionLocator_1.SessionLocator.HomeComponent.SignoutClicked();
    };
    UserUnlockComponent.prototype.UnlockClicked = function () {
        var _this = this;
        this.IsShowProgressLoading = false;
        if (this.CurrentPassword) {
            this.IsShowProgressLoading = true;
            var computerId = SessionLocator_1.SessionLocator.GetComputerIdFromStorage();
            this._passwordChangeService.GetSetUserLastLogin(this.CurrentPassword, SessionInfo_1.SessionInfo.LoggedUserPM.Id, SessionInfo_1.SessionInfo.LoggedUserTenant, computerId).subscribe(function (res) {
                _this.IsShowProgressLoading = false;
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                    else {
                        _this.IsValidPassword = true;
                    }
                }
            });
        }
        else {
            this.IsValidPassword = true;
        }
    };
    UserUnlockComponent.prototype.onPasswordChanged = function (event) {
        if (event) {
            this.IsValidPassword = false;
        }
    };
    UserUnlockComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UserUnlockComponent.html',
            providers: [PasswordChangeService_1.PasswordChangeService],
        }),
        __metadata("design:paramtypes", [PasswordChangeService_1.PasswordChangeService])
    ], UserUnlockComponent);
    return UserUnlockComponent;
}());
exports.UserUnlockComponent = UserUnlockComponent;
//# sourceMappingURL=UserUnlockComponent.js.map