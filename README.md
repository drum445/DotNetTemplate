## Running

#### Dev
$ dotnet run dev
#### Prod
**Only need sudo if using SSL**
$ sudo ./backend

## Building
$ dotnet publish -c Release -r ubuntu.18.04-x64

## Endpoints
### Todo
**View tasks**  
GET: http://localhost:5000/api/todo

**Create new task, doesn't insert into db**  
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

**Attempt to login, accepts where username = "drum"**  
POST: http://localhost:5000/api/person
```
{
	"username": "drum",
	"password": "password"
}
```


**Logout user and clear session**  
POST: http://localhost:5000/api/person/logout

