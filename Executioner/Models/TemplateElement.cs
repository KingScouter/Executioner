namespace Executioner.Models
{
    internal class TemplateElement(string keyword, bool isParam)
    {
        public const string templateParamStart = "${{";
        public const string templateParamEnd = "}}";

        public string keyword = keyword;
        public bool isParam = isParam;

        public TemplateElement(string data) : this(data.Trim(), false)
        {
            if (!keyword.StartsWith(templateParamStart) || !keyword.EndsWith(templateParamEnd))
                return;

            keyword = keyword.Substring(
                templateParamStart.Length,
                keyword.Length - templateParamStart.Length - templateParamEnd.Length)
                .Trim();
            isParam = true;
        }

        public override string ToString()
        {
            if (isParam)
                return $"{templateParamStart} {keyword} {templateParamEnd}";

            return keyword;
        }
    }
}
