


import { LoginCloud } from './LoginCloud';
import { CreateRandom } from './CreateRandom';
import { BankAccount } from './BankAccount';

describe('New Bank Account ', () => {

    let B: BankAccount = new BankAccount();
    let l: LoginCloud = new LoginCloud();
    let R: CreateRandom = new CreateRandom();


    it('New Bank Account Created Successfully', function () {

        var str = R.createrandomnum();
        l.dologin();
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


