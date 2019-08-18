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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var AccountingOpService_1 = require("../../Services/Others/AccountingOpService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var Generate1000Component = /** @class */ (function (_super) {
    __extends(Generate1000Component, _super);
    function Generate1000Component() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "GLAccount";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Email = SessionLocator_1.SessionLocator.LoggedUserPM.Email;
        _this.UIProperties.SetRequired("Email", _this.ObjectTableName, true);
        _this.CurrentSession.StopBusyIndicator();
        _this._AccountingOpService = new AccountingOpService_1.AccountingOpService();
        return _this;
    }
    Generate1000Component.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Generate1000Component.prototype.OnKeyUp = function (key) {
        if (!Tools_1.AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.OkButtonClicked();
            }
        }
    };
    Generate1000Component.prototype.FillErrors = function () {
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.Email)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("Email is Required");
        }
        else if (!Tools_1.FormatTool.IsEmail(this.Email)) {
            this.ValidationErrorsList.push("Email is not valid");
        }
        else {
            this.ValidationErrorsList = [];
        }
    };
    Generate1000Component.prototype.OkButtonClicked = function () {
        var _this = this;
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        this._AccountingOpService
            .Generate1000(this.Email)
            .subscribe(function (res) {
            if (res.HasError) {
                _this.ValidationErrorsList = res.ErrorsArray;
            }
            else {
                if (res.Result) {
                    var mw = new MessageWindow_1.MessageWindow();
                    mw.Show(res.Result.Message);
                }
            }
        }, function (err) {
            alert(err);
        }, function () {
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    Generate1000Component = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './Generate1000Component.html',
        }),
        __metadata("design:paramtypes", [])
    ], Generate1000Component);
    return Generate1000Component;
}(BaseComponent_1.BaseComponent));
exports.Generate1000Component = Generate1000Component;
//# sourceMappingURL=Generate1000Component.js.map