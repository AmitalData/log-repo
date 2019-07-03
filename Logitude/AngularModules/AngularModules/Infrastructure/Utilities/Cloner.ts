export class Cloner {
    private Object: any = null;
    private Fields: ClonerField[] = [];
    private Entities: ClonerEntity[] = [];
    constructor(object: any) {
        this.Object = object;
    }

    public AddField(fieldName: string) {
        if (fieldName != null) {
            if (this.Object != null) {

                var existsField = this.Fields.filter(f => f.FieldName == fieldName)[0];

                if (existsField) {
                    existsField.FieldValue = this.Object[fieldName];
                }

                else {
                    this.Fields.push(new ClonerField(fieldName, this.Object[fieldName]));
                }
            }
        }
    }
    public AddEntity(entity: any) {
        if (entity != null) {
            if (this.Object != null) {
                this.Entities.push(new ClonerEntity(entity));
            }
        }
    }
    public RejectChanges() {
     
            if (this.Object != null) {

                this.Fields.forEach(item => {
                    var currentValue = this.Object[item.FieldName];
                    if (currentValue != item.FieldValue) {
                        this.Object[item.FieldName] = item.FieldValue;

                    }
                });

                this.Entities.forEach(item => {
                    if (item != null) {
                        var currentValue = item.Entity['IsDirty']

                        if (currentValue != item.IsDirty) {
                            item.Entity['IsDirty'] = item.IsDirty;
                        }
                    }
                });
            }
       
    }
}
class ClonerField {
    public FieldName: string;
    public FieldValue: any;
    constructor(name: string, value: any) {
        this.FieldName = name;
        this.FieldValue = value;
    }
}

class ClonerEntity {
    public Entity: any = null;
    public IsDirty: boolean = false;
    constructor(entity: any) {
        this.Entity = entity;
        this.IsDirty = entity['IsDirty'];
    }
}
