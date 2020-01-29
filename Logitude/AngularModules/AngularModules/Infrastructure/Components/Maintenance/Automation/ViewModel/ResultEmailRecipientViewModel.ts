
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';
import {Component, OnInit, ChangeDetectorRef }  from '@angular/core';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {FieldValueResolver} from '../../../../../Infrastructure/Utilities/FieldValueResolver';
import {AddEditAutomationsComponent} from '../../../../../Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent';

export class ResultEmailRecipientViewModel extends BaseComponent implements OnInit {


    FieldCode: string;
    FieldName: string;
    FullName: string;
    Key: string;
    Tenant: number;
    IsChecked: boolean;
    EntityPM: ObjectFieldPM;
    EntityContactVariable: string[];
    AddEditAutomationsComponent: AddEditAutomationsComponent;
    constructor(entityPM: ObjectFieldPM, addEditAutomationsComponent: AddEditAutomationsComponent) {
        super();
        this.FieldName = entityPM.FieldName;
        this.FieldCode = entityPM.FieldCode;
        this.Tenant = entityPM.Tenant;
        this.FullName = entityPM.FullNameTextCodeDefaultText;
        this.Key = Guid.newGuid();
        this.EntityContactVariable = addEditAutomationsComponent.EntityContactVariable;
        this.AddEditAutomationsComponent = addEditAutomationsComponent;
        if (this.AddEditAutomationsComponent.EntityContactVariable && this.AddEditAutomationsComponent.EntityContactVariable.length > 0 && this.AddEditAutomationsComponent.EntityContactVariable.indexOf(entityPM.FieldCode) != -1) {
            this.IsChecked = true;
        } else this.IsChecked = false;
    }


    ngOnInit() {



    }



    CheckedResultEmailRecipient(item: ResultEmailRecipientViewModel)
    {
        if (this.AddEditAutomationsComponent.EntityContactVariable.indexOf(item.FieldCode) == -1) {
            this.AddEditAutomationsComponent.EntityContactVariable.push(item.FieldCode);
        }
        else {
            this.AddEditAutomationsComponent.EntityContactVariable = this.AddEditAutomationsComponent.EntityContactVariable.filter(d => d != item.FieldCode);
        }

        this.AddEditAutomationsComponent.IsChangeAutomation = true;
    }


}

class Operator {
    Code: string;
    Name: string;

    constructor(name: string, code: string) {
        this.Code = code;
        this.Name = name;

    }


}