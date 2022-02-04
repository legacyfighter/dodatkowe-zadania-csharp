using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LegacyFighter.Dietary.Models
{
    [Route("")]
    public class TaxConfigController : ControllerBase
    {
        private readonly TaxRuleService _taxRuleService;

        public TaxConfigController(TaxRuleService taxRuleService)
        {
            _taxRuleService = taxRuleService;
        }

        [HttpGet("config")]
        public async Task<Dictionary<string, List<TaxRule>>> TaxConfigs()
        {
            var taxConfigs = await _taxRuleService.FindAllConfigsAsync();

            var map = new Dictionary<string, List<TaxRule>>();
            foreach (var tax in taxConfigs)
            {
                if (!map.ContainsKey(tax.GetCountryCode()))
                {
                    if (tax.TaxRules is null)
                    {
                        tax.TaxRules = new List<TaxRule>();
                    }
            
                    map.Add(tax.CountryCode.AsString(), tax.TaxRules);
                }
                else
                {
                    map[tax.GetCountryCode()].AddRange(tax.TaxRules);
                }
            }

            var newRuleMap = new Dictionary<string, List<TaxRule>>();
            foreach (var (country, rules) in map)
            {
                var newList = rules.GroupBy(rule => rule.TaxCode).Select(g => g.First()).ToList();
                newRuleMap[country] = newList;
            }

            return newRuleMap;
        }
    }
}