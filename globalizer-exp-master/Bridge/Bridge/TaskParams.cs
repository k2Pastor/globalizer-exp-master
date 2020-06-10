using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bridge
{
    class TaskParams
    {
        public Tuple<String, String, String>[] MCO_solver =
   {
    Tuple.Create("global_dist", "вещественное число","0.9"),
    Tuple.Create("global_radius","вещественное число","0.2"),
    Tuple.Create("num_minima","целое число","10"),
    Tuple.Create("function_class","целое число","1"),
    Tuple.Create("constraintNums","массив целых чисел (сепаратор \"_\")","2_3"),
    Tuple.Create("function_number","целое число","10"),
    Tuple.Create("ParetoFile", "строка (путь)"," ")

};
        public Tuple<String, String, String>[] deceptive_problem =
  {
    Tuple.Create("function_number", "целое число","10"),
    Tuple.Create("dimension","целое число","2"),
    Tuple.Create("smoothness","вещественное число","2.0")
};

        public Tuple<String, String, String>[] ansys_problem =
{
     Tuple.Create("ansys", "строка"," "),
    Tuple.Create("project","строка"," "),
    Tuple.Create("script","строка"," "),
    Tuple.Create("lower","вещественное число","80.0"),
    Tuple.Create("upper","вещественное число","90.0"),
    Tuple.Create("defaultname","строка"," "),
    Tuple.Create("dimension", "целое число","1"),
    Tuple.Create("numOutputParams","целое число","1")

};

        public Tuple<String, String, String>[] problem_With_Constraints =
{
    Tuple.Create("constraint_count", "целое число","0"),
    Tuple.Create("delta","вещественное число","0.5"),
    Tuple.Create("Q","целое число","0"),
    Tuple.Create("IsZoom","false|true","false"),
    Tuple.Create("IsShift","false|true","false"),
    Tuple.Create("IsBoundaryShift","false|true","false"),
    Tuple.Create("BoundarySearchPrecision", "целое число","20"),
    Tuple.Create("IsImprovementOfTheObjective","false|true","false"),
    Tuple.Create("ImprovementCoefficients","массив целых чисел (сепаратор \"_\")","100_100")

};
    }
}
