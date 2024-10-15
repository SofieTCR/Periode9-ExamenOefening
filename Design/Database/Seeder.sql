SET foreign_key_checks = 0; -- DANGER!
START TRANSACTION;

-- Delete existing data
DELETE FROM `electablemember` WHERE 1;
DELETE FROM `election` WHERE 1;
DELETE FROM `party` WHERE 1;
DELETE FROM `user` WHERE 1;

-- Users
ALTER TABLE `user` AUTO_INCREMENT = 1;
INSERT INTO `user` (Username, Password, Firstname, Lastname, Email, Birthdate, City, Party_PartyId) VALUES 
-- Government
('SofieBrink', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Sofie', 'Brink', '9019232@student.zadkine.nl', '2004-08-11', 'Rotterdam', NULL), 
('LemonAddicted', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Stacey', 'Peters', '9018900@student.zadkine.nl', '2007-01-17', 'Rotterdam', NULL),
('JackOnKrack', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Jack', 'van Bommel', '9019233@student.zadkine.nl', '2005-01-15', 'Rotterdam', NULL),
-- Non Partisan Voters
('AmandaGroen', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Amanda', 'Groen', 'amanda.groen@gmail.com', '2003-05-23', 'Amsterdam', NULL),
('TomJanssen', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Tom', 'Janssen', 'tom.janssen@gmail.com', '1968-09-09', 'Utrecht', NULL),
('NinaVermeer', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Nina', 'Vermeer', 'nina.vermeer@gmail.com', '1958-12-13', 'Leiden', NULL),
('LucasSchouten', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Lucas', 'Schouten', 'lucas.schouten@gmail.com', '1992-07-04', 'Groningen', NULL),
('JadeBakker', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Jade', 'Bakker', 'jade.bakker@gmail.com', '2007-11-30', 'Rotterdam', NULL),
('BartdeVries', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Bart', 'de Vries', 'bart.devries@gmail.com', '1999-04-20', 'Maastricht', NULL),
('LunaDeBoer', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Luna', 'De Boer', 'luna.deboer@gmail.com', '2001-10-02', 'Rotterdam', NULL),
-- VVD Members
('MarkRutte', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Mark', 'Rutte', 'mark.rutte@gmail.com', '1967-02-14', 'Den Haag', 1),
('SimonGroot', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Simon', 'de Groot', 'simon.groot@gmail.com', '2000-09-09', 'Haarlem', 1),
('NenaMartens', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Nena', 'Martens', 'nena.martens@gmail.com', '1998-12-13', 'Delft', 1),
('AbegailKolijn', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Abigail', 'Kolijn', 'abegail.kolijn@gmail.com', '2002-12-02', 'Apeldoorn', 1),
-- GroenLinks Members
('JesseKlaver', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Jesse', 'Klaver', 'jesse.klaver@gmail.com', '1986-05-01', 'Roosendaal', 2),
('TanjaRietveld', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Tanja', 'Rietveld', 'tanja.rietveld@gmail.com', '1978-07-05', 'Kinderdijk', 2),
('LeoHout', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Leo', 'van der Hout', 'loe.hout@gmail.com', '1953-10-02', 'Groningen', 2),
('ThijsVroom', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Thijs', 'Vroom', 'thijs.vroom@gmail.com', '1996-12-18', 'Noordwijk', 2),
-- D66 Members
('RobJetten', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Rob', 'Jetten', 'rob.jetten@gmail.com', '1987-03-25', 'Veghel', 3),
('RobinSneider', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Robin', 'Sneider', 'robin.sneider@gmail.com', '1954-12-11', 'Scheveningen', 3),
('EvertJoosten', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Evert', 'Joosten', 'evert.joosten@gmail.com', '1986-05-09', 'Leiden', 3),
('RoelAbbing', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Roel', 'Abbing', 'roel.abbing@gmail.com', '1972-03-11', 'Lelystad', 3),
-- PvdA Members
('WillemDrees', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Willem', 'Drees', 'willem.drees@gmail.com', '1886-07-05', 'Amsterdam', 4),
('JesseCoemans', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Jesse', 'Coemans', 'jesse.coemans@gmail.com', '1983-07-02', 'Maastricht', 4),
('GuusBeek', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Guus', 'van Beek', 'guus.beek@gmail.com', '1982-05-11', 'Hilversum', 4),
('TygoBeek', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Tygo', 'van Beek', 'tygo.beek@gmail.com', '2001-09-11', 'Franeker', 4),
-- CDA Members
('WopkeHoekstra', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Wopke', 'Hoekstra', 'wopke.hoekstra@gmail.com', '1975-09-30', 'Bennekom', 5),
('MonaKeijzer', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Mona', 'Keijzer', 'mona.keijzer@gmail.com', '1968-10-09', 'Edam', 5),
('HugoDeJonge', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Hugo', 'de Jonge', 'hugo.dejonge@gmail.com', '1977-09-26', 'Capelle aan den IJssel', 5),
('PieterOmtzigt', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Pieter', 'Omtzigt', 'pieter.omtzigt@gmail.com', '1974-09-08', 'Enschede', 5),
-- PVV Members
('GeertWilders', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Geert', 'Wilders', 'geert.wilders@gmail.com', '1963-09-06', 'Venlo', 6),
('RaymondDeRoon', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Raymond', 'de Roon', 'raymond.deroon@gmail.com', '1952-09-01', 'Amsterdam', 6),
('FleurAgema', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Fleur', 'Agema', 'fleur.agema@gmail.com', '1976-09-16', 'Purmerend', 6),
('HarmBeertema', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Harm', 'Beertema', 'harm.beertema@gmail.com', '1952-03-09', 'Rotterdam', 6),
-- SP Members
('LilianMarijnissen', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Lilian', 'Marijnissen', 'lilian.marijnissen@gmail.com', '1985-07-11', 'Oss', 7),
('RonMeyer', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Ron', 'Meyer', 'ron.meyer@gmail.com', '1981-04-21', 'Heerlen', 7),
('TinyKox', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Tiny', 'Kox', 'tiny.kox@gmail.com', '1953-05-06', 'Tilburg', 7),
('JannieVisscher', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Jannie', 'Visscher', 'jannie.visscher@gmail.com', '1961-07-03', 'Groningen', 7),
-- FvD Members
('ThierryBaudet', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Thierry', 'Baudet', 'thierry.baudet@gmail.com', '1983-01-28', 'Heemstede', 8),
('TheoHiddema', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Theo', 'Hiddema', 'theo.hiddema@gmail.com', '1944-04-01', 'Maastricht', 8),
('EvaVlaardingerbroek', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Eva', 'Vlaardingerbroek', 'eva.vlaardingerbroek@gmail.com', '1996-09-03', 'Amsterdam', 8),
('FreekJansen', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Freek', 'Jansen', 'freek.jansen@gmail.com', '1992-08-01', 'Ede', 8),
-- ChristenUnie Members
('GertJanSegers', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Gert-Jan', 'Segers', 'gertjan.segers@gmail.com', '1969-07-09', 'Lisse', 9),
('CarolaSchouten', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Carola', 'Schouten', 'carola.schouten@gmail.com', '1977-10-06', 'Rotterdam', 9),
('StienekevanDerGraaf', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Stieneke', 'van der Graaf', 'stieneke.vandergraaf@gmail.com', '1984-09-07', 'Kampen', 9),
('MirjamBikker', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Mirjam', 'Bikker', 'mirjam.bikker@gmail.com', '1982-10-05', 'Gouda', 9),
-- Partij voor de Dieren Members
('EstherOuwehand', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Esther', 'Ouwehand', 'esther.ouwehand@gmail.com', '1976-06-10', 'Katwijk', 10),
('ChristineTeunissen', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Christine', 'Teunissen', 'christine.teunissen@gmail.com', '1985-09-06', 'Den Haag', 10),
('FrankWassenberg', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Frank', 'Wassenberg', 'frank.wassenberg@gmail.com', '1966-04-02', 'Sittard', 10),
('LammertvanRaan', '$2a$10$x50ecn2bVLTh1fB4KsxK8OYBk6aZC2LS47t5xuZaJvZPqsuMCcbMS', 'Lammert', 'van Raan', 'lammert.vanraan@gmail.com', '1962-04-03', 'Rotterdam', 10);


-- Parties
ALTER TABLE `party` AUTO_INCREMENT = 1;
INSERT INTO `party` (Name, Description, Positions, LogoLink, Leader_UserId) VALUES 
('VVD', 'Volkspartij voor Vrijheid en Democratie. Een liberale partij die zich inzet voor individuele vrijheid en economische groei.', '<ul><li>Economische groei</li><li>Lagere belastingen</li><li>Minder overheid</li></ul>', 'https://upload.wikimedia.org/wikipedia/commons/thumb/7/7a/VVD_logo_%282020%E2%80%93present%29.svg/1200px-VVD_logo_%282020%E2%80%93present%29.svg.png', 11),
('GroenLinks', 'GroenLinks zet zich in voor duurzaamheid, gelijkheid en sociale rechtvaardigheid.', '<ul><li>Klimaatverandering</li><li>Sociale gelijkheid</li><li>Inkomensherverdeling</li></ul>', 'https://upload.wikimedia.org/wikipedia/commons/c/cb/GroenLinks_logo_%28variant%29.png', 15),
('D66', 'Democraten 66, een progressieve partij gericht op onderwijs, duurzaamheid en democratische vernieuwing.', '<ul><li>Onderwijs</li><li>Klimaat</li><li>EU-integratie</li></ul>', 'https://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/D66_logo_%282019%E2%80%93present%29.svg/1200px-D66_logo_%282019%E2%80%93present%29.svg.png', 19),
('PvdA', 'Partij van de Arbeid, een sociaal-democratische partij die zich richt op sociale gelijkheid en solidariteit.', '<ul><li>Sociale gelijkheid</li><li>Werkgelegenheid</li><li>Zorg</li></ul>', 'https://albrandswaard.pvda.nl/wp-content/uploads/sites/14/2022/01/Pvda-logo-Boven-elkaar-Rood-RGB.png', 23),
('CDA', 'Christen-Democratisch Appèl, een partij die waarden zoals gezin, geloof en gemeenschapszin centraal stelt.', '<ul><li>Gezin</li><li>Duurzaamheid</li><li>Europese samenwerking</li></ul>', 'https://upload.wikimedia.org/wikipedia/commons/thumb/0/0a/CDA_logo_2021.svg/1200px-CDA_logo_2021.svg.png', 27),
('PVV', 'Partij voor de Vrijheid, een rechts-populistische partij die sterk anti-immigratie en eurosceptisch is.', '<ul><li>Anti-immigratie</li><li>Nationale soevereiniteit</li><li>Veiligheid</li></ul>', 'https://www.denederlandsegrondwet.nl/9353000/1/j4nvh0qavhjkdqd_j9vvkl1oucfq6v2/vle1m33215xy?sizew=372&sizeh=268&lm=qkb283', 31),
('SP', 'Socialistische Partij, een links-radicale partij die pleit voor herverdeling van rijkdom en controle over de zorg.', '<ul><li>Sociale rechtvaardigheid</li><li>Herverdeling van rijkdom</li><li>Gezondheidszorg</li></ul>', 'https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Socialistische_Partij_%28nl_2006%29_Logo.svg/1200px-Socialistische_Partij_%28nl_2006%29_Logo.svg.png', 35),
('FvD', 'Forum voor Democratie, een rechts-conservatieve partij die zich richt op nationale soevereiniteit en directe democratie.', '<ul><li>Directe democratie</li><li>Euroscepsis</li><li>Immigratie</li></ul>', 'https://winkel.forumvoordemocratie.nl/cdn/shop/files/FVD_LOGO_RGB__cirkel_met_tekst_ROOD_2.png?v=1695638800&width=600', 39),
('ChristenUnie', 'Een christelijke partij met nadruk op gezin, zorg en rentmeesterschap over de aarde.', '<ul><li>Zorg</li><li>Ethiek</li><li>Duurzaamheid</li></ul>', 'https://www.christenunie.nl/l/library/download/urn:uuid:3f0f25db-8450-4764-8654-4a1295e492b8/cu-impactlogo-rgb.png?format=save_to_disk&ext=.png', 43),
('Partij voor de Dieren', 'Een partij die zich inzet voor dierenrechten, milieu en duurzaamheid.', '<ul><li>Dierenwelzijn</li><li>Milieu</li><li>Duurzaamheid</li></ul>', 'https://upload.wikimedia.org/wikipedia/commons/9/9e/Party_for_the_Animals_logo.svg', 47);


-- Elections
ALTER TABLE `election` AUTO_INCREMENT = 1;
INSERT INTO `election` (Name, Description, Date) VALUES 
('Landelijke Verkiezingen 2021', 'Tweede kamer verkiezingen 5 maart 2021.', '2021-03-05'),
('Landelijke Verkiezingen 2022', 'Tweede kamer verkiezingen 5 maart 2022.', '2022-03-05'),
('Landelijke Verkiezingen 2023', 'Tweede kamer verkiezingen 5 maart 2023.', '2023-03-05'),
('Provinciale Verkiezingen 2023', 'Tweede kamer verkiezingen 7 september 2023.', '2023-09-07'),
('Provinciale Verkiezingen 2024', 'Tweede kamer verkiezingen 7 september 2024.', '2024-09-07'),
(CONCAT('Eerste Kamer Verkiezingen ', YEAR(CURDATE())), 'Verkiezingen voor de Eerste Kamer.', CURDATE()),
('Landelijke Verkiezingen 2025', 'Tweede kamer verkiezingen 5 maart 2025.', '2025-03-05'),
('Provinciale Verkiezingen 2026', 'Provinciale Staten verkiezingen 7 september 2026.', '2026-09-07');

COMMIT;
SET foreign_key_checks = 1; -- Back to safety