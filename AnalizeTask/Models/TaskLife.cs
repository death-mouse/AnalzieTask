using System.ComponentModel;


namespace AnalizeTask.Models
{
    class TaskLife : INotifyPropertyChanged
    {
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
        private string statusId;
        public string StatusId
        {
            get
            {
                return statusId;
            }
            set
            {
                if (statusId != value)
                {
                    statusId = value;
                    OnPropertyChanged("StatusId");
                }
            }
        }
        /// <summary>
        /// Добавлен комментарий
        /// </summary>
        private string comments;
        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                if (comments != value)
                {
                    comments = value;
                    OnPropertyChanged("Comments");
                }
            }
        }
        /// <summary>
        /// Изменился список наблюдателей
        /// </summary>
        private string participants;
        public string Participants
        {
            get
            {
                return participants;
            }
            set
            {
                if (participants != value)
                {
                    participants = value;
                    OnPropertyChanged("Participants");
                }
            }
        }
        /// <summary>
        /// Изменился список Исполнителей
        /// </summary>
        private string executors;
        public string Executors
        {
            get
            {
                return executors;
            }
            set
            {
                if (executors != value)
                {
                    executors = value;
                    OnPropertyChanged("Executors");
                }
            }
        }

        /// <summary>
        /// Изменился список Категория
        /// </summary>
        private string categories;
        public string Categories
        {
            get
            {
                return categories;
            }
            set
            {
                if (categories != value)
                {
                    categories = value;
                    OnPropertyChanged("Categories");
                }
            }
        }

        /// <summary>
        /// Изменился список Файлов
        /// </summary>
        private string files;
        public string Files
        {
            get
            {
                return files;
            }
            set
            {
                if (files != value)
                {
                    files = value;
                    OnPropertyChanged("Files");
                }
            }
        }

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
