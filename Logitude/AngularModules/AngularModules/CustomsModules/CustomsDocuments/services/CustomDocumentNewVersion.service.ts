import { Injectable } from "@angular/core";
import { ConfirmWindow } from "Controls/Windows/ConfirmWindow";
import { CustomsDocumentPM } from "Customs/EntityPMs/CustomsDocumentPM";
import { CustomsDocumentPMService } from "Customs/Services/StandardPMs/CustomsDocumentPMService";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

@Injectable()
export class CustomDocumentNewVersionService {
    
    async NewVersion(CustomsDocument: CustomsDocumentPM, skipConfirmMsg: boolean = false): Promise<ServiceResponse> {
        const confirm = skipConfirmMsg || await this.confirmMsg();
        
        if (confirm) {
            this.updateCustomsDocument(CustomsDocument);
            return await this.updateDB(CustomsDocument);
        }

        return null;
    }


    private confirmMsg(): Promise<boolean> {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Height = 200;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        confirmWindow.ShowNoButton = true;
        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.CustomsDocuments.NewVersionWarning"));

        return new Promise<boolean>((resolve, reject) =>
            confirmWindow.WindowClosed.subscribe((event: any) =>
                resolve(confirmWindow.Yes)));
    }


    private updateCustomsDocument(CustomsDocument: CustomsDocumentPM): void {
        CustomsDocument.DocumentVersion = CustomsDocument.DocumentVersion + 1;
        CustomsDocument.DocumentStatusCode = null;
        CustomsDocument.CustomRecievedDate = null;
        CustomsDocument.CustomsDocId = null;
        CustomsDocument.ForceRemoveCustomsDocId = true;
    }


    private updateDB(CustomsDocument: CustomsDocumentPM): Promise<ServiceResponse> {
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));

        return new Promise<ServiceResponse>((resolve, reject) => {
            new CustomsDocumentPMService().update(CustomsDocument).subscribe((docRes: ServiceResponse) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                resolve(docRes);
            });
        })
    }
}