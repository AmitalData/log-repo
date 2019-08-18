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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var TariffDomainService_1 = require("../../Services/TariffDomainService");
var Args_1 = require("../../../Infrastructure/Args");
var BatchTaskExecutionListService_1 = require("../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService");
var Tools_1 = require("../../../Infrastructure/Tools");
var DocumentsFilingExtendedPMService_1 = require("../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService");
var TariffModuleWorkspaceComponent = /** @class */ (function () {
    function TariffModuleWorkspaceComponent(_entityResourceService, tariffDomainService) {
        this._entityResourceService = _entityResourceService;
        this.tariffDomainService = tariffDomainService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.timerInterval = 5000;
        this.IsLoading = false;
        this.RunComponent();
    }
    TariffModuleWorkspaceComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    TariffModuleWorkspaceComponent.prototype.ngOnInit = function () {
        if (this.CurrentSession == null)
            this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.InitComponent();
    };
    TariffModuleWorkspaceComponent.prototype.ngOnDestroy = function () {
        this.StopTimer();
    };
    TariffModuleWorkspaceComponent.prototype.LoadAllScreenData = function () {
        this.LoadQueriesCounts();
    };
    TariffModuleWorkspaceComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this.tariffDomainService.GetTariffsCounts().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;
                    if (myResult != null) {
                        _this.AirFreightCount = myResult.AirFreightCount > 1000 ? "1000+" : myResult.AirFreightCount.toString();
                        _this.AirSurchargeCount = myResult.AirSurchargeCount > 1000 ? "1000+" : myResult.AirSurchargeCount.toString();
                    }
                }
            }
        });
    };
    TariffModuleWorkspaceComponent.prototype.CheckAirfreightCost = function () {
        this._entityResourceService.getEntityResourceByTableName("TariffLine").subscribe(function (res1) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.IsFillScreenHeight = true;
            logWindow.Width = 900;
            logWindow.Title = "Price Check";
            logWindow.Show("./TariffModule/Components/Workspaces/TariffSearchAirFreightPricesComponent");
        });
    };
    TariffModuleWorkspaceComponent.prototype.NewTariff = function (code) {
        var _this = this;
        switch (code) {
            case "A": {
                this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe(function (response) {
                    var windowTitle = "New Air Freight Cost";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 850;
                    logWindow.Height = 500;
                    logWindow.Title = windowTitle;
                    logWindow.WindowClosed.subscribe(function ($event) {
                        _this.LoadQueriesCounts();
                    });
                    logWindow.ComponentLoaded.subscribe(function (comp) {
                        comp.SetWindowArgs({ TypeCode: "AFC" });
                    });
                    logWindow.Show('./TariffModule/Components/NewEntity/NewAirFreightCostComponent');
                });
                break;
            }
            case "AS": {
                this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe(function (response) {
                    var windowTitle = "New Air Surcharges Cost";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 850;
                    logWindow.Height = 500;
                    logWindow.Title = windowTitle;
                    logWindow.WindowClosed.subscribe(function ($event) {
                        _this.LoadQueriesCounts();
                    });
                    logWindow.ComponentLoaded.subscribe(function (comp) {
                        comp.SetWindowArgs({ TypeCode: "ASC" });
                    });
                    logWindow.Show('./TariffModule/Components/NewEntity/NewAirFreightCostComponent');
                });
                break;
            }
            default: {
                break;
            }
        }
    };
    TariffModuleWorkspaceComponent.prototype.ViewTariffs = function (code) {
        var _this = this;
        switch (code) {
            case "A": {
                var listArgs = new Args_1.ListComponentArgs();
                listArgs.QueryCode = "Air Freight Cost Tariffs";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Air Freight Cost Tariffs";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                        _this.CurrentSession.AddMenuReference(cmpRef);
                    });
                });
                break;
            }
            case "AS": {
                var listArgs = new Args_1.ListComponentArgs();
                listArgs.QueryCode = "Air Surcharges Cost Tariffs";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Air Surcharges Cost";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                        _this.CurrentSession.AddMenuReference(cmpRef);
                    });
                });
                break;
            }
            default: {
                break;
            }
        }
    };
    TariffModuleWorkspaceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    TariffModuleWorkspaceComponent.prototype.InitComponent = function () {
        this.DocumentExtendedService = new DocumentsFilingExtendedPMService_1.DocumentsFilingExtendedPMService();
        this.LoadAllScreenData();
    };
    Object.defineProperty(TariffModuleWorkspaceComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            if (this.selectedItem != newValue) {
                this.selectedItem = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffModuleWorkspaceComponent.prototype.TariffSettingsClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = "Tariff Settings";
        logWindow.Show('./TariffModule/Components/Workspaces/TariffSettingComponent');
    };
    TariffModuleWorkspaceComponent.prototype.GenerateTariffsClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Generating...");
        this.tariffDomainService.GenerateTariffs().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.batchEntity = myResponse.Result;
                if (_this.batchEntity != null) {
                    _this.timer = setInterval(function () { _this.GetBTE(); }, _this.timerInterval);
                }
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                var window = new MessageWindow_1.MessageWindow();
                window.Show(myResponse.ErrorsArray[0]);
            }
        });
    };
    TariffModuleWorkspaceComponent.prototype.GenerateExcelTariffsClicked = function (fileEvent) {
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
    TariffModuleWorkspaceComponent.prototype.UploadExcel = function (file) {
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
    TariffModuleWorkspaceComponent.prototype.StartUploadingExcelFile = function (file) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    };
    TariffModuleWorkspaceComponent.prototype.ConvertArrayBufferToBase64 = function (file, context) {
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
            filter.FileName = context.FileName;
            context.SendExcelToServer(filter);
        };
        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    };
    TariffModuleWorkspaceComponent.prototype.SendExcelToServer = function (filter) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Generating...");
        this.tariffDomainService.GenerateTariffsFromExcel(filter).subscribe(function (response) {
            if (!response.HasError) {
                //this.batchEntity = response.Result;
                //if (this.batchEntity != null) {
                //    this.timer = setInterval(() => { this.GetBTE(); }, this.timerInterval);
                //}
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
                var window = new MessageWindow_1.MessageWindow();
                window.Show(response.ErrorsArray[0]);
            }
        });
    };
    TariffModuleWorkspaceComponent.prototype.GetBTE = function () {
        var _this = this;
        if (!this.IsLoading) {
            this.IsLoading = true;
            var bteList;
            var myService = new BatchTaskExecutionListService_1.BatchTaskExecutionListService();
            myService.getSingle(this.batchEntity.Id).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    bteList = myResponse.Result;
                    if (bteList.StatusCode == "D") {
                        _this.LoadQueriesCounts();
                        _this.CurrentSession.StopBusyIndicator();
                        _this.StopTimer();
                    }
                    else if (bteList.StatusCode == "F") {
                        _this.CurrentSession.StopBusyIndicator();
                        _this.StopTimer();
                    }
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                    _this.StopTimer();
                    var window = new MessageWindow_1.MessageWindow();
                    window.Show(myResponse.ErrorsArray[0]);
                }
                _this.IsLoading = false;
            });
        }
    };
    TariffModuleWorkspaceComponent.prototype.StopTimer = function () {
        if (this.timer) {
            clearInterval(this.timer);
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], TariffModuleWorkspaceComponent.prototype, "AllLocations", void 0);
    TariffModuleWorkspaceComponent = __decorate([
        core_1.Component({
            selector: 'TariffModuleWorkspaceComponent',
            moduleId: module.id,
            templateUrl: './TariffModuleWorkspaceComponent.html',
            providers: [EntityResourceService_1.EntityResourceService, TariffDomainService_1.TariffDomainService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, TariffDomainService_1.TariffDomainService])
    ], TariffModuleWorkspaceComponent);
    return TariffModuleWorkspaceComponent;
}());
exports.TariffModuleWorkspaceComponent = TariffModuleWorkspaceComponent;
//# sourceMappingURL=TariffModuleWorkspaceComponent.js.map