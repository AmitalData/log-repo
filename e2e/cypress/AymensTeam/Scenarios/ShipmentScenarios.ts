import { Random } from "../@e2e/core";
import { Resolvers } from "../Resolvers/Resolvers";

export class ShipmentScenarios {
    private levelCode: string;
    private direction: string;
    private transportMode: string;
    private shipmentType: string;
    private objectTable: string;
    private isFCL: boolean = false;
    private isInlandDomestic: boolean = false;
    public CreateWizardShipment(levelCode: string, direction: string, transportMode: string, shipmentType: string = null) {

        this.levelCode = levelCode;
        this.direction = direction;
        this.transportMode = transportMode;
        this.shipmentType = shipmentType;
        this.objectTable = this.levelCode == "M" ? "Master" : "Shipment";

        if ((this.transportMode == "O" && this.shipmentType == "FCLD") || (this.transportMode == "I" && this.shipmentType == "FTL")) {
            this.isFCL = true;
        }

        if (this.direction == "D" && this.transportMode == "I") {
            if (this.levelCode == "D") {
                this.isInlandDomestic = true;
            }
        }

        this.OpenWizardWindow();
        this.FillRadioButtons();
        this.FillPartners();
        this.FillMainCarriage();
        this.FillGeneral();
        this.FillOrderDetails();
        //this.Save();
    }

