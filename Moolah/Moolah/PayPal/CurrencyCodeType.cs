using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moolah.PayPal
{
    public enum CurrencyCodeType
    {
        /// <summary>
        /// United Arab Emirates Dirham
        /// </summary>
        AED,
        /// <summary>
        /// Afghanistan Afghani
        /// </summary>
        AFN,
        /// <summary>
        /// Albania Lek
        /// </summary>
        ALL,
        /// <summary>
        /// Armenia Dram
        /// </summary>
        AMD,
        /// <summary>
        /// Netherlands Antilles Guilder
        /// </summary>
        ANG,
        /// <summary>
        /// Angola Kwanza
        /// </summary>
        AOA,
        /// <summary>
        /// Argentina Peso
        /// </summary>
        ARS,
        /// <summary>
        /// Australia Dollar
        /// </summary>
        AUD,
        /// <summary>
        /// Aruba Guilder
        /// </summary>
        AWG,
        /// <summary>
        /// Azerbaijan New Manat
        /// </summary>
        AZN,
        /// <summary>
        /// Bosnia and Herzegovina Convertible Marka
        /// </summary>
        BAM,
        /// <summary>
        /// Barbados Dollar
        /// </summary>
        BBD,
        /// <summary>
        /// Bangladesh Taka
        /// </summary>
        BDT,
        /// <summary>
        /// Bulgaria Lev
        /// </summary>
        BGN,
        /// <summary>
        /// Bahrain Dinar
        /// </summary>
        BHD,
        /// <summary>
        /// Burundi Franc
        /// </summary>
        BIF,
        /// <summary>
        /// Bermuda Dollar
        /// </summary>
        BMD,
        /// <summary>
        /// Brunei Darussalam Dollar
        /// </summary>
        BND,
        /// <summary>
        /// Bolivia Boliviano
        /// </summary>
        BOB,
        /// <summary>
        /// Brazil Real
        /// </summary>
        BRL,
        /// <summary>
        /// Bahamas Dollar
        /// </summary>
        BSD,
        /// <summary>
        /// Bhutan Ngultrum
        /// </summary>
        BTN,
        /// <summary>
        /// Botswana Pula
        /// </summary>
        BWP,
        /// <summary>
        /// Belarus Ruble
        /// </summary>
        BYR,
        /// <summary>
        /// Belize Dollar
        /// </summary>
        BZD,
        /// <summary>
        /// Canada Dollar
        /// </summary>
        CAD,
        /// <summary>
        /// Congo/Kinshasa Franc
        /// </summary>
        CDF,
        /// <summary>
        /// Switzerland Franc
        /// </summary>
        CHF,
        /// <summary>
        /// Chile Peso
        /// </summary>
        CLP,
        /// <summary>
        /// China Yuan Renminbi
        /// </summary>
        CNY,
        /// <summary>
        /// Colombia Peso
        /// </summary>
        COP,
        /// <summary>
        /// Costa Rica Colon
        /// </summary>
        CRC,
        /// <summary>
        /// Cuba Convertible Peso
        /// </summary>
        CUC,
        /// <summary>
        /// Cuba Peso
        /// </summary>
        CUP,
        /// <summary>
        /// Cape Verde Escudo
        /// </summary>
        CVE,
        /// <summary>
        /// Czech Republic Koruna
        /// </summary>
        CZK,
        /// <summary>
        /// Djibouti Franc
        /// </summary>
        DJF,
        /// <summary>
        /// Denmark Krone
        /// </summary>
        DKK,
        /// <summary>
        /// Dominican Republic Peso
        /// </summary>
        DOP,
        /// <summary>
        /// Algeria Dinar
        /// </summary>
        DZD,
        /// <summary>
        /// Egypt Pound
        /// </summary>
        EGP,
        /// <summary>
        /// Eritrea Nakfa
        /// </summary>
        ERN,
        /// <summary>
        /// Ethiopia Birr
        /// </summary>
        ETB,
        /// <summary>
        /// Euro Member Countries
        /// </summary>
        EUR,
        /// <summary>
        /// Fiji Dollar
        /// </summary>
        FJD,
        /// <summary>
        /// Falkland Islands (Malvinas) Pound
        /// </summary>
        FKP,
        /// <summary>
        /// United Kingdom Pound
        /// </summary>
        GBP,
        /// <summary>
        /// Georgia Lari
        /// </summary>
        GEL,
        /// <summary>
        /// Guernsey Pound
        /// </summary>
        GGP,
        /// <summary>
        /// Ghana Cedi
        /// </summary>
        GHS,
        /// <summary>
        /// Gibraltar Pound
        /// </summary>
        GIP,
        /// <summary>
        /// Gambia Dalasi
        /// </summary>
        GMD,
        /// <summary>
        /// Guinea Franc
        /// </summary>
        GNF,
        /// <summary>
        /// Guatemala Quetzal
        /// </summary>
        GTQ,
        /// <summary>
        /// Guyana Dollar
        /// </summary>
        GYD,
        /// <summary>
        /// Hong Kong Dollar
        /// </summary>
        HKD,
        /// <summary>
        /// Honduras Lempira
        /// </summary>
        HNL,
        /// <summary>
        /// Croatia Kuna
        /// </summary>
        HRK,
        /// <summary>
        /// Haiti Gourde
        /// </summary>
        HTG,
        /// <summary>
        /// Hungary Forint
        /// </summary>
        HUF,
        /// <summary>
        /// Indonesia Rupiah
        /// </summary>
        IDR,
        /// <summary>
        /// Israel Shekel
        /// </summary>
        ILS,
        /// <summary>
        /// Isle of Man Pound
        /// </summary>
        IMP,
        /// <summary>
        /// India Rupee
        /// </summary>
        INR,
        /// <summary>
        /// Iraq Dinar
        /// </summary>
        IQD,
        /// <summary>
        /// Iran Rial
        /// </summary>
        IRR,
        /// <summary>
        /// Iceland Krona
        /// </summary>
        ISK,
        /// <summary>
        /// Jersey Pound
        /// </summary>
        JEP,
        /// <summary>
        /// Jamaica Dollar
        /// </summary>
        JMD,
        /// <summary>
        /// Jordan Dinar
        /// </summary>
        JOD,
        /// <summary>
        /// Japan Yen
        /// </summary>
        JPY,
        /// <summary>
        /// Kenya Shilling
        /// </summary>
        KES,
        /// <summary>
        /// Kyrgyzstan Som
        /// </summary>
        KGS,
        /// <summary>
        /// Cambodia Riel
        /// </summary>
        KHR,
        /// <summary>
        /// Comoros Franc
        /// </summary>
        KMF,
        /// <summary>
        /// Korea (North) Won
        /// </summary>
        KPW,
        /// <summary>
        /// Korea (South) Won
        /// </summary>
        KRW,
        /// <summary>
        /// Kuwait Dinar
        /// </summary>
        KWD,
        /// <summary>
        /// Cayman Islands Dollar
        /// </summary>
        KYD,
        /// <summary>
        /// Kazakhstan Tenge
        /// </summary>
        KZT,
        /// <summary>
        /// Laos Kip
        /// </summary>
        LAK,
        /// <summary>
        /// Lebanon Pound
        /// </summary>
        LBP,
        /// <summary>
        /// Sri Lanka Rupee
        /// </summary>
        LKR,
        /// <summary>
        /// Liberia Dollar
        /// </summary>
        LRD,
        /// <summary>
        /// Lesotho Loti
        /// </summary>
        LSL,
        /// <summary>
        /// Lithuania Litas
        /// </summary>
        LTL,
        /// <summary>
        /// Latvia Lat
        /// </summary>
        LVL,
        /// <summary>
        /// Libya Dinar
        /// </summary>
        LYD,
        /// <summary>
        /// Morocco Dirham
        /// </summary>
        MAD,
        /// <summary>
        /// Moldova Leu
        /// </summary>
        MDL,
        /// <summary>
        /// Madagascar Ariary
        /// </summary>
        MGA,
        /// <summary>
        /// Macedonia Denar
        /// </summary>
        MKD,
        /// <summary>
        /// Myanmar (Burma) Kyat
        /// </summary>
        MMK,
        /// <summary>
        /// Mongolia Tughrik
        /// </summary>
        MNT,
        /// <summary>
        /// Macau Pataca
        /// </summary>
        MOP,
        /// <summary>
        /// Mauritania Ouguiya
        /// </summary>
        MRO,
        /// <summary>
        /// Mauritius Rupee
        /// </summary>
        MUR,
        /// <summary>
        /// Maldives (Maldive Islands) Rufiyaa
        /// </summary>
        MVR,
        /// <summary>
        /// Malawi Kwacha
        /// </summary>
        MWK,
        /// <summary>
        /// Mexico Peso
        /// </summary>
        MXN,
        /// <summary>
        /// Malaysia Ringgit
        /// </summary>
        MYR,
        /// <summary>
        /// Mozambique Metical
        /// </summary>
        MZN,
        /// <summary>
        /// Namibia Dollar
        /// </summary>
        NAD,
        /// <summary>
        /// Nigeria Naira
        /// </summary>
        NGN,
        /// <summary>
        /// Nicaragua Cordoba
        /// </summary>
        NIO,
        /// <summary>
        /// Norway Krone
        /// </summary>
        NOK,
        /// <summary>
        /// Nepal Rupee
        /// </summary>
        NPR,
        /// <summary>
        /// New Zealand Dollar
        /// </summary>
        NZD,
        /// <summary>
        /// Oman Rial
        /// </summary>
        OMR,
        /// <summary>
        /// Panama Balboa
        /// </summary>
        PAB,
        /// <summary>
        /// Peru Nuevo Sol
        /// </summary>
        PEN,
        /// <summary>
        /// Papua New Guinea Kina
        /// </summary>
        PGK,
        /// <summary>
        /// Philippines Peso
        /// </summary>
        PHP,
        /// <summary>
        /// Pakistan Rupee
        /// </summary>
        PKR,
        /// <summary>
        /// Poland Zloty
        /// </summary>
        PLN,
        /// <summary>
        /// Paraguay Guarani
        /// </summary>
        PYG,
        /// <summary>
        /// Qatar Riyal
        /// </summary>
        QAR,
        /// <summary>
        /// Romania New Leu
        /// </summary>
        RON,
        /// <summary>
        /// Serbia Dinar
        /// </summary>
        RSD,
        /// <summary>
        /// Russia Ruble
        /// </summary>
        RUB,
        /// <summary>
        /// Rwanda Franc
        /// </summary>
        RWF,
        /// <summary>
        /// Saudi Arabia Riyal
        /// </summary>
        SAR,
        /// <summary>
        /// Solomon Islands Dollar
        /// </summary>
        SBD,
        /// <summary>
        /// Seychelles Rupee
        /// </summary>
        SCR,
        /// <summary>
        /// Sudan Pound
        /// </summary>
        SDG,
        /// <summary>
        /// Sweden Krona
        /// </summary>
        SEK,
        /// <summary>
        /// Singapore Dollar
        /// </summary>
        SGD,
        /// <summary>
        /// Saint Helena Pound
        /// </summary>
        SHP,
        /// <summary>
        /// Sierra Leone Leone
        /// </summary>
        SLL,
        /// <summary>
        /// Somalia Shilling
        /// </summary>
        SOS,
        /// <summary>
        /// Seborga Luigino
        /// </summary>
        SPL,
        /// <summary>
        /// Suriname Dollar
        /// </summary>
        SRD,
        /// <summary>
        /// São Tomé and Príncipe Dobra
        /// </summary>
        STD,
        /// <summary>
        /// El Salvador Colon
        /// </summary>
        SVC,
        /// <summary>
        /// Syria Pound
        /// </summary>
        SYP,
        /// <summary>
        /// Swaziland Lilangeni
        /// </summary>
        SZL,
        /// <summary>
        /// Thailand Baht
        /// </summary>
        THB,
        /// <summary>
        /// Tajikistan Somoni
        /// </summary>
        TJS,
        /// <summary>
        /// Turkmenistan Manat
        /// </summary>
        TMT,
        /// <summary>
        /// Tunisia Dinar
        /// </summary>
        TND,
        /// <summary>
        /// Tonga Pa'anga
        /// </summary>
        TOP,
        /// <summary>
        /// Turkey Lira
        /// </summary>
        TRY,
        /// <summary>
        /// Trinidad and Tobago Dollar
        /// </summary>
        TTD,
        /// <summary>
        /// Tuvalu Dollar
        /// </summary>
        TVD,
        /// <summary>
        /// Taiwan New Dollar
        /// </summary>
        TWD,
        /// <summary>
        /// Tanzania Shilling
        /// </summary>
        TZS,
        /// <summary>
        /// Ukraine Hryvna
        /// </summary>
        UAH,
        /// <summary>
        /// Uganda Shilling
        /// </summary>
        UGX,
        /// <summary>
        /// United States Dollar
        /// </summary>
        USD,
        /// <summary>
        /// Uruguay Peso
        /// </summary>
        UYU,
        /// <summary>
        /// Uzbekistan Som
        /// </summary>
        UZS,
        /// <summary>
        /// Venezuela Bolivar Fuerte
        /// </summary>
        VEF,
        /// <summary>
        /// Viet Nam Dong
        /// </summary>
        VND,
        /// <summary>
        /// Vanuatu Vatu
        /// </summary>
        VUV,
        /// <summary>
        /// Samoa Tala
        /// </summary>
        WST,
        /// <summary>
        /// Communauté Financière Africaine (BEAC) CFA Franc BEAC
        /// </summary>
        XAF,
        /// <summary>
        /// East Caribbean Dollar
        /// </summary>
        XCD,
        /// <summary>
        /// International Monetary Fund (IMF) Special Drawing Rights
        /// </summary>
        XDR,
        /// <summary>
        /// Communauté Financière Africaine (BCEAO) Franc
        /// </summary>
        XOF,
        /// <summary>
        /// Comptoirs Français du Pacifique (CFP) Franc
        /// </summary>
        XPF,
        /// <summary>
        /// Yemen Rial
        /// </summary>
        YER,
        /// <summary>
        /// South Africa Rand
        /// </summary>
        ZAR,
        /// <summary>
        /// Zambia Kwacha
        /// </summary>
        ZMK,
        /// <summary>
        /// Zimbabwe Dollar
        /// </summary>
        ZWD,

    }
}
