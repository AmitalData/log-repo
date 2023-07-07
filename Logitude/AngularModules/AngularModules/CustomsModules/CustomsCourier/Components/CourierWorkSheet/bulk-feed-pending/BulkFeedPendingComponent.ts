import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { CourierMasterPM } from 'Customs/EntityPMs/CourierMasterPM';
import { CourierWorksheetSharedDataService } from 'Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';
import { DeclarationCourierStatusPMService } from 'Customs/Services/StandardPMs/DeclarationCourierStatusPMService';
import { DeclarationsforBulkFeed, PendingWebService } from 'Customs/Services/WebServices/PendingWebService';
import { CourierMasterValidator } from 'Customs/Validators/CourierMasterValidator';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { QueryPM } from 'Infrastructure/EntityPMs/QueryPM';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { ObjectFieldPMExtendedService } from 'Infrastructure/Services/ExtendedPMs/ObjectFieldPMExtendedService';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { Observable } from 'rxjs';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { DropdownMenuFilterComponent } from '../DropdownMenuFilterComponent';

@Component({
  selector: 'app-bulk-feed-pending',
  templateUrl: './BulkFeedPendingComponent.html',
  styleUrls: ['./BulkFeedPendingComponent.scss'],
  providers: [CourierWorksheetSharedDataService]
})
export class BulkFeedPendingComponent extends BaseComponent {
  @Output() MenuHeaderchangeevent = new EventEmitter();

  @ViewChild(DropdownMenuFilterComponent) MyDropdownMenuFilterComponent: DropdownMenuFilterComponent = new DropdownMenuFilterComponent(null, null);

  DataContext: BulkFeedPendingComponent = this;
  ObjectTableName: string = "Customs.DeclarationCourierStatus";
  CourierMasterPM: CourierMasterPM = null as any;
  IsWorkSheetFromExcel:boolean=false;
  _SelectedTotalInvoiceValue: string = 'A';
  _SelectedFastIndividualProcessValue: string = 'A';
  _SelectedMissedDocsValue: string = 'A';
  IsFiltered: boolean = false;
  SearchFilter: string = '';
  declartionList: DeclarationsforBulkFeed[] = [];
  ItemsSource: ObservableCollection = new ObservableCollection([]);
  _CourierMasterValidator: CourierMasterValidator = new CourierMasterValidator();
  IsDisplayOnly: boolean = false;
  DisplayOnlyMessage: string = '';
  _CourierMasterService: CourierMasterService = new CourierMasterService();
  declarationIdsList: string[] = [];
  declarationCourierStatusPMService: DeclarationCourierStatusPMService = new DeclarationCourierStatusPMService();
  checkboxAll: boolean = false;
  public MyScrollTop: number = 0;
  filterAgrs: ApiQueryFilters;
  public PendingFilterItems: ApiQueryFilters;
  PendingList: string = '';
  columns: any[] = []
  query: any;
  private _entityListService: EntityListService = new EntityListService();

  private _RowsItems: any;
  public get RowsItems(): any {
    return this._RowsItems;
  }
  public set RowsItems(value: any) {
    this._RowsItems = value;
  }
  _LOVListPendings: any[] = [];
  get LOVListPendings() { return this._LOVListPendings; }
  set LOVListPendings(value) {
    if (this._LOVListPendings != value) {
      this._LOVListPendings = value;
    }
  }
  private _SelectedRow: any;
  public get SelectedRow(): any {
    return this._SelectedRow;
  }
  public set SelectedRow(value: any) {
    this._SelectedRow = value;
  }
  _stratSearch: boolean = true;

  DataSource = {
    pageSize: 30,
    rowCount: null,
    //sortingCol: "CourierHawb",
    //sortingDir: "Descending",
    getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {

      var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
      return tempo;

    },
  };


  constructor(
    public _CourierWorksheetSharedDataService: CourierWorksheetSharedDataService, private pendingWebService: PendingWebService
  ) {
    super();
    var objectFieldPMExtendedService: ObjectFieldPMExtendedService = new ObjectFieldPMExtendedService();
    objectFieldPMExtendedService.getSingleFromQueries("Customs.DeclarationCourierStatus.BulkFeedPending").subscribe((result: any) => {
      if (result) {
        this.query = result;
      }
    });
  }


  ngOnInit() {
    this.buildColumns();

    this.RefreshList()
  }

