DO
$$
BEGIN
   IF NOT EXISTS (
      SELECT FROM pg_database WHERE datname = 'microservices_users'
   ) THEN
      CREATE DATABASE microservices_users;
END IF;
END
$$;