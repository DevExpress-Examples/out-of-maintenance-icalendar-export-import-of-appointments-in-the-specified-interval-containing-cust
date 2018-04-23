using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace iCalendarExportImport {
    [Serializable]
    public class CustomObject {
        string name;
        string info;
        byte[] picture;

        public CustomObject() {
        }
        public string Name { get { return name; } set { name = value; } }
        public string Info { get { return info; } set { info = value; } }
        public byte[] PictureBytes { get { return picture; } set { picture = value; } }
        public Image Picture {
            get {
                MemoryStream ms = new MemoryStream(picture);
                return Image.FromStream(ms); ;
            }
            set {
                MemoryStream ms = new MemoryStream();
                value.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                picture = ms.ToArray();
            }
        }

        public override string ToString() {
            return String.Format("NAME={0} INFO={1}", Name, Info);
        }
    }

}
