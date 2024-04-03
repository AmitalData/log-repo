import { ChangeDetectorRef, Component } from "@angular/core";
import { AmitalAPIClientapiWebService, AmitalApiClientapi } from "Common/Services/AmitalAPIClientapiWebService";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { AmitalApiSchema } from "Common/Services/AmitalAPISchemaWebService";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { MoreParam } from "./amitalApiTypes";
import { TextBoxField } from "./components/LogTexBoxFormComponent";

@Component({
    template: `        
        <log-text-box-form *ngIf='fieldsReady' class='form-data-field' [DataContext]='data' [fields]='fields' [dir]="'ltr'">
            <div class='form-data-field-field'>
                <LogLabel [DataContext]="data" [Text]="'Schema'" [LayoutDirection]="'ltr'"></LogLabel>
                <span>
                    <select #selectedData (change)='onSchemaSelect(selectedData.value)' [value]='SchemaSelected' >
                        <option *ngFor='let schema of schemas' [value]='schema.Id'>{{schema.Id}} - {{schema.Name}}</option>
                    </select>
                </span>
            </div>
        </log-text-box-form>
        <log-close-save-buttons (close)='close($event)'></log-close-save-buttons>        
    `,    
    styleUrls: ['./fields.scss'],
    styles: [`      
        .form-data-field-field {
            margin: 0;
            line-height: unset !important;
        }

        .form-data-field-field span {
            width: 300px;
        }

        .form-data-field-field span select {
            width: 262px;
            height: 22px;
            box-shadow : inset 0 0 3px #AAAAAA;
        }
    `],
})
export class AmitalAPIAddApiWindowComponent {
    fieldsReady: boolean = false;
    logWindow: LogitudeWindow;
    schemas: AmitalApiSchema[] = [];
    SchemaSelected: string = '';
    data: AmitalApiClientapi & any = { UIProperties: new UIProperties() };
    moreParamsList: MoreParam[] = [];
    fields: TextBoxField[] = []
    basicFields: TextBoxField[] = [
        { name: 'PartnerName', label: 'Partner' },
        { name: 'PartnerToken', label: 'Token' },
        { name: 'Active', label: 'Active', type: 'boolean' },
    ];

    constructor(private readonly cdr: ChangeDetectorRef) { }

    SetWindowArgs({ row, logWindow, schemas, moreParams }: { row: AmitalApiClientapi, logWindow: LogitudeWindow, schemas: AmitalApiSchema[], moreParams: MoreParam[] }) {
        if (row) {
            this.data = { ...this.data, ...row };
            this.SchemaSelected = this.schemas.find(x => x.Id === row.SchemaId)?.Id;
        }

        this.moreParamsList = moreParams;
        this.logWindow = logWindow;
        this.schemas = schemas;
        this.updateFields();
        this.fieldsReady = true;
    }

    close(save: boolean) {
        delete this.data.UIProperties;
        this.logWindow.Close(save ? this.data : null);
    }

    onSchemaSelect(schemaId: string) {
        this.data.SchemaId = schemaId;
        this.data.SchemaName = this.schemas.find(x => x.Id === schemaId)?.Name;
        this.updateFields();
    }

    updateFields() {
        this.fieldsReady = false;
        this.cdr.detectChanges();

        this.SchemaSelected = this.data.SchemaId;
        this.fields = this.basicFields.concat(
            this.moreParamsList
                .filter(x => x.schemaId === this.data.SchemaId)
                .map(x => ({ name: x.name, label: x.label })));

        this.fieldsReady = true
        this.cdr.detectChanges();
    }

    static async openEditPopup(row: AmitalApiClientapi, schemas: AmitalApiSchema[], moreParams: MoreParam[], clientapis: AmitalApiClientapi[], isUpdate: boolean): Promise<AmitalApiClientapi | boolean> {
        let updateRow: AmitalApiClientapi = await AmitalAPIAddApiWindowComponent.openWindow(row, schemas, moreParams);
        if (!updateRow) return;

        if (!isUpdate && clientapis.some(clientapi => clientapi.SchemaId + ';' + clientapi.PartnerName === updateRow.SchemaId + ';' + updateRow.PartnerName)) {
            new MessageWindow().Show(TextCodeTranslator.Translate('General.O.ClientAPIAlreadyExists'));
            return;
        }

        updateRow = AmitalAPIAddApiWindowComponent.fixRowData(updateRow, row, moreParams);
        let res: AmitalApiClientapi | boolean = await AmitalAPIAddApiWindowComponent.sendToServer(isUpdate, updateRow);

        return res;
    }

    private static openWindow(row: AmitalApiClientapi, schemas: AmitalApiSchema[], moreParams: MoreParam[]): Promise<AmitalApiClientapi> {
        const logWindow = new LogitudeWindow();
        logWindow.Width = 850;
        logWindow.Height = 340;
        logWindow.Title = "Client API";
        logWindow.WindowArgs = { row, logWindow, schemas: schemas, moreParams };
        logWindow.Show('./Common/Components/Maintenance/AmitalAPI//AmitalAPIAddApiWindowComponent');
        return new Promise<AmitalApiClientapi>(resolve =>
            logWindow.WindowClosed.subscribe(async (row?: AmitalApiClientapi) => resolve(row)));
    }

    private static fixRowData(updateRow: AmitalApiClientapi, row: AmitalApiClientapi, moreParams: MoreParam[]): AmitalApiClientapi {
        const schemaChange: boolean = updateRow.SchemaId !== row?.SchemaId;
        if (schemaChange)
            updateRow.MoreParams = {} as any;

        moreParams
            .filter(param => param.schemaId === updateRow.SchemaId)
            .forEach(param => updateRow.MoreParams[param.name] = updateRow[param.name]);

        moreParams.forEach(param => delete updateRow[param.name]);
        return updateRow;
    }

    private static async sendToServer(isUpdate: boolean, updateRow: AmitalApiClientapi): Promise<AmitalApiClientapi | boolean> {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        let res: AmitalApiClientapi | boolean = null;
        try {
            res = await (isUpdate ? new AmitalAPIClientapiWebService().update(updateRow) : new AmitalAPIClientapiWebService().add(updateRow));
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();

        if (!res) {            
            const win = new MessageWindow();
            win.ShowErrorIcon = true;
            win.Show(isUpdate ? TextCodeTranslator.Translate('General.O.UpdateFailed') : TextCodeTranslator.Translate('General.O.AddFailed'))
        }

        return res;
    }
}