ALTER PROCEDURE SaveConditionTerm @TermId INT, @Conditions ClinicalCodes READONLY
AS
BEGIN
	DECLARE @AoisUsingTerm TABLE (Id UNIQUEIDENTIFIER, Name VARCHAR(255))
	INSERT INTO @AoisUsingTerm SELECT DISTINCT Id, Name FROM AreaOfInterest WHERE Term = @TermId
	
	DELETE aoi
	FROM AreaOfInterest aoi
	LEFT JOIN @Conditions c ON @TermId = aoi.Term AND aoi.ClinicalCode = c.ClinicalCode AND aoi.Terminology = c.Terminology
	WHERE c.ClinicalCode IS NULL AND aoi.Term = @TermId
			
	INSERT INTO AreaOfInterest
	SELECT termAois.Id, termAois.Name, c.ClinicalCode, @TermId, c.Terminology 
	FROM @AoisUsingTerm termAois, @Conditions c
	LEFT JOIN AreaOfInterest aoi ON aoi.Term = @TermId AND c.ClinicalCode = aoi.ClinicalCode AND c.Terminology = aoi.Terminology 
	WHERE aoi.ClinicalCode IS NULL
END
GO
