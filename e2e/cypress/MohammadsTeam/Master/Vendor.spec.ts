


import { VendorGLAccount } from './Vendor';
import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';

describe('New Vendor GlAccount ', () => {

  let v: VendorGLAccount = new VendorGLAccount();
let l: Login= new Login();
let R: Random= new CreateRandom();


  it('New Vendor GlAccount Created Successfully', function () {

      var str = R.createrandomnum();
      l.login("https://test.logitudeworld.com/test/","sg1209@test.com","!Sg13579")
     cy.get('li[id=GeneralMHMaintenance]').click()
   
cy.get('li[id=PAR]')
      v.CreateNewVendor("Test Vendor" + str);
      v.ActivateVendorGlaccount()

     
      
  });
});



