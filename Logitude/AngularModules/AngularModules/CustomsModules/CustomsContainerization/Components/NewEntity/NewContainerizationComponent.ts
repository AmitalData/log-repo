import {Component, AfterViewInit, ChangeDetectorRef, OnInit, Input, Output,EventEmitter}  from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    
    templateUrl: './NewContainerizationComponent.html',
})

export class NewContainerizationComponent extends BaseComponent {
    DataContext = this;
    objectTableNameDec: string = "Customs.Declaration";
    @Output() onQueryChangeEvent = new EventEmitter();
    entityListService: EntityListService;
    SearchFieldsFilter: FilterItem;
    private CurrentSession = SessionLocator.SelectedSession;
    isLoad: boolean = false;
    ExportFileFilter: FilterItem;

    private selectedValue: string = "All";
    public get SelectedValue() { return this.selectedValue; }
    public set SelectedValue(value: string) {
        if (this.selectedValue != value) {
            this.selectedValue = value;
        }
    }


    private exportFile: string;
    get ExportFile() { return this.exportFile; }
    set ExportFile(value: string) {
        if (this.exportFile != value) {
            this.exportFile = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.ExportFileFilter = new FilterItem("ExportFile", value, null, null, "StartsWith", false, false, false, "string", false);
            } else {
                this.ExportFileFilter = null
            }
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }


    constructor(private EntityResourceService: EntityResourceService) {
        super();


        this.EntityResourceService.getEntityResourceByTableName("Customs.Containerization").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.entityListService = new EntityListService();
                this.BuildColumns();
                this.isLoad = true;
            });
        });

    }


    DataSource = {
        pageSize: 30,
        rowCount: null,
        //sortingCol: "CreateDateTime",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
         var filters = new ApiQueryFilters;
        var ExportFilter = new FilterItem("Direction", 'E', null, null, "Equals", false, false, false, "string", false);
        filters.AdditionalFilters.push(ExportFilter);
        var ProcFilter = new FilterItem("ProcedureCurrentName", 'המכלה', null, null, "Contains", false, false, false, "string", false);
        filters.AdditionalFilters.push(ProcFilter);

        if (this.ExportFileFilter) {
            filters.AdditionalFilters.push(this.ExportFileFilter);
        }
        if (this.SearchFieldsFilter) {
            filters.AdditionalFilters.push(this.SearchFieldsFilter);
        }

        filters.PageSize = 30;
        filters.PageIndex = 0; // decremented 1 in the service
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;

        //filters.addAdditionalFilter("AccountingDate", true, null, null, "Between", false, false, false, "datetime");
        //return new Promise((resolve, reject) => {
        //     var service: DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService();
        //    resolve(service.getByFilters(filters));
        //});
        return this.entityListService.getByFilters(this.objectTableNameDec, filters);//this.ledgerTransactionListExtendedService.getByFilters(filters);
    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({

            FieldName: 'IsChecked',
            DataTypeCode: 'String',//'Number',
            Display: '',
            Styles: { width: '30px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsContainerizationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsContainerizationListTemplate',

        });


        this.columns.push({
            FieldName: 'CreateDateTime',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.CreateDateTime'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CreateDateTime',
                        HtmlListComponentName: 'CustomsContainerizationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsContainerizationListTemplate',

        });
        this.columns.push({
            FieldName: 'DeclarationNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationNumber'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'DeclarationNumber'

        });
        this.columns.push({
            FieldName: 'ExportFile',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.ExportFile'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ExportFile'

        });
        this.columns.push({

            FieldName: 'TransportModeForExport',
            DataTypeCode: 'String',//'Number',
            //Display: "הגשה",
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsContainerizationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsContainerizationListTemplate',
            


        });
 
        this.columns.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.CustomFileNo'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CustomFileNo'

        });
        this.columns.push({
            FieldName: 'CargoTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.CargoTypeName'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CargoTypeName'

        });

        this.columns.push({
            FieldName: 'ManifestNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.ManifestNumber'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ManifestNumber'

        });
        this.columns.push({
            FieldName: 'SecondCargoID',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.SecondCargoID'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'SecondCargoID'

        });
        this.columns.push({
            FieldName: 'ThirdCargoID',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.ThirdCargoID'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ThirdCargoID'

        });
        this.columns.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationStatusTypeName'),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'DeclarationStatusTypeName'

        });

        this.columns.push({

            FieldName: 'PaymentDate',
            DataTypeCode: 'String',//'Number',
            Display: "הגשה",
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsContainerizationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsContainerizationListTemplate',



        });

    }

    itemClicked(type:string) {

    }

    itemMouseOver(type: string){

}
    SendButtonClicked() {

    }
    private timerToken: any;
    TextChanged(searchtext: any) {
        if (searchtext != null || searchtext != undefined) {

            this.timerToken = setTimeout(() => {
                this.SearchFieldsFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
            }, 700);

        } else {
            this.SearchFieldsFilter = null;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); // refresh grid
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    SetWindowArgs(windowArgs) {

        //this.EntityResourceService.getEntityResourceByTableName("Customs.Containerization").subscribe((response: any) => {
        //    this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
        //        this.entityListService = new EntityListService();

        //    });
        //});

    }


    


 }
