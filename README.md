# Contract Testing

This is a POC to verify the possibility to have a better strategy to detect breaking changes in APIs.

This solution contains a simple Web API application (`PactNETPlayground.Producer`), and a client application consuming its APIs (`PactNETPlayground.Consumer`);

### How it should work

The central idea behind this approach is to have the ability to verify, at any time, if changes in API definitions will break any existing consumer. To achieve that, a consumer is expected to create tests for the URIs it is (or will be) using, using a mock server and valid mock responses. 

Pact.NET provides a set of tools and libraries which aim at standardize this process, e.g.

- A ready to use mock server implementation that can process prepared requests and return mocked responses: this is supposed to be use by consumers when writing tests to validate contracts
- A standard format to share these scenarios between consumers and producers
- A framework that allow producers to re-play scenarios previously generated and shared by consumers, to verify changes are compatible with existing consumers
- Ability to version these generated contract files, allowing producers to test their changes against multiple versions of consumers' implementations. 

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
  
## What are provider states?

When pact files are generated, the string used in `Given(...)` is then used when a producer verifies a scenario to set or restore a state that is consistent with the expected one; this is achieved using a custom middleware, which can run a proper action.

## How can we handle authentication?

For simplicity, in current implementation, a fake authority is used to validate token generated using a dedicated component (`TokenProvider`): there is no actual token service, and tokens are verified using a prefixed shared key.

When pact file is generated, a fake token is added (the mock server used at that stage does not care about tokens anyway); this token is saved as part of a scenario, but is clearly invalid (using a valid token is not a viable option, either, as it would expire sooner or later).

When we configure the test server, a custom middleware is added (`BearerTokenReplacementMiddleware`): this component search for a token in the request, and if one is found, its value is replaced with a freshly generated token, so that authentication can still work as expected.

> We first search for a token, and then replace the existing one because we may still want to write a test for unauthorized requests (not sure this is in the scope of contract testing though, I'll think a but more about that)

## Matchers

**TODO**


