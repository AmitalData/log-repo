import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { AddMultiPendingsRequestParams } from 'Customs/DataContract/RequestParams/AddMultiPendingsRequestParams';
import { CourierDeclarationList } from 'Customs/EntityLists/CourierDeclarationList';
import { DeclarationCourierStatusList } from 'Customs/EntityLists/DeclarationCourierStatusList';
import { CourierMasterPM } from 'Customs/EntityPMs/CourierMasterPM';
import { CustomsRequestsSheetPM } from 'Customs/EntityPMs/CustomsRequestsSheetPM';
import { DeclarationCourierStatusPM } from 'Customs/EntityPMs/DeclarationCourierStatusPM';
import { CourierWorksheetSharedDataService } from 'Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';
import { CourierDeclarationListService } from 'Customs/Services/StandardLists/CourierDeclarationListService';
import { DeclarationCourierStatusPMService } from 'Customs/Services/StandardPMs/DeclarationCourierStatusPMService';
import { DeclarationsforBulkFeed, PendingWebService } from 'Customs/Services/WebServices/PendingWebService';
import { CourierMasterValidator } from 'Customs/Validators/CourierMasterValidator';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { DeclarationPendingsBulkFeedingComponent } from '../../CourierPendingReason/DeclarationPendingsBulkFeedingComponent';
import { DropdownMenuFilterComponent } from '../DropdownMenuFilterComponent';

@Component({
  selector: 'app-bulk-feed-pending',
  templateUrl: './BulkFeedPendingComponent.html',
  styleUrls: ['./BulkFeedPendingComponent.scss']
})
export class BulkFeedPendingComponent extends BaseComponent {
  @Output() MenuHeaderchangeevent = new EventEmitter();

  @ViewChild(DropdownMenuFilterComponent) MyDropdownMenuFilterComponent: DropdownMenuFilterComponent = new DropdownMenuFilterComponent(null, null);
  @ViewChild(DeclarationPendingsBulkFeedingComponent) DeclarationPendingsBulkFeedingComponent: DeclarationPendingsBulkFeedingComponent;

  DataContext: BulkFeedPendingComponent = this;
  ObjectTableName: string = "Customs.CourierMaster";
  CourierMasterPM: CourierMasterPM = null as any;
  _SelectedTotalInvoiceValue: string = 'A';
  _SelectedFastIndividualProcessValue: string = 'A';
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
  columns: any[] = []

  private _RowsItems: any;
  public get RowsItems(): any {
    return this._RowsItems;
  }
  public set RowsItems(value: any) {
    this._RowsItems = value;
  }

  private _SelectedRow: any;
  public get SelectedRow(): any {
    return this._SelectedRow;
  }
  public set SelectedRow(value: any) {
    this._SelectedRow = value;
  }

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

  // declartionList = [];
  constructor(
    private pendingWebService: PendingWebService,
    private _CourierWorksheetSharedDataService: CourierWorksheetSharedDataService,
    private logtuideTableDataService: LogtuideTableDataService,
  ) {
    super();
  }


  async ngOnInit(): Promise<void> {
    this.buildColumns();

    await this.RefreshList();

    this.initPendingList()
  }


  SetWindowArgs(args: any) {
    this.CourierMasterPM = args?.CourierMasterPM;
  }


  async RefreshList() {
    // this.declartionList = await this.pendingWebService.getDeclarationsforBulkFeed(this.CourierMasterPM.Id, this.goodsDescription || '', this.weightFrom || '', this.weightTo || '', this.incotermCode || '', this.SearchFilter || '', this._SelectedTotalInvoiceValue || '', this._SelectedFastIndividualProcessValue || '', 0, 100)
    // this.ItemsSource.AppendCollection(this.declartionList)
    setTimeout(() => {
      this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }, 10);
  }


  async initPendingList() {
    const filters = new ApiQueryFilters();
    filters.GetAll = false;
    filters.PageSize = 1;
    filters.PageIndex = 0;
    filters.addAdditionalFilter("CourierMasterId", this.CourierMasterPM.Id, null, null, "Equals", false, false, false, "string");
    filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");

    const CourierDeclarationsStatusList: DeclarationCourierStatusList = (await this.logtuideTableDataService.getDataFromService((await new EntityListService().getExtendedByFilters("Customs.DeclarationCourierStatus", filters) as any)))[0];
    const CourierDeclarationsStatusPM = await this.logtuideTableDataService.getDataFromService(new DeclarationCourierStatusPMService().get(CourierDeclarationsStatusList.DeclarationId))
    const args = {
      // DeclarationCourierStatus: (await this.logtuideTableDataService.getDataFromService(this.declarationCourierStatusPMService.get(this.declartionList[0]?.DeclarationId))),
      DeclarationCourierStatus: CourierDeclarationsStatusPM,
      notUpdateSelf: true
    }

    this.DeclarationPendingsBulkFeedingComponent.SetWindowArgs(args);
  }


  SelectedTotalInvoiceValue(value: string) {
    this._SelectedTotalInvoiceValue = value;
    this.onFilteSelect()
  }


  SelectedFastIndividualProcessValueClick(value: string) {
    this._SelectedFastIndividualProcessValue = value;
    this.onFilteSelect()
  }


  onFilteSelect() {
    this.IsFiltered = [
      this._SelectedTotalInvoiceValue,
      this._SelectedFastIndividualProcessValue
    ].some(selected => selected !== 'A');

    this.RefreshList();
  }


  FilterCleanButtonClicked() {
    this._SelectedTotalInvoiceValue = 'A';
    this._SelectedFastIndividualProcessValue = 'A';
    this.RefreshList();
  }


