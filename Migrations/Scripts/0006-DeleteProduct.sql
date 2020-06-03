CREATE PROCEDURE DeleteProduct @Id INT
AS
BEGIN
	DELETE FROM Product WHERE Id = @Id
END
GO
GRANT EXECUTE ON DeleteProduct TO AreasOfInterestManagement;
GO