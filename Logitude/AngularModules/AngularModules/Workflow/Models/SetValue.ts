import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";

export class SetValue{    
    public id: number;
    public field: string;
    public operator: string;
    public value: string;
    public fieldType : string
    public fieldObjectField : ObjectFieldPM
}