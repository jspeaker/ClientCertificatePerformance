# Client Certificate Performance

This is a POC project to demonstrate caching of Client Certificate Validation results and the subsequent performance gains.

Trust Chain Validation is a very expensive operation. The creation of an X509Chain and ChainPolicy takes somewhere in the neighborhood of 150-200 ms. In addition, the verification of the Certificate based on that chain, takes an additional 100-150 ms. Adding an additional 250-350 ms to every API call is, of course, unacceptable in almost all circumstances.

This project's means of mitigating that performance hit is specifically oriented to the validation of client certificates that are within the control of the organization, i.e. service-to-service. It must be noted that this solution *does not* address validation concerns where the identity of end-user client applications is the aim.

###Getting Started

 1. Installation Process
   - Clone the solution.
   - Build the solution.
   - Run the tests.
   - Modify the Publish Profile to publish the API to your Azure tenant.
 2. Software Dependencies
  - All dependencies are included in the solution as Nuget packages.
 3. Latest Releases
  - n/a
 4. API References
  - n/a
