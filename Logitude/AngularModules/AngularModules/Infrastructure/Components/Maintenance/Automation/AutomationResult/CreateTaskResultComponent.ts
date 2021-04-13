 import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren } from '@angular/core'; 
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent'; 
import { CodeNameClass } from '../../../../DataContracts/CodeNameClass';
import { AutomationCreateTask } from '../../../../DataContracts/AutomationCreateTask';
import { ObjectFieldPM } from '../../../../EntityPMs/ObjectFieldPM';
import { EntityResourceService } from '../../../../Services/EntityResourceService';
declare var window: any;
@Component({
    selector: 'CreateTaskResult',
    templateUrl: './CreateTaskResultComponent.html',
    inputs: [''],

})
export class CreateTaskResultComponent extends BaseComponent implements OnInit {
     
    public DueDateFieldList: CodeNameClass[];
    ShowSpecificDate = false;
    ShowCurrentDate = false;
    public CurrentDateOperator: CodeNameClass[] = [];

    ContactObjectFieldLists: ObjectFieldPM[] = [];
    ObjectFieldsLists: ObjectFieldPM[] = [];
    ObjectTableId: string = '1-4'; // Shipment
    OwnerObjectFieldSelected: ObjectFieldPM;
    AssigneeObjectFieldSelected: ObjectFieldPM;

    DataContext: any;
    entityResourceService: EntityResourceService = new EntityResourceService();

    constructor() {
        super();
        this.DataContext = this;
        this.InitializeCreateTaskComponent();
    }

    ngOnInit() {

    }
     

    InitializeCreateTaskComponent() {
           
        this.FillDueDateFieldList(); 
        this.FillCurrentDateOperator();
        this.FillObjectField();

    }

    FillObjectField() {
         
        this.ContactObjectFieldLists = []; 
        this.ObjectFieldsLists = [];

        this.ObjectFieldsLists = window.ObjectFields.filter(f => f.ObjectTableId == this.ObjectTableId);
        this.ObjectFieldsLists.forEach((objectField) => {
              
            if (objectField.FieldName == "CreatedByUserId" || objectField.FieldName == "SalesmanUserId" || objectField.FieldName == "UpdatedByUserId" || objectField.FieldName == "AccountManagerUserId") {
                this.ContactObjectFieldLists.push(objectField);
            }
               
        });
         
        var specifiOwnerObjectField: ObjectFieldPM = new ObjectFieldPM();
        specifiOwnerObjectField.FullNameTextCodeDefaultText = "Specific";
        specifiOwnerObjectField.FieldCode = "Specific";
        specifiOwnerObjectField.FieldName = "Specific";
        this.ContactObjectFieldLists.push(specifiOwnerObjectField); 
           
    } 

    FillDueDateFieldList() {

        this.DueDateFieldList = [];
        this.DueDateFieldList.push(new CodeNameClass("MainCarriageETD", "Main Carriage ETD"));
        this.DueDateFieldList.push(new CodeNameClass("MainCarriageATD", "Main Carriage ATD"));
        this.DueDateFieldList.push(new CodeNameClass("MainCarriageETA", "Main Carriage ETA"));
        this.DueDateFieldList.push(new CodeNameClass("MainCarriageATA", "Main Carriage ATA"));

        // EndDateFieldType -> Field
        // Date -> Date
        // CurrentDate -> -,+ CalculateDate CurrentDate-@1

        this.DueDateFieldList.push(new CodeNameClass("DATE", "Date"));
        this.DueDateFieldList.push(new CodeNameClass("CALCULATEDATE", "@Current Date"));
    }
 
    FillCurrentDateOperator() {
        this.CurrentDateOperator.push(new CodeNameClass("+", "+"));
        this.CurrentDateOperator.push(new CodeNameClass("-", "-"));
        
    }



    //Owner
    OwnerObjectFieldComboBoxChanged(item: any) {
        this.OwnerValue = "";
        if (item) {
            this.automationCreateTask.OwnerFieldType = item.FieldName == "Specific" ? "Specific" : "Field";
        }

        this.OwnerValue = item.FieldName;

        this.OwnerObjectFieldSelected = item;
    }

    OwnerValueChange(item: any) {
        if (item) {
            this.OwnerValue = item.Id;
            this.automationCreateTask.OwnerFieldType = "Specific";
        }

        else this.OwnerValue = "";
    }



