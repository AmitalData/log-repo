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
var WebhookKeysExtendedPMService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/WebhookKeysExtendedPMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var WebhookTesterComponent = /** @class */ (function (_super) {
    __extends(WebhookTesterComponent, _super);
    function WebhookTesterComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        //public EntityPM: WebhookKeysPM = null;
        _this.ObjectTableName = "WebhookKeys";
        _this.DataContext = _this;
        //public EntityId: string = null;
        //public IsNewEntity: boolean = false;
        _this.ValidationErrorsList = [];
        //private isPrimaryGenerated: boolean = false;
        //private isSecondaryGenerated: boolean = false;
        _this.Operators = ["In Header", "In URL"];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.operation = "In Header";
        _this.messageWindow = new MessageWindow_1.MessageWindow();
        _this.myService = new WebhookKeysExtendedPMService_1.WebhookKeysExtendedPMService();
        return _this;
    }
    WebhookTesterComponent.prototype.SetWindowArgs = function (args) {
        this.AccessKey = args['AccessKey'];
        //var test = 'https://system.logitudeworld.com/Angular18123111/index.html';
        var MyURL = window.location.href.split('Angular')[0];
        this.PageURL = MyURL + "WebhooksReceiver.aspx";
        this.InitializeComponent();
    };
    WebhookTesterComponent.prototype.SetNewWizardArgs = function (args) {
        //this.IsNewEntity = args['IsNewEntity'];
        //this.InitializeComponent();
    };
    WebhookTesterComponent.prototype.InitializeComponent = function () {
        //this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
        //    this.IsResourcesReady = true;
        //    if (this.IsNewEntity) {
        //        this.EntityPM = new WebhookKeysPM();
        //        this.EntityPM.Tenant = SessionLocator.Tenant;
        //        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        //        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc(); 
        //        this.EntityPM.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        //        this.EntityPM.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName; 
        //        this.IsEntityReady = true;
        //    }
        //    else {
        //        this.CurrentSession.StartBusyIndicatorLoading();
        //        this.myService.get(this.EntityId).subscribe((myResponse: ServiceResponse) => {
        //            if (myResponse.HasError) {
        //                this.ValidationErrorsList = myResponse.ErrorsArray;
        //            }
        //            else {
        //                this.EntityPM = myResponse.Result;
        //                if (this.EntityPM) {
        //                    this.IsEntityReady = true;
        //                }
        //            }
        //            this.CurrentSession.StopBusyIndicator();
        //        });
        //    }
        //});
    };
    Object.defineProperty(WebhookTesterComponent.prototype, "AccessKey", {
        get: function () { return this.accessKey; },
        set: function (value) {
            this.accessKey = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookTesterComponent.prototype, "PageURL", {
        get: function () { return this.pageURL; },
        set: function (value) {
            this.pageURL = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookTesterComponent.prototype, "ContentToPush", {
        get: function () { return this.contentToPush; },
        set: function (value) {
            this.contentToPush = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WebhookTesterComponent.prototype, "Operation", {
        get: function () { return this.operation; },
        set: function (value) {
            this.operation = value;
        },
        enumerable: true,
        configurable: true
    });
    WebhookTesterComponent.prototype.OperationChanged = function (event) {
        this.Operation = event;
    };
    WebhookTesterComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    WebhookTesterComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        if (Tools_1.AppTool.IsNullOrEmpty(this.PageURL) || Tools_1.AppTool.IsNullOrEmpty(this.ContentToPush)) {
            this.ValidationErrorsList.push("Both URL and Content Fields Are Required .");
            return;
        }
        var DataToPush = { URL: this.PageURL, Operation: this.Operation, AccessKey: this.AccessKey, ContentToPush: this.ContentToPush };
        this.myService.PushHookContent(DataToPush).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.messageWindow.Width = 300;
                _this.messageWindow.Height = 150;
                _this.messageWindow.Title = "Success";
                _this.messageWindow.Message = "your data pushed to the webhook successfully";
                _this.messageWindow.Show(_this.messageWindow.Message);
            }
            else {
                //this.messageWindow.Width = 300;
                //this.messageWindow.Height = 150;
                //this.messageWindow.Title = "Error";
                //this.messageWindow.Message = "your data didn't pushed successfully";
                //this.messageWindow.Show(this.messageWindow.Message);
            }
        });
    };
    WebhookTesterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WebhookTesterComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], WebhookTesterComponent);
    return WebhookTesterComponent;
}(BaseComponent_1.BaseComponent));
exports.WebhookTesterComponent = WebhookTesterComponent;
//# sourceMappingURL=WebhookTesterComponent.js.map