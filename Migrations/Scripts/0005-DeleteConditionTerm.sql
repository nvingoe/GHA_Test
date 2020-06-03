CREATE PROCEDURE DeleteConditionTerm @Term INT
AS
BEGIN
	DELETE FROM AreaOfInterest WHERE Term = @Term
END
GO
GRANT EXECUTE ON DeleteConditionTerm TO AreasOfInterestManagement;
GO