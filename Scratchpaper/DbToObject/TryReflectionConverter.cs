using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Data;
using TaskTracker;
using System.IO;
using DataStore.SQLiteDataManagers;

namespace Scratchpaper
{
  
    class Converter<T>
    {

        public string OriginAttrName { get; set; }

        public string TargetAttrName { get; set; }

        public Func<object, object> InputProcessor { get; set; }

        private Action<object, object> GetPropertySetter()
        {
            return typeof(T).GetProperty(TargetAttrName).SetValue;
        }

        public void Do(SqliteDataReader reader, T taskInstance)
        {
            var propSetter = GetPropertySetter();
            var inputValue = reader.GetValue(OriginAttrName);
            var value = InputProcessor(inputValue);
            propSetter(taskInstance, value);
        }
    }

    class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        override public string ToString()
        {
            return $"{Id}  {Name}";
        }
    }

    public abstract class TryReflectionConverter
    {
        public static void Go(SqliteConnection connection)
        {
            try
            {
                // create converter objects for db -> user
                var converters = new List<Converter<Company>>();

                var idConverter = new Converter<Company>()
                {
                    OriginAttrName = "company_id",
                    TargetAttrName = "Id",
                    InputProcessor = x => Convert.ToInt32((long)x),
                };
                converters.Add(idConverter);

                var nameConverter = new Converter<Company>()
                {
                    OriginAttrName = "company_name",
                    TargetAttrName = "Name",
                    InputProcessor = x => x
                };
                converters.Add(nameConverter);


                // create user object
                var companies = new List<Company>();

                // create a select * query
                var sql = "SELECT * FROM Company;";

                // get a reader
                using (connection)
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.CommandType = CommandType.Text;
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            // loop through the converters, passing in the user instance and the reader
                            var company = new Company();
                            foreach (var converter in converters)
                            {
                                converter.Do(reader, company);
                            }
                            Console.WriteLine(company);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
