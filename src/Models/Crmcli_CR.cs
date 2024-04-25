namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcli_CR
    {
        public int ID { get; set; }

        public int? ID_CASES { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DATE { get; set; }

        public string DESTCLIENT { get; set; }

        public string DESTMAIL { get; set; }

        public string DOCREM { get; set; }
    }
}
