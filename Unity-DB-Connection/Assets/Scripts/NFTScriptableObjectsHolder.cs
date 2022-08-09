using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Scriptable object containing references to all NFT scriptable objects in your game
[CreateAssetMenu(menuName = "Scriptable Objects/NFTs Holder")]
public class NFTScriptableObjectsHolder : ScriptableObject
{
    //List of all the NFT Scriptable Objects in your game
    public List<NFTScriptableObject> NFTScriptableObjects; 
}
