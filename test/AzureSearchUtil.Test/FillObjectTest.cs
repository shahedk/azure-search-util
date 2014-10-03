using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureSearchUtil.Test
{
    /// <summary>
    /// Summary description for FillObjectTest
    /// </summary>
    [TestClass]
    public class FillObjectTest : TestBase
    {
        [TestMethod]
        public void LoadFromJson()
        {
            var jsonData = File.ReadAllText(@"..\..\data.json");
            
            var arr = JArray.Parse(jsonData);

            var data = GetData();

            Assert.IsTrue(data.Count == arr.Count);
            
        }
    }
}
