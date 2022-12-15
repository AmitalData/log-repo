import { Pipe, PipeTransform } from "@angular/core";
import { Formatter } from "Workflow/Models/Formatter";

@Pipe({
    name: "EntityLabelPipe"
})

export class EntityLabelPipe implements PipeTransform {

    private EntityNamesDictionary: { [code: string]: string } = {
        "ShipmentPackage": "Package",
        "ShipmentReceivable": "Receivable",
        "ShipmentPayable": "Payable",
        "ARInvoice": "AR Invoice",
        "APInvoice": "AP Invoice"
    };

    transform(entity: string) {
        if (entity) {
            entity = Formatter.getEntity(entity);
            let entityName = this.EntityNamesDictionary[entity];
            return entityName ? entityName : entity;
        }
        return null;
    }

}