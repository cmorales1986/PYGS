namespace PYGS.Api.Models
{
    public class Document
    {
        private string? _docType;
        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public string DocType
        {
            get { return _docType == "A" ? "dDocument_Items" : "dDocument_Service"; }
            set
            {
                switch (value)
                {
                    case "A":
                    case "S":
                    case "a":
                    case "s":
                        _docType = value.ToUpper();
                        break;
                    case "dDocument_Items":
                        _docType = "A";
                        break;
                    case "dDocument_Service":
                        _docType = "S";
                        break;
                    default:
                        throw new ArgumentException("Debe de igresar un valor valido A para Articulo y S para servicios", nameof(DocType));
                        break;
                }
            }
        }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string? CardCode { get; set; }
        public string? DocCurrency { get; set; }
        public string? FolioPrefixString { get; set; }
        public int? FolioNumber { get; set; }
        public string? U_EST { get; set; }
        public string? U_PDE { get; set; }
        public int? U_TIM { get; set; }
        public string? JournalMemo { get; set; }
        public string? Comments { get; set; }
        public int? RelatedEntry { get; set; }
        public List<DocumentLine> DocumentLines { get; set; }
    }
}
