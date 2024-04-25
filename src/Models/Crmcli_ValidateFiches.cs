namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcli_ValidateFiches
    {
        public int ID { get; set; }

        public int? ID_Cases { get; set; }

        public DateTime? Date_Comm { get; set; }

        public DateTime? Date_Validation { get; set; }

        public int? ID_Company { get; set; }

        public int? ID_Pers_Validateur { get; set; }

        public TimeSpan? Debut { get; set; }

        public TimeSpan? Fin { get; set; }

        [StringLength(5)]
        public string NumFiche { get; set; }

        [StringLength(20)]
        public string IP_CheckPC { get; set; }
    }
}
