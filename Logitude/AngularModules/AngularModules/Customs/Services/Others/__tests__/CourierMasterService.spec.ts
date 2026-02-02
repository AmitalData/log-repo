import { expect, jest } from '@jest/globals';
import { of } from 'rxjs';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

const baseUrl = 'https://customs/';
const sendAjaxMock = jest.fn();

jest.mock('Infrastructure/Services/logtuide-table-data.service', () => {
    class LogtuideTableDataServiceMock {
        static createInstance() {
            return new LogtuideTableDataServiceMock();
        }

        sendAjaxAndGetDataStandart(ajax: any) {
            return sendAjaxMock(ajax);
        }
    }

    return {
        __esModule: true,
        LogtuideTableDataService: LogtuideTableDataServiceMock,
    };
});

// eslint-disable-next-line @typescript-eslint/no-var-requires
const { CourierMasterService } = require('../CourierMasterService');

describe('CourierMasterService', () => {
    let httpClient: any;

    beforeEach(() => {
        httpClient = {
            get: jest.fn(),
            post: jest.fn(),
        };
        ServiceHelper.HttpClient = httpClient;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => {
            throw error;
        });
        SessionLocator.LoggedUserId = 'USER-1';
        sendAjaxMock.mockClear();
        sendAjaxMock.mockReturnValue(Promise.resolve('done'));
    });

    afterEach(() => jest.restoreAllMocks());

    it('getByFilters requests connected declarations and maps result', done => {
        httpClient.get.mockReturnValue(of({ HasError: false, Result: [{ Id: 'DEC' }] }));
        const service = new CourierMasterService();
        const filters = { Foo: 'bar baz' } as any;

        service.getByFilters(filters).subscribe(res => {
            const url = httpClient.get.mock.calls[0][0] as string;
            expect(url).toContain('GetCourierConnectedDeclarations');
            expect(url).toContain('Foo=bar%20baz');
            expect(res.Result[0].Id).toBe('DEC');
            done();
        });
    });

    it('GetRequiredFieldsForCourierMaster wraps result in ServiceResponse', done => {
        httpClient.get.mockReturnValue(of({ Requirement: 'Value' }));
        const service = new CourierMasterService();

        service.GetRequiredFieldsForCourierMaster('CM-1').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CourierMaster/GetRequiredFieldsForCourierMaster/?courierMasterId=CM-1`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result.Requirement).toBe('Value');
            done();
        });
    });

    it('GetRequiredFieldsForCourierMasterIncludeManifest calls include endpoint', done => {
        httpClient.get.mockReturnValue(of({ Requirement: 'Manifest' }));
        const service = new CourierMasterService();

        service.GetRequiredFieldsForCourierMasterIncludeManifest('CM-2').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CourierMaster/GetRequiredFieldsForCourierMasterIncludeManifest/?courierMasterId=CM-2`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result.Requirement).toBe('Manifest');
            done();
        });
    });

    it('getCourierMasterByDeclarationId maps response to CourierMasterPM', done => {
        httpClient.get.mockReturnValue(of({ Id: 'RAW' }));
        const service = new CourierMasterService();
        const mapSpy = jest.spyOn(service, 'MapJsonToCourierMasterPM').mockReturnValue({ Id: 'MAPPED' } as any);

        service.getCourierMasterByDeclarationId('DEC-1').subscribe(res => {
            expect(mapSpy).toHaveBeenCalledWith({ Id: 'RAW' });
            expect(res.Result.Id).toBe('MAPPED');
            done();
        });
    });

    it('GetSendPayReadyLow2755 maps DataResult payload', done => {
        httpClient.get.mockReturnValue(of({ Message: 'ready', RequestInProgressList: ['A'] }));
        const service = new CourierMasterService();

        service.GetSendPayReadyLow2755('CM-3', 'HAWB', 'BANK', true).subscribe(res => {
            const payload = res as any;
            expect(httpClient.get).toHaveBeenCalledWith(
                expect.stringContaining('/GetSendPayReadyLow2755?'),
                { headers: { Authorization: 'token' } }
            );
            expect(payload.Message).toBe('ready');
            expect(payload.RequestInProgressList).toEqual(['A']);
            done();
        });
    });

    it('PostSendPayReadyLow2755 posts payload and unwraps message', done => {
        httpClient.post.mockReturnValue(of({ Message: 'posted', RequestInProgressList: [] }));
        const service = new CourierMasterService();
        const params = { Ids: ['1'] } as any;

        service.PostSendPayReadyLow2755(params).subscribe(res => {
            const payload = res as any;
            expect(httpClient.post).toHaveBeenCalledWith(
                `${baseUrl}api/CourierMaster/PostSendPayReadyLow2755/`,
                JSON.stringify(params),
                { headers: { Authorization: 'token' } }
            );
            expect(payload.Message).toBe('posted');
            done();
        });
    });

    it('PostSend2750AndUpdaeClassificationByCourierMaster returns service response', done => {
        httpClient.post.mockReturnValue(of('UPDATED'));
        const service = new CourierMasterService();

        service.PostSend2750AndUpdaeClassificationByCourierMaster({} as any).subscribe(res => {
            expect(res.Result).toBe('UPDATED');
            done();
        });
    });

    it('PostSendALLTerminal unwraps DataResult payload', done => {
        httpClient.post.mockReturnValue(of({ Message: 'terminal', RequestInProgressList: ['T'] }));
        const service = new CourierMasterService();

        service.PostSendALLTerminal({} as any).subscribe(res => {
            const payload = res as any;
            expect(payload.Message).toBe('terminal');
            expect(payload.RequestInProgressList).toEqual(['T']);
            done();
        });
    });

    it('PostSendClosePending forwards message details', done => {
        httpClient.post.mockReturnValue(of({ Message: 'closed', RequestInProgressList: [] }));
        const service = new CourierMasterService();

        service.PostSendClosePending({} as any).subscribe(res => {
            const payload = res as any;
            expect(payload.Message).toBe('closed');
            done();
        });
    });

    it('GetPending builds url with worksheet flag', done => {
        httpClient.get.mockReturnValue(of([{ Id: 'pending' }]));
        const service = new CourierMasterService();

        service.GetPending('CM-4', true).subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/CourierMaster/GetPending?CourierMasterId=CM-4&IsWorkSheetFromExcel=true&UserId=USER-1`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result[0].Id).toBe('pending');
            done();
        });
    });

    it('GetIfAllowToCancelCourierMaster returns boolean result', done => {
        httpClient.get.mockReturnValue(of(true));
        const service = new CourierMasterService();

        service.GetIfAllowToCancelCourierMaster('CM-5').subscribe(res => {
            expect(res.Result).toBe(true);
            done();
        });
    });

    it('PostSendRecoverDeclaration maps data result', done => {
        httpClient.post.mockReturnValue(of({ Message: 'recover', RequestInProgressList: ['R'] }));
        const service = new CourierMasterService();

        service.PostSendRecoverDeclaration({} as any).subscribe(res => {
            const payload = res as any;
            expect(payload.Message).toBe('recover');
            done();
        });
    });

    it('PostApprovePending unwraps DataResult', done => {
        httpClient.post.mockReturnValue(of({ Message: 'approved', RequestInProgressList: ['A'] }));
        const service = new CourierMasterService();

        service.PostApprovePending({} as any).subscribe(res => {
            const payload = res as any;
            expect(payload.Message).toBe('approved');
            expect(payload.RequestInProgressList).toEqual(['A']);
            done();
        });
    });

    it('GetSendFTPMamanRequest wraps service response', done => {
        httpClient.get.mockReturnValue(of('request sent'));
        const service = new CourierMasterService();

        service.GetSendFTPMamanRequest('CM-6').subscribe(res => {
            expect(res.Result).toBe('request sent');
            done();
        });
    });

    it('sendConnectDeclaration delegates to table data service', async () => {
        httpClient.post.mockReturnValue(of({}));
        const service = new CourierMasterService();

        const promise = service.sendConnectDeclaration('CM-7', 7, 'MAWB', true, false, ['1'], ['2']);

        await expect(promise).resolves.toBe('done');
        expect(httpClient.post).toHaveBeenCalledWith(
            `${baseUrl}api/CourierMaster/sendConnectDeclaration`,
            {
                courierMasterId: 'CM-7',
                tenant: 7,
                MAWB: 'MAWB',
                connectedAll: true,
                disconnectedAll: false,
                connectedItems: ['1'],
                disconnectedItems: ['2'],
            },
            { headers: { Authorization: 'token' } }
        );
        expect(sendAjaxMock).toHaveBeenCalledWith(expect.anything());
    });

    it('MapJsonToDeclarationPM copies primitive fields', () => {
        const service = new CourierMasterService();
        const entity = service.MapJsonToDeclarationPM({ Id: '1', DeclarationNumber: 'DECL' } as any);

        expect(entity.Id).toBe('1');
        expect(entity.DeclarationNumber).toBe('DECL');
    });
});

