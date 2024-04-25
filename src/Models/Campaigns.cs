namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Campaigns
    {
        [Key]
        public int Camp_CampaignId { get; set; }

        [Required]
        [StringLength(100)]
        public string Camp_Name { get; set; }

        public DateTime? Camp_StartDate { get; set; }

        public DateTime? Camp_EndDate { get; set; }

        [StringLength(15)]
        public string Camp_Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Camp_Budget { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Camp_Cost { get; set; }

        [StringLength(40)]
        public string Camp_Type { get; set; }

        public int? Camp_AssignedUserId { get; set; }

        public int? Camp_CreatedBy { get; set; }

        public DateTime? Camp_CreatedDate { get; set; }

        public int? Camp_UpdatedBy { get; set; }

        public DateTime? Camp_UpdatedDate { get; set; }

        public DateTime? Camp_TimeStamp { get; set; }

        public byte? Camp_Deleted { get; set; }

        public int? Camp_ChannelID { get; set; }

        public int? Camp_WorkflowId { get; set; }

        public int? Camp_Budget_CID { get; set; }

        public int? Camp_Cost_CID { get; set; }

        [StringLength(40)]
        public string Camp_CampaignType { get; set; }

        public int? Camp_GroupId { get; set; }

        public DateTime? Camp_DripDate { get; set; }

        [StringLength(40)]
        public string Camp_GroupEmailField { get; set; }

        [StringLength(1)]
        public string Camp_Launched { get; set; }

        public int? Camp_EMarketingAccountId { get; set; }

        [StringLength(40)]
        public string Camp_SPStatusCode { get; set; }

        public string Camp_SPStatusMessage { get; set; }

        [StringLength(255)]
        public string camp_canacom { get; set; }
    }
}
