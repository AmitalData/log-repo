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

    static resetCustom(customObjectFields: ObjectFieldList[]) {
        if (customObjectFields) {
            this.AllObjectFields = this.AllObjectFields.filter(o => !o.IsCustom);
            this.AllObjectFields = this.AllObjectFields.concat(customObjectFields.filter(o => o.IsCustom));
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