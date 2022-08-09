
# Unity-Database-Connection

Unity-Database-Connection is a demo project that shows how to retrieve data from a SQL Server database into a Unity game.

# Requirements

+ Unity 5.x or higher
+ .NET Core 3.1
+ SQL Server Database
+ EF Core 5
+ LINQ

# Overview

In this project, we will retrieve NFT data from a SQL Server database into a Unity game.

Since Unity does not provide a direct method to retrieve data from databases, we will use an ASP.NET Core web application which will retrieve data from the database and make it available as a service via the HTTP protocol. Services created using the ASP.NET Core Web API framework conform to the REST architectural model. An important part of this is that the service must respond appropriately based on the HTTP verb that was used to make the request (GET, POST, PUT, DELETE, etc.). Different verbs are used by the client while making the request to express the intent behind the operation. They correspond to CRUD operations as follows:

HTTP Verb  | CRUD Operation
------------- | -------------
POST  | Create
GET  | Read
PUT  | Update
DELETE  | Delete

The following API endpoints are used in this demo.

API Endpoint  | Verb | Description
------------- | -------------  | ------------------------------
/api/nfts  | GET | Get all NFTs
/api/nfts/{id}  | GET | Get single NFT by ID | 
api/nfts/batch  | GET | Get multiple NFTs based on ID array
/api/nfts  | POST | Add a new NFT
/api/nfts  | PUT | Update existing NFTs
/api/nfts/reset  | PUT | Reset existing NFTs

# Quick Start

## Installation

1. Clone this repository
2. Open the ASP.NET Core Web API project
3. Run `Update-Database` command in the package manager console to create the database.
4. Run the application (Dummy data will be added to the database; to prevent this, remove the GenerateDummyData() line from API\Data\Managers\NFTManager.cs)
5. Open the Unity project
6. Open Demo scene.
7. Check the values of the scriptable objects inside folder Assets\SO\NFTs. At this stage, they will have default values.
8. Enter Play mode
9. Click "Load All" Button
10. Recheck the values of the scriptable objects; they should now include the values received from the database.

Video: https://www.youtube.com/watch?v=2UgQHIxluSo

## How to use NFTManager

The NFTManager class handles all interaction between the Unity application and the API. It sends HTTP request to the API and recevies the response back from the server.

#### **Get single NFT from database using NFTManager**

```cs
private IEnumerator GetSingleNFT(int IdToFetch)
{
    NFT FetchedNFT = null; 
    NFTManager _NFTManager = new NFTManager("http://localhost:5000/api/nfts");
    yield return _NFTManager.GetNFT(IdToFetch, NFT => FetchedNFT = NFT);
}
```

#### **Get multiple NFT from database using NFTManager**

```cs
private IEnumerator GetMultipleNFT(int[] IdsToFetch)
{
    NFT[] FetchedNFTs = null;
    NFTManager _NFTManager = new NFTManager("http://localhost:5000/api/nfts");
    yield return _NFTManager.GetNFT(IdsToFetch, NFTs => FetchedNFTs = NFTs);
}
```

#### **Get all NFT from database using NFTManager**

```cs
private IEnumerator GetAllNFT()
{
    NFT[] FetchedNFTs = null;
    NFTManager _NFTManager = new NFTManager("http://localhost:5000/api/nfts");
    yield return _NFTManager.GetNFT(null, NFTs => FetchedNFTs = NFTs);
}
```

#### **Create multiple new NFT in the database**

```cs
private IEnumerator CreateNFTs(NFT[] NFTsToCreate)
{
    bool NFTsCreatedStatus = false;
    yield return _NFTManager.CreateNFTs(NFTsToCreate, Status => NFTsCreatedStatus = Status);
}
```

#### **Updates existing NFTs in the database**

```cs
private IEnumerator UpdateNFTs(NFT[] UpdatedNFTs)
{
    bool NFTsUpdateStatus = false;
    yield return _NFTManager.UpdateNFTs(UpdatedNFTs, Status => NFTsUpdateStatus = Status);
}
```

#### **Resets mutiple NFT in the database**

```cs
private IEnumerator ResetNFTs(int[] IdsToReset)
{
    bool NFTsResetStatus = false;
    yield return _NFTManager.ResetNFTs(IdsToReset, Status => NFTsResetStatus = Status);
}
```

# Overview of ASP.NET Core Web Application

The ASP.NET Core web application will act as a mediator between the Unity application and the database. It will fetch data from the database through Entity Framework Core and then make it available as a service via HTTP protocol.

## What is Entity Framework Core or EF Core?

Entity Framework Core is a lightweight, extensible, open-sourced data-access technology built for .NET Core Applications. EFCore is an ORM( Object Relational Mapper) that enables developers to access & store data much more efficiently without compromising on the performance. With this technology, you no longer will interact directly with the database, like you used to do with traditional SQL queries and Stored Procedures.

