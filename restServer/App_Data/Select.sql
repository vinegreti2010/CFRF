--SELECT *
--  FROM opr_defn;

--SELECT *
--  FROM term_tbl;

--SELECT *
--  FROM facility_tbl
--  --where facility_id > 2;

--SELECT *
--  FROM class_tbl;

--SELECT *
--  FROM personal_data;

--SELECT *
--  FROM student_images;

--SELECT *
--  FROM stdnt_enrl;

--SELECT *
--  FROM class_attendence
-- WHERE student_id = '555555555555'
--   AND presence = 'Y'

--SELECT *
--  FROM recognize_log;

SELECT *
  FROM presence_validation_log;

--SELECT CONVERT(DATE, GETDATE()), CONVERT(TIME, GETDATE())
--  FROM personal_data;

--/*******SELECT student Name and Image********/
--SELECT A.name_display, B.student_image
--  FROM personal_data A
--			INNER JOIN
--	   student_images B
--			   ON B.student_id = A.student_id
-- WHERE A.student_id = 1;

--/*******SELECT FACILITY LOCATION AND CLASS DESCR BASED ON CURRENT CLASS********/
--SELECT c.student_id, c.class_nbr,
--	   C.strm,
--	   C.attend_dt,
--	   C.start_time,
--	   B.descr, 
--	   D.latitude_north_east, 
--	   D.longitude_north_east, 
--	   D.latitude_north_west, 
--	   D.longitude_north_west, 
--	   D.latitude_south_east, 
--	   D.longitude_south_east, 
--	   D.latitude_south_west, 
--	   D.longitude_south_west
--  FROM stdnt_enrl A
--			INNER JOIN
--	   class_tbl B
--			   ON B.class_nbr = A.class_nbr
--			  AND B.strm = A.strm
--			INNER JOIN
--	   class_attendence C
--			   ON C.class_nbr = A.class_nbr
--			  AND C.strm = A.strm
--			  AND C.student_id = A.student_id
--			INNER JOIN
--	   facility_tbl D
--			   ON D.facility_id = B.facility_id
-- WHERE 1=1
--   --and A.student_id = '555555555555'
--   AND C.attend_dt = CONVERT(DATE, GETDATE())
--   --AND C.attend_dt = CONVERT(DATE, '20180820 19:15:00 PM')
--   AND CONVERT(TIME, GETDATE()) BETWEEN C.start_time AND C.end_time
--   --AND CONVERT(TIME, '20180820 19:15:00 PM') BETWEEN C.start_time AND C.end_time;

--SELECT TOP 10 CONVERT(DATE, effdt) AS EFFDT, COUNT(1) AS QTDE
--  FROM recognize_log
-- WHERE success = 'Y'
-- GROUP BY CONVERT(DATE, effdt)
-- ORDER BY 1 DESC;

--update opr_defn
--set email_addr = 'vinegreti2010@hotmail.com'

--SELECT c.descr, a.descr, b.student_id
--  FROM class_tbl a
--			INNER JOIN
--	   stdnt_enrl b
--			   ON b.strm = a.strm
--			  AND b.class_nbr = a.class_nbr
--			INNER JOIN
--	   term_tbl c
--			   ON c.strm = a.strm
-- WHERE a.strm = '1801'
--   AND a.class_nbr = '1'

--select b.name_display
--  from stdnt_enrl a
--			inner join
--	   personal_data b
--			   on b.student_id = a.student_id
-- WHERE a.strm = '1801'
--   AND a.class_nbr = '1'

--SELECT DISTINCT b.descr
--	 , a.descr
--	 , (select distinct count(1)
--		  from class_attendence
--		 where strm = a.strm
--		   and class_nbr = a.class_nbr
--		 group by student_id) as qty_dates
--	   , convert(varchar(10), convert(date, a.attend_dt))
--	   , convert(varchar(5), convert(time, a.start_time))
--  FROM class_tbl a
--			INNER JOIN
--	   term_tbl b
--			   ON b.strm = a.strm
--			INNER JOIN
--	   class_attendence c
--			   ON c.strm = a.strm
--			  AND c.class_nbr = a.class_nbr
-- WHERE a.strm = '1801'
--   AND a.class_nbr = '1'

SELECT b.name_display
	 , a.presence
	 , convert(varchar(10), convert(date, a.attend_dt))
	 , convert(varchar(5), convert(time, a.start_time))
  FROM class_attendence a
			INNER JOIN
	   personal_data b
			   ON b.student_id = a.student_id
 WHERE a.strm = '1801'
   AND a.class_nbr = '1'