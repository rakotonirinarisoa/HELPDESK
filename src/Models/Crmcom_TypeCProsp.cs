namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcom_TypeCProsp
    {
        [Key]
        public int ID_TypeClient { get; set; }

        [StringLength(50)]
        public string Intitule { get; set; }
    }
}
