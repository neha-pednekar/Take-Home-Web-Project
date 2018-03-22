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

        public string OutputDecodedMessage { get; set; }

        public string MonogramWithMaxFrequency { get; set; }

        public string BigramWithMaxFrequency { get; set; }

        public string TrigramWithMaxFrequency { get; set; }
    }

    public enum StrategyTypes
    {
        Monogram,
        Bigram,
        Trigram
    }

}
