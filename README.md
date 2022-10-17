# Authorization with External Service

This is a sample that shows how we can implement additional authorization requirements with an external authorization service. In this example, we have an API (Contoso.API) secured with Azure AD. However, that is an additional authorization check with a custom API (Contoso.Security.API) which returns a payload describing whether this particular client app can invoke the GetWeather API.

## Disclaimer

The information contained in this README.md file and any accompanying materials (including, but not limited to, scripts, sample codes, etc.) are provided "AS-IS" and "WITH ALL FAULTS." Any estimated pricing information is provided solely for demonstration purposes and does not represent final pricing and Microsoft assumes no liability arising from your use of the information. Microsoft makes NO GUARANTEES OR WARRANTIES OF ANY KIND, WHETHER EXPRESSED OR IMPLIED, in providing this information, including any pricing information.

## Get Started

1. Create an App Registration in your AAD Tenant. Be sure to assign it an AppRole. Under API permissions, add the AppRole as a permission and grant Admin consent. Note that in this setup, this App Registration represents both the API and the Client invoking the API. If you like, you can create 2 App Registrations. If you are using this setup, be sure to only perform the API permissions, add AppRole as a permission step for only the Client. Only the Client App Registration requires a Client Secret to be generated.

2. Configure the Contoso.API with the following:

    ```json
    {
        "AzureAd": {
            "Instance": "https://login.microsoftonline.com/",
            "Domain": "<Tenant name from AAD properties>.onmicrosoft.com",
            "TenantId": "<Tenant Id from AAD properties>",
            "ClientId": "<Client Id from App Registration representing the API>"
        },
        "Logging": {
            "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
            }
        },
        "AllowedHosts": "*"
    }
    ```

3. Configure the Contoso.Security.API with the following:

    ```json
    {
        "Logging": {
            "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
            }
        },
        "AllowedHosts": "*",
        "AllowedClients": [
            "<Use the appropriate Client Id representing the Client calling the API>"
        ]
    }
    ```

4. Import the ContosoAPI.postman_collection.json file into Postman and configure an Environment with the following:

    * ClientId: Client Id from App Registration representing the Client calling the API.
    * ClientSecret: Client Secret from App Registration representing the Client calling the API.
    * TenantId: Tenant Id from AAD properties

5. Now we can run the Solution and use Postman to invoke the API. You can add breakpoints in the Contoso.Security.API in SecurityPolicyController and observe the Client Id is being passed in that is used to assert whether it is allowed to Get Weather.

## Have an issue?

You are welcome to create an issue if you need help but please note that there is no timeline to answer or resolve any issues you have with the contents of this project. Use the contents of this project at your own risk! If you are interested to volunteer to maintain this, please feel free to reach out to be added as a contributor and send Pull Requests (PR).
