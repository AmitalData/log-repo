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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AWBSpecialHandlingCodePM_1 = require("../../../../Shipment/EntityPMs/AWBSpecialHandlingCodePM");
var IATACodePM_1 = require("../../../../Infrastructure/EntityPMs/IATACodePM");
var BookingProductPM_1 = require("../../../../Booking/EntityPMs/BookingProductPM");
var CommodityPM_1 = require("../../../../Common/EntityPMs/CommodityPM");
var AirlineMessagingRulePM_1 = require("../../../../Common/EntityPMs/AirlineMessagingRulePM");
var AWBSpecialHandlingCodeListService_1 = require("../../../../Shipment/Services/StandardLists/AWBSpecialHandlingCodeListService");
var IATACodeListService_1 = require("../../../../Infrastructure/Services/StandardLists/IATACodeListService");
var BookingProductListService_1 = require("../../../../Booking/Services/StandardLists/BookingProductListService");
var CommodityListService_1 = require("../../../../Common/Services/StandardLists/CommodityListService");
var AirlineMessagingRuleListService_1 = require("../../../../Common/Services/StandardLists/AirlineMessagingRuleListService");
var AWBSpecialHandlingCodePMService_1 = require("../../../../Shipment/Services/StandardPMs/AWBSpecialHandlingCodePMService");
var IATACodePMService_1 = require("../../../../Infrastructure/Services/StandardPMs/IATACodePMService");
var BookingProductPMService_1 = require("../../../../Booking/Services/StandardPMs/BookingProductPMService");
var CommodityPMService_1 = require("../../../../Common/Services/StandardPMs/CommodityPMService");
var AirlineMessagingRulePMService_1 = require("../../../../Common/Services/StandardPMs/AirlineMessagingRulePMService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var AirlineAdaptationsTabComponent = /** @class */ (function (_super) {
    __extends(AirlineAdaptationsTabComponent, _super);
    function AirlineAdaptationsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Airline";
        _this.DataContext = _this;
        _this.HandlingCodesObslistCount = 0;
        _this.CommoditiesObslistCount = 0;
        _this.MessagingRulesObslistCount = 0;
        _this.IATACodesObslistCount = 0;
        _this.BookingProductsObslistCount = 0;
        _this.EntityPM = entityArgs.EntityPM;
        return _this;
    }
    AirlineAdaptationsTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.SetUIProperties();
            this.LoadAllData();
        }
    };
    AirlineAdaptationsTabComponent.prototype.SetUIProperties = function () {
        if (this.IsManagingProduct) {
            this.UIProperties.SetEnabled("IsProductMandatory", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetEnabled("IsProductMandatory", this.ObjectTableName, false);
            this.IsProductMandatory = false;
        }
    };
    AirlineAdaptationsTabComponent.prototype.LoadAllData = function () {
        this.LoadSpecialCodes();
        this.LoadCommodities();
        this.LoadMessagingRules();
        this.LoadIATACodes();
        this.LoadBookingProducts();
    };
    Object.defineProperty(AirlineAdaptationsTabComponent.prototype, "IsManagingProduct", {
        // Settings
        get: function () { return this.EntityPM.IsManagingProduct; },
        set: function (newValue) {
            if (this.EntityPM.IsManagingProduct != newValue) {
                this.EntityPM.IsManagingProduct = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineAdaptationsTabComponent.prototype, "IsProductMandatory", {
        get: function () { return this.EntityPM.IsProductMandatory; },
        set: function (newValue) {
            if (this.EntityPM.IsProductMandatory != newValue) {
                this.EntityPM.IsProductMandatory = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineAdaptationsTabComponent.prototype, "IsDescriptionOfGoodsFromList", {
        get: function () { return this.EntityPM.IsDescriptionOfGoodsFromList; },
        set: function (newValue) {
            if (this.EntityPM.IsDescriptionOfGoodsFromList != newValue) {
                this.EntityPM.IsDescriptionOfGoodsFromList = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineAdaptationsTabComponent.prototype, "ScheduleDays", {
        get: function () { return this.EntityPM.ScheduleDays; },
        set: function (newValue) {
            if (this.EntityPM.ScheduleDays != newValue) {
                this.EntityPM.ScheduleDays = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineAdaptationsTabComponent.prototype, "NoAvailabilityInFVAMessages", {
        get: function () { return this.EntityPM.NoAvailabilityInFVAMessages; },
        set: function (newValue) {
            if (this.EntityPM.NoAvailabilityInFVAMessages != newValue) {
                this.EntityPM.NoAvailabilityInFVAMessages = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AirlineAdaptationsTabComponent.prototype.LoadSpecialCodes = function () {
        var _this = this;
        this.HandlingCodesObslist = [];
        var myService = new AWBSpecialHandlingCodeListService_1.AWBSpecialHandlingCodeListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allData = myResponse.Result;
                if (allData != null) {
                    var myData = allData.filter(function (d) { return d.AirlineId == _this.EntityPM.Id; });
                    myData.filter(function (d) { return !d.InActive; }).sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; }).forEach(function (item) {
                        _this.HandlingCodesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "SpecialCode", _this));
                    });
                    myData.filter(function (d) { return d.InActive; }).sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; }).forEach(function (item) {
                        _this.HandlingCodesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "SpecialCode", _this));
                    });
                    _this.HandlingCodesObslistCount = _this.HandlingCodesObslist.length;
                }
            }
        });
    };
    AirlineAdaptationsTabComponent.prototype.LoadCommodities = function () {
        var _this = this;
        this.CommoditiesObslist = [];
        var myService = new CommodityListService_1.CommodityListService();
        myService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allData = myResponse.Result;
                if (allData != null) {
                    var myData = allData.filter(function (d) { return d.AirlineId == _this.EntityPM.Id; });
                    myData.filter(function (d) { return !d.InActive; }).sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; }).forEach(function (item) {
                        _this.CommoditiesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "Commodity", _this));
                    });
                    myData.filter(function (d) { return d.InActive; }).sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; }).forEach(function (item) {
                        _this.CommoditiesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "Commodity", _this));
                    });
                    _this.CommoditiesObslistCount = _this.CommoditiesObslist.length;
                }
            }
        });
    };
    AirlineAdaptationsTabComponent.prototype.LoadMessagingRules = function () {
        var _this = this;
        this.MessagingRulesObslist = [];
        var myService = new AirlineMessagingRuleListService_1.AirlineMessagingRuleListService();
        myService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allData = myResponse.Result;
                if (allData != null) {
                    var myData = allData.filter(function (d) { return d.AirlineId == _this.EntityPM.Id; });
                    myData.filter(function (d) { return !d.InActive; }).sort(function (a, b) { return (a.MessageTypeCode === b.MessageTypeCode) ? 0 : (a.MessageTypeCode < b.MessageTypeCode) ? -1 : 1; }).forEach(function (item) {
                        _this.MessagingRulesObslist.push(new MessagingRuleItem(item, _this));
                    });
                    myData.filter(function (d) { return d.InActive; }).sort(function (a, b) { return (a.MessageTypeCode === b.MessageTypeCode) ? 0 : (a.MessageTypeCode < b.MessageTypeCode) ? -1 : 1; }).forEach(function (item) {
                        _this.MessagingRulesObslist.push(new MessagingRuleItem(item, _this));
                    });
                    _this.MessagingRulesObslistCount = _this.MessagingRulesObslist.length;
                }
            }
        });
    };
    AirlineAdaptationsTabComponent.prototype.LoadIATACodes = function () {
        var _this = this;
        this.IATACodesObslist = [];
        var myService = new IATACodeListService_1.IATACodeListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allData = myResponse.Result;
                if (allData != null) {
                    var myData = allData.filter(function (d) { return d.AirlineId == _this.EntityPM.Id; });
                    myData.filter(function (d) { return !d.InActive; }).sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; }).forEach(function (item) {
                        _this.IATACodesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "IATACode", _this));
                    });
                    myData.filter(function (d) { return d.InActive; }).sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; }).forEach(function (item) {
                        _this.IATACodesObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "IATACode", _this));
                    });
                    _this.IATACodesObslistCount = _this.IATACodesObslist.length;
                }
            }
        });
    };
    AirlineAdaptationsTabComponent.prototype.LoadBookingProducts = function () {
        var _this = this;
        this.BookingProductsObslist = [];
        var myService = new BookingProductListService_1.BookingProductListService();
        myService.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allData = myResponse.Result;
                if (allData != null) {
                    var myData = allData.filter(function (d) { return d.AirlineId == _this.EntityPM.Id; });
                    myData.filter(function (d) { return !d.InActive; }).sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; }).forEach(function (item) {
                        _this.BookingProductsObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "BookingProduct", _this));
                    });
                    myData.filter(function (d) { return d.InActive; }).sort(function (a, b) { return (a.Code === b.Code) ? 0 : (a.Code < b.Code) ? -1 : 1; }).forEach(function (item) {
                        _this.BookingProductsObslist.push(new AirlineAdaptationItem(item.Id, item.Code, item.Name, item.InActive, "BookingProduct", _this));
                    });
                    _this.BookingProductsObslistCount = _this.BookingProductsObslist.length;
                }
            }
        });
    };
    AirlineAdaptationsTabComponent.prototype.AddClicked = function (type) {
        var _this = this;
        this.entityType = type;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        var myWindowTitle = "Add ";
        var itemPM;
        var objectTableName;
        switch (type) {
            case "SpecialCode": {
                myWindowTitle += "AWB Special Handling Code";
                objectTableName = "AWBSpecialHandlingCode";
                itemPM = new AWBSpecialHandlingCodePM_1.AWBSpecialHandlingCodePM();
                itemPM.AirlineId = this.EntityPM.Id;
                break;
            }
            case "Commodity": {
                myWindowTitle += "Commodity";
                objectTableName = "Commodity";
                itemPM = new CommodityPM_1.CommodityPM();
                itemPM.Tenant = this.EntityPM.Tenant;
                itemPM.AirlineId = this.EntityPM.Id;
                break;
            }
            case "MessagingRule": {
                myWindowTitle += "Messaging Rule";
                objectTableName = "AirlineMessagingRule";
                itemPM = new AirlineMessagingRulePM_1.AirlineMessagingRulePM();
                itemPM.Tenant = this.EntityPM.Tenant;
                itemPM.AirlineId = this.EntityPM.Id;
                itemPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                itemPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                itemPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                itemPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                break;
            }
            case "IATACode": {
                myWindowTitle += "IATA Code";
                objectTableName = "IATACode";
                itemPM = new IATACodePM_1.IATACodePM();
                itemPM.AirlineId = this.EntityPM.Id;
                break;
            }
            case "BookingProduct": {
                myWindowTitle += "Booking Product";
                objectTableName = "BookingProduct";
                itemPM = new BookingProductPM_1.BookingProductPM();
                itemPM.AirlineId = this.EntityPM.Id;
                break;
            }
        }
        logitudeWindow.Title = myWindowTitle;
        var service = new EntityResourceService_1.EntityResourceService();
        service.getEntityResourceByTableName(objectTableName).subscribe(function (response) {
            if (type == "MessagingRule") {
                logitudeWindow.WindowArgs = { AirlinePM: _this.EntityPM, EntityPM: itemPM, ObjectTableName: objectTableName, IsNew: true, };
                logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
                logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineMessagingRuleComponent');
            }
            else {
                logitudeWindow.WindowArgs = { AirlinePM: _this.EntityPM, EntityPM: itemPM, ObjectTableName: objectTableName, IsNew: true, };
                logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
                logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineAdaptationItemComponent');
            }
        });
    };
    AirlineAdaptationsTabComponent.prototype.EditClicked = function (item, type) {
        var _this = this;
        this.entityType = type;
        var myWindowTitle = "Edit ";
        var objectTableName;
        var myService;
        var itemPM;
        switch (type) {
            case "SpecialCode": {
                myWindowTitle += "AWB Special Handling Code";
                objectTableName = "AWBSpecialHandlingCode";
                myService = new AWBSpecialHandlingCodePMService_1.AWBSpecialHandlingCodePMService();
                myService.get(item.Id).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        itemPM = myResponse.Result;
                        if (itemPM != null) {
                            _this.OpenEditWindow(myWindowTitle, itemPM, objectTableName);
                        }
                    }
                });
                break;
            }
            case "Commodity": {
                myWindowTitle += "Commodity";
                objectTableName = "Commodity";
                myService = new CommodityPMService_1.CommodityPMService();
                myService.get(item.Id).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        itemPM = myResponse.Result;
                        if (itemPM != null) {
                            _this.OpenEditWindow(myWindowTitle, itemPM, objectTableName);
                        }
                    }
                });
                break;
            }
            case "IATACode": {
                myWindowTitle += "IATA Code";
                objectTableName = "IATACode";
                myService = new IATACodePMService_1.IATACodePMService();
                myService.get(item.Id).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        itemPM = myResponse.Result;
                        if (itemPM != null) {
                            _this.OpenEditWindow(myWindowTitle, itemPM, objectTableName);
                        }
                    }
                });
                break;
            }
            case "BookingProduct": {
                myWindowTitle += "Booking Product";
                objectTableName = "BookingProduct";
                myService = new BookingProductPMService_1.BookingProductPMService();
                myService.get(item.Id).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        itemPM = myResponse.Result;
                        if (itemPM != null) {
                            _this.OpenEditWindow(myWindowTitle, itemPM, objectTableName);
                        }
                    }
                });
                break;
            }
        }
    };
    AirlineAdaptationsTabComponent.prototype.OpenEditWindow = function (myWindowTitle, itemPM, objectTableName) {
        var _this = this;
        var service = new EntityResourceService_1.EntityResourceService();
        service.getEntityResourceByTableName(objectTableName).subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = myWindowTitle;
            logitudeWindow.WindowArgs = { AirlinePM: _this.EntityPM, EntityPM: itemPM, ObjectTableName: objectTableName, IsNew: false, };
            logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
            logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineAdaptationItemComponent');
        });
    };
    AirlineAdaptationsTabComponent.prototype.EditRuleClicked = function (item) {
        var _this = this;
        var service = new EntityResourceService_1.EntityResourceService();
        service.getEntityResourceByTableName("AirlineMessagingRule").subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            var myWindowTitle = "Edit Messaging Rule";
            var objectTableName = "AirlineMessagingRule";
            var myService = new AirlineMessagingRulePMService_1.AirlineMessagingRulePMService();
            myService.get(item.Id).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    var itemPM = myResponse.Result;
                    if (itemPM != null) {
                        logitudeWindow.Title = myWindowTitle;
                        logitudeWindow.WindowArgs = { AirlinePM: _this.EntityPM, EntityPM: itemPM, ObjectTableName: objectTableName, IsNew: false, };
                        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnWindowClosed($event); });
                        logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineMessagingRuleComponent');
                    }
                }
            });
        });
    };
    AirlineAdaptationsTabComponent.prototype.OnWindowClosed = function (message) {
        if (message == "ok") {
            switch (this.entityType) {
                case "SpecialCode": {
                    this.LoadSpecialCodes();
                    break;
                }
                case "Commodity": {
                    this.LoadCommodities();
                    break;
                }
                case "MessagingRule": {
                    this.LoadMessagingRules();
                    break;
                }
                case "IATACode": {
                    this.LoadIATACodes();
                    break;
                }
                case "BookingProduct": {
                    this.LoadBookingProducts();
                    break;
                }
            }
        }
    };
    AirlineAdaptationsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AirlineAdaptationsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AirlineAdaptationsTabComponent);
    return AirlineAdaptationsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AirlineAdaptationsTabComponent = AirlineAdaptationsTabComponent;
