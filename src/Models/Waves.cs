namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Waves
    {
        [Key]
        public int Wave_WaveId { get; set; }

        public int Wave_CampaignId { get; set; }

        [Required]
        [StringLength(100)]
        public string Wave_Name { get; set; }

        public DateTime? Wave_StartDate { get; set; }

        public DateTime? Wave_EndDate { get; set; }

        [StringLength(15)]
        public string Wave_Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Wave_Budget { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Wave_Cost { get; set; }

        public int? Wave_AssignedUserId { get; set; }

        public int? Wave_CreatedBy { get; set; }

        public DateTime? Wave_CreatedDate { get; set; }

        public int? Wave_UpdatedBy { get; set; }

        public DateTime? Wave_UpdatedDate { get; set; }

        public DateTime? Wave_TimeStamp { get; set; }

        public byte? Wave_Deleted { get; set; }

        public int? Wave_ChannelID { get; set; }

        public int? Wave_WorkflowId { get; set; }

        public int? Wave_Budget_CID { get; set; }

        public int? Wave_Cost_CID { get; set; }
    }
}
