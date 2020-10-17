import { Component, OnInit} from '@angular/core';
import { CalculatedChartsOfAccountPM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountPM';
import { CalculatedChartsOfAccountsLinePM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountsLinePM';
import { UserDefinedReportPM } from 'Accounting/EntityPMs/UserDefinedReportPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AppTool } from 'Infrastructure/Tools';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { Validator } from 'Infrastructure/Validators/Validator';
import { CalculatedChartsOfAccountItem, CalculatedChartsOfAccountsLineItem } from '../UserDefinedReportGeneralTabComponent';

@Component({
    
    templateUrl: './AddEditCalculatedChartsOfAccountComponent.html',
})


export class AddEditCalculatedChartsOfAccountComponent extends BaseComponent {
    public DataContext: CalculatedChartsOfAccountItem = null;
    public EntityPM: CalculatedChartsOfAccountPM;
    private CurrentSession = SessionLocator.SelectedSession;
    public ObjectTableName: string = "CalculatedChartsOfAccount";
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

    }
    OnFocus() {
        if (this.DataContext.CalculatedChartsOfAccountsLineItemList.Length == 0) {
            this.AddLine();
        }
    }
    OnRowEnded($event) {
        if (($event) == this.DataContext.CalculatedChartsOfAccountsLineItemList.Length) {
            this.AddLine();
        }
    }
    RemoveLine(){
        
    }
    CancelButtonClicked(){
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked(){
        this.ValidationErrorsList= [];
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
         if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        } else {
            this.ValidationErrorsList = errors;
        }
    }
    AddLine() {
        var calculatedChartsOfAccountsLinePM: CalculatedChartsOfAccountsLinePM = new CalculatedChartsOfAccountsLinePM(this.EntityPM);
        calculatedChartsOfAccountsLinePM.Tenant = this.EntityPM.Tenant;
        this.EntityPM.AddCalculatedChartsOfAccountsLine(calculatedChartsOfAccountsLinePM);
        var lastRow = this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection[this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.length - 1];
        calculatedChartsOfAccountsLinePM.Line = this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.length > 0 ? (lastRow.Line  + 1) : 1;
        if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
            calculatedChartsOfAccountsLinePM.CalculatedChartsOfAccountsId = this.EntityPM.Id;
        }
        var line = new CalculatedChartsOfAccountsLineItem(calculatedChartsOfAccountsLinePM,true, this.DataContext);
        this.DataContext.CalculatedChartsOfAccountsLineItemList.Insert(line);
    }

 
    SetDataContext(dataContext: CalculatedChartsOfAccountItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.BuildLinesData();
     }

 
    get EnglishName() { return this.EntityPM.EnglishName; }
    set InterestBaseStartDate(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }

    get Line() {
        if (this.EntityPM != null) {
            return this.EntityPM.Line;
        }
        else
            return null;
    }
    set Line(newValue: number) {
        if (this.EntityPM.Line != newValue) {
            this.EntityPM.Line = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get IsCancelled() { return this.EntityPM.IsCancelled; }
    set IsCancelled(newValue: boolean) {
        if (this.EntityPM.IsCancelled != newValue) {
            this.EntityPM.IsCancelled = newValue;
        }
    }

    get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    set UpdatedByUserId(newValue: string) {
        if (this.EntityPM.UpdatedByUserId != newValue) {
            this.EntityPM.UpdatedByUserId = newValue;
        }
    }

    get ChartOfAccountTypeCode() { return this.EntityPM.ChartOfAccountTypeCode; }
    set ChartOfAccountTypeCode(newValue: string) {
        if (this.EntityPM.ChartOfAccountTypeCode != newValue) {
            this.EntityPM.ChartOfAccountTypeCode = newValue;
        }
    }

    get ChartOfAccountTypeName() { return this.isRTL ? this.EntityPM.ChartOfAccountTypeLocalName : this.EntityPM.ChartOfAccountTypeEnglishName;  }

    get UpdatedByUserName() { return  this.isRTL ? this.EntityPM.UpdatedByLocalName : this.EntityPM.UpdatedByEnglishName; }

    get CreatedByUserName() { return this.isRTL ? this.EntityPM.CretedByLocalNameName : this.EntityPM.CreatedByEnglishName;  }
  
    get CreatedByUserId() { return this.EntityPM.CreatedByUserId; }
    set CreatedByUserId(newValue: string) {
        if (this.EntityPM.CreatedByUserId != newValue) {
            this.EntityPM.CreatedByUserId = newValue;
        }
    }

    get UpdatedDateTime() { return this.EntityPM.UpdatedDateTime; }
    set UpdatedDateTime(newValue: Date) {
        if (this.EntityPM.UpdatedDateTime != newValue) {
            this.EntityPM.UpdatedDateTime = newValue;
        }
    }

    get CreateDateTime() { return this.EntityPM.CreateDateTime; }
    set CreateDateTime(newValue: Date) {
        if (this.EntityPM.CreateDateTime != newValue) {
            this.EntityPM.CreateDateTime = newValue;
        }
    }


    public BuildLinesData() {
        this.DataContext.CalculatedChartsOfAccountsLineItemList.Clear();
        var list = [];
        if(this.EntityPM.CalculatedChartsOfAccountLines)
        this.EntityPM.CalculatedChartsOfAccountLines.forEach(item => {
            list.push(new CalculatedChartsOfAccountsLineItem(item, true, this));
        });
        this.DataContext.CalculatedChartsOfAccountsLineItemList.InsertCollection(list);
    }
 
 
}
 
 