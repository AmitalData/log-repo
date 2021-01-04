import { Component, AfterViewInit, ViewChildren, QueryList, OnDestroy} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentValidator} from '../../../../Shipment/Validators/ShipmentValidator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentDeliveryPM} from '../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import { ShipmentFollowUpPM } from '../../../../Shipment/EntityPMs/ShipmentFollowUpPM';
import { ShipmentPickUpDeliveryPackagePM } from '../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM';
import { PickUpDeliveryPackageHarmonizePM } from '../../../../Shipment/EntityPMs/PickUpDeliveryPackageHarmonizePM';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import { ShipmentDeliveryValidator } from '../../../../Shipment/Validators/ShipmentDeliveryValidator';
import { WarehouseReleaseListExtendedService } from '../../../../Warehouse/Services/ExtendedLists/WarehouseReleaseListExtendedService';

@Component({
    
    templateUrl: './AddEditDeliveryComponent.html',
})

export class AddEditDeliveryComponent implements AfterViewInit, OnDestroy {
  public SelectedTab: any;

    public EntityPM: ShipmentDeliveryPM;
    myCardListService: CardListService;
    warehouseReleaseListExtendedService: WarehouseReleaseListExtendedService;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "ShipmentPickUpDelivery";
    public IsNewEntity: boolean = false;
    public TabsItemsSource: TabItem[] = [];
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    public IsShowNewWarehouseReleaseButton: boolean = false;
    public IsContainerFollowup: boolean = false;
    public ContainerReturnDeliveryId: string = null;
    public IsCreatingContainerDelivery: boolean = false;
    public DisableNewWarehouseReleaseButton: boolean = true;
    IsShipmentEditComponent: boolean = true;
    WareHouseRelaseCustomerId: string;
    WareHouseRelaseWareHouseId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private entityResourceService: EntityResourceService) {
        this.myCardListService = new CardListService();
        this.warehouseReleaseListExtendedService = new WarehouseReleaseListExtendedService();
    }

    ReloadData() {
        this.DisableNewWarehouseReleaseButton = true;
        if (this.EntityPM && this.ShipmentPM && this.ShipmentPM.DirectionId == 'I') {
            this.warehouseReleaseListExtendedService.GetNumberOfConnectedWarehouseReleasesByChildEntityReference(this.EntityPM.PickUpDeliveryNumber).subscribe((serviceResponse: any) => {
                if (serviceResponse.Result == 0) this.DisableNewWarehouseReleaseButton = false;
            });
            if (this.ShipmentPM.IsCFSWarehouse) {
                this.warehouseReleaseListExtendedService.getActiveWarehouseReleaseListsByShipmentId(this.ShipmentPM.Id, this.ShipmentPM.Tenant).subscribe((serviceResponse: ServiceResponse) => {
                    var warehouseRelease = serviceResponse.Result;
                    if (warehouseRelease && warehouseRelease.length > 0) {
                        this.DisableNewWarehouseReleaseButton = true;
                    }
                });
            }
        }
        else this.DisableNewWarehouseReleaseButton = false;
    }

    SavedEntityId: string;
    SavedEntityNumber: string;
    SetWindowArgs(args: any) {
        this.IsNewEntity = args['IsNewEntity'];
        this.ShipmentPM = args['ShipmentPM'];
        this.EntityPM = args['EntityPM'];
        this.IsContainerFollowup = args['IsContainerFollowup'];
        this.ContainerReturnDeliveryId = args['ContainerReturnDeliveryId'];
        this.WareHouseRelaseCustomerId = args['WareHouseRelaseCustomerId'];
        this.WareHouseRelaseWareHouseId = args['WareHouseRelaseWareHouseId'];
        this.IsCreatingContainerDelivery = args["IsCreatingContainerDelivery"];
        var isOutSource = args['IsOutSource'];
        if (isOutSource) this.IsShipmentEditComponent = false;

        this.SavedEntityId = this.EntityPM.Id;
        this.SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;

        if (!this.EntityPM.TransportModeCode) {
            this.EntityPM.TransportModeCode = "BYTR";
        }

        this.Clone();
        this.ReloadData();

        if (this.ShipmentPM) {
            if (this.ShipmentPM.ShipmentLevelCode == "D" || this.ShipmentPM.ShipmentLevelCode == "H") {
                if (FeatureLocator.HasFeaturePermession("WarehouseRelease", "Module")) {
                    this.IsShowNewWarehouseReleaseButton = this.ShipmentPM.DirectionId == "I" ? true : false;
                }
            }
        }

        this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDelivery").subscribe((res: any) => {
            this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDeliveryPackage").subscribe((res2: any) => {
                this.IsResourcesReady = true;
                this.LoadTemplate();
                this.Listen();
            });
        });
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
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
    }

    Listen() {
        if (this.CurrentSession.CurrentEditComponent) {
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

    NewWarehouseReleaseButtonClicked() {

        if (this.ShipmentPM.IsDirty) {
            var errors: any[] = [];
            var myShipmentValidator = new ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.ShipmentPM);
            if (myShipmentErrors.length > 0) {
                errors.push("This shipment has validation errors you cant proceed adding cross dock release!");
            }

            if (errors.length == 0) {
                if (this.CurrentSession.CurrentEditComponent != null && this.IsShipmentEditComponent) {
                    if (!this.SaveCompletedEvent) {
                        this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                            if (isSaveSuccess) {
                                this.ShipmentPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                                this.Clone();
                                this.InitializeWareHousReleaseWindow();

                            } else {
                                this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
                            }

                            AppTool.KillEventEmitter(this.SaveCompletedEvent);
                            this.SaveCompletedEvent = null;
                        });
                    }

                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                }

                else if (this.IsContainerFollowup || !this.IsShipmentEditComponent) {

                    this.CurrentSession.StartBusyIndicatorSaving();

                    var entityPMService = new ShipmentPMService();
                    entityPMService.update(this.ShipmentPM).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            this.ShipmentPM = myResponse.Result;
                            this.Clone();
                            this.InitializeWareHousReleaseWindow();
                        }
                    });
                }
                
                else {
                    this.InitializeWareHousReleaseWindow();
                }
             
            }
            else {
                this.ValidationErrorsList = errors;
            }

        } else this.InitializeWareHousReleaseWindow();

    }

    InitializeWareHousReleaseWindow() {
        if (this.EntityPM.PickUpDeliveryFromTypeCode == "PART" && !AppTool.IsNullOrEmpty(this.EntityPM.FromPartnerCardId)) {

            this.myCardListService.getSingle(this.EntityPM.FromPartnerCardId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var myCardList: CardList = myResponse.Result;
                    if (myCardList && myCardList.PartnerTypeId == "WH") {
                        this.OpenWareHousReleaseWindow(myCardList.Id);

                    } else this.OpenWareHousReleaseWindow("");
                } else this.OpenWareHousReleaseWindow("");
            });


        } else {
            this.OpenWareHousReleaseWindow("");
        }
    }

    OpenWareHousReleaseWindow(warehouseId: string) {

        var windowArgs: any = {};
        windowArgs.WarehouseId = warehouseId; 
        windowArgs.ExpectedReleaseDate = this.EntityPM.ETA;
        windowArgs.ActualReleaseDate = this.EntityPM.ATA;
        windowArgs.ShipmentPM = this.ShipmentPM;
        windowArgs.ConnectedTo = "Delivery";
        windowArgs.ChildEntityReference = this.EntityPM.PickUpDeliveryNumber;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 620;
        logWindow.Title = "New Cross Dock Release";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Warehouse/Components/NewWarehouseReleaseComponent");
        logWindow.WindowClosed.subscribe((event: any) => {
            this.ReloadData();
        });

        //logWindow.WindowClosed.subscribe((event: any) => {
        //    if (event == "Refresh")
        //        this.ShipmentPM.IsDirty = true;
        //        this.CurrentSession.CurrentEditComponent.SaveChanges();
        //});

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
                            SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryMainTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_MAIN = cmpRef.instance;
                                    this.PageChild_MAIN.InitTab(this.EntityPM, this.ShipmentPM);

                                    if (this.IsNewEntity) {

                                        if (this.EntityPM.PickUpDeliveryTypeCode == "EMPT") {
                                            this.PageChild_MAIN.FromTypeCode = "PART";
                                            this.PageChild_MAIN.ToTypeCode = "PART";

                                            if (!AppTool.IsNullOrEmpty(this.ContainerReturnDeliveryId)) {
                                                var myDelivery = this.ShipmentPM.ShipmentDeliveries.filter(f => f.Id == this.ContainerReturnDeliveryId)[0];
                                            }

                                            if (myDelivery) {
                                                this.PageChild_MAIN.FromTypeCode = myDelivery.PickUpDeliveryToTypeCode;

                                                switch (myDelivery.PickUpDeliveryToTypeCode) {
                                                    case "PART": {
                                                        this.PageChild_MAIN.EntityPM.FromPartnerCardId = myDelivery.ToPartnerCardId;
                                                        this.PageChild_MAIN.SetUIProperties_From();
                                                        this.PageChild_MAIN.FromAddressId = myDelivery.ToAddressId;
                                                        break;
                                                    }

                                                    case "PORT": {
                                                        this.PageChild_MAIN.FromPortId = myDelivery.ToPortId;
                                                        break;
                                                    }

                                                    default: {
                                                        this.PageChild_MAIN.FromAddressCity = myDelivery.ToAddressCity;
                                                        this.PageChild_MAIN.FromAddressZipCode = myDelivery.ToAddressZipCode;
                                                        this.PageChild_MAIN.FromAddressCountryId = myDelivery.ToAddressCountryId;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        else {
                                          
                                            if (!AppTool.IsNullOrEmpty(this.WareHouseRelaseWareHouseId)) {
                                                this.PageChild_MAIN.FromTypeCode = "PART";
                                                this.PageChild_MAIN.FromPartnerCardId = this.WareHouseRelaseWareHouseId;
                                            }

                                            // Task 47686: Export& Domestic Terminal: Delivery From

                                            //else if (!AppTool.IsNullOrEmpty(this.ShipmentPM.WarehouseLegWarehouseId)) {
                                            //    this.PageChild_MAIN.FromTypeCode = "PART";
                                            //    this.PageChild_MAIN.FromPartnerCardId = this.ShipmentPM.WarehouseLegWarehouseId;
                                            //}

                                            else {
                                                var fromPortId = this.ShipmentPM.MainCarriageToPortId;

                                                if (!AppTool.IsNullOrEmpty(this.ShipmentPM.OnCarriageToPortId)) {
                                                    fromPortId = this.ShipmentPM.OnCarriageToPortId;
                                                }

                                                else if (!AppTool.IsNullOrEmpty(this.ShipmentPM.Transshipment3ToPortId)) {
                                                    fromPortId = this.ShipmentPM.Transshipment3ToPortId;
                                                }

                                                else if (!AppTool.IsNullOrEmpty(this.ShipmentPM.Transshipment2ToPortId)) {
                                                    fromPortId = this.ShipmentPM.Transshipment2ToPortId;
                                                }

                                                else if (!AppTool.IsNullOrEmpty(this.ShipmentPM.Transshipment1ToPortId)) {
                                                    fromPortId = this.ShipmentPM.Transshipment1ToPortId;
                                                }

                                                this.PageChild_MAIN.FromTypeCode = "PORT";
                                                this.PageChild_MAIN.FromPortId = fromPortId;
                                            }

                                            this.PageChild_MAIN.ToTypeCode = "PART";
                                            this.PageChild_MAIN.ToPartnerCardId = !AppTool.IsNullOrEmpty(this.WareHouseRelaseCustomerId) ? this.WareHouseRelaseCustomerId : this.ShipmentPM.ConsigneeId;

                                        }
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
                                SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryPackagesTabComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.PageChild_PACG = cmpRef.instance;
                                        this.PageChild_PACG.InitTab(this.EntityPM, this.ShipmentPM, this);
                                    });
                            });
                        }

                        break;
                    }

                    case "DCSO": {
                        if (this.PageChild_DCSO == null) {
                            SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryDocsOutTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_DCSO = cmpRef.instance;
                                    this.PageChild_DCSO.InitTab(this.EntityPM, this.ShipmentPM);
                                });
                        }

                        break;
                    }

                    case "DCSI": {
                        if (this.PageChild_DCSI == null) {
                            SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentRouting/Components/Routings/DeliveryTabs/DeliveryDocsInTabComponent', myLocation.viewContainerRef)
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
            confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", "Delivery"));
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
    Save(isClosingWindow: boolean) {

        var isValid = this.Validate();

        if (isValid) {

            this.SavedEntityId = this.EntityPM.Id;
            this.SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;

            if (this.IsNewEntity) {
                ServiceLocator.SendTotangoUserActivity("Container F/U", "Added Delivery");
                this.ShipmentPM.AddDelivery(this.EntityPM);
                this.isEntityAdded = true;
            }

            if (this.CurrentSession.CurrentEditComponent != null && this.IsShipmentEditComponent) {
                if (!this.SaveCompletedEvent) {

                    this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        this.OnSaveCompleted(isSaveSuccess, isClosingWindow);
                    });

                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                }
            }

            else {
                if (this.IsContainerFollowup || !this.IsShipmentEditComponent) {

                    this.CurrentSession.StartBusyIndicatorSaving();

                    var entityPMService = new ShipmentPMService();
                    entityPMService.update(this.ShipmentPM).subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            this.CurrentSession.CloseCurrentWindowEmit(myResponse.Result);
                        }
                    });
                }
            }
        }
    }

    Validate() {
        var validator = new ShipmentDeliveryValidator();
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

    OnSaveCompleted(isSaveSuccess: boolean, isClosingWindow: boolean) {
        if (isSaveSuccess) {

            if (this.IsNewEntity) {
                ServiceLocator.SendTotangoUserActivity("Shipment", "DeliveryOpen");
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
        }

        else {
            this.ValidationErrorsList = this.CurrentSession.CurrentEditComponent.ValidationErrorsList;
        }

        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        this.SaveCompletedEvent = null;
    }

    ResetEntityPM() {
        if (this.CurrentSession.CurrentEditComponent) {
            this.ShipmentPM = this.CurrentSession.CurrentEditComponent.EntityPM;

            if (this.SavedEntityId) {
                this.EntityPM = this.ShipmentPM.ShipmentDeliveries.filter(f => f.Id == this.SavedEntityId)[0];
            }

            else if (this.SavedEntityNumber) {
                this.EntityPM = this.ShipmentPM.ShipmentDeliveries.filter(f => f.PickUpDeliveryNumber == this.SavedEntityNumber)[0];

                if (this.EntityPM) {
                    this.SavedEntityId = this.EntityPM.Id;
                }
            }

            if (this.PageChild_MAIN) {
                this.PageChild_MAIN.InitTab(this.EntityPM, this.ShipmentPM);
            }

            if (this.PageChild_PACG) {
                this.PageChild_PACG.InitTab(this.EntityPM, this.ShipmentPM, this);
            }

            if (this.PageChild_DCSO) {
                this.PageChild_DCSO.InitTab(this.EntityPM, this.ShipmentPM);
            }

            if (this.PageChild_DCSI) {
                this.PageChild_DCSI.InitTab(this.EntityPM, this.ShipmentPM);
            }

            this.Clone();
        }
    }

    private myCloner: Cloner;
    private oldFollowups: ShipmentFollowUpPM[] = [];
    public isEntityAdded: boolean = false;
    private Clone() {

        this.ClonePackages();

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
            oldItem.Note = item.Note;
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
        this.myCloner.AddField('EmptyDeliveryContainerPartnerId');
        this.myCloner.AddField('EmptyDeliveryDepotReference');
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
                    this.ShipmentPM.RemoveDelivery(this.EntityPM);
                }
            }

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
