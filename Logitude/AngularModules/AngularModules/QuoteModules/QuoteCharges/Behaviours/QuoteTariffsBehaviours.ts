import { QuotePM } from '../../../Quote/EntityPMs/QuotePM';
import { TariffDomainService, SalesLocalCharges, SalesLocalChargesTariffSearchArgs } from '../../../TariffModule/Services/TariffDomainService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ArrayTool, DateTool} from '../../../Infrastructure/Tools';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';

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
