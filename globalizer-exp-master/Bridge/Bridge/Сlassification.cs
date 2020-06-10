using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bridge
{
    public class Сlassification
    {

        public Tuple<String, String, String, String>[] Other =
   {
    Tuple.Create("-Separator", "любая строка без пробелов, не может быть \".\" и \";\"", "_" , "разделитель элементов массива"),
    Tuple.Create("-HELP","не имеет(без аргумента)"," ","указывает, нужно ли выводить справку"),
    Tuple.Create("-StepPrintMessages",">0","1000","через сколько итераций печатать \r\n в консоли сообщение \r\nо текущем количестве итераций"),
    Tuple.Create("-IsPrintFile","0|1 или false|true","false","печатать отчет в файл"),
    Tuple.Create("-Comment","любая строка без пробелов","000","комментарий к запуску"),
    Tuple.Create("-IsPlot","false|true","false","Рисует линии уровней\r\n для двумерной задачи, \r\n если указан -sip то также \r\nотображаются точки испытаний"),
    Tuple.Create("-IsCalculateNumPoint", "false|true", "false", "Число испытаний \r\nза итерацию будет вычисляться  \r\n на каждой итерации \r\nв методе CalculateNumPoint()"),
    Tuple.Create("-iterPointsSavePath","любая строка без пробелов"," ","путь, по которому будут \r\nсохранены многомерные точки,\r\n поставленные методом \r\nкорневого процесса"),
    Tuple.Create("-printAdvancedInfo","false|true","false","флаг, включающий печать\r\n дополнительной статистики:\r\n оценки констант Гёльдера \r\nи значения функций в точке оптимума"),
    Tuple.Create("-disablePrintParameters","false|true","false","флаг, выключающий печать\r\n параметров при запуске системы"),
    Tuple.Create("-logFileNamePrefix","любая строка без пробелов","examin_log","префикс в имени лог-файла")
};
        public Tuple<String, String, String, String>[] Solver =
{
    Tuple.Create("-NumPoints", ">0", "1", "число точек испытания, порождаемых \r\n методом за одну итерацию"),
    Tuple.Create("-Epsilon", ">0", "0.01", "точность для критерия остановки,\r\n общее значение для всех \r\nуровней дерева процессов"),
    Tuple.Create("-M", ">0", "1", "начальная оценка константы Липшица"),
    Tuple.Create("-m", ">1", "10", "плотность построения развертки \r\n(точность 1/2^m по координате)"),
    Tuple.Create("-MapType", "Перечисление EMapType или значение от 0 до 2", "mpBase", "тип развертки (сдвиговая, вращаемая)"),
    Tuple.Create("-localVerificationType","Перечисление ELocalMethodScheme или значение от 0 до 2","None","cпособ использования локального метода\r\n(только для синхронного типа процесса)"),
    Tuple.Create("-localMix", ">=0", "0", "параметр смешивания в \r\nлокально-глобальном алгоритме,\r\n через какое число итераций \r\n используется локальное уточнение"),
    Tuple.Create("-localAlpha", ">0", "15", "степень локальной адаптации\r\n в локально-глобальном алгоритме"),
    Tuple.Create("-calculationsArray", "непустой массив из чисел от -1 до 2","-1_-1", "распределение типов вычислений по процессам"),
    Tuple.Create("-sepS", "Перечисление ELocalMethodScheme или значение от 0 до 2", "Off", "флаг сепарабельного поиска\r\n на первой итерации"),
    Tuple.Create("-rndS", "0|1 или false|true", "false", "флаг случайного поиска\r\n на первой итерации"),
    Tuple.Create("-stopCondition", "Перечисление EStopCondition или значение от 0 до 4", "Accuracy", "тип критерия остановки"),
    Tuple.Create("-TypeSolver", "Перечисление ETypeSolver или значение от 0 до 2", "SingleSearch", "тип способа решения задачи"),
    Tuple.Create("-DimInTask", "непустой массив из чисел >= 0", "0_0_0_0", "размерности каждой \r\n из подзадач в режиме\r\n сепарабильного или\r\n сикуенсального поиска"),
    Tuple.Create("-Lamda", "вещественные числа", "-1", "параметры свертки(double)"),
    Tuple.Create("-rLamda", "целые числа", "-1", "параметры свертки(int)"),
    Tuple.Create("-numberOfLamda", "целые числа", "50", "количество коэффициентов свертки"),
    Tuple.Create("-isCriteriaScaling", "0|1 или false|true","false", "нужно ли масштабирование\r\n значений критериев при свертке"),
    Tuple.Create("-itrEps", "целые числа", "0", "число итераций до попадания в eps-окрестность")
};
        public Tuple<String, String, String, String>[] Parallel =
{
    Tuple.Create("-TypeCalculation", "перечисление ETypeCalculation \r\n или число от 0 до 2", "OMP", "тип вычислительного ресурса,\r\n используемого для проведения испытаний"),
    Tuple.Create("-TypeProcess", "перечисление ETypeProcess \r\n или значение от 0 до 2", "SynchronousProces", "тип процесса"),
    Tuple.Create("-NumThread", ">0", "1", "число используемых потоков"),
    Tuple.Create("-SizeInBlock", ">0", "32", "размер CUDA блока"),
    Tuple.Create("-deviceCount", ">=-1","-1", "количество используемых ускорителей"),
    Tuple.Create("-IsSetDevice", "0|1 или false|true","false", "назначать каждому процессу свое устройство (ускоритель)"),
    Tuple.Create("-deviceIndex", ">=0","-1", "Индекс используемого \r\nустройства (ускорителей),\r\n если -1 используется \r\nпервые deviceCount устройств"),
    Tuple.Create("-ProcRank",">=0", "0", "Номер MPI процесса,\r\n вычисляется автоматически (нельзя задать)")
};
        public Tuple<String, String, String, String>[] Method =
{
    Tuple.Create("-TypeMethod","перечисление ETypeMethod \r\n или число от 0 до 8","StandartMethod","тип используемого метода"),
    Tuple.Create("-r",">1","2.3","надежность метода"),
    Tuple.Create("-rEps",">0","0.01","eps-резервирование, используется в индексном методе"),
    Tuple.Create("-rs ","непустой массив из чисел > 1","2.3_2.3_2.3_2.3","значение r для каждого из уровней дерева"),
    Tuple.Create("-Eps","непустой массив из чисел > 0","0.01_0.01_0.01_0.01","значение Epsilon для каждого из уровней дерева процессов"),
    Tuple.Create("-NumOfTaskLevels","от 1 до 5","1","число уровней в дереве задач,\r\n совпадает с NumOfProcLevels"),
    Tuple.Create("-DimInTaskLevel","непустой массив из неотрицательных чисел","2_0_0_0","число размерностей на каждом\r\n уровне дерева задач .\r\nразмер: NumOfTaskLevels"),
    Tuple.Create("-ChildInProcLevel","непустой массив из неотрицательных чисел","0_0_0_0","число потомков у процессов \r\n на уровнях с 1 до NumOfTaskLevels – 1\r\n размер: NumOfProcLevels – 1\r\n уровень NumOfTaskLevels – процессы-листья"),
    Tuple.Create("-MapInLevel", "непустой массив из чисел > 0", "1_1_1_1", "число разверток \r\nна каждом уровне дерева процессов\r\n размер: NumOfProcLevels"),
    Tuple.Create("-MapProcInLevel", "непустой массив из чисел > 0", "1_1_1_1","число процессов \r\nна каждом уровне дерева процессов,\r\n использующих множественную развертку\r\n размер - NumOfProcLevels\r\n последний уровень по разверткам не параллелится\r\n* число процессов обрабатывающие разные \r\n развертки на уровне (узле дерева распараллеливания)\r\n*если один корень то MapInLevel[0]*ProcNum=MapInLevel[0]\r\n*определяет число соседей"),
    Tuple.Create("-MaxNumOfPoints", "непустой массив из чисел > 0", "7000000_1000000_1000000_1000000", "максимальное число итераций\r\n для процессов на каждом уровне")
};
        public Tuple<String, String, String, String>[] Task =
{
    Tuple.Create("-Dimension", ">0", "2", "размерность исходной задачи"),
    Tuple.Create("-libPath", "любая строка без пробелов", "rastrigin.dll | ./librastrigin.so", "путь к библиотеке с задачей"),
    Tuple.Create("-libConfigPath", "любая строка без пробелов", " ", "путь к конфигурации для задачи"),
    Tuple.Create("-isLoadFirstPointFromFile", "0|1 или false|true", "false", "Загружать начальные точки из файла\r\n или распределять их равномерно \r\n(не поддерживается)"),
    Tuple.Create("-FirstPointFilePath", "любая строка без пробелов", " ", "Путь откуда будут считаны начальные точки испытания"),
    Tuple.Create("-func_num", "от 1 до 100", "1", "номер задачи из генератора GKLS"),
    Tuple.Create("-GKLS_global_dist",">0", "0.9", "расстояние от параболоидного минимизатора"),
    Tuple.Create("-GKLS_global_radius",">0", "0.33", "радиус глобального минимизатора")
};
        public Сlassification()
        { }

    }
}
