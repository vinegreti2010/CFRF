INSERT INTO term_tbl VALUES 
(1801, '1º Semestre de 2018', '20180201', '20180630'),
(1802, '2º Semestre de 2018', '20180801', '20181130'),
(1901, '1º Semestre de 2019', '20190201', '20190630'),
(1902, '2º Semestre de 2019', '20190801', '20191130');

INSERT INTO facility_tbl VALUES
('Sala 1 Bloco 1', -49.1840973, -25.35457, -49.1840973, -25.370000, -49.200000, -25.35457, -49.200000, -25.370000),
('Sala 1 Bloco 2', -49.3000000, -25.10000, -49.3000000, -25.120000, -49.320000, -25.10000, -49.320000, -25.120000);

INSERT INTO personal_data VALUES
('Joao'),
('Maria'),
('Pietra'),
('Vinicius');

INSERT INTO class_tbl VALUES
(1801, 'Processamento de Imagens', 1),
(1802, 'Arquitetura de sistemas Distribuídos', 2);

INSERT INTO stdnt_enrl VALUES
(4, 2, 1802),
(4, 1, 1801),
(1, 2, 1802),
(2, 2, 1802),
(3, 1, 1801);

INSERT INTO class_attendence VALUES
(1, 1801, 3, '20180205', '18:15:00 PM', '19:30:00 PM', 'Y'),
(1, 1801, 3, '20180205', '19:30:00 PM', '20:15:00 PM', 'Y'),
(1, 1801, 3, '20180212', '18:15:00 PM', '19:30:00 PM', 'N'),
(1, 1801, 3, '20180212', '19:30:00 PM', '20:15:00 PM', 'N'),
(1, 1801, 3, '20180219', '18:15:00 PM', '19:30:00 PM', 'Y'),
(1, 1801, 3, '20180219', '19:30:00 PM', '20:15:00 PM', 'Y'),
------------------------------------------
(1, 1801, 4, '20180205', '18:15:00 PM', '19:30:00 PM', 'Y'),
(1, 1801, 4, '20180205', '19:30:00 PM', '20:15:00 PM', 'N'),
(1, 1801, 4, '20180212', '18:15:00 PM', '19:30:00 PM', 'Y'),
(1, 1801, 4, '20180212', '19:30:00 PM', '20:15:00 PM', 'Y'),
(1, 1801, 4, '20180219', '18:15:00 PM', '19:30:00 PM', 'Y'),
(1, 1801, 4, '20180219', '19:30:00 PM', '20:15:00 PM', 'N'),
------------------------------------------
(2, 1802, 1, '20180806', '18:15:00 PM', '19:30:00 PM', 'Y'),
(2, 1802, 1, '20180806', '19:30:00 PM', '20:15:00 PM', 'Y'),
(2, 1802, 1, '20180813', '18:15:00 PM', '19:30:00 PM', 'Y'),
(2, 1802, 1, '20180813', '19:30:00 PM', '20:15:00 PM', 'Y'),
(2, 1802, 1, '20180820', '18:15:00 PM', '19:30:00 PM', 'Y'),
(2, 1802, 1, '20180820', '19:30:00 PM', '20:15:00 PM', 'Y'),
------------------------------------------
(2, 1802, 2, '20180806', '18:15:00 PM', '19:30:00 PM', 'Y'),
(2, 1802, 2, '20180806', '19:30:00 PM', '20:15:00 PM', 'Y'),
(2, 1802, 2, '20180813', '18:15:00 PM', '19:30:00 PM', 'N'),
(2, 1802, 2, '20180813', '19:30:00 PM', '20:15:00 PM', 'N'),
(2, 1802, 2, '20180820', '18:15:00 PM', '19:30:00 PM', 'Y'),
(2, 1802, 2, '20180820', '19:30:00 PM', '20:15:00 PM', 'Y'),
------------------------------------------
(2, 1802, 4, '20180806', '18:15:00 PM', '19:30:00 PM', 'Y'),
(2, 1802, 4, '20180806', '19:30:00 PM', '20:15:00 PM', 'Y'),
(2, 1802, 4, '20180813', '18:15:00 PM', '19:30:00 PM', 'Y'),
(2, 1802, 4, '20180813', '19:30:00 PM', '20:15:00 PM', 'Y'),
(2, 1802, 4, '20180820', '18:15:00 PM', '19:30:00 PM', 'N'),
(2, 1802, 4, '20180820', '19:30:00 PM', '20:15:00 PM', 'Y');