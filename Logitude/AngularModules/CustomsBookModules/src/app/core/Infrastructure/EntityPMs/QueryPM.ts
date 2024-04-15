import { SharedUserQueryPM } from './SharedUserQueryPM';
import { Output, EventEmitter } from '@angular/core';
import { PropertyChangedArgs } from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
// import { UIProperties } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { ServiceLocator } from '../../Infrastructure/Locators/ServiceLocator';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';

export class QueryPM {
	//
	constructor() {
		//
		this.IsDirty = false;
	}

	public OldEntityPM: QueryPM;

	private id: string;
	public get Id() {
		return this.id;
	}
	public set Id(newValue: string) {
		this.id = newValue;
	}

	private tenant: number;
	public get Tenant() {
		return this.tenant;
	}
	public set Tenant(newValue: number) {
		this.tenant = newValue;
	}

	private code: string;
	public get Code() {
		return this.code;
	}
	public set Code(newValue: string) {
		this.code = newValue;
	}

	private uniqueCode: string;
	public get UniqueCode() {
		return this.uniqueCode;
	}
	public set UniqueCode(newValue: string) {
		this.uniqueCode = newValue;
	}

	private objectTableId: string;
	public get ObjectTableId() {
		return this.objectTableId;
	}
	public set ObjectTableId(newValue: string) {
		this.objectTableId = newValue;
	}

	private systemLevel: boolean;
	public get SystemLevel() {
		return this.systemLevel;
	}
	public set SystemLevel(newValue: boolean) {
		this.systemLevel = newValue;
	}

	private tenantLevel: boolean;
	public get TenantLevel() {
		return this.tenantLevel;
	}
	public set TenantLevel(newValue: boolean) {
		this.tenantLevel = newValue;
	}

	private originalQueryId: string;
	public get OriginalQueryId() {
		return this.originalQueryId;
	}
	public set OriginalQueryId(newValue: string) {
		this.originalQueryId = newValue;
	}

	private originalQueryCode: string;
	public get OriginalQueryCode() {
		return this.originalQueryCode;
	}
	public set OriginalQueryCode(newValue: string) {
		this.originalQueryCode = newValue;
	}

	private querySection: string;
	public get QuerySection() {
		return this.querySection;
	}
	public set QuerySection(newValue: string) {
		this.querySection = newValue;
	}

	private indexOrder: number;
	public get IndexOrder() {
		return this.indexOrder;
	}
	public set IndexOrder(newValue: number) {
		this.indexOrder = newValue;
	}

	private displayCount: boolean;
	public get DisplayCount() {
		return this.displayCount;
	}
	public set DisplayCount(newValue: boolean) {
		this.displayCount = newValue;
	}

	private objectTableName: string;
	public get ObjectTableName() {
		return this.objectTableName;
	}
	public set ObjectTableName(newValue: string) {
		this.objectTableName = newValue;
	}

	private objectTableIsNewWizard: boolean;
	public get ObjectTableIsNewWizard() {
		return this.objectTableIsNewWizard;
	}
	public set ObjectTableIsNewWizard(newValue: boolean) {
		this.objectTableIsNewWizard = newValue;
	}

	private objectTableNewWizardControlName: string;
	public get ObjectTableNewWizardControlName() {
		return this.objectTableNewWizardControlName;
	}
	public set ObjectTableNewWizardControlName(newValue: string) {
		this.objectTableNewWizardControlName = newValue;
	}

	private queryGroupCode: string;
	public get QueryGroupCode() {
		return this.queryGroupCode;
	}
	public set QueryGroupCode(newValue: string) {
		this.queryGroupCode = newValue;
	}

	private queryGroupIndexOrder: number;
	public get QueryGroupIndexOrder() {
		return this.queryGroupIndexOrder;
	}
	public set QueryGroupIndexOrder(newValue: number) {
		this.queryGroupIndexOrder = newValue;
	}

	private isAddNewEntityEnabled: boolean;
	public get IsAddNewEntityEnabled() {
		return this.isAddNewEntityEnabled;
	}
	public set IsAddNewEntityEnabled(newValue: boolean) {
		this.isAddNewEntityEnabled = newValue;
	}

	private userId: string;
	public get UserId() {
		return this.userId;
	}
	public set UserId(newValue: string) {
		this.userId = newValue;
	}

	private nameTextCodeId: string;
	public get NameTextCodeId() {
		return this.nameTextCodeId;
	}
	public set NameTextCodeId(newValue: string) {
		this.nameTextCodeId = newValue;
	}

	private nameTextCodeCode: string;
	public get NameTextCodeCode() {
		return this.nameTextCodeCode;
	}
	public set NameTextCodeCode(newValue: string) {
		this.nameTextCodeCode = newValue;
	}

	private defaultSortDirection: string;
	public get DefaultSortDirection() {
		return this.defaultSortDirection;
	}
	public set DefaultSortDirection(newValue: string) {
		this.defaultSortDirection = newValue;
	}

	private defaultSortColumn: string;
	public get DefaultSortColumn() {
		return this.defaultSortColumn;
	}
	public set DefaultSortColumn(newValue: string) {
		this.defaultSortColumn = newValue;
	}

	private spotlightDataTemplate: string;
	public get SpotlightDataTemplate() {
		return this.spotlightDataTemplate;
	}
	public set SpotlightDataTemplate(newValue: string) {
		this.spotlightDataTemplate = newValue;
	}

