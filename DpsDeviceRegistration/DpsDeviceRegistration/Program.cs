using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;

namespace DpsDeviceRegistration
{
	internal class Program
    {
        static async Task Main(string[] args)
        {
            var globalDeviceEndpoint = "global.azure-devices-provisioning.net";
            
            // ToDo: following 3 details needs to be update as per device registration API response.
            var deviceRegistrationId = "";
            var idScope = "";
            var deviceDerivedKey = "";

            var securityProvider = new SecurityProviderSymmetricKey(deviceRegistrationId, deviceDerivedKey, deviceDerivedKey);

            var transport = new ProvisioningTransportHandlerAmqp(TransportFallbackType.TcpOnly);

            var client = ProvisioningDeviceClient.Create(globalDeviceEndpoint, idScope, securityProvider, transport);


            var deviceRegistrationResult = await client.RegisterAsync();

            Console.WriteLine($"Provisioning result: {deviceRegistrationResult.Status}");

            if (deviceRegistrationResult.Status != ProvisioningRegistrationStatusType.Assigned)
            {
                throw new InvalidOperationException("Something went wrong while trying to provision.");
            }

            if (!string.IsNullOrEmpty(deviceRegistrationResult.AssignedHub))
            {
                Console.WriteLine($"Assigned to hub : {deviceRegistrationResult.AssignedHub}");
            }

            Console.ReadLine();
        }
    }
}