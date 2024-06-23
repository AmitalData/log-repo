import { Component, ViewChild } from '@angular/core';
import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { FieldByTypeComponent } from './FieldByTypeComponent';
import { DefaultAndConfigurationPMService } from 'Infrastructure/Services/StandardPMs/DefaultAndConfigurationPMService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogTexBoxFormComponent, TextBoxField, ValueChange } from 'InfrastructureModules/InfrastructureOthers/AmitalAPI/components/LogTexBoxFormComponent';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';

@Component({
    template: `
            <log-text-box-form #form [fields]='fields' [DataContext]='DataContext'  [dir]="dir" (valueChange)='onValueChanged($event)'>
                <ng-container end>
                    <div class='form-data-field-field' [ngClass]='{"is-array": type1.endsWith("[]")}'>
                        <LogLabel [DataContext]="DataContext" [Text]="'Value1'" [LayoutDirection]="dir"></LogLabel>
                        <app-field-by-type #value1 [type]='type1' [dir]='dir' [name]='"Value1"' [DataContext]='DataContext'></app-field-by-type>
                    </div>
                    <div class='form-data-field-field' [ngClass]='{"is-array": type2.endsWith("[]")}'>
                        <LogLabel [DataContext]="DataContext" [Text]="'Value2'" [LayoutDirection]="dir"></LogLabel>
                        <app-field-by-type #value2 [type]='type2' [dir]='dir' [name]='"Value2"' [DataContext]='DataContext'></app-field-by-type>
                    </div>
                </ng-container>
            </log-text-box-form>
            
            <p *ngIf='errorMaeasge' class='error-message'>{{'Customs.General.O.RequiredFields' | TextCodeTranslationPipe}}!</p>
            <log-close-save-buttons (close)='close($event)'></log-close-save-buttons>
        `,
    styleUrls: ['../../AmitalAPI/fields.scss'],
    styles: [`
            :host ::ng-deep .form-data-field-field LogLabel {
                flex: 0 0 100px !important;
            }

            .form-data-field-field.is-array LogLabel {
                flex: 0 0 78px !important;
            }

            .form-data-field-field.is-array app-field-by-type {
                max-height: 110px;
                overflow-y: auto;
                overflow-x: hidden;
                max-width: initial;
            }

            :host ::ng-deep .form-data-field-field LogTextBox,
            :host ::ng-deep .form-data-field-field LogDatePicker,
            :host ::ng-deep .form-data-field-field .InputDiv,
            :host ::ng-deep .form-data-field-field .DatePickerInputDiv,
            :host ::ng-deep .form-data-field-field select {
                width: 290px !important;
            }

            .form-data-field-field {
                height: auto !important;
                align-items: flex-start;
            }
        `],
})
export class DefaultAndConfigurationComponent {
    @ViewChild('form') Form!: LogTexBoxFormComponent;
    @ViewChild('value1') value1!: FieldByTypeComponent;
    @ViewChild('value2') value2!: FieldByTypeComponent;
    dir: string = 'ltr';
    DataContext = { UIProperties: new UIProperties() };
    errorMaeasge: boolean = false;
    isEdit: boolean = false;
    type1: string = 'System.String';
    type2: string = 'System.String';
    typesList: string[] = ['System.String', 'System.Int', 'System.Double', 'System.Boolean', 'System.DateTime', 'System.String[]', 'System.Int[]', 'System.Double[]', 'System.Boolean[]'];
    fields: TextBoxField[] = [
        { name: 'Is_Active', label: 'Is Active', type: 'boolean', value: true },
        { name: 'StoreInCache', label: 'Store In Cache', type: 'boolean', value: true },
        { name: 'SetKey', label: 'Set Key', required: true },
        { name: 'AdditionalKey', label: 'Additional Key', required: true },
        { name: 'SortOrder', label: 'Sort Order', type: 'number', required: true },
        { name: 'AllowInheritance', label: 'Allow Inheritance', type: 'boolean' },
        { name: 'SetValueType1', label: 'Set Value Type 1', type: 'selectCustom', values: this.typesList, value: 'System.String', required: true },
        { name: 'SetValueType2', label: 'Set Value Type 2', type: 'selectCustom', values: this.typesList, value: 'System.String', required: true },
    ];
    defaultAndConfigurationPMService: DefaultAndConfigurationPMService = new DefaultAndConfigurationPMService();

    SetWindowArgs(args: any) {
        this.isEdit = true;
        this.initData(args['EntityId']);
    }

    async initData(entityId: string) {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        this.DataContext = await LogtuideTableDataService.createInstance().getDataFromService(new DefaultAndConfigurationPMService().get(entityId));
        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    onValueChanged(e: ValueChange) {
        if (e.field === 'SetValueType1')
            this.type1 = e.value;
        else if (e.field === 'SetValueType2')
            this.type2 = e.value;
    }

    async close(save: boolean) {
        if (save) {
            if (!this.Form.valid || !this.value1.valid || !this.value2.valid) {
                this.errorMaeasge = true;
                return;
            }

            const CreateDate = this.DataContext['CreateDate'] || new Date();
            const values = { ...this.DataContext, ...this.Form.values, Value1: this.value1.value, Value2: this.value2.value, CreateDate };
            SessionLocator.SelectedSession.StartBusyIndicator('');
            await this.sendToServer(values);
            SessionLocator.SelectedSession.StopBusyIndicator();
        }

        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    private async sendToServer(values: any): Promise<void> {
        const action = this.isEdit ? this.defaultAndConfigurationPMService.update : this.defaultAndConfigurationPMService.insert;
        return new Promise<void>((resolve, reject) => action.bind(this.defaultAndConfigurationPMService)(values).subscribe(
            () => resolve(),
            err => {
                this.showErrorMessage(err);
                reject();
            }));
    }

    showErrorMessage(msg: string): void {
        const msgWin: MessageWindow = new MessageWindow();
        msgWin.ShowErrorIcon = true;
        msgWin.Title = TextCodeTranslator.Translate('General.B.Erroroccured');
        msgWin.Show(msg);
    }
}