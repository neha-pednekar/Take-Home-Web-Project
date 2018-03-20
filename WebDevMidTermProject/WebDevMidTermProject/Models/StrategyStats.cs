using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDevMidTermProject.Models
{
    public class StrategyStats
    {
        // create student body         
        private StrategyManager _manager = new StrategyManager();

        public async Task<int> GetStrategyCount()
        {
            return await Task.FromResult(_manager.GetAllStrategies.Count());
        }

    }
}
