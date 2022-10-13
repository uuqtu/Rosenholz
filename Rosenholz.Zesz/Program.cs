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
            TaskModel item = new TaskModel($"test{DateTime.Now.ToString()}");

            Rosenholz.Model.TaskStorage.Instance.InsertTask(item);


            TaskItemModel tim = null;

            for (int i = 0; i < 23; i++)
            {
                tim = new TaskItemModel(item.Id);

                Rosenholz.Model.TaskStorage.Instance.InsertTaskItem(tim);
            }

            var a = Rosenholz.Model.TaskStorage.Instance.ReadTasks();
        }
    }
}
