namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcom_RebutReaProsp
    {
        [Key]
        public int ID_Rebut_Reason { get; set; }

        [StringLength(50)]
        public string Intitule { get; set; }
    }
}
