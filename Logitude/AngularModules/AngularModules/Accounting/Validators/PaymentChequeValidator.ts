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
                        //SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = errors;
                        //SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = false;
                        return errors;

                    } else {
                        //SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = true;
                        //SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
                    }
                
            });
            //}
      
    }

}