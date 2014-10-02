using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureSearchUtil.Test
{
    public abstract class TestBase
    {
        protected static List<object> GetData(int total = 9)
        {
            var jsonData = File.ReadAllText(@"..\..\data.json");

            var arr = JArray.Parse(jsonData);

            var list = new List<object>();
            var count = 0;
            foreach (JToken t in arr)
            {
                var item = new TestDocument();
                var jObj = t as JObject;

                jObj.FillObject(item);
                list.Add(item);

                if (++count > total)
                {
                    break;
                }
            }
            return list;
        }
    }
}
