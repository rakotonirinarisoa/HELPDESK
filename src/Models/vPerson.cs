namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vPerson")]
    public partial class vPerson
    {
        [StringLength(48)]
        public string phon_MobileFullNumber { get; set; }

        [StringLength(255)]
        public string Pers_EmailAddress { get; set; }

        [StringLength(5)]
        public string Pers_PhoneCountryCode { get; set; }

        [StringLength(20)]
        public string Pers_PhoneAreaCode { get; set; }

        [StringLength(21)]
        public string Pers_PhoneNumber { get; set; }

        [StringLength(5)]
        public string Pers_FaxCountryCode { get; set; }

        [StringLength(20)]
        public string Pers_FaxAreaCode { get; set; }

        [StringLength(21)]
        public string Pers_FaxNumber { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Pers_PersonId { get; set; }

        public int? Pers_CompanyId { get; set; }

        public int? Pers_PrimaryAddressId { get; set; }

        public int? Pers_PrimaryUserId { get; set; }

        [StringLength(10)]
        public string Pers_Salutation { get; set; }

        [StringLength(35)]
        public string Pers_FirstName { get; set; }

        [StringLength(40)]
        public string Pers_LastName { get; set; }

        [StringLength(30)]
        public string Pers_MiddleName { get; set; }

        [StringLength(20)]
        public string Pers_Suffix { get; set; }

        [StringLength(6)]
        public string Pers_Gender { get; set; }

        [StringLength(50)]
        public string Pers_Title { get; set; }

        [StringLength(40)]
        public string Pers_TitleCode { get; set; }

        [StringLength(35)]
        public string Pers_Department { get; set; }

        [StringLength(40)]
        public string Pers_Status { get; set; }

        [StringLength(40)]
        public string Pers_Source { get; set; }

        [StringLength(40)]
        public string Pers_Territory { get; set; }

        [StringLength(40)]
        public string Pers_WebSite { get; set; }

        [StringLength(40)]
        public string Pers_MailRestriction { get; set; }

        public int? Pers_CreatedBy { get; set; }

        public DateTime? Pers_CreatedDate { get; set; }

        public int? Pers_UpdatedBy { get; set; }

        public DateTime? Pers_UpdatedDate { get; set; }

        public DateTime? Pers_TimeStamp { get; set; }

        public byte? Pers_Deleted { get; set; }

        [StringLength(255)]
        public string Pers_LibraryDir { get; set; }

        public int? Pers_ChannelID { get; set; }

        public DateTime? Pers_UploadDate { get; set; }

        public int? pers_SecTerr { get; set; }

        public int? Pers_WorkflowId { get; set; }

        public int? Pers_AccountId { get; set; }

        [StringLength(255)]
        public string pers_intforeignid { get; set; }

        public int? pers_intid { get; set; }

        public DateTime? pers_intlastsyncdate { get; set; }

        [StringLength(1)]
        public string pers_promote { get; set; }

        public DateTime? pers_ConflictResDate { get; set; }

        [StringLength(30)]
        public string pers_departmentcode { get; set; }

        [StringLength(1)]
        public string Pers_OptOut { get; set; }

        [StringLength(40)]
        public string pers_TalendExterKey { get; set; }

        public int? pers_segmentid { get; set; }

        public string Pers_ssdi_cds { get; set; }

        public int? pers_addressIsSharedWithCompany { get; set; }

        [StringLength(1)]
        public string pers_ssdi_extranet { get; set; }

        public int? pers_ssdi_gdrid { get; set; }

        [StringLength(1)]
        public string pers_ssdi_estcontactn2 { get; set; }

        [StringLength(1000)]
        public string pers_ssdi_perimetren2 { get; set; }

        [StringLength(1)]
        public string pers_ssdi_anotifiern2 { get; set; }

        [StringLength(1)]
        public string pers_decideur { get; set; }

        [StringLength(255)]
        public string Pers_IntegratedSystems { get; set; }

        [StringLength(1)]
        public string pers_obfuscated { get; set; }
    }
}
