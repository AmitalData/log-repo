



import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';
import { ARPayment } from './ARPayment';

describe('New ARPayment ', () => {

 
let l: Login= new Login();
let R: Random= new CreateRandom();
let AP: ARPayment= new ARPayment();


  it('New ARPayment Created Successfully', function () {

      var str = R.createrandomnum();
      l.login("https://test.logitudeworld.com/test/","sg1209@test.com","!Sg13579")
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
      AP.CreateNewARPayment("Test Customer ")

     
      
  });
});



