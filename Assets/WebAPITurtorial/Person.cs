using UnityEngine;

public class Person 
{
    [CsvHelper.Configuration.Attributes.Name("id")]
    public int ID { get; set; }

    [CsvHelper.Configuration.Attributes.Name("name")]
    public string Name { get; set; }

    [CsvHelper.Configuration.Attributes.Name("age")]
    public int Age { get; set; }

    public Person() { }
    public Person(int id, string name, int age)
    {
        ID = id;
        Name = name;
        Age = age;
    }
}
