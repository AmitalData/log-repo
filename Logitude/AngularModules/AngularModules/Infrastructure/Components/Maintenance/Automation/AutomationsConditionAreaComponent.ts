import {Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren}  from '@angular/core';
import {AutomationPM} from '../../../../Common/EntityPMs/AutomationPMExtended';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';


import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AutomationCondition} from '../../../../Infrastructure/DataContracts/AutomationCondition';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {FieldValueResolver} from '../../../../Infrastructure/Utilities/FieldValueResolver';

@Component({
    
    selector: 'AutomationsConditionAreaComponent',
    templateUrl: './AutomationsConditionAreaComponent.html',
    inputs: ['AutomationCondationLists', 'Title', 'TiggerComponent', 'CondationListType', 'IsDisabled', 'ListHeight', 'NoEntity','CanSetValueFromField','AddFirstLine'],

})
export class AutomationsConditionAreaComponent extends BaseComponent implements OnInit {

    AutomationCondationLists: any[] = [];
    SelectedAutomationCondationList: any;
    Title: string;
    CondationListType: string;
    IsDisabled: boolean = false;
    AddFirstLine: boolean = false;
    ListHeight: string = "120px";
    TiggerComponent: any;
    NoEntity: boolean = false;
    CanSetValueFromField: boolean = true;
    
    constructor() {
        super();
      
    }
    item: any;
    ngOnInit() { 
        if (!this.ListHeight) {
            this.ListHeight = "120px";
        }
        if(this.AddFirstLine){
            this.AddAutomationConditionMethod();
        }

    }


    AddAutomationConditionMethod() {

        if (this.CondationListType == "Set") {
            this.TiggerComponent.AddAutomationSetValueButtonClick();
        } else this.TiggerComponent.AddAutomationConditionMethod(this.CondationListType);

    }
}

