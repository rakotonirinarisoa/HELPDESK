namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class pro_planning
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? AllDays { get; set; }

        public string Subject { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public int? Label { get; set; }

        public int? ResourceID { get; set; }

        public int? ResourceIDs { get; set; }

        public string ReminderInfo { get; set; }

        public string RecurrenceInfo { get; set; }

        public int? typeHoraire { get; set; }

        public int? status { get; set; }

        public int? EventType { get; set; }

        public int? AssignCustomerID { get; set; }

        public int? AssignEmployeeID { get; set; }

        [StringLength(50)]
        public string TypeO { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        [StringLength(50)]
        public string TypeTask { get; set; }

        public int? TypeLieu { get; set; }

        public int? Absent { get; set; }
    }
}
