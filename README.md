.NET 9, Dapper, Bogus, SQlite, Web API, Angular 

Three tables in Sqlite Employees and Projects were created, which have a many-to-many relationship; EmployeeProjects table ensures this feature;
The database and its tables will be created every time when the application is initialized;
The tables are filled with dummy data every time anew; Faker Bogus is used for this purpose;
Basic validations were applied when filling the data;
Two endpoints requested in the assignment are implemented;
For completeness, basic CRUD operations are also applied to Employees and Projects; they are not displayed as endpoints;
Dapper is used for data processing;
Six unit tests are written to ensure the solution;
Global error handling was applied to the API;
RepositoryResult was investigated further;
CustomDateConverter was provided; it allows different dates support;
Program.cs was set to enable CORS; different status codes were described;
Angular app was created:
- show the scv import feature;
- allows the requsted statistics for EmployeeProjects to be shown in a table.
