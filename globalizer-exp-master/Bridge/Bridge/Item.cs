using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using System.IO;

namespace Bridge
{
    public class Item
    {
        protected string name;
        protected string value;
        protected XElement xmlElement;
        public TaskConf config { get; set; }
        public Item()
        {
            name = "";
            value = "";
            xmlElement = null;
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                if (xmlElement != null)
                {
                    xmlElement.Value = value;
                }
            }
        }

        public XElement XMLElement
        {
            get
            {
                return xmlElement;
            }
            set
            {
                xmlElement = value;
                name = xmlElement.Name.ToString();
                Value = xmlElement.Value;
            }
        }
    }
}