var AirlineAdaptationItem = /** @class */ (function (_super) {
    __extends(AirlineAdaptationItem, _super);
    function AirlineAdaptationItem(entityId, entityCode, entityName, inactive, entityType, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.Id = entityId;
        _this.Code = entityCode;
        _this.Name = entityName;
        _this.InActive = inactive;
        _this.entityCode = entityCode;
        return _this;
    }
    return AirlineAdaptationItem;
}(BaseComponent_1.BaseComponent));
exports.AirlineAdaptationItem = AirlineAdaptationItem;
var MessagingRuleItem = /** @class */ (function (_super) {
    __extends(MessagingRuleItem, _super);
    function MessagingRuleItem(entity, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.entityList = entity;
        return _this;
    }
    Object.defineProperty(MessagingRuleItem.prototype, "Id", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MessagingRuleItem.prototype, "MessageTypeCode", {
        get: function () { return this.entityList.MessageTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MessagingRuleItem.prototype, "RuleFieldName", {
        get: function () { return this.entityList.RuleFieldName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MessagingRuleItem.prototype, "InActive", {
        get: function () { return this.entityList.InActive; },
        enumerable: true,
        configurable: true
    });
    return MessagingRuleItem;
}(BaseComponent_1.BaseComponent));
exports.MessagingRuleItem = MessagingRuleItem;
//# sourceMappingURL=AirlineAdaptationsTabComponent.js.map