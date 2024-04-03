import { QuotePM } from '../../../Quote/EntityPMs/QuotePM';
import { SalesLocalChargesTariffSearchArgs } from '../../../TariffModule/Services/TariffDomainService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ArrayTool} from '../../../Infrastructure/Tools';

export class QuoteTariffsBehaviours {
    public EntityPM: QuotePM = null;
    public CurrentSession = SessionLocator.SelectedSession;

    constructor(entityPM: QuotePM) {
        this.EntityPM = entityPM;
    }

    public GenerateSalesLocalCharges() {
        this.CurrentSession.StartBusyIndicatorLoading();
        var args: SalesLocalChargesTariffSearchArgs = new SalesLocalChargesTariffSearchArgs();
        args.FromCountryId = this.EntityPM.FromCountryId;
        args.ToCountryId = this.EntityPM.ToCountryId;;
        args.FriehgtAmount = ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT"), "CostTotalAmount");;
        args.ForiegnChargesAmount = ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(d => d.SaleCurrencyId != SessionLocator.LocalCurrencyId && d.SaleMeasurementCode != "PFCL"), "SaleTotalAmountLocal");
        args.QuoteId = this.EntityPM.Id;
        args.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        return args;
    }

}
