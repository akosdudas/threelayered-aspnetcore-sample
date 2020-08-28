# Three-layered ASP.NET Core web application (sample)

The source code is available in the `src` folder.

- You need a Microsoft SQL Server and the sample database. You can edit the connection string in `Webshop.Web/appsettings.json`.
- Web application: open the solution in Visual Studio, right-click project `Webshop.Web` and choose "Set as Startup Project", then **F5** to build and start; it will also open a browser to <http://localhost:5000>.
- REST API: start the application and use [Postman](https://www.getpostman.com/) to query <http://localhost:5000/api/customers>.
- There is a sample unit test that you can run with Visual Studio too.

## Structure of the application

The sample is a standard three-layered web app:

- `Webshop.Web`: ASP.NET Core web application; this is the entry point that hosts the web server
  - Contains an MVC Razor Pages web app
  - And also serves REST queries
- `Webshop.BL`: the business layer for managing the business workflows
- `Webshop.DAL`: the data access layer
  - Uses Enity Framework Core for data access
  - But wraps all operations using the _repository_ design pattern
- `Webshop.Test`: contains unit tests
