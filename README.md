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

#### Configuration

	var configuration = new PayPalConfiguration(PaymentEnvironment.Live, "userId", "password", "signature");
	var gateway = new PayPalExpressCheckout(configuration);

#### PayPal Express Checkout button (on basket page)

You must specify where the customer should be returned to if they cancel the process (`cancelUrl`) or if
they complete the checkout (`confirmationUrl`).
You should then redirect the customer to PayPal using the `response.RedirectUrl` provided.
	
	var cancelUrl = "http://yoursite.com/basket";
	var confirmationUrl = "http://yoursite.com/paypalconfirm";
	var response = gateway.SetExpressCheckout(12.99m, cancelUrl, confirmationUrl);
	if (response.Status == PaymentStatus.Failed)
		throw new Exception(response.FailureMessage);
	RedirectTo(response.RedirectUrl); 
	
#### Confirmation page

The customer is returned to the `confirmationUrl` you specified, to confirm payment.  
PayPal will add 2 query string parameters for you, `PayerID` and `token`.  
These values must be used when retrieving customer details, or performing the payment.

`GetExpressCheckoutDetails` will give you the customer details, including delivery address.
`DoExpressCheckoutPayment` will perform the PayPal payment.
	
	var checkoutDetails = gateway.GetExpressCheckoutDetails(Request["token"]);
	
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
