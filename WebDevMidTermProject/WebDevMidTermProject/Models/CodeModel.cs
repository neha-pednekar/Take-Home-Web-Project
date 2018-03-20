using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDevMidTermProject.Models
{
    public class CodeModel
    {
        public string InputSecretMessage { get; set; }

        public StrategyModel StrategyModel { get; set; }
    }

    public enum StrategyTypes
    {
        Monogram,
        Bigram,
        Trigram
    }

}
