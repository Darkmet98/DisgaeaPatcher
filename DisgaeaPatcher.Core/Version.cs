namespace DisgaeaPatcher.Core
{
    public class Version
    {
        public int Id { get; set; }
        public string PatchVersion { get; set; }
        public string Changelog { get; set; }
        public string[] Md5 { get; set; }
    }
}
