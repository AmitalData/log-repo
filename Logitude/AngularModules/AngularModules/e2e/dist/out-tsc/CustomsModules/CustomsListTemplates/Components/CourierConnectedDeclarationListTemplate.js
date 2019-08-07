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
var CourierConnectedDeclarationListTemplate = /** @class */ (function () {
    function CourierConnectedDeclarationListTemplate(CD) {
        this.CD = CD;
        this.IsConnectedDeclarationChecked = true;
        this.IsNotConnectedDeclarationChecked = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    CourierConnectedDeclarationListTemplate.prototype.setVariables = function (rowData, fieldName, additionalData) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.entityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.BuildDeclarationsCheckBox();
        this.CD.detectChanges();
    };
    CourierConnectedDeclarationListTemplate.prototype.BuildDeclarationsCheckBox = function () {
        var _this = this;
        var sNotConnectedDeclarations = this.entityPM.NotConnectedDeclarations;
        if (!Tools_1.AppTool.IsNullOrEmpty(sNotConnectedDeclarations)) {
            var NotConnectedDeclarations = sNotConnectedDeclarations.split(',');
            var res = NotConnectedDeclarations.filter(function (r) { return r == _this.rowData.Id; })[0];
            this.IsConnectedDeclarationChecked = Tools_1.AppTool.IsNullOrEmpty(res);
        }
        var sConnectedDeclarations = this.entityPM.ConnectedDeclarations;
        if (!Tools_1.AppTool.IsNullOrEmpty(sConnectedDeclarations)) {
            var ConnectedDeclarations = sConnectedDeclarations.split(',');
            var res = ConnectedDeclarations.filter(function (r) { return r == _this.rowData.Id; })[0];
            this.IsNotConnectedDeclarationChecked = !Tools_1.AppTool.IsNullOrEmpty(res);
        }
    };
    CourierConnectedDeclarationListTemplate.prototype.ShowDeclarationScreen = function () {
        //  this.EditEntity("Customs.Declaration", this.rowData.Id, null, "DEGC");
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: _this.rowData.Id,
                ObjectTableName: "Customs.Declaration"
            });
        });
    };
    CourierConnectedDeclarationListTemplate.prototype.OnConnectedCheckBoxChecked = function ($event) {
        if (!this.entityPM.NotConnectedDeclarations) {
            this.entityPM.NotConnectedDeclarations = "";
        }
        if (!$event) {
            if (!this.entityPM.NotConnectedDeclarations.includes(this.rowData.Id)) {
                this.entityPM.NotConnectedDeclarations = this.entityPM.NotConnectedDeclarations + this.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.NotConnectedDeclarations.includes(this.rowData.Id)) {
                this.entityPM.NotConnectedDeclarations = this.entityPM.NotConnectedDeclarations.replace(this.rowData.Id + ",", "");
            }
        }
    };
    CourierConnectedDeclarationListTemplate.prototype.OnNotConnectedCheckBoxChecked = function ($event) {
        if (!this.entityPM.ConnectedDeclarations) {
            this.entityPM.ConnectedDeclarations = "";
        }
        if ($event) {
            if (!this.entityPM.ConnectedDeclarations.includes(this.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations + this.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.ConnectedDeclarations.includes(this.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations.replace(this.rowData.Id + ",", "");
            }
        }
    };
    CourierConnectedDeclarationListTemplate = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CourierConnectedDeclarationListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], CourierConnectedDeclarationListTemplate);
    return CourierConnectedDeclarationListTemplate;
}());
exports.CourierConnectedDeclarationListTemplate = CourierConnectedDeclarationListTemplate;
//# sourceMappingURL=CourierConnectedDeclarationListTemplate.js.map