using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDevMidTermProject.Models
{
    public class TrigramDataModel
    {
        public string TrigramSequence { get; set; }

        public int NumberOfOccurences { get; set; }

        public bool Occupied { get; set; }

        public double Percentage { get; set; }
    }
}
