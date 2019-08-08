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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DropBoxTestFileComponent = /** @class */ (function (_super) {
    __extends(DropBoxTestFileComponent, _super);
    function DropBoxTestFileComponent(CD) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.messageWindow = new MessageWindow_1.MessageWindow();
        _this.IsConnected = false;
        _this.ShowTestButton = false;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    DropBoxTestFileComponent.prototype.ngOnInit = function () {
        this.ValidationErrorsList = [];
    };
    DropBoxTestFileComponent.prototype.ngAfterViewInit = function () {
    };
    DropBoxTestFileComponent.prototype.CloseBtnClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DropBoxTestFileComponent.prototype.SendTestFile = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.FolderName)) {
            this.ValidationErrorsList.push("Folder Name is required ");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.FileName)) {
            this.ValidationErrorsList.push("File Name is required ");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableId)) {
            this.ValidationErrorsList.push("ObjectTable is required ");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
            var myService = new CommonDomainService_1.CommonDomainService();
            myService.GetDropBoxComLogTestFile(SessionLocator_1.SessionLocator.Tenant, this.FileName, this.FolderName, this.FileText, this.ObjectTableId).subscribe(function (myResult) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var temp = myResult.Result;
                _this.messageWindow.Width = 300;
                _this.messageWindow.Height = 200;
                _this.messageWindow.Title = "DropBox Communicaiton Log";
                _this.messageWindow.Message = "Communicaiton Log Created For DropBox Test File Successfully";
                _this.messageWindow.Show(_this.messageWindow.Message);
            });
        }
    };
    Object.defineProperty(DropBoxTestFileComponent.prototype, "FolderName", {
        get: function () { return this.folderName; },
        set: function (newValue) { this.folderName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DropBoxTestFileComponent.prototype, "FileName", {
        get: function () { return this.fileName; },
        set: function (newValue) { this.fileName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DropBoxTestFileComponent.prototype, "FileText", {
        get: function () { return this.fileText; },
        set: function (newValue) { this.fileText = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DropBoxTestFileComponent.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (newValue) { this.objectTableId = newValue; },
        enumerable: true,
        configurable: true
    });
    DropBoxTestFileComponent = __decorate([
        core_1.Component({
            selector: 'DropBoxTestFile',
            moduleId: module.id,
            templateUrl: './DropBoxTestFileComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], DropBoxTestFileComponent);
    return DropBoxTestFileComponent;
}(BaseComponent_1.BaseComponent));
exports.DropBoxTestFileComponent = DropBoxTestFileComponent;
//# sourceMappingURL=DropBoxTestFileComponent.js.map