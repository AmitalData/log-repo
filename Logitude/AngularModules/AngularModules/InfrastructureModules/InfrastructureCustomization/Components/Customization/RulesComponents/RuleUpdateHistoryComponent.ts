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
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './RuleUpdateHistoryComponent.html',
})
export class RuleUpdateHistoryComponent {
    private _ruleUpdateHistoryListService: RuleUpdateHistoryListService = new RuleUpdateHistoryListService();
    private _objectTableRuleFieldPMService: ObjectTableRuleFieldPMService = new ObjectTableRuleFieldPMService();
    private _objectTableRulePMService: ObjectTableRulePMService = new ObjectTableRulePMService();
    public IsResourcesReady: boolean = false;
    private ObjectTableId: string;
    public TableRulesItems: ObjectTableRulePM[] = [];
    public AllTableRules: ObjectTableRulePM[] = [];
    private entityResourceService: EntityResourceService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        //        window.ObjectTableRules = [];

        this.entityResourceService = new EntityResourceService();
    }

    SetWindowArgs(windowArgs: any) {
        this.ObjectTableId = windowArgs.ObjectTableID;

        //this.AllTableRules = window.ObjectTableRules.filter(r => r.ObjectTableId === this.ObjectTableId && r.Internal === false);
        //this.TableRulesItems = this.AllTableRules;

        this.IsResourcesReady = true;

        //this.LoadRules();
    }
}
