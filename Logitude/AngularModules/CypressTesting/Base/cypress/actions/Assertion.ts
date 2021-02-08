import { Interception } from "cypress/types/net-stubbing";

export function AssertStatusCode(requestAlias: string, expectedStatusCode: number): Cypress.Chainable<Interception> {
    var Interception = cy.wait("@" + requestAlias);
    Interception.then((interception) => {
        assert.equal(interception.response.statusCode, expectedStatusCode)
    })
    return Interception;
}

export function IsImport(direction: string) {
    return (direction === "Import" || direction === "I");
}

export function IsExport(direction: string) {
    return (direction === "Export" || direction === "E");
}

export function IsDrop(direction: string) {
    return (direction === "Drop" || direction === "R");
}

export function IsDomestic(direction: string) {
    return (direction === "Domestic" || direction === "D");
}

export function IsAir(transportMode: string) {
    return (transportMode === "Air" || transportMode === "A");
}

export function IsInland(transportMode: string) {
    return (transportMode === "Inland" || transportMode === "I");
}

export function IsOcean(transportMode: string) {
    return (transportMode === "Ocean" || transportMode === "O");
}

export function IsMaster(shipmentLevel: string) {
    return (shipmentLevel === "Master" || shipmentLevel === "C");
}

export function IsInlandDomestic(direction: string, transportMode: string) {
    return (IsDomestic(direction) && IsInland(transportMode));
}

export function IsFCL(shipmentType: string) {
    return shipmentType === "FCLD";
}

export function IsFTL(shipmentType: string) {
    return shipmentType === "FTL";
}

export function IsLCL(shipmentType: string) {
    return shipmentType === "LCLD";
}

export function IsLTL(shipmentType: string) {
    return shipmentType === "LTL";
}

export function IsGroupage(shipmentType: string) {
    return shipmentType === "Groupage";
}
export function hasPacakageType(shipmentType: string) {
    return IsFCL(shipmentType) || IsFTL(shipmentType) || IsLCL(shipmentType) || IsLTL(shipmentType);
}