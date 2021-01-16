CREATE TABLE author_setting (
   setting_id 		SERIAL      NOT NULL,
   scholar     		BOOLEAN default true,
   open_citations 	BOOLEAN default false,
   semantics 		BOOLEAN default false,
   sync_status		BOOLEAN default false,
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

CREATE TABLE publication (
   pub_id 			SERIAL       	NOT NULL,
   author_id    	INT				NOT NULL,
   title     		VARCHAR(255),
   pub_year 		VARCHAR(255),
   author      		VARCHAR(255),
   journal          VARCHAR(255),
   volume 			VARCHAR(20),
   pages			VARCHAR(20),
   publisher 		VARCHAR(255),
   abstract 		TEXT,
   doi				VARCHAR(50),
   CONSTRAINT PKEY_PUBLICATION PRIMARY KEY (pub_id),
   FOREIGN KEY(author_id) REFERENCES author(author_id)
);

CREATE TABLE citation (
	citation_id 	SERIAL 			NOT NULL,
	title     		VARCHAR(255),
	author      	VARCHAR(255),
	pub_year 		VARCHAR(255),
	pub_url 		VARCHAR(255),
	venue 			VARCHAR(255),
	doi				VARCHAR(50),
	abstract 		TEXT,
	CONSTRAINT PKEY_CITATION PRIMARY KEY (citation_id)
);

CREATE TABLE publication_citation (
	pub_id 				    	INT NOT NULL,
	citation_id					INT NOT NULL,
	FOREIGN KEY(pub_id) 		REFERENCES publication(pub_id),
	FOREIGN KEY(citation_id) 	REFERENCES citation(citation_id)
);
