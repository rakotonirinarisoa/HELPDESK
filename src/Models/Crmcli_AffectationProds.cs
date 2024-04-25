namespace Helpdesk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcli_AffectationProds
    {
        public int ID { get; set; }

        [StringLength(7)]
        public string AFF_Produit { get; set; }

        public string AFF_Agent { get; set; }

        [NotMapped]
        public List<String> Produit { get; set; }
    }
}
