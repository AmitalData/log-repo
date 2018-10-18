export interface EmployeeGroupList {

    Id: string;
    Tenant: number;
    CreateDate: Date;
    CreatedByUserId: string;
    UpdateDate: Date;
    UpdatedByUserId: string;
    SearchFields: string;
    Name: string;
    Description: string;
    Inactive: boolean;
    EscalationUserId: string;

}