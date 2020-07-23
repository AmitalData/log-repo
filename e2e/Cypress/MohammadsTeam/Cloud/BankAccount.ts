/// <reference types="cypress"/>

export class BankAccount {
    constructor() {



    }
    CreateNewBankAccount(BankName: string, GLA: string, DeffGLA: string, TransGLA: string) {
        {
           // cy.get('li[id=General.MH.FullAccounting"]).click();
            cy.get('li[id=FABNKS]').click();
            cy.get('button[id=NEWBANK]').click();
            cy.get('input[id=BankAccount_BankId]').type("Leumi").should("have.value", "Leumi")
            cy.get('ul[id=mydatalist_BankAccount_BankId]').contains("Leumi").then(a => {
                a[0].click();
            })
            cy.get('input[id=BankAccount_BranchNumber]').type("7777").should("have.value", "7777")
            cy.get('input[id=BankAccount_AccountNumber]').type(BankName).should("have.value", BankName)
            cy.get('input[id=BankAccount_LocalName]').type(BankName).should("have.value", BankName)
            cy.get('input[id=BankAccount_EnglishName]').type(BankName).should("have.value", BankName)
            cy.get('input[id=BankAccount_CurrencyId]').type("NIS").should("have.value", "NIS")
            cy.get('ul[id=mydatalist_BankAccount_CurrencyId]').contains("NIS").then(a => {
                a[0].click();
            })
            cy.get('input[id=BankAccount_GLAccountId]').type(GLA).should("have.value", GLA)
            cy.get('ul[id=mydatalist_BankAccount_GLAccountId]').contains(GLA).then(a => {
                a[0].click();
            })
            cy.get('input[id=BankAccount_DeferredGLAccountId]').type(DeffGLA).should("have.value", DeffGLA)
            cy.get('ul[id=mydatalist_BankAccount_DeferredGLAccountId]').contains(DeffGLA).then(a => {
                a[0].click();
            })
            cy.get('input[id=BankAccount_TransferGLAcccountId]').type(TransGLA).should("have.value", TransGLA)
            cy.get('ul[id=mydatalist_BankAccount_TransferGLAcccountId]').contains(TransGLA).then(a => {
                a[0].click();
            })
            cy.get('button[id=OKBUTTON]').click();


        }

    }

    CreateBankGLAccount(Name: string) {
        
        cy.get('button[id=NewGLAccount]').click();
        cy.get('input[id=GLAccount_ChartOfAccountsTypeCode]').type('Banks').should("have.value", "Banks");
        cy.get('ul[id=mydatalist_GLAccount_ChartOfAccountsTypeCode]').contains("Banks").then(a => {
            a[0].click();
        })
        cy.get('input[id=GLAccount_ChartOfAccountsId]').type('Bank').should("have.value", "Bank");
        cy.get('ul[id=mydatalist_GLAccount_ChartOfAccountsId]').contains("Bank").then(a => {
            a[0].click();
        })
        cy.get('input[id=GLAccount_LocalName]').type(Name).should("have.value", Name);
        cy.get('input[id=GLAccount_CurrencyId]').type("NIS").should("have.value", "NIS");
        cy.get('ul[id=mydatalist_GLAccount_CurrencyId]').contains("NIS").then(a => {
            a[0].click();
        })
        cy.get('input[id=GLAccount_RevenueExpenseType]').type("Other").should("have.value", "Other");
        cy.get('ul[id=mydatalist_GLAccount_RevenueExpenseType]').contains("Other").then(a => {
            a[0].click();
        })


        cy.get('button[id=Ok-AddGLAccount]').click();

       





    }

    }
       
   

