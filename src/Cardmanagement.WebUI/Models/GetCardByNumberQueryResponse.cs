namespace Cardmanagement.WebUI.Models
{
    public class GetCardByNumberQueryResponse
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string CardHolderName { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationtYear { get; set; }
        public string CVC { get; set; }
    }
}
