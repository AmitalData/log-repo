import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";

export class NewQuoteWizardScenarios {
    private direction: string;
    private transportMode: string;
    private shipmentType: string;
    private objectTable: string;
    private isFCL: boolean = false;
    private isInlandDomestic: boolean = false;
    public EntityId: string;
    public EntityNumber: string;
    public quoteType: string;

    public RunScenario(direction: string, transportMode: string, shipmentType: string = null, quoteType:string) {

        this.direction = direction.toUpperCase();
        this.transportMode = transportMode.toUpperCase();
        this.shipmentType = shipmentType.toUpperCase();
        this.quoteType = quoteType.toUpperCase();
        this.objectTable = "Quote";

        if (this.shipmentType == 'FCLD' || this.shipmentType == 'FCL')
            this.shipmentType = 'FCLD';
        if (this.shipmentType == 'LCLD' || this.shipmentType == 'LCL')
            this.shipmentType = 'LCLD';
        if ((this.transportMode == "O" && this.shipmentType == "FCLD") || (this.transportMode == "I" && this.shipmentType == "FTL")) {
            this.isFCL = true;
        }

        if (this.direction == "D" && this.transportMode == "I") {
            this.isInlandDomestic = true;
        }

        this.OpenWizardWindow();
        this.FillRadioButtons();
        this.FillPartners();
        this.FillGeneral();
        this.FillMainCarriage();

        //this.FillOrderDetails();
        this.Save().then((entityNumber: string) => {
            cy.get('quicksearchtextbox')
                .find('.LogitudeQuickSearchTextBox')
                .eq(0)
                .within(() => {
                    cy.get('input').type(entityNumber).then(() => {
                        cy.get('ul > li').then(a => {
                            cy.contains('td', entityNumber).click({ force: true });
                            //cy.get('ul > li').eq(0).click({ force: true });
                        });
                    });
                });
        });


        //this.Save();
        //this.OpenWizardWindow();
        //this.CancelWizardWindow();
    }

    private OpenWizardWindow() {
        Resolvers.ButtonResolver.Selector('#NewQuote').Click();
        //Resolvers.ButtonResolver.Selector('Button').Text('New').Click();
        Resolvers.WindowResolver.ShouldBeOpend();
    }
    private CancelWizardWindow() {
        Resolvers.ButtonResolver.Selector('Button').Text('Cancel').ThenConfirmButtonText("Don't Save").Click();
        Resolvers.WindowResolver.ShouldBeClosed();
    }
    private FillRadioButtons() {

        cy.get("#DirectionRadio_" + Resolvers.Session + this.direction).click({ force: true });
        cy.get("#TransportModeRadio_" + Resolvers.Session + this.transportMode).click({ force: true });

        if (this.transportMode != "A") {
            cy.get("#ShipmentTypeRadio_" + Resolvers.Session + this.shipmentType).click({ force: true });
        }

        switch (this.transportMode) {
            case "A": {
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FCLD").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LCLD").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FTL").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LTL").should("not.exist");
                break;
            }

