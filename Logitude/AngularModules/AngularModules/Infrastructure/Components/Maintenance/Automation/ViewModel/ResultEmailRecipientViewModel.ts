
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';
import {Component, OnInit, ChangeDetectorRef }  from '@angular/core';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {ObjectFieldPM} from '../../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {FieldValueResolver} from '../../../../../Infrastructure/Utilities/FieldValueResolver';
import {AddEditAutomationsComponent, AutomationEmailRecipientFieldItem} from '../../../../../Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent';

export class ResultEmailRecipientViewModel extends BaseComponent implements OnInit {


    FieldCode: string;
    FieldName: string;
    FullName: string;
    Key: string;
    Tenant: number;
    private isChecked: boolean;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {
        if (this.isChecked != newValue) {
            this.isChecked = newValue;

            if (this.isChecked && !this.ObjectFieldPM.IsRequiered) this.AddEditAutomationsComponent.SelectedNotRequiredFields.push(this.ObjectFieldPM);
            else if (!this.isChecked && !this.ObjectFieldPM.IsRequiered) {
                var index = this.AddEditAutomationsComponent.SelectedNotRequiredFields.indexOf(this.ObjectFieldPM);
                if (index !== -1) {
                    this.AddEditAutomationsComponent.SelectedNotRequiredFields.splice(index, 1);
                }  
            }
        }
    }
    ObjectFieldPM: ObjectFieldPM;
    EntityContactVariable: string[];
    AddEditAutomationsComponent: AddEditAutomationsComponent;
    PartnerObjectFieldCode: string;

    constructor(automationEmailRecipientFieldItem: AutomationEmailRecipientFieldItem, addEditAutomationsComponent: AddEditAutomationsComponent) {
        super();

        this.ObjectFieldPM = automationEmailRecipientFieldItem.ObjectFieldPM;
        this.PartnerObjectFieldCode = automationEmailRecipientFieldItem.PartnerObjectFieldCode;



        this.FieldName = this.ObjectFieldPM.FieldName;
        this.FieldCode = this.ObjectFieldPM.FieldCode;
        this.Tenant = this.ObjectFieldPM.Tenant;
        this.FullName = this.ObjectFieldPM.FullNameTextCodeDefaultText;
        this.Key = Guid.newGuid();
        this.EntityContactVariable = addEditAutomationsComponent.EntityContactVariable;
        this.AddEditAutomationsComponent = addEditAutomationsComponent;
        if (this.AddEditAutomationsComponent.EntityContactVariable && this.AddEditAutomationsComponent.EntityContactVariable.length > 0 && this.AddEditAutomationsComponent.EntityContactVariable.indexOf(this.ObjectFieldPM.FieldCode) != -1) {
            this.IsChecked = true;
        } else this.IsChecked = false;

        if (this.ObjectFieldPM.ObjectTableId != addEditAutomationsComponent.ObjectTableId) {

            this.FullName = (this.ObjectFieldPM.ObjectTableName == "Card" ? "Customer" : this.ObjectFieldPM.ObjectTableName) + " "+ this.FullName;
        }

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
