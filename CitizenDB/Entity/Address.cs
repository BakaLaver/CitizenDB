using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenDB.Entity
{
    public class Address
    {
        public string name {  get; set; }
        public List<Room> rooms { get; set; }
        public Сompany? company { get; set; }
    }
}
