"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var LogTabsComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CommunicationLogMoreDetailsComponent = /** @class */ (function () {
    function CommunicationLogMoreDetailsComponent() {
        this.MyTabs = [];
    }
    Object.defineProperty(CommunicationLogMoreDetailsComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    CommunicationLogMoreDetailsComponent.prototype.ngOnInit = function () {
    };
    CommunicationLogMoreDetailsComponent.prototype.OnSelectedChanged = function (tab) {
        if (!Tools_1.AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;
            console.log("Tab selected: ", tab);
        }
    };
    CommunicationLogMoreDetailsComponent.prototype.SetWindowArgs = function (CustomsRequestsSheetList) {
        //alert();
        var tab = new LogTabsComponent_1.LogTab();
        //this.EntityPM.Id, this.EntityPM.Tenant
        tab.EntityPM = {
            'Id': CustomsRequestsSheetList.RequestComminicationId,
            'Tenant': CustomsRequestsSheetList.Tenant,
        };
        tab.Code = "CommunicationLogSteps";
        tab.Header = TextCodeTranslator_1.TextCodeTranslator.Translate("CommunicationLogSteps.O.CommunicationLogSteps");
        //tab.ComponentPath = "./Components/Maintenance/CommunicationLog/CommunicationLogStepsComponent";
        //tab.ComponentPath = "./Customs/Components/Declaration/EditTabs/ConsigmentTabContentComponent";
        //tab.ComponentPath = './Components/Communications/CommunicationStepsComponent';
        tab.ComponentPath = './InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationStepsComponent';
        this.MyTabs.push(tab);
        var tab2 = new LogTabsComponent_1.LogTab();
        tab2.EntityPM = {
            'Id': CustomsRequestsSheetList.RequestComminicationId,
            'Tenant': CustomsRequestsSheetList.Tenant,
            'CorrelationId': CustomsRequestsSheetList.CorrelationId,
            'CustomsRequestsSheetId': CustomsRequestsSheetList.Id,
        };
        tab2.Code = "MoreDetails";
        tab2.Header = TextCodeTranslator_1.TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");
        tab2.ComponentPath = './InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationMoreComponent';
        this.MyTabs.push(tab2);
        //console.debug("SetWindowArgs");
    };
    CommunicationLogMoreDetailsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'communication-log-more-details',
            templateUrl: './CommunicationLogMoreDetailsComponent.html',
        })
    ], CommunicationLogMoreDetailsComponent);
    return CommunicationLogMoreDetailsComponent;
}());
exports.CommunicationLogMoreDetailsComponent = CommunicationLogMoreDetailsComponent;
//# sourceMappingURL=CommunicationLogMoreDetailsComponent.js.map