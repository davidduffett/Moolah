using GCheckout.Util;

namespace Moolah.Specs.GoogleCheckout
{
    public class FakeGCheckoutResponse : GCheckoutResponse
    {
        private readonly bool _isGood;
        private readonly string _redirectUrl;
        private readonly string _errorMessage;

        public FakeGCheckoutResponse(bool isGood, string responseXml, string redirectUrl = null, string errorMessage = null)
            : base(responseXml)
        {
            _isGood = isGood;
            _redirectUrl = redirectUrl;
            _errorMessage = errorMessage;
        }

        public FakeGCheckoutResponse(string responseXml) : base(responseXml)
        {
        }

        protected override bool ParseMessage()
        {
            return true;
        }

        public override bool IsGood
        {
            get { return _isGood; }
        }

        public override string RedirectUrl
        {
            get { return _redirectUrl; }
        }

        public override string ErrorMessage
        {
            get { return _errorMessage; }
        }
    }
}