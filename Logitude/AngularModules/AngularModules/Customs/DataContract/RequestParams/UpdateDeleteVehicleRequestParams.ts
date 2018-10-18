import { RequestParamsBase } from './RequestParamsBase';


export class UpdateDeleteVehicleRequestParams extends RequestParamsBase {
    VehicleId: string;
    IsDelete: boolean;
}