import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ProductTypePM} from '../EntityPMs/ProductTypePM';

export class ProductTypePMInitService {

    public static InitValues(entityPM: ProductTypePM, isNew: boolean) {
    }

    public static ApplyUIPoperties(entityPM: ProductTypePM, isNew: boolean) {
        entityPM.UIProperties.SetEnabled("Code", "ProductType", false);
        entityPM.UIProperties.SetEnabled("Name", "ProductType", false);
    }

}