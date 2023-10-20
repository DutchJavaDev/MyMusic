create schema if not exists mymusic;

set search_path to mymusic;

create table if not exists server_configuration (
	created_utc timestamp not null default now(),
	server_password varchar(255) not null
);

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


