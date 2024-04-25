namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcli_Reg
    {
        public int ID { get; set; }

        public int? Comp_CompanyId { get; set; }

        [Required]
        [StringLength(20)]
        public string UserPassword { get; set; }

        public virtual Company Company { get; set; }
    }
}
