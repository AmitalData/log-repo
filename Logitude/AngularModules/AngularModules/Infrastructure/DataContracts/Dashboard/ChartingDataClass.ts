export class ChartingDataClass {

    private id: string;
    private groupedId: string;
    private integerProperty: number;
    private dataTypeCode: string;
    private labelProperty: string;
    private stringProperty: string;
    private doubleProperty: number;
    private decimalProperty: number;
    private dateTimeProperty: Date;
    private shortLabelProperty: string;
    private indexOrder: number;
    private day: number;
    private month: number;
    private year: number;
    private shipments: number;
    private grossWeight: number;
    private chargeableWeight: number;
    private receivablesInLocal: number;
    private receivablesInProfit: number;
    private profitInLocal: number;
    private profitInProfit: number;
    private typeIndex: number;
    private ownerId: string;
    private businessUnitId: string;
    private code: string;
    private classificationId: string;
    private ticketStageId: string;
    private labelColor: string;
    private employeeGroupId: string;
    private severityId: string;
    private ticketTypeId: string;
    private dateRange: string;
    private groupByCode: string;
    private participantId: string;
    private timeProperty: Date;


    public get Id() { return this.id; }
    public set Id(value: string) {
        this.id = value;
    }

    public get GroupedId() { return this.groupedId; }
    public set GroupedId(value: string) {
        this.groupedId = value;
    }

    public get IntegerProperty() { return this.integerProperty; }
    public set IntegerProperty(value: number) {
        this.integerProperty = value;
    }

    public get DataTypeCode() { return this.dataTypeCode; }
    public set DataTypeCode(value: string) {
        this.dataTypeCode = value;
    }

    public get LabelProperty() { return this.labelProperty; }
    public set LabelProperty(value: string) {
        this.labelProperty = value;
    }

    public get DoubleProperty() { return this.doubleProperty; }
    public set DoubleProperty(value: number) {
        this.doubleProperty = value;
    }

    public get StringProperty() { return this.stringProperty; }
    public set StringProperty(value: string) {
        this.stringProperty = value;
    }

    public get DecimalProperty() { return this.decimalProperty; }
    public set DecimalProperty(value: number) {
        this.decimalProperty = value;
    }


    public get DateTimeProperty() { return this.dateTimeProperty; }
    public set DateTimeProperty(value: Date) {
        this.dateTimeProperty = value;
    }

    public get ShortLabelProperty() { return this.shortLabelProperty; }
    public set ShortLabelProperty(value: string) {
        this.shortLabelProperty = value;
    }


    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(value: number) {
        this.indexOrder = value;
    }


    public get Day() { return this.day; }
    public set Day(value: number) {
        this.day = value;
    }

    public get Month() { return this.month; }
    public set Month(value: number) {
        this.month = value;
    }


    public get Shipments() { return this.shipments; }
    public set Shipments(value: number) {
        this.shipments = value;
    }


    public get Year() { return this.year; }
    public set Year(value: number) {
        this.year = value;
    }

    public get GrossWeight() { return this.grossWeight; }
    public set GrossWeight(value: number) {
        this.grossWeight = value;
    }

    public get CargeableWeight() { return this.chargeableWeight; }
    public set ChargeableWeight(value: number) {
        this.chargeableWeight = value;
    }

    public get ReceivablesInLocal() { return this.receivablesInLocal; }
    public set ReceivablesInLocal(value: number) {
        this.receivablesInLocal = value;
    }

    public get ReceivablesInProfit() { return this.receivablesInProfit; }
    public set ReceivablesInProfit(value: number) {
        this.receivablesInProfit = value;
    }

    public get ProfitInLocal() { return this.profitInLocal; }
    public set ProfitInLocal(value: number) {
        this.profitInLocal = value;
    }

    public get ProfitInProfit() { return this.profitInProfit; }
    public set ProfitInProfit(value: number) {
        this.profitInProfit = value;
    }

    public get TypeIndex() { return this.typeIndex; }
    public set TypeIndex(value: number) {
        this.typeIndex = value;
    }

    public get OwnerId() { return this.ownerId; }
    public set OwnerId(value: string) {
        this.ownerId = value;
    }

    public get BusinessUnitId() { return this.businessUnitId; }
    public set BusinessUnitId(value: string) {
        this.businessUnitId = value;
    }

    public get Code() { return this.code; }
    public set Code(value: string) {
        this.code = value;
    }

    public get ClassificationId() { return this.classificationId; }
    public set ClassificationId(value: string) {
        this.classificationId = value;
    }

    public get TicketStageId() { return this.ticketStageId; }
    public set TicketStageId(value: string) {
        this.ticketStageId = value;
    }


    public get LabelColor() { return this.labelColor; }
    public set LabelColor(value: string) {
        this.labelColor = value;
    }


    public get EmployeeGroupId() { return this.employeeGroupId; }
    public set EmployeeGroupId(value: string) {
        this.employeeGroupId = value;
    }


    public get SeverityId() { return this.severityId; }
    public set SeverityId(value: string) {
        this.severityId = value;
    }


    public get TicketTypeId() { return this.ticketTypeId; }
    public set TicketTypeId(value: string) {
        this.ticketTypeId = value;
    }


    public get DateRange() { return this.dateRange; }
    public set DateRange(value: string) {
        this.dateRange = value;
    }


    public get GroupByCode() { return this.groupByCode; }
    public set GroupByCode(value: string) {
        this.groupByCode = value;
    }


    public get ParticipantId() { return this.participantId; }
    public set ParticipantId(value: string) {
        this.participantId = value;
    }


    public get TimeProperty() { return this.timeProperty; }
    public set TimeProperty(value: Date) {
        this.timeProperty = value;
    }

    private countryName: string;
    public get CountryName() { return this.countryName; }
    public set CountryName(value: string) {
        this.countryName = value;
    }

    private countryId: string;
    public get CountryId() { return this.countryId; }
    public set CountryId(value: string) {
        this.countryId = value;
    }

    private total: number;
    public get Total() { return this.total; }
    public set Total(value: number) {
        this.total = value;
    }

    private count_All: number;
    public get Count_All() { return this.count_All; }
    public set Count_All(value: number) {
        this.count_All = value;
    }

    private count_Converted: number;
    public get Count_Converted() { return this.count_Converted; }
    public set Count_Converted(value: number) {
        this.count_Converted = value;
    }

    private transportModeId: string;
    public get TransportModeId() { return this.transportModeId; }
    public set TransportModeId(value: string) {
        this.transportModeId = value;
    }

    private directionId: string;
    public get DirectionId() { return this.directionId; }
    public set DirectionId(value: string) {
        this.directionId = value;
    }
    
}
