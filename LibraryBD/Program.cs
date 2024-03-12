using System;
using System.Data;
using Microsoft.Data.SqlClient;
using static System.Collections.Specialized.BitVector32;


class BD
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            string selectedTable = "";
            bool returnToTableSelection = false;
            string conectBD = "Server=DESKTOP-5BD88QO\\SQLEXPRESS;Database=Library;Trusted_Connection=True;TrustServerCertificate=true";

            if (!returnToTableSelection)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Выберите таблицу для дальнейших действий"); Console.ResetColor();
                Console.WriteLine("1. People");
                Console.WriteLine("2. Books");
                int chosenTable = 0;
                while (true)
                {
                    // Пытаемся преобразовать ввод пользователя в число
                    if (!int.TryParse(Console.ReadLine(), out chosenTable))
                    {
                        // Если ввод не является числом, выводим сообщение об ошибке и повторяем запрос
                        Console.WriteLine("Ошибка: Выберите номер таблицы.");
                    }
                    else if (chosenTable != 1 && chosenTable != 2)
                    {
                        // Если введено число отличное от 1 и 2, выводим сообщение об ошибке и повторяем запрос
                        Console.WriteLine("Ошибка: Пожалуйста, введите 1 или 2.");
                    }
                    else
                    {
                        // Ввод корректен, выходим из цикла
                        selectedTable = (chosenTable == 1) ? "People" : "Books";
                        break;
                    }
                }
                Console.WriteLine("Вы выбрали таблицу: " + selectedTable);

                if (returnToTableSelection)
                {
                    returnToTableSelection = false;
                    continue; // Переход в начало цикла для выбора таблицы
                }

                switch (chosenTable)
                {
                    //Действия с таблицей пользователей
                    case 1:
                        using (SqlConnection conect = new SqlConnection(conectBD))
                        {
                            conect.Open();
                            string sql = $"SELECT * FROM People;";
                            using (SqlCommand command = new SqlCommand(sql, conect))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine("\nPeople\n");
                                Console.ResetColor(); using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"{reader["ID"]} {reader["LastName"]} {reader["Name"]} {reader["Age"]} {reader["Book"]} ");
                                    }
                                }
                            }
                            if (!returnToTableSelection)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\nВыберите действие");
                                Console.ResetColor();
                                Console.WriteLine("1. Создать пользователя");
                                Console.WriteLine("2. Обновить данные пользователя");
                                Console.WriteLine("3. Удалить");
                                Console.WriteLine("4. Выбор таблицы\n");

                                int action;
                                while (true)
                                {
                                    // Пытаемся преобразовать ввод пользователя в число
                                    if (!int.TryParse(Console.ReadLine(), out action))
                                    {
                                        // Если ввод не является числом, выводим сообщение об ошибке и повторяем запрос
                                        Console.WriteLine("Ошибка: Введите номер действия.");
                                    }
                                    else if (action < 1 || action > 4)
                                    {
                                        // Если введено число, не находящееся в диапазоне от 1 до 4, выводим сообщение об ошибке и повторяем запрос
                                        Console.WriteLine("Ошибка: Выберите действие от 1 до 4.");
                                    }
                                    else
                                    {
                                        // Ввод корректен, выходим из цикла
                                        break;
                                    }
                                }

                                // Вывод сообщения о выбранном действии
                                switch (action)
                                {
                                    case 1:
                                        try
                                        {
                                            using (SqlCommand createUser = new SqlCommand(sql, conect))
                                            {
                                                int num = await createUser.ExecuteNonQueryAsync(); Console.WriteLine("\nВведите Фамилию"); Console.WriteLine("\nВведите Имя:");
                                                string lastname = Console.ReadLine(); Console.WriteLine("\nВведите Имя:");
                                                string firstname = Console.ReadLine(); Console.WriteLine("\nВведите Возраст:");
                                                string secondname = Console.ReadLine(); Console.WriteLine("\nВведите Книгу:");
                                                string book = Console.ReadLine();

                                                sql = $"INSERT INTO People (LastName,Name,Age,Book) VALUES ('{lastname}','{firstname}','{secondname}','{book}')"; createUser.CommandText = sql;
                                                num = await createUser.ExecuteNonQueryAsync(); Console.WriteLine($"\nДобавлено объектов: {num}");
                                            }
                                        }
                                        catch
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error");
                                            Console.ResetColor();
                                        }
                                        break;
                                    //Обновление данных с возможностью выбрать,что обновить
                                    case 2:
                                        try
                                        {
                                            using (SqlCommand updateUser = new SqlCommand(sql, conect))
                                            {
                                                int up = await updateUser.ExecuteNonQueryAsync(); Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine("\nВыберите действие"); Console.ResetColor();
                                                Console.WriteLine("1. Изменить Фамилию");
                                                Console.WriteLine("2. Изменить Имя");
                                                Console.WriteLine("3. Изменить Возраст");
                                                Console.WriteLine("4. Изменить Книгу\n");
                                                //Выбор что обновить
                                                int obnova = Convert.ToInt16(Console.ReadLine());
                                                switch (obnova)
                                                {
                                                    case 1:
                                                        Console.WriteLine("Введите ID пользователя для изменения");
                                                        string userID = Console.ReadLine(); Console.WriteLine("\nВведите новую Фамилию:");
                                                        string firstname = Console.ReadLine(); sql = $"UPDATE People set LastName='{firstname}' WHERE ID={userID}";
                                                   break;
                                                    case 2:
                                                        Console.WriteLine("Введите ID пользователя для изменения");
                                                        string useID = Console.ReadLine(); Console.WriteLine("\nВведите новое Имя:");
                                                        string second = Console.ReadLine(); sql = $"UPDATE People set Name='{second}' WHERE ID={useID}";
                                                   break;
                                                    case 3:
                                                        Console.WriteLine("Введите ID пользователя для изменения");
                                                        string usID = Console.ReadLine(); Console.WriteLine("\nВведите новый Возраст:");
                                                        string last = Console.ReadLine(); sql = $"UPDATE People set Age='{last}' WHERE ID={usID}";
                                                   break;
                                                    case 4:
                                                        Console.WriteLine("Введите ID пользователя для изменения");
                                                        string uID = Console.ReadLine(); Console.WriteLine("\nВведите новую Книгу:");
                                                        string lastу = Console.ReadLine(); sql = $"UPDATE Users set Book='{lastу}' WHERE ID={uID}";
                                                   break;
                                                }
                                                updateUser.CommandText = sql; up = await updateUser.ExecuteNonQueryAsync();
                                            }
                                        }
                                        catch
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error");
                                            Console.ResetColor();
                                        }
                                        break;
                                    //Удаление пользователя
                                    case 3:
                                        try
                                        {
                                            using (SqlCommand deleteUser = new SqlCommand(sql, conect))
                                            {
                                                int del = await deleteUser.ExecuteNonQueryAsync(); Console.WriteLine("Введите ID пользователя для удаления");
                                                string delID = Console.ReadLine(); sql = $"use Library delete from People where ID={delID}";
                                                deleteUser.CommandText = sql; del = await deleteUser.ExecuteNonQueryAsync();
                                            }
                                        }
                                        catch
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error");
                                            Console.ResetColor();
                                        }
                                        break;

                                    case 4:
                                        try
                                        {
                                            returnToTableSelection = true;
                                        }
                                        catch
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error");
                                            Console.ResetColor();
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("Выбрано неверное действие.");
                                   break;
                                }
                            }
                      break;
                    }






                    //Действия с таблицей книг         
                    case 2:
                        using (SqlConnection conect = new SqlConnection(conectBD))
                        {
                            conect.Open();
                            string sql = $"SELECT * FROM Books;";
                            using (SqlCommand command = new SqlCommand(sql, conect))
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine("\nBooks\n"); Console.ResetColor();
                                using (SqlDataReader read = command.ExecuteReader())
                                {
                                    while (read.Read())
                                    {
                                        Console.WriteLine($"{read["ID"]} {read["Book"]} {read["Author"]} {read["Release"]} {read["Rating"]}");
                                    }

                                }
                            }
                            if (!returnToTableSelection)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("\nВыберите действие");
                                Console.ResetColor();
                                Console.WriteLine("1. Вписать книгу");
                                Console.WriteLine("2. Обновить данные книги");
                                Console.WriteLine("3. Удалить");
                                Console.WriteLine("4. Выбор таблицы\n");
                                int M;
                                while (true)
                                {
                                    // Пытаемся преобразовать ввод пользователя в число
                                    if (!int.TryParse(Console.ReadLine(), out M))
                                    {
                                        // Если ввод не является числом, выводим сообщение об ошибке и повторяем запрос
                                        Console.WriteLine("Ошибка: Введите номер действия.");
                                    }
                                    else if (M < 1 || M > 4)
                                    {
                                        // Если введено число, не находящееся в диапазоне от 1 до 4, выводим сообщение об ошибке и повторяем запрос
                                        Console.WriteLine("Ошибка: Выберите действие от 1 до 4.");
                                    }
                                    else
                                    {
                                        // Ввод корректен, выходим из цикла
                                        break;
                                    }
                                }
                                switch (M)
                                {
                                    case 1:
                                        Console.WriteLine("Новая книга..."); 
                                        using (SqlCommand createOrder = new SqlCommand(sql, conect))
                                        {
                                            int crt = await createOrder.ExecuteNonQueryAsync();
                                            Console.WriteLine("Введите ID"); string crtID = Console.ReadLine();
                                            Console.WriteLine("Введите Book"); string dateOrder = Console.ReadLine();
                                            Console.WriteLine("Введите Author"); string crtID1 = Console.ReadLine();
                                            Console.WriteLine("Введите Release"); string dateOrder1 = Console.ReadLine();
                                            Console.WriteLine("Введите Rating"); string crtID2 = Console.ReadLine();

                                            sql = $"insert into Books (ID,Book,Author,Release,Rating) values ('{crtID}','{dateOrder}','{crtID1}','{dateOrder1}','{crtID2}')"; createOrder.CommandText = sql;
                                            crt = await createOrder.ExecuteNonQueryAsync();
                                            Console.WriteLine("Обновлено");
                                        }
                                        break;
                                    case 2:
                                        Console.WriteLine("Редактирование книги..."); 
                                        using (SqlCommand updateOrder = new SqlCommand(sql, conect))
                                        {
                                            int upOrd = await updateOrder.ExecuteNonQueryAsync();
                                            Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("\nВыберите действие");
                                            Console.ResetColor();
                                            Console.WriteLine("1. Изменить Book");
                                            Console.WriteLine("2. Изменить Autor\n");
                                            int O = Convert.ToInt16(Console.ReadLine()); switch (O)
                                            {
                                                case 1:
                                                    Console.WriteLine("Введите ID для редактирования"); string OID = Console.ReadLine();
                                                    Console.WriteLine("Введите Book для изменения"); string userID = Console.ReadLine();
                                                    sql = $"update Books set Book='{userID}' where ID='{OID}'"; break;
                                                case 2:
                                                    Console.WriteLine("Введите ID для редактирования");
                                                    string OrID = Console.ReadLine(); Console.WriteLine("Введите Author");
                                                    string dateOr = Console.ReadLine(); sql = $"update Books set Author='{dateOr}' where ID='{OrID}'";
                                                    break;
                                            }
                                            updateOrder.CommandText = sql; upOrd = await updateOrder.ExecuteNonQueryAsync();
                                            Console.WriteLine("Обновлено");
                                        }
                                        break;
                                    case 3:
                                        try
                                        {
                                            Console.WriteLine("Удаление книги...");
                                            using (SqlCommand deleteUser = new SqlCommand(sql, conect))
                                            {
                                                int del = await deleteUser.ExecuteNonQueryAsync(); Console.WriteLine("Введите ID для удаления");
                                                string delID = Console.ReadLine(); sql = $"use Library delete from Books where ID={delID}";
                                                deleteUser.CommandText = sql; del = await deleteUser.ExecuteNonQueryAsync();
                                                Console.WriteLine("Обновлено");
                                            }
                                        }
                                        catch
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Error");
                                            Console.ResetColor();
                                        }
                                        break;
                                    case 4:
                                        try
                                        {
                                            Console.WriteLine("Выбор таблицы");
                                            returnToTableSelection = true;
                                        }
                                        catch
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red; 
                                            Console.WriteLine("Error");
                                            Console.ResetColor();
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("Выбрано неверное действие.");
                                   break;
                                }
                            }
                        }
                   break;
                }
            }
        }
    }
}
