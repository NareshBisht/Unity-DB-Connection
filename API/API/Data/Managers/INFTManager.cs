using API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Managers
{
    public interface INFTManager
    {
        /// <summary>
        /// Retrieves all the NFTs from the database
        /// </summary>
        /// <returns>A List of all the NFTs in the database</returns>
        List<NFT> GetNFT();

        /// <summary>
        /// Retrieves an NFT from the database whose ID matches the ID passed in
        /// </summary>
        /// <param name="Id">ID of the NFT you want to retrieve</param>
        /// <returns>An NFT whose Id matches with the ID passed in</returns>
        NFT GetNFT(int Id);

        /// <summary>
        /// Retrieves NFTs from the database whose ID matches any of the ID passed in the IDs array
        /// </summary>
        /// <param name="Ids">Array containing IDs of the NFTs to be retrieved</param>
        /// <returns>A List of NFTs whose ID matches with any of the ID in the IDs array</returns>
        List<NFT> GetNFT(int[] Ids);

        /// <summary>
        /// Adds new NFTs in the database
        /// </summary>
        /// <param name="NFTs">Array of NFTs to be added in the database</param>
        /// <returns>A Boolean indicating whether the NFTs were added to the database or not</returns>
        bool CreateNFTs(NFT[] NFTs);

        /// <summary>
        /// Updates existing NFTs in the database and if passed NFT does not exists creates new one.
        /// </summary>
        /// <param name="NFTs">Array of updated NFTs</param>
        /// <returns>A Boolean indicating whether the NFTs were updated in the database or not</returns>
        bool UpdateNFTs(NFT[] NFTs);

        /// <summary>
        /// Resets existing NFTs in the database
        /// </summary>
        /// <param name="Ids">IDs of the NFTs to be reset</param>
        /// <returns>A Boolean indicating whether the NFTs were reset in the database or not</returns>
        bool ResetNFTs(int[] Ids);
    }
}
