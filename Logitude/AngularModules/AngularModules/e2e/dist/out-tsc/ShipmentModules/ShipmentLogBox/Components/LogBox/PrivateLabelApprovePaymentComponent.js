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
var CustomNumbersPipe_1 = require("../../../../Infrastructure/Pipes/CustomNumbersPipe");
var ShipmentPM_1 = require("../../../../Shipment/EntityPMs/ShipmentPM");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DocumentsFilingExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var ShipmentAdditionalCloudDataService_1 = require("../../../../Shipment/Services/Others/ShipmentAdditionalCloudDataService");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DocumentTypeMetaDataExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var PrivateLabelApprovePaymentComponent = /** @class */ (function (_super) {
    __extends(PrivateLabelApprovePaymentComponent, _super);
    function PrivateLabelApprovePaymentComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.messageWindow = new MessageWindow_1.MessageWindow();
        _this.EntityPm = new ShipmentPM_1.ShipmentPM();
        _this.DimApproveButton = false;
        _this.DimDenyButton = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isAccepted = false;
        _this.ValidationWarningsList = null;
        _this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        _this._ShipmentAdditionalCloudDataService = new ShipmentAdditionalCloudDataService_1.ShipmentAdditionalCloudDataService();
        _this._ImageLibraryService = new ImageLibraryService_1.ImageLibraryService();
        _this._DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService_1.DocumentTypeMetaDataExtendedService();
        return _this;
    }
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "IsAccepted", {
        get: function () { return this.isAccepted; },
        set: function (newValue) { this.isAccepted = newValue; },
        enumerable: true,
        configurable: true
    });
    PrivateLabelApprovePaymentComponent.prototype.IsAcceptedChanged = function ($event) {
        this.IsAccepted = $event;
    };
    PrivateLabelApprovePaymentComponent.prototype.ngOnInit = function () {
    };
    PrivateLabelApprovePaymentComponent.prototype.ngAfterViewInit = function () {
    };
    PrivateLabelApprovePaymentComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (args.EntityPm) {
            this.EntityPm = args.EntityPm;
            this.AdditionalData = args.AdditionalData;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.AdditionalData.DenyReason)) {
                this.DimDenyButton = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.AdditionalData.ApprovedByUserName) && !Tools_1.AppTool.IsNullOrEmpty(this.AdditionalData.VersionApproved) && (this.AdditionalData.VersionApproved == this.AdditionalData.VersionId)) {
                var today = new Date(this.AdditionalData.ApproveDateTime);
                var d = today.getDate();
                var m = today.getMonth() + 1; //January is 0!
                var dd = "";
                var mm = "";
                var yyyy = today.getFullYear().toString();
                if (d < 10) {
                    dd = '0' + d;
                }
                else {
                    dd = d.toString();
                }
                if (m < 10) {
                    mm = '0' + m;
                }
                else {
                    mm = m.toString();
                }
                var to = dd + '/' + mm + '/' + yyyy;
                this.ValidationWarningsList = "גרסת הצהרה זו כבר אושרה על ידי " + this.AdditionalData.ApprovedByUserName + " בתאריך " + to + "";
                this.DimApproveButton = true;
            }
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Shipment"; })[0];
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
            this._documentsFilingExtendedPMService.getAllDocumentsFilingsByEntityIdAndObjectTable(this.EntityPm.Id, ObjectTable.Id, "I", SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var Result = []; //DocumentTypeMetaDataExtendedService
                Result = res.Result.filter(function (a) { return a.IsDeleted == false; });
                _this.externalDocs = [];
                var SupplierInvoice = Result.filter(function (a) { return a.DocumentTypeCode == "380" || a.DocumentTypeCode == "721"; });
                var Others = Result.filter(function (a) { return a.DocumentTypeCode != "721" && a.DocumentTypeCode != "380"; });
                var tempSupplierInvoice = [];
                var tempOthers = [];
                var DRELID = "";
                _this._DocumentTypeMetaDataExtendedService.GetDocumentsMetaDataTypeByCode("DREL").subscribe(function (myResult) {
                    if (myResult.Result) {
                        DRELID = myResult.Result.Id;
                        if (!Tools_1.AppTool.IsNullOrEmpty(DRELID)) {
                            SupplierInvoice.forEach(function (mydoc) {
                                var DRELTypes = mydoc.DocumentsFilingMetaDataValues.filter(function (a) { return a.DocumentsMetaDataTypeId == DRELID; });
                                if (DRELTypes != null && DRELTypes.length > 0) {
                                    tempSupplierInvoice.push(mydoc);
                                }
                            });
                            Others.forEach(function (docin) {
                                var DRELTypes = docin.DocumentsFilingMetaDataValues.filter(function (a) { return a.DocumentsMetaDataTypeId == DRELID; });
                                if (DRELTypes != null && DRELTypes.length > 0) {
                                    tempOthers.push(docin);
                                }
                            });
                        }
                    }
                });
                //else {
                //    tempSupplierInvoice = SupplierInvoice;
                //    tempOthers = Others;
                //}
                _this.externalDocs.push({ key: "חשבונות ספק ורשימות אריזה", value: tempSupplierInvoice });
                _this.externalDocs.push({ key: "מסמכים נוספים", value: tempOthers });
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }, function (error) {
                var dd = error;
            });
        }
    };
    PrivateLabelApprovePaymentComponent.prototype.ApproveButtonClicked = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Approving ...");
        this.ValidationWarningsList = null;
        this._ShipmentAdditionalCloudDataService.getsingledata(this.EntityPm.Id).subscribe(function (AdditionalResult) {
            var entity = AdditionalResult.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(entity.ApprovedByUserName) || !Tools_1.AppTool.IsNullOrEmpty(entity.DenyReason)) {
                _this.messageWindow.RTL = true;
                _this.messageWindow.Width = 300;
                _this.messageWindow.Height = 150;
                _this.messageWindow.Title = "אזהרה!";
                _this.messageWindow.Message = "גרסה זו כבר אושרה על ידי משתמש אחר";
                _this.messageWindow.Show(_this.messageWindow.Message);
                //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
            else {
                entity.ApprovedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                _this._ShipmentAdditionalCloudDataService.update(entity).subscribe(function (AdditionalResult) {
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Approve Declaration");
                    _this.DimApproveButton = true;
                    var today = new Date();
                    var d = today.getDate();
                    var m = today.getMonth() + 1; //January is 0!
                    var dd = "";
                    var mm = "";
                    var yyyy = today.getFullYear().toString();
                    if (d < 10) {
                        dd = '0' + d;
                    }
                    else {
                        dd = d.toString();
                    }
                    if (m < 10) {
                        mm = '0' + m;
                    }
                    else {
                        mm = m.toString();
                    }
                    var to = dd + '/' + mm + '/' + yyyy;
                    _this.messageWindow.RTL = true;
                    _this.messageWindow.Width = 300;
                    _this.messageWindow.Height = 150;
                    _this.messageWindow.Title = "הצהרה אושרה";
                    _this.messageWindow.Message = "אישור הצהרה נשלח ל -" + SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                    _this.messageWindow.Show(_this.messageWindow.Message);
                    //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                });
            }
        });
    };
    PrivateLabelApprovePaymentComponent.prototype.DenyButtonClicked = function () {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("...");
        var newWindow = new LogitudeWindow_1.LogitudeWindow();
        newWindow.Width = 350;
        newWindow.Height = 220;
        newWindow.RTL = true;
        this._ShipmentAdditionalCloudDataService.getsingledata(this.EntityPm.Id).subscribe(function (AdditionalResult) {
            var entity = AdditionalResult.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(entity.DenyReason) || !Tools_1.AppTool.IsNullOrEmpty(entity.ApprovedByUserName)) {
                _this.messageWindow.RTL = true;
                _this.messageWindow.Width = 300;
                _this.messageWindow.Height = 150;
                _this.messageWindow.Title = "אזהרה!";
                _this.messageWindow.Message = "גרסה זו כבר נדחתה על ידי משתמש אחר";
                _this.messageWindow.Show(_this.messageWindow.Message);
                //this.ValidationWarningsList = " גרסת הצהרה זו אושרה על ידי המשתמש " + entity.ApprovedByUserName + " בתאריך " + to;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
            else {
                newWindow.Title = "הסבר לדחיית הצהרה";
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var windowArgs = {};
                windowArgs.AdditionalData = entity;
                newWindow.WindowArgs = windowArgs;
                //newWindow.Add(control); 
                newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/DenyReasonComponent');
                newWindow.WindowClosed.subscribe(function ($event) {
                    if ($event == "Denied") {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("LogBox", "Deny Declaration");
                        _this.DimDenyButton = true;
                        _this.CurrentSession.CloseCurrentWindow();
                        _this.messageWindow.RTL = true;
                        _this.messageWindow.Width = 300;
                        _this.messageWindow.Height = 150;
                        _this.messageWindow.Title = "הצהרה נדחתה";
                        _this.messageWindow.Message = "דחיית הצהרה נשלח ל -" + SessionLocator_1.SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                        _this.messageWindow.Show(_this.messageWindow.Message);
                    }
                });
            }
        });
    };
    PrivateLabelApprovePaymentComponent.prototype.DownloadDocumentFile = function (item) {
        var _this = this;
        this._ImageLibraryService.DownloadFile(item.DocumentId, item.FileExtension, item.Folder, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var EntityNumber = "";
            if (_this.EntityPm != null) {
                EntityNumber = _this.EntityPm.ShipmentNumber;
            }
            var documentName = item.DocumentId + "*" + item.DocumentTypeCode + "-" + (!Tools_1.AppTool.IsNullOrEmpty(EntityNumber) ? EntityNumber : item.EntityId) + "-" + item.Code; // +"." + CurrentDocument.Extension;
            DownloadManager_1.DownloadManager.DownloadPage(documentName);
        });
    };
    PrivateLabelApprovePaymentComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "ShipperReference1", {
        get: function () { return this.EntityPm.ShipperReference1; },
        set: function (newValue) { this.EntityPm.ShipperReference1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "CustomerReference1", {
        get: function () { return this.EntityPm.CustomerReference1; },
        set: function (newValue) { this.EntityPm.CustomerReference1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPm.FromPortId; },
        set: function (newValue) { this.EntityPm.FromPortId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "CustomsFileNo", {
        get: function () { return this.AdditionalData.CustomsFileNo; },
        set: function (newValue) { this.AdditionalData.CustomsFileNo = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "DeclarationNo", {
        get: function () { return this.AdditionalData.DeclarationNo; },
        set: function (newValue) { this.AdditionalData.DeclarationNo = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "MishgorDescOfGoods1", {
        get: function () { return this.AdditionalData.MishgorDescOfGoods1; },
        set: function (newValue) { this.AdditionalData.MishgorDescOfGoods1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "GoodsValue", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.GoodsValue, 0); },
        set: function (newValue) { this.AdditionalData.GoodsValue = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "CifValue", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.CifValue, 0); },
        set: function (newValue) { this.AdditionalData.CifValue = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "TotalTax", {
        get: function () { return new CustomNumbersPipe_1.CustomNumbersPipe().transform(this.AdditionalData.TotalTax, 0); },
        set: function (newValue) { this.AdditionalData.TotalTax = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "MishgorPackageQuantity", {
        get: function () { return this.AdditionalData.MishgorPackageQuantity; },
        set: function (newValue) { this.AdditionalData.MishgorPackageQuantity = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "MishgorPackageWeight", {
        get: function () { return this.AdditionalData.MishgorPackageWeight; },
        set: function (newValue) { this.AdditionalData.MishgorPackageWeight = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "IsImporterApprovalRequried", {
        get: function () { return this.AdditionalData.IsImporterApprovalRequried; },
        set: function (newValue) { this.AdditionalData.IsImporterApprovalRequried = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "ApprovedByUserName", {
        get: function () { return this.AdditionalData.ApprovedByUserName; },
        set: function (newValue) { this.AdditionalData.ApprovedByUserName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "Taxtypename", {
        get: function () { return this.AdditionalData.Taxtypename; },
        set: function (newValue) { this.AdditionalData.Taxtypename = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "TaxBasis", {
        get: function () { return this.AdditionalData.TaxBasis; },
        set: function (newValue) { this.AdditionalData.TaxBasis = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "TaxToPay", {
        get: function () { return this.AdditionalData.TaxToPay; },
        set: function (newValue) { this.AdditionalData.TaxToPay = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "TaxAmount", {
        get: function () { return this.AdditionalData.TaxAmount; },
        set: function (newValue) { this.AdditionalData.TaxAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "SupAccount", {
        get: function () { return this.AdditionalData.SupAccount; },
        set: function (newValue) { this.AdditionalData.SupAccount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "IncotermId", {
        get: function () { return this.AdditionalData.IncotermId; },
        set: function (newValue) { this.AdditionalData.IncotermId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "Value", {
        get: function () { return this.AdditionalData.Value; },
        set: function (newValue) { this.AdditionalData.Value = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "CurrencyName", {
        get: function () { return this.AdditionalData.CurrencyName; },
        set: function (newValue) { this.AdditionalData.CurrencyName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "CountryName", {
        get: function () { return this.AdditionalData.CountryName; },
        set: function (newValue) { this.AdditionalData.CountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "SupplierName", {
        get: function () { return this.AdditionalData.SupplierName; },
        set: function (newValue) { this.AdditionalData.SupplierName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PrivateLabelApprovePaymentComponent.prototype, "SupplierFreight", {
        get: function () { return this.AdditionalData.SupplierFreight; },
        set: function (newValue) { this.AdditionalData.SupplierFreight = newValue; },
        enumerable: true,
        configurable: true
    });
    //public get ApprovedByUserName() { return this.AdditionalData.ApprovedByUserName }
    //public set ApprovedByUserName(newValue: string) { this.AdditionalData.ApprovedByUserName = newValue; }
    PrivateLabelApprovePaymentComponent.prototype.GoodsValueClick = function () {
        var newWindow = new LogitudeWindow_1.LogitudeWindow();
        newWindow.Width = 722;
        newWindow.Height = 230;
        newWindow.RTL = true;
        newWindow.Title = "פרטי חשבון ספק";
        var windowArgs = {};
        //windowArgs.IsNew = false;
        windowArgs.AdditionalData = this.AdditionalData;
        newWindow.WindowArgs = windowArgs;
        //newWindow.Add(control); 
        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/GoodsValueComponent');
    };
    PrivateLabelApprovePaymentComponent.prototype.TotalTaxClick = function () {
        var newWindow = new LogitudeWindow_1.LogitudeWindow();
        newWindow.Width = 550;
        newWindow.Height = 230;
        newWindow.RTL = true;
        newWindow.Title = "פרטי מס";
        var windowArgs = {};
        //windowArgs.IsNew = false;
        windowArgs.AdditionalData = this.AdditionalData;
        newWindow.WindowArgs = windowArgs;
        //newWindow.Add(control); 
        newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/TaxScreenComponent');
    };
    PrivateLabelApprovePaymentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PrivateLabelApprovePaymentComponent.html'
        }),
        __metadata("design:paramtypes", [])
    ], PrivateLabelApprovePaymentComponent);
    return PrivateLabelApprovePaymentComponent;
}(BaseComponent_1.BaseComponent));
exports.PrivateLabelApprovePaymentComponent = PrivateLabelApprovePaymentComponent;
//# sourceMappingURL=PrivateLabelApprovePaymentComponent.js.map