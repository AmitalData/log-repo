import { IHeaderScreenService, HeaderScreenServiceResult } from '../../../Infrastructure/Interface/IHeaderScreenService';
declare var window: any;
export class ShipmentHeaderScreenService implements IHeaderScreenService {

    GetHeaderScreens(args: any): HeaderScreenServiceResult {
        let result: HeaderScreenServiceResult = new HeaderScreenServiceResult();
        result.HeaderScreen = window.Screens.filter(d => d.ObjectTableId === args.ObjectTableId && d.Code.indexOf("HeaderScreen") != -1 )[0];
        result.ObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === args.ObjectTableId && d.FieldName == "ShipperName");
        return result;
    }

}
