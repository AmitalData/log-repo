 import { Component, OnInit, ChangeDetectorRef, QueryList, ViewChildren } from '@angular/core'; 
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent'; 
import { CodeNameClass } from '../../../../DataContracts/CodeNameClass';
import { AutomationCreateTask } from '../../../../DataContracts/AutomationCreateTask';

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
      
    DataContext: any;

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

        this.DueDateFieldList.push(new CodeNameClass("Date", "Date"));
        this.DueDateFieldList.push(new CodeNameClass("@CurrentDate", "@Current Date"));
    }
 
    FillCurrentDateOperator() {
        this.CurrentDateOperator.push(new CodeNameClass("+", "+"));
        this.CurrentDateOperator.push(new CodeNameClass("-", "-"));
        
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


 
    private assigneeId: string;
    get AssigneeId() { return this.assigneeId; }
    set AssigneeId(newValue: string) {
        if (newValue != this.assigneeId) {
            this.assigneeId = newValue;
            if (this.automationCreateTask.AssigneeId != newValue) {
                this.automationCreateTask.AssigneeId = newValue; 
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


    private ownerId: string;
    get OwnerId() { return this.ownerId; }
    set OwnerId(newValue: string) {
        if (newValue != this.ownerId) {
            this.ownerId = newValue;
            if (this.automationCreateTask.OwnerId != newValue) {
                this.automationCreateTask.OwnerId = newValue;
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
        // EndDateFieldType -> MainCarriageETD || MainCarriageATD || MainCarriageETA || MainCarriageATA -> Field
        // Date -> Date
        // CurrentDate -> -,+ CalculateDate CurrentDate-@1

        this.EndDate = null; 
        switch (value.Code) {
            case "Date": {
                this.ShowSpecificDate = true;
                this.ShowCurrentDate = false;
                this.EndDateFieldType = "Date"; 
                break;
            }
            case "@CurrentDate": {
                this.ShowSpecificDate = false;
                this.ShowCurrentDate = true;
                this.SelectedCurrentDateOperator = this.CurrentDateOperator[0];
                this.NumberOfDays = 0;
                this.EndDateFieldType = "CalculateDate";
                this.ComputedCurrentDate();  
                break;
            }
            default: {
                this.EndDate = value;
                this.ShowSpecificDate = false;
                this.ShowCurrentDate = false;
                this.EndDateFieldType = "Field";
                break;
            } 
        }


        this.SelectedDueDateField = value;
    }

    ComputedCurrentDate() { 
        // CurrendDate@-@1
        this.EndDate = "CurrentDate@" + this.SelectedCurrentDateOperator.Code + "@" + this.NumberOfDays; 

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

        let newOwnerIdValue;
        if (value) newOwnerIdValue = value.Id;
        this.OwnerId = newOwnerIdValue; 
    }

    SelectedAssigneeChange(value) {  
        let newAssigneeIdValue;
        if (value) newAssigneeIdValue = value.Id;
        this.AssigneeId = newAssigneeIdValue; 

    } 

      
    public automationCreateTask: AutomationCreateTask;
    Run(automationCreateTask: AutomationCreateTask) {
        this.automationCreateTask = automationCreateTask;

        this.SetSelectedDelfultData();
    }

    SetSelectedDelfultData() {

        if (this.automationCreateTask) {  
            this.AssigneeId = this.automationCreateTask.AssigneeId;
            this.OwnerId = this.automationCreateTask.OwnerId;
            this.TaskType = this.automationCreateTask.TaskType;
            this.EndDate = this.automationCreateTask.EndDateValue;
            this.EndDateFieldType = this.automationCreateTask.EndDateTypeValue; 
            this.SetDefultDate(this.EndDate); 

        }

    }

    SetDefultDate(Date) {
        if (Date && this.EndDateFieldType == "CalculateDate") {
            this.SelectedDueDateField = "@CurrentDate";
            let DateList = this.EndDate.split("@"); 
                this.SelectedCurrentDateOperator = DateList[1];
                this.NumberOfDays = DateList[2]; 
        }
    }
}

