using API.Data.Entities;
using API.Data.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/nfts")]
    [ApiController]
    // This is a controller class. A controller is a class that handles HTTP requests. The public methods of the controller are called action methods or simply actions.
    public class NFTsController : ControllerBase 
    {
        private INFTManager NFTManager { get; }

        // An instance of NFTManager is injected into the _NFTManager variable by IOC (Dependency Injection). For more info visit: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0#overview-of-dependency-injection
        public NFTsController(INFTManager _NFTManager) 
        {
            NFTManager = _NFTManager;
        }

        // Gets all the NFTs from the database
        // Responds to HTTP GET request made at address <yourdomain.com>/api/nfts
        [HttpGet] 
        public ActionResult<List<NFT>> Get() 
        {
            try
            {
                List<NFT> NFTs = NFTManager.GetNFT(); //Gets a list of all NFTs in the database

                return StatusCode(StatusCodes.Status200OK, NFTs); //If all NFTs are successfully retrieved from the database, the NFTs data is returned in the response body with status code 200(OK).
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); //If something goes wrong while processing the request, status code 500(Internal Server Error) is returned as the response code.
            }
        }

        // Gets an NFT from the database whose ID matches the ID passed in
        // Id variable gets its value from HTTP request URL through Model Binding. For more info on Model Binding visit: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-6.0#example-1
        // GET: api/nfts/5
        [HttpGet("{id}")]        
        public ActionResult<NFT> Get(int Id) 
        {
            try
            {
                NFT _NFT = NFTManager.GetNFT(Id); // Retrieves the first NFT from the database whose ID matches the passed ID and retrieves null if no match is found

                if (_NFT == null)
                    return StatusCode(StatusCodes.Status404NotFound); // If no NFT is found in the database for the passed ID, status code 404(Not Found) is returned as the response code.

                return StatusCode(StatusCodes.Status200OK, _NFT); // If an NFT with a matching ID is found in the database, the matched NFT data is returned in the response body with status code 200(Ok).
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);  // If something goes wrong while processing the request, status code 500(Internal Server Error) is returned as the response code.
            }
        }

        // Gets NFTs from the database whose ID matches any of the ID in the IDs array
        // The ids variable gets its value from HTTP request query string through Model Binding.
        // Get: api/nfts/batch?ids=2,6,9
        [HttpGet("batch")]
        public ActionResult<List<NFT>> Get([FromQuery] string ids) 
        {
            try
            {
                if (string.IsNullOrEmpty(ids))
                    return StatusCode(StatusCodes.Status400BadRequest); //If the ids variable is null or empty, status code 400(Bad Request) is returned as the response code.

                int[] Ids;

                try
                {
                    Ids = Array.ConvertAll(ids.Split(","), int.Parse); //Converts the comma-separated ids string to an array of integers.
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Error: Invalid Ids. Make sure all the ids are of type number"); //If the passed ids cannot be parsed into an array of integers, status code 400(Bad Request) is returned as the response code.
                }

                List<NFT> NFTs = NFTManager.GetNFT(Ids); //Retrieves NFTs from the database whose ID matches any of the ID in the Ids array

                if (NFTs != null)
                    return StatusCode(StatusCodes.Status200OK, NFTs); //If an NFT is found for each ID in the Ids array, the NFT data is returned in the response body with status code 200 (OK).
                else
                    return StatusCode(StatusCodes.Status404NotFound, "Error: No NFT found in the database for one or more IDs"); //If no NFT is found in the database for one or more ID in the Ids array, status code 404(Not Found) is returned as the response code.
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Internal Server Error"); //If something goes wrong while processing the request, status code 500(Internal Server Error) is returned as the response code.
            }
        }

        //Creates new NFTs in the database.
        //The NFTs variable gets its value from HTTP request body string through Model Binding.
        //POST: api/nfts
        [HttpPost]         
        public ActionResult Post([FromBody] NFT[] NFTs) 
        {
            bool NFTsCreateStatus = NFTManager.CreateNFTs(NFTs);

            if (NFTsCreateStatus)
                return StatusCode(StatusCodes.Status201Created, "Success: NFTs Created Successfully"); //If NFTs are successfully created in the database, status code 201 is returned as the response code.
            else
                return StatusCode(StatusCodes.Status400BadRequest, "Error: NFTs Creation Failed: Invalid NFT data"); //If the NFTs cannot be added to the database, status code 400 is returned as the response code.
        }

        // Updates existing NFTs in the database and if passed NFT does not exists creates new one.
        // The NFTs variable gets its value from HTTP request body string through Model Binding.
        //PUT: api/nfts
        [HttpPut]
        public ActionResult Put([FromBody] NFT[] NFTs) 
        {
            bool NFTsUpdateStatus = NFTManager.UpdateNFTs(NFTs); //Updates the existing NFTs in the database else creates new ones.

            if (NFTsUpdateStatus)
                return StatusCode(StatusCodes.Status200OK, "Success: NFTs Updated Successfully"); //If the NFTs are updated successfully, status code 200(Ok) is returned as the response code.
            else
                return StatusCode(StatusCodes.Status400BadRequest, "Error: NFTs Update Failed: Invalid NFTs Data"); //If the NFTs fails to update, status code 400(Bad Request) is returned as the response code.
        }

        // Resets the NFTs in the database
        // The Ids variable gets its value from HTTP request body string through Model Binding.
        // Put: api/nfts/reset
        [HttpPut("reset")]
        public ActionResult Put([FromBody] int[] Ids)
        {
            bool NFTsResetStatus = NFTManager.ResetNFTs(Ids); //Resets the NFTs in the database

            if (NFTsResetStatus)
                return StatusCode(StatusCodes.Status200OK, "Success: NFTs Resetted Successfully"); //If the NFTs gets reset successfully, status code 200(Ok) is returned as the response code.
            else
                return StatusCode(StatusCodes.Status400BadRequest, "Success: NFTs Reset Opertaion Failed"); //If the NFTs fails to reset, status code 400(Bad Request) is returned as the response code.
        }
    }
}
