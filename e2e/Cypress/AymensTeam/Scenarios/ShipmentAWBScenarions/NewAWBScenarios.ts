import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";


export class NewAWBScenarios {
    private levelCode: string;

    public RunScenario(levelCode: string) {
        this.levelCode = levelCode;

        this.OpenWizardWindow();
        this.FillPartners();
        this.FillRoutings();
        this.FillPackages();
        this.FillFreightCharges();
        this.FillOtherCharges();
        //this.FillRADetails();
        this.FillGeneralDetails();
        this.SaveShipment();
    }

    private OpenWizardWindow() {
        let index: number = 0;

        switch (this.levelCode) {
            case "D": { index = 0; break; }
            case "H": { index = 1; break; }
            case "M": { index = 2; break; }
        }
        Resolvers.ToggleButtonResolver.Selector('.ToggleButton').SelectByIndex(index);
        Resolvers.WindowResolver.ShouldBeOpend();
    }
    private FillPartners() {
        this.FillShipper('TestShipper');
        this.FillConsignee('TestConsi');
    }
    private FillShipper(name: string) {
        Resolvers.LOVResolver.Selector('#Shipment_ShipperId').Type(name);
        Resolvers.TextBoxResolver.Selector("#Shipment_ShipperReference1").Type(Random.GetRandomNumber());
    }
    private FillConsignee(name: string) {
        Resolvers.LOVResolver.Selector('#Shipment_ConsigneeId').Type(name);
        Resolvers.TextBoxResolver.Selector("#Shipment_ConsigneeReference1").Type(Random.GetRandomNumber());
    }
    private FillRoutings() {

        var fromPortCode: string = "EZE";
        var toPortCode: string = "MVD";
        var carrier: string = "AR";
        var carrierNumber: string = "1250";

        cy.get('#ROU').click();
        if (this.levelCode == "M") {
            Resolvers.LOVResolver.Selector('#Master_MainCarriageFromPortId').Type(fromPortCode);
            Resolvers.LOVResolver.Selector('#Master_MainCarriageToPortId').Type(toPortCode);
        }
        else {
            Resolvers.LOVResolver.Selector('#Shipment_MainCarriageFromPortId').Type(fromPortCode);
            Resolvers.LOVResolver.Selector('#Shipment_MainCarriageFinalDestinationPortId').Type(toPortCode);
        }

        if (this.levelCode == "D") {
            Resolvers.LOVResolver.Selector('#Shipment_MainCarriageCarrierId').Type(carrier);
            Resolvers.TextBoxResolver.Selector('#Shipment_MainCarriageCarrierNumber').Type(carrierNumber);
            Resolvers.TextBoxResolver.Selector('#Shipment_Master').Type(Random.GetRandom());
            Resolvers.DatePickerResolver.Selector('#date_Shipment_MAWBOBLDate').Type('.');
            Resolvers.DatePickerResolver.Selector('#date_Shipment_MainCarriageETD').Type('.');

        } else if (this.levelCode == "M") {
            Resolvers.LOVResolver.Selector('#Master_MainCarriageCarrierId').Type(carrier);
            Resolvers.TextBoxResolver.Selector('#Master_MainCarriageCarrierNumber').Type(carrierNumber);
        }
    }
    private FillPackages() {
        cy.get('#PAC').click();
        this.AddPackage('5', null, null, null, '10', '100')
    }
    private AddPackage(quantity: string, length: string = null, width: string = null, hight: string = null, volume: string = null, grossweight: string = null) {
        Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Quantity').Type(quantity);
        if (volume == null) {
            Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Length').Type(length);
            Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Width').Type(width);
            Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Height').Type(hight);
        } else {
            Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Volume').Type(volume);
        }
        Resolvers.TextBoxResolver.Selector('#ShipmentPackage_Weight').Type(grossweight);
    }
    private FillFreightCharges() {
        cy.get('#FRE').click();
        Resolvers.TextBoxResolver.Selector('#Shipment_AWBChargeRate').Type('1');

    }
    private FillOtherCharges() {
        cy.get('#OTC').click();
        Resolvers.ButtonResolver.Selector('#AddCharge').Click();
        Resolvers.LOVResolver.Selector('#ShipmentAWBPrintOnly_IATACodeId').Type('a');
        Resolvers.LOVResolver.Selector('#ShipmentAWBPrintOnly_DueTypeCode').Type('Ag');
        Resolvers.TextBoxResolver.Selector('#ShipmentAWBPrintOnly_Quantity').Type('10');
        Resolvers.TextBoxResolver.Selector('#ShipmentAWBPrintOnly_UnitPrice').Type('5');
        Resolvers.ButtonResolver.Selector('#OkAddCharge').Click();
    }
    private FillRADetails() {
        cy.get('#RAD').click();
        Resolvers.TextBoxResolver.Selector('#Shipment_RegulatedAgentRANumber').Type('RAN');
        Resolvers.TextBoxResolver.Selector('#Shipment_KnownConsignorNumber').Type('Cos Num');
    }
    private FillGeneralDetails() {
        cy.get('#GEN').click();
        Resolvers.TextBoxResolver.Selector('#Shipment_AWBSignature').Type('Razan');
    }
    private SaveShipment() {
        Resolvers.ButtonResolver.Selector('#SaveWizard').Click();
        //Resolvers.ButtonResolver.Selector('#SendFWB').Click();

    }
}