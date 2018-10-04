CREATE PROC [dbo].[addimage]
	@Code VARCHAR(12),
	@image IMAGE
	AS
	BEGIN
		INSERT INTO student_images VALUES (@Code, @image);
	END