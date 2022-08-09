using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    //URL of the API
    [SerializeField] private string BaseURL = "http://localhost:5000/api/nfts";

    //Reference to the scriptable object containing the reference to all NFT scriptable objects
    [SerializeField] private NFTScriptableObjectsHolder NFTScriptableObjectsHolder;

    [SerializeField] private int IdToFetch = 5; //ID of the NFT you want to retrieve from the database.
    [SerializeField] private int[] IdsToFetch = new [] { 2, 6, 9 }; //IDs of the NFTs you want to retrieve from the database in a single HTTP request.
    [SerializeField] private int[] IdsToReset = new[] { 3, 5, 7 }; //IDs of the NFTs you want to reset in the database

    [SerializeField] private Button LoadSingleNFTButton; //Button to retrieve single NFT from database based on ID property
    [SerializeField] private Button LoadMultipleNFTButton; //Button to retrieve multiple NFT from the database based on a list of IDs
    [SerializeField] private Button LoadAllNFTButton; //Button to retrieve all the NFT from the database
    [SerializeField] private Button CreateNFTsButton; //Button to create one or more new NFT in the database
    [SerializeField] private Button UpdateNFTsButton; //Button to update one or more existing NFT in the database
    [SerializeField] private Button ResetNFTsButton; //Button to reset one or more existing NFTs in the database

    private NFTManager _NFTManager;
    private List<NFTScriptableObject> NFTScriptableObjects; //List of all the NFT scriptable object in the game

    private void Start()
    {
        //Creating an NFTManager object and assigning it to _NFTManager variable.
        _NFTManager = new NFTManager(BaseURL);

        //Getting a list of all the NFT scriptable object in the game
        NFTScriptableObjects = NFTScriptableObjectsHolder.NFTScriptableObjects;

        //Retrieving an NFT from the database when "LoadSingleNFTButton" button is clicked
        LoadSingleNFTButton.onClick.AddListener(() => StartCoroutine(GetSingleNFT()));

        //Retrieving multiple NFTs from the database when "LoadMultipleNFTButton" button is clicked.
        LoadMultipleNFTButton.onClick.AddListener(() => StartCoroutine(GetMultipleNFTs()));
        
        //Retrieving all the NFTs from the database when "LoadAllNFTsButton" button is pressed
        LoadAllNFTButton.onClick.AddListener(() => StartCoroutine(GetAllNFTs()));

        //Creating one or more new NFT in the database when "CreateNFTsButton" is clicked
        CreateNFTsButton.onClick.AddListener(() => StartCoroutine(CreateNFTs()));

        //Updating one ore more already existing NFTs in the database when "UpdateNFTsButton" is clicked
        UpdateNFTsButton.onClick.AddListener(() => StartCoroutine(UpdateNFTs()));

        //Resetting one or more already existing NFTs in the database when "ResetNFTsButton" is clicked
        ResetNFTsButton.onClick.AddListener(() => StartCoroutine(ResetNFTs()));
    }

    /// <summary>
    /// Retrieves single NFT from the database
    /// </summary>
    private IEnumerator GetSingleNFT()
    {
        //Variable in which the NFT retrieved from the database will be stored
        NFT FetchedNFT = null;

        //Wait for the web request to the database to complete. The retrieved NFT will be assigned to the FetchedNFT variable.
        yield return _NFTManager.GetNFT(IdToFetch, NFT => FetchedNFT = NFT);

        //If NFT is retrieved successfully from the database...
        if (FetchedNFT != null)
            NFTScriptableObjects.Where(e => e._NFT.Id == FetchedNFT.Id).FirstOrDefault()?.Save(FetchedNFT); //Save the NFT to the scriptable object whose ID macthes the ID of NFT
        else
            Debug.Log("NFT could not be retrived from the database"); 
    }

    /// <summary>
    /// Retrieves multiple NFTs from the database
    /// </summary>
    private IEnumerator GetMultipleNFTs()
    {
        //Varibale in which the NFTs retrieved from the database will be stored
        NFT[] FetchedNFTs = null;

        //Wait for the web request to the database to complete. The retrieved NFT array will be assigned to the FetchedNFTs variable.
        yield return _NFTManager.GetNFT(IdsToFetch, NFTs => FetchedNFTs = NFTs);

        //If NFTs are retrieved successfully...
        if (FetchedNFTs != null)
        {
            foreach (NFT FetchedNFT in FetchedNFTs)
            {
                NFTScriptableObjects.Where(e => e._NFT.Id == FetchedNFT.Id).FirstOrDefault()?.Save(FetchedNFT); //Save the NFTs in their respective scriptable object
            }
        }
        else
            Debug.Log("NFTs could not be retrived from the database");
    }

    /// <summary>
    /// Retrieves all the NFTs from the database
    /// </summary>
    private IEnumerator GetAllNFTs()
    {
        //Variable in which All the NFTs retrieved from the database will be stored.
        NFT[] FetchedNFTs = null;

        //Wait for the web request to the database to complete. The retrieved NFT array will be assigned to the FetchedNFTs variable.
        yield return _NFTManager.GetNFT(null, NFTs => FetchedNFTs = NFTs);

        //If NFTs are retieved successfully...
        if (FetchedNFTs != null)
        {
            foreach (NFT FetchedNFT in FetchedNFTs)
            {
                NFTScriptableObjects.Where(e => e._NFT.Id == FetchedNFT.Id).FirstOrDefault()?.Save(FetchedNFT); //Save the NFTs in their respective scriptable object
            }
        }
    }

    /// <summary>
    /// Create new NFTs in the database
    /// </summary>
    private IEnumerator CreateNFTs()
    {
        //NFTsCreateStatus variable will become true If NFTs were created/added successfully in the database else it will become false.
        bool NFTsCreatedStatus = false;

        //Dummy NFTs which will be created in the database.
        NFT[] DummyNFTs = GenerateDummyNFT(5);

        //Wait for the web request to the database to complete. If retrieved successfully set NFTsCreatedStatus to true else set it to false.
        yield return _NFTManager.CreateNFTs(DummyNFTs, Status => NFTsCreatedStatus = Status);

        //If NFTs are created successfully...
        if (NFTsCreatedStatus)
            Debug.Log("NFTs created successfully"); 
        else
            Debug.Log("NFTs could not be created");

    }

    /// <summary>
    /// Updates existing NFTs in the database else creates new NFTs.
    /// </summary>
    private IEnumerator UpdateNFTs()
    {
        //NFTsUpdateStatus variable will become true if NFTs get updated successfully in the DB else it will become false.
        bool NFTsUpdateStatus = false;
        
        //Generating dummy NFTs
        NFT[] UpdatedNFTs = GenerateDummyNFT(2);               
        UpdatedNFTs[0].Id = 3;
        UpdatedNFTs[1].Id = 8;

        //Wait for the web request to the database to complete. If NFT values were updated successfully in the DB, NFTsUpdateStatus variable will become true else it will become false.
        yield return _NFTManager.UpdateNFTs(UpdatedNFTs, Status => NFTsUpdateStatus = Status);

        //If NFTs get updated successfully...
        if (NFTsUpdateStatus)
            Debug.Log("NFTs updated successfully");
        else
            Debug.Log("NFTs could not be updated");

    }

    /// <summary>
    /// Resets the NFTs in the database
    /// </summary>
    private IEnumerator ResetNFTs()
    {
        //NFTsResetStatus variable will become true if NFTs get resetted in the database else it will become false.
        bool NFTsResetStatus = false;

        //Wait for the web request to the database to complete. If NFT values get reset successfully in the database, NFTsResetStatus will become true else it will become false.
        yield return _NFTManager.ResetNFTs(IdsToReset, Status => NFTsResetStatus = Status);

        //If NFTs get reset in the database...
        if (NFTsResetStatus)
            Debug.Log("NFTs resetted successfully");
        else
            Debug.Log("NFTs could not be reset");

    }

    /// <summary>
    /// Generates dummy NFTs for testing purpose
    /// </summary>
    /// <param name="Count">Number of NFT you want to generate</param>
    /// <returns>Array of dummy NFTs data</returns>
    private NFT[] GenerateDummyNFT(int Count)
    {
        NFT[] NFTs = new NFT[Count];
        
        System.Random Random = new System.Random();

        for (int i = 0; i < Count; i++)
        {
            NFT _NFT = new NFT();

            _NFT.Id = i;
            _NFT.CHP = 1000;
            _NFT.ATK = Random.Next(0, 100);
            _NFT.ATK_XP = (float)Random.NextDouble();
            _NFT.DEF = Random.Next(0, 100);
            _NFT.DEF_XP = (float)Random.NextDouble();
            _NFT.SPD = Random.Next(0, 100);
            _NFT.SPD_XP = (float)Random.NextDouble();
            _NFT.ATK_SPD = Random.Next(0, 100);
            _NFT.ATK_SPD_XP = (float)Random.NextDouble();
            _NFT.ATK_SPD_C = Random.Next(0, 100);
            _NFT.ATK_SPD_C_XP = (float)Random.NextDouble();
            _NFT.DEF_SPD_D = Random.Next(0, 100);
            _NFT.DEF_SPD_D_XP = (float)Random.NextDouble();
            _NFT.ATK_SPD_S = Random.Next(0, 100);
            _NFT.ATK_SPD_S_XP = (float)Random.NextDouble();
            _NFT.Type = Random.Next(0, 10).ToString();
            _NFT.Skill = Random.Next(0, 10);
            _NFT.SpriteSheetLink = "https://www.google.com/" + i;

            NFTs[i] = _NFT;
        }

        return NFTs;
    }
}