namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class WaveItems
    {
        [Key]
        public int WaIt_WaveItemId { get; set; }

        public int WaIt_WaveId { get; set; }

        [Required]
        [StringLength(100)]
        public string WaIt_Name { get; set; }

        [StringLength(40)]
        public string WaIt_Type { get; set; }

        public string WaIt_Details { get; set; }

        public DateTime? WaIt_StartDate { get; set; }

        public DateTime? WaIt_EndDate { get; set; }

        [StringLength(15)]
        public string WaIt_Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WaIt_Budget { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WaIt_Cost { get; set; }

        public int? WaIt_TargetList { get; set; }

        public int? WaIt_AssignedUserId { get; set; }

        public int? WaIt_ChannelId { get; set; }

        public int? WaIt_CreatedBy { get; set; }

        public DateTime? WaIt_CreatedDate { get; set; }

        public int? WaIt_UpdatedBy { get; set; }

        public DateTime? WaIt_UpdatedDate { get; set; }

        public DateTime? WaIt_TimeStamp { get; set; }

        public byte? WaIt_Deleted { get; set; }

        public int? WaIt_WorkflowId { get; set; }

        public int? WaIt_Budget_CID { get; set; }

        public int? WaIt_Cost_CID { get; set; }

        public int? wait_ddentrypoint { get; set; }

        public int? wait_DailyCalls { get; set; }

        public int? wait_channel { get; set; }

        public string wait_introtext { get; set; }

        public string wait_contactedtext { get; set; }

        public string wait_commdetails { get; set; }

        [StringLength(80)]
        public string wait_commpriority { get; set; }

        [StringLength(40)]
        public string wait_commsecterr { get; set; }

        public int? wait_commchannel { get; set; }

        [StringLength(255)]
        public string WaIt_CommSubject { get; set; }
    }
}
