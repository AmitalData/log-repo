




import  { CreateRandom } from './CreateRandom';
import { APPayment } from './APPayment';
import { LoginComp } from "../../login/Login.po";
export class ARPAyemntSpec {

  private login: LoginComp = new LoginComp();
}
describe('New APPayment ', () => {

 

let R: CreateRandom= new CreateRandom();
let AP: APPayment= new APPayment();


  it('New APPayment Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
      AP.CreateNewAPPayment("1000")

    });
      
 
});



