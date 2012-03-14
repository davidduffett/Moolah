using System;
using System.Web;
using NLog;

namespace Moolah.GoogleCheckout
{
    public class GoogleCheckoutGateway : IGoogleCheckout
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly GoogleCheckoutConfiguration _configuration;
        private readonly IGoogleCheckoutRequestBuilder _requestBuilder;

        public GoogleCheckoutGateway()
            : this(MoolahConfiguration.Current.GoogleCheckout)
        {
        }

        public GoogleCheckoutGateway(GoogleCheckoutConfiguration configuration)
            : this(configuration, new GoogleCheckoutRequestBuilder(configuration))
        {
        }

        public GoogleCheckoutGateway(GoogleCheckoutConfiguration configuration, IGoogleCheckoutRequestBuilder requestBuilder)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (requestBuilder == null) throw new ArgumentNullException("requestBuilder");
            _configuration = configuration;
            _requestBuilder = requestBuilder;
        }

        public string GoogleCheckoutButtonImage(ButtonSize size = ButtonSize.Small, ButtonStyle style = ButtonStyle.White)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("merchant_id", _configuration.MerchantId);

            var dimensions = size.Dimensions();
            queryString.Add("w", dimensions.Item1.ToString());
            queryString.Add("h", dimensions.Item2.ToString());

            queryString.Add("style", style.ToString().ToLower());

            queryString.Add("variant", "text");

            // TODO: Introduce different locales
            queryString.Add("loc", "en_GB");

            return _configuration.ButtonSrc + '?' + queryString;
        }

        public GoogleCheckoutRedirect RequestCheckout(ShoppingCart shoppingCart, CheckoutOptions options = null)
        {
            if (shoppingCart == null) throw new ArgumentNullException("shoppingCart");

            _logger.Log("RequestCheckout.Request", shoppingCart);
            var request = _requestBuilder.CreateRequest(shoppingCart);
            if (options != null)
                _requestBuilder.AddOptions(request, options);

            var response = request.Send();

            _logger.Log("RequestCheckout.Response", response);
            var redirect = new GoogleCheckoutRedirect
            {
                GoogleResponse = response.ResponseXml,
                RedirectUrl = response.RedirectUrl,
                Status = response.IsGood ? PaymentStatus.Pending : PaymentStatus.Failed
            };

            if (!response.IsGood)
            {
                redirect.IsSystemFailure = true;
                redirect.FailureMessage = response.ErrorMessage;
            }

            return redirect;
        }
    }
}