These are a few of the features of Entity Framework Core
1. Manage database connection
2. Querying data
3. Saving data

## How to connect to database through EF core?

1. #### **Install EF Core**:

First, let's install the necessary packages for using EF Core in our application after building the ASP.NET Core Web API Project. On your package manager console, type the following commands:

    Install-Package Microsoft.EntityFrameworkCore -Version 5.0.17
    Install-Package Microsoft.EntityFrameworkCore.Tools -Version 5.0.17
    Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 5.0.17
    Install-Package Microsoft.EntityFrameworkCore.Design -Version 5.0.17

2. #### **Add a Model class**

Add a new file named NFT.cs and add the following code to it:

```cs
public class NFT
{
    public int Id { get; set; }
    public int CHP { get; set; }
    public int ATK { get; set; }
    public float ATK_XP { get; set; }
    public int DEF { get; set; }
    public float DEF_XP { get; set; }
    public int SPD { get; set; }
    public float SPD_XP { get; set; }
    public int ATK_SPD { get; set; }
    public float ATK_SPD_XP { get; set; }
    public int ATK_SPD_C { get; set; }
    public float ATK_SPD_C_XP { get; set; }
    public int DEF_SPD_D { get; set; }
    public float DEF_SPD_D_XP { get; set; }
    public int ATK_SPD_S { get; set; }
    public float ATK_SPD_S_XP { get; set; }
    public string Type { get; set; }
    public int Skill { get; set; }
    public string SpriteSheetLink { get; set; }
}
```

3. #### **Add a DBContext Class**

Add a new file named NFTContext.cs and add the following code to it:

```cs
public class NFTContext : DbContext
{
    public DbSet<NFT> NFTs { get; set; }

    public NFTContext(DbContextOptions<NFTContext> options)
        : base(options)
    {
    }
}
```

Intuitively, a DbContext corresponds to your database(or a collection of tables in your database) whereas a DbSet corresponds to a table in your database. Basically, the DbContext class, will be a middleware component for the communication with the database

You will be using a DbContext object to get access to the tables in the database and you will be using your DbSet's to get access, create, update, delete and modify your table data.

4. #### **Add the Connection String**

Let’s add the connection string pointing to the database we need to connect to. It can either be an existing database or a completely new one. I'm going to create a new database. Open up appsettings.json and add this piece of line to the top.

```json
"ConnectionStrings": {    
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=nft;Trusted_Connection=True;"
}
```

5. #### **Register the DbContext class**

In ASP.NET Core, services such as the DBcontext must be registered with the dependency injection(DI) container.  

Go to your Startup class and add the following code to the ConfigureServices method:

```cs
services.AddDbContext<NFTContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
```

Above code registers a DbContext subclass called NFTContext as a scoped service in the ASP.NET Core application service provider (a.k.a. the dependency injection container). The context is configured to use the SQL Server database provider and will read the connection string from ASP.NET Core configuration. It typically does not matter where in ConfigureServices the call to AddDbContext is made. 

NFTContext can now be used in ASP.NET Core controllers or other services through constructor injection.

6. #### **Create the database**

At this point, there is no database for our application which can store the data for our Model classes. Open the Package Manager Console from the menu Tools -> NuGet Package Manager -> Package Manager Console in Visual Studio and execute the following commands:

    1. Add-Migration Initial
    2. Update-Database

1. `Add-Migration` will add a new Migrations folder to your Project. This contains the schema data of your models. EFCore uses this data to generate the Tables.

2. `Update-Database` command will create the database based on the DbContext and Model classes and the migration snapshot, which is created using the `Add-Migration` command

We have now successfully installed and configured EF Core in our web application. We can now use a DbContext instance or object to access the database.

# How to access database data from ASP.NET Core Web Applications?

Now that we have configured the EF Core and registered our NFTContext with the dependency injection(DI) container, we can request an instance of our DbContext type in any service or class that needs it, and start working with NFT stored in the database using LINQ as if they were simply in a collection. EF Core does the work of translating your LINQ expressions into SQL queries to store and retrieve your data. 

The code below demonstrates how to fetch and save to a database:

```cs
public class NFTManager
{
    public NFTContext _NFTContext { get; }

    //An instance of NFTContext is injected into the _NFTContext variable by IOC (Dependency Injection).
    public NFTManager(NFTContext nFTContext)
    {
        _NFTContext = nFTContext;
    }

    //Retrieves an NFT from the database based on Id
    public List<NFT> GetNFT(int Id)
    {
        return _NFTContext.NFTs.Where(e => e.Id == Id).ToList(); 
    }

    //Creates a new NFT in the database
    public void CreateNFT(NFT NFT)
    {
        _NFTContext.Add(NFT);
        _NFTContext.SaveChanges();
    }
}
```

