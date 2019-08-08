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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var MorningMessageRequestParams_1 = require("../../../Customs/DataContract/RequestParams/MorningMessageRequestParams");
var MorningMessageResponseData_1 = require("../../../Customs/DataContract/ResponseData/MorningMessageResponseData");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var MorningMessageComponent = /** @class */ (function (_super) {
    __extends(MorningMessageComponent, _super);
    function MorningMessageComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        ///public ValidationErrorsList: string[] = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        _this.MorningMessageObservableList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    MorningMessageComponent.prototype.ngOnInit = function () {
        _super.prototype.ngOnInit.call(this);
    };
    MorningMessageComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    MorningMessageComponent.prototype.OnMassageDisplayMethod = function () {
        var _this = this;
        if (this.RequestParams == null) {
            this.RequestParams = new MorningMessageRequestParams_1.MorningMessageRequestParams();
        }
        if (this.ResponseData && this.ResponseData.MorningMessageList) {
            var myArr = this.ResponseData.MorningMessageList;
            var fast = true;
            if (!fast) {
                this.ResponseData.MorningMessageList.forEach(function (itemMess) {
                    _this.MorningMessageObservableList.Insert(itemMess);
                });
            }
            else {
                this.MorningMessageObservableList.InsertCollection(myArr);
            }
        }
    };
    MorningMessageComponent.prototype.OnRowLoaded = function (Row) {
        var isExpandaple = false;
        if (Row) {
            var item = Row.rowData;
            if (item.Content) {
                if (item.Content.split('\n').length > 1) {
                    item.NeedExpandaple = isExpandaple = true;
                }
            }
            ///Row.SetExpandaple(isExpandaple);
        }
    };
    MorningMessageComponent.prototype.ShowMore = function (itemContent, itemSubject) {
        //alert(itemContent);
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = itemSubject; //TextCodeTranslator.Translate("CommunicationLogSteps.O.Log");
        logitudeWindow.WindowArgs = itemContent;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/LogFieldComponent');
        //var messageWindow = new MessageWindow();
        //messageWindow.Width = 800;
        //messageWindow.Height = 300;
        //messageWindow.Title = itemSubject;
        //messageWindow.Show(itemContent);
    };
    Object.defineProperty(MorningMessageComponent.prototype, "FromDate", {
        get: function () { return this.RequestParams.FromDate; },
        set: function (value) {
            if (this.RequestParams.FromDate != value) {
                this.RequestParams.FromDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MorningMessageComponent.prototype, "ToDate", {
        get: function () { return this.RequestParams.ToDate; },
        set: function (value) {
            if (this.RequestParams.ToDate != value) {
                this.RequestParams.ToDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MorningMessageComponent.prototype, "SubjectText", {
        get: function () { return this.RequestParams.SubjectText; },
        set: function (value) {
            if (this.RequestParams.SubjectText != value) {
                this.RequestParams.SubjectText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MorningMessageComponent.prototype, "ContentText", {
        get: function () { return this.RequestParams.ContentText; },
        set: function (value) {
            if (this.RequestParams.ContentText != value) {
                this.RequestParams.ContentText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MorningMessageComponent.prototype, "Category", {
        get: function () { return this.RequestParams.Category; },
        set: function (value) {
            if (this.RequestParams.Category != value) {
                this.RequestParams.Category = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    MorningMessageComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        ///alert(customSendOptionsArgs.Option);
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        errors.forEach(function (err) { _this.ValidationErrorsList.push(err); });
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        if (this.MorningMessageObservableList.Length > 0) {
            this.MorningMessageObservableList.Clear();
        }
        var currRequestParams = new MorningMessageRequestParams_1.MorningMessageRequestParams(); ///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.FromDate = this.FromDate;
        currRequestParams.ToDate = this.ToDate;
        currRequestParams.Category = this.Category;
        currRequestParams.ContentText = this.ContentText;
        currRequestParams.SubjectText = this.SubjectText;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת שאילתא להודעות בוקר", true)
            .then(function (res) {
            _this.ResponseData = res;
            _this.OnMassageDisplayMethod();
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this._IIGGeneralMessagesService.PostMorningMessages(currRequestParams)
            //.subscribe((myServiceResponse: ServiceResponse) => {
            //this.CurrentSession.StopBusyIndicator();
            //console.log(myServiceResponse);
            //this.ResponseData = myServiceResponse.Result;
            //this.OnMassageDisplayMethod();
            .subscribe(function () { });
    };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], MorningMessageComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    MorningMessageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'MorningMessageComponent',
            templateUrl: './MorningMessageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MorningMessageComponent);
    return MorningMessageComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.MorningMessageComponent = MorningMessageComponent;
var MyMorningMessageResult = /** @class */ (function (_super) {
    __extends(MyMorningMessageResult, _super);
    function MyMorningMessageResult() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.Toggle = false;
        _this.Indicator = "-";
        return _this;
    }
    MyMorningMessageResult.prototype.ToggleIt = function () {
        this.Toggle = !this.Toggle;
        if (this.Toggle) {
            this.Indicator = "+++";
        }
        else {
            this.Indicator = "---";
        }
    };
    return MyMorningMessageResult;
}(MorningMessageResponseData_1.MorningMessageResult));
//# sourceMappingURL=MorningMessageComponent.js.map