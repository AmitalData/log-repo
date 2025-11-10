import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { CRMDomainService } from '../CRMDomainService';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';
import { ClassLevelValidator } from 'Infrastructure/Validators/ClassLevelValidator';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

describe('CRMDomainService', () => {
    const baseUrl = 'https://crm/';
    let httpClient: any;
    let validateSpy: any;

    beforeEach(() => {
        httpClient = {
            post: jest.fn(),
            get: jest.fn(),
        };
        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => {
            throw error;
        });
        SessionLocator.SelectedSession = {} as any;
        validateSpy = jest.spyOn(ClassLevelValidator.prototype, 'Validate').mockReturnValue([]);
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('InserNewTicket posts mapped entity when validation passes', done => {
        const service = new CRMDomainService();
        const entity: any = { Subject: 'Ticket subject' };
        const mapSpy = jest.spyOn(service as any, 'MapJsonToEntityPM').mockReturnValue({ Id: 'MAPPED' });
        httpClient.post.mockReturnValue(of({ Id: 'SERVER' }));

        service.InserNewTicket(entity).subscribe(res => {
            expect(validateSpy).toHaveBeenCalledWith('Ticket', entity);
            expect(mapSpy).toHaveBeenCalledWith(entity, false);
            const [url, body, headers] = httpClient.post.mock.calls[0];
            expect(url).toBe(`${baseUrl}api/CRMDomain/InserNewTicket?entityPM=${entity}`);
            expect(typeof body).toBe('string');
            expect(headers).toEqual({ headers: { Authorization: 'token' } });
            expect(res.Result).toEqual({ Id: 'MAPPED' });
            done();
        });
    });

    it('InserNewTicket returns validation errors without calling http', done => {
        validateSpy.mockReturnValue(['subject missing']);
        const service = new CRMDomainService();

        service.InserNewTicket({} as any).subscribe(res => {
            expect(httpClient.post).not.toHaveBeenCalled();
            expect(res.HasError).toBe(true);
            expect(res.ErrorsArray).toEqual(['subject missing']);
            done();
        });
    });

    it('GetActivitiesDashBoard wraps response in ServiceResponse', done => {
        httpClient.get.mockReturnValue(of([{ id: 1 }]));
        const service = new CRMDomainService();

        service.GetActivitiesDashBoard('OWNER', 'BU', 'TYPE', 'REC').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetActivitiesDashBoard?OwnerId=OWNER&BusinessUnitId=BU&activityTypeCodeFilter=TYPE&RecordsTypeCode=REC`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toEqual([{ id: 1 }]);
            done();
        });
    });

    it('GetOwnerEmployeeGroup forwards parameters', done => {
        httpClient.get.mockReturnValue(of({ group: 'G1' }));
        const service = new CRMDomainService();

        service.GetOwnerEmployeeGroup('OWN', 'GRP').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetOwnerEmployeeGroup?ownerId=OWN&employeeGroupId=GRP`,
                { headers: { Authorization: 'token' } }
            );
            expect(res).toEqual({ group: 'G1' });
            done();
        });
    });

    const chartCases = [
        {
            name: 'GetActivitiesChartData',
            call: (service: CRMDomainService) => service.GetActivitiesChartData('code', 'owner', 'bu', 'chart'),
            expectations: ['GetActivitiesChartData', 'code=code', 'ownerId=owner', 'businessUnitId=bu', 'chartCode=chart'],
        },
        {
            name: 'GetActivitiesChartDataCustom',
            call: (service: CRMDomainService) =>
                service.GetActivitiesChartDataCustom(new Date('2024-01-01'), new Date('2024-01-31'), 'owner', 'bu', 'chart'),
            expectations: ['GetActivitiesChartDataCustom', 'ownerId=owner', 'businessUnitId=bu', 'chartCode=chart'],
        },
        {
            name: 'GetQuotesChartData',
            call: (service: CRMDomainService) => service.GetQuotesChartData('code', 'owner', 'bu', 'chart'),
            expectations: ['GetQuotesChartData', 'code=code', 'ownerId=owner', 'businessUnitId=bu', 'chartCode=chart'],
        },
        {
            name: 'GetQuotesChartDataCustom',
            call: (service: CRMDomainService) =>
                service.GetQuotesChartDataCustom(new Date('2024-01-01'), new Date('2024-01-31'), 'owner', 'bu', 'chart'),
            expectations: ['GetQuotesChartDataCustom', 'ownerId=owner', 'businessUnitId=bu', 'chartCode=chart'],
        },
        {
            name: 'GetOpportunitiesChartData',
            call: (service: CRMDomainService) => service.GetOpportunitiesChartData('code', 'owner', 'bu', 'chart'),
            expectations: ['GetOpportunitiesChartData', 'code=code', 'ownerId=owner', 'businessUnitId=bu', 'chartCode=chart'],
        },
        {
            name: 'GetOpportunitiesChartDataCustom',
            call: (service: CRMDomainService) =>
                service.GetOpportunitiesChartDataCustom(new Date('2024-01-01'), new Date('2024-01-31'), 'owner', 'bu', 'chart'),
            expectations: ['GetOpportunitiesChartDataCustom', 'ownerId=owner', 'businessUnitId=bu', 'chartCode=chart'],
        },
    ];

    chartCases.forEach(({ name, call, expectations }) => {
        it(`${name} wraps response`, done => {
            httpClient.get.mockReturnValue(of(['data']));
            const service = new CRMDomainService();

            call(service).subscribe(res => {
                const url = httpClient.get.mock.calls[0][0] as string;
                expectations.forEach(expectation => expect(url).toContain(expectation));
                expect(res.Result).toEqual(['data']);
                done();
            });
        });
    });

    [
        {
            name: 'GetOpenedTicketsGroupByClassification',
            call: (service: CRMDomainService) =>
                service.GetOpenedTicketsGroupByClassification('code', 'owner', 'group'),
            expectations: ['GetOpenedTicketsGroupByClassification', 'code=code', 'ownerId=owner', 'employeeGroupId=group'],
        },
        {
            name: 'GetOpenedTicketsGroupBySeverity',
            call: (service: CRMDomainService) => service.GetOpenedTicketsGroupBySeverity('code', 'owner', 'group'),
            expectations: ['GetOpenedTicketsGroupBySeverity', 'code=code', 'ownerId=owner', 'employeeGroupId=group'],
        },
        {
            name: 'GetOpenedTicketsGroupByOwner',
            call: (service: CRMDomainService) => service.GetOpenedTicketsGroupByOwner('code', 'owner', 'group'),
            expectations: ['GetOpenedTicketsGroupByOwner', 'code=code', 'ownerId=owner', 'employeeGroupId=group'],
        },
    ].forEach(({ name, call, expectations }) => {
        it(`${name} groups ticket data`, done => {
            httpClient.get.mockReturnValue(of([1, 2, 3]));
            const service = new CRMDomainService();

            call(service).subscribe(res => {
                const url = httpClient.get.mock.calls[0][0] as string;
                expectations.forEach(expectation => expect(url).toContain(expectation));
                expect(res.Result).toEqual([1, 2, 3]);
                done();
            });
        });
    });

    [
        {
            name: 'GetActivitiesGroupBySalesman',
            call: (service: CRMDomainService) =>
                service.GetActivitiesGroupBySalesman('code', 'owner', 'bu', 'FIELD', true),
            expectations: ['GetActivitiesGroupBySalesman', 'code=code', 'ownerId=owner', 'businessUnitId=bu', 'fieldCode=FIELD', 'isTopTen=true'],
        },
        {
            name: 'GetActivitiesGroupBySalesmanCustom',
            call: (service: CRMDomainService) =>
                service.GetActivitiesGroupBySalesmanCustom(new Date('2024-01-01'), new Date('2024-01-31'), 'owner', 'bu', 'FIELD', true),
            expectations: ['GetActivitiesGroupBySalesmanCustom', 'ownerId=owner', 'businessUnitId=bu', 'fieldCode=FIELD', 'isTopTen=true'],
        },
        {
            name: 'GetOpportunitiesGroupBySalesman',
            call: (service: CRMDomainService) =>
                service.GetOpportunitiesGroupBySalesman('code', 'owner', 'bu', 'FIELD', true),
            expectations: ['GetOpportunitiesGroupBySalesman', 'code=code', 'ownerId=owner', 'businessUnitId=bu', 'fieldCode=FIELD', 'isTopTen=true'],
        },
        {
            name: 'GetOpportunitiesGroupBySalesmanCustom',
            call: (service: CRMDomainService) =>
                service.GetOpportunitiesGroupBySalesmanCustom(new Date('2024-01-01'), new Date('2024-01-31'), 'owner', 'bu', 'FIELD', true),
            expectations: ['GetOpportunitiesGroupBySalesmanCustom', 'ownerId=owner', 'businessUnitId=bu', 'fieldCode=FIELD', 'isTopTen=true'],
        },
        {
            name: 'GetQuotesGroupBySalesman',
            call: (service: CRMDomainService) =>
                service.GetQuotesGroupBySalesman('code', 'owner', 'bu', 'FIELD', true),
            expectations: ['GetQuotesGroupBySalesman', 'code=code', 'ownerId=owner', 'businessUnitId=bu', 'fieldCode=FIELD', 'isTopTen=true'],
        },
        {
            name: 'GetQuotesGroupBySalesmanCustom',
            call: (service: CRMDomainService) =>
                service.GetQuotesGroupBySalesmanCustom(new Date('2024-01-01'), new Date('2024-01-31'), 'owner', 'bu', 'FIELD', true),
            expectations: ['GetQuotesGroupBySalesmanCustom', 'ownerId=owner', 'businessUnitId=bu', 'fieldCode=FIELD', 'isTopTen=true'],
        },
        {
            name: 'GetCustomersGroupBySalesman',
            call: (service: CRMDomainService) =>
                service.GetCustomersGroupBySalesman(30, 'owner', 'bu', 'FIELD', false),
            expectations: ['GetCustomersGroupBySalesman', 'days=30', 'ownerId=owner', 'businessUnitId=bu', 'fieldCode=FIELD', 'isTopTen=false'],
        },
        {
            name: 'GetCustomersGroupBySalesmanCustom',
            call: (service: CRMDomainService) =>
                service.GetCustomersGroupBySalesmanCustom(new Date('2024-01-01'), new Date('2024-01-31'), 'owner', 'bu', 'FIELD', false),
            expectations: ['GetCustomersGroupBySalesmanCustom', 'ownerId=owner', 'businessUnitId=bu', 'fieldCode=FIELD', 'isTopTen=false'],
        },
    ].forEach(({ name, call, expectations }) => {
        it(`${name} builds sales grouping request`, done => {
            httpClient.get.mockReturnValue(of(['sales']));
            const service = new CRMDomainService();

            call(service).subscribe(res => {
                const url = httpClient.get.mock.calls[0][0] as string;
                expectations.forEach(expectation => expect(url).toContain(expectation));
                expect(res.Result).toEqual(['sales']);
                done();
            });
        });
    });

    it('GetContactCards returns raw response', done => {
        const payload = { cards: [1, 2] };
        httpClient.get.mockReturnValue(of(payload));
        const service = new CRMDomainService();

        service.GetContactCards('COMP', 'CONT').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetContactCards?companyId=COMP&contactId=CONT`,
                { headers: { Authorization: 'token' } }
            );
            expect(res).toBe(payload);
            done();
        });
    });

    it('GetConnectContactCards returns raw response', done => {
        const payload = { connect: true };
        httpClient.get.mockReturnValue(of(payload));
        const service = new CRMDomainService();

        service.GetConnectContactCards('COMP', 'CONT').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetConnectContactCards?companyId=COMP&contactId=CONT`,
                { headers: { Authorization: 'token' } }
            );
            expect(res).toBe(payload);
            done();
        });
    });

    it('GetActivitiesSummary wraps summary result', done => {
        httpClient.get.mockReturnValue(of({ summary: 'ok' }));
        const service = new CRMDomainService();

        service.GetActivitiesSummary('TYPE', 'OWNER', 'BU', 'REC').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetActivitiesSummary?activityTypeCodeFilter=TYPE&ownerId=OWNER&businessUnitId=BU&RecordsTypeCode=REC`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toEqual({ summary: 'ok' });
            done();
        });
    });

    it('GetActivitiesByOpportunityId wraps list', done => {
        httpClient.get.mockReturnValue(of([{ id: 1 }]));
        const service = new CRMDomainService();

        service.GetActivitiesByOpportunityId('OPP').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetActivitiesByOpportunityId?entityId=OPP`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toEqual([{ id: 1 }]);
            done();
        });
    });

    it('GetActivitiesByTicketId wraps list', done => {
        httpClient.get.mockReturnValue(of([{ id: 2 }]));
        const service = new CRMDomainService();

        service.GetActivitiesByTicketId('TICKET').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetActivitiesByTicketId?entityId=TICKET`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toEqual([{ id: 2 }]);
            done();
        });
    });

    it('GetTicketOverViewStatisticsSummary wraps data', done => {
        httpClient.get.mockReturnValue(of({ stats: 5 }));
        const service = new CRMDomainService();

        service.GetTicketOverViewStatisticsSummary('TKT').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetTicketOverViewStatisticsSummary?entityId=TKT`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toEqual({ stats: 5 });
            done();
        });
    });

    it('GetBusinessHours wraps response', done => {
        httpClient.get.mockReturnValue(of({ hours: [] }));
        const service = new CRMDomainService();

        service.GetBusinessHours('COMP').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetBusinessHours?entityId=COMP`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toEqual({ hours: [] });
            done();
        });
    });

    it('GetOpportunitiesSummary maps response into CRMSummary', done => {
        httpClient.get.mockReturnValue(of({ Won: 3, Lost: 1 }));
        const service = new CRMDomainService();

        service.GetOpportunitiesSummary('OWNER', 'BU', 'REC').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetOpportunitiesSummary?ownerId=OWNER&businessUnitId=BU&RecordsTypeCode=REC`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result.Won).toBe(3);
            expect(res.Result.Lost).toBe(1);
            done();
        });
    });

    it('GetOccasionsSummary maps response into OccasionSummary', done => {
        httpClient.get.mockReturnValue(of({ Total: 4 }));
        const service = new CRMDomainService();

        service.GetOccasionsSummary().subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetOccasionsSummary`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result.Total).toBe(4);
            done();
        });
    });
});

