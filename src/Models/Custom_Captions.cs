namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Custom_Captions
    {
        [Key]
        public int Capt_CaptionId { get; set; }

        [StringLength(20)]
        public string Capt_FamilyType { get; set; }

        [StringLength(30)]
        public string Capt_Family { get; set; }

        [StringLength(80)]
        public string Capt_Code { get; set; }

        public string Capt_US { get; set; }

        public string Capt_UK { get; set; }

        public string Capt_FR { get; set; }

        public string Capt_DE { get; set; }

        public string Capt_ES { get; set; }

        public int? Capt_Order { get; set; }

        [StringLength(1)]
        public string Capt_System { get; set; }

        public int? Capt_CreatedBy { get; set; }

        public DateTime? Capt_CreatedDate { get; set; }

        public int? Capt_UpdatedBy { get; set; }

        public DateTime? Capt_UpdatedDate { get; set; }

        public DateTime? Capt_TimeStamp { get; set; }

        public int? Capt_Deleted { get; set; }

        [StringLength(255)]
        public string Capt_Context { get; set; }

        public string Capt_DU { get; set; }

        public string Capt_JP { get; set; }

        [StringLength(20)]
        public string Capt_Component { get; set; }

        public int? capt_deviceid { get; set; }

        public int? Capt_IntegrationId { get; set; }

        public string Capt_CS { get; set; }
    }
}
