using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApi.middleware;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetErrors()
        {
            var errors = middleware.ExceptionHandlerMiddleware.GetErrorLogs();
            return Ok(errors);
        }
    }
}