  FilterCancelButtonClicked() {
    this.IsFiltered = false;
    this.MyDropdownMenuFilterComponent.DropdowndisplayToggle(null);
  }


  onSearchTextChangeEvent(text: string) {
    this.SearchFilter = text;
    this.RefreshList();
  }

  // async OnRowEnded($event) {
  //   console.log("this.ItemsSource.Length : " + this.ItemsSource.Length, $event);
  //   if (($event) == this.ItemsSource.Length) {
  //     //setTimeout(() => this.Add(), 1);
  //     // this.Add();

  //     this.declartionList = await this.pendingWebService.getDeclarationsforBulkFeed(this.CourierMasterPM.Id, this.goodsDescription || '', this.weightFrom || '', this.weightTo || '', this.incotermCode || '', this.SearchFilter || '', this._SelectedTotalInvoiceValue || '', this._SelectedFastIndividualProcessValue || '', 0, 100)
  //     this.ItemsSource.AppendCollection(this.declartionList)
  //   }
  // }

  // OnFocus() {
  //   // if (this.ItemsSource.Length == 0) {
  //   //     this.Add();
  //   // }
  // }


  async onOkClick(declarationCourierStatus: DeclarationCourierStatusPM) {
    console.log(declarationCourierStatus)
    const listPending = declarationCourierStatus.DeclarationPendings.map(x => x.CourierPendingReasonCode)
    const listPendingRemark = declarationCourierStatus.DeclarationPendings.map(x => x.PendingRemarks)

    SessionLocator.SelectedSession.StartBusyIndicatorSaving();
    const res = await this.pendingWebService.postBulkFeeding(listPending, listPendingRemark, this.declarationIdsList, this.CourierMasterPM.Id, this.checkboxAll)
    SessionLocator.SelectedSession.StopBusyIndicator();

    const myMessageWindow = new MessageWindow();
    myMessageWindow.Width = 250;
    myMessageWindow.Height = 150;
    myMessageWindow.Show(res);
    SessionLocator.SelectedSession.StopBusyIndicator();
    this.CancelButtonClicked()
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
    this.checkboxAll = check;
  }


  buildColumns() {
    this.columns = []

    this.columns.push({
      FieldName: "MyDeclarationCheckBox",
      DataTypeCode: 'String',
      Display: '',
      IsCustomTemplate: true,
      Styles: { width: '27px' },
      //IsCheckBox: true
    })
    this.columns.push({
      FieldName: 'CourierHAWB',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.Declaration.CH.CourierHAWBListLable"),
      Styles: { width: '90px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'CourierHAWB'
    });
    this.columns.push({
      FieldName: 'Importername',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.Declaration.F.ImporterName"),
      Styles: { width: '120px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'Importername'
    });
    this.columns.push({
      FieldName: 'Code',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.ImporterCode"),
      Styles: { width: '120px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'Code'
    });
    this.columns.push({
      FieldName: 'Cargodescription',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.ReleaseGoods.O.GoodsDescription"),
      Styles: { width: '150px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'Cargodescription'
    });
    this.columns.push({
      FieldName: 'IncotermCode',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.General.O.TermsOfSale"),
      Styles: { width: '60px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'IncotermCode'
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
      FieldName: 'PackageMeasureQualifierCode',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.MasterBOLQuery.O.TotalWeight"),
      Styles: { width: '45px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'PackageMeasureQualifierCode'
    });
    this.columns.push({
      FieldName: 'Casualimporteraddress',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.ClientAddress"),
      Styles: { width: '150px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'Casualimporteraddress'
    });
    this.columns.push({
      FieldName: 'Casualimportercity',
      DataTypeCode: 'String',
      Display: TextCodeTranslator.Translate("Customs.City.Q.CityQuery"),
      Styles: { width: '110px' },
      IsCustomTemplate: true,
      ServerSideSortable: true,
      SortByName: 'Casualimportercity'
    });
  }


  getRows(skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) {
    return new Promise<any>((resolve, reject) => {
      const res = this.pendingWebService.getDeclarationsforBulkFeed(this.CourierMasterPM.Id, this.goodsDescription || '', this.weightFrom || '', this.weightTo || '', this.incotermCode || '', this.SearchFilter || '', this._SelectedTotalInvoiceValue || '', this._SelectedFastIndividualProcessValue || '', skip, take, sortingCol, sortingDir);
      resolve(res)
    })
  }

  ViewInitCompleted(e) { }
  OnRowSelected(e) { }


  private goodsDescription: string;
  public get GoodsDescription() { return this.goodsDescription; }
  public set GoodsDescription(newValue: string) {
    this.goodsDescription = newValue;
  }

  private weightFrom: string;
  public get WeightFrom() { return this.weightFrom; }
  public set WeightFrom(newValue: string) {
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
    this._CourierMasterService.disconnectedSelectAll = true;
    this.CourierMasterPM.ConnectedDeclarations = "ALL";

    this.LoadNotConnectedDeclarationGrid();
    this._CourierMasterService.isNotDirty = false;
  }

  OnNoneBtnClickedNot() {
    this.IsSelectedNot = false;
    this._CourierMasterService.disconnectedSelectAll = false;
    this.CourierMasterPM.ConnectedDeclarations = "";
    this._CourierMasterService.isNotDirty = false;

    this.LoadNotConnectedDeclarationGrid();
  }

  LoadNotConnectedDeclarationGrid() {
    this.filterAgrs = new ApiQueryFilters();
    this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
  }
}
