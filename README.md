# DynamicDAO - _Providing an easy way to access databases and fill objects_


[![license](https://img.shields.io/badge/license-MIT-brightgreen)](https://github.com/raphaelcrejo/DynamicDAO/blob/main/LICENSE) [![MSBuild](https://github.com/raphaelcrejo/DynamicDAOfx/actions/workflows/msbuild.yml/badge.svg)](https://github.com/raphaelcrejo/DynamicDAOfx/actions/workflows/msbuild.yml) [![Donate](https://img.shields.io/badge/Donate-PayPal-informational.svg)](https://www.paypal.com/donate/?hosted_button_id=544HTWNBJUUPG)

**Note:** This repository contains the **.NET Framework** implementation of the library. For the **.NET6** implementation, [click here][N6]

## About
DynamicDAO is a .NET MicroORM that allows you to access and work with your database within minimal efforts.

## Features
- Creates an `IDBConnection` based on pre-defined information (Database provider, connection string, parameter identifier and database command type)
- Execute methods based on mapped objects or manually entered parameters 
- Queries are executed and returning data are converted to object automatically

## A little code
### Setting up your provider information

```csharp
ProviderInfo provider = new ProviderInfo("Data Source=.\SQLEXPRESS;Initial Catalog=tempdb;User ID=sa;Password=adm")
// Defaut parameters: 
// - providerName: System.Data.SqlClient
// - identifier  = @
// - commandType = CommandType.StoredProcedure
// - commandTimeout = 30sec
// - lockTransaction = false
// - isolationLevel = IsolationLevel.ReadCommited (works only when lockTransaction is true)
```

### Creating a data access object

```csharp
using DynamicDAO;
...
using (AutoDAO db = new AutoDAO(provider))
{
    // Your code here
}
```
or
```csharp
using DynamicDAO;
...
// Assumes default settings, like ProviderInfo
using (AutoDAO db = new AutoDAO("Data Source=.\SQLEXPRESS;Initial Catalog=tempdb;User ID=sa;Password=adm"))
{
    // Your code here
}
```

### Adding manual parameters to the data access object

Considers the following procedure:

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

    inputParameters[0] = new object[] { "NAME", "Raphael Crejo" };
    inputParameters[1] = new object[] { "AGE", 37 };
    inputParameters[2] = new object[] { "DOB", new DateTime(1985, 3, 15) };
    
    db.AddInputParameters(inputParameters);
    // Your code here
}
```

### Adding mapped object to data access object

Considers the following class:

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
    Age = 39,
    DOB = new DateTime(1985, 3, 15)
};

using (AutoDAO db = new AutoDAO(provider))
{
    db.AddParameters(person);
    // Your code here
}
```

### `ExecuteScalar` and `ExecuteNonQuery` methods

```csharp
object result = db.ExecuteScalar("INSERT_PERSON"); // considering that your stored procedure returns the Person ID
```
```csharp
int result = db.ExecuteNonQuery("INSERT_PERSON");
// or just
db.ExecuteNonQuery("INSERT_PERSON");
```

### `Query<T>` and `QueryList<T>` methods

Considers the `Person` class above:

```csharp
Person person = db.Query<Person>("SELECT_PERSON");
```

```csharp
List<Person> personList = db.QueryList<Person>("SELECT_PERSONS");
```

### Removing parameters

```csharp
db.ClearParameters(); // remove all parameters from IDBCommand
db.RemoveParameters(new string[] { "PERSON_DOB" }); // remove specific parameter from IDBCommand
```

[//]: #
[N6]: <https://github.com/raphaelcrejo/DynamicDAO>
[Lic]: <https://github.com/raphaelcrejo/DynamicDAOfx/blob/main/LICENSE>
