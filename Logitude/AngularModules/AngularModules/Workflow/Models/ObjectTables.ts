import { Formatter } from "./Formatter";

export class ObjectTables {

    static getAll() {
        return (window as any).ObjectTables || [];
    }

    static getById(entityId: string) {
        if (entityId) {
            return this.getAll().find((o: any) => o.Id.toLowerCase() === entityId.toLowerCase()) || null;
        }
        return null;
    }

    static getByName(entityName: string) {
        if (entityName) {
            return this.getAll().find((o: any) => o.Name.toLowerCase() === entityName.toLowerCase()) || null;
        }
        return null;
    }

    static getIdByName(entityName: string) {
        if (entityName) {
            entityName = Formatter.getEntity(entityName);
            let entityObjectTable = this.getByName(entityName);
            return entityObjectTable ? entityObjectTable.Id : null;
        }
        return null;
    }

    static getKeyPropertyPathByName(entityName: string) {
        if (entityName) {
            entityName = Formatter.getEntity(entityName);
            let entityObjectTable = this.getByName(entityName);
            return entityObjectTable ? entityObjectTable.KeyPropertyPath : null;
        }
        return null;
    }

    static getNameById(id: string) {
        if (id) {
            let entityObjectTable = this.getById(id);
            return entityObjectTable ? entityObjectTable.Name : null;
        }
        return null;
    }

}