using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenDB.Entity
{
    public class Citizen
    {
        public string citizenName { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }

        public DescriptorsEntity descriptors { get; set; }

        public string blood { get; set; }

        public int home { get; set; }
        public int job { get; set; }
    }
}
