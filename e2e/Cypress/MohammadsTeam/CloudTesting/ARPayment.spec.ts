



import { LoginCloud } from './LoginCloud';
import  { CreateRandom } from './CreateRandom';
import { ARPayment } from './ARPayment';

describe('New ARPayment ', () => {

 
let l: LoginCloud= new LoginCloud();
let R: CreateRandom= new CreateRandom();
let AP: ARPayment= new ARPayment();


  it('New ARPayment Created Successfully', function () {

      var str = R.createrandomnum();
      l.dologin();
      cy.get('li[id=GeneralMHMaintenance]').click()
   
      cy.get('li[id=PAR]')
      cy.get('li[id=GeneralMHFullAccounting]').click();
      AP.CreateNewARPayment("Test Customer ")

     
      
  });
});



