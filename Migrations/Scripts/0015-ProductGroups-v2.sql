ALTER PROCEDURE SaveProduct
    @id INT,
	@group BIGINT = NULL,
	@groupName NVARCHAR(255) = NULL,
	@references ProductReferences READONLY
AS
BEGIN
	DELETE p FROM Product p
	LEFT JOIN  @references r ON @id = p.Id AND r.Code = p.Code AND r.Terminology = p.Terminology
	WHERE r.Code IS NULL AND p.Id = @id

	INSERT INTO Product SELECT @id, r.Code, r.Name, r.Terminology, @group, @groupName
	FROM @references r 
	LEFT JOIN Product existingProduct ON @id = existingProduct.Id AND r.Code = existingProduct.Code AND r.Terminology = existingProduct.Terminology
	WHERE existingProduct.Id IS NULL

	UPDATE Product SET Id = @id, Code = r.Code, Name = r.Name, Terminology = r.Terminology, [Group] = @group, GroupName = @groupName
	FROM @references r 
	INNER JOIN Product existingProduct ON @id = existingProduct.Id AND r.Code = existingProduct.Code AND r.Terminology = existingProduct.Terminology

END
GO