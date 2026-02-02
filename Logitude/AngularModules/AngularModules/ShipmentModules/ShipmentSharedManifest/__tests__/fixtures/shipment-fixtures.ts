import type { AgentSharedManifestPM } from '../../../../Common/EntityPMs/AgentSharedManifestPM';
import type { AgentSharedManifestList } from '../../../../Common/EntityLists/AgentSharedManifestList';
import type { ManifestSL } from '../../../../Common/DataContracts/ManifestSL';
import type { HouseSL } from '../../../../Common/DataContracts/HouseSL';

export const createManifestSL = (overrides: Partial<ManifestSL> = {}): ManifestSL => {
    const manifest: ManifestSL = {
        SharedManifestRef: 'REF-001',
        ShipmentLevelCode: 'H',
        DirectionId: 'IMP',
        TransportModeId: 'A',
        AgentName: 'Sample Agent',
        ShipperName: 'Sample Shipper',
        CarrierName: 'Carrier',
        FreightPrepaidCollectId: 'PRE',
        OtherPrepaidCollectId: 'COL',
        MainCarriageATD: undefined,
        MainCarriageETA: undefined,
        MAWBOBLDate: undefined,
        LongMaster: '',
        Houses: [],
        EntityId: null
    } as ManifestSL;
    return Object.assign(manifest, overrides);
};

export const createAgentSharedManifestPM = (overrides: Partial<AgentSharedManifestPM> = {}): AgentSharedManifestPM => {
    const pm: AgentSharedManifestPM = {
        Id: 'PM-001',
        StatusCode: 'WAIT',
        ManifestSL: createManifestSL(),
        CreateDate: new Date('2024-01-01T00:00:00Z'),
        AgentReference: '',
        CancelledBySenderAgent: false,
        UpdateDate: undefined
    } as AgentSharedManifestPM;
    return Object.assign(pm, overrides);
};

export const createAgentSharedManifestList = (
    overrides: Partial<AgentSharedManifestList> = {}
): AgentSharedManifestList => {
    const list: AgentSharedManifestList = {
        Id: 'LIST-001',
        Routing: 'TLV-LAX',
        ShipmentLevelName: 'Master'
    } as AgentSharedManifestList;
    return Object.assign(list, overrides);
};

export const createHouseSL = (overrides: Partial<HouseSL> = {}): HouseSL => {
    const house: HouseSL = {
        EntityId: null,
        SharedManifestRef: '',
        Shipper: { Code: 'HOUSE-SHIPPER' } as any,
        Consignee: { Code: 'HOUSE-CONSIGNEE' } as any
    } as HouseSL;
    return Object.assign(house, overrides);
};

