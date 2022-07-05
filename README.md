# Contract Testing

This is a POC to verify the possibility to have a better strategy to detect breaking changes in APIs.

This solution contains a simple Web API application (`PactNETPlayground.Producer`), and a client application consuming its APIs (`PactNETPlayground.Consumer`);

## Producer

> ⚠️ **IMPORTANT**
> 
> You can't use the Microsoft.AspNetCore.Mvc.Testing library to host your API for provider tests. If your tests are using TestServer or WebApplicationFactory then these are running the API with a special in-memory test server instead of running on a real TCP socket. This means the Rust internals can't call the API and therefore all of your provider tests will fail. You must host the API on a proper TCP socket, e.g. by using the Host method shown in the sample above, so that they can be called from non-.Net code.

[Source](https://github.com/pact-foundation/pact-net/blob/master/README.md)

## How do I know which tests to run?

There are multiple ways to get the tests to run

- `WithFileSource`
  
  This is probably the simplest scenario: a consumer generates its file and simply shares it with the producer; the producer references the specific file explicitly and run the requests against the current implementation. 
  
- `WithDirectorySource`
- `WithUriSource`

- `WithPactBrokerSource`
  
  From [Pact Broker repository](https://github.com/pact-foundation/pact_broker)

  > The Pact Broker is an application for sharing of consumer driven contracts and verification results. It is optimised for use with "pacts" (contracts created by the Pact framework), but can be used for any type of contract that can be serialized to JSON. 
  
  Can be used in different ways

  - Hosted, e.g. [PactFlow](https://pactflow.io/)
  - [Containerized](https://github.com/pact-foundation/pact-broker-docker)
  - In house, needs ruby and PostgreSQL (or MySQL)
  

