namespace Helpdesk
{
    public partial class Crmcli_Taches
    {
        public int ID { get; set; }

        public int? IDDemande { get; set; }

        public int? Case_CaseId { get; set; }

        public virtual Crmcli_Demandes Crmcli_Demandes { get; set; }
    }
}
