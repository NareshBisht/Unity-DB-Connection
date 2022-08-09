using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data.Entities
{
    public class NFT
    {
        [KeyAttribute()] // Making Id variable the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Turn on Identity-Insert (Lets you insert Id explicitly)
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
}
