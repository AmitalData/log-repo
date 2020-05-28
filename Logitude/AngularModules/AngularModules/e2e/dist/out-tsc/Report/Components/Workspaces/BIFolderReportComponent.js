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
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var BIReportListService_1 = require("../../../Infrastructure/Services/StandardLists/BIReportListService");
var InfrastructureDomainService_1 = require("../../../Infrastructure/Services/InfrastructureDomainService");
var BIReportFolderListService_1 = require("../../../Infrastructure/Services/StandardLists/BIReportFolderListService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Args_1 = require("../../../Infrastructure/Args");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var BIFolderReportComponent = /** @class */ (function () {
    function BIFolderReportComponent() {
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.mySearchText = null;
        this.folderListService = new BIReportFolderListService_1.BIReportFolderListService();
        this.reportListService = new BIReportListService_1.BIReportListService();
        this._InfrastructureDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        this.LoadData();
        this.Listen();
    }
    BIFolderReportComponent.prototype.Listen = function () {
        var _this = this;
        this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "BIRefresh") {
                _this.LoadData();
            }
        });
    };
    BIFolderReportComponent.prototype.InitComponent = function () {
    };
    BIFolderReportComponent.prototype.LoadData = function () {
        var _this = this;
        this.folderListService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.folderList = myResponse.Result;
                _this.reportListService.getAll().subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.reportList = myResponse.Result;
                        _this.FillItemsSource();
                    }
                });
            }
        });
    };
    BIFolderReportComponent.prototype.FillItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        this.folderList.forEach(function (item) {
            var myReports = _this.reportList.filter(function (d) { return d.BIReportFolderId == item.Id; });
            if (Tools_1.AppTool.IsNullOrEmpty(_this.mySearchText)) {
                _this.ItemsSource.push(new BIFolderClass(item, myReports));
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(_this.mySearchText.toUpperCase()) > -1
                    ||
                        !Tools_1.AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(_this.mySearchText.toUpperCase()) > -1) {
                    _this.ItemsSource.push(new BIFolderClass(item, myReports));
                }
            }
        });
    };
    BIFolderReportComponent.prototype.NewFolderButtonClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Folder";
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReportFolderComponent');
        logWindow.WindowClosed.subscribe(function (d) {
            if (d) {
                _this.LoadData();
            }
        });
    };
    BIFolderReportComponent.prototype.NewReportButtonClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New BI Report";
        logWindow.Show('./InfrastructureModules/InfrastructureBIReport/Components/NewEntity/NewBIReport');
        logWindow.WindowClosed.subscribe(function (d) {
            if (d) {
                //this.LoadData();
            }
        });
    };
    BIFolderReportComponent.prototype.DeleteFolderClicked = function (item) {
        var _this = this;
        if (item != null) {
            if (item.reportsList != null && item.reportsList.length > 0) {
                var msg = new MessageWindow_1.MessageWindow();
                msg.Width = 450;
                msg.Show("Can't delete this folder since it contains reports, please delete/move them first");
            }
            else {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.NoButtonText = "No";
                confirmWindow.YesButtonText = "Yes";
                confirmWindow.Title = "Confirm Deletion";
                confirmWindow.Show("Are you sure you want to delete this folder?");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        // save
                        _this._InfrastructureDomainService.DeleteFolder(item.FolderId).subscribe(function (myResult) {
                            if (!myResult.HasError) {
                                _this.LoadData();
                            }
                        });
                    }
                    else if (confirmWindow.No) {
                        //nth
                    }
                });
            }
        }
    };
    BIFolderReportComponent.prototype.ViewFolderClicked = function (folder) {
        var _this = this;
        var objectTableName = "BIReport";
        var queryCode = "ALLBIREPORTS";
        var filterName = "BIReportFolderId";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        filterAgrs.addAdditionalFilter(filterName, folder.FolderId, null, null, "Equals", false, false, false, "String");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = folder.Name; //listArgs.QueryCode;
        listArgs.BackButtonTitle = "Back";
        listArgs.BIReportFolderId = folder.FolderId;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadData(); });
        });
    };
    BIFolderReportComponent.prototype.SearchTextChanged = function (text) {
        this.mySearchText = text;
        this.FillItemsSource();
    };
    BIFolderReportComponent = __decorate([
        core_1.Component({
            moduleId: './Report/Components/Workspaces/',
            templateUrl: 'BIFolderReportComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BIFolderReportComponent);
    return BIFolderReportComponent;
}());
exports.BIFolderReportComponent = BIFolderReportComponent;
var BIFolderClass = /** @class */ (function () {
    function BIFolderClass(myFolder, myReports) {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.FolderIcon = "folder_icon";
        this.LinkColor = "#282E30";
        this.folder = myFolder;
        this.reportsList = myReports;
        this.FolderId = myFolder.Id;
        this.Name = myFolder.Name;
        this.FolderIcon = this.FolderIcon + this.CurrentSession.GetNewId(this.FolderIcon);
        this.ComputeTitle();
    }
    BIFolderClass.prototype.ComputeTitle = function () {
        this.Title = this.folder.Name + " (" + this.reportsList.length + ")";
    };
    BIFolderClass.prototype.FolderIconMouseOver = function () {
        var img = document.getElementById(this.FolderIcon);
        img.setAttribute("src", "./Images/Icons/Folder_L.png");
        this.LinkColor = "#1B90CB";
    };
    BIFolderClass.prototype.FolderIconMouseLeave = function () {
        var img = document.getElementById(this.FolderIcon);
        img.setAttribute("src", "./Images/Icons/Folder_B.png");
        this.LinkColor = "#282E30";
    };
    return BIFolderClass;
}());
exports.BIFolderClass = BIFolderClass;
//# sourceMappingURL=BIFolderReportComponent.js.map