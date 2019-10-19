using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.Core2.x.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStore.Core2.x.Controllers
{
    /// <summary>
    /// 图书商店
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BookStoreController : ControllerBase
    {
        private static readonly List<Book> Books;

        static BookStoreController()
        {
            Books = new List<Book>
            {
                new Book()
                {
                    CreateTime = DateTime.Now,
                    Inventory = 10,
                    Name = "C#并发编程实例",
                    Price = 30,
                    Author = new List<Author>() {new Author() {Name = "Stephen Cleary", Age = 32}}
                },
                new Book()
                {
                    CreateTime = DateTime.Now,
                    Inventory = 10,
                    Name = "C#深入理解",
                    Price = 20,
                    Author = new List<Author>() {new Author() {Name = "Jon Skeet", Age = 33}}
                }
            };
        }

        private readonly ILogger<BookStoreController> _logger;

        public BookStoreController(ILogger<BookStoreController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取所有图书
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Book> GetAll()
        {
            return Books;
        }

        /// <summary>
        /// 获取指定名称图书
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public Book Get(string name)
        {
            return Books.FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// 添加图书
        /// </summary>
        /// <param name="book"></param>
        [HttpPost]
        public void Add(Book book)
        {
            Books.Add(book);
        }
    }
}
