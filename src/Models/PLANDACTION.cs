namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PLANDACTION")]
    public partial class PLANDACTION
    {
        [Key]
        public int plan_PLANDACTIONid { get; set; }

        public int? plan_CreatedBy { get; set; }

        public DateTime? plan_CreatedDate { get; set; }

        public int? plan_UpdatedBy { get; set; }

        public DateTime? plan_UpdatedDate { get; set; }

        public DateTime? plan_TimeStamp { get; set; }

        public int? plan_Deleted { get; set; }

        public int? plan_WorkflowId { get; set; }

        public int? plan_Secterr { get; set; }

        public int? plan_UserId { get; set; }

        [StringLength(100)]
        public string plan_name { get; set; }

        public int? plan_ChannelId { get; set; }

        public int? plan_companyid { get; set; }

        [StringLength(40)]
        public string plan_Status { get; set; }

        [StringLength(40)]
        public string plan_TalendExterKey { get; set; }

        [StringLength(40)]
        public string plan_commission { get; set; }

        public DateTime? plan_dated { get; set; }

        public DateTime? plan_datef { get; set; }

        public int? plan_comm_communicationid { get; set; }

        public DateTime? plan_dateintv { get; set; }

        [StringLength(40)]
        public string plan_localisation { get; set; }

        public string plan_intervclient { get; set; }

        [StringLength(255)]
        public string plan_intervesoftw { get; set; }

        public string plan_plandaction { get; set; }

        [StringLength(40)]
        public string plan_site { get; set; }

        public int? plan_caseid { get; set; }

        [StringLength(40)]
        public string plan_typepresta { get; set; }
    }
}
