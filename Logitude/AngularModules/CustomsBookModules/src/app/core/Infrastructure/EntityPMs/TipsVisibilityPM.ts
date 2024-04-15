export class TipsVisibilityPM {
	constructor() {
		this.UIProperties = new UIProperties();
		this.IsDirty = false;
	}

	private id: string;
	public get Id() {
		return this.id;
	}
	public set Id(newValue: string) {
		this.id = newValue;
		this.MarkAsDirty();
	}

	private tenant: number;
	public get Tenant() {
		return this.tenant;
	}
	public set Tenant(newValue: number) {
		this.tenant = newValue;
		this.MarkAsDirty();
	}

	private userId: string;
	public get UserId() {
		return this.userId;
	}
	public set UserId(newValue: string) {
		this.userId = newValue;
		this.MarkAsDirty();
	}

	private tipCode: string;
	public get TipCode() {
		return this.tipCode;
	}
	public set TipCode(newValue: string) {
		this.tipCode = newValue;
		this.MarkAsDirty();
	}

	private isVisible: boolean;
	public get IsVisible() {
		return this.isVisible;
	}
	public set IsVisible(newValue: boolean) {
		this.isVisible = newValue;
		this.MarkAsDirty();
	}

	public OldEntityPM: TipsVisibilityPM;

	public IsDirty: boolean;
	MarkAsDirty() {
		this.IsDirty = true;
	}
}
