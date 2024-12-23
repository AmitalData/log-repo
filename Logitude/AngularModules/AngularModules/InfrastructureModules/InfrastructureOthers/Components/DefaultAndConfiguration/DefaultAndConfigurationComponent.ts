import { Component, ViewChild } from '@angular/core';
import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { FieldByTypeComponent } from './FieldByTypeComponent';
import { DefaultAndConfigurationPMService } from 'Infrastructure/Services/StandardPMs/DefaultAndConfigurationPMService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { LogTexBoxFormComponent, ValueChange } from 'InfrastructureModules/InfrastructureOthers/AmitalAPI/components/LogTexBoxFormComponent';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';
import { DefaultAndConfigurationListService } from 'Infrastructure/Services/StandardLists/DefaultAndConfigurationListService';
import { LogTab, LogTabsComponent } from 'Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { Subject } from 'rxjs';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { fieldsError } from 'InfrastructureModules/InfrastructureOthers/AmitalAPI/WindowsComponent/AmitalAPIAddWindowService';

@Component({
    template: `
        <LogTabs #tabs [TabsSource]="tabsList" [IsFixedTabs]="'true'" [HideCloseButton]='true'></LogTabs>            
        <p *ngIf='errorMaeasge' class='error-message'>{{ errorMaeasge }}</p>
        <log-close-save-buttons (close)='close($event)'></log-close-save-buttons>
        `,
    styleUrls: ['../../AmitalAPI/fields.scss'],
    styles: [`
            LogTabs {
                display: block; 
                height: 300px;
            }
        `],
})
export class DefaultAndConfigurationComponent {
    @ViewChild('tabs') tabs!: LogTabsComponent;
    $setKeyChange: Subject<ValueChange> = new Subject<ValueChange>();
    forms: { form: LogTexBoxFormComponent, value1: FieldByTypeComponent, value2: FieldByTypeComponent } = { form: null, value1: null, value2: null };
    isEdit: boolean = false;
    DataContext = { UIProperties: new UIProperties() };
    errorMaeasge: string = '';
    defaultAndConfigurationPMService: DefaultAndConfigurationPMService = new DefaultAndConfigurationPMService();
    tabsList: any[] = [
        {
            Header: 'Details',//TextCodeTranslator.Translate("DefaultAndConfigurationTab");
            Code: 'Details',
            ComponentPath: './InfrastructureModules/InfrastructureOthers/Components/DefaultAndConfiguration/DefaultAndConfigurationDetailsTabComponent',
            EntityPM: { dataContext: this.DataContext, $setKeyChange: this.$setKeyChange, forms: this.forms }
        },
        {
            Header: 'Value 1',
            Code: 'Value1',
            ComponentPath: './InfrastructureModules/InfrastructureOthers/Components/DefaultAndConfiguration/DefaultAndConfigurationValueTabComponent',
            EntityPM: { dataContext: this.DataContext, $setKeyChange: this.$setKeyChange, forms: this.forms, valueNumber: 1, required: true  }
        },
        {
            Header: 'Value 2',
            Code: 'Value2',
            ComponentPath: './InfrastructureModules/InfrastructureOthers/Components/DefaultAndConfiguration/DefaultAndConfigurationValueTabComponent',
            EntityPM: { dataContext: this.DataContext, $setKeyChange: this.$setKeyChange, forms: this.forms, valueNumber: 2, required: true }
        },
    ];

    ngAfterViewInit() {
        this.tabs.TabsSource.forEach((tab: LogTab) => this.tabs.LoadTab(tab));
    }

    SetWindowArgs(args: any) {
        this.isEdit = true;
        this.initData(args['EntityId']);
    }

    private async initData(entityId: string) {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        this.DataContext = await LogtuideTableDataService.createInstance().getDataFromService(new DefaultAndConfigurationListService().getSingle(entityId));
        SessionLocator.SelectedSession.StopBusyIndicator();

        this.DataContext.UIProperties = new UIProperties();
        this.$setKeyChange.next({ field: '', value: this.DataContext });
    }

    async close(save: boolean) {
        let res;
        if (save) {
            const forms = this.forms;
            this.errorMaeasge = '';
            let errors: fieldsError[] = forms.form.errors.concat(forms.value1.errors).concat(forms.value2.errors);

            if (errors.length > 0) {                
                this.errorMaeasge = errors.map(error => `${error.fieldName}: ${error.error}`).join(', ');
                return;
            }

            const values = {
                ...this.DataContext,
                ...forms.form.values,
                Value1: forms.value1.value,
                Value2: forms.value2.value,
                CreateDate: this.DataContext['CreateDate'] || new Date(),
                Tenant: SessionLocator.Tenant
            };
            SessionLocator.SelectedSession.StartBusyIndicator('');
            res = await this.sendToServer(values);
            SessionLocator.SelectedSession.StopBusyIndicator();
        }

        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    private async sendToServer(values: any): Promise<any> {
        const action = this.isEdit ? this.defaultAndConfigurationPMService.update(values) : this.defaultAndConfigurationPMService.insert(values);
        return await new Promise<any>((resolve, reject) => action.subscribe((res: ServiceResponse) => {
            if (res.HasError)
                MessageWindow.showErrorMessage('an error accord: ' + res.ErrorsArray.join('\n'));
            resolve(res.HasError)
        },
            err => {
                const msg: string = err ? JSON.stringify(err) : 'Error';
                MessageWindow.showErrorMessage(msg);
                reject(false);
            }));
    }
}