using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfToDo
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Static Resources
        public static string fileName = "tasks.bin";
        #endregion

        #region Attributes
        private ObservableCollection<Task> tasks;
        public ObservableCollection<Task> Tasks
        {
            get { return tasks; }
            set
            {
                if (value != tasks)
                {
                    tasks = value;
                    OnPropertyChanged("Tasks");
                }
            }
        }

        private Task selectedTask;
        public Task SelectedTask
        {
            get { return selectedTask; }
            set
            {
                if (value != selectedTask)
                {
                    selectedTask = value;
                    OnPropertyChanged("SelectedTask");
                }
            }
        }
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<Task>();
            SelectedTask = null;
            DataContext = this;
            deserializeTasks();
        }
        #endregion

        #region Event Handling
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Tasks.Add(new Task(textBox.Text));
            serializeTasks();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                int index = -1;
                for (int i = 0; i < Tasks.Count; i++)
                {
                    if (Tasks[i].task.Equals(SelectedTask.task))
                    {
                        index = i;
                    }
                }
                if (index != -1)
                {
                    Tasks.RemoveAt(index);
                    OnPropertyChanged("Tasks");
                }
            }
        }
        #endregion

        #region PropertyChangedNotifier
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        #region Serialization
        public void serializeTasks()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = null;

            try
            {
                stream = File.Open("tasks.bin", FileMode.OpenOrCreate);
                formatter.Serialize(stream, Tasks);
            }
            catch
            {
                throw new DriveNotFoundException();
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }
        }

        public void deserializeTasks()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = null;

            if (File.Exists(fileName))
            {
                try
                {
                    stream = File.Open(fileName, FileMode.Open);
                    Tasks = (ObservableCollection<Task>)formatter.Deserialize(stream);
                }
                catch
                {
                    throw new FileNotFoundException();
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }

            }
        }
        #endregion
    }
}
