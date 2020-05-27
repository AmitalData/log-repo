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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var GITITEMExtendedPMService_1 = require("../../../Customs/Services/ExtendedPMs/GITITEMExtendedPMService");
var GITITEMDto_1 = require("../../EntityPMs/Extended/GITITEMDto");
var CustomsSettingExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var GITITEMCacheService = /** @class */ (function () {
    function GITITEMCacheService() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.GITITEMExtendedPMService = new GITITEMExtendedPMService_1.GITITEMExtendedPMService();
        this.IsCountryPURForItems = false;
        this.IsUnitPURForItems = false;
        this._ItemCode_LocalCache = [];
        this.GetCountryPURForItems();
        this.GetUnitPURForItems();
    }
    GITITEMCacheService_1 = GITITEMCacheService;
    Object.defineProperty(GITITEMCacheService.prototype, "ItemCode_LocalCache", {
        /*public*/ get: function () {
            return this._ItemCode_LocalCache;
        },
        enumerable: true,
        configurable: true
    });
    //public set ItemCode_LocalCache(value: ItemCodeComponent[]) {
    //  this._ItemCode_LocalCache = value;
    //}
    GITITEMCacheService.prototype.FirstItemCodeComponent = function (itemCode) {
        var itemCodeDetails = GITITEMCacheService_1.Instance.ItemCode_LocalCache.filter(function (vm) { return vm.ItemCode == itemCode; })[0];
        return itemCodeDetails;
    };
    GITITEMCacheService.prototype.AddItemCodeComponent = function (ItemCodeComponent) {
        GITITEMCacheService_1.Instance.ItemCode_LocalCache.push(ItemCodeComponent);
    };
    //public const  ToChangeRow ="ToChangeRow"
    GITITEMCacheService.prototype.OnItemCodeAdd = function (mySupplierInvoiceItemPM, itemCodeDetails) {
        //ToChangeRowTrue_ToChangeDBFalse
        return new Promise(function (resolve) {
            var isChanged = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(mySupplierInvoiceItemPM.ClassificationCode) && mySupplierInvoiceItemPM.ClassificationCode != itemCodeDetails.ClassificationCode) {
                isChanged = true;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(mySupplierInvoiceItemPM.ItemDescription) && mySupplierInvoiceItemPM.ItemDescription != itemCodeDetails.ItemDescription) {
                isChanged = true;
            }
            if (GITITEMCacheService_1.Instance.IsUnitPURForItems && !Tools_1.AppTool.IsNullOrEmpty(mySupplierInvoiceItemPM.InvoiceQuantityType) && mySupplierInvoiceItemPM.InvoiceQuantityType != itemCodeDetails.InvoiceQuantityType) {
                isChanged = true;
            }
            if (GITITEMCacheService_1.Instance.IsCountryPURForItems && !Tools_1.AppTool.IsNullOrEmpty(mySupplierInvoiceItemPM.OriginCountryCode) && mySupplierInvoiceItemPM.OriginCountryCode != itemCodeDetails.OriginCountryCode) {
                isChanged = true;
            }
            if (!isChanged) {
                resolve(OnItemCodeAddResult.voidDoNothing);
                return;
            }
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("הערכים בטבלת פריטים שונים , האם לדרוס ערכי השורה ?");
            console.log("אחרת הנתונים הנ'ל יועדכנו ב DB !!!!!!!!!!!");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    console.log("לדרוס ערכי השורה ");
                    resolve(OnItemCodeAddResult.OverwriteRowFromDB);
                    return;
                }
                else if (confirmWindow.No) {
                    console.log("אחרת הנתונים הנ'ל יועדכנו ב DB !!!!!!!!!!!");
                    resolve(OnItemCodeAddResult.AddTaskToUpdateDB);
                    return;
                }
            });
        });
    };
    GITITEMCacheService.prototype.GetCountryPURForItems = function () {
        var _this = this;
        //this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_CTRY", "NON", "NON", SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (response) {
            //this.CurrentSession.StopBusyIndicator();
            if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
                _this.IsCountryPURForItems = true;
            }
        });
    };
    GITITEMCacheService.prototype.GetUnitPURForItems = function () {
        var _this = this;
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_UNIT", "NON", "NON", SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (response) {
            if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
                _this.IsUnitPURForItems = true;
            }
        });
    };
    GITITEMCacheService.prototype.SaveItemCodeLocalCache = function () {
        var listGITITEMDto = [];
        if (this.ItemCode_LocalCache != null && this.ItemCode_LocalCache.length > 0) {
            for (var _i = 0, _a = this.ItemCode_LocalCache; _i < _a.length; _i++) {
                var item = _a[_i];
                if (item.IsNew) {
                    item.IsNew = false; //
                    var myGITITEMPM = new GITITEMDto_1.GITITEMDto();
                    //myGITITEMPM.PARTNERID = this.declarationPM.CustomerCode;
                    if (Tools_1.AppTool.IsNullOrEmpty(item.VendorNumber)) {
                        item.VendorNumber = "NULL";
                    }
                    myGITITEMPM.PARTNERID = item.CustomerCode;
                    myGITITEMPM.SAPAKID = item.VendorNumber;
                    myGITITEMPM.ITEMNO = item.ItemCode;
                    myGITITEMPM.PRATID = item.ClassificationCode;
                    myGITITEMPM.NAMEENG = item.ItemDescription;
                    myGITITEMPM.ORIGINCOUNTRY = item.OriginCountryCode;
                    myGITITEMPM.UNITID = item.InvoiceQuantityType;
                    //this.GITITEMExtendedPMService.insert(myGITITEMPM).subscribe(myResult => {
                    //  var mm: ServiceResponse = myResult;
                    //  if (!mm.HasError) {
                    //    //this.entity = mm.Result;
                    //  }
                    //});
                    listGITITEMDto.push(myGITITEMPM);
                }
            }
            var i = 0;
            var j = 0;
            var chunk = 50;
            for (i = 0, j = listGITITEMDto.length; i < j; i += chunk) {
                var chunkDtos = listGITITEMDto.slice(i, i + chunk);
                this.GITITEMExtendedPMService.insert(chunkDtos).subscribe(function (myResult) {
                    var mm = myResult;
                    if (!mm.HasError) {
                        var entity = mm.Result;
                    }
                });
            }
        }
    };
    Object.defineProperty(GITITEMCacheService, "Instance", {
        get: function () {
            return this._instance || (this._instance = new this());
        },
        enumerable: true,
        configurable: true
    });
    var GITITEMCacheService_1;
    GITITEMCacheService = GITITEMCacheService_1 = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], GITITEMCacheService);
    return GITITEMCacheService;
}());
exports.GITITEMCacheService = GITITEMCacheService;
var OnItemCodeAddResult;
(function (OnItemCodeAddResult) {
    OnItemCodeAddResult[OnItemCodeAddResult["voidDoNothing"] = 0] = "voidDoNothing";
    OnItemCodeAddResult[OnItemCodeAddResult["OverwriteRowFromDB"] = 1] = "OverwriteRowFromDB";
    OnItemCodeAddResult[OnItemCodeAddResult["AddTaskToUpdateDB"] = 2] = "AddTaskToUpdateDB";
})(OnItemCodeAddResult = exports.OnItemCodeAddResult || (exports.OnItemCodeAddResult = {}));
//# sourceMappingURL=GITITEMCacheService.js.map