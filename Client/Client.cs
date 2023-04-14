using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Банковская_система
{
    public class Client : Person
    {
        #region основные данные
        private decimal _money = 0.00M;
        private string _login { get; set; }
        private string _password { get; set; }

        public Client(string name, string secondName, string fatherName) : base(name, secondName, fatherName)
        {
        }//конструктор класса возвращаемый методом EnterName()
        public Client(string login,string password) : base()
        {
            if(string.IsNullOrEmpty(login))
                throw new ArgumentNullException("Логин не может быть пустым", login);
            else
                _login = login;
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("Пароль не может быть путсым", password);
            else
                _password = password;
        }//конструктор класса возвращаемый методом Registration()
        public Client():base()
        {

        }//конструктор класса нужный для реализации других конструкторов

        protected Client EnterName()//ввод инициалов,использовать через downcast
        {
            string name, secondName, fatherName;
            Console.WriteLine("Введите свою фамилию: ");
            secondName = Console.ReadLine();
            Console.WriteLine("Введите своё имя: ");
            name = Console.ReadLine();
            Console.WriteLine("Введите своё отчество: ");
            fatherName = Console.ReadLine();
            return new Client(name, secondName, fatherName);
        }
        private Client EnterLogin()
        {
            string login, password;
            Console.WriteLine("Введите логин: ");
            login = Console.ReadLine();
            Console.WriteLine("Введите пароль");
            password = Console.ReadLine();

            return new Client(login,password);
        }//нормальный ввод логина и пароля
        #endregion
        #region Авторизация,Регистрация,перезапись данных
        private Client Authorization()//считаывание данных из текстового файла
        {
            Client LoginPassword = new Client().EnterLogin();
            StreamReader sr = new StreamReader($@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataClient\{LoginPassword._login}_{LoginPassword._password}.txt");
            string[] Initials=sr.ReadLine().Split('.');
            string[] Balans=sr.ReadLine().Split(' ');
            sr.Close();
            Client client= new Client(Initials[1], Initials[0], Initials[2]);
            client._login = LoginPassword._login;
            client._password = LoginPassword._password;
            client._money = Convert.ToDecimal(Balans[2]);
            return client;
        }

        private void Registration()//создание текстового файла с использованием данных из методов EnterName(),EnterLogin()
        {
            Client Initials = new Client().EnterName();//Фамилия,имя,отчество
            Client LoginPassword=new Client().EnterLogin();//Логин,пароль
            FileInfo[] SearchLogin= new DirectoryInfo(@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataClient").EnumerateFiles().ToArray();
            bool newLogin = true;
            foreach (FileInfo file in SearchLogin)
            {
                string[] login = file.Name.Split('_');
                if (LoginPassword._login == login[0])
                {
                    newLogin = false;
                    break;
                }
            }
            if(newLogin==true) 
            {
                using (StreamWriter sw = new StreamWriter($@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataClient\{LoginPassword._login}_{LoginPassword._password}.txt", false, Encoding.UTF8))
                {
                    sw.WriteLine($"{Initials.SecondName}.{Initials.Name}.{Initials.FatherName}");
                    sw.WriteLine("Ваш баланс: " + Initials._money);
                    sw.Close();
                }
            }
            else
            {
                Console.WriteLine("Такой логин уже существует,повторите попытку");
            }

        }
        private void ReloadData(Client client)
        {
            using (StreamWriter sw = new StreamWriter($@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataClient\{client._login}_{client._password}.txt", false, Encoding.UTF8))
            {
                sw.WriteLine($"{client.SecondName}.{client.Name}.{client.FatherName}");
                sw.WriteLine("Ваш баланс: " + client._money);
                sw.Close();
            }
        }
        #endregion
        #region программа пользователя
        public void UsingProgramm()
        {

                    

            bool openProgramm = true;
            Console.WriteLine("Добрый день");
            while (openProgramm)
            {
                Console.SetCursorPosition(70, 0);
                Console.WriteLine("Количество пользователей нашего приложения: " + new Bank().QuantityClient);
                Console.SetCursorPosition (0, 0);
                
                
                Console.WriteLine("Для регистрации нажмите 1");
                Console.WriteLine("Для авторизаиции нажмите 2");
                Console.WriteLine("Для выхода из программы нажмите 3");

                ConsoleKeyInfo userNumber = Console.ReadKey();
                Console.Clear();
                if (userNumber.Key==ConsoleKey.NumPad1 || userNumber.Key == ConsoleKey.D1)//регистрация
                {
                    new Client().Registration();
                    Console.ReadLine();
                    Console.Clear();
                }
                else if (userNumber.Key == ConsoleKey.NumPad2 || userNumber.Key == ConsoleKey.D2)//авторизация
                {
                    bool openAcc=true;
                    while(openAcc)
                    {
                        Client client = new Client().Authorization();

                        Thread.Sleep(100);
                        Console.Clear();

                        Console.WriteLine($"Добро пожаловать,{client.FullName}");
                        Console.WriteLine($"Ваш баланс: {client._money}");
                        Console.WriteLine("\nДля того чтобы пополнить баланс нажмите 1");
                        Console.WriteLine("Для того чтобы снять деньги с баланса нажмите 2");
                        Console.WriteLine("Для того чтобы перести деньги другомупользователю нажмите 3");
                        Console.WriteLine("Для того чтобы выйти нажмите 0");

                        userNumber = Console.ReadKey();

                        Console.WriteLine();
                        if (userNumber.Key == ConsoleKey.NumPad1 || userNumber.Key == ConsoleKey.D1)
                        {
                            client._money = new Bank().AddMoneyBalance();
                            ReloadData(client);
                        }
                        else if (userNumber.Key == ConsoleKey.NumPad2 || userNumber.Key == ConsoleKey.D2)
                        {
                            client._money = new Bank().TakeMoney(client._money);
                            ReloadData(client);
                        }
                        else if (userNumber.Key == ConsoleKey.NumPad3 || userNumber.Key == ConsoleKey.D3)
                        {
                            client._money = new Bank().TransferMoney(client._money);
                            ReloadData(client);
                        }
                        else if (userNumber.Key == ConsoleKey.NumPad0 || userNumber.Key == ConsoleKey.D0)
                        {
                            openAcc=false;
                        }
                        else
                        {
                            Console.WriteLine("Вы ввели не то значение пожалуйста повторите попытку");
                            Console.WriteLine("Нажмите Enter<>,чтобы продожить");
                        }
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                else if(userNumber.Key == ConsoleKey.NumPad3 || userNumber.Key == ConsoleKey.D3)
                {
                    Console.WriteLine("Досвидания");
                    openProgramm = false;
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Вы ввели не то значение пожалуйста повторите попытку");
                    Console.WriteLine("Нажмите Enter<>,чтобы продожить");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }
        #endregion 
    }
}
