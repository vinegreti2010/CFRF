CREATE PROC [dbo].[insertLog]
	@Sucess CHAR(1),
	@Error VARCHAR(254),
	@Student_id VARCHAR(12)
	AS
	BEGIN
		INSERT INTO presence_validation_log values (@Student_id, SYSDATETIME(), @Sucess, @Error);
	END