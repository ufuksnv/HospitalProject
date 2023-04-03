using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Service.OptionsModels
{
    public class EmailSettings
    {
        public string? Host { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
