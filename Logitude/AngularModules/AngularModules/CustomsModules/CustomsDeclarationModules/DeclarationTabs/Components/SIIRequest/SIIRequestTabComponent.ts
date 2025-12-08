import { Component, OnInit } from '@angular/core';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { SIIRequestWebService, SupplierInvoiceItemsForSIIRequest } from 'Customs/Services/WebServices/SIIRequestWebService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { SIIRequestPMService } from 'Customs/Services/StandardPMs/SIIRequestPMService';
import { AppTool } from 'Infrastructure/Tools';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SIIRequestListService } from 'Customs/Services/StandardLists/SIIRequestListService';

@Component({
  selector: 'SIIRequestTabComponent',
  templateUrl: './SIIRequestTabComponent.html',
  styleUrls: ['./SIIRequestTabComponent.scss'],
  providers: [DeclarationExtendedListService]
})
export class SIIRequestTabComponent extends BaseComponent implements OnInit {
  public siiRequestWebService: SIIRequestWebService;
  public entityResourceService: EntityResourceService = new EntityResourceService();
  public siiRequestPMService: SIIRequestPMService;
  public siiRequestListService: SIIRequestListService;
  public ItemsSource: ObservableCollection = new ObservableCollection([]);
  public siiRequestList: SIIRequestPM[] = [];
  public currentDeclaration: DeclarationPM;
  public FilterStatus: 'All' | 'Open' | 'Closed' = 'Open';
  public DisplayOnlyMessage: string = '';
  public IsDisplayMessage: string = '';
  public IsDisplayOnly: boolean = false;
  private CurrentSession = SessionLocator.SelectedSession;
  public filterAgrs: ApiQueryFilters;
  public SelectedRow: SIIRequestPM = null;
  public ObjectTableName: string = null;
  public IsLoaded: boolean = false;
  public selectedSIIRequest = new SIIRequestPM();
  public supplierInvoiceItemsForSIIRequest: SupplierInvoiceItemsForSIIRequest[] = [];
  public isOpen: boolean;
  public isCloseRequests: SiiRequestIsClosed = SiiRequestIsClosed.IsOpen;
  public querySelectionList: QueryOption[] = [];

  constructor(public entityArgs: EntityArgs) {
    super();
    this.EntityPM = this.CurrentSession?.CurrentEditComponent?.EntityPM;
    this.currentDeclaration = this.EntityPM;
    this.siiRequestWebService = new SIIRequestWebService();
    this.siiRequestListService = new SIIRequestListService();
  }

