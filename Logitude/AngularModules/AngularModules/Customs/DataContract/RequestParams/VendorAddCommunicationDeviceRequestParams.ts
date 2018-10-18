import {RequestParamsBase} from './RequestParamsBase';
import {VendorCommunicationResult} from '../ResponseData/VendorCommunicationResult';

export class VendorAddCommunicationDeviceRequestParams extends RequestParamsBase{
    VendorNumber: number;
    CommunicationDevices: VendorCommunicationResult[];
}