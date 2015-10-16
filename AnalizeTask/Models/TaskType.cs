using System.ComponentModel;

namespace AnalizeTask.Models
{
    class TaskType
    {
        private string id;
        private string name;
        private string imgUrl;

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
        public string ImgUrl
        {
            get
            {
                return imgUrl;
            }
            set
            {
                if (imgUrl != value)
                {
                    imgUrl = value;
                    OnPropertyChanged("ImgUrl");
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
