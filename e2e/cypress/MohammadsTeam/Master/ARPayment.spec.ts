



import { Login } from './Login';
import  { CreateRandom } from './CreateRandom';

describe('New ARPayment ', () => {

 
let l: Login= new Login();
let R: Random= new CreateRandom();


  it('New ARPayment Created Successfully', function () {

      var str = R.createrandomnum();
      l.login("https://test.logitudeworld.com/test/","sg1209@test.com","!Sg13579")

     
      
  });
});



