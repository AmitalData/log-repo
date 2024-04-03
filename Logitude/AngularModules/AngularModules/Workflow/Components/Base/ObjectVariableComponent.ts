import { Component, OnInit } from '@angular/core';
import { prettyPrintJson } from 'pretty-print-json';
import { BlobDownloader } from 'Workflow/Utilities/BlobDownloader';

@Component({
    templateUrl: './ObjectVariableComponent.html',
})

export class ObjectVariableComponent implements OnInit {

    public Value: string | null = null;
    public PrettyJsonMaxLength: number = 60000;
    public PrettyJsonHtmlContent: string | null = null;
    public IsPrettyJsonHtmlContent: boolean = false;

    SetWindowArgs(args: any) {
        this.Value = args && args.Value ? args.Value : null;
    }

    ngOnInit() {
        if (this.Value && this.Value.length <= this.PrettyJsonMaxLength) {
            this.PrettyJsonHtmlContent = prettyPrintJson.toHtml(JSON.parse(this.Value));
            this.IsPrettyJsonHtmlContent = true;
        }
    }

    downloadValueJson() {
        if (this.Value) {
            let valueObject = JSON.parse(this.Value);
            BlobDownloader.downloadJson(valueObject);
        }
    }

    copyValueToClipboard(): void {
        if (this.Value) {
            navigator.clipboard.writeText(this.Value);
        }
    }
}