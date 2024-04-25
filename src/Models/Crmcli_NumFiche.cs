namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcli_NumFiche
    {
        public int ID { get; set; }

        [StringLength(5)]
        public string NumFiche { get; set; }
    }
}
