namespace Helpdesk
{
    public partial class pro_user
    {
        public int Id { get; set; }

        public byte[] photo { get; set; }

        public int? user_Id { get; set; }

        public string Email { get; set; }

        public string PasswordMail { get; set; }

        public int? Departement { get; set; }

        public string Destinataire { get; set; }

        public string Mail { get; set; }

        public string CopieMail { get; set; }

        public int? Profil { get; set; }
    }
}
