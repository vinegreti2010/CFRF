alter PROC [dbo].[insertRecognizeLog]
	@Student_id VARCHAR(12),
	@Sucess CHAR(1),
	@Error VARCHAR(254),
	@Time FLOAT,
	@Distance FLOAT
	AS
	BEGIN
		INSERT INTO recognize_log values (@Student_id, SYSDATETIME(), @Sucess, @Error, @Time, @Distance);
	END