using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebDevMidTermProject.Models;

namespace WebDevMidTermProject.Controllers
{
    public class FrequencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckFrequencyForm(CodeModel codeModel)
        {
            StrategyManager _sm = new StrategyManager();
            List<StrategyModel> _strategy = _sm.GetStrategyByStrategyType(codeModel.StrategyModel.StrategyType);

            if(_strategy != null && _strategy.Count > 0 && 
                !String.IsNullOrWhiteSpace(codeModel.InputSecretMessage))
            {
                if(_strategy[0].StrategyType == "Monogram")
                {
                    codeModel = MonogramDecoding(codeModel, _strategy);
                }
                else if(_strategy[0].StrategyType == "Bigram")
                {
                    codeModel = BigramDecoding(codeModel, _strategy);
                }
                else if(_strategy[0].StrategyType == "Trigram")
                {
                    if(codeModel.InputSecretMessage.Contains(" "))
                    {
                        codeModel = TrigramDecodingForStringWithSpaces(codeModel, _strategy);
                    }
                    else
                    {
                        codeModel = TrigramDecoding(codeModel, _strategy);
                    }
                }
                else if(_strategy[0].StrategyType == "Monogram and Bigram and Trigram")
                {
                    codeModel = TrigramDecoding(codeModel, _strategy);
                    codeModel = BigramDecoding(codeModel, _strategy);
                    codeModel = MonogramDecoding(codeModel, _strategy);
                }

              
            }

            return View("Index",codeModel);
        }

        private CodeModel MonogramDecoding(CodeModel codeModel, List<StrategyModel> strategyModel)
        {
            #region Monogram Substitution
            //Get the frequencies of the input cipher secret code

            int totalNumberOfLetters = codeModel.InputSecretMessage.Count();

            List<MonogramDataModel> frequencyDataMonograms = codeModel.InputSecretMessage
                                       .GroupBy(x => x)
                                       .Select(x => new MonogramDataModel
                                       {
                                           Character = x.Key,
                                           NumberOfOccurences = x.Count(),
                                           Occupied = false,
                                           Percentage = Math.Round(((double)x.Count() / totalNumberOfLetters) * 100, 2)
                                       })
                                       .OrderByDescending(x => x.Percentage)
                                       .ToList();
           

            var monogramsWithMaxFreq = codeModel.InputSecretMessage
                                                .GroupBy(p => p)
                                                .Select(p => new { Count = p.Count(), Char = p.Key })
                                                .GroupBy(p => p.Count, p => p.Char)
                                                .OrderByDescending(p => p.Key)
                                                .FirstOrDefault();


            var orderedStrategicDictionary = strategyModel
                .ElementAt(0)
                .FrequencyGramMapping
                .OrderByDescending(x => x.Value);

            IList<MonogramDataModel> standardFreqDataMonograms = strategyModel.ElementAt(0).FrequencyGramMapping
                                       .GroupBy(x => x)
                                       .Select(x => new MonogramDataModel
                                       {
                                           Character = Convert.ToChar(x.Key.Key),
                                           NumberOfOccurences = x.Count(),
                                           Occupied = false,
                                           Percentage = x.Key.Value
                                       })
                                       .OrderByDescending(x => x.Percentage)
                                       .ToList();

            
            codeModel.OutputDecodedMessage = codeModel.InputSecretMessage
                .Replace(monogramsWithMaxFreq.FirstOrDefault(), Convert.ToChar(orderedStrategicDictionary.FirstOrDefault().Key));

            StringBuilder sb = new StringBuilder();
            sb.Append(orderedStrategicDictionary.FirstOrDefault().Key.Trim());
            sb.Append(" ,");
            codeModel.MonogramsAlreadyOccupied = sb.ToString();

            foreach (MonogramDataModel monogramDataModel in standardFreqDataMonograms)
            {
                if (orderedStrategicDictionary.FirstOrDefault().Key.Trim().Equals(monogramDataModel.Character))
                {
                    monogramDataModel.Occupied = true;
                }
            }

            ViewData["MonogramDataModels"] = standardFreqDataMonograms; 

            #endregion

            return codeModel;
        }

