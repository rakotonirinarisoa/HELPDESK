namespace Helpdesk.Models
{
    public class Mailing
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Sujet { get; set; }
        public string Body { get; set; }
        public string Attachement { get; set; }


        public string DateIntervention { get; set; }
        public string IdCases { get; set; }


        public bool TriggerOnLoad { get; set; }
        public string TriggerOnLoadMessage { get; set;  }
    }
}
