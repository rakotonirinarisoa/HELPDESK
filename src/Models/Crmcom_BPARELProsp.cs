namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcom_BPARELProsp
    {
        [Key]
        public int ID_DateRelBPA { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int? ID_Prosp { get; set; }

        [StringLength(2)]
        public string P1 { get; set; }
    }
}
