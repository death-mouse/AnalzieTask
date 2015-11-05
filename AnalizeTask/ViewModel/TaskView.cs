using System;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using AnalizeTask.Models;
using AnalizeTask.need;
using System.Windows.Input;
using AnalizeTask.ViewModel;
using AnalizeTask.Notification;
using System.Xml;
using MahApps.Metro.Controls;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using Xceed.Wpf.Toolkit;
using Hardcodet.Wpf.TaskbarNotification;
using System.Xml.Linq;

namespace AnalizeTask.View
{
    class TaskView
    {
        public Browser browser;
        
        public BindingList<Task> TaskModel { get; set; }
        public BindingList<TaskStatus> TaskStatus { get; set; }
        public BindingList<ColorsModel> ColorsModel { get; set; }
        public BindingList<ThemModel> themModel { get; set; }
        public BindingList<TaskLife> TaskLife { get; set; }
        public BindingList<TaskComment> TaskComment { get; set; }
        public BindingList<TaskType> TaskType { get; set; }
        public bool isRunAnalize;
        public ProgressDialogController controller;
        public int selectedIndex;
        public int SelectedIndex 
        { 
           
            get 
            {
                return selectedIndex;
            } set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    //if (isRunAnalize )
                    if (browser != null)
                    {
                        if (TaskModel.Count < selectedIndex)
                            return;
                        string taskLifetimeXML = browser.GET(string.Format(string.Format("{0}api/TaskLifetime?taskid={1}",Properties.Settings.Default["URL"], TaskModel[selectedIndex].TaskId)), Encoding.UTF8);
                        XmlDocument newXmlDocument = new XmlDocument();
                        newXmlDocument.LoadXml(taskLifetimeXML);
                        if (TaskLife.Count != 0)
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate
                                       {
                                           TaskLife.Clear();
                                       });
                        }

