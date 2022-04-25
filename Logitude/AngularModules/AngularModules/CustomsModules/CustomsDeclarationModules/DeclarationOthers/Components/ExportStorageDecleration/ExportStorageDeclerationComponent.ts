import { Component, AfterViewInit, ChangeDetectorRef, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { GenericRequestParams } from 'Customs/DataContract/RequestParams/GenericRequestParams';
import { ContainerizationPM } from 'Customs/EntityPMs/ContainerizationPM';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { ContainerizationExtendedListService } from 'Customs/Services/ExtendedLists/ContainerizationExtendedListService';
import { ContainerizationPMService } from 'Customs/Services/StandardPMs/ContainerizationPMService';
import { ContainerizationMessagesService } from 'Customs/Services/WebServices/ContainerizationMessagesService';
import { DeclarationEventManager } from 'Customs/Utilities/DeclarationEventManager';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { result } from 'cypress/types/lodash';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters, FilterItem } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { AppTool } from 'Infrastructure/Tools';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { ExportStoragePM } from 'Customs/EntityPMs/ExportStoragePM';
import { ExportStoragePMService } from 'Customs/Services/StandardPMs/ExportStoragePMService';
import { ExportStorageExtendedListService } from 'Customs/Services/ExtendedLists/ExportStorageExtendedListService';
import { ConsignmentPackagePM } from 'Customs/EntityPMs/ConsignmentPackagePM';
import { ConsignmentPM } from 'Customs/EntityPMs/ConsignmentPM';
import { DeclarationPMService } from 'Customs/Services/StandardPMs/DeclarationPMService';
@Component({

    templateUrl: './ExportStorageDeclerationComponent.html',
    providers: [ExportStorageExtendedListService]
})

export class ExportStorageDeclerationComponent extends BaseComponent {
    public SelectedRow: any;
    DataContext = this;
    objectTableNameDec: string = "Customs.ExportStorage";
    objectTableName: string = "Customs.Declaration";
    entityPM: ExportStoragePM;
    declarationPM: DeclarationPM;
    @Output() onQueryChangeEvent = new EventEmitter();
    entityListService: EntityListService;
    SearchFieldsFilter: FilterItem;
    private CurrentSession = SessionLocator.SelectedSession;
    isLoad: boolean = false;
    ExportFileFilter: FilterItem;
    IsSelectedNot: boolean;
    IsSelected: boolean;
    @Output() MenuHeaderchangeevent = new EventEmitter();
    connectedListIds: ObservableCollection;
    exportStoragePMService: ExportStoragePMService = new ExportStoragePMService();
    containerizationMessagesService: ContainerizationMessagesService = new ContainerizationMessagesService();
    exportStorage: ServiceResponse;
    consignmet: ConsignmentPM;
    consignmentPackagePM: ConsignmentPackagePM;

