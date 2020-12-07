export function OpenShipment(dataFile: string){
    cy.fixture("ResponseData/" + dataFile + ".json").then((shipment) => {
        cy.SelectSearchBoxFirstElement("#Shipment_Search", shipment.ShipmentNumber)
    })
}

export function FillGeneralTab(){
    cy.Click("#ShipmentTHGeneral", null)
    cy.FillRandomNumber("#Shipment_GrossWeightInKG", 100, 1000, false)
    cy.SelectLogLovFirstElement("#Shipment_MoveTypeId")
}

export function FillOrdersTab(){
    cy.Click("#ShipmentTHOrders", null)
    AddPackages(3)
}

function AddPackages(numberOfPackages: number){
    for (let i = 0; i < numberOfPackages; i++){
        cy.Click("#Orders-AddPackage", null)
        cy.FillRandomNumber("#ShipmentOrderPackage_Quantity", 1, 10, true)
        cy.Click("#OrderOKbtn", null)
    }
}