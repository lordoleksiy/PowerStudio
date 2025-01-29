using Microsoft.VisualStudio.Text.Differencing;
using Newtonsoft.Json;
using PowerStudio.Models.Azure;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PowerStudio.Services
{
    public static class JsonProcessingService
    {
        public static string Serialize(ICollection<AppSettingsModel> appSettings)
        {
            var config = new Dictionary<string, object>();

            foreach (var settingModel in appSettings)
            {
                var key = settingModel.Key.ToString();
                var value = settingModel.Value.ToString();
                ProcessNode(key, value, config);
            }

            return JsonConvert.SerializeObject(config, Formatting.Indented);
        }

        private static void ProcessNode(string key, string value, Dictionary<string, object> settings)
        {
            var numberPattern = new Regex(@":(\d+):");
            var dotPattern = new Regex(@"\:");

            var numberMatch = numberPattern.Match(key);
            var dotMatch = dotPattern.Match(key);
            if (numberMatch.Success)
            {
                var firstSplitIndex = Math.Min(numberMatch.Index, dotMatch.Index);
                if (firstSplitIndex == numberMatch.Index)
                {
                    var beforeMatch = key.Substring(0, numberMatch.Index);
                    var secondStartIndex = numberMatch.Index + numberMatch.Length;
                    var afterMatch = secondStartIndex >= key.Length? null: key.Substring(secondStartIndex);
                    var number = Int32.Parse(numberMatch.Groups[1].Value);
                    SplitForArray(beforeMatch, afterMatch, value, number, settings);
                }
                else
                {
                    SplitForDictionary(key, value, settings);
                }
            }
            else if (dotMatch.Success)
            {
                SplitForDictionary(key, value, settings);
            }
            else
            {
                settings[key] = value;
            }
        }

        private static void SplitForDictionary(string key, string value, Dictionary<string, object> settings)
        {
            var keys = key.Split([':'], 2);
            if (!settings.TryGetValue(keys[0], out var objectList))
            {
                settings[keys[0]] = new Dictionary<string, object>();
            }
            var curValue = settings[keys[0]] as Dictionary<string, object>;
            ProcessNode(keys[1], value, curValue);
        }

        private static void SplitForArray(string beforeMatch, string afterMatch, string value, int index, Dictionary<string, object> settings)
        {
            if (string.IsNullOrEmpty(afterMatch))
            {
                if (!settings.TryGetValue(beforeMatch, out var objectList))
                {
                    settings[beforeMatch] = new List<object>();
                }
                var curValue = settings[beforeMatch] as List<object>;
                curValue.Add(value);
                settings[beforeMatch] = curValue;
            }
            else
            {
                if (!settings.TryGetValue(beforeMatch, out var objectList))
                {
                    settings[beforeMatch] = new List<Dictionary<string, object>>();
                }
                var curValue = settings[beforeMatch] as List<Dictionary<string, object>>;
                while (curValue.Count < index + 1)
                {
                    curValue.Add([]);
                }

                ProcessNode(afterMatch, value, curValue[index]);
            }
        }
    }
}
