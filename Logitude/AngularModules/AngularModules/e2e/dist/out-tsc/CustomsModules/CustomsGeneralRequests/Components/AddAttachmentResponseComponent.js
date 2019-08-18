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
var CustomMessageWrapperComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var BaseRequestsSheetMassaging_1 = require("../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging");
var AddAttachmentResponseComponent = /** @class */ (function (_super) {
    __extends(AddAttachmentResponseComponent, _super);
    function AddAttachmentResponseComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this._IIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        ///public ValidationErrorsList: string[] = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SuperCustomMessageWrapperComponent = new CustomMessageWrapperComponent_1.CustomMessageWrapperComponent();
        return _this;
    }
    AddAttachmentResponseComponent.prototype.ngOnInit = function () {
        ///alert("AddAttachmentResponseComponent:ngOnInit")
        //super.ngOnInit();
    };
    AddAttachmentResponseComponent.prototype.ngAfterViewInit = function () {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent();
    };
    AddAttachmentResponseComponent.prototype.OnMassageDisplayMethod = function () {
        if (this.RequestParams == null) {
        }
        if (this.ResponseData) {
        }
        else {
            ///console.error("this.ResponseData == null !?!?!?")
        }
        var tst = false;
        if (tst) {
            var rsp = this.ResponseData;
            rsp.HasException = true;
            ;
            rsp.DocumentNumber = "DocumentNumber";
            rsp.ErrorCode = "ErrorCode";
            rsp.ErrorRemarks = "ErrorRemarks\nErrorRemarks\nErrorRemarks";
        }
    };
    Object.defineProperty(AddAttachmentResponseComponent.prototype, "DocumentNumber", {
        get: function () { return this.ResponseData ? this.ResponseData.DocumentNumber : null; },
        set: function (value) {
            if (this.ResponseData.DocumentNumber != value) {
                this.ResponseData.DocumentNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddAttachmentResponseComponent.prototype, "CustomDocument", {
        get: function () { return this.ResponseData ? this.ResponseData.CustomDocument : null; },
        set: function (value) {
            if (this.ResponseData.CustomDocument != value) {
                this.ResponseData.CustomDocument = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddAttachmentResponseComponent.prototype, "CustomRecievedDate", {
        get: function () { return this.ResponseData ? this.ResponseData.CustomRecievedDate : null; },
        set: function (value) {
            if (this.ResponseData.CustomRecievedDate != value) {
                this.ResponseData.CustomRecievedDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddAttachmentResponseComponent.prototype, "Remarks", {
        get: function () { return this.ResponseData ? this.ResponseData.Remarks : null; },
        set: function (value) {
            if (this.ResponseData.Remarks != value) {
                this.ResponseData.Remarks = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddAttachmentResponseComponent.prototype, "ErrorCode", {
        get: function () { return this.ResponseData ? this.ResponseData.ErrorCode : null; },
        set: function (value) {
            if (this.ResponseData.ErrorCode != value) {
                this.ResponseData.ErrorCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddAttachmentResponseComponent.prototype, "ErrorRemarks", {
        get: function () { return this.ResponseData ? this.ResponseData.ErrorRemarks : null; },
        set: function (value) {
            if (this.ResponseData.ErrorRemarks != value) {
                this.ResponseData.ErrorRemarks = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddAttachmentResponseComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddAttachmentResponseComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) { };
    __decorate([
        core_1.ViewChild(CustomMessageWrapperComponent_1.CustomMessageWrapperComponent),
        __metadata("design:type", CustomMessageWrapperComponent_1.CustomMessageWrapperComponent)
    ], AddAttachmentResponseComponent.prototype, "SuperCustomMessageWrapperComponent", void 0);
    AddAttachmentResponseComponent = __decorate([
        core_1.Component({
            selector: 'AddAttachmentResponseComponent',
            moduleId: module.id,
            templateUrl: './AddAttachmentResponseComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddAttachmentResponseComponent);
    return AddAttachmentResponseComponent;
}(BaseRequestsSheetMassaging_1.BaseRequestsSheetMassaging));
exports.AddAttachmentResponseComponent = AddAttachmentResponseComponent;
//# sourceMappingURL=AddAttachmentResponseComponent.js.map