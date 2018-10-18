

--on main
Update contacts set computedKey = NVL(TRIM(contacts.Email), contacts.Id);

ALTER TABLE contacts
ADD CONSTRAINT UQ_Tenant_ComputedKey UNIQUE (computedKey,Tenant)


--on global

ALTER TABLE GlobalContacts
ADD CONSTRAINT UQ_Tenant_Email UNIQUE (Email,GlobalTenantId)