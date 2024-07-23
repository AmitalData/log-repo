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
                    <div class='form-data-field-field' [ngClass]='{"is-array": type1IsArray}'>
                        <LogLabel [DataContext]="DataContext" [Text]="'Value1'" [LayoutDirection]="dir"></LogLabel>
                        <app-field-by-type #value1 [type]='type1' [dir]='dir' [name]='"Value1"' [DataContext]='DataContext'></app-field-by-type>
                    </div>
                    <div class='form-data-field-field' [ngClass]='{"is-array": type2IsArray}'>
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
            :host ::ng-deep .form-data-field-field { 
                flex: 0 0 410px !important;
            }
            
            :host ::ng-deep .form-data-field-field LogLabel {
                flex: 0 0 100px !important;
            }    

            :host ::ng-deep .is-array app-field-by-type .values-container {
                overflow-y: auto;
                overflow-x: hidden;
                max-width: initial;
                max-height: 90px;
            }

            :host ::ng-deep .is-array app-field-by-type LogTextBox,
            :host ::ng-deep .is-array app-field-by-type LogDatePicker,
            :host ::ng-deep .is-array app-field-by-type .InputDiv,
            :host ::ng-deep .is-array app-field-by-type .DatePickerInputDiv,
            :host ::ng-deep .is-array app-field-by-type select {
                width: 260px !important;
            }

            .form-data-field-field {
                height: auto !important;
                align-items: flex-start;
            }

            .form-data-field-field.is-array {
                height: 110px !important;
            }

            log-close-save-buttons {
                bottom: 0;
                position: absolute;
                left: 0;
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
    type1IsArray: boolean = false;
    type2: string = 'System.String';
    type2IsArray: boolean = false;
    typesList: string[] = ['System.String', 'System.Int32', 'System.Double32', 'System.Boolean', 'System.DateTime', 'System.String[]', 'System.Int32[]', 'System.Double32[]', 'System.Boolean[]'];
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
        this.type1 = this.DataContext['SetValueType1'];
        this.type1IsArray = this.type1.endsWith('[]');
        this.type2 = this.DataContext['SetValueType2'];
        this.type2IsArray = this.type2.endsWith('[]');

        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    onValueChanged(e: ValueChange) {
        if (e.field === 'SetValueType1') {
            this.type1 = e.value;
            this.type1IsArray = e.value.endsWith('[]');
        } else if (e.field === 'SetValueType2') {
            this.type2 = e.value;
            this.type2IsArray = e.value.endsWith('[]');
        }
    }

    async close(save: boolean) {
        if (save) {
            if (!this.Form.valid || !this.value1.valid || !this.value2.valid) {
                this.errorMaeasge = true;
                return;
            }
            
            const values = { 
                ...this.DataContext, 
                ...this.Form.values, 
                Value1: JSON.stringify(this.value1.value), 
                Value2: JSON.stringify(this.value2.value), 
                CreateDate: this.DataContext['CreateDate'] || new Date(),
                Tenant: SessionLocator.Tenant
            };
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