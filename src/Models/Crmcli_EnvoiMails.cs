namespace Helpdesk
{
    using System;

    public partial class Crmcli_EnvoiMails
    {
        public int ID { get; set; }

        public DateTime? CommDATE { get; set; }

        public int? Case_CaseId { get; set; }

        public DateTime? DateInterne { get; set; }

        public DateTime? DateClient { get; set; }
    }
}
