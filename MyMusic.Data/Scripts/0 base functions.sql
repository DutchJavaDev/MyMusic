set search_path to public;
--generates a secure password
create or replace function generate_random_password() returns text
    as $$
declare
   j int4;
   result text;
   allowed text;
   allowed_len int4;
begin
   allowed := '0123456789abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ!@#$%^&*()_+';
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