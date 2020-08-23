import { Component, OnInit} from '@angular/core';
 
import { InterestReportPM } from 'Accounting/EntityPMs/InterestReportPM';
import { InterestReportPMService } from 'Accounting/Services/StandardPMs/InterestReportPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from 'Infrastructure/Tools';
import {  PDFDocumentInvoices } from '../../../Accounting/Services/ExtendedLists/InterestReportExtendedListService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';



@Component({
    
    templateUrl: './BtatchPrintConfirmComponent.html',
})

export class BtatchPrintConfirmComponent implements OnInit    {
    private CurrentSession = SessionLocator.SelectedSession;
    private DownloadorViewText= TextCodeTranslator.Translate("InterestReport.O.DownloadorView");
    private DownloadText=TextCodeTranslator.Translate("InterestReport.O.Download");
    private ViewText= TextCodeTranslator.Translate("InterestReport.O.View");
    private isRTL:boolean;
    private URL:any;
    private newWindow:any;
    ngOnInit() {
       
    }

    constructor(){
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    SetDataContext(URL: any) {
      this.URL= URL;
 
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
 
    DownloadButtonClicked() {
        var a = document.createElement("a");
        a.href = this.URL;
        a.download = "InterestInvoices.pdf"; 
        a.click();
         window.URL.revokeObjectURL(this.URL);   
        this.CurrentSession.CloseCurrentWindowEmit("Download");

    }

    ViewButtonClicked() {
        this.newWindow = window.open('', '_blank');//OPEN WINDOW FIRST ON SUBMIT THEN POPULATE PDF
        this.newWindow.location.href = this.URL;
        //OPEN WINDOW FIRST ON SUBMIT THEN POPULATE PDF  
        // window.open(this.URL,  '_blank');
         this.CurrentSession.CloseCurrentWindowEmit("View");
         
    }
   
}
 
 