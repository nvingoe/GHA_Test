﻿ALTER PROCEDURE SaveAreaOfInterest @Id UNIQUEIDENTIFIER, @Name VARCHAR(255), @TermId INT, @Conditions ClinicalCodes READONLY
AS
BEGIN
	DELETE aoi 
	FROM AreaOfInterest aoi LEFT JOIN @Conditions c ON aoi.ClinicalCode = c.ClinicalCode AND aoi.Terminology = c.Terminology 
	WHERE aoi.Id = @Id AND aoi.Term <> @TermId

	DELETE aoi 
	FROM AreaOfInterest aoi LEFT JOIN @Conditions c ON aoi.Id = @Id AND aoi.Term = @TermId AND aoi.ClinicalCode = c.ClinicalCode AND aoi.Terminology = c.Terminology 
	WHERE c.ClinicalCode IS NULL
	AND aoi.Term = @TermId

	INSERT INTO AreaOfInterest
	SELECT @Id, @Name, c.ClinicalCode, @TermId, c.Terminology 
	FROM @Conditions c 
	LEFT JOIN AreaOfInterest aoi ON aoi.Id = @Id AND aoi.Term = @TermId AND c.ClinicalCode = aoi.ClinicalCode AND c.Terminology = aoi.Terminology 
	WHERE aoi.ClinicalCode IS NULL

	UPDATE AreaOfInterest SET Name = @Name WHERE Id = @Id
END
GO
