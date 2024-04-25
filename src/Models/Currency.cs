namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Currency")]
    public partial class Currency
    {
        [Key]
        public int Curr_CurrencyID { get; set; }

        [Required]
        [StringLength(4)]
        public string Curr_Symbol { get; set; }

        public int Curr_DecimalPrecision { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Curr_Rate { get; set; }

        public int? Curr_CreatedBy { get; set; }

        public DateTime? Curr_CreatedDate { get; set; }

        public int? Curr_UpdatedBy { get; set; }

        public DateTime? Curr_UpdatedDate { get; set; }

        public DateTime? Curr_TimeStamp { get; set; }

        public byte? Curr_Deleted { get; set; }
    }
}
