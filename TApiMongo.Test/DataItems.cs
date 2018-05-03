using System.Collections.Generic;
using TApiMongo.Data.Entities;

namespace TApiMongo.Test
{
    public class DataItems
    {
        public static List<Item> Data = new List<Item>
        {
            new Item
            {
                ID = 1,
                Description = "Описание 1",
                Name = "Объект 1",
                Tags = new List<string>
                {
                    "Первый tag",
                    "Второй tag",
                }
            },
            new Item
            {
                ID = 2,
                Description = "Описание 2",
                Name = "Объект 2",
                Tags = new List<string>
                {
                    "Третий tag",
                    "Четвертый tag",
                },
            },
            new Item
            {
                ID = 3,
                Description = "Описание 3",
                Name = "Объект 3",
                Tags = new List<string>
                {
                    "Третий tag",
                    "Четвертый tag",
                },
            },
            new Item
            {
                ID = 4,
                Description = "Описание 4",
                Name = "Объект 4",
                Tags = new List<string>
                {
                    "Третий tag",
                    "Четвертый tag",
                },
            },
            new Item
            {
                ID = 5,
                Description = "Описание 5",
                Name = "Объект 5",
                Tags = new List<string>
                {
                    "Третий tag",
                    "Четвертый tag",
                },
            },
            new Item
            {
                ID = 6,
                Description = "Описание 6",
                Name = "Объект 6",
                Tags = new List<string>
                {
                    "Третий tag",
                    "Четвертый tag",
                },
            },
            new Item
            {
            ID = 7,
            Description = "Описание 7",
            Name = "Объект 7",
            Tags = new List<string>
            {
                "Третий tag",
                "Четвертый tag",
            },
        },
            new Item
            {
                ID = 8,
                Description = "Описание 8",
                Name = "Объект 8",
                Tags = new List<string>
                {
                    "Третий tag",
                    "Четвертый tag",
                },
            },
            new Item
            {
                ID = 9,
                Description = "Описание 9",
                Name = "Объект 9",
                Tags = new List<string>
                {
                    "Третий tag",
                    "Четвертый tag",
                },
            },
            new Item
            {
                ID = 10,
                Description = "Описание 10",
                Name = "Объект 10",
                Tags = new List<string>
                {
                    "Третий tag",
                    "Четвертый tag",
                },
            },
        };

        public static string ConnectionString = "mongodb://localhost:27017/TApiMongo";
    }
}