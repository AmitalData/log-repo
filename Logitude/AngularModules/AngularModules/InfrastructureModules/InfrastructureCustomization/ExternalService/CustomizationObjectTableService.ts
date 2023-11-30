import { ObjectFieldPM } from "../../../Infrastructure/EntityPMs/ObjectFieldPM";
import { ObjectTablePM } from "../../../Infrastructure/EntityPMs/ObjectTablePM";
declare var window: any;

export class CustomizationObjectTableService {

    constructor() {

    }
    public GetChildsById(objectTableId: string): Array<ObjectTablePM> {
        let childObjectTables: Array<ObjectTablePM> = [];

        let objectFields : ObjectFieldPM[] = window.ObjectFields.filter(d => d.ObjectTableId == objectTableId && d.IsMulti);

        objectFields.forEach((item) => {
                let objectTablePM = window.ObjectTables.filter(d => d.Id == item.MultiTableId && d.AvailableInCustomization == true)[0];
                if (objectTablePM != null) {
                    childObjectTables.push(objectTablePM);
                }
        });
        let newSubEntities = window.ObjectTables.filter(d => d.ParentObjectTableId == objectTableId && d.AvailableInCustomization == true);
        newSubEntities.forEach((item) => {
            childObjectTables.push(item);
        });

        return childObjectTables.filter(function (elem, index, self) {
            return index === self.indexOf(elem);
        });
    }

}
