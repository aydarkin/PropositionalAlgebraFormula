# Работа с формулами алгебры высказываний
### Менеджер формул
Позволяет создавать все возможные комбинации формул (расстановка скобок). Поддерживает сложные формулы.

Инициализируется отдельно после создания экземпляра:
```sh
var formulaManager = new FormulaManager(input_string);
try
{
    formulaManager.Init();
}
catch (Exception err)
{
    MessageBox.Show(err.Message);
} 
```
### Операторы
В качестве операторов используются символы:
- \+ \- дизъюнкция
- \* \- конъюнкция
- \> \- импликация
- = \- эквиваленция
- ~ \- отрицание

Изменить символы операторов можно в классе [Formula](https://github.com/aydarkin/PropositionalAlgebraFormula/blob/c33ca91da1b18ff8334161d062df71ea2290ff97/Formula.cs#L35)
### Требования
.NET Standart 2.0
