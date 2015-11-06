using System.ComponentModel;

namespace AnalizeTask.Models
{
    class TaskStatus
    {
        private string description;
        private string id;
        private string imageUrl;
        private string imageUrl24;
        private string name;
        private bool end;

        public bool End
        {
            get
            {
                return end;
            }
            set
            {
                if (end != value)
                {
                    end = value;
                    OnPropertyChanged("End");
                }
            }
        }
        private string color;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged("Color");
                }
            }
        }
        public string ImageUrl24
        {
            get
            {
                return imageUrl24;
            }
            set
            {
                if (imageUrl24 != value)
                {
                    imageUrl24 = value;
                    OnPropertyChanged("ImageUrl24");
                }
            }
        }
        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    OnPropertyChanged("ImageUrl");
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
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
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
