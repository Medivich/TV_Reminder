using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV_Reminder.Model
{
    class Poster
    {
        public string fileName { get; set; }

        public byte[] _memoryStream { get; set; }

        public byte[] Post
        {
            get { return _memoryStream; }
            set
            {
                _memoryStream = value;
            }
        }

        public Poster()
        {
            ;
        }

        public Poster(string fileName)
        {
            this.fileName = fileName;
        }

        public Poster(byte[] stream)
        {
            this._memoryStream = stream;
        }
    }
}
