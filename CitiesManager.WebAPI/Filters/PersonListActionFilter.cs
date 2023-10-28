using Microsoft.AspNetCore.Mvc.Filters;

namespace CitiesManager.WebAPI.Filters
{
    public class PersonListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonListActionFilter> logger;
        public PersonListActionFilter(ILogger<PersonListActionFilter> _logger)
        {
            logger= _logger;
        }
        /// <summary>
        /// ActionFilter, method = OnActionExecuted => Code which gets executed after action method executed
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("PersonListActionFilter OnActionExecuted method");
        }
        /// <summary>
        /// ActionFilter= OnActionExecuting => Code which gets executed before action method executed
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("PersonListActionFilter OnActionExecuting method");
        }
    }
}
