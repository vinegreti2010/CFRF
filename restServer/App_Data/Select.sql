SELECT *
  FROM opr_defn;

SELECT *
  FROM term_tbl;

SELECT *
  FROM facility_tbl;

SELECT *
  FROM class_tbl;

SELECT *
  FROM personal_data;

SELECT *
  FROM student_images;

SELECT *
  FROM stdnt_enrl;

SELECT *
  FROM class_attendence;

SELECT CONVERT(DATE, GETDATE()), CONVERT(TIME, GETDATE())
  FROM personal_data;

/*******SELECT student Name and Image********/
SELECT A.name_display, B.student_image
  FROM personal_data A
			INNER JOIN
	   student_images B
			   ON B.student_id = A.student_id
 WHERE A.student_id = 1;

/*******SELECT FACILITY LOCATION AND CLASS DESCR BASED ON CURRENT CLASS********/
SELECT B.descr, 
	   D.latitude_north_east, 
	   D.longitude_north_east, 
	   D.latitude_north_west, 
	   D.longitude_north_west, 
	   D.latitude_south_east, 
	   D.longitude_south_east, 
	   D.latitude_south_west, 
	   D.longitude_south_west
  FROM stdnt_enrl A
			INNER JOIN
	   class_tbl B
			   ON B.class_nbr = A.class_nbr
			  AND B.strm = A.strm
			INNER JOIN
	   class_attendence C
			   ON C.class_nbr = A.class_nbr
			  AND C.strm = A.strm
			  AND C.student_id = A.student_id
			INNER JOIN
	   facility_tbl D
			   ON D.facility_id = B.facility_id
 WHERE A.student_id = 1
   --AND C.attend_dt = CONVERT(DATE, GETDATE())
   AND C.attend_dt = CONVERT(DATE, '20180820 19:15:00 PM')
   --AND CONVERT(TIME, GETDATE()) BETWEEN C.start_time AND C.end_time
   AND CONVERT(TIME, '20180820 19:15:00 PM') BETWEEN C.start_time AND C.end_time;

/*******SELECT FACILITY LOCATION, CLASS DESCR, NAME AND IMAGE BASED ON CURRENT CLASS********/
SELECT B.descr, 
	   D.latitude_north_east, 
	   D.longitude_north_east, 
	   D.latitude_north_west, 
	   D.longitude_north_west, 
	   D.latitude_south_east, 
	   D.longitude_south_east, 
	   D.latitude_south_west, 
	   D.longitude_south_west,
	   E.name_display,
	   F.student_image
  FROM stdnt_enrl A
			INNER JOIN
	   class_tbl B
			   ON B.class_nbr = A.class_nbr
			  AND B.strm = A.strm
			INNER JOIN
	   class_attendence C
			   ON C.class_nbr = A.class_nbr
			  AND C.strm = A.strm
			  AND C.student_id = A.student_id
			INNER JOIN
	   facility_tbl D
			   ON D.facility_id = B.facility_id
			INNER JOIN
	   personal_data E
			   ON E.student_id = A.student_id
			INNER JOIN
	   student_images F
			   ON F.student_id = A.student_id
 WHERE A.student_id = 4
   --AND C.attend_dt = CONVERT(DATE, GETDATE())
   AND C.attend_dt = CONVERT(DATE, '20180820 19:15:00 PM')
   --AND CONVERT(TIME, GETDATE()) BETWEEN C.start_time AND C.end_time
   AND CONVERT(TIME, '20180820 19:15:00 PM') BETWEEN C.start_time AND C.end_time;