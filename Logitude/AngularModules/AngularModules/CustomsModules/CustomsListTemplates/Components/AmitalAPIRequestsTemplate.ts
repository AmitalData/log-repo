import { ChangeDetectorRef, Component } from "@angular/core";
import { AmitalAPISchemaWebService } from "Common/Services/AmitalAPISchemaWebService";
import { AmitalApiRequestsSelectionService } from "Customs/Services/DataChange/AmitalApiRequestsSelectionService";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

@Component({
    template: `
    <div class="TextTrimming" *ngIf="fieldName == 'Checkbox'">
        <input type="checkbox" 
               [checked]="selectionService.isSelected(rowData.Id)" 
               (change)="onCheckboxChange(rowData.Id)" />
    </div>

    <div class="TextTrimming" *ngIf="fieldName == 'HasError'">        
        <img [src]="'./Images/Icons/' + (rowData[fieldName] ? 'RedX.png' : 'GreenV.png')" />
    </div>

    <div class="TextTrimming" *ngIf="fieldName == 'CreateDate'">
        <span>{{rowData[fieldName] | DateTimePipe: 'DT'}}</span>
    </div>

    <div class="TextTrimming" *ngIf="fieldName == 'Buttons' && rowData.StorageBlob">
        <img src='./Images/Icons/ArrowDown.png' (click)='download(rowData.Id, rowData.StorageBlob)' />
    </div>
    `,
    styles: [`
        .TextTrimming {
            text-align: center;
            direction: ltr;
        }

        .TextTrimming img {
            height: 20px;
            width: 15px;
            vertical-align: central;
            padding-bottom: 5px;
        }

        .TextTrimming input[type="checkbox"] {
            cursor: pointer;
        }
    `],
}) export class AmitalAPIRequestsTemplate {
    private amitalAPISchemaWebService: AmitalAPISchemaWebService = new AmitalAPISchemaWebService();
    public rowData: any = null;
    public fieldName: string = '';

    constructor(public selectionService: AmitalApiRequestsSelectionService, private cd: ChangeDetectorRef) {}

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
    }

    onCheckboxChange(id: string) {
        this.selectionService.toggleSelection(id);
        this.cd.detectChanges();
    }

    async download(id: string, fileName: string ) {
        SessionLocator.SelectedSession.StartBusyIndicator('');

        try {
            const data = await this.amitalAPISchemaWebService.downloadRequest(id);
            const blob = new Blob([data])
            const downloadURL = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadURL;
            link.download = fileName;
            link.click();
            link.remove();
        } catch (error) {
            const msgWin: MessageWindow = new MessageWindow();
            msgWin.ShowErrorIcon = true;
            msgWin.Show(TextCodeTranslator.Translate('Customs.General.O.Fail'));
        }

        SessionLocator.SelectedSession.StopBusyIndicator();
    }
}
