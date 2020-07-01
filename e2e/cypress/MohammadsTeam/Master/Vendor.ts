
export class VendorGLAccount {
    constructor() {



    }
    CreateNewVendor(Name: string) {
        {

            cy.get('input[id=null_Search]').type('Vendors').should("have.value", "Vendors")
            cy.get('div[id=MaintenanceItemMTVD]').click()
            cy.get('button[id=NewButton_Vendor]').click()
            cy.get('input[id=Address_Name]').type(Name).should("have.value", Name)
           
            cy.get('input[id=Address_CountryId]').type('PS').should("have.value", "PS")
            cy.get('ul[id=mydatalist_Address_CountryId]').contains("State Of Palestine").then(a => {


                a[0].click();
            })
            cy.get('input[id=Address_City]').type('Nablus').should("have.value", "Nablus")
            cy.get('button[id=Ok-AddVendor]').click()
            cy.get('input[id=SearchFieldsId_0_0]').type(Name).should("have.value", Name)
            cy.get('div[id=ListDataLoaded]').should('exist');
            cy.get('#row0col0').click();
            cy.log('Createing new vendor is done');

        })

    }
    ActivateVendorGlaccount() {
        cy.get('li[id=VendorTHAccounting]').click()
        cy.get('a[id=Activate]').click()
        cy.get('input[id=GLAccount_ChartOfAccountsId]').type("vendor").should("have.value", "vendor")
        cy.get('ul[id=mydatalist_GLAccount_ChartOfAccountsId]').contains("Vendor").then(a => {


            a[0].click();
        })
        cy.get('input[id=GLAccount_CurrencyId]').type("NIS").should("have.value", "NIS")
        cy.get('ul[id=mydatalist_GLAccount_CurrencyId]').contains("NIS").then(a => {


            a[0].click();
        })
        cy.get('button[id=Ok-AddGLAccount]').click()
        cy.log('Activating New VendorGLAccount is done');
    }
}
 
