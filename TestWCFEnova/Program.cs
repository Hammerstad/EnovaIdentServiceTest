using System;
using System.ServiceModel;
using System.Threading.Tasks;
using IdentService;

namespace TestWCFEnova
{
    class Program
    {
        private static RegisterenhetsrettsandelService _registerenhetsrettsandelService;

        private static async Task Main()
        {
            try
            {
                var pw = "insertpasswordhere";
                var username = "insertuserhere";
                var address = "https://etgltest.grunnbok.no:443/grunnbok/wsapi/v2/RegisterenhetsrettsandelServiceWS?WSDL";

                var binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                binding.MaxReceivedMessageSize =
                    65536 * 32; // This may have to be increased if return object(s) are too large
                var endpointAddress = new EndpointAddress(address);
                var factory = new ChannelFactory<RegisterenhetsrettsandelService>(binding, endpointAddress);
                factory.Credentials.UserName.UserName = username;
                factory.Credentials.UserName.Password = pw;
                _registerenhetsrettsandelService = factory.CreateChannel();

                var x = await _registerenhetsrettsandelService.findAndelerForRettighetshavereAsync(
                    new findAndelerForRettighetshavereRequest(
                        new findAndelerForRettighetshavereRequestBody(new PersonIdList
                        {
                            new() {value = "insertpersonidhere"}
                        }, Context)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static GrunnbokContext Context
        {
            get
            {
                // This timestamp represents valid data, not the date of the data.
                // This timestamp equals "9999-01-01T00:00:00.00+01:00"
                var gjeldende = new Timestamp { timestamp = new DateTime(9999, 1, 1, 0, 0, 0, DateTimeKind.Local) };
                var context = new GrunnbokContext
                {
                    clientIdentification = "Enova: Energimerkesystemet",
                    locale = "nb_NO",
                    snapshotVersion = gjeldende,
                    systemVersion = "1.0"
                };
                return context;
            }
        }
    }
}
