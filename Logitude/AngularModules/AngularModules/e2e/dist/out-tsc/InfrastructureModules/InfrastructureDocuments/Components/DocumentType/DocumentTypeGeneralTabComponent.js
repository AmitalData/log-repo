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
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var DocumentTypeTemplatePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService");
var forms_1 = require("@angular/forms");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CountryListService_1 = require("../../../../Common/Services/StandardLists/CountryListService");
var DocumentTypeGeneralTabComponent = /** @class */ (function (_super) {
    __extends(DocumentTypeGeneralTabComponent, _super);
    function DocumentTypeGeneralTabComponent(fb, entityArgs, _documentTypeTemplatePMExtendedService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._documentTypeTemplatePMExtendedService = _documentTypeTemplatePMExtendedService;
        _this.SelectedFormat = "";
        _this.IsEnableFormat = false;
        _this.IsLoadTemplate = false;
        _this.IsVisibile = false;
        _this.DataContext = _this;
        _this.PointerEventsAreaStimulDocument = "none";
        _this.PointerEventsHTMLDocument = "auto";
        _this.OpacityAreaStimulDocument = "0.5";
        _this.OpacityAreaHTMLDocument = "1";
        _this.CountryLists = [];
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.InputFileNameId = "";
        _this.myForm = fb.group({});
        _this.CurrentSession.StartBusyIndicatorLoading();
        return _this;
    }
    DocumentTypeGeneralTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate", 0).subscribe(function (response) {
            _this.EntityPM = _this.entityArgs.EntityPM;
            if (_this.EntityPM) {
                var myService = new CountryListService_1.CountryListService();
                myService.getAllFromCache().subscribe(function (myResponse) {
                    if (!myResponse.HasError && myResponse.Result) {
                        _this.CountryLists = myResponse.Result;
                        if (_this.CountryLists) {
                            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.CountryCode)) {
                                var countryList = _this.CountryLists.filter(function (d) { return d.Code == _this.EntityPM.CountryCode; })[0];
                                if (countryList) {
                                    _this.CountryId = countryList.Id;
                                }
                            }
                        }
                    }
                    _this.IsVisibile = true;
                    _this.Run();
                    _this.LoadTemplate();
                });
            }
        });
    };
    DocumentTypeGeneralTabComponent.prototype.Run = function () {
        var _this = this;
        this.EntityPM.UIProperties.SetEnabled("Code", "DocumentType", false);
        if (this.EntityPM.Code == "SLCIN" || this.EntityPM.Code == "SLCRP") {
            this.EntityPM.UIProperties.SetEnabled("Name", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("IsDocIn", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("IsDocOut", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("CountryCode", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("IsEnabledForCustomers", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("IsCopiedAtSignup", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("InActive", "DocumentType", false);
            this.EntityPM.UIProperties.SetEnabled("DocumentTypeCategoryCode", "DocumentType", false);
            this.IsEnableEdit = false;
        }
        else {
            this.IsEnableEdit = true;
        }
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
                case "TaxReport":
                case "PaymentCheque":
                case "WarehouseRelease":
                case "Airline":
                case "CustomAgent":
                case "Participant":
                case "ShippingAgent":
                case "ShippingLine":
                case "Trucker":
                case "Vendor":
                case "Warehouse":
                    {
                        if (tempList.filter(function (f) { return f.Name == item.Name; }).length == 0) {
                            tempList.push(item);
                        }
                        break;
                    }
            }
        });
        this.ObjectTablesList = tempList.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
        this.ObjectTablesList.forEach(function (item) {
            // if (item.Name == "WarehouseEntry" || item.Name == "WarehouseRelease") {
            //     item.DisplayName = item.Name == "WarehouseEntry" ? "CrossDockEntry" : "CrossDockRelease";
            // }
            // else item.DisplayName = item.Name;
        });
        this.SelectedObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.EntityPM.ObjectTableId; })[0];
        if (!this.SelectedObjectTable) {
            this.SelectedObjectTable = window.ObjectTables[0];
        }
        this.FormatList = [];
        this.FormatList.push("Print");
        this.FormatList.push("Message");
        if (this.EntityPM.TemplateFormatCode == "P") {
            this.SelectedFormat = "Print";
            this.PointerEventsAreaStimulDocument = "auto";
            this.OpacityAreaStimulDocument = "1";
        }
        else if (this.EntityPM.TemplateFormatCode == "M") {
            this.SelectedFormat = "Message";
            //this.PointerEventsHTMLDocument = "auto";
            // this.OpacityAreaHTMLDocument = "1";
        }
        if (!this.EntityPM.IsDocOut) {
            this.PointerEventsAreaStimulDocument = "none";
            this.PointerEventsHTMLDocument = "none";
            this.OpacityAreaStimulDocument = "0.5";
            this.OpacityAreaHTMLDocument = "0.5";
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPEPROPERTIES"))
            this.ShowFeildTenant0 = true;
        else
            this.ShowFeildTenant0 = false;
        if (this.SelectedObjectTable.Name == "Shipment" || this.SelectedObjectTable.Name == "Quote") {
            this.IsShowAdvanceLink = true;
        }
        else
            this.IsShowAdvanceLink = false;
        if (this.EntityPM.IsDocOut) {
            this.IsEnableFormat = true;
        }
    };
    DocumentTypeGeneralTabComponent.prototype.AdvanceLinkMethod = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 400;
        logitudeWindow.Height = 380;
        logitudeWindow.Title = "Advance";
        logitudeWindow.DataContext = this.EntityPM;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/AdvanceDocumentTypeComponent');
    };
    DocumentTypeGeneralTabComponent.prototype.FormatValueChanged = function (format) {
        this.PointerEventsAreaStimulDocument = "none";
        this.OpacityAreaStimulDocument = "0.5";
        if (format == "Print") {
            this.EntityPM.TemplateFormatCode = "P";
            this.PointerEventsAreaStimulDocument = "auto";
            this.OpacityAreaStimulDocument = "1";
        }
        else {
            this.EntityPM.TemplateFormatCode = "M";
        }
    };
    DocumentTypeGeneralTabComponent.prototype.ObjectTableValueChanged = function (table) {
        if (table) {
            this.EntityPM.ObjectTableName = table.Name;
            this.EntityPM.ObjectTableId = table.Id;
            if (table.Name == "Shipment" || table.Name == "Quote") {
                this.IsShowAdvanceLink = true;
            }
            else
                this.IsShowAdvanceLink = false;
        }
        else {
            this.EntityPM.ObjectTableId = null;
            this.SelectedObjectTable = null;
        }
    };
    DocumentTypeGeneralTabComponent.prototype.IsDocOutChange = function (isdocu) {
        if (isdocu) {
            this.IsEnableFormat = true;
            this.PointerEventsHTMLDocument = "auto";
            this.OpacityAreaHTMLDocument = "1";
            if (this.EntityPM.TemplateFormatCode == "P") {
                this.PointerEventsAreaStimulDocument = "auto";
                this.OpacityAreaStimulDocument = "1";
            }
        }
        else {
            this.IsEnableFormat = false;
            this.PointerEventsAreaStimulDocument = "none";
            this.PointerEventsHTMLDocument = "none";
            this.OpacityAreaStimulDocument = "0.5";
            this.OpacityAreaHTMLDocument = "0.5";
        }
    };
    DocumentTypeGeneralTabComponent.prototype.InputFileNameIdGenerated = function (id) {
        this.InputFileNameId = id;
    };
    DocumentTypeGeneralTabComponent.prototype.AddDataField = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ObjectTableId)) {
            var tableId = "";
            var table = window.ObjectTables.filter(function (d) { return d.Id == _this.EntityPM.ObjectTableId; })[0];
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
                        _this.EntityPM.FileName = insertAtSubject(_this.InputFileNameId, $event);
                    }
                });
            });
        }
    };
    DocumentTypeGeneralTabComponent.prototype.LoadTemplate = function () {
        var _this = this;
        this._documentTypeTemplatePMExtendedService.GetDocumentTypeTemplatesPMForDocumentType(this.EntityPM.Id, this.EntityPM.TemplateFormatCode, this.EntityPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    _this.DocumentTypeTemplates = result;
                }
            }
            _this.IsLoadTemplate = true;
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DocumentTypeGeneralTabComponent.prototype.CountrySelectedChange = function (value) {
        if (value) {
            this.EntityPM.CountryCode = value.Code;
        }
        else
            this.EntityPM.CountryCode = "";
    };
    DocumentTypeGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentTypeGeneral',
            templateUrl: './DocumentTypeGeneralTabComponent.html',
            providers: [DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService],
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder, EntityArgs_1.EntityArgs, DocumentTypeTemplatePMExtendedService_1.DocumentTypeTemplatePMExtendedService])
    ], DocumentTypeGeneralTabComponent);
    return DocumentTypeGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentTypeGeneralTabComponent = DocumentTypeGeneralTabComponent;
//# sourceMappingURL=DocumentTypeGeneralTabComponent.js.map