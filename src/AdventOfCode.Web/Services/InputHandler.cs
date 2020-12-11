using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdventOfCode.Web.Services
{
    public interface IInputHandler
    {
        bool IsCachedInputAvailable(int day);

        Task<string> GetInputAsync(int year, int day);
        
        Task<string> GetSourceCodeAsync(int year,int day);

        object[] GetResults(int day);

        void ClearResults(int day);
    }

    public sealed class InputHandler : IInputHandler
    {
        private readonly HttpClient _myHttpClient;
        private readonly Dictionary<int, string> _myInputCache = new Dictionary<int, string>();
        private readonly Dictionary<int, object[]> _myResultCache = new Dictionary<int, object[]>();
        private readonly Dictionary<int, string> _mySourceCodeCache = new Dictionary<int, string>();

        public InputHandler(HttpClient httpClient)
        {
            _myHttpClient = httpClient;
        }

        public bool IsCachedInputAvailable(int day)
        {
            return _myInputCache.ContainsKey(day);
        }

        public async Task<string> GetInputAsync(int year, int day)
        {
            var input = await GetFileAsync(year, day, "input/{0}/day{1}.txt", _myInputCache);
            return input;
        }

        public async Task<string> GetSourceCodeAsync(int year, int day)
        {
            var source = await GetFileAsync(year, day, "source/{0}/Day{1}.cs", _mySourceCodeCache);
            return source ?? "No source file available.";
        }

        public object[] GetResults(int day)
        {
            if (!_myResultCache.TryGetValue(day, out var results))
            {
                results = new object[2];
                _myResultCache.Add(day, results);
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

        private async Task<string> GetFileAsync(int year, int day, string pathTemplate, IDictionary<int, string> cache)
        {
            if (!cache.TryGetValue(day, out var description))
            {
                var dayString = day.ToString().PadLeft(2, '0');
                try
                {
                    description = await _myHttpClient.GetStringAsync(string.Format(pathTemplate, year, dayString));
                }
                catch (HttpRequestException)
                {
                    return null;
                }

                cache.Add(day, description);
            }

            return description;
        }
    }
}