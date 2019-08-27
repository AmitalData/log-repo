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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var core_1 = require("@angular/core");
var PasswordChangeService_1 = require("../../../Common/Services/Others/PasswordChangeService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var forms_1 = require("@angular/forms");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ResetUserPasswordComponent = /** @class */ (function (_super) {
    __extends(ResetUserPasswordComponent, _super);
    function ResetUserPasswordComponent(fb, _passwordChangeService) {
        var _this = _super.call(this) || this;
        _this._passwordChangeService = _passwordChangeService;
        _this.DataContext = _this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.myForm = fb.group({});
        return _this;
    }
    ResetUserPasswordComponent.prototype.ngOnInit = function () {
        this._entityResourceService.getEntityResourceByTableName("User").subscribe(function (response) {
        });
    };
    ResetUserPasswordComponent.prototype.SetDataContext = function (data) {
    };
    ResetUserPasswordComponent.prototype.ResetClicked = function () {
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (!this.UserId) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "User"));
        }
        else {
            this.ResetPassword();
        }
    };
    ResetUserPasswordComponent.prototype.ResetPassword = function () {
        var _this = this;
        var confirmMsg = TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.YouWantToResetPassword");
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("User.O.ResetUserPassword");
        confirmWindow.Width = 400;
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.SendResetRequist();
            }
        });
    };
    ResetUserPasswordComponent.prototype.SendResetRequist = function () {
        var _this = this;
        this._passwordChangeService.ResetUserPassword(this.UserId, SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.UserPasswordResetCompletedSuccessfully") + ": " + result + ".");
                }
            }
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    };
    ResetUserPasswordComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ResetUserPasswordComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ResetUserPassword',
            templateUrl: './ResetUserPasswordComponent.html',
            providers: [PasswordChangeService_1.PasswordChangeService]
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder, PasswordChangeService_1.PasswordChangeService])
    ], ResetUserPasswordComponent);
    return ResetUserPasswordComponent;
}(BaseComponent_1.BaseComponent));
exports.ResetUserPasswordComponent = ResetUserPasswordComponent;
//# sourceMappingURL=ResetUserPasswordComponent.js.map