"use strict";
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
var common_1 = require("@angular/common");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var TariffVersionPM_1 = require("../../../EntityPMs/TariffVersionPM");
var TariffLinePM_1 = require("../../../EntityPMs/TariffLinePM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var TariffDomainService_1 = require("../../../Services/TariffDomainService");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var ChargesTypeListService_1 = require("../../../../Common/Services/StandardLists/ChargesTypeListService");
var TariffVersionExtendedPMService_1 = require("../../../Services/ExtendedPMs/TariffVersionExtendedPMService");
var VersionHistoryTabComponent = /** @class */ (function () {
    function VersionHistoryTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsActionsEnabled = false;
        this.SaveCompletedEvent = null;
        this.versions = [];
        this.isCopyButtonClicked = false;
        this.EntityPM = entityArgs.EntityPM;
        this.VersionLinesSource = new ObservableCollection_1.ObservableCollection([]);
        this.TariffDomainService = new TariffDomainService_1.TariffDomainService();
        if (this.EntityPM.TypeCode == "AFC") {
            this.SetStepsLabelsAndVisibility();
        }
        else if (this.EntityPM.TypeCode == "ASC") {
            this.GetAllChargesTypes();
        }
        this.LoadVersions();
        this.Listen();
    }
    VersionHistoryTabComponent.prototype.GetAllChargesTypes = function () {
        var _this = this;
        var chargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        chargesTypeListService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllChargesTypes = myResponse.Result;
                if (_this.AllChargesTypes != null) {
                    _this.AllChargesTypes = _this.AllChargesTypes.filter(function (d) { return d.InActive == false; });
                    _this.SetSurchargesLabelsAndVisibility();
                }
            }
        });
    };
    VersionHistoryTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    if (_this.isCopyButtonClicked) {
                        _this.isCopyButtonClicked = false;
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
            });
        }
    };
    VersionHistoryTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
    };
    VersionHistoryTabComponent.prototype.SetStepsLabelsAndVisibility = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PriceSteps)) {
            if (this.EntityPM.PriceSteps.indexOf(',') > -1) {
                var steps = this.EntityPM.PriceSteps.split(",");
                var count = steps.length;
                if (count == 1) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step1PriceVisibility = true;
                }
                else if (count == 2) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                }
                else if (count == 3) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                }
                else if (count == 4) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                }
                else if (count == 5) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                }
                else if (count == 6) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                }
                else if (count == 7) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step7PriceLabel = steps[6] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                    this.Step7PriceVisibility = true;
                }
                else if (count == 8) {
                    this.Step1PriceLabel = steps[0] + " KG";
                    this.Step2PriceLabel = steps[1] + " KG";
                    this.Step3PriceLabel = steps[2] + " KG";
                    this.Step4PriceLabel = steps[3] + " KG";
                    this.Step5PriceLabel = steps[4] + " KG";
                    this.Step6PriceLabel = steps[5] + " KG";
                    this.Step7PriceLabel = steps[6] + " KG";
                    this.Step8PriceLabel = steps[7] + " KG";
                    this.Step1PriceVisibility = true;
                    this.Step2PriceVisibility = true;
                    this.Step3PriceVisibility = true;
                    this.Step4PriceVisibility = true;
                    this.Step5PriceVisibility = true;
                    this.Step6PriceVisibility = true;
                    this.Step7PriceVisibility = true;
                    this.Step8PriceVisibility = true;
                }
            }
            else {
                this.Step1PriceLabel = this.EntityPM.PriceSteps;
                this.Step1PriceVisibility = true;
            }
        }
    };
    VersionHistoryTabComponent.prototype.SetSurchargesLabelsAndVisibility = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge1Id; })[0];
            this.Surcharge1PriceLabel = chargeType.Code;
            this.Surcharge1PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge2Id; })[0];
            this.Surcharge2PriceLabel = chargeType.Code;
            this.Surcharge2PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge3Id; })[0];
            this.Surcharge3PriceLabel = chargeType.Code;
            this.Surcharge3PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge4Id; })[0];
            this.Surcharge4PriceLabel = chargeType.Code;
            this.Surcharge4PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge5Id; })[0];
            this.Surcharge5PriceLabel = chargeType.Code;
            this.Surcharge5PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge6Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge6Id; })[0];
            this.Surcharge6PriceLabel = chargeType.Code;
            this.Surcharge6PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge7Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge7Id; })[0];
            this.Surcharge7PriceLabel = chargeType.Code;
            this.Surcharge7PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge8Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge8Id; })[0];
            this.Surcharge8PriceLabel = chargeType.Code;
            this.Surcharge8PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge9Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge9Id; })[0];
            this.Surcharge9PriceLabel = chargeType.Code;
            this.Surcharge9PriceVisibility = true;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge10Id)) {
            var chargeType = this.AllChargesTypes.filter(function (a) { return a.Id == _this.EntityPM.Surcharge10Id; })[0];
            this.Surcharge10PriceLabel = chargeType.Code;
            this.Surcharge10PriceVisibility = true;
        }
    };
    VersionHistoryTabComponent.prototype.LoadVersions = function () {
        var _this = this;
        var service = new TariffVersionExtendedPMService_1.TariffVersionExtendedPMService();
        service.GetAllTariffVersionsForTariff(this.EntityPM.Id).subscribe(function (response) {
            if (!response.HasError) {
                _this.versions = response.Result;
                _this.BuildVersionsList();
            }
        });
    };
    VersionHistoryTabComponent.prototype.BuildVersionsList = function () {
        var _this = this;
        this.VersionsList = [];
        var datePipe = new common_1.DatePipe("en-US");
        this.versions.filter(function (d) { return !d.IsDraft; }).forEach(function (item) {
            var newVersion = new CodeNameClass_1.CodeNameClass();
            newVersion.Code_Int = item.Version;
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
        if (this.VersionsList.length > 0) {
            this.SelectedVersion = this.VersionsList[0];
            this.IsActionsEnabled = true;
        }
    };
    Object.defineProperty(VersionHistoryTabComponent.prototype, "SelectedVersion", {
        get: function () { return this.selectedVersion; },
        set: function (value) {
            if (this.selectedVersion != value) {
                this.selectedVersion = value;
                this.VersionPM = this.versions.filter(function (d) { return d.Version == value.Code_Int; })[0];
                this.LoadTariffLines();
            }
        },
        enumerable: true,
        configurable: true
    });
    VersionHistoryTabComponent.prototype.LoadTariffLines = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.TariffDomainService.GetTariffVersionLines(this.EntityPM.Id, this.VersionPM.Version).subscribe(function (response) {
            if (!response.HasError) {
                _this.tariffLines = response.Result;
                _this.FillLines();
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    VersionHistoryTabComponent.prototype.FillLines = function () {
        this.VersionLinesSource.Clear();
        var itemsCollection = [];
        this.tariffLines.forEach(function (item) {
            itemsCollection.push(item);
        });
        this.VersionLinesSource.InsertCollection(itemsCollection);
    };
    // Download Excel 
    VersionHistoryTabComponent.prototype.DownloadExcelClicked = function (type) {
        this.TariffDomainService.DownloadTariff(this.EntityPM.Id, this.VersionPM.Version, type).subscribe(function (myResponse) {
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
    VersionHistoryTabComponent.prototype.CopyVersionClicked = function () {
        if (this.EntityPM.TariffVersions.filter(function (d) { return d.IsDraft; })[0]) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("You can't copy this version since you already have draft one");
        }
        else {
            this.DoCopy();
        }
    };
    VersionHistoryTabComponent.prototype.DoCopy = function () {
        var _this = this;
        this.isCopyButtonClicked = true;
        this.EntityPM.LastVersion = this.EntityPM.LastVersion + 1;
        this.EntityPM.LastStartDate = this.VersionPM.StartDate;
        this.EntityPM.LastExpirationDate = this.VersionPM.ExpirationDate;
        var copiedVersion = new TariffVersionPM_1.TariffVersionPM(this.EntityPM);
        copiedVersion.TariffId = this.VersionPM.TariffId;
        copiedVersion.Version = this.EntityPM.LastVersion;
        copiedVersion.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        copiedVersion.CreatedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
        copiedVersion.ExpirationDate = this.VersionPM.ExpirationDate;
        copiedVersion.IsDraft = true;
        copiedVersion.StartDate = this.VersionPM.StartDate;
        copiedVersion.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        copiedVersion.ParentVersionNumber = this.VersionPM.Version;
        this.EntityPM.AddTariffVersion(copiedVersion);
        this.tariffLines.forEach(function (item) {
            var tariffLine = new TariffLinePM_1.TariffLinePM(copiedVersion);
            tariffLine.StartDate = _this.VersionPM.StartDate;
            tariffLine.ExpirationDate = _this.VersionPM.ExpirationDate;
            tariffLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
            tariffLine.Version = copiedVersion.Version;
            tariffLine.OriginPortId = item.OriginPortId;
            tariffLine.OriginPortCode = item.OriginPortCode;
            tariffLine.OriginPortName = item.OriginPortName;
            tariffLine.DestinationPortId = item.DestinationPortId;
            tariffLine.DestinationPortCode = item.DestinationPortCode;
            tariffLine.DestinationPortName = item.DestinationPortName;
            if (_this.EntityPM.TypeCode == "AFC") {
                tariffLine.MinPrice = item.MinPrice;
                tariffLine.Step1Price = item.Step1Price;
                tariffLine.Step2Price = item.Step2Price;
                tariffLine.Step3Price = item.Step3Price;
                tariffLine.Step4Price = item.Step4Price;
                tariffLine.Step5Price = item.Step5Price;
                tariffLine.Step6Price = item.Step6Price;
                tariffLine.Step7Price = item.Step7Price;
                tariffLine.Step8Price = item.Step8Price;
            }
            else if (_this.EntityPM.TypeCode == "ASC") {
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
            }
            copiedVersion.AddTariffLine(tariffLine);
        });
        this.CurrentSession.CurrentEditComponent.SaveChanges("Creating...");
    };
    VersionHistoryTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VersionHistoryTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], VersionHistoryTabComponent);
    return VersionHistoryTabComponent;
}());
exports.VersionHistoryTabComponent = VersionHistoryTabComponent;
//# sourceMappingURL=VersionHistoryTabComponent.js.map