using CitizenDB.Entity.Dictionery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenDB.Entity
{
    public class CitizenExcel
    {
        public string citizenName { get; set; }
        public string birthday { get; set; }
        public string gender { get; set; }
        public string blood { get; set; }
        public string build { get; set; }
        public double heightCM { get; set; }
        public int shoeSize { get; set; }
        public string hairType { get; set; }
        public string eyeColour { get; set; }
        public string Address { get; set; }
        public string workPlace { get; set; }


        public static CitizenExcel MappingToExcel(Citizen citizen, List<CityTiles> buildings) 
        {
            CitizenExcel citizenExcel = new CitizenExcel
            {
                citizenName = citizen.citizenName,
                birthday = citizen.birthday,
                gender = ConvertToString(citizen.gender, CitizenMappingDictionaries.GenderLabels),
                blood = ConvertToString(citizen.blood, CitizenMappingDictionaries.BloodTypeLabels),
                build = ConvertToString(citizen.descriptors.build.ToString(), CitizenMappingDictionaries.BuildLabels),
                heightCM = citizen.descriptors.heightCM,
                shoeSize = citizen.descriptors.shoeSize,
                hairType = ConvertToString(citizen.descriptors.hairType.ToString(), CitizenMappingDictionaries.HairTypeLabels),
                eyeColour = ConvertToString(citizen.descriptors.eyeColour.ToString(), CitizenMappingDictionaries.EyeColourLabels)
            };

            citizenExcel.Address = buildings
                                   .SelectMany(t => t.building?.floors ?? new List<Floor>())
                                   .SelectMany(f => f.addresses ?? new List<Address>())
                                   .SelectMany(a => a.rooms ?? new List<Room>())
                                   .Where(room => room.id == citizen.home)
                                   .Select(room => room.name)
                                   .FirstOrDefault();

            var address = buildings
                          .SelectMany(t => t.building?.floors ?? new List<Floor>())
                          .SelectMany(f => f.addresses ?? new List<Address>())
                          .FirstOrDefault(a => a.company?.companyRoster != null &&
                                               a.company.companyRoster.Any(r => r.id == citizen.job));

            citizenExcel.workPlace = address?.name;

            return citizenExcel;
        }

        private static string ConvertToString(string value, Dictionary<int, string> dictionary)
        {
            if (int.TryParse(value, out int intValue))
            {
                return dictionary.GetValueOrDefault(intValue, "Unknown");
            }
            return "Unknown";
        }
    }
}
