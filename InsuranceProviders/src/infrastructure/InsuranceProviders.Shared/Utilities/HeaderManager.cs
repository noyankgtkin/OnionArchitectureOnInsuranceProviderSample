using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InsuranceProviders.Shared.Utilities
{
    public class InspectorBehavior : IEndpointBehavior
    {
        public ClientInspector ClientInspector { get; set; }

        public InspectorBehavior(ClientInspector clientInspector)
        {
            ClientInspector = clientInspector;
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (ClientInspector == null) throw new InvalidOperationException("Caller must supply ClientInspector.");
            clientRuntime.ClientMessageInspectors.Add(ClientInspector);
        }
    }

    public class ClientInspector : IClientMessageInspector
    {
        public MessageHeader[] Headers { get; set; }

        public ClientInspector(params MessageHeader[] headers)
        {
            Headers = headers;
        }
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (Headers != null)
            {
                for (var i = Headers.Length - 1; i >= 0; i--)
                {
                    request.Headers.Insert(0, Headers[i]);
                }
            }
            return request;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {

        }
    }

    public class SecurityHeader : MessageHeader
    {
        private string SystemUser { get; set; }

        private string SystemPassword { get; set; }

        public SecurityHeader(string systemUser, string systemPassword)
        {
            SystemUser = systemUser;
            SystemPassword = systemPassword;
        }

        public override string Name => "Security";

        public override string Namespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            WriteHeader(writer);
        }

        private void WriteHeader(XmlDictionaryWriter writer)
        {
            var nonce = CreateNonce();

            var created = DateTime.UtcNow.AddSeconds(-10).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            writer.WriteStartElement("wsse", "UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            writer.WriteStartElement("wsse", "Username", null);
            writer.WriteString(SystemUser);
            writer.WriteEndElement();
            writer.WriteStartElement("wsse", "Password", null);
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest");
            writer.WriteString(ComputePasswordDigest(SystemPassword, nonce, created));
            writer.WriteEndElement();
            writer.WriteStartElement("wsse", "Nonce", null);
            writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            writer.WriteBase64(nonce, 0, nonce.Length);
            writer.WriteEndElement();
            writer.WriteStartElement("wsu", "Created", null);
            writer.WriteString(created);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
        }

        private byte[] CreateNonce()
        {
            var rand = new RNGCryptoServiceProvider();
            var buf = new byte[0x10];
            rand.GetBytes(buf);
            return buf;
        }

        private string ComputePasswordDigest(string secret, byte[] nonceInBytes, string created)
        {
            var createdInBytes = Encoding.UTF8.GetBytes(created);
            var secretInBytes = Encoding.UTF8.GetBytes(secret);
            var concatenation = new byte[nonceInBytes.Length + createdInBytes.Length + secretInBytes.Length];
            Array.Copy(nonceInBytes, concatenation, nonceInBytes.Length);
            Array.Copy(createdInBytes, 0, concatenation, nonceInBytes.Length, createdInBytes.Length);
            Array.Copy(secretInBytes, 0, concatenation, (nonceInBytes.Length + createdInBytes.Length), secretInBytes.Length);

            var sha1Hasher = new SHA1CryptoServiceProvider();
            var hashedDataBytes = sha1Hasher.ComputeHash(concatenation);

            var data = Convert.ToBase64String(hashedDataBytes);

            var dummyData = data.Trim().Replace(" ", "+");

            if ((dummyData.Length % 4) > 0)
            {
                dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
            }

            return dummyData;
        }
    }

    public class SecurityHeaderNonCrypt : MessageHeader
    {
        private string SystemUser { get; set; }

        private string SystemPassword { get; set; }

        public SecurityHeaderNonCrypt(string systemUser, string systemPassword)
        {
            SystemUser = systemUser;
            SystemPassword = systemPassword;
        }

        public override string Name => "Security";

        public override string Namespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            WriteHeader(writer);
        }

        private void WriteHeader(XmlDictionaryWriter writer)
        {
            var nonce = CreateNonce();

            var created = DateTime.UtcNow.AddSeconds(-10).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            writer.WriteStartElement("wsse", "UsernameToken", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            writer.WriteStartElement("wsse", "Username", null);
            writer.WriteString(SystemUser);
            writer.WriteEndElement();
            writer.WriteStartElement("wsse", "Password", null);
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest");
            writer.WriteString(SystemPassword);
            writer.WriteEndElement();
            writer.WriteStartElement("wsse", "Nonce", null);
            writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            writer.WriteBase64(nonce, 0, nonce.Length);
            writer.WriteEndElement();
            writer.WriteStartElement("wsu", "Created", null);
            writer.WriteString(created);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
        }

        private byte[] CreateNonce()
        {
            var rand = new RNGCryptoServiceProvider();
            var buf = new byte[0x10];
            rand.GetBytes(buf);
            return buf;
        }
    }

    public class HttpHeaderMessageInspector : IClientMessageInspector
    {
        private readonly Dictionary<string, string> _httpHeaders;

        public HttpHeaderMessageInspector(Dictionary<string, string> httpHeaders)
        {
            _httpHeaders = httpHeaders;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState) { }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;

            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out object httpRequestMessageObject))
            {
                httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;

                foreach (var httpHeader in _httpHeaders)
                {
                    if (httpRequestMessage != null)
                    {
                        httpRequestMessage.Headers[httpHeader.Key] = httpHeader.Value;
                    }
                }
            }
            else
            {
                httpRequestMessage = new HttpRequestMessageProperty();

                foreach (var httpHeader in _httpHeaders)
                {
                    httpRequestMessage.Headers.Add(httpHeader.Key, httpHeader.Value);
                }
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
            }

            return null;
        }
    }

    internal class HttpHeadersEndpointBehavior : IEndpointBehavior
    {
        private readonly Dictionary<string, string> _httpHeaders;

        public HttpHeadersEndpointBehavior(Dictionary<string, string> httpHeaders)
        {
            _httpHeaders = httpHeaders;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var inspector = new HttpHeaderMessageInspector(_httpHeaders);

            clientRuntime.ClientMessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }
        public void Validate(ServiceEndpoint endpoint) { }
    }
}
