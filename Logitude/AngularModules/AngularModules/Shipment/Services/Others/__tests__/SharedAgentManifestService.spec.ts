import { expect } from '@jest/globals';
import { of, throwError } from 'rxjs';
import { SharedAgentManifestService } from '../SharedAgentManifestService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';

const createService = () => new SharedAgentManifestService();

describe('SharedAgentManifestService', () => {
    const baseUrl = 'https://api/';
    let httpClient: { get: jest.Mock; post: jest.Mock };

    beforeEach(() => {
        httpClient = {
            get: jest.fn(),
            post: jest.fn(),
        };

        ServiceHelper.HttpClient = httpClient as any;
        jest.spyOn(ServiceHelper, 'GetLogitudeURL').mockReturnValue(baseUrl);
        jest.spyOn(ServiceHelper, 'GetHttpHeaders').mockReturnValue({ headers: {} } as any);
        jest.spyOn(ServiceHelper, 'HandleServiceError').mockImplementation(error => throwError(() => error));
        SessionInfo.LoggedUserTenant = 77;
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('getSharedAgentManifestTransLateIdByCode builds correct URL', done => {
        httpClient.get.mockReturnValue(of({ value: 'v' }));
        const service = createService();

        service.getSharedAgentManifestTransLateIdByCode('CODE', 5).subscribe(res => {
            expect(res.Result).toEqual({ value: 'v' });
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/SharedAgentManifest/getSharedAgentManifestTransLateIdByCode?code=CODE&tenant=5`,
                { headers: {} }
            );
            done();
        });
    });

    it('getAgentSharedManifesRefShipmentListsByIds posts list ids', done => {
        const payload = ['REF-1', 'REF-2'];
        httpClient.post.mockReturnValue(of({ any: 'value' }));
        const service = createService();

        service.getAgentSharedManifesRefShipmentListsByIds(payload).subscribe(res => {
            expect(res.Result).toEqual({ any: 'value' });
            expect(httpClient.post).toHaveBeenCalledWith(
                `${baseUrl}api/SharedAgentManifest/postagentsharedmanifesrefshipmentListsbyids`,
                JSON.stringify(payload),
                { headers: {} }
            );
            done();
        });
    });

    it('get uses MapJsonToEntityPM to shape response', async () => {
        const service = createService();
        const mapSpy = jest.spyOn(service, 'MapJsonToEntityPM').mockReturnValue({ Id: 'PM-1' } as any);
        httpClient.get.mockReturnValue(of({ Id: 'raw' }));

        const res = await service.get('ID-1').toPromise();

        expect(mapSpy).toHaveBeenCalledWith({ Id: 'raw' });
        expect(res.Result).toEqual({ Id: 'PM-1' });
        expect(httpClient.get).toHaveBeenCalledWith(
            `${baseUrl}api/SharedAgentManifest/getsingle?id=ID-1`,
            { headers: {} }
        );
    });

    it('ShareAgentManifest passes logged tenant', () => {
        httpClient.get.mockReturnValue(of({}));
        const service = createService();

        service.ShareAgentManifest('SHIP-1', true).subscribe();

        expect(httpClient.get).toHaveBeenCalledWith(
            `${baseUrl}api/SharedAgentManifest/GetSharedAgentManifest?shipmentId=SHIP-1&isUpdateAgent=true&tenant=77`,
            { headers: {} }
        );
    });

    it('GetCheckIfAnyShipmentHaveMasterNumber forwards parameters', done => {
        httpClient.get.mockReturnValue(of('SHIP-42'));
        const service = createService();

        service.GetCheckIfAnyShipmentHaveMasterNumber('MASTER', 'LONG', 9).subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/SharedAgentManifest/GetCheckIfAnyShipmentHaveMasterNumber?master=MASTER&longMaster=LONG&tenant=9`,
                { headers: {} }
            );
            expect(res.Result).toBe('SHIP-42');
            done();
        });
    });

    it('GetCheckIfMasterShipmentHaveHouseWithOtherAgent returns service response', done => {
        httpClient.get.mockReturnValue(of({ isConflict: true }));
        const service = createService();

        service.GetCheckIfMasterShipmentHaveHouseWithOtherAgent('ENTITY', 'AGENT', 12).subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/SharedAgentManifest/GetCheckIfMasterShipmentHaveHouseWithOtherAgent?entityId=ENTITY&agentId=AGENT&tenant=12`,
                { headers: {} }
            );
            expect(res.Result).toEqual({ isConflict: true });
            done();
        });
    });

    it('getAgentSharedManifestsWorkspaceSummary wraps response', done => {
        httpClient.get.mockReturnValue(of({ summary: 'value' }));
        const service = createService();

        service.getAgentSharedManifestsWorkspaceSummary().subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/SharedAgentManifest/GetAgentSharedManifestsWorkspaceSummary`,
                { headers: {} }
            );
            expect(res.Result).toEqual({ summary: 'value' });
            done();
        });
    });

    it('GetIsAgentSharedManifests returns result payload', done => {
        httpClient.get.mockReturnValue(of(true));
        const service = createService();

        service.GetIsAgentSharedManifests('AGT', 'ENT').subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/SharedAgentManifest/GetIsAgentSharedManifests?agentId=AGT&entityid=ENT`,
                { headers: {} }
            );
            expect(res.Result).toBe(true);
            done();
        });
    });

    it('GetAgentSharedManifestsForDashBoard maps dashboard params', done => {
        httpClient.get.mockReturnValue(of({ count: 3 }));
        const service = createService();

        service.GetAgentSharedManifestsForDashBoard(2, 5, 1).subscribe(res => {
            expect(httpClient.get).toHaveBeenCalledWith(
                `${baseUrl}api/SharedAgentManifest/GetAgentSharedManifestsForDashBoard?lastMonths=2&lastDays=5&selectedIndex=1`,
                { headers: {} }
            );
            expect(res.Result).toEqual({ count: 3 });
            done();
        });
    });

    it('MapJsonToEntityPM clones when mapParent is true', () => {
        const service = createService();
        const cloneSpy = jest.spyOn(service, 'clone');
        const entity = service.MapJsonToEntityPM({ Id: '1', UIProperties: 'skip' });

        expect(entity.Id).toBe('1');
        expect(cloneSpy).toHaveBeenCalled();
        expect(entity.IsDirty).toBe(false);
        expect(entity.OldEntityPM).toBeDefined();
    });

    it('MapJsonToEntityPM respects mapParent false', () => {
        const service = createService();
        const existing: any = { Id: 'existing', OldEntityPM: {} };

        const result = service.MapJsonToEntityPM({ Id: '2' }, false, existing);

        expect(result).toBe(existing);
        expect(result.Id).toBe('2');
        expect(result.OldEntityPM).toBeNull();
    });

    it('clone filters UI properties and copies fields', () => {
        const service = createService();
        const source = {
            Id: 'ID',
            UIProperties: {},
            OldEntityPM: {},
            PropertyChanged: {},
            entityParentPM: {},
            Value: 'value',
        };

        const cloned = service.clone(source);

        expect(cloned).toEqual({ Id: 'ID', Value: 'value' });
    });
});

