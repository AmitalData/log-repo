import { Component } from '@angular/core';
import { AutomationHistoryPM } from '../../../../Common/EntityPMs/AutomationHistoryPM';
import { EntityChangePM } from '../../../../Common/EntityPMs/EntityChangePM';
import { AutomationExtendedPMService } from '../../../../Common/Services/ExtendedPMs/AutomationExtendedPMService';
import { AutomationHistoryExtendedPMService } from '../../../../Common/Services/ExtendedPMs/AutomationHistoryExtendedPMService';
import { ApiQueryFilters } from '../../../DataContracts/ApiQueryFilters';
import { AutomationCondition } from '../../../DataContracts/AutomationCondition';
import { ServiceResponse } from '../../../DataContracts/ServiceResponse';
import { ObjectFieldPM } from '../../../EntityPMs/ObjectFieldPM';
import { EntityListService } from '../../../Services/EntityListService';
import { EntityResourceService } from '../../../Services/EntityResourceService';
import { AppTool } from '../../../Tools';
import { SessionLocator } from '../../../Utilities/SessionLocator';
import { AutomationConditionViewModel } from './ViewModel/AutomationConditionViewModel';
declare var window: any;


@Component({
    templateUrl: './AutomationConditionsDetailsComponent.html',
    providers: [EntityListService, AutomationHistoryExtendedPMService]

})
export class AutomationConditionsDetailsComponent {
    public EntityPM: any;
    public DataContext: any;
    private CurrentSession = SessionLocator.SelectedSession;
    AutomationCondationList: AutomationConditionViewModel[] = [];
    AutomationCondationAndList: AutomationConditionViewModel[] = [];
    AutomationCondationOrList: AutomationConditionViewModel[] = []; 
    AutomationHistoryLists: AutomationHistoryPM[];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    AllowedinAutomationConditionsFieldLists: ObjectFieldPM[] = [];
    DisplayValue: string;
    public SelectedItem: any; 
    ObjectTableId: string;
    ObjectTableName: string;
    CurrentEntityPM: any;
    DataViewModel: any; 
    IsAtuomationResourceReady: boolean = false; 
    automationConditionPMList: any;
    IsMasterShipment: boolean = false; 
    public automationExtendedPMService: AutomationExtendedPMService = new AutomationExtendedPMService();
    constructor(public entityListService: EntityListService, public _automationHistoryExtendedPMService: AutomationHistoryExtendedPMService) {

    }


    SetWindowArgs(windowArgs) {
        this.DataViewModel = windowArgs.DataViewModel;
        this.ObjectTableName = this.DataViewModel.ObjectTableName;
        this.automationConditionPMList = windowArgs.CurrentEntityPM.ConditionsList;
        this.CurrentEntityPM = windowArgs.CurrentEntityPM;
        this.Initialize(); 
    }


    Initialize() { 
        var table = window.ObjectTables.filter(d => d.Name == this.ObjectTableName)[0];
        this.ObjectTableId = table.Id;
        this.LoadEntityAuomationAllowedinAutomationConditionsObjectFields(table.Name);
    }

    public EntityObjectAutomationFieldLists: any[];
    LoadEntityAuomationAllowedinAutomationConditionsObjectFields(tableName: string) {
        var entityObjectTable: string[] = [];
        var tableId: string = this.ObjectTableId;
        if (this.ObjectTableName == "Master") {
            var table = window.ObjectTables.filter(d => d.Name == "Shipment")[0];
            if (table) tableId = table.Id;
        }

        window.ObjectFields.filter(f => f.DisplayInAutomationAsEnitity == true && f.ObjectTableId == tableId && (!f.RecordType || (f.RecordType && f.RecordType.split(',').filter(d => d == tableName)[0]))).forEach((objectField) => {
            if (objectField.LookUpTableId) {
                if (entityObjectTable.indexOf(objectField.ObjectTable_LookUpTableName) == -1) {
                    entityObjectTable.push(objectField.ObjectTable_LookUpTableName);
                }
            }
        });

        if (entityObjectTable.length > 0) this.LoadAdditionalEntityResource(entityObjectTable);
        else this.Start();

    }

    Start() { 
        this.LoadConditionsFieldLists();
        this.LoadAutomationConditionsList();
        this.IsAtuomationResourceReady = true;
    }

    private LoadAutomationList() {
        let automationId = this.CurrentEntityPM.AutomationId;
        this.automationExtendedPMService.GetIsMasterAutomation(automationId).subscribe((response: any) => {
            let serviceResponse: ServiceResponse = response;
            if (!serviceResponse.HasError) {
                if (serviceResponse.Result == true) {
                    this.IsMasterShipment = true;
                }
                this.LoadAutomationConditionsList();
                }
            else {
                this.LoadAutomationConditionsList();
            }
        });
    }

    LoadConditionsFieldLists() {

        this.AllowedinAutomationConditionsFieldLists = window.ObjectFields.filter(f => f.ObjectTableId == this.ObjectTableId && (f.AllowedinAutomationConditions == true || f.IsCustom));

        //Additional Automation Entity 
        window.ObjectFields.filter(f => f.DisplayInAutomationAsEnitity == true && f.ObjectTableId == this.ObjectTableId && (!f.RecordType || (f.RecordType && f.RecordType.split(',').filter(d => d == this.ObjectTableName)[0]))).forEach((otherEntityObjectField) => {
            window.ObjectFields.filter(f => f.AllowedinAutomationConditions || f.IsCustom == true && f.ObjectTableId == otherEntityObjectField.LookUpTableId).forEach((item) => {
                this.AllowedinAutomationConditionsFieldLists.push(item);
            });

        })
    }

    LoadAutomationConditionsList() {
        if (this.automationConditionPMList != null) {
            this.automationConditionPMList.forEach((item) => { 
                if (item.ConditionType == "And") { 
                    this.AutomationCondationAndList.push(new AutomationConditionViewModel(item, this));
                }
                else if (item.ConditionType == "Or") { 
                    this.AutomationCondationOrList.push(new AutomationConditionViewModel(item, this));
                }
            });
        } 
    }

    NumberOfLeadedAdditionalEntityResource: number = 0;
    LoadAdditionalEntityResource(additionalentityObjectTableNames: string[]) {
        this.NumberOfLeadedAdditionalEntityResource = 0;
        var additionalentityObjectTableCount: number = additionalentityObjectTableNames.length;

        additionalentityObjectTableNames.forEach((objectTableName) => {
            this._entityResourceService.getEntityResourceByTableName(objectTableName).subscribe((response: any) => { this.NumberOfLeadedAdditionalEntityResource += 1; this.CompleteLoadAdditionalEntityResource(additionalentityObjectTableCount) });

        });
    }

    CompleteLoadAdditionalEntityResource(additionalentityObjectTableCount: number) {

        if (additionalentityObjectTableCount == this.NumberOfLeadedAdditionalEntityResource) this.Start();
    }


    SetDataContext(DataContext: EntityChangePM) {
        this.DataContext = DataContext;
    }

    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("Cancel");
    }
     
}
