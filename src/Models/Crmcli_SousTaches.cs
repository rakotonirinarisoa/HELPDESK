namespace Helpdesk
{
    using System;

    public partial class Crmcli_SousTaches
    {
        public int ID { get; set; }

        public int? ID_Taches { get; set; }

        public int? Comm_CommunicationId { get; set; }

        public string Commentaires { get; set; }

        public DateTime? DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        public virtual Communication Communication { get; set; }

        public virtual Crmcli_Taches Crmcli_Taches { get; set; }
    }
}
