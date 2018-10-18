-- Rami Issa
-- 07-10-2012
-- Remove DocumentType from features list , to not show it in the modules features


DELETE FROM PackageFeatures
WHERE        (FeatureId =
                             (SELECT        Id
                               FROM            Features
                               WHERE        (FeatureTypeCode = 'MODL') AND (ObjectTableId =
                                                             (SELECT        Id
                                                               FROM            ObjectTables
                                                               WHERE        (Name = 'documenttype')))))


DELETE FROM Features
WHERE        (FeatureTypeCode = 'MODL') AND (ObjectTableId =
                             (SELECT        Id
                               FROM            ObjectTables
                               WHERE        (Name = 'documenttype')))








