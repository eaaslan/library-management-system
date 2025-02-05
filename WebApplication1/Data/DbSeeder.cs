//using CsvHelper;
//using System.Globalization;
//using WebApplication1.Models;

//namespace WebApplication1.Data
//{
//    public static class DbSeeder
//    {
//        public static async Task SeedData(ApplicationDbContext context)
//        {
//            if (!context.Categories.Any())
//            {
//                using var reader = new StreamReader("Data/Category.csv");
//                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
//                var categories = csv.GetRecords<Category>().ToList();
//                await context.Categories.AddRangeAsync(categories);
//                await context.SaveChangesAsync();
//            }

//            if (!context.Books.Any())
//            {
//                using var reader = new StreamReader("Data/Books.csv");
//                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
//                var books = csv.GetRecords<Book>().ToList();
//                await context.Books.AddRangeAsync(books);
//                await context.SaveChangesAsync();
//            }
//        }
//    }
//} 