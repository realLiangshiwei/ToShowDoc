using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Core2.x.Models
{
    /// <summary>
    /// 图书
    /// </summary>
    public class Book
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不可以为空")]
        [StringLength(10)]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [Range(1, 100)]
        public int Inventory { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public List<Author> Author { get; set; }
    }

    /// <summary>
    /// 作者
    /// </summary>
    public class Author
    {
        /// <summary>
        /// 作者姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄 
        /// </summary>
        public int Age { get; set; }
    }
}
