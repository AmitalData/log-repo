
ALTER TABLE Queries
DROP CONSTRAINT  FK_QueryFeature;

ALTER TABLE ObjectTableTabs
DROP CONSTRAINT  FK_ObjectTableTabFeature;


ALTER TABLE Reports
DROP CONSTRAINT  [FK_dbo.Reports_dbo.Features_FeatureId]

ALTER TABLE MenuButtons
DROP CONSTRAINT  FK_MenuButtonFeature;