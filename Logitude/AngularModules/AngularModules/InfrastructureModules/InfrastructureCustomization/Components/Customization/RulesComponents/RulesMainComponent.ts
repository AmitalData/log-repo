import {Component} from '@angular/core';
import {GeneralDomainService, FieldsTranslations} from '../../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {TextCodePM} from '../../../../../Infrastructure/EntityPMs/TextCodePM';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ObjectTableRulePMService} from '../../../../../Infrastructure/Services/StandardPMs/ObjectTableRulePMService';
import {ObjectTableRuleFieldPMService} from '../../../../../Infrastructure/Services/StandardPMs/ObjectTableRuleFieldPMService';
import {ObjectTableRulePM} from '../../../../../Infrastructure/EntityPMs/ObjectTableRulePM';
import {RuleConditionFieldPM} from '../../../../../Infrastructure/EntityPMs/RuleConditionFieldPM';
import {ObjectTableRuleFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectTableRuleFieldPM';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
declare var window: any;

@Component({
    moduleId: module.id,
    templateUrl: './RulesMainComponent.html',
})

export class RulesMainComponent {

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

        this.LoadRules();
    }

    LoadRules() {

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Loading"));
        this._objectTableRulePMService.getAllByTenant(SessionLocator.Tenant).subscribe(response => {
            if (!response.HasError && response.Result) {

                window.ObjectTableRules = response.Result;

                this.AllTableRules = response.Result.filter(r => r.ObjectTableId === this.ObjectTableId && r.Internal === false);
                this.TableRulesItems = this.AllTableRules;
            }

            this._objectTableRuleFieldPMService.getAllByTenant(SessionLocator.Tenant).subscribe(response2 => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();

                if (!response2.HasError && response2.Result) {
                    window.ObjectTableRuleFields = response2.Result;
                }

                this.IsResourcesReady = true;
               
            });
        });
    }

    SearchTextChanged(searchText) {
        this.TableRulesItems = [];
        if (!AppTool.IsNullOrEmpty(searchText)) {

            this.TableRulesItems = this.AllTableRules.filter(f =>
                   f.RuleCode.toLowerCase().indexOf(searchText.toLowerCase()) > -1
                || f.Name != null && f.Name.toLowerCase().indexOf(searchText.toLowerCase()) > -1
                || f.RuleTypeCode != null && f.RuleTypeCode.toLowerCase().indexOf(searchText.toLowerCase()) > -1
                || f.RuleTypeName != null && f.RuleTypeName.toLowerCase().indexOf(searchText.toLowerCase()) > -1);
           
        } else {
            this.TableRulesItems = this.AllTableRules;
        }
        

    }
    OnEditRule(item) {

       
            //RulesMainComponent
        if (item) {
           var ObjectTable = window.ObjectTables.filter((d: any) => d.Id == this.ObjectTableId)[0];
           this.entityResourceService.getEntityResourceByTableName(ObjectTable.Name, 0).subscribe(response => {
                var windowArgs: any = {};
                windowArgs.ObjectTableId = this.ObjectTableId;
                windowArgs.EntityPM = item;
                windowArgs.IsNewEntity = false;
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Edit Rule";
                logWindow.IsFillScreen_115 = true;
                //logWindow.IsFillScreen_90
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/AddEditRuleComponent');

                logWindow.WindowClosed.subscribe(($event: string) => {
                    if ($event === 'saved') {
                        this.LoadRules();
                    }
                });
            });
        }
        
    }

    OnAddRule() {
        var ObjectTable = window.ObjectTables.filter((d: any) => d.Id == this.ObjectTableId)[0];
        this.entityResourceService.getEntityResourceByTableName(ObjectTable.Name, 0).subscribe(response => {
            var windowArgs: any = {};
            windowArgs.ObjectTableId = this.ObjectTableId;
            windowArgs.EntityPM = new ObjectTableRulePM();
            windowArgs.IsNewEntity = true;
            var logWindow = new LogitudeWindow();
            logWindow.Title = "New Rule";
            logWindow.IsFillScreen_115 = true;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/RulesComponents/AddEditRuleComponent');

            logWindow.WindowClosed.subscribe(($event: string) => {
                if ($event === 'saved') {
                    this.LoadRules();
                }
            });

        });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    
}
