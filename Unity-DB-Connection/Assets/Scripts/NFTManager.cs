using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System;

public class NFTManager
{
    // Base URL of the web API
    private string BaseURL;
    
    public NFTManager(string _BaseURL)
    {
        BaseURL = _BaseURL;
    }

    /// <summary>
    /// Retrieves an NFT from the database whose ID matches the passed ID varibale
    /// </summary>
    /// <param name="ID">ID of the NFT you want to retrieve from the database</param>
    /// <param name="CallbackMethod">Function you want to get called when web request completes. Retrieved NFT will be passed as the function parameter</param>
    public IEnumerator GetNFT(int ID, Action<NFT> CallbackMethod)
    {
        //Variable in which the serialized NFT data retrieved from the database will be stored
        string SerializedNFT = null;

        //Wait for the web request to the database to complete. The retrieved serialzed NFT data will be assigned to the SerializedNFT variable.
        yield return FetchNFT(Path.Combine(BaseURL, ID.ToString()), serializedNFT => { SerializedNFT = serializedNFT; });

        //If web request results in a error i.e. no data is retrieved...
        if (string.IsNullOrEmpty(SerializedNFT))
        {
            //If callback method is not null, invoke the callback method and pass null in the function parameter.
            CallbackMethod?.Invoke(null);
            
            //exit the coroutine
            yield break; 
        }
        //If web request is successful and serialized NFT data is retreived...
        else
        {
            //Parse the serialized NFT data into NFT object
            NFT FetchedNFT = JsonConvert.DeserializeObject<NFT>(SerializedNFT);

            //If callback method is not null, invoke the callback method and pass the retrieved NFT as the function parameter
            CallbackMethod?.Invoke(FetchedNFT);
        }
    }

    /// <summary>
    /// Retrieves multiple NFT from the database whose Id matches any of the ID in the IDs array. Retrieves all NFTs from the database if IDs is null.
    /// </summary>
    /// <param name="IDs">Array containing ID of the NFTs to be retrieved</param>
    /// <param name="CallbackMethod">Function you want to get called when the web request completes. Retrieved NFTs will be passed in the function parameter</param>
    public IEnumerator GetNFT(int[] IDs, Action<NFT[]> CallbackMethod)
    {
        //Variable in which the serialized NFT data retrieved from the database will be stored.
        string SerializedNFT = null;
        
        //If IDs array is null, send web request to the BaseURL and retrieve all the NFTs.
        //If IDs array is not null, send web request to different URL and retrieve just those NFTs whose ID matches the ID in the IDs array.
        string URL = IDs == null ? BaseURL : $"{BaseURL}/batch?ids={string.Join(",", IDs)}";

        //Wait for the web request to the database to complete. The retrieved serialzed NFT data will be assigned to the SerializedNFT variable.
        yield return FetchNFT(URL, serializedNFT => { SerializedNFT = serializedNFT; });

        //If web request results in a error i.e. no serialized NFT data is retrieved...
        if (string.IsNullOrEmpty(SerializedNFT))
        {
            //If callback method is not null, invoke the callback method and pass null in the function parameter.
            CallbackMethod?.Invoke(null);

            //exit the coroutine.
            yield break;
        }
        //If web request is successful and serialized NFT data is retreived...
        else
        {
            //Parse the serialized NFT data into NFT object
            NFT[] FetchedNFTs = JsonConvert.DeserializeObject<NFT[]>(SerializedNFT);

            //If callback method is not null, invoke the callback method and pass the retrieved NFT as the function parameter
            CallbackMethod?.Invoke(FetchedNFTs);
        }
    }

    /// <summary>
    /// Sends the HTTP get request to the server and receives the data in HTTP web response.
    /// </summary>
    /// <param name="URL">The URL on which you want to send the get request</param>
    /// <param name="CallbackMethod">Function which will be called when the web request completes. Serialized NFT data received in the web response will be passed as the function parameter</param>
    private IEnumerator FetchNFT(string URL, Action<string> CallbackMethod)
    {
        //UnityWebRequest object to send HTTP GET request to the server.
        UnityWebRequest WebRequest = UnityWebRequest.Get(URL);

        //Send the HTTP GET request and wait for response
        yield return WebRequest.SendWebRequest();

        //If web request results in a success...
        if (WebRequest.result == UnityWebRequest.Result.Success)
        {
            //If callback method is not null, invoke the callback method and pass the web response message in the function parameter.
            CallbackMethod?.Invoke(WebRequest.downloadHandler.text);

            //Log success message in the Unity editor console 
            Debug.Log($"Response Code({WebRequest.responseCode}): NFT Data Fetched Successfully");
        }
        else
        {
            //If callback method is not null, invoke the callback method and pass null in the function parameter.
            CallbackMethod?.Invoke(null);

            //Log error message in the unity editor console
            Debug.LogError($"Response Code({WebRequest.responseCode}): Failed to Fetch NFT Data with error \"{WebRequest.error}\"");
        }
    }

