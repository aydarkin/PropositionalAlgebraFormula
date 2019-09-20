using System;
using System.Collections.Generic;
using System.Linq;

namespace PropositionalAlgebraFormula
{
    /// <summary>
    /// Класс формул алгебры высказываний
    /// </summary>
    public class Formula
    {
        private string view;
        /// <summary>
        /// Строка формулы
        /// </summary>
        public string View
        {
            get
            {
                return this.view;
            }
            set
            {
                this.view = value;
                ReadOperators();
                ReadVariables();
                if (Operators.Count == 0)
                    throw new Exception("Ошибка при составлении базовой формулы. Операторы отсутствуют.");
            }
        }
        public List<List<int>> Combinations { get; set; }
        public List<Variable> Variables { get; set; }
        public List<char> Operators { get; set; }

        public static List<char> AccessOperators = new List<char> { '+', '*', '>', '=' };
        public static char AccessNegativeOperator = '~';

        /// <summary>
        /// Инициализация формулы алгебры высказываний
        /// </summary>
        /// <param name="view">Строка формулы</param>
        public Formula(string view)
        {
            Combinations = new List<List<int>>();
            Variables = new List<Variable>();
            Operators = new List<char>();
            this.View = view;
        }

        /// <summary>
        /// Получение переменных из формулы в список. 
        /// </summary>
        protected void ReadOperators()
        {
            for (int i = 0; i < this.View.Count(); i++)
            {
                //если оператор из списка, то заносим в массив
                if (AccessOperators.Contains(View[i]))
                    Operators.Add(View[i]);
            }
        }
        /// <summary>
        /// Получение переменных из формулы в список
        /// </summary>
        protected void ReadVariables()
        {
            var str = View.Replace("(", "").Replace(")", "");
            var variables = str.Split(AccessOperators.ToArray());
            foreach (var variable in variables)
            {
                if (string.IsNullOrEmpty(variable))
                    throw new Exception("Ошибка при составлении базовой формулы. Содержатся 1 или более лишних операторов.");
                Variables.Add(new Variable(variable));
            }
        }

        /// <summary>
        /// Генерирование комбинаций приоритетов операторов
        /// </summary>
        public void MakePermutation()
        {
            List<int> combination = new List<int>();
            for (int i = 0; i < Operators.Count; i++)
            {
                combination.Add(0); // особенность алгоритма =), TODO: заменить на нормальный алгоритм сознания перестановок
            }
            while (true)
            {
                var size = this.Operators.Count;
                if (size == 0)
                {
                    Combinations.Clear();
                    Combinations.Add(new List<int>(combination));
                    return;
                }

                if (combination.Distinct().Count() == size)
                {
                    Combinations.Add(new List<int>(combination));
                }

                combination[0]++;
                if (combination[0] == size)
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (combination[size - 1] == size)
                            return;
                        if (combination[i] == size)
                        {
                            combination[i + 1]++;
                            combination[i] -= size;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Получение списка всех комбинаций для отображения
        /// </summary>
        /// <returns>Список формул, оформленных по правилам алгебры высказываний</returns>
        public List<string> GetViewCombinations()
        {
            var views = new List<string>();
            for (int i = 0; i < Combinations.Count; i++)
            {
                views.Add(GetView(Combinations[i]));
            }
            return views;
        }

        /// <summary>
        /// Расстановка скобок согласно приоритетам операторов
        /// </summary>
        /// <param name="combination">Массив приоритетов операторов (комбинация)</param>
        /// <returns>Формула, оформленная по правилам алгебры высказываний</returns>
        public string GetView(List<int> combination)
        {
            var tempVariables = new List<Variable>();
            foreach (var v in Variables)
            {
                tempVariables.Add(new Variable(v)); //чтобы не копировать ссылки
            }
            var tempCombination = new List<int>(combination); //
            var tempOperators = new List<char>(Operators); //а тут копируются значения
            var str = "";
            var count = tempCombination.Count;
            for (int i = 0; i < count; i++)
            {
                var minIndex = tempCombination.IndexOf(0); //индекс минимального номера в комбинации
                str = string.Format("({0}{1}{2})", tempVariables[minIndex], tempOperators[minIndex], tempVariables[minIndex + 1]);
                tempVariables[minIndex].View = str;
                tempVariables[minIndex].IsNegative = false; //после аггрегации убирается отрицание выражения
                tempVariables.RemoveAt(minIndex + 1);

                tempOperators.RemoveAt(minIndex);
                tempCombination.RemoveAt(minIndex);
                for (int j = 0; j < tempCombination.Count; j++)
                {
                    tempCombination[j]--;
                }
            }
            return str;
        }

    }
}
