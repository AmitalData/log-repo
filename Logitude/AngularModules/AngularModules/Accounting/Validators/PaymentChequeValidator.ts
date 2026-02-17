import { PaymentChequePM } from '../EntityPMs/PaymentChequePM';
import {PaymentChequeService} from '../Services/Others/PaymentChequeService';



export class PaymentChequeValidator
{

    public Validate(entityPM: PaymentChequePM) {
        var errors = [];
        var result = [];
        this.CheckIfExists(entityPM)


        return errors;
    }
    public  CheckIfExists(entityPM: PaymentChequePM) {
        var errors = [];
        var paymentChequeService = new PaymentChequeService();

            paymentChequeService.CheckIfPaymentChequeExists(entityPM.BankAccountId, entityPM.ChequeNumber).subscribe((exist) => {
               
                    if (exist) {
                        errors.push("Payment Cheque already exists");
                        //SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
                        //SessionLocator.CurrentSession.CurrentEditComponent.IsEditValid = false;
                        return errors;

                    } else {
                        //SessionLocator.CurrentSession.CurrentEditComponent.IsEditValid = true;
                        //SessionLocator.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                    }
                
            });
            //}
      
    }

}