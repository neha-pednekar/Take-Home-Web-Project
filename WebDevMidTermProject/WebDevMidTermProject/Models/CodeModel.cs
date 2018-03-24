using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebDevMidTermProject.Models
{
    public class CodeModel
    {
        [Display(Name = "Enter a secret message to decode")]
        public string InputSecretMessage { get; set; }

        public StrategyModel StrategyModel { get; set; }

        [Display(Name = "Output Decoded Message")]
        public string OutputDecodedMessage { get; set; }

        public string MonogramWithMaxFrequency { get; set; }

        public string BigramWithMaxFrequency { get; set; }

        public string TrigramWithMaxFrequency { get; set; }

        public string MonogramsAlreadyOccupied { get; set; }

        public string BigramsAlreadyOccupied { get; set; }

        public string TrigramsAlreadyOccupied { get; set; }

        public List<MonogramDataModel> MonogramDataModels { get; set; }

        public List<BigramDataModel> BigramDataModels { get; set; }

        public List<TrigramDataModel> TrigramDataModels { get; set; }
    }

    [Flags]
    public enum StrategyTypes
    {
        [Display(Name = "Monogram")]
        Monogram = 0,
        [Display(Name = "Bigram(Monogram and Bigram Both Covered)")]
        Bigram = 1,
        [Display(Name = "Trigram(Monogram and Trigram Both Covered)")]
        Trigram = 2
    }

}
