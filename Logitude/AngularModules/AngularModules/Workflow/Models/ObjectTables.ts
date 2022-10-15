export class ObjectTables {

    static getAll() {
        return (window as any).ObjectTables || [];
    }

    static getById(entityId: string) {
        if (entityId) {
            return this.getAll().filter((o: any) => o.Id.toLowerCase() === entityId.toLowerCase())[0] || null;
        }
        return null;
    }

    static getByName(entityName: string) {
        if (entityName) {
            return this.getAll().filter((o: any) => o.Name.toLowerCase() === entityName.toLowerCase())[0] || null;
        }
        return null;
    }

}