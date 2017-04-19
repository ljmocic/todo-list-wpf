using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfToDo
{
    [Serializable]
    public class Task
    {
        public string task { get; set; }

        public Task(string taskText)
        {
            task = taskText;
        }
    }
}
