using TodoList.Models;

namespace TodoList.Repo
{
    public class TasksRepo
    {
        private readonly TodoListContext _context;

        public TasksRepo(TodoListContext context)
        {
            _context = context;
        }

        public virtual List<Models.Task> GetTasksByStatus(string status)
        {
            List<Models.Task> tasksByStatus = new List<Models.Task>();

            if(!status.ToLower().Equals("started") && !status.ToLower().Equals("not started") && !status.ToLower().Equals("completed"))
            {
                return tasksByStatus;
            }

            var byStatusQuery =
                (from tasks in _context.Tasks
                 where tasks.TaskStatus == status
                 select new
                 {
                     taskId = tasks.TaskId,
                     taskDesc = tasks.TaskDesc,
                     taskStatus = tasks.TaskStatus
                 });

            foreach(var task in byStatusQuery)
            {
                tasksByStatus.Add(new Models.Task
                {
                    TaskId = task.taskId,
                    TaskDesc = task.taskDesc,
                    TaskStatus = task.taskStatus
                });
            }

            return tasksByStatus;
        }
    }
}
