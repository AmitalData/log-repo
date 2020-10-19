import { Component, OnInit} from '@angular/core';
import { CalculatedChartsOfAccountPM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountPM';
import { CalculatedChartsOfAccountsLinePM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountsLinePM';
import { ChartOfAccountsTypePM } from 'Accounting/EntityPMs/ChartOfAccountsTypePM';
import { UserDefinedReportPM } from 'Accounting/EntityPMs/UserDefinedReportPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AppTool } from 'Infrastructure/Tools';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from 'Infrastructure/Validators/Validator';
import { CalculatedChartsOfAccountItem, CalculatedChartsOfAccountsLineItem } from '../UserDefinedReportGeneralTabComponent';

@Component({
    
    templateUrl: './AddEditCalculatedChartsOfAccountComponent.html',
})


export class AddEditCalculatedChartsOfAccountComponent extends BaseComponent {
    public DataContext: CalculatedChartsOfAccountItem = null;
    public ThisDataContext: AddEditCalculatedChartsOfAccountComponent = this;
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
    RemoveLine(line){
        this.DataContext.CalculatedChartsOfAccountsLineItemList.Remove(line);
        this.DataContext.EntityPM.RemoveCalculatedChartsOfAccountsLine(line);
        this.DataContext.EntityPM.CalculatedChartsOfAccountLines.splice(line.Line - 1, 1);
        for (var i = 0; i < this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.length; i++) {
            var oldItem =this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection[i];
            var updatedItem = this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection[i];
            updatedItem.Line = i + 1;
            this.DataContext.CalculatedChartsOfAccountsLineItemList.Update(oldItem, updatedItem);
        }
    }
    CancelButtonClicked(){
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked(){
        this.ValidationErrorsList= [];
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidateErrorLogsLines();
        this.ValidateLine(errors);
         if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        } else {
            this.ValidationErrorsList = errors;
        }
    }

    ValidateLine(errors: string[]){
        if(this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.length == 0){
            errors.push(TextCodeTranslator.Translate("UserDefinedReport.O.CalculatedChartofAccount")+" "+TextCodeTranslator.Translate("UserDefinedReport.O.DontHaveAnyLinesInThem."))
        }
        else {
            var haveValues = false;
            this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.forEach(s=> 
                !s.IsCancelled ?haveValues = true:null
            );
            if(!haveValues)
            errors.push(TextCodeTranslator.Translate("UserDefinedReport.O.CalculatedChartofAccount")+" "+TextCodeTranslator.Translate("UserDefinedReport.O.DontHaveAnyLinesInThem."))

        }
        var LinesHaveErrors:number[]=[]
        this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.forEach(s=> !s.IsCancelled && !AppTool.IsNullOrEmpty(s.ErrorLog)?LinesHaveErrors.push(s.Line):null);
        if(LinesHaveErrors.length!=0){
            var ErrorMessage = TextCodeTranslator.Translate("UserDefinedReport.O.CantSaveTheCalculatedChartofAccount");
            if(LinesHaveErrors.length>1) 
                ErrorMessage+="s";
            ErrorMessage+=" "+LinesHaveErrors.toString();
            errors.push(ErrorMessage);
        }

    }

    
    ValidateErrorLogsLines(){
        var FIELD_IS_REQUIERD: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var RequiredChartsofAccountFiled= FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("CalculatedChartsOfAccountsLine.F.ChartOfAccountId"));
        var RequiredGLAccountFiled= FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("CalculatedChartsOfAccountsLine.F.GLAccountId"));
        var RequiredLineTypeFiled= FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("CalculatedChartsOfAccountsLine.F.LineTypeCode"));
        this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.forEach(s=> !s.IsCancelled && (AppTool.IsNullOrEmpty(s.LineTypeCode)? s.ErrorLog = RequiredLineTypeFiled:
                                                                                        s.LineTypeCode=='1' && !s.GLAccountId? s.ErrorLog = RequiredGLAccountFiled:                                             
                                                                                        s.LineTypeCode=='2' && !s.ChartOfAccountId? s.ErrorLog = RequiredChartsofAccountFiled:null)
                                                                                   );
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
     }

 
    get ChartOfAccountTypeCode() { return this.DataContext.ChartOfAccountTypeCode; }
    set ChartOfAccountTypeCode(newValue: string) {
        if (this.DataContext.ChartOfAccountTypeCode != newValue) {
            if(this.DataContext.ChartOfAccountTypeCode  && this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.length > 0){
                 this.ValidationErrorsList.push(TextCodeTranslator.Translate("UserDefinedReport.O.CantUpdateTheChartofAccountType"));
            }
            else{
                this.DataContext.ChartOfAccountTypeCode = newValue;
            }
        }
    }
   
    get ChartOfAccountsType() { return this.DataContext.ChartOfAccountsType; }
    set ChartOfAccountsType(newValue: ChartOfAccountsTypePM) {
        if (this.DataContext.ChartOfAccountsType != newValue) {
                this.DataContext.ChartOfAccountsType = newValue;
            }
        }
 
 
}
 
 