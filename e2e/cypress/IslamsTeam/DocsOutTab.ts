

export class DocsOutTabComponent {

  

    public DocsOutTab() {

        cy.get('Shipment.TH.DocsOut').click();
       //this.Helper.WaitBusyIndicator();

    }

    QuickSearchDocOut(docsOutId: string, docOutRow: string, searchTerm: string) {
         //this.Helper.WaitBusyIndicator();
         this.UseDocsOutSearchBox('SearchFieldsId_0_0', searchTerm, docsOutId, docOutRow);
    }

    UseDocsOutSearchBox(searchFeildId: string, searchByRef: string, docOutId: string, docOutRow: string) {
         //this.Helper.WaitBusyIndicator();
         this.Helper.WaitByIdAndFill(searchFeildId, searchByRef);
        this.Helper.WaitByIdAndClick(docOutRow);
        this.Helper.WaitByIdAndClick(docOutId);
    }
}
