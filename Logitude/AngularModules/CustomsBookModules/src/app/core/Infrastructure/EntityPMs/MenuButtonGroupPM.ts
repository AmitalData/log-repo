import { MenuButtonPM } from './MenuButtonPM';

export class MenuButtonGroupPM {
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

	private name: string;
	public get Name() {
		return this.name;
	}
	public set Name(newValue: string) {
		this.name = newValue;
		this.MarkAsDirty();
	}

	private menuButtonGroupType: string;
	public get MenuButtonGroupType() {
		return this.menuButtonGroupType;
	}
	public set MenuButtonGroupType(newValue: string) {
		this.menuButtonGroupType = newValue;
		this.MarkAsDirty();
	}

	private objectTableId: string;
	public get ObjectTableId() {
		return this.objectTableId;
	}
	public set ObjectTableId(newValue: string) {
		this.objectTableId = newValue;
		this.MarkAsDirty();
	}

	private objectTableName: string;
	public get ObjectTableName() {
		return this.objectTableName;
	}
	public set ObjectTableName(newValue: string) {
		this.objectTableName = newValue;
		this.MarkAsDirty();
	}

	private menuButtons: MenuButtonPM[];
	get MenuButtons() {
		if (this.menuButtons == null) {
			this.menuButtons = [];
		}

		return this.menuButtons;
	}
	set MenuButtons(newValue: MenuButtonPM[]) {
		if (this.menuButtons != newValue) {
			this.menuButtons = newValue;
		}
	}

	public OldEntityPM: MenuButtonGroupPM;

	public IsDirty: boolean;
	MarkAsDirty() {
		this.IsDirty = true;
	}
}
