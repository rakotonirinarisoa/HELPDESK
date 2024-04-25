namespace Helpdesk
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcli_UsersSession
    {
        public int ID { get; set; }

        public int? ID_Company { get; set; }

        [StringLength(20)]
        public string Pseudo { get; set; }

        [StringLength(20)]
        public string Mdp { get; set; }

        public int? ID_Person { get; set; }

        [NotMapped]
        public string SIGNNAME { get; set; }

        [NotMapped]
        public List<string> SIGNNAME2 { get; set; }
    }
}
