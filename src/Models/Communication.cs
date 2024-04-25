namespace Helpdesk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Communication")]
    public partial class Communication
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Communication()
        {
            //Crmcli_EnvoiMail = new HashSet<Crmcli_EnvoiMail>();
            Crmcli_SousTaches = new HashSet<Crmcli_SousTaches>();
        }

        [Key]
        public int Comm_CommunicationId { get; set; }

        public int? Comm_OpportunityId { get; set; }

        public int? Comm_CaseId { get; set; }

        public int? Comm_ChannelId { get; set; }

        [StringLength(40)]
        public string Comm_Type { get; set; }

        [StringLength(40)]
        public string Comm_Action { get; set; }

        [StringLength(40)]
        public string Comm_Status { get; set; }

        [StringLength(40)]
        public string Comm_Priority { get; set; }

        public DateTime? Comm_DateTime { get; set; }

        public DateTime? Comm_ToDateTime { get; set; }

        [StringLength(1)]
        public string Comm_Private { get; set; }

        [StringLength(40)]
        public string Comm_OutCome { get; set; }

        public string Comm_Note { get; set; }

        public string Comm_Email { get; set; }

        public int? Comm_CreatedBy { get; set; }

        public DateTime? Comm_CreatedDate { get; set; }

        public int? Comm_UpdatedBy { get; set; }

        public DateTime? Comm_UpdatedDate { get; set; }

        public DateTime? Comm_TimeStamp { get; set; }

        public byte? Comm_Deleted { get; set; }

        [StringLength(255)]
        public string Comm_DocDir { get; set; }

        [StringLength(255)]
        public string Comm_DocName { get; set; }

        public int? Comm_TargetListId { get; set; }

        public DateTime? Comm_NotifyTime { get; set; }

        public int? Comm_NotifyDelta { get; set; }

        public string Comm_Description { get; set; }

        [StringLength(5)]
        public string Comm_SMSMessageSent { get; set; }

        [StringLength(5)]
        public string Comm_SMSNotification { get; set; }

        public int? Comm_WaveItemId { get; set; }

        public int? Comm_RecurrenceId { get; set; }

        public int? Comm_LeadID { get; set; }

        public int? Comm_SecTerr { get; set; }

        public int? Comm_WorkflowId { get; set; }

        public int? comm_messageid { get; set; }

        public string Comm_From { get; set; }

        public string Comm_TO { get; set; }

        public string Comm_CC { get; set; }

        public string Comm_BCC { get; set; }

        public string Comm_ReplyTo { get; set; }

        public int? Comm_IsReplyToMsgId { get; set; }

        public int? Comm_SolutionId { get; set; }

        [StringLength(1)]
        public string Comm_IsHtml { get; set; }

        [StringLength(1)]
        public string Comm_HasAttachments { get; set; }

        [StringLength(1)]
        public string Comm_EmailLinksCreated { get; set; }

        public DateTime? comm_completedtime { get; set; }

        public int? comm_percentcomplete { get; set; }

        [StringLength(1)]
        public string comm_taskreminder { get; set; }

        [StringLength(1)]
        public string Comm_CRMOnly { get; set; }

        public DateTime? Comm_OriginalDateTime { get; set; }

        public DateTime? Comm_OriginalToDateTime { get; set; }

        [StringLength(1)]
        public string Comm_Exception { get; set; }

        [StringLength(384)]
        public string Comm_Organizer { get; set; }

        public int? Comm_OrderId { get; set; }

        public int? Comm_QuoteId { get; set; }

        [StringLength(30)]
        public string Comm_OutlookID { get; set; }

        [StringLength(500)]
        public string Comm_MeetingID { get; set; }

        [StringLength(1)]
        public string Comm_IsAllDayEvent { get; set; }

        [StringLength(255)]
        public string Comm_Subject { get; set; }

        [StringLength(255)]
        public string Comm_Location { get; set; }

        [StringLength(2)]
        public string Comm_IsStub { get; set; }

        [StringLength(40)]
        public string Comm_TalendExterKey { get; set; }

        public int? Comm_SSDI_BUSINESSId { get; set; }

        public int? Comm_SSDI_SERVICEId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_ssdi_temps { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_ssdi_deptemps { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_ssdi_valorisqte { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_ssdi_depvalorisqte { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_ssdi_valoriscalc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_ssdi_depcalc { get; set; }

        [StringLength(2)]
        public string comm_ssdi_slaid { get; set; }

        [StringLength(6)]
        public string comm_ssdi_slaseverity { get; set; }

        [StringLength(2)]
        public string comm_ssdi_valorismotif { get; set; }

        public int? comm_ssdi_adressesite { get; set; }

        [StringLength(4)]
        public string comm_ssdi_modeconso { get; set; }

        [StringLength(30)]
        public string comm_ssdi_compltypeaction { get; set; }

        public int? comm_ssdi_jaid { get; set; }

        public int? comm_ssdi_code { get; set; }

        [StringLength(1)]
        public string comm_ssdi_closeCase { get; set; }

        [StringLength(2)]
        public string comm_ssdi_domaine { get; set; }

        public int? Comm_ssdi_ciId { get; set; }

        [StringLength(1)]
        public string comm_emailsend1 { get; set; }

        [StringLength(1)]
        public string comm_emailsend2 { get; set; }

        public DateTime? comm_notifydate1 { get; set; }

        public string comm_planaction { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_dureeact { get; set; }

        [StringLength(40)]
        public string comm_moyenrep { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_ESSAITCOMPTE { get; set; }

        [StringLength(40)]
        public string comm_moyenreppl { get; set; }

        [StringLength(40)]
        public string Comm_MailChimpCampaignId { get; set; }

        public string comm_rapport { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_delai_traitement { get; set; }

        [StringLength(40)]
        public string comm_comissione { get; set; }

        [StringLength(2)]
        public string comm_ssdi_motifAnnulation { get; set; }

        [StringLength(2)]
        public string comm_ssdi_objectifAtteint { get; set; }

        [StringLength(2)]
        public string comm_ssdi_typeResolution { get; set; }

        [StringLength(2)]
        public string comm_ssdi_motifNonResolution { get; set; }

        [StringLength(1)]
        public string comm_ssdi_visibleExtranetSig { get; set; }

        [StringLength(1)]
        public string comm_ssdi_nonvisibltech { get; set; }

        public int? Comm_SSDI_GDRId { get; set; }

        [StringLength(50)]
        public string comm_ssdi_refescalade { get; set; }

        public int? Comm_ssdi_cn2Id { get; set; }

        public int? comm_ssdi_DepRefArt { get; set; }

        public int? comm_ssdi_MORefArt { get; set; }

        public int? comm_ssdi_productid { get; set; }

        public int? comm_ssdi_product2id { get; set; }

        public int? comm_ssdi_product3id { get; set; }

        [StringLength(8)]
        public string comm_ssdi_doPiece { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comm_delaitraitement { get; set; }

        [StringLength(40)]
        public string comm_retourcomm { get; set; }

        public DateTime? comm_datefin { get; set; }

        public DateTime? comm_dernaction { get; set; }

        [StringLength(1)]
        public string comm_sendconfirm { get; set; }

        [StringLength(1)]
        public string comm_sendreminder { get; set; }

        [StringLength(1)]
        public string comm_obfuscated { get; set; }

        public string comm_theme1 { get; set; }

        public string comm_theme2 { get; set; }

        public string comm_theme3 { get; set; }

        public string comm_theme4 { get; set; }

        public string comm_theme5 { get; set; }

        public string comm_obs1 { get; set; }

        public string comm_com1 { get; set; }

        public string comm_obs2 { get; set; }

        public string comm_obs3 { get; set; }

        public string comm_obs4 { get; set; }

        public string comm_obs5 { get; set; }

        public string comm_com2 { get; set; }

        public string comm_com3 { get; set; }

        public string comm_com4 { get; set; }

        public string comm_com5 { get; set; }

        public string comm_ordrej { get; set; }

        public DateTime? comm_datea1 { get; set; }

        public DateTime? comm_datea2 { get; set; }

        public DateTime? comm_datea3 { get; set; }

        public string comm_objt1 { get; set; }

        public string comm_objt2 { get; set; }

        public string comm_objt3 { get; set; }

        public DateTime? comm_ech1 { get; set; }

        public DateTime? comm_ech2 { get; set; }

        public DateTime? comm_ech3 { get; set; }

        public string comm_partsoft { get; set; }

        public string comm_partsoc { get; set; }

        public string comm_destsoft { get; set; }

        public string comm_destsoc { get; set; }

        [StringLength(50)]
        public string comm_lieu { get; set; }

        public string comm_interv1 { get; set; }

        public string comm_interv2 { get; set; }

        public string comm_interv3 { get; set; }

        [StringLength(40)]
        public string comm_case_depart { get; set; }

        [StringLength(40)]
        public string comm_phaseproj { get; set; }

        [StringLength(40)]
        public string comm_detailproj { get; set; }

        [StringLength(40)]
        public string comm_commision { get; set; }

        [StringLength(40)]
        public string comm_typepresta { get; set; }

        [StringLength(40)]
        public string comm_site { get; set; }

        [StringLength(40)]
        public string comm_nature { get; set; }

        [StringLength(40)]
        public string comm_typeass { get; set; }

        public int? Comm_PROJETId { get; set; }

        public int? comm_proj_name { get; set; }

        public string comm_probleme { get; set; }

        [StringLength(255)]
        public string comm_userintervenant { get; set; }

        [StringLength(40)]
        public string comm_acceshotline { get; set; }

        public int? comm_pause { get; set; }

        public DateTime? comm_datefin2 { get; set; }

        public int? comm_planid { get; set; }

        public int? Comm_PLANDACTIONId { get; set; }

        [StringLength(40)]
        public string comm_presence { get; set; }

        [StringLength(255)]
        public string comm_assistform { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Crmcli_EnvoiMail> Crmcli_EnvoiMail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Crmcli_SousTaches> Crmcli_SousTaches { get; set; }
    }
}
