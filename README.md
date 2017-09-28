# Exploring-Dapper

#### Summary

Code samples for Dapper presentation

#### Getting up and running

This repository consists of a visual studio solution with one Unit (Integration) Tests project. 
To run the tests build the solution and use your favorite test runner.

The following Frameworks are used in this solution
* Dapper
* NUnit
* Fluent.Assertions

To run the tests you will need to have Northwind Database installed somewhere and the connection string (in Constants.cs file) needs to be updated to point to that.

NOTE: Since these are integration tests there is No mocking !!! We hit the database in every single test