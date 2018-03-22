using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
                    codeModel = TrigramDecoding(codeModel, _strategy);
                }

              
            }

            return View("Index",codeModel);
        }

        private CodeModel MonogramDecoding(CodeModel codeModel, List<StrategyModel> strategyModel)
        {
            #region Monogram Substitution
            //Get the frequencies of the input cipher secret code

            int totalNumberOfLetters = codeModel.InputSecretMessage.Count();

            var frequencyDataMonograms = codeModel.InputSecretMessage
                                       .GroupBy(x => x)
                                       .Select(x => new
                                       {
                                           Character = x.Key,
                                           NoOfAppearances = x.Count(),
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

            codeModel.OutputDecodedMessage = codeModel.InputSecretMessage
                .Replace(monogramsWithMaxFreq.FirstOrDefault(), Convert.ToChar(orderedStrategicDictionary.FirstOrDefault().Key));

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


            bigramCombinations = bigramCombinations.Where(i => i.Value > 0)
                     .ToDictionary(i => i.Key, i => i.Value);

            var bigramsWithMaxFreq = bigramCombinations
                .FirstOrDefault(x => x.Value == bigramCombinations.Values.Max())
                .Key;

            codeModel.OutputDecodedMessage = codeModel.InputSecretMessage
                .Replace(bigramsWithMaxFreq, orderedStrategyInnerDictionary.FirstOrDefault().Key);

            #endregion

            return codeModel;
        }

        private CodeModel TrigramDecoding(CodeModel codeModel, List<StrategyModel> strategyModel)
        {
            #region Trigram Substitution

            var orderedStrategyInnerDictionaryTrigram = strategyModel
                .ElementAt(0)
                .FrequencyGramMapping
                .OrderByDescending(x => x.Value);


            char[] alphabetTri = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            Dictionary<string, int> trigramCombinations = new Dictionary<string, int>();


            var trigrams = from I in Enumerable.Range(0, 26)
                           from J in Enumerable.Range(0, 26)
                           from K in Enumerable.Range(0, 26)
                           select new { I, J, K };


            foreach (var trigram in trigrams)
            {
                // do something
                if (!trigramCombinations.ContainsKey(alphabetTri[trigram.I] + alphabetTri[trigram.J] + alphabetTri[trigram.K].ToString()))
                    trigramCombinations.Add(alphabetTri[trigram.I] + alphabetTri[trigram.J] + alphabetTri[trigram.K].ToString(), 0);
            }


            char[] textCharArrayTri = codeModel.InputSecretMessage.ToCharArray();
            for (int i = 0; i < codeModel.InputSecretMessage.Length - 2; i++)
            {
                string partial = textCharArrayTri[i] + textCharArrayTri[i + 1] + textCharArrayTri[i + 2].ToString();
                trigramCombinations[partial]++;
            }


            trigramCombinations = trigramCombinations.Where(i => i.Value > 0)
                     .ToDictionary(i => i.Key, i => i.Value);

            var trigramsWithMaxFreq = trigramCombinations
                .FirstOrDefault(x => x.Value == trigramCombinations.Values.Max())
                .Key;

            codeModel.OutputDecodedMessage = codeModel.InputSecretMessage
                .Replace(trigramsWithMaxFreq.FirstOrDefault(), Convert.ToChar(orderedStrategyInnerDictionaryTrigram.FirstOrDefault().Key));



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