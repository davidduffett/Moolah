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
            var xPathItem = (IEnumerable<object>)document.XPathEvaluate(xPath);
            var node = xPathItem.FirstOrDefault();
            if (node is XElement)
                return ((XElement)node).Value;

            if (node is XAttribute)
                return ((XAttribute)node).Value;

            throw new XPathException(string.Format("XPath element or attribute not found: {0}", xPath));
        }
    }
}