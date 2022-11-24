import { Component, AfterViewInit, ViewChildren, QueryList, OnDestroy} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentValidator} from '../../../../Shipment/Validators/ShipmentValidator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentPickUpPM} from '../../../../Shipment/EntityPMs/ShipmentPickUpPM';
import { ShipmentFollowUpPM } from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import { ShipmentPickUpDeliveryPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import { PickUpDeliveryPackageHarmonizePM } from '../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {WarehouseHelper} from '../../../../Warehouse/Helpers/WarehouseHelper';
import { ShipmentPickupValidator } from '../../../../Shipment/Validators/ShipmentPickupValidator';
import { ShipmentTool } from '../../../../Shipment/Tools';
import { NewShipmentComponentArgs } from '../../../../Shipment/Args';
import { FeatureToggleList } from '../../../../Infrastructure/EntityLists/FeatureToggleList';
import { ShipmentDomainService } from '../../../../Shipment/Services/ShipmentDomainService';
import { PackageTypePMService } from '../../../../Common/Services/StandardPMs/PackageTypePMService';
import { WarehouseEntryListExtendedService } from '../../../../Warehouse/Services/ExtendedLists/WarehouseEntryListExtendedService';

@Component({    
    templateUrl: './AddEditPickupComponent.html',
})

export class AddEditPickupComponent implements AfterViewInit, OnDestroy {
  public SelectedTab: any;
    public EntityPM: ShipmentPickUpPM;
    public ShipmentPM: ShipmentPM;
    public ConnectedWarehouseEntry: any;
    public ObjectTableName: string = "ShipmentPickUpDelivery";
    public IsNewEntity: boolean = false;   
    public TabsItemsSource: TabItem[] = [];
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    public IsShowNewWarehouseEntryButton: boolean = false;
    public IsFromCallBack: boolean = false;
    private myCardListService: CardListService;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsAddingStandaloneShipmentVisible: boolean = false;
    public IsEditingEnabled: boolean = true;
    private warehouseEntryListExtendedService: WarehouseEntryListExtendedService;
    constructor(private entityResourceService: EntityResourceService) {
        this.myCardListService = new CardListService();
        this.warehouseEntryListExtendedService = new WarehouseEntryListExtendedService();     
    }

    SavedEntityId: string;
    SavedEntityNumber: string;
    SetWindowArgs(args: any) {
        this.IsNewEntity = args['IsNewEntity'];
        this.ShipmentPM = args['ShipmentPM'];
        this.EntityPM = args['EntityPM'];

        this.SavedEntityId = this.EntityPM.Id;
        this.SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;

        if (this.IsNewEntity && !this.EntityPM.TransportModeCode) {
            this.EntityPM.TransportModeCode = "BYTR";
        }

        this.Clone();

        if (!AppTool.IsNullOrEmpty(this.EntityPM.StandaloneShipmentId)) {
            this.IsEditingEnabled = false;
        }

        else {
            this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.ShipmentPM);
        }

        var featureToggle: FeatureToggleList = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "SAS")[0];
        if (featureToggle) {
            this.IsAddingStandaloneShipmentVisible = true;
        }

        if (this.ShipmentPM) {
            if (this.ShipmentPM.ShipmentLevelCode == "D" || this.ShipmentPM.ShipmentLevelCode == "H") {
                if (FeatureLocator.HasFeaturePermession("WarehouseEntry", "Module")) {
                    this.IsShowNewWarehouseEntryButton = this.ShipmentPM.DirectionId != "I" ? true : false;
                }
            }
        }
  
        this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDelivery").subscribe((res: any) => {
            this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDeliveryPackage").subscribe((res2: any) => {
                this.LoadConnectedWarehouseEntry();
                this.Listen();
            });
        });
    }

    LoadConnectedWarehouseEntry() {
        this.warehouseEntryListExtendedService.GetActiveWarehouseEntriesByShipmentId(this.ShipmentPM.Id).subscribe((serviceResponse: ServiceResponse) => {
            var warehouseEntries = serviceResponse.Result;
            if (warehouseEntries && warehouseEntries.length > 0) {
                this.ConnectedWarehouseEntry = warehouseEntries.filter(warehouseEntry => warehouseEntry.ConnectedToReferenceNumber == this.EntityPM.PickUpDeliveryNumber)[0];
            }
            this.IsResourcesReady = true;
            this.LoadTemplate();
        });
    }

    get IsCreateStandaloneShipmentEnabled() {
        var isEnabled: boolean = false;

        if (this.EntityPM.FullResponsibility) {
            isEnabled = true;
        }

        return isEnabled;
    }

    private isViewInited: boolean = false;
    ngAfterViewInit() {
        this.isViewInited = true;
        this.LoadTemplate();
    }
    LoadTemplate() {
        if (this.isViewInited && this.IsResourcesReady) {
            this.BuildTabs();
            this.SelectionChanged();
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private SessionEvent: any = null;
    private BackCompletedEvent: any = null;

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.SessionEvent);
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.SessionEvent = null;
    }

    Listen() {
        if (this.CurrentSession.CurrentEditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "ReloadPickUpDelivery") {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            });


            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.ShipmentPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.ResetEntityPM();
                    }
                });
            }
        }
    }

    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("MAIN", "ShipmentPickUpDelivery.TH.Main"));
        this.TabsItemsSource.push(new TabItem("PACG", "ShipmentPickUpDelivery.TH.Packages"));
        this.TabsItemsSource.push(new TabItem("DCSO", "ShipmentPickUpDelivery.TH.DocsOut", this.IsNewEntity));
        this.TabsItemsSource.push(new TabItem("DCSI", "ShipmentPickUpDelivery.TH.DocsIn", this.IsNewEntity));
        this.selectedTabCode = "MAIN";
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    private PageChild_MAIN: any = null;
    private PageChild_PACG: any = null;
    private PageChild_DCSO: any = null;
    private PageChild_DCSI: any = null;
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {

                    case "MAIN": {
                        if (this.PageChild_MAIN == null) {
                            myLocation.viewContainerRef.clear();

                            SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/PickupTabs/PickupMainTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_MAIN = cmpRef.instance;
                                    this.PageChild_MAIN.InitTab(this.EntityPM, this.ShipmentPM);

                                    if (this.IsNewEntity) {
                                        var toPortId = this.ShipmentPM.MainCarriageFromPortId;
                                        if (!AppTool.IsNullOrEmpty(this.ShipmentPM.PreCarriageFromPortId)) {
                                            toPortId = this.ShipmentPM.PreCarriageFromPortId;
                                        }

                                        this.PageChild_MAIN.FromPartnerCardId = this.ShipmentPM.ShipperId;
                                        this.PageChild_MAIN.ToPortId = toPortId;
                                    }

                                    else {
                                        this.PageChild_MAIN.OnEditMoodScreen();
                                    }
                                });
                        }

                        break;
                    }

                    case "PACG": {
                        if (this.PageChild_PACG == null) {
                            this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDeliveryPackage").subscribe((response:any) => {
                                SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/PickupTabs/PickupPackagesTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_PACG = cmpRef.instance;
                                        this.PageChild_PACG.InitTab(this.EntityPM, this.ShipmentPM);
                                    });
                            });
                        }

                        break;
                    }

                    case "DCSO": {
                        if (this.PageChild_DCSO == null) {
                            SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/PickupTabs/PickupDocsOutTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_DCSO = cmpRef.instance;
                                    this.PageChild_DCSO.InitTab(this.EntityPM, this.ShipmentPM);
                                });
                        }

                        break;
                    }

                    case "DCSI": {
                        if (this.PageChild_DCSI == null) {
                            SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/PickupTabs/PickupDocsInTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_DCSI = cmpRef.instance;
                                    this.PageChild_DCSI.InitTab(this.EntityPM, this.ShipmentPM);
                                });
                        }

                        break;
                    }
                }
            }
        }
    }

    CloseClicked() {
        if (this.EntityPM.IsDirty) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.DontSave");
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("General.B.Save");
            confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", "Pickup"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.Save(true);
                }

                else if (confirmWindow.No) {
                    this.CloseWindow();
                }
            });
        }

        else {
            this.CloseWindow();
        }
    }
    CloseWindow() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.Save(false);
    }
    SaveChangesAndClose() {
        this.Save(true);
    }
    Save(isClosingWindow: boolean, callbackMethod: string = "") {

        var isValid = this.Validate();

        if (isValid) {

            this.SavedEntityId = this.EntityPM.Id;
            this.SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;

            if (this.IsNewEntity) {
                this.ShipmentPM.AddPickUp(this.EntityPM);
                this.isEntityAdded = true;
            }

            if (this.CurrentSession.CurrentEditComponent != null) {
                if (!this.SaveCompletedEvent) {

                    this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        this.OnSaveCompleted(isSaveSuccess, isClosingWindow, callbackMethod);
                    });

                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                }
            }
        }
    }

    Validate() {
        var validator = new ShipmentPickupValidator();
        var errors: string[] = validator.Validate(this.EntityPM, this.ShipmentPM);

        if (errors.length == 0) {
            var myShipmentValidator = new ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.ShipmentPM);
            if (myShipmentErrors.length > 0) {
                errors.push(TextCodeTranslator.Translate("Shipment.M.Routings.CantProceedAddingDelivery"));
            }
        }

        this.ValidationErrorsList = errors;

        var isValid: boolean = errors.length == 0 ? true : false;

        return isValid;
    }

    OnSaveCompleted(isSaveSuccess: boolean, isClosingWindow: boolean, callbackMethod: string = "") {
        if (isSaveSuccess) {

            if (this.IsNewEntity) {
                ServiceLocator.SendTotangoUserActivity("Shipment", "PickUpOpen");
                this.IsNewEntity = false;

                this.TabsItemsSource.forEach(item => {
                    item.IsDisabled = false;
                });
            }

            if (isClosingWindow) {
                this.CurrentSession.CloseCurrentWindow();
            }

            else {
                this.ResetEntityPM();
            }

            if (!isClosingWindow && !AppTool.IsNullOrEmpty(callbackMethod)) {
                this.CallBack(callbackMethod);
            }
        }

        else {
            this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
        }

        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        this.SaveCompletedEvent = null;     
    }

    CallBack(callbackMethod) {
        this.IsFromCallBack = true;

        if (callbackMethod == "NewWarehouseEntryButtonClicked") {
            this.NewWarehouseEntryButtonClicked();
        }
    }

    ResetEntityPM() {
        if (this.CurrentSession.CurrentEditComponent) {
            this.ShipmentPM = this.CurrentSession.CurrentEditComponent.EntityPM;

            if (this.SavedEntityId) {
                this.EntityPM = this.ShipmentPM.ShipmentPickUps.filter(f => f.Id == this.SavedEntityId)[0];
            }

            else if (this.SavedEntityNumber) {
                this.EntityPM = this.ShipmentPM.ShipmentPickUps.filter(f => f.PickUpDeliveryNumber == this.SavedEntityNumber)[0];

                if (this.EntityPM) {
                    this.SavedEntityId = this.EntityPM.Id;
                }
            }

            if (this.PageChild_MAIN) {
                // code modified due to refresh dates issue
                //this.PageChild_MAIN.InitTab(this.EntityPM, this.ShipmentPM);

                this.PageChild_MAIN = null;

                if (this.SelectedTabCode == "MAIN") {
                    this.SelectionChanged();
                }
            }

            if (this.PageChild_PACG) {
                this.PageChild_PACG.InitTab(this.EntityPM, this.ShipmentPM);
            }

            if (this.PageChild_DCSO) {
                this.PageChild_DCSO.InitTab(this.EntityPM, this.ShipmentPM);
            }

            if (this.PageChild_DCSI) {
                this.PageChild_DCSI.InitTab(this.EntityPM, this.ShipmentPM);
            }

            if (this.isCreateStandaloneShipmentClicked) {
                this.isCreateStandaloneShipmentClicked = false;
                this.ValidateStandaloneAddresses();
            }

            if (this.isConnctingStandaloneShipmentClicked) {
                this.isConnctingStandaloneShipmentClicked = false;
                this.ValidateStandaloneAddresses();
            }

            this.Clone();
        }
    }

    NewWarehouseEntryButtonClicked() {
        if (AppTool.IsNullOrEmpty(this.SavedEntityId) && !this.IsFromCallBack) {
            this.Save(false, "NewWarehouseEntryButtonClicked");
        }
        this.IsFromCallBack = false;

        if (AppTool.IsNullOrEmpty(this.SavedEntityId)) {
            return;
        }

        if (this.ShipmentPM.IsDirty) {
            var errors: any[] = [];
            var myShipmentValidator = new ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.ShipmentPM);
            if (myShipmentErrors.length > 0) {
                errors.push("This shipment has validation errors you cant proceed adding cross dock entry!");
            }

            if (errors.length == 0) {

                if (this.CurrentSession.CurrentEditComponent != null) {
                    if (!this.SaveCompletedEvent) {
                        this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                            if (isSaveSuccess) {
                                this.ShipmentPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                                this.Clone();
                                this.InitializeWareHousEntryWindow();

                            } else {
                                this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                            }

                            AppTool.KillEventEmitter(this.SaveCompletedEvent);
                            this.SaveCompletedEvent = null;
                        });
                    }

                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                } else this.InitializeWareHousEntryWindow();


            }
            else {
                this.ValidationErrorsList = errors;
            }

        } else this.InitializeWareHousEntryWindow();
    }
    InitializeWareHousEntryWindow() {
        if (this.EntityPM.PickUpDeliveryToTypeCode == "PART" && !AppTool.IsNullOrEmpty(this.EntityPM.ToPartnerCardId)) {

            this.myCardListService.getSingle(this.EntityPM.ToPartnerCardId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var myCardList: CardList = myResponse.Result;
                    if (myCardList && myCardList.PartnerTypeId == "WH") {
                        this.LoadAddViewWarehouseEntryWindow(myCardList.Id, false);

                    } else this.LoadAddViewWarehouseEntryWindow("");
                } else this.LoadAddViewWarehouseEntryWindow("");
            });


        } else {
            this.LoadAddViewWarehouseEntryWindow("");
        }
    }

    ViewWareHouseEntry() {
        if (!this.ConnectedWarehouseEntry || !this.ConnectedWarehouseEntry.Id) return;

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef).then(cmpRef => {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: this.ConnectedWarehouseEntry.Id, ObjectTableName: 'WarehouseEntry', BackButtonLabel: "Shipment pickup" + ": " + this.EntityPM.PickUpDeliveryNumber });
            cmpRef.instance.BackCompleted.subscribe(ok => {
                //
            });
        });
    }

    OpenWareHouseEntryWindow(warehouseId: string) {
        this.CurrentSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.ExpectedEntryDate = this.EntityPM.ETA;
        windowArgs.ActualEntryDate = this.EntityPM.ATA;
        windowArgs.WarehouseId = warehouseId;
        windowArgs.EntityPM = this.ShipmentPM;
        windowArgs.EntityChildPM = this.EntityPM;
        windowArgs.PageRequest = "ShipmentPickUp";
        windowArgs.ConnectedTo = "PickUp";
        windowArgs.ChildEntityReference = this.EntityPM.PickUpDeliveryNumber;
        windowArgs.ParentComponent = this;

        var warehouseHelper: WarehouseHelper = new WarehouseHelper();
        warehouseHelper.ShowNewWarehouseEntryComponent(windowArgs);

    }

    LoadAddViewWarehouseEntryWindow(warehouseId: string, getWarehouseFromShipment: boolean = true) {
        if (getWarehouseFromShipment) {
            warehouseId = this.ShipmentPM.WarehouseLegWarehouseId;
        }
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.warehouseEntryListExtendedService.GetActiveWarehouseEntriesByWarehouseId(warehouseId).subscribe((serviceResponse: ServiceResponse) => {
            var warehouseEntries = serviceResponse.Result;
            this.LoadWarehouseEntryWindow(warehouseId, warehouseEntries, getWarehouseFromShipment)
        });
    }

    LoadWarehouseEntryWindow(warehouseId, warehouseEntries, getWarehouseFromShipment) {
        if (warehouseEntries && warehouseEntries.length > 0) {
            warehouseEntries = warehouseEntries.filter(warehouseEntry => this.FilterActiveWarehouseEntries(warehouseEntry));
        }
        if (warehouseEntries && warehouseEntries.length > 0) {
            this.ShowSelectionAddChooseWarehouseEntryWindow(warehouseId, warehouseEntries);
            return;
        }
        if (!getWarehouseFromShipment) {
            this.OpenWareHouseEntryWindow(warehouseId);
            return;
        }

        this.OpenWareHouseEntryWindow("");
    }


    private FilterActiveWarehouseEntries(warehouseEntry: any) {
        if (warehouseEntry.CustomerId != this.ShipmentPM.CustomerId) return false;
        if (warehouseEntry.ConnectedToShipment) return false;
        if (!AppTool.IsNullOrEmpty(warehouseEntry.ConnectedToReferenceNumber)) return false;

        return true;
    }

    ShowSelectionAddChooseWarehouseEntryWindow(warehouseId, warehouseEntries) {
        this.CurrentSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.ExpectedEntryDate = this.EntityPM.ETA;
        windowArgs.ActualEntryDate = this.EntityPM.ATA;
        windowArgs.WarehouseId = warehouseId;
        windowArgs.WarehouseEntries = warehouseEntries;
        windowArgs.EntityPM = this.ShipmentPM;
        windowArgs.EntityChildPM = this.EntityPM;
        windowArgs.PageRequest = "ShipmentPickUp";
        windowArgs.ConnectedTo = "PickUp";
        windowArgs.ChildEntityReference = this.EntityPM.PickUpDeliveryNumber;
        windowArgs.ParentComponent = this;

        var warehouseHelper: WarehouseHelper = new WarehouseHelper();
        warehouseHelper.ShowSelectionAddChooseWarehouseEntryWindow(windowArgs);
    }

    private myCloner: Cloner;
    private oldFollowups: ShipmentFollowUpPM[] = [];
    private isEntityAdded: boolean = false;
    private Clone() {

        this.ClonePackages();

        this.oldFollowups = [];        
        this.ShipmentPM.FollowUps.forEach(item => {
            var oldItem: ShipmentFollowUpPM = new ShipmentFollowUpPM(null);
            oldItem.Id = item.Id;
            oldItem.Date = item.Date;
            oldItem.Deleted = item.Deleted;
            oldItem.Done = item.Done;
            oldItem.DoneDateTime = item.DoneDateTime;
            oldItem.DoneNote = item.DoneNote;
            oldItem.EntityDateId = item.EntityDateId;
            oldItem.EventTypeFollowUpName = item.EventTypeFollowUpName;
            oldItem.EventTypeId = item.EventTypeId;
            oldItem.Tenant = item.Tenant;
            oldItem.ExternalDocumentId = item.ExternalDocumentId;
            oldItem.IsNew = item.IsNew;
            oldItem.JobId = item.JobId;
            oldItem.LegType = item.LegType;
            oldItem.ManualActivatedFollowUp = item.ManualActivatedFollowUp;
            oldItem.Notes = item.Notes;
            oldItem.OwnerUserId = item.OwnerUserId;
            oldItem.OwnerUserName = item.OwnerUserName;
            oldItem.ShipmentId = item.ShipmentId;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            oldItem.IsDirty = item.IsDirty;
            oldItem.EntityParentPM = item.EntityParentPM;
            this.oldFollowups.push(oldItem);
        });
        
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('FullResponsibility');
        this.myCloner.AddField('FromTypeCode');
        this.myCloner.AddField('PickUpDeliveryFromTypeCode');
        this.myCloner.AddField('FromPartnerCardId');
        this.myCloner.AddField('FromPortId');
        this.myCloner.AddField('FromPortCode');
        this.myCloner.AddField('FromPortName');
        this.myCloner.AddField('FromPortCountryCode');
        this.myCloner.AddField('FromPortCountryName');
        this.myCloner.AddField('FromAddressId');
        this.myCloner.AddField('FromAddressCity');
        this.myCloner.AddField('FromAddressCity_Dummy');
        this.myCloner.AddField('FromAddressZipCode');
        this.myCloner.AddField('FromAddressCountryId');
        this.myCloner.AddField('FromAddressCountryCode');
        this.myCloner.AddField('FromAddressCountryName');
        this.myCloner.AddField('FromAddress');
        this.myCloner.AddField('ToTypeCode');
        this.myCloner.AddField('PickUpDeliveryToTypeCode');
        this.myCloner.AddField('ToPartnerCardId');
        this.myCloner.AddField('ToPortId');
        this.myCloner.AddField('ToPortCode');
        this.myCloner.AddField('ToPortName');
        this.myCloner.AddField('ToPortCountryCode');
        this.myCloner.AddField('ToPortCountryName');
        this.myCloner.AddField('ToAddressId');
        this.myCloner.AddField('ToAddressCity');
        this.myCloner.AddField('ToAddressCity_Dummy');
        this.myCloner.AddField('ToAddressZipCode');
        this.myCloner.AddField('ToAddressCountryId');
        this.myCloner.AddField('ToAddressCountryCode');
        this.myCloner.AddField('ToAddressCountryName');
        this.myCloner.AddField('ToAddress');
        this.myCloner.AddField('CarrierId');
        this.myCloner.AddField('CarrierCode');
        this.myCloner.AddField('CarrierName');
        this.myCloner.AddField('CarrierWebSite');
        this.myCloner.AddField('CarrierNumber');
        this.myCloner.AddField('Driver');
        this.myCloner.AddField('TruckNumber');
        this.myCloner.AddField('TrailerNumber');
        this.myCloner.AddField('TransportModeCode');        
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('EmptyPickupContainerPartnerId');
        this.myCloner.AddField('EmptyPickupDepotReference');
        this.myCloner.AddField('ETD');
        this.myCloner.AddField('ETA');
        this.myCloner.AddField('ATD');
        this.myCloner.AddField('ATA');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.ShipmentPM);
    }
    private RejectChanges() {
        if (this.EntityPM.IsDirty) {

            this.RejectPackages();

            var addedItems: any[] = [];
            var removedItems: any[] = [];

            this.oldFollowups.forEach(item => {
                var existingItem = this.ShipmentPM.FollowUps.filter(f => f.LegType == item.LegType)[0];
                if (!existingItem) {
                    removedItems.push(item);
                }
            });

            this.ShipmentPM.FollowUps.forEach(item => {
                var oldItem = this.oldFollowups.filter(f => f.LegType == item.LegType)[0];
                if (oldItem == null) {
                    addedItems.push(item);
                }
            });

            if (addedItems.length > 0 || removedItems.length > 0) {
                addedItems.forEach(item => {
                    this.ShipmentPM.RemoveShipmentFollowUp(item);
                });

                removedItems.forEach(item => {
                    this.ShipmentPM.AddShipmentFollowUp(item);
                });

                this.CurrentSession.FireEvent("FollowupsChanged");
            }

            if (this.isEntityAdded) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    this.ShipmentPM.RemovePickUp(this.EntityPM);
                }
            }

            if (this.IsAddingStandaloneShipmentVisible) {
                if (this.ShipmentPM.ShipmentPackages != null) {
                    this.ShipmentPM.ShipmentPackages.filter(item => AppTool.IsNullOrEmpty(item.Id)).forEach(item => {
                        this.ShipmentPM.RemovePackage(item);
                    });
                }
            }
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.myCloner.RejectChanges();
        }
    }

    private oldPackages: ShipmentPickUpDeliveryPackagePM[] = [];
    private ClonePackages() {

        var PackageFields: string[] = [];
        PackageFields.push("Id");
        PackageFields.push("Tenant");
        PackageFields.push("ContainerNumber");
        PackageFields.push("PackageTypeId");
        PackageFields.push("PackageTypeName");
        PackageFields.push("PackageTypeTEU");
        PackageFields.push("ShipmentPickUpDeliveryId");
        PackageFields.push("Quantity");
        PackageFields.push("Volume");
        PackageFields.push("Weight");
        PackageFields.push("Description");
        PackageFields.push("Harmonize");
        PackageFields.push("ShipperSeal");
        PackageFields.push("Width");
        PackageFields.push("Height");
        PackageFields.push("Length");
        PackageFields.push("OriginalShipmentPackageId");
        PackageFields.push("IsMultiHarmonize");
        PackageFields.push("IsDirty");
        PackageFields.push("ChangeSetOp");
        PackageFields.push("EntityParentPM");

        var PackageHarmonizeFields: string[] = [];
        PackageHarmonizeFields.push("Id");
        PackageHarmonizeFields.push("Tenant");
        PackageHarmonizeFields.push("PackageId");
        PackageHarmonizeFields.push("Harmonize");
        PackageHarmonizeFields.push("IsDirty");
        PackageHarmonizeFields.push("ChangeSetOp");
        PackageHarmonizeFields.push("EntityParentPM");

        this.EntityPM.ShipmentPickUpDeliveryPackages.forEach((item: ShipmentPickUpDeliveryPackagePM) => {
            
            var oldItemPackage: ShipmentPickUpDeliveryPackagePM = new ShipmentPickUpDeliveryPackagePM(null);

            PackageFields.forEach((x: string) => {
                oldItemPackage[x] = item[x];
            });

            item.PickUpDeliveryPackageHarmonizes.forEach(itemHarmonize => {

                var oldItemPackageHarmonize: PickUpDeliveryPackageHarmonizePM = new PickUpDeliveryPackageHarmonizePM(null);

                PackageHarmonizeFields.forEach((r: string) => {
                    oldItemPackageHarmonize[r] = itemHarmonize[r];
                });

                oldItemPackage.PickUpDeliveryPackageHarmonizes.push(oldItemPackageHarmonize);
            });

            this.oldPackages.push(oldItemPackage);
        });
    }
    private RejectPackages() {
        this.EntityPM.ShipmentPickUpDeliveryPackages = [];

        this.oldPackages.forEach((item: ShipmentPickUpDeliveryPackagePM) => {
            this.EntityPM.ShipmentPickUpDeliveryPackages.push(item);
        });
    }

    private standaloneAction: string;
    StandAloneShipmentButtonClicked(buttonCode: string) {
        this.standaloneAction = buttonCode;

        if (buttonCode == "CreateStandalone") {
            this.ValidateNumberOfPickupPackages("Create");
            this.ValidateTypeOfPickUpPackages();
        }

        else if (buttonCode == "ConnectStandalone") {
            this.ValidateNumberOfPickupPackages("Connect");
            this.ConnctingStandaloneShipmentClicked();
        }

        this.DropdownClose();
    }
    ValidateNumberOfPickupPackages(actionType: string) {
        var numberOfAllowedPackages = 1;
        var errors: string[] = [];

        if (this.EntityPM.ShipmentPickUpDeliveryPackages.length > numberOfAllowedPackages) {
            errors.push("Can't " + actionType + " a Stand Alone Shipment Since Pickup has more than one Container");
        }

        this.ValidationErrorsList = errors;
    }

    ValidateTypeOfPickUpPackages() {
        var shipmentPickUpDeliveryPackage = this.EntityPM?.ShipmentPickUpDeliveryPackages?.find(d => d != null && d.ChangeSetOp != "3");
        if (shipmentPickUpDeliveryPackage == null) {
            this.CreateStandaloneShipmentClicked();
        }

        this.CompareCompatiblityOfPackgeAndShipmentTypes(shipmentPickUpDeliveryPackage?.PackageTypeId);
    }

    CompareCompatiblityOfPackgeAndShipmentTypes(packageTypeId: string) {
        if (AppTool.IsNullOrEmpty(packageTypeId))
            return;

        var isContainer = false;
        var packageTypePMService: PackageTypePMService = new PackageTypePMService();

        packageTypePMService.get(packageTypeId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                return;
            }

            var packageTypePM = myResponse.Result;
            if (packageTypePM) {
                isContainer = packageTypePM.IsContainer
                this.ValidateCompatiblityOfPackgeAndShipmentTypes(isContainer);
            }
        });
    }

    ValidateCompatiblityOfPackgeAndShipmentTypes(isContainer: boolean) {
        var isFCLShipment = AppTool.IsFCLEntity(this.ShipmentPM?.TransportModeId, this.ShipmentPM?.ShipmentTypeId);
        var isLCLShipment = AppTool.IsLCLEntity(this.ShipmentPM?.TransportModeId, this.ShipmentPM?.ShipmentTypeId);

        if (isLCLShipment && isContainer) {
            this.ValidationErrorsList.push("The container type is not compatible with the standalone shipment (LTL)");
        } else if (isFCLShipment && !isContainer) {
            this.ValidationErrorsList.push("The package type is not compatible with the standalone shipment (FTL)");
        } else {
            this.CreateStandaloneShipmentClicked();
        }
    }

    private isCreateStandaloneShipmentClicked: boolean = false;
    CreateStandaloneShipmentClicked() {
        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.IsDirty) {
                this.isCreateStandaloneShipmentClicked = true;
                this.Save(false);
            }

            else {
                this.ValidateStandaloneAddresses();
            }
        }
    }

    private isConnctingStandaloneShipmentClicked: boolean = false;
    ConnctingStandaloneShipmentClicked() {
        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.IsDirty) {
                this.isConnctingStandaloneShipmentClicked = true;
                this.Save(false);
            }

            else {
                this.ValidateStandaloneAddresses();
            }
        }
    }

    private ValidateStandaloneAddresses() {
        this.CurrentSession.StartBusyIndicator("");

        var service: ShipmentDomainService = new ShipmentDomainService();
        service.GetPickupDeliveryValidForInlandDomestic(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var isValid = myResponse.Result;
                if (isValid) {
                    if (this.standaloneAction == "CreateStandalone") {
                        this.CreateStandaloneShipment();
                    }

                    else {
                        this.ChooseStandAloneShipment();
                    }
                }

                else {
                    this.ValidationErrorsList.push("Both Addresses must be in the same country since the direction is Domestic");
                }
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private CreateStandaloneShipment() {
        var shipmentPM: ShipmentPM = ShipmentTool.BuildStansaloneShipment(null, this.EntityPM, this.ShipmentPM);

        var args = new NewShipmentComponentArgs();
        args.Shipment = shipmentPM;
        args.IsStandalone = true;
        args.ForwarderShipmentPickUpDeliveryTypeCode = "Pickup";
        var str: string = TextCodeTranslator.Translate("General.O.NewEntity");
        str = str.replace("%Entity", TextCodeTranslator.TranslateTable("Shipment"));

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = str;
        logWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');
    }    

    public ShipmentNumber: string = null;
    public ShipmentId: string = null;
    ChooseStandAloneShipment() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 570;
        logWindow.Title = "Shipments Search";
        var args: any = {};        
        args.ShipmentType = this.ShipmentPM?.ShipmentTypeId;
        args.FromType = this.EntityPM.PickUpDeliveryFromTypeCode;
        args.ToType = this.EntityPM.PickUpDeliveryToTypeCode;
        args.FromPartnerId = this.EntityPM.FromPartnerCardId;
        args.ToPartnerId = this.EntityPM.ToPartnerCardId;
        args.FromPortId = this.EntityPM.FromPortId;
        args.ToPortId = this.EntityPM.ToPortId;        
        args.FromCity = this.EntityPM.FromAddressCity;
        args.ToCity = this.EntityPM.ToAddressCity;
        args.FromCountryId = this.EntityPM.FromAddressCountryId;
        args.ToCountryId = this.EntityPM.ToAddressCountryId;
        args.CarrierId = this.EntityPM.CarrierId;
        args.NumberOfPackages = this.EntityPM.ShipmentPickUpDeliveryPackages != null ? this.EntityPM.ShipmentPickUpDeliveryPackages.length : 0;
        logWindow.WindowArgs = args;
        logWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/ChooseStandaloneShipmentComponent');
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                var shipmentList = s.SelectedShipment;
                if (shipmentList != null) {
                    this.EntityPM.StandaloneShipmentId = shipmentList.Id;
                    this.EntityPM.StandaloneShipmentNumber = shipmentList.ShipmentNumber;
                    this.Save(false);
                }
            });
        });
    }

    ViewStandaloneShipmentClicked() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.EntityPM.StandaloneShipmentId, ObjectTableName: 'Shipment', BackButtonLabel: "Shipment" + ": " + this.ShipmentPM.ShipmentNumber });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                });

            });
    }    

    dropdownDisplay: string = 'none';
    DropdowndisplayToggle() {

        if (this.dropdownDisplay == 'none') {
            this.dropdownDisplay = 'block';
        }
        else {
            this.dropdownDisplay = 'none';
        }
    }

    DropdownClose() {
        this.dropdownDisplay = 'none';
    }
}

class TabItem {
    public Code: string;
    public TextCode: string = null;
    public IsDisabled: boolean = false;
    constructor(myCode: string, myTextCode: string, isDisabled: boolean = false) {
        this.Code = myCode;
        this.TextCode = myTextCode;
        this.IsDisabled = isDisabled;
    }
}
