import {BluesnapContractPM} from '../EntityPMs/BluesnapContractPM';

export class BluesnapContractPMInitService {

    public static InitValues(entityPM: BluesnapContractPM, isNew: boolean) {
        if (isNew) {
            entityPM.UIProperties.SetEnabled("Code", "BluesnapContract", true);
        }
    }

    public static ApplyUIPoperties(entityPM: BluesnapContractPM, isNew: boolean) {
    }

}