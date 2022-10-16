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

}