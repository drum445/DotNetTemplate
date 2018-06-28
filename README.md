## Prereqs
Local instance of MySQL/MariaDB (run dbinit.sql)  
Local install of dotnet core

## Running
#### First time
$ dotnet restore

#### Dev
$ dotnet run dev
#### Prod
$ sudo ./backend

## Building
$ dotnet publish -c Release -r ubuntu.18.04-x64

## Endpoints
### Todo
**View tasks**  
GET: http://localhost:5000/api/todo

**Create new task and insert into db**  
POST: http://localhost:5000/api/todo
```
{
	"title": "Task title",
	"body": "task description"
}
```

### Person
**Check if user is logged in, if so return username**  
GET: http://localhost:5000/api/person

**Attempt to login**  
POST: http://localhost:5000/api/person
```
{
	"username": "drum",
	"password": "password"
}
```

**Register**  
POST: http://localhost:5000/api/person/register
```
{
	"username": "drum",
	"password": "password"
}
```

**Logout user and clear session**  
POST: http://localhost:5000/api/person/logout

