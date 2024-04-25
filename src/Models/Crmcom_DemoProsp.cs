namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcom_DemoProsp
    {
        [Key]
        public int ID_DemoLogk { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public string Rmq { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_CRR { get; set; }

        [StringLength(50)]
        public string Agent { get; set; }

        public int? ID_Prosp { get; set; }

        [StringLength(2)]
        public string P1 { get; set; }
        [StringLength(2)]
        public string P2 { get; set; }
    }
}
