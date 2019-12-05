/// <reference path="../../../tools.ts" />
import {Component, OnInit, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {PortListService} from '../../../../Common/Services/StandardLists/PortListService';
import {AddressListService} from '../../../../Common/Services/StandardLists/AddressListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {AddressList} from '../../../../Common/EntityLists/AddressList';
import {WarehouseReleasePM} from '../../../EntityPMs/WarehouseReleasePM';
import {PortList} from '../../../../Common/EntityLists/PortList';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AddressPM} from '../../../../Common/EntityPMs/AddressPM';
import {CitySelectionArgs} from '../../../../Common/Args';
@Component({
    selector: 'WarehouseReleaseRoutingsTabComponent',
    moduleId: module.id,
    templateUrl: './WarehouseReleaseRoutingsTabComponent.html',
})

export class WarehouseReleaseRoutingsTabComponent extends BaseComponent {

    public EntityPM: WarehouseReleasePM;
    public ObjectTableName: string = null;
    public DataContext = this;

    public CardDependencyProperty1: string = "CS";
    public CardDependencyProperty1IsList: boolean = false;
  
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.Listen();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.CardDependencyProperty1 = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }
        this.Initialize();

    }

    ngOnInit() {
        if (this.EntityPM != null) {

            this.SetUIProperties();
            this.OnEditMoodScreen();
        }
    }



    SetUIProperties() {

        if (this.EntityPM.FromPortId) {
            this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, false);
        }
        this.UIProperties.SetEnabled("ToAddress", this.ObjectTableName, false);

    }





    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                }
            });
        }
    }


    private myPortListService: PortListService;
    private myAddressListService: AddressListService;
    private myCardListService: CardListService;
    Initialize() {
        this.myPortListService = new PortListService();
        this.myAddressListService = new AddressListService();
        this.myCardListService = new CardListService();
    }




    get ToTypeCode() { return this.EntityPM.ToTypeCode; }
    set ToTypeCode(value: string) {
        if (this.EntityPM.ToTypeCode != value) {
            this.EntityPM.ToTypeCode = value;

            this.EntityPM.ToPartnerCardId = null;
            this.EntityPM.ToAddressId = null;
            this.EntityPM.ToPortId = null;
            this.EntityPM.ToAddressCity = null;
            this.EntityPM.ToAddressZipCode = null;
            this.EntityPM.ToAddressCountryId = null;
            this.ToAddress = null;
            this.ToAddressList = null;
            this.SetUIProperties_To();
        }
    }


    get ToPartnerCardId() { return this.EntityPM.ToPartnerCardId; }
    set ToPartnerCardId(value: string) {
        if (this.EntityPM.ToPartnerCardId != value) {
            this.EntityPM.ToPartnerCardId = value;
            this.SetUIProperties_To();

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToAddressId = null;
            }

            else {
                this.myCardListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            if (!AppTool.IsNullOrEmpty(list.PickAddressId)) {
                                this.ToAddressId = list.PickAddressId;
                            }

                            else {
                                this.ToAddressId = list.MainAddressId;
                            }
                        }
                    }
                });
            }
        }
    }

    get ToAddressId() { return this.EntityPM.ToAddressId; }
    set ToAddressId(value: string) {
        if (this.EntityPM.ToAddressId != value) {
            this.EntityPM.ToAddressId = value;

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToAddressList = null;
               // this.EntityPM.ToAddressCity_Dummy = null;
               // this.EntityPM.ToAddressCountryCode = null;
               // this.EntityPM.ToAddressCountryName = null;
            }

            else {
                this.myAddressListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: AddressList = myResponse.Result;
                        if (list) {
                            this.ToAddressList = list;
                           // this.EntityPM.ToAddressCity_Dummy = list.City;
                           // this.EntityPM.ToAddressCountryCode = list.CountryCode;
                          //  this.EntityPM.ToAddressCountryName = list.CountryName;
                        }
                    }
                });
            }
        }
    }

    private toAddressList: AddressList;
    get ToAddressList() { return this.toAddressList; }
    set ToAddressList(newValue: AddressList) {
        this.toAddressList = newValue;
    }

    get ToPortId() { return this.EntityPM.ToPortId; }
    set ToPortId(value: string) {
        if (this.EntityPM.ToPortId != value) {
            this.EntityPM.ToPortId = value;
            this.SetUIProperties_To();

            if (AppTool.IsNullOrEmpty(value)) {
                this.ToAddress = null;
                ////this.EntityPM.ToPortCode = null;
                ////this.EntityPM.ToPortName = null;
                ////this.EntityPM.ToPortCountryCode = null;
                ////this.EntityPM.ToPortCountryName = null;
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        if (list) {
                            this.ToAddress = "Port Of: " + list.EnglishName;
                            //this.EntityPM.ToPortCode = list.Code;
                            //this.EntityPM.ToPortName = list.EnglishName;
                            //this.EntityPM.ToPortCountryCode = list.CountryCode;
                            //this.EntityPM.ToPortCountryName = list.CountryName;
                        }
                    }
                });
            }
        }
    }


    get ToAddressCity() { return this.EntityPM.ToAddressCity; }
    set ToAddressCity(value: string) {
        if (this.EntityPM.ToAddressCity != value) {
            this.EntityPM.ToAddressCity = value;
          //  this.EntityPM.ToAddressCity_Dummy = value;
            this.SetUIProperties_To();
        }
    }

    get ToAddressZipCode() { return this.EntityPM.ToAddressZipCode; }
    set ToAddressZipCode(value: string) {
        if (this.EntityPM.ToAddressZipCode != value) {
            this.EntityPM.ToAddressZipCode = value;
            this.SetUIProperties_To();
        }
    }

    get ToAddressCountryId() { return this.EntityPM.ToAddressCountryId; }
    set ToAddressCountryId(value: string) {
        if (this.EntityPM.ToAddressCountryId != value) {
            this.EntityPM.ToAddressCountryId = value;
            this.SetUIProperties_To();

            //if (AppTool.IsNullOrEmpty(value)) {
            //    this.EntityPM.ToAddressCountryCode = null;
               // this.EntityPM.ToAddressCountryName = null;
            //}

            //else {
            //    this.myCountryListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
            //        if (!myResponse.HasError) {
            //            var list: PortList = myResponse.Result;
            //            if (list) {
            //                this.EntityPM.ToAddressCountryCode = list.Code;
            //                this.EntityPM.ToAddressCountryName = list.EnglishName;
            //            }
            //        }
            //    });
            //}
        }
    }


    SelectCityCommand(myAddressCode: string) {

        var mySourceCountryId: string = this.ToAddressCountryId;

        var args = new CitySelectionArgs(mySourceCountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {

                this.ToAddressCity = args.CityName;
                this.ToAddressCountryId = args.CountryId;
             
            }
        });
    }

    SetUIProperties_To() {
        switch (this.ToTypeCode) {
            case "PART": {
                this.UIProperties.SetRequired("ToPartnerCardId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPartnerCardId) ? true : false);

                var isAddressIdEnabled = false;
                    if (!AppTool.IsNullOrEmpty(this.ToPartnerCardId)) {
                        isAddressIdEnabled = true;
                    }
                

                this.UIProperties.SetEnabled("ToAddressId", this.ObjectTableName, isAddressIdEnabled);
            }

            case "PORT": {
                this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToPortId) ? true : false);
            }

            case "CASL": {
                this.UIProperties.SetRequired("ToAddressCity", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToAddressCity) && AppTool.IsNullOrEmpty(this.ToAddressZipCode) ? true : false);
                this.UIProperties.SetRequired("ToAddressCountryId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ToAddressCountryId) ? true : false);
            }
        }
    }


    public FromPortList: PortList = null;
    get FromPortId() { return this.EntityPM.FromPortId; }
    set FromPortId(value: string) {
        if (this.EntityPM.FromPortId != value) {
            this.EntityPM.FromPortId = value;
            this.SetUIProperties_Ports();
            //if (AppTool.IsNullOrEmpty(value)) {
            //    this.FromPortList = null;
            //}
            //else {
            //    this.myPortListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
            //        if (!myResponse.HasError) {
            //            this.FromPortList = myResponse.Result;
            //        }
            //    });
            //}
        }
    }


    OnEditMoodScreen() {

        if (!AppTool.IsNullOrEmpty(this.ToAddressId)) {
            this.myAddressListService.getSingle(this.ToAddressId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.ToAddressList = myResponse.Result;
                }
            });
        }

        if (!AppTool.IsNullOrEmpty(this.ToPortId)) {
            this.myPortListService.getSingleFromCache(this.ToPortId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    var list: PortList = myResponse.Result;
                    if (list) {
                        this.ToAddress = "Port Of: " + list.EnglishName;
                    }
                }
            });
        }

    }





    private toAddress: string = "";
    get ToAddress() { return this.toAddress; }
    set ToAddress(value: string) {
        if (this.toAddress != value) {
            this.toAddress = value;
        }
    }



    get TransportModeId() { return this.EntityPM.TransportModeId; }
    set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue;
        }
    }

    SetUIProperties_Ports() {
        var isFromRequired: boolean = false;
        var isToRequired: boolean = false;

        var isShipperIdRequired: boolean = false;
        var isConsigneeIdRequired: boolean = false;
        var isCustomerIdRequired: boolean = false;

        this.UIProperties.SetRequired("FromPortId", this.ObjectTableName, isFromRequired);
        this.UIProperties.SetRequired("ToPortId", this.ObjectTableName, isToRequired);
    }





  
}
