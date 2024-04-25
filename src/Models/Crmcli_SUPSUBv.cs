namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcli_SUPSUBv
    {
        public int ID { get; set; }

        public DateTime? DateValidation { get; set; }

        public int? IDUser { get; set; }

        public int? IDValideur { get; set; }

        public TimeSpan? HUser { get; set; }

        public TimeSpan? HValidateur { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateRapport { get; set; }

        public TimeSpan? HPLUS { get; set; }
        public TimeSpan? HMOINS { get; set; }

        public string COMMS { get; set; }
    }
}
