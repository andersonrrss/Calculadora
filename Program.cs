using System;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace Calculadora_Basica
{
    internal class Program
    {
        public static bool ErrorMessageShowed = false;
        public static char[] validChars = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '+', '-', '*', '/', ',', '.'};
        static void Main(string[] args)
        {
            Console.WriteLine("Pressione 'Esc' para encerrar a calculadora.\n");

            Console.WriteLine("Soma: +");
            Console.WriteLine("Subtração: -");
            Console.WriteLine("Multiplicação: *");
            Console.WriteLine("Divisão: / \n");

            string userInput = "";
            Console.SetCursorPosition(userInput.Length, Console.CursorTop);

            while (true)
            {
                double result;

                string expression = editExpression(userInput);
                try
                {
                    result = calculateResult(expression);
                    userInput = $"{result}";
                }
                catch (Exception ex)
                {
                    deleteLine();

                    userInput = "";
                    Console.Write("Ocorreu um erro ao efetuar o cálculo :/");

                    ErrorMessageShowed = true;
                }
            }
        }
        static string editExpression(string initialInput)
        {
            StringBuilder input = new StringBuilder(initialInput);
            int cursorPosition = initialInput.Length;

            Console.SetCursorPosition(0, Console.CursorTop); // Move o cursor para o início da linha
            Console.Write(input.ToString()); // Exibe a entrada inicial
            Console.SetCursorPosition(cursorPosition, Console.CursorTop); // Move o cursor para a posição correta

            while (true)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Escape)
                {
                    closeProgram();
                    break;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    deleteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace && cursorPosition > 0)
                {
                    // Remove o último caractere inserido e move o cursor para a posição correta
                    input.Remove(cursorPosition - 1, 1);
                    cursorPosition--;

                    // Move o cursor uma posição para trás
                    Console.SetCursorPosition(cursorPosition, Console.CursorTop);
                    Console.Write(" ");  // Apaga o caractere visivelmente no console
                    Console.SetCursorPosition(cursorPosition, Console.CursorTop); // Move o cursor de volta
                }
                else if (validChars.Any(c => c == key.KeyChar)) // Verifica se é um caractere válido
                {
                    // Apaga a mensagem de erro assim que o usuário tenta insere algum número
                    if (ErrorMessageShowed)
                    {
                        deleteLine();
                        ErrorMessageShowed = false;
                    }
                    // Insere o caractere no input e move o cursor para frente
                    input.Insert(cursorPosition, key.KeyChar);
                    cursorPosition++;

                    // Exibe a entrada na linha
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(input.ToString()); // Mostra a string atualizada
                    Console.SetCursorPosition(cursorPosition, Console.CursorTop); // Move o cursor para a posição correta
                }
            }

            return input.ToString(); // Retorna a expressão editada
        }

        static double calculateResult(string expression)
        {
            if(Thread.CurrentThread.CurrentCulture.Name == "pt-BR")
            {
                expression = expression.Replace(',', '.');  // Substitui vírgulas por pontos para compatibilidade com DataTable.Compute
            }

            // Faz os cálculos a partir da string de expressão
            DataTable table = new DataTable();
            return Convert.ToDouble(table.Compute(expression, null));
        }
        public static void deleteLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public static void closeProgram()
        {
            Console.WriteLine("\nEncerrando Programa..");

            Environment.Exit(0); // Encerra o programa
        }

    }
}
