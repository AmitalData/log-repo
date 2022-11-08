export class Formatter {

    static getFieldCode(field: string) {
        if (field) {
            if (field.indexOf("_") === -1) {
                return field;
            }
            let fieldCode = field.split("_")[1];
            return fieldCode;
        }
        return null;
    }

    static getEntity(entity: string) {
        if (entity) {
            if (entity.indexOf(".") !== -1) {
                let entities = entity.split(".");
                return entities[entities.length - 1];
            }
            return entity;
        }
        return null;
    }

}