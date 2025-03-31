using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Model
{
    public class LookUpQuestion
    {
        public int Id { get; set; } // Primary key, auto-increment (IDENTITY)
        public int Order { get; set; } // Represents the [order] column
        public string Content { get; set; } // Represents the Content column
        public bool IsActive { get; set; } // Represents the ISActive column
    }
}
