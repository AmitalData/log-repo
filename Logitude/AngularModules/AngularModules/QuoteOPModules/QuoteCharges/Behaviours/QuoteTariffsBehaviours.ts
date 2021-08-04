import { QuotePM } from '../../../Quote/EntityPMs/QuotePM';

export class QuoteTariffsBehaviours {
    public EntityPM: QuotePM = null;

    constructor(entityPM: QuotePM) {
        this.EntityPM = entityPM;
    }
}
