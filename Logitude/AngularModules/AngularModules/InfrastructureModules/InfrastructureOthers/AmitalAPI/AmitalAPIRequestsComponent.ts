import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { AmitalAPISchemaWebService, RequestQueryParams } from 'Common/Services/AmitalAPISchemaWebService';
import { AmitalApiRequestsSelectionService } from   'Customs/Services/DataChange/AmitalApiRequestsSelectionService';
import { LogTexBoxFormComponent, TextBoxField } from './components/LogTexBoxFormComponent';
import { GridColumn, LogitudeGridSimpleComponent } from './components/LogitudeGridSimpleComponent';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { Subscription } from 'rxjs';


@Component({
    selector: 'appAmitalApiRequests',
    template: `
        <div class='header'>
            <log-text-box-form #form class='form-data-field' [fields]='fields' [dir]="'ltr'"></log-text-box-form>
            <div class="button-container">
                <button class="Button" (click)="refreshTable(grid)">{{'General.O.Search' | TextCodeTranslationPipe}}</button>
                <button class="Button" [disabled]="!hasSelection" (click)="returnToQueue()">{{'Customs.General.O.ReturnToQueue' | TextCodeTranslationPipe}}</button>
            </div>
        </div>
        <logitude-grid-simple #grid [columns]='columns' [getData]='getData' [directionRTL]='false' [htmlTemplateComponentUrl]='htmlTemplateComponentUrl' ></logitude-grid-simple>        
        `,
    styles: [`      
        :host() { 
            height: 100%; 
            display: flex;
            flex-flow: column;
            align-items: stretch;
        }

        .header {
            flex: 0 1 auto;
        }

        .header log-text-box-form {
            background: none;
            border: none;
        }
        
        .button-container {
            display: flex;
            gap: 10px;
            justify-content: center;
            margin-bottom: 20px;
        }

        .header button {
            width: 100px;
            margin: 0;
        }

        .header button:disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }

        logitude-grid-simple {
            flex: 1 1 auto;
            position: relative;
            padding:5px;
        }
    `],
    providers: [AmitalApiRequestsSelectionService]
})
export class AmitalAPIRequestsComponent implements OnInit, OnDestroy {
    @ViewChild('form') form!: LogTexBoxFormComponent;
    @ViewChild('grid') grid!: LogitudeGridSimpleComponent;
    
    amitalAPISchemaWebService: AmitalAPISchemaWebService = new AmitalAPISchemaWebService();
    htmlTemplateComponentUrl: string = './CustomsModules/CustomsListTemplates/Components/AmitalAPIRequestsTemplate';
    hasSelection: boolean = false;
    private subscription?: Subscription;
    fields: TextBoxField[] = [
        { name: 'fromCreateDate', label: 'Create Date', type: 'date', required: true },
        { name: 'reference', label: 'Ref' },
        { name: 'partner', label: 'Partner' },
        { name: 'taskName', label: 'Req Type', type: 'selectCustom', values: ['Manifest', 'Document', ''] },
        { name: 'isParent', label: 'Is Parent', type: 'selectCustom', values: ['Yes', 'No', 'All'] },
        { name: 'hasErrors', label: 'Has Error', type: 'selectCustom', values: ['Yes', 'No', 'All'] },
        { name: 'clientApi', label: 'Client Api' },
        { name: 'minItems', label: 'Min Items', type: 'number' },
    ];
    columns: GridColumn[] = [
        { Display: '', FieldName: 'Checkbox', Styles: { width: '60px' }, isTemplate: true },
        { Display: 'Id', FieldName: 'Id', Styles: { width: '140px' } },
        { Display: 'Base Com', FieldName: 'IsParent', Styles: { width: '70px' } },
        { Display: 'Req Type', FieldName: 'TaskName', Styles: { width: '70px' } },
        { Display: 'Partner', FieldName: 'PartnerName', Styles: { width: '125px' } },
        { Display: 'Ref1', FieldName: 'Ref1', Styles: { width: '110px' } },
        { Display: 'Ref2', FieldName: 'Ref2', Styles: { width: '110px' } },
        { Display: 'Ref3', FieldName: 'Ref3', Styles: { width: '110px' } },
        { Display: 'Ref4', FieldName: 'Ref4', Styles: { width: '110px' } },
        { Display: 'Total Items', FieldName: 'TotalItems', Styles: { width: '75px' } },
        { Display: 'Total Chunks', FieldName: 'TotalChunks', Styles: { width: '85px' } },
        { Display: 'Chunk Idx', FieldName: 'ChunkIdx', Styles: { width: '70px' } },
        { Display: 'Create Date', FieldName: 'CreateDate', Styles: { width: '140px' }, isTemplate: true },
        { Display: 'Blob', FieldName: 'StorageBlob', Styles: { width: '300px' } },
        { Display: 'Success', FieldName: 'HasError', Styles: { width: '60px' }, isTemplate: true },
        { Display: '', FieldName: 'Buttons', Styles: { width: '60px' }, isTemplate: true },
    ]

    constructor(public selectionService: AmitalApiRequestsSelectionService) {}

    ngOnInit() {
        this.subscription = this.selectionService.selectionChanged$.subscribe(count => {
            this.hasSelection = count > 0;
        });
    }

    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    convertYesNoToBoolean(value: string) {
        return value === 'Yes' ? true : value === 'No' ? false : null;
    }

    getData = (skip: number, take: number, page: number): Promise<any[]> => {
        const params: RequestQueryParams & any = {
            ...this.form?.values,
            page: page,
            pageSize: take,
            fromCreateDate: new Date(this.form?.values.fromCreateDate).toISOString().split('T')[0] as any,
            hasErrors: this.convertYesNoToBoolean(this.form?.values.hasErrors) as any,
            isParent: this.convertYesNoToBoolean(this.form?.values.isParent) as any,
        };
        delete (<any>params).UIProperties;
        Object.keys(params).filter(key => params[key] === '' || params[key] === null).forEach(key => delete params[key]);

        return this.amitalAPISchemaWebService.getRequestQuery(params);
    }

    refreshTable(logitudeGridSimpleComponent: LogitudeGridSimpleComponent) {
        if (!this.form.valid)
            return;

        this.selectionService.clearAll();
        logitudeGridSimpleComponent.refreshTable();
    }

    async returnToQueue() {
        const selectedIds = this.selectionService.getSelectedIds();
        
        if (selectedIds.length === 0) {
            return;
        }

        SessionLocator.SelectedSession.StartBusyIndicator('');

        try {
            const result = await this.amitalAPISchemaWebService.requeue(selectedIds);
            
            if (result.success) {
                const msgWin: MessageWindow = new MessageWindow();
                msgWin.ShowSuccessIcon = true;
                msgWin.Show(TextCodeTranslator.Translate('Customs.General.O.Success'));
                
                this.selectionService.clearAll();
                this.grid.refreshTable();
            } else {
                throw new Error(result.message || 'Failed to requeue');
            }
        } catch (error) {
            const msgWin: MessageWindow = new MessageWindow();
            msgWin.ShowErrorIcon = true;
            msgWin.Show(TextCodeTranslator.Translate('Customs.General.O.Fail'));
        }

        SessionLocator.SelectedSession.StopBusyIndicator();
    }
}