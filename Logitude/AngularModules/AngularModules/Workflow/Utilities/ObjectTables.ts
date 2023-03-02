import { ObjectTableList } from "Infrastructure/EntityLists/ObjectTableList";

export class ObjectTables {

    private static IsLoaded: boolean = false;
    private static AllObjectTables: ObjectTableList[] = [];

    static setLoaded(loaded: boolean = true) {
        this.IsLoaded = loaded;
    }

    static isLoaded() {
        return this.IsLoaded;
    }

    static set(objectTables: ObjectTableList[]) {
        if (objectTables) {
            this.AllObjectTables = objectTables;
        }
    }

    static replace(objectTables: ObjectTableList[]) {
        if (objectTables) {
            let objectTablesNames = objectTables.map(o => { return o.Name });
            this.AllObjectTables = this.AllObjectTables.filter(o => !objectTablesNames.includes(o.Name)).concat(objectTables);
        }
    }

    static getAll() {
        return this.AllObjectTables;
    }

    static getById(id: string) {
        if (id) {
            return this.getAll().find(o => o.Id === id) || null;
        }
        return null;
    }

    static getByName(name: string) {
        if (name) {
            return this.getAll().find(o => o.Name === name) || null;
        }
        return null;
    }

    static getIdByName(name: string) {
        if (name) {
            let objectTable = this.getByName(name);
            return objectTable ? objectTable.Id : null;
        }
        return null;
    }

    static getKeyPropertyPathByName(name: string) {
        if (name) {
            let objectTable = this.getByName(name);
            if (objectTable && objectTable.IsCustom && !objectTable.KeyPropertyPath) {
                return "Id";
            }
            return objectTable ? objectTable.KeyPropertyPath : null;
        }
        return null;
    }

    static getNameById(id: string) {
        if (id) {
            let objectTable = this.getById(id);
            return objectTable ? objectTable.Name : null;
        }
        return null;
    }

    static getDisplayNameById(id: string) {
        if (id) {
            let objectTable = this.getById(id);
            return objectTable ? (objectTable.FullNameTextCodeDefaultText || objectTable.Name) : null;
        }
        return null;
    }

    static getDisplayNameByName(name: string) {
        if (name) {
            let objectTable = this.getByName(name);
            return objectTable ? (objectTable.FullNameTextCodeDefaultText || objectTable.Name) : null;
        }
        return null;
    }

    static getIsCustomByName(name: string) {
        if (name) {
            let objectTable = this.getByName(name);
            return objectTable ? objectTable.IsCustom : null;
        }
        return null;
    }

}