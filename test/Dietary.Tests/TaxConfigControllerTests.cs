using System;
using System.Threading.Tasks;
using LegacyFighter.Dietary.DAL;
using LegacyFighter.Dietary.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace LegacyFighter.Dietary.Tests
{
    public class TaxConfigControllerTests : IDisposable
    {
        [Fact]
        public async Task shouldReturnCorrectMapOfConfigs()
        {
            //given
            await NewConfigWithRuleAndMaxRules(CountryCode, 2, TaxRule.LinearRule(1, 6, "tax-code1"));
            //and
            await NewConfigWithRuleAndMaxRules(CountryCode, 2, TaxRule.SquareRule(1, 5, 6, "tax-code2"));
            //and
            await NewConfigWithRuleAndMaxRules(CountryCode2, 2, TaxRule.LinearRule(1, 6, "tax-code3"));

            //then
            using var scope = _host.Services.CreateScope();
            var configMap = await TaxConfigController(scope).TaxConfigs();
            Assert.Equal(2, configMap.Count);
            Assert.Equal(2, configMap[CountryCode].Count);
            Assert.Contains(TaxRule.LinearRule(1, 6, "tax-code1"), configMap[CountryCode]);
            Assert.Contains(TaxRule.SquareRule(1, 5, 6, "tax-code2"), configMap[CountryCode]);
            Assert.Single(configMap[CountryCode2]);
            Assert.Contains(TaxRule.SquareRule(1, 5, 6, "tax-code3"), configMap[CountryCode2]);
        }

        private async Task<TaxConfig> ConfigBy(string countryCode)
        {
            using var scope = _host.Services.CreateScope();
            return await TaxConfigRepository(scope).FindByCountryCodeAsync(Models.CountryCode.Of(countryCode));
        }

        private async Task NewConfigWithRuleAndMaxRules(string countryCode, int maxRules, TaxRule aTaxRuleWithParams)
        {
            using var scope = _host.Services.CreateScope();
            var taxConfig = new TaxConfig(countryCode, maxRules, aTaxRuleWithParams);
            await TaxConfigRepository(scope).SaveAsync(taxConfig);
        }

        private const string CountryCode = "country-code";
        private const string CountryCode2 = "country-code2";

        TaxRuleService TaxRuleService(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<TaxRuleService>();
        ITaxConfigRepository TaxConfigRepository(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<ITaxConfigRepository>();
        TaxConfigController TaxConfigController(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<TaxConfigController>();
        private readonly IHost _host;

        public TaxConfigControllerTests()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder
                        .UseStartup<Startup>()
                        .UseTestServer();
                })
                .ConfigureServices(collection =>
                {
                    collection.AddDbContext<DietaryDbContext>();
                }).Build();
            _host.Start();
        }

        public void Dispose()
        {
            _host.Dispose();
        }
    }
}