import { Component, OnInit, ViewChild } from '@angular/core';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { AddMultiPendingsRequestParams } from 'Customs/DataContract/RequestParams/AddMultiPendingsRequestParams';
import { CourierMasterPM } from 'Customs/EntityPMs/CourierMasterPM';
import { CustomsRequestsSheetPM } from 'Customs/EntityPMs/CustomsRequestsSheetPM';
import { DeclarationCourierStatusPM } from 'Customs/EntityPMs/DeclarationCourierStatusPM';
import { CourierWorksheetSharedDataService } from 'Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';
import { DeclarationCourierStatusPMService } from 'Customs/Services/StandardPMs/DeclarationCourierStatusPMService';
import { DeclarationsforBulkFeed, PendingWebService } from 'Customs/Services/WebServices/PendingWebService';
import { CourierMasterValidator } from 'Customs/Validators/CourierMasterValidator';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { DeclarationPendingsBulkFeedingComponent } from '../../CourierPendingReason/DeclarationPendingsBulkFeedingComponent';
import { DropdownMenuFilterComponent } from '../DropdownMenuFilterComponent';

@Component({
  selector: 'app-bulk-feed-pending',
  templateUrl: './BulkFeedPendingComponent.html',
  styleUrls: ['./BulkFeedPendingComponent.scss']
})
export class BulkFeedPendingComponent extends BaseComponent {
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

  constructor(
    private pendingWebService: PendingWebService,
    private _CourierWorksheetSharedDataService: CourierWorksheetSharedDataService,
    private logtuideTableDataService: LogtuideTableDataService,
  ) {
    super();
  }


  async ngOnInit(): Promise<void> {
    console.log(this.CourierMasterPM)
    await this.RefreshList();

    this.initPendingList()
  }


  SetWindowArgs(args: any) {
    this.CourierMasterPM = args?.CourierMasterPM;
  }


  async RefreshList() {
    this.declartionList = await this.pendingWebService.getDeclarationsforBulkFeed(this.CourierMasterPM.Id, this.goodsDescription || '', this.weightFrom || '', this.weightTo || '', this.incotermCode || '', this.SearchFilter || '', this._SelectedTotalInvoiceValue || '', this._SelectedFastIndividualProcessValue || '', 0, 100)
    this.ItemsSource.InsertCollection(this.declartionList)
  }
  
  
  async initPendingList() {
    const args = {
      DeclarationCourierStatus : (await this.logtuideTableDataService.getDataFromService(this.declarationCourierStatusPMService.get(this.declartionList[0]?.DeclarationId))),
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

  async OnRowEnded($event) {
    //console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
    if (($event) == this.ItemsSource.Length) {
      //setTimeout(() => this.Add(), 1);
      // this.Add();

      this.declartionList = await this.pendingWebService.getDeclarationsforBulkFeed(this.CourierMasterPM.Id, this.goodsDescription || '', this.weightFrom || '', this.weightTo || '', this.incotermCode || '', this.SearchFilter || '', this._SelectedTotalInvoiceValue || '', this._SelectedFastIndividualProcessValue || '', 0, 100)
      this.ItemsSource.AppendCollection(this.declartionList)
    }
  }

  OnFocus() {
    // if (this.ItemsSource.Length == 0) {
    //     this.Add();
    // }
  }


  async onOkClick(declarationCourierStatus : DeclarationCourierStatusPM) {
    console.log(declarationCourierStatus)
    const listPending = declarationCourierStatus.DeclarationPendings.map(x => x.CourierPendingReasonCode)
    const listPendingRemark = declarationCourierStatus.DeclarationPendings.map(x => x.PendingRemarks)

    SessionLocator.SelectedSession.StartBusyIndicatorSaving();
    const res = await this.pendingWebService.postBulkFeeding(listPending, listPendingRemark, this.declarationIdsList)
    SessionLocator.SelectedSession.StopBusyIndicator();

    const myMessageWindow = new MessageWindow();
    myMessageWindow.Width = 250;
    myMessageWindow.Height = 150;
    myMessageWindow.Show(res);
    SessionLocator.SelectedSession.StopBusyIndicator();
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
}
