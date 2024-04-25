namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcom_RebutProsp
    {
        [Key]
        public int ID_Rebut { get; set; }

        [StringLength(50)]
        public string Intitule { get; set; }
    }
}
