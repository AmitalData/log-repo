import { IHeaderScreenService, HeaderScreenDataResult } from '../../../Infrastructure/Interface/IHeaderScreenService';
declare var window: any;
export class DeclarationHeaderScreenService implements IHeaderScreenService {

    GetHeaderScreens(args: any): HeaderScreenDataResult {
        let result: HeaderScreenDataResult = new HeaderScreenDataResult();
        result.HeaderScreen = window.Screens.filter(d => d.ObjectTableId === args.ObjectTableId && d.Code.indexOf("HeaderScreen") != -1 )[0];
        result.ObjectFields = window.ObjectFields.filter(d => d.ObjectTableId === args.ObjectTableId);
        return result;
    }

}
