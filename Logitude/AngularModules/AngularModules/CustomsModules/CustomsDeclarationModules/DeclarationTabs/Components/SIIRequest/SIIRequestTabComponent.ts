import { Component, OnInit } from '@angular/core';
import { BaseRequestsSheetMassaging } from 'CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SIIRequestWebService } from 'Customs/Services/WebServices/SIIRequestWebService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';

@Component({
    selector: 'SIIRequestTabComponent',
    templateUrl: './SIIRequestTabComponent.html',
    styleUrls: ['./SIIRequestTabComponent.scss'],
    providers: [DeclarationExtendedListService]
})
export class SIIRequestTabComponent extends BaseRequestsSheetMassaging implements OnInit {
  public ItemsSource: ObservableCollection = new ObservableCollection([]);
  public SelectedRow: SIIRequestPM = null;
  public EntityPM: any;
  public FilterStatus: 'All' | 'Open' | 'Closed' = 'Open';
  public DisplayOnlyMessage: string = '';
  public IsDisplayOnly: boolean = false;
  public IsVisible: boolean = true;
  filterAgrs: ApiQueryFilters;

  constructor(private siiRequestWebService: SIIRequestWebService, public entityArgs: EntityArgs) {
    super();
  }

  ngOnInit(): void {
    //this.EntityPM = SessionLocator?.CurrentEditComponent?.EntityPM;
    this.DisplayOnlyCheck();
    this.loadRequests();
  }

  loadRequests(): void {
    const declarationId = this.EntityPM?.Id;
    // this.siiRequestWebService.getRequestsByDeclarationId(declarationId, this.EntityPM?.Tenant).subscribe((data: any) => {
    //     const SIIRequests: SIIRequestPM[] = data?.Result;
    //     this.ItemsSource = new ObservableCollection(SIIRequests);
    // });
    // create moke data for testing to itemsource from type SIIRequestPM[]:
    const mock1 = new SIIRequestPM();
    mock1.Id = '1';
    mock1.RequestNo = 'REQ-1001';
    mock1.Status = 'Open';
    mock1.WareHouseAddress = 'רח\' הגפן 12';
    mock1.WareHouseCity = 'ת\"א';
    mock1.IsClosed = false;
    
    const mock2 = new SIIRequestPM();
    mock2.Id = '2';
    mock2.RequestNo = 'REQ-1002';
    mock2.Status = 'Closed';
    mock2.WareHouseAddress = 'הרצל 45';
    mock2.WareHouseCity = 'חיפה';
    mock2.IsClosed = true;
    
    const mock3 = new SIIRequestPM();
    mock3.Id = '3';
    mock3.RequestNo = 'REQ-1003';
    mock3.Status = 'Open';
    mock3.WareHouseAddress = 'דרך מנחם בגין 78';
    mock3.WareHouseCity = 'ירושלים';
    mock3.IsClosed = false;
    
    this.ItemsSource = new ObservableCollection([mock1, mock2, mock3]);
    
    
  }

  onOpenNewRequest(): void {
    // פתיחת חלון בקשה חדשה
  }

  onEditRequest(item: SIIRequestPM): void {
    this.SelectedRow = item;
    // פתיחת חלון עריכה לפריט הנבחר
  }

  onRowSelected(item: SIIRequestPM): void {
    this.SelectedRow = item;
    this.filterAgrs = new ApiQueryFilters();
    // this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
  }

  RefreshEntity() {
    // SessionLocator?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
    // SessionLocator?.CurrentEditComponent?.ReloadEntityPM();
  }

  DisplayOnlyCheck() {
    // this.IsDisplayOnly = SessionLocator?.CurrentEditComponent?.EditComponentController?.InDisplayMode;
    if (this.EntityPM?.AmendmentMessage) {
      this.DisplayOnlyMessage = this.EntityPM.AmendmentMessage;
      if (this.EntityPM.IsAmendmentDisplayOnly) {
        this.IsDisplayOnly = true;
      }
    } else if (this.IsDisplayOnly) {
      this.DisplayOnlyMessage = 'לתצוגה בלבד';
    }
  }

  get IsAllowChange(): boolean {
    return !this.IsDisplayOnly;
  }
} 