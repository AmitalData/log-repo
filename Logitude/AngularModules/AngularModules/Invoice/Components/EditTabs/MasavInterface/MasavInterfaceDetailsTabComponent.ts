import {Component,EventEmitter, Output}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from 'Infrastructure/Services/EntityListService';

@Component({
    
    templateUrl: './MasavInterfaceDetailsTabComponent.html',
})

export class MasavInterfaceDetailsTabComponent extends BaseComponent {

    public columns: any[] = null;
    public MasavInterfaceColumnsReady: EventEmitter<any> = new EventEmitter();
    private entityListService: EntityListService = new EntityListService();
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: 'TransmitStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Accounting.O.Included"),
            Styles: { width: '70px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true

        });

        this.columns.push({
            FieldName: 'Line',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.Line"),
            Styles: { width: '50px'},
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });


        this.columns.push({
            FieldName: 'LineTypeCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.LineTypeCode"),
            Styles: { width: '50px'},
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });


        this.columns.push({
            FieldName: 'VatNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.VatNumber"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

        this.columns.push({
            FieldName: 'Reference',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.Reference"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true

        });

        this.columns.push({
            FieldName: 'ReferecneGroup',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.ReferecneGroup"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

        this.columns.push({
            FieldName: 'ReferenceDate',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.ReferenceDate"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });



        
        this.columns.push({
            FieldName: 'SubTotalInLocalCurrency',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("ARInvoice.F.SubTotalInLocalCurrency"),
            Styles: { width: '160px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        
    
        this.columns.push({
            FieldName: 'TotalInvoiceAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.TotalInvoiceAmount"),
            Styles: { width: '160px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'VatAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.VatAmount"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

        this.columns.push({
            FieldName: SessionLocator.LoggedUserPM.DontShowLocal ? 'StatusEnglishName' : 'StatusLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.StatusEnglishName"),
            Styles: { width: '300px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });

        this.columns.push({
            FieldName: 'JournalNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.JournalNumber"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'ConfirmationNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("TaxReportLine.F.ConfirmationNumber"),
            Styles: { width: '85px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true
        });
        this.columns.push({
            FieldName: 'Buttons;' + this.EntityPM.StatusCode,
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            ServerSideSortable: true,
            IsCustomTemplate: true,
        });

        this.columns.push({
            FieldName: 'IsManuallyChanged',
            DataTypeCode: 'boolean',
            Display: '',
            Styles: { width: '30px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true

        });
        this.columns.push({
            FieldName: 'IsExternalLine',
            DataTypeCode: 'String',

            Styles: { width: '40px' },
            HtmlListComponentName: 'MasavInterfaceListTemplate',
            HtmlListComponentUrl: './Accounting/Components/ListTemplates/MasavInterfaceListTemplate',
            IsCustomTemplate: true,
            ServerSideSortable: true,
        });
        this.MasavInterfaceColumnsReady.emit(this.columns);

    }
    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "Line",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.GetRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    GetRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {

      
        var filters = new ApiQueryFilters;
        //filters.addAdditionalFilter('TaxReportId', this.EntityPM.Id, null, null, 'Equals', false, false, false, 'string');        

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetCount = true;

        if (sortingDir !== "") {
            filters.SortBy = sortingCol;
            filters.SortDirection = sortingDir;
        }
        else {
            filters.SortBy = "Line";
            filters.SortDirection = "Ascending";
        }

        return this.entityListService.getExtendedByFilters("APPayment", filters);

    }
}