    /// <summary>
    /// Creates or adds a new NFT in the database
    /// </summary>
    /// <param name="NFTs">Array of NFT to be added in the database</param>
    /// <param name="CallbackMethod">Function you want to get called when the web request completes</param>
    public IEnumerator CreateNFTs(NFT[] NFTs, Action<bool> CallbackMethod = null)
    {
        //UnityWebRequest object to send HTTP POST request to the server.
        UnityWebRequest WebRequest = new UnityWebRequest(BaseURL, "POST");

        //Serialzing the NFTs object in order to send them to the server.
        byte[] SerializedNFTs = new System.Text.UTF8Encoding().GetBytes(JsonConvert.SerializeObject(NFTs));

        //Handler for the web request
        WebRequest.uploadHandler = new UploadHandlerRaw(SerializedNFTs);
        WebRequest.downloadHandler = new DownloadHandlerBuffer();
        
        //Setting the request header.
        WebRequest.SetRequestHeader("Content-Type", "application/json");

        //Sends the request to the server and wait for response.
        yield return WebRequest.SendWebRequest();

        //If web request results in a success i.e. NFTs are added to the database
        if (WebRequest.result == UnityWebRequest.Result.Success)
        {
            //If callback method is not null, invoke the callback method and pass true in the function parameter.
            CallbackMethod?.Invoke(true);

            //Log success msg in the Unity editor console
            Debug.Log($"Response Code({WebRequest.responseCode}): New NFTs Created Successfully");
        }
        //If web request results in an error i.e. NFTs are not added to the database.
        else
        {
            //If callback method is not null, invoke the callback method and pass false in the function parameter.
            CallbackMethod?.Invoke(false);
            
            //Log error in the Unity editor console.
            Debug.LogError($"Response Code({WebRequest.responseCode}): Failed to create new NFTs with error \"{WebRequest.error}\"");
        }
    }

    /// <summary>
    /// Updates existing NFTs in the database else creates new NFTs.
    /// </summary>
    /// <param name="NFTs">Array of NFTs with updated values</param>
    /// <param name="CallbackMethod">Function you want to get called when the web request completes</param>
    public IEnumerator UpdateNFTs(NFT[] NFTs, Action<bool> CallbackMethod = null)
    {
        //UnityWebRequest object to send HTTP PUT request to the server.
        UnityWebRequest WebRequest = new UnityWebRequest(BaseURL, "PUT");

        //Serialzing the NFTs object in order to send them to the server.
        byte[] SerializedNFTs = new System.Text.UTF8Encoding().GetBytes(JsonConvert.SerializeObject(NFTs));

        //Handler for the web request
        WebRequest.uploadHandler = new UploadHandlerRaw(SerializedNFTs);
        WebRequest.downloadHandler = new DownloadHandlerBuffer();

        //Setting the request header.
        WebRequest.SetRequestHeader("Content-Type", "application/json");

        //Sends the request to the server and waits for response.
        yield return WebRequest.SendWebRequest();

        //If web request results in a success i.e. NFTs got updated in the database
        if (WebRequest.result == UnityWebRequest.Result.Success)
        {
            //If callback method is not null, invoke the callback method and pass true in the function parameter.
            CallbackMethod?.Invoke(true);

            //Log success msg in the Unity editor console
            Debug.Log($"Response Code({WebRequest.responseCode}): NFTs Updated Successfully");
        }
        else
        {
            //If callback method is not null, invoke the callback method and pass false in the function parameter.
            CallbackMethod?.Invoke(false);

            //Log error in the Unity editor console.
            Debug.LogError($"Response Code({WebRequest.responseCode}): Failed to update NFTs with error \"{WebRequest.error}\"");
        }
    }

    /// <summary>
    /// Resets existing NFTs in the database
    /// </summary>
    /// <param name="Ids">IDs of the NFTs to reset</param>
    /// <param name="CallbackMethod">Function you want to get called when the web request completes</param>
    public IEnumerator ResetNFTs(int[] Ids, Action<bool> CallbackMethod = null)
    {
        //UnityWebRequest object to send HTTP PUT request to the server.
        UnityWebRequest WebRequest = new UnityWebRequest($"{BaseURL}/reset", "PUT");

        //Serialzing the IDs in order to send them to the server.
        byte[] SerializedIds = new System.Text.UTF8Encoding().GetBytes(JsonConvert.SerializeObject(Ids));

        //Handler for the web request
        WebRequest.uploadHandler = new UploadHandlerRaw(SerializedIds);
        WebRequest.downloadHandler = new DownloadHandlerBuffer();

        //Setting the request header
        WebRequest.SetRequestHeader("Content-Type", "application/json");

        //Sends the request to the server and waits for response.
        yield return WebRequest.SendWebRequest();

        //If web request results in a success i.e. NFTs got reset in the database
        if (WebRequest.result == UnityWebRequest.Result.Success)
        {
            //If callback method is not null, invoke the callback method and pass true in the function parameter.
            CallbackMethod?.Invoke(true);

            //Log success msg in the Unity editor console
            Debug.Log($"Response Code({WebRequest.responseCode}): NFTs Resetted Successfully");
        }
        else
        {
            //If callback method is not null, invoke the callback method and pass false in the function parameter.
            CallbackMethod?.Invoke(false);

            //Log error in the Unity editor console.
            Debug.LogError($"Response Code({WebRequest.responseCode}): Failed to reset NFTs with error \"{WebRequest.error}\"");
        }
    }
}
