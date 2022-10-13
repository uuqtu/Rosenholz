// See https://aka.ms/new-console-template for more information
using Rosenholz.Model;

Console.WriteLine("Hello, World!");

TaskModel item = new TaskModel("test");

Rosenholz.Model.TaskStorage.Instance.InsertTask(item);


TaskItemModel tim = new TaskItemModel(item.Id);

Rosenholz.Model.TaskStorage.Instance.InsertTaskItem(tim);
