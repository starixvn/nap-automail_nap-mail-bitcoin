using System;

namespace Fanex.Bitcoin.Business.Models
{
    public class MailModel
    {
        public DateTime Yesterday { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FileName { get; set; }
    }
}