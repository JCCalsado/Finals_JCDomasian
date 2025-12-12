using System;

namespace Finals_JCDomasian.Models
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