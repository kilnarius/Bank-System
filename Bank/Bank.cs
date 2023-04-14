using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Банковская_система
{
    public class Bank
    {
        #region DataBank
        protected decimal _money = Convert.ToDecimal(new StreamReader(@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataBank\bank_money.txt").Read());
        
        
        public int QuantityClient=new DirectoryInfo(@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataClient").GetFiles().Length;   
        
        public Bank()
        {

        }
        protected void QC()
        {
            StreamWriter sw = new StreamWriter(@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataBank\bank_client.txt", false, Encoding.UTF8);
            sw.Write(new Bank().QuantityClient);
            sw.Close();
        }
        protected void ReturFileBankMoney()
        {
            ;
            using(StreamWriter sw=new StreamWriter(@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataBank\bank_money.txt",false,Encoding.UTF8))
            {
                sw.Write(new Bank()._money);
                sw.Close();
            }
        }
        public void SaveDataBank()
        {
            QC();
            ReturFileBankMoney();
        }
        #endregion
        #region пополнение баланса,снятие денег,перевод по логину,вклады
        public decimal AddMoneyBalance()//пополнение баланса
        {
            decimal money = 0.00M;
            Console.WriteLine("какое количество денег вы хотите внести?");
            Console.WriteLine("Для указания десятичной части используйте: \',\'");
            money = Convert.ToDecimal(Console.ReadLine());
            if(money<0)
            {
                Console.WriteLine("вы ввели неверное значение");
                return 0;
            }
            else
            {
                Console.WriteLine("Пополнение прошло успешно");
                new Bank()._money += money;
                return money;
            }
        }
        public decimal TakeMoney(decimal _money)//вывод средств
        {
            decimal money = 0.00M;
            Console.WriteLine("Какое количество денег вы хотите снять?");
            Console.WriteLine("Для указания десятичной части используйте: \',\'");
            money = Convert.ToDecimal(Console.ReadLine());
            if (money < 0)
            {
                Console.WriteLine("Вы ввели отрицательное значение");
                return _money;
            }
            else if (money > _money)
            {
                Console.WriteLine("У вас недостаточно средств");
                return _money;
            }
            else
            {
                Console.WriteLine("Вы успешно вывели средства");
                new Bank()._money -= money;
                return _money - money;
            }
            
        }
        public decimal TransferMoney(decimal _money)
        {
            Console.Write("Введите логин человека которому вы собираетесь перевести деньги: ");
            string login = Console.ReadLine();
            decimal money = 0.00M;
            Console.WriteLine("Какое количество денег вы хотите перевести?");
            Console.WriteLine("Для указания десятичной части используйте: \',\'");
            money = Convert.ToDecimal(Console.ReadLine());
            if (money < 0)
            {
                Console.WriteLine("Вы ввели отрицательное значение");
                return _money;
            }
            else if (money > _money)
            {
                Console.WriteLine("У вас недостаточно средств");
                return _money;
            }
            else
            {
                Console.WriteLine("Вы успешно перевели средства");
                try
                {
                    string[] log = new string[2];
                    FileInfo[] file= new DirectoryInfo(@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataClient").GetFiles();
                    foreach(FileInfo file2 in file) 
                    {
                        log = file.ToString().Split('_');
                        if (log[0]==login)
                        {
                            break;
                        }
                    }
                    using(StreamReader sr=new StreamReader($@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataClient\{log[0]}_{log[1]}")) 
                    {
                        string[] Inizials=sr.ReadLine().Split('.');
                        Client client = new Client(Inizials[1], Inizials[0], Inizials[2]);
                        string[] Balance=sr.ReadLine().Split(' ');
                        decimal Money = Convert.ToDecimal(Balance[2]);
                        sr.Close();
                        Money += money;
                        using (StreamWriter sw = new StreamWriter($@"C:\Users\Александр\Desktop\C#\Клиент-Сервер\Банковская система\DataClient\{log[0]}_{log[1]}", false, Encoding.UTF8))
                        {
                            sw.WriteLine($"{Inizials[1]}.{Inizials[0]}.{Inizials[2]}");
                            sw.WriteLine("Ваш баланс: " + Money);
                            sw.Close();
                        }
                    }
                    
                }
                catch(Exception ex) 
                {
                    Console.WriteLine("Такого пользователя не существует\n"+ex.Message);
                }
                return _money - money;
            }
        }
        #endregion
    }
}
