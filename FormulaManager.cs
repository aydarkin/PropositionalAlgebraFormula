using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PropositionalAlgebraFormula
{
    /// <summary>
    /// Менеджер формул алгебры высказываний
    /// </summary>
    public class FormulaManager
    {
        public List<Formula> Formulas { get; set; }
        public string BaseFormula { get; set; }
        public bool isInit { get; set; } = false;


        /// <summary>
        /// Создание неинициализированного менеджера формул алгебры высказываний
        /// </summary>
        /// <param name="baseFormula">Базовая формула</param>
        public FormulaManager(string baseFormula)
        {
            BaseFormula = baseFormula;
            Formulas = new List<Formula>();
        }
        /// <summary>
        /// Инициализация по базовой формуле
        /// </summary>
        public void Init()
        {
            Init(BaseFormula);
        }
        /// <summary>
        /// Инициализация по базовой формуле
        /// </summary>
        /// <param name="baseFormula">Базовая формула</param>
        public void Init(string baseFormula)
        {
            if (string.IsNullOrEmpty(baseFormula))
                return;

            BaseFormula = baseFormula;
            Formulas.Clear();

            //разделение сложной формулы на простые
            SplitFormula();

            //составление комбинаций
            MakeAllPermutation();

            isInit = true;
        }

        

        /// <summary>
        /// Рекурсивное разделение сложной формулы на простые
        /// </summary>
        /// <returns></returns>
        protected string SplitFormula()
        {
            return SplitFormula(BaseFormula);
        }
        /// <summary>
        /// Рекурсивное разделение сложной формулы на простые
        /// </summary>
        /// <returns></returns>
        protected string SplitFormula(string input)
        {
            int open, close, begin, end;
            open = close = begin = end = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    open++;
                    begin = i;
                    i++;
                    while (open != close)
                    {
                        if (input[i] == '(') open++;
                        if (input[i] == ')') close++;
                        i++;
                    }
                    end = i - 1;
                    var forReplace = input.Substring(begin, end - begin + 1); //со скобками
                    var forNextStep = forReplace.Trim(new char[] { '(', ')'}); //без
                    input = input.Replace(forReplace, SplitFormula(forNextStep));
                    input = "(" + input + ")";

                    //проверка на то, содержит ли формула отрицание всего
                    if(input.StartsWith("(~{") && input.EndsWith("})"))
                    {
                        input = Formulas.Last().View.Insert(1, "~");
                        Formulas.RemoveAt(Formulas.Count - 1); //удалить последний, чтобы потом заменить
                    }
                        
                    Formulas.Add(new Formula(input));
                    return "{" + (Formulas.Count - 1) + "}";
                }
            }
            input = "(" + input + ")";
            Formulas.Add(new Formula(input));
            var result = "{" + (Formulas.Count - 1) + "}";
            return result;
        }

        /// <summary>
        /// Генерация комбинаций для всех простых формул, составляющую базовую
        /// </summary>
        protected void MakeAllPermutation()
        {
            foreach (var formula in Formulas)
                formula.MakePermutation();
        }

        /// <summary>
        /// Получение асинхронно списка всех комбинаций базовой формулы
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllViewCombinationsAsync()
        {
            List<string> result = await Task.Run(() => GetAllViewCombinations());
            return result;
        }

        /// <summary>
        /// Получение списка всех комбинаций базовой формулы
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllViewCombinations()
        {
            if (!isInit)
                return null;

            IEnumerable<IEnumerable<string>> query = new[] { Enumerable.Empty<string>() }; //приведение к интерфейсу для работы с LINQ запросами
            foreach (var f in Formulas)
            {
                var combs = f.GetViewCombinations();
                //декартово произведение с помощью LINQ запроса
                query =
                    from res in query
                    from add in combs
                    select res.Concat(new[] { add }); //аккумулирование значений
            }

            var result = new List<string>();
            foreach (var combinationSplitted in query)
            {
                result.Add(MergeFormula(combinationSplitted.ToList()));
            }
            return result;
        }

        /// <summary>
        /// Рекурсивная сборка формулы из простых, которые её составляют
        /// </summary>
        /// <param name="combinationSplitted"></param>
        /// <returns></returns>
        protected string MergeFormula(List<string> combinationSplitted)
        {
            return MergeFormula(combinationSplitted, combinationSplitted.Last());
        }
        protected string MergeFormula(List<string> combinationSplitted, string str)
        {
            var regex = new Regex(@"{\d+}");
            if (regex.IsMatch(str))
            {
                int num = Int32.Parse(regex.Match(str).ToString().Replace("{", "").Replace("}", ""));
                str = regex.Replace(str, MergeFormula(combinationSplitted, combinationSplitted[num]));
            }
            return str;
        }


    }
}
