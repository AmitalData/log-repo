import { Component } from "@angular/core";
import { DocumentsFilingExtendedPMService } from "Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { InfraSettings } from "Infrastructure/Utilities/InfraSettings";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    selector: "app-new-documents-filing",
    templateUrl: "./NewDocumentsFilingComponent.html",
    styleUrls: ["./NewDocumentsFilingComponent.scss"]
})
export class NewDocumentsFilingComponent {
    dataContext: any = {UIProperties: new UIProperties(), DocumentTypeId: ''}; 
    dataUrl: string = '';
    base64File: string = '';
    fileName: string = '';
    directionCode: string = '';
    directionCodes: {name: string, id: string}[] = [
        { name: 'In', id: 'I' },
        { name: 'Out', id: 'O' }
    ];

    constructor() { }

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;

        if (input.files && input.files.length > 0) {
            const file = input.files[0];
            this.fileName = file.name;

            const reader = new FileReader();
            reader.onload = (e: any) => {
                var decodedString = '';
                var bytes = new Uint8Array(e.target.result);
                var len = bytes.byteLength;
                for (var i = 0; i < len; i++)
                    decodedString += String.fromCharCode(bytes[i]);

                this.base64File = btoa(decodedString);
                const blob = new Blob([e.target.result], { type: file.type });
                this.dataUrl = URL.createObjectURL(blob);
            }
            reader.readAsArrayBuffer(file);
        }
    }

    close(save: boolean): void {
        if (save) 
            this.sendToServer();

        SessionLocator.SelectedSession.StopBusyIndicator();
    }
    
    async sendToServer(): Promise<void> {  
        SessionLocator.SelectedSession.StartBusyIndicator('');
        await new DocumentsFilingExtendedPMService().PostDocumentAndDocumentFiling(InfraSettings.TenantPM.Id, this.fileName, this.base64File, this.directionCode, this.dataContext.documentTypeId).toPromise();
        SessionLocator.SelectedSession.StopBusyIndicator();
    }
}