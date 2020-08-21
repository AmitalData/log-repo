
export class ChartOfAccount {

    constructor() {


    }


    CreateNewChartOFAccount(ChartOfAccountNo:string, Type:string) {

        cy.get('input[id=null_Search]').type('chart')
          
        cy.get('#MaintenanceItemMTCA').click();
        cy.get('#NewButton_ChartOfAccount').click();
        cy.get('input[id=ChartOfAccount_EnglishName]').type(Type + ChartOfAccountNo);
        cy.get('input[id=ChartOfAccount_LocalName]').type(Type + ChartOfAccountNo);
        cy.get('input[id=ChartOfAccount_Code]').type(ChartOfAccountNo);


        if (Type == 'Customer') {
            cy.get('input[id=ChartOfAccount_TypeCode').type('Customer');
            cy.get('.DropDownListItem').contains('Customers').click();
            cy.get('#ok-AddChartOfAccount').click();
            //cy.get('#BusyIndicator_0').should('not.be.visible');
           // cy.get(".LogitudeWindow").should('not.be.visible');
           
        }

    }

    EditChartOFAccount(ChartOfAccountNo:string) {
        cy.get('input[id=SearchFieldsId_0_0]').type(ChartOfAccountNo);
        cy.get('div[id=ListDataLoaded]').then( a=> {
            cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
        })
      
      //  cy.get('#BusyIndicator_0').should('not.be.visible');
        cy.get('input[id=ChartOfAccount_EnglishName]').type('English Name Modified');
        cy.get('input[id=ChartOfAccount_LocalName]').type('Local Name Modified');
       // cy.get('#ChartOfAccount-SaveClose').should('be.visible');
        cy.get('#ChartOfAccount-SaveClose').click();
       // cy.get('#BusyIndicator_0').should('not.be.visible');



    }


}
