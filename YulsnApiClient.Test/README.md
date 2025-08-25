# Local tests for the API

Create a new file named `local.settings.json` in the `YulsnApiClient.Test` project directory. This file should contain your local configuration settings for testing the API client.

The following settings are used by the YulsnClient:

- yulsn-api-key
	- the API key, required
- yulsn-api-host
	- sets the base URL for the API, defaults to `api.yulsn.com`
- yulsn-api-accountid
	- needed for v2
- yulsn-api-v1-baseurl
	- overrides the base URL for v1 endpoints
- yulsn-api-v2-baseurl
	- overrides the base URL for v2 endpoints

Here is an example of what the `local.settings.json` file might look like:
```json
{
  "yulsn-api-key": "your-api"
}
```

## Running the Tests
- When the api is running, run the tests from the console
- Navigate to the `YulsnApiClient.Test` directory and run the following command:

Run all the test:

```
dotnet test
```

Or run a specific test:

```
dotnet test --filter "FullyQualifiedName~YulsnApiClient.Test.StoresTest.GetStoreById"
```