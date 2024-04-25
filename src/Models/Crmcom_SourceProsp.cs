namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcom_SourceProsp
    {
        [Key]
        public int ID_Source { get; set; }

        [StringLength(50)]
        public string Intitule { get; set; }
    }
}
