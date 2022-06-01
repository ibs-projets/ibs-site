using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siteibs.Models
{
    public class MailModel
    {
        public string To { get; set; }
        public List<string> CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Persiste { get; set; }
        public string Nom { get; set; }
    }
}
