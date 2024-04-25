namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ThistoSendMail")]
    public partial class ThistoSendMail
    {
        public int id { get; set; }

        public int? User_id { get; set; }

        [StringLength(300)]
        public string UserName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateRapport { get; set; }

        public DateTime? DateEnvoi { get; set; }

        public string Oject { get; set; }

        public int? NbjrRetard { get; set; }

        public int? Penalite { get; set; }
    }
}