        private CodeModel BigramDecoding(CodeModel codeModel, List<StrategyModel> strategyModel)
        {
            #region Bigram Substition

            var orderedStrategyInnerDictionary = strategyModel
                .ElementAt(0)
                .FrequencyGramMapping
                .OrderByDescending(x => x.Value);


            char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Dictionary<string, int> bigramCombinations = new Dictionary<string, int>();


            var bigrams = from I in Enumerable.Range(0, 26)
                          from J in Enumerable.Range(0, 26)
                          select new { I, J };


            foreach (var bigram in bigrams)
            {
                // do something
                bigramCombinations.Add(alphabet[bigram.I] + alphabet[bigram.J].ToString(), 0);
            }


            char[] textCharArray = codeModel.InputSecretMessage.ToCharArray();
            for (int i = 0; i < codeModel.InputSecretMessage.Length - 1; i++)
            {
                string partial = textCharArray[i] + textCharArray[i + 1].ToString();
                bigramCombinations[partial]++;
            }

            int totalNumberOfBigrams = bigramCombinations.Count();


            bigramCombinations = bigramCombinations.Where(i => i.Value > 0)
                     .ToDictionary(i => i.Key, i => i.Value);

            List<BigramDataModel> frequencyDataBigrams = bigramCombinations
                                       .Select(x => new BigramDataModel
                                       {
                                           BigramSequence = x.Key,
                                           NumberOfOccurences = x.Value,
                                           Occupied = false,
                                           Percentage = Math.Round(((double)x.Value / totalNumberOfBigrams) * 100, 2)
                                       })
                                       .OrderByDescending(x => x.Percentage)
                                       .ToList();

            var bigramsWithMaxFreq = bigramCombinations
                .FirstOrDefault(x => x.Value == bigramCombinations.Values.Max())
                .Key;

            codeModel.OutputDecodedMessage = codeModel.InputSecretMessage
                .Replace(bigramsWithMaxFreq, orderedStrategyInnerDictionary.FirstOrDefault().Key);

            for (int i = 0; i < 2; i++)
            {
                codeModel.OutputDecodedMessage = codeModel.OutputDecodedMessage
                .Replace(bigramsWithMaxFreq.ToCharArray()[i], orderedStrategyInnerDictionary.FirstOrDefault().Key.ToCharArray()[i]);
            }

            #endregion

            return codeModel;
        }

        private CodeModel TrigramDecoding(CodeModel codeModel, List<StrategyModel> strategyModel)
        {
            #region Trigram Substitution

            //Use OrderBy(and not Order By Descending) as we are retrieving the trigram according to the rank and not percentage.
            //Therefore, the trigram with minimum rank i.e 1 will have the highest freq
            var orderedStrategyInnerDictionaryTrigram = strategyModel
                .ElementAt(0)
                .FrequencyGramMapping
                .OrderBy(x => x.Value);


            char[] alphabetTri = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Dictionary<string, int> trigramCombinations = new Dictionary<string, int>();


            var trigrams = from I in Enumerable.Range(0, 26)
                           from J in Enumerable.Range(0, 26)
                           from K in Enumerable.Range(0, 26)
                           select new { I, J, K };


            foreach (var trigram in trigrams)
            {
                // do something
                if (!trigramCombinations.ContainsKey((alphabetTri[trigram.I] + alphabetTri[trigram.J].ToString()) + alphabetTri[trigram.K].ToString()))
                    trigramCombinations.Add((alphabetTri[trigram.I] + alphabetTri[trigram.J].ToString()) + alphabetTri[trigram.K].ToString(), 0);
            }

            codeModel.InputSecretMessage = codeModel.InputSecretMessage.Replace(" ", "");

            char[] textCharArrayTri = codeModel.InputSecretMessage.ToCharArray();
            for (int i = 0; i < codeModel.InputSecretMessage.Length - 2; i++)
            {
                string partial = (textCharArrayTri[i] + textCharArrayTri[i + 1].ToString()) + textCharArrayTri[i + 2].ToString();
                if(trigramCombinations.ContainsKey(partial))
                {
                    trigramCombinations[partial]++;
                }
                
            }

            int totalNumberOfTrigrams = trigramCombinations.Count();


            trigramCombinations = trigramCombinations.Where(i => i.Value > 0)
                     .ToDictionary(i => i.Key, i => i.Value);

            List<TrigramDataModel> frequencyDataTrigrams = trigramCombinations
                                       .Select(x => new TrigramDataModel
                                       {
                                           TrigramSequence = x.Key,
                                           NumberOfOccurences = x.Value,
                                           Occupied = false,
                                           Percentage = Math.Round(((double)x.Value / totalNumberOfTrigrams) * 100, 2)
                                       })
                                       .OrderByDescending(x => x.Percentage)
                                       .ToList();


            var trigramsWithMaxFreq = trigramCombinations
                .FirstOrDefault(x => x.Value == trigramCombinations.Values.Max())
                .Key;

            codeModel.OutputDecodedMessage = codeModel.InputSecretMessage
                .Replace(trigramsWithMaxFreq, orderedStrategyInnerDictionaryTrigram.FirstOrDefault().Key);

            for(int i = 0; i < 3; i++)
            {
                codeModel.OutputDecodedMessage = codeModel.OutputDecodedMessage
                .Replace(trigramsWithMaxFreq.ToCharArray()[i], orderedStrategyInnerDictionaryTrigram.FirstOrDefault().Key.ToCharArray()[i]);
            }
            


            #endregion

            return codeModel;
        }