  ngOnInit() {
    this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
      this.entityResourceService.getEntityResourceByTableName("Customs.SIIRequest").subscribe((response: any) => {
        this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsReqList").subscribe((response: any) => {
          this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe((response: any) => {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
              this.siiRequestPMService = new SIIRequestPMService();
              this.ObjectTableName = this.entityArgs.ObjectTableName;
              this.IsLoaded = true;
              this.initQuerySelectionList();
            });
          });
        });
      });
    });
    this.initFullScreen();
  }

  initFullScreen() {
    this.DisplayOnlyCheck();
    this.loadRequests();
  }

  initQuerySelectionList() {
    this.querySelectionList = [
      new QueryOption(TextCodeTranslator.Translate('Customs.SIIRequest.O.OpenRequest'), SiiRequestIsClosed.IsOpen),
      new QueryOption(TextCodeTranslator.Translate('Customs.SIIRequest.O.ClosedRequest'), SiiRequestIsClosed.IsClosed),
      new QueryOption(TextCodeTranslator.Translate('Customs.SIIRequest.O.AllRequest'), SiiRequestIsClosed.All)
    ];
  }

  initFilterArgs() {
    let filter = new ApiQueryFilters();
    filter.PageSize = 200;
    filter.PageIndex = 0;
    filter.GetAll = false;
    filter.GetCount = true;
    return filter;
  }

  loadRequests(): void {
    this.filterAgrs = this.initFilterArgs();
    this.filterAgrs.SortBy = "RequestDate";
    this.filterAgrs.SortDirection = "descending";

    this.filterAgrs.addAdditionalFilter("DeclarationId", this.currentDeclaration?.Id, null, null, "Equals", false, false, false, "string", false);
    this.filterAgrs.addAdditionalFilter("Tenant", this.currentDeclaration?.Tenant, null, null, "Equals", true, false, false, "string");
    if (SiiRequestIsClosed.IsClosed === this.isCloseRequests)
      this.filterAgrs.addAdditionalFilter("IsClosed", true, null, null, "Equals", false, false, false, "boolean", false);
    else if (SiiRequestIsClosed.IsOpen === this.isCloseRequests)
      this.filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "boolean", false);

    this.siiRequestListService.getByFilters(this.filterAgrs).subscribe((response: ServiceResponse) => {
      if (!response?.HasError && response?.Result !== null) {
        this.siiRequestList = response.Result;
        this.ItemsSource.Clear();
        let counter = 0;
        this.siiRequestList?.forEach(item => {
          item.ListCounter = ++counter;
          this.ItemsSource.Insert(item, true);
        });
      }
      else this.ItemsSource.Clear();
    });
  }

  editRequestFilterClosed(data: SiiRequestIsClosed) {
    this.isCloseRequests = data;
    this.loadRequests();
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
    const isEdit = (siiRequestMode === SiiRequestMode.IsEdit);

    let siiRequest: SIIRequestPM;

    if (isEdit) {
      siiRequest = this.selectedSIIRequest;
    } else {
      const declarationId =
        AppTool.IsNullOrEmpty(this.EntityPM.AmendmentOriginalDeclartation)
          ? this.EntityPM.Id
          : this.EntityPM.AmendmentOriginalDeclartation;

      siiRequest = new SIIRequestPM();
      siiRequest.DeclarationId = declarationId;
      siiRequest.Tenant = this.EntityPM.Tenant;
    }

    let isAllowChange = this.IsAllowChange;

    if (isEdit && !AppTool.IsNullOrEmpty(siiRequest?.RequestNo)) {
      isAllowChange = false;
    }

    let args: any = {
      Decalaration: this.EntityPM,
      SIIRequest: siiRequest,
      IsNewOrEdit: siiRequestMode,
      filterAgrs: this.initFilterArgs(),
      isAllowChange: isAllowChange
    };

    if (siiRequestMode === SiiRequestMode.IsNew)
      this.getSIIRequestDataAndopenLogWindow(this.currentDeclaration.Id, null, siiRequestMode, args);
    else
      this.getSIIRequestDataAndopenLogWindow(this.currentDeclaration.Id, this.SelectedRow.Id, siiRequestMode, args);
  }

  getSIIRequestDataAndopenLogWindow(declarationId: string, id: string, siiRequestMode: SiiRequestMode, args: any) {
    this.siiRequestWebService.getByDeclarationId(declarationId, id).subscribe(myResult => {
      let myResponse: ServiceResponse = myResult;
      if (!myResponse?.HasError && myResponse?.Result) {
        this.selectedSIIRequest = myResponse.Result;
        args.SIIRequest = myResponse.Result;
        args.errorMassage = [];
        this.siiRequestWebService.getSupplierInvoiceItemsForSIIRequest(declarationId, this.selectedSIIRequest?.Id).subscribe(myResult => {
          let myResponse: ServiceResponse = myResult;
          if (!myResponse?.HasError && myResponse?.Result) {
            this.supplierInvoiceItemsForSIIRequest = myResponse.Result;
            args.supplierInvoiceItemsForSIIRequest = myResponse.Result;
            args.errorMassage = [];
            this.openLogWindow(siiRequestMode, args);
          }
          else {
            this.supplierInvoiceItemsForSIIRequest = [];
            args.supplierInvoiceItemsForSIIRequest = [];
            args.errorMassage = ["error in getting supplier invoice items for SII request"];
            this.openLogWindow(siiRequestMode, args);
          }
        });
      }
      else {
        args.errorMassage = ["error in getting SII request data"];
      }
    });
  }

  openLogWindow(isNewOrEditMode, args) {
    if (this.isOpen) return;
    this.isOpen = true;
    let logWindow = new LogitudeWindow();
    logWindow.Width = 1030;
    logWindow.Height = 770;
    let title = TextCodeTranslator.Translate("Customs.Declaration.TH.SIIRequest");
    const requestNo = this.selectedSIIRequest?.RequestNo || args?.SIIRequest?.RequestNo;
    if (!AppTool.IsNullOrEmpty(requestNo)) {
      title += ` - ${requestNo}`;
    }
    logWindow.Title = title;
    logWindow.SubTitle = `${this.EntityPM?.CustomFileNo}`;
    if (!AppTool.IsNullOrEmpty(this.selectedSIIRequest?.ImporterId)) logWindow.SubTitle += ` / ${TextCodeTranslator.Translate("Customs.SIIRequest.F.ImporterId")}: ${this.selectedSIIRequest?.ImporterId}`;
    args.isAllowChange = this.IsAllowChange;
    logWindow.WindowArgs = args;
    logWindow.ShowCloseButton = true;
    logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/SIIRequest/SIIRequestTabs/SIIRequestComponent');
    args.logWindow = logWindow;
    if (isNewOrEditMode === SiiRequestMode.IsNew) this.SelectedRow = null

    logWindow.WindowClosed.subscribe(($event: any) => {
      this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
      this.ReloadMyScreen();
      this.isOpen = false;
      this.initFullScreen();
    });
  }

  ReloadMyScreen() {
    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
    this.DisplayOnlyCheck();
  }

  OnRowSelected(itemComponent: SIIRequestPM) {
    this.SelectedRow = itemComponent;
    this.selectedSIIRequest = this.SelectedRow;
    this.filterAgrs = this.initFilterArgs();
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
      this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.DisplayOnly");
      ;
    }
    if (this.EntityPM.HatraDate || this.EntityPM.PaymentDate) {
      this.IsDisplayOnly = true;
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

export enum SiiRequestIsClosed {
  IsClosed = 'IsClosed',
  IsOpen = 'IsOpen',
  All = 'All',
}

class QueryOption {
  public name: string;
  public value: string;
  constructor(name: string, value: string) {
    this.name = name;
    this.value = value;
  }
}
