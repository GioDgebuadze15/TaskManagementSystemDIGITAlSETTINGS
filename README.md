# TaskManagementSystemDIGITAlSETTINGS

# Getting Started. 
The following instructions will help you setup the project on your local development machine.
### Prerequisites
	1. ASP.NET Core - The main c# based server side platform. Version .Net 6.
	
### Installing
All we are trying to achieve is getting the code on to your computer;
	1. After setting up the prerequisites, git clone or fork this repository.
	2. run KeyGenerator and move key file into TMS.Api.
  
If you want to use MsSql instead of in moemory databsae u can do the following;
	1. Set the DefaultDb connection string in the appsettings.json file in TMS.Api, to connect to your database in this project.
	2. comment UseInMemoryDatabase service in program.cs and uncomment UseSqlServer.
	3. Add migrations to create database using enity framework.

Username and password for admin is in program.cs. u can change it.
