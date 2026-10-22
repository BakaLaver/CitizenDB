using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenDB.Entity
{
    public class Floor
    {
        public string name { get; set; }

        public List<Address> addresses { get; set; }
    }
}
