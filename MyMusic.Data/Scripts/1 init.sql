create schema if not exists mymusic;

set search_path to mymusic;

-- config
create table if not exists server_configuration (
	created_utc timestamp not null default now(),
	server_password text not null
);

-- music
create table if not exists music (
	serial serial primary key,
	name text not null unique, -- constraint to prevent duplicates
	release_date date not null,
	created_utc timestamp not null default now()
);

-- download
create table if not exists download (
	serial serial primary key,
	music_serial int references music (serial),
	state int not null,
	download_id text not null unique, -- constraint to prevent duplicates
	created_utc timestamp not null default now()
);

-- mp3 files
create table if not exists mp3media (
	serial serial primary key,
	download_serial int references download (serial),
	file_path text not null,
	created_utc timestamp not null default now()
);

-- logging
create table if not exists exception (
	serial serial primary key,
	message text not null,
	stacktrace text null,
	created_utc timestamp not null default now()
);

--thumbnails
-- might not even use this
--create table if not exists thumbnail (
--	serial serial primary key,
--	music_serial int references music (serial),
--	name text not null,
--	value text not null unique,
--);

-- set server password
DO $$ 
declare 
server_configuration_count int;

begin
server_configuration_count := 0;

select count(*) into server_configuration_count from 
(select 1 from mymusic.server_configuration limit 1);

	if server_configuration_count = 0 then
		insert into mymusic.server_configuration(
		created_utc, server_password)
		values(now(),generate_random_password());
	end if;

end $$;