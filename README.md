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
### Требования
.NET Standart 2.0
