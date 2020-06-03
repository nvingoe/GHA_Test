CREATE TABLE ConditionGroup (
	Id UNIQUEIDENTIFIER NOT NULL,
	Name VARCHAR(255) NOT NULL,
	ClinicalCode VARCHAR(25) NOT NULL,
	Term INT NOT NULL,
	Terminology TINYINT NOT NULL,
	IsAreaOfInterest BIT NOT NULL,
	IsFilter BIT NOT NULL
) 
GO
CREATE INDEX ClinicalCode_Terminology ON ConditionGroup(ClinicalCode, Terminology)
GO
DROP PROC SaveAreaOfInterest
GO
CREATE PROCEDURE SaveConditionGroup @Id UNIQUEIDENTIFIER, @Name VARCHAR(255), @TermId INT, @Conditions ClinicalCodes READONLY, @IsAreaOfInterest BIT, @IsFilter BIT
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
	FROM @Conditions c LEFT JOIN ConditionGroup cg ON cg.Term = @TermId AND c.ClinicalCode = cg.ClinicalCode AND c.Terminology = cg.Terminology 
	WHERE cg.ClinicalCode IS NULL

	UPDATE ConditionGroup SET Name = @Name WHERE Id = @Id
END
GO
GRANT EXECUTE ON SaveConditionGroup TO AreasOfInterestManagement;
GO

ALTER PROC Analyse @ClinicalEntries ClinicalEntries READONLY, @ClinicalTerminology TINYINT
AS
BEGIN
	SELECT cg.Id, cg.Name, cg.ClinicalCode, cg.Terminology, cg.IsAreaOfInterest, cg.IsFilter
	FROM ConditionGroup cg 
	INNER JOIN @ClinicalEntries ce ON cg.ClinicalCode = ce.ClinicalCode AND cg.Terminology = @ClinicalTerminology
END
GO

DROP PROC DeleteAreaOfInterest
GO

CREATE PROCEDURE DeleteConditionGroup @Id UNIQUEIDENTIFIER
AS
BEGIN
	DELETE FROM ConditionGroup WHERE Id = @Id
END
GO
GRANT EXECUTE ON DeleteConditionGroup TO AreasOfInterestManagement;
GO
ALTER PROCEDURE [dbo].[SaveConditionTerm] @TermId INT, @Conditions ClinicalCodes READONLY
AS
BEGIN
	DECLARE @ConditionGroupsUsingTerm TABLE (Id UNIQUEIDENTIFIER, Name VARCHAR(255), IsAreaOfInterest BIT, IsFilter BIT)
	INSERT INTO @ConditionGroupsUsingTerm SELECT DISTINCT Id, Name, IsAreaOfInterest, IsFilter FROM ConditionGroup WHERE Term = @TermId
	
	DELETE cg
	FROM ConditionGroup cg
	LEFT JOIN @Conditions c ON @TermId = cg.Term AND cg.ClinicalCode = c.ClinicalCode AND cg.Terminology = c.Terminology
	WHERE c.ClinicalCode IS NULL AND cg.Term = @TermId
			
	INSERT INTO ConditionGroup
	SELECT conditionGroups.Id, conditionGroups.Name, c.ClinicalCode, @TermId, c.Terminology, conditionGroups.IsAreaOfInterest, conditionGroups.IsFilter 
	FROM @ConditionGroupsUsingTerm conditionGroups, @Conditions c
	LEFT JOIN ConditionGroup cg ON cg.Term = @TermId AND c.ClinicalCode = cg.ClinicalCode AND c.Terminology = cg.Terminology 
	WHERE cg.ClinicalCode IS NULL
END

GO

INSERT ConditionGroup
SELECT aoi.*, 1, 0
FROM  AreaOfInterest aoi

GO

DROP TABLE AreaOfInterest
GO

ALTER PROCEDURE [dbo].[DeleteConditionTerm] @Term INT
AS
BEGIN
	DELETE FROM ConditionGroup WHERE Term = @Term
END
