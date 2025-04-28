import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SIIRequestWebService } from 'Customs/Services/WebServices/SIIRequestWebService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { SIIRequestPMService } from 'Customs/Services/StandardPMs/SIIRequestPMService';
import { AppTool } from 'Infrastructure/Tools';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
  selector: 'SIIRequestTabComponent',
  templateUrl: './SIIRequestTabComponent.html',
  styleUrls: ['./SIIRequestTabComponent.scss'],
  providers: [DeclarationExtendedListService]
})
export class SIIRequestTabComponent extends BaseComponent implements OnInit {
  public ItemsSource: ObservableCollection = new ObservableCollection([]);
  public currentDeclaration: DeclarationPM;
  public FilterStatus: 'All' | 'Open' | 'Closed' = 'Open';
  public DisplayOnlyMessage: string = '';
  public IsDisplayMessage: string = '';
  public IsDisplayOnly: boolean = false;
  public IsVisible: boolean = true;
  private CurrentSession = SessionLocator.SelectedSession;
  siiRequestWebService: SIIRequestWebService
  filterAgrs: ApiQueryFilters;
  public SelectedRow: SIIRequestPM = null;
  @Output() MenuHeaderchangeevent = new EventEmitter();
  public entityResourceService: EntityResourceService = new EntityResourceService();
  public ObjectTableName: string = null;
  siiRequestPMService: SIIRequestPMService;
  IsLoaded: boolean = false;
  selectedSIIRequest = new SIIRequestPM();
  isOpen: boolean;

  constructor(public entityArgs: EntityArgs) {
    super();
    this.EntityPM = this.CurrentSession?.CurrentEditComponent?.EntityPM;
    this.currentDeclaration = this.EntityPM;
    this.siiRequestWebService = new SIIRequestWebService();
  }


  ngOnInit() {
    this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
      this.entityResourceService.getEntityResourceByTableName("Customs.SIIRequest").subscribe((response: any) => {
        this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsReqList").subscribe((response: any) => {
          this.siiRequestPMService = new SIIRequestPMService();
          this.ObjectTableName = this.entityArgs.ObjectTableName;
          this.IsLoaded = true;
        });
      });
    });
    this.DisplayOnlyCheck();
    this.loadRequests();
  }

  loadRequests(): void {
      //TODO:change to real call to server getbyfilter - by declarationid + tenant

    // TODO: Delete after finish - create moke data for testing to itemsource from type SIIRequestPM[]:
    const mock1 = new SIIRequestPM();
    mock1.Id = '1';
    mock1.ListCounter = 1;
    mock1.Remarks = 'test 1';
    mock1.RequestNo = 'REQ-1001';
    mock1.Status = 'Open';
    mock1.WareHouseAddress = 'רח\' הגפן 12';
    mock1.WareHouseCity = 'ת\"א';
    mock1.IsClosed = false;

    const mock2 = new SIIRequestPM();
    mock2.Id = '2';
    mock2.ListCounter = 2;
    mock2.Remarks = 'test 2';
    mock2.RequestNo = 'REQ-1002';
    mock2.Status = 'Closed';
    mock2.WareHouseAddress = 'הרצל 45';
    mock2.WareHouseCity = 'חיפה';
    mock2.IsClosed = true;

    const mock3 = new SIIRequestPM();
    mock3.Id = '3';
    mock3.ListCounter = 3;
    mock3.Remarks = 'test 3';
    mock3.RequestNo = 'REQ-1003';
    mock3.Status = 'Open';
    mock3.WareHouseAddress = 'דרך מנחם בגין 78';
    mock3.WareHouseCity = 'ירושלים';
    mock3.IsClosed = false;
    this.ItemsSource = new ObservableCollection([mock1, mock2, mock3]);
  }

  onOpenNewRequest(): void {
    this.AddNewSIIRequest(SiiRequestMode.IsNew);
  }

  onEditRequest(item: SIIRequestPM): void {
    this.SelectedRow = item;
    this.selectedSIIRequest = this.SelectedRow;
    this.AddNewSIIRequest(SiiRequestMode.IsEdit);
  }

  AddNewSIIRequest(siiRequestMode: SiiRequestMode) {
    const newSIIRequestPM = new SIIRequestPM();
    newSIIRequestPM.DeclarationId = AppTool.IsNullOrEmpty(this.EntityPM.AmendmentOriginalDeclartation) ? this.EntityPM.Id : this.EntityPM.AmendmentOriginalDeclartation;
    newSIIRequestPM.Tenant = this.EntityPM.Tenant;

    let args: any = {
      Decalaration: this.EntityPM,
      SIIRequest: SiiRequestMode.IsEdit === siiRequestMode ? this.selectedSIIRequest : newSIIRequestPM,
      IsNewOrEdit: siiRequestMode
    };

    if (siiRequestMode === SiiRequestMode.IsNew) {
      this.openLogWindow(siiRequestMode, args);
    }
    else {
      this.getSIIRequestByIDAndopenLogWindow(args.SIIRequest.Id, this.EntityPM.Id, siiRequestMode, args);
    }

  }

  getSIIRequestByIDAndopenLogWindow(requestId: number, declarationId: string, siiRequestMode: SiiRequestMode, args: any) {
    // TODO: build the logic in GetRequestsByDeclarationIdIncludeChildrens
    this.siiRequestWebService.GetRequestsByDeclarationIdIncludeChildrens(requestId, declarationId, this.EntityPM.Tenant).subscribe(myResult => {
      let myResponse: ServiceResponse = myResult;
      if (!myResponse.HasError && myResponse.Result) {
        this.selectedSIIRequest = myResponse.Result;
        args.SIIRequest = myResponse.Result;
        this.openLogWindow(siiRequestMode, args);
      }
    });
  }

  openLogWindow(isNewOrEditMode, args) {
    if (this.isOpen) return;
    this.isOpen = true;
    let logWindow = new LogitudeWindow();
    logWindow.Width = 1030;
    logWindow.Height = 735;
    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.SIIRequest");
    args.isAllowChange = this.IsAllowChange;
    logWindow.WindowArgs = args;
    logWindow.ShowCloseButton = true;
    logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestTabs/SIIRequestTabsComponent');
    args.logWindow = logWindow;
    if (isNewOrEditMode === SiiRequestMode.IsNew)
      this.SelectedRow = null

    logWindow.WindowClosed.subscribe(($event: any) => {
      this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
      this.ReloadMyScreen();
      this.isOpen = false;
    });
  }

  ReloadMyScreen() {
    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
    // TODO: Create getSiiRequest method getSiiRequest()
    this.DisplayOnlyCheck();
  }

  OnRowSelected(itemComponent: SIIRequestPM) {
    this.SelectedRow = itemComponent;
    this.filterAgrs = new ApiQueryFilters();

    this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
  }

  RefreshEntity() {
    this.CurrentSession?.CurrentEditComponent?.EditComponentController?.ResetMustRefresh();
    this.CurrentSession?.CurrentEditComponent?.ReloadEntityPM();
  }

  DisplayOnlyCheck() {
    this.IsDisplayOnly = this.CurrentSession?.CurrentEditComponent?.EditComponentController?.InDisplayMode;
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

export enum SiiRequestMode {
  IsNew = 'IsNew',
  IsEdit = 'IsEdit',
}
