import { MenuButtonGroupPM } from './MenuButtonGroupPM';

export class MenuButtonPM {
	constructor(entityParent: MenuButtonGroupPM) {
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

	private eventCode: string;
	public get EventCode() {
		return this.eventCode;
	}
	public set EventCode(newValue: string) {
		this.eventCode = newValue;
		this.MarkAsDirty();
	}

	private labelTextCodeId: string;
	public get LabelTextCodeId() {
		return this.labelTextCodeId;
	}
	public set LabelTextCodeId(newValue: string) {
		this.labelTextCodeId = newValue;
		this.MarkAsDirty();
	}

	private labelTextCodeCode: string;
	public get LabelTextCodeCode() {
		return this.labelTextCodeCode;
	}
	public set LabelTextCodeCode(newValue: string) {
		this.labelTextCodeCode = newValue;
		this.MarkAsDirty();
	}

	private menuButtonGroupId: string;
	public get MenuButtonGroupId() {
		return this.menuButtonGroupId;
	}
	public set MenuButtonGroupId(newValue: string) {
		this.menuButtonGroupId = newValue;
		this.MarkAsDirty();
	}

	private parentMenuButtonId: string;
	public get ParentMenuButtonId() {
		return this.parentMenuButtonId;
	}
	public set ParentMenuButtonId(newValue: string) {
		this.parentMenuButtonId = newValue;
		this.MarkAsDirty();
	}

	private index: number;
	public get Index() {
		return this.index;
	}
	public set Index(newValue: number) {
		this.index = newValue;
		this.MarkAsDirty();
	}

	private isActive: boolean;
	public get IsActive() {
		return this.isActive;
	}
	public set IsActive(newValue: boolean) {
		this.isActive = newValue;
		this.MarkAsDirty();
	}

	private isHidden: boolean;
	public get IsHidden() {
		return this.isHidden;
	}
	public set IsHidden(newValue: boolean) {
		this.isHidden = newValue;
		this.MarkAsDirty();
	}

	private isDisabled: boolean;
	public get IsDisabled() {
		return this.isDisabled;
	}
	public set IsDisabled(newValue: boolean) {
		this.isDisabled = newValue;
		this.MarkAsDirty();
	}

	private featureId: string;
	public get FeatureId() {
		return this.featureId;
	}
	public set FeatureId(newValue: string) {
		this.featureId = newValue;
		this.MarkAsDirty();
	}

	private showMenuButton: boolean;
	public get ShowMenuButton() {
		return this.showMenuButton;
	}
	public set ShowMenuButton(newValue: boolean) {
		this.showMenuButton = newValue;
		this.MarkAsDirty();
	}

	private menuButtonType: string;
	public get MenuButtonType() {
		return this.menuButtonType;
	}
	public set MenuButtonType(newValue: string) {
		this.menuButtonType = newValue;
		this.MarkAsDirty();
	}

	private dropDownControl: string;
	public get DropDownControl() {
		return this.dropDownControl;
	}
	public set DropDownControl(newValue: string) {
		this.dropDownControl = newValue;
		this.MarkAsDirty();
	}

	private style: string;
	public get Style() {
		return this.style;
	}
	public set Style(newValue: string) {
		this.style = newValue;
		this.MarkAsDirty();
	}

	private width: number;
	public get Width() {
		return this.width;
	}
	public set Width(newValue: number) {
		this.width = newValue;
		this.MarkAsDirty();
	}

	private controlPath: string;
	public get ControlPath() {
		return this.controlPath;
	}
	public set ControlPath(newValue: string) {
		this.controlPath = newValue;
		this.MarkAsDirty();
	}

	private htmlComponentPath: string;
	public get HtmlComponentPath() {
		return this.htmlComponentPath;
	}
	public set HtmlComponentPath(newValue: string) {
		this.htmlComponentPath = newValue;
		this.MarkAsDirty();
	}

	private displayText: string;
	public get DisplayText() {
		return this.displayText;
	}
	public set DisplayText(newValue: string) {
		this.displayText = newValue;
		this.MarkAsDirty();
	}

	private featureUniqeCode: string;
	public get FeatureUniqeCode() {
		return this.featureUniqeCode;
	}
	public set FeatureUniqeCode(newValue: string) {
		this.featureUniqeCode = newValue;
		this.MarkAsDirty();
	}

	public OldEntityPM: MenuButtonPM;

	public IsDirty: boolean;
	MarkAsDirty() {
		this.IsDirty = true;
	}
}
