import {TotangoService} from '../Services/WebServices/TotangoService'
import {RulesValidator} from '../Validators/RulesValidator';

export class ServiceLocator {

    public static TotangoSenderService: TotangoService;
    public static SendTotangoUserActivity(module: string, activity: string) {

        if (this.TotangoSenderService == null) {
            this.TotangoSenderService = new TotangoService();
        }

        this.TotangoSenderService.SendTotangoUserActivity(module, activity);
    }

    private static rulesValidator: RulesValidator;
    public static get RulesValidator(): RulesValidator {

        if (!this.rulesValidator) {
            this.rulesValidator = new RulesValidator();
        }
        return this.rulesValidator;
    }
    public static set RulesValidator(newValue: RulesValidator) { this.rulesValidator = newValue; }
}