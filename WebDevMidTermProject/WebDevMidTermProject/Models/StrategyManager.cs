using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDevMidTermProject.Models
{
    public class StrategyManager
    {
        readonly List<StrategyModel> _strategies = new List<StrategyModel>()
        {
            new StrategyModel {
                StrategyType = "Monogram",
                FrequencyGramMapping = new Dictionary<string, double>(){
                    { "a", 8.167 },{ "b", 1.492 },{ "c", 2.782 },{ "d", 4.253 },
                    { "e", 12.702 },{ "f", 2.228 },{ "g", 2.015 },{ "h", 6.094 },
                    { "i", 6.966 },{ "j", 0.153 },{ "k", 0.772 },{ "l", 4.025 },
                    { "m", 2.406 },{ "n", 6.749 },{ "o", 7.507 },{ "p", 1.929 },
                    { "q", 0.095 },{ "r", 5.987 },{ "s", 6.327 },{ "t", 9.056 },
                    { "u", 2.758 },{ "v", 0.978 },{ "w", 2.360 },{ "x", 0.150 },
                    { "y", 1.974 },{ "z", 0.074 } }
            },

            new StrategyModel {
                StrategyType = "Bigram",
                FrequencyGramMapping = new Dictionary<string, double>(){
                    { "th", 1.52 },{ "en", 0.55 },{ "ng", 0.18 },
                    { "he", 1.28 },{ "ed", 0.53 },{ "of", 0.16 },
                    { "in", 0.94 },{ "to", 0.52 },{ "al", 0.09 },
                    { "er", 0.94 },{ "it", 0.50 },{ "de", 0.09 },
                    { "an", 0.82 },{ "ou", 0.50 },{ "se", 0.08 },
                    { "re", 0.68 },{ "ea", 0.47 },{ "le", 0.08 },
                    { "nd", 0.63 },{ "hi", 0.46 },{ "sa", 0.06 },
                    { "at", 0.59 },{ "is", 0.46 },{ "si", 0.05 },
                    { "on", 0.57 },{ "or", 0.43 },{ "ar", 0.04 },
                    { "nt", 0.56 },{ "ti", 0.34 },{ "ve", 0.04 },
                    { "ha", 0.56 },{ "as", 0.33 },{ "ra", 0.04 },
                    { "es", 0.56 },{ "te", 0.27 },{ "ld", 0.02 },
                    { "st", 0.55 },{ "et", 0.19 },{ "ur", 0.02 }}
            },

            new StrategyModel {
                StrategyType = "Trigram",
                FrequencyGramMapping = new Dictionary<string, double>(){
                    { "the", 1 },{ "and", 2 },{ "tha", 3 }, { "ent", 4},
                    { "ing", 5 },{ "ion", 6 },{ "tio", 7 }, { "for", 8},
                    { "nde", 9 },{ "has", 10 },{ "nce", 11 }, { "edt", 12},
                    { "tis", 13 },{ "oft", 14 },{ "sth", 15 }, { "men", 16}
                    }
            },
            new StrategyModel
            {
                StrategyType = "ParagraphWithSpacing",
                FrequencyGramMapping = null
            },
            new StrategyModel
            {
                StrategyType = "StringWithSpecialCharacters",
                FrequencyGramMapping = null
            }

        };

              

            public IEnumerable<StrategyModel> GetAllStrategies
            {
            get { return _strategies; }
            }

            public List<StrategyModel> GetStrategyByStrategyType(string strategyType)
            {
                return _strategies.Where(x => x.StrategyType == strategyType).ToList();
            }
    }
}
