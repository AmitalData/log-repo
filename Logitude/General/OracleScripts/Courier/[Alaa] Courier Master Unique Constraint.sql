ALTER TABLE CourierMasters
ADD CONSTRAINT  UQ_Airline_MAWB_HAWB_Tenant  UNIQUE (AirlineId, MAWB,HAWB,Tenant);