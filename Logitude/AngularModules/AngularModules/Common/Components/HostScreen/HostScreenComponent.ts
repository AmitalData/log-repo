import { Component, ElementRef, HostListener, ViewChild } from "@angular/core";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TaxesWebService } from "Customs/Services/WebServices/TaxesWebService";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { Observable, Subject } from "rxjs";

@Component({
    selector: 'app-host-screen',
    template: `<iframe #iframe style="width:100%; height: 100%; margin: 5px;" [src]="urlLoignToExport" ></iframe>`,
    styles: []
})
export class HostScreenComponent {
    // @ViewChild('iframe') iframe!: ElementRef<HTMLIFrameElement>;
    urlLoignToExport: SafeResourceUrl = '';
    confirmationNumberTokenLogIsOpen: boolean = false;
    logitudeCommandId: string = '';
    windowInstance: LogitudeWindow = null;
    private readonly event: Subject<HomeScreenEvent> = new Subject<HomeScreenEvent>();
    public readonly $event: Observable<HomeScreenEvent> = this.event.asObservable();

    constructor(private domSanitizer: DomSanitizer) {
        new EntityResourceService().getEntityResourceByTableName("CommunicationLog", 0).subscribe((resp: any) => { })
    }

    SetWindowArgs(args: { logitudeCommandId: string, windowInstance: LogitudeWindow }) {
        this.logitudeCommandId = args.logitudeCommandId;
        this.windowInstance = args.windowInstance;
    }

    async ngOnInit() {
        await this.initUrlLoignToExport();
    }

    @HostListener('window:message', ['$event'])
    onMessage(message: MessageEvent) {
        const data: PostMessageData = message.data;

        if (data?.messageID === 'amitalBackButtonClicked')
            this.closeWindow();

        if (data?.messageID === 'site ready')
            this.sendLogitudeCommand(message.source as Window);

        if (data?.messageID === 'OpenNewBrowser') {
            const link: string = data.moreParams.replace('"', '').replace('"', '');
            this.event.next({ event: HomeScreenEventTypes.openNewBrowser, data: link });
            open(link);
        }
    }

    async initUrlLoignToExport() {
        try {
            const link: string = await new TaxesWebService().getlinkLogin();
            this.urlLoignToExport = this.domSanitizer.bypassSecurityTrustResourceUrl(link);
        } catch (error) {
            console.log('******* error throw when try get login link to export', error);
            await this.showErrorMessage();
            this.closeWindow();
        }
    }

    sendLogitudeCommand(currentTarget: Window) {
        if (this.confirmationNumberTokenLogIsOpen) return;
        this.confirmationNumberTokenLogIsOpen = true;
        currentTarget.postMessage({ detail: { LogitudeCommandId: this.logitudeCommandId }, isFromIframe: true, }, '*')
    }

    closeWindow() {
        this.windowInstance.Close('');
    }

    showErrorMessage(): Promise<void> {
        const msgWin: MessageWindow = new MessageWindow();
        msgWin.ShowErrorIcon = true;
        msgWin.Show(TextCodeTranslator.Translate('CommunicationLog.O.Error'));

        return new Promise<void>(res => msgWin.WindowClosed.subscribe(() => res()));
    }
}

export enum HomeScreenEventTypes { openNewBrowser }

export type HomeScreenEvent = {
    event: HomeScreenEventTypes,
    data?: any
}

export type PostMessageData = {
    messageID?: string,
    moreParams?: string
}
