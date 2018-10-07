INSERT INTO term_tbl VALUES 
('1801', '1º Semestre de 2018', '20180201', '20180630'),
('1802', '2º Semestre de 2018', '20180801', '20181130'),
('1901', '1º Semestre de 2019', '20190201', '20190630'),
('1902', '2º Semestre de 2019', '20190801', '20191130');

INSERT INTO facility_tbl VALUES
('Sala 1 Bloco 1', -25.35457, -49.1840973, -25.35457, -49.1840973, -25.370000, -49.200000, -25.370000, -49.200000),
('Sala 1 Bloco 2', -25.43394508, -49.21893256, -25.43394508, -49.28372744, -25.466634252, -49.21893256, -25.466634252, -49.28372744);

INSERT INTO personal_data VALUES
('123123123123', 'Joao'),
('111111111111', 'Maria'),
('222222222222', 'Pietra'),
('555555555555', 'Vinicius');

INSERT INTO class_tbl VALUES
('1', '1801', 'Processamento de Imagens', 1),
('2', '1802', 'Arquitetura de sistemas Distribuídos', 2);

INSERT INTO stdnt_enrl VALUES
('555555555555', '2', 1802),
('222222222222', '1', 1801),
('123123123123', '2', 1802),
('111111111111', '2', 1802),
('555555555555', '1', 1801);

INSERT INTO class_attendence VALUES
('1', '1801', '222222222222', '20180205', '18:15:00 PM', '19:30:00 PM', 'Y'),
('1', '1801', '222222222222', '20180205', '19:30:00 PM', '20:15:00 PM', 'Y'),
('1', '1801', '222222222222', '20180212', '18:15:00 PM', '19:30:00 PM', 'N'),
('1', '1801', '222222222222', '20180212', '19:30:00 PM', '20:15:00 PM', 'N'),
('1', '1801', '222222222222', '20180219', '18:15:00 PM', '19:30:00 PM', 'Y'),
('1', '1801', '222222222222', '20180219', '19:30:00 PM', '20:15:00 PM', 'Y'),
------------------------------------------
('1', '1801', '555555555555', '20180205', '18:15:00 PM', '19:30:00 PM', 'Y'),
('1', '1801', '555555555555', '20180205', '19:30:00 PM', '20:15:00 PM', 'N'),
('1', '1801', '555555555555', '20180212', '18:15:00 PM', '19:30:00 PM', 'Y'),
('1', '1801', '555555555555', '20180212', '19:30:00 PM', '20:15:00 PM', 'Y'),
('1', '1801', '555555555555', '20180219', '18:15:00 PM', '19:30:00 PM', 'Y'),
('1', '1801', '555555555555', '20180219', '19:30:00 PM', '20:15:00 PM', 'N'),
------------------------------------------
('2', '1802', '123123123123', '20180806', '18:15:00 PM', '19:30:00 PM', 'Y'),
('2', '1802', '123123123123', '20180806', '19:30:00 PM', '20:15:00 PM', 'Y'),
('2', '1802', '123123123123', '20180813', '18:15:00 PM', '19:30:00 PM', 'Y'),
('2', '1802', '123123123123', '20180813', '19:30:00 PM', '20:15:00 PM', 'Y'),
('2', '1802', '123123123123', '20180820', '18:15:00 PM', '19:30:00 PM', 'Y'),
('2', '1802', '123123123123', '20180820', '19:30:00 PM', '20:15:00 PM', 'Y'),
------------------------------------------
('2', '1802', '111111111111', '20180806', '18:15:00 PM', '19:30:00 PM', 'Y'),
('2', '1802', '111111111111', '20180806', '19:30:00 PM', '20:15:00 PM', 'Y'),
('2', '1802', '111111111111', '20180813', '18:15:00 PM', '19:30:00 PM', 'N'),
('2', '1802', '111111111111', '20180813', '19:30:00 PM', '20:15:00 PM', 'N'),
('2', '1802', '111111111111', '20180820', '18:15:00 PM', '19:30:00 PM', 'Y'),
('2', '1802', '111111111111', '20180820', '19:30:00 PM', '20:15:00 PM', 'Y');
--------------------------------------------
/*('2', '1802', '555555555555', '20180806', '18:15:00 PM', '19:30:00 PM', 'Y'),
('2', '1802', '555555555555', '20180806', '19:30:00 PM', '20:15:00 PM', 'Y'),
('2', '1802', '555555555555', '20180813', '18:15:00 PM', '19:30:00 PM', 'Y'),
('2', '1802', '555555555555', '20180813', '19:30:00 PM', '20:15:00 PM', 'Y'),
('2', '1802', '555555555555', '20180820', '18:15:00 PM', '19:30:00 PM', 'N'),
('2', '1802', '555555555555', '20180820', '19:30:00 PM', '20:15:00 PM', 'Y');*/

EXEC populatePresenceForSemester;

update facility_tbl
set latitude_north_east = -25.43394508
, longitude_north_east = -49.21893256
, latitude_north_west = -25.43394508
, longitude_north_west = -49.28372744
, latitude_south_east = -25.466634252
, longitude_south_east = -49.21893256
, latitude_south_west = -25.466634252
, longitude_south_west = -49.28372744
where facility_id = 2

select *
  from facility_tbl