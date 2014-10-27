using System;
using Knapcode.StandardSerializer;
using Newtonsoft.Json;

namespace Example
{
    public class Developer
    {
        public string Name { get; set; }

        public bool IsCSSNoob { get; set; }
    }

    public static class Program
    {
        public static void Main()
        {
            string json = "{'name': 'Joel', 'is_css_noob': true}";

            var resolver = new StandardContractResolver
            {
                // split the property into words by camel case and acronyms
                WordSplitOptions =
                    WordSplitOptions.SplitCamelCase |
                    WordSplitOptions.SplitAcronyms,

                // each output word should be lowercase
                CapitalizationOptions = CapitalizationOptions.AllLowercase,

                // join the words by underscores
                WordDelimiter = "_"
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = resolver
            };

            var d = JsonConvert.DeserializeObject<Developer>(json, settings);

            Console.WriteLine(
                "{0} is {1} a CSS noob.",
                d.Name,
                d.IsCSSNoob ? "pretty much" : "not");
        }
    }
}