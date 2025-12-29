import { Component, OnDestroy } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { finalize } from 'rxjs/operators';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SIIRequestWebService } from '../../../../../../Customs/Services/WebServices/SIIRequestWebService';

@Component({
    selector: 'ApprovalReportViewerComponent',
    templateUrl: './ApprovalReportViewerComponent.html',
    styleUrls: ['./ApprovalReportViewerComponent.scss']
})
export class ApprovalReportViewerComponent implements OnDestroy {

    private currentSession = SessionLocator.SelectedSession;

    public title: string = 'Approval Report';
    public isLoading: boolean = false;
    public errorText: string = null;

    public pdfSrc: SafeResourceUrl = null;
    private objectUrl: string = null;

    private remoteUrl: string = null;
    private logWindow: any = null;

    private siiRequestWebService: SIIRequestWebService = new SIIRequestWebService();

    constructor(private sanitizer: DomSanitizer) { }

    SetWindowArgs(args: any) {
        this.remoteUrl = args?.remoteUrl || args?.url || null;
       // this.title = args?.title || TextCodeTranslator.Translate('Customs.SIIRequest.O.ApprovalReport') || 'Approval Report';
        this.logWindow = args?.logWindow || null;


        if (!this.remoteUrl) {
            this.errorText = 'Missing PDF url';
            return;
        }

        this.loadPdf();
    }

    private loadPdf(): void {
        this.isLoading = true;
        this.errorText = null;

        this.currentSession?.StartBusyIndicator?.(this.title);

        this.siiRequestWebService.getApprovalReportBlob(this.remoteUrl)
            .pipe(finalize(() => {
                this.isLoading = false;
                this.currentSession?.StopBusyIndicator?.();
            }))
            .subscribe({
                next: (blob: Blob) => {
                    if (!blob || blob.size === 0) {
                        this.errorText = 'Empty PDF response';
                        return;
                    }

                    this.objectUrl = URL.createObjectURL(blob);
                    this.pdfSrc = this.sanitizer.bypassSecurityTrustResourceUrl(this.objectUrl);
                },
                error: (err) => {
                    this.errorText =
                        err?.error?.Message ||
                        err?.message ||
                        err?.statusText ||
                        'Failed to load PDF';
                }
            });
    }

    public close(): void {
        if (this.logWindow && this.logWindow.Close) {
            this.logWindow.Close();
            return;
        }
        this.currentSession?.CloseCurrentWindow?.();
    }

    public download(): void {
        if (!this.objectUrl) return;
        const a = document.createElement('a');
        a.href = this.objectUrl;
        a.download = 'ApprovalReport.pdf';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }

    public openInNewTab(): void {
        if (!this.objectUrl) return;
        window.open(this.objectUrl, '_blank');
    }

    ngOnDestroy(): void {
        if (this.objectUrl) {
            try { URL.revokeObjectURL(this.objectUrl); } catch { }
            this.objectUrl = null;
        }
    }
}
