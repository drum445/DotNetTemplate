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
*view tasks*
GET: http://localhost:5000/api/todo

*create new task, doesn't insert into db*
POST: http://localhost:5000/api/todo
{
	"title": "Task title",
	"body": "task description"
}

### Person
*check if user is logged in, if so return username*
GET: http://localhost:5000/api/person

*attempt to login, accepts where username = "drum"*
POST: http://localhost:5000/api/person
{
	"username": "drum",
	"password": "oasswird"
}

*logout user and clear session*
POST: http://localhost:5000/api/person/logout

