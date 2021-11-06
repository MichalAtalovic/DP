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
INSERT INTO citation_category (code, name) values (1, 'Citácie v zahranicných publikáciách registrované v citacných indexoch Web of Science a databáze SCOPUS');
INSERT INTO citation_category (code, name) values (2, 'Citácie v domácich publikáciách registrované v citacných indexoch Web of Science a databáze SCOPUS');
INSERT INTO citation_category (code, name) values (3, 'Citácie v zahranicných publikáciách neregistrované v citacných indexoch');
INSERT INTO citation_category (code, name) values (4, 'Citácie v domácich publikáciách neregistrované v citacných indexoch');

INSERT INTO publication_category (category_group, code, name) values ('A1', 'AAA', 'Vedecké monografie vydané v zahranicných vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'AAB', 'Vedecké monografie vydané v domácich vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'ABA', 'Štúdie charakteru vedeckej monografie v casopisoch a zborníkoch vydané v zahranicných vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'ABB', 'Štúdie charakteru vedeckej monografie v casopisoch a zborníkoch vydané v domácich vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'ABC', 'Kapitoly vo vedeckých monografiách vydané v zahranicných vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A1', 'ABD', 'Kapitoly vo vedeckých monografiách vydané v domácich vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'ACA', 'Vysokoškolské ucebnice vydané v zahranicných vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'ACB', 'Vysokoškolské ucebnice vydané v domácich vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'BAA', 'Odborné knižné publikácie vydané v zahranicných vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'BAB', 'Odborné knižné publikácie vydané v domácich vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'BCB', 'Ucebnice pre stredné a základné školy');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'BCI', 'Skriptá a ucebné texty');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'CAA', 'Umelecké monografie, dramatické diela, scenáre, umelecké preklady publikácií, autorské katalógy vydané v zahranicných vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'CAB', 'Umelecké monografie, dramatické diela, scenáre, umelecké preklady publikácií, autorské katalógy vydané v domácich vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'CBA', 'Kapitoly v umeleckých monografiách, kapitoly umeleckých prekladov publikácií vydaných v zahranicných vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'CBB', 'Kapitoly v umeleckých monografiách, kapitoly umeleckých prekladov publikácií vydaných v domácich vydavatelstvách');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'EAI', 'Prehladové práce');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'EAJ', 'Odborné preklady publikácií');
INSERT INTO publication_category (category_group, code, name) values ('A2', 'FAI', 'Zostavovatelské práce knižného charakteru (bibliografie, encyklopédie, katalógy, slovníky, zborníky, atlasy...)');
INSERT INTO publication_category (category_group, code, name) values ('B', 'ADC', 'Vedecké práce v zahranicných karentovaných casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'ADD', 'Vedecké práce v domácich karentovaných casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'ADM', 'Vedecké práce v zahranicných casopisoch registrovaných v databázach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'ADN', 'Vedecké práce v domácich casopisoch registrovaných v databázach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AEG', 'Abstrakty vedeckých prác v zahranicných karentovaných casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AEH', 'Abstrakty vedeckých prác v domácich karentovaných casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AEM', 'Abstrakty vedeckých prác v zahranicných casopisoch registrovaných v databázach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AEN', 'Abstrakty vedeckých prác v domácich casopisoch registrovaných v databázach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'AGJ', 'Patentové prihlášky, prihlášky úžitkových vzorov, prihlášky dizajnov, prihlášky ochranných známok, žiadosti o udelenie dodatkových ochranných osvedcení, prihlášky topografií polovodicových výrobkov, prihlášky oznacení pôvodu výrobkov, prihlášky zemepisných oznacení výrobkov, prihlášky na udelenie šlachtitelských osvedcení');
INSERT INTO publication_category (category_group, code, name) values ('B', 'BDC', 'Odborné práce v zahranicných karentovaných casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'BDD', 'Odborné práce v domácich karentovaných casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'BDM', 'Odborné práce v zahranicných casopisoch registrovaných v databázach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'BDN', 'Odborné práce v domácich casopisoch registrovaných v databázach Web of Science alebo SCOPUS');
INSERT INTO publication_category (category_group, code, name) values ('B', 'CDC', 'Umelecké práce a preklady v zahranicných karentovaných casopisoch');
INSERT INTO publication_category (category_group, code, name) values ('B', 'CDD', 'Umelecké práce a preklady v domácich karentovaných casopisoch');

INSERT INTO export_format (id, code, template) values (1, 'APA6', '<b>${title}</b><i>${authors}</i><em>${(publicationYear)}</em><i>${journal}</i>');
