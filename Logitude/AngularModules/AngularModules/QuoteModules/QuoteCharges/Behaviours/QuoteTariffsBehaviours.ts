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
        args.BetweenDate = this.GetDate();
        args.FriehgtAmount = ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT"), "CostTotalAmount");;
        args.ForiegnChargesAmount = ArrayTool.Sum(this.EntityPM.QuoteCharges.filter(d => d.SaleCurrencyId != SessionLocator.LocalCurrencyId && d.SaleMeasurementCode != "PFCL"), "SaleTotalAmountLocal");
        args.QuoteId = this.EntityPM.Id;
        args.LocalCurrencyId = SessionLocator.LocalCurrencyId;
        var tariffService: TariffDomainService = new TariffDomainService();
        tariffService.GetAvailableSalesLocalChargesTariffs(args).subscribe((res: ServiceResponse) => {
            if (!res.HasError && res.Result) {
                var saleLocalCharges = res.Result;
                return saleLocalCharges;
            }
            else {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = res.ErrorsArray;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    GetDate() {
        var betweenDate: Date = DateTool.GetCurrentDateAsUtc();
        if (this.EntityPM.DirectionId == "I") {
            betweenDate = this.EntityPM.ETA;
        }
        else {
            betweenDate = this.EntityPM.ETD;
        }

        return betweenDate;
    }
}
