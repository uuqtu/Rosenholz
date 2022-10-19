using Rosenholz.Extensions;
using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Zesz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //    TaskModel item = new TaskModel($"test{DateTime.Now.ToString()}");

            //    Rosenholz.Model.TaskStorage.Instance.InsertTask(item);


            //    TaskItemModel tim = null;

            //    for (int i = 0; i < 23; i++)
            //    {
            //        tim = new TaskItemModel(item.Id);

            //        Rosenholz.Model.TaskStorage.Instance.InsertTaskItem(tim);
            //    }

            //    var a = Rosenholz.Model.TaskStorage.Instance.ReadTasks();
            //}


            var a = Model.TaskStorage.Instance.ReadTasks();

            var basicTable = new TextTable(4);
            basicTable.AddCell("Level");
            basicTable.AddCell("State");
            basicTable.AddCell("Title");
            basicTable.AddCell("Description");

            foreach (var parent in a)
            {
                basicTable.AddCell(NewMethod(parent));
                basicTable.AddCell(parent.TaskStateString);
                basicTable.AddCell(parent.Title.Truncate(50));
                basicTable.AddCell(parent.Description.Truncate(50));

                foreach (var child in parent.LinkedTaskItems)
                {
                    writechildren(basicTable, child);
                }

            }

            Console.WriteLine(basicTable.Render());
        }

        private static string NewMethod(TaskModel parent)
        {
            switch (parent.Level)
            {
                case 0: return "0";
                case 1: return "|-1";
                case 2: return "||-2";
                case 3: return "|||-3";
                case 4: return "||||-4";
                case 5: return "|||||-5";
                case 6: return "||||||-6";
                case 7: return "|||||||-7";
                case 8: return "||||||||-8";
                case 9: return "|||||||||-9";
                default: return "";
            }
        }

        public static void writechildren(TextTable t, TaskModel tm)
        {
            t.AddCell(NewMethod(tm));
            t.AddCell(tm.TaskStateString);
            t.AddCell(tm.Title.Truncate(50));
            t.AddCell(tm.Description.Truncate(50));

            if (tm.LinkedTaskItems != null)
            {

                foreach (var child in tm.LinkedTaskItems)
                {
                    if (child != null)
                        writechildren(t, child);
                    else
                        continue;
                }

            }
            return;
        }
    }





    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength, string truncationSuffix = "…")
        {
            return value?.Length > maxLength
                ? value.Substring(0, maxLength) + truncationSuffix
                : value;
        }
    }
}
