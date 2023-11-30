import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";

export class ObjectFields {

    private static IsLoaded: boolean = false;
    private static AllObjectFields: ObjectFieldList[] = [];

    static setLoaded(loaded: boolean = true) {
        this.IsLoaded = loaded;
    }

    static isLoaded() {
        return this.IsLoaded;
    }

    static set(objectFields: ObjectFieldList[]) {
        if (objectFields) {
            this.AllObjectFields = objectFields;
        }
    }

    static replace(objectFields: ObjectFieldList[]) {
        if (objectFields) {
            let objectFieldsCodes = objectFields.map(o => { return o.FieldCode });
            this.AllObjectFields = this.AllObjectFields.filter(o => !objectFieldsCodes.includes(o.FieldCode)).concat(objectFields);
        }
    }

    static getAll() {
        return this.AllObjectFields;
    }

    static getByCode(code: string) {
        if (code) {
            return this.getAll().find(o => o.FieldCode === code) || null;
        }
        return null;
    }

    static getByObjectTableId(objectTableId: string) {
        if (objectTableId) {
            return this.getAll().filter(o => o.ObjectTableId === objectTableId);
        }
        return [];
    }

}