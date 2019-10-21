import { browser, by, element } from 'protractor';
import { NewUser } from './Users';
import { NewUserScenario } from './UsersScenario';
import { LoginComp } from "../../login/Login.po";




describe('NewUser', () => {

    let NewUsers: NewUser = new NewUser();
    let UsersScenario: NewUserScenario = new NewUserScenario();

    beforeEach(() => {

    });

    browser.ignoreSynchronization = true;

    it('QuickSearch', function () {

        UsersScenario.Quicksearch()
    });

    it('SearchUserTab', function () {

        UsersScenario.SearchUserTab();
    });


    it('CreateNewUser', function () {


        UsersScenario.CreateNewUser();
    });

    it('SearchUser', function () {


        UsersScenario.SearchUser();
    });



});