  GetPending() {
    SessionLocator.SelectedSession.StartBusyIndicatorCreating();
    this._CourierMasterService.GetPending(this.CourierMasterPM?.Id,this.IsWorkSheetFromExcel)
      .subscribe((resu: any) => {
        SessionLocator.SelectedSession.StopBusyIndicator();
        var list: string[];
        list = resu.Result;
        this.PendingList = list.toString();
        this.PendingFilterItems = new ApiQueryFilters();
        this.PendingFilterItems.addAdditionalFilter("Code", this.PendingList, null, null, "InListExact", false, false, false, "string", false, true);

      });
  }


  getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
    filters = this.getFilter(filters, take, skip, sortingCol, sortingDir);




    var myout = this._entityListService
      .getExtendedByFilters("Customs.DeclarationCourierStatus", filters);
    myout.then(res => {
      this._stratSearch = false;
      //this.CurrentSession.StopBusyIndicator();
    });

    return myout;

  }


  private getFilter(filters: ApiQueryFilters = null, take: any = null, skip: any = null, sortingCol: any = null, sortingDir: any = null): ApiQueryFilters {
    if (filters == null) {
      filters = new ApiQueryFilters();
    }

    filters.PageSize = take;
    filters.PageIndex = skip;
    filters.GetAll = false;
    filters.GetCount = true;

    if (AppTool.IsNullOrEmpty(sortingCol)) {
      filters.SortBy = "CourierHAWB";
      filters.SortDirection = "Descending";
    }
    else {
      filters.SortBy = sortingCol;
      filters.SortDirection = sortingDir;

    }

    if(this.IsWorkSheetFromExcel){
      filters.addAdditionalFilter("CourierHawbsFromExcel", SessionLocator.LoggedUserId, null, null, "Equal", true, false, false, "string");
    }
    filters.addAdditionalFilter("CourierMasterId", this.CourierMasterPM?.Id, null, null, "Equals", false, true, false, "string");
    filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
    filters.addAdditionalFilter("pendingView", SessionLocator.Tenant, null, null, "Equals", true, false, false, "number");


    if (!AppTool.IsNullOrEmpty(this.IncotermCode))
      filters.addAdditionalFilter("IncoTermCode", this.IncotermCode, null, null, "Equals", false, false, false, "string");

    if (this._LOVListPendings.length > 0)
      filters.addAdditionalFilter("CourierPendingReasonList", this.UsersListString, null, null, "InList", false, false, false, "string", this._LOVListPendings.length == 0);


    if (!AppTool.IsNullOrEmpty(this.WeightFrom) && !AppTool.IsNullOrEmpty(this.WeightTo))
      filters.addAdditionalFilter("GrossMassMeasure", this.WeightFrom, this.WeightTo, null, "Between", false, false, false, "number");
    else if (!AppTool.IsNullOrEmpty(this.WeightFrom))
      filters.addAdditionalFilter("GrossMassMeasure", this.WeightFrom, null, null, "GreaterThanOrEqual", false, false, false, "number");
    else if (!AppTool.IsNullOrEmpty(this.WeightTo))
      filters.addAdditionalFilter("GrossMassMeasure", this.WeightTo, null, null, "LessThanOrEqual", false, false, false, "number");

    if (!AppTool.IsNullOrEmpty(this.GoodsDescription))
      filters.addAdditionalFilter("CargoDescription", this.GoodsDescription, null, null, "Contains", true, false, false, "string");
    if (!AppTool.IsNullOrEmpty(this.CasualSupplierName))
      filters.addAdditionalFilter("CasualSupplierName", this.CasualSupplierName, null, null, "Contains", true, false, false, "string");

      if (!AppTool.IsNullOrEmpty(this.CustomerName))
          filters.addAdditionalFilter("ImporterName", this.CustomerName, null, null, "Contains", true, false, false, "string");
      
    switch (this._SelectedFastIndividualProcessValue) {
      case "F": {
        filters.addAdditionalFilter("FastIndividualProcessCode", "F", null, null, "Equals", false, false, false, "string");
        break;
      }
      case "I": {
        filters.addAdditionalFilter("FastIndividualProcessCode", "I", null, null, "Equals", false, false, false, "string");
        break;
      }
    }

    switch (this._SelectedMissedDocsValue) {
      case "T": {
        filters.addAdditionalFilter("MissedDocumentStatusCode", "null", null, null, "Equals", false, false, false, "string");
        break;
      }
      case "I": {
        filters.addAdditionalFilter("MissedDocumentStatusCode", "I", null, null, "Equals", false, false, false, "string");
        break;
      }
      case "C": {
        filters.addAdditionalFilter("MissedDocumentStatusCode", "C", null, null, "Equals", false, false, false, "string");
        break;
      }
    }

    switch (this._SelectedTotalInvoiceValue) {
      case "75": {
        filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 75, null, null, "LessThanOrEqual", false, false, false, "number");
        break;
      }
      case "500": {
        filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 76, 500, null, "Between", false, false, false, "number", false);
        break;
      }
      case "1000": {
        filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 501, 1000, null, "Between", false, false, false, "number", false);
        break;
      }
    }

    if (!AppTool.IsNullOrEmpty(this.SearchFilter))
      filters.addAdditionalFilter("CourierSearchFields", this.SearchFilter.toLowerCase(), null, null, "Contains", false, false, false, "string");

    return filters;
  }
  UsersListString: string = "";
  UsersListStringl: string[] = [];
  SelectedValueChangedEmitUser() {
    var RemoveFilter = false;
    //if (this.getFilter().AdditionalFilters.length > 0) {
    //    this.getFilter().AdditionalFilters = this.getFilter().AdditionalFilters.filter(a => a.FieldName != "CourierPendingReasonList");
    //}
    this.UsersListString = "";
    if (this._LOVListPendings.length > 0) {

      this._LOVListPendings.forEach(item => { this.UsersListString += item["Code"] + ","; this.UsersListStringl.push(item["Code"]); });//Id: "1-3697"
      this.UsersListString = this.UsersListString.slice(0, -1); // trim last comma
    } else {
      this.UsersListString = "HowCare"
      RemoveFilter = true;
    }
    //this.getFilter().addAdditionalFilter("RetrievData", true, null, null, "Equal", true, false, false, "string", this._LOVListPendings.length == 0);
    //this.SelectedValueChanged.emit({ Filters: this.getFilter(), RemoveFilter: RemoveFilter });
  }

  RefreshButtonClicked() {
    // this._CourierWorksheetSharedDataService._SelectedItems.Clear();
    // this._CourierWorksheetSharedDataService.connectedSelectAll = false;

    this.RefreshList();
    this.GetPending();
  }

  OpenMultiUpdateWindow() {

    if (!this._CourierWorksheetSharedDataService._SelectedItems?.Collection?.length && !this._CourierWorksheetSharedDataService.connectedSelectAll)
      return new MessageWindow().Show(TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.O.NotCheckDeclarations"));

    var windowArgs: any = {
      // Declaration: this.EntityPM,
    };
    windowArgs.courierMasterId = this.CourierMasterPM?.Id;
    windowArgs.declarationIdsList = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
    windowArgs.allWithoutdeclarationIdsList = this._CourierWorksheetSharedDataService._UnSelectedItems.Collection;
    windowArgs.checkboxAll = this._CourierWorksheetSharedDataService.connectedSelectAll;
    windowArgs.notUpdateSelf = true;
    windowArgs.filter = this.getFilter();
    windowArgs.filter.GetAll = this._CourierWorksheetSharedDataService.connectedSelectAll;
    var logWindow = new LogitudeWindow();
    logWindow.Width = 500;
    logWindow.Height = 320;
    logWindow.Title = "עדכון הצהרות";

    //logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.MultiUpdate");
    logWindow.WindowArgs = windowArgs;
    logWindow.ShowCloseButton = true;
    logWindow.Width = 500;
    logWindow.Height = 550;
    logWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/MultiUpdateDecComponent');
    logWindow.WindowClosed.subscribe(($event: any) => {
      //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    });
  }

  async CreateInvoiceDocumentWindow() {
    if (!this._CourierWorksheetSharedDataService._SelectedItems?.Collection?.length && !this._CourierWorksheetSharedDataService.connectedSelectAll)
      return new MessageWindow().Show(TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.O.NotCheckDeclarations"));

    var filter = this.getFilter();
    filter.GetAll = this._CourierWorksheetSharedDataService.connectedSelectAll;

    SessionLocator.SelectedSession.StartBusyIndicatorSaving();
    const msg: string = await this.pendingWebService.postBulkFeeding(null, null, this._CourierWorksheetSharedDataService._SelectedItems.Collection,
      this.CourierMasterPM.Id, this._CourierWorksheetSharedDataService.connectedSelectAll,
      this._CourierWorksheetSharedDataService._UnSelectedItems.Collection, filter, true)
    SessionLocator.SelectedSession.StopBusyIndicator();

    const myMessageWindow = new MessageWindow();
    myMessageWindow.Width = 250;
    myMessageWindow.Height = 150;
    myMessageWindow.Show(msg);
    SessionLocator.SelectedSession.CloseCurrentWindow();
  }

  AddPendings() {
    if (!this._CourierWorksheetSharedDataService._SelectedItems?.Collection?.length && !this._CourierWorksheetSharedDataService.connectedSelectAll)
      return new MessageWindow().Show(TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.O.NotCheckDeclarations"));


    var logitudeWindow = new LogitudeWindow();
    var windowArgs: any = {};
    windowArgs.courierMasterId = this.CourierMasterPM?.Id;
    windowArgs.IsWorkSheetFromExcel=this.IsWorkSheetFromExcel;
    windowArgs.declarationIdsList = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
    windowArgs.allWithoutdeclarationIdsList = this._CourierWorksheetSharedDataService._UnSelectedItems.Collection;
    windowArgs.checkboxAll = this._CourierWorksheetSharedDataService.connectedSelectAll;
    windowArgs.notUpdateSelf = true;
    windowArgs.filter = this.getFilter();
    windowArgs.filter.GetAll = this._CourierWorksheetSharedDataService.connectedSelectAll;


    //windowArgs.CourierPendingReasonList = this._CourierWorksheet.CourierPendingReasonList;
    //windowArgs.PendingRemarks = this._CourierWorksheet.PendingRemarks;
    //}
    logitudeWindow.Width = 470;
    logitudeWindow.Height = 300;
    logitudeWindow.IsShowCloseButton = true;
    logitudeWindow.Title = "Pending";//TextCodeTranslator.Translate("CommunicationLog.O.MoreDetails");;
    logitudeWindow.WindowArgs = windowArgs;
    //logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/CourierPendingReasonGeneralComponent');
    logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierPendingReason/DeclarationPendingsBulkFeedingComponent');
    logitudeWindow.WindowClosed.subscribe(($event: any) => {
      // this.RefreshData();
    });
  }

  Export2Excel() {
    var windowArgs: any = {};
    windowArgs.query = this.query;
    windowArgs.currentObjectTable = this.ObjectTableName;
    windowArgs.tenant = SessionInfo.LoggedUserTenant;
    windowArgs.userid = SessionInfo.LoggedUserId;
    windowArgs.Filters = this.getFilter();
    var logitudeWindow = new LogitudeWindow();
    logitudeWindow.Width = 500;
    logitudeWindow.Height = 200;
    logitudeWindow.Title = TextCodeTranslator.Translate("General.B.ExportingDataToExcel");//"Exporting View Data List To Excel File";
    logitudeWindow.WindowArgs = windowArgs;
    logitudeWindow.Show('./Infrastructure/Components/Export2ExcelControl/Export2ExcelControl');
    }

    SendALLSVG(isAll: boolean) {

        
        this.Navigate();
        
    }

    Navigate() {

        //MyFilters.SortBy = this.currentSortingCol;
        //MyFilters.SortDirection = this.currentSortingDir;

        var filter = this.getFilter();
        filter.GetAll = this._CourierWorksheetSharedDataService.connectedSelectAll;


        filter.GetCount = false;
        filter.PageIndex = 0;

        filter.GetAll = true;
        // MyFilters.PageSize = 100;

        //this.CurrentQueryFilters = MyFilters;
        var ids: string[] = [];
        this._entityListService.getExtendedByFilters("Customs.DeclarationCourierStatus", filter, null).then((observable: Observable<any>) => {
            observable.subscribe((response: ServiceResponse) => {
                console.log(response);
                response.Result.forEach((item) => {
                    ids.push(item.DeclarationId);
                });

                console.log(ids);

                var selectedEntityId = ids[0];
                if (ids.length == 0) {
                    return new MessageWindow().Show(TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.O.NotCheckDeclarations"));
                }
                SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = true;
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        var label = "מסך עבודה";//TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({
                            EntityId: selectedEntityId,///$event.rowData.Id
                            BackButtonLabel: label,
                            NavigationIds: ids,
                            SelectedTabCode: "DCCF",
                            ObjectTableName: "Customs.Declaration",
                        });
                        cmpRef.instance.BackCompleted.subscribe(bk => {
                            if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
                                SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = false;
                            }
                            this.OnBackFromEdit(selectedEntityId, event);
                        });                        //  if (SessionLocator.LoggedUserPM.Email == "mohammad@fnarsoft.com") {
                        //this.DestroyMe = true;
                        //}

                    });
            });

        });


    }

    OnBackFromEdit(selectedEntityId, $event) {
        this.MyScrollTop = $event.scrollTop;
        this.RefreshButtonClicked();
    }
  SetWindowArgs(args: any) {
    this.CourierMasterPM = args?.CourierMasterPM;
    this.IsWorkSheetFromExcel  = args?.IsWorkSheetFromExcel;
    this.GetPending();
  }


  async RefreshList() {
    // this.ItemsSource.AppendCollection(this.declartionList)
    setTimeout(() => {
      this.MenuHeaderchangeevent.emit({ filters: this.filterAgrs, ignorefilter: false });
    }, 10);
  }


  SelectedTotalInvoiceValue(value: string) {
    this._SelectedTotalInvoiceValue = value;
    this.onFilteSelect()
  }


  SelectedFastIndividualProcessValueClick(value: string) {
    this._SelectedFastIndividualProcessValue = value;
    this.onFilteSelect()
  }
  SelectedMissedDocsValueClick(value: string) {
    this._SelectedMissedDocsValue = value;
    this.onFilteSelect()
  }

  onFilteSelect() {
    this.IsFiltered = [
      this._SelectedTotalInvoiceValue,
      this._SelectedFastIndividualProcessValue,
      this._SelectedMissedDocsValue
    ].some(selected => selected !== 'A');

    this.RefreshList();
  }


  FilterCleanButtonClicked() {
    this._SelectedTotalInvoiceValue = 'A';
    this._SelectedFastIndividualProcessValue = 'A';
    this._SelectedMissedDocsValue = 'A';

    this.RefreshList();
  }


  FilterCancelButtonClicked() {
    this.IsFiltered = false;
    this.MyDropdownMenuFilterComponent.DropdowndisplayToggle(null);
  }


  onSearchTextChangeEvent(text: string) {
    this.SearchFilter = text;
    this.search()
  }

  search() {
    // this.declarationIdsList = []
    // this._CourierWorksheetSharedDataService._SelectedItems.Clear();
    this.RefreshList();
  }



  CancelButtonClicked() {
    SessionLocator.SelectedSession.CloseCurrentWindow();
  }


  onCheckboxClick(checked: boolean, declarationId: string) {
    if (checked)
      this.declarationIdsList.push(declarationId)
    else
      this.declarationIdsList = this.removeFromArray(this.declarationIdsList, declarationId)

    console.log(this.declarationIdsList)
  }


  private removeFromArray(arr: any[], value: any): any[] {
    return arr.filter(item => item !== value)
  }


  onCheckAllClick(check: boolean) {
    this._CourierWorksheetSharedDataService.connectedSelectAll = true;
    this.checkboxAll = check;
  }


  buildColumns() {
    this.columns = []

    this.columns.push({
      FieldName: "MyDeclarationCheckBox",
      DataTypeCode: 'String',
      Display: '',
      IsCustomTemplate: true,
      HtmlListComponentName: 'CourierWorksheetListTemplate',
      HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
      Styles: { width: '27px' },
      //IsCheckBox: true
    })
    //this.columns.push({
    //    FieldName: 'CourierHawb',
    //  DataTypeCode: 'String',
    //  Display: TextCodeTranslator.Translate("Customs.Declaration.CH.CourierHAWBListLable"),
    //  Styles: { width: '90px' },
    //  IsCustomTemplate: false,
    //  ServerSideSortable: true,
    //    SortByName: 'CourierHawb'
    //});

    this.columns.push({
      FieldName: 'CourierHawb',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierHawb"),
      Styles: { width: '108px' },
      IsCustomTemplate: true,
      HtmlListComponentName: 'CourierWorksheetListTemplate',
      HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
      ServerSideSortable: true,
      SortByName: 'CourierHawb'
    });

    this.columns.push({
      FieldName: 'ImporterName',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CustomerName"),
      Styles: { width: '200px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'ImporterName'
    });

    this.columns.push({
      FieldName: 'ImporterCode',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.ImporterCode"),
      Styles: { width: '90px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'ImporterCode'
    });

    this.columns.push({
      FieldName: 'CargoDescription',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CargoDescription"),
      Styles: { width: '350px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'CargoDescription'
    });
    this.columns.push({
      FieldName: 'IncoTermCode',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.General.O.TermsOfSale"),
      Styles: { width: '60px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'IncoTermCode'
    });
    this.columns.push({
      FieldName: 'TotalInvoiceAmountInUSD',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.Declaration.F.TotalInvoiceAmountInUSD"),
      Styles: { width: '90' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'TotalInvoiceAmountInUSD'
    });
    this.columns.push({
      FieldName: 'GrossMassMeasure',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.MasterBOLQuery.O.TotalWeight"),
      Styles: { width: '45px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'GrossMassMeasure'
    });


    this.columns.push({
      FieldName: 'CasualSupplierAddress',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.ClientAddress"),
      Styles: { width: '150px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'CasualSupplierAddress'
    });
    this.columns.push({
      FieldName: 'CasualImporterCity',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate('Customs.DeclarationCourierStatus.O.City'),
      Styles: { width: '110px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'CasualImporterCity'
    });
    this.columns.push({
      FieldName: 'CourierPendingReasonName',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPendingReasonList"),
      Styles: { width: '105px' },
      IsCustomTemplate: true,
      HtmlListComponentName: 'CourierWorksheetListTemplate',
      HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
      ServerSideSortable: true,
      SortByName: 'CourierPendingReasonName'
    });
    this.columns.push({
      FieldName: 'CasualSupplierName',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CasualSupplierName"),
      Styles: { width: '105px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'CasualSupplierName'
    });
  }


  ViewInitCompleted(e) { }
  OnRowSelected(e) { }


  private goodsDescription: string;
  public get GoodsDescription() { return this.goodsDescription; }
  public set GoodsDescription(newValue: string) {
    this.goodsDescription = newValue;
  }

  private casualSupplierName: string;
  public get CasualSupplierName() { return this.casualSupplierName; }
  public set CasualSupplierName(newValue: string) {
    this.casualSupplierName = newValue;
  }
    private customerName: string;
    public get CustomerName() { return this.customerName; }
    public set CustomerName(newValue: string) {
        this.customerName = newValue;
    }
  private weightFrom: number;
  public get WeightFrom() { return this.weightFrom; }
  public set WeightFrom(newValue: number) {
    this.weightFrom = newValue;
  }

  private weightTo: string;
  public get WeightTo() { return this.weightTo; }
  public set WeightTo(newValue: string) {
    this.weightTo = newValue;
  }

  incotermCode: string;
  public get IncotermCode() { return this.incotermCode; }
  public set IncotermCode(newValue: string) {
    this.incotermCode = newValue;
  }

  IsSelectedNot

  OnAllBtnClickedNot() {
    this.IsSelectedNot = true;

    this._CourierWorksheetSharedDataService.connectedSelectAll = true;
    this._CourierWorksheetSharedDataService._SelectedItems.Clear();
    this._CourierWorksheetSharedDataService._UnSelectedItems.Clear();


    this.RefreshList();
    //this._CourierMasterService.disconnectedSelectAll = true;
    //this.CourierMasterPM.ConnectedDeclarations = "ALL";

    //this.LoadNotConnectedDeclarationGrid();
    //this._CourierMasterService.isNotDirty = false;
  }

  OnNoneBtnClickedNot() {

    this._CourierWorksheetSharedDataService.connectedSelectAll = false;
  
    this._CourierWorksheetSharedDataService._UnSelectedItems.Clear();
    this._CourierWorksheetSharedDataService._SelectedItems.Clear();


    this.RefreshList();

    this.IsSelectedNot = false;
    //this._CourierMasterService.disconnectedSelectAll = false;
    //this.CourierMasterPM.ConnectedDeclarations = "";
    //this._CourierMasterService.isNotDirty = false;

    //this.LoadNotConnectedDeclarationGrid();
  }

  LoadNotConnectedDeclarationGrid() {
    this.filterAgrs = new ApiQueryFilters();
    this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
  }
}
