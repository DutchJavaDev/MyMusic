-- add column to set how many downloads are allowed to be run each cycle
set search_path to public;
alter table server_configuration
add if not exists concurrent_downloads int not null default 2