            case "O": {
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FCLD").should("be.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LCLD").should("be.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FTL").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LTL").should("not.exist");
                break;
            }

            case "I": {
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FCLD").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LCLD").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FTL").should("be.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LTL").should("be.exist");
                break;
            }
        }
    }
    private FillPartners() {
        if (this.isInlandDomestic) {
            this.FillShipper('TestShipper');
            this.FillConsignee('TestShipper');
        }

        else if (this.direction == "I") {
            this.FillConsignee('TestConsi');
        }

        else {
            this.FillShipper('TestShipper');
        }
    }
    private FillShipper(name: string) {
        Resolvers.LOVResolver.Selector('#Quote_ShipperId').Type(name);
        Resolvers.TextBoxResolver.Selector("#Quote_ShipperReference1").Type(Random.GetRandomNumber());
        Resolvers.TextBoxResolver.Selector("#Quote_ShipperReference2_1").Type(Random.GetRandomNumber());

        //Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("ShipperId").Type(name);
        //Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("ShipperReference1").Type(Random.GetRandomNumber());
        //Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("ShipperReference2").Type(Random.GetRandomNumber());
    }
    private FillConsignee(name: string) {
        Resolvers.LOVResolver.Selector('#Quote_ConsigneeId').Type(name);
        Resolvers.TextBoxResolver.Selector("#Quote_ConsigneeReference1").Type(Random.GetRandomNumber());
        Resolvers.TextBoxResolver.Selector("#Quote_ConsigneeReference2_1").Type(Random.GetRandomNumber());

        //Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("ConsigneeId").Type(name);
        //Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("ConsigneeReference1").Type(Random.GetRandomNumber());
        //Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("ConsigneeReference2").Type(Random.GetRandomNumber());
    }
    private FillGeneral() {
        if (this.quoteType == 'SR') {
            Resolvers.ButtonResolver.Selector("#AdhocRadio0").Click();
        } else if (this.quoteType == 'RR') {
            Resolvers.ButtonResolver.Selector("#RoutingRadio0").Click();
        }
        Resolvers.LOVResolver.Selector("#Quote_IncotermId").SelectFirst();
        Resolvers.LOVResolver.Selector("#Quote_MoveTypeId").SelectFirst();

        //Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("IncotermId").SelectFirst();
        //Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("MoveTypeId").SelectFirst();
    }
    private FillMainCarriage() {

        var fromPortCode: string = "EZE";
        var toPortCode: string = this.direction == "D" ? "EZE" : "MVD";
        var carrier: string;

        var fromPortLabel: string;
        var toPortLabel: string;
        var carrierLabel: string;

        switch (this.transportMode) {
            case "A": {
                carrier = "BA";
                fromPortLabel = "Gateway";
                toPortLabel = "Destination";
                carrierLabel = "Airline";
                break;
            }

            case "O": {
                carrier = "MAEU";
                fromPortLabel = "Loading Port";
                toPortLabel = "Discharge Port";
                carrierLabel = "Shipping line";
                break;
            }

            default: {
                carrier = "Trucker1London";
                fromPortLabel = "From";
                toPortLabel = "To";
                carrierLabel = "Trucker";
                break;
            }
        }
        if (!this.isInlandDomestic) {
            Resolvers.LOVResolver.Selector("#Quote_FromPortId").Type(fromPortCode);
            Resolvers.LOVResolver.Selector("#Quote_ToPortId").Type(toPortCode);
            Resolvers.LOVResolver.Selector("#Quote_MainCarriageCarrierId").Type(carrier);
        }

        //if (this.isInlandDomestic) {
        //    cy.get('label').contains('Include PickUp').should('not.exist');
        //    cy.get('label').contains('Include Delivery').should('not.exist');
        //    cy.get('loglabel[ng-reflect--object-field-name="FromPortId"]').find('label').contains(fromPortLabel).should('not.exist');
        //    cy.get('loglabel[ng-reflect--object-field-name="ToPortId"]').find('label').contains(toPortLabel).should('not.exist');
        //}

        //else {
        //    cy.get('loglabel[ng-reflect--object-field-name="FromPortId"]').find('label').contains(fromPortLabel).should('be.exist');
        //    cy.get('loglabel[ng-reflect--object-field-name="ToPortId"]').find('label').contains(toPortLabel).should('be.exist');
        //    cy.get('loglabel[ng-reflect--object-field-name="MainCarriageCarrierId"]').find('label').contains(carrierLabel).should('be.exist');
        //    Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("FromPortId").Type(fromPortCode);
        //    Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("ToPortId").Type(toPortCode);
        //    Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("MainCarriageCarrierId").Type(carrier);
        //}
    }
    private FillOrderDetails() {

        if (this.isFCL) {
            this.FillContainers();
        }

        else {
            this.FillDimentions();
        }

        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("DescriptionOfGoods").IsTextArea().Type('Cypress testing - Creating New Shipment');
    }
    private FillContainers() {
        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("Quantity1").Type("1");
        Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("PackageTypeId1").Type('20 Ft. Bulk');

        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("Quantity2").Type("1");
        Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("PackageTypeId2").Type('20 ft. high cube');

        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("Quantity3").Type("1");
        Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("PackageTypeId3").Type('20 Ft. Tank');

        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("Quantity4").Type("1");
        Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("PackageTypeId4").Type('20 Ft. Open Top');
    }
    private FillDimentions() {
        Resolvers.ButtonResolver.Selector('Hyperlink').Text('Fill Dimensions').Click();
        Resolvers.WindowResolver.Index(1).ShouldBeOpend();

        this.FillDimentionsRow(0, "1", "100", "250", "Bags");
        this.FillDimentionsRow(1, "1", "50", "150", "Bales");

        Resolvers.GridViewResolver.DeleteSimpleGridRow(4);
        Resolvers.GridViewResolver.DeleteSimpleGridRow(3);
        Resolvers.GridViewResolver.DeleteSimpleGridRow(2);

        this.FillDimentionsWindow(2);

        Resolvers.WindowResolver.Index(1).CloseOk();
        Resolvers.WindowResolver.Index(1).ShouldBeClosed();
    }
    private FillDimentionsRow(index: number, quantity: string, dimention: string, grossWeight: string, packageType: string) {
        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("Quantity").Index(index).Type(quantity);

        if (this.transportMode != "A") {
            Resolvers.LOVResolver.ObjectTable("QuotePackage").ObjectField("PackageTypeId").Index(index).Type(packageType);
        }

        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("Length").Index(index).Type(dimention);
        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("Width").Index(index).Type(dimention);
        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("Height").Index(index).Type(dimention);
        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("GrossWeight").Index(index).Type(grossWeight);
    }
    private FillDimentionsWindow(index: number) {
        Resolvers.ButtonResolver.Selector('Button').Text('Add Package').Click();
        Resolvers.WindowResolver.Index(2).ShouldBeOpend();

        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("Quantity").Index(index).Type("1");
        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("Length").Index(index).Type("200");
        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("Width").Index(index).Type("200");
        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("Height").Index(index).Type("200");
        Resolvers.TextBoxResolver.ObjectTable("QuotePackage").ObjectField("GrossWeight").Index(index).Type("400");

        Resolvers.WindowResolver.Index(2).CloseOk();

        if (this.transportMode == "A") {
            Resolvers.WindowResolver.Index(2).ShouldBeClosed();
        }

        else {
            Resolvers.WindowResolver.Index(2).ShouldBeOpend();
            Resolvers.WindowResolver.Index(2).HasValidationError("Package Type is required");

            Resolvers.LOVResolver.ObjectTable("QuotePackage").ObjectField("PackageTypeId").Index(index).Type("Animals");
            Resolvers.WindowResolver.Index(2).CloseOk();
            Resolvers.WindowResolver.Index(2).ShouldBeClosed();
        }
    }
    Save() {

        return new Cypress.Promise((resolve, reject) => {

            cy.server();

            cy.route({
                method: 'POST',
                url: '**/quotes',
                onResponse: (xhr) => {
                    expect(xhr.status).to.eq(200);
                }
            }).as('Create')

            Resolvers.ButtonResolver.Selector('Button').Text('Create').Click();

            cy.wait('@Create').its('responseBody').then((json) => {
                //this.EntityId = json['Id'];
                //this.EntityNumber = json['QuoteNumber'];

                //Resolvers.WindowResolver.ShouldBeClosed();

                resolve(json['QuoteNumber']);
            });
        });

        // https://stackoverflow.com/questions/60031254/cypress-get-value-from-json-response-body
        // https://docs.cypress.io/guides/guides/network-requests.html#Assertions
        // https://docs.cypress.io/api/commands/route.html#Command-Log
        // https://docs.cypress.io/api/commands/request.html#Syntax
    }
}