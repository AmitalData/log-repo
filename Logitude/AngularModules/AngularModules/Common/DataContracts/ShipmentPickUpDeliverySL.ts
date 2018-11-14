
import { PartnerSL } from './PartnerSL';
import { PortSL } from './PortSL';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool} from '../../Infrastructure/Tools';
import {CardListService} from '../../Common/Services/StandardLists/CardListService';
import {EntityListService} from '../../Infrastructure/Services/EntityListService';
import { SessionInfo } from '../../Infrastructure/Utilities/SessionInfo';
import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';
export class ShipmentPickUpDeliverySL extends BaseComponent {

    private myCardListService: CardListService;
    private myEntityListService: EntityListService;

    
    public PickUpDeliveryTypeCode: string;
    public PickUpDeliveryToTypeCode: string;
    public PickUpDeliveryFromTypeCode: string;
    public FromAddressCity_Dummy: string;


    public FromAddressCity: string;
    public FromAddressZipCode: string;
    //public FromAddressCountryId: string;
    public FromAddressCountryCode: string;
    public FromAddressCountryName: string;
    public ToAddressZipCode: string;
    public ToAddressCity_Dummy: string;
    public ToAddressCity: string;


    public ToAddressCountryCode: string;
    public ToAddressCountryName: string;

    public FromPort: PortSL;
    public ToPort: PortSL;

    public FromPortId: string;
    public ToPortId: string;
    public FromPartner: PartnerSL;
    public ToPartner: PartnerSL;


  //  public FromPartnerCardId: string;
    public FromPartnerCardCode: string;
    public ToPartnerCardCode: string;

    public MainCarriageETD: Date;
    public MainCarriageATD: Date;
    public MainCarriageETA: Date;
    public MainCarriageATA: Date;


    public TransportModeCode: string;
    public TransportModeName: string;

    TransportModeLists: ShipmentPickUpDeliveryTransportMode[];
    SelectedTransportMode: ShipmentPickUpDeliveryTransportMode;
    
    ToPartnerDefaultValues: string;
    FromPartnerDefaultValues: string;




    private fromAddressCountryId: string;
    get FromAddressCountryId() { return this.fromAddressCountryId; }
    set FromAddressCountryId(newValue: string) {
        if (this.fromAddressCountryId != newValue) {
            this.fromAddressCountryId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.FromAddressCountryCode = null;
                this.FromAddressCountryName = null;
            }

            else {
                var filters = new ApiQueryFilters();
                filters.Tenant = SessionInfo.LoggedUserTenant;
                filters.GetAll = true;
                filters.ForceCacheRefresh = true;
      
                this.myEntityListService.getAllFromCache("Country", filters).then((res: any) => {
                    res.subscribe((myResponse: any) => {
                        if (!myResponse.HasError && myResponse.Result) {
                            var country = myResponse.Result.filter(d => d.Id == this.fromAddressCountryId )[0];
                            if (country) {
                                this.FromAddressCountryCode = country.Code;
                                this.FromAddressCountryName = country.EnglishName;
                            
                            }
                        }

                    });


                });

            }
        }
    }


    private toAddressCountryId: string;
    get ToAddressCountryId() { return this.toAddressCountryId; }
    set ToAddressCountryId(newValue: string) {
        if (this.toAddressCountryId != newValue) {
            this.toAddressCountryId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ToAddressCountryCode = null;
                this.ToAddressCountryName = null;
            }

            else {
                var filters = new ApiQueryFilters();
                filters.Tenant = SessionInfo.LoggedUserTenant;
                filters.GetAll = true;
                filters.ForceCacheRefresh = true;

                this.myEntityListService.getAllFromCache("Country", filters).then((res: any) => {
                    res.subscribe((myResponse: any) => {
                        if (!myResponse.HasError && myResponse.Result) {
                            var country = myResponse.Result.filter(d => d.Id == this.toAddressCountryId)[0];
                            if (country) {
                                this.ToAddressCountryCode = country.Code;
                                this.ToAddressCountryName = country.EnglishName;

                            }
                        }

                    });


                });

            }
        }
    }








    FromAddressId: string;
    ToAddressId: string;
    private fromPartnerCardId: string;
    get FromPartnerCardId() { return this.fromPartnerCardId; }
    set FromPartnerCardId(newValue: string) {
        if (this.fromPartnerCardId != newValue) {
            this.fromPartnerCardId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.FromPartnerCardCode = null;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: any) => {
                    if (!myResponse.HasError) {
                        var list: any = myResponse.Result;
                        if (list) {
                            this.FromPartnerCardCode = list.Code;
                            if (!AppTool.IsNullOrEmpty(list.PickAddressId)) {
                                this.FromAddressId = list.PickAddressId;
                            }

                            else {
                                this.FromAddressId = list.MainAddressId;
                            }


                        }
                    }
                });
            }
        }
    }

    private toPartnerCardId: string;
    get ToPartnerCardId() { return this.toPartnerCardId; }
    set ToPartnerCardId(newValue: string) {
        if (this.toPartnerCardId != newValue) {
            this.toPartnerCardId = newValue;
            if (AppTool.IsNullOrEmpty(newValue)) {
                this.ToPartnerCardCode = null;
            }

            else {
                this.myCardListService.getSingle(newValue).subscribe((myResponse: any) => {
                    if (!myResponse.HasError) {
                        var list: any = myResponse.Result;
                        if (list) {
                            this.ToPartnerCardCode = list.Code;
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


    constructor() {
        super();
        this.myCardListService = new CardListService();
        this.myEntityListService = new EntityListService();
        
        this.TransportModeLists = [];
        this.TransportModeLists.push(new ShipmentPickUpDeliveryTransportMode("BYTR","By Truck"));
        this.TransportModeLists.push(new ShipmentPickUpDeliveryTransportMode("BYRA", "By Rail"));
    } 


}

export class ShipmentPickUpDeliveryTransportMode {

    public Code: string;
    public Name: string;
    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;

    }
}


