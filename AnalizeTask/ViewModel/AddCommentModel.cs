using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalizeTask.Models;
using System.ComponentModel;

namespace AnalizeTask.ViewModel
{
    class AddCommentModel
    {
        public BindingList<AddComment> addComment { get; set; }

        public AddCommentModel()
        {
            addComment = new BindingList<AddComment>();            
        }
    }
}
