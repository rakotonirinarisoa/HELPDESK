namespace Helpdesk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Cases
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cases()
        {
            //Crmcli_EnvoiMail = new HashSet<Crmcli_EnvoiMail>();
            Crmcli_Taches = new HashSet<Crmcli_Taches>();
        }

        [Key]
        public int Case_CaseId { get; set; }

        public int? Case_PrimaryCompanyId { get; set; }

        public int? Case_PrimaryPersonId { get; set; }

        public int? Case_AssignedUserId { get; set; }

        public int? Case_ChannelId { get; set; }

        [StringLength(255)]
        public string Case_Description { get; set; }

        [StringLength(30)]
        public string Case_CustomerRef { get; set; }

        [StringLength(12)]
        public string Case_Source { get; set; }

        [StringLength(30)]
        public string Case_SerialNumber { get; set; }

        [StringLength(30)]
        public string Case_Product { get; set; }

        [StringLength(80)]
        public string Case_ProblemType { get; set; }

        [StringLength(40)]
        public string Case_SolutionType { get; set; }

        public string Case_ProblemNote { get; set; }

        public string Case_SolutionNote { get; set; }

        public DateTime? Case_Opened { get; set; }

        public int? Case_OpenedBy { get; set; }

        public DateTime? Case_Closed { get; set; }

        public int? Case_ClosedBy { get; set; }

        [StringLength(11)]
        public string Case_Status { get; set; }

        [StringLength(13)]
        public string Case_Stage { get; set; }

        [StringLength(40)]
        public string Case_Priority { get; set; }

        public DateTime? Case_TargetClose { get; set; }

        public int? Case_CreatedBy { get; set; }

        public DateTime? Case_CreatedDate { get; set; }

        public int? Case_UpdatedBy { get; set; }

        public DateTime? Case_UpdatedDate { get; set; }

        public DateTime? Case_TimeStamp { get; set; }

        public byte? Case_Deleted { get; set; }

        [StringLength(128)]
        public string Case_ReferenceId { get; set; }

        [StringLength(13)]
        public string Case_ProductArea { get; set; }

        [StringLength(40)]
        public string Case_TargetVer { get; set; }

        [StringLength(40)]
        public string Case_FoundVer { get; set; }

        public int? Case_ProductId { get; set; }

        public DateTime? Case_NotifyTime { get; set; }

        public int? Case_SecTerr { get; set; }

        public int? Case_WorkflowId { get; set; }

        [StringLength(40)]
        public string Case_SLASeverity { get; set; }

        public int? case_SLAID { get; set; }

        public DateTime? case_SLACloseBy { get; set; }

        public DateTime? case_SLAAmberCloseBy { get; set; }

        public int? Case_PrimaryAccountId { get; set; }

        [StringLength(40)]
        public string case_TalendExterKey { get; set; }

        public int? Case_SSDI_BUSINESSId { get; set; }

        public int? Case_SSDI_SERVICEId { get; set; }

        public int? case_ssdi_ticketprincipal { get; set; }

        [StringLength(1)]
        public string case_ssdi_nomail { get; set; }

        [StringLength(1)]
        public string case_ssdi_visibleExtranetSig { get; set; }

        [StringLength(20)]
        public string case_ssdi_contacttel { get; set; }

        [StringLength(40)]
        public string case_ssdi_contactnom { get; set; }

        [StringLength(2)]
        public string case_ssdi_familleci { get; set; }

        [StringLength(3)]
        public string case_ssdi_ci { get; set; }

        public string case_ssdi_demandecomment { get; set; }

        public DateTime? case_ssdi_datemessage { get; set; }

        [StringLength(80)]
        public string case_ssdi_ci_infosuppl { get; set; }

        [StringLength(2)]
        public string case_ssdi_impact { get; set; }

        [StringLength(2)]
        public string case_ssdi_urgence { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? case_ssdi_totalValuation { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? case_ssdi_totalCommTime { get; set; }

        public int? case_ssdi_ciid { get; set; }

        [StringLength(1)]
        public string case_emailsend1 { get; set; }

        [StringLength(1)]
        public string case_emailsend2 { get; set; }

        [StringLength(1)]
        public string case_emailsend3 { get; set; }

        [StringLength(1)]
        public string case_emailsend4 { get; set; }

        [StringLength(1)]
        public string case_emailsend5 { get; set; }

        [StringLength(40)]
        public string case_prestation { get; set; }

        public int? case_HP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? case_JP { get; set; }

        [StringLength(40)]
        public string case_version { get; set; }

        public string case_plandaction { get; set; }

        [StringLength(20)]
        public string case_codeessai { get; set; }

        public int? case_acontacte { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? case_ssdi_tpsvendu { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? case_ssdi_resteafaire { get; set; }

        [StringLength(32)]
        public string case_ssdi_satisde_hash { get; set; }

        public DateTime? case_ssdi_satisde_start { get; set; }

        public DateTime? case_ssdi_satisde_validity { get; set; }

        public DateTime? case_ssdi_satifde_submit { get; set; }

        [StringLength(1)]
        public string case_emailsendsatis { get; set; }

        [StringLength(1)]
        public string case_emailsendsatis2 { get; set; }

        public int? case_SSDI_TDSId { get; set; }

        [StringLength(5)]
        public string case_ssdi_sla { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? case_ssdi_ElapsedTimeSolved { get; set; }

        public DateTime? case_ssdi_deadLineSolved { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? case_ssdi_ElapsedTimeLogged { get; set; }

        public DateTime? case_ssdi_deadLineLogged { get; set; }

        [StringLength(8)]
        public string case_ssdi_slacolor { get; set; }

        [StringLength(1)]
        public string case_ssdi_nonvisibltech { get; set; }

        [StringLength(80)]
        public string case_gamme { get; set; }

        public int? case_contrat2 { get; set; }

        public int? case_contrat3 { get; set; }

        public int? case_contrat4 { get; set; }

        [StringLength(80)]
        public string case_Etape { get; set; }

        public DateTime? case_debut { get; set; }

        [StringLength(255)]
        public string case_Produit1 { get; set; }

        [StringLength(255)]
        public string case_produit2 { get; set; }

        [StringLength(255)]
        public string case_produit3 { get; set; }

        [StringLength(255)]
        public string case_produit4 { get; set; }

        [StringLength(1)]
        public string case_djFA { get; set; }

        [StringLength(40)]
        public string case_etatfac { get; set; }

        public DateTime? case_derninterv { get; set; }

        [StringLength(1)]
        public string case_oncreatesendsms { get; set; }

        [StringLength(1)]
        public string case_onclosesendsms { get; set; }

        [StringLength(40)]
        public string case_typeform { get; set; }

        [StringLength(40)]
        public string case_prod1 { get; set; }

        [StringLength(255)]
        public string case_pord2 { get; set; }

        [StringLength(40)]
        public string case_prod3 { get; set; }

        [StringLength(40)]
        public string case_prod4 { get; set; }

        [StringLength(40)]
        public string case_version2 { get; set; }

        [StringLength(40)]
        public string case_version3 { get; set; }

        [StringLength(40)]
        public string case_version4 { get; set; }

        [StringLength(1)]
        public string case_p1 { get; set; }

        public int? case_p2 { get; set; }

        [StringLength(1)]
        public string case_p3 { get; set; }

        [StringLength(1)]
        public string case_p4 { get; set; }

        public int? case_test { get; set; }

        public int? Case_PROJETId { get; set; }

        public int? case_contrat5 { get; set; }

        public int? case_contr { get; set; }

        public int? case_test2 { get; set; }

        public int? case_societe2 { get; set; }

        public DateTime? case_fincont1 { get; set; }

        [StringLength(40)]
        public string case_depart { get; set; }

        [StringLength(10)]
        public string case_numdemande { get; set; }

        public DateTime? case_dateconfirme { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Crmcli_EnvoiMail> Crmcli_EnvoiMail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Crmcli_Taches> Crmcli_Taches { get; set; }
    }
}
