namespace ApiAngularApp.Models.Domain
{
    public class Blogpost
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string ShortDes { get; set; }
        public string Content { get; set; }
        public string FeatureIamgeurl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublistedDate { get; set; }
        public string Author { get; set; }
        public bool Isvisble { get; set; }

    }
}
