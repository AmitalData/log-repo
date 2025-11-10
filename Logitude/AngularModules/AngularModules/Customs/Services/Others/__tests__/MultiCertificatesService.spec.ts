import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { MultiCertificatesService } from '../MultiCertificatesService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { CertificateConnectedItem } from '../../../DataContract/CertificateConnectedItem';
import { CertificateTicket } from '../../../DataContract/CertificateTicket';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';

describe('MultiCertificatesService', () => {
    const baseUrl = 'https://customs/';
    let httpClient: { get: jest.Mock; post: jest.Mock; put: jest.Mock };

    beforeEach(() => {
        httpClient = {
            get: jest.fn(),
            post: jest.fn(),
            put: jest.fn()
        };
        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: { Authorization: 'token' } } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => {
            throw error;
        });
        jest.spyOn(console, 'log').mockImplementation(() => undefined);
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('GetCertificateConnectedItems maps response array to CertificateConnectedItem instances', done => {
        httpClient.get.mockReturnValue(of([{ Id: '1', ItemCode: 'CODE1' }]));
        const service = new MultiCertificatesService();

        service
            .GetCertificateConnectedItems('DECL', 'ATT', 'REQ', 'EX', 'NUM', 'RES')
            .subscribe(res => {
                expect(httpClient.get).toHaveBeenCalledWith(
                    `${baseUrl}api/MultiCertificates/GetCertificateConnectedItems?` +
                        `declarationId=DECL&attachmentTypeCode=ATT&reqConfirmationTypeCode=REQ&` +
                        `CertificateExemptionTypeCode=EX&CertificateNumber=NUM&ResConfirmationTypeCode=RES`,
                    { headers: { Authorization: 'token' } }
                );
                expect(res.Result).toHaveLength(1);
                expect(res.Result[0]).toBeInstanceOf(CertificateConnectedItem);
                expect(res.Result[0].ItemCode).toBe('CODE1');
                done();
            });
    });

    it('PutCertificateTickets posts mapped ticket and returns updated ticket', done => {
        const responseBody = { Id: 'T-1', CertificateNumber: '123' };
        httpClient.post.mockReturnValue(of(responseBody));
        const ticket = new CertificateTicket();
        ticket.Id = '';
        ticket.CertificateNumber = '';
        ticket.SelectedItems = [];
        const service = new MultiCertificatesService();

        service.PutCertificateTickets(ticket).subscribe(res => {
            expect(typeof httpClient.post.mock.calls[0][1]).toBe('string');
            expect(httpClient.post.mock.calls[0][0]).toBe(
                `${baseUrl}api/MultiCertificates/PostCertificateTicket/`
            );
            expect(res.Result).toBe(ticket);
            expect(ticket.Id).toBe('T-1');
            expect(ticket.CertificateNumber).toBe('123');
            done();
        });
    });

    it('getByFilters builds query string and maps service response', done => {
        const filters = new ApiQueryFilters();
        (filters as any).Foo = 'bar baz';
        filters.AdditionalFilters = [{ FieldName: 'X', FieldValue: '1' } as any];
        httpClient.get.mockReturnValue(of({ Result: [{ ItemCode: 'NEW' }] }));
        const service = new MultiCertificatesService();

        service
            .getByFilters(filters, 'DECL', 'ATT', 'REQ', 'EX', 'NUM', 'RES')
            .subscribe(res => {
                const requestedUrl = httpClient.get.mock.calls[0][0] as string;
                expect(requestedUrl).toContain('GetByFilters');
                expect(requestedUrl).toContain('Foo=bar%20baz');
                expect(requestedUrl).toContain('AdditionalFilters=');
                expect(requestedUrl).toContain('declarationId=DECL');
                expect(res.Result[0]).toBeInstanceOf(CertificateConnectedItem);
                expect(res.Result[0].ItemCode).toBe('NEW');
                done();
            });
    });

    it('getCountByFilters wraps numeric result into ServiceResponse', done => {
        const filters = new ApiQueryFilters();
        (filters as any).Status = 'Open';
        httpClient.get.mockReturnValue(of(42));
        const service = new MultiCertificatesService();

        service
            .getCountByFilters(filters, 'DECL', 'ATT', 'REQ', 'EX', 'NUM', 'RES')
            .subscribe(res => {
                const requestedUrl = httpClient.get.mock.calls[0][0] as string;
                expect(requestedUrl).toContain('getCountByFilters');
                expect(requestedUrl).toContain('Status=Open');
                expect(res.Result).toBe(42);
                done();
            });
    });

    it('DeclarationHasInvoices wraps boolean response', done => {
        httpClient.get.mockReturnValue(of(true));
        const service = new MultiCertificatesService();

        service.DeclarationHasInvoices('DECL-2').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/MultiCertificates/GetDeclarationHasInvoices?declarationId=DECL-2`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toBe(true);
            done();
        });
    });

    it('GetCertificateConnectedItems returns empty array when server result missing', done => {
        httpClient.get.mockReturnValue(of(null));
        const service = new MultiCertificatesService();

        service.GetCertificateConnectedItems('DECL', 'ATT', 'REQ', 'EX', 'NUM', 'RES').subscribe(res => {
            expect(res.Result).toEqual([]);
            done();
        });
    });

    it('UpdateAllCertificateWithoutResponse issues expected request', done => {
        httpClient.get.mockReturnValue(of({ status: 'ok' }));
        const service = new MultiCertificatesService();

        service.UpdateAllCertificateWithoutResponse('DEC-1', 'CFN-1').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/MultiCertificates/GetUpdateAllCertificateWithoutResponse?declarationId=DEC-1&customFileNo=CFN-1`,
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toEqual({ status: 'ok' });
            done();
        });
    });

    it('UpdateCertificatesBySearchFields hits expected endpoint', done => {
        httpClient.get.mockReturnValue(of('updated'));
        const service = new MultiCertificatesService();

        service
            .UpdateCertificatesBySearchFields('DECL', 1, 'EXT', 'APR', 'REQ', 'ATT', 'NUM', 'EXM', 'RES')
            .subscribe(res => {
                const url = httpClient.get.mock.calls[0][0] as string;
                expect(url).toContain('GetUpdateCertificatesBySearchFields');
                expect(url).toContain('declarationId=DECL');
                expect(res.Result).toBe('updated');
                done();
            });
    });

    it('CreateCertificateForInvoiceItems calls expected endpoint', done => {
        httpClient.get.mockReturnValue(of('created'));
        const service = new MultiCertificatesService();

        service
            .CreateCertificateForInvoiceItems('DECL', 'FILE', 'ATT', 'REQ', 'RES', 'NUM', 'EXM', 'KEYS')
            .subscribe(res => {
                const url = httpClient.get.mock.calls[0][0] as string;
                expect(url).toContain('GetCreateCertificateForInvoiceItems');
                expect(url).toContain('customFileNo=FILE');
                expect(res.Result).toBe('created');
                done();
            });
    });

    it('PutSupplierInvoiceItemCatalogNumber maps response to CertificateConnectedItem', done => {
        httpClient.put.mockReturnValue(of({ Id: 'ITEM-1', ItemCode: 'CODE' }));
        const service = new MultiCertificatesService();
        const item = new CertificateConnectedItem();
        item.Id = 'ITEM-1';
        item.ItemCode = 'CODE';

        service.PutSupplierInvoiceItemCatalogNumber(item).subscribe(res => {
            expect(httpClient.put).toHaveBeenCalledWith(
                `${baseUrl}api/MultiCertificates/PutSupplierInvoiceItemCatalogNumber/`,
                JSON.stringify(item),
                { headers: { Authorization: 'token' } }
            );
            expect(res.Result).toBeInstanceOf(CertificateConnectedItem);
            expect(res.Result.Id).toBe('ITEM-1');
            done();
        });
    });

    it('getPromiseByFilters resolves to observable from getByFilters', async () => {
        httpClient.get.mockReturnValue(of({ Result: [] }));
        const service = new MultiCertificatesService();
        const filters = new ApiQueryFilters();

        const observable = (await service.getPromiseByFilters(
            filters,
            'DECL',
            'ATT',
            'REQ',
            'EX',
            'NUM',
            'RES'
        )) as any;

        expect(typeof observable.subscribe).toBe('function');
    });
});

