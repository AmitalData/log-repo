
import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';
import { ARInvoice } from './ARInvoice';
import { LoginComp } from "../../login/Login.po";
export class ARPAyemntSpec {

  private login: LoginComp = new LoginComp();
}
describe('New ARInvoice ', () => {

 
let l: Login= new Login();
let R: CreateRandom= new CreateRandom();
let AR: ARInvoice= new ARInvoice();


  it('New ARInvoice Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
      AR.CreateNewARInvoice("70724")

    });
      
 
});



