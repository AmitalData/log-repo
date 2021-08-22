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
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ContainerizationPMService } from '../../../../Customs/Services/StandardPMs/ContainerizationPMService';
import { Response } from 'selenium-webdriver/http';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ContainerizationMessagesService } from '../../../../Customs/Services/WebServices/ContainerizationMessagesService';
import { GenericRequestParams } from '../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { DeclarationEventManager } from '../../../../Customs/Utilities/DeclarationEventManager';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { CustomMessageProgressComponent } from '../../../CustomsControls/Components/CustomMessageProgressComponent';


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
    declarationPM: DeclarationPM;
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
    containerizationPMService: ContainerizationPMService = new ContainerizationPMService();
    containerizationMessagesService: ContainerizationMessagesService = new ContainerizationMessagesService();
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

    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService, public containerizationExtendedListService: ContainerizationExtendedListService) {
        super();
        this.entityPM = new ContainerizationPM();
        this.entityPM.Tenant = SessionLocator.Tenant;
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
        filters.addAdditionalFilter("IsContainerization", true, null, null, "Equal", true, false, false, "string");
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
            Styles: { width: '25px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsContainerizationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsContainerizationListTemplate',
        });


        this.columns.push({
            FieldName: 'CreateDateTime',
            DataTypeCode: 'String',
            Display: "תאריך פתיחת הצהרה",
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
            Display: "מס' תיק יצוא",
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
            Display: "תיק מכס",
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
            Display: "מזהה מטען 1",
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ManifestNumber'

        });
        this.columns.push({
            FieldName: 'SecondCargoID',
            DataTypeCode: 'String',
            Display: "מזהה מטען 2",
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'SecondCargoID'

        });
        this.columns.push({
            FieldName: 'ThirdCargoID',
            DataTypeCode: 'String',
            Display: "מזהה מטען 3",
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
            SortByName: 'DeclarationStatusTypeName',
            HtmlListComponentName: 'CustomsContainerizationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsContainerizationListTemplate',

        });

        this.columns.push({

            FieldName: 'IsSubmitDeclaration',
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
        this.containerizationExtendedListService.SelectedDeclarations = true;
        this.containerizationExtendedListService.ConnectedDeclarations = this.containerizationExtendedListService.AllDeclarations + this.entityPM.ConnectedDeclarations;
        this.LoadConnectedItems();

    }

    filterAgrs: ApiQueryFilters;
    LoadConnectedItems() {
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }

    OnNoneBtnClicked() {
        this.IsSelected = false;
        this.containerizationExtendedListService.connectedSelectAll = false;
        this.entityPM.ConnectedDeclarations = "";
        this.containerizationExtendedListService.ConnectedDeclarations = "";
        this.containerizationExtendedListService.SelectedDeclarations = false;
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
        if (this.entityPM.Id != null) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.entityPM.ConnectedDeclarations = this.containerizationExtendedListService.ConnectedDeclarations;
            this.entityPM.OperationMode = "2";
            this.entityPM.IsChange = true;
            SessionLocator.SelectedSession.CurrentEditComponent.EntityPM = this.entityPM;
            DeclarationEventManager.AddDeclarationToContainerization.emit(null);
            this.CurrentSession.CurrentWindow.Close("0");
        } else {
            var windowArgs: any = {};
            if (this.declarationPM != null && this.declarationPM.ProcedureCurrentName != null && this.declarationPM.ProcedureCurrentName.includes("טעינה ישירה")) {
                windowArgs.IsDirectCharging = true;
            }
            if (this.containerizationExtendedListService.IsDirectCharging != "") {
                windowArgs.IsDirectCharging = true;
            }
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Height = 200;
        logitudeWindow.Width = 250;
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Title = "הצהרת סוכן";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe((event: any) => {
                if (event != null) {
                    SessionLocator.SelectedSession.StartBusyIndicatorLoading();
                    if (event == "true") {
                        this.entityPM.AgentDeclaration = true;
                    } else {
                        this.entityPM.AgentDeclaration = false;
                    }
                    this.entityPM.ConnectedDeclarations = this.containerizationExtendedListService.ConnectedDeclarations;
                    this.containerizationPMService.insert(this.entityPM).subscribe((response: ServiceResponse) => {
                        this.CurrentSession.CurrentWindow.Close("0");
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                            .then(cmpRef => {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({
                                    EntityId: response.Result.Id,
                                    ObjectTableName: "Customs.Containerization"
                                });
                                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                });
                            });
                        if (!response.HasError) {
                            var params = this.getParams(response, event);
                            CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession,params.PBId, "שליחת המכלה", false).then((res) => { });
                            this.containerizationMessagesService.SendContainerization(params)
                                .subscribe(res1 => {
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                });
                        }
                    });
                }
            });
        });
        logitudeWindow.Show('./CustomsModules/CustomsContainerization/Components/Other/AgentStatementContainerization');
    }
}

getParams(response: ServiceResponse, event: any) {
    var params: GenericRequestParams = new GenericRequestParams();
    params.Tenant = SessionLocator.Tenant;
    params.RequestVIA = event.RequestVIA;
    params.ForcePersonalSign = event.ForcePersonalSign;
    params.LoggingEnabled = true;
    params.LoggingEntityId = response.Result.Id;
    params.LoggingUserId = SessionLocator.LoggedUserId;
    params.RequestName = "המכלה";
    params.ResponseName = "המכלה תשובה"
    return params
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
    if (windowArgs.EntityPM != null) {
        if (windowArgs.EntityIsDeclarationPM == "true") {
            this.declarationPM = windowArgs.EntityPM;
            this.ExportFile = this.declarationPM.ExportFile;
            this.containerizationExtendedListService.ConnectedDeclarations = this.declarationPM.Id + ",";
            this.containerizationExtendedListService.SelectedDeclarations = true;
        } else {
            this.entityPM = windowArgs.EntityPM;
        }
    }
}





}
