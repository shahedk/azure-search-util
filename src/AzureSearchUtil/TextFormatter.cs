namespace AzureSearchUtil
{
    public static class TextFormatter
    {
        /// <summary>
        /// Converts first letter of the word to lower case (e.g. ShortName => shortName)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            else
            {
                var firstLetter = name.Substring(0, 1);

                var newName = firstLetter.ToLower() + name.Substring(1, name.Length - 1);

                return newName;
            }
        }

        /// <summary>
        /// Converts first letter of the word to upper case (e.g. shortName => ShortName)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            else
            {
                var firstLetter = name.Substring(0, 1);

                var newName = firstLetter.ToUpper() + name.Substring(1, name.Length - 1);

                return newName;
            }
        }
    }
}
