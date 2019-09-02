namespace TP.UnitTests.Utils
{
    /// <summary>
    /// Réunit les constantes
    /// </summary>
    public static class ConstantsTPFact
    {
        //ClientName
        public static readonly string ClientValidName = "ClientNomTest";
        public static readonly string ClientInvalidSystemName = Entities.TPFact.Common.InvalidFactureEnteteClientName;

        //FactureEntetes.MontantHTVA 
        public static readonly decimal FactureEntetesMontantHTVA = 120;

        //FactureDetails
        public static readonly int DetailsAmountExpected = 2;
        public static readonly decimal FactureDetailMontantHTVA = 15;
        public static readonly decimal FactureDetailMontantHTVANegatif = -15;
        public static readonly int FactureDetailQuantite = 8;
    }
}