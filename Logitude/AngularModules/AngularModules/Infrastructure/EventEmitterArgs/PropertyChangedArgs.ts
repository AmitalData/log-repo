export class PropertyChangedArgs {
    public PropertyName: string;
    public EntityPM: any;

    constructor(propertyName: string, entityPM: any) {
        this.PropertyName = propertyName;
        this.EntityPM = entityPM;
    }
}