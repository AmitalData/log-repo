import { Component, OnInit} from '@angular/core';
 
import { InterestReportPM } from 'Accounting/EntityPMs/InterestReportPM';
import { InterestReportPMService } from 'Accounting/Services/StandardPMs/InterestReportPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from 'Infrastructure/Tools';
import {  InterestReportExtendedListService, PDFDocumentInvoices } from '../../../Accounting/Services/ExtendedLists/InterestReportExtendedListService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { InterestReportArguments } from 'Accounting/DataContracts/InterestReportArgs';



@Component({
    
    templateUrl: './BtatchPrintConfirmComponent.html',
})

export class BtatchPrintConfirmComponent implements OnInit    {
    public CurrentSession = SessionLocator.SelectedSession;
    public DownloadorViewText= TextCodeTranslator.Translate("InterestReport.O.DownloadorView");
    public DownloadText=TextCodeTranslator.Translate("InterestReport.O.Download");
    public ViewText= TextCodeTranslator.Translate("InterestReport.O.View");
    public isRTL:boolean;
    public URL:any;
    public newWindow:any;
    private interestReportExtendedListService: InterestReportExtendedListService = new InterestReportExtendedListService();
    interestReportArgs: InterestReportArguments;

    ngOnInit() {
       
    }

    constructor(){
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    SetDataContext(URL: any) {
      this.URL= URL;
 
    }

    SetWindowArgs(args: any) {
        this.interestReportArgs = args.interestReportArgs;
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
           this.CurrentSession.CloseCurrentWindowEmit("View");

    }
   
}
 
