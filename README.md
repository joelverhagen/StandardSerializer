StandardSerializer
==================

A contract resolver for Json.NET to make making serializing property names simpler.

.NET properties are typically written in Pascal case (e.g. `MyProperty`). Many JSON properties are written in a
lowercase, underscore-delimited form (e.g. `my_property`). This leads to headaches when consuming some web API
endpoints that serve out JSON because the property names don't automatically deserialize to your wonderful C# models.

If you are writing .NET code to consume JSON and you're using [Json.NET][1] (Newtonsoft.Json on NuGet), maybe you've
got lucky with the default contract serializer... more likely is that you've annotated the heck out of your models with
[`JsonPropertyAttribute`][2]. This leads to a lot of annoyance and really clouds up your model with mundain details
like serialization. Bah!

[1]: http://james.newtonking.com/json
[2]: http://james.newtonking.com/json/help/index.html?topic=html/T_Newtonsoft_Json_JsonPropertyAttribute.htm)

This library tries to give you all the flexibility you need to avoid `JsonPropertyAttribute` in your models while still
allowing you to consume that juicy JSON.

Example
-------

```csharp
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
```