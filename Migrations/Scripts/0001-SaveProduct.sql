CREATE TABLE Product (
	Id INT NOT NULL,
	Code VARCHAR(25) NOT NULL,
	Name VARCHAR(255) NOT NULL,
	Terminology TINYINT NOT NULL
)
GO
CREATE CLUSTERED INDEX a ON Product (Id, Terminology)
CREATE INDEX b ON Product (Code) INCLUDE (Name)
GO
CREATE TYPE ProductReferences AS TABLE (
	Code VARCHAR(25) NOT NULL,
	Name VARCHAR(255) NOT NULL,
	Terminology TINYINT NOT NULL
)
GO
CREATE PROCEDURE SaveProduct
    @id INT,
	@references ProductReferences READONLY
AS
BEGIN
	DELETE p FROM Product p
	LEFT JOIN  @references r ON @id = p.Id AND r.Code = p.Code AND r.Terminology = p.Terminology
	WHERE r.Code IS NULL AND p.Id = @id

	INSERT INTO Product SELECT @id, r.Code, r.Name, r.Terminology 
	FROM @references r 
	LEFT JOIN Product existingProduct ON @id = existingProduct.Id AND r.Code = existingProduct.Code AND r.Terminology = existingProduct.Terminology
	WHERE existingProduct.Id IS NULL

	UPDATE Product SET Id = @id, Code = r.Code, Name = r.Name, Terminology = r.Terminology 
	FROM @references r 
	INNER JOIN Product existingProduct ON @id = existingProduct.Id AND r.Code = existingProduct.Code AND r.Terminology = existingProduct.Terminology

END
GO
CREATE PROCEDURE GetProduct
	@code VARCHAR(25),
	@terminology TINYINT
AS
BEGIN
	SELECT allTerminologies.* FROM Product p
	INNER JOIN Product allTerminologies ON p.Id = allTerminologies.Id
	WHERE p.Code = @code AND p.Terminology = @terminology
END
GO
CREATE ROLE AreasOfInterestManagement;
CREATE ROLE AreasOfInterestRetrieval;
GO
GRANT EXECUTE ON GetProduct TO AreasOfInterestRetrieval;
GRANT EXECUTE ON SaveProduct TO AreasOfInterestManagement;
GRANT EXECUTE ON TYPE :: ProductReferences TO AreasOfInterestManagement;  
GO
