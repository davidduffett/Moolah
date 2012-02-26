# Moolah

An easy to use library for e-commerce payment processing which will eventually have support for multiple payment providers.

To begin with, it will support DataCash (MOTO and 3D-Secure).

You can also install using [NuGet](http://nuget.org/Packages/Search?packageType=Packages&searchCategory=All+Categories&searchTerm=machine.specifications):
<pre>
  PM> Install-Package Moolah
</pre>

## Examples
### DataCash (MoTo)

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
			DisplayError(response.FailureMessage);
	}
	else
		DisplayMessage("Transaction successful! DataCash Reference: {0}", response.TransactionReference);


### DataCash (3D-Secure)

#### Configuration

3D-Secure requires you to provide your site URL and a description of the products you sell.

	var configuration = new DataCash3DSecureConfiguration(PaymentEnvironment.Live, "merchantId", "password",
								"http://yoursite.com", "Description of products you sell");
	var gateway = new DataCash3DSecureGateway(configuration);
	
#### Payment

If the card is not enrolled in 3D-Secure, payment will be authorised immediately.
If it is enrolled, you will need to show the 3D-Secure iframe and perform a POST to the URL
specified in `response.ACSUrl`.  This POST must include the `response.PAReq` value provided by Moolah.

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
			DisplayError(response.FailureMessage);
	}
	else if (response.Requires3DSecurePayerVerification)
	{
		// Show 3D-Secure iframe and post the response.PAReq value to response.ACSUrl.
	}
	else
		DisplayMessage("Transaction successful! DataCash Reference: {0}", response.TransactionReference);

#### Authorising the transaction after 3D-Secure verification
		
When customer returns to your site after 3D-Secure, you will be provided with a "PARes" value.
You will also need the original `response.TransactionReference` value provided when you called the `Payment` method.

	var response = gateway.Authorise(transactionReference, PARes);
	if (response.Status == PaymentStatus.Failed)
	{
		if (response.IsSystemFailure)
			// System failures can indicate issues like a configuration problem
			// eg. "The vTID or password were incorrect"
			throw new Exception(response.FailureMessage);
		else
			// Non-system failure messages can be shown directly to the customer
			// eg. "Transaction was declined by your bank."
			DisplayError(response.FailureMessage);
	}
	else
		DisplayMessage("Transaction successful! DataCash Reference: {0}", response.TransactionReference);
	
		
### PayPal Express Checkout

	// Configuration
	var configuration = new PayPalConfiguration(PaymentEnvironment.Live, "userId", "password", "signature");
	var gateway = new PayPalExpressCheckout(configuration);
	
	// PayPal Express Checkout button (on basket page)
	var cancelUrl = "http://yoursite.com/basket"; // Where the customer returns to if they cancel
	var confirmationUrl = "http://yoursite.com/paypalconfirm"; // Where the customer returns to confirm their order
	var token = gateway.SetExpressCheckout(12.99m, cancelUrl, confirmationUrl);
	if (token.Status == PaymentStatus.Failed)
		throw new Exception(token.FailureMessage);
	RedirectTo(token.RedirectUrl); // Redirect the customer to PayPal
	
	// When the customer returns to the confirmation page, PayPal will provide the PayerID and token.
	// eg. http://yoursite.com/paypalconfirm?PayerID=*PayPalPayerId*&token=*PayPalToken*
	// Get customer details, including delivery address:
	var checkoutDetails = gateway.GetExpressCheckoutDetails(Request["PayPalToken"]);
	
	// Confirm the payment:
	var response = gateway.DoExpressCheckoutPayment(12.99m, Request["token"], Request["PayerID"]);
	if (response.Status == PaymentStatus.Failed)
	{
		if (response.IsSystemFailure)
			// System failures can indicate issues like a configuration problem
			throw new Exception(response.FailureMessage);
		else
			// Non-system failure messages can be shown directly to the customer
			DisplayError(response.FailureMessage);
	}
	else
		DisplayMessage("Transaction successful! PayPal Reference: {0}", response.TransactionReference);
