namespace Helpdesk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Mvc;

    public partial class Crmcom_CommercialeProsp
    {
        public int ID { get; set; }

        public int? Comp_CompanyId { get; set; }

        [StringLength(10)]
        public string Bailleur_Fond { get; set; }

        public int? ID_TypeClient { get; set; }

        public int? ID_Etat { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Debut { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Fin { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_REBUT { get; set; }

        public int? ID_Rebut { get; set; }

        public int? ID_Rebut_Reason { get; set; }

        [StringLength(20)]
        public string Produit { get; set; }

        public string ApporteurAffaire { get; set; }

        [StringLength(20)]
        public string Options { get; set; }

        [StringLength(20)]
        public string ModeComm { get; set; }

        [StringLength(20)]
        public string Formation { get; set; }

        [StringLength(5)]
        public string BudgetDevise { get; set; }

        [StringLength(20)]
        public string EtapS { get; set; }

        public int? Nombre_User { get; set; }

        public int? Nombre_Salarie { get; set; }

        public int? ID_Classification { get; set; }

        public string Existant { get; set; }

        public string Analyse_Besoin { get; set; }

        [StringLength(50)]
        public string Budget { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeadLine { get; set; }

        [StringLength(20)]
        public string ID_Source { get; set; }

        public string Interlocuteur { get; set; }

        public string Decisionnaire { get; set; }

        public string PA { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_ProposContrat { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_ProgForm { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_PrecoTech { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_ProForma { get; set; }

        public string Remise { get; set; }

        public string Modalite_Paiement { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_ReceptionBC { get; set; }

        [StringLength(50)]
        public string Duree_Traitement { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_ReceptionAcompte { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_BL { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Contrat { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_CRL { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_RetourContrat { get; set; }

        public int? User_UserId { get; set; }

        public string ReferenceOppo { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Creation { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Update { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Synch { get; set; }

        [StringLength(50)]
        public string MontantOffre { get; set; }

        [StringLength(5)]
        public string MontantOffreDevise { get; set; }

        [StringLength(50)]
        public string MPF { get; set; }

        /// <summary>
        /// COLLECTIONS COMBOBOX
        /// </summary>
        /// 
        [NotMapped]
        public List<String> ProduitsCollection { get; set; }

        [NotMapped]
        public List<String> FMFPCollection { get; set; }

        [NotMapped]
        public List<String> ModeCollection { get; set; }

        [NotMapped]
        public List<String> EtapSCollection { get; set; }

        [NotMapped]
        public List<String> FormationCollection { get; set; }

        [NotMapped]
        public List<String> CampagneCollection { get; set; }

        [NotMapped]
        public List<String> BudgetMonCollection { get; set; }

        [NotMapped]
        public List<String> MontantOffCollection { get; set; }

        [NotMapped]
        public List<String> TypesCollection { get; set; }

        [NotMapped]
        public List<String> UserCollection { get; set; }

        [NotMapped]
        public List<String> EtatsCollection { get; set; }

        [NotMapped]
        public List<String> ClassificationsCollection { get; set; }

        [NotMapped]
        public List<String> SourcesCollection { get; set; }

        [NotMapped]
        public List<String> PrevisReel { get; set; }

        [NotMapped]
        public List<String> PDeadCollection { get; set; }

        [NotMapped]
        public List<String> BailleursCollection { get; set; }

        [NotMapped]
        public List<String> RebutsCollection { get; set; }

        [NotMapped]
        public List<String> RebutsRCollection { get; set; }

        [NotMapped]
        public string da { get; set; }
        [NotMapped]
        public string daa { get; set; }
        [NotMapped]
        public string daaa { get; set; }

        [NotMapped]
        public string User_Name { get; set; }

        [NotMapped]
        public List<SelectListItem> Ag { get; set; }
        [NotMapped]
        public int[] AgIds { get; set; }

        [NotMapped]
        public List<SelectListItem> Ag2 { get; set; }
        [NotMapped]
        public int[] AgIds2 { get; set; }

        [NotMapped]
        public List<SelectListItem> Op { get; set; }
        [NotMapped]
        public int[] OpIds { get; set; }

        /*[NotMapped]
        public List<SelectListItem> ProdMod2 { get; set; }
        [NotMapped]
        public int[] ProdMod { get; set; }*/

        /*[NotMapped]
        public List<AnalYs> AnalY { get; set; }*/

        [StringLength(2)]
        public string P1ProposContrat { get; set; }
        [StringLength(2)]
        public string P1ProgForm { get; set; }
        [StringLength(2)]
        public string P1PrecoTech { get; set; }
        [StringLength(2)]
        public string P1ReceptionBC { get; set; }
        [StringLength(2)]
        public string P1ReceptionAcompte { get; set; }
        [StringLength(2)]
        public string P1BL { get; set; }
        [StringLength(2)]
        public string P1Contrat { get; set; }
        [StringLength(2)]
        public string P1CRL { get; set; }
        [StringLength(2)]
        public string P1RetourContrat { get; set; }
        [StringLength(2)]
        public string P1ProForma { get; set; }

        public int? Camp_CampaignId { get; set; }

        public int? WaIt_WaveItemId { get; set; }

        [NotMapped]
        public string CCAMP { get; set; }
        [NotMapped]
        public string WAIT { get; set; }
        [NotMapped]
        public string WAIT2 { get; set; }

        [StringLength(10)]
        public string FMFP { get; set; }

        [StringLength(2)]
        public string PDead { get; set; }

        [NotMapped]
        public int IDAGETNS { get; set; }

        public int? AssDemarage { get; set; }

        public int? AssAnnuel { get; set; }

        public int? AssFormation { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateEnvoieOFF { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? MoisPrev { get; set; }
        [NotMapped]
        public DateTime? DateEO { get; set; }
        [NotMapped]
        public DateTime? Mprev { get; set; }
        [NotMapped]
        public string AssD { get; set; }
        [NotMapped]
        public string AssA { get; set; }
        [NotMapped]
        public string AssF { get; set; }


    }
}
