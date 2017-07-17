using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace WhatsAppAPI.GateWay
{
    public class WhatsAppProtocol
    {
        #region C..tor
        public WhatsAppProtocol(string phoneNumber, string imei, string name) : this(phoneNumber, imei, name, false) { }
        public WhatsAppProtocol(string phoneNumber, string imei, string name, bool debug) 
        {
            this.imei = imei;
            this.phoneNumber = phoneNumber;
            this.name = name;
            this.debug = debug;
            this.loginStatus = disconnectedStatus;
            Dictionary<int, object> dict = this.GetDictionary();
            this.writer = new BinTreeNodeWriter(dict);
            this.reader = new BinTreeNodeReader(dict);
        }

        #endregion

        #region Const
        const string whatsAppHost = "bin-short.whatsapp.net";
        const string whatsAppServer = "s.whatsapp.net";
        const string whatsAppRealm = "s.whatsapp.net";
        const string whatsAppDigest = "xmpp/s.whatsapp.net";
        const string device = "iPhone";
        const string whatsAppVer = "2.8.2";
        const int port = 5222;

        const string disconnectedStatus = "disconnected";
        const string connectedStatus = "connected";
        #endregion

        #region Props
        protected string phoneNumber{get;set;}
        protected string imei{get;set;}
        protected string name{get;set;}
        protected string loginStatus{get;set;}
        protected string accountInfo{get;set;}

        protected int timeoutSecs = 2;
        protected int timeoutUSecs = 0;
        protected string incomplete_message = String.Empty;

        protected Socket socket{get;set;}
        protected BinTreeNodeWriter writer;
        internal BinTreeNodeReader reader;

        protected bool debug;
        #endregion

        #region Protected Members
        protected ProtocolNode AddFeatures() 
        { 
            var child = new ProtocolNode("receipt_acks", null, null, String.Empty);
            var parent = new ProtocolNode("stream:features", null, new[]{child}.ToList(), String.Empty);
            return parent;
        }
        protected ProtocolNode AddAuth() 
        { 
            var authHash = new Dictionary<string,string>();
            authHash.Add("xmlns", "urn:ietf:params:xml:ns:xmpp-sasl");
            authHash.Add("mechanism","DIGEST-MD5-1");
            var node = new ProtocolNode("auth", authHash, null, String.Empty);
            return node;
        }
        protected string Authenticate(string nonce) { return String.Empty; }
        protected ProtocolNode AddAuthResponse() { return null; }
        protected void SendData(string data) { }
        protected void SendNode(ProtocolNode node) { }
        protected string ReadData(){return String.Empty;}
        protected void ProcessChallenge(ProtocolNode node) { }
        protected void SendMessageReceived(string msg) { }
        protected void ProcessInboundData(string data) { }
        protected void SendMessageNode(string msgid, string to, string node) { }
        protected void DebugPrint(string debugMsg) { }
        protected Dictionary<int, object> GetDictionary()
        {
            Dictionary<int,object> returnValue = new Dictionary<int,object>();
    
	returnValue.Add(0, 0);
	returnValue.Add(1, 0);
	returnValue.Add(2, 0);
	returnValue.Add(3, 0);
	returnValue.Add(4, 0);
	returnValue.Add(5, "1");
	returnValue.Add(6, "1.0");
	returnValue.Add(7, "ack");
	returnValue.Add(8, "action");
	returnValue.Add(9, "active");
	returnValue.Add(10, "add");
	returnValue.Add(11, "all");
	returnValue.Add(12, "allow");
	returnValue.Add(13, "apple");
	returnValue.Add(14, "audio");
	returnValue.Add(15, "auth");
	returnValue.Add(16, "author");
	returnValue.Add(17, "available");
	returnValue.Add(18, "bad-request");
	returnValue.Add(19, "basee64");
	returnValue.Add(20, "Bell.caf");
	returnValue.Add(21, "bind");
	returnValue.Add(22, "body");
	returnValue.Add(23, "Boing.caf");
	returnValue.Add(24, "cancel");
	returnValue.Add(25, "category");
	returnValue.Add(26, "challenge");
	returnValue.Add(27, "chat");
	returnValue.Add(28, "clean");
	returnValue.Add(29, "code");
	returnValue.Add(30, "composing");
	returnValue.Add(31, "config");
	returnValue.Add(32, "conflict");
	returnValue.Add(33, "contacts");
	returnValue.Add(34, "create");
	returnValue.Add(35, "creation");
	returnValue.Add(36, "default");
	returnValue.Add(37, "delay");
	returnValue.Add(38, "delete");
	returnValue.Add(39, "delivered");
	returnValue.Add(40, "deny");
	returnValue.Add(41, "DIGEST-MD5");
	returnValue.Add(42, "DIGEST-MD5-1");
	returnValue.Add(43, "dirty");
	returnValue.Add(44, "en");
	returnValue.Add(45, "enable");
	returnValue.Add(46, "encoding");
	returnValue.Add(47, "error");
	returnValue.Add(48, "expiration");
	returnValue.Add(49, "expired");
	returnValue.Add(50, "failure");
	returnValue.Add(51, "false");
	returnValue.Add(52, "favorites");
	returnValue.Add(53, "feature");
	returnValue.Add(54, "field");
	returnValue.Add(55, "free");
	returnValue.Add(56, "from");
	returnValue.Add(57, "g.us");
	returnValue.Add(58, "get");
	returnValue.Add(59, "Glas.caf");
	returnValue.Add(60, "google");
	returnValue.Add(61, "group");
	returnValue.Add(62, "groups");
	returnValue.Add(63, "g_sound");
	returnValue.Add(64, "Harp.caf");
	returnValue.Add(65, "http://etherx.jabber.org/streams");
	returnValue.Add(66, "http://jabber.org/protocol/chatstates");
	returnValue.Add(67, "id");
	returnValue.Add(68, "image");
	returnValue.Add(69, "img");
	returnValue.Add(70, "inactive");
	returnValue.Add(71, "internal-server-error");
	returnValue.Add(72, "iq");
	returnValue.Add(73, "item");
	returnValue.Add(74, "item-not-found");
	returnValue.Add(75, "jabber:client");
	returnValue.Add(76, "jabber:iq:last");
	returnValue.Add(77, "jabber:iq:privacy");
	returnValue.Add(78, "jabber:x:delay");
	returnValue.Add(79, "jabber:x:event");
	returnValue.Add(80, "jid");
	returnValue.Add(81, "jid-malformed");
	returnValue.Add(82, "kind");
	returnValue.Add(83, "leave");
	returnValue.Add(84, "leave-all");
	returnValue.Add(85, "list");
	returnValue.Add(86, "location");
	returnValue.Add(87, "max_groups");
	returnValue.Add(88, "max_participants");
	returnValue.Add(89, "max_subject");
	returnValue.Add(90, "mechanism");
	returnValue.Add(91, "mechanisms");
	returnValue.Add(92, "media");
	returnValue.Add(93, "message");
	returnValue.Add(94, "message_acks");
	returnValue.Add(95, "missing");
	returnValue.Add(96, "modify");
	returnValue.Add(97, "name");
	returnValue.Add(98, "not-acceptable");
	returnValue.Add(99, "not-allowed");
	returnValue.Add(100, "not-authorized");
	returnValue.Add(101, "notify");
	returnValue.Add(102, "Offline Storage");
	returnValue.Add(103, "order");
	returnValue.Add(104, "owner");
	returnValue.Add(105, "owning");
	returnValue.Add(106, "paid");
	returnValue.Add(107, "participant");
	returnValue.Add(108, "participants");
	returnValue.Add(109, "participating");
	returnValue.Add(110, "fail");
	returnValue.Add(111, "paused");
	returnValue.Add(112, "picture");
	returnValue.Add(113, "ping");
	returnValue.Add(114, "PLAIN");
	returnValue.Add(115, "platform");
	returnValue.Add(116, "presence");
	returnValue.Add(117, "preview");
	returnValue.Add(118, "probe");
	returnValue.Add(119, "prop");
	returnValue.Add(120, "props");
	returnValue.Add(121, "p_o");
	returnValue.Add(122, "p_t");
	returnValue.Add(123, "query");
	returnValue.Add(124, "raw");
	returnValue.Add(125, "receipt");
	returnValue.Add(126, "receipt_acks");
	returnValue.Add(127, "received");
	returnValue.Add(128, "relay");
	returnValue.Add(129, "remove");
	returnValue.Add(130, "Replaced by new connection");
	returnValue.Add(131, "request");
	returnValue.Add(132, "resource");
	returnValue.Add(133, "resource-constraint");
	returnValue.Add(134, "response");
	returnValue.Add(135, "result");
	returnValue.Add(136, "retry");
	returnValue.Add(137, "rim");
	returnValue.Add(138, "s.whatsapp.net");
	returnValue.Add(139, "seconds");
	returnValue.Add(140, "server");
	returnValue.Add(141, "session");
	returnValue.Add(142, "set");
	returnValue.Add(143, "show");
	returnValue.Add(144, "sid");
	returnValue.Add(145, "sound");
	returnValue.Add(146, "stamp");
	returnValue.Add(147, "starttls");
	returnValue.Add(148, "status");
	returnValue.Add(149, "stream:error");
	returnValue.Add(150, "stream:features");
	returnValue.Add(151, "subject");
	returnValue.Add(152, "subscribe");
	returnValue.Add(153, "success");
	returnValue.Add(154, "system-shutdown");
	returnValue.Add(155, "s_o");
	returnValue.Add(156, "s_t");
	returnValue.Add(157, "t");
	returnValue.Add(158, "TimePassing.caf");
	returnValue.Add(159, "timestamp");
	returnValue.Add(160, "to");
	returnValue.Add(161, "Tri-tone.caf");
	returnValue.Add(162, "type");
	returnValue.Add(163, "unavailable");
	returnValue.Add(164, "uri");
	returnValue.Add(165, "url");
	returnValue.Add(166, "urn:ietf:params:xml:ns:xmpp-bind");
	returnValue.Add(167, "urn:ietf:params:xml:ns:xmpp-sasl");
	returnValue.Add(168, "urn:ietf:params:xml:ns:xmpp-session");
	returnValue.Add(169, "urn:ietf:params:xml:ns:xmpp-stanzas");
	returnValue.Add(170, "urn:ietf:params:xml:ns:xmpp-streams");
	returnValue.Add(171, "urn:xmpp:delay");
	returnValue.Add(172, "urn:xmpp:ping");
	returnValue.Add(173, "urn:xmpp:receipts");
	returnValue.Add(174, "urn:xmpp:whatsapp");
	returnValue.Add(175, "urn:xmpp:whatsapp:dirty");
	returnValue.Add(176, "urn:xmpp:whatsapp:mms");
	returnValue.Add(177, "urn:xmpp:whatsapp:push");
	returnValue.Add(178, "value");
	returnValue.Add(179, "vcard");
	returnValue.Add(180, "version");
	returnValue.Add(181, "video");
	returnValue.Add(182, "w");
	returnValue.Add(183, "w:g");
	returnValue.Add(184, "w:p:r");
	returnValue.Add(185, "wait");
	returnValue.Add(186, "x");
	returnValue.Add(187, "xml-not-well-formed");
	returnValue.Add(188, "xml:lang");
	returnValue.Add(189, "xmlns");
	returnValue.Add(190, "xmlns:stream");
	returnValue.Add(191, "Xylophone.caf");
	returnValue.Add(192, "account");
	returnValue.Add(193, "digest");
	returnValue.Add(194, "g_notify");
	returnValue.Add(195, "method");
	returnValue.Add(196, "password");
	returnValue.Add(197, "registration");
	returnValue.Add(198, "stat");
	returnValue.Add(199, "text");
	returnValue.Add(200, "user");
	returnValue.Add(201, "username");
	returnValue.Add(202, "event");
	returnValue.Add(203, "latitude");
	returnValue.Add(204, "longitude");
	returnValue.Add(205, "true");
	returnValue.Add(206, "after");
	returnValue.Add(207, "before");
	returnValue.Add(208, "broadcast");
	returnValue.Add(209, "count");
	returnValue.Add(210, "features");
	returnValue.Add(211, "first");
	returnValue.Add(212, "index");
	returnValue.Add(213, "invalid-mechanism");
	returnValue.Add(214, "lreturnValue.Add(t");
	returnValue.Add(215, "max");
	returnValue.Add(216, "offline");
	returnValue.Add(217, "proceed");
	returnValue.Add(218, "required");
	returnValue.Add(219, "sync");
	returnValue.Add(220, "elapsed");
	returnValue.Add(221, "ip");
	returnValue.Add(222, "microsoft");
	returnValue.Add(223, "mute");
	returnValue.Add(224, "nokia");
	returnValue.Add(225, "off");
	returnValue.Add(226, "pin");
	returnValue.Add(227, "pop_mean_time");
	returnValue.Add(228, "pop_plus_minus");
	returnValue.Add(229, "port");
	returnValue.Add(230, "reason");
	returnValue.Add(231, "server-error");
	returnValue.Add(232, "silent");
	returnValue.Add(233, "timeout");
	returnValue.Add(234, "lc");
	returnValue.Add(235, "lg");
	returnValue.Add(236, "bad-protocol");
	returnValue.Add(237, "none");
	returnValue.Add(238, "remote-server-timeout");
	returnValue.Add(239, "service-unavailable");
	returnValue.Add(240, "w:p");
	returnValue.Add(241, "w:profile:picture");
	returnValue.Add(242, "notification");
	returnValue.Add(243, 0);
	returnValue.Add(244, 0);
	returnValue.Add(245, 0);
	returnValue.Add(246, 0);
	returnValue.Add(247, 0);
    returnValue.Add(248, "XXX");
    return returnValue;
        }

        #endregion

        #region Public Members
        public string EncryptPassword() { return String.Empty; }
        public string GetMessages() { return String.Empty; }
        public void AccountInfo() { }
        public void Connect() { }
        public void Login() { }
        public void PollMessages() { }
        public void Message(string msgid, string to, string txt) { }
        public void MessageImage(string msgid, string to, string url, string file, string size, string icon) { }
        public void Pong(string msgid){}
        public string RequestLastSeen(string var) { return String.Empty; }
        #endregion
    }
}
