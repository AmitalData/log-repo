declare var System: any;
declare var window: any;
import { Component, OnInit, OnDestroy, EventEmitter, Output } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { TapagPMService } from '../../../../../Customs/Services/StandardPMs/TapagPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';;
import { CustomsCollateralPM } from '../../../../../Customs/EntityPMs/CustomsCollateralPM';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralPMService } from '../../../../../Customs/Services/StandardPMs/CustomsCollateralPMService';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsCollateralList } from '../../../../../Customs/EntityLists/CustomsCollateralList';
import { CustomsCollateralAnswerSharedDataService } from '../../../../../Customs/Services/DataChange/CustomsCollateralAnswerSharedDataService'

@Component({
  moduleId: module.id,
    templateUrl: './DeclarationCollateralsComponent.html',
    providers: [CustomsCollateralAnswerSharedDataService]
})

export class DeclarationCollateralsComponent extends BaseComponent implements OnInit, OnDestroy {
  public EntityPM: DeclarationPM = null;
  public ObjectTableName = "Customs.Declaration";
  public DataContext: DeclarationCollateralsComponent = this;
  public CurrentEditComponentId: string;
  public collateralObslist: ObservableCollection;
  public collateralToSendlist: number[]=[];
  @Output() MenuHeaderchangeevent = new EventEmitter();

  private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
  private _CustomsCollateralPMService: CustomsCollateralPMService = new CustomsCollateralPMService;

  IsLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public NewView: boolean = true;
    
    IsCollateralChecked: boolean;
    IsDisplayButtonSend: boolean;
    constructor(private entityArgs: EntityArgs, private EntityResourceService: EntityResourceService, public _customsCollateralAnswerSharedDataService: CustomsCollateralAnswerSharedDataService) {
    super();
       this.collateralObslist = new ObservableCollection([]);

      this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe((response: any) => {
          this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe((response: any) => {
              this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe((response: any) => {
                  this.EntityPM = this.entityArgs.EntityPM;
                  this.LoadDeclarationCollateralsList();
                  this.Listen();

                  this.BuildColumns();

                  setTimeout(() => {
                      this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
                  }, 10);


                  this.IsLoaded = true;
              });
          });
      });
  }

  ngOnInit() {
      this.EntityPM = this.entityArgs.EntityPM;
  
  }

