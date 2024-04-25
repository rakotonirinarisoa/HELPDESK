namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcom_EtatProsp
    {
        [Key]
        public int ID_Etat { get; set; }

        [StringLength(50)]
        public string Intitule { get; set; }
    }
}
