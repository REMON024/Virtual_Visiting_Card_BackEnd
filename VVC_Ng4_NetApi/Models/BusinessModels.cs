using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VVC_Ng4_NetApi.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Field1 { get; set; }
        public string Field1Placeholder { get; set; }
        public string Field2 { get; set; }
        public string Field2Placeholder { get; set; }
        public string Field3 { get; set; }
        public string Field3Placeholder { get; set; }
        public string Field4 { get; set; }
        public string Field4Placeholder { get; set; }
        public string Field5 { get; set; }
        public string Field5Placeholder { get; set; }
        public string Field6 { get; set; }
        public string Field6Placeholder { get; set; }
        public string Field7 { get; set; }
        public string Field7Placeholder { get; set; }
        

        public ApplicationUser Admin { get; set; }
        public string AdminId { get; set; }

        public virtual List<Card> Cards { get; set; }
    }


    public class Card
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public bool RequestAccept { get; set; }

        public Organization Organization { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }

        public virtual ApplicationUser User { get; set; }

    }

    public class Wallet
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }

        public virtual ApplicationUser Card { get; set; }
        public string CardId { get; set; }

    }

    public class CardRequest
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser FromUser { get; set; }
        public string FromUserId { get; set; }


        public virtual ApplicationUser ToUser { get; set; }
        public string ToUserId { get; set; }


        public TimeSpan Time { get; set; }

        public DateTime Date { get; set; }

        public bool RequestAccept { get; set; }
    }
}