using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatsAppAPI.GateWay
{
    public class ProtocolNode
    {
        #region Private Properties
        internal string tag { get; set; }
        internal Dictionary<string, string> attributeHash { get; set; }
        internal string data { get; set; }
        internal List<ProtocolNode> children { get; set; }
        #endregion

        #region C...tor
        internal ProtocolNode(string tag,Dictionary<string,string> attributeHash,List<ProtocolNode> children,string data)
        {
            this.tag = tag;
            this.attributeHash = attributeHash;
            this.data = data;
            this.children = children;
        }
        #endregion
        
        #region Public Methods
        public string NodeString()
        {
            return NodeString(String.Empty);
        }

        public string NodeString(string indent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Concat(Environment.NewLine,indent,"<",this.tag));
            
            if (this.attributeHash != null)
            {
                foreach (var e in attributeHash)
                {
                    sb.Append(String.Concat(" ",e.Key,"=\"",e.Value,"\""));
                }
            }
            sb.Append(">");
            
            if (!String.IsNullOrEmpty(data))
            {
                sb.Append(data);
            }
            foreach (var p in children)
            {
                sb.Append(p.NodeString(indent + " "));
            }
            return sb.ToString();
        }

        public ProtocolNode GetChild(string tag)
        {
            ProtocolNode returnValue = null;
            if (this.children != null)
            {
                returnValue = (from x in children where String.Compare(x.tag, tag) == 0 select x).FirstOrDefault();
                if (returnValue == null)
                {
                    returnValue = (from x in children where x.GetChild(tag) != null select x).FirstOrDefault();
                }
            }
            return returnValue;
        }
        #endregion
    }
}
