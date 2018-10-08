ALTER PROC [dbo].[updatePresence]
	@Class_nbr VARCHAR(4),
	@Strm VARCHAR(4),
	@Student_id VARCHAR(12),
	@Attend_dt DATE,
	@Start_time TIME
	AS
	BEGIN
		UPDATE class_attendence 
		   SET presence = 'Y' 
		 WHERE class_nbr = @Class_nbr
		   AND strm = @Strm
		   AND student_id = @Student_id
		   AND attend_dt = @Attend_dt
		   AND start_time = @Start_time;
	END