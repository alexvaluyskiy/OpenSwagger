using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    [Route("simple")]
    public class SimpleController : Controller
    {
        [HttpGet("{id}")]
        public Product GetById(int id)
        {
            return new Product { Id = id, Description = "A product" };
        }
    }
}
