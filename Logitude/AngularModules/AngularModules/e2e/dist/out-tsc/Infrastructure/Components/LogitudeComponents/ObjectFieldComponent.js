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
var ObjectFieldPM_1 = require("../../EntityPMs/ObjectFieldPM");
var ObjectFieldComponent = /** @class */ (function () {
    function ObjectFieldComponent() {
        this.ComponentRef = null;
        this.ComponentInstance = null;
    }
    ObjectFieldComponent.prototype.ngOnInit = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        if (table) {
            //this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
            if (this.ObjectField) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load(this.ObjectField.GeneratedComponentPath, this.viewContainerRef)
                    .then(function (cmpRef) {
                    _this.ComponentRef = cmpRef;
                    cmpRef.instance.ComponentRef = cmpRef;
                    //cmpRef.instance.IsInsideWindow = true;
                    //cmpRef.instance.ComponentBackground = "transparent";
                    cmpRef.instance.Run({ ObjectField: _this.ObjectField, ObjectTableName: _this.ObjectTableName, DataContext: _this.DataContext, IsNewEntityCall: _this.IsNewEntityCall });
                });
            }
        }
    };
    ObjectFieldComponent.prototype.ngAfterViewInit = function () {
    };
    ObjectFieldComponent.prototype.ngOnDestroy = function () {
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", ObjectFieldPM_1.ObjectFieldPM)
    ], ObjectFieldComponent.prototype, "ObjectField", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], ObjectFieldComponent.prototype, "ObjectTableName", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], ObjectFieldComponent.prototype, "DataContext", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], ObjectFieldComponent.prototype, "IsNewEntityCall", void 0);
    __decorate([
        core_1.ViewChild("ComponentContent", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ObjectFieldComponent.prototype, "viewContainerRef", void 0);
    ObjectFieldComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ObjectFieldComponent',
            template: "<div #ComponentContent></div>",
        })
    ], ObjectFieldComponent);
    return ObjectFieldComponent;
}());
exports.ObjectFieldComponent = ObjectFieldComponent;
//# sourceMappingURL=ObjectFieldComponent.js.map