

import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TaxReportPM } from '../../EntityPMs/TaxReportPM';
import { TaxReportPMService } from '../../Services/StandardPMs/TaxReportPMService';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool } from '../../../Infrastructure/Tools';
import { UIProperties } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { TaxReportExtendedPMService } from '../../Services/ExtendedPMs/TaxReportExtendedPMService';
import { BatchTaskExecutionListService } from '../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from '../../../Infrastructure/EntityLists/BatchTaskExecutionList';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { MenuTypes } from 'Report/Components/ProcessMenuComponent';


@Component({
    selector: 'NewTaxReportComponent',
    

    templateUrl: './NewTaxReportComponent.html',
})

export class NewTaxReportComponent extends BaseComponent {

    ObjectTableName: string = "TaxReport";
    DataContext: any = this;
    entityPM: TaxReportPM = new TaxReportPM();
    TaxReportPMService: TaxReportPMService = new TaxReportPMService();
    public TenantPM: TenantPM;
    _TaxReportExtendedPMService: TaxReportExtendedPMService = new TaxReportExtendedPMService();
    btePM: any;
    private CurrentSession = SessionLocator.SelectedSession;
    _BatchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    timer: any;
    timerInterval: number = 1000;
    bteList: BatchTaskExecutionList;

    constructor() {
        super();

        ///  this.entityPM.TaxReportMonth = new Date();
     
        this.entityPM.Tenant = SessionLocator.Tenant;
        this.BuildMonthList();

    }

    get TaxReportMonth() { return this.entityPM.TaxReportMonth; }
    set TaxReportMonth(value: Date) {
        if (this.entityPM.TaxReportMonth != value) {
            this.entityPM.TaxReportMonth = value;
            if (this.entityPM.Year != null) {
                this.entityPM.TaxReportMonth.setFullYear(this.entityPM.Year);
            }
        }
    }

    get Year() { return this.entityPM.Year; }
    set Year(value: number) {
        if (this.entityPM.Year != value) {
            this.entityPM.Year = value;
            if (this.entityPM.TaxReportMonth != null)
            this.entityPM.TaxReportMonth.setFullYear(value);
        }
    }



    SelectedItemChanged(item) {
        this.entityPM.TaxReportMonth = item.Code;
    }
    public MonthsList: CodeNameClass[];
    BuildMonthList() {

        this.MonthsList = [];
        this.MonthsList.push(new CodeNameClass("1", "January"));
        this.MonthsList.push(new CodeNameClass("2", "February"));

        this.MonthsList.push(new CodeNameClass("3", "March"));
        this.MonthsList.push(new CodeNameClass("4", "April"));
        this.MonthsList.push(new CodeNameClass("5", "May"));
        this.MonthsList.push(new CodeNameClass("6", "June"));
        this.MonthsList.push(new CodeNameClass("7", "July"));
        this.MonthsList.push(new CodeNameClass("8", "August"));
        this.MonthsList.push(new CodeNameClass("9", "September"));
        this.MonthsList.push(new CodeNameClass("10", "October"));
        this.MonthsList.push(new CodeNameClass("11", "November"));
        this.MonthsList.push(new CodeNameClass("12", "December"));


    }

    private selectedMonth: CodeNameClass;
    get SelectedMonth() { return this.selectedMonth; }
    set SelectedMonth(value: CodeNameClass) {
        if (this.selectedMonth != value) {
            this.selectedMonth = value;
            if (value != null) {
                this.UIProperties.SetRequired("TaxReportMonth", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("TaxReportMonth", this.ObjectTableName, true);
            }

        }
    }
    FIELD_IS_REQUIERD: string = null;
    ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.entityPM.LastUpdateDate = new Date();
        this.entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        var errors: string[] = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        // Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.entityPM.Year)) {
            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("TaxReport.F.Year"));

            errors.push(s);
        }
        if (this.SelectedMonth == null) {
            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("TaxReport.F.TaxReportMonth"));

            errors.push(s);
        }


        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            
            this.entityPM.TaxReportMonth = new Date(this.entityPM.Year, (+this.SelectedMonth.Code -1), 15, 0, 0, 0, 0);
            this.CurrentSession.StartBusyIndicator("");
            this.TaxReportPMService.insert(this.entityPM).subscribe((myResult:any) => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;

                    this._TaxReportExtendedPMService.PostCreateTaxReportInBatch(entity).subscribe((myResult:any) => {
                       if (!myResult.HasError) {
                           
                          this.CurrentSession.StopBusyIndicator();
                          this.CurrentSession.CloseCurrentWindowEmit("ok");
                          var messageWindow = new MessageWindow();
                          messageWindow.ShowSuccessIcon = true;
                          messageWindow.Show(TextCodeTranslator.Translate("General.O.ReportInProcess"));
                          SessionLocator.HomeComponent.IsProcessMenuVisible = true;
                          SessionLocator.HomeComponent.CurrentProcessId = myResult.ReportKey;
                          SessionLocator.HomeComponent.SelectedTab = MenuTypes.BatchTaskExecution.toString();
                          SessionLocator.HomeComponent.isPinned = true;
                      }

                    });
                   
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });


        }



    }


   

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }

}
