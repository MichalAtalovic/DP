DROP TABLE IF EXISTS quarantine_citation;
DROP TABLE IF EXISTS publication_citation;
DROP TABLE IF EXISTS citation;
DROP TABLE IF EXISTS quarantine;
DROP TABLE IF EXISTS publication;
DROP TABLE IF EXISTS author;
DROP TABLE IF EXISTS author_setting;

CREATE TABLE author_setting (
   setting_id 			SERIAL  NOT NULL,
   scholar     			BOOLEAN default true,
   open_citations 		BOOLEAN default false,
   semantics 			BOOLEAN default false,
   library_table_view 	BOOLEAN default false,
   graph_font_size      numeric(2) default 14,
   CONSTRAINT PKEY_SETTING PRIMARY KEY (setting_id)
);

CREATE TABLE author (
	author_id 	 SERIAL 	NOT NULL,
	scholar_id 	 VARCHAR(20),
	name         VARCHAR(100),
	display_name VARCHAR(100),
	url_picture  VARCHAR(500),
	affiliation  VARCHAR(500),
	total_cites  NUMERIC(20),
	setting_id   INT,
	CONSTRAINT PKEY_AUTHOR PRIMARY KEY (author_id),
	FOREIGN KEY(setting_id) REFERENCES author_setting(setting_id)
);

CREATE TABLE publication_category (
   id 					SERIAL  NOT NULL,
   code     			VARCHAR(30) NOT NULL,
   category_group     	VARCHAR(30) NOT NULL,
   name 				VARCHAR(500) NOT NULL,
   UNIQUE(code),
   CONSTRAINT PKEY_PUBLICATION_CATEGORY PRIMARY KEY (id)
);

CREATE TABLE publication (
   pub_id 					SERIAL NOT NULL,
   author_id    			INT	NOT NULL,
   publication_category_id  INT,
   title     				VARCHAR(255),
   pub_year 				VARCHAR(255),
   author      				VARCHAR(255),
   journal          		VARCHAR(255),
   volume 					VARCHAR(20),
   pages					VARCHAR(20),
   publisher 				VARCHAR(255),
   abstract 				TEXT,
   doi						VARCHAR(50),
   cites_per_year   		VARCHAR(255),
   eprint_url       		VARCHAR(1000),
   CONSTRAINT PKEY_PUBLICATION PRIMARY KEY (pub_id),
   FOREIGN KEY(author_id) REFERENCES author(author_id),
   FOREIGN KEY(publication_category_id) REFERENCES publication_category(id)
);

CREATE TABLE quarantine (
   pub_id 					SERIAL NOT NULL,
   author_id    			INT	NOT NULL,
   publication_category_id  INT,
   title     				VARCHAR(255),
   pub_year 				VARCHAR(255),
   author      				VARCHAR(255),
   journal          		VARCHAR(255),
   volume 					VARCHAR(20),
   pages					VARCHAR(20),
   publisher 				VARCHAR(255),
   abstract 				TEXT,
   doi						VARCHAR(50),
   cites_per_year   		VARCHAR(255),
   eprint_url       		VARCHAR(1000),
   CONSTRAINT PKEY_QUARANTINE PRIMARY KEY (pub_id),
   FOREIGN KEY(author_id) REFERENCES author(author_id),
   FOREIGN KEY(publication_category_id) REFERENCES publication_category(id)
);

CREATE TABLE citation_category (
   id 					SERIAL  NOT NULL,
   code     			numeric NOT NULL,
   name 				VARCHAR(500) NOT NULL,
   UNIQUE(code),
   CONSTRAINT PKEY_CITATION_CATEGORY PRIMARY KEY (id)
);

CREATE TABLE citation (
	citation_id 				SERIAL 	NOT NULL,
	citation_category_id  		INT,
	title     					VARCHAR(255),
	author      				VARCHAR(255),
	pub_year 					VARCHAR(255),
	pub_url 					VARCHAR(255),
	journal 					VARCHAR(255),
	volume 						VARCHAR(20),
	doi							VARCHAR(50),
	abstract 					TEXT,
	CONSTRAINT PKEY_CITATION PRIMARY KEY (citation_id),
	FOREIGN KEY(citation_category_id) REFERENCES citation_category(id)
);

CREATE TABLE publication_citation (
	pub_id 				    	INT NOT NULL,
	citation_id					INT NOT NULL,
	FOREIGN KEY(pub_id) 		REFERENCES publication(pub_id),
	FOREIGN KEY(citation_id) 	REFERENCES citation(citation_id)
);

CREATE TABLE quarantine_citation (
	quarantine_id 				INT NOT NULL,
	citation_id					INT NOT NULL,
	FOREIGN KEY(quarantine_id) 	REFERENCES quarantine(pub_id),
	FOREIGN KEY(citation_id) 	REFERENCES citation(citation_id)
);

CREATE TABLE export_format (
   id 					SERIAL  NOT NULL,
   code     			VARCHAR(30) NOT NULL,
   template 			VARCHAR(500) NOT NULL,
   UNIQUE(code),
   CONSTRAINT PKEY_EXPORT_FORMAT PRIMARY KEY (id)
);

