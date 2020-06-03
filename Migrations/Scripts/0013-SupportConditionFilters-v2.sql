ALTER PROCEDURE SaveConditionGroup @Id UNIQUEIDENTIFIER, @Name VARCHAR(255), @TermId INT, @Conditions ClinicalCodes READONLY, @IsAreaOfInterest BIT, @IsFilter BIT
AS
BEGIN
	DELETE cg
	FROM ConditionGroup cg LEFT JOIN @Conditions c ON cg.ClinicalCode = c.ClinicalCode AND cg.Terminology = c.Terminology 
	WHERE cg.Id = @Id AND cg.Term <> @TermId

	DELETE cg 
	FROM ConditionGroup cg LEFT JOIN @Conditions c ON cg.ClinicalCode = c.ClinicalCode AND cg.Terminology = c.Terminology 
	WHERE cg.Id = @Id AND cg.Term = @TermId AND c.ClinicalCode IS NULL

	INSERT INTO ConditionGroup
	SELECT @Id, @Name, c.ClinicalCode, @TermId, c.Terminology, @IsAreaOfInterest, @IsFilter 
	FROM @Conditions c 
	LEFT JOIN ConditionGroup cg ON cg.Id = @Id
	WHERE cg.ClinicalCode IS NULL

	UPDATE ConditionGroup SET Name = @Name WHERE Id = @Id
END
GO