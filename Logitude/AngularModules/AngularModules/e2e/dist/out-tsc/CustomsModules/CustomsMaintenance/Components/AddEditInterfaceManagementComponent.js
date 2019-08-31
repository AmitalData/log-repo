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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
//import { InterfaceManagementPMService } from '../../../Customs/Services/StandardPMs/InterfaceManagementPMService';
var InterfaceManagementPMExtendService_1 = require("../../../Customs/Services/ExtendedPMs/InterfaceManagementPMExtendService");
var InterfaceManagementListService_1 = require("../../../Customs/Services/StandardLists/InterfaceManagementListService");
var AddEditInterfaceManagementComponent = /** @class */ (function (_super) {
    __extends(AddEditInterfaceManagementComponent, _super);
    function AddEditInterfaceManagementComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.InterfaceManagement";
        _this.columns = null;
        //private _InterfaceManagementPMService: InterfaceManagementPMService = new InterfaceManagementPMService();
        _this._InterfaceManagementListService = new InterfaceManagementListService_1.InterfaceManagementListService();
        _this._InterfaceManagementPMExtendService = new InterfaceManagementPMExtendService_1.InterfaceManagementPMExtendService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.entityPM = null;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Loaded = false;
        _this.SignatureTypeVisibility = false;
        _this.DcaRenameVisibility = false;
        ///#region Properties
        //IsUnifreightCertificateActivatedEnabled: boolean = true;
        _this._BolTest = false;
        return _this;
    }
    AddEditInterfaceManagementComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Customs.InterfaceTenantDefinition").subscribe(function (response) {
            });
            //this.RefreshBtnClick()
        });
    };
    AddEditInterfaceManagementComponent.prototype.SetWindowArgs = function (WinArg) {
        var _this = this;
        ;
        this._TenantInterfaceManagementList = WinArg.SelectedItem;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Customs.InterfaceTenantDefinition").subscribe(function (response) {
                _this._InterfaceManagementPMExtendService
                    .GetSingleInterfaceManagementwithDefinition(_this._TenantInterfaceManagementList.Code, SessionLocator_1.SessionLocator.Tenant)
                    .subscribe(function (rsp) {
                    _this.entityPM = rsp.Result;
                    _this.ValidScreen();
                    _this.CurrentSession.StopBusyIndicator();
                });
            });
        });
    };
    AddEditInterfaceManagementComponent.prototype.ValidScreen = function () {
        if (this.entityPM.InOut == "O") {
            this.SignatureTypeVisibility = true;
        }
        else {
            this.DcaRenameVisibility = true;
        }
    };
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "BolTest", {
        get: function () {
            return this._BolTest;
        },
        set: function (value) {
            this._BolTest = value;
        },
        enumerable: true,
        configurable: true
    });
    AddEditInterfaceManagementComponent.prototype.SetDcaRenameFileEnable = function (bol) {
        this.DcaRenameFileEnable = bol;
    };
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "DcaRenameFileEnable", {
        get: function () { return this.entityPM != null ? this.entityPM.DcaRenameFileEnable : false; },
        set: function (value) {
            this.entityPM.DcaRenameFileEnable = value;
            if (!this.entityPM.DcaRenameFileEnable) {
                this.DcaRenameFilePrefix = null;
                this.UIProperties.SetEnabled("DcaRenameFilePrefix", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetEnabled("DcaRenameFilePrefix", this.ObjectTableName, true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "DcaRenameFilePrefix", {
        get: function () { return this.entityPM != null ? this.entityPM.DcaRenameFilePrefix : null; },
        set: function (value) { this.entityPM.DcaRenameFilePrefix = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "SignatureTypeCode", {
        get: function () { return this.entityPM != null ? this.entityPM.SignatureTypeCode : null; },
        set: function (value) { this.entityPM.SignatureTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "Code", {
        get: function () { return this.entityPM != null ? this.entityPM.Code : null; },
        set: function (value) { this.entityPM.Code = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "InOut", {
        get: function () { return this.entityPM != null ? this.entityPM.InOut : null; },
        set: function (value) { this.entityPM.InOut = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "DefaultPriority", {
        get: function () { return this.entityPM != null ? this.entityPM.DefaultPriority : null; },
        set: function (value) { this.entityPM.DefaultPriority = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "AllowRestore", {
        get: function () { return this.entityPM != null ? this.entityPM.AllowRestore : false; },
        set: function (value) { this.entityPM.AllowRestore = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "Description", {
        get: function () { return this.entityPM != null ? this.entityPM.Description : null; },
        set: function (value) { this.entityPM.Description = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "DefaultSendOptionsCode", {
        get: function () { return this.entityPM != null ? this.entityPM.DefaultSendOptionsCode : null; },
        set: function (value) { this.entityPM.DefaultSendOptionsCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "DcaPrefixName", {
        get: function () { return this.entityPM != null ? this.entityPM.DcaPrefixName : null; },
        set: function (value) { this.entityPM.DcaPrefixName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "DcaPrefixName2", {
        get: function () { return this.entityPM != null ? this.entityPM.DcaPrefixName2 : null; },
        set: function (value) { this.entityPM.DcaPrefixName2 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "DcaPrefixName3", {
        get: function () { return this.entityPM != null ? this.entityPM.DcaPrefixName3 : null; },
        set: function (value) { this.entityPM.DcaPrefixName3 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "DcaPrefixName4", {
        get: function () { return this.entityPM != null ? this.entityPM.DcaPrefixName4 : null; },
        set: function (value) { this.entityPM.DcaPrefixName4 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "TenantPriority", {
        //for edit 
        get: function () { return this.entityPM != null ? this.entityPM.TenantPriority : null; },
        set: function (value) { this.entityPM.TenantPriority = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditInterfaceManagementComponent.prototype, "TenantSendOptionsCode", {
        get: function () { return this.entityPM != null ? this.entityPM.TenantSendOptionsCode : null; },
        set: function (value) { this.entityPM.TenantSendOptionsCode = value; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    AddEditInterfaceManagementComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditInterfaceManagementComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var IsNew = false; //itzik : In  InterfaceManagementUpdateService  Insert/Update Tenant  !!!
        if (IsNew) {
            return;
        }
        this._InterfaceManagementPMExtendService.PutInterfaceManagementPM(this.entityPM)
            .subscribe(function (resp) {
            if (resp.HasError) {
                _this.ValidationErrorsList = [];
                _this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                return;
            }
            _this.CancelButtonClicked();
        });
    };
    AddEditInterfaceManagementComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditInterfaceManagementComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditInterfaceManagementComponent);
    return AddEditInterfaceManagementComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditInterfaceManagementComponent = AddEditInterfaceManagementComponent;
//# sourceMappingURL=AddEditInterfaceManagementComponent.js.map