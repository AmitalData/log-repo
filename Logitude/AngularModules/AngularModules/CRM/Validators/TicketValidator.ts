
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {CRMDomainService} from '../Services/CRMDomainService';
import {TicketPM} from '../EntityPMs/TicketPM';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

export class TicketValidator {

    private EntityPM: TicketPM;
    private Errors: string[] = [];
    private message: string;

    constructor() {
        this.Errors = [];
        this.message = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    }
    public ValidateCurrenctEntity(entityPM: TicketPM) {
        this.Errors = [];
        this.EntityPM = entityPM;
        Validator.TryValidateObject(entityPM, "Ticket", this.Errors);
        if (AppTool.IsNullOrEmpty(entityPM.OwnerId)) {
            this.Errors.push("Owner field is required");
        }
        if (AppTool.IsNullOrEmpty(entityPM.CompanyId)) {
            this.Errors.push("Company field is required");
        }
        return this.Errors;
    }
}