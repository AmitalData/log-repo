import { Component, AfterViewInit, ViewChildren, QueryList, OnDestroy} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ShipmentValidator} from '../../../../Shipment/Validators/ShipmentValidator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentDeliveryPM} from '../../../../Shipment/EntityPMs/ShipmentDeliveryPM';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ShipmentPMService} from '../../../../Shipment/Services/StandardPMs/ShipmentPMService';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import { ShipmentDeliveryValidator } from '../../../../Shipment/Validators/ShipmentDeliveryValidator';
import { ShipmentTool } from '../../../../Shipment/Tools';
import { PartnersDomainService } from 'Common/Services/PartnersDomainService';
import { DeclarationExtendedListService } from 'Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { DeliverySiteTypeList } from 'Customs/EntityLists/DeliverySiteTypeList';
import { PackingTypeList } from 'Customs/EntityLists/PackingTypeList';
import { DeliverySiteTypeListService } from 'Customs/Services/StandardLists/DeliverySiteTypeListService';
import { PackingTypeListService } from 'Customs/Services/StandardLists/PackingTypeListService';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryCityListService } from 'Common/Services/StandardLists/CountryCityListService';
import { TruckerSettingExtendedService } from 'Common/Services/ExtendedLists/TruckerSettingExtendedService';

@Component({    
    templateUrl: './AddEditInlandTransportComponent.html',
})

export class AddEditInlandTransportComponent implements AfterViewInit, OnDestroy {
  public SelectedTab: any;
    public EntityPM: ShipmentDeliveryPM;
    myCardListService: CardListService;
    _partnersDomainService: PartnersDomainService = new PartnersDomainService();
    _declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _deliverySiteTypeListService: DeliverySiteTypeListService = new DeliverySiteTypeListService();
    _packingTypeListService: PackingTypeListService = new PackingTypeListService();
    _countryCityService: CountryCityListService = new CountryCityListService();
    _truckerSettingExtendedService: TruckerSettingExtendedService = new TruckerSettingExtendedService();
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "ShipmentPickUpDelivery";
    public IsNewEntity: boolean = false;
    public TabsItemsSource: TabItem[] = [];
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    IsShipmentEditComponent: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsEditingEnabled: boolean = true;
    public IsAddEditEmptyCR: boolean = true 
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    constructor(private entityResourceService: EntityResourceService) {
        this.myCardListService = new CardListService();
    }

    ReloadData() {
    }

    SavedEntityId: string;
    SavedEntityNumber: string;
    SetWindowArgs(args: any) {
        this.IsNewEntity = args['IsNewEntity'];
        this.ShipmentPM = args['Shipment'];

        if (this.IsNewEntity) {
            this.CreateNewInlandTransport();
        }
        else {
            this.EntityPM = args['InlandTransport'];
        }
        this.EntityPM.EntityParentPM = this.ShipmentPM;
        this.SavedEntityId = this.EntityPM.Id;
        this.SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;

        this.Clone();
        this.ReloadData();

        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.ShipmentPM);

