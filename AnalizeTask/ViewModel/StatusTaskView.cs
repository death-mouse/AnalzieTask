using System.Threading.Tasks;
using System.ComponentModel;

namespace AnalizeTask.View
{
    class StatusTaskView
    {
        public StatusTaskView()
        {
            TaskStatus = new BindingList<Models.TaskStatus>();
        }
        public BindingList<Models.TaskStatus> TaskStatus { get; set; }

        private void initFromFile()
        {
        
        }
    }
}
