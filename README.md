.NET 9, Dapper, Bogus, SQlite, Web API, Angular 

Three tables in Sqlite Employees and Projects were created, which have a many-to-many relationship; EmployeeProjects table ensures this feature;
The database and its tables will be created every time when the application is initialized;
The tables are filled with dummy data every time anew; Faker Bogus is used for this purpose;
Basic validations were applied when filling the data;
Two endpoints requested in the assignment are implemented;
For completeness, basic CRUD operations are also applied to Employees and Projects; they are not displayed as endpoints;
Dapper is used for data processing;
Eight unit tests are written to ensure the solution;
Global error handling was applied to the API;
RepositoryResult was investigated further;
CustomDateConverter was provided; it allows different dates support;
Program.cs was set to enable CORS; different status codes were described;
Angular app was created:
- show the scv import feature;
- allows the requested statistics for EmployeeProjects to be shown in a table.

Steps to run the app:
- pull/download the repo
- set API project as start project if is not set;
- open swagger http://localhost:5095/index.html	- you will see the endpoint for employees who have worked together on some project
- test the API endpoint
- run unit test to figure out all scenarios I tested
- run the angular app in some terminal separately through ng serve and wait all packages to be installed; it is running on 4200
- endpoints:
    http://localhost:5095/employeeprojects
    http://localhost:5095/import