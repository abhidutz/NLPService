using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;


using System.Text;
using System.IO;
using java.util;
using java.io;
using System.Xml;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.ie.crf;
using Console = System.Console;


namespace NLPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(string value)
        {
            var jarRoot2 = @"C:\Workspace\Bofa\NLP\stanford-ner-2015-04-20";
            var classifiersDirecrory = jarRoot2 + @"\classifiers";

            // Loading 3 class classifier model
            var classifier = CRFClassifier.getClassifierNoExceptions(
                classifiersDirecrory + @"\english.all.3class.distsim.crf.ser.gz");

            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            //XmlElement root = doc.DocumentElement;
            //doc.InsertBefore(xmlDeclaration, root);
            doc.LoadXml(string.Format("<root>{0}</root>", classifier.classifyToString(value, "xml", true)));
            // doc.InnerXml = ;

            XmlNodeList orgs = doc.SelectNodes("root/wi[@entity='ORGANIZATION']");
            XmlNodeList ppl = doc.SelectNodes("root/wi[@entity='PERSON']");
            XmlNodeList locs = doc.SelectNodes("root/wi[@entity='LOCATION']");
            StringBuilder sd = new StringBuilder();
            foreach (XmlNode xmlN in orgs)
            {
                sd.Append(xmlN.InnerText + ", ");

            }
            foreach (XmlNode xmlN in ppl)
            {
                sd.Append(xmlN.InnerText + ", ");

            }
            foreach (XmlNode xmlN in locs)
            {
                sd.Append(xmlN.InnerText + ", ");

            }
            return sd.ToString();            
           
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
