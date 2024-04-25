namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Opportunity")]
    public partial class Opportunity
    {
        [Key]
        public int Oppo_OpportunityId { get; set; }

        public int? Oppo_PrimaryCompanyId { get; set; }

        public int? Oppo_PrimaryPersonId { get; set; }

        public int? Oppo_AssignedUserId { get; set; }

        public int? Oppo_ChannelId { get; set; }

        [StringLength(50)]
        public string Oppo_Description { get; set; }

        [StringLength(40)]
        public string Oppo_Type { get; set; }

        [StringLength(30)]
        public string Oppo_Product { get; set; }

        [StringLength(40)]
        public string Oppo_Source { get; set; }

        public string Oppo_Note { get; set; }

        [StringLength(30)]
        public string Oppo_CustomerRef { get; set; }

        public DateTime? Oppo_Opened { get; set; }

        public DateTime? Oppo_Closed { get; set; }

        [StringLength(15)]
        public string Oppo_Status { get; set; }

        [StringLength(20)]
        public string Oppo_Stage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Oppo_Forecast { get; set; }

        public int? Oppo_Certainty { get; set; }

        [StringLength(40)]
        public string Oppo_Priority { get; set; }

        public DateTime? Oppo_TargetClose { get; set; }

        public int? Oppo_CreatedBy { get; set; }

        public DateTime? Oppo_CreatedDate { get; set; }

        public int? Oppo_UpdatedBy { get; set; }

        public DateTime? Oppo_UpdatedDate { get; set; }

        public DateTime? Oppo_TimeStamp { get; set; }

        public byte? Oppo_Deleted { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Oppo_Total { get; set; }

        public DateTime? Oppo_NotifyTime { get; set; }

        [StringLength(20)]
        public string Oppo_SMSSent { get; set; }

        public int? Oppo_WaveItemId { get; set; }

        public int? Oppo_SecTerr { get; set; }

        public int? Oppo_WorkflowId { get; set; }

        public int? Oppo_LeadID { get; set; }

        public byte? Oppo_Forecast_CID { get; set; }

        public byte? Oppo_Total_CID { get; set; }

        [StringLength(40)]
        public string oppo_scenario { get; set; }

        [StringLength(40)]
        public string oppo_decisiontimeframe { get; set; }

        public int? oppo_Currency { get; set; }

        public byte? oppo_TotalOrders_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_TotalOrders { get; set; }

        public byte? oppo_totalQuotes_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_totalQuotes { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_NoDiscAmtSum { get; set; }

        public byte? oppo_NoDiscAmtSum_CID { get; set; }

        public int? Oppo_PrimaryAccountId { get; set; }

        [StringLength(40)]
        public string oppo_TalendExterKey { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_proj_gonogo { get; set; }

        public int? oppo_ssdi_proj_consultantgo { get; set; }

        public DateTime? oppo_ssdi_proj_dateconsultan { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_poj_decisionconsul { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_marge { get; set; }

        public byte? oppo_ssdi_marge_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_margepond { get; set; }

        public byte? oppo_ssdi_margepond_CID { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_satisroi { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_satisrelation { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_satisreactivite { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_satisqualiteprodui { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_satisfaction { get; set; }

        public DateTime? oppo_ssdi_satisdatemaj { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_satisconditcom { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_satiscompetence { get; set; }

        public int? oppo_ssdi_clone_nb { get; set; }

        public int? oppo_clone_frequence { get; set; }

        public int? oppo_ssdi_caneg_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_caneg { get; set; }

        public int? oppo_ssdi_canegma_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_canegma { get; set; }

        public int? oppo_ssdi_caneglo_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_caneglo { get; set; }

        public int? oppo_ssdi_margneg_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_margneg { get; set; }

        public int? oppo_ssdi_txmargneg { get; set; }

        public int? oppo_ssdi_acnegma_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_acnegma { get; set; }

        public int? oppo_ssdi_acneglo_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_acneglo { get; set; }

        public int? oppo_ssdi_caservice_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_caservice { get; set; }

        public int? oppo_ssdi_margser_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_margser { get; set; }

        public int? oppo_ssdi_txmargserv { get; set; }

        public int? oppo_ssdi_proj_ct_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_proj_ct { get; set; }

        public int? oppo_ssdi_txmargtotal { get; set; }

        public int? Oppo_SSDI_BUSINESSId { get; set; }

        public int? oppo_CaseId { get; set; }

        public byte? oppo_ssdi_vente_sage_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_vente_sage { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_ssdi_vente_sage_pourc { get; set; }

        [StringLength(40)]
        public string oppo_sscont { get; set; }

        public int? oppo_elmntparc { get; set; }

        public int? oppo_prestserv { get; set; }

        public string oppo_motifnogo { get; set; }

        [StringLength(40)]
        public string oppo_sousetap { get; set; }

        [StringLength(255)]
        public string oppo_SCRMcompetitor { get; set; }

        [StringLength(40)]
        public string oppo_SCRMwinner { get; set; }

        public string oppo_SCRMreasonforloss { get; set; }

        [StringLength(1)]
        public string Oppo_SCRMIsCrossSell { get; set; }

        public int? Oppo_SCRMOriginalOppoId { get; set; }

        [StringLength(255)]
        public string oppo_cle_en_main { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_nb_param { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_nb_jr_formation { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_nb_jr_dev { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_jr_facture { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_nb_jr_tot { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_jr_offert { get; set; }

        [StringLength(1)]
        public string oppo_parc { get; set; }

        public int? Oppo_SSDI_SERVICEId { get; set; }

        [StringLength(30)]
        public string oppo_ssdi_vvtypediag { get; set; }

        [StringLength(150)]
        public string oppo_ssdi_vvurletude { get; set; }

        [StringLength(10)]
        public string oppo_ssdi_vvdiagid { get; set; }

        [StringLength(10)]
        public string oppo_ssdi_vvoppoid { get; set; }

        [StringLength(25)]
        public string oppo_ssdi_bdcsage { get; set; }

        public DateTime? oppo_ssdi_datelivr { get; set; }

        [StringLength(25)]
        public string oppo_ssdi_dopiece { get; set; }

        [StringLength(2)]
        public string oppo_ssdi_typedoc { get; set; }

        public DateTime? oppo_ssdi_majgesco { get; set; }

        [StringLength(30)]
        public string oppo_ssdi_reference { get; set; }

        [StringLength(40)]
        public string oppo_etapesoftwell { get; set; }

        [StringLength(40)]
        public string oppo_interesse { get; set; }

        [StringLength(40)]
        public string oppo_logicielconforme { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_dernierprix { get; set; }

        public int? oppo_dernierprix_CID { get; set; }

        public DateTime? oppo_dateprobablelivraison { get; set; }

        [StringLength(40)]
        public string oppo_modalitepaiement { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_durreTraitement { get; set; }

        public DateTime? oppo_DateAnalyseBesoin { get; set; }

        public DateTime? oppo_DateCRA { get; set; }

        public DateTime? oppo_DateDemo { get; set; }

        public DateTime? oppo_DateCRDemo { get; set; }

        public DateTime? oppo_DateDevis { get; set; }

        public DateTime? oppo_DatePROFORM { get; set; }

        public DateTime? oppo_DatePRECO { get; set; }

        public DateTime? oppo_dateCRDevis { get; set; }

        public DateTime? oppo_DateAutreRDV { get; set; }

        public DateTime? oppo_CRAutreRDV { get; set; }

        public DateTime? oppo_DatepropoContr { get; set; }

        public DateTime? oppo_DateNego { get; set; }

        public DateTime? oppo_DateCRNego { get; set; }

        [StringLength(80)]
        public string oppo_ModalitPaiement { get; set; }

        public DateTime? oppo_DateRIB { get; set; }

        public int? oppo_PRODUIT { get; set; }

        public string oppo_detailmodalite { get; set; }

        [StringLength(255)]
        public string oppo_produit2 { get; set; }

        public DateTime? oppo_DateProfo { get; set; }

        public DateTime? oppo_DateBC { get; set; }

        public DateTime? oppo_DateBL { get; set; }

        public DateTime? oppo_DateFA { get; set; }

        public DateTime? oppo_DateContrat { get; set; }

        public DateTime? oppo_BPSage { get; set; }

        public DateTime? oppo_PCSage { get; set; }

        public DateTime? oppo_CRLivraison { get; set; }

        public string oppo_SoldeVente { get; set; }

        public string oppo_RecouVente { get; set; }

        public DateTime? oppo_BLSage { get; set; }

        public string oppo_Datefinform { get; set; }

        public DateTime? oppo_DateCRF { get; set; }

        public DateTime? oppo_DateFAForm { get; set; }

        public string oppo_RecouForm { get; set; }

        public string oppo_formSolde { get; set; }

        public DateTime? oppo_Datedemoadv { get; set; }

        public int? oppo_utldemoadv { get; set; }

        [StringLength(1)]
        public string oppo_p { get; set; }

        public int? oppo_appaffaire { get; set; }

        public string oppo_appcom { get; set; }

        [StringLength(1)]
        public string oppo_p1 { get; set; }

        [StringLength(1)]
        public string oppo_p2 { get; set; }

        [StringLength(1)]
        public string oppo_p4 { get; set; }

        [StringLength(1)]
        public string oppo_p3 { get; set; }

        [StringLength(1)]
        public string oppo_p6 { get; set; }

        [StringLength(1)]
        public string oppo_p7 { get; set; }

        [StringLength(1)]
        public string oppo_p8 { get; set; }

        [StringLength(1)]
        public string oppo_p9 { get; set; }

        [StringLength(1)]
        public string oppo_p10 { get; set; }

        [StringLength(1)]
        public string oppo_p11 { get; set; }

        [StringLength(1)]
        public string oppo_p12 { get; set; }

        [StringLength(1)]
        public string oppo_p13 { get; set; }

        [StringLength(1)]
        public string oppo_p14 { get; set; }

        [StringLength(1)]
        public string oppo_p16 { get; set; }

        [StringLength(1)]
        public string oppo_p15 { get; set; }

        [StringLength(1)]
        public string oppo_p17 { get; set; }

        [StringLength(1)]
        public string oppo_p18 { get; set; }

        [StringLength(1)]
        public string oppo_p20 { get; set; }

        [StringLength(1)]
        public string oppo_p19 { get; set; }

        [StringLength(1)]
        public string oppo_p21 { get; set; }

        [StringLength(1)]
        public string oppo_p22 { get; set; }

        [StringLength(1)]
        public string oppo_p23 { get; set; }

        [StringLength(1)]
        public string oppo_p24 { get; set; }

        [StringLength(1)]
        public string oppo_p25 { get; set; }

        [StringLength(1)]
        public string oppo_P26 { get; set; }

        [StringLength(1)]
        public string oppo_p27 { get; set; }

        public int? oppo_nombreopp { get; set; }

        public int? oppo_NbreBC { get; set; }

        public int? oppo_nombreopp1 { get; set; }

        public int? oppo_NbreBC1 { get; set; }

        [StringLength(1)]
        public string oppo_obfuscated { get; set; }

        [StringLength(40)]
        public string oppo_modecom { get; set; }

        [StringLength(255)]
        public string oppo_condemostd { get; set; }

        [StringLength(255)]
        public string oppo_condemoavc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? oppo_NbreSal { get; set; }

        public int? oppo_nbreuser { get; set; }

        [StringLength(255)]
        public string oppo_option { get; set; }

        [StringLength(40)]
        public string oppo_formation { get; set; }

        [StringLength(40)]
        public string oppo_typecon { get; set; }

        public int? Oppo_PROJETId { get; set; }

        [StringLength(40)]
        public string oppo_depart { get; set; }

        [StringLength(40)]
        public string oppo_fmfp { get; set; }
    }
}
