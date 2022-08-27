# DynamicDAO
### _Providing an easy way to access databases and fill objects_

### About
---------
DynamicDAO is a .NET MicroORM that allows you to access and work with your database within minimal efforts.

### Features
- Creates an `IDBConnection` based on pre-defined information (Database provider, connection string, parameter identifier and database command type)
- Execute methods based on mapped objects or manually entered parameters 
- Queries are executed and returning data are converted to object automatically

### A little code
-----------------
Setting up your provider information:

```csharp
ProviderInfo provider = new ProviderInfo("System.Data.SqlClient", "Data Source=.\SQLEXPRESS;Initial Catalog=tempdb;User ID=sa;Password=adm")
```

Creating a data access object:

```csharp
using DynamicDAO;
...
using (AutoDAO db = new AutoDAO(provider))
{
    // Your code here
}
```

Adding manual parameters to the data access object (considers the following procedure):

```sql
-- @PERSON_NAME VARCHAR(100)
-- @PERSON_AGE INT
-- @PERSON_DOB DATE

INSERT INTO dbo.Person
    (PersonName, PersonAge, PersonDOB)
VALUES
    (@PERSON_NAME, @PERSON_AGE, @PERSON_DOB)
```

```csharp
using (AutoDAO db = new AutoDAO(provider))
{
    objetc[][] inputParameters = new object[3][];

    inputParameters[0] = new object { "NAME", "Raphael Crejo" };
    inputParameters[1] = new object { "AGE", 37 };
    inputParameters[2] = new object { "DOB", new DateTime(1985, 3, 15) };
    
    db.AddInputParameters(inputParameters);
    // Your code here
}
```

Adding mapped object to data access object (considers the following class):

```csharp
using System.Data;
using DynamicDAO.Mapping;
...
public class Person
{
    [Field("PersonId", "PERSON_ID")]
    public long PersonId { get; set; }
    
    [Field("PersonName", "PERSON_NAME")]
    [Parameter("PersonName", "PERSON_NAME", ParameterDirection.Input)]
    public string PersonName { get; set; }
    
    [Field("PersonAge", "PERSON_AGE")]
    [Parameter("PersonName", "PERSON_AGE", ParameterDirection.Input)]
    public int PersonAge { get; set; }
    
    [Field("PersonDOB", "PERSON_DOB")]
    [Parameter("PersonName", "PERSON_DOB", ParameterDirection.Input)]
    public DateTime PersonDOB { get; set; }
}
```

```csharp
Person person = new Person
{
    Name = "Raphael da Cunha Crejo",
    Age = 37,
    DOB = new DateTime(1985, 3, 15)
};

using (AutoDAO db = new AutoDAO(provider))
{
    db.AddParameters(person);
    // Your code here
}
```

`ExecuteScalar` and `ExecuteNonQuery` methods:

```csharp
object result = db.ExecuteScalar("INSERT_PERSON"); // considering that your stored procedure returns the Person ID
```
```csharp
int result = db.ExecuteNonQuery("INSERT_PERSON");
// or just
db.ExecuteNonQuery("INSERT_PERSON");
```

`Query<T>` and `QueryList<T>` methods (considers the `Person` class above):

```csharp
Person person = db.Query<Person>("SELECT_PERSON");
```

```csharp
List<Person> personList = db.QueryList<Person>("SELECT_PERSONS");
```

Removing parameters:

```csharp
db.ClearParameters(); // remove all parameters from IDBCommand
db.RemoveParameters(new string[] { "PERSON_DOB" }); // remove specific parameter from IDBCommand
```

### Version info
----------------
* 1.0.0-rc1
This library will stay Release Candidate 1 due by the following reasons:
    * `AddOutputParameter` are not tested
    * `IDBTransaction` are not implemented
    * Tested on SQL Server only. Tests on other databases pending 

### Future implementations
--------------------------
* Add an `IDBTransaction` to manage transactions
* Create an agnostic BulkCopy method, for mass-insert operations

### License
-----------
[MIT][Lic]

**This software is free, and I like it!**

[//]: #
[Lic]: <https://github.com/raphaelcrejo/DynamicDAO/blob/main/LICENSE>