                        if (TaskComment.Count != 0)
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate
                                       {
                                           TaskComment.Clear();
                                       });
                        }
                        

                        foreach (XmlElement xmlElement in newXmlDocument.DocumentElement)
                        {
                            XmlNodeList xmlNodeList = xmlElement.SelectNodes("TaskLifetime");
                            foreach (XmlNode xmlNode in xmlNodeList)
                            {
                                string date = xmlNode.SelectSingleNode("Date").InnerText;
                                
                                string image = "" ;
                                string text = "";
                                XmlNode xmlImage = xmlNode.SelectSingleNode("StatusId");
                                if (xmlImage == null)
                                    image = this.findOrAddImage("http://servicedesk.gradient.ru/img/newcomment_gray.png");
                                else
                                {
                                    //ObservableCollection<AnalizeTask.Models.TaskStatus> filtererdTests;
                                    ObservableCollection<Models.TaskStatus>filtererdTests;
                                    filtererdTests = new ObservableCollection<Models.TaskStatus>(TaskStatus.Where(t => t.Id == xmlImage.InnerText));
                                    image = this.findOrAddImage(filtererdTests[0].ImageUrl);
                                    text+=string.Format("Изменен статус заявки -> {0}\n\n",filtererdTests[0].Name);
                                }
                                string editor = xmlNode.SelectSingleNode("Editor").InnerText+"\n";
                                XmlNode xmlAnalize = xmlNode.SelectSingleNode("Executors");

                                if(xmlAnalize != null)
                                    text += string.Format("Назначены исполнители: {0}\n\n", xmlAnalize.InnerText);

                                xmlAnalize = xmlNode.SelectSingleNode("Participants");

                                if (xmlAnalize != null)
                                    text += string.Format("Назначены наблюдатели: {0}\n\n", xmlAnalize.InnerText);

                                xmlAnalize = xmlNode.SelectSingleNode("Categories");

                                if (xmlAnalize != null)
                                    text += string.Format("Изменены категории заявки -> {0}\n\n", xmlAnalize.InnerText);

                                xmlAnalize = xmlNode.SelectSingleNode("Files");

                                if (xmlAnalize != null)
                                {
                                    string[] file = xmlAnalize.InnerText.Split(new char[]{','});
                                    string line = "";

                                    foreach(string str in file)
                                    {
                                        string[] split = str.Split(new char[]{'|'});
                                        if (split.Length == 1)
                                            continue;
                                        if (str == "")
                                            continue;
                                        if(line == "")
                                            line = str.Split(new char[]{'|'})[1];
                                        else
                                        {  
                                            line = string.Format("{0},{1}", line, str.Split(new char[]{'|'})[1]);
                                        }
                                       
                                    }
                                    text += string.Format("Добавлены/Удалены файлы -> {0}\n\n", line);
                                }

                                xmlAnalize = xmlNode.SelectSingleNode("Comments");

                                if (xmlAnalize != null)
                                {
                                    if (xmlAnalize.InnerText.IndexOf("создал заявку от") == -1)
                                    {
                                        text += string.Format("{0}\n\n", xmlAnalize.InnerText);
                                        string comment = string.Format("{0}\n\n", xmlAnalize.InnerText);
                                        Application.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            TaskComment.Add(new TaskComment() { Editor = editor, Image = image, Date = date, Text = comment });
                                        });
                                    }
                                }

                                text = text.Trim();
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    TaskLife.Add(new TaskLife() { Editor = editor, Image = image, Date = date, Text = text });
                                });
                            }
                            
                        }
                    }
                }
            } 
        }


        public ICommand ClickCommand { get; set; }
        public ICommand ClickCommandAddTask { get; set; }
        public ICommand ClickCommandDeleteTask { get; set; }
        
        public TaskView()
        {
            ClickCommand = new RelayCommand(arg => GetTask());
            ClickCommandAddTask = new RelayCommand(arg => addTask());
            ClickCommandDeleteTask = new RelayCommand(arg => deleteTask());
            /*TaskModel = new ObservableCollection<AnalizeTask.Models.Task>();
            ColorsModel = new ObservableCollection<ColorsModel>();
            themModel = new ObservableCollection<ThemModel>();
            TaskLife = new ObservableCollection<Models.TaskLife>();
            TaskComment = new ObservableCollection<Models.TaskComment>();*/
            TaskModel = new BindingList<Models.Task>();
            ColorsModel = new BindingList<Models.ColorsModel>();
            themModel = new BindingList<ThemModel>();
            TaskLife = new BindingList<Models.TaskLife>();
            TaskComment = new BindingList<Models.TaskComment>();
            TaskType = new BindingList<Models.TaskType>();

            initColor();
           
            //TaskModel.Add(new AnalizeTask.Models.Task() { TaskId = "46580" });
            //TaskModel.Add(new AnalizeTask.Models.Task() { TaskId = "83795" });
            this.initFromFile();
            //this.GetTaskTherd();
        }
        private void initFromFile()
        {
            if (System.IO.File.Exists(string.Format(@"{0}\{1}",Environment.CurrentDirectory, Properties.Settings.Default["FileName"])))
            {
                int i = 0;
                XmlDocument newXmlDocument = new XmlDocument();
                newXmlDocument.Load(string.Format(@"{0}\{1}",Environment.CurrentDirectory, Properties.Settings.Default["FileName"]));
                foreach (XmlElement xmlElement in newXmlDocument.DocumentElement)
                {
                    //XmlNodeList xmlNodeList = xmlElement.SelectNodes("Task");
                    //foreach (XmlNode xmlNode in xmlNodeList)
                    {
                        string taskId;
                        string creater;
                        string taskStatus;
                        string taskName;
                        string taskPerfomer;
                        string image;
                        DateTime deadLine;

                        taskId = xmlElement.GetElementsByTagName("TaskId")[0].InnerText;
                        creater = xmlElement.GetElementsByTagName("Creater")[0].InnerText;
                        taskStatus = xmlElement.GetElementsByTagName("TaskStatus")[0].InnerText;
                        taskName = xmlElement.GetElementsByTagName("TaskName")[0].InnerText;
                        image = xmlElement.GetElementsByTagName("Image")[0].InnerText;
                        taskPerfomer = xmlElement.GetElementsByTagName("TaskPerfomer")[0].InnerText;
                        deadLine = Convert.ToDateTime(xmlElement.GetElementsByTagName("DeadLine")[0].InnerText);
                        TaskModel.Add(new Task() { TaskId = taskId, Creater = creater, Image = image, TaskName = taskName, TaskPerfomer = taskPerfomer, TaskStatus = taskStatus, TaskDeadLine = deadLine });
                        if (i == 1)
                        {
                            SelectedIndex = 1;
                            SelectedIndex = 0;
                        }
                        i++;
                    }
                }
                //AnalizeTask();
            }
        }
        private void initColor()
        {

            ColorsModel.Add(new ColorsModel() { colors = "Red" });
            ColorsModel.Add(new ColorsModel() { colors = "Green" });
            ColorsModel.Add(new ColorsModel() { colors = "Blue" });
            ColorsModel.Add(new ColorsModel() { colors = "Purple" });
            ColorsModel.Add(new ColorsModel() { colors = "Orange" });
            ColorsModel.Add(new ColorsModel() { colors = "Lime" });
            ColorsModel.Add(new ColorsModel() { colors = "Emerald" });
            ColorsModel.Add(new ColorsModel() { colors = "Teal" });
            ColorsModel.Add(new ColorsModel() { colors = "Cyan" });
            ColorsModel.Add(new ColorsModel() { colors = "Cobalt" });
            ColorsModel.Add(new ColorsModel() { colors = "Indigo" });
            ColorsModel.Add(new ColorsModel() { colors = "Violet" });
            ColorsModel.Add(new ColorsModel() { colors = "Pink" });
            ColorsModel.Add(new ColorsModel() { colors = "Magenta" });
            ColorsModel.Add(new ColorsModel() { colors = "Crimson" });
            ColorsModel.Add(new ColorsModel() { colors = "Amber" });
            ColorsModel.Add(new ColorsModel() { colors = "Yellow" });
            ColorsModel.Add(new ColorsModel() { colors = "Brown" });
            ColorsModel.Add(new ColorsModel() { colors = "Olive" });
            ColorsModel.Add(new ColorsModel() { colors = "Steel" });
            ColorsModel.Add(new ColorsModel() { colors = "Mauve" });
            ColorsModel.Add(new ColorsModel() { colors = "Taupe" });
            ColorsModel.Add(new ColorsModel() { colors = "Sienna" });

            themModel.Add(new ThemModel() { colors = "BaseLight" });
            themModel.Add(new ThemModel() { colors = "BaseDark" });

        }
        private string findOrAddImage(string _url)
        {
            if (_url.IndexOf("http://") == -1)
                return _url;
            if (!System.IO.Directory.Exists(Environment.CurrentDirectory + @"\ico"))
                System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + @"\ico");

            string[] fileNameTemp = _url.Split(new char[] { '/' });
            string fileName = fileNameTemp[fileNameTemp.Length - 1];
            fileName = string.Format(@"{0}\ico\{1}", Environment.CurrentDirectory, fileName);
            if (!System.IO.File.Exists(fileName))
            {
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default["Login"].ToString(), Properties.Settings.Default["Password"].ToString(), Properties.Settings.Default["Domain"].ToString());
                webClient.DownloadFile(_url, fileName);
            }

            

            return fileName;
        }
        private bool _caseSensitive;
        public bool CaseSensitive
        {
            get { return Properties.Settings.Default.AutoUpdate; }
            set
            {
                _caseSensitive = value;
                Properties.Settings.Default.AutoUpdate = value;
            }
        }
        private  void GetTask()
        {
            GetTaskTherd();
        }
        public async System.Threading.Tasks.Task<bool> getThredTask()
        {
            await GetTaskTherd();

            return true;
        }
        public async System.Threading.Tasks.Task<bool> GetTaskTherd()
        {

            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
            controller = await metroWindow.ShowProgressAsync("Идет обновление задач", "В данный момент идет обновления задач");
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            controller.SetProgress(0);
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            isRunAnalize = true;
            worker.RunWorkerAsync(this);

            return true;
            
            
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            controller.CloseAsync();
            isRunAnalize = false;
            MainWindow main = (MainWindow)Application.Current.Windows[0];
            main.UpdateLayout();
            main.updateLyout();

        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            isRunAnalize = true;
            AnalizeTask();
           
        }

        private bool AnalizeOneTask(string _taskId)
        {
            if (browser == null)
            {
                browser = new Browser();
                string empty = browser.GET(/*"http://servicedesk.gradient.ru/"*/string.Format("{0}", Properties.Settings.Default["URL"]), Encoding.UTF8);
            }
            if (TaskStatus == null || TaskStatus.Count == 0)
            {

                TaskStatus = new BindingList<Models.TaskStatus>();//new ObservableCollection<Models.TaskStatus>();
                string taskStatusXML = browser.GET(/*"http://servicedesk.gradient.ru/api/taskstatus"*/string.Format("{0}/api/taskstatus", Properties.Settings.Default["URL"]), Encoding.UTF8);
                XmlDocument newXmlDocument = new XmlDocument();
                newXmlDocument.LoadXml(taskStatusXML);

                foreach (XmlElement xmlElement in newXmlDocument.DocumentElement)
                {
                    string description;
                    string id;
                    string imageUrl;
                    string name;
                    description = xmlElement.GetElementsByTagName("Description")[0].InnerText;
                    id = xmlElement.GetElementsByTagName("Id")[0].InnerText;

                    imageUrl = this.findOrAddImage(xmlElement.GetElementsByTagName("Image16Url")[0].InnerText);
                    name = xmlElement.GetElementsByTagName("Name")[0].InnerText;

                    TaskStatus.Add(new Models.TaskStatus() { Name = name, Description = description, Id = id, ImageUrl = imageUrl });

                }
            }
            
            ObservableCollection<Models.Task> filtererdTask;
            filtererdTask = new ObservableCollection<Models.Task>(TaskModel.Where(t => t.TaskId == _taskId));
            //BindingList<AnalizeTask.Models.Task> taskBinding = new BindingList<AnalizeTask.Models.Task>(){}
            int i = TaskModel.IndexOf(filtererdTask[0]);
            string taskInfo = browser.GET(/*string.Format("http://servicedesk.gradient.ru/api/task/{0}",task.TaskId)*/string.Format("{0}/api/task/{1}", Properties.Settings.Default["URL"], _taskId), Encoding.UTF8);
            if (taskInfo == null)
            {
                TaskModel.Remove(filtererdTask[0]);
                return false;
            }
            
            
            XmlDocument newXmlDocumentTask = new XmlDocument();
            newXmlDocumentTask.LoadXml(taskInfo);
            
            foreach (XmlElement xmlElement in newXmlDocumentTask.DocumentElement)
            {
                string taskId;
                string creater;
                string taskStatus;
                string taskName;
                string taskPerfomer;
                string observers;
                string serviceName;
                string type;
                string description;
                string priorityName;
                string statusName;
                string completionStatus;

                taskId = xmlElement.GetElementsByTagName("Id")[0].InnerText;
                creater = xmlElement.GetElementsByTagName("Creator")[0].InnerText;
                taskStatus = xmlElement.GetElementsByTagName("StatusId")[0].InnerText;
                taskName = xmlElement.GetElementsByTagName("Name")[0].InnerText;
                taskPerfomer = xmlElement.GetElementsByTagName("Executors")[0].InnerText;
                observers = xmlElement.GetElementsByTagName("Observers")[0].InnerText;
                serviceName = xmlElement.GetElementsByTagName("ServiceName")[0].InnerText;
                type = xmlElement.GetElementsByTagName("Type")[0].InnerText;
                description = xmlElement.GetElementsByTagName("Description")[0].InnerText;
                priorityName = xmlElement.GetElementsByTagName("PriorityName")[0].InnerText;
                statusName = xmlElement.GetElementsByTagName("StatusName")[0].InnerText;
                completionStatus = xmlElement.GetElementsByTagName("CompletionStatus")[0].InnerText;

                if (TaskStatus == null)
                    TaskStatus = new BindingList<Models.TaskStatus>();

                ObservableCollection<AnalizeTask.Models.TaskStatus> filtererdTests;
                filtererdTests = new ObservableCollection<Models.TaskStatus>(TaskStatus.Where(t => t.Id == taskStatus));
                if (filtererdTests.Count == 0)
                {
                    this.addTaskStatus();
                    filtererdTests = new ObservableCollection<Models.TaskStatus>(TaskStatus.Where(t => t.Id == taskStatus));
                }
                
                if (TaskType == null)
                    TaskType = new BindingList<TaskType>();
                ObservableCollection<AnalizeTask.Models.TaskType> filtererdTaskType;
                string serviceId = xmlElement.GetElementsByTagName("ServiceId")[0].InnerText;
                string typeId = xmlElement.GetElementsByTagName("TypeId")[0].InnerText;
                filtererdTaskType = new ObservableCollection<Models.TaskType>(TaskType.Where(t => t.Id == typeId));
                if (filtererdTaskType.Count == 0)
                {
                    this.addTaskType(serviceId);
                    filtererdTaskType = new ObservableCollection<Models.TaskType>(TaskType.Where(t => t.Id == typeId));
                }
                TaskModel[i].TaskId = taskId;
                TaskModel[i].Creater = creater;
                TaskModel[i].Image = filtererdTests[0].ImageUrl;
                TaskModel[i].Image24 = filtererdTests[0].ImageUrl24;
                TaskModel[i].ImageTaskType = filtererdTaskType[0].ImgUrl;
                TaskModel[i].TaskStatus = filtererdTests[0].Name;
                TaskModel[i].TaskName = taskName;
                TaskModel[i].TaskPerfomer = taskPerfomer;
                TaskModel[i].Observers = observers;
                TaskModel[i].ServiceName = serviceName;
                TaskModel[i].Type = type;
                TaskModel[i].Description = description;
                TaskModel[i].StatusName = statusName;
                TaskModel[i].PriorityName = priorityName;
                TaskModel[i].CompletionStatus = completionStatus;
                Application.Current.Dispatcher.Invoke((Action)delegate
                {

                   
                    /*AddCommnet addComment = new AddCommnet();
                    addComment.DataContext = new AnalizeTask.Models.AddComment() { Comment = "Тестирование добавление комментария", TaskId = taskId };
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.Notification.ShowCustomBalloon(addComment, System.Windows.Controls.Primitives.PopupAnimation.Slide, 400);*/
                });
                if (i == 1)
                {
                    SelectedIndex = 1;
                    SelectedIndex = 0;
                }
                
                i++;


            }

            return true;
        }
        private void AnalizeTask()
        {
            double progressCount;
            browser = new Browser();
            string empty = browser.GET(string.Format("{0}" ,Properties.Settings.Default["URL"]), Encoding.UTF8);
            if (empty == null)
                return;
            
            int i = 0;
            progressCount = (double)1 / TaskModel.Count;
            progressCount = Math.Round(progressCount, 4);
            double progressOne = progressCount;
            progressCount = 0;
            foreach (Task task in TaskModel)
            {
                string taskInfo = browser.GET(string.Format("{0}/api/task/{1}",Properties.Settings.Default["URL"],task.TaskId) , Encoding.UTF8);
                XmlDocument newXmlDocument = new XmlDocument();
                newXmlDocument.LoadXml(taskInfo);
                foreach (XmlElement xmlElement in newXmlDocument.DocumentElement)
                {
                    string taskId;
                    string creater;
                    string taskStatus;
                    string taskName;
                    string taskPerfomer;
                    string observers;
                    string serviceName;
                    string type;
                    string description;
                    string priorityName;
                    string statusName;
                    string completionStatus;
                   
                    taskId = xmlElement.GetElementsByTagName("Id")[0].InnerText;
                    if(controller != null)
                        controller.SetMessage(string.Format("Обновление заявки {0}", taskId));
                    creater = xmlElement.GetElementsByTagName("Creator")[0].InnerText;
                    taskStatus = xmlElement.GetElementsByTagName("StatusId")[0].InnerText;
                    taskName = xmlElement.GetElementsByTagName("Name")[0].InnerText;
                    taskPerfomer = xmlElement.GetElementsByTagName("Executors")[0].InnerText;

                    observers = xmlElement.GetElementsByTagName("Observers")[0].InnerText;
                    serviceName = xmlElement.GetElementsByTagName("ServiceName")[0].InnerText;
                    type = xmlElement.GetElementsByTagName("Type")[0].InnerText;
                    description = xmlElement.GetElementsByTagName("Description")[0].InnerText;
                    priorityName = xmlElement.GetElementsByTagName("PriorityName")[0].InnerText;
                    statusName = xmlElement.GetElementsByTagName("StatusName")[0].InnerText;
                    completionStatus = xmlElement.GetElementsByTagName("CompletionStatus")[0].InnerText;
                    if (TaskStatus == null)
                        TaskStatus = new BindingList<Models.TaskStatus>();
                    ObservableCollection<AnalizeTask.Models.TaskStatus> filtererdTests;
                    filtererdTests = new ObservableCollection<Models.TaskStatus>(TaskStatus.Where(t => t.Id == taskStatus));
                    if (filtererdTests.Count == 0)
                    {
                        this.addTaskStatus();
                        filtererdTests = new ObservableCollection<Models.TaskStatus>(TaskStatus.Where(t => t.Id == taskStatus));
                    }

                    if (TaskType == null)
                        TaskType = new BindingList<TaskType>();
                    ObservableCollection<AnalizeTask.Models.TaskType> filtererdTaskType;
                    string serviceId = xmlElement.GetElementsByTagName("ServiceId")[0].InnerText;
                    string typeId = xmlElement.GetElementsByTagName("TypeId")[0].InnerText;
                    filtererdTaskType = new ObservableCollection<Models.TaskType>(TaskType.Where(t => t.Id == typeId));
                    if (filtererdTaskType.Count == 0)
                    {
                        this.addTaskType(serviceId);
                        filtererdTaskType = new ObservableCollection<Models.TaskType>(TaskType.Where(t => t.Id == typeId));
                    }
                    TaskModel[i].TaskId = taskId;
                    TaskModel[i].Creater = creater;
                    TaskModel[i].Image = filtererdTests[0].ImageUrl;
                    TaskModel[i].Image24 = filtererdTests[0].ImageUrl24;
                    TaskModel[i].ImageTaskType = filtererdTaskType[0].ImgUrl;
                    TaskModel[i].TaskStatus = filtererdTests[0].Name;
                    TaskModel[i].StatusId = filtererdTests[0].Id;
                    TaskModel[i].TaskName = taskName;
                    TaskModel[i].TaskPerfomer = taskPerfomer;
                    TaskModel[i].Observers = observers;
                    TaskModel[i].ServiceName = serviceName;
                    TaskModel[i].Type = type;
                    TaskModel[i].Description = description;
                    TaskModel[i].StatusName = statusName;
                    TaskModel[i].PriorityName = priorityName;
                    TaskModel[i].CompletionStatus = completionStatus;

                    if (i == 1)
                    {
                        SelectedIndex = 1;
                        SelectedIndex = 0;
                    }
                    i++;
                    if (controller != null)
                    {
                        controller.SetMessage(string.Format("Заявка {0} обновлена", taskId));
                        controller.SetProgress(progressCount);
                    }
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        this.AnalazeChanged(taskId);
                        /*AddCommnet addComment = new AddCommnet();
                   
                        addComment.DataContext = new AnalizeTask.Models.AddComment() { Comment = "Тестирование добавление комментария", TaskId = taskId };
                        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.Notification.ShowCustomBalloon(addComment, System.Windows.Controls.Primitives.PopupAnimation.Slide, 500);*/
                    });
                        progressCount += progressOne;


                }
            }
            
        }
        private void AnalazeChanged(String _taskId)
        {
            DateTime dateTime = (DateTime)Properties.Settings.Default["LastAnalize"];
            /*DateTime dateTime = new DateTime();
            dateTime = DateTime.Parse("31.08.2015 0:38:41");*/
            
            string xml = browser.GET(string.Format(string.Format("{0}api/TaskLifetime?taskid={1}", Properties.Settings.Default["URL"], _taskId)), Encoding.UTF8);

            XDocument xmlDoc = XDocument.Parse(xml);
            /*IEnumerable<XElement> tracks = from t in xmlDoc.Root.Elements("TaskLifetimes").Elements("TaskLifetime").Where
                                                (t => t.Element("Date").Value == dateTime.ToString())
                                                select t;*/
            IEnumerable<XElement> changed = xmlDoc.Root.Descendants("TaskLifetimes").Elements("TaskLifetime").Where(
                                                        t => (DateTime.Parse(t.Element("Date").Value) >= dateTime)).ToList();
            string text="";
            string user;
            string what;
            bool changeStatus = false;
            foreach (XElement change in changed)
            {
                user = change.Element("Editor").Value;
                if(change.Element("StatusId") != null)
                {
                    ObservableCollection<Models.TaskStatus> filtererdTests = new ObservableCollection<Models.TaskStatus>(TaskStatus.Where(t => t.Id == change.Element("StatusId").Value));
                    if (filtererdTests.Count == 0)
                    {
                        this.addTaskStatus();
                        filtererdTests = new ObservableCollection<Models.TaskStatus>(TaskStatus.Where(t => t.Id == change.Element("StatusId").Value));
                        
                    }
                    text += string.Format("Пользователь {0} изменил статус на {1}\n\r", user, filtererdTests[0].Name);
                    changeStatus = true;
                }
            }
            if(changeStatus)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    AddNewStatus addComment = new AddNewStatus();

                    addComment.DataContext = new AnalizeTask.Models.AddComment() { Comment = text, TaskId = _taskId };
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.Notification.ShowCustomBalloon(addComment, System.Windows.Controls.Primitives.PopupAnimation.Slide, 500);
                });
            }
        }
        private void addTaskStatus()
        {
            if (TaskStatus == null || TaskStatus.Count == 0)
                TaskStatus = new BindingList<Models.TaskStatus>();
            string taskStatusXML = browser.GET(string.Format("{0}/api/taskstatus", Properties.Settings.Default["URL"]), Encoding.UTF8);
            if (taskStatusXML == null)
                return;
            XmlDocument newXmlDocument = new XmlDocument();
            newXmlDocument.LoadXml(taskStatusXML);

            foreach (XmlElement xmlElement in newXmlDocument.DocumentElement)
            {
                string description;
                string id;
                string imageUrl;
                string imageUrl24;
                string name;
                description = xmlElement.GetElementsByTagName("Description")[0].InnerText;
                id = xmlElement.GetElementsByTagName("Id")[0].InnerText;

                imageUrl = this.findOrAddImage(xmlElement.GetElementsByTagName("Image16Url")[0].InnerText);
                imageUrl24 = this.findOrAddImage(xmlElement.GetElementsByTagName("Image24Url")[0].InnerText);
                name = xmlElement.GetElementsByTagName("Name")[0].InnerText;

                TaskStatus.Add(new Models.TaskStatus() { Name = name, Description = description, Id = id, ImageUrl = imageUrl, ImageUrl24 = imageUrl24 });

            }
        }
        private void addTaskType(string _serviceId)
        {
            if (TaskType == null)
                TaskType = new BindingList<TaskType>();
            string taskTypeXML = browser.GET(string.Format("{0}/api/tasktype?serviceid={1}", Properties.Settings.Default["URL"], _serviceId), Encoding.UTF8);
            if (taskTypeXML == null)
                return;
            XmlDocument newXmlDocument = new XmlDocument();
            newXmlDocument.LoadXml(taskTypeXML);

            foreach (XmlElement xmlElement in newXmlDocument.DocumentElement)
            {
                if (xmlElement.Name == "Paginator")
                    continue;
                string id;
                string name;
                string imgUrl;
                
                id = xmlElement.GetElementsByTagName("Id")[0].InnerText;

                name = xmlElement.GetElementsByTagName("Name")[0].InnerText;
                imgUrl = this.findOrAddImage(xmlElement.GetElementsByTagName("Image16Url")[0].InnerText);
                name = xmlElement.GetElementsByTagName("Name")[0].InnerText;

                TaskType.Add(new Models.TaskType() { Name = name, Id = id, ImgUrl = imgUrl });

            }
        }
        

        private async void addTask()
        {
           
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Ок",
                AnimateShow = true,
                NegativeButtonText = "Отмена"
            };
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
            string numberTask = await metroWindow.ShowInputAsync("Добавить заявку", "Введите номер заявки или заявок через запятую", mySettings);
           
            if (numberTask != null)
            {
                
                string[] taskNumbers = numberTask.Split(new char[] { ',' });
                string text = "";
                string addTask = "";
                string notAddTask = "";
                foreach (string task in taskNumbers)
                {
                    TaskModel.Add(new Task() { TaskId = task });
                    bool analizeOne = AnalizeOneTask(task);
                    if (analizeOne)
                    {
                        if (addTask == "")
                            addTask = task;
                        else
                            addTask = string.Format("{0}, {1}", addTask, task);
                    }
                    else
                    {
                        if (notAddTask == "")
                            notAddTask = task;
                        else
                            notAddTask = string.Format("{0}, {1}", notAddTask, task);
                    }
                }

                text = string.Format("Заявки {0} были добавлены.\nЗаявки {1} не были найдены и не добавлены", addTask, notAddTask);
                var metroWindow1 = (Application.Current.MainWindow as MetroWindow);
                metroWindow1.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
                metroWindow1.ShowMessageAsync("Операция произведена", text, MessageDialogStyle.Affirmative); 
            }
            
        }

        private async void deleteTask()
        {
            string text = "";
            string delTask = "";
            string notFoundTask = "";
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Ок",
                AnimateShow = true,
                NegativeButtonText = "Отмена"
            };
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
            string numberTask = await metroWindow.ShowInputAsync("Удалить заявки", "Введите номер заявки или заявок через запятую", mySettings);
            if (numberTask != null)
            {
                string[] taskNumbers = numberTask.Split(new char[] { ',' });
                foreach (string task in taskNumbers)
                {
                    //TaskModel.Add(new AnalizeTask.Models.Task() { TaskId = task });
                    ObservableCollection<Models.Task> filtererdTests;
                    filtererdTests = new ObservableCollection<Models.Task>(TaskModel.Where(t => t.TaskId == task));
                    if (filtererdTests != null && filtererdTests.Count != 0)
                    {
                        TaskModel.Remove(filtererdTests[0]);
                        if (delTask == "")
                            delTask = task;
                        else
                            delTask = string.Format("{0},{1}", delTask, task);
                    }
                    if (filtererdTests.Count == 0)
                    {
                        if (notFoundTask == "")
                            notFoundTask = task;
                        else
                            notFoundTask = string.Format("{0},{1}", notFoundTask, task);
                    }
                }
               
            }
            text = string.Format("Заявки {0} были удалены.\nЗаявки {1} не были найдены", delTask, notFoundTask);
            var metroWindow1 = (Application.Current.MainWindow as MetroWindow);
            metroWindow1.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
            metroWindow1.ShowMessageAsync("Операция произведена", text, MessageDialogStyle.Affirmative);  
        }
    }

    
}
