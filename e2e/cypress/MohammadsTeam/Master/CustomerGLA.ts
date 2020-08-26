export class CustomerGLA {

    constructor() { }



    createCustomer(name: string) {
        cy.get('li[id="GeneralMHCustomers"]').click();
        cy.get('Button[id="NewButton_Customer"]').click();

        cy.get('#Address_Address1').type('Ramallah');
        cy.get('#Address_Address2').type('Nablus');
        cy.get('#Address_ZipCode').type('00970');
        cy.get('#Address_CountryId').type('ps');
        cy.get('#Address_LocalName').type(name);
        cy.get('#textboxdiv_Address_Name').type(name);
        cy.get('#Address_City').type('Nablus');
        cy.get('#Address_CountryId').type('ps');
        cy.get('.DropDownListItem').contains(' State Of Palestine ').click();
        cy.get('#Ok-AddCustomer').click();
        //cy.get('#BusyIndicator_0').should('not.be.visible');
        //cy.get(".LogitudeWindow").should('not.be.visible');

    }

    activatecustomer(name: string) {
        cy.get('#SearchFieldsId_0_0').type(name,{ force: true });
        cy.get('div[id=ListDataLoaded]').should('exist');
        cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
        cy.get('li[id=CustomerTHGeneral]').click();
        cy.get('#Customer_EnglishName');
        // cy.get('#Customer.TH.Accounting').click(); didnt work  ???? should talk with moh about it 
        cy.get('.TextTrimming').contains('Accounting').click(); // took me a year to work 
        cy.get('#Activate').click();
        cy.get('#GLAccount_ChartOfAccountsId').type('cust');
        cy.get('.DropDownListItem').contains('Customer').click(); 
        cy.get('#GLAccount_CurrencyId').type('Nis');
        cy.get('.DropDownListItem').contains(' NIS ').click(); 
        cy.get('#Ok-AddGLAccount').click();
        cy.get('td').contains('Dispaly transactions').click();
       // cy.get('#BusyIndicator_0').should('not.be.visible');
        //cy.get(".LogitudeWindow").should('not.be.visible');




    }

}