	private internal: boolean;
	public get Internal() {
		return this.internal;
	}
	public set Internal(newValue: boolean) {
		this.internal = newValue;
	}

	private customer: boolean;
	public get Customer() {
		return this.customer;
	}
	public set Customer(newValue: boolean) {
		this.customer = newValue;
	}

	private agent: boolean;
	public get Agent() {
		return this.agent;
	}
	public set Agent(newValue: boolean) {
		this.agent = newValue;
	}

	private isHiddenFromView: boolean;
	public get IsHiddenFromView() {
		return this.isHiddenFromView;
	}
	public set IsHiddenFromView(newValue: boolean) {
		this.isHiddenFromView = newValue;
	}

	private isNewFromTenantZeroOnly: boolean;
	public get IsNewFromTenantZeroOnly() {
		return this.isNewFromTenantZeroOnly;
	}
	public set IsNewFromTenantZeroOnly(newValue: boolean) {
		this.isNewFromTenantZeroOnly = newValue;
	}

	private featureId: string;
	public get FeatureId() {
		return this.featureId;
	}
	public set FeatureId(newValue: string) {
		this.featureId = newValue;
	}

	private editWizardName: string;
	public get EditWizardName() {
		return this.editWizardName;
	}
	public set EditWizardName(newValue: string) {
		this.editWizardName = newValue;
	}

	private perspective: string;
	public get Perspective() {
		return this.perspective;
	}
	public set Perspective(newValue: string) {
		this.perspective = newValue;
	}

	private newViewName: string;
	public get NewViewName() {
		return this.newViewName;
	}
	public set NewViewName(newValue: string) {
		this.newViewName = newValue;
	}

	private displayAsCustom: boolean;
	public get DisplayAsCustom() {
		return this.displayAsCustom;
	}
	public set DisplayAsCustom(newValue: boolean) {
		this.displayAsCustom = newValue;
	}

	private isDummy: boolean;
	public get IsDummy() {
		return this.isDummy;
	}
	public set IsDummy(newValue: boolean) {
		this.isDummy = newValue;
	}

	private editWizardComponentPath: string;
	public get EditWizardComponentPath() {
		return this.editWizardComponentPath;
	}
	public set EditWizardComponentPath(newValue: string) {
		this.editWizardComponentPath = newValue;
	}

	private sharedWithAll: boolean;
	public get SharedWithAll() {
		return this.sharedWithAll;
	}
	public set SharedWithAll(newValue: boolean) {
		this.sharedWithAll = newValue;
	}

	private sharedWithSpecificUsers: boolean;
	public get SharedWithSpecificUsers() {
		return this.sharedWithSpecificUsers;
	}
	public set SharedWithSpecificUsers(newValue: boolean) {
		this.sharedWithSpecificUsers = newValue;
	}

	private sharedByUserId: string;
	public get SharedByUserId() {
		return this.sharedByUserId;
	}
	public set SharedByUserId(newValue: string) {
		this.sharedByUserId = newValue;
	}

	private sharedByUserName: string;
	public get SharedByUserName() {
		return this.sharedByUserName;
	}
	public set SharedByUserName(newValue: string) {
		this.sharedByUserName = newValue;
	}

	private sharedByUserEmail: string;
	public get SharedByUserEmail() {
		return this.sharedByUserEmail;
	}
	public set SharedByUserEmail(newValue: string) {
		this.sharedByUserEmail = newValue;
	}

	private spotlightModeActivated: boolean;
	public get SpotlightModeActivated() {
		return this.spotlightModeActivated;
	}
	public set SpotlightModeActivated(newValue: boolean) {
		this.spotlightModeActivated = newValue;
	}

	private featureUniqeCode: string;
	public get FeatureUniqeCode() {
		return this.featureUniqeCode;
	}
	public set FeatureUniqeCode(newValue: string) {
		this.featureUniqeCode = newValue;
	}

	private isViewOnly: boolean;
	public get IsViewOnly() {
		return this.isViewOnly;
	}
	public set IsViewOnly(newValue: boolean) {
		this.isViewOnly = newValue;
	}

	private sharedUserQueries: SharedUserQueryPM[];
	get SharedUserQueries() {
		if (this.sharedUserQueries == null) {
			this.sharedUserQueries = [];
		}

		return this.sharedUserQueries;
	}
	set SharedUserQueries(newValue: SharedUserQueryPM[]) {
		if (this.sharedUserQueries != newValue) {
			this.sharedUserQueries = newValue;
		}
	}
	public AddSharedUserQueryPM(item: SharedUserQueryPM) {
		if (item != null) {
			var index = this.SharedUserQueries.indexOf(item);
			if (index == -1) {
				item.EntityParentPM = this;

				this.SharedUserQueries.push(item);
				this.MarkAsDirty();
			}
		}
	}
	public RemoveSharedUserQueryPM(item: SharedUserQueryPM) {
		if (item != null) {
			var index = this.SharedUserQueries.indexOf(item);
			if (index > -1) {
				this.SharedUserQueries.splice(index, 1);
				this.MarkAsDirty();
			}
		}
	}

	public IsDirty: boolean;
	MarkAsDirty(propertyName: string = null) {
		this.IsDirty = true;

		if (propertyName != null) {
			ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, 'Query');
		}
	}

	private MyClone: QueryPM;

	public CloneMe() {
		ServiceHelper.CloneEntityPM(this);
	}

	public RejectChanges() {
		ServiceHelper.RejectEntityPMChanges(this);
	}
}
