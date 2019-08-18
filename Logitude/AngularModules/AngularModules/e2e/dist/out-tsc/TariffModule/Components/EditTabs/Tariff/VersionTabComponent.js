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
var TariffVersionExtendedPMService_1 = require("../../../Services/ExtendedPMs/TariffVersionExtendedPMService");
var common_1 = require("@angular/common");
var VersionTabComponent = /** @class */ (function (_super) {
    __extends(VersionTabComponent, _super);
    function VersionTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Tariff";
        _this.DeletedTariffsLines = [];
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.IsApproveVersionButtonVisible = false;
        _this.IsDraftVersion = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsFirstDraft = false;
        _this.SaveCompletedEvent = null;
        _this.ItemsCollection = [];
        _this.isComparToChecked = false;
        _this.isUploadExcelFinished = false;
        _this.isApproveButtonClicked = false;
        _this.isCopyButtonClicked = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.Listen();
        return _this;
    }
    VersionTabComponent.prototype.Intialize = function (args) {
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
        this.LoadCompareToVersions();
        this.SetUIProperties();
        this.SetStepsLabelsAndVisibility();
        if (this.CurrentVersion.IsDraft) {
            this.FillTariffLines(this.CurrentVersion.TariffLines);
        }
        else {
            this.LoadTariffLines("currentVersion");
        }
        this.GetTariffSettings();
    };
    VersionTabComponent.prototype.GetTariffSettings = function () {
        var _this = this;
        this.TariffDomainService.GetTenantTariffSetting().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.warningPercentage = myResponse.Result.DefaultWarningPercentage;
            }
        });
    };
    VersionTabComponent.prototype.Listen = function () {
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
                }
            });
        }
    };
    VersionTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
    };
    VersionTabComponent.prototype.LoadTariffLines = function (type) {
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
    VersionTabComponent.prototype.SetUIProperties = function () {
        var isApproveVersionButtonVisible = false;
        if (this.IsDraftVersion && FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "TARRIFAPPROVEVERSION")) {
            isApproveVersionButtonVisible = true;
        }
        this.IsApproveVersionButtonVisible = isApproveVersionButtonVisible;
    };
    Object.defineProperty(VersionTabComponent.prototype, "VersionNumber", {
        get: function () {
            return (this.CurrentVersion == null ? null : this.CurrentVersion.Version);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VersionTabComponent.prototype, "StartDate", {
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
    Object.defineProperty(VersionTabComponent.prototype, "ExpirationDate", {
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
    VersionTabComponent.prototype.UpdateDates = function (dateType, date) {
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
    Object.defineProperty(VersionTabComponent.prototype, "PriceSteps", {
        get: function () {
            return this.EntityPM.PriceSteps;
        },
        set: function (value) {
            if (this.EntityPM.PriceSteps != value) {
                this.EntityPM.PriceSteps = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    VersionTabComponent.prototype.FillTariffLines = function (tariffLines) {
        var _this = this;
        if (this.TariffsLinesSource != null) {
            this.TariffsLinesSource.Clear();
        }
        this.ItemsCollection = [];
        tariffLines.sort(function (a, b) { return a.Index - b.Index; }).forEach(function (item) {
            _this.ItemsCollection.push(new TariffLineData(item, _this));
        });
        this.TariffsLinesSource.InsertCollection(this.ItemsCollection);
        this.DoCompare();
    };
    VersionTabComponent.prototype.DoCompare = function () {
        this.DeletedTariffsLines = [];
        if (this.IsComparToChecked && this.ComparedToVersionPM != null) {
            this.ComaredLines();
            this.BuildDeletedLines();
        }
    };
    VersionTabComponent.prototype.ComaredLines = function () {
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
    VersionTabComponent.prototype.BuildDeletedLines = function () {
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
    Object.defineProperty(VersionTabComponent.prototype, "IsComparToChecked", {
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
    Object.defineProperty(VersionTabComponent.prototype, "WarningPercentage", {
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
    VersionTabComponent.prototype.SetStepsLabelsAndVisibility = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PriceSteps)) {
            if (this.PriceSteps.indexOf(',') > -1) {
                var steps = this.PriceSteps.split(",");
                var count = steps.length;
                if (count == 0) {
                    this.Step1PriceLabel = this.PriceSteps;
                    this.Step1PriceVisibility = true;
                }
                for (var i = 1; i <= count; i++) {
                    this["Step" + i + "PriceLabel"] = steps[i - 1] + " KG";
                    this["Step" + i + "PriceVisibility"] = true;
                }
            }
        }
    };
    VersionTabComponent.prototype.AddTariffLine = function () {
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
    VersionTabComponent.prototype.EditTariffButtonClicked = function (item) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.DataContext = item;
        logWindow.Title = "Edit Tariff Line";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditTariffLineComponent");
    };
    VersionTabComponent.prototype.DeleteTariffButtonClicked = function (item) {
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
    VersionTabComponent.prototype.OnFileChanged = function (fileEvent) {
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
    VersionTabComponent.prototype.UploadExcel = function (file) {
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
    VersionTabComponent.prototype.StartUploadingExcelFile = function (file) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    };
    VersionTabComponent.prototype.ConvertArrayBufferToBase64 = function (file, context) {
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
    VersionTabComponent.prototype.SendExcelToServer = function (filter) {
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
    VersionTabComponent.prototype.InsertNewRowsFromExcel = function (tariffLines) {
        var _this = this;
        tariffLines.sort(function (a, b) { return a.Index - b.Index; }).forEach(function (item) {
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
            if (_this.PriceSteps.indexOf(',') > -1) {
                var steps = _this.PriceSteps.split(",");
                var count = steps.length;
                tariffLine.MinPrice = item.MinPrice;
                tariffLine.MinPriceText = item.MinPriceText;
                for (var i = 1; i <= count; i++) {
                    tariffLine["Step" + i + "Price"] = item["Step" + i + "Price"];
                    tariffLine["Step" + i + "PriceText"] = item["Step" + i + "PriceText"];
                }
            }
            _this.CurrentVersion.AddTariffLine(tariffLine);
        });
        this.EntityPM.TariffLinesAddedFromExcel = true;
        this.isUploadExcelFinished = true;
        this.CurrentSession.CurrentEditComponent.SaveChanges("Saving...");
    };
    // Download Excel 
    VersionTabComponent.prototype.DownloadExcelClicked = function (type) {
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
    VersionTabComponent.prototype.ApproveVersionClicked = function () {
        if (!this.isApproveButtonClicked) {
            this.isApproveButtonClicked = true;
            this.EntityPM.IsApprovingDraftVersion = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
        //var errors: string[] = [];
        //if (this.CurrentVersion.TariffLines.filter(d => d.HasErrors).length > 0) {
        //    errors.push("Invalid Tariff Lines");
        //}
        //if (DateTool.GetDateParts(this.CurrentVersion.ExpirationDate).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
        //    errors.push("Approving past version is not allowed, please update the dates");
        //}
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
    VersionTabComponent.prototype.CopyVersionClicked = function () {
        if (this.EntityPM.TariffVersions.filter(function (d) { return d.IsDraft; })[0]) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("You can't copy this version since you already have draft one");
        }
        else {
            this.DoCopy();
        }
    };
    VersionTabComponent.prototype.DoCopy = function () {
        var _this = this;
        var windowTitle = "New Copy Version";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 200;
        logWindow.WindowArgs = this.CurrentVersion;
        logWindow.Title = windowTitle;
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if (s && d == "ok") {
                    _this.isCopyButtonClicked = true;
                    _this.EntityPM.LastVersion = _this.EntityPM.LastVersion + 1;
                    _this.EntityPM.LastStartDate = _this.StartDate;
                    _this.EntityPM.LastExpirationDate = _this.ExpirationDate;
                    var copiedVersion = new TariffVersionPM_1.TariffVersionPM(_this.EntityPM);
                    copiedVersion.TariffId = _this.CurrentVersion.TariffId;
                    copiedVersion.Version = _this.EntityPM.LastVersion;
                    copiedVersion.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    copiedVersion.CreatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                    copiedVersion.StartDate = s.StartDate;
                    copiedVersion.ExpirationDate = s.ExpirationDate;
                    copiedVersion.IsDraft = true;
                    copiedVersion.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    copiedVersion.ParentVersionNumber = _this.CurrentVersion.Version;
                    _this.EntityPM.AddTariffVersion(copiedVersion);
                    _this.loadedTariffLines.forEach(function (item) {
                        var tariffLine = new TariffLinePM_1.TariffLinePM(copiedVersion);
                        tariffLine.StartDate = _this.StartDate;
                        tariffLine.ExpirationDate = _this.ExpirationDate;
                        tariffLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        tariffLine.Version = copiedVersion.Version;
                        tariffLine.OriginPortId = item.OriginPortId;
                        tariffLine.OriginPortCode = item.OriginPortCode;
                        tariffLine.OriginPortName = item.OriginPortName;
                        tariffLine.DestinationPortId = item.DestinationPortId;
                        tariffLine.DestinationPortCode = item.DestinationPortCode;
                        tariffLine.DestinationPortName = item.DestinationPortName;
                        tariffLine.MinPrice = item.MinPrice;
                        tariffLine.Step1Price = item.Step1Price;
                        tariffLine.Step2Price = item.Step2Price;
                        tariffLine.Step3Price = item.Step3Price;
                        tariffLine.Step4Price = item.Step4Price;
                        tariffLine.Step5Price = item.Step5Price;
                        tariffLine.Step6Price = item.Step6Price;
                        tariffLine.Step7Price = item.Step7Price;
                        tariffLine.Step8Price = item.Step8Price;
                        tariffLine.Index = item.Index;
                        tariffLine.Notes = item.Notes;
                        copiedVersion.AddTariffLine(tariffLine);
                    });
                    _this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
                }
            });
        });
        logWindow.Show('./TariffModule/Components/EditTabs/Tariff/TariffDatesValidationComponent');
    };
    VersionTabComponent.prototype.LoadCompareToVersions = function () {
        var _this = this;
        var service = new TariffVersionExtendedPMService_1.TariffVersionExtendedPMService();
        service.GetAllTariffVersionsForTariff(this.EntityPM.Id).subscribe(function (response) {
            if (!response.HasError) {
                _this.compareToVersions = response.Result;
                _this.BuildVersionsList();
            }
        });
    };
    VersionTabComponent.prototype.BuildVersionsList = function () {
        var _this = this;
        this.VersionsList = [];
        var datePipe = new common_1.DatePipe("en-US");
        this.compareToVersions.filter(function (a) { return a.Version != _this.CurrentVersion.Version; }).forEach(function (item) {
            var from = datePipe.transform(item.StartDate, 'dd/MM/yyyy');
            var to = datePipe.transform(item.ExpirationDate, 'dd/MM/yyyy');
            var newVersion = new VersionClass();
            newVersion.Version = item.Version;
            newVersion.ParentVersionNumber = item.ParentVersionNumber;
            newVersion.Name = "Version " + item.Version + " (" + from + " - " + to + ")";
            newVersion.Id = item.TariffId;
            _this.VersionsList.push(newVersion);
        });
        this.SelectedVersion = this.VersionsList.filter(function (a) { return a.Version == _this.CurrentVersion.ParentVersionNumber; })[0];
        this.UIProperties.SetEnabled("WarningPercentage", null, this.IsComparToChecked);
        if (this.VersionsList == null || (this.VersionsList != null && this.VersionsList.length == 0)) {
            this.isComparToChecked = false;
            this.IsFirstDraft = true;
        }
    };
    Object.defineProperty(VersionTabComponent.prototype, "SelectedVersion", {
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
    VersionTabComponent.prototype.ComparingCalculations = function (load) {
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
    VersionTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VersionTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], VersionTabComponent);
    return VersionTabComponent;
}(BaseComponent_1.BaseComponent));
exports.VersionTabComponent = VersionTabComponent;
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
        _this.MinPriceComparingTextColor = null;
        _this.Step1ComparingTextColor = null;
        _this.Step2ComparingTextColor = null;
        _this.Step3ComparingTextColor = null;
        _this.Step4ComparingTextColor = null;
        _this.Step5ComparingTextColor = null;
        _this.Step6ComparingTextColor = null;
        _this.Step7ComparingTextColor = null;
        _this.Step8ComparingTextColor = null;
        _this.DefaultColor = "blue";
        _this.EntityPM = entity;
        _this.IsNewEntity = isNew;
        _this.IsEditEnabled = FatherComponent.IsDraftVersion;
        _this.SetUIProperties();
        return _this;
    }
    TariffLineData.prototype.SetCellsComparingText = function () {
        if (this.ComparedEntity != null) {
            var value1 = Tools_1.AppTool.IsNullOrZero(this.MinPrice) ? 0 : this.MinPrice;
            var value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.MinPrice) ? 0 : this.ComparedEntity.MinPrice;
            var minPriceComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(minPriceComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.MinPriceComparingPrice = Tools_1.AppTool.Round((minPriceComparingValue / value2) * 100, 2);
                this.MinPriceComparingTextColor = this.ComputeWarningPercentageColor(this.MinPriceComparingPrice);
            }
            else {
                this.MinPriceComparingPrice = null;
                this.MinPriceComparingTextColor = this.DefaultColor;
            }
            // step 1
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step1Price) ? 0 : this.Step1Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step1Price) ? 0 : this.ComparedEntity.Step1Price;
            var step1ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step1ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step1ComparingPrice = (step1ComparingValue / value2) * 100;
                this.Step1ComparingTextColor = this.ComputeWarningPercentageColor(this.Step1ComparingPrice);
            }
            else {
                this.Step1ComparingPrice = null;
                this.Step1ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step2Price) ? 0 : this.Step2Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step2Price) ? 0 : this.ComparedEntity.Step2Price;
            var step2ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step2ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step2ComparingPrice = (step2ComparingValue / value2) * 100;
                this.Step2ComparingTextColor = this.ComputeWarningPercentageColor(this.Step2ComparingPrice);
            }
            else {
                this.Step2ComparingPrice = null;
                this.Step2ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step3Price) ? 0 : this.Step3Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step3Price) ? 0 : this.ComparedEntity.Step3Price;
            var step3ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step3ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step3ComparingPrice = (step3ComparingValue / value2) * 100;
                this.Step3ComparingTextColor = this.ComputeWarningPercentageColor(this.Step3ComparingPrice);
            }
            else {
                this.Step3ComparingPrice = null;
                this.Step3ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step4Price) ? 0 : this.Step4Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step4Price) ? 0 : this.ComparedEntity.Step4Price;
            var step4ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step4ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step4ComparingPrice = (step4ComparingValue / value2) * 100;
                this.Step4ComparingTextColor = this.ComputeWarningPercentageColor(this.Step4ComparingPrice);
            }
            else {
                this.Step4ComparingPrice = null;
                this.Step4ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step5Price) ? 0 : this.Step5Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step5Price) ? 0 : this.ComparedEntity.Step5Price;
            var step5ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step5ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step5ComparingPrice = (step5ComparingValue / value2) * 100;
                this.Step5ComparingTextColor = this.ComputeWarningPercentageColor(this.Step5ComparingPrice);
            }
            else {
                this.Step5ComparingPrice = null;
                this.Step5ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step6Price) ? 0 : this.Step6Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step6Price) ? 0 : this.ComparedEntity.Step6Price;
            var step6ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step6ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step6ComparingPrice = (step6ComparingValue / value2) * 100;
                this.Step6ComparingTextColor = this.ComputeWarningPercentageColor(this.Step6ComparingPrice);
            }
            else {
                this.Step6ComparingPrice = null;
                this.Step6ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step7Price) ? 0 : this.Step7Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step7Price) ? 0 : this.ComparedEntity.Step7Price;
            var step7ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step7ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step7ComparingPrice = (step7ComparingValue / value2) * 100;
                this.Step7ComparingTextColor = this.ComputeWarningPercentageColor(this.Step7ComparingPrice);
            }
            else {
                this.Step7ComparingPrice = null;
                this.Step7ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step8Price) ? 0 : this.Step8Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step8Price) ? 0 : this.ComparedEntity.Step8Price;
            var step8ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step8ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step8ComparingPrice = (step8ComparingValue / value2) * 100;
                this.Step8ComparingTextColor = this.ComputeWarningPercentageColor(this.Step8ComparingPrice);
            }
            else {
                this.Step8ComparingPrice = null;
                this.Step8ComparingTextColor = this.DefaultColor;
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
    TariffLineData.prototype.CheckIfLineHasError = function () {
        if (this.ErrorText != 'Line is a duplicate') {
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
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MinPriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Min price format is invalid";
                }
                else {
                    errorText = errorText + ", Min price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step1PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 1 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 1 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step2PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 2 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 2 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step3PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 3 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 3 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step4PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 4 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 4 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step5PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 5 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 5 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step6PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 6 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 6 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step7PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 7 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 7 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step8PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 8 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 8 price format is invalid";
                }
            }
            this.HasErrors = error;
            this.ErrorText = errorText;
        }
    };
    TariffLineData.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DestinationPortId));
    };
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
    Object.defineProperty(TariffLineData.prototype, "MinPrice", {
        // Min Price
        get: function () {
            return this.EntityPM.MinPrice;
        },
        set: function (value) {
            if (this.EntityPM.MinPrice != value) {
                this.EntityPM.MinPrice = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "MinPriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.MinPrice, "N3");
            }
            else {
                return this.EntityPM.MinPriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "MinPriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step1Price", {
        // Step 1
        get: function () {
            return this.EntityPM.Step1Price;
        },
        set: function (value) {
            if (this.EntityPM.Step1Price != value) {
                this.EntityPM.Step1Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step1PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step1Price, "N3");
            }
            else {
                return this.EntityPM.Step1PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step1PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step2Price", {
        // Step 2
        get: function () {
            return this.EntityPM.Step2Price;
        },
        set: function (value) {
            if (this.EntityPM.Step2Price != value) {
                this.EntityPM.Step2Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step2PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step2Price, "N3");
            }
            else {
                return this.EntityPM.Step2PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step2PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step3Price", {
        // Step 3
        get: function () {
            return this.EntityPM.Step3Price;
        },
        set: function (value) {
            if (this.EntityPM.Step3Price != value) {
                this.EntityPM.Step3Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step3PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step3Price, "N3");
            }
            else {
                return this.EntityPM.Step3PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step3PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step4Price", {
        // Step 4
        get: function () {
            return this.EntityPM.Step4Price;
        },
        set: function (value) {
            if (this.EntityPM.Step4Price != value) {
                this.EntityPM.Step4Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step4PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step4Price, "N3");
            }
            else {
                return this.EntityPM.Step4PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step4PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step5Price", {
        // Step 5
        get: function () {
            return this.EntityPM.Step5Price;
        },
        set: function (value) {
            if (this.EntityPM.Step5Price != value) {
                this.EntityPM.Step5Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step5PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step5Price, "N3");
            }
            else {
                return this.EntityPM.Step5PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step5PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step6Price", {
        // Step 6
        get: function () {
            return this.EntityPM.Step6Price;
        },
        set: function (value) {
            if (this.EntityPM.Step6Price != value) {
                this.EntityPM.Step6Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step6PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step6Price, "N3");
            }
            else {
                return this.EntityPM.Step6PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step6PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step7Price", {
        // Step 7
        get: function () {
            return this.EntityPM.Step7Price;
        },
        set: function (value) {
            if (this.EntityPM.Step7Price != value) {
                this.EntityPM.Step7Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step7PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step7Price, "N3");
            }
            else {
                return this.EntityPM.Step7PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step7PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step8Price", {
        // Step 8
        get: function () {
            return this.EntityPM.Step8Price;
        },
        set: function (value) {
            if (this.EntityPM.Step8Price != value) {
                this.EntityPM.Step8Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step8PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step8Price, "N3");
            }
            else {
                return this.EntityPM.Step8PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffLineData.prototype, "Step8PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
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
//# sourceMappingURL=VersionTabComponent.js.map