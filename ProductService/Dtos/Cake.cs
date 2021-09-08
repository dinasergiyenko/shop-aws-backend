using System;

namespace ProductService.Dtos
{
    public class Cake
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Ingredient { get; set; }

        public string Calories { get; set; }

        public double Price { get; set;  }

        public int Count { get; set; }
    }
}
