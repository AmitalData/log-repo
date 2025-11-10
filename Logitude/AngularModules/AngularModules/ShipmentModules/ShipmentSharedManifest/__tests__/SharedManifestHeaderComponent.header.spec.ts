import { expect } from '@jest/globals';
import { SharedManifestHeaderComponent, SharedManifestHeader } from '../Components/SharedManifestHeaderComponent';
import {
    createAgentSharedManifestList,
    createAgentSharedManifestPM,
    createManifestSL
} from './fixtures/shipment-fixtures';

describe('SharedManifestHeaderComponent', () => {
    it('Run stores SharedManifestHeader data', () => {
        const component = new SharedManifestHeaderComponent();
        const manifest = createManifestSL({
            LongMaster: 'LM123',
            AgentName: 'Agent X',
            ShipperName: 'Shipper X',
            CarrierName: 'Carrier X',
            FreightPrepaidCollectId: 'PRE',
            OtherPrepaidCollectId: 'COL',
            DirectionId: 'EXP',
            TransportModeId: 'S',
            ShipmentLevelCode: 'M'
        });
        const entity = createAgentSharedManifestPM({ ManifestSL: manifest, AgentReference: 'AG-REF' });
        const list = createAgentSharedManifestList({ Routing: 'TLV-NYC', ShipmentLevelName: 'Master' });

        component.Run(entity, list);

        expect(component.SharedManifestHeaderData instanceof SharedManifestHeader).toBe(true);
        expect(component.SharedManifestHeaderData.LongMaster).toBe('LM123');
        expect(component.SharedManifestHeaderData.Route).toBe('TLV-NYC');
        expect(component.SharedManifestHeaderData.AgentReference).toBe('AG-REF');
        expect(component.SharedManifestHeaderData.TransportModeId).toBe('S');
    });

    it('Run handles missing manifest gracefully', () => {
        const component = new SharedManifestHeaderComponent();
        const emptyEntity = createAgentSharedManifestPM({ ManifestSL: null });
        const list = createAgentSharedManifestList();

        component.Run(emptyEntity, list);

        expect(component.SharedManifestHeaderData.LongMaster).toBe('');
        expect(component.SharedManifestHeaderData.AgentName).toBe('');
        expect(component.SharedManifestHeaderData.Route).toBe(list.Routing);
    });
});

