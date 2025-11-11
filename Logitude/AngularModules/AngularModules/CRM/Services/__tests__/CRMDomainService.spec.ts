import { expect, jest } from '@jest/globals';
import type { Mock, SpyInstance } from 'jest-mock';
import { of, throwError } from 'rxjs';
import { CRMDomainService } from '../CRMDomainService';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator';

describe('CRMDomainService', () => {
    const baseUrl = 'https://crm/';
    let httpClient: { get: Mock<any, any>; post: Mock<any, any> };
    let handleErrorSpy: SpyInstance<any, any>;

    beforeEach(() => {
        httpClient = {
            get: jest.fn(),
            post: jest.fn(),
        };
        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        handleErrorSpy = jest.spyOn(ServiceHelper, 'HandleServiceError');
        handleErrorSpy.mockImplementation(error => {
            throw error;
        });
        (global as any).window = { TextCodesCache: [] };
        SessionLocator.LoggedUserPM = { DontShowLocal: false } as any;
        SessionLocator.SelectedSession = { CurrentEditComponent: { ReloadEntityPM: jest.fn() } } as any;
    });

    afterEach(() => jest.restoreAllMocks());

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

    it('InserNewTicket returns validation errors without calling backend', done => {
        const ticket = { Id: 'T-1', IsDirty: true } as any;
        const validationErrors = ['Ticket number required'];
        jest.spyOn(ClassLevelValidator.prototype, 'Validate').mockReturnValue(validationErrors);
        const service = new CRMDomainService();

        service.InserNewTicket(ticket).subscribe(res => {
            expect(httpClient.post).not.toHaveBeenCalled();
            expect(res.HasError).toBe(true);
            expect(res.ErrorsArray).toEqual(validationErrors);
            expect(res.Result).toBeNull();
            done();
        });
    });

    it('InserNewTicket maps successful response and resets dirty state', done => {
        const ticket = { Id: 'T-2', TicketNumber: 'TN-2', IsDirty: true } as any;
        jest.spyOn(ClassLevelValidator.prototype, 'Validate').mockReturnValue([]);
        httpClient.post.mockReturnValue(of({ Id: 'T-2', TicketNumber: 'TN-2' }));
        const service = new CRMDomainService();

        service.InserNewTicket(ticket).subscribe(res => {
            expect(httpClient.post).toHaveBeenCalled();
            expect(res.HasError).toBe(false);
            expect(res.Result.Id).toBe('T-2');
            expect(ticket.IsDirty).toBe(false);
            expect(res.Result.OldEntityPM).toBeTruthy();
            done();
        });
    });

    it('InserNewTicket delegates HTTP errors to ServiceHelper.HandleServiceError', done => {
        const ticket = { Id: 'T-3' } as any;
        jest.spyOn(ClassLevelValidator.prototype, 'Validate').mockReturnValue([]);
        const backendError = new Error('backend failure');
        httpClient.post.mockReturnValue(throwError(backendError));
        const service = new CRMDomainService();

        expect.assertions(1);
        service.InserNewTicket(ticket).subscribe({
            next: () => {
                done(new Error('Expected observable to error'));
            },
            error: err => {
                expect(handleErrorSpy).toHaveBeenCalled();
                done();
            },
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
            expect(res.Result).toEqual({ summary: 'ok' });
            done();
        });
    });

    it('GetActivitiesByOpportunityId wraps list', done => {
        httpClient.get.mockReturnValue(of([{ id: 1 }]));
        const service = new CRMDomainService();

        service.GetActivitiesByOpportunityId('OPP').subscribe(res => {
            expect(res.Result).toEqual([{ id: 1 }]);
            done();
        });
    });

    it('GetActivitiesByTicketId wraps list', done => {
        httpClient.get.mockReturnValue(of([{ id: 2 }]));
        const service = new CRMDomainService();

        service.GetActivitiesByTicketId('TICKET').subscribe(res => {
            expect(res.Result).toEqual([{ id: 2 }]);
            done();
        });
    });

    it('GetTicketOverViewStatisticsSummary wraps data', done => {
        httpClient.get.mockReturnValue(of({ stats: 5 }));
        const service = new CRMDomainService();

        service.GetTicketOverViewStatisticsSummary('TKT').subscribe(res => {
            expect(res.Result).toEqual({ stats: 5 });
            done();
        });
    });

    it('GetBusinessHours wraps response', done => {
        httpClient.get.mockReturnValue(of({ hours: [] }));
        const service = new CRMDomainService();

        service.GetBusinessHours('COMP').subscribe(res => {
            expect(res.Result).toEqual({ hours: [] });
            done();
        });
    });

    it('GetOpportunitiesSummary maps response into CRMSummary', done => {
        httpClient.get.mockReturnValue(of({ Won: 3, Lost: 1 }));
        const service = new CRMDomainService();

        service.GetOpportunitiesSummary('OWNER', 'BU', 'REC').subscribe(res => {
            expect(res.Result.Won).toBe(3);
            expect(res.Result.Lost).toBe(1);
            done();
        });
    });

    it('GetOccasionsSummary maps response into OccasionSummary', done => {
        httpClient.get.mockReturnValue(of({ Total: 4 }));
        const service = new CRMDomainService();

        service.GetOccasionsSummary().subscribe(res => {
            expect(res.Result.Total).toBe(4);
            done();
        });
    });

    [
        {
            name: 'GetOpenedTicketsBySLAViolation',
            call: (service: CRMDomainService) =>
                service.GetOpenedTicketsBySLAViolation(2, 'code', 'owner', 'group'),
            expectations: [
                'GetOpenedTicketsBySLAViolation',
                'selectedIndex=2',
                'code=code',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
        {
            name: 'GetOpenedTicketsByOpenedStage',
            call: (service: CRMDomainService) =>
                service.GetOpenedTicketsByOpenedStage(3, 'code', 'owner', 'group'),
            expectations: [
                'GetOpenedTicketsByOpenedStage',
                'selectedIndex=3',
                'code=code',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
        {
            name: 'GetClosedTicketsGroupByClassification',
            call: (service: CRMDomainService) =>
                service.GetClosedTicketsGroupByClassification('code', 'owner', 'group'),
            expectations: [
                'GetClosedTicketsGroupByClassification',
                'code=code',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
        {
            name: 'GetClosedTicketsGroupBySeverity',
            call: (service: CRMDomainService) =>
                service.GetClosedTicketsGroupBySeverity('code', 'owner', 'group'),
            expectations: [
                'GetClosedTicketsGroupBySeverity',
                'code=code',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
        {
            name: 'GetClosedTicketsGroupByType',
            call: (service: CRMDomainService) =>
                service.GetClosedTicketsGroupByType('code', 'owner', 'group'),
            expectations: [
                'GetClosedTicketsGroupByType',
                'code=code',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
        {
            name: 'GetClosedTicketsBySLAViolation',
            call: (service: CRMDomainService) =>
                service.GetClosedTicketsBySLAViolation(1, 'code', 'owner', 'group'),
            expectations: [
                'GetClosedTicketsBySLAViolation',
                'selectedIndex=1',
                'code=code',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
        {
            name: 'GetClosedTicketsBySolvedStage',
            call: (service: CRMDomainService) =>
                service.GetClosedTicketsBySolvedStage(4, 'code', 'owner', 'group'),
            expectations: [
                'GetClosedTicketsBySolvedStage',
                'selectedIndex=4',
                'code=code',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
        {
            name: 'GetOpenTicketsGroupByClassification',
            call: (service: CRMDomainService) =>
                service.GetOpenTicketsGroupByClassification('owner', 'group'),
            expectations: [
                'GetOpenTicketsGroupByClassification',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
        {
            name: 'GetOpenTicketsByDueTime',
            call: (service: CRMDomainService) => service.GetOpenTicketsByDueTime('owner', 'group'),
            expectations: [
                'GetOpenTicketsByDueTime',
                'ownerId=owner',
                'employeeGroupId=group',
            ],
        },
    ].forEach(({ name, call, expectations }) => {
        it(`${name} returns ticket analytics`, done => {
            httpClient.get.mockReturnValue(of([{ id: name }]));
            const service = new CRMDomainService();

            call(service).subscribe(res => {
                const url = httpClient.get.mock.calls[0][0] as string;
                expectations.forEach(expectation => expect(url).toContain(expectation));
                expect(res.Result).toEqual([{ id: name }]);
                done();
            });
        });
    });

    it('GetTicketOverviewPerformance wraps performance response', done => {
        httpClient.get.mockReturnValue(of({ perf: true }));
        const service = new CRMDomainService();

        service.GetTicketOverviewPerformance('TKT-1').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetTicketOverviewPerformance?ticketId=TKT-1`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toEqual({ perf: true });
            done();
        });
    });

    it('GetRecentTickets returns recent ticket list', done => {
        httpClient.get.mockReturnValue(of([{ ticket: 1 }]));
        const service = new CRMDomainService();

        service.GetRecentTickets('owner', 'group').subscribe(res => {
            expect(res).toEqual([{ ticket: 1 }]);
            done();
        });
    });

    it('GetRecentOpportunities returns recent opportunities', done => {
        httpClient.get.mockReturnValue(of([{ opp: 1 }]));
        const service = new CRMDomainService();

        service.GetRecentOpportunities('owner', 'bu').subscribe(res => {
            expect(res).toEqual([{ opp: 1 }]);
            done();
        });
    });

    it('GetCorrespondencesList returns correspondence data', done => {
        httpClient.get.mockReturnValue(of({ docs: [] }));
        const service = new CRMDomainService();

        service.GetCorrespondencesList('ENTITY').subscribe(res => {
            expect(res).toEqual({ docs: [] });
            done();
        });
    });

    it('GetTicketsCounts returns raw counts', done => {
        httpClient.get.mockReturnValue(of({ open: 5 }));
        const service = new CRMDomainService();

        service.GetTicketsCounts('owner', 'group').subscribe(res => {
            expect(res).toEqual({ open: 5 });
            done();
        });
    });

    it('GetTopTickets returns prioritized tickets', done => {
        httpClient.get.mockReturnValue(of([{ top: true }]));
        const service = new CRMDomainService();

        service.GetTopTickets('owner', 'group').subscribe(res => {
            expect(res).toEqual([{ top: true }]);
            done();
        });
    });

    it('GetTicketsCountByShipmentNumber returns numeric response', done => {
        httpClient.get.mockReturnValue(of(4));
        const service = new CRMDomainService();

        service.GetTicketsCountByShipmentNumber('SHIP-1').subscribe(res => {
            expect(res).toBe(4);
            done();
        });
    });

    it('GetEmployeeGroupsPMList wraps groups into ServiceResponse', done => {
        httpClient.get.mockReturnValue(of([{ id: 'grp' }]));
        const service = new CRMDomainService();

        service.GetEmployeeGroupsPMList().subscribe(res => {
            expect(res.Result).toEqual([{ id: 'grp' }]);
            done();
        });
    });

    [
        {
            name: 'GetUsersByEmployeeGroupIds',
            call: (service: CRMDomainService) => service.GetUsersByEmployeeGroupIds('1,2'),
            url: 'GetUsersByEmployeeGroupIds?employeeIds=1,2',
        },
        {
            name: 'GetContactListsByEmailsString',
            call: (service: CRMDomainService) => service.GetContactListsByEmailsString('a@b'),
            url: 'GetContactListsByEmailsString?emails=a@b',
        },
        {
            name: 'GetUserListsByEmailsString',
            call: (service: CRMDomainService) => service.GetUserListsByEmailsString('a@b'),
            url: 'GetUserListsByEmailsString?emails=a@b',
        },
        {
            name: 'GetTicketEscalationListsByTicketId',
            call: (service: CRMDomainService) => service.GetTicketEscalationListsByTicketId('TKT'),
            url: 'GetTicketEscalationListsByTicketId?entityId=TKT',
        },
        {
            name: 'GetCountOfOccasionAllCustomers',
            call: (service: CRMDomainService) => service.GetCountOfOccasionAllCustomers('1,2'),
            url: 'GetCountOfOccasionAllCustomers?contactIds=1,2',
        },
    ].forEach(({ name, call, url }) => {
        it(`${name} wraps result payload`, done => {
            httpClient.get.mockReturnValue(of({ value: name }));
            const service = new CRMDomainService();

            call(service).subscribe(res => {
                const expectedUrl = `${baseUrl}api/CRMDomain/${url}`;
                expect(httpClient.get).toHaveBeenCalledWith(expectedUrl, { headers: { Authorization: 'token' } });
                expect(res.Result).toEqual({ value: name });
                done();
            });
        });
    });

    it('UpdatingActivityMettingSummary calls backend with provided params', done => {
        httpClient.get.mockReturnValue(of({ updated: true }));
        const service = new CRMDomainService();

        service.UpdatingActivityMettingSummary('summary', true, 'ACT-1').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CRMDomain/GetUpdatingActivityMettingSummary?mettingSummary=summary&post=true&activityId=ACT-1`,
                { headers: { Authorization: 'token' } }
            );
            expect(res).toEqual({ updated: true });
            done();
        });
    });

    describe('SLA mapping helpers', () => {
        it('MapJsonToSLAHeaderEntityPM skips deleted children when mapping parent', () => {
            const service = new CRMDomainService();
            const mapped = service.MapJsonToSLAHeaderEntityPM(
                {
                    SLALines: [
                        { ChangeSetOp: 'Delete', Duration: 1 },
                        { ChangeSetOp: 'None', Duration: 2 },
                    ],
                    SLAEscalations: [
                        {
                            ChangeSetOp: 'Delete',
                            SLAEscalationRecepients: [{ ChangeSetOp: 'Delete', Recipient: 'old' }],
                        },
                        {
                            ChangeSetOp: 'None',
                            SLAEscalationRecepients: [{ ChangeSetOp: 'None', Recipient: 'keep' }],
                        },
                    ],
                } as any,
                true
            );

            expect(mapped.SLALines).toHaveLength(1);
            expect((mapped.SLALines[0] as any).Duration).toBe(2);
            expect(mapped.SLAEscalations).toHaveLength(1);
            expect(mapped.SLAEscalations[0].SLAEscalationRecepients).toHaveLength(1);
            expect(((mapped.SLAEscalations[0] as any).SLAEscalationRecepients[0] as any).Recipient).toBe('keep');
        });

        it('MapJsonToSLAHeaderEntityPM flags missing items as delete during updates', () => {
            const service = new CRMDomainService();
            const initial = service.MapJsonToSLAHeaderEntityPM(
                {
                    SLALines: [{ Duration: 5 }],
                    SLAEscalations: [
                        {
                            SLAEscalationRecepients: [{ Recipient: 'persist' }],
                        },
                    ],
                } as any,
                true
            );

            const updated = service.MapJsonToSLAHeaderEntityPM(
                {
                    SLALines: [],
                    SLAEscalations: [
                        {
                            SLAEscalationRecepients: [],
                        },
                    ],
                } as any,
                false,
                initial
            );

            const deletedLine = updated.SLALines.find(line => line.ChangeSetOp === 'Delete');
            expect(deletedLine).toBeTruthy();
            const deletedRecipient = updated.SLAEscalations
                .reduce((acc: any[], esc: any) => acc.concat(esc.SLAEscalationRecepients), [])
                .find(rec => rec.ChangeSetOp === 'Delete');
            expect(deletedRecipient).toBeTruthy();
        });
    });
});

