namespace PYGS.Api.Models
{
    public class DocumentLine
    {
        private string? _transactionType;
        public int? LineNum { get; set; }
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double? PriceAfterVAT { get; set; }
        public string WarehouseCode { get; set; }
        public string TaxCode { get; set; }
        public double? LineTotal { get; set; }
        public string? TransactionType
        {
            get { return _transactionType; }
            set
            {
                switch (value)
                {
                    case null:
                        _transactionType = null;
                        break;
                    case "C":
                        _transactionType = "botrntComplete";
                        break;
                    case "botrntComplete":
                        _transactionType = value;
                        break;
                    case "botrntReject":
                        _transactionType = value;
                        break;
                    default:
                        throw new ArgumentException("Debe de igresar un valor valido!", nameof(TransactionType));
                        break;
                }
            }
        }
        public int? BaseType { get; set; }

        public string? AccountCode { get; set; }

        public string? ProjectCode { get; set; }
        public int? BaseEntry { get; set; }

        public string? CostingCode { get; set; }
        public string? CostingCode2 { get; set; }
    }
}
