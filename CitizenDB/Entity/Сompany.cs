using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenDB.Entity
{
    public class Сompany
    {
        public int id { get; set; }
        public string name {  get; set; }
        public List<СompanyRoster> companyRoster {  get; set; }
    }
}
