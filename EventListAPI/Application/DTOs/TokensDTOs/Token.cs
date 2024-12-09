using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TokensDTOs
{
    public class Token
    {
        public string Value { get; set; }

        public DateTime Expires { get; set; }
    }
}
