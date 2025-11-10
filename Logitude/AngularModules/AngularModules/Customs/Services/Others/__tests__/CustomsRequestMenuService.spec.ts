import { expect } from '@jest/globals';
import { CustomsRequestMenuService } from '../CustomsRequestMenuService';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';

describe('CustomsRequestMenuService', () => {
    beforeEach(() => {
        jest.spyOn(TextCodeTranslator, 'Translate').mockImplementation(key => `translated:${key}`);
    });

    afterEach(() => {
        jest.restoreAllMocks();
    });

    it('builds reports list when reports flag is true', () => {
        const allowed = new Set([
            'Customs.Declaration:SlaReport',
            'Customs.Declaration:LastMileReport',
            'Customs.Declaration:WorkSheetFromExcel'
        ]);
        jest.spyOn(FeatureLocator, 'HasFeaturePermession').mockImplementation((module: string, feature: string) =>
            allowed.has(`${module}:${feature}`)
        );

        const service = new CustomsRequestMenuService(true);

        expect(service.CustomsRequestMenuItems).toHaveLength(3);
        const names = service.CustomsRequestMenuItems.map(item => item.TranslatedName);
        expect(names).toEqual(['דוח SLA', 'דוח הפצה', 'מסך עבודה - הטענת אקסל']);
    });

    it('omits export report when feature is missing in customs list', () => {
        jest.spyOn(FeatureLocator, 'HasFeaturePermession').mockImplementation(() => false);

        const service = new CustomsRequestMenuService(false);

        const hasExportReport = service.CustomsRequestMenuItems.some(item => item.ScreenName === 'ExportReport');
        expect(hasExportReport).toBe(false);
    });
});

