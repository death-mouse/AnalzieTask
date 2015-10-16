using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using AnalizeTask.Models;
using System.ComponentModel;

namespace AnalizeTask
{
    /// <summary>
    /// Логика взаимодействия для TaskStatusWindow.xaml
    /// </summary>
    public partial class TaskStatusWindow : MetroWindow
    {

        public TaskStatusWindow()
        {
            InitializeComponent();
        }

        private void colPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
        }
        
        public void initPicker()
        {
            BindingList<Models.TaskStatus> taskStatus = listView.ItemsSource as BindingList<Models.TaskStatus>;
            if (!System.IO.File.Exists(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"])))
            {
                System.Drawing.Color color;
                if (taskStatus[0].Color != null)
                    color = System.Drawing.Color.FromArgb(Convert.ToInt32(taskStatus[0].Color));
                else
                    color = new System.Drawing.Color();
                
                colPicker.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

               
            }
            else
            {
                System.Drawing.Color color;
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                document.Load(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"]));
                System.Xml.XmlNodeList nodeList = document.SelectNodes("TasksStatuses/TaskStatus");
                bool find = false;
                //foreach(System.Xml.XmlElement in document.SelectNodes)
                foreach(System.Xml.XmlNode node in nodeList)
                {
                    
                    System.Xml.XmlNode colorNode = node.SelectSingleNode("Color");
                    System.Xml.XmlNode idNode = node.SelectSingleNode("Id");

                    if (idNode.InnerText != taskStatus[0].Id)
                        continue;
                    find = true;
                    color = System.Drawing.Color.FromArgb(Convert.ToInt32(colorNode.InnerText));
                    colPicker.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    break;
                }
                if (!find)
                {
                    color = System.Drawing.Color.Azure;
                    colPicker.SelectedColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                }
            }

        }
        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Color color;
            color = System.Drawing.Color.FromArgb(colPicker.SelectedColor.Value.R, colPicker.SelectedColor.Value.G, colPicker.SelectedColor.Value.B);
            BindingList<Models.TaskStatus> taskStatus = listView.ItemsSource as BindingList<Models.TaskStatus>;
            if (!System.IO.File.Exists(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"])))
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                System.Xml.XmlNode rootElement = document.CreateElement("TasksStatuses");
                document.AppendChild(rootElement);

                System.Xml.XmlNode element = document.CreateElement("TaskStatus");
                //document.DocumentElement.AppendChild(rootElement); // указываем родителя
                System.Xml.XmlNode children = document.CreateElement("Id"); // даём имя
                children.InnerText = taskStatus[0].Id;
                element.AppendChild(children);
                children = document.CreateElement("Color"); // даём имя

                color = System.Drawing.Color.FromArgb(colPicker.SelectedColor.Value.R, colPicker.SelectedColor.Value.G, colPicker.SelectedColor.Value.B);
                children.InnerText = Convert.ToString(color.ToArgb());
                //color.A = colPicker.SelectedColor.Value.;
                //children.InnerText = colPicker.SelectedColor.;
                element.AppendChild(children);
                rootElement.AppendChild(element);
                document.Save(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"]));
            }
            else
            {
                System.Xml.XmlDocument document = new System.Xml.XmlDocument();
                document.Load(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"]));
                System.Xml.XmlNodeList nodeList = document.SelectNodes("TasksStatuses/TaskStatus");
                Boolean find = false;
                foreach (System.Xml.XmlNode node in nodeList)
                {
                    System.Xml.XmlNode colorNode = node.SelectSingleNode("Color");
                    System.Xml.XmlNode idNode = node.SelectSingleNode("Id");

                    if (idNode.InnerText != taskStatus[0].Id)
                        continue;

                    find = true;
                    colorNode.InnerText = Convert.ToString(color.ToArgb());
                }
                if (!find)
                {
                    System.Xml.XmlNode rootElement = document.SelectSingleNode("TasksStatuses");
                    System.Xml.XmlNode element = document.CreateElement("TaskStatus");
                    //document.DocumentElement.AppendChild(rootElement); // указываем родителя
                    System.Xml.XmlNode children = document.CreateElement("Id"); // даём имя
                    children.InnerText = taskStatus[0].Id;
                    element.AppendChild(children);
                    children = document.CreateElement("Color"); // даём имя

                    color = System.Drawing.Color.FromArgb(colPicker.SelectedColor.Value.R, colPicker.SelectedColor.Value.G, colPicker.SelectedColor.Value.B);
                    children.InnerText = Convert.ToString(color.ToArgb());
                    //color.A = colPicker.SelectedColor.Value.;
                    //children.InnerText = colPicker.SelectedColor.;
                    element.AppendChild(children);
                    rootElement.AppendChild(element);
                }
                document.Save(string.Format(@"{0}\{1}", Environment.CurrentDirectory, Properties.Settings.Default["FileStatusTaskColor"]));
            }
            this.Close();
            
        }

        private void cancelBnt_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    
}
