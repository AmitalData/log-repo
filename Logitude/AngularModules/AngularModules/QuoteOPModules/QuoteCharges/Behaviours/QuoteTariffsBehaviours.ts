import { QuoteOPPM } from '../../../QuoteOPM/EntityPMs/QuoteOPPM';

export class QuoteTariffsBehaviours {
    public EntityPM: QuoteOPPM = null;

    constructor(entityPM: QuoteOPPM) {
        this.EntityPM = entityPM;
    }
}
