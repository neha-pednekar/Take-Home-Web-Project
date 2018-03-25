using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebDevMidTermProject.Custom_Validations;
using static WebDevMidTermProject.Models.StrategyManager;

namespace WebDevMidTermProject.Models
{
    public class StrategyModel
    {
        [ValidateStrategyTypeDropdown]
        [Display(Name = "Strategy Type")]
        public string StrategyType { get; set; }

        public Dictionary<string, double> FrequencyGramMapping { get; set; }

    }
}
