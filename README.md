# Moolah

An easy to use library for e-commerce payment processing in .NET.

Support is currently provided for:
* DataCash MOTO and 3D-Secure card transactions
* PayPal Express Checkout

You can also install using [NuGet](http://nuget.org/Packages/Search?packageType=Packages&searchCategory=All+Categories&searchTerm=machine.specifications):
<pre>
  PM> Install-Package Moolah
</pre>

## Configuration
### Through Code
The examples below show how configuration of merchant credentials can be injected at runtime through code.

### App.Config or Web.Config
You also have the option of placing these values in your `web.config` or `app.config` file:

	<configSections>
		<section name="Moolah" type="Moolah.MoolahConfiguration, Moolah"/>
	</configSections>
	
	<Moolah xmlns="urn:MoolahConfiguration">
		<DataCashMoTo environment="Test" merchantId="motoMerchantId" password="motoPassword" />
		<DataCash3DSecure environment="Test" merchantId="3dsMerchantId" password="3dsPassword" merchantUrl="3dsMerchantUrl" purchaseDescription="3dsPurchaseDescription" />
		<PayPal environment="Test" userId="paypalUserId" password="paypalPassword" signature="paypalSignature" />
	</Moolah>

If you do this, you simply need to new up the gateway without the configuration class, and you're ready to go:

	var gateway = new DataCash3DSecureGateway();


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
	var response = gateway.SetExpressCheckout(12.99m, CurrencyCodeType.GBP, cancelUrl, confirmationUrl);
	if (response.Status == PaymentStatus.Failed)
		throw new Exception(response.FailureMessage);
	RedirectTo(response.RedirectUrl); 
	
#### PayPal Express Checkout (Advanced)

You may want to specify more than just the transaction amount to PayPal in order to populate the PayPal checkout and invoices with more useful details, well we support that to.

	var response = gateway.SetExpressCheckout(new OrderDetails
		{
			OrderDescription = "Thanks for your order!",
			Items = new[]
				{
					new OrderDetailsItem { Description = "A widget", Quantity = 2, UnitPrice = 1.99m, ItemUrl = "http://mysite.com/product?widget" },
					new OrderDetailsItem { Description = "Widget box", Quantity = 1, UnitPrice = 10m, ItemUrl = "http://mysite.com/product?widgetBox" }
				},
			Discounts = new []
				{
					new DiscountDetails { Description = "Loyalty discount", Amount = -0.99m }
				},
			ShippingTotal = 2m,
			OrderTotal = 14.99m,
			CurrencyCodeType = CurrencyCodeType.GBP
		}, cancelUrl, confirmationUrl);
	
#### Confirmation page

The customer is returned to the `confirmationUrl` you specified, to confirm payment.  
PayPal will add 2 query string parameters for you, `PayerID` and `token`.  
These values must be used when retrieving customer details, or performing the payment.

`GetExpressCheckoutDetails` will give you the customer details, including delivery address.
`DoExpressCheckoutPayment` will perform the PayPal payment.
	
	var checkoutDetails = gateway.GetExpressCheckoutDetails(Request["token"]);
	
	var response = gateway.DoExpressCheckoutPayment(12.99m, CurrencyCodeType.GBP, Request["token"], Request["PayerID"]);
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
