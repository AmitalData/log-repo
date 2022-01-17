import { Component, OnInit, ViewChild } from '@angular/core';
import { CourierMasterPM } from 'Customs/EntityPMs/CourierMasterPM';
import { DeclarationsforBulkFeed, PendingWebService } from 'Customs/Services/WebServices/PendingWebService';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { DropdownMenuFilterComponent } from '../DropdownMenuFilterComponent';

@Component({
  selector: 'app-bulk-feed-pending',
  templateUrl: './BulkFeedPendingComponent.html',
  styleUrls: ['./BulkFeedPendingComponent.scss']
})
export class BulkFeedPendingComponent extends BaseComponent {
  @ViewChild(DropdownMenuFilterComponent) MyDropdownMenuFilterComponent: DropdownMenuFilterComponent = new DropdownMenuFilterComponent(null, null);

  DataContext: BulkFeedPendingComponent = this;
  ObjectTableName: string = "Customs.CourierMaster";
  CourierMasterPM: CourierMasterPM = null as any;
  _SelectedTotalInvoiceValue: string = 'A';
  _SelectedFastIndividualProcessValue: string = 'A';
  IsFiltered: boolean = false;
  SearchFilter: string = '';
  declartionList: DeclarationsforBulkFeed[] = [];
  ItemsSource: ObservableCollection = new ObservableCollection([]);

  constructor(
    private pendingWebService: PendingWebService,
  ) {
    super();
  }


  ngOnInit(): void {
    console.log(this.CourierMasterPM)
    this.RefreshList();
  }


  SetWindowArgs(args: any) {
    this.CourierMasterPM = args?.CourierMasterPM;
  }


  async RefreshList() {
    console.log('goodsDescription', this.goodsDescription)
    console.log('weightFrom', this.weightFrom)
    console.log('weightTo', this.weightTo)
    console.log('incotermCode', this.incotermCode)
    console.log('incotermCode', this.SearchFilter)
    this.declartionList = await this.pendingWebService.getDeclarationsforBulkFeed(this.goodsDescription || '', this.weightFrom || '', this.weightTo || '', this.incotermCode || '', this.SearchFilter || '', this._SelectedTotalInvoiceValue || '', this._SelectedFastIndividualProcessValue || '', 0, 100)
    this.ItemsSource.InsertCollection(this.declartionList)
    console.log(this.declartionList)
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

      this.declartionList = await this.pendingWebService.getDeclarationsforBulkFeed(this.goodsDescription || '', this.weightFrom || '', this.weightTo || '', this.incotermCode || '', this.SearchFilter || '', this._SelectedTotalInvoiceValue || '', this._SelectedFastIndividualProcessValue || '', 0, 100)
      this.ItemsSource.AppendCollection(this.declartionList)
    }
  }

  OnFocus() {
    // if (this.ItemsSource.Length == 0) {
    //     this.Add();
    // }
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
