CREATE PROC [dbo].[insertLog]
	@Latitude DECIMAL(14, 10),
	@Longitude DECIMAL(14, 10),
	@Sucess CHAR(1),
	@Error VARCHAR(254),
	@Student_id VARCHAR(12)
	AS
	BEGIN
		INSERT INTO presence_validation_log values (@Student_id, SYSDATETIME(), @Latitude, @Longitude, @Sucess, @Error);
	END