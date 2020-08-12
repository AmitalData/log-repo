import { Component, OnInit} from '@angular/core';
 
import { InterestReportPM } from 'Accounting/EntityPMs/InterestReportPM';
import { InterestReportPMService } from 'Accounting/Services/StandardPMs/InterestReportPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from 'Infrastructure/Tools';
import {  PDFDocumentInvoices } from '../../../Accounting/Services/ExtendedLists/InterestReportExtendedListService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';



@Component({
    
    templateUrl: './BtatchPrintWarningComponent.html',
})

export class BtatchPrintWarningComponent implements OnInit    {
   private CurrentSession = SessionLocator.SelectedSession;
    private InvoicesnovalidcopiestoprintText= TextCodeTranslator.Translate("InterestReport.O.Invoicesnovalidcopiestoprint");
    private ReportsnovalidcopiestoprintText=TextCodeTranslator.Translate("InterestReport.O.Reportsnovalidcopiestoprint");
    private InvoicesText= TextCodeTranslator.Translate("InterestReport.O.Invoices")+" ";
    private ReportsText= TextCodeTranslator.Translate("InterestReport.O.Reports")+" ";
    public SingleLine: boolean = true;
    public HideHeader: boolean = true;
    public ErrorsCount: number;
    public ItemWidth: string = "50%";
    ngOnInit() {
        if (this.SingleLine) {
            this.ItemWidth = "100%";
        }
    }
    SetDataContext(pDFDocumentInvoices: PDFDocumentInvoices) {
   if(!AppTool.IsNullOrEmpty(pDFDocumentInvoices.ARInvoiceNumbersNotPrinted) && pDFDocumentInvoices.ARInvoiceNumbersNotPrinted.length > 0){
    for(let i =0 ; i < pDFDocumentInvoices.ARInvoiceNumbersNotPrinted.length ;i++){
 
        if(i != pDFDocumentInvoices.ARInvoiceNumbersNotPrinted.length -1){
            this.InvoicesText+=pDFDocumentInvoices.ARInvoiceNumbersNotPrinted[i]+", ";
        }     
        else{
            this.InvoicesText+=pDFDocumentInvoices.ARInvoiceNumbersNotPrinted[i]
        }
    }
    this.InvoicesAndReportsNotPrinted.push(this.InvoicesText+" "+this.InvoicesnovalidcopiestoprintText);
    }

   if(!AppTool.IsNullOrEmpty(pDFDocumentInvoices.InterestReportNumbersNotPrinted) && pDFDocumentInvoices.InterestReportNumbersNotPrinted.length > 0){
    for(let i =0 ; i < pDFDocumentInvoices.InterestReportNumbersNotPrinted.length ;i++){
        if(i != pDFDocumentInvoices.InterestReportNumbersNotPrinted.length -1){
            this.ReportsText+=pDFDocumentInvoices.InterestReportNumbersNotPrinted[i]+", ";
        }     
        else{
            this.ReportsText+=pDFDocumentInvoices.InterestReportNumbersNotPrinted[i]
        }
    }
    this.InvoicesAndReportsNotPrinted.push(this.ReportsText+" "+this.ReportsnovalidcopiestoprintText);
   }
 
 
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked(Val:any) {
 
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }
 
    private itemsSource: string[] = [];
    get InvoicesAndReportsNotPrinted() { return this.itemsSource; }
    set InvoicesAndReportsNotPrinted(newValue: string[]) {
        if (this.itemsSource != newValue) {
            this.itemsSource = [];

            if (newValue != null) {
                newValue.forEach((item) => {

                    if (item != null) {
                        if (item.startsWith("!!")) {
                            item = item.substr(1);
                        }

                        if (this.itemsSource.indexOf(item) == -1) {
                            this.itemsSource.push(item);
                        }
                    }
 

                });
            }

            this.ErrorsCount = this.itemsSource.length;
            this.UpdateItemWidth();
        }
    }

    private UpdateItemWidth() {
        if (this.ErrorsCount <= 1) {
            this.ItemWidth = "100%";
        }

        else {
            if (this.SingleLine) {
                this.ItemWidth = "100%";
            }

            else {
                this.ItemWidth = "50%";
            }
        }
    }
}
 
 