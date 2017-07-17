using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatsAppAPI.GateWay
{
    public class BinTreeNodeWriter
    {
        #region Prop
        public string output { get; set; }
        public Dictionary<int,object> tokenMap;
        #endregion
        #region C...tor
        public BinTreeNodeWriter(Dictionary<int, object> tokenMap)
        {
            this.tokenMap = tokenMap;
        }
        #endregion
        #region Members
        public string StartStream(string domain, string resources)
        {
            var attributes = new Dictionary<string, string>();
            this.output = "WA";
            this.output += "\x01\x01\x00\x19";

            attributes.Add("to", domain);
            attributes.Add("resource", resources);

            this.WriteListStart(attributes.Count * 2 + 1);
            
            this.output += "\x01";
            this.WriteAttributes(attributes);

            var returnValue = this.output;
            this.output = String.Empty;
            return returnValue;
        }

        public string Write(ProtocolNode node)
        {
            if (node != null)
            {
                this.output += "\x00";
            }
            else
            {
                this.WriteInternal(node);
            }
            return this.FlushBuffer();
        }
        
        protected void WriteInternal(ProtocolNode node)
        {
            var len = 1;
            if (node.attributeHash != null)
            {
                len += node.attributeHash.Count * 2;
            }
            if (node.children.Count > 0)
            {
                ++len;
            }

            if (node.data.Length > 0)
            {
                ++len;
            }
            this.WriteListStart(len);
            this.WriteString(node.tag);
            this.WriteAttributes(node.attributeHash);
            if (node.data.Length > 0)
            {
                this.writeBytes(node.data);
            }
            if (node.children.Count > 0)
            {
                this.WriteListStart(node.children.Count);
                foreach (var child in node.children)
                {
                    this.WriteInternal(child);
                }
            }
        }

        protected string FlushBuffer()
        {
            var size = this.output.Length;
            var returnValue = this.writeInt16(size);
            returnValue += this.output;
            this.output = String.Empty;
            return returnValue;
        }

        protected void WriteToken(int token)
        {
            if (token < 0xf5)
            {
                this.output += (char)(token);
            }
            else if (token <= 0x1f4)
            {
                this.output += "\xfe" + ((char)(token - 0xf5));
            }
        }

        protected void WriteJid(string user, string server)
        {
            this.output += "\xfa";
            if (user.Length > 0)
            {
                this.WriteString(user);
            }
            else
            {
                this.WriteToken(0);
            }
            this.WriteString(server);
        }

        protected void writeInt8(int v)
        {
            this.output += (char)(v & 0xff);
        }

        protected string writeInt16(int v)
        {
            string returnValue  = (char)((v & 0xff00) >> 8) + String.Empty;
            returnValue += (char)((v & 0x00ff) >> 0);
            return returnValue;
        }

        protected void writeInt24(int v)
        {
            this.output += (char)((v & 0xff0000) >> 16);
            this.output += (char)((v & 0x00ff00) >> 8);
            this.output += (char)((v & 0x0000ff) >> 0);
        }

        protected void writeBytes(string bytes)
        {
            var len = bytes.Length;
            if (len >= 0x100)
            {
                this.output += "\xfd";
                this.writeInt24(len);
            }
            else
            {
                this.output += "\xfc";
                this.writeInt8(len);
            }
            this.output += bytes;
        }

        protected void WriteString(string tag)
        {
            if (this.tokenMap.ContainsKey(tag))
            {
                var key = this.tokenMap[tag];
                this.WriteToken((char)key);
            }
            else
            {
                var index = tag.IndexOf('@');
                if (index > -1)
                {
                    var server = tag.Substring(index + 1);
                    var user = tag.Substring(0, index);
                    this.WriteJid(user, server);
                }
                else
                {
                    this.writeBytes(tag);
                }
            }
        }

        protected void WriteAttributes(Dictionary<string,string> attributes)
        {
            if (attributes != null && attributes.Count > 0)
            {
                foreach (var e in attributes)
                {
                    this.WriteString(e.Key);
                    this.WriteString(e.Value);
                }
            }
        }
    
        protected void WriteListStart(int len)
        {
            if (len == 0)
            {
                this.output += "\x00";
            }
            else if (len < 256)
            {
                this.output += "\xf8" + (char)len;
            }
            else
            {
                this.output += "\xf9" + (char)(len);
            }
        }
        #endregion
        #region Auxiliar Members
        #endregion
    }
}
