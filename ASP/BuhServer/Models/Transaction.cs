using System;

namespace BuhServer.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public bool Plus { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}
