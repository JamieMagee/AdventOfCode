using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode._2020.Web.Services
{
    public interface IInputHandler
    {
        bool IsCachedInputAvailable(int day);

        Task<string> GetInputAsync(int day);

        Task<string> GetDescriptionAsync(int day);

        Task<string> GetSourceCodeAsync(int day);

        object[] GetResults(int day);

        void ClearResults(int day);
    }

    public sealed class InputHandler : IInputHandler
    {
        public InputHandler(HttpClient httpClient)
        {
            myHttpClient = httpClient;
        }

        public bool IsCachedInputAvailable(int day) => myInputCache.ContainsKey(day);

        public async Task<string> GetInputAsync(int day)
        {
            var input = await GetFileAsync(day, "input/day{0}.txt", myInputCache);
            return input;
        }

        public async Task<string> GetDescriptionAsync(int day)
        {
            var description = await GetFileAsync(day, "description/day{0}.html", myDescriptionCache);
            return description ?? "No description available.";
        }

        public async Task<string> GetSourceCodeAsync(int day)
        {
            var source = await GetFileAsync(day, "source/Day{0}.cs", mySourceCodeCache);
            return source ?? "No source file available.";
        }

        public async Task<string> GetFileAsync(int day, string pathTemplate, IDictionary<int, string> cache)
        {
            if (!cache.TryGetValue(day, out var description))
            {
                var dayString = day.ToString().PadLeft(2, '0');
                try
                {
                    description = await myHttpClient.GetStringAsync(string.Format(pathTemplate, dayString));
                }
                catch (HttpRequestException)
                {
                    return null;
                }
                cache.Add(day, description);
            }

            return description;
        }

        public object[] GetResults(int day)
        {
            if (!myResultCache.TryGetValue(day, out var results))
            {
                results = new object[2];
                myResultCache.Add(day, results);
            }
            return results;
        }

        public void ClearResults(int day)
        {
            var results = GetResults(day);
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = null;
            }
        }

        private readonly HttpClient myHttpClient;
        private readonly Dictionary<int, object[]> myResultCache = new Dictionary<int, object[]>();
        private readonly Dictionary<int, string> myInputCache = new Dictionary<int, string>();
        private readonly Dictionary<int, string> myDescriptionCache = new Dictionary<int, string>();
        private readonly Dictionary<int, string> mySourceCodeCache = new Dictionary<int, string>();
    }
}