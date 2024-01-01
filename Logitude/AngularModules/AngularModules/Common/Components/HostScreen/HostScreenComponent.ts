import { Component, ElementRef, HostListener, ViewChild } from "@angular/core";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { TaxesWebService } from "Customs/Services/WebServices/TaxesWebService";

@Component({
    selector: 'app-host-screen',
    template: `<iframe #iframe style="width:100%; height: 100%; margin: 5px;" [src]="urlLoignToExport" ></iframe>`,
    styles: []
})
export class HostScreenComponent {
    @ViewChild('iframe') iframe!: ElementRef<HTMLIFrameElement>;
    urlLoignToExport: SafeResourceUrl = '';
    confirmationNumberTokenLogIsOpen: boolean = false;
    logitudeCommandId: string = '';
    windowInstance: LogitudeWindow = null;

    constructor(private domSanitizer: DomSanitizer) { }

    SetWindowArgs(args: { logitudeCommandId: string, windowInstance: LogitudeWindow }) {
        this.logitudeCommandId = args.logitudeCommandId;
        this.windowInstance = args.windowInstance;
    }

    async ngOnInit() {
        await this.initUrlLoignToExport();
    }

    @HostListener('window:message', ['$event'])
    onMessage(message: MessageEvent) {
        if (message.data === 'amitalBackButtonClicked')
            this.closeWindow();

        if (message.data === 'site ready')
            this.sendLogitudeCommand(message.source as Window);
    }

    async initUrlLoignToExport() {
        const link: string = await new TaxesWebService().getTokens(); //'http://iigtest/customssql/?token=yLBzbt3vU4Mbdln8votnlJXCWffS1lqa/IU=&tenant=6&AmitalSSOAngular=1&xxxx=133469323926013458'
        this.urlLoignToExport = this.domSanitizer.bypassSecurityTrustResourceUrl(link);
    }

    sendLogitudeCommand(currentTarget: Window) {
        if (this.confirmationNumberTokenLogIsOpen) return;
        this.confirmationNumberTokenLogIsOpen = true;
        currentTarget.postMessage({ detail: { LogitudeCommandId: this.logitudeCommandId }, isFromIframe: true, }, '*')
    }

    closeWindow() {
        this.windowInstance.Close('');
    }
}