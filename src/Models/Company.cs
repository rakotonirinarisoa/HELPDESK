namespace Helpdesk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Company")]
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            Crmcli_Reg = new HashSet<Crmcli_Reg>();
        }

        [Key]
        public int Comp_CompanyId { get; set; }

        public int? Comp_PrimaryPersonId { get; set; }

        public int? Comp_PrimaryAddressId { get; set; }

        public int? Comp_PrimaryUserId { get; set; }

        [StringLength(69)]
        public string Comp_Name { get; set; }

        [StringLength(40)]
        public string Comp_Type { get; set; }

        [StringLength(40)]
        public string Comp_Status { get; set; }

        [StringLength(40)]
        public string Comp_Source { get; set; }

        [StringLength(40)]
        public string Comp_Territory { get; set; }

        [StringLength(40)]
        public string Comp_Revenue { get; set; }

        [StringLength(40)]
        public string Comp_Employees { get; set; }

        [StringLength(40)]
        public string Comp_Sector { get; set; }

        [StringLength(40)]
        public string Comp_IndCode { get; set; }

        [StringLength(100)]
        public string Comp_WebSite { get; set; }

        [StringLength(40)]
        public string Comp_MailRestriction { get; set; }

        public int? Comp_CreatedBy { get; set; }

        public DateTime? Comp_CreatedDate { get; set; }

        public int? Comp_UpdatedBy { get; set; }

        public DateTime? Comp_UpdatedDate { get; set; }

        public DateTime? Comp_TimeStamp { get; set; }

        public byte? Comp_Deleted { get; set; }

        [StringLength(255)]
        public string Comp_LibraryDir { get; set; }

        public int? Comp_ChannelID { get; set; }

        public int? Comp_SecTerr { get; set; }

        public int? Comp_WorkflowId { get; set; }

        public DateTime? Comp_UploadDate { get; set; }

        public int? comp_SLAId { get; set; }

        public int? Comp_PrimaryAccountId { get; set; }

        [StringLength(255)]
        public string comp_intforeignid { get; set; }

        public int? comp_intid { get; set; }

        public DateTime? comp_intlastsyncdate { get; set; }

        [StringLength(1)]
        public string comp_promote { get; set; }

        [StringLength(1)]
        public string Comp_OptOut { get; set; }

        [StringLength(20)]
        public string comp_codecomptable { get; set; }

        [StringLength(20)]
        public string comp_naf { get; set; }

        [StringLength(20)]
        public string comp_siret { get; set; }

        [StringLength(25)]
        public string comp_reference { get; set; }

        [StringLength(20)]
        public string comp_nomabrege { get; set; }

        [StringLength(15)]
        public string comp_sousactivite { get; set; }

        public string comp_description { get; set; }

        public string comp_itineraire { get; set; }

        public int? comp_DefaultIntId { get; set; }

        public int? comp_currencyid { get; set; }

        [StringLength(40)]
        public string comp_typetax { get; set; }

        [StringLength(40)]
        public string comp_compfamilyid { get; set; }

        [StringLength(1)]
        public string comp_gdalangue { get; set; }

        [StringLength(40)]
        public string comp_TalendExterKey { get; set; }

        public DateTime? comp_ssdi_dateex { get; set; }

        [StringLength(1)]
        public string comp_ssdi_public { get; set; }

        public int? comp_ssdi_dependsociete { get; set; }

        [StringLength(2)]
        public string comp_ssdi_motifFermeture { get; set; }

        [StringLength(1)]
        public string comp_ssdi_logo { get; set; }

        public string comp_ssdi_cds { get; set; }

        [StringLength(10)]
        public string comp_ssdi_codesage { get; set; }

        public int? comp_representantid { get; set; }

        [StringLength(13)]
        public string comp_numprinc_repli { get; set; }

        public string comp_condpaiement_repli { get; set; }

        [StringLength(35)]
        public string comp_coderisque_repli { get; set; }

        [StringLength(1)]
        public string comp_actionrisque_repli { get; set; }

        public int? comp_oldcompanyid { get; set; }

        [StringLength(69)]
        public string comp_carte_prof_iltx { get; set; }

        [StringLength(6)]
        public string comp_classeur_n_iltx { get; set; }

        [StringLength(69)]
        public string comp_code_client_sage_iltx { get; set; }

        [StringLength(35)]
        public string comp_etat_du_client_iltb { get; set; }

        [StringLength(25)]
        public string comp_nif_iltx { get; set; }

        [StringLength(69)]
        public string comp_quit_iltx { get; set; }

        [StringLength(25)]
        public string comp_rc_iltx { get; set; }

        [StringLength(30)]
        public string comp_stat_iltx { get; set; }

        public int? comp_accgenerique { get; set; }

        [StringLength(17)]
        public string comp_numpayeur_repli { get; set; }

        public string comp_Activite { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comp_ssdi_satisfaction { get; set; }

        [StringLength(1)]
        public string comp_ssdi_vip { get; set; }

        [StringLength(10)]
        public string comp_ssdi_vvdiagid { get; set; }

        [StringLength(10)]
        public string comp_ssdi_vvcompid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comp_ssdi_solde { get; set; }

        public byte? comp_ssdi_solde_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comp_ssdi_encaisse { get; set; }

        public byte? comp_ssdi_encaisse_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comp_ssdi_encours1 { get; set; }

        public byte? comp_ssdi_encours1_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comp_ssdi_encours2 { get; set; }

        public byte? comp_ssdi_encours2_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comp_ssdi_encours3 { get; set; }

        public byte? comp_ssdi_encours3_CID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comp_ssdi_BLFA { get; set; }

        public byte? comp_ssdi_blfa_CID { get; set; }

        public DateTime? comp_ssdi_dernierreglement { get; set; }

        public DateTime? comp_ssdi_dernierefact { get; set; }

        [StringLength(1)]
        public string comp_ssdi_bloque { get; set; }

        public int? comp_primaryaddressSalesId { get; set; }

        [StringLength(40)]
        public string comp_categorie { get; set; }

        [StringLength(255)]
        public string comp_bdf { get; set; }

        [StringLength(50)]
        public string comp_entsource { get; set; }

        [StringLength(20)]
        public string comp_filiale { get; set; }

        [StringLength(20)]
        public string comp_autresousact { get; set; }

        [StringLength(20)]
        public string comp_autresector { get; set; }

        [StringLength(40)]
        public string comp_listetypesoc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? comp_capitalsoc { get; set; }

        [StringLength(20)]
        public string comp_socmer { get; set; }

        public string comp_fil { get; set; }

        public int? comp_breag { get; set; }

        [StringLength(1)]
        public string comp_prjenc { get; set; }

        [StringLength(25)]
        public string comp_dureptj { get; set; }

        public string comp_actproj { get; set; }

        [StringLength(255)]
        public string Comp_IntegratedSystems { get; set; }

        [StringLength(1)]
        public string comp_obfuscated { get; set; }

        [StringLength(35)]
        public string comp_bailleur_1_ilst { get; set; }

        [StringLength(35)]
        public string comp_bailleur_2_ilst { get; set; }

        [StringLength(35)]
        public string comp_bailleur_3_ilst { get; set; }

        [StringLength(35)]
        public string comp_bailleur_ilst { get; set; }

        [StringLength(69)]
        public string comp_code_client_editeur_iltx { get; set; }

        [StringLength(69)]
        public string comp_raison_sociale_af_ay_iltx { get; set; }

        [StringLength(35)]
        public string comp_secteur_d_activite_ilst { get; set; }

        [StringLength(35)]
        public string comp_taille_de_l_entre_bd_ilst { get; set; }

        public string comp_maildest { get; set; }

        [StringLength(8)]
        public string comp_swc_corres_code_iltx { get; set; }

        [StringLength(50)]
        public string comp_corespondance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Crmcli_Reg> Crmcli_Reg { get; set; }
    }
}
