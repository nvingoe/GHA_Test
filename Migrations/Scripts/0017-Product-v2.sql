ALTER PROC [dbo].[GetProducts] @ProductCodes ProductCodes READONLY, @DrugTerminology TINYINT
AS
BEGIN
	SELECT p.Code, p.Name, @DrugTerminology as Terminology, [Group] as GroupId, GroupName
	FROM Product p
	INNER JOIN @ProductCodes c ON p.Code = c.Code AND p.Terminology = @DrugTerminology
END
GO