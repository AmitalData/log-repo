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
var MenuButtonPM_1 = require("../../../EntityPMs/MenuButtonPM");
var MenuButtonGroupPM_1 = require("../../../EntityPMs/MenuButtonGroupPM");
var EntityArgs_1 = require("../../../DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../Utilities/SessionLocator");
var ServiceHelper_1 = require("../../../Utilities/ServiceHelper");
var Tools_1 = require("../../../Tools");
var FeatureLocator_1 = require("../../../Utilities/FeatureLocator");
var ObjectsLocator_1 = require("../../../Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var MenuButtonsComponent = /** @class */ (function () {
    function MenuButtonsComponent(entityArgs, cd) {
        this.entityArgs = entityArgs;
        this.cd = cd;
        this.ToggleButtonTop = "22px";
        this.IsDisableMenuOther = false;
        this.Loaded = false;
        this.ToggleButtonWidth = 60;
        this.LoadCompleted = new core_1.EventEmitter();
        this.LayoutDirection = 'ltr';
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.baseMetaUrlApi = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/ngMetaData";
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    MenuButtonsComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        if (_this.MenuButtonsHandler && _this.MenuButtons) {
                            _this.MenuButtonsHandler.EntityPM = _this.EntityPM;
                            _this.MenuButtonsHandler.CheckButtonState(_this.MenuButtons);
                        }
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                        if (_this.MenuButtonsHandler && _this.MenuButtons) {
                            _this.MenuButtonsHandler.EntityPM = _this.EntityPM;
                            _this.MenuButtonsHandler.CheckButtonState(_this.MenuButtons);
                        }
                    }
                });
            }
        }
    };
    MenuButtonsComponent.prototype.ngOnDestroy = function () {
        if (this.SaveCompletedEvent) {
            this.SaveCompletedEvent.unsubscribe();
            this.SaveCompletedEvent = null;
        }
        if (this.LoadCompletedEvent) {
            this.LoadCompletedEvent.unsubscribe();
            this.LoadCompletedEvent = null;
        }
    };
    MenuButtonsComponent.prototype.Run = function (args) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTable = args['ObjectTable'];
        if (this.ObjectTable && this.ObjectTable.Name == "Quote") {
            if (this.EntityPM != null) {
                if (this.EntityPM.IsQuoteDataExternal && this.EntityPM.IsQuoteDocumentExternal) {
                    this.IsDisableMenuOther = true;
                }
            }
        }
        this.LoadMenuButtons();
    };
    MenuButtonsComponent.prototype.LoadMenuButtons = function () {
        var _this = this;
        ServiceHelper_1.ServiceHelper.Http.get(this.baseMetaUrlApi + "/getmenubuttongrouppms?tenant=" + SessionLocator_1.SessionLocator.Tenant + "&objecttableid=" + this.ObjectTable.Id).subscribe(function (response) {
            var pm = response.json()[0];
            _this.MenuButtonGroup = _this.MapJsonToEntityPM(pm, true);
            _this.BuildMenuButtons();
        });
    };
    MenuButtonsComponent.prototype.BuildMenuButtons = function () {
        var _this = this;
        this.Listen();
        this.ToggleButtonWidth = 60;
        var btns = this.MenuButtonGroup.MenuButtons.sort(function (a, b) {
            if (a.Index > b.Index) {
                return 1;
            }
            else if (b.Index > a.Index) {
                return -1;
            }
            return 0;
        });
        var buttons = [];
        for (var i = 0; i < btns.length; i++) {
            if (btns[i].FeatureId != null && btns[i].FeatureId != undefined) {
                if (FeatureLocator_1.FeatureLocator.IsFeatureGranted(btns[i].FeatureId)) {
                    buttons.push(btns[i]);
                }
            }
            else {
                buttons.push(btns[i]);
            }
        }
        var objectTableName = this.ObjectTable.Name;
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/MenuButtons/" + objectTableName + "MenuButtonsHandler";
        var myComponentName = Tools_1.AppTool.GetComponentName(myComponentPath);
        SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(myComponentPath).then(function (instance) {
            if (instance) {
                _this.MenuButtonsHandler = instance;
                instance.SetEntityPM(_this.entityArgs);
                instance.CheckButtonState(buttons);
                for (var i = 0; i < buttons.length; i++) {
                    buttons[i].Width == 0 ? buttons[i].Width = 110 : null;
                    if (buttons[i].EventCode == "Accept" || buttons[i].EventCode == "Decline") {
                        buttons[i].Width = 70;
                    }
                    if (objectTableName == "PaymentCheque" && buttons[i].EventCode == "More") {
                        _this.ToggleButtonWidth = 100;
                    }
                    if (buttons[i].DisplayText == null) {
                        buttons[i].DisplayText = TextCodeTranslator_1.TextCodeTranslator.Translate(buttons[i].LabelTextCodeCode);
                    }
                    else
                        buttons[i].DisplayText = buttons[i].DisplayText;
                }
                _this.MenuButtons = buttons;
                return;
            }
        });
    };
    MenuButtonsComponent.prototype.OnClick = function (button) {
        this.MenuButtonsHandler.MenuButtonClick(button);
    };
    MenuButtonsComponent.prototype.MapJsonToEntityPM = function (jsonPM, getCallMap, entityPM) {
        if (getCallMap === void 0) { getCallMap = true; }
        if (entityPM === void 0) { entityPM = null; }
        if (!entityPM) {
            entityPM = new MenuButtonGroupPM_1.MenuButtonGroupPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        if (getCallMap) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {
            entityPM.OldEntityPM = null;
        }
        this.MapMenuButtons(entityPM, jsonPM);
        return entityPM;
    };
    MenuButtonsComponent.prototype.clone = function (jsonPM) {
        var entityPM;
        entityPM = {};
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        return entityPM;
    };
    MenuButtonsComponent.prototype.MapMenuButtons = function (entityPM, jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        entityPM.MenuButtons = new Array();
        for (var pack in jsonPM.MenuButtons) {
            var itemJson = jsonPM.MenuButtons[pack];
            var itemPM;
            if (mapParent) {
                itemPM = new MenuButtonPM_1.MenuButtonPM(entityPM);
            }
            else {
                itemPM = new MenuButtonPM_1.MenuButtonPM(null);
            }
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {
                if (pmKeys[key] === "entityParentPM" || pmKeys[key] === "UIProperties") {
                    continue;
                }
                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }
            itemPM.IsDirty = false;
            //itemPM.UIProperties = null;
            entityPM.MenuButtons.push(itemPM);
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], MenuButtonsComponent.prototype, "LoadCompleted", void 0);
    MenuButtonsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'MenuButtonsComponent',
            templateUrl: "./MenuButtonsComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], MenuButtonsComponent);
    return MenuButtonsComponent;
}());
exports.MenuButtonsComponent = MenuButtonsComponent;
//# sourceMappingURL=MenuButtonsComponent.js.map