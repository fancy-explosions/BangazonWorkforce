# Bangazon

This is a C# ASP.NET MVC Razor Tempalte Web Form App that makes each resource in Bangazon available to internal users of Bangazon Inc.

The resources currently within the databases are:

1. Products
1. Product types
1. Customers
1. Orders
1. Payment types
1. Employees
1. Computers
1. Training programs
1. Departments

### Prerequisites

What things you need to install the software and how to install them:
1. Visual Studio 2019 [click here to view installation instructions](https://visualstudio.microsoft.com/downloads/)
2. Azure Data Studio [click here to view installation instructions](https://docs.microsoft.com/en-us/sql/azure-data-studio/download?view=sql-server-2017)

## Getting Started

These instructions will get you a copy of the project up and running, from scratch, on your local machine for development and testing purposes.

1. `git clone git@github.com:fancy-explosions/BangazonWorkforce.git`
2. You will be using the [Official Bangazon SQL](https://github.com/fancy-explosions/BangazonWorkforce/blob/master/bangazon.sql) file to create your database. Create the database using Azure Data Studio, or similar application. Create a new SQL query for that database, copy the contents of the SQL file into your script, and then run it.
3. Once the database is created, find the SQLDATA.sql file located in the BangazonWorkforce root directory. Import, or copy paste, this file into Azure Data Studio (or similar application) and run the insert queries in order from the top (i.e. start with INSERT INTO Customer on line 14 within the file).
4. Run the SELECT statements at the very top after this in order to ensure that the tables have been populated with the correct data, which is referenced in the Unit Tests for each resource in the /TestBangazonAPI directory.

## Running the app

1. Open the BangazonWorkforce.sln file within Visual Studio
1. Check to make sure that the `IIS Express` is changed to `BangazonWorkFroce` within the dropdown menu of the Run (play arrow) button on your top toolbar within Visual Studio.
1. A browser should automatically run containing app. If a browser does not

## Running manual tests
1. 

## Authors

* **Sam Brit**
* **Billy Mitchell**
* **Joel Mondesir**
* **Michael Yankura**

## Acknowledgments

* Hat tip to **Andy Collins** and **Leah Hoefling** for helping out throughout the project!
* Kudos to **Steve Brownlee** and **Jisie David** for the help you all have provided in our back end education!
