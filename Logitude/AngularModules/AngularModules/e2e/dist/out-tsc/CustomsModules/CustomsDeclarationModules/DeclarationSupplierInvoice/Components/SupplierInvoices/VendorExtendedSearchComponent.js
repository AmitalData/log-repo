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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CustomsVendorListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsVendorListService");
var EntityListService_1 = require("../../../../../Infrastructure/Services/EntityListService");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var VendorExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/VendorExtendedListService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var RequestParamsBase_1 = require("../../../../../Customs/DataContract/RequestParams/RequestParamsBase");
var ImporterDeclarationRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/ImporterDeclarationRequestParams");
var IIGGeneralMessagesService_1 = require("../../../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var VendorSearchByCustomsAgentRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/VendorSearchByCustomsAgentRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var VendorMessagesService_1 = require("../../../../../Customs/Services/WebServices/VendorMessagesService");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var CustomsVendorPM_1 = require("../../../../../Customs/EntityPMs/CustomsVendorPM");
var VendorCommunicationPM_1 = require("../../../../../Customs/EntityPMs/VendorCommunicationPM");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var CustomsVendorPMService_1 = require("../../../../../Customs/Services/StandardPMs/CustomsVendorPMService");
var VendorExtendedSearchComponent = /** @class */ (function (_super) {
    __extends(VendorExtendedSearchComponent, _super);
    function VendorExtendedSearchComponent() {
        var _this = _super.call(this) || this;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.customsVendorListService = new CustomsVendorListService_1.CustomsVendorListService();
        _this._entityListService = new EntityListService_1.EntityListService();
        _this.vendorExtendedListService = new VendorExtendedListService_1.VendorExtendedListService();
        _this.iIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        _this.vendorMessagesService = new VendorMessagesService_1.VendorMessagesService();
        _this.customsVendorPMService = new CustomsVendorPMService_1.CustomsVendorPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.searchText = null;
        _this.columns = null;
        _this.DataSource = {
            pageSize: 10,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.isChecked = true;
        _this.showOnlyValid = true;
        _this.SelectedRow = null;
        _this._PeriodDeclarationListSave = [];
        _this._HavePeriodDecResult = false;
        _this.ObjectTableName = "Customs.CustomsVendor";
        _this._PeriodDeclarationList = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.BuildColumns();
        return _this;
    }
    VendorExtendedSearchComponent.prototype.Search = function (text) {
        this.searchText = text;
        if (!this._HavePeriodDecResult) {
            this.LoadData();
        }
        else {
            this.FilterLocal();
        }
    };
    VendorExtendedSearchComponent.prototype.FilterLocal = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.searchText)) {
            this._PeriodDeclarationList.InsertCollection(this._PeriodDeclarationListSave);
            return;
        }
        var searchList = this._PeriodDeclarationListSave.filter(function (periodDecRow) { return _this.ShowPeriodDeclaration(periodDecRow); });
        this._PeriodDeclarationList.Clear();
        this._PeriodDeclarationList.InsertCollection(searchList);
    };
    VendorExtendedSearchComponent.prototype.ShowPeriodDeclaration = function (periodDeclaration) {
        if (periodDeclaration == null)
            return false;
        var VendorID = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(periodDeclaration.VendorID)) {
            VendorID = periodDeclaration.VendorID;
        }
        var VendorName = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(periodDeclaration.VendorName)) {
            VendorName = periodDeclaration.VendorName;
        }
        var searchField = "";
        searchField += VendorID;
        searchField += VendorName;
        return searchField.toLowerCase().includes(this.searchText.toLowerCase());
    };
    VendorExtendedSearchComponent.prototype.SetWindowArgs = function (args) {
        this.ImporterId = args.ImporterId;
        this.IsDisplayOnly = args.IsDisplayOnly;
        this.LoadData();
    };
    VendorExtendedSearchComponent.prototype.LoadData = function () {
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    VendorExtendedSearchComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'VendorNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.F.VendorNumber"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'VendorName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.F.VendorName"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CountryCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsVendor.F.CountryCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ImporterDespositionNumber',
            DataTypeCode: 'String',
            Display: "תצהיר",
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'EndDate',
            DataTypeCode: 'DateTime',
            Display: "תוקף תצהיר",
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'EndDateComponent',
            HtmlListComponentUrl: './Customs/Components/ListTemplates/EndDateComponent',
        });
    };
    VendorExtendedSearchComponent.prototype.ViewInitCompleted = function ($event) {
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    VendorExtendedSearchComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        var _this = this;
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortDirection = "Ascending";
        var importer = null;
        //if (this.IsChecked || this.ShowOnlyValid) {
        //  //  importer = this.ImporterId;
        //}
        //else {
        //    importer = null;
        //}
        importer = this.ImporterId;
        return new Promise(function (resolve, reject) {
            resolve(_this.vendorExtendedListService.GetVendorsWithImporterDespositions(null, importer, _this.ShowOnlyValid, _this.IsChecked, _this.searchText));
        });
    };
    Object.defineProperty(VendorExtendedSearchComponent.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (newValue) {
            this.isChecked = newValue;
            this.LoadData();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VendorExtendedSearchComponent.prototype, "ShowOnlyValid", {
        get: function () { return this.showOnlyValid; },
        set: function (newValue) {
            this.showOnlyValid = newValue;
            this.LoadData();
        },
        enumerable: true,
        configurable: true
    });
    VendorExtendedSearchComponent.prototype.OnItemRowSelected = function (selected) {
        this.SelectedRow = selected.rowData;
        this.CurrentSession.CloseCurrentWindowEmit("close");
    };
    VendorExtendedSearchComponent.prototype.CancelButtonClicked = function () {
        //if (this.SelectedRow) {
        //    this.CurrentSession.CloseCurrentWindowEmit(this.SelectedRow.VendorId);
        //}
        //else {
        this.CurrentSession.CloseCurrentWindow();
        //  }
    };
    VendorExtendedSearchComponent.prototype.NewVendorButtonClicked = function () {
        var _this = this;
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe(function (response) {
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
                logWindow.Show('./CustomsModules/CustomsVendor/Components/Components/EditTabs/VendorEditComponent');
                logWindow.WindowClosed.subscribe(function ($event) { return _this.LoadData(); });
            });
        });
    };
    VendorExtendedSearchComponent.prototype.UpdateImporterDeposition = function () {
        var _this = this;
        this.searchText = "";
        this.CurrentSession.StartBusyIndicator("");
        var customSendOptionsArgs = new RequestParamsBase_1.CustomSendOptionsArgs();
        //var month = new Date().getMonth();
        //var Year = new Date().getFullYear();
        //var day = new Date().getDay();
        // replace the above code with: //mohammad. task 40432
        var month = new Date().getUTCMonth();
        var Year = new Date().getUTCFullYear();
        var day = new Date().getUTCDate();
        var date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(Year + 1);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        var currRequestParams = new ImporterDeclarationRequestParams_1.ImporterDeclarationRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.ImporterNumber = this.CurrentSession.CurrentEditComponent.EntityPM.ImporterCode;
        currRequestParams.IsByExpireDate = true;
        currRequestParams.DeclarationExpire = date; // new Date(Year + 1, month, day);
        currRequestParams.JoinCustomsVendors = true;
        this.iIGGeneralMessagesService.PostImporterDeclarationRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
            if (!Tools_1.AppTool.IsNullOrEmpty(myServiceResponse.Result)) {
                //let tstPeriodDeclarationList = [];
                //let dmmy = { PeriodDeclarationID: "70227976", "VendorID": "2873975", "VendorName": "PANDUIT LTD", "CreateDate": "01.11.2017", "ValidityFrom": "01.11.2017", "ExpirationDate": "31.10.2018", "Status": "1", "StatusName": "טרם נבדק", "DocumentID": "562775218" };
                //tstPeriodDeclarationList.push(dmmy);
                //myServiceResponse.Result.PeriodDeclarationList = tstPeriodDeclarationList;
                if (myServiceResponse.Result.PeriodDeclarationList != null && myServiceResponse.Result.PeriodDeclarationList.length > 0) {
                    _this._HavePeriodDecResult = true;
                    _this.IsChecked = true;
                    _this.ShowOnlyValid = true;
                    _this._PeriodDeclarationListSave = myServiceResponse.Result.PeriodDeclarationList;
                    //this._PeriodDeclarationList.InsertCollection(myServiceResponse.Result.PeriodDeclarationList);
                    _this.FilterLocal();
                }
                if ((myServiceResponse.Result.PeriodDeclarationList != null && myServiceResponse.Result.PeriodDeclarationList.length > 0)
                    || myServiceResponse.Result.HasException == true) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Title = "עדכון תצהירים מהמכס";
                    messageWindow.Width = 250;
                    messageWindow.Height = 150;
                    messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                    messageWindow.Show(myServiceResponse.Result.UserMessage);
                }
            }
            //this.LoadData();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    VendorExtendedSearchComponent.prototype.SearchAddVendorRequest = function (rowData) {
        var _this = this;
        this.SelectedRow = rowData;
        if (!Tools_1.AppTool.IsNullOrEmpty(rowData.DBVendorID)) {
            var mm = new MessageWindow_1.MessageWindow();
            mm.Show("קיים במערכת");
            return;
        }
        var searchParams = new VendorSearchByCustomsAgentRequestParams_1.VendorSearchByCustomsAgentRequestParams();
        searchParams.IsFakeResponse = true;
        searchParams.LoggingEnabled = false;
        searchParams.RequestName = "Search For Vendor Request";
        searchParams.ResponseName = "Search For Vendor Response";
        searchParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        ///searchParams.VendorName = rowData.VendorName;
        searchParams.VendorNumber = rowData.VendorID; // odi said ENOUGH
        searchParams.VendorTypeCode = null;
        ////   //{"$id":"1","NumberOfResult":null,"VendorResults":null,"HasException":true,"UserMessage":"SendWS failed:FaultException.Detail:FaultException`1\r\nThe content type text/xml of the response message does not match the content type of the binding (application/soap+xml; charset=utf-8). If using a custom encoder, be sure that the IsContentTypeSupported method is implemented properly. The first 39 bytes of the response were: '<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n'.","Succeeded":false,"ContinueProcessInBackground":false,"CustomsRequestsSheetId":"6614faa1-e053-4556-ae24-16825f120d2d","CorrelationId":""}
        CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(searchParams.PBId, "חיפוש ספק", true)
            .then(function (myServiceResponse) {
            console.log("[Send] Response/ShowProgressBar : ", myServiceResponse);
            var response = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                var VendorResults = response.VendorResults;
                if (response.IsCustomWarning) {
                    //this.CurrentSession.CloseCurrentWindow();
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
                if (VendorResults.length == 1) {
                    if (VendorResults[0].StatusCode == '3') {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Show("VendorResults[0].StatusCode == 3");
                    }
                    else if (VendorResults[0].Exists == "true") {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Show("Already Exists");
                    }
                    else {
                        _this.AddButtonClicked(VendorResults[0]);
                    }
                }
                else {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Show(VendorResults.length.toString() + "מצפה לספק אחד בלבד התקבל ");
                }
            }
            else {
                var message = "Service returned a null response!";
            }
            //this.OnSendCompleted();
        });
    };
    VendorExtendedSearchComponent.prototype.AddButtonClicked = function (item) {
        var _this = this;
        if (item.StatusCode == '3')
            return;
        var errors = [];
        //this.ValidationErrorsList = errors;
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
                    _this.SelectedRow.DBVendorID = newVendor.Id;
                    _this.SelectedRow.DBCountryCode = newVendor.CountryCode;
                    // after success
                }
                else {
                    var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                    confirmWindow.Show(res.ErrorsArray[0]);
                    //this.ValidationErrorsList = res.ErrorsArray;
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            //this.ValidationErrorsList = errors;
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(errors[0]);
        }
    };
    VendorExtendedSearchComponent.prototype.onPeriodDeclarationCellSelected = function (event, item) {
        if (Tools_1.AppTool.IsNullOrEmpty(item.DBVendorID)) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            //messageWindow.Title = "עדכון תצהירים מהמכס";
            messageWindow.Show("ספק לא הוקם ");
        }
        else {
            this.SelectedRow = item;
            this.CurrentSession.CloseCurrentWindowEmit("close");
            //periodDec.DBVendorID = dbVendor.Id;
            //periodDec.DBCountryCode = dbVendor.CountryCode;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], VendorExtendedSearchComponent.prototype, "MenuHeaderchangeevent", void 0);
    VendorExtendedSearchComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VendorExtendedSearchComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], VendorExtendedSearchComponent);
    return VendorExtendedSearchComponent;
}(BaseComponent_1.BaseComponent));
exports.VendorExtendedSearchComponent = VendorExtendedSearchComponent;
//# sourceMappingURL=VendorExtendedSearchComponent.js.map