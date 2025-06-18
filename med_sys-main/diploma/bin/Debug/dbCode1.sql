create table IF NOT EXISTS  med_procedure
(
	id serial primary key,
	name varchar(50) not null,
	price integer,
	descr text,
	duration int

);


create table IF NOT EXISTS specialisation
(
	id serial primary key,
	name varchar(50) not null,
	descr text
);
create table IF NOT EXISTS post
(
	id serial primary key,
	name varchar(50) not null,
	descr text
);


create table IF NOT EXISTS employee
(
	id serial primary key,
	login varchar(50) not null,
	first_name varchar(20) not null,
	patronymic varchar(20) not null,
	last_name varchar(20) not null,
	email varchar(50) not null,
	phone varchar(12) not null,
	id_post integer REFERENCES post(id) on delete cascade,
	h_password varchar(500) not null,
	education text,
	experience varchar(50),
	gender varchar(50),
	birthdate date not null,
	date_of_emp date,
	passport_issued varchar(200),
	seria varchar(10),
	numb varchar(10),
	passport_issued_in date
);
--ALTER TABLE  employee DROP COLUMN if EXISTS experience;
--ALTER TABLE  employee add COLUMN experience varchar(50);
--ALTER TABLE employee ALTER COLUMN experience SET DEFAULT '45';

create table IF NOT EXISTS patient
(
	id serial primary key,
	login varchar(50) not null,
	first_name varchar(20) not null,
	patronymic varchar(20) not null,
	last_name varchar(20) not null,
	email varchar(50) not null,
	phone varchar(12) not null,
	snils VARCHAR(20),
	polis VARCHAR(20),
	h_password varchar(500) not null,
	gender varchar(50),
	birthdate date not null,
	passport_issued varchar(200),
	seria varchar(10),
	numb varchar(10),
	passport_issued_in date,
	register_date date
);





create table IF NOT EXISTS spec_emp
(
	id serial primary key,
	id_spec integer REFERENCES Specialisation (id) on delete cascade,
	id_emp integer REFERENCES Employee (id) on delete cascade
);

create table IF NOT EXISTS spec_proc
(
	id serial primary key,
	id_spec integer REFERENCES Specialisation (id) on delete cascade,
	id_proc integer REFERENCES med_procedure (id) on delete cascade
);



create table IF NOT EXISTS images_emp
(
  id serial primary key,
  name varchar(500),
  image bytea,
  id_emp integer REFERENCES Employee (id) on delete cascade
);

create table IF NOT EXISTS images_pat
(
  id serial primary key,
  name varchar(500),
  image bytea,
  id_pat integer REFERENCES Patient (id) on delete cascade
);


CREATE TABLE IF NOT EXISTS Diseases 
(
    id serial primary key,
    name VARCHAR(255) NOT NULL,
    MKB_code VARCHAR(20),
	symptoms text,
	descr text,
	treatment text
);
CREATE TABLE IF NOT EXISTS Medication 
(
    id serial primary key,
    name VARCHAR(255) NOT NULL,
	producer VARCHAR(255),
	release_form VARCHAR(50),
	descr text
);



CREATE TABLE IF NOT EXISTS Appointment 
(
    id serial primary key,
    id_spec integer REFERENCES Patient (id) on delete cascade,
	id_emp integer REFERENCES Employee (id) on delete cascade,
    id_proc integer REFERENCES med_procedure (id) on delete cascade,
    date_app date,
	time_app time,
    status VARCHAR(50)
);


CREATE TABLE IF NOT EXISTS Diagnoses (
    id serial primary key,
    id_app INT REFERENCES Appointment(id) on delete cascade,
    attendance int
);

CREATE TABLE IF NOT EXISTS Diagnos_dis (
    id serial primary key,
    id_diag INT REFERENCES Diagnoses(id) on delete cascade,
    id_dis INT REFERENCES Diseases(id) on delete cascade,
	descr text
);

CREATE TABLE IF NOT EXISTS Diagnos_med (
    id serial primary key,
    id_diag INT REFERENCES Diagnoses(id) on delete cascade,
    id_med INT REFERENCES Medication(id) on delete cascade,
    dosage text,
    duration text
);


CREATE TABLE IF NOT EXISTS Patient_add_info (
    id serial primary key,
    id_pat integer REFERENCES Patient (id) on delete cascade,
    height INT,
    weight DECIMAL(5, 2),
	phys_activ varchar(20),
    bloodType VARCHAR(100),
	smoking_status varchar(20),
	alcohol varchar(20),
	med_allergy text,
    allergies TEXT,
    chronic_dis Text,
    other Text
);

CREATE TABLE IF NOT EXISTS Schedule (
    id serial primary key,
    id_emp integer REFERENCES Employee (id) on delete cascade,
    time_start TIME,
    time_end TIME,
    date_shed date
);

CREATE TABLE IF NOT EXISTS Work_days (
    id serial primary key,
    id_emp integer REFERENCES Employee (id) on delete cascade,
    day_week varchar(60),
	time_start Time,
	time_end Time,
    is_working BOOLEAN
);

ALTER TABLE  med_procedure DROP COLUMN if EXISTS price;

-- Создаем таблицу DoctorProcedureCosts
CREATE TABLE IF NOT EXISTS emp_proc_cost (
    id serial primary key,
    id_emp integer REFERENCES Employee (id) on delete cascade,
	id_proc integer REFERENCES med_procedure (id) on delete cascade,
    cost DECIMAL(10, 2)
);

CREATE TABLE IF NOT EXISTS vaccine (
    id serial primary key,
	name varchar(50) not null,
	descr text,
	term int,  -- колво месяцев
	id_dis INT REFERENCES Diseases(id) on delete cascade,
	manufacturer varchar(100)
);

CREATE TABLE IF NOT EXISTS vaccination (
    id serial primary key,
	date_get date,
	date_exp date,
	id_vac INT REFERENCES vaccine(id) on delete cascade,
	id_emp integer REFERENCES Employee (id) on delete cascade,
	id_pat integer REFERENCES Patient (id) on delete cascade,
	status_vac varchar
);

CREATE TABLE IF NOT EXISTS analysis (
    id serial primary key,
	date_get date,
	type_analysis varchar(30),
	results text,
	mes_units varchar(10), --units of measurement of results
	norm text, -- normal limits for indicators
	deviation text, --description of deviations
	id_emp integer REFERENCES Employee (id) on delete cascade,
	id_pat integer REFERENCES Patient (id) on delete cascade,
	status_an varchar, --completed/expected/cancelled
	descr text
);


--ALTER TABLE  vaccine add COLUMN cost DECIMAL(10, 2) ;


Create or replace function calculate_date_exp()
returns TRIGGER as $$
begin
  NEW.date_exp := NEW.date_get + INTERVAL '1 month' * (select term from vaccine where id = NEW.id_vac);
  return NEW;
end;
$$ LANGUAGE plpgsql;

--Create trigger calculate_date_exp
--Before insert on vaccination
--for each row
--execute procedure calculate_date_exp();

