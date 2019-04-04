import {Component,Output,EventEmitter} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../Infrastructure/Tools';
import {ComputingPartnerPM} from '../../../Common/EntityPMs/ComputingPartnerPM';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ComputingPartnerTablePM} from '../../../Common/EntityPMs/ComputingPartnerTablePM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ComputingPartnerTranslationPMService} from '../../../Common/Services/StandardPMs/ComputingPartnerTranslationPMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {FilterItem} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {CommonDomainService, CustomApiQueryFilters, TranslationItem} from '../../../Common/Services/CommonDomainService';
import {ComputingPartnerTranslationPM} from '../../../Common/EntityPMs/ComputingPartnerTranslationPM'; 

declare var window: any;
@Component({
    selector: 'ComputingPartnerTranslateComponent',
    moduleId: module.id,
    templateUrl: './ComputingPartnerTranslateComponent.html',
})

export class ComputingPartnerTranslateComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: ComputingPartnerTablePM;
    constructor(private _entityListService: EntityListService) {
        super();
        this.CurrentSession.PseventRowSelectEvent.subscribe((res) => {
            if (res.Name == "btnComponentComputingPartnerEdit") {
                var filters;
                if (filters == null) {
                    filters = new CustomApiQueryFilters();
                }

                var table = window.ObjectTables.filter(d => d.Name === res.Value.ObjectTableName)[0];
                if (table)
                    filters.ClinetName = table.ClientModuleName;
                filters.computingPartnerId = res.Value.ComputingPartnerId;
                filters.objectTableId = res.Value.ObjectTableId;
                filters.objectTableName = res.Value.ObjectTableName;
                filters.ComputingPartnerName = res.Value.ComputingPartnerName;
                var domainService: CommonDomainService = new CommonDomainService();
                var items: any[] = [];
                    domainService.getNoneZeroTenantTranslation(res.Value.ComputingPartnerId, res.Value.ObjectTableId, res.Value.OurCode).subscribe(p => {
                        var item: any = {};
                        item.ComputingPartnerId = p.Result.ComputingPartnerId != null ? p.Result.ComputingPartnerId : res.Value.ComputingPartnerId;
                        item.OurCode = p.Result.OurCode != null ? p.Result.OurCode : res.Value.OurCode;
                        item.PartnerCode = p.Result.PartnerCode != null ? p.Result.PartnerCode : res.Value.PartnerCode;
                        item.Id = p.Result.Id != null ? p.Result.Id : res.Value.Id;
                        item.ObjectTableId = p.Result.ObjectTableId != null ? p.Result.ObjectTableId: res.Value.ObjectTableId;
                        item.CreatedByUserId = p.Result.CreatedByUserId != null ? p.Result.CreatedByUserId : res.Value.CreatedByUserId;
                        item.UpdatedByUserId = p.Result.UpdatedByUserId != null ? p.Result.UpdatedByUserId : res.Value.UpdatedByUserId;
                        item.UpdateDate = p.Result.UpdateDate != null ? p.Result.UpdateDate: res.Value.UpdateDate;
                        item.CreateDate = p.Result.CreateDate != null ? p.Result.CreateDate : res.Value.CreateDate;
                        item.Name = p.Result.Name != null ? p.Result.Name : res.Value.Name;
                        if (SessionLocator.Tenant != 0)
                            item.DefaultTranslationPartnerCode = p.Result.DefaultTranslationCode != null ? p.Result.DefaultTranslationCode: res.Value.DefaultTranslationCode;
                        items.push({ rowData: item, rowIndex: res.RowIndex });

                        this.CustomBackFromEditevent.emit(items);
                    });
                          

            }
        });
    }
    @Output() SearchFieldchangeevent = new EventEmitter();
    @Output() CustomBackFromEditevent = new EventEmitter();
    public searchFields: string;
    public SearchText: string = "Search";
    public ObjectTableName: string;
    TextChanged(searchtext) {
        if (searchtext == null)
            searchtext = "";
        this.searchFields = searchtext;        
        this.SearchFieldchangeevent.emit(this.searchFields);
    }


    SetWindowArgs(entity: ComputingPartnerTablePM) {
        this.EntityPM = entity;
        this.ObjectTableName = this.EntityPM.ObjectTableName;  
        this.BuildColumns();      
    }
    
    DataSource = {
        pageSize: 20,
        rowCount: null,
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: CustomApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };
    public columns: any[] = [];
    public items: any[] = [];

    BuildColumns() {      
     
        this.columns.push({
            FieldName: "OurCode",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Our Code',
            Styles: { width: '80px' },

        });


        this.columns.push({
            FieldName: "Name",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            Styles: { width: '100px' },

        });


        if (SessionLocator.Tenant != 0) {
            this.columns.push({
                FieldName: "DefaultTranslationPartnerCode",
                DataTypeCode: 'String',
                IsCustomTemplate: true,
                Display: 'Default Translation',
                Styles: { width: '180px' },

            });

        }

        this.columns.push({
            FieldName: "PartnerCode",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Partner Code',
            Styles: { width: '180px' },

        });
      
        

        this.columns.push({
            FieldName: "MoreDetails",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '80px' },
            HtmlListComponentName: 'btnComponent',
            HtmlListComponentUrl: './InfrastructureModules/InfrastructureComputingPartner/Components/QueryColumnsComponents/btnComponentComputingPartner',
            
        });

        this.columns.push({
            FieldName: "Edit",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '30px' },
            HtmlListComponentName: 'btnComponentComputingPartnerEdit',
            HtmlListComponentUrl: './InfrastructureModules/InfrastructureComputingPartner/Components/QueryColumnsComponents/btnComponentComputingPartnerEdit',

        });
       
    }
    public $event;
    onRowSelected($event) {
        this.$event = $event;
    }

    public rowCount: number;

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: CustomApiQueryFilters = null) {
            if (filters == null) {
                filters = new CustomApiQueryFilters();
            }

            var table = window.ObjectTables.filter(d => d.Name === this.EntityPM.ObjectTableName)[0];
            if (table)
                filters.ClinetName = table.ClientModuleName;

            filters.GetAll = true;
            filters.GetCount = getCount;
            filters.PageIndex = skip;
            filters.PageSize = take;
            filters.Tenant = SessionLocator.Tenant;
            filters.tenant2 = SessionLocator.Tenant;
            filters.computingPartnerId = this.EntityPM.ComputingPartnerId;
            filters.objectTableId = this.EntityPM.ObjectTableId;
            filters.objectTableName = this.EntityPM.ObjectTableName;
            filters.ComputingPartnerName = this.EntityPM.ComputingPartnerName;
            filters.SearchingFields = this.searchFields;
            filters.IsClosedTable = table.IsClosed;
            var service: CommonDomainService = new CommonDomainService();
            return new Promise((resolve, reject) => {
                resolve(service.getByFilters(filters))
            }).then(result => {
                return result;
            });


    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }




}



