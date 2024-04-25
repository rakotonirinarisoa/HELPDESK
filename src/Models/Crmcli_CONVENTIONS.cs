namespace Helpdesk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcli_CONVENTIONS
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string REFERENCE { get; set; }

        public int? NUMERO { get; set; }

        [StringLength(100)]
        public string INTITULE { get; set; }

        public DateTime? DATESAISIE { get; set; }

        public DateTime? DATEENVOI { get; set; }

        public int? SAISISSEUR { get; set; }

        public string DESCRIPTION { get; set; }

        [StringLength(50)]
        public string ETAT { get; set; }

        public int? CLIENT { get; set; }

        public string NOMCONTACT { get; set; }

        [StringLength(100)]
        public string NUMCONTACT { get; set; }

        public string MAILCONTACT { get; set; }

        public DateTime? DATEPHASE1 { get; set; }

        public DateTime? DATEPHASE2 { get; set; }

        public DateTime? DATEPHASE3 { get; set; }

        public DateTime? DATEPHASE4 { get; set; }

        public DateTime? DATEPHASE5 { get; set; }

        public DateTime? DATEPHASE6 { get; set; }

        public string COMMERCIALE { get; set; }

        [NotMapped]
        public List<String> EtatCollection { get; set; }
    }
}
