using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


var jsonData = File.ReadAllText("C:\\Users\\jarod\\source\\repos\\Scrapper\\Scrapper\\data.json");
var data = Cencus.FromJson(jsonData);
string[] labels = { "Abortion", "LGBT", "Transgender", "BLM Black Lives Matter", "Free Speech", "Capitalism", "Immigration" };

int abortionCount = 0;
int lgbtCount = 0;
int transgenderCount = 0;
int blmCount = 0;
int freeSpeechCount = 0;
int capitalismCount = 0;
int immigrationCount = 0;
foreach(var item in data.Variables)
{
    if (item.Value.Label.ToLower().Contains(labels[0].ToLower()))
    {
        abortionCount++;
        continue;
    }
    if (item.Value.Label.ToLower().Contains(labels[1].ToLower()))
    {
        lgbtCount++;
        continue;
    }
    if (item.Value.Label.ToLower().Contains(labels[2].ToLower()))
    {
        transgenderCount++;
        continue;
    }
    if (item.Value.Label.ToLower().Contains(labels[3].ToLower()))
    {
        blmCount++;
        continue;
    }
    if (item.Value.Label.ToLower().Contains(labels[4].ToLower()))
    {
        freeSpeechCount++;
        continue;
    }
    if (item.Value.Label.ToLower().Contains(labels[5].ToLower()))
    {
        capitalismCount++;
        continue;
    }
    if (item.Value.Label.ToLower().Contains(labels[6].ToLower()))
    {
        immigrationCount++;
        continue;
    }
}
Console.WriteLine($"Abortion:{abortionCount} LGBT:{lgbtCount} Transgender:{transgenderCount} BLM:{blmCount} FreeSpeech:{freeSpeechCount} Capitalism:{capitalismCount} Immigration:{immigrationCount}");
    public partial class Cencus
{
    [JsonProperty("variables")]
    public Dictionary<string, Variable> Variables { get; set; }
}

public partial class Variable
{
    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("concept", NullValueHandling = NullValueHandling.Ignore)]
    public string Concept { get; set; }

    [JsonProperty("predicateType", NullValueHandling = NullValueHandling.Ignore)]
    public PredicateType? PredicateType { get; set; }

    [JsonProperty("group")]
    public string Group { get; set; }

    [JsonProperty("limit")]
    public long Limit { get; set; }

    [JsonProperty("predicateOnly", NullValueHandling = NullValueHandling.Ignore)]
    public bool? PredicateOnly { get; set; }

    [JsonProperty("hasGeoCollectionSupport", NullValueHandling = NullValueHandling.Ignore)]
    public bool? HasGeoCollectionSupport { get; set; }

    [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
    public string Attributes { get; set; }

    [JsonProperty("required", NullValueHandling = NullValueHandling.Ignore)]
    public string VariableRequired { get; set; }
}

public enum PredicateType { FipsFor, FipsIn, Float, Int, String, Ucgid };

public partial class Cencus
{
    public static Cencus FromJson(string json) => JsonConvert.DeserializeObject<Cencus>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this Cencus self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
            {
                PredicateTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}

internal class PredicateTypeConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(PredicateType) || t == typeof(PredicateType?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "fips-for":
                return PredicateType.FipsFor;
            case "fips-in":
                return PredicateType.FipsIn;
            case "float":
                return PredicateType.Float;
            case "int":
                return PredicateType.Int;
            case "string":
                return PredicateType.String;
            case "ucgid":
                return PredicateType.Ucgid;
        }
        throw new Exception("Cannot unmarshal type PredicateType");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (PredicateType)untypedValue;
        switch (value)
        {
            case PredicateType.FipsFor:
                serializer.Serialize(writer, "fips-for");
                return;
            case PredicateType.FipsIn:
                serializer.Serialize(writer, "fips-in");
                return;
            case PredicateType.Float:
                serializer.Serialize(writer, "float");
                return;
            case PredicateType.Int:
                serializer.Serialize(writer, "int");
                return;
            case PredicateType.String:
                serializer.Serialize(writer, "string");
                return;
            case PredicateType.Ucgid:
                serializer.Serialize(writer, "ucgid");
                return;
        }
        throw new Exception("Cannot marshal type PredicateType");
    }

    public static readonly PredicateTypeConverter Singleton = new PredicateTypeConverter();
}