    //Assignee
    AssigneeObjectFieldComboBoxChanged(item: any) {
        this.AssigneeValue = "";
        if (item) {
            this.automationCreateTask.AssigneeFieldType = item.FieldName == "Specific" ? "Specific" : "Field";
        }

        this.AssigneeValue = item.FieldName;

        this.AssigneeObjectFieldSelected = item;
    }

    AssigneeValueChange(item: any) {
        if (item) {
            this.AssigneeValue = item.Id;
            this.automationCreateTask.AssigneeFieldType = "Specific";
        }

        else this.AssigneeValue = "";
    }



    private selectedDueDateField: any;
    get SelectedDueDateField() { return this.selectedDueDateField; }
    set SelectedDueDateField(value: any) {
        if (this.selectedDueDateField != value) {
            this.selectedDueDateField = value;
        }
    }

     
    private selectedCurrentDateOperator: CodeNameClass;
    get SelectedCurrentDateOperator() { return this.selectedCurrentDateOperator; }
    set SelectedCurrentDateOperator(value: CodeNameClass) {
        if (this.selectedCurrentDateOperator != value) {
            this.selectedCurrentDateOperator = value;
        }
    }


    currentDateFieldValue: string; 
    get CurrentDateFieldValue() { return this.currentDateFieldValue; }
    set CurrentDateFieldValue(newValue: string) {
        if (newValue != this.currentDateFieldValue) {
            this.currentDateFieldValue = newValue; 

        }
    } 


 
    private assigneeValue: string;
    get AssigneeValue() { return this.assigneeValue; }
    set AssigneeValue(newValue: string) {
        if (newValue != this.assigneeValue) {
            this.assigneeValue = newValue;
            if (this.automationCreateTask.AssigneeValue != newValue) {
                this.automationCreateTask.AssigneeValue = newValue; 
            }

        }
    } 

    private taskType: string;
    get TaskType() { return this.taskType; }
    set TaskType(newValue: string) {
        if (newValue != this.taskType) {
            this.taskType = newValue;
            if (this.automationCreateTask.TaskType != newValue) {
                this.automationCreateTask.TaskType = newValue;
            }

        }
    } 


    private ownerValue: string;
    get OwnerValue() { return this.ownerValue; }
    set OwnerValue(newValue: string) {
        if (newValue != this.ownerValue) {
            this.ownerValue = newValue;
            if (this.automationCreateTask.OwnerValue != newValue) {
                this.automationCreateTask.OwnerValue = newValue;
            }

        }
    }

     

    private ownerFieldType: string;
    get OwnerFieldType() { return this.ownerFieldType; }
    set OwnerFieldType(newValue: string) {
        if (newValue != this.ownerFieldType) {
            this.ownerFieldType = newValue;
            if (this.automationCreateTask.OwnerFieldType != newValue) {
                this.automationCreateTask.OwnerFieldType = newValue;
            }

        }
    }

    private assigneeFieldType: string;
    get AssigneeFieldType() { return this.assigneeFieldType; }
    set AssigneeFieldType(newValue: string) {
        if (newValue != this.assigneeFieldType) {
            this.assigneeFieldType = newValue;
            if (this.automationCreateTask.AssigneeFieldType != newValue) {
                this.automationCreateTask.AssigneeFieldType = newValue;
            }

        }
    }

    private endDate: any;
    get EndDate() { return this.endDate; }
    set EndDate(newValue: any) {
        if (newValue != this.endDate) {
            this.endDate = newValue; 
            if (this.automationCreateTask.EndDateValue != newValue) {
                this.automationCreateTask.EndDateValue = newValue;
            } 

        }
    }
     

    private endDateFieldType: string;
    get EndDateFieldType() { return this.endDateFieldType; }
    set EndDateFieldType(newValue: string) {
        if (newValue != this.endDateFieldType) {
            this.endDateFieldType = newValue;
            if (this.automationCreateTask.EndDateTypeValue != newValue) {
                this.automationCreateTask.EndDateTypeValue = newValue;
            }

        }
    }

