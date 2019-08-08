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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var InterfaceManagementsListTemplate = /** @class */ (function () {
    function InterfaceManagementsListTemplate(CD) {
        this.CD = CD;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.SendOptionName = "";
        //if (AppTool.IsNullOrEmpty(InterfaceManagementsListTemplate.translate_CommunicationLogBView)) {
        //    this._entityResourceService.getEntityResourceByTableName("CommunicationLog")
        //        .subscribe(response => {
        //            InterfaceManagementsListTemplate.translate_CommunicationLogBView = TextCodeTranslator.Translate("CommunicationLog.B.View");// itzik : Translate +_entityResourceService - its bad :due that i done this- 
        //        });
        //}
    }
    InterfaceManagementsListTemplate_1 = InterfaceManagementsListTemplate;
    Object.defineProperty(InterfaceManagementsListTemplate.prototype, "CommunicationLogBView", {
        // itzik : Translate +_entityResourceService - its bad :due that i done this- 
        get: function () {
            return InterfaceManagementsListTemplate_1.translate_CommunicationLogBView;
        },
        enumerable: true,
        configurable: true
    });
    InterfaceManagementsListTemplate.prototype.setVariables = function (InterfaceManagementList, fieldName) {
        ///console.log(rowData);
        this._InterfaceManagementList = InterfaceManagementList;
        this.fieldName = fieldName;
        this.RefreshFields();
    };
    InterfaceManagementsListTemplate.prototype.RefreshFields = function () {
        if (this._InterfaceManagementList.HasDefinition) {
            this.SendOptionName = this._InterfaceManagementList.TenantSendOptionName;
        }
        else {
            this.SendOptionName = this._InterfaceManagementList.DefaultSendOptionName;
        }
        //C	Company
        //N	None
        //P	Personal
        //p	Personal
        if (!Tools_1.AppTool.IsNullOrEmpty(this._InterfaceManagementList.SignatureTypeCode)) {
            if (this._InterfaceManagementList.SignatureTypeCode.toUpperCase() == "C") {
                this._InterfaceManagementList.SignatureTypeName = "חברתי";
            }
            else if (this._InterfaceManagementList.SignatureTypeCode.toUpperCase() == "C") {
                this._InterfaceManagementList.SignatureTypeName = "אישי";
            }
        }
        //else {
        //    this.TableUpdateButtonIsEnabled = false;
        //    this.TableUpdateButtonOpacity = "0.7";
        //}
        this.CD.detectChanges();
    };
    InterfaceManagementsListTemplate.prototype.EditDetailsButtonClick = function () {
        alert("EditDetailsButtonClick");
    };
    var InterfaceManagementsListTemplate_1;
    InterfaceManagementsListTemplate.translate_CommunicationLogBView = "";
    InterfaceManagementsListTemplate = InterfaceManagementsListTemplate_1 = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './InterfaceManagementsListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], InterfaceManagementsListTemplate);
    return InterfaceManagementsListTemplate;
}());
exports.InterfaceManagementsListTemplate = InterfaceManagementsListTemplate;
//# sourceMappingURL=InterfaceManagementsListTemplate.js.map