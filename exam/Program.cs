using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using System.Diagnostics.Metrics;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace exam
{
    internal class Program
    {
        static string? connectionString;
        static void Main(string[] args)
        {
            #region ApplicationContext
            using (ApplicationContext db = new ApplicationContext())
            {

                var customers = new List<Customer>
                {
                    new Customer { Name = "Иван", Surname = "Иванов" },
                    new Customer { Name = "Петр", Surname = "Петров" },
                    new Customer { Name = "Сергей", Surname = "Сергеев" },
                    new Customer { Name = "Анна", Surname = "Антонова" },
                    new Customer { Name = "Мария", Surname = "Павлова" },
                    new Customer { Name = "Алексей", Surname = "Сидоров" },
                    new Customer { Name = "Елена", Surname = "Козлова" },
                    new Customer { Name = "Владимир", Surname = "Орлов" },
                    new Customer { Name = "Ольга", Surname = "Смирнова" },
                    new Customer { Name = "Дмитрий", Surname = "Васильев" }
                };
                db.Customers.AddRange(customers);
                db.SaveChanges();

                var books = new List<Book>
                {
                    new Book { Name = "Книга 1", Author = "Автор 1", PubName = "Издательство 1", Pages = 200, Genre = "Фэнтези", Year = new DateTime(2020, 1, 1), StartPrice = 500, SellPrice = 600 },
                    new Book { Name = "Книга 2", Author = "Автор 2", PubName = "Издательство 2", Pages = 250, Genre = "Детектив", Year = new DateTime(2019, 5, 10), StartPrice = 400, SellPrice = 500 },
                    new Book { Name = "Книга 3", Author = "Автор 3", PubName = "Издательство 3", Pages = 300, Genre = "Роман", Year = new DateTime(2021, 3, 15), StartPrice = 550, SellPrice = 700 },
                    new Book { Name = "Книга 4", Author = "Автор 4", PubName = "Издательство 4", Pages = 220, Genre = "Фантастика", Year = new DateTime(2018, 7, 20), StartPrice = 450, SellPrice = 550 },
                    new Book { Name = "Книга 5", Author = "Автор 5", PubName = "Издательство 5", Pages = 180, Genre = "Научная", Year = new DateTime(2022, 2, 5), StartPrice = 600, SellPrice = 750 },
                    new Book { Name = "Книга 6", Author = "Автор 6", PubName = "Издательство 6", Pages = 275, Genre = "Триллер", Year = new DateTime(2017, 10, 8), StartPrice = 380, SellPrice = 480 },
                    new Book { Name = "Книга 7", Author = "Автор 7", PubName = "Издательство 7", Pages = 310, Genre = "Приключения", Year = new DateTime(2020, 4, 12), StartPrice = 570, SellPrice = 670 },
                    new Book { Name = "Книга 8", Author = "Автор 8", PubName = "Издательство 8", Pages = 195, Genre = "История", Year = new DateTime(2021, 6, 22), StartPrice = 500, SellPrice = 650 },
                    new Book { Name = "Книга 9", Author = "Автор 9", PubName = "Издательство 9", Pages = 285, Genre = "Комедия", Year = new DateTime(2016, 9, 30), StartPrice = 430, SellPrice = 530 },
                    new Book { Name = "Книга 10", Author = "Автор 10", PubName = "Издательство 10", Pages = 150, Genre = "Фэнтези", Year = new DateTime(2019, 12, 1), StartPrice = 390, SellPrice = 490 }
                };
                db.Books.AddRange(books);
                db.SaveChanges();

                var discounts = new List<Discount>
                {
                    new Discount { Name = "Летняя акция", Percent = 10, SaleTopic = "Сезонная скидка", StartTime = new DateTime(2024, 6, 1), EndTime = new DateTime(2024, 8, 31) },
                    new Discount { Name = "Чёрная пятница", Percent = 20, SaleTopic = "Специальная акция", StartTime = new DateTime(2024, 11, 25), EndTime = new DateTime(2024, 11, 30) },
                    new Discount { Name = "Новогодняя распродажа", Percent = 15, SaleTopic = "Праздничная скидка", StartTime = new DateTime(2024, 12, 20), EndTime = new DateTime(2025, 1, 5) }
                };
                db.Discounts.AddRange(discounts);
                db.SaveChanges();
                var booksDiscounts = new List<BookDiscount>
                {
                    new BookDiscount { BookId = books[0].Id, DiscountId = discounts[0].Id },
                    new BookDiscount { BookId = books[1].Id, DiscountId = discounts[1].Id },
                    new BookDiscount { BookId = books[2].Id, DiscountId = discounts[2].Id }
                };
                db.BooksDiscounts.AddRange(booksDiscounts);
                db.SaveChanges();

                var booksCustomers = new List<BookCustomer>
                {
                    new BookCustomer { BookId = books[0].Id, CustomerId = customers[0].Id },
                    new BookCustomer { BookId = books[1].Id, CustomerId = customers[1].Id },
                    new BookCustomer { BookId = books[2].Id, CustomerId = customers[2].Id }
                };
                db.BooksCustomers.AddRange(booksCustomers);
                db.SaveChanges();
            }
            #endregion

            #region MainCat
            var builder = new ConfigurationBuilder();
            string path = Directory.GetCurrentDirectory();
            builder.SetBasePath(path);
            builder.AddJsonFile("C:\\Users\\vovan\\source\\repos\\exam\\exam\\appsettings.json");
            var config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");
            #endregion

            Menu();

        }
        #region MainMenu
        static void Menu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить/списать книгу");
                Console.WriteLine("3. Отредактировать книгу");
                Console.WriteLine("4. Добавить книгу в скидки");
                Console.WriteLine("5. Оставить книгу для покупателя");
                Console.WriteLine("6. Поиск");
                Console.WriteLine("0. Выход");
                int result = int.Parse(Console.ReadLine());
                switch (result)
                {
                    case 1:
                        Addbook();
                        break;
                    case 2:
                        DeleteBook();
                        break;
                    case 3:
                        RedactBook();
                        break;
                    case 4:
                        RedactBook();
                        break;
                    case 5:
                        LeftforCustomer();
                        break;
                    case 6:
                        SearchMenu();
                        return;
                    case 0:
                        return;
                }
            }
        }
        #endregion
        #region Addbook
        public static void Addbook()
        {
            Console.WriteLine("Введите название книги:");
            var name = Console.ReadLine();
            Console.WriteLine("Введите автора: ");
            var author = Console.ReadLine();
            Console.WriteLine("Введите название издательства: ");
            var pubname = Console.ReadLine();
            Console.WriteLine("Введите количесто страниц: ");
            int pages = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите жанр: ");
            var genre = Console.ReadLine();
            Console.WriteLine("Введите год выпуска: ");
            int year = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите цену закупки: ");
            int startprice = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите цену продажи: ");
            int sellprice = int.Parse(Console.ReadLine());

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sql = "INSERT INTO Books (Name, Author, Pubname, Pages, Genre, Year, Startprice, SellPrice) " +
                          "VALUES (@Name, @Author, @Pubname, @Pages, @Genre, @Year, @Startprice, @SellPrice)";

                db.Execute(sql, new
                {
                    Name = name,
                    Author = author,
                    Pubname = pubname,
                    Pages = pages,
                    Genre = genre,
                    Year = year,
                    Startprice = startprice,
                    SellPrice = sellprice
                });
            }
            Console.WriteLine("Книга добавлена");
        }
        #endregion
        #region DeleteBook
        public static void DeleteBook()
        {
            Console.Clear();
            Console.Write("Введите ID книги, которую нужно удалить: ");
            int id = int.Parse(Console.ReadLine());
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute("DELETE FROM Books WHERE Id = @Id", new { Id = id });
            }
            Console.WriteLine("Книга удалена");
        }
        #endregion
        #region Redactbook
        static void RedactBook()
        {
            Console.Clear();
            Console.WriteLine("Введите Id книги, которую нужно извенить");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Выберите данные которые нужно изменить: \n" +
                "1. Название книги \n" +
                "2. Имя автора \n" +
                "3. Кол-во страниц \n" +
                "4. Жанр \n" +
                "5. Цену продажи\n" +
                "0. Выход");
            var result = int.Parse(Console.ReadLine());
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                switch (result)
                {
                    case 1:
                        Console.WriteLine("Введите новое название: ");
                        var name = Console.ReadLine();
                        db.Execute("UPDATE Books SET Name = @Name WHERE Id = @Id", new { Name = name, Id = id });
                        break;
                    case 2:
                        Console.WriteLine("Введите новое имя автора: ");
                        var author = Console.ReadLine();
                        db.Execute("UPDATE Books SET Author = @Author WHERE Id = @Id", new { Author = author, Id = id });
                        break;
                    case 3:
                        Console.WriteLine("Введите новое кол-во страниц: ");
                        var pages = int.Parse(Console.ReadLine());
                        db.Execute("UPDATE Books SET Pages = @Pages WHERE Id = @Id", new { Pages = pages, Id = id });
                        break;
                    case 4:
                        Console.WriteLine("Введите новый жанр: ");
                        var genre = Console.ReadLine();
                        db.Execute("UPDATE Books SET Genre = @Genre WHERE Id = @Id", new { Genre = genre, Id = id });
                        break;
                    case 5:
                        Console.WriteLine("Введите новую цену: ");
                        var sellprice = Console.ReadLine();
                        db.Execute("UPDATE Books SET SellPrice = @SellPrice WHERE Id = @Id", new { SellPrice = sellprice, Id = id });
                        break;
                    case 0:
                        return;
                }
            }
        }
        #endregion
        #region AddDisc
        public static void AddDisc()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите Id книги, которую вы хотите добавить к скидке");
                int bookId = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите Id скидки");
                int discountId = int.Parse(Console.ReadLine());
                db.Execute("INSERT INTO BooksDiscounts (BookId, DiscountId) VALUES (@BookId, @DiscountId)", new { BookId = bookId, DiscountId = discountId });
            }
        }
        #endregion
        #region LeftforCustomer
        public static void LeftforCustomer()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите Id покупателя, которому хотите оставить книгу");
                int customerId = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите Id книги, которую хотите оставить покупателю");
                int leftBookId = int.Parse(Console.ReadLine());
                db.Execute("UPDATE Customers SET LeftBookId = @LeftBookId WHERE Id = @CustomerId", new { LeftBookId = leftBookId, CustomerId = customerId });
            }
        }
        #endregion

        #region search
        static void SearchMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Найти по названию");
            Console.WriteLine("2. Найти по автору");
            Console.WriteLine("3. Найти по названию редакции");
            Console.WriteLine("0. Выход");
            int result = int.Parse(Console.ReadLine());
            switch (result)
            {
                case 1:
                    FindByName();
                    break;
                case 2:
                    FindByAuthor();
                    break;
                case 3:
                    FindByGenre();
                    break;
                case 0:
                    return;
            }
        }
        static void FindByName()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите название книги: ");
                string name = Console.ReadLine();
                var books = db.Query<Book>("SELECT * FROM Books WHERE Name = @Name", new { Name = name });
                int iter = 0;
                foreach (var book in books)
                    Console.WriteLine($"Book #{++iter} {book.Name} {book.Author} {book.Pages} {book.Genre} ");
            }
        }
        static void FindByAuthor()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите автора: ");
                string author = Console.ReadLine();
                var books = db.Query<Book>("SELECT * FROM Books WHERE Author = @Author", new { Author = author });
                int iter = 0;
                foreach (var book in books)
                    Console.WriteLine($"Book #{++iter} {book.Name} {book.Author} {book.Pages} {book.Genre} ");
            }
        }
        static void FindByGenre()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                Console.WriteLine("Введите жанр книги: ");
                string genre = Console.ReadLine();
                var books = db.Query<Book>("SELECT * FROM Books WHERE Genre = @Genre", new { Genre = genre });
                int iter = 0;
                foreach (var book in books)
                    Console.WriteLine($"Book #{++iter} {book.Name} {book.Author} {book.Pages} {book.Genre} ");
            }
        }
        #endregion
    }
}
