using API.Data.Contexts;
using API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Managers
{
    public class NFTManager : INFTManager
    {
        public NFTContext NFTContext { get; }

        //An instance of NFTContext is injected into the _NFTContext variable by IOC (Dependency Injection). For more info on Dependency Injection visit: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0#overview-of-dependency-injection
        public NFTManager(NFTContext _NFTContext)
        {
            NFTContext = _NFTContext;
            
            GenerateDummyData(); //Adds 100 dummy NFTs to the database for testing purpose.
        }

        /// <summary>
        /// Generates 100 dummy NFTs in the database for testing purposes.
        /// </summary>
        private void GenerateDummyData()
        {
            if (NFTContext.NFTs.Count() > 0)
                return;

            Random _Random = new Random();

            for (int i = 0; i < 100; i++)
            {
                NFT _NFT = new NFT();

                _NFT.Id = i;
                _NFT.CHP = _Random.Next(0, 100);
                _NFT.ATK = _Random.Next(0, 100);
                _NFT.ATK_XP = (float)_Random.NextDouble();
                _NFT.DEF = _Random.Next(0, 100);
                _NFT.DEF_XP = (float)_Random.NextDouble();
                _NFT.SPD = _Random.Next(0, 100);
                _NFT.SPD_XP = (float)_Random.NextDouble();
                _NFT.ATK_SPD = _Random.Next(0, 100);
                _NFT.ATK_SPD_XP = (float)_Random.NextDouble();
                _NFT.ATK_SPD_C = _Random.Next(0, 100);
                _NFT.ATK_SPD_C_XP = (float)_Random.NextDouble();
                _NFT.DEF_SPD_D = _Random.Next(0, 100);
                _NFT.DEF_SPD_D_XP = (float)_Random.NextDouble();
                _NFT.ATK_SPD_S = _Random.Next(0, 100);
                _NFT.ATK_SPD_S_XP = (float)_Random.NextDouble();
                _NFT.Type = _Random.Next(0, 10).ToString();
                _NFT.Skill = _Random.Next(0, 10);
                _NFT.SpriteSheetLink = "https://www.google.com/" + i;

                NFTContext.Add(_NFT); //Begin Tracking
            }

            NFTContext.SaveChanges(); //Adds NFTs to the database.
        }

        /// <summary>
        /// Copies properties values from destination NFT to source NFT
        /// </summary>
        /// <param name="Source">NFT object to which the properties value will be copied</param>
        /// <param name="Destination">NFT object whose properties value will be copied</param>
        private void CopyProperties(NFT Source, NFT Destination)
        {
            Source.CHP = Destination.CHP;
            Source.ATK = Destination.ATK;
            Source.ATK_XP = Destination.ATK_XP;
            Source.DEF = Destination.DEF;
            Source.DEF_XP = Destination.DEF_XP;
            Source.SPD = Destination.SPD;
            Source.SPD_XP = Destination.SPD_XP;
            Source.ATK_SPD = Destination.ATK_SPD;
            Source.ATK_SPD_XP = Destination.ATK_SPD_XP;
            Source.ATK_SPD_C = Destination.ATK_SPD_C;
            Source.ATK_SPD_C_XP = Destination.ATK_SPD_C_XP;
            Source.DEF_SPD_D = Destination.DEF_SPD_D;
            Source.DEF_SPD_D_XP = Destination.DEF_SPD_D_XP;
            Source.ATK_SPD_S = Destination.ATK_SPD_S;
            Source.ATK_SPD_S_XP = Destination.ATK_SPD_S_XP;
            Source.Type = Destination.Type;
            Source.Skill = Destination.Skill;
            Source.SpriteSheetLink = Destination.SpriteSheetLink;
        }

        /// <summary>
        /// Retrieves all the NFTs from the database
        /// </summary>
        /// <returns>A List of all the NFTs in the database</returns>
        public List<NFT> GetNFT()
        {
            // Returns a list of all the NFTs in the DB. (Note: ToList is an extension method from the System.Linq namespace)
            return NFTContext.NFTs.ToList(); 
        }

        /// <summary>
        /// Retrieves an NFT from the database whose ID matches the ID passed in
        /// </summary>
        /// <param name="Id">ID of the NFT you want to retrieve</param>
        /// <returns>An NFT whose Id matches with the ID passed in</returns>
        public NFT GetNFT(int Id)
        {
            // Returns the first NFT from the database whose ID matches the passed ID and null is returned if no match is found. This is a LINQ statement. For more info on LINQ visit: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/ or https://www.youtube.com/watch?v=yClSNQdVD7g
            return NFTContext.NFTs.Where(NFT => NFT.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves NFTs from the database whose ID matches any of the ID in the IDs array
        /// </summary>
        /// <param name="Ids">Array containing ID of the NFTs to be retrieved</param>
        /// <returns>A List of NFTs whose ID matches with any of the ID in the IDs array</returns>
        public List<NFT> GetNFT(int[] Ids)
        {
            // Returns NFTs from the database whose ID matches any of the ID passed in the IDs array
            return NFTContext.NFTs.Where(NFT => Ids.Contains(NFT.Id)).ToList();              
        }

        /// <summary>
        /// Adds new NFTs in the database
        /// </summary>
        /// <param name="NFTs">Array of NFT to be added in the database</param>
        /// <returns>A Boolean indicating whether the NFTs were added to the database or not</returns>
        public bool CreateNFTs(NFT[] NFTs)
        {
            try
            {
                foreach (NFT _NFT in NFTs)
                {
                    NFTContext.Add(_NFT); //Begin Tracking
                }

                NFTContext.SaveChanges(); //Adds NFTs to the database.

                return true; //Returns true if the NFT is successfully added to the database
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates existing NFTs in the database and if passed NFT does not exists creates new one.
        /// </summary>
        /// <param name="NFTs">Array of updated NFTs</param>
        /// <returns>A Boolean indicating whether the NFTs were updated in the database or not</returns>
        public bool UpdateNFTs(NFT[] NFTs)
        {
            try
            {
                foreach (NFT _NFT in NFTs)
                {
                    NFT DatabaseNFT = GetNFT(_NFT.Id); //Retrieves the NFT from the database that needs to be updated.

                    if (DatabaseNFT != null)
                        CopyProperties(DatabaseNFT, _NFT); //Copy properties value from the updated NFT to the NFT in the database and Begin Tracking.
                    else
                        NFTContext.Add(_NFT);
                }

                NFTContext.SaveChanges(); //Apply all changes to the database.

                return true; //If NFTs gets updated successfully, true is returned
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Resets existing NFTs in the database
        /// </summary>
        /// <param name="Ids">IDs of the NFTs to reset</param>
        /// <returns>A Boolean indicating whether the NFTs were reset in the database or not</returns>
        public bool ResetNFTs(int[] Ids)
        {
            try
            {
                foreach (int Id in Ids)
                {
                    NFT CurrentNFT = GetNFT(Id); //Retrieves the NFT from the database that needs to be reset.

                    if (CurrentNFT != null)
                        CopyProperties(CurrentNFT, new NFT()); //Resets the value of NFT to default values and begin tracking.
                }

                NFTContext.SaveChanges(); //Applying changes to the database.

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}