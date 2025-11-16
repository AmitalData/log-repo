import { expect } from '@jest/globals';
import { of } from 'rxjs';
import { CacheCourierPendingReasonService } from '../CacheCourierPendingReasonService';
import { DateTool } from '../../../../Infrastructure/Tools';

const getAllMock = jest.fn();

jest.mock('../../StandardLists/CourierPendingReasonListService', () => ({
    CourierPendingReasonListService: jest.fn().mockImplementation(() => ({
        getAll: getAllMock
    }))
}));

describe('CacheCourierPendingReasonService', () => {
    let currentTime: Date;

    beforeEach(() => {
        currentTime = new Date('2024-01-01T00:00:00Z');
        getAllMock.mockReset();
        jest.spyOn(DateTool, 'GetCurrentDateTimeAsUtc').mockImplementation(() => currentTime);
        (CacheCourierPendingReasonService as any)._instance = undefined;
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('loads cache when empty', () => {
        const pendingReasons = [{ Code: 'A1' }] as any;
        getAllMock.mockReturnValue(of({ Result: pendingReasons }));

        const service = CacheCourierPendingReasonService.Instance;
        const result = service.GetCache();

        expect(getAllMock).toHaveBeenCalledTimes(1);
        expect(result).toEqual(pendingReasons);
    });

    it('reuses cache within freshness window', () => {
        const pendingReasons = [{ Code: 'B1' }] as any;
        getAllMock.mockReturnValue(of({ Result: pendingReasons }));

        const service = CacheCourierPendingReasonService.Instance;
        service.GetCache();
        getAllMock.mockClear();

        service.GetCache();

        expect(getAllMock).not.toHaveBeenCalled();
    });

    it('refreshes cache after freshness window expires', () => {
        const initialReasons = [{ Code: 'C1' }] as any;
        getAllMock.mockReturnValue(of({ Result: initialReasons }));

        const service = CacheCourierPendingReasonService.Instance;
        service.GetCache();
        getAllMock.mockClear();

        currentTime = new Date(currentTime.getTime() + 16 * 60 * 1000);
        const refreshedReasons = [{ Code: 'C2' }] as any;
        getAllMock.mockReturnValue(of({ Result: refreshedReasons }));

        const result = service.GetCache();

        expect(getAllMock).toHaveBeenCalledTimes(1);
        expect(result).toEqual(refreshedReasons);
    });
});
