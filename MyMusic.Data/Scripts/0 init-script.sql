set search_path to public;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

--generates a secure password
create or replace function generate_random_password() returns text
    as $$
declare
   j int4;
   result text;
   allowed text;
   allowed_len int4;
begin
   allowed := '0123456789abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ';
   allowed_len := length(allowed);
   result := '';
   while length(result) < 24 loop
      j := int4(random() * allowed_len);
      result := result || substr(allowed, j+1, 1);
   end loop;
   return result;
end;
$$
    language plpgsql;

-- config
create table if not exists server_configuration (
	created_utc timestamp not null default now(),
	server_password text not null,
	concurrent_downloads int not null default 2 -- how many downloads are allowed to be run each cycle
);

-- music
create table if not exists music (
	serial serial primary key,
	name text not null unique, -- constraint to prevent duplicates
	release_date date not null,
	created_utc timestamp not null default now(),
	tracking_id uuid not null default uuid_generate_v4()
);

-- download
create table if not exists download (
	serial serial primary key,
	music_serial int references music (serial),
	state int not null,
	video_id text not null unique, -- constraint to prevent duplicates
	created_utc timestamp not null default now()
);

-- mp3 files
create table if not exists mp3media (
	serial serial primary key,
	download_serial int references download (serial),
	file_path text not null unique, -- make unique
	created_utc timestamp not null default now()
);

-- logging
create table if not exists exception (
	serial serial primary key,
	message text not null,
	app text not null,
	stacktrace text null,
	created_utc timestamp not null default now()
);

-- set server password
DO $$ 
declare 
server_configuration_count int;
begin
server_configuration_count := 0;
select count(*) into server_configuration_count from 
(select 1 from public.server_configuration limit 1) as config;

	if server_configuration_count = 0 then
		insert into public.server_configuration(
		created_utc, server_password)
		values(now(),generate_random_password());
	end if;

end $$;