import { ObjectFieldPM } from "./ObjectFieldPM";

export class DeploymentPackageDetails {

    public Name: string;
    public Code: string;
    public Description: string;
    public CustomFields: Array<ObjectFieldPM>;
    constructor() {
        this.Name = "";
        this.Code = "";
        this.Description = "";
        this.CustomFields = [];
    }

}
