update PackageTypes set PrintAs = Code where PrintAs is null;
/
ALTER TABLE "PACKAGETYPES" MODIFY "PRINTAS" NOT NULL;