import {Component, OnInit, ViewChild, ViewContainerRef, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {QuoteTemplateSectionExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
@Component({
    selector: 'PreviewQuoteTemplateReportComponent',
    
    templateUrl: './PreviewQuoteTemplateReportComponent.html',
})

export class PreviewQuoteTemplateReportComponent implements OnInit, AfterViewInit {

    QuoteTemplateId: string;
    QuoteId: string;
    PdfDivKey: string = Guid.newGuid();
    quoteTemplateSectionExtendedPMService: QuoteTemplateSectionExtendedPMService;
    HeightPdf: number;
    isFromLibrary: boolean = false;
    AreaName: string;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService();
    }

    ngOnInit() {


    }

    ngAfterViewInit() {
        this.GetQuoteTemplatePdfReport();
    }



    GetQuoteTemplatePdfReport() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.quoteTemplateSectionExtendedPMService.GetQuoteTemplatePdfReport(this.QuoteId, this.QuoteTemplateId, SessionLocator.LoggedUserId, this.isFromLibrary).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {

                var buffer = EntityResourceService.base64ToBufferConvertor(pmResponse.Result);
                var blob = new Blob([buffer], { type: 'application/pdf' });
                var objectURL = URL.createObjectURL(blob);
                var doc = document.getElementById(this.PdfDivKey);
                if (doc) {
                    doc.innerHTML = "<iframe id='fred' style='border: 1px solid gray;' frameborder='1' scrolling='auto' height=" + this.HeightPdf+" width='100%' src=" + objectURL + "> </iframe>";
                }

            
            }
            else if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show(pmResponse.ErrorsArray[0]);
            }

        });

    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    SetWindowArgs(args: any) {
        this.QuoteTemplateId = args.QuoteTemplateId;
        this.QuoteId = args.QuoteId;
        this.HeightPdf = (this.CurrentSession.CurrentWindow.Height - 100);
  
        if (args.AreaName == "FromLibrary") this.isFromLibrary = true;

    }
}
