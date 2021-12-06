using System;
using Npgsql;
using System.Data;
class Program
{
    static void Main(string[] args)
    {
        //NpgsqlConnection connection = new NpgsqlConnection(@"Server = 127.0.0.1;Port = 5432;User Id = postgres;Password = 123456789; DataBase = mydb");
        try
        {
          //  if (connection.State == ConnectionState.Open)
            {
                ShopService shopService = new ShopService();
                ProcessArguments.Run(shopService);
            }
            //else
            {
              //  throw new Exception("Cannot get accsess to the data base. Check connection");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


    }
}