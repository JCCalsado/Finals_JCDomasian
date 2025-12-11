using System;

namespace FinalsTP_YourLastName.Models
{
    public class Participant
    {
        public int ParticipantID { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string BibNumber { get; set; }
        public int TypeID { get; set; }
        public string PaymentStatus { get; set; }
    }
}