using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace RoyalVillaWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        // TEMP: Dummy data (replace with DB later)
        private static List<Villa> villas = new List<Villa>
        {
            new Villa { Id = 1, Name = "Luxury Villa", Price = 4000 },
            new Villa { Id = 2, Name = "Beach Villa", Price = 6000 },
            new Villa { Id = 3, Name = "Budget Villa", Price = 2500 },
            new Villa { Id = 4, Name = "Premium Villa", Price = 8000 }
        };

        [HttpPost]
        public IActionResult Post([FromBody] ChatRequest request)
        {
            if (string.IsNullOrEmpty(request.Message))
                return BadRequest();

            string msg = request.Message.ToLower();

            // Greeting
            if (msg.Contains("hello") || msg.Contains("hi"))
            {
                return Ok(new { reply = "Hello! Ask me about villas 😊" });
            }

            // Extract price from message
            var match = Regex.Match(msg, @"\d+");

            if (match.Success)
            {
                int price = int.Parse(match.Value);

                var result = villas
                    .Where(v => v.Price <= price)
                    .Take(5)
                    .ToList();

                if (!result.Any())
                    return Ok(new { reply = $"No villas found under : ₹{price}" });

                string reply = "Villas under ₹" + price + ":\n" +
                               string.Join("\n", result.Select(v => $"{v.Name} - ₹{v.Price}"));

                return Ok(new { reply });
            }

            // Default
            return Ok(new { reply = "Try asking more: 'villas under 5000'" });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }

    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}