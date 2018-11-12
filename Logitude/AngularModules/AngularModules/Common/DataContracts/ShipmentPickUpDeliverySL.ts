
import { PartnerSL } from './PartnerSL';
import { PortSL } from './PortSL';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
export class ShipmentPickUpDeliverySL extends BaseComponent {

    public PickUpDeliveryTypeCode: string;
    public PickUpDeliveryToTypeCode: string;
    public PickUpDeliveryFromTypeCode: string;
    public FromAddressCity_Dummy: string;


    public FromAddressCity: string;
    public FromAddressZipCode: string;
    public FromAddressCountryId: string;
    public FromAddressCountryCode: string;
    public FromAddressCountryName: string;
    public ToAddressZipCode: string;
    public ToAddressCity_Dummy: string;
    public ToAddressCity: string;


    public ToAddressCountryId: string;
    public ToAddressCountryCode: string;
    public ToAddressCountryName: string;

    public FromPort: PortSL;
    public ToPort: PortSL;

    public FromPortId: string;
    public ToPortId: string;
    public FromPartner: PartnerSL;
    public ToPartner: PartnerSL;


    public FromPartnerCardId: string;
    public ToPartnerCardId: string;


    public MainCarriageETD: Date;
    public MainCarriageATD: Date;
    public MainCarriageETA: Date;
    public MainCarriageATA: Date;


    public TransportModeCode: string;
    public TransportModeName: string;


    constructor() {
        super();
    }
}


