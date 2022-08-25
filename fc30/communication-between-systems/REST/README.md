# REST

REST API level 3 using laminas-api-tools (PHP)

### Links

- https://github.com/codeedu/api-tools-skeleton

### Commands

- Open folder on container (VSCode option -> `Remote-Containers: Add Development Container Configuration Files...`)
- Maybe you need access The Remote Explorer (extension), on the containers tab, and click in Add icon, and select `Open current folder in container`
- Select PHP at v7.4
- In the container run: `composer create-project laminas-api-tools/api-tools-skeleton`
- `cd api-tools-skeleton`
- `sudo su`
- `apt-get update`
- `apt-get install sqlite3`
- `touch test.sqlite`
- `sqlite3 test.sqlite`
- `create table user(id int, name varchar(255), email varchar(255));`
- `.tables`
- Ctrl + C to exit from sqlite
- `exit` (to leave of root)
- `cd api-tools-skeleton`
- `php -S 0.0.0.0:8080 -t public public/index.php`

- Access `http://localhost:8080` on your browser.
