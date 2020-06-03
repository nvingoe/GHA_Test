DROP PROCEDURE Analyse
GO
DROP TYPE Drugs
GO

CREATE TYPE Drugs AS TABLE(
	ProductCode varchar(25) NOT NULL,
	AuthorisationDate date NULL
)

GO

CREATE PROC [dbo].[Analyse] @DrugTerminology TINYINT, @Drugs Drugs READONLY, @ClinicalTerminology TINYINT, @ClinicalEntries ClinicalEntries READONLY
AS
BEGIN
	SELECT p.Code, p.Name, reAuthDrugs.AuthorisationDate 
	FROM Product p
	INNER JOIN @Drugs reAuthDrugs ON p.Code = reAuthDrugs.ProductCode AND p.Terminology = @DrugTerminology

	SELECT aoi.Id, aoi.Name, aoi.ClinicalCode, aoi.Terminology
	FROM AreaOfInterest aoi 
	INNER JOIN @ClinicalEntries ce ON aoi.ClinicalCode = ce.ClinicalCode AND aoi.Terminology = @ClinicalTerminology
END

GO

GRANT EXECUTE ON Analyse TO AreasOfInterestRetrieval;
GRANT EXECUTE ON TYPE :: Drugs TO AreasOfInterestRetrieval;  