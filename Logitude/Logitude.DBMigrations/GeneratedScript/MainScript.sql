/* Generated Script For Test.dxml */
-- Rename Column From Col2 To Col22
ALTER TABLE "Test" RENAME COLUMN "Col2" TO "Col22";

-- Change Size From 50 To 100 For Column Col5
ALTER TABLE "Test" MODIFY "Col5" NVARCHAR2(100);

-- Set Nullable For Column Col6
ALTER TABLE "Test" MODIFY "Col6" TIMESTAMP(7) NULL;


