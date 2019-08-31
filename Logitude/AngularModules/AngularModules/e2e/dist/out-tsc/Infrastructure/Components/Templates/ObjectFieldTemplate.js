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
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var Tools_1 = require("../../Tools");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var ObjectFieldTemplate = /** @class */ (function () {
    function ObjectFieldTemplate(CD) {
        this.CD = CD;
        this.Entity = null;
        this.CustomField = null;
        this.ObjectTable = null;
        this.ObjectField = null;
        this.FieldName = null;
        this.FieldValue = null;
        this.HasTemplate = false;
        this.DataTypeCode = null;
        this.DigitsAfterPoints = "n0";
        this.IsAutoFormat = false;
        this.IsLookUp = false;
        this.LookUpFieldValue = null;
        this.IsHeaderScreenTemplate = false;
        this.IsListColumnCellTemplate = false;
        this.IsListColumnHeaderTemplate = false;
        this.IsSpotLightTemplate = false;
        this.Direction = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        this.TextAlign = this.Direction == 'rtl' ? 'right' : 'left';
        this.NumberFieldTextAlign = "right";
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ObjectFieldTemplate.prototype.ngOnInit = function () {
        var _this = this;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        if (this.ObjectField != null && this.IsSpotLightTemplate == false) {
            //this.IsCustom = this.ObjectField.IsCustom;
            this.HasTemplate = this.ObjectField.HasTemplate;
            this.DataTypeCode = this.ObjectField.DataTypeCode;
            this.DigitsAfterPoints = "n" + this.ObjectField.DigitsAfterPoint;
            if (this.IsHeaderScreenTemplate) {
                this.FieldName = this.ObjectField.PMPropertyPath;
                this.NumberFieldTextAlign = "left";
            }
            else {
                this.FieldName = this.ObjectField.FieldName;
            }
            if (Tools_1.AppTool.IsNullOrEmpty(this.FieldName)) {
                this.FieldName = this.ObjectField.FieldName;
            }
            switch (this.DataTypeCode) {
                case "Decimal":
                case "Double":
                case "Boolean":
                case "Date":
                case "DateTime":
                case "Integer":
                    {
                        if (this.HasTemplate != true) {
                            this.IsAutoFormat = true;
                        }
                        break;
                    }
                case "LookUp":
                case "PickList":
                    {
                        this.IsLookUp = true;
                        break;
                    }
            }
            if (this.HasTemplate) {
                this.LoadTemplate();
            }
            else {
                if (this.Entity != null) {
                    if (this.IsHeaderScreenTemplate && this.IsLookUp) {
                        if (this.ObjectField.IsCustom) {
                            this.CustomField = this.Entity[this.FieldName];
                            if (this.CustomField) {
                                this.FieldValue = this.CustomField.Value;
                            }
                        }
                        else {
                            this.FieldValue = this.Entity[this.FieldName];
                        }
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.FieldValue)) {
                            if (this.ObjectField.LookUpTableId) {
                                var ObjectTable = window.ObjectTables.filter(function (x) { return x.Id === _this.ObjectField.LookUpTableId; })[0];
                                if (ObjectTable) {
                                    var moduleName = ObjectTable.ClientModuleName;
                                    var objectTableName = ObjectTable.Name;
                                    if (objectTableName.indexOf('Customs.') > -1) {
                                        objectTableName = objectTableName.split('.')[1];
                                    }
                                    var servicename = objectTableName + "ListService";
                                    var servicelink = './' + moduleName + '/Services/StandardLists/' + servicename;
                                    return new Promise(function (resolve, reject) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (myService) {
                                            if (myService) {
                                                if (ObjectTable.CacheOnClient)
                                                    myService.getSingleFromCache(_this.FieldValue).subscribe(function (myResponse) {
                                                        if (!myResponse.HasError) {
                                                            if (myResponse.Result) {
                                                                var myResultValue = myResponse.Result["Name"];
                                                                if (Tools_1.AppTool.IsNullOrEmpty(myResultValue)) {
                                                                    myResultValue = myResponse.Result["EnglishName"];
                                                                }
                                                                _this.LookUpFieldValue = myResultValue;
                                                            }
                                                        }
                                                    });
                                                else
                                                    myService.getSingle(_this.FieldValue).subscribe(function (myResponse) {
                                                        if (!myResponse.HasError) {
                                                            if (myResponse.Result) {
                                                                var myResultValue = myResponse.Result["Name"];
                                                                if (Tools_1.AppTool.IsNullOrEmpty(myResultValue)) {
                                                                    myResultValue = myResponse.Result["EnglishName"];
                                                                }
                                                                _this.LookUpFieldValue = myResultValue;
                                                            }
                                                        }
                                                    });
                                            }
                                        });
                                    });
                                }
                            }
                        }
                    }
                    else {
                        if (this.ObjectField.IsCustom) {
                            this.CustomField = this.Entity[this.FieldName];
                            if (this.CustomField) {
                                if (this.ObjectField.DataTypeCode == 'Boolean') {
                                    if (this.IsHeaderScreenTemplate) {
                                        this.FieldValue = (this.CustomField.Value == "True" || this.CustomField.Value == "true") ? true : false;
                                    }
                                    else {
                                        this.FieldValue = (this.CustomField == "True" || this.CustomField == "true") ? true : false;
                                    }
                                }
                                else {
                                    if (this.IsHeaderScreenTemplate) {
                                        this.FieldValue = this.CustomField['Value'];
                                    }
                                    else {
                                        this.FieldValue = this.CustomField;
                                    }
                                }
                            }
                        }
                        else {
                            this.FieldValue = this.Entity[this.FieldName];
                        }
                    }
                    this.DetectChanges();
                }
            }
        }
        if (this.IsSpotLightTemplate == true) {
            this.LoadSpotLightTemplate();
        }
    };
    ObjectFieldTemplate.prototype.LoadTemplate = function () {
        var _this = this;
        if (this.HasTemplate) {
            if (this.viewContainerRef) {
                if (this.Entity != null && this.ObjectTable != null && this.ObjectField != null) {
                    var myComponentPath = null;
                    if (this.ObjectTable.ClientModuleName == "Infrastructure") {
                        myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Templates/InfrastructureFieldTemplateComponent";
                    }
                    else {
                        myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Templates/FieldTemplateComponent";
                    }
                    SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.Run({ Entity: _this.Entity, FieldName: _this.FieldName, ObjectTableName: _this.ObjectTable.Name, IsHeaderScreenTemplate: _this.IsHeaderScreenTemplate });
                        _this.DetectChanges();
                    });
                }
            }
        }
    };
    ObjectFieldTemplate.prototype.LoadSpotLightTemplate = function () {
        var _this = this;
        if (this.viewContainerRef) {
            if (this.Entity != null && this.ObjectTable != null) {
                var myComponentPath = null;
                if (this.ObjectTable.ClientModuleName == "Infrastructure") {
                    myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Templates/InfrastructureFieldTemplateComponent";
                }
                else {
                    myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/Templates/FieldTemplateComponent";
                }
                SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.Run({ Entity: _this.Entity, FieldName: _this.FieldName, ObjectTableName: _this.ObjectTable.Name, IsHeaderScreenTemplate: _this.IsHeaderScreenTemplate, IsSpotLightTemplate: _this.IsSpotLightTemplate, SpotlightDataTemplate: _this.SpotlightDataTemplate });
                    _this.DetectChanges();
                    _this.CurrentSession.SessionEvent.subscribe(function (s) {
                        if (s == "SpotLightDetectChanges") {
                            _this.DetectChanges();
                        }
                    });
                });
            }
        }
    };
    ObjectFieldTemplate.prototype.DetectChanges = function () {
        if (this.CD) {
            var isDestroyed = this.CD['destroyed'];
            if (!isDestroyed) {
                //console.log("DetectChanges Templates") 
                this.CD.detectChanges();
            }
        }
    };
    ObjectFieldTemplate.prototype.ngOnDestroy = function () {
        this.CD = null;
    };
    __decorate([
        core_1.ViewChild('Template', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ObjectFieldTemplate.prototype, "viewContainerRef", void 0);
    ObjectFieldTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "./ObjectFieldTemplate.html",
            selector: 'ObjectFieldTemplate',
            inputs: ['ObjectTable', 'ObjectField', 'FieldName', 'Entity', 'IsHeaderScreenTemplate', 'IsListColumnCellTemplate', 'IsListColumnHeaderTemplate', 'IsSpotLightTemplate', 'SpotlightDataTemplate'],
        })
        // https://github.com/angular/angular/issues/10762
        // http://stackoverflow.com/questions/39794156/angular2-dynamically-added-elements
        ,
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ObjectFieldTemplate);
    return ObjectFieldTemplate;
}());
exports.ObjectFieldTemplate = ObjectFieldTemplate;
//# sourceMappingURL=ObjectFieldTemplate.js.map