    NumberOfDays: number;
    SelectedDateField: any;
    SelectedItemChanged(value) {
        // EndDateFieldType -> MainCarriageETD || MainCarriageATD || MainCarriageETA || MainCarriageATA -> FIELD
        // DATE -> Date
        // CALCULATEDATE -> -,+ CalculateDate CurrentDate-@1

        this.EndDate = null; 
        switch (value.Code) {
            case "DATE": {
                this.ShowSpecificDate = true;
                this.ShowCurrentDate = false;
                this.EndDateFieldType = value.Code;
                break;
            }
            case "CALCULATEDATE": {
                this.ShowSpecificDate = false;
                this.ShowCurrentDate = true;
                this.SelectedCurrentDateOperator = this.CurrentDateOperator[0];
                this.NumberOfDays = 0;
                this.EndDateFieldType = value.Code;
                this.ComputedCurrentDate();  
                break;
            }
            default: {
                this.EndDate = value.Code;
                this.ShowSpecificDate = false;
                this.ShowCurrentDate = false;
                this.EndDateFieldType = "FIELD";
                break;
            } 
        }


        this.SelectedDueDateField = value;
    }

    ComputedCurrentDate() { 
        // CurrendDate*-*1
        this.EndDate = "CurrentDate*" + this.SelectedCurrentDateOperator.Code + "*" + this.NumberOfDays;


    }

    SelectedCurrentDateOperatorChange(value) {
        this.SelectedCurrentDateOperator = value;
        this.ComputedCurrentDate();
    }

    NumberOfDaysChanged(value) {
        this.NumberOfDays = value;
        this.ComputedCurrentDate();
    }

    SelectedOwnerChange(value) { 

        let newOwnerValueValue;
        if (value) newOwnerValueValue = value.Id;
        this.OwnerValue = newOwnerValueValue; 
    }

    SelectedAssigneeChange(value) {  
        let newAssigneeIdValue;
        if (value) newAssigneeIdValue = value.Id;
        this.AssigneeValue = newAssigneeIdValue; 

    } 

      
    public automationCreateTask: AutomationCreateTask;
    Run(automationCreateTask: AutomationCreateTask) {
        this.automationCreateTask = automationCreateTask;

        this.SetSelectedDelfultData();
    }

    SetSelectedDelfultData() {

        if (this.automationCreateTask) {   
            this.TaskType = this.automationCreateTask.TaskType;
            this.EndDate = this.automationCreateTask.EndDateValue;
            this.EndDateFieldType = this.automationCreateTask.EndDateTypeValue;
             
            this.OwnerValue = this.automationCreateTask.OwnerValue;
            this.OwnerFieldType = this.automationCreateTask.OwnerFieldType;
            this.AssigneeFieldType = this.automationCreateTask.AssigneeFieldType;  
            this.AssigneeValue = this.automationCreateTask.AssigneeValue; 

            this.SetSelectedOwner();
            this.SetSelectedAssignee();
             
            if (this.EndDateFieldType == "DATE") this.ShowSpecificDate = true;

            this.SetEndDateFieldType();
            this.SetCalculateDateParts(); 
          
        }

    }
    SetSelectedAssignee() {

        if (this.ContactObjectFieldLists) {

            if (this.automationCreateTask.AssigneeFieldType == "Specific") {
                this.AssigneeObjectFieldSelected = this.ContactObjectFieldLists.filter(d => d.FieldCode == "Specific")[0];

            }

            else {
                this.AssigneeObjectFieldSelected = this.ContactObjectFieldLists.filter(d => d.FieldName == this.AssigneeValue)[0];
            }
        }

    }

    SetSelectedOwner() {

        if (this.ContactObjectFieldLists) {

            if (this.automationCreateTask.OwnerFieldType == "Specific") {
                this.OwnerObjectFieldSelected = this.ContactObjectFieldLists.filter(d => d.FieldCode == "Specific")[0];

            }

            else {
                this.OwnerObjectFieldSelected = this.ContactObjectFieldLists.filter(d => d.FieldName == this.OwnerValue)[0];
            }
        }

    }
    SetEndDateFieldType() {
        if (this.automationCreateTask.EndDateTypeValue == "FIELD") {
            this.SelectedDueDateField = this.DueDateFieldList.filter(d => d.Code == this.automationCreateTask.EndDateValue)[0];

        } else {
            this.SelectedDueDateField = this.DueDateFieldList.filter(d => d.Code == this.automationCreateTask.EndDateTypeValue)[0];
        }
    }

    SetCalculateDateParts() {
        if (this.EndDate && this.EndDateFieldType == "CALCULATEDATE") { 
            let calculateDateParts = this.EndDate.split("*"); 
            this.NumberOfDays = calculateDateParts[2];
            this.SelectedCurrentDateOperator = this.CurrentDateOperator.filter(d => d.Code == calculateDateParts[1])[0];

            this.ShowSpecificDate = false;
            this.ShowCurrentDate = true;
        }
    }
}

