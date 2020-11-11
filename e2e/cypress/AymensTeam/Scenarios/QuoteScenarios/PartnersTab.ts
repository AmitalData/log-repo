import { Resolvers } from "../../Resolvers/Resolvers";

export class PartnersTab {
    private direction: string;
    private transportMode: string;
    private isInlandDomestic: boolean = false;

    public RunPartnersTabScenarios(direction: string, transportMode: string,) {
        this.direction = direction;
        this.transportMode = transportMode;

        if (this.direction == "D" && this.transportMode == "I") {
            this.isInlandDomestic = true;
        }
        this.GoToPartnerTab();
        this.AddPartners();
        this.DeletePartner();
    }
    private GoToPartnerTab() {
        cy.get('#QuoteTHPartners').click();
    }
    private AddPartners() {
        if (this.isInlandDomestic ) {
            this.FillAgent('TestAgent');
            this.FillNotify('TestAgent');
        }
        else if (this.direction == "I") {
            this.FillConsignee('TestShipper');
            this.FillAgent('TestAgent');
            this.FillNotify('TestAgent');
        }
        else {
            this.FillConsignee('TestCons')
            this.FillAgent('TestAgent');
            this.FillNotify('TestAgent');
        }
    }
    private DeletePartner() {
        if (this.isInlandDomestic) {
            Resolvers.ButtonResolver.Selector('#Delete_1').ThenConfirmButtonText("Yes").Click();
        } else {
            Resolvers.ButtonResolver.Selector('#Delete_3').ThenConfirmButtonText("Yes").Click();
        }
    }
    private FillShipper(name: string) {
        cy.contains('label', 'Add Partners').click({force:true});
        Resolvers.ButtonResolver.Selector('#SHIPR').Click();
        Resolvers.LOVResolver.Selector('#Quote_ShipperId').Type('Testship');
        Resolvers.ButtonResolver.Selector('#PartnerOKbtn').Click();
        //Resolvers.ButtonResolver.Selector('Button').Text('Ok').Click();
    }
    private FillConsignee(name: string) {
        cy.contains('label', 'Add Partners').click({ force: true });
        Resolvers.ButtonResolver.Selector('#CONSI').Click();
        Resolvers.LOVResolver.Selector('#Quote_ConsigneeId').Type('TestCons');
        Resolvers.ButtonResolver.Selector('#PartnerOKbtn').Click();

    }
    private FillAgent(name: string) {
        cy.contains('label', 'Add Partners').click({ force: true });
        Resolvers.ButtonResolver.Selector('#AGENT').Click();
        Resolvers.LOVResolver.Selector('#Quote_AgentId').Type('TestAgent');
        Resolvers.ButtonResolver.Selector('#PartnerOKbtn').Click();
        //Resolvers.ButtonResolver.Selector('Button').Text('Ok').Click();
    }
    private FillNotify(name: string) {
        cy.contains('label', 'Add Partners').click({ force: true });
        Resolvers.ButtonResolver.Selector('#NOTFY').Click();
        Resolvers.LOVResolver.Selector('#Quote_NotifyId').Type('TestAgent');
        Resolvers.ButtonResolver.Selector('#PartnerOKbtn').Click();
       //Resolvers.ButtonResolver.Selector('Button').Text('Ok').Click();

    }
}