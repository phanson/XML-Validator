using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace XmlValidator
{
    /// <summary>
    /// Handles setting up the XML libraries and validating documents
    /// </summary>
    public class XmlValidator
    {
        private int errorCount = 0;
        private string errorMessage = "";

        /// <summary>
        /// Gets validation error message, if any
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
        }

        private void ValidationHandler(object sender, ValidationEventArgs args)
        {
            errorMessage += "\r\n" + args.Message;
            errorCount++;
        }

        /// <summary>
        /// Validates an XML file using the given schema.
        /// </summary>
        /// <param name="schemaURI">URI of schema</param>
        /// <param name="xmlURI">URI of document</param>
        /// <returns><c>true</c> if validation is successful</returns>
        public bool Validate(string schemaURI, string xmlURI)
        {
            XmlReaderSettings s = new XmlReaderSettings();
            s.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints;
            s.ValidationType = ValidationType.Schema;
            s.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
            s.Schemas.Add(null, schemaURI);

            XmlReader reader = XmlReader.Create(xmlURI, s);

            while (reader.Read()) ;

            reader.Close();
            return errorCount == 0;
        }
    }
}
