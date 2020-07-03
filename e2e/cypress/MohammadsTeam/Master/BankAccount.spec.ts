


import { Login } from './Login';
import { CreateRandom } from './CreateRandom';
import { NewBankAccount } from './BankAccount';

describe('New Bank Account ', () => {

    let B: BankAccount = new NewBankAccount();
    let l: Login = new Login();
    let R: Random = new CreateRandom();


    it('New Bank Account Created Successfully', function () {

        var str = R.createrandomnum();
        l.login("https://test.logitudeworld.com/test/", "sg1209@test.com", "!Sg13579")
        cy.get('li[id=GeneralMHMaintenance]').click()

        cy.get('li[id=PAR]')
        var g = 'BankGLAccount' + str;
        var d = 'BankDeffGLAccount' + str;
        var t = 'BankTransGLAccount' + str;
        var ba= 'Bank'+str
        cy.get('li[id=GeneralMHFullAccounting]').click();
        cy.get('li[id=FAGLAccouts]').click();
        B.CreateBankGLAccount(g)
        B.CreateBankGLAccount(d)
        B.CreateBankGLAccount(t)

        B.CreateNewBankAccount(ba, g, d, t);
        cy.log('New Bank Creaed Successfully');


    });
});


