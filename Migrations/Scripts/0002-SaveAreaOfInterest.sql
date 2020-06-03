CREATE TABLE AreaOfInterest (
	Id UNIQUEIDENTIFIER NOT NULL,
	Name VARCHAR(255) NOT NULL,
	ClinicalCode VARCHAR(25) NOT NULL,
	Term INT NOT NULL,
	Terminology TINYINT NOT NULL
) 
GO
CREATE INDEX ClinicalCode_Terminology ON AreaOfInterest(ClinicalCode, Terminology)
GO
CREATE TYPE ClinicalCodes AS TABLE(
	ClinicalCode VARCHAR(25) NOT NULL,
	Terminology TINYINT NOT NULL
)
GO
CREATE PROCEDURE SaveAreaOfInterest @Id UNIQUEIDENTIFIER, @Name VARCHAR(255), @TermId INT, @Conditions ClinicalCodes READONLY
AS
BEGIN
	DELETE aoi 
	FROM AreaOfInterest aoi LEFT JOIN @Conditions c ON aoi.ClinicalCode = c.ClinicalCode AND aoi.Terminology = c.Terminology 
	WHERE aoi.Id = @Id AND aoi.Term <> @TermId

	DELETE aoi 
	FROM AreaOfInterest aoi LEFT JOIN @Conditions c ON aoi.ClinicalCode = c.ClinicalCode AND aoi.Terminology = c.Terminology 
	WHERE aoi.Id = @Id AND aoi.Term = @TermId AND c.ClinicalCode IS NULL

	INSERT INTO AreaOfInterest
	SELECT @Id, @Name, c.ClinicalCode, @TermId, c.Terminology 
	FROM @Conditions c LEFT JOIN AreaOfInterest aoi ON aoi.Term = @TermId AND c.ClinicalCode = aoi.ClinicalCode AND c.Terminology = aoi.Terminology 
	WHERE aoi.ClinicalCode IS NULL

	UPDATE AreaOfInterest SET Name = @Name WHERE Id = @Id
END
GO
GRANT EXECUTE ON SaveAreaOfInterest TO AreasOfInterestManagement;
GRANT EXECUTE ON TYPE :: ClinicalCodes TO AreasOfInterestManagement;  
GO
CREATE TYPE Drugs AS TABLE(
	ProductCode VARCHAR(25) NOT NULL,
	AuthorisationDate DATE NOT NULL
)
GO
CREATE TYPE ClinicalEntries AS TABLE(
	ClinicalCode VARCHAR(25) NOT NULL
)
GO
CREATE PROC Analyse @DrugTerminology TINYINT, @Drugs Drugs READONLY, @ClinicalTerminology TINYINT, @ClinicalEntries ClinicalEntries READONLY
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
GRANT EXECUTE ON TYPE :: ClinicalEntries TO AreasOfInterestRetrieval;  
GO
DROP PROCEDURE GetProduct;
GO