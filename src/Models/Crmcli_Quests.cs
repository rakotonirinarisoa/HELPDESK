namespace Helpdesk
{
    using System;

    public partial class Crmcli_Quests
    {
        public int ID { get; set; }

        public int? ID_Demandes { get; set; }

        public DateTime? Date_Comm { get; set; }

        public DateTime? Date_Responses { get; set; }

        public int? ClariteExplications { get; set; }

        public int? DelaisTraitements { get; set; }

        public int? QualiteSolutions { get; set; }

        public int? LogistiqueUtilises { get; set; }

        public int? Reactivites { get; set; }

        public int? Attitudes { get; set; }

        public int? Disponibilites { get; set; }

        public int? Ponctualites { get; set; }

        public int? Competences { get; set; }

        public string Commentaires { get; set; }
    }
}