  ngOnDestroy() {
    console.log("DeclarationCollateralsComponent:ngOnDestroy");
      this.entityArgs = null;
      
  }
  private Listen() {
    if (this.CurrentSession.CurrentEditComponent != null) {

      this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

      this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
        this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
          if (isSaveSuccess) {
            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
          }
        })
      );

      this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
        this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
          if (isLoadSuccess) {
            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            this.LoadDeclarationCollateralsList();
          }
        })
      );

      this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
        this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
          if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
            if (tabCode == "DCCL") {
              this.LoadDeclarationCollateralsList();
            }
          }
        })
      );
    }
  }

    private LoadDeclarationCollateralsList() {
        if (this.NewView) {
            //this._DeclarationCollateralsVListView.MenuHeaderchangeevent()
            return; 
        }
    this.collateralObslist = new ObservableCollection([]);

    this._DeclarationWebService.GetDeclarationCollateralsList(this.EntityPM.Id, this.EntityPM.Tenant)
      .subscribe((myResponse: ServiceResponse) => {
        this.CurrentSession.StopBusyIndicator();
        this.GetDeclarationCollateralsListsOp_Completed(myResponse, false);
      });
  }

  private GetDeclarationCollateralsListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
    if (myResponse.Result != null) {
      myResponse.Result.forEach((item: CustomsCollateralPM) => {
        this.collateralObslist.Insert(item);
      });
    }
  }

  RefreshEntity() {
    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
  }

  EditButtonClicked(item: /*CustomsCollateralPM*/ any) {
    if (!AppTool.IsNullOrEmpty(item)) {
      this._CustomsCollateralPMService.get(item.Id).subscribe(response => {
        var windowArgs: any = {};
        windowArgs.CurrentEntity = response.Result;
        windowArgs.declarationPM = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 700;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');
        this.CurrentSession.StopBusyIndicator();

      });
    }

    }



    ///xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    public DeclarationCollateralsVListViewObjectTableName: string = "Customs.CustomsCollateral";
    public columns: any[] = null;

    
    

    BackBtnTitle: string;
    IsShowTipArea: boolean;

    _stratSearch: boolean = true;
    
    MenuHeaderchangeevent = new EventEmitter();
    
    onQueryChangeEvent = new EventEmitter();

    private _entityListService: EntityListService = new EntityListService();
    
    

    BuildColumns() {
        this.columns = [];
        this.columns.push({

            FieldName: 'IsChecked',
            DataTypeCode: 'String',//'Number',
            Display: '',
            Styles: { width: '30px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsCollateralListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsCollateralListTemplate',
            

          });

        this.columns.push({

            FieldName: 'CollateralRequestNumber',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.CollateralRequestNumber"),
            Styles: { width: '120px' },
            IsCustomTemplate: true
            , ServerSideSortable: true,
            SortByName: 'CollateralRequestNumber'
        });

        this.columns.push({

            FieldName: 'CollateralRequestStatusName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.CollateralRequestStatusName"),
            Styles: { width: '120px' },
            IsCustomTemplate: true


        });


        this.columns.push({

            FieldName: 'RequestValidityDate',
            DataTypeCode: 'Date',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.RequestValidityDate"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsCollateralListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsCollateralListTemplate',


        });

        this.columns.push({

            FieldName: 'CustomsEntityTypeName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.CustomsEntityTypeName"),
            Styles: { width: '120px' },
            IsCustomTemplate: true


        });

        this.columns.push({

            FieldName: 'CustomerName',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.CustomerName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true


        });
        this.columns.push({

            FieldName: 'EntityIdKey1',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.EntityIdKey1"),
            Styles: { width: '120px' },
            IsCustomTemplate: true


        });
        this.columns.push({

            FieldName: 'EntityIdKey2',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.EntityIdKey2"),
            Styles: { width: '120px' },
            IsCustomTemplate: true


        });
        this.columns.push({

            FieldName: 'EntityIdKey3',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.EntityIdKey3"),
            Styles: { width: '120px' },
            IsCustomTemplate: true


        });

        this.columns.push({

            FieldName: 'IsClosed',
            DataTypeCode: 'String',//'Number',
            Display: TextCodeTranslator.Translate("Customs.CustomsCollateral.F.IsClosed"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsCollateralListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsCollateralListTemplate',



        });

    }



    DataSource = {

        pageSize: 10,
        rowCount: null,
        //SortData("RequestCreateDate", "Descending", false, false);
        sortingCol: "",// "Id",
        sortingDir: "",//"Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };
    filterAgrs: ApiQueryFilters;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {



        if (filters == null) {
            filters = new ApiQueryFilters();
        }

        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;

        filters.SortBy = "Id";
        filters.SortDirection = "Descending";

        filters.SortBy = "CollateralRequestNumber";//"Id";
        filters.SortDirection = "Descending";//"Descending";


        
        let declarationId = this.EntityPM.Id;
        filters.addAdditionalFilter("DeclarationId", declarationId, null, null, "Equals", false, false, false, "string");
         
        /// filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");

        var myout = this._entityListService
            .getExtendedByFilters("Customs.CustomsCollateral", filters);
        myout.then(res => {
            this._stratSearch = false;
            //this.CurrentSession.StopBusyIndicator();
        });

        return myout;

    }
    OnRowSelected($event) {
        let customsCollateralList: CustomsCollateralList= $event.rowData;
        this.EditButtonClicked(customsCollateralList);
    }
   
 

    OnCheckedWithSystemEvent(eventM,id) {
         eventM.stopPropagation();
        if (!this.collateralToSendlist.includes(id)) {
            this.collateralToSendlist.push(id);
        }
        else {
                        var removedIndex = null;
            for (var i = 0; i < this.collateralToSendlist.length; i++) {
                if (id == this.collateralToSendlist[i]) {
                    removedIndex = i;
                    break;
                }
            }
            if (removedIndex != null) {
                this.collateralToSendlist.splice(removedIndex,1);
            }

        }

        this.IsDisplayButtonSend = (this.collateralToSendlist.length > 1);

        
 
    }

    OpenEditCollateralAnswerWindow() {
      //   if (!AppTool.IsNullOrEmpty(item)) {
        var windowArgs: any = {};
        windowArgs.DeclarationId = this.EntityPM.Id;
        windowArgs.collateralToSendlist = this._customsCollateralAnswerSharedDataService._SelectedItems.Collection;
            var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.CollateralAnswer");

            var logWindow = new LogitudeWindow();
            logWindow.Width = 400;
            logWindow.Height = 300;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            debugger;
            this._customsCollateralAnswerSharedDataService._SelectedItems.Collection = [];
            this.RefreshList();
        });
            logWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralAnswerComponent');
      

       // }
    }

    RefreshList() {

        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);
    }
}
