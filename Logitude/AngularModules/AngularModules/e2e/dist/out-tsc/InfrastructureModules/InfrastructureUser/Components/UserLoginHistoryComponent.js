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
var UserExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/UserExtendedPMService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var UserLoginHistoryComponent = /** @class */ (function () {
    function UserLoginHistoryComponent(_userExtendedPMService) {
        this._userExtendedPMService = _userExtendedPMService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    UserLoginHistoryComponent.prototype.ngOnInit = function () {
    };
    UserLoginHistoryComponent.prototype.SetDataContext = function (usersWorkspaceRecentItem) {
        this.Username = usersWorkspaceRecentItem.Username;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this.LoadData(usersWorkspaceRecentItem.Id);
    };
    UserLoginHistoryComponent.prototype.LoadData = function (userId) {
        var _this = this;
        this._userExtendedPMService.GetUserLoginHistory(userId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.UserLoginHistoryLists = myResult;
                }
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    UserLoginHistoryComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    UserLoginHistoryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'UserLoginHistory',
            templateUrl: './UserLoginHistoryComponent.html',
            providers: [UserExtendedPMService_1.UserExtendedPMService]
        }),
        __metadata("design:paramtypes", [UserExtendedPMService_1.UserExtendedPMService])
    ], UserLoginHistoryComponent);
    return UserLoginHistoryComponent;
}());
exports.UserLoginHistoryComponent = UserLoginHistoryComponent;
//# sourceMappingURL=UserLoginHistoryComponent.js.map