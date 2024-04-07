using System.Text;

namespace Executioner.Models
{
    internal class CommandTemplate
    {
        public const string templateRegex = "(?:([^(?:\\$\\{\\{)]*)(\\$\\{\\{[\\s|\\d|\\w]*\\}\\})?)";

        public List<TemplateElement> elements = [];

        public CommandTemplate(string data)
        {
            ParseTemplate(data);
        }

        public void ParseTemplate(string rawTemplate)
        {
            elements.Clear();

            List<TemplateElement> parsedElements = [];

            var matches = System.Text.RegularExpressions.Regex.Matches(rawTemplate, templateRegex);
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                bool first = true;
                foreach (System.Text.RegularExpressions.Group group in match.Groups)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    parsedElements.Add(new TemplateElement(group.Value));
                }
            }

            string currElement = "";

            foreach (TemplateElement element in parsedElements)
            {
                if (element.isParam)
                {
                    if (currElement.Length > 0)
                    {
                        elements.Add(new TemplateElement(currElement, false));
                        currElement = "";
                    }
                    elements.Add(element);
                    continue;
                }

                if (currElement.Length > 0)
                    currElement += " ";
                currElement += element.keyword;
            }

            if (currElement.Length > 0)
                elements.Add(new TemplateElement(currElement, false));
        }

        public List<TemplateElement> GetParamElements()
        {
            return elements.FindAll(elem => elem.isParam);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (TemplateElement element in elements)
            {
                sb.Append(element.ToString());
                sb.Append(' ');
            }

            return sb.ToString().TrimEnd();
        }
    }
}
