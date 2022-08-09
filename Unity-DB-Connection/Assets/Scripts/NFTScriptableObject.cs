using UnityEngine;

//Scriptable object representing an NFT
[CreateAssetMenu(menuName = "Scriptable Objects/NFT")]
public class NFTScriptableObject : ScriptableObject
{
    public NFT _NFT;

    /// <summary>
    /// Saves the passed NFT properties value to the scriptable object
    /// </summary>
    /// <param name="nft">NFT whose properties value will be copied to the scriptable object</param>
    public void Save(NFT nft)
    {
        _NFT = nft;
    }
}
