import { ObjectFieldPM } from '../EntityPMs/ObjectFieldPM';
import { ScreenPM } from '../EntityPMs/ScreenPM';

export interface IHeaderScreenService {
    GetHeaderScreens(args): HeaderScreenDataResult; 
}




export class HeaderScreenDataResult{
    public ObjectFields: ObjectFieldPM[];
    public HeaderScreen: ScreenPM;
}














