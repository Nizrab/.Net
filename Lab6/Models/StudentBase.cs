using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Models
{
    public class StudentBase
    {
        [StringLength(50, MinimumLength = 1)]
        public String FirstName{
            get;
            set;
        }
        [StringLength(50, MinimumLength = 1)]
        public String LastName{
            get;
            set;
        }
        [StringLength(50, MinimumLength = 1)]
        public String Program{
            get;
            set;
        }
    }
}
