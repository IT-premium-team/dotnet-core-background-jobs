using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundTest.Data.Entities
{
    public class ManagerChange
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int OldManagerId { get; set; }
        public int NewManagerId { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfChange { get; set; }
        public bool IsChanged { get; set; }
    }
}
