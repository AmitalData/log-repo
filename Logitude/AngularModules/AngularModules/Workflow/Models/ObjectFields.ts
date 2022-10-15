import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";

export class ObjectFields {

    private static Fields: ObjectFieldPM[] | null = null;

    static isLoaded() {
        return this.Fields !== null;
    }

    static set(objectFields: ObjectFieldPM[]) {
        return this.Fields = objectFields;
    }

    static getAll() {
        return this.Fields || [];
    }

}