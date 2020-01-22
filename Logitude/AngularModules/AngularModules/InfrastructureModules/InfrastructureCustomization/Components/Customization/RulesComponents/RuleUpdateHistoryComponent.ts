import { Component } from '@angular/core';
import { GeneralDomainService, FieldsTranslations } from '../../../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { ObjectFieldPM } from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { TextCodePM } from '../../../../../Infrastructure/EntityPMs/TextCodePM';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { ObjectTableRulePMService } from '../../../../../Infrastructure/Services/StandardPMs/ObjectTableRulePMService';
import { ObjectTableRuleFieldPMService } from '../../../../../Infrastructure/Services/StandardPMs/ObjectTableRuleFieldPMService';
import { ObjectTableRulePM } from '../../../../../Infrastructure/EntityPMs/ObjectTableRulePM';
import { RuleConditionFieldPM } from '../../../../../Infrastructure/EntityPMs/RuleConditionFieldPM';
import { ObjectTableRuleFieldPM } from '../../../../../Infrastructure/EntityPMs/ObjectTableRuleFieldPM';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { RuleUpdateHistoryListService } from '../../../../../Infrastructure/Services/StandardLists/RuleUpdateHistoryListService';
import { from } from 'rxjs/observable/from';
import { RuleUpdateHistoryList } from '../../../../../Infrastructure/EntityLists/RuleUpdateHistoryList';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { filter } from 'rxjs/operator/filter';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './RuleUpdateHistoryComponent.html',
})
export class RuleUpdateHistoryComponent {

    public IsNoDataFound:boolean = false;
    private ruleUpdateHistoryListService: RuleUpdateHistoryListService;
    public IsResourcesReady: boolean = false;
    private ObjectTableId: string;
    private RulePM:ObjectTableRulePM;
    public RuleUpdateHistoryItems: RuleUpdateHistoryList[] = [];
    
    private entityResourceService: EntityResourceService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        //        window.ObjectTableRules = [];

        this.entityResourceService = new EntityResourceService();
        this.ruleUpdateHistoryListService =  new RuleUpdateHistoryListService();     
    }

    SetWindowArgs(windowArgs: any) {
        this.ObjectTableId = windowArgs.ObjectTableID;
        this.RulePM = windowArgs.EntityPM;
        //this.AllTableRules = window.ObjectTableRules.filter(r => r.ObjectTableId === this.ObjectTableId && r.Internal === false);
        //this.TableRulesItems = this.AllTableRules;

        this.IsResourcesReady = true;

        this.LoadRuleHistoryItems();
    }
    LoadRuleHistoryItems(){
        let filters:ApiQueryFilters = new ApiQueryFilters();
        filters.Filter1Name = "RuleCode";
        filters.Filter1Operator = "Equals",
        filters.Filter1Value = this.RulePM.RuleCode;
        filters.GetAll = true;
        //filters.addAdditionalFilter("RuleCode",this.RulePM.RuleCode,null,null,"Equal");
        this.ruleUpdateHistoryListService.getByFilters(filters).subscribe((response:ServiceResponse)=>{
            if (!response.HasError) {
                this.RuleUpdateHistoryItems = response.Result;
                this.IsNoDataFound = this.RuleUpdateHistoryItems.length === 0;
            }
            else
            {
                this.IsNoDataFound = true;
            }

        });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
