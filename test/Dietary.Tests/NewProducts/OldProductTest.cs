using System;
using LegacyFighter.Dietary.Models.NewProducts;
using Xunit;

namespace LegacyFighter.Dietary.Tests.NewProducts
{
    public class OldProductTest
    {
        [Fact]
        public void priceCannotBeNull()
        {
            Assert.Throws<InvalidOperationException>(() => Price.Of(null));
        }

        [Fact]
        public void canIncrementCounterIfPriceIsPositive()
        {
            //given
            var p = ProductWithPriceAndCounter(10, 10);

            //when
            p.IncrementCounter();

            //then
            Assert.Equal(11, p.GetCounter());
        }

        [Fact]
        public void cannotIncrementCounterIfPriceIsNotPositive()
        {
            //given
            var p = ProductWithPriceAndCounter(0, 10);

            //expect
            Assert.Throws<InvalidOperationException>(() => p.IncrementCounter());
        }

        [Fact]
        public void canDecrementCounterIfPriceIsPositive()
        {
            //given
            var p = ProductWithPriceAndCounter(10, 10);
        
            //when
            p.DecrementCounter();
        
            //then
            Assert.Equal(9, p.GetCounter());
        }
        
        [Fact]
        public void cannotDecrementCounterIfPriceIsNotPositive()
        {
            //given
            var p = ProductWithPriceAndCounter(0, 0);
        
            //expect
            Assert.Throws<InvalidOperationException>(() => p.DecrementCounter());
        }
        
        [Fact]
        public void canChangePriceIfCounterIsPositive()
        {
            //given
            var p = ProductWithPriceAndCounter(0, 10);
        
            //when
            p.ChangePriceTo(10);
        
            //then
            Assert.Equal(10, p.GetPrice());
        }
        
        [Fact]
        public void cannotChangePriceIfCounterIsNotPositive()
        {
            //given
            var p = ProductWithPriceAndCounter(0, 0);
        
            //when
            p.ChangePriceTo(10);
        
            //then
            Assert.Equal(0, p.GetPrice());
        }

        private static OldProduct ProductWithPriceAndCounter(decimal? price, int counter)
            => new OldProduct(price, "desc", "longDesc", counter);

        private static OldProduct ProductWithDesc(string desc, string longDesc)
            => new OldProduct(10, desc, longDesc, 10);
    }
}