import { TabItem } from '../Components/EditComponent/EditComponent';
import { ObjectFieldPM } from '../EntityPMs/ObjectFieldPM';
import { ScreenPM } from '../EntityPMs/ScreenPM';

export interface IHeaderScreenService {
    GetHeaderScreens(args): HeaderScreenServiceResult; 
}




export class HeaderScreenServiceResult{
    public ObjectFields: ObjectFieldPM[];
    public HeaderScreen: ScreenPM;
}














