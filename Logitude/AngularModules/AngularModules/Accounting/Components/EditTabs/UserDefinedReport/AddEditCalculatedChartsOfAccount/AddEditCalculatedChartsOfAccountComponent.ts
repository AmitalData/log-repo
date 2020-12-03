import { Component, OnInit} from '@angular/core';
import { CalculatedChartsOfAccountPM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountPM';
import { CalculatedChartsOfAccountsLinePM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountsLinePM';
import { ChartOfAccountsTypePM } from 'Accounting/EntityPMs/ChartOfAccountsTypePM';
import { UserDefinedReportPM } from 'Accounting/EntityPMs/UserDefinedReportPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
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
    public ChartOfAccountFilterItems :ApiQueryFilters;
    public GLAccountFilterItems :ApiQueryFilters;
    public IsNewEntity:boolean =false;

    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl"); 
         
    }


    SetFilterItems(){
        this.ChartOfAccountFilterItems = new ApiQueryFilters();
        this.GLAccountFilterItems = new ApiQueryFilters();
        this.ChartOfAccountFilterItems.addAdditionalFilter("TypeCode", this.ChartOfAccountTypeCode, null, null, "Equals", false, false, false, "string");
        this.GLAccountFilterItems.addAdditionalFilter("ChartOfAccountsTypeCode", this.ChartOfAccountTypeCode, null, null, "Equals", false, false, false, "string");
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
        this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.forEach(s=>s.ErrorLog=null);
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
        }
         else {
            this.ValidationErrorsList = errors;
        }
    }
    SetUIProperty() {
        var IsEnabled = this.ValidateChartofAccountType();
        this.UIProperties.SetEnabled("ChartOfAccountTypeCode", this.ObjectTableName, IsEnabled);
        this.UIProperties.SetEnabled("IsCancelled", this.ObjectTableName, !this.DataContext.IsNewEntity);

    }
    ValidateLine(errors: string[]){
        if(!this.DataContext.IsCancelled){
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

    ValidateCreateLine(){
        this.ValidationErrorsList = [];
        if(AppTool.IsNullOrEmpty(this.ChartOfAccountTypeCode)){
            var FIELD_IS_REQUIERD: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var RequiredChartsOfAccountTypeFiled= FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("CalculatedChartsOfAccount.F.ChartOfAccountTypeEnglishName"));
            this.ValidationErrorsList.push(RequiredChartsOfAccountTypeFiled);
        }
    }

    AddLine() {
        if(!this.EntityPM.IsCancelled){
            this.ValidateCreateLine();
            if( this.ValidationErrorsList.length == 0){
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
      }
    }

    ValidateLinesAfterChangedChartsofAccountTypeCode(){
        this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.forEach(s=> s.LineCancelledValidation());
    }
 
    SetDataContext(dataContext: CalculatedChartsOfAccountItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNewEntity = this.DataContext.IsNewEntity;
        this.SetUIProperty();
        this.SetFilterItems();  
     }

     ValidateChartofAccountType():boolean{
        var IsValid:boolean=true;
        for (var i = 0; i < this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection.length; i++) {
            var line = this.DataContext.CalculatedChartsOfAccountsLineItemList.Collection[i];
            if(!line.IsCancelled && (line.GLAccountId ||line.ChartOfAccountId) 
             && (line.ChartOfAccountTypeCode == this.ChartOfAccountTypeCode)
                ){
                IsValid = false;
                break;
            }
        }
        return IsValid;
    }
    get ChartOfAccountTypeCode() { return this.DataContext.ChartOfAccountTypeCode; }
    set ChartOfAccountTypeCode(newValue: string) {
        if (this.DataContext.ChartOfAccountTypeCode != newValue) {
            var IsValid= this.ValidateChartofAccountType();
            if(IsValid){
                this.DataContext.ChartOfAccountTypeCode = newValue;
                this.ValidateLinesAfterChangedChartsofAccountTypeCode();
                this.SetFilterItems();
            }
            else{
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("UserDefinedReport.O.CantUpdateTheChartofAccountType"));
            }
        }
    }

    get IsChartOfAccountTypeCodeEnabled(){
        this.SetUIProperty();
        return this.ValidateChartofAccountType();
          
    }
   
    get ChartOfAccountsType() { return this.DataContext.ChartOfAccountsType; }
    set ChartOfAccountsType(newValue: ChartOfAccountsTypePM) {
        if (this.DataContext.ChartOfAccountsType != newValue) {
                this.DataContext.ChartOfAccountsType = newValue;
            }
        }
 

}
 
