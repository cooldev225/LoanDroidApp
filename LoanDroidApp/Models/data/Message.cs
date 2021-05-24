using Models.data;
using Models.data.Interfaces;
using System;
namespace Models.data
{
    public class Message : IAuditableEntity
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set;}
        public string Question { get; set;}
        public string QuestionAttach { get; set; }
        public string Answer { get; set; }
        public string AnswerAttach { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDevice { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDevice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}