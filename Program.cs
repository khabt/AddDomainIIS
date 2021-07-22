using Microsoft.Web.Administration;
using System;
using System.Linq;

namespace AddDomainIIS
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            AddSiteBinding();
        }

        static void AddSiteBinding()
        {
            string siteNameInIIS = "admin";
            string newBindingValue = "192.168.1.17";
            string protocol = "http";
            using (ServerManager serverManager = new ServerManager())
            {
                Site site = serverManager.Sites.Where<Site>(q => q.Name == siteNameInIIS).FirstOrDefault<Site>();
                if (site == null)
                    throw new Exception("The specified site names does not exist in IIS");
                if (site.Bindings.Count<Binding>(x =>
                {
                    if (x.BindingInformation == newBindingValue)
                        return x.Protocol == protocol;
                    return false;
                }) != 0) return;

                site.Bindings.Add(string.Format("*:{0}:{1}", (object)80, (object)newBindingValue), protocol);
                serverManager.CommitChanges();
            }            
        }
    }
}
