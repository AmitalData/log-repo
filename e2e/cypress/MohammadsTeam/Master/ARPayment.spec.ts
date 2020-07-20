



import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';
import { ARPayment } from './ARPayment';
import { LoginComp } from "../../login/Login.po";
export class ARPAyemntSpec {

  private login: LoginComp = new LoginComp();
}
describe('New ARPayment ', () => {

 
let l: Login= new Login();
let R: CreateRandom= new CreateRandom();
let AP: ARPayment= new ARPayment();


  it('New ARPayment Created Successfully', function () {

      var str = R.createrandomnum();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
      AP.CreateNewARPayment("Test Customer ")

    });
      
 
});



