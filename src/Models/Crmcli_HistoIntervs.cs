namespace Helpdesk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class Crmcli_HistoIntervs
    {
        public int ID { get; set; }

        public int? ID_Demandes { get; set; }

        public DateTime? Date_Comm { get; set; }

        public string Sujets { get; set; }

        public string Descriptions { get; set; }

        public int? ID_Pers_Validateur { get; set; }

        public TimeSpan? Debut { get; set; }

        public TimeSpan? Fin { get; set; }

        [StringLength(20)]
        public string IP_CheckPC { get; set; }

        public string Observations { get; set; }

        public DateTime? Date_Validation { get; set; }

        public int? ID_Company { get; set; }

        public string ID_Agent { get; set; }

        [StringLength(20)]
        public string EtatH { get; set; }

        [StringLength(20)]
        public string Activite { get; set; }

        [StringLength(20)]
        public string Nature { get; set; }

        public string CommentairesH { get; set; }

        [NotMapped]
        public List<String> TypePrestaCollection { get; set; }

        [NotMapped]
        public List<String> EtatsCollection { get; set; }

        public TimeSpan? Debut_Pause1 { get; set; }

        public TimeSpan? Fin_Pause1 { get; set; }

        public TimeSpan? Debut_Pause2 { get; set; }

        public TimeSpan? Fin_Pause2 { get; set; }

        [NotMapped]
        public List<SelectListItem> Ag { get; set; }

        [NotMapped]
        public int[] AgIds { get; set; }

        public string Lien_Validation { get; set; }

        public int? SenderAgent { get; set; }

        public DateTime? DateSaisieHisto { get; set; }

        public string Seance { get; set; }

        public string PERSP { get; set; }
    }
}
