import { Component} from '@angular/core';
import { InterestReportPMService } from 'Accounting/Services/StandardPMs/InterestReportPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
 


@Component({
    
    templateUrl: './InvoiceDateForBatchInvoicesComponent.html',
})

export class InvoiceDateForBatchInvoicesComponent extends BaseComponent{
    private CurrentSession = SessionLocator.SelectedSession;
    public ObjectTableName: string = "ARInvoice";
    public DataContext: InvoiceDateForBatchInvoicesComponent = this;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[]=[];
    public EntertheInvoiceDateText = TextCodeTranslator.Translate("InterestReport.O.EntertheInvoiceDate");
    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        //this.InvoiceDate = new Date();
        this.SetUIProperties();
    }
 
    SetUIProperties(){
        if(AppTool.IsNullOrEmpty(this.InvoiceDate)){
            this.UIProperties.SetRequired("InvoiceDate", this.ObjectTableName, true);
        }
        else{
            this.UIProperties.SetRequired("InvoiceDate", this.ObjectTableName, false);
        }
        
    }

    SetDataContext() {
         
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        if(AppTool.IsNullOrEmpty(this.InvoiceDate)){
            var FIELD_IS_REQUIERD: string = null;
            FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            FIELD_IS_REQUIERD=FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("ARInvoice.F.InvoiceDate")); 
            this.ValidationErrorsList.push(FIELD_IS_REQUIERD);
        }
        else if (this.InvoiceDate > DateTool.GetCurrentDateTimeAsUtc()) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.FutureDateNotAllowed"));
        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit(this.invoiceDate+"");
        }
    }
    private invoiceDate:Date; 
    get InvoiceDate(){
        return this.invoiceDate;
    }
    set InvoiceDate(val: Date){
         this.invoiceDate=val;
         this.SetUIProperties();
    }
    
}
 
 