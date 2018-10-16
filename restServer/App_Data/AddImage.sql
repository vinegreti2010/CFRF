CREATE PROC [dbo].[addOrUpdateImage]
	@Code VARCHAR(12),
	@Image IMAGE
	AS
	IF EXISTS (SELECT 1 FROM student_images WHERE student_id = @Code)
		BEGIN
			UPDATE student_images
			   SET student_image = @Image
             WHERE student_id = @Code;
		END
	ELSE
		BEGIN
			INSERT INTO student_images VALUES (@Code, @Image);
		END