namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Comm_Link
    {
        [Key]
        public int CmLi_CommLinkId { get; set; }

        public int? CmLi_Comm_UserId { get; set; }

        public int CmLi_Comm_CommunicationId { get; set; }

        public int? CmLi_CreatedBy { get; set; }

        public DateTime? CmLi_CreatedDate { get; set; }

        public int? CmLi_UpdatedBy { get; set; }

        public DateTime? CmLi_UpdatedDate { get; set; }

        public DateTime? CmLi_TimeStamp { get; set; }

        public byte? CmLi_Deleted { get; set; }

        public int? CmLi_Comm_PersonId { get; set; }

        public int? CmLi_Comm_CompanyId { get; set; }

        public DateTime? CmLi_Comm_NotifyTime { get; set; }

        [StringLength(1)]
        public string CmLi_Comm_InitialWave { get; set; }

        [StringLength(40)]
        public string CmLi_Comm_WaveResponse { get; set; }

        public int? CmLi_Comm_LeadID { get; set; }

        [StringLength(2)]
        public string Cmli_SMSMessageSent { get; set; }

        [StringLength(1)]
        public string Cmli_MassEmailSent { get; set; }

        public int? cmli_status { get; set; }

        [StringLength(255)]
        public string cmli_recipient { get; set; }

        [StringLength(1)]
        public string cmli_reminder { get; set; }

        public int? CmLi_Comm_AccountId { get; set; }

        public int? CmLi_Comm_QuoteId { get; set; }

        public int? CmLi_Comm_OrderId { get; set; }

        [StringLength(1)]
        public string CmLi_IsExternalAttendee { get; set; }

        public int? CmLi_ExternalPersonID { get; set; }

        public int? CmLi_EntityId { get; set; }

        public int? CmLi_RecordId { get; set; }

        [StringLength(40)]
        public string cmli_TalendExterKey { get; set; }

        [StringLength(2)]
        public string CmLi_ssdi_NotifMobileStatut { get; set; }
    }
}
