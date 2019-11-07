CREATE TABLE Cities(
Code VARCHAR(6) PRIMARY KEY NOT NULL,
Name NVARCHAR(20) NOT NULL,
SearchFields NVARCHAR(MAX),
)
-------------------------------------------------------
CREATE TABLE Users(
Id VARCHAR(15) PRIMARY KEY NOT NULL,
Tenant INT NOT NULL,
FirstName NVARCHAR(30) NOT NULL,
LastName NVARCHAR(20) NOT NULL,
Inactive BIT NOT NULL,
BirthDate DATETIME NOT NULL,
)
-------------------------------------------------------
