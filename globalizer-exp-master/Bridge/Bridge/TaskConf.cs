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
    public class TaskConf
    {
        protected List<Item> items;
        public string Name { get; set; }
        public string Comment { get; set; }
        public static string XMLConfiguration = "XML";
        public TaskConf()
        {
            items = new List<Item>();
            Name = "";
        }
        public List<Item> GetItems()
        {
            return items;
        }
        public void Add(XElement element)
        {
            Item item = new Item();
            item.XMLElement = element;
            item.config = this;
            items.Add(item);
        }
        public void Add(Item item)
        {
            bool f = false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Name == item.Name)
                {
                    f = true;
                }
            }
            if (f == false)
            {
                item.config = this;
                items.Add(item);
            }
        }
    }
}
