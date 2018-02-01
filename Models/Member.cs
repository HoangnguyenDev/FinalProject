using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ID { get; set; }
        public bool IsDeleted { get; set; }
        public List<GoLeave> ListHistory { get; set; }
        public string FirstMidName { get; set; }
        public string LastName { get; set; }
        public long? ImageID { get; set; }
        public University UniversityID { get; set; }
        public int StudentID { get; set; }
        public Sex Sex { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string MobilePhone { get; set; }
        public DateTime? DateIdentityCard { get; set; }
        public string WhereIdentityCard { get; set; }
        public string Address { get; set; }
        public DateTime? DateofBirth { get; set; }
        public ContactStatus Status { get; set; }
        public DateTime? CreateDT { get; set; }
        public DateTime? UpdateDT { get; set; }
    }
    public enum ContactStatus
    {
        Submitted,
        Approved,
        Rejected
    }
    public enum Sex
    {
        Male,
        Female,
    }
    public enum University
    {
        CNTT,
        NV,
    }
}
