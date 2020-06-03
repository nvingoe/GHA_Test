ALTER PROCEDURE GetProductGroups @ProductCodes ProductCodes READONLY, @DrugTerminology TINYINT
AS
BEGIN
	SELECT p.Code, p.[Group]
	FROM @ProductCodes c
	LEFT JOIN Product p ON p.Code = c.Code AND p.Terminology = @DrugTerminology
END
GO