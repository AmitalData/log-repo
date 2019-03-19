import { TMBudgetPM } from '../EntityPMs/TMBudgetPM';

export class TMBudgetPMInitService {

    public static InitValues(entityPM: TMBudgetPM, isNew: boolean) {
    }

    public static ApplyUIPoperties(entityPM: TMBudgetPM, isNew: boolean) {
        if (isNew) {
            entityPM.UIProperties.SetVisibility("Inactive", "TMBudget", false);
        }
    }
}
