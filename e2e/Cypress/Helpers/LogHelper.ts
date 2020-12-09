export class LogHelper
{
    public static LOVSearchAndSelectFirst(code: string, searchText: string)
    {
        cy.get('input[id="' + code + '"]').should('be.visible').then(a =>
        {
           cy.get('input[id="' + code + '"]').clear();
           cy.get('input[id="' + code + '"]').type(searchText,{ force: true }).should("have.value", searchText);
           cy.get('ul[id="mydatalist_' + code + '"]').contains(searchText).then(a =>
           {
                a[0].click();
            });
        });
    }
    public static ClickButton(buttonId: string)
    {
        cy.get('button[id="' + buttonId + '"]').click();
    }
    public static ClickListItem(buttonId: string)
    {
        cy.get('li[id="' + buttonId + '"]').click();
    }
    public static TypeInput(inputId: string, text: string = "", verifyText = false)
    {
        if (verifyText)
            cy.get('input[id="' + inputId + '"]').type(text).should("have.value", text);
        else
            cy.get('input[id="' + inputId + '"]').type(text);

    }
    /*
        List Component
    */
    public static QuerySearchAndSelectFirst(searchText: string = "")
    {
        cy.get('input[id=SearchFieldsId_0_0]').should('be.visible').then(a =>
        {
            cy.get('input[id=SearchFieldsId_0_0]').type(searchText, { force: true });
            cy.get('div[id=ListDataLoaded]').then(a =>
            {
                cy.get('div[id=LogGrid_0_0row0]').click({ force: true });
            })
        });
    }
    public static ComboBoxSearchAndSelectFirst(code: string, searchText: string){
        throw "Not implemented method";
    }
    
    /* Edit Grid Should have id on any wrapper div */
    public static AssertEditGridHaveItems(gridWrapperId: string)
    {
        cy.get("#" + gridWrapperId + " .ag-row").should("have.length.greaterThan", 0);
    }

}