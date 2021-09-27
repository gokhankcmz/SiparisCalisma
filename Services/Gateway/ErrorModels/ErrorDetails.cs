using Newtonsoft.Json;

namespace Gateway.ErrorModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorDetails : System.Exception
    {
        [JsonProperty] public int StatusCode { get; set; }
        [JsonProperty] public string ErrorMessage { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }


}