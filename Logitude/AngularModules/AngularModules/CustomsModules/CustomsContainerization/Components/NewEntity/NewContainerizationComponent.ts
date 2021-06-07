import { Component, AfterViewInit, ChangeDetectorRef, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { result } from 'cypress/types/lodash';
import { ContainerizationPM } from '../../../../Customs/EntityPMs/ContainerizationPM';
import { ContainerizationExtendedListService } from '../../../../Customs/Services/ExtendedLists/ContainerizationExtendedListService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';


@Component({

    templateUrl: './NewContainerizationComponent.html',
    providers: [ContainerizationExtendedListService]
})

export class NewContainerizationComponent extends BaseComponent {
    public SelectedRow: any;
    DataContext = this;
    objectTableNameDec: string = "Customs.Declaration";
    objectTableName: string = "Customs.Containerization";
    entityPM: ContainerizationPM;
    @Output() onQueryChangeEvent = new EventEmitter();
    entityListService: EntityListService;
    SearchFieldsFilter: FilterItem;
    private CurrentSession = SessionLocator.SelectedSession;
    isLoad: boolean = false;
    ExportFileFilter: FilterItem;
    TransportFilter_A: string;
    TransportFilter_O: string;
    TransportFilter_I: string;
    IsSelectedNot: boolean;
    IsSelected: boolean;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    connectedListIds: ObservableCollection;


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
    

    ViewInitCompleted($event) {
        this.LoadConnectedDeclarationGrid();
    }

    LoadConnectedDeclarationGrid() {
        this.filterAgrs = new ApiQueryFilters();
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }

    onCheckBoxChecked($event) {
        this.IsSelected = false;
        if (!this.entityPM.ConnectedDeclarations) {
            this.entityPM.ConnectedDeclarations = "";
        }
        if ($event.IsChecked) {
            if (!this.entityPM.ConnectedDeclarations.includes($event.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations + $event.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.ConnectedDeclarations.includes($event.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations.replace($event.rowData.Id + ",", "");
            }
        }
    }

    constructor(public entityArgs: EntityArgs,private EntityResourceService: EntityResourceService, public containerizationExtendedListService: ContainerizationExtendedListService) {
        super();
        this.entityPM = new ContainerizationPM();
        this.connectedListIds = new ObservableCollection([]);
        this.EntityResourceService.getEntityResourceByTableName("Customs.Containerization").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe((response: any) => {
                    this.entityListService = new EntityListService();
                    if (this.CurrentSession == null) {
                        this.TransportFilter_A = "TransportFilter_A_-1_-1";
                        this.TransportFilter_O = "TransportFilter_O_-1_-1";
                        this.TransportFilter_I = "TransportFilter_I_-1_-1";
                    } else {
                        var index_T = this.CurrentSession.GetNewId("ShipmentTransportFilterMenu");
                        this.TransportFilter_A = "TransportFilter_A" + index_T;
                        this.TransportFilter_O = "TransportFilter_O" + index_T;
                        this.TransportFilter_I = "TransportFilter_I" + index_T;
                    }
                    this.BuildColumns();
                    this.isLoad = true;
                });
            });
        });

    }

    itemMouseOver(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I.png");
                    //img_I.style.top = "1px";
                    break;
                }
            }
        }
    }

    itemMouseLeave(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);

            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
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

        if (this.selectedValue != 'All') {
            var ModeFilter = new FilterItem("TransportModeId", this.selectedValue, null, null, "Equals", false, false, false, "string", false);
            filters.AdditionalFilters.push(ModeFilter);
        }

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

        var myout = this.entityListService
            .getExtendedByFilters("Customs.Containerization", filters);
        myout.then(res => {
        });
        return myout;

    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'MyConnectedCheckBox',
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
            Styles: { width: '80px' },
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
            ColumnHeaderTemplateName: 'BlackTransportModeListHeaderTemplate',
        });

        this.columns.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.CustomFileNo'),
            Styles: { width: '80px' },
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
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ManifestNumber'

        });
        this.columns.push({
            FieldName: 'SecondCargoID',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Declaration.F.SecondCargoID'),
            Styles: { width: '100px' },
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
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsContainerizationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsContainerizationListTemplate',



        });

    }


    OnAllBtnClicked() {
        this.IsSelected = true;
        this.containerizationExtendedListService.connectedSelectAll = true;
        this.LoadConnectedItems();

    }

    filterAgrs: ApiQueryFilters;
    LoadConnectedItems() {
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }

    OnNoneBtnClicked() {
        this.IsSelected = false;
        this.containerizationExtendedListService.connectedSelectAll = false;
        this.entityPM.ConnectedDeclarations = "ALL";
        this.LoadConnectedItems();

    }

    itemClicked(itemValue: string) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
        }


        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });

        this.ApplyTransportSelectedStyle();
    }


    ApplyTransportSelectedStyle() {
        var itemValue = this.SelectedValue;
        var img_A = document.getElementById(this.TransportFilter_A);
        var img_O = document.getElementById(this.TransportFilter_O);
        var img_I = document.getElementById(this.TransportFilter_I);
        if (img_A) {
            this.CurrentSession.ChangeSessionHeader({ TransportId: itemValue });
            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
                    break;
                }

                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
                    break;
                }

                case "L": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
                    break;
                }
            }
        }
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