    declarationPMService: DeclarationPMService = new DeclarationPMService();

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
                this.ExportFileFilter = new FilterItem("ExportFileNo", value, null, null, "StartsWith", false, false, false, "string", false);
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
        if (!this.entityPM.DeclarationId) {
            this.entityPM.DeclarationId = "";
        }
        if ($event.IsChecked) {
            if (!this.entityPM.DeclarationId.includes($event.rowData.Id)) {
                this.entityPM.DeclarationId = this.entityPM.DeclarationId + $event.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.DeclarationId.includes($event.rowData.Id)) {
                this.entityPM.DeclarationId = this.entityPM.DeclarationId.replace($event.rowData.Id + ",", "");
            }
        }
    }

    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService, public exportStorageExtendedListService: ExportStorageExtendedListService) {
        super();
        this.entityPM = new ExportStoragePM();
        this.entityPM.Tenant = SessionLocator.Tenant;
        this.connectedListIds = new ObservableCollection([]);
        this.EntityResourceService.getEntityResourceByTableName("Customs.ExportStorage").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe((response: any) => {
                    this.entityListService = new EntityListService();

                    this.BuildColumns();
                    this.isLoad = true;
                });
            });
        });

    }


    DataSource = {
        pageSize: 30,
        rowCount: null,
        sortingCol: "OpenDate",
        sortingDir: "Decending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        var filters = new ApiQueryFilters;
        filters.addAdditionalFilter("DeclarationId", "123", null, null, "IsNull", false, false, false, "string");
        filters.addAdditionalFilter("StorageStatus", "Cancel", null, null, "NotEqual", false, false, false, "string");


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
            .getExtendedByFilters("Customs.ExportStorage", filters);
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
            HtmlListComponentName: 'CustomsExportStorageListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsExportStorageListTemplate',
        });


        this.columns.push({
            FieldName: 'OpenDate',
            DataTypeCode: 'DateTime',
            Display: "תאריך פתיחה",
            Styles: { width: '110px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'OpenDate',
            HtmlListComponentName: 'CustomsExportStorageListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsExportStorageListTemplate',

        });
        this.columns.push({
            FieldName: 'ExporterName',
            DataTypeCode: 'String',
            Display: 'יצואן',
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ExporterName'

        });
        this.columns.push({
            FieldName: 'ExportFileNo',
            DataTypeCode: 'String',
            Display: "מס' תיק יצוא",
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ExportFileNo'

        });
        this.columns.push({

            FieldName: 'StorageNo',
            DataTypeCode: 'String',//'Number',
            Display: 'מספר אחסנה',
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'StorageNo'
        });
        this.columns.push({
            FieldName: 'ShipName',
            DataTypeCode: 'String',
            Display: 'אוניה',
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ShipName'

        });
        this.columns.push({
            FieldName: 'CargoTypeName',
            DataTypeCode: 'String',
            Display: 'סוג מטען FCL/LCL',
            Styles: { width: '110px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CargoTypeName'

        });
        this.columns.push({
            FieldName: 'StorageStatus',
            DataTypeCode: 'String',
            Display: 'סטטוס אחסנה',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'StorageStatus'

        });
        this.columns.push({
            FieldName: 'CustomsStatus',
            DataTypeCode: 'String',
            Display: 'קוד סטטוס מטען',
            Styles: { width: '110px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CustomsStatus'

        });
        this.columns.push({
            FieldName: 'ActionCode',
            DataTypeCode: 'String',
            Display: 'היתר לוגיסטי מכסי',
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ActionCode',
        });
    }


    OnAllBtnClicked() {
        this.IsSelected = true;
        this.exportStorageExtendedListService.connectedSelectAll = true;
        this.exportStorageExtendedListService.SelectedExportStorage = true;
        this.exportStorageExtendedListService.ConnectedExportStorage = this.exportStorageExtendedListService.AllExportStorage + this.entityPM.DeclarationId;
        this.LoadConnectedItems();

    }

    filterAgrs: ApiQueryFilters;
    LoadConnectedItems() {
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }

    OnNoneBtnClicked() {
        this.IsSelected = false;
        this.exportStorageExtendedListService.connectedSelectAll = false;
        this.entityPM.DeclarationCustomFileNo = "";
        this.exportStorageExtendedListService.ConnectedExportStorage = "";
        this.exportStorageExtendedListService.SelectedExportStorage = false;
        this.LoadConnectedItems();
    }


    SendButtonClicked() {
        debugger;
        let ArrayExportStorageId = this.exportStorageExtendedListService.ConnectedExportStorage.split(',');

        ArrayExportStorageId.forEach(ExportStorageId => {
            if (!AppTool.IsNullOrEmpty(ExportStorageId)) {
                this.declarationPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                if (!AppTool.IsNullOrEmpty(this.declarationPM)) {

                    this.exportStoragePMService.get(ExportStorageId).toPromise().then(res => {
                        this.exportStorage = res
                        if (!AppTool.IsNullOrEmpty(this.exportStorage)) {

                            var isConsignment = this.declarationPM.Consignments.find(x => x.CargoTypeCode == this.exportStorage.Result.cargoTypeCode
                                && x.ManifestNumber == this.exportStorage.Result.firstCargoID
                                && x.SecondCargoID == this.exportStorage.Result.secondCargoID);

                            if (isConsignment != undefined) {

                                this.exportStorage.Result.DeclarationId = this.declarationPM.Id;
                                this.exportStoragePMService.update(this.exportStorage.Result).subscribe((response: ServiceResponse) => {
                                    isConsignment.ExportStoragesId = ExportStorageId;
                                    this.declarationPMService.update(this.declarationPM).subscribe((response: ServiceResponse) => {
                                    });
                                });


                            }
                            else {
                                var consignment: ConsignmentPM;
                                consignment = new ConsignmentPM(this.declarationPM);
                                consignment.Tenant = SessionLocator.Tenant;
                                consignment.CargoTypeCode = this.exportStorage.Result.cargoTypeCode;
                                consignment.ManifestNumber = this.exportStorage.Result.firstCargoID;
                                consignment.SecondCargoID = this.exportStorage.Result.SecondCargoID;
                                consignment.CargoDescription = this.exportStorage.Result.MarksNumbers;
                                consignment.StorageSiteCode = "10081" //this.exportStorage.Result.StorageSiteCode;
                                consignment.IsDangerousGoods = this.exportStorage.Result.IsDangerousGoods == null ? 0 : this.exportStorage.Result.IsDangerousGoods;
                                consignment.ExportUnloadingPortCode = "ALBUT" //this.exportStorage.Result.ExportUnloadingPortCode;
                                consignment.ExportLoadingPortCode = this.exportStorage.Result.ExportLoadingPortCode;
                                consignment.ConsignmentType = "E";
                                consignment.DeclarationId = this.declarationPM.Id;
                                consignment.ConsignmentNumber = this.declarationPM.Consignments.length > 0 ? this.declarationPM.Consignments[this.declarationPM.Consignments.length - 1].ConsignmentNumber + 1 : 1;
                                consignment.SequenceNumeric = this.declarationPM.Consignments[this.declarationPM.Consignments.length - 1].SequenceNumeric + 1;




                                var consignmentPackage: ConsignmentPackagePM;
                                consignmentPackage = new ConsignmentPackagePM(consignment);
                                consignmentPackage.ConsignmentNumber = consignment.ConsignmentNumber
                                consignmentPackage.SequenceNumeric = 1;
                                consignmentPackage.LineNumber = 1;
                                consignmentPackage.Tenant = SessionLocator.Tenant;
                                consignmentPackage.PackageQuantity = this.exportStorage.Result.packageQuantity;
                                consignmentPackage.PackageMeasureQualifierCode = "2";
                                consignmentPackage.GrossMassMeasure = this.exportStorage.Result.grossMassMeasure;
                                consignmentPackage.PackageTypeCode = this.exportStorage.Result.cargoType == 1 ? "D5" : "PP";
                                consignmentPackage.MarksNumbers = this.exportStorage.Result.MarksNumbers;
                                consignmentPackage.PackageQuantityTypeCode = "EA";
                                consignmentPackage.GrossMassMeasureTypeCode = "KGM";
                                consignmentPackage.DeclarationId = this.declarationPM.Id;


                                consignment.AddConsignmentPackage(consignmentPackage);
                                this.declarationPM.AddConsignment(consignment);

                                this.declarationPMService.update(this.declarationPM).subscribe((response: ServiceResponse) => {
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                });
                            }
                        }
                        else {

                        }

                    });

                }
            }
            this.CurrentSession.CloseCurrentWindowEmit("");

        });



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

        this.declarationPM = windowArgs.DeclarationPM;
        this.ExportFile = this.declarationPM.ExportFile;
    }
}
