using Microsoft.AspNetCore.Mvc;

namespace PSAPI.Test
{
    internal static class TestExtensions
    {
        public static T GetObjectResult<T>(this ActionResult<T> result)
        {
            if (result.Result != null)
                return (T)((ObjectResult)result.Result).Value;
            return result.Value;
        }
    }
}
