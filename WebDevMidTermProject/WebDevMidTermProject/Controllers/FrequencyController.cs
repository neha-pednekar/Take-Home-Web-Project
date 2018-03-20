using System;
using System.Collections.Generic;
using System.Linq;
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

            if(_strategy != null && _strategy.Count > 0 && !String.IsNullOrWhiteSpace(codeModel.InputSecretMessage))
            {
                //Get the frequencies of the input cipher secret code

                int totalNumberOfLetters = codeModel.InputSecretMessage.Count();

                var frequencyPercentages = codeModel.InputSecretMessage
                                           .GroupBy(x => x)
                                           .Select(x => new
                                           {
                                               Character = x.Key,
                                               Percentage = x.Count() * 100 / totalNumberOfLetters 
                                           })
                                           .OrderByDescending(x => x.Percentage);

                frequencyPercentages.ToDictionary(r => r.Character, r => r.Percentage);

                var orderedStrategicDictionary = _strategy
                    .ElementAt(0)
                    .FrequencyGramMapping
                    .OrderByDescending(x => x.Value);

                var mappedDictionary = orderedStrategicDictionary.Zip(frequencyPercentages, (first, second) => first.Key
                + " " + second.Character);

                foreach (char c in codeModel.InputSecretMessage)
                {
                    
                }


            }

            return View();
        }
    }
}