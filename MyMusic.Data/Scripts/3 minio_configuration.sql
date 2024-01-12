set search_path to mymusic;

create table if not exists minio_users(
	serial serial primary key,
	name text not null,
	password text not null,
	policy text not null
);