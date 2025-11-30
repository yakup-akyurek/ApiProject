namespace ApiProject.WebUI.Dtos
{
    public class UpdateProduct
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductStock { get; set; }
        public decimal ProductPrice { get; set; }
        public int CategoryId { get; set; }
    }
}
