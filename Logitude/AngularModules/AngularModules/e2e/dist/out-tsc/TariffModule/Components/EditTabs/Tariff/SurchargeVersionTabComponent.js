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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var DocumentsFilingExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var TariffDomainService_1 = require("../../../Services/TariffDomainService");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var TariffLinePM_1 = require("../../../EntityPMs/TariffLinePM");
var TariffVersionPM_1 = require("../../../EntityPMs/TariffVersionPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var MeasurementListService_1 = require("../../../../Common/Services/StandardLists/MeasurementListService");
var common_1 = require("@angular/common");
var TariffVersionExtendedPMService_1 = require("../../../Services/ExtendedPMs/TariffVersionExtendedPMService");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var Args_1 = require("../../../Args");
var SurchargeVersionTabComponent = /** @class */ (function (_super) {
    __extends(SurchargeVersionTabComponent, _super);
    function SurchargeVersionTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Tariff";
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.IsApproveVersionButtonVisible = false;
        _this.IsDraftVersion = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsUpdateSurchargesButtonVisible = false;
        _this.IsFirstDraft = false;
        _this.SaveCompletedEvent = null;
        _this.tariffCharges = [];
        _this.ItemsCollection = [];
        _this.DeletedTariffsLines = [];
        _this.isComparToChecked = false;
        _this.isUploadExcelFinished = false;
        _this.isApproveButtonClicked = false;
        _this.isCopyButtonClicked = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.Listen();
        return _this;
    }
    SurchargeVersionTabComponent.prototype.Intialize = function (args) {
        var _this = this;
        this.TariffsLinesSource = new ObservableCollection_1.ObservableCollection([]);
        this.DocumentExtendedService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        this.TariffDomainService = new TariffDomainService_1.TariffDomainService();
        this.CurrentVersion = args['CurrentVersion'];
        if (this.CurrentVersion != null) {
            this.IsDraftVersion = this.CurrentVersion.IsDraft;
        }
        if (this.IsDraftVersion) {
            this.IsComparToChecked = true;
        }
        this.GetTariffSettings();
        var iChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        var iMeasurementListService = new MeasurementListService_1.MeasurementListService();
        iChargesTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllChargesTypes = myResponse.Result;
                iMeasurementListService.getAllFromCache().subscribe(function (myResponse2) {
                    if (!myResponse2.HasError) {
                        _this.AllMeasurements = myResponse2.Result;
                        _this.LoadCompareToVersions();
                        _this.SetUIProperties();
                        _this.SetSurchargesLabelsAndVisibility();
                        if (_this.CurrentVersion.IsDraft) {
                            _this.FillTariffLines(_this.CurrentVersion.TariffLines);
                        }
                        else {
                            _this.LoadTariffLines("currentVersion");
                        }
                    }
                });
            }
        });
    };
    SurchargeVersionTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.CurrentVersion = _this.EntityPM.TariffVersions.filter(function (d) { return d.Version == _this.CurrentVersion.Version; })[0];
                    if (_this.CurrentVersion == null) {
                        // after creating new version
                        _this.CurrentVersion = _this.EntityPM.TariffVersions.filter(function (d) { return d.Version == _this.EntityPM.LastVersion; })[0];
                    }
                    if (_this.CurrentVersion.IsDraft) {
                        _this.FillTariffLines(_this.CurrentVersion.TariffLines);
                    }
                    else {
                        _this.LoadTariffLines("currentVersion");
                    }
                    if (_this.isApproveButtonClicked) {
                        _this.isApproveButtonClicked = false;
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                    if (_this.isCopyButtonClicked) {
                        _this.isCopyButtonClicked = false;
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                    if (_this.isUploadExcelFinished) {
                        _this.isUploadExcelFinished = false;
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                    _this.SetSurchargesLabelsAndVisibility();
                }
            });
        }
    };
    SurchargeVersionTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
    };
    SurchargeVersionTabComponent.prototype.GetTariffSettings = function () {
        var _this = this;
        this.TariffDomainService.GetTenantTariffSetting().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.warningPercentage = myResponse.Result.DefaultWarningPercentage;
            }
        });
    };
    SurchargeVersionTabComponent.prototype.LoadTariffLines = function (type) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (type == "currentVersion") {
            this.TariffDomainService.GetTariffVersionLines(this.EntityPM.Id, this.CurrentVersion.Version).subscribe(function (response) {
                if (!response.HasError) {
                    _this.loadedTariffLines = response.Result;
                    _this.FillTariffLines(_this.loadedTariffLines);
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        else if (type == "compareVersion") {
            this.TariffDomainService.GetTariffVersionLines(this.EntityPM.Id, this.ComparedToVersionPM.Version).subscribe(function (response) {
                if (!response.HasError) {
                    _this.compareTariffLines = response.Result;
                    _this.DoCompare();
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    SurchargeVersionTabComponent.prototype.SetUIProperties = function () {
        var isApproveVersionButtonVisible = false;
        var isUpdateSurchargesButtonVisible = false;
        if (this.IsDraftVersion) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARRIFAPPROVEVERSION")) {
                isApproveVersionButtonVisible = true;
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "UPDATESURCHARGES")) {
                isUpdateSurchargesButtonVisible = true;
            }
        }
        this.IsApproveVersionButtonVisible = isApproveVersionButtonVisible;
        this.IsUpdateSurchargesButtonVisible = isUpdateSurchargesButtonVisible;
    };
    SurchargeVersionTabComponent.prototype.SetSurchargesLabelsAndVisibility = function () {
        this.tariffCharges = [];
        this.AddChargeColumn(this.EntityPM.Surcharge1Id, this.EntityPM.Surcharge1UOM, 1);
        this.AddChargeColumn(this.EntityPM.Surcharge2Id, this.EntityPM.Surcharge2UOM, 2);
        this.AddChargeColumn(this.EntityPM.Surcharge3Id, this.EntityPM.Surcharge3UOM, 3);
        this.AddChargeColumn(this.EntityPM.Surcharge4Id, this.EntityPM.Surcharge4UOM, 4);
        this.AddChargeColumn(this.EntityPM.Surcharge5Id, this.EntityPM.Surcharge5UOM, 5);
        this.AddChargeColumn(this.EntityPM.Surcharge6Id, this.EntityPM.Surcharge6UOM, 6);
        this.AddChargeColumn(this.EntityPM.Surcharge7Id, this.EntityPM.Surcharge7UOM, 7);
        this.AddChargeColumn(this.EntityPM.Surcharge8Id, this.EntityPM.Surcharge8UOM, 8);
        this.AddChargeColumn(this.EntityPM.Surcharge9Id, this.EntityPM.Surcharge9UOM, 9);
        this.AddChargeColumn(this.EntityPM.Surcharge10Id, this.EntityPM.Surcharge10UOM, 10);
    };
    SurchargeVersionTabComponent.prototype.AddChargeColumn = function (iChargeTypeId, iMeasurementId, index) {
        if (!Tools_1.AppTool.IsNullOrEmpty(iChargeTypeId)) {
            var iChargeType = this.AllChargesTypes.filter(function (a) { return a.Id == iChargeTypeId; })[0];
            if (iChargeType) {
                var item = new CodeNameClass_1.CodeNameClass();
                item.Code = iChargeType.Id;
                item.Name = iChargeType.Code;
                item.DisplyText = iChargeType.Code;
                item.Code_Int = index;
                var iMeasurement = this.AllMeasurements.filter(function (f) { return f.Id == iMeasurementId; })[0];
                if (iMeasurement) {
                    item.DisplyText = iChargeType.Code + " (" + iMeasurement.Code + ")";
                }
                this.tariffCharges.push(item);
                this['Surcharge' + index + 'PriceLabel'] = item.DisplyText;
                this['Surcharge' + index + 'PriceVisibility'] = true;
            }
        }
    };
    Object.defineProperty(SurchargeVersionTabComponent.prototype, "VersionNumber", {
        get: function () {
            return (this.CurrentVersion == null ? null : this.CurrentVersion.Version);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SurchargeVersionTabComponent.prototype, "StartDate", {
        get: function () {
            return (this.CurrentVersion == null ? null : this.CurrentVersion.StartDate);
        },
        set: function (value) {
            if (this.CurrentVersion.StartDate != value) {
                this.CurrentVersion.StartDate = value;
                this.UpdateDates("start", value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SurchargeVersionTabComponent.prototype, "ExpirationDate", {
        get: function () {
            return (this.CurrentVersion == null ? null : this.CurrentVersion.ExpirationDate);
        },
        set: function (value) {
            if (this.CurrentVersion.ExpirationDate != value) {
                this.CurrentVersion.ExpirationDate = value;
                this.UpdateDates("expire", value);
            }
        },
        enumerable: true,
        configurable: true
    });
    SurchargeVersionTabComponent.prototype.UpdateDates = function (dateType, date) {
        if (dateType == "start") {
            this.EntityPM.LastStartDate = date;
            this.CurrentVersion.TariffLines.forEach(function (item) {
                item.StartDate = date;
            });
        }
        else if (dateType == "expire") {
            this.EntityPM.LastExpirationDate = date;
            this.CurrentVersion.TariffLines.forEach(function (item) {
                item.ExpirationDate = date;
            });
        }
    };
    SurchargeVersionTabComponent.prototype.FillTariffLines = function (tariffLines) {
        var _this = this;
        if (this.TariffsLinesSource != null) {
            this.TariffsLinesSource.Clear();
        }
        this.ItemsCollection = [];
        tariffLines.sort(function (p) { return p.Index; }).forEach(function (item) {
            _this.ItemsCollection.push(new TariffLineData(item, _this));
        });
        this.TariffsLinesSource.InsertCollection(this.ItemsCollection);
        this.DoCompare();
    };
    SurchargeVersionTabComponent.prototype.DoCompare = function () {
        this.DeletedTariffsLines = [];
        if (this.IsComparToChecked && this.ComparedToVersionPM != null) {
            this.ComaredLines();
            this.BuildDeletedLines();
        }
    };
    SurchargeVersionTabComponent.prototype.ComaredLines = function () {
        var _this = this;
        this.ItemsCollection.forEach(function (item) {
            var line = _this.compareTariffLines.sort(function (p) { return p.Index; }).filter(function (a) { return a.DestinationPortId == item.DestinationPortId && a.OriginPortId == item.OriginPortId; })[0];
            if (line) {
                item.ComparedEntity = line;
                item.SetCellsComparingText();
            }
            else {
                item.IsNewEntity = true;
            }
        });
    };
    SurchargeVersionTabComponent.prototype.BuildDeletedLines = function () {
        var _this = this;
        var lines;
        if (this.CurrentVersion.IsDraft) {
            lines = this.CurrentVersion.TariffLines;
        }
        else {
            lines = this.loadedTariffLines;
        }
        this.compareTariffLines.sort(function (p) { return p.Index; }).forEach(function (item) {
            var line = lines.sort(function (p) { return p.Index; }).filter(function (a) { return a.DestinationPortId == item.DestinationPortId && a.OriginPortId == item.OriginPortId; })[0];
            if (line == null) {
                _this.DeletedTariffsLines.push(new TariffLineData(item, _this)); // Deleted 
            }
        });
    };
    Object.defineProperty(SurchargeVersionTabComponent.prototype, "IsComparToChecked", {
        get: function () {
            return this.isComparToChecked;
        },
        set: function (value) {
            if (this.isComparToChecked != value) {
                this.isComparToChecked = value;
                this.UIProperties.SetEnabled("WarningPercentage", null, value);
                this.ComparingCalculations(false);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SurchargeVersionTabComponent.prototype, "WarningPercentage", {
        get: function () {
            return this.warningPercentage;
        },
        set: function (value) {
            if (this.warningPercentage != value) {
                this.warningPercentage = value;
                this.ComparingCalculations(false);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SurchargeVersionTabComponent.prototype, "SelectedVersion", {
        get: function () { return this.selectedVersion; },
        set: function (value) {
            var _this = this;
            if (this.selectedVersion != value) {
                this.selectedVersion = value;
                this.ComparedToVersionPM = this.compareToVersions.filter(function (d) { return d.Version == _this.SelectedVersion.Version; })[0];
                this.ComparingCalculations(true);
            }
        },
        enumerable: true,
        configurable: true
    });
    SurchargeVersionTabComponent.prototype.LoadCompareToVersions = function () {
        var _this = this;
        var service = new TariffVersionExtendedPMService_1.TariffVersionExtendedPMService();
        service.GetAllTariffVersionsForTariff(this.EntityPM.Id).subscribe(function (response) {
            if (!response.HasError) {
                _this.compareToVersions = response.Result;
                _this.BuildVersionsList();
            }
        });
    };
    SurchargeVersionTabComponent.prototype.BuildVersionsList = function () {
        var _this = this;
        this.VersionsList = [];
        var datePipe = new common_1.DatePipe("en-US");
        this.compareToVersions.filter(function (a) { return a.Version != _this.CurrentVersion.Version; }).forEach(function (item) {
            var newVersion = new VersionClass();
            newVersion.Version = item.Version;
            newVersion.ParentVersionNumber = item.ParentVersionNumber;
            newVersion.Id = item.TariffId;
            if (_this.EntityPM.TypeCode == "ASC") {
                newVersion.Name = "Version " + item.Version;
            }
            else {
                var from = datePipe.transform(item.StartDate, 'dd/MM/yyyy');
                var to = datePipe.transform(item.ExpirationDate, 'dd/MM/yyyy');
                newVersion.Name = "Version " + item.Version + " (" + from + " - " + to + ")";
            }
            _this.VersionsList.push(newVersion);
        });
        this.SelectedVersion = this.VersionsList.filter(function (a) { return a.Version == _this.CurrentVersion.ParentVersionNumber; })[0];
        this.UIProperties.SetEnabled("WarningPercentage", null, this.IsComparToChecked);
        if (this.VersionsList == null || (this.VersionsList != null && this.VersionsList.length == 0)) {
            this.isComparToChecked = false;
            this.IsFirstDraft = true;
        }
    };
    SurchargeVersionTabComponent.prototype.ComparingCalculations = function (load) {
        if (load) {
            this.LoadTariffLines("compareVersion");
        }
        else {
            if (this.CurrentVersion != null && this.CurrentVersion.IsDraft) {
                this.FillTariffLines(this.CurrentVersion.TariffLines);
            }
            else {
                this.FillTariffLines(this.loadedTariffLines);
            }
        }
    };
    SurchargeVersionTabComponent.prototype.AddTariffLine = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var itemPM = new TariffLinePM_1.TariffLinePM(null);
        itemPM.StartDate = this.StartDate;
        itemPM.ExpirationDate = this.ExpirationDate;
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        itemPM.Version = this.CurrentVersion.Version;
        itemPM.Index = 0;
        var Version = this.EntityPM.TariffVersions.filter(function (p) { return p.Version == itemPM.Version; })[0];
        if (Version) {
            if (Version.TariffLines.length > 0) {
                var index = Math.max.apply(Math, Version.TariffLines.map(function (o) { return o.Index; })) + 1;
                if (index) {
                    itemPM.Index = index;
                }
            }
        }
        var itemComponent = new TariffLineData(itemPM, this, true);
        logWindow.DataContext = itemComponent;
        logWindow.Title = "New Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    };
    SurchargeVersionTabComponent.prototype.EditTariffButtonClicked = function (item) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.DataContext = item;
        logWindow.Title = "Edit Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    };
    SurchargeVersionTabComponent.prototype.DeleteTariffButtonClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Delete this Tariff Line?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.CurrentVersion.RemoveTariffLine(item.EntityPM);
                _this.TariffsLinesSource.Remove(item);
                _this.FillTariffLines(_this.CurrentVersion.TariffLines);
            }
        });
    };
    // Upload Excel File 
    SurchargeVersionTabComponent.prototype.OnFileChanged = function (fileEvent) {
        var file = fileEvent.target.files[0];
        if (file) {
            var extension = file.name.split('.')[1];
            if (extension.includes("xls")) {
                var file = fileEvent.target.files[0];
                this.UploadExcel(file);
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("You have to upload excel files only");
            }
        }
    };
    SurchargeVersionTabComponent.prototype.UploadExcel = function (file) {
        var _this = this;
        this.FileName = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(file.name)) {
            var name = file.name.split('.');
            if (name.length == 2) {
                this.FileName = name[0];
            }
        }
        if (file && file.size > 0) {
            this.DocumentExtendedService.GetFileSizeAndUnit(file.size).subscribe(function (response) {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        _this.StartUploadingExcelFile(file);
                    }
                }
            });
        }
    };
    SurchargeVersionTabComponent.prototype.StartUploadingExcelFile = function (file) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    };
    SurchargeVersionTabComponent.prototype.ConvertArrayBufferToBase64 = function (file, context) {
        var reader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            var filter = new TariffDomainService_1.TariffFilterParameter();
            filter.FileData = window.btoa(binary);
            filter.PriceSteps = context.PriceSteps;
            filter.TariffId = context.EntityPM.Id;
            filter.Version = context.CurrentVersion.Version;
            filter.TariffType = context.EntityPM.TypeCode;
            filter.FileName = context.FileName;
            context.SendExcelToServer(filter);
        };
        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
        context.EntityPM.FileUploadedName = this.FileName;
    };
    SurchargeVersionTabComponent.prototype.SendExcelToServer = function (filter) {
        var _this = this;
        this.TariffDomainService.PostUploadExcelFile(filter).subscribe(function (response) {
            if (!response.HasError) {
                var tariffLines = response.Result;
                if (tariffLines) {
                    _this.CurrentVersion.TariffLines = [];
                    _this.InsertNewRowsFromExcel(tariffLines);
                }
            }
        });
    };
    SurchargeVersionTabComponent.prototype.InsertNewRowsFromExcel = function (tariffLines) {
        var _this = this;
        tariffLines.forEach(function (item) {
            var tariffLine = new TariffLinePM_1.TariffLinePM(null);
            tariffLine.StartDate = _this.StartDate;
            tariffLine.ExpirationDate = _this.ExpirationDate;
            tariffLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
            tariffLine.Version = _this.CurrentVersion.Version;
            tariffLine.OriginPortId = item.FromPortId;
            tariffLine.OriginPortCode = item.FromPortCode;
            tariffLine.OriginPortName = item.FromPortName;
            tariffLine.DestinationPortId = item.ToPortId;
            tariffLine.DestinationPortCode = item.ToPortCode;
            tariffLine.DestinationPortName = item.ToPortName;
            tariffLine.OriginPortText = item.FromPortText;
            tariffLine.DestinationPortText = item.ToPortText;
            tariffLine.HasErrors = item.HasErrors;
            tariffLine.ErrorText = item.ErrorText;
            tariffLine.Index = item.Index;
            tariffLine.Notes = item.Notes;
            tariffLine.StartDate = item.StartDate;
            //tariffLine.StartDateText = item.StartDateText;
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge1Id)) {
                tariffLine.Surcharge1Price = item.Surcharge1Price;
                tariffLine.Surcharge1PriceText = item.Surcharge1PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge2Id)) {
                tariffLine.Surcharge2Price = item.Surcharge2Price;
                tariffLine.Surcharge2PriceText = item.Surcharge2PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge3Id)) {
                tariffLine.Surcharge3Price = item.Surcharge3Price;
                tariffLine.Surcharge3PriceText = item.Surcharge3PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge4Id)) {
                tariffLine.Surcharge4Price = item.Surcharge4Price;
                tariffLine.Surcharge4PriceText = item.Surcharge4PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge5Id)) {
                tariffLine.Surcharge5Price = item.Surcharge5Price;
                tariffLine.Surcharge5PriceText = item.Surcharge5PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge6Id)) {
                tariffLine.Surcharge6Price = item.Surcharge6Price;
                tariffLine.Surcharge6PriceText = item.Surcharge6PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge7Id)) {
                tariffLine.Surcharge7Price = item.Surcharge7Price;
                tariffLine.Surcharge7PriceText = item.Surcharge7PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge8Id)) {
                tariffLine.Surcharge8Price = item.Surcharge8Price;
                tariffLine.Surcharge8PriceText = item.Surcharge8PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge9Id)) {
                tariffLine.Surcharge9Price = item.Surcharge9Price;
                tariffLine.Surcharge9PriceText = item.Surcharge9PriceText;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Surcharge10Id)) {
                tariffLine.Surcharge10Price = item.Surcharge10Price;
                tariffLine.Surcharge10PriceText = item.Surcharge10PriceText;
            }
            _this.CurrentVersion.AddTariffLine(tariffLine);
        });
        this.EntityPM.TariffLinesAddedFromExcel = true;
        this.isUploadExcelFinished = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges("Saving...");
    };
    // Download Excel 
    SurchargeVersionTabComponent.prototype.DownloadExcelClicked = function (type) {
        this.TariffDomainService.DownloadTariff(this.EntityPM.Id, this.CurrentVersion.Version, type).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var fileName = myResponse.Result;
                var tempDate = new Date();
                var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
                var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + fileName + "&tempId=" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + "Tariffs" + "_" + MyDate + "&Type=SaveToMicrosoftExcel2007";
                {
                    window.open(url);
                }
            }
        });
    };
    SurchargeVersionTabComponent.prototype.ApproveVersionClicked = function () {
        if (!this.isApproveButtonClicked) {
            this.isApproveButtonClicked = true;
            this.EntityPM.IsApprovingDraftVersion = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        //var errors: string[] = [];
        //if (this.CurrentVersion.TariffLines.filter(d => d.HasErrors).length > 0) {
        //    errors.push("Invalid Tariff Lines");
        //}
        ////if (DateTool.GetDateParts(this.CurrentVersion.ExpirationDate).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
        ////    errors.push("Approving past version is not allowed, please update the dates");
        ////}
        //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        //if (errors.length == 0) {
        //    this.isApproveButtonClicked = true;
        //    if (this.EntityPM.IsDirty) {
        //        this.CurrentSession.CurrentEditComponent.SaveChanges("Saving...");
        //    }
        //    else {
        //        this.DoApprove();
        //    }
        //}
    };
    //private DoApprove() {
    //    this.TariffDomainService.ApproveVersion(this.EntityPM.Id, this.CurrentVersion.Version).subscribe((response: ServiceResponse) => {
    //        if (!response.HasError) {
    //            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    //        }
    //    });
    //}
    SurchargeVersionTabComponent.prototype.CopyVersionClicked = function () {
        if (this.EntityPM.TariffVersions.filter(function (d) { return d.IsDraft; })[0]) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("You can't copy this version since you already have draft one");
        }
        else {
            this.DoCopy();
        }
    };
    SurchargeVersionTabComponent.prototype.DoCopy = function () {
        //var windowTitle = "New Copy Version";
        //var logWindow = new LogitudeWindow();
        //logWindow.Width = 450;
        //logWindow.Height = 200;
        //logWindow.WindowArgs = this.CurrentVersion;
        //logWindow.Title = windowTitle;
        //logWindow.ComponentLoaded.subscribe(s => {
        //logWindow.WindowClosed.subscribe(d => {
        //if (s && d == "ok") {
        this.isCopyButtonClicked = true;
        this.EntityPM.LastVersion = this.EntityPM.LastVersion + 1;
        this.EntityPM.LastStartDate = this.StartDate;
        this.EntityPM.LastExpirationDate = this.ExpirationDate;
        var copiedVersion = new TariffVersionPM_1.TariffVersionPM(this.EntityPM);
        copiedVersion.TariffId = this.CurrentVersion.TariffId;
        copiedVersion.Version = this.EntityPM.LastVersion;
        copiedVersion.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        copiedVersion.CreatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
        //copiedVersion.ExpirationDate = s.ExpirationDate;
        copiedVersion.IsDraft = true;
        //copiedVersion.StartDate = s.StartDate;
        copiedVersion.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        copiedVersion.ParentVersionNumber = this.CurrentVersion.Version;
        this.EntityPM.AddTariffVersion(copiedVersion);
        this.loadedTariffLines.sort(function (p) { return p.Index; }).forEach(function (item) {
            var tariffLine = new TariffLinePM_1.TariffLinePM(copiedVersion);
            tariffLine.StartDate = item.StartDate;
            tariffLine.ExpirationDate = item.ExpirationDate;
            tariffLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
            tariffLine.Version = copiedVersion.Version;
            tariffLine.OriginPortId = item.OriginPortId;
            tariffLine.OriginPortCode = item.OriginPortCode;
            tariffLine.OriginPortName = item.OriginPortName;
            tariffLine.DestinationPortId = item.DestinationPortId;
            tariffLine.DestinationPortCode = item.DestinationPortCode;
            tariffLine.DestinationPortName = item.DestinationPortName;
            tariffLine.Surcharge1Price = item.Surcharge1Price;
            tariffLine.Surcharge2Price = item.Surcharge2Price;
            tariffLine.Surcharge3Price = item.Surcharge3Price;
            tariffLine.Surcharge4Price = item.Surcharge4Price;
            tariffLine.Surcharge5Price = item.Surcharge5Price;
            tariffLine.Surcharge6Price = item.Surcharge6Price;
            tariffLine.Surcharge7Price = item.Surcharge7Price;
            tariffLine.Surcharge8Price = item.Surcharge8Price;
            tariffLine.Surcharge9Price = item.Surcharge9Price;
            tariffLine.Surcharge10Price = item.Surcharge10Price;
            tariffLine.Index = item.Index;
            tariffLine.Notes = item.Notes;
            copiedVersion.AddTariffLine(tariffLine);
        });
        this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
        // }
        //});
        // });
        //logWindow.Show('./TariffModule/Components/EditTabs/Tariff/TariffDatesValidationComponent');
    };
    SurchargeVersionTabComponent.prototype.UpdateSurchargesClicked = function () {
        var _this = this;
        var args = new Args_1.UpdateTariffArgs();
        args.Version = this.CurrentVersion;
        args.TariffCharges = this.tariffCharges;
        args.AirlineId = this.EntityPM.SellerId;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.WindowArgs = args;
        logWindow.Title = "Tariff Surchage Update";
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "ok") {
                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/UpdateSurchargesComponent');
    };
    SurchargeVersionTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SurchargeVersionTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], SurchargeVersionTabComponent);
    return SurchargeVersionTabComponent;
}(BaseComponent_1.BaseComponent));
exports.SurchargeVersionTabComponent = SurchargeVersionTabComponent;
var TariffLineData = /** @class */ (function (_super) {
    __extends(TariffLineData, _super);
    function TariffLineData(entity, FatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.FatherComponent = FatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "TariffLine";
        _this.IsNewEntity = false;
        _this.IsEditEnabled = false;
        _this.Surcharge1ComparingTextColor = null;
        _this.Surcharge2ComparingTextColor = null;
        _this.Surcharge3ComparingTextColor = null;
        _this.Surcharge4ComparingTextColor = null;
        _this.Surcharge5ComparingTextColor = null;
        _this.Surcharge6ComparingTextColor = null;
        _this.Surcharge7ComparingTextColor = null;
        _this.Surcharge8ComparingTextColor = null;
        _this.Surcharge9ComparingTextColor = null;
        _this.Surcharge10ComparingTextColor = null;
        _this.DefaultColor = "blue";
        _this.EntityPM = entity;
        _this.IsNewEntity = isNew;
        _this.IsEditEnabled = FatherComponent.IsDraftVersion;
        _this.SetUIProperties();
        return _this;
    }
    TariffLineData.prototype.CheckIfLineHasError = function () {
        var error = false;
        var errorText;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Port with code " + this.EntityPM.OriginPortText + " not found";
            }
            else {
                errorText = errorText + ", Port with code " + this.EntityPM.OriginPortText + " not found";
            }
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Missing Origin Port";
            }
            else {
                errorText = errorText + ", Missing Origin Port";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Port with code " + this.EntityPM.DestinationPortText + " not found";
            }
            else {
                errorText = errorText + ", Port with code " + this.EntityPM.DestinationPortText + " not found";
            }
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Missing Destination Port";
            }
            else {
                errorText = errorText + ", Missing Destination Port";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 1 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 1 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 2 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 2 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 3 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 3 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 4 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 4 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 5 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 5 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge6PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 6 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 6 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge7PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 7 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 7 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge8PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 8 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 8 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge9PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 9 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 9 price format is invalid";
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge10PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
            error = true;
            if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                errorText = "Surcharge 10 price format is invalid";
            }
            else {
                errorText = errorText + ", Surcharge 10 price format is invalid";
            }
        }
        this.HasErrors = error;
        this.ErrorText = errorText;
    };
    TariffLineData.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DestinationPortId));
    };
    TariffLineData.prototype.SetCellsComparingText = function () {
        this.CompareSurcharge1Price();
        this.CompareSurcharge2Price();
        this.CompareSurcharge3Price();
        this.CompareSurcharge4Price();
        this.CompareSurcharge5Price();
        this.CompareSurcharge6Price();
        this.CompareSurcharge7Price();
        this.CompareSurcharge8Price();
        this.CompareSurcharge9Price();
        this.CompareSurcharge10Price();
    };
    TariffLineData.prototype.CompareSurcharge1Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge1ComparingPrice = null;
            this.Surcharge1ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge1Price != null) {
                var surcharge1ComparingValue = this.Surcharge1Price - this.ComparedEntity.Surcharge1Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge1ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge1Price)) {
                    this.Surcharge1ComparingPrice = (surcharge1ComparingValue / this.ComparedEntity.Surcharge1Price) * 100;
                    this.Surcharge1ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge1ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge2Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge2ComparingPrice = null;
            this.Surcharge2ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge2Price != null) {
                var surcharge2ComparingValue = this.Surcharge2Price - this.ComparedEntity.Surcharge2Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge2ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge2Price)) {
                    this.Surcharge2ComparingPrice = (surcharge2ComparingValue / this.ComparedEntity.Surcharge2Price) * 100;
                    this.Surcharge2ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge2ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge3Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge3ComparingPrice = null;
            this.Surcharge3ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge3Price != null) {
                var surcharge3ComparingValue = this.Surcharge3Price - this.ComparedEntity.Surcharge3Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge3ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge3Price)) {
                    this.Surcharge3ComparingPrice = (surcharge3ComparingValue / this.ComparedEntity.Surcharge3Price) * 100;
                    this.Surcharge3ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge3ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge4Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge4ComparingPrice = null;
            this.Surcharge4ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge4Price != null) {
                var surcharge4ComparingValue = this.Surcharge4Price - this.ComparedEntity.Surcharge4Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge4ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge4Price)) {
                    this.Surcharge4ComparingPrice = (surcharge4ComparingValue / this.ComparedEntity.Surcharge4Price) * 100;
                    this.Surcharge4ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge4ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge5Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge5ComparingPrice = null;
            this.Surcharge5ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge5Price != null) {
                var surcharge5ComparingValue = this.Surcharge5Price - this.ComparedEntity.Surcharge5Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge5ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge5Price)) {
                    this.Surcharge5ComparingPrice = (surcharge5ComparingValue / this.ComparedEntity.Surcharge5Price) * 100;
                    this.Surcharge5ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge5ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge6Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge6ComparingPrice = null;
            this.Surcharge6ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge6Price != null) {
                var surcharge6ComparingValue = this.Surcharge6Price - this.ComparedEntity.Surcharge6Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge6ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge6Price)) {
                    this.Surcharge6ComparingPrice = (surcharge6ComparingValue / this.ComparedEntity.Surcharge6Price) * 100;
                    this.Surcharge6ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge6ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge7Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge7ComparingPrice = null;
            this.Surcharge7ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge7Price != null) {
                var surcharge7ComparingValue = this.Surcharge7Price - this.ComparedEntity.Surcharge7Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge7ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge7Price)) {
                    this.Surcharge7ComparingPrice = (surcharge7ComparingValue / this.ComparedEntity.Surcharge7Price) * 100;
                    this.Surcharge7ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge7ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge8Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge8ComparingPrice = null;
            this.Surcharge8ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge8Price != null) {
                var surcharge8ComparingValue = this.Surcharge8Price - this.ComparedEntity.Surcharge8Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge8ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge8Price)) {
                    this.Surcharge8ComparingPrice = (surcharge8ComparingValue / this.ComparedEntity.Surcharge8Price) * 100;
                    this.Surcharge8ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge8ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge9Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge9ComparingPrice = null;
            this.Surcharge9ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge9Price != null) {
                var surcharge9ComparingValue = this.Surcharge9Price - this.ComparedEntity.Surcharge9Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge9ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge9Price)) {
                    this.Surcharge9ComparingPrice = (surcharge9ComparingValue / this.ComparedEntity.Surcharge9Price) * 100;
                    this.Surcharge9ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge9ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.CompareSurcharge10Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge10ComparingPrice = null;
            this.Surcharge10ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge10Price != null) {
                var surcharge10ComparingValue = this.Surcharge10Price - this.ComparedEntity.Surcharge10Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge10ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge10Price)) {
                    this.Surcharge10ComparingPrice = (surcharge10ComparingValue / this.ComparedEntity.Surcharge10Price) * 100;
                    this.Surcharge10ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge10ComparingPrice);
                }
            }
        }
    };
    TariffLineData.prototype.ComputeWarningPercentageColor = function (price) {
        var color = "blue";
        if (this.FatherComponent.WarningPercentage == null) {
            color = "blue";
        }
        else {
            var price_abs = Math.abs(price);
            if (price_abs > this.FatherComponent.WarningPercentage) {
                color = "red";
            }
        }
        return color;
    };
    Object.defineProperty(TariffLineData.prototype, "HasErrors", {
        get: function () {
            return this.EntityPM.HasErrors;
        },
        set: function (value) {
            if (this.EntityPM.HasErrors != value) {
                this.EntityPM.HasErrors = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "ErrorText", {
        get: function () {
            return this.EntityPM.ErrorText;
        },
        set: function (value) {
            if (this.EntityPM.ErrorText != value) {
                this.EntityPM.ErrorText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "OriginPortId", {
        // Origin Port
        get: function () {
            return this.EntityPM.OriginPortId;
        },
        set: function (value) {
            if (this.EntityPM.OriginPortId != value) {
                this.EntityPM.OriginPortId = value;
                this.SetUIProperties();
                this.CheckIfLineHasError();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "OriginPortCode", {
        get: function () {
            return this.EntityPM.OriginPortCode;
        },
        set: function (value) {
            if (this.EntityPM.OriginPortCode != value) {
                this.EntityPM.OriginPortCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "OriginPort", {
        get: function () { return this.originPort; },
        set: function (value) {
            if (this.originPort != value) {
                this.originPort = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.OriginPortCode = value.Code;
            }
            else {
                this.OriginPortCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "OriginPortValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCode)) {
                return this.EntityPM.OriginPortCode;
            }
            else {
                return this.EntityPM.OriginPortText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "OriginPortColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "DestinationPortId", {
        // Destination Port
        get: function () {
            return this.EntityPM.DestinationPortId;
        },
        set: function (value) {
            if (this.EntityPM.DestinationPortId != value) {
                this.EntityPM.DestinationPortId = value;
                this.SetUIProperties();
                this.CheckIfLineHasError();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "DestinationPortCode", {
        get: function () {
            return this.EntityPM.DestinationPortCode;
        },
        set: function (value) {
            if (this.EntityPM.DestinationPortCode != value) {
                this.EntityPM.DestinationPortCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "DestinationPort", {
        get: function () { return this.destinationPort; },
        set: function (value) {
            if (this.destinationPort != value) {
                this.destinationPort = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.DestinationPortCode = value.Code;
            }
            else {
                this.DestinationPortCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "DestinationPortValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCode)) {
                return this.EntityPM.DestinationPortCode;
            }
            else {
                return this.EntityPM.DestinationPortText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "DestinationPortColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "StartDate", {
        get: function () { return this.EntityPM.StartDate; },
        set: function (value) {
            if (this.EntityPM.StartDate != value) {
                this.EntityPM.StartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "ExpirationDate", {
        get: function () { return this.EntityPM.ExpirationDate; },
        set: function (value) {
            if (this.EntityPM.ExpirationDate != value) {
                this.EntityPM.ExpirationDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Notes", {
        get: function () {
            return this.EntityPM.Notes;
        },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge1Price", {
        // Surcharge 1
        get: function () {
            return this.EntityPM.Surcharge1Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge1Price != value) {
                this.EntityPM.Surcharge1Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge1Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge1PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge1Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge1PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge1PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge2Price", {
        // Surcharge 2
        get: function () {
            return this.EntityPM.Surcharge2Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge2Price != value) {
                this.EntityPM.Surcharge2Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge2Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge2PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge2Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge2PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge2PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge3Price", {
        // Surcharge 3
        get: function () {
            return this.EntityPM.Surcharge3Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge3Price != value) {
                this.EntityPM.Surcharge3Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge3Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge3PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge3Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge3PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge3PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge4Price", {
        // Surcharge 4
        get: function () {
            return this.EntityPM.Surcharge4Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge4Price != value) {
                this.EntityPM.Surcharge4Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge4Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge4PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge4Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge4PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge4PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge5Price", {
        // Surcharge 5
        get: function () {
            return this.EntityPM.Surcharge5Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge5Price != value) {
                this.EntityPM.Surcharge5Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge5Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge5PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge5Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge5PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge5PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge6Price", {
        // Surcharge 6
        get: function () {
            return this.EntityPM.Surcharge6Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge6Price != value) {
                this.EntityPM.Surcharge6Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge6Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge6PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge6Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge6PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge6PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge7Price", {
        // Surcharge 7
        get: function () {
            return this.EntityPM.Surcharge7Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge7Price != value) {
                this.EntityPM.Surcharge7Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge7Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge7PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge7Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge7PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge7PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge8Price", {
        // Surcharge 8
        get: function () {
            return this.EntityPM.Surcharge8Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge8Price != value) {
                this.EntityPM.Surcharge8Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge8Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge8PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge8Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge8PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge8PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge9Price", {
        // Surcharge 9
        get: function () {
            return this.EntityPM.Surcharge9Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge9Price != value) {
                this.EntityPM.Surcharge9Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge9Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge9PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge9Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge9PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge9PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge10Price", {
        // Surcharge 10
        get: function () {
            return this.EntityPM.Surcharge10Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge10Price != value) {
                this.EntityPM.Surcharge10Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge10Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge10PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge10Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge10PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Surcharge10PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    return TariffLineData;
}(BaseComponent_1.BaseComponent));
exports.TariffLineData = TariffLineData;
var VersionClass = /** @class */ (function () {
    function VersionClass() {
    }
    return VersionClass;
}());
exports.VersionClass = VersionClass;
//# sourceMappingURL=SurchargeVersionTabComponent.js.map