using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Moolah
{
    public static class XPathExtensions
    {
        public static string XPathValue(this XDocument document, string xPath)
        {
            string value;
            if (!TryGetXPathValue(document, xPath, out value))
                throw new XPathException(string.Format("XPath element or attribute not found: {0}", xPath));
            return value;
        }

        public static bool TryGetXPathValue(this XDocument document, string xPath, out string value)
        {
            value = null;
            
            var xPathItem = (IEnumerable<object>)document.XPathEvaluate(xPath);
            var node = xPathItem.FirstOrDefault();
            if (node is XElement)
                value = ((XElement)node).Value;
            if (node is XAttribute)
                value = ((XAttribute)node).Value;

            return value != null;
        }
    }
}