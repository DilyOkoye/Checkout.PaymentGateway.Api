# Checkout.PaymentGateway API
###### Responsible for validating requests, storing card information and forwarding payment requests and accepting payment responses to and from the acquiring bank.

#### ✨Deliverables ✨
1. Build an API that allows a merchant:
a. To process a payment through your payment gateway.
b. To retrieve details of a previously made payment.
2. Build a bank simulator to test your payment gateway API.

#### ✨Assumptions ✨
- MerchantId and PaymentReference is known and passed from the merchant
- Acquiring Bank Simulator returns failure for certain blacklisted cards and some locations that are not within the allowable list
- Transaction Status are Settled,Settling and Declined where settled means the transaction has completed successfully from the acquiring bank simulator, settling are transactions that are still in the process and later be settled or decline and lastly decline represents a failed transaction.
- Authentication is currently set to false via a key/value called IsEnabled
- Setting this to false ensures that the request is authenticated

#### Flow Diagram
![alt text](https://www.websequencediagrams.com/cgi-bin/cdraw?lz=dGl0bGUgQ2hlY2tvdXRQYXltZW50R2F0ZXdheQoKcGFydGljaXBhbnQgTWVyY2hhbnQACA0AIg8AJAxBY3F1aXJpbmcgQmFuayAKCgoAOggtPgBaDjogUE9TVCAtIENyZWF0ZSBuZXcgcACBBAYgLSAvAIEOB3MKAIEQDi0-AFQOOnZhbGlkYXRlcyByZXF1ZXN0IGFuZCBQdWJsaXNoZXMgdGhlABEJCgCBEA4tAIEAE3JvY2VzcwApD2FuZCByZXR1cm5zIDIwMCB3aXRoIHN0YXR1cyBTZXR0bGVkL0RlY2xpbmVkAIEgEC0-AII7CDogUgA7CTEgYwCBcQVkAEYGAEAFaW5nIFMAUAUKAIIaHUdFVCAtIFJldHJpZXZlAIIgFC97aWR9AIIZICBHZXQAgmgJaW5mb3JtYXRpb24AggMicmVzcG9uc2Ugb2YALBUAgWcbABwf&s=default)

#### Software Design Approach
[Domain Driven Design](https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/february/best-practice-an-introduction-to-domain-driven-design/) 
Mediator Design Pattern in C#

#### Implementation Strategy
ProcessPayment Controller that houses:-
- NewPayment
- RetrievePayment
#### Packages
- [language-ext](https://github.com/louthy/language-ext/) for functional programming 
- mediator implementation in .NET with query-command segregation; fits the functional paradigm quite nicely
- Swashbuckle(Comes directly in .Net 6)
- Microsoft.Extensions.Caching.Abstraction as a form of storage on memory
- Moq for mocking test 
- xunit for test

#### Running the Application
- .NET 6
- Docker where applicable
The application can be ran via:-
> Setting the Checkout.PaymentGateway.Api as the startup project and hit f5 to launch 
- Swagger doc included as a documentation guide
- Sample request located in the Checkout.PaymentGateway.Api/Assets

#### Project Structure
- Checkout.PaymentGateway.Api
- Checkout.PaymentGateway.Application
- Checkout.PaymentGateway.Domain
- Checkout.PaymentGateway.Persistence
- Checkout.PaymentGateway.Api.Tests
- Checkout.PaymentGateway.Application.Tests
- Checkout.PaymentGateway.Persistence.Tests
- AcquiringBank.Simulator

#### Checkout.PaymentGateway.Api
This project is the entry point api of the solution for handling the two important request
- POST/Payment NewPayment
- GET/Payment/{id}

**Sample request for NewPayment**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa4",
  "merchantId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 20,
  "payload": "test",
  "paymentDate": "2022-10-24T10:27:03.167Z",
  "currencyIso": "GBP",
  "card": {
    "holderName": "Dilichukwu Okoye",
    "number": "4165490075885000",
    "expiryDate": "01/2027",
    "cvv": "322",
    "billingInformation": "55 Roland Stree, Newark, NG244PZ, United Kingdom"
  },
  "countryCode": "GB",
  "description": "Payment of goods"
}
```

**Sample Successful Response**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa4",
  "amount": 20,
  "payload": "test",
  "paymentDate": "2022-10-24T10:27:03.167Z",
  "currencyIso": "GBP",
  "cardInfo": {
    "holderName": "Dilichukwu Okoye",
    "number": "4165490075885000",
    "expiryDate": "01/2027",
    "cvv": "322",
    "billingInformation": "55 Roland Stree, Newark, NG244PZ, United Kingdom"
  },
  "status": "Settling"
}
```
**Sample Failure Reponses:400**
```json
{
 [
  {
    "title": "Amount must be set and be greater than zero"
  }
]
}
```

```json
{
 [
  {
    "title": "payment reference 3fa85f64-5717-4562-b3fc-2c963f66afa6 must be a valid payment reference"
  }
]
}
```

```json
{
 [
  {
    "title": "Currency must be set"
  }
]
}
```
```json
{
 [
  {
    "title": "Card Information is not Valid"
  }
]
}
```

```json
{
 [
  {
    "title": "payment reference 3fa85f64-5717-4562-b3fc-2c963f66afa4 already exists"
  }
]
}
```
**GET/Payment/{id}**
Retrieves the previous transaction in the memory cache

**Sample Response**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa4",
  "amount": 20,
  "payload": "test",
  "paymentDate": "2022-10-24T10:27:03.167Z",
  "currencyIso": "GBP",
  "cardInfo": {
    "holderName": "Dilichukwu Okoye",
    "number": "4165490075885000",
    "expiryDate": "01/2027",
    "cvv": "322",
    "billingInformation": "55 Roland Stree, Newark, NG244PZ, United Kingdom"
  },
  "status": "Settled"
}
```

**Error**: response status is 404 for payment reference not found

Checkout.PaymentGateway.Application
This class library contains the commandHandler implmentation,Event handlers, Queries, request validations and response viewModels

**Checkout.PaymentGateway.Domain**
This is where the concepts of the business domain are. This layer has all the information about the business case and the business rules. Here’s where the entities are. 

**Checkout.PaymentGateway.Persistence**
This class library contains the repositories; interfaces and implementation of the saved request via MemoryCache

**Checkout.PaymentGateway.Api.Tests**
This project covers the unit tests for the two enpoints in the ProcessPayment Controller alongside with the different possible result returned by ToActionResult() 

**Checkout.PaymentGateway.Application.Tests**
This project test the different scenarios for the handlers and behaviours of the different lang ext package used

**Checkout.PaymentGateway.Pesistence.Tests**
This project covers the Repository Test via MemoryCache

**AcquiringBank.Simulator**
This class library is a responsible for simulating the responses from a bank.
It returns a successful response except for scenarios such as using a blacklisted card or where the transation country code is not amongst a list of included countries.

#### ✨Extra  Miles✨ 
Custom Authentication
The solution contains a custom authentication which by default is disabled via a feature flag
```json
"SecurityConfiguration": {
    "IsEnabled": false,
    "IdentitySecrets": {
      "payments": "535bfa52-2122-4fb4-bc22-761b54661dil"
    }
  }
