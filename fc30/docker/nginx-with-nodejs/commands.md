## Some commands used

### Acess Mysql into the container of Mysql:

- `mysql -uroot -p`

### Password:

- `root`

### Show database list:

- `show databases`

### use database "nodedb":

- `use nodedb;`

### Create table "People" (if not exists):

- `CREATE TABLE IF NOT EXISTS people(id int not null auto_increment,name varchar(255), primary key(id));`

### Select all row from table "People":

- `SELECT * FROM people;`
