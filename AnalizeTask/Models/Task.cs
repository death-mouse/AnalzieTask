using System;
using System.ComponentModel;

namespace AnalizeTask.Models
{
    class Task : INotifyPropertyChanged
    {
        private string taskId;
        private string creater;
        private string taskStatus;
        private string taskName;
        private string taskPerfomer;
        private string image;
        private string image24;
        private string imageTaskType;

        private string observers;
        private string serviceName;
        private string type;
        private string description;
        private string priorityName;
        private string statusName;
        private string completionStatus;
        private bool needAnalize;
        private string divergence;
        public string Divergence
        {
            get
            {
                return divergence;
            }
            set
            {
                if (divergence != value)
                {
                    divergence = value;
                    OnPropertyChanged("Divergence");
                }
            }
        }
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

        public bool NeedAnalize
        {
            get
            {
                return needAnalize;
            }
            set
            {
                if (needAnalize != value)
                {
                    needAnalize = value;
                    OnPropertyChanged("NeedAnalize");
                }
            }
        }
        private DateTime taskDeadLine;
        public DateTime TaskDeadLine
        {
            get
            {
                if (taskDeadLine.Date == new DateTime(1, 1, 1))
                    taskDeadLine = DateTime.Today;
                return taskDeadLine;
            }
            set
            {
                if (taskDeadLine != value)
                {
                    taskDeadLine = value;
                    OnPropertyChanged("TaskDeadLine");
                }
            }
        }
        public string CompletionStatus
        {
            get
            {
                return completionStatus;
            }
            set
            {
                if (completionStatus != value)
                {
                    completionStatus = value;
                    OnPropertyChanged("CompletionStatus");
                }
            }
        }
        public string StatusName
        {
            get
            {
                return statusName;
            }
            set
            {
                if (statusName != value)
                {
                    statusName = value;
                    OnPropertyChanged("StatusName");
                }
            }
        }
        public string PriorityName
        {
            get
            {
                return priorityName;
            }
            set
            {
                if (priorityName != value)
                {
                    priorityName = value;
                    OnPropertyChanged("PriorityName");
                }
            }
        }
        public string ServiceName
        {
            get
            {
                return serviceName;
            }
            set
            {
                if (serviceName != value)
                {
                    serviceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }      
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public string Observers
        {
            get
            {
                return observers;
            }
            set
            {
                if (observers != value)
                {
                    observers = value;
                    OnPropertyChanged("Observers");
                }
            }
        }

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
        public string Creater
        {
            get
            {
                return creater;
            }
            set
            {
                if (creater != value)
                {
                    creater = value;
                    OnPropertyChanged("Creater");
                }
            }
        }
        public string TaskStatus
        {
            get
            {
                return taskStatus;
            }
            set
            {
                if (taskStatus != value)
                {
                    taskStatus = value;
                    OnPropertyChanged("TaskStatus");
                }
            }
        }
        public string TaskName
        {
            get
            {
                return taskName;
            }
            set
            {
                if (taskName != value)
                {
                    taskName = value;
                    OnPropertyChanged("TaskName");
                }
            }
        }
        public string TaskPerfomer
        {
            get
            {
                return taskPerfomer;
            }
            set
            {
                if (taskPerfomer != value)
                {
                    taskPerfomer = value;
                    OnPropertyChanged("TaskPerfomer");
                }
            }
        }

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
        public string Image24
        {
            get
            {
                return image24;
            }
            set
            {
                if (image24 != value)
                {
                    image24 = value;
                    OnPropertyChanged("Image24");
                }
            }
        }
        public string ImageTaskType
        {
            get
            {
                return imageTaskType;
            }
            set
            {
                if (imageTaskType != value)
                {
                    imageTaskType = value;
                    OnPropertyChanged("ImageTaskType");
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
