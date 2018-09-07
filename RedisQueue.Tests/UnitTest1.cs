using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RedisQueue.Tests
{
    public class UnitTest1
    {
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [Theory]
        public async Task EnqueueDequeue(int n)
        {
            var queue = new RedisQueue<string>();
            var values = Enumerable.Range(0, n).Select(i => $"value-{i}");
            var invertedValues = values.Reverse();

            foreach (var value in values)
                await queue.EnqueueAsync(value);

            foreach (var value in invertedValues)
                Assert.Equal(value, await queue.DequeueAsync());
        }
    }
}
