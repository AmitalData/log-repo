
import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';
import { PaymentCheque } from './PaymentCheque';
import { LoginComp } from "../../login/Login.po";
export class PaymentChequeSpec {

  private login: LoginComp = new LoginComp();
}
describe('New P ', () => {

 

let R: CreateRandom= new CreateRandom();
let AP: PaymentCheque= new PaymentCheque();


  it('New PaymentCheque  Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
      cy.get('#FABNKS').click()
     
      
      AP.CreateNewPaymentCheque('DiffUR822383FN3');
     

    });
      
 
});



