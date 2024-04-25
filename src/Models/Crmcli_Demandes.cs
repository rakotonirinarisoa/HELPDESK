namespace Helpdesk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcli_Demandes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Crmcli_Demandes()
        {
            Crmcli_Taches = new HashSet<Crmcli_Taches>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroDemande { get; set; }

        public string SujetDemande { get; set; }

        public string DescriptionDemande { get; set; }

        public short? PrioriteDemande { get; set; }

        public short? NiveauDemande { get; set; }

        public string CommentaireNivDemande { get; set; }

        [StringLength(7)]
        public string TypeDemande { get; set; }

        public int? EtatDemande { get; set; }

        [StringLength(7)]
        public string ProduitDemande { get; set; }

        public int? Comp_CompanyId { get; set; }

        public DateTime? DateDemande { get; set; }

        public DateTime? DatePropose { get; set; }

        public DateTime? DateAccepte { get; set; }

        public string ComAnnulation { get; set; }

        public int? Demandeur { get; set; }

        public bool? Etat_Accuse { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Crmcli_Taches> Crmcli_Taches { get; set; }

        public int? Consu_LU { get; set; }

        public int? Theme { get; set; }

        [StringLength(7)]
        public string Rubrique { get; set; }

        [NotMapped]
        public List<String> ProduitsCollection { get; set; }

        [NotMapped]
        public List<String> TypeDemanCollection { get; set; }

        [NotMapped]
        public List<String> DemandeurCollection { get; set; }

        [NotMapped]
        public List<String> PrioritesCollection { get; set; }

        [NotMapped]
        public List<String> EtatCollection { get; set; }

        [NotMapped]
        public List<String> NiveauCollection { get; set; }

        [NotMapped]
        public List<String> ThemeCollection { get; set; }

        [NotMapped]
        public string ClientCollection { get; set; }
    }
}
