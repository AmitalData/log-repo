declare var require: any

export function CloneDeep(object: any) {
    if (object) {
        let cloneDeep = require("lodash.clonedeep");
        let clonedObject = cloneDeep(object);
        return clonedObject;
    }
    return null;
}