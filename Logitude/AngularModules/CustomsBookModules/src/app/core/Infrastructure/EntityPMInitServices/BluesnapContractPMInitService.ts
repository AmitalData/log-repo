import {BluesnapContractPM} from '../EntityPMs/BluesnapContractPM';

export class BluesnapContractPMInitService {

    public static InitValues(entityPM: BluesnapContractPM, isNew: boolean) {
        if (isNew) {
            entityPM.UIProperties.SetEnabled("Code", "BluesnapContract", true);
            entityPM.UIProperties.SetEnabled("BluesnapContractTypeCode", "BluesnapContract", true);
        }
    }


    public static ApplyUIPoperties(entityPM: BluesnapContractPM, isNew: boolean) {
        if (isNew) {
            entityPM.UIProperties.SetEnabled("BluesnapContractTypeCode", "BluesnapContract", true);
            entityPM.UIProperties.SetEnabled("Code", "BluesnapContract", true);
        }

        else {
            entityPM.UIProperties.SetEnabled("BluesnapContractTypeCode", "BluesnapContract", false);
            entityPM.UIProperties.SetEnabled("Code", "BluesnapContract", false);
        }
    }

}
