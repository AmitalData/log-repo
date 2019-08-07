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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var VendorCommunicationPM_1 = require("../../../../../Customs/EntityPMs/VendorCommunicationPM");
var VendorCommunicationResult_1 = require("../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult");
var VendorAddCommunicationDeviceRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/VendorAddCommunicationDeviceRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var VendorMessagesService_1 = require("../../../../../Customs/Services/WebServices/VendorMessagesService");
var CustomsVendorPMService_1 = require("../../../../../Customs/Services/StandardPMs/CustomsVendorPMService");
var AddVendorCommunicationComponent = /** @class */ (function (_super) {
    __extends(AddVendorCommunicationComponent, _super);
    function AddVendorCommunicationComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.VendorCommunication";
        _this.DataContext = _this;
        _this.vendorMessagesService = new VendorMessagesService_1.VendorMessagesService();
        _this.customsVendorPMService = new CustomsVendorPMService_1.CustomsVendorPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddVendorCommunicationComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.VendorPM = args.VendorPM;
            this.EntityPM = new VendorCommunicationPM_1.VendorCommunicationPM(this.VendorPM);
            this.EntityPM.VendorId = this.VendorPM.Id;
            this.EntityPM.LineNumber = 1; // it will be override by EntityUpdateService.OnCreating() in server.
        }
    };
    AddVendorCommunicationComponent.prototype.SendButtonClicked = function () {
        // Validate Requierd Fields
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        }
        else {
            this.SendRequest();
        }
    };
    AddVendorCommunicationComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddVendorCommunicationComponent.prototype.SendRequest = function () {
        var _this = this;
        var vendorNumber = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VendorPM.VendorNumber)) {
            var result;
            //int.TryParse(this.VendorPM.VendorNumber, out result);
            vendorNumber = Number(this.VendorPM.VendorNumber);
        }
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.CustomsVendor"; })[0];
        var addParams = new VendorAddCommunicationDeviceRequestParams_1.VendorAddCommunicationDeviceRequestParams();
        addParams.LoggingEntityId = this.VendorPM.Id;
        addParams.LoggingObjectTableId = ObjectTable.Id;
        addParams.LoggingEntityReference = this.VendorPM.VendorNumber;
        addParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        addParams.LoggingEnabled = true;
        addParams.RequestName = "Add Vendor Communication Request";
        addParams.ResponseName = "Add Vendor Communication Response";
        addParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        addParams.VendorNumber = vendorNumber;
        addParams.IsFakeResponse = false;
        //addParams.TestCase = SelectedTest;
        //if (SendOption == null) {
        //    addParams.RequestVIA = SendRequestVIA.Default;
        //}
        //else if (SendOption == "WI") {
        //    addParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
        //}
        //else if (SendOption == "WB") {
        //    addParams.RequestVIA = SendRequestVIA.WebServiceBatch;
        //}
        //else if (SendOption == "D") {
        //    addParams.RequestVIA = SendRequestVIA.DCABatch;
        //}
        var communicationResult = new VendorCommunicationResult_1.VendorCommunicationResult();
        communicationResult.CommunicationAddress = this.CommunicationAddress;
        communicationResult.CommunicationType = this.CommunicationTypeCode;
        addParams.CommunicationDevices = [];
        addParams.CommunicationDevices.push(communicationResult);
        CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(addParams.PBId, "שליחת מסר הוספה/עדכון/מחיקת ספק", true).then(function (res) {
            console.log("[Send] Response/ShowProgressBar : ", res);
        }).catch(function (err) {
            _this.ValidationErrorsList.push(err);
        });
        this.vendorMessagesService.PostAddNewVendorCommunicationRequest(addParams).subscribe(function (myServiceResponse) {
            console.log("[Send] Response/PostAddNewVendorCommunicationRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                if (response.IsCustomWarning) {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Show(response.UserMessage);
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                        }
                    });
                }
                else {
                    var message = response.UserMessage;
                    if (Tools_1.AppTool.IsNullOrEmpty((response.UserMessage))) {
                        //if (!response.HasException && response.Succeeded) {
                        //    message = operationDescription + response.ApplicationID + " בוצע בהצלחה";
                        //}
                        //else {
                        //    message = operationDescription + response.ApplicationID + " נכשלה";
                        //}
                    }
                }
            }
            else {
                message = "Service returned a null response!";
            }
            _this.OnSendCompleted(response);
            //this.CurrentSession.CloseCurrentWindowEmit("Ok");
        });
    };
    AddVendorCommunicationComponent.prototype.OnSendCompleted = function (response) {
        if (!response.HasException && response.Succeeded) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
        else {
            //CustomsMessageViewModel viewmodel = new CustomsMessageViewModel();
            //CustomsMessageControl actionControl = new CustomsMessageControl() { DataContext = viewmodel };
            //actionControl.OkButton.Click += OkButton_Click;
            //window = new SimplogWindow();
            //window.Add(actionControl);
            //window.Height = 600;
            //window.Width = 800;
            //window.CancelButton.Visibility = Visibility.Collapsed;
            //window.Show();
            //viewmodel.Title = "Send Vendor";
            //viewmodel.Message = responseData.UserMessage;
        }
    };
    Object.defineProperty(AddVendorCommunicationComponent.prototype, "CommunicationTypeCode", {
        //#region Properties
        get: function () { return this.EntityPM.CommunicationTypeCode; },
        set: function (value) {
            if (this.EntityPM.CommunicationTypeCode != value) {
                this.EntityPM.CommunicationTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddVendorCommunicationComponent.prototype, "CommunicationAddress", {
        get: function () { return this.EntityPM.CommunicationAddress; },
        set: function (value) {
            if (this.EntityPM.CommunicationAddress != value) {
                this.EntityPM.CommunicationAddress = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddVendorCommunicationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddVendorCommunicationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddVendorCommunicationComponent);
    return AddVendorCommunicationComponent;
}(BaseComponent_1.BaseComponent));
exports.AddVendorCommunicationComponent = AddVendorCommunicationComponent;
//# sourceMappingURL=AddVendorCommunicationComponent.js.map