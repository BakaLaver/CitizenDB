using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenDB.Entity.Dictionery
{
    public static class CitizenMappingDictionaries
    {
            public static readonly Dictionary<int, string> HairTypeLabels = new Dictionary<int, string>
            {
                { 0, "A bald head" },
                { 1, "Short" },
                { 2, "Long" }
            };

            public static readonly Dictionary<int, string> HairColourLabels = new Dictionary<int, string>
            {
                { 0, "Black" },
                { 1, "Brown" },
                { 2, "Blonde" },
                { 3, "Ginger" },
                { 4, "Red" },
                { 5, "Blue" },
                { 6, "Green" },
                { 7, "Purple" },
                { 8, "Pink" },
                { 9, "Grey" },
                { 10, "White" }
            };

            public static readonly Dictionary<int, string> EyeColourLabels = new Dictionary<int, string>
            {
                { 0, "Blue" },
                { 1, "Brown" },
                { 2, "Green" },
                { 3, "Grey" }
            };

            public static readonly Dictionary<int, string> FacialFeatureLabels = new Dictionary<int, string>
            {
                { 0, "Scarring" },
                { 1, "Beard" },
                { 2, "Moustache" },
                { 3, "Piercing" },
                { 4, "Tattoo" },
                { 5, "Glasses" },
                { 6, "Mole" }
            };

            public static readonly Dictionary<int, string> BloodTypeLabels = new Dictionary<int, string>
            {
                { 1, "A+" },
                { 2, "A-" },
                { 3, "B+" },
                { 4, "B-" },
                { 5, "O+" },
                { 6, "O-" },
                { 7, "AB+" },
                { 8, "AB-" }
            };

            public static readonly Dictionary<int, string> HeightLabels = new Dictionary<int, string>
            {
                { 0, "Very short" },
                { 1, "Short" },
                { 2, "Average" },
                { 3, "Tall" },
                { 4, "Very tall" }
            };

            public static readonly Dictionary<int, string> BuildLabels = new Dictionary<int, string>
            {
                { 0, "Skinny" },
                { 1, "Average" },
                { 2, "Overweight" },
                { 3, "Muscular" }
            };

            public static readonly Dictionary<int, string> GenderLabels = new Dictionary<int, string>
            {
                { 0, "Male" },
                { 1, "Female" },
                { 2, "Non-binary" }
            };
    }
}
