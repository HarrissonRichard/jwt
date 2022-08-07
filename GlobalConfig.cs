using Microsoft.Extensions.Configuration;
using Tweet.Settings;

namespace Tweet
{
    public static class GlobalConfig
    {
       public static IConfiguration configuration;


        public static string GetConnString()
        {
            // var settings = config.GetSection("SqlServerSettings").Get<SqlServerSettings>();
            string tempPorayConn = $"Server=.;Database=TweetTestDB;Trusted_Connection=True;"; 
            return tempPorayConn;
            //return $"Server={settings.Server};Database={settings.Database};Trusted_Connection={settings.Trusted_Connection};"; 
        }

        // public string CreateJson()
        // {
        //     dynamic obj = new JObject();
	    //     obj.Name = "Satinder Singh";
	    //     obj.Location = "Mumbai";
	    //     obj.blog = "Codepedia.info";
	    //     var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
	    //     return jsonString;
        // }
    }
}