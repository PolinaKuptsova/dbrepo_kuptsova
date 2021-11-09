using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

static public class Authentication
{
    // Реєстрація перевіряє унікальність імені користувача створює новий запис у таблиці користувачів. 
    // Хешувати паролі користувачів за допомогою алгоритму SHA256. 
    // Функція логіну перевіряє надане ім’я користувача та пароль і повертає об’єкт користувача на основі запису з БД. 
    
    static public Customer ValidateUsername(string username, CustomerRepository customerRepository)
    {
        List<Customer> customers = customerRepository.GetAll();
        if(customers.Count > 0)
        {
            foreach(Customer customer in customers)
            {
                if(username == customer.userName)
                {
                    return customer;
                }
            }
        }
        return null;
    }
    
    public static string GetHash(string input)
    {
        SHA256 sha256Hash = SHA256.Create();
        byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        var sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }

    public static bool VerifyHash(string input, string hash)
    {
        var hashOfInput = GetHash(input);

        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(hashOfInput, hash) == 0;
    }
}