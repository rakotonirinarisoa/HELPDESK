namespace Helpdesk.Models
{
    public class Cr
    {
        public string IdCases { get; set; }
        //public string IdTaches { get; set; }
        //public string IdPlannings { get; set; }

        public string RefTicket { get; set; }
        public string Module { get; set; }
        public string Type { get; set; }
        public string DateIntervention { get; set; }
        public string Hdeb { get; set; }
        public string Hfin { get; set; }
        public string Lieu { get; set; }
        public string Client { get; set; }
        public string Sujet { get; set; }

        public string DESTMAIL { get; set; }
        public string DESTCLT { get; set; }

        public string DESCPROBLEME { get; set; }

        public string DocRem { get; set; }
        public string DocAnnex { get; set; }

        public string Attachement { get; set; }

        public string CONSULTINTERV { get; set; }
        
    }
}
