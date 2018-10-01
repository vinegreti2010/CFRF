CREATE TABLE opr_defn (
  user_id			VARCHAR(20)		NOT NULL,
  password_user		VARCHAR(64)		NOT NULL,
  access			CHAR(1)			NOT NULL	DEFAULT 'N',
  PRIMARY KEY(user_id)
);

CREATE TABLE personal_data (
  student_id		INTEGER			NOT NULL	IDENTITY(1, 1),
  name_display		VARCHAR(50),
  PRIMARY KEY(student_id)
);

CREATE TABLE term_tbl (
  strm				INTEGER			NOT NULL,
  descr				VARCHAR(20),
  begin_dt			DATE			NOT NULL,
  end_dt			DATE			NOT NULL,
  PRIMARY KEY(strm)
);

CREATE TABLE facility_tbl (
  facility_id		INTEGER			NOT NULL	IDENTITY(1, 1),
  descr				VARCHAR(30),
  latitude_north_east DECIMAL(14, 10),
  longitude_north_east DECIMAL(14, 10),
  latitude_north_west DECIMAL(14, 10),
  longitude_north_west DECIMAL(14, 10),
  latitude_south_east DECIMAL(14, 10),
  longitude_south_east DECIMAL(14, 10),
  latitude_south_west DECIMAL(14, 10),
  longitude_south_west DECIMAL(14, 10),
  PRIMARY KEY(facility_id)
);

CREATE TABLE student_images (
  student_id		INTEGER		NOT NULL,
  student_image		IMAGE		NOT NULL
  PRIMARY KEY(student_id),
  FOREIGN KEY(student_id)
    REFERENCES personal_data(student_id)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
);

CREATE TABLE class_tbl (
  class_nbr			INTEGER		NOT NULL	IDENTITY(1, 1),
  strm				INTEGER		NOT NULL,
  descr				VARCHAR(100),
  facility_id		INTEGER		NOT NULL,
  PRIMARY KEY(class_nbr, strm),
  FOREIGN KEY(facility_id)
    REFERENCES facility_tbl(facility_id)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION,
  FOREIGN KEY(strm)
    REFERENCES term_tbl(strm)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
);

CREATE TABLE stdnt_enrl (
  student_id		INTEGER		NOT NULL,
  class_nbr			INTEGER		NOT NULL,
  strm				INTEGER		NOT NULL,
  PRIMARY KEY(student_id, class_nbr, strm),
  FOREIGN KEY(student_id)
    REFERENCES personal_data(student_id)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION,
  FOREIGN KEY(class_nbr, strm)
    REFERENCES class_tbl(class_nbr, strm)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
);

CREATE TABLE class_attendence (
  class_nbr			INTEGER		NOT NULL,
  strm				INTEGER		NOT NULL,
  student_id		INTEGER		NOT NULL,
  attend_dt			DATE		NOT NULL,
  start_time		TIME		NOT NULL,
  end_time			TIME		NOT NULL,
  presence			CHAR(1)		DEFAULT 'N',
  PRIMARY KEY(class_nbr, strm, student_id, attend_dt, start_time),
  FOREIGN KEY(student_id, class_nbr, strm)
    REFERENCES stdnt_enrl(student_id, class_nbr, strm)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
);