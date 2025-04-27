DROP TABLE IF EXISTS "user";

CREATE TABLE "user" (
    "id" UUID NOT NULL PRIMARY KEY,
    "name" VARCHAR(100) NOT NULL,
    "email" VARCHAR(100) NOT NULL UNIQUE
);

INSERT INTO "user" ("id", "name", "email") VALUES
  ('e1a2b3c4-d5f6-7890-ab12-cd34ef56gh78', 'Alice Johnson', 'alice.johnson@example.com'),
  ('f2b3c4d5-e6a7-8901-bc23-de45fg67hi89', 'Bob Smith', 'bob.smith@example.com'),
  ('g3c4d5e6-f7b8-9012-cd34-ef56gh78ij90', 'Charlie Brown', 'charlie.brown@example.com'),
  ('h4d5e6f7-g8c9-0123-de45-fg67hi89jk01', 'Diana Prince', 'diana.prince@example.com'),
  ('i5e6f7g8-h9d0-1234-ef56-gh78ij90kl12', 'Ethan Hunt', 'ethan.hunt@example.com'),
  ('j6f7g8h9-i0e1-2345-fg67-hi89jk01lm23', 'Fiona Gallagher', 'fiona.gallagher@example.com'),
  ('k7g8h9i0-j1f2-3456-gh78-ij90kl12mn34', 'George Martin', 'george.martin@example.com'),
  ('l8h9i0j1-k2g3-4567-hi89-jk01lm23no45', 'Hannah Lee', 'hannah.lee@example.com'),
  ('m9i0j1k2-l3h4-5678-ij90-kl12mn34op56', 'Ian Wright', 'ian.wright@example.com'),
  ('n0j1k2l3-m4i5-6789-jk01-lm23no45pq67', 'Julia Roberts', 'julia.roberts@example.com');