# Moolah

An easy to use library for e-commerce payment processing which will eventually have support for multiple payment providers.

To begin with, it will support DataCash (MOTO and 3D-Secure).

## DataCash (MoTo) Example

    using System;
    using Moolah;
    using Moolah.DataCash;
    
    namespace GiveMeMoolah
    {
        class Program
        {
            static void Main(string[] args)
            {
                var configuration = new DataCashConfiguration(PaymentEnvironment.Test, "merchantId", "password");
                var gateway = new DataCashMoToGateway(configuration);
                var response = gateway.Payment("your_transaction_reference", 12.99m,
                                               new CardDetails
                                                   {
                                                       Number = "1000010000000007",
                                                       ExpiryDate = "10/15",
                                                       Cv2 = "123"
                                                   });
    
                if (response.Status == PaymentStatus.Failed)
                {
                    if (response.IsSystemFailure)
                        // System failures can indicate issues like a configuration problem
                        // eg. "The vTID or password were incorrect"
                        throw new Exception(response.FailureMessage);
                    else
                        // Non-system failure messages can be shown directly to the customer
                        // eg. "Transaction was declined by your bank."
                        Console.WriteLine(response.FailureMessage);
                }
                else
                    Console.WriteLine("Transaction successful! DataCash Reference: {0}", response.TransactionReference);
            }
        }
    }
