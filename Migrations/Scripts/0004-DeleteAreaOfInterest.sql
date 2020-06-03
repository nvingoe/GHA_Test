CREATE PROCEDURE DeleteAreaOfInterest @Id UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM AreaOfInterest WHERE Id = @Id
END
GO
GRANT EXECUTE ON DeleteAreaOfInterest TO AreasOfInterestManagement;
GO