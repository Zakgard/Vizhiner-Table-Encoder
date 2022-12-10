using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;
using System.Text;

namespace ИБ_6_Шифратор
{
    class Program
    {
        private static bool isEverythingAlright1 = false;
        private static bool _isEverythingAlright = false;
        private static string _wordToCode;
        private static string _keyWord;
        private static char[] _alphabetList = Enumerable.Range(0, 32).Select((x, i) => (char)('а' + i)).ToArray();
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Вас привествует программа шифровки текста методом таблицы Вижинера, написанная Пироговым Захаром и Сафроновым Пaвлом!");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor= ConsoleColor.White;
            Console.WriteLine("Введите шифруемое слово:");
            _wordToCode = Convert.ToString(Console.ReadLine()).ToLower();
            while (!CheckIfTextInRussian(_wordToCode))
            {
                Console.WriteLine("Введите шифруемое слово:");
                _wordToCode = Convert.ToString(Console.ReadLine()).ToLower(); 
            }
                     
            Console.WriteLine("Введите ключ:");
            _keyWord = Convert.ToString(Console.ReadLine()).ToLower();
            while (!CheckIfTextInRussian(_keyWord))
            {
                Console.WriteLine("Введите ключ:");
                _keyWord = Convert.ToString(Console.ReadLine()).ToLower();
            }
            CreateTheArray(_wordToCode.Length, _keyWord.Length);
        }

        private static bool CheckIfTextInRussian(string text)
        {
            for(int i=0;i< text.Length;i++)
            {
                for(int j=0; j < _alphabetList.Length; j++)
                {
                    if (j == _alphabetList.Length - 1)
                    {
                        if (text.ToCharArray()[i].Equals(_alphabetList[j]) || text.ToCharArray()[i].Equals(' '))
                        {
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor= ConsoleColor.Red;
                            Console.WriteLine("Допускаются только буквы русского алфивита и пробелы!");
                            Console.ForegroundColor = ConsoleColor.White;
                            return false;                           
                        }
                    }
                    else if (text.ToCharArray()[i].Equals(_alphabetList[j]) || text.ToCharArray()[i].Equals(' '))
                    {
                        break;
                    }
                }               
            }
            return true;    
        }

        private static void CreateTheArray(int wordToCodesize, int keyWordSize)
        {
            char[,] codeAlphabet = new char[keyWordSize+1, 33];
            bool tempAcsees=false;
            for (int h=0; h<32; h++)
            {
                codeAlphabet[0, h] = Enumerable.Range(0, 32).Select((x, i) => (char)('а' + i)).ToArray()[h];
            }
            for(int k=0; k < keyWordSize; k++)
            {   
                char[] tempArray = Enumerable.Range(0, 32).Select((x, i) => (char)(_keyWord.ToCharArray()[k] + i)).ToArray();
                for (int j = 0; j < 32; j++)
                {
                    if (!_alphabetList.Contains(tempArray[j]) || tempAcsees)
                    {
                        char[] tempArray2 = Enumerable.Range(0, 32).Select((x, i) => (char)('а' + i)).ToArray();
                        for (int f = 0; f < tempArray2.Length-j; f++)
                        {
                            tempArray[j+f] = tempArray2[f];
                        }
                    }
                    codeAlphabet[k+1, j] = tempArray[j];
                }
                tempArray = null;
            }                     
            CodeTheWord(codeAlphabet, keyWordSize+1);
        }      

        private static void CodeTheWord(char[,] codeAlphabet, int keySize)
        {
            List<char> tempArray = new List<char>();
            List<string> ints= new List<string>();
            int h=1;
            for(int i=0; i < _wordToCode.Length; i++)
            {
                for(int j=0; j<33; j++)
                {
                    if (_wordToCode.ToCharArray()[i].Equals(' '))
                    {
                        tempArray.Add(' ');
                        h -= 1;
                        break;
                    }
                    else if (_wordToCode.ToCharArray()[i].Equals(codeAlphabet[0, j]))
                    {
                        if ((i+h)% _keyWord.Length == 0)
                        {
                            tempArray.Add(codeAlphabet[_keyWord.Length, j]);
                            break;
                        }
                        else
                        {
                            tempArray.Add(codeAlphabet[(i + h) % _keyWord.Length, j]);
                            break;
                        }                                                                                             
                    }
                }
            }          
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("Шифрование окончено!");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("ожидайте запись в файл...");
            Console.ForegroundColor = ConsoleColor.Green;
            SaveCodedMEssageIntoFile(tempArray);
            Console.ReadLine();
        }

        private static void SaveCodedMEssageIntoFile(List<char> tempArray)
        {
            StringBuilder stringBuilder= new StringBuilder();
            for(int i=0; i<tempArray.Count;i++)
            {
                stringBuilder.Append(tempArray[i]);
            }
            try
            {
                string workPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\CodedTExt.txt";
                isEverythingAlright1=true;
                StreamWriter streamWriter = new StreamWriter(workPath);
                streamWriter.WriteLineAsync(stringBuilder.ToString());
                streamWriter.Close();
            }
            catch
            {
                isEverythingAlright1 = false;
                Console.WriteLine("Возникла ошибка!");
                
            }
            try
            {
                string workPath1 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\Key.txt";
                StreamWriter streamWriter1 = new StreamWriter(new FileStream(workPath1, FileMode.Create, FileAccess.Write), Encoding.BigEndianUnicode);
                streamWriter1.WriteAsync(_keyWord.ToString());
                streamWriter1.Close();
                _isEverythingAlright = true;
            }
            catch
            {
                Console.WriteLine("Возникла ошибка!");
                _isEverythingAlright = false;
            }            
            if(_isEverythingAlright && isEverythingAlright1)
            {
                Console.WriteLine("Успешная запись в файл!");
            }           
        }
    }
}
