using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public class GoLeave
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ImageID { get; set; }
        public string GoOcg { get; set; }
        public string GoFull { get; set; }
        public string GoAvatar { get; set; }
        public string LeaveOcg { get; set; }
        public string LeaveFull { get; set; }
        public string leaveAvatar { get; set; }
        public string OCR { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? GoDT { get; set; }
        public DateTime? LeaveDT { get; set; }
        public string OwnerID { get; set; }
        public Member Owner { get; set; }
    }
}
