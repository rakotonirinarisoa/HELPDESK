namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Crmcli_Signatures
    {
        public int ID { get; set; }

        public int? ID_Company { get; set; }

        public int? ID_Person1 { get; set; }

        public int? ID_Person2 { get; set; }

        public int? ID_Person3 { get; set; }

        public int? ID_Person4 { get; set; }

        [Column(TypeName = "image")]
        public byte[] Sign1 { get; set; }

        [Column(TypeName = "image")]
        public byte[] Sign2 { get; set; }

        [Column(TypeName = "image")]
        public byte[] Sign3 { get; set; }

        [Column(TypeName = "image")]
        public byte[] Sing4 { get; set; }
    }
}
