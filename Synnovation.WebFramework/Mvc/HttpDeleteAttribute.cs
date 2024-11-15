using Synnovation.WebFramework.Mvc.Routing;

namespace Synnovation.WebFramework.Mvc;

public class HttpDeleteAttribute : HttpMethodAttribute
{
    private static readonly List<string> SupportedMethods = ["DELETE"];
     
    public HttpDeleteAttribute() 
        : base(SupportedMethods)
    {
    }
}