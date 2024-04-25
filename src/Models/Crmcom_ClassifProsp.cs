namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcom_ClassifProsp
    {
        [Key]
        public int ID_Classification { get; set; }

        [StringLength(50)]
        public string Intitule { get; set; }
    }
}
