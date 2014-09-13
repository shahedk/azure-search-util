using MongoDB.Bson;

namespace AzureSearchUtil
{
    public static class MongoDbPropertyExtensions
    {

        public static void FillObject(this BsonDocument doc, object obj)
        {
            var props = obj.GetType().GetProperties();
            foreach (var propertyInfo in props)
            {
                var sourceFieldName = propertyInfo.GetSourceFieldName();

                if (doc.Contains(sourceFieldName))
                {
                    var val = doc[sourceFieldName].RawValue;

                    if (val != null)
                    {
                        obj.SetValue(propertyInfo, val);
                    }
                }
            }
        }


    }
}
