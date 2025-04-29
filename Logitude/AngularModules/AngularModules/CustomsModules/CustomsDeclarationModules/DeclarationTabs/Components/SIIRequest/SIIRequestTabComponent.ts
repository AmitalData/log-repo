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
import { SIIRequestListService } from 'Customs/Services/StandardLists/SIIRequestListService';

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
  private CurrentSession = SessionLocator.SelectedSession;
  siiRequestWebService: SIIRequestWebService
  filterAgrs: ApiQueryFilters;
  public SelectedRow: SIIRequestPM = null;
  @Output() MenuHeaderchangeevent = new EventEmitter();
  public entityResourceService: EntityResourceService = new EntityResourceService();
  public ObjectTableName: string = null;
  siiRequestPMService: SIIRequestPMService;
  siiRequestListService: SIIRequestListService;
  IsLoaded: boolean = false;
  selectedSIIRequest = new SIIRequestPM();
  isOpen: boolean;

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
    let filters = new ApiQueryFilters();


    filters.PageSize = 200;
    filters.PageIndex = 0;
    filters.GetAll = false;
    filters.GetCount = true;

    filters.addAdditionalFilter("DeclarationId", this.currentDeclaration?.Id, null, null, "Equals", false, false, false, "string", false);
    filters.addAdditionalFilter("Tenant", this.currentDeclaration?.Tenant, null, null, "Equals", true, false, false, "string");


    this.siiRequestListService.getByFilters(filters).subscribe((response: ServiceResponse) => {
      if (!response?.HasError && response?.Result !== null) {
        this.ItemsSource = new ObservableCollection(response.Result);
      }
      // TODO: ADD TRY CATCH
    });

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
      this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.DisplayOnly");
      ;
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
