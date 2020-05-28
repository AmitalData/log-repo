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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var RequestParamsBase_1 = require("../../../../Customs/DataContract/RequestParams/RequestParamsBase");
var CustomsVendorPM_1 = require("../../../../Customs/EntityPMs/CustomsVendorPM");
var VendorCommunicationPM_1 = require("../../../../Customs/EntityPMs/VendorCommunicationPM");
// Send Request
var VendorSearchByCustomsAgentRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/VendorSearchByCustomsAgentRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var VendorMessagesService_1 = require("../../../../Customs/Services/WebServices/VendorMessagesService");
var CustomsVendorPMService_1 = require("../../../../Customs/Services/StandardPMs/CustomsVendorPMService");
var CustomsVendorListService_1 = require("../../../../Customs/Services/StandardLists/CustomsVendorListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ImporterDeclarationRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/ImporterDeclarationRequestParams");
var IIGGeneralMessagesService_1 = require("../../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var NewVendorComponent = /** @class */ (function (_super) {
    __extends(NewVendorComponent, _super);
    function NewVendorComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CustomsVendor";
        _this.ValidationErrorsList = [];
        _this.IsWarningBoxVisible = false;
        _this.WarningMsg = "";
        _this.vendorMessagesService = new VendorMessagesService_1.VendorMessagesService();
        _this.customsVendorPMService = new CustomsVendorPMService_1.CustomsVendorPMService();
        _this.customsVendorListService = new CustomsVendorListService_1.CustomsVendorListService();
        _this.iIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this.IsFromDeclarationMode = false;
        _this.IsImporterDespositionValide = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsSearchMode = false; // yaron want to allowed to send response Even there is only VendorNum
        _this.SelectedRow = null;
        _this.EntityPM = new CustomsVendorPM_1.CustomsVendorPM();
        _this.EntityPM.VendorTypeCode = "1";
        _this.UIProperties.SetEnabled("SubCountryCode", _this.ObjectTableName, false);
        _this.VendorsList = new ObservableCollection_1.ObservableCollection([]);
        _this.entityResourceService.getEntityResourceByTableName("Customs.CustomsClosedTable").subscribe(function (response) { });
        _this.entityResourceService.getEntityResourceByTableName("Customs.ClientsAddressCommType").subscribe(function (response) { });
        if (_this.CurrentSession.CurrentEditComponent != null && _this.CurrentSession.CurrentEditComponent.ObjectTableName == "Customs.Declaration") {
            _this.IsFromDeclarationMode = true;
        }
        return _this;
    }
    NewVendorComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            //this.EntityPM = args.EntityPM;
            if (args.IsSearchMode) {
                this.IsSearchMode = true;
            }
        }
    };
    Object.defineProperty(NewVendorComponent.prototype, "VendorTypeCode", {
        //#region Properties
        get: function () { return this.EntityPM.VendorTypeCode; },
        set: function (newValue) {
            this.EntityPM.VendorTypeCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "CountryCode", {
        get: function () { return this.EntityPM.CountryCode; },
        set: function (newValue) {
            this.EntityPM.CountryCode = newValue;
            this.SubCountryCode = null;
            this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !Tools_1.AppTool.IsNullOrEmpty(newValue));
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "CityName", {
        get: function () { return this.EntityPM.CityName; },
        set: function (newValue) {
            this.EntityPM.CityName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "PostalCode", {
        get: function () { return this.EntityPM.PostalCode; },
        set: function (newValue) {
            this.EntityPM.PostalCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "VATNumber", {
        get: function () { return this.EntityPM.VATNumber; },
        set: function (newValue) {
            this.EntityPM.VATNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "VendorName", {
        get: function () { return this.EntityPM.VendorName; },
        set: function (newValue) {
            this.EntityPM.VendorName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "VendorNumber", {
        get: function () { return this.EntityPM.VendorNumber; },
        set: function (newValue) {
            this.EntityPM.VendorNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "SubCountryCode", {
        get: function () { return this.EntityPM.SubCountryCode; },
        set: function (newValue) {
            this.EntityPM.SubCountryCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "MainAddressLine", {
        get: function () { return this.EntityPM.MainAddressLine; },
        set: function (newValue) {
            this.EntityPM.MainAddressLine = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "DunsNumber", {
        get: function () { return this.EntityPM.DunsNumber; },
        set: function (newValue) {
            this.EntityPM.DunsNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewVendorComponent.prototype, "TransactionTypeID", {
        get: function () { return this.EntityPM.TransactionTypeID; },
        set: function (newValue) {
            this.EntityPM.TransactionTypeID = newValue;
        },
        enumerable: true,
        configurable: true
    });
    NewVendorComponent.prototype.SendButtonClicked = function (event) {
        var _this = this;
        this.RequestVIA = event.RequestVIA;
        this.ValidationErrorsList = [];
        this.VendorsList.Clear();
        this.IsWarningBoxVisible = false;
        if ((!Tools_1.AppTool.IsNullOrEmpty(this.VendorName) && !Tools_1.AppTool.IsNullOrEmpty(this.CountryCode))
            || this.VendorName == "TST" || !Tools_1.AppTool.IsNullOrEmpty(this.VendorNumber)) {
            if (this.VendorName && this.VendorName.length < 2) {
                this.CurrentSession.StopBusyIndicator();
                var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.O.mustEnterAtLeast2Chars"); // Bug 28579: Customs Query - search Vendor
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(msg);
                return;
            }
            if ( //this.IsSearchMode &&
            !Tools_1.AppTool.IsNullOrEmpty(this.VendorNumber)) {
                // yaron want to allowed to send response Even there is only VendorNum
            }
            else {
                var errors = [];
                Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
                if (errors.length > 0) {
                    this.ValidationErrorsList = errors;
                    return;
                }
            }
            // Replaced with if clause ternarry this.VendorNumber == "0" ? null : this.VendorNumber
            //var vendortypeCode;
            //var vendortypeCodeNullable;
            ////int.TryParse(VendorTypeCode, out vendortypeCode);
            //if (vendortypeCode == 0) {
            //    vendortypeCodeNullable = null;
            //}
            //else {
            //    vendortypeCodeNullable = vendortypeCode;
            //}
            //var vendorNumber;
            //var vendorNumberNullable;
            ////int.TryParse(VendorNumber, out vendorNumber);
            //if (vendorNumber == 0) {
            //    vendorNumberNullable = null;
            //}
            //else {
            //    vendorNumberNullable = vendorNumber;
            //}
            //var dunsNumber;
            //var dunsNumberNullable;
            ////int.TryParse(DunsNumber, out dunsNumber);
            //if (dunsNumber == 0) {
            //    dunsNumberNullable = null;
            //}
            //else {
            //    dunsNumberNullable = dunsNumber;
            //}
            var searchParams = new VendorSearchByCustomsAgentRequestParams_1.VendorSearchByCustomsAgentRequestParams();
            searchParams.IsFakeResponse = true;
            searchParams.LoggingEnabled = false;
            searchParams.RequestName = "Search For Vendor Request";
            searchParams.ResponseName = "Search For Vendor Response";
            searchParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
            searchParams.VendorName = this.VendorName == undefined ? null : this.VendorName;
            searchParams.MainAddressLine = this.MainAddressLine;
            searchParams.VendorNumber = this.VendorNumber == "0" ? null : +this.VendorNumber;
            searchParams.VendorTypeCode = this.VendorTypeCode == "0" ? null : +this.VendorTypeCode;
            searchParams.DunsNumber = this.DunsNumber == "0" ? null : +this.DunsNumber;
            searchParams.CountryCode = this.CountryCode;
            searchParams.CityName = this.CityName;
            searchParams.PostalCode = this.PostalCode;
            searchParams.SubCountryCode = this.SubCountryCode;
            searchParams.LicensedDealerNumber = this.VATNumber;
            searchParams.IsPalestinian = this.EntityPM.IsPalestinian;
            searchParams.RequestVIA = this.RequestVIA;
            //searchParams.TestCase = SelectedTest;
            //
            CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(searchParams.PBId, "חיפוש ספק", true)
                .then(function (myServiceResponse) {
                console.log("[Send] Response/ShowProgressBar : ", myServiceResponse);
                var response = myServiceResponse.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                    var VendorResults = response.VendorResults;
                    if (response.IsCustomWarning) {
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                }
            }).catch(function (err) {
                console.error("[ERROR] CustomMessageProgressComponent error: ", err);
            });
            this.vendorMessagesService.PostSearchVendorRequest(searchParams).subscribe(function (myServiceResponse) {
                console.log("[Send] Response/PostSearchVendorRequest : ", myServiceResponse.Result);
                var response = myServiceResponse.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                    var VendorResults = response.VendorResults;
                    if (response.IsCustomWarning) {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Show(response.UserMessage);
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                            }
                        });
                    }
                    else {
                        //var message = response.UserMessage;
                        //if (AppTool.IsNullOrEmpty((response.UserMessage))) {
                        //    if (!response.HasException && response.Succeeded) {
                        //        message = operationDescription + response.ApplicationID + " בוצע בהצלחה";
                        //    }
                        //    else {
                        //        message = operationDescription + response.ApplicationID + " נכשלה";
                        //    }
                        //}
                    }
                    // fill grid
                    _this.VendorsList.Clear();
                    _this.VendorsList.InsertCollection(VendorResults);
                    // warning msg 
                    var resultNumber = Tools_1.AppTool.IsNullOrEmpty(response.NumberOfResult) ? 0 : response.NumberOfResult;
                    var vendorsResultLength = Tools_1.AppTool.IsNullOrEmpty(VendorResults) ? 0 : VendorResults.length;
                    _this.IsWarningBoxVisible = vendorsResultLength < resultNumber ? true : false;
                    if (_this.IsWarningBoxVisible) {
                        var pre = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.O.DifferenceVendorCountMessagePre");
                        var post = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.O.DifferenceVendorCountMessagePost");
                        var msg = pre + response.NumberOfResult + post;
                        _this.WarningMsg = msg;
                    }
                    if (_this.VendorsList != null && _this.VendorsList.Length > 0 && _this.IsFromDeclarationMode) {
                        _this.GetImporterDeposition();
                    }
                }
                else {
                    var message = "Service returned a null response!";
                }
                //this.OnSendCompleted();
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.O.SearchRequieredFieldsError");
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(msg);
        }
    };
    NewVendorComponent.prototype.CreateSupplierButtonClicked = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe(function (response) {
                var vendor = new CustomsVendorPM_1.CustomsVendorPM();
                vendor.Tenant = SessionLocator_1.SessionLocator.Tenant;
                vendor.VendorTypeCode = "1";
                var args = {};
                args.IsNewEntity = true;
                args.EntityPM = vendor;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 570;
                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.New");
                logWindow.WindowArgs = args;
                logWindow.ShowCloseButton = true;
                logWindow.Show('./CustomsModules/CustomsVendor/Components/EditTabs/VendorEditComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                });
            });
        });
    };
    NewVendorComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewVendorComponent.prototype.AddButtonClicked = function (item) {
        var _this = this;
        if (item.StatusCode == '3')
            return;
        var errors = [];
        this.ValidationErrorsList = errors;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        var newVendor = new CustomsVendorPM_1.CustomsVendorPM();
        newVendor.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newVendor.VendorName = item.VendorName;
        newVendor.CityName = item.CityName;
        newVendor.CountryCode = item.CountryCode;
        newVendor.DunsNumber = item.DunsNumber;
        newVendor.MainAddressLine = item.MainAddressLine;
        newVendor.PostalCode = item.PostalCode;
        newVendor.StatusCode = item.StatusCode;
        newVendor.SubCountryCode = item.SubCountryCode;
        newVendor.VendorNumber = item.VendorNumber;
        newVendor.VendorTypeCode = item.VendorTypeCode;
        newVendor.VATNumber = item.VATNumber;
        Validator_1.Validator.TryValidateObject(newVendor, this.ObjectTableName, errors);
        if (errors.length == 0) {
            for (var _i = 0, _a = item.VendorCommunications; _i < _a.length; _i++) {
                var vendorCommunicationResult = _a[_i];
                var newVendorCommunication = new VendorCommunicationPM_1.VendorCommunicationPM(newVendor);
                newVendorCommunication.Tenant = SessionLocator_1.SessionLocator.Tenant;
                newVendorCommunication.CommunicationAddress = vendorCommunicationResult.CommunicationAddress;
                newVendorCommunication.CommunicationTypeCode = vendorCommunicationResult.CommunicationType;
                newVendor.VendorCommunications.push(newVendorCommunication);
            }
            // Call service to add vendor
            this.customsVendorPMService.insert(newVendor).subscribe(function (myResult) {
                var res = myResult;
                if (!res.HasError) {
                    var entity = res.Result;
                    console.log("..Vendor added successfully ", entity);
                    // after success
                    var index = _this.VendorsList.GetIndex(item);
                    item.Exists = true;
                    var temp;
                    temp = _this.VendorsList.Collection;
                    _this.VendorsList.Clear();
                    _this.VendorsList.InsertCollection(temp);
                }
                else {
                    _this.ValidationErrorsList = res.ErrorsArray;
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.ValidationErrorsList = errors;
        }
    };
    NewVendorComponent.prototype.UpdateButtonClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.O.UpdateVendor"));
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
                // Get vendor and update it
                _this.vendorMessagesService.GetVendorByNumber(item.VendorNumber).subscribe(function (res) {
                    var pmResponse = res;
                    if (!pmResponse.HasError) {
                        var entity = pmResponse.Result;
                        _this.UpdateVendor(entity, item);
                        console.log("entity:", entity);
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
            }
        });
    };
    NewVendorComponent.prototype.UpdateVendor = function (vendorPM, VendorResult) {
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = errors;
        if (vendorPM != null) {
            vendorPM.CityName = VendorResult.CityName;
            vendorPM.CountryCode = VendorResult.CountryCode;
            vendorPM.InActive = VendorResult.InActive;
            vendorPM.MainAddressLine = VendorResult.MainAddressLine;
            vendorPM.PostalCode = VendorResult.PostalCode;
            vendorPM.StatusCode = VendorResult.StatusCode;
            vendorPM.SubCountryCode = VendorResult.SubCountryCode;
            vendorPM.Tenant = VendorResult.Tenant;
            vendorPM.VATNumber = VendorResult.VATNumber;
            vendorPM.VendorNumber = VendorResult.VendorNumber;
            vendorPM.VendorTypeCode = VendorResult.VendorTypeCode;
            vendorPM.DunsNumber = VendorResult.DunsNumber;
            vendorPM.IsPalestinian = VendorResult.IsPalestinian;
            vendorPM.VendorName = VendorResult.VendorName;
            vendorPM.InActive = VendorResult.StatusCode == "1" ? false : true;
        }
        Validator_1.Validator.TryValidateObject(vendorPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            // update service
            this.customsVendorPMService.update(vendorPM).subscribe(function (myResult) {
                var res = myResult;
                if (!res.HasError) {
                    var entity = res.Result;
                    var temp;
                    temp = _this.VendorsList.Collection;
                    _this.VendorsList.Clear();
                    _this.VendorsList.InsertCollection(temp);
                    console.log(".. Vendor update successfully ", entity);
                }
                else {
                    _this.ValidationErrorsList = res.ErrorsArray;
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        else {
            this.ValidationErrorsList = errors;
            this.CurrentSession.StopBusyIndicator();
        }
    };
    NewVendorComponent.prototype.OnRowLoaded = function (myRow) {
        if (myRow) {
            var isExpandaple = false;
            var item = myRow.rowData;
            if (item) {
                item.Row = myRow;
                //if (item.InsideItemsSource.length > 0) {
                //    isExpandaple = true;
                //}
            }
            myRow.SetExpandaple(true);
        }
    };
    NewVendorComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    NewVendorComponent.prototype.GetImporterDeposition = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        var customSendOptionsArgs = new RequestParamsBase_1.CustomSendOptionsArgs();
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var day = new Date().getDay();
        var currRequestParams = new ImporterDeclarationRequestParams_1.ImporterDeclarationRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.ImporterNumber = this.CurrentSession.CurrentEditComponent.EntityPM.ImporterCode;
        currRequestParams.IsByExpireDate = true;
        currRequestParams.DeclarationExpire = new Date(Year + 1, month, day);
        this.iIGGeneralMessagesService.PostImporterDeclarationRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
            if (!Tools_1.AppTool.IsNullOrEmpty(myServiceResponse.Result)) {
                _this.JoinVendorDeposition(myServiceResponse.Result.PeriodDeclarationList);
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    NewVendorComponent.prototype.JoinVendorDeposition = function (periodDeclarationList) {
        var _this = this;
        if (periodDeclarationList != null && periodDeclarationList.length > 0) {
            this.VendorsList.Collection.forEach(function (item) {
                var temp = periodDeclarationList.filter(function (a) { return a.VendorID == item.VendorNumber; })[0];
                if (temp != null) {
                    item.ImporterDesposition = temp.ExpirationDate;
                    if (temp.ExpirationDate > new Date().getDate) {
                        _this.IsImporterDespositionValide = false;
                    }
                    else {
                        _this.IsImporterDespositionValide = true;
                    }
                }
            });
        }
    };
    NewVendorComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewVendorComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewVendorComponent);
    return NewVendorComponent;
}(BaseComponent_1.BaseComponent));
exports.NewVendorComponent = NewVendorComponent;
//# sourceMappingURL=NewVendorComponent.js.map