```
The authetication is injected via SwaggerDoc with the inclusion of an identityHeaderOperationFilter and MacHeaderOperationFilter
This in practical terms involves passing in an identity and mac via swagger.
The identity is a unique key  used with the mac to identify which secret the Checkout API will use when calculating the hash, ideally shared between the Merchant and PaymentGateway while the mac is a hash value using the SHA256 algorithm of the request body, UTC timestamp and secret, for example data being {"Test": true}2019110812secret the hash would be 9d5d046f04ea07ab53727c6cfe40c18ad9cddd20d1cd1b5ef240bb22f7b72946, please note if HTTP method used is GET or there is no body create the hash with an empty body.

Functional programming via Language-ext
This paradigm encourages and promotes:-
- Write less code and more meaningful one : improved code clarity
- Avoid side effects by using immutability and pure functions : Easier concurrent programming
- Easier testing : it’s really easy to test functions
- Easier debugging

**Areas of Improvement**
- A proper database infrastructure would be ideal instead of an in-memory state
- A proper queueing in place where once the payment or transaction is saved successfully in a database and a subsequently published to a queue, it can be subscribed via another instance to complete the transaction process
- A much more robust test case alongside with an automated unit test, typical scenarios such as an automated load test to ensure that the api can handle and respond to a specific threashold of request.
- Efficient file logging that can be fed to various data monitoring tools like Datadog

#### ✨Proposed Cloud Technolgy
Amazon AWS would be my preferrd choice. Due to the potential volume of transaction that would be expected in a typical payment gateway which typically involves processing thousands-millions of request, Amazon Simple Queue Service (SQS) is a fantastic fully managed message queuing service that can reliabily and continuously exchange volumes of request from anywhere in a secured way between publishers and consumers