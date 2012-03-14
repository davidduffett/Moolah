using System;

namespace Moolah.GoogleCheckout
{
    /// <summary>
    /// The Google Checkout button is only available in 3 preset sizes.
    /// </summary>
    public enum ButtonSize
    {
        /// <summary>
        /// 160px x 43px
        /// </summary>
        Small,

        /// <summary>
        /// 168px x 44px
        /// </summary>
        Medium,

        /// <summary>
        /// 180px x 46px
        /// </summary>
        Large
    }

    public static class ButtonSizes
    {
        public static Tuple<int, int> Dimensions(this ButtonSize buttonSize)
        {
            switch (buttonSize)
            {
                case ButtonSize.Large:
                    return Tuple.Create(180, 46);
                case ButtonSize.Medium:
                    return Tuple.Create(168, 44);
                case ButtonSize.Small:
                default:
                    return Tuple.Create(160, 43);
            }
        }
    }
}