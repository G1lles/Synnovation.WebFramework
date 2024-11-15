using Synnovation.WebFramework.Mvc.Routing;

namespace Synnovation.WebFramework.Mvc;

public class HttpGetAttribute : HttpMethodAttribute
{
    private static readonly List<string> SupportedMethods = ["GET"];
     
         public HttpGetAttribute() 
             : base(SupportedMethods)
         {
         }
}