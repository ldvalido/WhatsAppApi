using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatsAppAPI.GateWay
{
    internal class BinTreeNodeReader
    {
        #region Props
        public Dictionary<int, object> dictionary { get; set; }
        public string input { get; set; }
        #endregion

        #region C...tor
        public BinTreeNodeReader(Dictionary<int, object> dictionary)
        {
            this.dictionary = dictionary;
        }
        #endregion

        #region Public Methods

        public ProtocolNode nextTree()
        {
            return nextTree(null);
        }
        public ProtocolNode nextTree(string input)
        {
            if (input != null)
            {
                this.input = input;
            }
            var stanzaSize = this.peekInt16();
            if (stanzaSize > input.Length)
            {
                throw new WhatsAppException("Incomplete message", input);
            }
            readInt16();
            if (stanzaSize > 0)
            {
                return nextTreeInternal();
            }
            return null;
        }


        #endregion

        #region Auxiliar Methods

        protected ProtocolNode nextTreeInternal()
        {
            var token = this.readInt8();
            var size = this.readListSize(token);
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            token = this.readInt8();
            if (token == 1)
            {
                attributes = this.readAttributes(size);
                return new ProtocolNode("start", attributes, null, String.Empty);
            }
            else if (token == 2)
            {
                return null;
            }
            var tag = this.readString(token);
            attributes = this.readAttributes(size);
            if ((size % 2) == 1)
            {
                return new ProtocolNode(tag, attributes, null, String.Empty);
            }
            token = this.readInt8();
            if (this.isListTag(token))
            {
                return new ProtocolNode(tag, attributes, this.readList(token), String.Empty);
            }
            return new ProtocolNode(tag, attributes, null, this.readString(token));
        }

        protected bool isListTag(int token)
        {
            return ((token == 248) || (token == 0) || (token == 249));
        }

        protected Dictionary<string, string> readAttributes(int size)
        {
            Dictionary<string, string> returnValue = new Dictionary<string, string>();
            int attribCount = (size - 2 + size % 2) / 2;
            for (var i = 0; i < attribCount; i++)
            {
                var key = this.readString(this.readInt8());
                var value = this.readString(this.readInt8());
                returnValue.Add(key, value);
            }
            return returnValue;
        }

        protected List<ProtocolNode> readList(int token)
        {
            var returnValue = new List<ProtocolNode>();
            var size = this.readListSize(token);
            
            for (var i = 0; i < size; i++)
            {
                returnValue.Add(this.nextTreeInternal());
            }
            return returnValue;
        }

        protected int readListSize(int token)
        {
            var size = 0;
            if (token == 0xf8)
            {
                size = this.readInt8();
            }
            else if (token == 0xf9)
            {
                size = this.readInt16();
            }
            else
            {
                throw new ArgumentException("BinTreeNodeReader->readListSize: Invalid token $token");
            }
            return size;
        }

        string readString(int token)
        {
            int size = default(int);
            string returnValue = String.Empty;
            if (token == -1)
            {
                throw new ArgumentException("BinTreeNodeReader->readString: Invalid token $token");
            }

            if ((token > 4) && (token < 0xf5))
            {
                returnValue = this.getToken(token);
            }
            else if (token == 0)
            {
                returnValue = String.Empty;
            }
            else if (token == 0xfc)
            {
                size = this.readInt8();
                returnValue = this.fillArray(size);
            }
            else if (token == 0xfd)
            {
                size = this.readInt24();
                returnValue = this.fillArray(size);
            }
            else if (token == 0xfe)
            {
                token = this.readInt8();
                returnValue = this.getToken(token + 0xf5);
            }
            else if (token == 0xfa)
            {
                var user = this.readString(this.readInt8());
                var server = this.readString(this.readInt8());
                if ((user.Length > 0) && (server.Length > 0))
                {
                    returnValue = String.Concat(user, "@", server);
                }
                else if (server.Length > 0)
                {
                    returnValue = server;
                }
            }
            return returnValue;
        }

        string getToken(int token)
        {
            string returnValue = String.Empty;
            if ((token >= 0) && (token < dictionary.Count))
            {
                returnValue = dictionary[token].ToString();
            }
            else
            {
                throw new ArgumentException("BinTreeNodeReader->getToken: Invalid token $token");
            }
            return returnValue;
        }

        protected string fillArray(int len)
        {
            var returnValue = String.Empty;
            if (this.input.Length >= len)
            {
                returnValue = this.input.Substring(0, len);
                //Verify equivalences btw PHP & C#
                this.input = this.input.Substring(len);
            }
            return returnValue;
        }
        
        int peekInt16(){ return 0; }
//        int read16() { return 0; }
        int readInt8() { return 0; }
        int readInt16() { return 0; }
        int readInt24() { return 0; }
#endregion
    }
}
