export class ServiceResponse {
	constructor() {}
	public Data: any = null;
	public Result: any = null;
	public Count!: number;
	public HasError: boolean = false;
	public ErrorsArray: string[] = [];
	public CallTime!: Date;
}
