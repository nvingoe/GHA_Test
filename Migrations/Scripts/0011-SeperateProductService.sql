
ALTER PROC Analyse @ClinicalEntries ClinicalEntries READONLY, @ClinicalTerminology TINYINT
AS
BEGIN
	SELECT aoi.Id, aoi.Name, aoi.ClinicalCode, aoi.Terminology
	FROM AreaOfInterest aoi 
	INNER JOIN @ClinicalEntries ce ON aoi.ClinicalCode = ce.ClinicalCode AND aoi.Terminology = @ClinicalTerminology
END

GO
DROP TYPE Drugs

GO
CREATE TYPE ProductCodes AS TABLE(Code varchar(25) NOT NULL)

GO
CREATE PROC GetProducts @ProductCodes ProductCodes READONLY, @DrugTerminology TINYINT
AS
BEGIN
	SELECT p.Code, p.Name, @DrugTerminology as Terminology
	FROM Product p
	INNER JOIN @ProductCodes c ON p.Code = c.Code AND p.Terminology = @DrugTerminology
END

GO
GRANT EXECUTE ON GetProducts TO AreasOfInterestRetrieval;
GRANT EXECUTE ON TYPE :: ProductCodes TO AreasOfInterestRetrieval;  
