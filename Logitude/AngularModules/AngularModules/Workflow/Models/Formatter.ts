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

}