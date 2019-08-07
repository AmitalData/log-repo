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
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var VendorCommunicationPM_1 = require("../../../../../Customs/EntityPMs/VendorCommunicationPM");
// Send Request
var INF_MSG_GenericResponseData_1 = require("../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData");
var VendorCommunicationResult_1 = require("../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult");
var VendorInsertUpdateDeleteMessageRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var VendorMessagesService_1 = require("../../../../../Customs/Services/WebServices/VendorMessagesService");
var CustomsVendorPMService_1 = require("../../../../../Customs/Services/StandardPMs/CustomsVendorPMService");
var VendorGeneralTabComponent = /** @class */ (function (_super) {
    __extends(VendorGeneralTabComponent, _super);
    function VendorGeneralTabComponent() {
        var _this = _super.call(this) || this;
        _this.FillValidationErrorList = new core_1.EventEmitter();
        _this.ObjectTableName = "Customs.CustomsVendor";
        _this.DataContext = _this;
        _this.IsNewEntity = false;
        _this.SubCountryCodeEnabled = false;
        _this.IsDelete = false;
        _this.vendorMessagesService = new VendorMessagesService_1.VendorMessagesService();
        _this.customsVendorPMService = new CustomsVendorPMService_1.CustomsVendorPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#endregion
        _this.line = 0;
        _this._ApplicationID = "";
        _this.CommunicationsList = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    VendorGeneralTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        console.log("EntityPM", this.EntityPM);
        this.FillGridData();
        this.SetFieldsEditability();
    };
    VendorGeneralTabComponent.prototype.FillGridData = function () {
        // Communications List
        this.CommunicationsList = new ObservableCollection_1.ObservableCollection([]);
        for (var _i = 0, _a = this.EntityPM.VendorCommunications; _i < _a.length; _i++) {
            var item = _a[_i];
            this.CommunicationsList.Insert(new CommunicationItemModel(item));
        }
    };
    VendorGeneralTabComponent.prototype.SetFieldsEditability = function () {
        this.UIProperties.SetEnabled("VendorTypeCode", this.ObjectTableName, this.IsNewEntity);
        this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.CountryCode));
    };
    Object.defineProperty(VendorGeneralTabComponent.prototype, "VendorTypeCode", {
        //#region Properties
        get: function () { return this.EntityPM.VendorTypeCode; },
        set: function (newValue) {
            this.EntityPM.VendorTypeCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "CountryCode", {
        get: function () { return this.EntityPM.CountryCode; },
        set: function (newValue) {
            this.EntityPM.CountryCode = newValue;
            this.SubCountryCode = null;
            this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(newValue));
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "CityName", {
        get: function () { return this.EntityPM.CityName; },
        set: function (newValue) {
            this.EntityPM.CityName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "PostalCode", {
        get: function () { return this.EntityPM.PostalCode; },
        set: function (newValue) {
            this.EntityPM.PostalCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "VATNumber", {
        get: function () { return this.EntityPM.VATNumber; },
        set: function (newValue) {
            this.EntityPM.VATNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (newValue) {
            this.EntityPM.VendorName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "SubCountryCode", {
        get: function () { return this.EntityPM.SubCountryCode; },
        set: function (newValue) {
            this.EntityPM.SubCountryCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "MainAddressLine", {
        get: function () { return this.EntityPM.MainAddressLine; },
        set: function (newValue) {
            this.EntityPM.MainAddressLine = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "DunsNumber", {
        get: function () { return this.EntityPM.DunsNumber; },
        set: function (newValue) {
            this.EntityPM.DunsNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorGeneralTabComponent.prototype, "TransactionTypeID", {
        get: function () { return this.EntityPM.TransactionTypeID; },
        set: function (newValue) {
            this.EntityPM.TransactionTypeID = newValue;
        },
        enumerable: true,
        configurable: true
    });
    VendorGeneralTabComponent.prototype.AddButonClicked = function () {
        var _this = this;
        if (this.IsNewEntity) {
            var newCommunicationPM = new VendorCommunicationPM_1.VendorCommunicationPM(this.EntityPM);
            newCommunicationPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            newCommunicationPM.VendorId = this.EntityPM.Id;
            newCommunicationPM.LineNumber = this.line++; // it will be override by EntityUpdateService.OnCreating() in server.
            if (!this.EntityPM.VendorCommunications.includes(newCommunicationPM)) {
                this.EntityPM.AddVendorCommunication(newCommunicationPM);
                this.CommunicationsList.Insert(new CommunicationItemModel(newCommunicationPM));
            }
        }
        else {
            //open send window
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.O.AddVendorCommunication"); //"Add Vendor Communication";
            logWindow.WindowArgs = { VendorPM: this.EntityPM };
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event == "ok") {
                    _this.CurrentSession.StartBusyIndicatorLoading();
                    _this.customsVendorPMService.get(_this.EntityPM.Id).subscribe(function (res) {
                        var vendor = res.Result;
                        _this.CurrentSession.StopBusyIndicator();
                        if (!Tools_1.AppTool.IsNullOrEmpty(vendor)) {
                            _this.EntityPM = vendor;
                            _this.FillGridData();
                            _this.SetFieldsEditability();
                        }
                        else {
                            console.log("[ERROR/VendorCommunication] empty response");
                        }
                    });
                }
            });
            logWindow.Show('./CustomsModules/CustomsVendor/Components/EditTabs/General/AddVendorCommunicationComponent');
        }
    };
    VendorGeneralTabComponent.prototype.RemoveRow = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.DeleteCommunication"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.CommunicationsList.Remove(item);
                    _this.EntityPM.RemoveVendorCommunication(item.CommunicationPM);
                }
            });
        }
    };
    //#region Send + Delete
    VendorGeneralTabComponent.prototype.SendButtonClicked = function (event) {
        this.RequestVIA = event.RequestVIA;
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs
        // validate Vendor
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.CustomsVendor", errors);
        if (this.EntityPM.VendorCommunications.length == 0) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.RequierdCommunication"));
        }
        else {
            // validate vendor communication items
            this.EntityPM.VendorCommunications.forEach(function (item) {
                Validator_1.Validator.TryValidateObject(item, "Customs.VendorCommunication", errors);
            });
        }
        if (errors.length > 0) {
            this.ValdationErrorList = errors;
            this.FillValidationErrorList.emit(errors);
        }
        else {
            // send request
            this.SendRequest(false);
        }
    };
    VendorGeneralTabComponent.prototype.DeleteButtonClicked = function (isAfterWarning, event) {
        var _this = this;
        if (event != null)
            this.RequestVIA = event.RequestVIA;
        this.IsDelete = true;
        var operationDescription = "Delete vendor ";
        var operation = VendorInsertUpdateDeleteMessageRequestParams_1.OperationTypes.Delete;
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Sending");
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.CustomsVendor"; })[0];
        var deleteParams = new VendorInsertUpdateDeleteMessageRequestParams_1.VendorInsertUpdateDeleteMessageRequestParams();
        deleteParams.OperationType = operation;
        deleteParams.LoggingObjectTableId = ObjectTable.Id;
        deleteParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        deleteParams.IsFakeResponse = true;
        deleteParams.LoggingEnabled = true;
        deleteParams.RequestName = "delete Vendor Request";
        deleteParams.ResponseName = "delete Vendor Response";
        deleteParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        deleteParams.LoggingEntityId = this.EntityPM.Id;
        deleteParams.LoggingEntityReference = this.EntityPM.VendorNumber;
        deleteParams.VendorName = this.EntityPM.VendorName;
        deleteParams.CityName = this.EntityPM.CityName;
        deleteParams.CountryCode = this.EntityPM.CountryCode;
        deleteParams.DunsNumber = this.EntityPM.DunsNumber;
        deleteParams.MainAddressLine = this.EntityPM.MainAddressLine;
        deleteParams.PostalCode = this.EntityPM.PostalCode;
        deleteParams.StatusCode = this.EntityPM.StatusCode;
        deleteParams.SubCountryCode = this.EntityPM.SubCountryCode;
        deleteParams.VendorNumber = this.EntityPM.VendorNumber;
        deleteParams.VendorTypeCode = this.EntityPM.VendorTypeCode;
        //deleteParams.TestCase = SelectedTest;
        deleteParams.IsAfterWarning = isAfterWarning;
        deleteParams.ResponseName = "מחיקת ספק" + " - " + this.EntityPM.VendorName;
        deleteParams.RequestVIA = this.RequestVIA;
        if (this.IsNewEntity) { // Abdullah: No delete button in Add New Vendor !!!
            deleteParams.CommunicationDevices = [];
            this.EntityPM.VendorCommunications.forEach(function (item) {
                var vendorCommunicationResult = new VendorCommunicationResult_1.VendorCommunicationResult();
                vendorCommunicationResult.CommunicationAddress = item.CommunicationAddress;
                vendorCommunicationResult.CommunicationType = item.CommunicationTypeCode;
                vendorCommunicationResult.CommunicationTypeName = item.CommunicationTypeName;
                deleteParams.CommunicationDevices.push(vendorCommunicationResult);
            });
        }
        CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(deleteParams.PBId, "שליחת מסר הוספה/עדכון/מחיקת ספק", false).then(function (res) {
            _this.ResponseData = res;
            console.log("[Delete] Response/ShowProgressBar : ", _this.ResponseData);
        }).catch(function (err) {
            _this.FillValidationErrorList.emit(err);
        });
        this.vendorMessagesService.PostAddNewVendorRequest(deleteParams).subscribe(function (myServiceResponse) {
            console.log("[Delete] Response/PostAddNewVendorRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                if (response.IsCustomWarning) {
                    //_CustomMassagingProgressService.CloseWin();
                    //_CustomMassagingProgressService.Dispose();
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Show(response.UserMessage);
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            if (!_this.IsDelete) {
                                _this.SendRequest(true);
                            }
                            else {
                                _this.DeleteButtonClicked(true, null);
                            }
                        }
                    });
                }
                else {
                    var message = response.UserMessage;
                    if (Tools_1.AppTool.IsNullOrEmpty((response.UserMessage))) {
                        if (!response.HasException && response.Succeeded) {
                            message = operationDescription + response.ApplicationID + " בוצע בהצלחה";
                        }
                        else {
                            message = operationDescription + response.ApplicationID + " נכשלה";
                        }
                    }
                }
            }
            else {
                message = "Service returned a null response!";
            }
            _this.OnSendCompleted();
        });
    };
    VendorGeneralTabComponent.prototype.SendRequest = function (isAfterWarning) {
        var _this = this;
        var operationDescription;
        var operation;
        if (this.IsNewEntity) {
            operation = VendorInsertUpdateDeleteMessageRequestParams_1.OperationTypes.Add;
            operationDescription = "הקמת ספק ";
        }
        else {
            operation = VendorInsertUpdateDeleteMessageRequestParams_1.OperationTypes.Update;
            operationDescription = "עדכון ספק ";
        }
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.CustomsVendor"; })[0];
        var addParams = new VendorInsertUpdateDeleteMessageRequestParams_1.VendorInsertUpdateDeleteMessageRequestParams();
        addParams.OperationType = operation;
        addParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        addParams.LoggingObjectTableId = ObjectTable.Id;
        addParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        addParams.IsFakeResponse = true;
        addParams.LoggingEnabled = true;
        addParams.RequestName = "Add Vendor Request";
        addParams.ResponseName = "Add Vendor Response";
        addParams.LoggingEntityId = this.EntityPM.Id;
        addParams.LoggingEntityReference = this.EntityPM.VendorNumber;
        addParams.VendorName = this.EntityPM.VendorName;
        addParams.CityName = this.EntityPM.CityName;
        addParams.CountryCode = this.EntityPM.CountryCode;
        addParams.DunsNumber = this.EntityPM.DunsNumber;
        addParams.MainAddressLine = this.EntityPM.MainAddressLine;
        addParams.PostalCode = this.EntityPM.PostalCode;
        addParams.StatusCode = this.EntityPM.StatusCode;
        addParams.SubCountryCode = this.EntityPM.SubCountryCode;
        addParams.VendorNumber = this.EntityPM.VendorNumber;
        addParams.VendorTypeCode = this.EntityPM.VendorTypeCode;
        addParams.VATNumber = this.EntityPM.VATNumber;
        addParams.TransactionTypeID = this.EntityPM.TransactionTypeID;
        addParams.IsPalestinian = this.EntityPM.IsPalestinian;
        addParams.ExternalId = this.EntityPM.ExternalId;
        addParams.ConcurrencyGUID = this.EntityPM.ConcurrencyGUID;
        addParams.RequestVIA = this.RequestVIA;
        //addParams.TestCase = SelectedTest; // this is should be in RequestParamsBase but it does not
        addParams.IsAfterWarning = isAfterWarning;
        addParams.ResponseName = operationDescription + " - " + this.EntityPM.VendorName;
        addParams.CommunicationDevices = [];
        this.EntityPM.VendorCommunications.forEach(function (item) {
            var vendorCommunicationResult = new VendorCommunicationResult_1.VendorCommunicationResult();
            vendorCommunicationResult.CommunicationAddress = item.CommunicationAddress;
            vendorCommunicationResult.CommunicationType = item.CommunicationTypeCode;
            vendorCommunicationResult.CommunicationTypeName = item.CommunicationTypeName;
            addParams.CommunicationDevices.push(vendorCommunicationResult);
        });
        //if (editSendOption == null) {
        //    addParams.RequestVIA = SendRequestVIA.Default;
        //}
        //else if (editSendOption == "WI") {
        //    addParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
        //}
        //else if (editSendOption == "WB") {
        //    addParams.RequestVIA = SendRequestVIA.WebServiceBatch;
        //}
        //else if (editSendOption == "D") {
        //    addParams.RequestVIA = SendRequestVIA.DCABatch;
        //}
        var myShowProgressBarParams = new CustomMessageProgressComponent_1.ShowProgressBarParams();
        myShowProgressBarParams.OnSuccessAnalyzeCloseWinMethod =
            function (res1) {
                if (!Tools_1.AppTool.IsNullOrEmpty(res1)) {
                    if (res1.IsCustomWarning) {
                        return true;
                    }
                }
                return false;
            };
        CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(addParams.PBId, "שליחת מסר הוספה/עדכון/מחיקת ספק", false, 
        //(res1) => {
        //    if (!AppTool.IsNullOrEmpty(res1)) {
        //        if (res1.IsCustomWarning) {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        myShowProgressBarParams).then(function (res) {
            _this.ResponseData = res;
            console.log("[Send] Response/ShowProgressBar : ", _this.ResponseData);
            //--OnMassageDisplayMethod
            if (_this.RequestParams == null) {
                _this.RequestParams = new VendorInsertUpdateDeleteMessageRequestParams_1.VendorInsertUpdateDeleteMessageRequestParams();
            }
            if (_this.ResponseData == null) {
                _this.ResponseData = new INF_MSG_GenericResponseData_1.INF_MSG_GenericResponseData();
            }
            //--
        }).catch(function (err) {
            _this.FillValidationErrorList.emit(err);
        });
        this.vendorMessagesService.PostAddNewVendorRequest(addParams).subscribe(function (myServiceResponse) {
            console.log("[Send] Response/PostAddNewVendorRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;
            _this._ApplicationID = response.ApplicationID;
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                if (response.IsCustomWarning) {
                    //_CustomMassagingProgressService.CloseWin();
                    //_CustomMassagingProgressService.Dispose();
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Show(response.UserMessage);
                    confirmWindow.WindowClosed.subscribe(function (event) {
                        if (confirmWindow.Yes) {
                            if (!_this.IsDelete) {
                                _this.SendRequest(true);
                            }
                            else {
                                _this.DeleteButtonClicked(true, null);
                            }
                        }
                    });
                }
                else {
                    var message = response.UserMessage;
                    if (Tools_1.AppTool.IsNullOrEmpty((response.UserMessage))) {
                        if (!response.HasException && response.Succeeded) {
                            message = operationDescription + response.ApplicationID + " בוצע בהצלחה";
                        }
                        else {
                            message = operationDescription + response.ApplicationID + " נכשלה";
                        }
                    }
                }
            }
            else {
                message = "Service returned a null response!";
            }
            //var mess = message;
            //if (!string.IsNullOrWhiteSpace(mess)) {
            //    _CustomMassagingProgressService.ShowResponseMessage(mess);
            //    _CustomMassagingProgressService.WindowClosed += (canIContinueEventArgs) => {
            //        _CustomMassagingProgressService.Dispose();
            _this.OnSendCompleted();
            //    };
            //}
        });
    };
    VendorGeneralTabComponent.prototype.OnSendCompleted = function () {
        if (this.IsDelete) {
            this.ApplyDeleteVendor();
        }
        else {
            if (this.IsNewEntity && !Tools_1.AppTool.IsNullOrEmpty(this._ApplicationID)) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    this.EntityPM.Id = this._ApplicationID; //ResponseData.ApplicationID;
                    console.log("Vendor created " + this.EntityPM.Id);
                }
            }
        }
    };
    VendorGeneralTabComponent.prototype.ApplyDeleteVendor = function () {
        this.IsDelete = false;
        //RefreshDataEvent refreshDataEvent = eventAggregator.GetEvent<RefreshDataEvent>();
        //refreshDataEvent.Publish(new RefreshDataEventArgs() { });
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vendor", DateTime.UtcNow, true);
        //this.Dispose();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], VendorGeneralTabComponent.prototype, "FillValidationErrorList", void 0);
    VendorGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VendorGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], VendorGeneralTabComponent);
    return VendorGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.VendorGeneralTabComponent = VendorGeneralTabComponent;
