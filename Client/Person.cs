using System;

namespace Банковская_система
{
    public class Person
    {
        #region MainData
        public string Name { get; private set; }
        public string SecondName { get; private set; }
        public string FatherName { get; private set; }
        public string FullName
        {
            get
            {
                return $"{SecondName}.{Name.Substring(0, 1)}.{FatherName.Substring(0, 1)}";
            }
        }
        public Person(string name, string secondName,string fatherName)
        {
            if(string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Имя не может быть пустым",name);

            else
                Name = name;
            if(string.IsNullOrEmpty(secondName)) 
                throw new ArgumentNullException("Фамилия не может быть пустой",secondName);
            else
                SecondName = secondName;
            if(string.IsNullOrEmpty(fatherName)) 
                throw new ArgumentNullException("Отчество не может быть пустым",fatherName);
            else
                FatherName = fatherName;
        }
        public Person() { } //конструктор для упрощения наследования
        #endregion
    }
}
