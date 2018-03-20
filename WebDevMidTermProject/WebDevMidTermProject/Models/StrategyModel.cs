using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebDevMidTermProject.Models.StrategyManager;

namespace WebDevMidTermProject.Models
{
    public class StrategyModel
    {
        public string StrategyType { get; set; }

        public Dictionary<string, double> FrequencyGramMapping { get; set; }

    }
}
