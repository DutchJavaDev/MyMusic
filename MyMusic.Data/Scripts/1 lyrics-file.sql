set search_path to public;

create table if not exists lyric_snippet(
	serial serial primary key,
	music_serial int not null references music (serial),
	id int not null,
	start_time int not null,
	end_time int not null,
	lyric_text int not null
)
