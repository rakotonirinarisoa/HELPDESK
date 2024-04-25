namespace Helpdesk
{
    using System.ComponentModel.DataAnnotations;

    public partial class Crmcli_Theme
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Theme { get; set; }
    }
}
