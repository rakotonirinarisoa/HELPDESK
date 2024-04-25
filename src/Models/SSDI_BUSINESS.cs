namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SSDI_BUSINESS
    {
        [Key]
        public int busi_SSDI_BUSINESSid { get; set; }

        public int? busi_CreatedBy { get; set; }

        public DateTime? busi_CreatedDate { get; set; }

        public int? busi_UpdatedBy { get; set; }

        public DateTime? busi_UpdatedDate { get; set; }

        public DateTime? busi_TimeStamp { get; set; }

        public byte? busi_Deleted { get; set; }

        public int? busi_WorkflowId { get; set; }

        public int? busi_Secterr { get; set; }

        public int? busi_PersonId { get; set; }

        [StringLength(67)]
        public string busi_name { get; set; }

        public int? busi_channelid { get; set; }

        public int? busi_companyid { get; set; }

        public int? busi_UserId { get; set; }

        public int? busi_prestataireid { get; set; }

        public int? busi_contactclient { get; set; }

        [StringLength(2)]
        public string busi_statut { get; set; }

        [StringLength(1)]
        public string busi_appartconcurrent { get; set; }

        public int? busi_partenaireid { get; set; }

        [StringLength(2)]
        public string busi_typebusiness { get; set; }

        [StringLength(2)]
        public string busi_positionconcurren { get; set; }

        [StringLength(1)]
        public string busi_estcontratservice { get; set; }

        public int? busi_resptechnique { get; set; }

        public int? busi_interv2 { get; set; }

        public DateTime? busi_dateinit { get; set; }

        public int? busi_periodicite { get; set; }

        public DateTime? busi_datefin { get; set; }

        [StringLength(2)]
        public string busi_typereconduction { get; set; }

        [StringLength(11)]
        public string busi_typecontrat { get; set; }

        public int? busi_refcontratcrm { get; set; }

        public int? busi_dureecontrat { get; set; }

        public DateTime? busi_dateresilitation { get; set; }

        [StringLength(80)]
        public string busi_motifresiliation { get; set; }

        [StringLength(3126)]
        public string busi_commentcontrat { get; set; }

        [StringLength(4)]
        public string busi_modereglement { get; set; }

        [StringLength(4)]
        public string busi_echeancefacturation { get; set; }

        [StringLength(50)]
        public string busi_refcontrat { get; set; }

        [StringLength(24)]
        public string busi_listeprestations { get; set; }

        [StringLength(2)]
        public string busi_listemodelecontrat { get; set; }

        [StringLength(500)]
        public string busi_descriservice { get; set; }

        public byte? busi_prix_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? busi_prix { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? busi_qtemodele { get; set; }

        [StringLength(30)]
        public string busi_listemateriels { get; set; }

        [StringLength(40)]
        public string busi_listelogiciels { get; set; }

        [StringLength(10)]
        public string busi_modefinance { get; set; }

        public byte? busi_financeloyer_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? busi_financeloyer { get; set; }

        public byte? busi_financemont_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? busi_financemont { get; set; }

        public byte? busi_financeracha_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? busi_financeracha { get; set; }

        [StringLength(2)]
        public string busi_satisfaction { get; set; }

        [StringLength(500)]
        public string busi_descrisatisfaction { get; set; }

        [StringLength(2)]
        public string busi_satisreactivite { get; set; }

        [StringLength(40)]
        public string busi_satiscompetence { get; set; }

        [StringLength(40)]
        public string busi_satisrelation { get; set; }

        [StringLength(40)]
        public string busi_satisqualiteproduit { get; set; }

        [StringLength(40)]
        public string busi_satisconditcom { get; set; }

        public DateTime? busi_satisdatemaj { get; set; }

        [StringLength(40)]
        public string busi_talendexterkey { get; set; }

        public DateTime? busi_notifydate1 { get; set; }

        [StringLength(1)]
        public string busi_acqlog { get; set; }

        [StringLength(1)]
        public string busi_reabo { get; set; }

        [StringLength(40)]
        public string busi_modecom { get; set; }

        public DateTime? busi_dattac { get; set; }

        [StringLength(20)]
        public string busi_objet { get; set; }

        [StringLength(1)]
        public string busi_emailsendrenta { get; set; }

        [StringLength(1)]
        public string busi_nonvisibltech { get; set; }

        public DateTime? busi_majGesco { get; set; }

        public int? busi_tiers { get; set; }

        public int? busi_codecomptable { get; set; }

        [StringLength(20)]
        public string busi_codecomptable2 { get; set; }

        [StringLength(255)]
        public string busi_offre { get; set; }

        public DateTime? busi_proforma { get; set; }

        public DateTime? busi_devis { get; set; }

        public DateTime? busi_FA { get; set; }

        [StringLength(20)]
        public string busi_Etat1 { get; set; }

        [StringLength(20)]
        public string busi_Etat2 { get; set; }

        [StringLength(20)]
        public string busi_Etat3 { get; set; }

        [StringLength(40)]
        public string busi_name2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? busi_nbrejourlim { get; set; }

        [StringLength(40)]
        public string busi_nbjrlim { get; set; }

        [StringLength(255)]
        public string busi_name3 { get; set; }
    }
}