# How to make database data accessible via API

Now that our web application can retrieve data from the database, we need to make this data accessible through the Web API. Making this data available as a service through the HTTP protocol is the responsibility of the Web API Controller class.

#### **What is Web API Controller?**

Web API controller is a class which derives from Microsoft.AspNetCore.Mvc.ControllerBase class. All the public methods of the controller class are called action methods. Web API Controller handles incoming HTTP requests and send response back to the caller. Based on the incoming request URL and HTTP verb (GET/POST/PUT/PATCH/DELETE), Web API decides which Web API controller and action method to execute.

The following code create an API endpoint which returns the all the NFTs in the database.

```cs
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using API.Data.Entities;
using API.Data.Contexts;

namespace API.Controllers
{
    [Route("api/nfts")]
    [ApiController]
    public class NFTsController : ControllerBase
    {
        public NFTContext _NFTContext { get; }

        public NFTsController(NFTContext nFTContext)
        {
            _NFTContext = nFTContext;
        }

        [HttpGet]
        public ActionResult<List<NFT>> GetNFT()
        {
            return _NFTContext.NFTs.ToList();
        }

        [HttpPost]
        public void CreateNFT([FromBody] NFT _NFT)
        {
            _NFTContext.Add(_NFT);
            _NFTContext.SaveChanges();
        }
    }
}
```

As you can see in the above code, we have one GetNFT method which is decorated with the HttpGet attribute which means that when we send a HTTP GET request to the URL `http://localhost:5000/api/nft` using any client (Unity or Browser), this is the method which ASP.NET core web app will invoke and it will return a list of all the NFT present in the database.

Similarly, when you send a POST request to the URL `http://localhost:5000/api/nft`, ASP.NET core web app will invoke the CreateNFT method and new NFT will be created in the database.

# How to send HTTP requests via Unity?

Now that our API is operational, let's see how we can send different types of requests from our Unity application to various API endpoints to retrieve and save data in the database.

In Unity, all interactions with the web server happen through the UnityWebRequest object. You must use coroutine with UnityWebRequest because with coroutine you can yield the request until it is completed. This will not block the Main Thread or prevent other scripts from running.

For any given HTTP transaction, the generic code flow is:

+ Create a Web Request object
+ Configure the Web Request object
  + Set custom headers
    + Set HTTP verb (GET, POST, HEAD, etc.)
    + Custom verbs are permitted
  + Set URL
+ (Optional) Create an Upload Handler & attach it to the Web Request (An `UploadHandler` object handles transmission of data to the server)
  + Provide data to be uploaded
  + Provide HTTP form to be uploaded
+ (Optional) Create a Download Handler & attach it to the Web Request (A `DownloadHandler` object handles receipt, buffering and postprocessing of data received from the server)
+ Send the Web Request
  + If inside a coroutine, you may Yield the result of the Send() call to wait for the request to complete
+ (Optional) Read received data from the Download Handler
+ (Optional) Read error information, HTTP status code and response headers from the UnityWebRequest object

#### **How to send HTTP GET request to fetch data from a server?**

To retrieve data from a web server, we use UnityWebRequest.Get method. This method takes a single string as an argument; The string specifies the URL from which data will be retrieved.

```cs
using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Networking;
 
class MyBehaviour: public MonoBehaviour 
{
    void Start() 
    {
        StartCoroutine(GetText());
    }
 
    IEnumerator GetText() 
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/api/nft");
        
        yield return www.Send();
 
        if(www.isError) 
        {
            Debug.Log(www.error);
        }
        else 
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
 
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
```

Above code, creates a UnityWebRequest object and sets its target URL to the string argument.

By default, this method attaches a standard DownloadHandlerBuffer to the UnityWebRequest. This handler will buffer the data received from the server and make it available to your scripts when the request is complete.

By default, this method attaches no UploadHandler to the UnityWebRequest. You may attach one manually if you wish.

#### **How to send HTTP POST request to post JSON data to a server?**

```cs
 void Start()
 {
     StartCoroutine(Post());
 }

 IEnumerator Post()
 {
     var www = new UnityWebRequest("http://localhost:5000/api/nft", "POST");
     
     byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes("Serialized Json Data");
     www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
     www.SetRequestHeader("Content-Type", "application/json");

     yield return www.SendWebRequest();

     if (www.isError)
         Debug.Log("Error: " + www.error);
     else
         Debug.Log("Upload complete!");
 }
```

Above code, creates a UnityWebRequest object and sets its target URL to `http://localhost:5000/api/nft` and its method to "POST"

This method attaches a standard DownloadHandlerBuffer to the UnityWebRequest. 

This method stores the input upload data in a standard UploadHandlerRaw object and attaches it to the UnityWebRequest.
'+
