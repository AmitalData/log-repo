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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DocumentTypePMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SharedDocumentsPermissionsComponent = /** @class */ (function () {
    function SharedDocumentsPermissionsComponent(_documentTypePMExtendedService) {
        this._documentTypePMExtendedService = _documentTypePMExtendedService;
        this.FullComponentsVisibility = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.CurrentSession.StartBusyIndicatorLoading();
    }
    SharedDocumentsPermissionsComponent.prototype.ngOnInit = function () {
        this.Run();
    };
    SharedDocumentsPermissionsComponent.prototype.Run = function () {
        this.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0].Id;
        this.LoadData();
    };
    SharedDocumentsPermissionsComponent.prototype.LoadData = function () {
        var _this = this;
        this.DocumentPermissiosLists = [];
        this.AllDocumentPermissiosLists = [];
        this._documentTypePMExtendedService.GetDocumentTypesPMByObjectTableIdForDocumentPremissions(this.ObjectTableId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                var myList = pmResponse.Result;
                myList.forEach(function (item) {
                    _this.DocumentPermissiosLists.push(new SharedDocumentsPermissionsViewModel(item));
                    _this.AllDocumentPermissiosLists.push(new SharedDocumentsPermissionsViewModel(item));
                });
            }
            _this.SortItemSource();
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    SharedDocumentsPermissionsComponent.prototype.BuildData = function () {
        var _this = this;
        this.DocumentPermissiosLists = [];
        if (!this.mySearchText) {
            this.DocumentPermissiosLists = this.AllDocumentPermissiosLists;
        }
        else {
            this.DocumentPermissiosLists = this.AllDocumentPermissiosLists.filter(function (d) { return (d.EntityPM.Code && d.EntityPM.Code.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1) || (d.EntityPM.Name && d.EntityPM.Name.toLowerCase().indexOf(_this.mySearchText.toLowerCase()) > -1); });
        }
        this.SortItemSource();
    };
    SharedDocumentsPermissionsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedDocumentsPermissionsComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        var documentTypePMList = [];
        this.CurrentSession.StartBusyIndicatorSaving();
        this.DocumentPermissiosLists.forEach(function (item) {
            if (item.EntityPM.IsDirty) {
                documentTypePMList.push(item.EntityPM);
            }
        });
        if (documentTypePMList.length > 0) {
            this._documentTypePMExtendedService.update(documentTypePMList).subscribe(function (res) {
                _this.CloseButtonClicked();
            });
        }
        else {
            this.CloseButtonClicked();
        }
    };
    SharedDocumentsPermissionsComponent.prototype.onSearchTextChangeEvent = function (searchText) {
        if (!searchText)
            searchText = "";
        this.mySearchText = searchText;
        this.BuildData();
    };
    SharedDocumentsPermissionsComponent.prototype.SortItemSource = function () {
        this.DocumentPermissiosLists = this.DocumentPermissiosLists.sort(function (a, b) {
            if (a.DocumentTypeName.toLowerCase() < b.DocumentTypeName.toLowerCase()) {
                return -1;
            }
            else if (a.DocumentTypeName.toLowerCase() > b.DocumentTypeName.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });
    };
    SharedDocumentsPermissionsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedDocumentsPermissionsComponent',
            templateUrl: './SharedDocumentsPermissionsComponent.html',
            providers: [DocumentTypePMExtendedService_1.DocumentTypePMExtendedService],
        }),
        __metadata("design:paramtypes", [DocumentTypePMExtendedService_1.DocumentTypePMExtendedService])
    ], SharedDocumentsPermissionsComponent);
    return SharedDocumentsPermissionsComponent;
}());
exports.SharedDocumentsPermissionsComponent = SharedDocumentsPermissionsComponent;
var SharedDocumentsPermissionsViewModel = /** @class */ (function () {
    function SharedDocumentsPermissionsViewModel(entityPM) {
        var _this = this;
        this.DocumentTypeCopyLists = [];
        this.EntityPM = entityPM;
        this.DocumentTypeName = entityPM.Name;
        this.IsAgentSharedInHouseEnable = entityPM.IsHouse;
        this.IsAgentSharedInMasterEnable = entityPM.IsMaster;
        this.IsAgentSharedInDirectEnable = entityPM.IsDirect;
        if (entityPM.DocumentTypeCopies && entityPM.DocumentTypeCopies.length > 1) {
            this.DocumentTypeCopyLists = entityPM.DocumentTypeCopies;
            if (this.EntityPM.SharedDocumentTypeCopyId) {
                this.SelectedDocumentTypeCopy = this.DocumentTypeCopyLists.filter(function (d) { return d.Id == _this.EntityPM.SharedDocumentTypeCopyId; })[0];
            }
            if (!this.SelectedDocumentTypeCopy)
                this.SelectedDocumentTypeCopy = this.DocumentTypeCopyLists[0];
        }
    }
    Object.defineProperty(SharedDocumentsPermissionsViewModel.prototype, "IsAgentSharedInHouse", {
        get: function () {
            if (this.EntityPM) {
                return this.EntityPM.IsAgentSharedInHouse;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                this.EntityPM.IsAgentSharedInHouse = value;
                this.SetSharedDocumentTypeCopyId();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedDocumentsPermissionsViewModel.prototype, "IsAgentSharedInDirect", {
        get: function () {
            if (this.EntityPM) {
                return this.EntityPM.IsAgentSharedInDirect;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                this.EntityPM.IsAgentSharedInDirect = value;
                this.SetSharedDocumentTypeCopyId();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedDocumentsPermissionsViewModel.prototype, "IsAgentSharedInMaster", {
        get: function () {
            if (this.EntityPM) {
                return this.EntityPM.IsAgentSharedInMaster;
            }
            else
                return false;
        },
        set: function (value) {
            if (this.EntityPM != null) {
                this.EntityPM.IsAgentSharedInMaster = value;
                this.SetSharedDocumentTypeCopyId();
            }
        },
        enumerable: true,
        configurable: true
    });
    SharedDocumentsPermissionsViewModel.prototype.DocumentTypeCopyListValueChanged = function (copy) {
        if (copy) {
            this.EntityPM.SharedDocumentTypeCopyId = copy.Id;
            this.SelectedDocumentTypeCopy = copy;
        }
    };
    SharedDocumentsPermissionsViewModel.prototype.SetSharedDocumentTypeCopyId = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.SharedDocumentTypeCopyId)) {
            if (this.SelectedDocumentTypeCopy) {
                this.EntityPM.SharedDocumentTypeCopyId = this.SelectedDocumentTypeCopy.Id;
            }
            else if (this.EntityPM.DocumentTypeCopies && this.EntityPM.DocumentTypeCopies.length > 0) {
                this.EntityPM.SharedDocumentTypeCopyId = this.EntityPM.DocumentTypeCopies[0].Id;
            }
        }
    };
    return SharedDocumentsPermissionsViewModel;
}());
exports.SharedDocumentsPermissionsViewModel = SharedDocumentsPermissionsViewModel;
//# sourceMappingURL=SharedDocumentsPermissionsComponent.js.map