// Communication Tab
var CommunicationItemModel = /** @class */ (function (_super) {
    __extends(CommunicationItemModel, _super);
    function CommunicationItemModel(communicationPM) {
        var _this = _super.call(this) || this;
        _this.communicationPM = communicationPM;
        _this.CommunicationPM = null;
        _this.ObjectTableName = "Customs.VendorCommunication";
        _this.DataContext = _this;
        _this.CommunicationPM = communicationPM;
        return _this;
    }
    Object.defineProperty(CommunicationItemModel.prototype, "CommunicationAddress", {
        //#region Properties
        get: function () { return this.CommunicationPM.CommunicationAddress; },
        set: function (value) {
            if (this.CommunicationPM.CommunicationAddress != value) {
                this.CommunicationPM.CommunicationAddress = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommunicationItemModel.prototype, "CommunicationTypeCode", {
        get: function () { return this.CommunicationPM.CommunicationTypeCode; },
        set: function (value) {
            if (this.CommunicationPM.CommunicationTypeCode != value) {
                this.CommunicationPM.CommunicationTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CommunicationItemModel.prototype, "CommunicationTypeName", {
        get: function () { return this.CommunicationPM.CommunicationTypeName; },
        set: function (value) {
            if (this.CommunicationPM.CommunicationTypeName != value) {
                this.CommunicationPM.CommunicationTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    CommunicationItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return CommunicationItemModel;
}(BaseComponent_1.BaseComponent));
exports.CommunicationItemModel = CommunicationItemModel;
//# sourceMappingURL=VendorGeneralTabComponent.js.map