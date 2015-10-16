using System.ComponentModel;

namespace AnalizeTask.Models 
{
    class TaskComment : INotifyPropertyChanged
    {
        /// <summary>
        /// Картинка
        /// </summary>
        private string image;
        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        /// <summary>
        /// Когда сделали изменение
        /// </summary>
        private string date;
        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        /// <summary>
        /// Код заявки
        /// </summary>
        private string taskId;
        public string TaskId
        {
            get
            {
                return taskId;
            }
            set
            {
                if (taskId != value)
                {
                    taskId = value;
                    OnPropertyChanged("TaskId");
                }
            }
        }
        /// <summary>
        /// Кто сделал изменение
        /// </summary>
        private string editor;
        public string Editor
        {
            get
            {
                return editor;
            }
            set
            {
                if (editor != value)
                {
                    editor = value;
                    OnPropertyChanged("Editor");
                }
            }
        }
        /// <summary>
        /// Изменился статус
        /// </summary>
        /// <summary>
        /// Текст который будет виден
        /// </summary>
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged("Text");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
