import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";

export class ObjectFields {

    private static Fields: ObjectFieldList[] | null = null;

    static isLoaded() {
        return this.Fields !== null;
    }

    static set(objectFields: ObjectFieldList[]) {
        return this.Fields = objectFields;
    }

    static getAll() {
        return this.Fields || [];
    }

}