        private CodeModel TrigramDecodingForStringWithSpaces(CodeModel codeModel, List<StrategyModel> strategyModel)
        {
            #region Trigram Substitution

            //Use OrderBy(and not Order By Descending) as we are retrieving the trigram according to the rank and not percentage.
            //Therefore, the trigram with minimum rank i.e 1 will have the highest freq
            var orderedStrategyInnerDictionaryTrigram = strategyModel
                .ElementAt(0)
                .FrequencyGramMapping
                .OrderBy(x => x.Value);


            char[] alphabetTri = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Dictionary<string, int> trigramCombinations = new Dictionary<string, int>();


            var trigrams = from I in Enumerable.Range(0, 26)
                           from J in Enumerable.Range(0, 26)
                           from K in Enumerable.Range(0, 26)
                           select new { I, J, K };


            foreach (var trigram in trigrams)
            {
                // do something
                if (!trigramCombinations.ContainsKey((alphabetTri[trigram.I] + alphabetTri[trigram.J].ToString()) + alphabetTri[trigram.K].ToString()))
                    trigramCombinations.Add((alphabetTri[trigram.I] + alphabetTri[trigram.J].ToString()) + alphabetTri[trigram.K].ToString(), 0);
            }

            codeModel.InputSecretMessage = codeModel.InputSecretMessage.Replace(" ", "");

            char[] textCharArrayTri = codeModel.InputSecretMessage.ToCharArray();
            for (int i = 0; i < codeModel.InputSecretMessage.Length - 2; i++)
            {
                string partial = (textCharArrayTri[i] + textCharArrayTri[i + 1].ToString()) + textCharArrayTri[i + 2].ToString();
                if (trigramCombinations.ContainsKey(partial))
                {
                    trigramCombinations[partial]++;
                }

            }

            int totalNumberOfTrigrams = trigramCombinations.Count();


            trigramCombinations = trigramCombinations.Where(i => i.Value > 0)
                     .ToDictionary(i => i.Key, i => i.Value);

            List<TrigramDataModel> frequencyDataTrigrams = trigramCombinations
                                       .Select(x => new TrigramDataModel
                                       {
                                           TrigramSequence = x.Key,
                                           NumberOfOccurences = x.Value,
                                           Occupied = false,
                                           Percentage = Math.Round(((double)x.Value / totalNumberOfTrigrams) * 100, 2)
                                       })
                                       .OrderByDescending(x => x.Percentage)
                                       .ToList();


            var trigramsWithMaxFreq = trigramCombinations
                .FirstOrDefault(x => x.Value == trigramCombinations.Values.Max())
                .Key;

            codeModel.OutputDecodedMessage = codeModel.InputSecretMessage
                .Replace(trigramsWithMaxFreq, orderedStrategyInnerDictionaryTrigram.FirstOrDefault().Key);

            for (int i = 0; i < 3; i++)
            {
                codeModel.OutputDecodedMessage = codeModel.OutputDecodedMessage
                .Replace(trigramsWithMaxFreq.ToCharArray()[i], orderedStrategyInnerDictionaryTrigram.FirstOrDefault().Key.ToCharArray()[i]);
            }



            #endregion

            return codeModel;
        }


        public IActionResult MonogramTableView()
        {
            StrategyManager _sm = new StrategyManager();
            IEnumerable<StrategyModel> strategies = _sm.GetStrategyByStrategyType("Monogram");
            return View(strategies);
        }

        public IActionResult BigramTableView()
        {
            StrategyManager _sm = new StrategyManager();
            IEnumerable<StrategyModel> strategies = _sm.GetStrategyByStrategyType("Bigram");
            return View(strategies);
        }

        public IActionResult TrigramTableView()
        {
            StrategyManager _sm = new StrategyManager();
            IEnumerable<StrategyModel> strategies = _sm.GetStrategyByStrategyType("Trigram");
            return View(strategies);
        }
    }
}