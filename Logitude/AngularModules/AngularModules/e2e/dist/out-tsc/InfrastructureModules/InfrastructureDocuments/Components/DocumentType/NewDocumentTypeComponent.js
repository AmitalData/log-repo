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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DocumentTypePM_1 = require("../../../../Common/EntityPMs/DocumentTypePM");
var ClassLevelValidator_1 = require("../../../../Infrastructure/Validators/ClassLevelValidator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DocumentTypePMService_1 = require("../../../../Common/Services/StandardPMs/DocumentTypePMService");
var DocumentTypePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var forms_1 = require("@angular/forms");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var NewDocumentTypeComponent = /** @class */ (function (_super) {
    __extends(NewDocumentTypeComponent, _super);
    function NewDocumentTypeComponent(fb, _documentTypePMExtendedService) {
        var _this = _super.call(this) || this;
        _this._documentTypePMExtendedService = _documentTypePMExtendedService;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.NewDocumentTypePM = new DocumentTypePM_1.DocumentTypePM();
        _this.TemplateFormatCode = "";
        _this.IsEnableFormat = false;
        _this.ShowFeildTenant0 = true;
        _this.IsNewEntityCall = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsShowAdvanceLink = false;
        _this.ExsitCode = "";
        _this.InputFileNameId = "";
        if (_this.documentTypePMService == null) {
            _this.documentTypePMService = new DocumentTypePMService_1.DocumentTypePMService();
        }
        _this.myForm = fb.group({});
        _this.validator = new ClassLevelValidator_1.ClassLevelValidator();
        return _this;
    }
    NewDocumentTypeComponent.prototype.ngOnInit = function () {
        this.Run();
    };
    NewDocumentTypeComponent.prototype.SetWindowArgs = function (args) {
    };
    NewDocumentTypeComponent.prototype.Run = function () {
        this.NewDocumentTypePM.DocumentTypeCategoryCode = "O";
        this.NewDocumentTypePM.Tenant = InfraSettings_1.InfraSettings.TenantPM.Id;
        //this.NewDocumentTypePM.TemplateFormatCode = "P";
        var tempList = [];
        window.ObjectTables.forEach(function (item) {
            switch (item.Name) {
                case "Shipment":
                case "Master":
                case "Quote":
                case "Opportunity":
                case "Ticket":
                case "APInvoice":
                case "ARInvoice":
                case "APPayment":
                case "ARPayment":
                case "Agent":
                case "Customer":
                case "Customs.Declaration":
                case "Customs.CheckRepresentativeType":
                case "LogitudeMessagesTransmissionLog":
                case "SharedLogistics":
                case "ShipmentPickUpDelivery":
                case "Journal":
                case "BankDeposit":
                case "GLAccount":
                case "WarehouseEntry":
                case "PaymentCheque":
                case "WarehouseRelease":
                case "TaxReport":
                case "TaxDeductionReport":
                case "Airline":
                case "CustomAgent":
                case "Participant":
                case "ShippingAgent":
                case "ShippingLine":
                case "Trucker":
                case "Vendor":
                case "Warehouse":
                case "OpenFormatReport":
                    {
                        if (tempList.filter(function (f) { return f.Name == item.Name; }).length == 0) {
                            tempList.push(item);
                        }
                        break;
                    }
            }
        });
        this.ObjectTablesList = tempList.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
        //this.ObjectTablesList.forEach((item) => {
        //    if (item.Name == "WarehouseEntry" || item.Name == "WarehouseRelease") {
        //        item.DisplayName = item.Name == "WarehouseEntry" ? "CrossDockEntry" : "CrossDockRelease"; 
        //    }
        //   else item.DisplayName = item.Name;
        //});
        this.SelectedObjectTable = this.ObjectTablesList[0];
        this.NewDocumentTypePM.ObjectTableId = this.SelectedObjectTable.Id;
        this.NewDocumentTypePM.ObjectTableName = this.SelectedObjectTable.Name;
        this.FormatList = [];
        this.FormatList.push("Print");
        this.FormatList.push("Message");
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPEPROPERTIES"))
            this.ShowFeildTenant0 = true;
        else
            this.ShowFeildTenant0 = false;
    };
    NewDocumentTypeComponent.prototype.FormatValueChanged = function (format) {
        if (format == "Print")
            this.NewDocumentTypePM.TemplateFormatCode = "P";
        else
            this.NewDocumentTypePM.TemplateFormatCode = "M";
    };
    NewDocumentTypeComponent.prototype.CodeLostFocusMethod = function (code) {
        var _this = this;
        if (code && this.ExsitCode != code) {
            this.ExsitCode = code;
            this._documentTypePMExtendedService.GetDoesDocumentTypeCodeExist(code, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult == true) {
                        _this.ValidationErrorsList = [];
                        _this.ValidationErrorsList.push("The code " + code + " already exists");
                    }
                    else
                        _this.ValidationErrorsList = [];
                }
            });
        }
    };
    NewDocumentTypeComponent.prototype.ObjectTableValueChanged = function (table) {
        if (table) {
            this.NewDocumentTypePM.ObjectTableName = table.Name;
            this.NewDocumentTypePM.ObjectTableId = table.Id;
            if (table.Name == "Shipment" || table.Name == "Quote") {
                this.IsShowAdvanceLink = true;
            }
            else
                this.IsShowAdvanceLink = false;
        }
        else {
            this.NewDocumentTypePM.ObjectTableId = null;
            this.SelectedObjectTable = null;
        }
    };
    NewDocumentTypeComponent.prototype.IsDocOutChange = function (isdoc) {
        if (isdoc)
            this.IsEnableFormat = true;
        else
            this.IsEnableFormat = false;
    };
    NewDocumentTypeComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var errorsArray = this.validator.Validate("DocumentType", this.NewDocumentTypePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach(function (item) {
                _this.ValidationErrorsList.push(item);
            });
        }
        if (!this.NewDocumentTypePM.IsDocIn && !this.NewDocumentTypePM.IsDocOut) {
            this.ValidationErrorsList.push("Please chose Doc in or Doc out");
        }
        if (this.NewDocumentTypePM.ObjectTableName == "Shipment" || this.NewDocumentTypePM.ObjectTableName == "Quote") {
            var valid = ((this.NewDocumentTypePM.IsAir) || (this.NewDocumentTypePM.IsOcean) || (this.NewDocumentTypePM.IsInland));
            if (!valid) {
                this.ValidationErrorsList.push("Please choose the transportation method of the document type");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this.documentTypePMService.insert(this.NewDocumentTypePM).subscribe(function (res) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        _this.CurrentSession.CloseCurrentWindow();
                        CachedDataManager_1.CachedDataManager.RefreshTableData("DocumentType", true);
                    }
                }
                else {
                    pmResponse.ErrorsArray.forEach(function (item) {
                        _this.ValidationErrorsList.push(item);
                    });
                }
            });
        }
    };
    NewDocumentTypeComponent.prototype.AdvanceLinkMethod = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 400;
        logitudeWindow.Height = 380;
        logitudeWindow.Title = "Advance";
        logitudeWindow.DataContext = this.NewDocumentTypePM;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/AdvanceDocumentTypeComponent');
    };
    NewDocumentTypeComponent.prototype.InputFileNameIdGenerated = function (id) {
        this.InputFileNameId = id;
    };
    NewDocumentTypeComponent.prototype.AddDataField = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.NewDocumentTypePM.ObjectTableId)) {
            var tableId = "";
            var table = window.ObjectTables.filter(function (d) { return d.Id == _this.NewDocumentTypePM.ObjectTableId; })[0];
            if (table)
                tableId = table.Id;
            this._entityResourceService.getEntityResourceByTableName(table.Name).subscribe(function (response) {
                var windowArgs = {};
                windowArgs.ObjectTypeField = "DocuemntFileName";
                windowArgs.HideSystemDataTab = true;
                windowArgs.ObjectTableId = tableId;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 600;
                logWindow.Title = "Insert Data Field";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    if ($event) {
                        _this.NewDocumentTypePM.FileName = insertAtSubject(_this.InputFileNameId, $event);
                    }
                });
            });
        }
    };
    NewDocumentTypeComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewDocumentTypeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'NewDocumentType',
            templateUrl: './NewDocumentTypeComponent.html',
            providers: [DocumentTypePMService_1.DocumentTypePMService, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService]
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder, DocumentTypePMExtendedService_1.DocumentTypePMExtendedService])
    ], NewDocumentTypeComponent);
    return NewDocumentTypeComponent;
}(BaseComponent_1.BaseComponent));
exports.NewDocumentTypeComponent = NewDocumentTypeComponent;
//# sourceMappingURL=NewDocumentTypeComponent.js.map