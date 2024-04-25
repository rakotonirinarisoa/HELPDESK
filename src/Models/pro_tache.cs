namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class pro_tache
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public short? AllDays { get; set; }

        public string Subject { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public int? Label { get; set; }

        public int? ResourceID { get; set; }

        public int? status { get; set; }

        public int? EventType { get; set; }

        public int? AssignCustomerID { get; set; }

        public int? AssignEmployeeID { get; set; }

        [StringLength(50)]
        public string TypeO { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        public string TypeTask { get; set; }

        public int? TypeLieu { get; set; }

        public int? Label2 { get; set; }

        public string TypeTask2 { get; set; }

        public int? Absent { get; set; }
    }
}
