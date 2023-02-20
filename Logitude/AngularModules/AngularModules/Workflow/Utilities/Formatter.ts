export class Formatter {

    static getFieldCode(field: string) {
        if (field) {
            if (field.indexOf("_") !== -1) {
                let fieldSections = field.split("_");
                return fieldSections[fieldSections.length - 1];
            }
            return field;
        }
        return null;
    }

    static getCodeFromName(name: string, splitter: string = "_") {
        if (name) {
            return name.replace(/\ /gi, "").replace(new RegExp(splitter, "gi"), "").toLowerCase();
        }
        return null;
    }

    static removeSpaces(name: string) {
        if (name) {
            return name.replace(/\ /gi, "");
        }
        return null;
    }

}