-- INITIALIZE ENUM DATA
INSERT INTO citation_category (code, name) values (1, 'Cit�cie v zahranicn�ch publik�ci�ch registrovan� v citacn�ch indexoch Web of Science a datab�ze SCOPUS');
INSERT INTO citation_category (code, name) values (2, 'Cit�cie v dom�cich publik�ci�ch registrovan� v citacn�ch indexoch Web of Science a datab�ze SCOPUS');
INSERT INTO citation_category (code, name) values (3, 'Cit�cie v zahranicn�ch publik�ci�ch neregistrovan� v citacn�ch indexoch');
INSERT INTO citation_category (code, name) values (4, 'Cit�cie v dom�cich publik�ci�ch neregistrovan� v citacn�ch indexoch');

INSERT INTO publication_category (category_group, code, name) values ('A1', 'AAA', 'Vedeck� monografie vydan� v zahranicn�ch vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'AAB', 'Vedeck� monografie vydan� v dom�cich vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'ABA', '�t�die charakteru vedeckej monografie v casopisoch a zborn�koch vydan� v zahranicn�ch vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'ABB', '�t�die charakteru vedeckej monografie v casopisoch a zborn�koch vydan� v dom�cich vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'ABC', 'Kapitoly vo vedeck�ch monografi�ch vydan� v zahranicn�ch vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'ABD', 'Kapitoly vo vedeck�ch monografi�ch vydan� v dom�cich vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'ACA', 'Vysoko�kolsk� ucebnice vydan� v zahranicn�ch vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'ACB', 'Vysoko�kolsk� ucebnice vydan� v dom�cich vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'BAA', 'Odborn� kni�n� publik�cie vydan� v zahranicn�ch vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'BAB', 'Odborn� kni�n� publik�cie vydan� v dom�cich vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'BCB', 'Ucebnice pre stredn� a z�kladn� �koly');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'BCI', 'Skript� a ucebn� texty');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'CAA', 'Umeleck� monografie, dramatick� diela, scen�re, umeleck� preklady publik�ci�, autorsk� katal�gy vydan� v zahranicn�ch vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'CAB', 'Umeleck� monografie, dramatick� diela, scen�re, umeleck� preklady publik�ci�, autorsk� katal�gy vydan� v dom�cich vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'CBA', 'Kapitoly v umeleck�ch monografi�ch, kapitoly umeleck�ch prekladov publik�ci� vydan�ch v zahranicn�ch vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'CBB', 'Kapitoly v umeleck�ch monografi�ch, kapitoly umeleck�ch prekladov publik�ci� vydan�ch v dom�cich vydavatelstv�ch');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'EAI', 'Prehladov� pr�ce');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'EAJ', 'Odborn� preklady publik�ci�');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'FAI', 'Zostavovatelsk� pr�ce kni�n�ho charakteru (bibliografie, encyklop�die, katal�gy, slovn�ky, zborn�ky, atlasy...)');
INSERT INTO publication_category (category_group, code, name) values ('B', 'ADC', 'Vedeck� pr�ce v zahranicn�ch karentovan�ch casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'ADD', 'Vedeck� pr�ce v dom�cich karentovan�ch casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'ADM', 'Vedeck� pr�ce v zahranicn�ch casopisoch registrovan�ch v datab�zach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'ADN', 'Vedeck� pr�ce v dom�cich casopisoch registrovan�ch v datab�zach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AEG', 'Abstrakty vedeck�ch pr�c v zahranicn�ch karentovan�ch casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AEH', 'Abstrakty vedeck�ch pr�c v dom�cich karentovan�ch casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AEM', 'Abstrakty vedeck�ch pr�c v zahranicn�ch casopisoch registrovan�ch v datab�zach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AEN', 'Abstrakty vedeck�ch pr�c v dom�cich casopisoch registrovan�ch v datab�zach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AGJ', 'Patentov� prihl�ky, prihl�ky ��itkov�ch vzorov, prihl�ky dizajnov, prihl�ky ochrann�ch zn�mok, �iadosti o udelenie dodatkov�ch ochrann�ch osvedcen�, prihl�ky topografi� polovodicov�ch v�robkov, prihl�ky oznacen� p�vodu v�robkov, prihl�ky zemepisn�ch oznacen� v�robkov, prihl�ky na udelenie �lachtitelsk�ch osvedcen�');
INSERT INTO publication_category (category_group, code, name) values ('B', 'BDC', 'Odborn� pr�ce v zahranicn�ch karentovan�ch casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'BDD', 'Odborn� pr�ce v dom�cich karentovan�ch casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'BDM', 'Odborn� pr�ce v zahranicn�ch casopisoch registrovan�ch v datab�zach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'BDN', 'Odborn� pr�ce v dom�cich casopisoch registrovan�ch v datab�zach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'CDC', 'Umeleck� pr�ce a preklady v zahranicn�ch karentovan�ch casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'CDD', 'Umeleck� pr�ce a preklady v dom�cich karentovan�ch casopisoch');

INSERT INTO export_format (id, code, template) values (1, 'APA6', '<b>${title}</b><i>${authors}</i><em>${(publicationYear)}</em><i>${journal}</i>');
