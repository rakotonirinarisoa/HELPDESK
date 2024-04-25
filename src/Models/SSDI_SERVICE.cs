namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SSDI_SERVICE
    {
        [Key]
        public int serv_SSDI_SERVICEid { get; set; }

        public int? serv_CreatedBy { get; set; }

        public DateTime? serv_CreatedDate { get; set; }

        public int? serv_UpdatedBy { get; set; }

        public DateTime? serv_UpdatedDate { get; set; }

        public DateTime? serv_TimeStamp { get; set; }

        public byte? serv_Deleted { get; set; }

        public int? serv_WorkflowId { get; set; }

        public int? serv_Secterr { get; set; }

        public int? serv_companyid { get; set; }

        public int? serv_UserId { get; set; }

        public int? serv_SSDI_BUSINESSid { get; set; }

        [StringLength(19)]
        public string serv_typeservice { get; set; }

        [StringLength(69)]
        public string serv_name { get; set; }

        public int? serv_productid { get; set; }

        public int? serv_productfamilyid { get; set; }

        public int? serv_contactclient { get; set; }

        public int? serv_partenaire { get; set; }

        public byte? serv_partenairecommission_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_partenairecommission { get; set; }

        public byte? serv_tarif_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_tarif { get; set; }

        [StringLength(768)]
        public string serv_commentgestion { get; set; }

        public int? serv_channelid { get; set; }

        [StringLength(11)]
        public string serv_typecontrat { get; set; }

        [StringLength(2)]
        public string serv_statut { get; set; }

        public DateTime? serv_dateresiliation { get; set; }

        [StringLength(80)]
        public string serv_motifresiliation { get; set; }

        public DateTime? serv_datedebut { get; set; }

        public DateTime? serv_datefin { get; set; }

        [StringLength(2)]
        public string serv_modeleconso { get; set; }

        [StringLength(6)]
        public string serv_uniteconso { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_qtevendu { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_qteofferte { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_reportforfaitprec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_equivheure { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_equivheureN2 { get; set; }

        public int? serv_reportref { get; set; }

        [StringLength(717)]
        public string serv_commentservice { get; set; }

        [StringLength(2)]
        public string serv_slaid { get; set; }

        public byte? serv_prixunitaire_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_prixunitaire { get; set; }

        [StringLength(2)]
        public string serv_frequencedetail { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_deplacement { get; set; }

        [StringLength(40)]
        public string serv_talendexterkey { get; set; }

        [StringLength(1)]
        public string serv_emailsend1 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_total_credit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_actions_total_conso { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_actions_pourcent_valoris { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_actions_pourcent_conso { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_total_solde { get; set; }

        public int? serv_tickets_nbre_total { get; set; }

        public int? serv_actions_nbre_total { get; set; }

        public int? serv_actions_nbre_nonvaloris { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_actions_total_tps_mo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_actions_total_tps_depl { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_pourcent_temps_passe { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_nbjr { get; set; }

        [StringLength(1)]
        public string serv_emailsendrenta { get; set; }

        [StringLength(1)]
        public string serv_visibleExtranetSig { get; set; }

        [StringLength(1)]
        public string serv_nonvisibltech { get; set; }

        public int? serv_SSDI_TDSid { get; set; }

        public DateTime? serv_majgesco { get; set; }

        public DateTime? serv_majgescoabo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_nombrejourfacture { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? serv_nombrejouroffert { get; set; }
    }
}
