



import  { CreateRandom } from './CreateRandom';
import { APInvoice } from './APinvoice';
import { LoginComp } from "../../login/Login.po";
export class APInvoiceSpec {

  private login: LoginComp = new LoginComp();
}
describe('New APInvoice ', () => {

 

let R: CreateRandom= new CreateRandom();
let AP: APInvoice= new APInvoice();


  it('New APInvoice Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
      AP.CreateNewAPInvoice("1000",str)

    });
      
 
});



