namespace Helpdesk
{
    using System;

    public partial class Crmcli_HistoriqueRelances
    {
        public int ID { get; set; }

        public int ID_Demande { get; set; }

        public DateTime? DateRelance { get; set; }
    }
}
