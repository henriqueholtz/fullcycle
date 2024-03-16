Create table clients (id varchar(255), name varchar(255), email varchar(255), created_at date);
Create table accounts (id varchar(255), client_id varchar(255), balance int, created_at date);
Create table transactions (id varchar(255), account_id_from varchar(255), account_id_to varchar(255), amount int, created_at date);

INSERT INTO clients VALUES ('a33ab931-7268-469f-b3bc-7946e126a8b7', 'Jhon Doe', 'j@example.com', '2024-03-16');
INSERT INTO clients VALUES ('5e88ddae-98e0-4a0d-bda1-ebe85532a823', 'Mary Groes', 'm@example.com', '2024-03-16');


INSERT INTO accounts VALUES ('c9222008-579e-4b3f-8a2e-4e09910bd2dd', 'a33ab931-7268-469f-b3bc-7946e126a8b7', 1000,  '2024-03-16');
INSERT INTO accounts VALUES ('c5cb7470-999d-4a85-9a23-63ac83e1c89c', '5e88ddae-98e0-4a0d-bda1-ebe85532a823', 1000,  '2024-03-16');