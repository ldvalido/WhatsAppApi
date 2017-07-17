using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatsAppAPI.GateWay
{
    public class WhatsAppException : Exception
    {
        #region Properties
        public string input { get; set; }
        #endregion

        #region C...tor
        public WhatsAppException(string message, string input):base(message)
        {
            this.input = input;
        }
        #endregion
    }
}