        this.entityResourceService.getEntityResourceByTableName("ShipmentPickUpDelivery").subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.LoadTemplate();
            this.Listen();
        });
    }

    CreateNewInlandTransport() {

        this.EntityPM = new ShipmentDeliveryPM(this.ShipmentPM);
        this.EntityPM.Tenant = this.ShipmentPM.Tenant;
        this.EntityPM.ShipmentId = this.ShipmentPM.Id;
        this.EntityPM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
        this.EntityPM.PickUpDeliveryTypeCode = "DELV";
        this.EntityPM.PickUpDeliveryFromTypeCode = "PORT";
        this.EntityPM.PickUpDeliveryToTypeCode = "PART"; // CASL
        this.EntityPM.TransportModeCode = "BYTR";
        this.EntityPM.ToPartnerCardId = this.ShipmentPM.CustomerId;
        this.EntityPM.Quantity = this.ShipmentPM.NumberOfPackages;
        this.EntityPM.GrossWeight = this.ShipmentPM.GrossWeight;
        this.EntityPM.Volume = this.ShipmentPM.Volume;
        this.EntityPM.DescriptionOfGoods = this.ShipmentPM.DescriptionOfGoods;
        this.EntityPM.Commodity = this.ShipmentPM.Commodity;
        this.EntityPM.ToPartnerCardId = this.ShipmentPM.CustomerId;

        // get client pickup addresses to set values by default
        this.clientPickUpAddressSet = false;
        this.toAddressCityIdSet = false;
        this._partnersDomainService.GetAddressByCardAndType(this.EntityPM.ToPartnerCardId, "P").subscribe((address: AddressList) => {
            this.ClientPickUpAddress = address;

            if (this.ClientPickUpAddress) {
                this.SetToAddressFields(this.ClientPickUpAddress);
    
                this.EntityPM.Notes = this.ClientPickUpAddress.TransportationInstructions;
            }
            else {
                this._partnersDomainService.GetAddressByCardAndType(this.EntityPM.ToPartnerCardId, "M").subscribe((clientMainAddress: AddressList) => {
                    if (clientMainAddress) {
                        this.SetToAddressFields(clientMainAddress);
                    }
                });
            }
        });

        // set FromAddressCityId by default
        this.fromAddressCityIdSet = false;
        this._declarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.ShipmentPM.ShipmentNumber).subscribe((consignmentResponse: ServiceResponse) => {
            if (consignmentResponse.Result?.length > 0 && consignmentResponse.Result[0].StorageSiteCode) {
                this._deliverySiteTypeListService.getSingle(consignmentResponse.Result[0].StorageSiteCode).subscribe((response: ServiceResponse) => {
                    if (response?.Result) {
                        const deliverySiteType: DeliverySiteTypeList = response.Result;
                        this.EntityPM.FromAddressCityId = deliverySiteType.CityId;

                        if (this.EntityPM.FromAddressCityId) {
                            this._countryCityService.getSingleFromCache(this.EntityPM.FromAddressCityId).subscribe((myResponse: ServiceResponse) => {
                                if (!myResponse.HasError) {
                                    var countryCity: CountryCityList = myResponse.Result;
                                    if (countryCity != null) {
                                        this.EntityPM.FromAddressCity = countryCity.LocalName;
                                    }
                                }
                            });
                        }
                    }
                    this.fromAddressCityIdSet = true;
                    this.SetCarrierIdByDefault();
                });
            }
            else {
                this.fromAddressCityIdSet = true;
                this.SetCarrierIdByDefault();
            }
        });

        // set PackageTypeCode by default
        if (this.ShipmentPM.PackageTypeCode)
        {
            this._packingTypeListService.getSingleFromCache(this.ShipmentPM.PackageTypeCode).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var packingTypeList: PackingTypeList = myResponse.Result;
                    if (packingTypeList != null) {
                        this.EntityPM.PackageTypeCode = packingTypeList.PackageTypeId;
                    }
                }
            });
        }
    }

    SetToAddressFields(address: AddressList) {
        this.EntityPM.ToAddressId = address?.Id ;
        this.EntityPM.ToAddressCity = address.City;
        this.EntityPM.ToAddressCityId = address.CityId;
        this.EntityPM.ToAddress = (address.Address1 || "") + "\r" + 
                                    (address.Address2 || "") + "\r" + 
                                    (address.City || "") + "\r" + 
                                    (address.CountryName || "");

        this.toAddressCityIdSet = true;
        this.SetCarrierIdByDefault();
    }

    SetCarrierIdByDefault() {
        if (this.toAddressCityIdSet && this.fromAddressCityIdSet && this.clientPickUpAddressSet) {
            this._truckerSettingExtendedService.getTruckerSettingForDefaultShipmentDelivery(this.EntityPM.FromAddressCityId, this.EntityPM.ToAddressCityId, this.ShipmentPM.ShipmentTypeId).subscribe((myResult: any) => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError && myResponse?.Result?.length > 0) {
                    const truckerSetting = myResponse.Result[0];
                    this.EntityPM.ResponsibilityCode = truckerSetting.Responsibility;
                    this.EntityPM.CarrierId = truckerSetting.TruckerId;
                }
                else {
                    this.EntityPM.ResponsibilityCode = this.ClientPickUpAddress?.Responsibility;
                    this.EntityPM.CarrierId = this.ClientPickUpAddress?.TruckerId;
                }
            });
        }
    }


    AddNewInlandTransport() {
        var args: any = {
            Shipment: this.ShipmentPM,
            IsNewEntity: true
        };
        this.SetWindowArgs(args);

        this.ResetEntityPM();
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
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {

                    case "MAIN": {
                        if (this.PageChild_MAIN == null) {
                            myLocation.viewContainerRef.clear();

                            SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentTabs/Components/InlandTransport/InlandTransportTabs/InlandTransportMainTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_MAIN = cmpRef.instance;
                                    this.PageChild_MAIN.InitTab(this.EntityPM, this.ShipmentPM);
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
            confirmWindow.Show(TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", "הובלה"));
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
        // var isValid = this.Validate();

        // if (isValid) {
            this.SavedEntityId = this.EntityPM.Id;
            this.SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;

            this.ContinueSaving(isClosingWindow);
        // }
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
    ContinueSaving(isClosingWindow: boolean) {
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
            if (!this.IsShipmentEditComponent) {

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

    OnSaveCompleted(isSaveSuccess: boolean, isClosingWindow: boolean) {
        if (isSaveSuccess) {

            this.EntityPM.IsDirty = false;

            if (this.IsNewEntity) {
                // display the delivery data in the screen
                let ShipmentDeliveries = this.CurrentSession.CurrentEditComponent.EntityPM.ShipmentDeliveries;
                ShipmentDeliveries.sort((a, b) => new Date(b.CreateDate).getTime() - new Date(a.CreateDate).getTime());
                this.EntityPM = ShipmentDeliveries[0];
                this.SavedEntityNumber = this.EntityPM.PickUpDeliveryNumber;

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
                // code modified due to refresh dates issue
                //this.PageChild_MAIN.InitTab(this.EntityPM, this.ShipmentPM);

                this.PageChild_MAIN = null;

                if (this.SelectedTabCode == "MAIN") {
                    this.SelectionChanged();
                }
            }

            this.Clone();
        }
    }

    private fromAddressCityIdSet: boolean;
    private toAddressCityIdSet: boolean;
    private clientPickUpAddressSet: boolean;
    private clientPickUpAddress: AddressList;

    get ClientPickUpAddress() { return this.clientPickUpAddress; }
    set ClientPickUpAddress(value: AddressList) {
        if (this.clientPickUpAddress != value) {
            this.clientPickUpAddress = value;
        }
        this.clientPickUpAddressSet = true;
        this.SetCarrierIdByDefault();
    }

    private myCloner: Cloner;
    public isEntityAdded: boolean = false;
    private Clone() {

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
        this.myCloner.AddField('DeliveryContact');
        this.myCloner.AddField('PackageTypeCode');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('GrossWeight');
        this.myCloner.AddField('Volume');
        this.myCloner.AddField('CustomerChargeableWeight');
        this.myCloner.AddField('TruckerChargeableWeight');
        this.myCloner.AddField('DescriptionOfGoods');
        this.myCloner.AddField('Commodity');
        this.myCloner.AddField('ToAddressCityId');
        this.myCloner.AddField('ResponsibilityCode');
        this.myCloner.AddField('Responsibility');
        this.myCloner.AddField('PackageTypeName');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.ShipmentPM);
    }
    private RejectChanges() {
        if (this.EntityPM.IsDirty) {

            if (this.isEntityAdded) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    this.ShipmentPM.RemoveDelivery(this.EntityPM);
                }
            }

            this.myCloner.RejectChanges();
        }
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
