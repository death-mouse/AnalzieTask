using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AnalizeTask.Notification;
using System.Threading;


namespace AnalizeTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : MetroWindow
    {
        public ArrayList ColorsModelArray;
        public ArrayList ColorThemModel;
        public MainWindow()
        {
            InitializeComponent();
            this.initColor();
            if ((int)Properties.Settings.Default["Color"] == -1 || (int)Properties.Settings.Default["Them"] == -1)
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
                metroWindow.ShowMessageAsync("Ошибка", "Не все параметры заполнены", MessageDialogStyle.Affirmative);
                return;
            }
            string color = ColorsModelArray[(int)Properties.Settings.Default["Color"]].ToString();
            string colorThem = ColorThemModel[(int)Properties.Settings.Default["Them"]].ToString();
            if (color == null || colorThem == null)
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
                metroWindow.ShowMessageAsync("Ошибка", "Не все параметры заполнены", MessageDialogStyle.AffirmativeAndNegative);
                return;
                
            }
            ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent(color),
                                    ThemeManager.GetAppTheme(colorThem));
            if ((bool)Properties.Settings.Default["VisibleDedLine"] == true)
                DeadLine.Visibility = System.Windows.Visibility.Visible;
            else
                DeadLine.Visibility = System.Windows.Visibility.Hidden;

            if ((bool)Properties.Settings.Default["VisibleMinus"] == true)
                divergence.Visibility = System.Windows.Visibility.Visible;
            else
                divergence.Visibility = System.Windows.Visibility.Hidden;
            
           


        }
        private void initColor()
        {
            ColorsModelArray = new ArrayList();
            ColorThemModel = new ArrayList();
            ColorsModelArray.Add("Red");
            ColorsModelArray.Add("Green" );
            ColorsModelArray.Add("Blue" );
            ColorsModelArray.Add("Purple" );
            ColorsModelArray.Add("Orange" );
            ColorsModelArray.Add("Lime" );
            ColorsModelArray.Add("Emerald" );
            ColorsModelArray.Add("Teal" );
            ColorsModelArray.Add("Cyan" );
            ColorsModelArray.Add("Cobalt" );
            ColorsModelArray.Add( "Indigo" );
            ColorsModelArray.Add( "Violet" );
            ColorsModelArray.Add( "Pink" );
            ColorsModelArray.Add("Magenta" );
            ColorsModelArray.Add("Crimson");
            ColorsModelArray.Add("Amber" );
            ColorsModelArray.Add("Yellow" );
            ColorsModelArray.Add("Brown" );
            ColorsModelArray.Add("Olive" );
            ColorsModelArray.Add("Steel" );
            ColorsModelArray.Add("Mauve" );
            ColorsModelArray.Add("Taupe" );
            ColorsModelArray.Add("Sienna" );

            ColorThemModel.Add ("BaseLight" );
            ColorThemModel.Add("BaseDark" );

        }   
        private void MainDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            AnalizeTask.Models.Task task = e.Row.Item as AnalizeTask.Models.Task;
               
            if (System.IO.File.Exists(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"])))
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                document.Load(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"]));
                System.Xml.XmlNodeList nodeList = document.SelectNodes("TasksStatuses/TaskStatus");
                bool find = false;
                foreach (System.Xml.XmlNode node in nodeList)
                {
                    System.Xml.XmlNode colorNode = node.SelectSingleNode("Color");
                    System.Xml.XmlNode idNode = node.SelectSingleNode("Id");

                    if (idNode.InnerText != task.StatusId)
                        continue;
                    
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(Convert.ToInt32(colorNode.InnerText));
                    var colorBrush = new System.Windows.Media.SolidColorBrush(this.ColorToColor(color));
                    e.Row.Background = colorBrush;
                    find = true;
                    break;
                }
                if (!find)
                {
                    System.Drawing.Color color = System.Drawing.Color.Azure;
                    var colorBrush = new System.Windows.Media.SolidColorBrush(this.ColorToColor(color, true));
                    e.Row.Background = colorBrush;
                    
                    
                }
            }
            else
            {
                System.Drawing.Color color = System.Drawing.Color.Azure;
                var colorBrush = new System.Windows.Media.SolidColorBrush(this.ColorToColor(color));
                e.Row.Background = colorBrush;
            }
           

        }
        private System.Windows.Media.Color ColorToColor(System.Drawing.Color color, bool colorThem = false)
        {
            if (colorThem)
            {
                if ((int)Properties.Settings.Default["Them"] == 0)
                {
                    color = System.Drawing.Color.Azure;
                    return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                }
                else
                    return System.Windows.Media.Color.FromArgb(1, 37, 37, 37);
            }
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        private void AddColor_Click(object sender, RoutedEventArgs e)
        {
            if(ColorButton.SelectedIndex == -1 || ColorThem.SelectedIndex == -1 ) 
               
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
                metroWindow.ShowMessageAsync("Ошибка", "Не все параметры заполнены", MessageDialogStyle.Affirmative);
                return;
            }
            string color = (ColorButton.Items[ColorButton.SelectedIndex] as AnalizeTask.Models.ColorsModel).colors;
            string colorThem = (ColorThem.Items[ColorThem.SelectedIndex] as AnalizeTask.Models.ThemModel).colors;
            if (color == null || colorThem == null)
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Theme;
                metroWindow.ShowMessageAsync("Ошибка", "Не все параметры заполнены", MessageDialogStyle.AffirmativeAndNegative);
                return;
            }
            ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent(color),
                                    ThemeManager.GetAppTheme(colorThem));
            Properties.Settings.Default["Color"] = ColorButton.SelectedIndex;
            Properties.Settings.Default["Them"] = ColorThem.SelectedIndex;

            Properties.Settings.Default.Save();

            if ((bool)Properties.Settings.Default["VisibleDedLine"] == true)
                DeadLine.Visibility = System.Windows.Visibility.Visible;
            else
                DeadLine.Visibility = System.Windows.Visibility.Hidden;

            if ((bool)Properties.Settings.Default["VisibleMinus"] == true)
                divergence.Visibility = System.Windows.Visibility.Visible;
            else
                divergence.Visibility = System.Windows.Visibility.Hidden;
        }
        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        private void LifeTaskTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            this.saveTask();
        }
        private void saveTask()
        {
            
            if (MainDataGrid.Items.Count != 0)
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                System.Xml.XmlNode rootElement = document.CreateElement("Tasks");
                document.AppendChild(rootElement);
                //document.DocumentElement.AppendChild(rootElement); // указываем родителя
                foreach (AnalizeTask.Models.Task itemCollection in MainDataGrid.Items)
                {
                    System.Xml.XmlNode element = document.CreateElement("Task");
                    //document.DocumentElement.AppendChild(rootElement); // указываем родителя
                    System.Xml.XmlNode children = document.CreateElement("TaskId"); // даём имя
                    children.InnerText = itemCollection.TaskId;
                    element.AppendChild(children);
                    children = document.CreateElement("TaskStatus"); // даём имя
                    children.InnerText = itemCollection.TaskStatus;
                    element.AppendChild(children);
                    children = document.CreateElement("TaskPerfomer"); // даём имя
                    children.InnerText = itemCollection.TaskPerfomer;
                    element.AppendChild(children);
                    children = document.CreateElement("TaskName"); // даём имя
                    children.InnerText = itemCollection.TaskName;
                    element.AppendChild(children);
                    children = document.CreateElement("Creater"); // даём имя
                    children.InnerText = itemCollection.Creater;
                    element.AppendChild(children);
                    children = document.CreateElement("Image"); // даём имя
                    children.InnerText = itemCollection.Image;
                    element.AppendChild(children);
                    children = document.CreateElement("DeadLine"); // даём имя
                    children.InnerText = itemCollection.TaskDeadLine.ToShortDateString();
                    element.AppendChild(children);
                    rootElement.AppendChild(element);
                }
                document.Save(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileName"]));
            }
        }

        private void MetroWindow_Closing_1(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
            saveTask();

            //base.OnClosing(e);
            System.Environment.Exit(1);
        }

        private void TaskIdGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.TextBlock textBox = ( System.Windows.Controls.TextBlock)sender;
            View.TaskView taskView;
            taskView = (AnalizeTask.View.TaskView)MainDataGrid.DataContext;
            BindingList<AnalizeTask.Models.Task> taskModel = taskView.TaskModel;
            ObservableCollection<AnalizeTask.Models.Task> filtererdTests;
            filtererdTests = new ObservableCollection<Models.Task>(taskModel.Where(t => t.TaskId == textBox.Text));

            pLink.DataContext = filtererdTests[0];
            pLink.IsOpen = false;
            pLink.IsOpen = true;
        }

        private void TaskIdGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            pLink.IsOpen = false;
        }

        private void DeadLineDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            AnalizeTask.View.TaskView taskView;
            taskView = (AnalizeTask.View.TaskView)MainDataGrid.DataContext;
            int selectedIndex = taskView.selectedIndex;
            System.Windows.Controls.DatePicker datePicker = (System.Windows.Controls.DatePicker)sender;


            taskView.TaskModel[selectedIndex].TaskDeadLine = (DateTime)datePicker.SelectedDate;

        }

        private void WindowsTaskStatus_Click(object sender, RoutedEventArgs e)
        {
           View.TaskView taskView = (View.TaskView)MainDataGrid.DataContext;
            
            TaskStatusWindow window = new TaskStatusWindow();
            
            BindingList<Models.Task> taskModel = taskView.TaskModel;
            BindingList<Models.TaskStatus> taskStatus = taskView.TaskStatus;
            BindingList<Models.TaskStatus> taskStatusNew = new BindingList<Models.TaskStatus>();
            View.StatusTaskView statusTaskView = (View.StatusTaskView)window.DataContext;
            foreach (Models.Task task in taskModel)
            {
                ObservableCollection<AnalizeTask.Models.TaskStatus> filtererdTests;
                filtererdTests = new ObservableCollection<Models.TaskStatus>(taskStatus.Where(t => t.Id == task.StatusId));
                if (statusTaskView.TaskStatus == null)
                    statusTaskView.TaskStatus = new BindingList<Models.TaskStatus>();
                statusTaskView.TaskStatus.Add(new Models.TaskStatus() { Name = filtererdTests[0].Name, Description = filtererdTests[0].Description, Id = filtererdTests[0].Id, ImageUrl = filtererdTests[0].ImageUrl, ImageUrl24 = filtererdTests[0].ImageUrl24 });
            }
            
           
            window.ShowDialog();
            
        }

        private void MarkStatus_Click(object sender, RoutedEventArgs e)
        {
            View.TaskView taskView = (View.TaskView)MainDataGrid.DataContext;

            TaskStatusWindow window = new TaskStatusWindow();
           
            BindingList<Models.Task> taskModel = taskView.TaskModel;
            Models.Task task = (Models.Task)MainDataGrid.Items[MainDataGrid.SelectedIndex];
            BindingList<Models.TaskStatus> taskStatus = taskView.TaskStatus;
            BindingList<Models.TaskStatus> taskStatusNew = new BindingList<Models.TaskStatus>();
            View.StatusTaskView statusTaskView = (View.StatusTaskView)window.DataContext;
            
            ObservableCollection<AnalizeTask.Models.TaskStatus> filtererdTests;
            if (task.StatusId != null)
            {
                filtererdTests = new ObservableCollection<Models.TaskStatus>(taskStatus.Where(t => t.Id == task.StatusId));
                if (statusTaskView.TaskStatus == null)
                    statusTaskView.TaskStatus = new BindingList<Models.TaskStatus>();
                statusTaskView.TaskStatus.Add(new Models.TaskStatus() { Name = filtererdTests[0].Name, Description = filtererdTests[0].Description, Id = filtererdTests[0].Id, ImageUrl = filtererdTests[0].ImageUrl, ImageUrl24 = filtererdTests[0].ImageUrl24 });
                window.initPicker();
                window.ShowDialog();

                if (System.IO.File.Exists(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"])))
                {
                    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    document.Load(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"]));
                    System.Xml.XmlNodeList nodeList = document.SelectNodes("TasksStatuses/TaskStatus");
                    
                    foreach (System.Xml.XmlNode node in nodeList)
                    {
                        System.Xml.XmlNode colorNode = node.SelectSingleNode("Color");
                        System.Xml.XmlNode idNode = node.SelectSingleNode("Id");

                        if (idNode.InnerText != task.StatusId)
                            continue;
                        //int i = 0;
                        int count = MainDataGrid.Items.Count;
                        for(int i = 0; i != MainDataGrid.Items.Count; i++)
                        {
                            Models.Task taskFind = (Models.Task)MainDataGrid.Items[i];
                            if(taskFind.StatusId != task.StatusId)
                                continue;
                            DataGridRow row = (DataGridRow)MainDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                            if (row != null)
                            {
                                MainDataGrid.UpdateLayout();
                                MainDataGrid.ScrollIntoView(MainDataGrid.Items[i]);
                                System.Drawing.Color color = System.Drawing.Color.FromArgb(Convert.ToInt32(colorNode.InnerText));
                                var colorBrush = new System.Windows.Media.SolidColorBrush(this.ColorToColor(color));
                                row.Background = colorBrush;
                            }
                             
                           
                        }
                        break;
                    }
                }
            }
        }

        public void updateLyout()
        {
            MainDataGrid.UpdateLayout();
            MainDataGrid.UpdateDefaultStyle();
            MainDataGrid.Items.Refresh();
        }
       
        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
         
           
            View.TaskView taskView;
            taskView = (AnalizeTask.View.TaskView)MainDataGrid.DataContext;
            taskView.GetTaskTherd();
            
            if (MainDataGrid.Items.Count != 0)
            {
                
                MainDataGrid.ScrollIntoView(MainDataGrid.Items[MainDataGrid.Items.Count - 1]);
                MainDataGrid.SelectedIndex = MainDataGrid.Items.Count - 1;
                MainDataGrid.SelectedIndex = 0;
                MainDataGrid.ScrollIntoView(MainDataGrid.Items[0]);
            }
            MainDataGrid.UpdateLayout();
            MainDataGrid.UpdateDefaultStyle();
            MainDataGrid.Items.Refresh();
            MainDataGrid.DataContext = taskView;
           
            /**/
        }

        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            MainDataGrid.UpdateLayout();
            MainDataGrid.UpdateDefaultStyle();
            MainDataGrid.Items.Refresh();

            MainDataGrid.ScrollIntoView(MainDataGrid.Items[MainDataGrid.Items.Count - 1]);
            MainDataGrid.SelectedIndex = MainDataGrid.Items.Count - 1;
            MainDataGrid.SelectedIndex = 0;
            MainDataGrid.ScrollIntoView(MainDataGrid.Items[0]);

        }    

        private void MainDataGrid_LoadingRowDetails(object sender, DataGridRowDetailsEventArgs e)
        {
            MainDataGrid.UpdateLayout();
            MainDataGrid.UpdateDefaultStyle();
            MainDataGrid.Items.Refresh();
        }    

        private void MainDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            MainDataGrid.UpdateLayout();
            MainDataGrid.UpdateDefaultStyle();
            MainDataGrid.Items.Refresh();
        }

        private void MainDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            MainDataGrid.UpdateLayout();
            MainDataGrid.UpdateDefaultStyle();
            MainDataGrid.Items.Refresh();
        }

       

        private void DeleteMarkStatus_Click(object sender, RoutedEventArgs e)
        {
            View.TaskView taskView = (View.TaskView)MainDataGrid.DataContext;

            TaskStatusWindow window = new TaskStatusWindow();

            BindingList<Models.Task> taskModel = taskView.TaskModel;
            Models.Task task = (Models.Task)MainDataGrid.Items[MainDataGrid.SelectedIndex];
            BindingList<Models.TaskStatus> taskStatus = taskView.TaskStatus;
            BindingList<Models.TaskStatus> taskStatusNew = new BindingList<Models.TaskStatus>();
            View.StatusTaskView statusTaskView = (View.StatusTaskView)window.DataContext;

            ObservableCollection<AnalizeTask.Models.TaskStatus> filtererdTests;
            if (task.StatusId != null)
            {
                filtererdTests = new ObservableCollection<Models.TaskStatus>(taskStatus.Where(t => t.Id == task.StatusId));
                if (statusTaskView.TaskStatus == null)
                    statusTaskView.TaskStatus = new BindingList<Models.TaskStatus>();
                statusTaskView.TaskStatus.Add(new Models.TaskStatus() { Name = filtererdTests[0].Name, Description = filtererdTests[0].Description, Id = filtererdTests[0].Id, ImageUrl = filtererdTests[0].ImageUrl, ImageUrl24 = filtererdTests[0].ImageUrl24 });

                if (System.IO.File.Exists(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"])))
                {
                    System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                    document.Load(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"]));
                    System.Xml.XmlNodeList nodeList = document.SelectNodes("TasksStatuses/TaskStatus");

                    foreach (System.Xml.XmlNode node in nodeList)
                    {
                        System.Xml.XmlNode colorNode = node.SelectSingleNode("Color");
                        System.Xml.XmlNode idNode = node.SelectSingleNode("Id");

                        if (idNode.InnerText != task.StatusId)
                            continue;
                        //int i = 0;
                        idNode.RemoveAll();
                        int count = MainDataGrid.Items.Count;
                        for (int i = 0; i != MainDataGrid.Items.Count; i++)
                        {
                            Models.Task taskFind = (Models.Task)MainDataGrid.Items[i];
                            if (taskFind.StatusId != task.StatusId)
                                continue;
                            DataGridRow row = (DataGridRow)MainDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                            if (row != null)
                            {
                                MainDataGrid.UpdateLayout();
                                MainDataGrid.ScrollIntoView(MainDataGrid.Items[i]);
                                System.Drawing.Color color = System.Drawing.Color.FromArgb(Convert.ToInt32(colorNode.InnerText));
                                var colorBrush = new System.Windows.Media.SolidColorBrush(this.ColorToColor(color, true));
                                row.Background = colorBrush;
                            }


                        }
                        break;
                    }
                    document.Save(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"]));
                }
            }
        }

        private void ImportTask_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV documents (.csv)|*.csv"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                View.TaskView taskView;
                taskView = (AnalizeTask.View.TaskView)MainDataGrid.DataContext;
                taskView.importFromCsv(filename);
            }

        }
    }
    public static class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
           typeof(PasswordHelper));


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}