    private OpenWizardWindow() {
        let index: number = 0;

        switch (this.levelCode) {
            case "D": { index = 0; break; }
            case "H": { index = 1; break; }
            case "M": { index = 2; break; }
        }

        Resolvers.ToggleButtonResolver.Selector('ToggleButton').SelectByIndex(index);
        //Resolvers.ToggleButtonResolver.Selector("ToggleButton").Parent('OperationsComponent').SelectByIndex(0);

        Resolvers.WindowResolver.ShouldBeOpend();
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

                if (this.levelCode == "M") {
                    cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "MyGO").should("not.exist");
                    cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "MyGI").should("not.exist");
                }

                break;
            }

            case "O": {
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FCLD").should("be.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LCLD").should("be.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FTL").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LTL").should("not.exist");

                if (this.levelCode == "M") {
                    cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "MyGO").should("be.exist");
                    cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "MyGI").should("not.exist");
                }

                break;
            }

            case "I": {
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FCLD").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LCLD").should("not.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "FTL").should("be.exist");
                cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "LTL").should("be.exist");

                if (this.levelCode == "M") {
                    cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "MyGO").should("not.exist");
                    cy.get("#ShipmentTypeRadio_" + Resolvers.Session + "MyGI").should("be.exist");
                }

                break;
            }
        }

        if (this.direction == "D" && this.levelCode != "D") {
            cy.get("#TransportModeRadio_" + Resolvers.Session + "I").should("not.exist");
        }
    }
    private FillPartners() {
        if (this.levelCode == "M") {
            this.FillAgent('TestAgentExport1');
        }

        else if (this.isInlandDomestic) {
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
    private FillAgent(name: string) {
        Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("AgentId").Type(name);
        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("AgentReference1").Type(Random.GetRandomNumber());
        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("AgentReference2").Type(Random.GetRandomNumber());
    }
    private FillShipper(name: string) {
        Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("ShipperId").Type(name);
        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("ShipperReference1").Type(Random.GetRandomNumber());
        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("ShipperReference2").Type(Random.GetRandomNumber());

        //Resolvers.LOVResolver.Selector('#Shipment_ShipperId').Type('TestShipper');
        //Resolvers.TextBoxResolver.Selector("#Shipment_ShipperReference1").Type(Helper.GetRandomNumber());
        //Resolvers.TextBoxResolver.Selector("#Shipment_ShipperReference2").Type(Helper.GetRandomNumber());
    }
    private FillConsignee(name: string) {
        Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("ConsigneeId").Type(name);
        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("ConsigneeReference1").Type(Random.GetRandomNumber());
        Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("ConsigneeReference2").Type(Random.GetRandomNumber());
    }
    private FillMainCarriage() {

        var fromPortCode: string = "EZE";
        var toPortCode: string = this.direction == "D" ? "EZE" : "MVD";
        var carrier: string;
        var carrierNumber: string;

        var fromPortLabel: string;
        var toPortLabel: string;
        var carrierLabel: string;
        var carrierNumberLabel: string;

        switch (this.transportMode) {
            case "A": {
                carrier = "BA";
                carrierNumber = "115";
                fromPortLabel = "Gateway";
                toPortLabel = "Destination";
                carrierLabel = "Airline";
                carrierNumberLabel = "Flight No";
                break;
            }

            case "O": {
                carrier = "MAEU";
                carrierNumber = "Voyage 1";
                fromPortLabel = "Loading Port";
                toPortLabel = "Discharge Port";
                carrierLabel = "Shipping line";
                carrierNumberLabel = "Voyage No";
                break;
            }

            default: {
                carrier = "Trucker1London";
                carrierNumber = "Trucker # 1";
                fromPortLabel = "From";
                toPortLabel = "To";
                carrierLabel = "Trucker";
                carrierNumberLabel = "Trucker No";
                break;
            }
        }

        if (this.isInlandDomestic) {
            cy.get('label').contains('Include PickUp').should('not.exist');
            cy.get('label').contains('Include Delivery').should('not.exist');
            cy.get('loglabel[ng-reflect--object-field-name="MainCarriageFromPortId"]').find('label').contains(fromPortLabel).should('not.exist');
            cy.get('loglabel[ng-reflect--object-field-name="MainCarriageToPortId"]').find('label').contains(toPortLabel).should('not.exist');
        }

        else {
            cy.get('loglabel[ng-reflect--object-field-name="MainCarriageFromPortId"]').find('label').contains(fromPortLabel).should('be.exist');
            cy.get('loglabel[ng-reflect--object-field-name="MainCarriageToPortId"]').find('label').contains(toPortLabel).should('be.exist');
            Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("MainCarriageFromPortId").Type(fromPortCode);
            Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("MainCarriageToPortId").Type(toPortCode);
        }

        if (this.levelCode != "H") {
            cy.get('loglabel[ng-reflect--object-field-name="MainCarriageCarrierId"]').find('label').contains(carrierLabel).should('be.exist');
            cy.get('loglabel[ng-reflect--object-field-name="MainCarriageCarrierNumber"]').find('label').contains(carrierNumberLabel).should('be.exist');
            Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("MainCarriageCarrierId").Type(carrier);
            Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("MainCarriageCarrierNumber").Type(carrierNumber);
        }
    }
    private FillGeneral() {
        if (this.levelCode != "M") {
            Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("IncotermId").SelectFirst();
            Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("MoveTypeId").SelectFirst();
        }
    }
    private FillOrderDetails() {

        if (this.isFCL) {
            this.FillContainers();
        }

        else {
            if (this.levelCode == "M") {
                Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("OrderGrossWeight").Type("500");
                Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("BookingVolume").Type("1");
                Resolvers.TextBoxResolver.ObjectTable(this.objectTable).ObjectField("BookingNumberOfPackages").Type("1");
            }

            else {
                this.FillDimentions();
            }
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

        var index: number;

        index = 0;
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Quantity").Index(index).Type("1");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Length").Index(index).Type("100");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Width").Index(index).Type("100");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Height").Index(index).Type("100");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("GrossWeight").Index(index).Type("250");

        index = 1;
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Quantity").Index(index).Type("1");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Length").Index(index).Type("50");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Width").Index(index).Type("50");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Height").Index(index).Type("50");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("GrossWeight").Index(index).Type("150");

        Resolvers.GridViewResolver.DeleteSimpleGridRow(4);
        Resolvers.GridViewResolver.DeleteSimpleGridRow(3);
        Resolvers.GridViewResolver.DeleteSimpleGridRow(2);

        //cy.get('button').contains('Add Package').click();
        Resolvers.ButtonResolver.Selector('Button').Text('Add Package').Click();
        Resolvers.WindowResolver.Index(2).ShouldBeOpend();

        index = 2;
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Quantity").Index(index).Type("1");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Length").Index(index).Type("200");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Width").Index(index).Type("200");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("Height").Index(index).Type("200");
        Resolvers.TextBoxResolver.ObjectTable("ShipmentOrderPackage").ObjectField("GrossWeight").Index(index).Type("400");

        Resolvers.WindowResolver.Index(2).CloseOk();
        Resolvers.WindowResolver.Index(2).ShouldBeClosed();

        Resolvers.WindowResolver.Index(1).CloseOk();
        Resolvers.WindowResolver.Index(1).ShouldBeClosed();

        ////cy.get('button').should('have.class', 'RedButton').contains('Ok').click();
    }
    private Save() {
        cy.server();
        cy.route({
            method: 'POST',
            url: '**/shipment',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('CreateShipment')

        cy.get('#ShipmentCreatebtn').click();
        cy.wait('@CreateShipment');

        Resolvers.WindowResolver.ShouldBeClosed();
    }
}