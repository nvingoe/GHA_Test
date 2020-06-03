update  dbo.AreaOfInterest
set Name = SUBSTRING(Name, 1, 15)
GO

ALTER TABLE dbo.AreaOfInterest ALTER COLUMN [Name] NVARCHAR(15) not NULL;
GO
