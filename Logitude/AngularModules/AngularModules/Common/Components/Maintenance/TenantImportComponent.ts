declare var window: any;
import {Component, OnInit, Output, EventEmitter, AfterViewInit} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {PortList} from '../../EntityLists/PortList';
import {CardList} from '../../EntityLists/CardList';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { CachedDataManager } from '../../../Infrastructure/Utilities/CachedDataManager';

@Component({
    moduleId: module.id,
    selector: 'TenantImportComponent',
    templateUrl: './TenantImportComponent.html',
})

export class TenantImportComponent extends BaseComponent implements OnInit, AfterViewInit {
    public SearchText: string = "Search";
    public DataContext: TenantImportComponent = this;
    public ObjectTableName: string;
    public ObjectTableId: string;
    public columns: any[] = [];
    public ObjectFields: any[] = [];
    public AddButtonVisibility: boolean = false;
    public TenantPM: TenantPM;
    public PortsList: PortList[] = [];
    public AirLinesList: CardList[] = [];
    public ShippingLinesList: CardList[] = [];
    public items: any[] = [];
    public IsVisible: boolean = false;
    public searchFields: string;
    public searchText: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() SearchFieldchangeevent = new EventEmitter();
    public IsNewEntityButtonVisible: boolean = false;
    public NewEntityButtonLabel: string = "New";
    constructor(private _entityListService: EntityListService) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
    }

    SetWindowArgs(args: ImportEntityArgs) {
        this.ObjectTableName = args.ObjectTableName;
        this.ObjectTableId = args.ObjectTableId;

        if (this.ObjectTableName == "ShippingLine") {
            this.AddButtonVisibility = true;
        }
        else {
            this.AddButtonVisibility = false;
        }

        if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline") {
            this.IsNewEntityButtonVisible = true;
        }

        this.NewEntityButtonLabel = TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator.TranslateTable(this.ObjectTableName));
    }

    ngOnInit() {
        var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline") {
            ObjectTable = window.ObjectTables.filter(x => x.Name === "Carrier")[0];             
        }

        this._entityResourceService.getEntityResourceByTableName(ObjectTable.Name, 0).subscribe(response=> {
            this.IsVisible = true;
            this.BuildColumns();
        });
    }

    ngAfterViewInit() {

    }

    public IsDataReady = false;

    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    BuildColumns() {
        var objectTableId = this.ObjectTableId;
        if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline") {
            var ObjectTable = window.ObjectTables.filter(x => x.Name === "Carrier")[0];
            if (ObjectTable != null) {
                objectTableId = ObjectTable.Id;
            }
        }

        this.ObjectFields = window.ObjectFields.filter(f => f.DisplayInSearchWindowList == true && f.ObjectTableId == objectTableId);

        this.columns = [];

        this.columns.push({
            FieldName: this.ObjectTableName,
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '60px' },
            HtmlListComponentName: 'btnComponent',
            HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/btnComponent',
        });
        
        for (var i = 0; i < this.ObjectFields.length; i++) {
            this.columns.push({
                FieldName: this.ObjectFields[i].FieldName,
                IsCustomTemplate : true,
                DataTypeCode: this.ObjectFields[i].DataTypeCode,
                Display: TextCodeTranslator.Translate(this.ObjectFields[i].ListTextCodeCode),
                Styles: { width: '100px' },
                HtmlListComponentName: this.ObjectFields[i].HtmlListComponentName,
                HtmlListComponentUrl: this.ObjectFields[i].HtmlListComponentUrl,
                ColumnHeaderTemplateName: this.ObjectFields[i].ColumnHeaderTemplateName,
            });
        }
        
        if (this.ObjectTableName == "Port") {
            if (this.columns.length > 0){
                this.columns[0].Styles = { width: '65px' };
                if (this.columns.length > 1) {
                    this.columns[1].Styles = { width: '50px' };
                    if (this.columns.length > 2) {
                        this.columns[2].Styles = { width: '100px' };
                        if (this.columns.length > 3) {
                            this.columns[3].Styles = { width: '50px' };
                            if (this.columns.length > 4) {
                                this.columns[4].Styles = { width: '170px' };
                            }
                        }
                    }
                }
            }
        }

        if (this.ObjectTableName == "ShippingLine" || this.ObjectTableName == "Airline" || this.ObjectTableName == "Warehouse") {
            if (this.columns.length > 0) {
                this.columns[0].Styles = { width: '65px' };
                if (this.columns.length > 1) {
                    this.columns[1].Styles = { width: '50px' };
                    if (this.columns.length > 2) {
                        this.columns[2].Styles = { width: '170px' };
                        if (this.columns.length > 3) {
                            this.columns[3].Styles = { width: '50px' };
                        }
                    }
                }
            }
        }
        
        if ((this.ObjectTableName == "Airline" || this.ObjectTableName == "ShippingLine") && this.TenantPM.Id != 0) {

            this.columns.push({
                FieldName: this.ObjectTableName,
                DataTypeCode: 'String',
                Display: '',
                IsCustomTemplate: true,
                Styles: { width: '80px' },
                HtmlListComponentName: 'btnComponent',
                HtmlListComponentUrl: './Infrastructure/Components/QueryColumnsComponents/btnUpdateComponent',
            });
        }
    }

    public rowCount: number;

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.Tenant = this.TenantPM.Id;

        var x = filters.AdditionalFilters.filter(a => a.FieldName == "InActive");
        if (x.length == 0) {
            filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, true, false, "Boolean");
        }

        var rowsObjectTable = this.ObjectTableName;
        
        if (this.ObjectTableName == "Airline") {
            var x = filters.AdditionalFilters.filter(a => a.FieldName == "PartnerTypeId" && a.FieldValue == "AL");
            if (x.length == 0) {
                filters.addAdditionalFilter("PartnerTypeId", "AL", null, null, "Equals", false, true, false, "Text");
            }
            rowsObjectTable = "Carrier";
        }

        if (this.ObjectTableName == "ShippingLine") {
            var x = filters.AdditionalFilters.filter(a => a.FieldName == "PartnerTypeId" && a.FieldValue == "SL");
            if (x.length == 0) {
                filters.addAdditionalFilter("PartnerTypeId", "SL", null, null, "Equals", false, true, false, "Text");
            }
            rowsObjectTable = "Carrier";
        }

        if (this.ObjectTableName == "Warehouse") {
            var x = filters.AdditionalFilters.filter(a => a.FieldName == "PartnerTypeId" && a.FieldValue == "WH");
            if (x.length == 0) {
                filters.addAdditionalFilter("PartnerTypeId", "WH", null, null, "Equals", false, true, false, "Text");
            }
        }
        
        return this._entityListService.getCustomByFilters(rowsObjectTable, filters);
    }

    LoadCachedLists(): void {
        if (this.ObjectTableName == "Port") {
            this.LoadPortListMethod();
        }

        if (this.ObjectTableName == "Airline") {
            this.LoadAirLineListMethod();
        }

        if (this.ObjectTableName == "ShippingLine") {
            this.LoadShippingLineListMethod();
        }

        if (this.ObjectTableName == "Warehouse") {
            this.LoadWarehouseListMethod();
        }
    }

    // Lists 
    private LoadPortListMethod() {
        var filters = new ApiQueryFilters();
        filters.GetAll = true;
        this._entityListService.getByFilters("Port", filters).then((res:any) => {
            res.subscribe(resp => {
                if (resp.Data) {
                    this.PortsList = resp.Data;
                }
                else {
                    this.PortsList = resp;
                }
                this.IsDataReady = true;
            })
        });
    }

    private LoadAirLineListMethod() {
        var filters = new ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("PartnerTypeId", "AL", null, null, "Equals", false, true, false, "Text");
        this._entityListService.getByFilters("Carrier", filters).then((res:any) => {
            res.subscribe(resp => {
                if (resp.Data) {
                    this.AirLinesList = resp.Data;
                }
                else {
                    this.AirLinesList = resp;
                }
                this.IsDataReady = true;
            })
        });
    }

    private LoadShippingLineListMethod() {
        var filters = new ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("PartnerTypeId", "SL", null, null, "Equals", false, true, false, "Text");
        this._entityListService.getByFilters("Carrier", filters).then((res:any) => {
            res.subscribe(resp => {
                if (resp.Data) {
                    this.ShippingLinesList = resp.Data;
                }
                else {
                    this.ShippingLinesList = resp;
                }
                this.IsDataReady = true;
            })
        });
    }

    private LoadWarehouseListMethod(){
        var filters = new ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("PartnerTypeId", "WH", null, null, "Equals", false, true, false, "Text");
        this._entityListService.getByFilters("Card", filters).then((res: any) => {
            res.subscribe(resp => {
                if (resp.Data) {
                    this.AirLinesList = resp.Data;
                }
                else {
                    this.AirLinesList = resp;
                }
                this.IsDataReady = true;
            })
        });
    }

    //Commands 
    CloseButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow(); 
    }

    TextChanged(searchtext) {
        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);
    }

    AddShippingLineButton() {
        this._entityResourceService.getEntityResourceByTableName("ShippingLine", 0).subscribe(response => {
            // Add Shipping Line
            var windowTitle = "New Shipping Line";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 570;
            logWindow.Title = windowTitle;
            logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewShippingLineComponent');
        });
    }

    AddNewEntityClicked() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            SessionLocator.CurrentSession.CloseCurrentWindow(); 

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;            
            logWindow.Title = this.NewEntityButtonLabel;

            logWindow.WindowClosed.subscribe((event: any) => {
                SessionLocator.CurrentSession.FireEvent("NewAirlineShippingLineClosed");
                CachedDataManager.RefreshTableData(this.ObjectTableName, true);
            });

            if (this.ObjectTableName == "ShippingLine") {
                logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewShippingLineComponent');
            }

            else {
                logWindow.Show('./CommonModules/CommonAirline/Components/NewEntity/NewAirlineComponent');
            }            
        });
    }    
}

export class ImportEntityArgs {
    public ObjectTableName: string = null;
    public ObjectTableId: string = null;
}
