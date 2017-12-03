using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    [Route("/orders")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AspNetCoreFiltersController : Controller
    {
        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public ActionResult Create([FromBody]Order order)
        {
            return Created("/orders/1", 1);
        }

        [HttpPost("registrations")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(int), 201)]
        public ActionResult PostForm([FromForm]RegistrationForm form)
        {
            throw new System.NotImplementedException();
        }

        [HttpPost("custom")]
        [Produces("application/xml")]
        [ProducesResponseType(typeof(int), 201)]
        public ActionResult CustomProduces([FromBody]Order order)
        {
            return Created("/orders/1", 1);
        }
    }

    public class Order
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Total { get; set; }
    }

    public class RegistrationForm
    {
        public string Name { get; set; }

        public IEnumerable<int> PhoneNumbers { get; set; }
    }
}